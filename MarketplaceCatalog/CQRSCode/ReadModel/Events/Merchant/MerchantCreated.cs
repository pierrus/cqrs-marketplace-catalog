using System;
using CQRSlite.Events;

namespace CQRSCode.ReadModel.Events
{
    public class MerchantCreated : IEvent 
	{
        public string Name { get; set; }
        public string Email { get; set; }
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