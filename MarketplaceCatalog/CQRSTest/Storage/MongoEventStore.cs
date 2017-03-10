
using CQRSCode.ReadModel.Events;
using CQRSCode.WriteModel.Domain;
using CQRSlite.Domain;
using CQRSlite.Events;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CQRSTests.Storage
{
  public class MongoEventStore
  {
    private CQRSCode.WriteModel.EventStore.Mongo.EventStore _eStore = (CQRSCode.WriteModel.EventStore.Mongo.EventStore) null;
    private Mock<IEventPublisher> _publisherMock;

    public MongoEventStore()
    {
      this._publisherMock = new Mock<IEventPublisher>();
      this._eStore = new CQRSCode.WriteModel.EventStore.Mongo.EventStore(this._publisherMock.Object, "mongodb://localhost:27017", "marketplacecatalog", new List<Type>() { typeof(OfferCreated), typeof(ProductCreated), typeof(OfferStockSet) });
    }

    [Fact]
    public void InsertEvents()
    {
      List<IEvent> ieventList = new List<IEvent>();
      OfferCreated offerCreated = new OfferCreated(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), (short) 1, Decimal.One, true, "mysku", "mymerchant", true, true);
      offerCreated.TimeStamp = DateTimeOffset.UtcNow;

      offerCreated.Version = 1;
      ieventList.Add((IEvent) offerCreated);
      this._eStore.Save<Offer>((IEnumerable<IEvent>) ieventList);
    }

    [Fact]
    public void ReadEventsByAggregate()
    {
      Guid guid1 = Guid.NewGuid();
      Guid guid2 = Guid.NewGuid();
      List<IEvent> ieventList = new List<IEvent>();
      
      ProductCreated productCreated = new ProductCreated(guid1, "name", "description", true, true);
      DateTimeOffset utcNow1 = DateTimeOffset.UtcNow;
      productCreated.TimeStamp = utcNow1;
      int num1 = 1;
      productCreated.Version = num1;
      ieventList.Add((IEvent) productCreated);
      
      
      OfferCreated offerCreated = new OfferCreated(guid1, guid2, Guid.NewGuid(), (short) 1, Decimal.One, true, "mysku2", "mymerchant", true, true);
      DateTimeOffset utcNow2 = DateTimeOffset.UtcNow;
      offerCreated.TimeStamp = utcNow2;
      int num2 = 2;
      offerCreated.Version = num2;
      ieventList.Add((IEvent) offerCreated);

      OfferStockSet offerStockSet = new OfferStockSet(guid1, guid2, (short) 1);
      DateTimeOffset utcNow3 = DateTimeOffset.UtcNow;
      offerStockSet.TimeStamp = utcNow3;
      int num3 = 3;
      offerStockSet.Version = num3;
      ieventList.Add((IEvent) offerStockSet);
      
      this._eStore.Save<Product>((IEnumerable<IEvent>) ieventList);
      IEnumerable<IEvent> source = this._eStore.Get<Product>(guid1, -1);
      
      Assert.NotNull((object) source);
      Assert.Equal(3, source.Count<IEvent>());
    }

    [Fact]
    public void LoadAggregateEventsHistory()
    {
      Guid guid1 = Guid.NewGuid();
      Guid guid2 = Guid.NewGuid();
      List<IEvent> ieventList1 = new List<IEvent>();

      ProductCreated productCreated = new ProductCreated(guid1, "name", "description", true, true);
      DateTimeOffset utcNow1 = DateTimeOffset.UtcNow;
      productCreated.TimeStamp = utcNow1;
      int num1 = 1;
      productCreated.Version = num1;
      ieventList1.Add((IEvent) productCreated);

      OfferCreated offerCreated1 = new OfferCreated(guid1, guid2, Guid.NewGuid(), 1, new Decimal(65, 0, 0, false, 1), true, "mysku", "mymerchant", true, true);
      DateTimeOffset utcNow2 = DateTimeOffset.UtcNow;
      offerCreated1.TimeStamp = utcNow2;
      int num2 = 2;
      offerCreated1.Version = num2;
      ieventList1.Add((IEvent) offerCreated1);
      
      OfferStockSet offerStockSet1 = new OfferStockSet(guid1, guid2, (short) 1);
      DateTimeOffset utcNow3 = DateTimeOffset.UtcNow;
      offerStockSet1.TimeStamp = utcNow3;
      int num3 = 3;
      offerStockSet1.Version = num3;
      ieventList1.Add((IEvent) offerStockSet1);
      
      OfferStockSet offerStockSet2 = new OfferStockSet(guid1, guid2, (short) 7);
      DateTimeOffset utcNow4 = DateTimeOffset.UtcNow;
      offerStockSet2.TimeStamp = utcNow4;
      int num4 = 4;
      offerStockSet2.Version = num4;

      ieventList1.Add((IEvent) offerStockSet2);
      this._eStore.Save<Product>((IEnumerable<IEvent>) ieventList1);
      
      IEnumerable<IEvent> ievents1 = this._eStore.Get<Product>(guid1, -1);
      Product instance1 = (Product) Activator.CreateInstance(typeof (Product), true);
      
      ((AggregateRoot) instance1).LoadFromHistory(ievents1);

      Assert.Equal(1, instance1.Offers.Count);
      Assert.Equal<int>(7, instance1.Offers[0].Stock);
      Assert.Equal(new Decimal(65, 0, 0, false, (byte) 1), instance1.Offers[0].Price);
      Assert.Equal<Guid>(guid2, instance1.Offers[0].Id);
      Assert.Equal<Guid>(guid1, instance1.Offers[0].ProductId);
      
      Guid guid3 = Guid.NewGuid();
      CQRSCode.WriteModel.EventStore.Mongo.EventStore eStore = this._eStore;
      List<IEvent> ieventList2 = new List<IEvent>();
      OfferCreated offerCreated2 = new OfferCreated(guid1, guid3, Guid.NewGuid(), 456, new Decimal(7165, 0, 0, false, 1), true, "mysku2", "mymerchant", true, true);
      DateTimeOffset utcNow5 = DateTimeOffset.UtcNow;
      offerCreated2.TimeStamp = utcNow5;
      int num5 = 5;
      offerCreated2.Version = num5;

      ieventList2.Add((IEvent) offerCreated2);
      eStore.Save<Product>((IEnumerable<IEvent>) ieventList2);
      
      IEnumerable<IEvent> ievents2 = this._eStore.Get<Product>(guid1, -1);
      
      Product instance2 = (Product) Activator.CreateInstance(typeof (Product), true);
      ((AggregateRoot) instance2).LoadFromHistory(ievents2);
      
      Assert.Equal(2, instance2.Offers.Count);
    }
  }
}
