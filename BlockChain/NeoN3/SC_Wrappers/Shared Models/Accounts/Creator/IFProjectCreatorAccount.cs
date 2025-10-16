
using Neo;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;
using System;
using System.Numerics;

namespace InnFork.NeoN3;

// ������ ������ � ������������ ������� � ��������� ������ 
public partial class IF_MainGateway
{
    //[Serializable]
    public partial class ProjectCreatorAccount
    {

        public BigInteger TotalFLMUSDBalance { get; set; } = 0;
        public BigInteger FreeFLMUSDBalance { get; set; } = 0; // ������ ��������� �� ����� �������

        public UInt160 WalletAddress { get; set; } = UInt160.Zero;
        public string PublicKey { get; set; } = string.Empty;
        public string Name { get; internal set; }


    }


}