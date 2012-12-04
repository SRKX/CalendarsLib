using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalendarsLib;
using CalendarsLib.Daily;
using TimeSeriesLib;

namespace CalendarsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //DailyCalendar cal = new DailyCalendar(DateTime.Today);
            //List<DateTime> dates= cal.ToList(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(2));
            //foreach (var date in dates)
            //{
            //    Console.WriteLine(date);
            //}

            //Console.WriteLine(cal.CurrentDate);

            //Console.WriteLine("Operation finished.");

            //Console.ReadLine();

            //DailyCalendar cal2 = new DailyCalendar(DateTime.Parse("01.10.2012"), DateTime.Parse("01.10.2012"), DateTime.Parse("31.10.2012"));
            //foreach (var date in cal2)
            //{
            //    Console.WriteLine(date);
            //}

            //BusinessDaysCalendar cal3 = new BusinessDaysCalendar(DateTime.Parse("01.10.2012"), DateTime.Parse("01.10.2012"), DateTime.Parse("31.10.2012"));
            //foreach (var date in cal3)
            //{
            //    Console.WriteLine(date);
            //}


            DailyCalendar cal2 = new DailyCalendar(DateTime.Parse("01.10.2012"), DateTime.Parse("01.10.2012"), DateTime.Parse("31.10.2012"));
            CalendarDoubleTimeSeries ts = new CalendarDoubleTimeSeries(cal2);
            ts["02.10.2012"] = 1;
            ts["05.10.2012"] = 2;
            printTS(ts);
            ts.Remove("05.10.2012");
            printTS(ts);
            ts["05.10.2012"] = 2;
            ts["10.10.2012"] = 3;
            printTS(ts);
            printTS(ts.GetDoubleTimeSeries(CalendarDoubleTimeSeries.ExportMode.FULL));
            printTS(ts.GetDoubleTimeSeries(CalendarDoubleTimeSeries.ExportMode.LIMITED));
            Console.ReadLine();
        }

        private static void printTS(IEnumerable<KeyValuePair<DateTime,double?>> ts)
        {
            foreach (var item in ts)
            {
                Console.WriteLine(item.Key.ToShortDateString() + " : " + (item.Value.HasValue ? item.Value.ToString() : "null"));
            }
            Console.WriteLine();
        }
    }
}
