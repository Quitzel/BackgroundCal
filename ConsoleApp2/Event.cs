using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp2
{
    class Event
    {
        public Event(String startTime, String endTime, String eventName, int eventDate, int eventMonth)
        {
            String StartTime = startTime;
            String EndTime = endTime;
            String EventName = eventName;
            int EventMonth = eventMonth;
            int EventDate = eventDate;
        }

        public String StartTime { get; }

        public String EndTime { get; }

        public String EventName { get; }

        public int EventMonth { get; }

        public int EventDate { get; }


        public override string ToString()
        {
            StringBuilder rString = new StringBuilder();
            rString.Append(StartTime);
            rString.Append(" - ");
            rString.Append(EndTime);
            rString.Append("\n");
            rString.Append(EventName);
            return rString.ToString();
        }
    }
}
