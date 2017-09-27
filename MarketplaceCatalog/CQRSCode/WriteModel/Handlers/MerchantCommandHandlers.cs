using System.Threading.Tasks;
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

        public async Task Handle(CreateMerchant message)
        {
            var merchant = new Merchant(message.Id, message.Name, message.Email);
            await _session.Add(merchant);
            await _session.Commit();
        }

        public async Task Handle(ActivateMerchant message)
        {
            var merchant = await _session.Get<Merchant>(message.Id);
            merchant.Activate();
            await _session.Commit();
        }

        public async Task Handle(DeactivateMerchant message)
        {
            var merchant = await _session.Get<Merchant>(message.Id);
            merchant.Deactivate();
            await _session.Commit();
        }
    }
}
