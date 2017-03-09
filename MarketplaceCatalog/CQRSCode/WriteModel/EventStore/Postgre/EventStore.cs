using System;
using System.Collections.Generic;
using System.Linq;
using CQRSlite.Events;
using Newtonsoft.Json;

namespace CQRSCode.WriteModel.EventStore.Postgre
{
    public class EventStore : IEventStore
    {
        private readonly IEventPublisher _publisher;
        private readonly EventsDBContext _eDBContext;

        public EventStore(IEventPublisher publisher, EventsDBContext eDBContext)
        {
            _publisher = publisher;
            _eDBContext = eDBContext;
        }

        public void Save<T>(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                _eDBContext.Events.Add(ToDBModel(@event));
                _publisher.Publish(@event);
            }

            _eDBContext.SaveChanges();
        }

        public IEnumerable<IEvent> Get<T>(Guid aggregateId, int fromVersion)
        {
            return _eDBContext
                    .Events
                    .Where(e => e.SequenceNumber > fromVersion)
                    .Select(e => FromDBModel(e));
        }

        private EventModel ToDBModel(IEvent @event)
        {
            EventModel dbModel = new EventModel
            {
                AggregateId = @event.Id,
                SequenceNumber = @event.Version,
                EventType = @event.GetType().FullName,
                TimeStamp = DateTime.Now,
                PayLoad = JsonConvert.SerializeObject(@event)
            };

            return dbModel;
        }

        private IEvent FromDBModel(EventModel dbEvent)
        {
            IEvent @event = (IEvent)JsonConvert.DeserializeObject(
                dbEvent.PayLoad, Type.GetType(dbEvent.EventType)
                );

            return @event;
        }
    }
}
