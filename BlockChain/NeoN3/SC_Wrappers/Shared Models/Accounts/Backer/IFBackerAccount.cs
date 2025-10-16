
using Neo;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;
using System;
using System.Numerics;

namespace InnFork.NeoN3;

public partial class IF_MainGateway
{
    //[Serializable]
    public partial class BackerAccount
    {
        public byte[] PublicKey = new byte[33];

        public UInt160 BackerWalletAddress = UInt160.Zero;

        public BigInteger TotalBalance;
        public BigInteger FreeBalance;

        public bool AllowAutoPaymentByInnForkGateway = true; // autopayments via the InnFork gateway

        public bool BackerAutoVotingPositive = true; // auto-voting for projects by baker. if Baker is financing, he most likely allows auto-voting (if he missed it)
                                                     // this is used when this baker account has invested in the candidate's manufacturer

        public string Name { get; internal set; }

    }

}