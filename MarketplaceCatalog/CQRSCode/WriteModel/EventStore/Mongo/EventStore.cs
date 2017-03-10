using System;
using System.Collections.Generic;
using System.Linq;
using CQRSlite.Events;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using CQRSCode.ReadModel.Events;
using System.Reflection;

namespace CQRSCode.WriteModel.EventStore.Mongo
{
    public class EventStore : IEventStore
    {
        private readonly IEventPublisher _publisher;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<IEvent> _collection;
        

        public EventStore(IEventPublisher publisher, String connectionString, String databaseName, IList<Type> events)
        {
            _publisher = publisher;

            if (!BsonClassMap.IsClassMapRegistered(@events[0]))
            {
                // Insuffisant car quand les entités sont enregistrées en tant que IEvent, seuls les champs de l'interface sont persistés
                // BsonClassMap.RegisterClassMap<ProductCreated>(cm => { cm.AutoMap(); cm.unmap } );
                // BsonClassMap.RegisterClassMap<OfferCreated>();
                // BsonClassMap.RegisterClassMap<OfferStockSet>();

                // BsonClassMap.RegisterClassMap<EventBase>(cm => {
                //     cm.MapIdProperty(eb => eb._id).SetIdGenerator(MongoDB.Bson.Serialization.IdGenerators.BsonObjectIdGenerator.Instance);
                // });

                // Impossible d'utiliser automap et de définir un autre champ ID que .Id
                // BsonClassMap.RegisterClassMap<OfferCreated>(cm => 
                // {
                //     cm.MapProperty(o => o.Id); // Nécessaire de mapper en tant que simple propriété afin que ce champ ne soit pas défini comme l'ID du document (_id) dans Mongo
                //     cm.AutoMap();
                //     //cm.GetMemberMapForElement("_id").SetIdGenerator(MongoDB.Bson.Serialization.IdGenerators.BsonObjectIdGenerator.Instance);
                // });

                foreach (var @event in events)
                {
                    BsonClassMap m = new BsonClassMap(@event);
                    @event.GetProperties().ToList().ForEach(p => m.MapProperty(p.Name));
                    BsonClassMap.RegisterClassMap(m);
                }
            }

            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
            _collection = _database.GetCollection<IEvent>("events");
        }

        public void Save<T>(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                _collection.InsertOne(@event);
                _publisher.Publish(@event);
            }
        }

        // Don't forget to create the index according to this query
        public IEnumerable<IEvent> Get<T>(Guid aggregateId, Int32 fromVersion)
        {
            var builder = Builders<IEvent>.Filter;
            var filter = builder.Eq("Id", aggregateId) & builder.Gt("Version", fromVersion);

            var list = _collection
                        .Find(filter)
                        .ToList();

            return list;
        }
    }
}
