using System;
using System.Collections.Generic;
using System.Linq;
using CQRSlite.Events;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using CQRSCode.ReadModel.Events;

namespace CQRSCode.WriteModel.EventStore.Mongo
{
    public class EventStore : IEventStore
    {
        private readonly IEventPublisher _publisher;
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<IEvent> _collection;
        

        public EventStore(IEventPublisher publisher, String connectionString, String databaseName)
        {
            _publisher = publisher;

            if (!BsonClassMap.IsClassMapRegistered(typeof(OfferCreated)))
            {
                // Insuffisant car quand les entités sont enregistrées en tant que IEvent, seuls les champs de l'interface sont persistés
                // BsonClassMap.RegisterClassMap<ProductCreated>(cm => { cm.AutoMap(); cm.unmap } );
                // BsonClassMap.RegisterClassMap<OfferCreated>();
                // BsonClassMap.RegisterClassMap<OfferStockSet>();

                #region Product

                BsonClassMap.RegisterClassMap<OfferCreated>(cm => 
                {
                    cm.MapCreator(oc => new OfferCreated(oc.Id, oc.OfferId, oc.MerchantId, oc.Stock, oc.Price, oc.MerchantActivated, oc.SKU, oc.MerchantName, oc.IsActivated, oc.IsVisible) { TimeStamp = oc.TimeStamp, Version = oc.Version });
                    cm.MapProperty(o => o.MerchantActivated);
                    cm.MapProperty(o => o.MerchantId);
                    cm.MapProperty(o => o.OfferId);
                    cm.MapProperty(o => o.SKU);
                    cm.MapProperty(o => o.Price);
                    cm.MapProperty(o => o.Stock);
                    cm.MapProperty(o => o.TimeStamp);
                    cm.MapProperty(o => o.Version);
                    cm.MapProperty(o => o.Id); // Nécessaire de mapper en tant que simple propriété afin que ce champ ne soit pas défini comme l'ID du document (_id) dans Mongo
                });

                BsonClassMap.RegisterClassMap<ProductCreated>(cm => 
                {
                    cm.MapCreator(pc => new ProductCreated(pc.Id, pc.Name, pc.Description, pc.IsActivated, pc.IsVisible, pc.EAN, pc.UPC) { TimeStamp = pc.TimeStamp, Version = pc.Version });
                    cm.MapProperty(pc => pc.Id);
                    cm.MapProperty(pc => pc.Name);
                    cm.MapProperty(pc => pc.Description);
                    cm.MapProperty(pc => pc.TimeStamp);
                    cm.MapProperty(pc => pc.Version);
                    cm.MapProperty(pc => pc.EAN);
                    cm.MapProperty(pc => pc.UPC);
                });

                BsonClassMap.RegisterClassMap<OfferStockSet>(cm => 
                {
                    cm.MapCreator(oss => new OfferStockSet(oss.Id, oss.OfferId, oss.Stock) { TimeStamp = oss.TimeStamp, Version = oss.Version });
                    cm.MapProperty(oss => oss.Id);
                    cm.MapProperty(oss => oss.OfferId);
                    cm.MapProperty(oss => oss.Stock);
                    cm.MapProperty(oss => oss.Version);
                    cm.MapProperty(oss => oss.TimeStamp);
                });

                BsonClassMap.RegisterClassMap<OfferMerchantActivated>(cm => 
                {
                    cm.MapCreator(oma => new OfferMerchantActivated(oma.Id, oma.OfferId, oma.MerchantId) { TimeStamp = oma.TimeStamp, Version = oma.Version });
                    cm.MapProperty(oma => oma.Id);
                    cm.MapProperty(oma => oma.OfferId);
                    cm.MapProperty(oma => oma.MerchantId);
                    cm.MapProperty(oma => oma.Version);
                    cm.MapProperty(oma => oma.TimeStamp);
                });

                BsonClassMap.RegisterClassMap<OfferMerchantDeactivated>(cm => 
                {
                    cm.MapCreator(omd => new OfferMerchantDeactivated(omd.Id, omd.OfferId, omd.MerchantId) { TimeStamp = omd.TimeStamp, Version = omd.Version });
                    cm.MapProperty(omd => omd.Id);
                    cm.MapProperty(omd => omd.OfferId);
                    cm.MapProperty(omd => omd.MerchantId);
                    cm.MapProperty(omd => omd.Version);
                    cm.MapProperty(omd => omd.TimeStamp);
                });

                // BsonClassMap.RegisterClassMap<ProductCategoryDefined>(cm => 
                // {
                //     cm.MapCreator(pcd => new ProductCategoryDefined(pcd.Id, pcd.CategoryId) { TimeStamp = pcd.TimeStamp, Version = pcd.Version });
                //     cm.MapProperty(pcd => pcd.Id);
                //     cm.MapProperty(pcd => pcd.CategoryId);
                //     cm.MapProperty(pcd => pcd.Version);
                //     cm.MapProperty(pcd => pcd.TimeStamp);
                // });

                #endregion


                #region Category

                BsonClassMap.RegisterClassMap<CategoryCreated>(cm => 
                {
                    cm.MapCreator(cc => new CategoryCreated(cc.Id, cc.Name, cc.ParentId) { TimeStamp = cc.TimeStamp, Version = cc.Version });
                    cm.MapProperty(cc => cc.Name);
                    cm.MapProperty(cc => cc.ParentId);
                    cm.MapProperty(cc => cc.TimeStamp);
                    cm.MapProperty(cc => cc.Version);
                    cm.MapProperty(cc => cc.Id); // Nécessaire de mapper en tant que simple propriété afin que ce champ ne soit pas défini comme l'ID du document (_id) dans Mongo
                });

                BsonClassMap.RegisterClassMap<CategoryActivated>(cm => 
                {
                    cm.MapCreator(cc => new CategoryActivated(cc.Id) { TimeStamp = cc.TimeStamp, Version = cc.Version });
                    cm.MapProperty(cc => cc.TimeStamp);
                    cm.MapProperty(cc => cc.Version);
                    cm.MapProperty(cc => cc.Id); // Nécessaire de mapper en tant que simple propriété afin que ce champ ne soit pas défini comme l'ID du document (_id) dans Mongo
                });

                BsonClassMap.RegisterClassMap<CategoryDeactivated>(cm => 
                {
                    cm.MapCreator(cc => new CategoryDeactivated(cc.Id) { TimeStamp = cc.TimeStamp, Version = cc.Version });
                    cm.MapProperty(cc => cc.TimeStamp);
                    cm.MapProperty(cc => cc.Version);
                    cm.MapProperty(cc => cc.Id); // Nécessaire de mapper en tant que simple propriété afin que ce champ ne soit pas défini comme l'ID du document (_id) dans Mongo
                });

                #endregion


                #region Merchant

                BsonClassMap.RegisterClassMap<MerchantCreated>(cm => 
                {
                    cm.MapCreator(mc => new MerchantCreated(mc.Id, mc.Name, mc.Email) { TimeStamp = mc.TimeStamp, Version = mc.Version });
                    cm.MapProperty(cc => cc.Name);
                    cm.MapProperty(cc => cc.Email);
                    cm.MapProperty(cc => cc.TimeStamp);
                    cm.MapProperty(cc => cc.Version);
                    cm.MapProperty(cc => cc.Id); // Nécessaire de mapper en tant que simple propriété afin que ce champ ne soit pas défini comme l'ID du document (_id) dans Mongo
                });

                BsonClassMap.RegisterClassMap<MerchantActivated>(cm => 
                {
                    cm.MapCreator(ma => new MerchantActivated(ma.Id) { TimeStamp = ma.TimeStamp, Version = ma.Version });
                    cm.MapProperty(ma => ma.TimeStamp);
                    cm.MapProperty(ma => ma.Version);
                    cm.MapProperty(ma => ma.Id); // Nécessaire de mapper en tant que simple propriété afin que ce champ ne soit pas défini comme l'ID du document (_id) dans Mongo
                });

                BsonClassMap.RegisterClassMap<MerchantDeactivated>(cm => 
                {
                    cm.MapCreator(ma => new MerchantDeactivated(ma.Id) { TimeStamp = ma.TimeStamp, Version = ma.Version });
                    cm.MapProperty(ma => ma.TimeStamp);
                    cm.MapProperty(ma => ma.Version);
                    cm.MapProperty(ma => ma.Id); // Nécessaire de mapper en tant que simple propriété afin que ce champ ne soit pas défini comme l'ID du document (_id) dans Mongo
                });

                #endregion
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
