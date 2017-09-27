using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using CQRSlite.Events;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using CQRSCode.ReadModel.Events;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace CQRSCode.WriteModel.EventStore.Mongo
{
    public class EventStore : IEventStore
    {
        private readonly IEventPublisher _publisher;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<IEvent> _collection;
        

        public EventStore(IEventPublisher publisher, CQRSCode.ReadModel.Repository.MongoOptions mongoOptions, IList<EventType> events)
        {
            _publisher = publisher;

            if (!BsonClassMap.IsClassMapRegistered(@events[0].Type))
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
                    BsonClassMap m = new BsonClassMap(@event.Type);
                    @event.Type.GetProperties().ToList().ForEach(p => m.MapProperty(p.Name));
                    BsonClassMap.RegisterClassMap(m);
                }
            }

            _client = new MongoClient(mongoOptions.ConnectionString);
            _database = _client.GetDatabase(mongoOptions.Database);
            _collection = _database.GetCollection<IEvent>("events");
        }

        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var @event in events)
            {
                _collection.InsertOne(@event);
                await _publisher.Publish(@event);
            }
        }

        // Don't forget to create the index according to this query
        public async Task<IEnumerable<IEvent>> Get(Guid aggregateId, Int32 fromVersion, CancellationToken cancellationToken = default(CancellationToken))
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
