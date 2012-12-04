using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalendarsLib.Daily
{
    public class DailyCalendar:Calendar
    {
        public DailyCalendar()
            :base(new DailyDatesEnumerator())
        {}

        public DailyCalendar(DateTime startingTime)
            :base(new DailyDatesEnumerator(startingTime,DateTime.MinValue, DateTime.MaxValue))
        {}

        public DailyCalendar(DateTime startingTime,DateTime minValue, DateTime maxValue)
            :base(new DailyDatesEnumerator(startingTime,minValue,maxValue))
        {}
        public override IEnumerator<DateTime> GetEnumerator(DateTime startingDate, DateTime minValue, DateTime maxValue)
        {
            return new DailyDatesEnumerator(startingDate,minValue,maxValue);
        }

        public override string Name
        {
            get { return "Daily Calendar"; }
        }

        public override Calendar Clone(DateTime currentDate, DateTime minValue, DateTime maxValue)
        {
            return new DailyCalendar(currentDate, minValue, maxValue);
        }
    }
}
