using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CQRSCode.ReadModel.Repository
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        IList<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);
        TEntity GetById(Guid id);
    }
}