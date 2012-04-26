using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
    public class Event
    {
        Dictionary<String, String> propmap;
        EventType type;

        public Event()
        {
            propmap = new Dictionary<String, String>();
        }

        public void setEventType(EventType type)
        {
            this.type = type;
        }

        public EventType getEventType()
        {
            return this.type;
        }

        public void addProperty(String name, String value)
        {
            propmap.Add(name, value);
        }

        public String getProperty(String name)
        {
            return propmap[name];
        }

        public String[] getKeys()
        {
            return propmap.Keys.ToArray();
        }
    }
}
