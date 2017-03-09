using System;
using CQRSlite.Commands;

namespace CQRSCode.WriteModel.Commands
{
    public class ActivateCategoryOnProduct : ICommand 
	{
        public Guid CategoryId { get; set; }

        public ActivateCategoryOnProduct(Guid id, Guid categoryId) 
		{
            Id = id;
            CategoryId = categoryId;
        }

        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
	}
}