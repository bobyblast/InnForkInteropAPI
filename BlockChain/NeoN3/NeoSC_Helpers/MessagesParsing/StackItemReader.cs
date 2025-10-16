using Neo;
using Neo.IO;
using Neo.Network.RPC.Models;

using Neo.VM;
using Neo.VM.Types;
using System.Text;


namespace InnFork.Blockchain.NEO3;

public class StackItemReader
{

    public async Task DisplayInvokeResult(RpcInvokeResult RespTestInvoke)
    {
        Console.WriteLine("\n");

        Console.WriteLine("RespTestInvoke json is: " + RespTestInvoke.ToJson().ToString());

        foreach (StackItem parsedItem in RespTestInvoke.Stack)
        {

            if (parsedItem is Map parsedMap)
            {
                Console.WriteLine("Processing Map:");
                foreach (KeyValuePair<PrimitiveType, StackItem> mapEntry in parsedMap)
                {
                    Console.WriteLine($"Key: {mapEntry.Key}");

                    switch (mapEntry.Value)
                    {
                        case ByteString byteString:
                            try
                            {
                                Console.WriteLine($"Value (Base64): {byteString.GetString()}");

                                // Получаем массив байтов из ByteString
                                byte[] bytes = byteString.GetSpan().ToArray();

                                // Если длина массива равна 20 байтам (размер UInt160)
                                if (bytes.Length == 20)
                                {
                                    // Создаем UInt160 из массива байтов
                                    UInt160 uint160 = new UInt160(bytes);


                                    Console.WriteLine($"Value (UInt160): {uint160}");
                                    //    Console.WriteLine($"Value (Neo3 Address): {uint160.()}");
                                }
                                else
                                {
                                    // Если это не UInt160, выводим как -строку
                                    Console.WriteLine($"Value (Hex): {BitConverter.ToString(bytes).Replace("-", "")}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error decoding value: {ex.Message}");
                            }
                            break;

                        case Integer integer:
                            Console.WriteLine($"Value (Integer): {integer.GetInteger().ToString()}");
                            break;

                        case Neo.VM.Types.Boolean boolean:
                            Console.WriteLine($"Value (Boolean): {boolean.GetBoolean().ToString()}");
                            break;

                        case Neo.VM.Types.Array array:
                            Console.WriteLine($"Value (Array): {array.GetString()}");
                            break;

                        default:
                            Console.WriteLine($"Value (Unknown type): {mapEntry.Value.GetString()}");
                            break;
                    }
                    Console.WriteLine(); // Пустая строка для разделения элементов
                }
            }
            else if (parsedItem is Neo.VM.Types.Array parsedArray)
            {
                Console.WriteLine("Processing Array:");
                foreach (var arrayItem in parsedArray.ToArray())
                {
                    // Console.WriteLine($"Array item: {arrayItem.get.GetValueString()}");
                }
            }
            else if (parsedItem is Integer parsedInteger)
            {
                Console.WriteLine($"Integer value: {parsedInteger.GetInteger()}");
            }
            else if (parsedItem is Neo.VM.Types.Boolean parsedBoolean)
            {
                Console.WriteLine($"Boolean value: {parsedBoolean.GetBoolean()}");
            }
            else if (parsedItem is ByteString parsedByteString)
            {
                if (parsedByteString == null) continue;

                try
                {
                    Console.WriteLine($"Value (Base64): {parsedByteString.ToString()}");

                    // Получаем массив байтов из ByteString
                    byte[] bytes = parsedByteString.GetSpan().ToArray();

                    // Если длина массива равна 20 байтам (размер UInt160)
                    if (bytes.Length == 20)
                    {
                        // Создаем UInt160 из массива байтов
                        UInt160 uint160 = new UInt160(bytes);
                        Console.WriteLine($"Value (UInt160): {uint160}");
                        //     Console.WriteLine($"Value (Neo3 Address): {uint160.ToAddress()}");
                    }
                    else
                    {
                        Console.WriteLine($"Value (Hex): {Convert.ToHexString(bytes).ToLower()} ");

                        // Если это не UInt160, выводим как -строку
                        //    Console.WriteLine($"Value (Hex): {BitConverter.ToString(bytes).Replace("-", "")}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error decoding ByteString: {ex.Message}");
                }
            }
        }

        Console.WriteLine("\n");
    }

    public async Task DisplayTestInvokeResult1(RpcInvokeResult RespTestInvoke)
    {
        Console.WriteLine("RespTestInvoke json is: " + RespTestInvoke.ToJson().ToString());

        foreach (StackItem parsedItem in RespTestInvoke.Stack)
        {
            if (parsedItem is Map parsedMap)
            {
                Console.WriteLine("Processing Map:");
                foreach (KeyValuePair<PrimitiveType, StackItem> mapEntry in parsedMap)
                {
                    Console.WriteLine($"Key: {mapEntry.Key}");

                    switch (mapEntry.Value)
                    {
                        case ByteString byteString:
                            try
                            {
                                Console.WriteLine($"Value (Base64): {byteString.ToString()}");
                                // Декодируем Base64 в массив байтов
                                byte[] bytes = Convert.FromBase64String(byteString.GetString());

                                // Если длина массива равна 20 байтам (размер UInt160)
                                if (bytes.Length == 20)
                                {
                                    // Создаем UInt160 из массива байтов
                                    UInt160 uint160 = new UInt160(bytes);
                                    Console.WriteLine($"Value (UInt160): {uint160}");
                                }
                                else
                                {
                                    // Если это не UInt160, выводим как обычную строку
                                    Console.WriteLine($"Value (Decoded): {Encoding.UTF8.GetString(bytes)}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error decoding value: {ex.Message}");
                            }
                            break;

                        case Integer integer:
                            Console.WriteLine($"Value (Integer): {integer.GetInteger().ToString()}");
                            break;

                        case Neo.VM.Types.Boolean boolean:
                            Console.WriteLine($"Value (Boolean): {boolean.GetBoolean().ToString()}");
                            break;

                        case Neo.VM.Types.Array array:
                            Console.WriteLine($"Value (Array): {array.GetString()}");
                            break;

                        default:
                            Console.WriteLine($"Value (Unknown type): {mapEntry.Value.GetString()}");
                            break;
                    }
                    Console.WriteLine(); // Пустая строка для разделения элементов
                }
            }
            else if (parsedItem is Neo.VM.Types.Array parsedArray)
            {
                Console.WriteLine("Processing Array:");
                foreach (var arrayItem in parsedArray.ToArray())
                {
                    // Console.WriteLine($"Array item: {arrayItem.get.GetValueString()}");
                }
            }
            else if (parsedItem is Integer parsedInteger)
            {
                Console.WriteLine($"Integer value: {parsedInteger.GetInteger()}");
            }
            else if (parsedItem is Neo.VM.Types.Boolean parsedBoolean)
            {
                Console.WriteLine($"Boolean value: {parsedBoolean.GetBoolean()}");
            }
            else if (parsedItem is ByteString parsedByteString)
            {
                if (parsedByteString == null) continue;

                try
                {
                    Console.WriteLine($"Value (Base64): {parsedByteString.GetString()}");
                    byte[] bytes = Convert.FromBase64String(parsedByteString.GetString());
                    Console.WriteLine($"ByteString value (Decoded): {Encoding.UTF8.GetString(bytes)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error decoding ByteString: {ex.Message}");
                }
            }
        }
    }

    public async Task DisplayTestInvokeResult2(RpcInvokeResult RespTestInvoke)
    {

        Console.WriteLine("RespTestInvoke json is: " + RespTestInvoke.ToJson().ToString());

        foreach (StackItem parsedItem in RespTestInvoke.Stack)
        {
            if (parsedItem is Map parsedMap)
            {
                Console.WriteLine("Processing Map:");
                foreach (KeyValuePair<PrimitiveType, StackItem> mapEntry in parsedMap)
                {
                    Console.WriteLine($"Key: {mapEntry.Key}");

                    switch (mapEntry.Value)
                    {
                        case ByteString byteString:

                            try
                            {
                                Console.WriteLine($"Value (Base64): {byteString.ToString()}");
                                // Декодируем Base64 в массив байтов
                                byte[] bytes = byteString.GetSpan().ToArray();

                                // Если длина массива равна 20 байтам (размер UInt160)
                                if (bytes.Length == 20)
                                {
                                    // Создаем UInt160 из массива байтов
                                    UInt160 uint160 = new UInt160(bytes);
                                    Console.WriteLine($"Value (UInt160): {uint160}");
                                }
                                else
                                {
                                    // Если это не UInt160, выводим как обычную строку
                                    Console.WriteLine($"Value (Decoded): {byteString.GetString()}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error decoding value: {ex.Message}");
                                //     Console.WriteLine($"Value (Raw): {byteString.GetString()}");
                            }
                            break;

                        case Integer integer:
                            Console.WriteLine($"Value (Integer): {integer.GetInteger().ToString()}");
                            break;

                        case Neo.VM.Types.Boolean boolean:
                            Console.WriteLine($"Value (Boolean): {boolean.GetBoolean().ToString()}");
                            break;

                        case Neo.VM.Types.Array array:
                            Console.WriteLine($"Value (Array): {array.GetString()}");
                            break;

                        default:
                            Console.WriteLine($"Value (Unknown type): {mapEntry.Value.GetString()}");
                            break;
                    }
                    Console.WriteLine(); // Пустая строка для разделения элементов
                }
            }
            else if (parsedItem is Neo.VM.Types.Array parsedArray)
            {
                Console.WriteLine("Processing Array:");
                foreach (var arrayItem in parsedArray.ToArray())
                {
                    // Console.WriteLine($"Array item: {arrayItem.get.GetValueString()}");
                }
            }
            else if (parsedItem is Integer parsedInteger)
            {
                Console.WriteLine($"Integer value: {parsedInteger.GetInteger()}");
            }
            else if (parsedItem is Neo.VM.Types.Boolean parsedBoolean)
            {
                Console.WriteLine($"Boolean value: {parsedBoolean.GetBoolean()}");
            }
            else if (parsedItem is ByteString parsedByteString)
            {
                if (parsedByteString == null) continue;

                Console.WriteLine($"Value (Base64): {parsedByteString.GetString()}");
                Console.WriteLine($"ByteString value (Decoded): {parsedByteString.GetString()}");
            }
        }
    }

}