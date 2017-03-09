using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CQRSCode.WriteModel.Referential
{
    public class RefRepository<TModel> : IRefRepository<TModel> where TModel:class
    {
        RefDBContext _refDBContext = null;
        DbSet<TModel> _set = null;

        public RefRepository(RefDBContext refDBContext)
        {
            _refDBContext = refDBContext;
            _set = refDBContext.Set<TModel>();
        }

        public void Save(TModel entity)
        {
            _set.Add(entity);
            _refDBContext.SaveChanges();
        }

        public IQueryable<TModel> Get()
        {
            return _set.AsQueryable<TModel>();
        }
    }
}