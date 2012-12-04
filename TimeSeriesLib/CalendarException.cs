using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalendarsLib;

namespace TimeSeriesLib
{
    public class CalendarException:Exception
    {
        private CalendarException(Calendar calendar, DateTime date)
            :base(date.ToShortDateString() + " is not in " + calendar.ToString())
        { }

        public static CalendarException Create(Calendar calendar, DateTime date)
        {
            return new CalendarException(calendar, date);
        }
    }
}
