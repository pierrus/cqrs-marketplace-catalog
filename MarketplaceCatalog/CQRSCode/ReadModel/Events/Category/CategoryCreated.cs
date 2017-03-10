using System;
using CQRSlite.Events;

namespace CQRSCode.ReadModel.Events
{
    public class CategoryCreated : IEvent 
	{
        public String Name { get; set; }
        public Guid? ParentId { get; set; }
        public Boolean Activated { get; set; }
                
        public CategoryCreated(Guid id, String name, Boolean activated, Guid? parentId = null) 
        {
            Id = id;
            Name = name;
            Activated = activated;
            ParentId = parentId;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}