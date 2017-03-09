using System;
using CQRSlite.Commands;

namespace CQRSCode.WriteModel.Commands
{
    public class CreateCategory : ICommand 
	{
        public readonly String Name;

        public readonly Guid? ParentId;

        public CreateCategory(Guid id, String name, Guid? parentId) 
		{
            Id = id;
            Name = name;
            ParentId = parentId;
        }

        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
	}
}