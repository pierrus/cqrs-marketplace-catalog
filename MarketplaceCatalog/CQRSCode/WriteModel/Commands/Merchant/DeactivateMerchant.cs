using System;
using CQRSlite.Commands;

namespace CQRSCode.WriteModel.Commands
{
    public class DeactivateMerchant : ICommand 
	{
        public DeactivateMerchant(Guid id, int expectedVersion) 
		{
            Id = id;
            ExpectedVersion = expectedVersion;
        }

        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
	}
}