
using System.Collections;

using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using InnFork.Blockchain.NEO3;
using Neo;
using Neo.Extensions;
using Neo.Network.RPC.Models;
using Neo.SmartContract;

namespace InnForkInteropAPI.BlockChain.NeoN3.SC_Wrappers;

public partial class InnFork_SCPlatform
{
    private static readonly JsonSerializerOptions ContractJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private static ContractParameter[] BuildParameters(params object?[] args)
        => args?.Select(arg => CreateContractParameter(arg)).ToArray() ?? Array.Empty<ContractParameter>();

    private static RpcInvokeResult? ExecuteContract(UInt160 address, string methodName, bool useTestNet, bool testInvoke,
                                                    string? defaultWif, ContractParameter[] parameters)
    {
        NeoSCInterop interop = new(useTestNet);
        if (!string.IsNullOrWhiteSpace(defaultWif))
        {
            interop.DefaultUserWIF = defaultWif;
        }

        try
        {
            if (testInvoke || string.IsNullOrWhiteSpace(interop.DefaultUserWIF))
            {
                return interop.TestInvokeSCWithContractParamenets(address, methodName, parameters)
                              .ConfigureAwait(false)
                              .GetAwaiter()
                              .GetResult();
            }

            RpcApplicationLog? applicationLog = interop.InvokeSCWithContractParamenets(address, methodName, parameters)
                                                          .ConfigureAwait(false)
                                                          .GetAwaiter()
                                                          .GetResult();

            return applicationLog is null
                ? null
                : NeoInvokeResultsConverter.ConvertToRpcInvokeResult_RpcApplicationLog(applicationLog);
        }
        catch (Exception ex)
        {
            throw new SmartContractInvocationException(address, methodName, ex);
        }
    }

    private static void ExecuteContractWithoutResult(UInt160 address,
                                                      string methodName,
                                                      bool useTestNet,
                                                      bool testInvoke,
                                                      string? defaultWif,
                                                      params ContractParameter[] parameters)
    {
        _ = ExecuteContract(address, methodName, useTestNet, testInvoke, defaultWif, parameters);
    }

    private static T? ExecuteContractWithResult<T>(UInt160 address,
                                                   string methodName,
                                                   bool useTestNet,
                                                   bool testInvoke,
                                                   string? defaultWif,
                                                   params ContractParameter[] parameters)
    {
        RpcInvokeResult? result = ExecuteContract(address, methodName, useTestNet, testInvoke, defaultWif, parameters);
        return ExtractResult<T>(result);
    }

    private static T? ExtractResult<T>(RpcInvokeResult? invokeResult)
    {
        if (invokeResult?.Stack is null || invokeResult.Stack.Length == 0)
        {
            return default;
        }

        ContractParameter? parameter = invokeResult.Stack[0]?.ToParameter();
        if (parameter is null)
        {
            return default;
        }

        object? converted = ConvertContractParameter(parameter, typeof(T));
        return converted is null ? default : (T)converted;
    }

    private static object? ConvertContractParameter(ContractParameter parameter, Type targetType)
    {
        object? rawValue = ConvertParameterToClr(parameter);
        return ConvertFromRaw(targetType, rawValue);
    }

    private static object? ConvertParameterToClr(ContractParameter parameter)
        => parameter.Type switch
        {
            ContractParameterType.Boolean => parameter.Value,
            ContractParameterType.Integer => parameter.Value,
            ContractParameterType.ByteArray => parameter.Value,
            ContractParameterType.String => parameter.Value?.ToString(),
            ContractParameterType.Hash160 => parameter.Value?.ToString(),
            ContractParameterType.Hash256 => parameter.Value?.ToString(),
            ContractParameterType.PublicKey => parameter.Value?.ToString(),
            ContractParameterType.Array => ((IList<ContractParameter>)parameter.Value)
                .Select(ConvertParameterToClr)
                .ToList(),
            ContractParameterType.Map => ((IList<KeyValuePair<ContractParameter, ContractParameter>>)parameter.Value)
                .ToDictionary(kvp => ConvertParameterToClr(kvp.Key)?.ToString() ?? string.Empty,
                              kvp => ConvertParameterToClr(kvp.Value)),
            ContractParameterType.Any or ContractParameterType.Void => null,
            _ => parameter.Value
        };

    private static object? ConvertFromRaw(Type targetType, object? raw)
    {
        if (raw is null)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }

        if (targetType.IsAssignableFrom(raw.GetType()))
        {
            return raw;
        }

        if (targetType.IsEnum)
        {
            Type underlying = Enum.GetUnderlyingType(targetType);
            object? numeric = ConvertFromRaw(underlying, raw);
            return numeric is null ? Activator.CreateInstance(targetType) : Enum.ToObject(targetType, numeric);
        }

        if (targetType == typeof(string))
        {
            return raw.ToString();
        }

        if (targetType == typeof(bool))
        {
            return raw switch
            {
                bool b => b,
                string s when bool.TryParse(s, out bool parsed) => parsed,
                BigInteger bigInt => bigInt != BigInteger.Zero,
                _ => Convert.ToBoolean(raw)
            };
        }

        if (targetType == typeof(BigInteger))
        {
            return raw switch
            {
                BigInteger big => big,
                string s when BigInteger.TryParse(s, out BigInteger parsed) => parsed,
                byte[] bytes => new BigInteger(bytes),
                bool b => b ? BigInteger.One : BigInteger.Zero,
                _ when raw.GetType().IsPrimitive => new BigInteger(Convert.ToInt64(raw)),
                _ => BigInteger.Parse(raw.ToString() ?? "0")
            };
        }

        if (targetType == typeof(int))
        {
            BigInteger value = (BigInteger)ConvertFromRaw(typeof(BigInteger), raw)!;
            return (int)value;
        }

        if (targetType == typeof(long))
        {
            BigInteger value = (BigInteger)ConvertFromRaw(typeof(BigInteger), raw)!;
            return (long)value;
        }

        if (targetType == typeof(byte))
        {
            BigInteger value = (BigInteger)ConvertFromRaw(typeof(BigInteger), raw)!;
            return (byte)value;
        }

        if (targetType == typeof(ushort))
        {
            BigInteger value = (BigInteger)ConvertFromRaw(typeof(BigInteger), raw)!;
            return (ushort)value;
        }

        if (targetType == typeof(uint))
        {
            BigInteger value = (BigInteger)ConvertFromRaw(typeof(BigInteger), raw)!;
            return (uint)value;
        }

        if (targetType == typeof(ulong))
        {
            BigInteger value = (BigInteger)ConvertFromRaw(typeof(BigInteger), raw)!;
            return (ulong)value;
        }

        if (targetType == typeof(UInt160))
        {
            return raw switch
            {
                UInt160 hash => hash,
                string s when !string.IsNullOrWhiteSpace(s) => UInt160.Parse(s),
                byte[] bytes => UInt160.Parse(bytes.ToHexString()),
                _ => UInt160.Parse(raw.ToString() ?? string.Empty)
            };
        }

        if (targetType == typeof(UInt160))
        {
            object? parsed = ConvertFromRaw(typeof(UInt160), raw);
            return parsed;
        }

        if (targetType == typeof(UInt256))
        {
            return raw switch
            {
                UInt256 hash => hash,
                string s when !string.IsNullOrWhiteSpace(s) => UInt256.Parse(s),
                byte[] bytes => UInt256.Parse(bytes.ToHexString()),
                _ => UInt256.Parse(raw.ToString() ?? string.Empty)
            };
        }

        if (targetType == typeof(byte[]))
        {
            return raw switch
            {
                byte[] bytes => bytes,
                string s when !string.IsNullOrEmpty(s) => Convert.FromBase64String(s),
                BigInteger big => big.ToByteArray(),
                _ => Convert.FromBase64String(raw.ToString() ?? string.Empty)
            };
        }

        if (targetType.IsArray)
        {
            Type elementType = targetType.GetElementType()!;
            var rawEnumerable = raw as IEnumerable ?? new[] { raw };
            var values = rawEnumerable.Cast<object?>().ToList();
            Array array = Array.CreateInstance(elementType, values.Count);
            for (int i = 0; i < values.Count; i++)
            {
                array.SetValue(ConvertFromRaw(elementType, values[i]), i);
            }

            return array;
        }

        if (typeof(IEnumerable).IsAssignableFrom(targetType) && targetType.IsGenericType)
        {
            Type elementType = targetType.GetGenericArguments()[0];
            var listType = typeof(List<>).MakeGenericType(elementType);
            var list = (IList)Activator.CreateInstance(listType)!;
            foreach (object? item in (raw as IEnumerable ?? new[] { raw }).Cast<object?>())
            {
                list.Add(ConvertFromRaw(elementType, item));
            }

            if (targetType.IsAssignableFrom(list.GetType()))
            {
                return list;
            }

            object? collection = Activator.CreateInstance(targetType);
            if (collection is IList targetList)
            {
                foreach (object? item in list)
                {
                    targetList.Add(item);
                }
                return targetList;
            }

            return list;
        }

        string json = System.Text.Json.JsonSerializer.Serialize(raw, ContractJsonOptions);
        return System.Text.Json.JsonSerializer.Deserialize(json, targetType, ContractJsonOptions);
    }

    private static ContractParameter CreateContractParameter(object? value)
        => CreateContractParameter(value, new HashSet<object>(ReferenceEqualityComparer.Instance));

    private static ContractParameter CreateContractParameter(object? value, HashSet<object> visited)
    {
        if (value is null)
        {
            return new ContractParameter(ContractParameterType.Any) { Value = null };
        }

        if (!value.GetType().IsValueType)
        {
            if (!visited.Add(value))
            {
                return new ContractParameter(ContractParameterType.Any) { Value = null };
            }
        }

        switch (value)
        {
            case ContractParameter contractParameter:
                return contractParameter;
            case string str:
                return new ContractParameter(ContractParameterType.String) { Value = str };
            case bool boolean:
                return new ContractParameter(ContractParameterType.Boolean) { Value = boolean };
            case BigInteger bigInteger:
                return new ContractParameter(ContractParameterType.Integer) { Value = bigInteger };
            case byte[] bytes:
                return new ContractParameter(ContractParameterType.ByteArray) { Value = bytes };
            case UInt160 hash160:
                return new ContractParameter(ContractParameterType.Hash160) { Value = hash160 };
            case UInt256 hash256:
                return new ContractParameter(ContractParameterType.Hash256) { Value = hash256 };
            case Enum enumValue:
                return new ContractParameter(ContractParameterType.Integer)
                {
                    Value = new BigInteger(Convert.ToInt64(enumValue))
                };
            case byte b:
                return new ContractParameter(ContractParameterType.Integer) { Value = new BigInteger(b) };
            case sbyte sb:
                return new ContractParameter(ContractParameterType.Integer) { Value = new BigInteger(sb) };
            case short s:
                return new ContractParameter(ContractParameterType.Integer) { Value = new BigInteger(s) };
            case ushort us:
                return new ContractParameter(ContractParameterType.Integer) { Value = new BigInteger(us) };
            case int i:
                return new ContractParameter(ContractParameterType.Integer) { Value = new BigInteger(i) };
            case uint ui:
                return new ContractParameter(ContractParameterType.Integer) { Value = new BigInteger(ui) };
            case long l:
                return new ContractParameter(ContractParameterType.Integer) { Value = new BigInteger(l) };
            case ulong ul:
                return new ContractParameter(ContractParameterType.Integer) { Value = new BigInteger(ul) };
        }

        Type valueType = value.GetType();
        if (value is IDictionary dictionary)
        {
            return CreateMapParameter(dictionary.Cast<DictionaryEntry>().Select(entry => (entry.Key, entry.Value)), visited);
        }

        if (IsMapType(valueType))
        {
            return CreateMapParameter(EnumerateMapEntries(value), visited);
        }

        if (value is IEnumerable enumerable && value is not string)
        {
            var list = new List<ContractParameter>();
            foreach (object? item in enumerable)
            {
                list.Add(CreateContractParameter(item, visited));
            }
            return new ContractParameter(ContractParameterType.Array) { Value = list };
        }

        return CreateMapParameter(EnumerateObjectMembers(value), visited);
    }

    private static ContractParameter CreateMapParameter(IEnumerable<(object? Key, object? Value)> entries, HashSet<object> visited)
    {
        var mapEntries = new List<KeyValuePair<ContractParameter, ContractParameter>>();
        foreach ((object? Key, object? Value) in entries)
        {
            ContractParameter keyParam = CreateContractParameter(Key, visited);
            ContractParameter valueParam = CreateContractParameter(Value, visited);
            mapEntries.Add(new KeyValuePair<ContractParameter, ContractParameter>(keyParam, valueParam));
        }

        return new ContractParameter(ContractParameterType.Map) { Value = mapEntries };
    }

    private static IEnumerable<(object? Key, object? Value)> EnumerateObjectMembers(object value)
    {
        Type type = value.GetType();
        HashSet<string> processed = new(StringComparer.Ordinal);

        foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanRead || property.GetIndexParameters().Length > 0)
            {
                continue;
            }

            if (!processed.Add(property.Name))
            {
                continue;
            }

            yield return (property.Name, property.GetValue(value));
        }

        foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!processed.Add(field.Name))
            {
                continue;
            }

            yield return (field.Name, field.GetValue(value));
        }
    }

    private static IEnumerable<(object? Key, object? Value)> EnumerateMapEntries(object value)
    {
        if (value is IDictionary dictionary)
        {
            foreach (DictionaryEntry entry in dictionary)
            {
                yield return (entry.Key, entry.Value);
            }
            yield break;
        }

        if (value is IEnumerable enumerable)
        {
            foreach (object? item in enumerable)
            {
                if (item is null)
                {
                    continue;
                }

                if (item is DictionaryEntry entry)
                {
                    yield return (entry.Key, entry.Value);
                    continue;
                }

                PropertyInfo? keyProperty = item.GetType().GetProperty("Key");
                PropertyInfo? valueProperty = item.GetType().GetProperty("Value");
                if (keyProperty != null && valueProperty != null)
                {
                    yield return (keyProperty.GetValue(item), valueProperty.GetValue(item));
                }
            }
        }
    }

    private static bool IsMapType(Type type)
    {
        if (typeof(IDictionary).IsAssignableFrom(type))
        {
            return true;
        }

        if (type.IsGenericType)
        {
            Type genericDefinition = type.GetGenericTypeDefinition();
            if (genericDefinition == typeof(Dictionary<,>) || genericDefinition == typeof(IDictionary<,>))
            {
                return true;
            }
        }

        return type.Name.Contains("Map", StringComparison.OrdinalIgnoreCase);
    }

    private sealed class ReferenceEqualityComparer : IEqualityComparer<object>
    {
        public static readonly ReferenceEqualityComparer Instance = new();

        public bool Equals(object? x, object? y) => ReferenceEquals(x, y);

        public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
    }
}

public sealed class SmartContractInvocationException : Exception
{
    public SmartContractInvocationException(UInt160 contractAddress, string methodName, Exception inner)
        : base($"Invocation of '{methodName}' on contract '{contractAddress}' failed: {inner.Message}", inner)
    {
        ContractAddress = contractAddress;
        MethodName = methodName;
    }

    public UInt160 ContractAddress { get; }
    public string MethodName { get; }
}
