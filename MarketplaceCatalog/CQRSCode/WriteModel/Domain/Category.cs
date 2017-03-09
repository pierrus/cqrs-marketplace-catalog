using System;
using CQRSCode.ReadModel.Events;
using CQRSlite.Domain;
using System.Collections.Generic;

namespace CQRSCode.WriteModel.Domain
{
    public class Category : AggregateRoot
    {
        public bool Activated { get; set; }

        public Guid? ParentId { get; set; }


        private void Apply(CategoryCreated e)
        {
             Activated = e.Activated;
             ParentId = e.ParentId;
        }

        private void Apply(CategoryActivated e)
        {
             Activated = true;
        }

        private void Apply(CategoryDeactivated e)
        {
             Activated = false;
        }


        public void Activate()
        {
            if(Activated) throw new InvalidOperationException("already deactivated");
            ApplyChange(new CategoryActivated(Id));
        }

        public void Deactivate()
        {
            if(!Activated) throw new InvalidOperationException("already deactivated");
            ApplyChange(new CategoryDeactivated(Id));
        }

        private Category(){}
        
        public Category(Guid id, String name, Guid? parentId = null)
        {
            Id = id;
            ApplyChange(new CategoryCreated(id, name, true, parentId));
        }
    }
}