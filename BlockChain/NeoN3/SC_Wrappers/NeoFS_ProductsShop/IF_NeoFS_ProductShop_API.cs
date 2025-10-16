
using InnFork.NeoN3;

using Neo.SmartContract.Framework.Native;
using System;
using System.Numerics;
using InnFork.Blockchain.NEO3;
using Neo;
using Neo.SmartContract;
using Neo.VM.Types;

namespace InnForkInteropAPI.BlockChain.NeoN3.SC_Wrappers
{

    public partial class InnFork_SCPlatform
    {

        public class IF_NeoFS_ProductShop
        {
            public static UInt160 Address = "";
            public static bool testnet = false;
            public static bool TestInvoke = false;
            public static string? DefaultUserWif { get; set; }

            public static BigInteger CalculateFinallyProductPrice(UInt160 productId, UInt160 addonId, BigInteger Quantity)
            {
                return ExecuteContractWithResult<BigInteger>(Address,
                                                         nameof(CalculateFinallyProductPrice),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(productId, addonId, Quantity));
            }


            public static void cancelOrder(UInt160 orderId, UInt160 customerAddress)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(cancelOrder),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(orderId, customerAddress));
            }


            public static void confirmOrder(UInt160 orderId, UInt160 manufacturerAddress, UInt160 paymentTokenHash)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(confirmOrder),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(orderId, manufacturerAddress, paymentTokenHash));
            }


            public static void convertToFLMStableCoin()
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(convertToFLMStableCoin),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters());
            }


            public static void createManufacturerAccount(UInt160 manufacturerAddress, string name, byte[] publicKey)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(createManufacturerAccount),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(manufacturerAddress, name, publicKey));
            }


            public static void createOrder(UInt160 orderId, UInt160 productId, UInt160 customerId, BigInteger quantity, UInt160 customerAddress)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(createOrder),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(orderId, productId, customerId, quantity, customerAddress));
            }


            public static void depositCustomerFunds(UInt160 customerAddress, UInt160 token, BigInteger amount)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(depositCustomerFunds),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(customerAddress, token, amount));
            }


            public static void doRequest()
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(doRequest),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters());
            }


            public static IF_NeoFS_Products.ManufacturerAccount getManufacturerAccount(UInt160 manufacturerAddress)
            {
                return ExecuteContractWithResult<IF_NeoFS_Products.ManufacturerAccount>(Address,
                                                         nameof(getManufacturerAccount),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(manufacturerAddress));
            }


            public static Dictionary<string, BigInteger> getManufacturerSalesAnalytics(UInt160 manufacturerAddress)
            {
                return ExecuteContractWithResult<Dictionary<string, BigInteger>>(Address,
                                                                          nameof(getManufacturerSalesAnalytics),
                                                                          testnet,
                                                                          TestInvoke,
                                                                          DefaultUserWif,
                                                                          BuildParameters(manufacturerAddress));
            }

            public static IF_NeoFS_Products.Order getOrder(UInt160 orderId)
            {
                return ExecuteContractWithResult<IF_NeoFS_Products.Order>(Address,
                                                         nameof(getOrder),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(orderId));
            }


            public static IF_NeoFS_Products.ProductSalesStats getProductStatistics(UInt160 productId)
            {
                return ExecuteContractWithResult<IF_NeoFS_Products.ProductSalesStats>(Address,
                                                         nameof(getProductStatistics),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(productId));
            }


            public static string GetResponse()
            {
                return ExecuteContractWithResult<string>(Address,
                                                         nameof(GetResponse),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters()) ?? string.Empty;
            }


            public static BigInteger getSwapRate(UInt160 fromToken, UInt160 toToken)
            {
                return ExecuteContractWithResult<BigInteger>(Address,
                                                         nameof(getSwapRate),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(fromToken, toToken));
            }


            public static BigInteger getTotalBackerRewards(string projectId, UInt160 backerAddress)
            {
                return ExecuteContractWithResult<BigInteger>(Address,
                                                         nameof(getTotalBackerRewards),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(projectId, backerAddress));
            }


            public static void onOracleFlamingoPriceResponse(string requestedUrl, object userData, OracleResponseCode oracleResponse, string jsonString)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(onOracleFlamingoPriceResponse),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(requestedUrl, userData, oracleResponse, jsonString));
            }


            public static void OracleCallback(ByteString userKey, ByteString result)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(OracleCallback),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(userKey, result));
            }



            public static UInt160 purchaseProduct(UInt160 productId, UInt160 customerAddress, BigInteger quantity, UInt160 paymentToken)
            {
                return ExecuteContractWithResult<UInt160>(Address,
                                                         nameof(purchaseProduct),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(productId, customerAddress, quantity, paymentToken)) ?? UInt160.Zero;
            }


            public static ByteString Query(string key)
            {
                return ExecuteContractWithResult<ByteString>(Address,
                                                         nameof(Query),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(key));
            }


            public static void registerCustomer(UInt160 customerAddress)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(registerCustomer),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(customerAddress));
            }


            public static UInt160 registerProduct(UInt160 manufacturerAddress, string projectId, string name, string description, string neoFSContainerId, string neoFSObjectId, BigInteger price, UInt160 priceToken, BigInteger quantity)
            {
                return ExecuteContractWithResult<UInt160>(Address,
                                                         nameof(registerProduct),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(manufacturerAddress, projectId, name, description, neoFSContainerId, neoFSObjectId, price, priceToken, quantity)) ?? UInt160.Zero;
            }


            public static void RequestObject(string containerId, string objectId, string userKey)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(RequestObject),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(containerId, objectId, userKey));
            }


            public static void setProductDiscount(UInt160 productId, bool isActive, BigInteger discountPercent)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(setProductDiscount),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(productId, isActive, discountPercent));
            }


            public static void setSwapRate(UInt160 fromToken, UInt160 toToken, BigInteger rate)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(setSwapRate),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(fromToken, toToken, rate));
            }


            public static BigInteger swapTokenToFUSD(UInt160 userAddress, UInt160 fromToken, BigInteger amount)
            {
                return ExecuteContractWithResult<BigInteger>(Address,
                                                         nameof(swapTokenToFUSD),
                                                         testnet,
                                                         TestInvoke,
                                                         DefaultUserWif,
                                                         BuildParameters(userAddress, fromToken, amount));
            }


            public static void updateProductStock(UInt160 productId, BigInteger newQuantity)
            {
                ExecuteContractWithoutResult(Address,
                                                 nameof(updateProductStock),
                                                 testnet,
                                                 TestInvoke,
                                                 DefaultUserWif,
                                                 BuildParameters(productId, newQuantity));
            }

        }
    }
}
