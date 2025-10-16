using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using InnForkInteropAPI.BlockChain.NeoN3.NeoSC_Helpers.MessagesParsing;
using Neo;
using Neo.Extensions;
using Neo.Json;
using Neo.Network.RPC;
using Neo.Network.RPC.Models;
using Neo.SmartContract;

namespace InnFork.Blockchain.NEO3;

/// <summary>
/// Wrapper around <see cref="RpcClient"/> that encapsulates creation, configuration and helper
/// conversions required for working with InnFork specific RPC stack payloads.
/// </summary>
public class NeoConnectClientService
{
    private readonly NeoRpcClientOptions _options;
    private readonly Lazy<RpcClient> _rpcClientLazy;
    private readonly Lazy<ContractClient> _contractClientLazy;

    public NeoConnectClientService(NeoRpcClientOptions? options = null, HttpMessageHandler? httpMessageHandler = null)
    {
        _options = options ?? NeoRpcClientOptions.CreateFromEnvironment();

        // »нициализаци€ RpcClient использу€ именованный параметр protocolSettings
        _rpcClientLazy = new Lazy<RpcClient>(() => new RpcClient(_options.RpcUri, protocolSettings: _options.ProtocolSettings));

        _contractClientLazy = new Lazy<ContractClient>(() => new ContractClient(RpcClient));
    }

    public static NeoConnectClientService CreateDefault(bool useTestNet = true) => new(NeoRpcClientOptions.CreateDefault(useTestNet));

    public NeoRpcClientOptions Options => _options;
    public RpcClient RpcClient => _rpcClientLazy.Value;
    public ContractClient ContractClient => _contractClientLazy.Value;

    public async Task<RpcInvokeResult> TestInvokeContractAsync(UInt160 contractScriptHash,
                                                               string methodName,
                                                               NeoN3Scope scope,
                                                               ContractParameter[] contractParameterArgs,
                                                               CancellationToken cancellationToken = default)
    {
        _ = scope; // Scope reserved for future witness handling; invoke does not require it currently
        if (contractParameterArgs is null) throw new ArgumentNullException(nameof(contractParameterArgs));
        if (string.IsNullOrWhiteSpace(methodName)) throw new ArgumentException("Method name is required", nameof(methodName));

        cancellationToken.ThrowIfCancellationRequested();
        return await ContractClient.TestInvokeAsync(contractScriptHash, methodName, contractParameterArgs).ConfigureAwait(false);
    }

    public Task<RpcInvokeResult> TestInvokeContractAsync(UInt160 contractScriptHash,
                                                         string methodName,
                                                         NeoN3Scope scope,
                                                         InnFork_RpcStack[] rpcStackArgs,
                                                         CancellationToken cancellationToken = default)
    {
        _ = scope;
        var parameters = InnForkRpcStackConverter.ToContractParameters(rpcStackArgs);
        return TestInvokeContractAsync(contractScriptHash, methodName, scope, parameters, cancellationToken);
    }

    public InnFork_RpcStack[] ConvertInvokeResultToInnFork(RpcInvokeResult invokeResult)
    {
        if (invokeResult is null) throw new ArgumentNullException(nameof(invokeResult));
        return InnForkRpcStackConverter.FromRpcStacks(invokeResult.Stack?.Select(s => new RpcStack
        {
            Type = s.Type.ToString(),
            Value = s.ToJson()["value"]
        }));
    }
}

public class InnFork_RpcStack
{
    public string? Type { get; set; }
    public object? Value { get; set; }

    public JObject ToJson()
    {
        JObject json = new()
        {
            ["type"] = Type
        };

        if (Value is null)
        {
            json["value"] = null;
            return json;
        }

        switch (Value)
        {
            case bool boolValue:
                json["value"] = boolValue;
                break;
            case byte[] byteArray:
                json["value"] = byteArray.ToHexString();
                break;
            case JObject jObject:
                json["value"] = jObject;
                break;
            case JArray jArray:
                json["value"] = jArray;
                break;
            case IEnumerable<InnFork_RpcStack> enumerable:
                JArray array = new();
                foreach (var item in enumerable)
                {
                    array.Add(item.ToJson());
                }
                json["value"] = array;
                break;
            default:
                json["value"] = Value switch
                {
                    string s => s,
                    JsonElement element => element.ToString(),
                    _ => Value.ToString()
                };
                break;
        }

        return json;
    }

    public ContractParameter ToContractParameter() => InnForkRpcStackConverter.ToContractParameter(this);
    public RpcStack ToRpcStack() => InnForkRpcStackConverter.ToRpcStack(InnForkRpcStackConverter.ParseRpcEnum(Type), Value);
}

public class NeoRpcStackFabric
{
    private readonly List<RpcStackItem> _rpcStackList = new();

    public IReadOnlyList<RpcStackItem> RpcStackList => _rpcStackList;

    public enum RpcEnumTypes
    {
        Any,
        Boolean,
        Integer,
        ByteArray,
        String,
        Hash160,
        Hash256,
        PublicKey,
        Array,
        Void
    }

    public record RpcStackItem(RpcEnumTypes Type, object? Value, string? RawValue)
    {
        public InnFork_RpcStack ToInnForkStack() => new() { Type = Type.ToString(), Value = Value };
        public RpcStack ToRpcStack() => InnForkRpcStackConverter.ToRpcStack(Type, Value);
        public ContractParameter ToContractParameter() => InnForkRpcStackConverter.ToContractParameter(Type, Value);
    }

    public void AddRpcStack(RpcEnumTypes rpcEnumType, string? value)
    {
        if (!InnForkRpcStackConverter.TryConvertToClrValue(rpcEnumType, value, out var parsed, out var error))
        {
            throw new ArgumentException($"Invalid value for type {rpcEnumType}: {error}", nameof(value));
        }

        _rpcStackList.Add(new RpcStackItem(rpcEnumType, parsed, value));
    }

    public void AddRpcStack(RpcEnumTypes rpcEnumType, object? value)
    {
        _rpcStackList.Add(new RpcStackItem(rpcEnumType, value, value?.ToString()));
    }

    public void AddInnForkStack(InnFork_RpcStack stack)
    {
        if (stack is null) throw new ArgumentNullException(nameof(stack));
        var type = InnForkRpcStackConverter.ParseRpcEnum(stack.Type);
        _rpcStackList.Add(new RpcStackItem(type, stack.Value, stack.Value?.ToString()));
    }

    public void AddInnForkStacks(IEnumerable<InnFork_RpcStack>? stacks)
    {
        if (stacks is null) return;
        foreach (var stack in stacks)
        {
            AddInnForkStack(stack);
        }
    }

    public void AddContractParameter(ContractParameter parameter)
    {
        if (parameter is null) throw new ArgumentNullException(nameof(parameter));
        var innFork = InnForkRpcStackConverter.ToInnForkRpcStack(parameter);
        AddInnForkStack(innFork);
    }

    public void AddContractParameters(IEnumerable<ContractParameter>? parameters)
    {
        if (parameters is null) return;
        foreach (var parameter in parameters)
        {
            AddContractParameter(parameter);
        }
    }

    public void LoadFromInvokeResult(RpcInvokeResult? invokeResult)
    {
        if (invokeResult?.Stack == null) return;
        AddInnForkStacks(InnForkRpcStackConverter.FromRpcStacks(
            invokeResult.Stack.Select(s => new RpcStack
            {
                Type = s.Type.ToString(),
                Value = s.ToJson()["value"]
            })
        ));
    }

    public void LoadFromApplicationLog(RpcApplicationLog? applicationLog)
    {
        if (applicationLog?.Executions == null) return;
        foreach (var execution in applicationLog.Executions)
        {
            if (execution.Stack != null)
            {
                var rpcStacks = execution.Stack
                    .Select(s => new RpcStack
                    {
                        Type = s.Type.ToString(),
                        Value = s.ToJson()["value"]
                    });
                AddInnForkStacks(InnForkRpcStackConverter.FromRpcStacks(rpcStacks));
            }
        }
    }

    public InnFork_RpcStack[] GetInnForkRpcStackBundle() => _rpcStackList.Select(i => i.ToInnForkStack()).ToArray();

    public RpcStack[] GetRpcStackBundle() => _rpcStackList.Select(i => i.ToRpcStack()).ToArray();

    public ContractParameter[] GetContractParameters() => _rpcStackList.Select(i => i.ToContractParameter()).ToArray();

    public void RemoveRpcStack(RpcStackItem item) => _rpcStackList.Remove(item);

    public void ClearRpcStack() => _rpcStackList.Clear();

    public static string ContractParameterToString(ContractParameter parameter, HashSet<ContractParameter>? context = null)
    {
        context ??= new HashSet<ContractParameter>();

        return parameter.Value switch
        {
            null => "(null)",
            byte[] data => data.ToHexString(),
            IList<ContractParameter> data => ConvertArrayToString(parameter, data, context),
            IList<KeyValuePair<ContractParameter, ContractParameter>> data => ConvertMapToString(parameter, data, context),
            _ => parameter.Value.ToString() ?? string.Empty
        };
    }

    public static JObject ContractParameterToJson(ContractParameter parameter, HashSet<ContractParameter>? context = null)
    {
        context ??= new HashSet<ContractParameter>();
        JObject json = new()
        {
            ["type"] = parameter.Type.ToString()
        };

        if (parameter.Value != null)
        {
            switch (parameter.Type)
            {
                case ContractParameterType.Signature:
                case ContractParameterType.ByteArray:
                    json["value"] = Convert.ToBase64String((byte[])parameter.Value);
                    break;
                case ContractParameterType.Boolean:
                    json["value"] = (bool)parameter.Value;
                    break;
                case ContractParameterType.Integer:
                case ContractParameterType.Hash160:
                case ContractParameterType.Hash256:
                case ContractParameterType.PublicKey:
                case ContractParameterType.String:
                    json["value"] = parameter.Value.ToString();
                    break;
                case ContractParameterType.Array:
                    if (context.Contains(parameter))
                        throw new InvalidOperationException("Recursive array detected");
                    context.Add(parameter);
                    json["value"] = new JArray(((IList<ContractParameter>)parameter.Value).Select(p => ContractParameterToJson(p, context)));
                    break;
                case ContractParameterType.Map:
                    if (context.Contains(parameter))
                        throw new InvalidOperationException("Recursive map detected");
                    context.Add(parameter);
                    json["value"] = new JArray(((IList<KeyValuePair<ContractParameter, ContractParameter>>)parameter.Value).Select(p =>
                    {
                        JObject item = new();
                        item["key"] = ContractParameterToJson(p.Key, context);
                        item["value"] = ContractParameterToJson(p.Value, context);
                        return item;
                    }));
                    break;
            }
        }

        return json;
    }

    private static string ConvertArrayToString(ContractParameter parameter, IList<ContractParameter> data, HashSet<ContractParameter> context)
    {
        if (context.Contains(parameter))
        {
            return "(array)";
        }

        context.Add(parameter);
        StringBuilder sb = new();
        sb.Append('[');
        foreach (ContractParameter item in data)
        {
            sb.Append(ContractParameterToString(item, context));
            sb.Append(", ");
        }
        if (data.Count > 0)
            sb.Length -= 2;
        sb.Append(']');
        return sb.ToString();
    }

    private static string ConvertMapToString(ContractParameter parameter, IList<KeyValuePair<ContractParameter, ContractParameter>> data, HashSet<ContractParameter> context)
    {
        if (context.Contains(parameter))
        {
            return "(map)";
        }

        context.Add(parameter);
        StringBuilder sb = new();
        sb.Append('[');
        foreach (var item in data)
        {
            sb.Append('{');
            sb.Append(ContractParameterToString(item.Key, context));
            sb.Append(',');
            sb.Append(ContractParameterToString(item.Value, context));
            sb.Append('}');
            sb.Append(", ");
        }
        if (data.Count > 0)
            sb.Length -= 2;
        sb.Append(']');
        return sb.ToString();
    }
}
