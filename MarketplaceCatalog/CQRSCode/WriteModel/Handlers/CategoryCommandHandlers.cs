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

        public void Handle(CreateCategory message)
        {
            var category = new Category(message.Id, message.Name, message.ParentId);
            _session.Add(category);
            _session.Commit();
        }

        public void Handle(ActivateCategory message)
        {
            var category = _session.Get<Category>(message.Id, message.ExpectedVersion);
            category.Activate();
            _session.Commit();
        }

        public void Handle(DeactivateCategory message)
        {
            var category = _session.Get<Category>(message.Id, message.ExpectedVersion);
            category.Deactivate();
            _session.Commit();
        }
    }
}
