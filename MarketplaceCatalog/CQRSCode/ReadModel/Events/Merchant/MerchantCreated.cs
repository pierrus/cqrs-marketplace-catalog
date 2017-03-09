using System;
using CQRSlite.Events;

namespace CQRSCode.ReadModel.Events
{
    public class MerchantCreated : IEvent 
	{
        public readonly string Name;
        public readonly string Email;
        public MerchantCreated(Guid id, string name, String email) 
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}