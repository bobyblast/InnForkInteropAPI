using Neo.IO;
using Neo.Network.RPC.Models;
using Neo.Json;
using Neo.Network.RPC.Models;
using Neo.VM;
using Neo.VM.Types;
using Boolean = Neo.VM.Types.Boolean;
using Array = Neo.VM.Types.Array;

namespace InnFork.Blockchain.NEO3;

public class StackItemRecursiveReader
{
    // Рекурсивная функция для вывода StackItem
    public static void PrintStackItem(StackItem item, int indentLevel = 0)
    {
        try
        {
            string indent = new string(' ', indentLevel * 2);

            switch (item)
            {
                case ByteString byteString:
                    Console.WriteLine($"{indent}ByteString: {byteString.GetString()}");
                    break;

                case Integer integer:
                    Console.WriteLine($"{indent}Integer: {integer.GetInteger()}");
                    break;

                case Boolean boolean:
                    Console.WriteLine($"{indent}Boolean: {boolean.GetBoolean()}");
                    break;

                case Array array:
                    Console.WriteLine($"{indent}Array:");
                    foreach (var arrayItem in array)
                    {
                        PrintStackItem(arrayItem, indentLevel + 1);
                    }
                    break;

                case Map map:
                    Console.WriteLine($"{indent}Map:");
                    foreach (var mapItem in map)
                    {
                        Console.WriteLine($"{indent}  Key:");
                        PrintStackItem(mapItem.Key, indentLevel + 2);
                        Console.WriteLine($"{indent}  Value:");
                        PrintStackItem(mapItem.Value, indentLevel + 2);
                    }
                    break;

                case Null _:
                    Console.WriteLine($"{indent}Null");
                    break;

                default:
                    Console.WriteLine($"{indent}Unknown StackItem type: {item.GetType().Name}");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading StackItem: {ex.Message}");
        }

    }

    // Функция для обработки одного Execution
    public static void PrintExecution(Execution execution)
    {
        if (execution == null)
        {
            Console.WriteLine("Execution is null.");
            return;
        }

        Console.WriteLine($"Trigger: {execution.Trigger}");
        Console.WriteLine($"VMState: {execution.VMState}");
        Console.WriteLine($"GasConsumed: {execution.GasConsumed}");
        Console.WriteLine($"ExceptionMessage: {execution.ExceptionMessage ?? "None"}");

        if (execution.Stack != null)
        {
            Console.WriteLine("Stack:");
            foreach (var stackItem in execution.Stack)
            {
                PrintStackItem(stackItem, 1);
            }
        }

        if (execution.Notifications != null)
        {
            Console.WriteLine("Notifications:");
            foreach (var notification in execution.Notifications)
            {
                Console.WriteLine($"  Contract: {notification.Contract}, Event: {notification.EventName}");
                Console.WriteLine("  State:");
                PrintStackItem(notification.State, 2);
            }
        }
    }

    // Функция для обработки RpcInvokeResult
    public static void PrintRpcInvokeResult(RpcInvokeResult result)
    {
        if (result == null)
        {
            Console.WriteLine("RpcInvokeResult is null.");
            return;
        }

        Console.WriteLine("Executions:");
        foreach (var execution in result.Stack)
        {
            PrintStackItem(execution);
        }
    }

    // Функция для обработки списка Execution
    public static void PrintExecutions(List<Execution> executions)
    {
        if (executions == null)
        {
            Console.WriteLine("Executions list is null.");
            return;
        }

        foreach (var execution in executions)
        {
            PrintExecution(execution);
        }
    }
}