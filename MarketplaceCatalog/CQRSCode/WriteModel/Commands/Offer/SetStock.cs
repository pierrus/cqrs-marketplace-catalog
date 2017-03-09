using System;
using CQRSlite.Commands;

namespace CQRSCode.WriteModel.Commands
{
    public class SetStock : ICommand 
	{
        public readonly Guid ProductId;

        public readonly Int16 Stock;

        public SetStock(Guid id, Guid productId, Int16 stock, int expectedVersion) 
		{
            Id = id;
            ProductId = productId;
            Stock = stock;
            ExpectedVersion = expectedVersion;
        }

        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
	}
}