using System.Threading.Tasks;
using CQRSCode.WriteModel.Commands;
using CQRSCode.WriteModel.Domain;
using CQRSlite.Commands;
using CQRSlite.Domain;

namespace CQRSCode.WriteModel.Handlers
{
    public class CategoryCommandHandlers : ICommandHandler<CreateCategory>,
											ICommandHandler<ActivateCategory>,
											ICommandHandler<DeactivateCategory>
    {
        private readonly ISession _session;

        public CategoryCommandHandlers(ISession session)
        {
            _session = session;
        }

        public async Task Handle(CreateCategory message)
        {
            var category = new Category(message.Id, message.Name, message.ParentId);
            await _session.Add(category);
            await _session.Commit();
        }

        public async Task Handle(ActivateCategory message)
        {
            var category = await _session.Get<Category>(message.Id, message.ExpectedVersion);
            await _session.Commit();
            category.Activate();
        }

        public async Task Handle(DeactivateCategory message)
        {
            var category = await _session.Get<Category>(message.Id, message.ExpectedVersion);
            await _session.Commit();
            category.Deactivate();
        }
    }
}
