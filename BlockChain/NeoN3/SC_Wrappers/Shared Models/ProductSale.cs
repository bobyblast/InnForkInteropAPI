
using Neo;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;
using System;
using System.ComponentModel;
using System.Numerics;

namespace InnFork.NeoN3;

public partial class IF_NeoFS_Products
{
    public struct SaleRecord
    {
        public UInt160 SaleId;
        public UInt160 ProductId;
        public UInt160 ManufacturerAddress;
        public UInt160 CustomerAddress;
        public BigInteger Quantity;
        public BigInteger TotalPrice;
        public UInt160 PaymentToken;
        public ulong SaleTimestamp;
        public string ProjectId;
        public bool RewardsDistributed;
    }

    public struct ProductSalesStats
    {
        public UInt160 ProductId;
        public BigInteger TotalSales;
        public BigInteger TotalRevenue;
        public BigInteger TotalQuantitySold;
        public ulong LastSaleTimestamp;
    }

    public struct Customer
    {
        public UInt160 CustomerAddress;
        public BigInteger Balance;
        public ulong RegisteredAt;
        public BigInteger TotalPurchases;
    }

}