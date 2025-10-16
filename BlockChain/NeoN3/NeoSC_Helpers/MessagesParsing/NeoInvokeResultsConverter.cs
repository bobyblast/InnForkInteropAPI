using Neo;
using Neo.Cryptography.ECC;
using Neo.Extensions;
using Neo.IO;
using Neo.Json;
using Neo.Network.RPC.Models;
using Neo.SmartContract;
using Neo.VM;
using Neo.VM.Types;
using System.Numerics;


namespace InnFork.Blockchain.NEO3;


public class NeoInvokeResultsConverter
{
    public static RpcInvokeResult ConvertToRpcInvokeResult(string executionJson, string txId)
    {
        if (string.IsNullOrEmpty(executionJson)) return null;

        JToken? jsonObject = null;
        try
        {
            jsonObject = JToken.Parse(executionJson);
        }
        catch { return null; }


        RpcInvokeResult rpcInvokeResult = new RpcInvokeResult
        {
            Script = null,
            State = jsonObject["vmstate"]?.GetEnum<VMState>() ?? VMState.NONE,
            GasConsumed = long.Parse(jsonObject["gasconsumed"]?.AsString() ?? "")
        };

        rpcInvokeResult.Exception = jsonObject["exception"]?.AsString() ?? "";
        rpcInvokeResult.Session = jsonObject["session"]?.AsString() ?? "";


        //rpcInvokeResult.Tx = txId;

        try
        {
            rpcInvokeResult.Stack = ((JArray)jsonObject["stack"]).Select((p) => Neo.Network.RPC.Utility.StackItemFromJson((JObject)p)).ToArray();
        }
        catch { }

        try
        {
            rpcInvokeResult.Script = jsonObject["script"]?.AsString();
        }
        catch { }

        rpcInvokeResult.Tx = jsonObject["tx"]?.AsString();

        return rpcInvokeResult;
    }

    public static RpcInvokeResult ConvertToRpcInvokeResult_JObject(JObject executionJson, string txId)
    {
        return ConvertToRpcInvokeResult(executionJson.ToString(), txId);
    }

    public static RpcInvokeResult ConvertToRpcInvokeResult_RpcApplicationLog(RpcApplicationLog rpcAppLog_Param)
    {
        return ConvertToRpcInvokeResult_JObject(rpcAppLog_Param.Executions[0].ToJson(), rpcAppLog_Param.TxId.ToString());
    }

}




public class NeoResultValueParser
{
    public static T GetValue<T>(RpcInvokeResult rpcInvokeResult, T defaultValue = default)
    {

        if (rpcInvokeResult != null && rpcInvokeResult.Stack != null && rpcInvokeResult.Stack.Length > 0)
        {
            StackItem item = rpcInvokeResult.Stack[0];

            if (item == null || item.Type.Equals(StackItemType.Any))
                return defaultValue;
            if (item is Neo.VM.Types.Boolean booleanValue)
                return (T)(object)booleanValue.GetBoolean();
            if (item is Neo.VM.Types.Integer integerValue)
                return (T)(object)integerValue.GetInteger();
            if (item is Neo.VM.Types.ByteString stringValue)
                return (T)(object)stringValue.GetString();
            if (item is Neo.VM.Types.Array arrayValue)
                return (T)(object)arrayValue.ToArray();
            if (item is Neo.VM.Types.Map mapValue)
                return (T)(object)mapValue.ToDictionary();
            if (item is Neo.VM.Types.Buffer BufferValue)
                return (T)(object)BufferValue.InnerBuffer.ToArray();

        }

        return defaultValue;
    }

    public static T GetValueFromParameter<T>(RpcInvokeResult rpcInvokeResult, int ValueIndex, T defaultValue = default)
    {
        if (rpcInvokeResult != null && rpcInvokeResult.Stack != null && rpcInvokeResult.Stack.Length > ValueIndex)
        {
            ContractParameter item = rpcInvokeResult.Stack[ValueIndex].ToParameter();

            object itemValue = item.Value; // ѕолучаем значение из ContractParameter
            ContractParameterType Type = item.Type;// 

            // ѕровер€ем тип значени€ и приводим его к нужному типу
            if (itemValue == null)
                return defaultValue;

            if (Type == ContractParameterType.Boolean && itemValue is bool boolValue)
            {
                return (T)(object)boolValue;
            }
            else if (Type == ContractParameterType.Integer && itemValue is BigInteger bigIntValue)
            {
                return (T)(object)bigIntValue;
            }
            else if (Type == ContractParameterType.ByteArray && itemValue is byte[] byteArray)
            {
                return (T)(object)byteArray;
            }
            else if (Type == ContractParameterType.String && itemValue is string stringValue)
            {
                return (T)(object)stringValue;
            }
            else if (Type == ContractParameterType.Hash160 && itemValue is UInt160 uint160Value)
            {
                return (T)(object)uint160Value;
            }
            else if (Type == ContractParameterType.Hash256 && itemValue is UInt256 uint256Value)
            {
                return (T)(object)uint256Value;
            }
            else if (Type == ContractParameterType.PublicKey && itemValue is Neo.Cryptography.ECC.ECPoint ecPoint)
            {
                return (T)(object)ecPoint;
            }
            else if (Type == ContractParameterType.Array && itemValue is Neo.VM.Types.Array arrayValue)
            {
                return (T)(object)arrayValue.ToArray();
            }
            else if (Type == ContractParameterType.Map && itemValue is Neo.VM.Types.Map mapValue)
            {
                return (T)(object)mapValue.ToDictionary();
            }
            else if (Type == ContractParameterType.InteropInterface && itemValue is Neo.VM.Types.InteropInterface interopValue)
            {
                return (T)(object)interopValue;
            }
            else if (Type == ContractParameterType.Void && itemValue == null)
            {
                return defaultValue;
            }
            else if (Type == ContractParameterType.Signature && itemValue is byte[] signatureBytes)
            {
                return (T)(object)signatureBytes;
            }
            else if (Type == ContractParameterType.Any)
            {
                return (T)itemValue; // ¬озвращаем значение как есть, если тип Any
            }

        }
        return defaultValue;

    }


    public static ContractParameter ConvertStackItemToContractParameter(StackItem item)
    {
        if (item == null) return null;
        StackItemType type = item.Type;

        ContractParameter parameter;

        switch (type)
        {
            case StackItemType.Any:
                parameter = new ContractParameter(ContractParameterType.Any);
                break;

            case StackItemType.Boolean:
                parameter = new ContractParameter(ContractParameterType.Boolean);
                parameter.Value = item.GetBoolean();
                break;

            case StackItemType.Integer:
                parameter = new ContractParameter(ContractParameterType.Integer);
                parameter.Value = item.GetInteger();
                break;

            case StackItemType.ByteString:
                // ѕытаемс€ преобразовать в строку, если это возможно
                try
                {
                    string str = item.GetString();
                    parameter = new ContractParameter(ContractParameterType.String);
                    parameter.Value = str;
                }
                catch
                {
                    // ≈сли не удалось преобразовать в строку, возвращаем как байтовый массив
                    byte[] bytes = item.GetSpan().ToArray();
                    parameter = new ContractParameter(ContractParameterType.ByteArray);
                    parameter.Value = bytes;
                }
                break;

            case StackItemType.Buffer:
                var bufferItem = (Neo.VM.Types.Buffer)item;
                parameter = new ContractParameter(ContractParameterType.ByteArray);
                parameter.Value = bufferItem.InnerBuffer.ToArray();
                break;

            case StackItemType.Array:
            case StackItemType.Struct:
                var arrayItem = (Neo.VM.Types.Array)item;
                parameter = new ContractParameter(ContractParameterType.Array);
                var arrayValue = new List<ContractParameter>();

                foreach (var element in arrayItem)
                {
                    var paramElement = ConvertStackItemToContractParameter(element);
                    if (paramElement != null)
                    {
                        arrayValue.Add(paramElement);
                    }
                }
                parameter.Value = arrayValue;
                break;

            case StackItemType.Map:
                var mapItem = (Neo.VM.Types.Map)item;
                parameter = new ContractParameter(ContractParameterType.Map);
                var mapValue = new List<KeyValuePair<ContractParameter, ContractParameter>>();

                foreach (var kvp in mapItem)
                {
                    var keyParam = ConvertStackItemToContractParameter(kvp.Key);
                    var valueParam = ConvertStackItemToContractParameter(kvp.Value);

                    if (keyParam != null && valueParam != null)
                    {
                        mapValue.Add(new KeyValuePair<ContractParameter, ContractParameter>(keyParam, valueParam));
                    }
                }
                parameter.Value = mapValue;
                break;

            case StackItemType.InteropInterface:
                parameter = new ContractParameter(ContractParameterType.InteropInterface);
                parameter.Value = item;
                break;

            case StackItemType.Pointer:
                try
                {
                    var pointerItem = (Neo.VM.Types.Pointer)item;
                    parameter = new ContractParameter(ContractParameterType.Integer);
                    parameter.Value = pointerItem.Position;
                }
                catch
                {
                    parameter = new ContractParameter(ContractParameterType.Any);
                }
                break;

            default:
                throw new NotSupportedException($"Ќеподдерживаемый тип StackItem: {type}");
        }

        return parameter;
    }

    public static async Task<string[]> GetSerializedStringArray(StackItem[] stackItems)
    {
        List<string> finalResults = new List<string>();
        foreach (var item in stackItems)
        {
            if (item is Neo.VM.Types.ByteString byteString)
            {

                StackItem deserializedItem = Neo.SmartContract.BinarySerializer.Deserialize(byteString.GetSpan().ToArray(), new ExecutionEngineLimits(), null);
                if (deserializedItem is Neo.VM.Types.Array array)
                {
                    foreach (var innerItem in array)
                    {
                        if (innerItem is Neo.VM.Types.Array innerArray)
                        {
                            foreach (var innerInnerItem in innerArray)
                            {
                                if (innerInnerItem is Neo.VM.Types.ByteString)
                                {
                                    string String = BitConverter.ToString(innerInnerItem.GetSpan().ToArray()).Replace("-", "");

                                    finalResults.Add(String);
                                }
                            }
                        }
                        else if (innerItem is Neo.VM.Types.ByteString)
                        {
                            string innerString = innerItem.GetString();
                            finalResults.Add(innerString);
                        }
                    }
                }
                else if (deserializedItem is Neo.VM.Types.ByteString)
                {
                    string innerString = deserializedItem.GetString();
                    finalResults.Add(innerString);
                }
            }
        }
        return finalResults.ToArray();
    }

    public static async Task<StackItem> GetSerializedStackItem(StackItem[] stackItems, int ValueIndex)
    {
        if (stackItems == null || stackItems.Length <= ValueIndex) return null;

        StackItem item = stackItems[ValueIndex];

        if (item is Neo.VM.Types.ByteString byteString)
        {
            //   string String = BitConverter.ToString(byteString.GetSpan().ToArray()).Replace("-", "");
            StackItem deserialized = Neo.SmartContract.BinarySerializer.Deserialize(byteString, new ExecutionEngineLimits(), null);

            return deserialized;

            StackItem deserializedItem;
            try
            {
                deserializedItem = Neo.SmartContract.BinarySerializer.Deserialize(byteString.GetSpan().ToArray(), new ExecutionEngineLimits(), null);
            }
            catch
            {
                return null;
            }

            return deserializedItem;
        }

        return null;
    }


}



