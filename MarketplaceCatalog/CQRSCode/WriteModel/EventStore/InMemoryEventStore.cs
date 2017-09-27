using System;
using System.Collections.Generic;
using System.Linq;
using CQRSlite.Events;
using System.Threading.Tasks;
using System.Threading;

namespace CQRSCode.WriteModel.EventStore
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly IEventPublisher _publisher;
        private readonly Dictionary<Guid, List<IEvent>> _inMemoryDb = new Dictionary<Guid, List<IEvent>>();

        public InMemoryEventStore(IEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var @event in events)
            {
                List<IEvent> list;
                _inMemoryDb.TryGetValue(@event.Id, out list);

                if (list == null)
                {
                    list = new List<IEvent>();
                    _inMemoryDb.Add(@event.Id, list);
                }

                list.Add(@event);
                await _publisher.Publish(@event);
            }
        }

        public async Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default(CancellationToken))
        {
            List<IEvent> events;
            _inMemoryDb.TryGetValue(aggregateId, out events);
            return events?.Where(x => x.Version > fromVersion) ?? new List<IEvent>();
        }
    }
}
