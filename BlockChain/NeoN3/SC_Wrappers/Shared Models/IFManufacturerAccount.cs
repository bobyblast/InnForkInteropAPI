
using Neo;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;
using System;
using System.Numerics;

namespace InnFork.NeoN3;

/*������������� ����� �������� ������ \ ������� � ������� ����� ���������� ������� �� ����� ��������.
���� ������������� �� ������ ����� ����������, �� �� �� ����� �������� ����������?
������� ���������� � ���������� ��������� �������� � ����� �������� �������������.

����� �������������, ��� � ������������� ����� ���� ������ ���� ��������, �� ������ �������� ���������� � �������� �������������, 
� ������� �������� ����� ��������� ���������. ������ ��� ��������� �����-���� ����� �� ��������� �������������, �� ������ ���������, 
��������� �� ����� ����������� �������� � ����������� ������� ��������� ���������.

*/

public partial class IF_NeoFS_Products
{
    //[Serializable]
    public partial class ManufacturerAccount
    {
        public UInt160 ManufacturerWalletAddress { get; set; }
        public byte[] PublicKey { get; set; } = new byte[33];
        public UInt160 OwnerCreatorAddressAccount { get; set; }
        public UInt160 ManufacturerProjectAccountId { get; set; }
        public BigInteger TotalBalance { get; set; } = 0;
        public BigInteger FreeBalance { get; set; } = 0;
        public BigInteger InvestmentAmount { get; set; } = 0;

        public BigInteger ReputationScore { get; set; } = 0;
        public BigInteger ExternalRating { get; set; } = 0;

    }
}






















