using System;
using CQRSlite.Commands;

namespace CQRSCode.WriteModel.Commands
{
    public class DeactivateCategory : ICommand 
	{
        public DeactivateCategory(Guid id, int expectedVersion) 
		{
            Id = id;
            ExpectedVersion = expectedVersion;
        }

        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
	}
}