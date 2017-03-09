using CQRSCode.WriteModel.Commands;
using CQRSCode.WriteModel.Domain;
using CQRSlite.Commands;
using CQRSlite.Domain;

namespace CQRSCode.WriteModel.Handlers
{
    public class MerchantCommandHandlers : ICommandHandler<CreateMerchant>,
											ICommandHandler<ActivateMerchant>,
											ICommandHandler<DeactivateMerchant>
    {
        private readonly ISession _session;

        public MerchantCommandHandlers(ISession session)
        {
            _session = session;
        }

        public void Handle(CreateMerchant message)
        {
            var merchant = new Merchant(message.Id, message.Name, message.Email);
            _session.Add(merchant);
            _session.Commit();
        }

        public void Handle(ActivateMerchant message)
        {
            var merchant = _session.Get<Merchant>(message.Id);
            merchant.Activate();
            _session.Commit();
        }

        public void Handle(DeactivateMerchant message)
        {
            var merchant = _session.Get<Merchant>(message.Id);
            merchant.Deactivate();
            _session.Commit();
        }
    }
}
