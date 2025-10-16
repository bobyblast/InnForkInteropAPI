

using Neo;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;
using System;
using System.Numerics;


namespace InnFork.NeoN3;

public partial class IF_MainGateway //
{
    //[Serializable]
    public partial class InvestorAccount
    {
        public BigInteger TotalBalance { get; set; } = 0;
        public BigInteger FreeBalance { get; set; } = 0;
        public UInt160 InvestorWalletAddress { get; set; }

        public byte[] PublicKey { get; set; } = new byte[33];
        public UInt160 OwnerCreatorAddressAccount { get; set; }

    }



    public partial class InvestmentAgreement
    {
        public byte[] AssetId { get; set; } = new byte[64];
        public ulong DueDate { get; set; }
        public BigInteger InvestmentAmount { get; set; }
        public UInt160 Investor { get; set; }
        public UInt160 Manufacturer { get; set; }
        public BigInteger RepaidAmount { get; set; }
        public BigInteger RepaymentAmount { get; set; }
        public byte Status { get; set; }
        public string Name { get; internal set; }

    }
}
