using System;
using CQRSlite.Commands;

namespace CQRSCode.WriteModel.Commands
{
    public class SetProductCategory : ICommand 
	{
        public readonly Guid CategoryId;

        public SetProductCategory(Guid productId, Guid categoryId, int expectedVersion) 
		{
            Id = productId;
            CategoryId = categoryId;
            ExpectedVersion = expectedVersion;
        }

        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
	}
}