using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using System;
using Neo;
using Neo.Json;
using Neo.Network.RPC.Models;
using Neo.SmartContract;
using Neo.Extensions;
using InnFork.Blockchain.NEO3;

namespace InnForkInteropAPI.BlockChain.NeoN3.NeoSC_Helpers.MessagesParsing;

/// <summary>
/// Provides bidirectional conversions between InnFork specific RPC stack representations, native
/// NEO RPC models and <see cref="ContractParameter"/> values.
/// </summary>
public static class InnForkRpcStackConverter
{
    private static readonly string[] TrueLiterals = new[] { "true", "1", "yes", "y", "on" }; // support common truthy values
    private static readonly string[] FalseLiterals = new[] { "false", "0", "no", "n", "off" }; // support common falsy values

    public static bool TryConvertToClrValue(NeoRpcStackFabric.RpcEnumTypes type, string? rawValue, out object? value, out string? error)
    {
        try
        {
            value = ConvertToClrValueCore(type, rawValue);
            error = null;
            return true;
        }
        catch (Exception ex)
        {
            value = null;
            error = ex.Message;
            return false;
        }
    }

    public static ContractParameter ToContractParameter(InnFork_RpcStack stack)
    {
        if (stack is null)
        {
            throw new ArgumentNullException(nameof(stack));
        }

        var type = ParseRpcEnum(stack.Type);
        return ToContractParameter(type, stack.Value);
    }

    public static ContractParameter ToContractParameter(NeoRpcStackFabric.RpcEnumTypes type, object? value)
    {
        return type switch
        {
            NeoRpcStackFabric.RpcEnumTypes.Any => new ContractParameter(ContractParameterType.Any) { Value = null },
            NeoRpcStackFabric.RpcEnumTypes.Boolean => new ContractParameter(ContractParameterType.Boolean) { Value = ConvertToBoolean(value) },
            NeoRpcStackFabric.RpcEnumTypes.Integer => new ContractParameter(ContractParameterType.Integer) { Value = ConvertToBigInteger(value) },
            NeoRpcStackFabric.RpcEnumTypes.ByteArray => new ContractParameter(ContractParameterType.ByteArray) { Value = ConvertToByteArray(value) },
            NeoRpcStackFabric.RpcEnumTypes.String => new ContractParameter(ContractParameterType.String) { Value = value?.ToString() ?? string.Empty },
            NeoRpcStackFabric.RpcEnumTypes.Hash160 => new ContractParameter(ContractParameterType.Hash160) { Value = UInt160.Parse(value?.ToString() ?? throw new ArgumentNullException(nameof(value))) },
            NeoRpcStackFabric.RpcEnumTypes.Hash256 => new ContractParameter(ContractParameterType.Hash256) { Value = UInt256.Parse(value?.ToString() ?? throw new ArgumentNullException(nameof(value))) },

            NeoRpcStackFabric.RpcEnumTypes.PublicKey => new ContractParameter(ContractParameterType.PublicKey)
            {
                Value = Neo.Cryptography.ECC.ECPoint.Parse(value?.ToString() ??
            throw new ArgumentNullException(nameof(value)), Neo.Cryptography.ECC.ECCurve.Secp256r1)
            },

            NeoRpcStackFabric.RpcEnumTypes.Array => CreateArrayParameter(value),
            NeoRpcStackFabric.RpcEnumTypes.Void => new ContractParameter(ContractParameterType.Void) { Value = null },
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported RPC stack type")
        };
    }

    public static InnFork_RpcStack ToInnForkRpcStack(ContractParameter parameter)
    {
        if (parameter is null)
        {
            throw new ArgumentNullException(nameof(parameter));
        }

        var (type, value) = ConvertContractParameter(parameter, new HashSet<ContractParameter>());
        return new InnFork_RpcStack { Type = type, Value = value };
    }

    public static RpcStack ToRpcStack(NeoRpcStackFabric.RpcEnumTypes type, object? value)
    {
        return new RpcStack
        {
            Type = type.ToString(),
            Value = ToJTokenValue(value)
        };
    }

    public static RpcStack[] ToRpcStacks(IEnumerable<InnFork_RpcStack>? stacks)
    {
        if (stacks is null)
        {
            return Array.Empty<RpcStack>();
        }

        return stacks.Select(stack => new RpcStack
        {
            Type = ParseRpcEnum(stack.Type).ToString(),
            Value = ToJTokenValue(stack.Value)
        }).ToArray();
    }

    public static ContractParameter[] ToContractParameters(IEnumerable<InnFork_RpcStack>? stacks)
    {
        if (stacks is null)
        {
            return Array.Empty<ContractParameter>();
        }

        return stacks.Select(ToContractParameter).ToArray();
    }

    public static InnFork_RpcStack[] FromContractParameters(IEnumerable<ContractParameter>? parameters)
    {
        if (parameters is null)
        {
            return Array.Empty<InnFork_RpcStack>();
        }

        return parameters.Select(ToInnForkRpcStack).ToArray();
    }

    public static InnFork_RpcStack[] FromRpcStacks(IEnumerable<RpcStack>? stacks)
    {
        if (stacks is null)
        {
            return Array.Empty<InnFork_RpcStack>();
        }

        return stacks.Select(FromRpcStack).ToArray();
    }

    public static InnFork_RpcStack FromRpcStack(RpcStack stack)
    {
        if (stack is null)
        {
            throw new ArgumentNullException(nameof(stack));
        }

        return new InnFork_RpcStack
        {
            Type = stack.Type,
            Value = NormalizeRpcValue(stack.Value)
        };
    }

    public static NeoRpcStackFabric.RpcEnumTypes ParseRpcEnum(string? type)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            return NeoRpcStackFabric.RpcEnumTypes.Any;
        }

        if (Enum.TryParse<NeoRpcStackFabric.RpcEnumTypes>(type, true, out var result))
        {
            return result;
        }

        throw new ArgumentException($"Unknown RPC stack type '{type}'", nameof(type));
    }

    public static object? ConvertToClrValueCore(NeoRpcStackFabric.RpcEnumTypes type, string? rawValue)
    {
        return type switch
        {
            NeoRpcStackFabric.RpcEnumTypes.Any => null,
            NeoRpcStackFabric.RpcEnumTypes.Boolean => ConvertToBoolean(rawValue),
            NeoRpcStackFabric.RpcEnumTypes.Integer => ConvertToBigInteger(rawValue),
            NeoRpcStackFabric.RpcEnumTypes.ByteArray => ConvertToByteArray(rawValue),
            NeoRpcStackFabric.RpcEnumTypes.String => rawValue ?? string.Empty,
            NeoRpcStackFabric.RpcEnumTypes.Hash160 => rawValue is null ? throw new ArgumentNullException(nameof(rawValue)) : UInt160.Parse(rawValue),
            NeoRpcStackFabric.RpcEnumTypes.Hash256 => rawValue is null ? throw new ArgumentNullException(nameof(rawValue)) : UInt256.Parse(rawValue),
            NeoRpcStackFabric.RpcEnumTypes.PublicKey => rawValue is null ? throw new ArgumentNullException(nameof(rawValue)) : Neo.Cryptography.ECC.ECPoint.Parse(rawValue, Neo.Cryptography.ECC.ECCurve.Secp256r1),
            NeoRpcStackFabric.RpcEnumTypes.Array => ConvertToArray(rawValue),
            NeoRpcStackFabric.RpcEnumTypes.Void => null,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported RPC stack type")
        };
    }

    private static object? NormalizeRpcValue(object? value)
    {
        return value switch
        {
            null => null,
            JToken token => ConvertJToken(token),
            JsonElement element => ConvertJsonElement(element),
            RpcStack stack => NormalizeRpcValue(stack.Value),
            InnFork_RpcStack stack => NormalizeRpcValue(stack.Value),
            IEnumerable<InnFork_RpcStack> innForkStacks => innForkStacks.Select(s => s.ToJson()).ToArray(),
            IEnumerable enumerable when value is not string => enumerable.Cast<object?>().Select(NormalizeRpcValue).ToArray(),
            _ => value
        };
    }

    private static ContractParameter CreateArrayParameter(object? value)
    {
        if (value is null)
        {
            return new ContractParameter(ContractParameterType.Array) { Value = new List<ContractParameter>() };
        }

        if (value is IEnumerable enumerable)
        {
            var list = new List<ContractParameter>();
            foreach (var item in enumerable)
            {
                if (item is ContractParameter cp)
                {
                    list.Add(cp);
                }
                else if (item is InnFork_RpcStack innFork)
                {
                    list.Add(ToContractParameter(innFork));
                }
                else
                {
                    // fallback treat as string value
                    list.Add(new ContractParameter(ContractParameterType.String) { Value = item?.ToString() ?? string.Empty });
                }
            }

            return new ContractParameter(ContractParameterType.Array) { Value = list };
        }

        if (value is string json)
        {
            var parsed = ConvertToArray(json);
            return CreateArrayParameter(parsed);
        }

        throw new ArgumentException("Unsupported array value type", nameof(value));
    }

    private static IEnumerable<InnFork_RpcStack> ConvertToArray(string? rawValue)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            return Array.Empty<InnFork_RpcStack>();
        }

        using JsonDocument doc = JsonDocument.Parse(rawValue);
        if (doc.RootElement.ValueKind != JsonValueKind.Array)
        {
            throw new ArgumentException("Array values must be JSON arrays", nameof(rawValue));
        }

        var result = new List<InnFork_RpcStack>();
        foreach (var element in doc.RootElement.EnumerateArray())
        {
            if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty("type", out var typeProperty))
            {
                var type = typeProperty.GetString();
                object? value = null;
                if (element.TryGetProperty("value", out var valueProperty))
                {
                    value = ConvertJsonElement(valueProperty);
                }

                result.Add(new InnFork_RpcStack { Type = type ?? nameof(NeoRpcStackFabric.RpcEnumTypes.Any), Value = value });
            }
            else
            {
                result.Add(new InnFork_RpcStack { Type = nameof(NeoRpcStackFabric.RpcEnumTypes.String), Value = ConvertJsonElement(element) });
            }
        }

        return result;
    }

    private static object? ConvertJsonElement(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Null => null,
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.TryGetInt64(out var longValue) ? longValue : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Object => element.ToString(),
            JsonValueKind.Array => element.ToString(),
            _ => element.ToString()
        };
    }

    private static object? ConvertJToken(JToken token)
    {
        // Normalize NeoN3.Json.JToken into simple CLR representations
        return token switch
        {
            JObject obj => obj,
            JArray array => array,
            _ => token.ToString()
        };
    }

    private static JToken ToJTokenValue(object? value)
    {
        switch (value)
        {
            case null:
                return JToken.Null;
            case JToken token:
                return token;
            case bool b:
                return new JBoolean(b);
            case string s:
                return new JString(s);
            case sbyte or byte or short or ushort or int or uint or long or ulong:
                return new JNumber(Convert.ToDouble(value, CultureInfo.InvariantCulture));
            case float or double or decimal:
                return new JNumber(Convert.ToDouble(value, CultureInfo.InvariantCulture));
            case BigInteger bi:
                // Represent BigInteger as string to avoid precision loss
                return new JString(bi.ToString(CultureInfo.InvariantCulture));
            case IEnumerable<InnFork_RpcStack> innForkStacks:
                {
                    var jarr = new JArray();
                    foreach (var s in innForkStacks)
                        jarr.Add(s.ToJson());
                    return jarr;
                }
            case IEnumerable enumerable when value is not string:
                {
                    var jarr = new JArray();
                    foreach (var item in enumerable)
                        jarr.Add(ToJTokenValue(item));
                    return jarr;
                }
            case ContractParameter cp:
                return NeoRpcStackFabric.ContractParameterToJson(cp);
            case InnFork_RpcStack stack:
                return stack.ToJson();
            default:
                return new JString(value?.ToString() ?? string.Empty);
        }
    }

    private static bool ConvertToBoolean(object? value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value), "Boolean value cannot be null");
        }

        if (value is bool b)
        {
            return b;
        }

        if (value is string s)
        {
            var trimmed = s.Trim();
            if (bool.TryParse(trimmed, out bool boolValue))
            {
                return boolValue;
            }

            if (Array.Exists(TrueLiterals, literal => literal.Equals(trimmed, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            if (Array.Exists(FalseLiterals, literal => literal.Equals(trimmed, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            if (long.TryParse(trimmed, NumberStyles.Integer, CultureInfo.InvariantCulture, out long numeric))
            {
                if (numeric is 0 or 1)
                {
                    return numeric == 1;
                }
            }

            throw new FormatException($"Cannot convert '{value}' to boolean");
        }

        if (value is sbyte or byte or short or int or long)
        {
            return Convert.ToInt64(value, CultureInfo.InvariantCulture) != 0;
        }

        if (value is BigInteger bigInteger)
        {
            return !bigInteger.IsZero;
        }

        throw new FormatException($"Cannot convert value of type {value.GetType()} to boolean");
    }

    private static BigInteger ConvertToBigInteger(object? value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (value is BigInteger bi)
        {
            return bi;
        }

        if (value is string s)
        {
            return BigInteger.Parse(s.Trim(), CultureInfo.InvariantCulture);
        }

        if (value is sbyte or byte or short or int or long or ulong)
        {
            return new BigInteger(Convert.ToInt64(value, CultureInfo.InvariantCulture));
        }

        if (value is uint or ushort)
        {
            return new BigInteger(Convert.ToInt64(value, CultureInfo.InvariantCulture));
        }

        throw new FormatException($"Cannot convert value of type {value.GetType()} to BigInteger");
    }

    private static byte[] ConvertToByteArray(object? value)
    {
        if (value is null)
        {
            return Array.Empty<byte>();
        }

        if (value is byte[] bytes)
        {
            return bytes;
        }

        if (value is string s)
        {
            var trimmed = s.Trim();
            if (trimmed.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                trimmed = trimmed[2..];
            }

            if (trimmed.Length % 2 == 0 && trimmed.All(IsHexDigit))
            {
                var buffer = new byte[trimmed.Length / 2];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = Convert.ToByte(trimmed.Substring(i * 2, 2), 16);
                }

                return buffer;
            }

            try
            {
                return Convert.FromBase64String(trimmed);
            }
            catch (FormatException)
            {
                throw new FormatException("ByteArray values must be valid hex or Base64 encoded strings");
            }
        }

        throw new FormatException($"Cannot convert value of type {value.GetType()} to byte array");
    }

    private static bool IsHexDigit(char c) => c is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F';

    private static (string Type, object? Value) ConvertContractParameter(ContractParameter parameter, HashSet<ContractParameter> context)
    {
        if (context.Contains(parameter))
        {
            return (parameter.Type.ToString(), null);
        }

        context.Add(parameter);

        switch (parameter.Value)
        {
            case null:
                return (parameter.Type.ToString(), null);

            case byte[] data:
                return (parameter.Type.ToString(), data.ToHexString());

            case IList<ContractParameter> list:
                var array = new List<InnFork_RpcStack>();
                foreach (var item in list)
                {
                    array.Add(ToInnForkRpcStack(item));
                }
                return (parameter.Type.ToString(), array);

            case IList<KeyValuePair<ContractParameter, ContractParameter>> map:
                var jArray = new JArray();
                foreach (var pair in map)
                {
                    var key = ToInnForkRpcStack(pair.Key);
                    var value = ToInnForkRpcStack(pair.Value);
                    JObject obj = new();
                    obj["key"] = key.ToJson();
                    obj["value"] = value.ToJson();
                    jArray.Add(obj);
                }
                return (parameter.Type.ToString(), jArray);

            default:
                return (parameter.Type.ToString(), parameter.Value);
        }
    }
}
