using System;
using CQRSlite.Commands;

namespace CQRSCode.WriteModel.Commands
{
    public class DeactivateCategoryOnProduct : ICommand 
	{
        public Guid CategoryId { get; set; }

        public DeactivateCategoryOnProduct(Guid id, Guid categoryId)
		{
            Id = id;
            CategoryId = categoryId;
        }

        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
	}
}