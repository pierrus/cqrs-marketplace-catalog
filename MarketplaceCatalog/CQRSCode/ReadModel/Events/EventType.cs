using System;

namespace CQRSCode.ReadModel.Events
{
    public class EventType
    {
        public EventType(Type type)
        {
            this.Type = type;
        }

        public Type Type { get; private set; }
    }
}