using System;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.Extensions.Options;


/// <summary>
/// A generic MongoDB repository. Maps to a collection with the same name
/// as type TEntity.
/// Can be inherited for a more specific behavior with _collectionGetter
/// </summary>
/// <typeparam name="T">Entity type for this repository</typeparam>

namespace CQRSCode.ReadModel.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private IMongoDatabase _database;
        private IMongoCollection<TEntity> _collection;
        private String _connectionString;
        private String _databaseName;
        protected Func<TEntity, IMongoCollection<TEntity>> _collectionGetter = null;

        public Repository(MongoOptions mongoOptions)
        {
            _databaseName = mongoOptions.Database;
            _connectionString = mongoOptions.ConnectionString;

            var client = new MongoClient(_connectionString);
            _database = client.GetDatabase(_databaseName);
            _collection = _database.GetCollection<TEntity>(typeof(TEntity).Name.ToLower());
        }

        public void Insert(TEntity entity)
        {
            // var collection = GetCollection(entity);

            _collection.InsertOne(entity);
        }

        public void Update(TEntity entity)
        {
            // var collection = GetCollection(entity);

            _collection.ReplaceOne(Builders<TEntity>.Filter.Eq("_id", entity.Id), entity);
        }

        public void Delete(TEntity entity)
        {
            // var collection = GetCollection(entity);

            _collection.DeleteOne(Builders<TEntity>.Filter.Eq("_id", entity.Id));
        }

        public IList<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate, Int32? startIndex = null, Int32? limit = null)
        {
            var findQuery = _collection.Find(predicate).Skip(startIndex).Limit(limit);

            return findQuery.ToList();
        }

        public long Count(Expression<Func<TEntity, bool>> predicate)
        {
            var findQuery = _collection.Find(predicate);

            return findQuery.Count();
        }

        public TEntity GetById(Guid id)
        {
            // var collection = GetCollection(entity);

            return _collection.Find(Builders<TEntity>.Filter.Eq("_id", id)).FirstOrDefault();
        }

        private IMongoCollection<TEntity> GetCollection (TEntity entity)
        {
            if (_collectionGetter != null)
                return _collectionGetter(entity);

            return _collection;
        }
    }
}