
using Neo;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;
using System;
using System.Numerics;

namespace InnFork.NeoN3;

/*производитель может добавить проект \ продукт и указать сумму инвестиций которую он хочет получить.
если производитель не указал сумму инвестиций, то он не может получить инвестиции?
систему соглашения с контрактом инвестора встроить в смарт контракт производителя.

Чтобы гарантировать, что у производителя может быть только один инвестор, вы можете включить переменную в контракт производителя, 
в котором хранится адрес контракта инвестора. Прежде чем разрешить какой-либо отказ от контракта производителя, вы должны проверить, 
совпадает ли адрес вызывающего абонента с сохраненным адресом контракта инвестора.

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






















