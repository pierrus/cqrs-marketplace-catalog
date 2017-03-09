using System;
using CQRSCode.ReadModel.Events;
using CQRSlite.Domain;

namespace CQRSCode.WriteModel.Domain
{
    public class Merchant : AggregateRoot
    {
        public bool Activated { get; set; }

        public String Name { get; set; }

        private void Apply(MerchantDeactivated e)
        {
            Activated = false;
        }

        private void Apply(MerchantActivated e)
        {
            Activated = true;
        }

        private void Apply(MerchantCreated e)
        {
            Activated = true;
            Name = e.Name;
        }

        public void Deactivate()
        {
            if(!Activated) throw new InvalidOperationException("already deactivated");
            ApplyChange(new MerchantDeactivated(Id));
        }

        public void Activate()
        {
            if(Activated) throw new InvalidOperationException("already activated");
            ApplyChange(new MerchantActivated(Id));
        }

        private Merchant(){}
        
        public Merchant(Guid id, String name, String email)
        {
            Id = id;
            ApplyChange(new MerchantCreated(id, name, email));
        }
    }
}