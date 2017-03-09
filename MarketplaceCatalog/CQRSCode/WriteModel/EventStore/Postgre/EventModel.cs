using System;
using CQRSCode.ReadModel.Events;
using CQRSlite.Domain;
using System.Collections.Generic;

namespace CQRSCode.WriteModel.EventStore.Postgre
{
    public class EventModel
    {
        //Primary key for sequential inserts
        public Int32 ID { get; set; }


        //Unique constraint over the next 3 properties
        public Guid AggregateId { get; set; }

        public Int32 SequenceNumber { get; set; }

        //public String AggregateType { get; set; }


        public String PayLoad { get; set; }

        //Later
        //public String EventRevision { get; set; }

        public String EventType { get; set; }

        public DateTime TimeStamp { get; set; }        
    }
}