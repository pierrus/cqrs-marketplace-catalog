using System;
using CQRSlite.Commands;

namespace CQRSCode.WriteModel.Commands
{
    public class SetProductCategory : ICommand 
	{
        public readonly Guid ProductId;
        public readonly Guid CategoryId;

        public SetProductCategory(Guid productId, Guid categoryId) 
		{
            ProductId = productId;
            CategoryId = categoryId;
        }

        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
	}
}