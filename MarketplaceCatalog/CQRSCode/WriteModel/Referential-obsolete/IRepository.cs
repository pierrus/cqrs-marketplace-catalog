using System.Linq;

namespace CQRSCode.WriteModel.Referential
{
    public interface IRefRepository<TModel> where TModel: class
    {
        void Save(TModel entity);

        IQueryable<TModel> Get();
    }
}