
using Neo;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;

using System;
using System.Numerics;
using System.Threading;

namespace InnFork.NeoN3;

public partial class IF_NeoFS_Products
{
    /*

    3. Реализовать метод для создания заказа в смарт-контракте - createOrder(). 
       Метод принимает идентификатор продукта и количество. При вызове происходит резервирование указанного количества продукта.

    4. Реализовать запись созданного заказа в хранилище NEO FS, используя оракул для доступа.
       У заказа должен быть уникальный идентификатор.

    5. Реализовать метод для подтверждения заказа продавцом в смарт-контракте - confirmOrder().
       Метод принимает идентификатор заказа. При вызове метода заказ помечается как подтвержденный. 

    6. При подтверждении заказа, из смарт-контракта инициируется транзакция списания средств с покупателя на счет продавца.
     
    */

    public struct Order
    {
        public UInt160 Id { get; set; }
        public UInt160 ProductId { get; set; }
        public UInt160 CustomerId { get; set; }
        public UInt160 CustomerNeoN3Address { get; set; }
        public ulong OrderDate { get; set; }
        public string ProductName { get; set; }
        public BigInteger Price { get; set; }
        public BigInteger Amount { get; set; }
        public BigInteger Quantity { get; set; }
        public string shipping_FirstName { get; set; }
        public string shipping_LastName { get; set; }
        public string shipping_Address { get; set; }
        public string shipping_City { get; set; }
        public string shipping_ZipCode { get; set; }
        public bool IsConfirmed { get; set; }
    }

}