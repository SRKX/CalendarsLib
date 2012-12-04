using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalendarsLib.Daily
{
    class BusinessDaysDatesEnumerator:DatesEnumerator
    {
        
        public BusinessDaysDatesEnumerator()
            :base(GetNextBusinessDay(DateTime.Today))
        {

        }

        public BusinessDaysDatesEnumerator(DateTime minValue, DateTime maxValue)
            : base(minValue, maxValue)
        { }

        public BusinessDaysDatesEnumerator(DateTime startingDate, DateTime minValue, DateTime maxValue)
            :base(startingDate.Date,minValue.Date,maxValue.Date)
        {
        }

        protected override DateTime? _InternalMoveNext()
        {
            return GetNextBusinessDay(Current.AddDays(1));
        }

        protected override DateTime? _InternalMovePrevious()
        {
            return GetPreviousBusinessDay(Current.AddDays(-1));
        }

        public override DatesEnumerator Clone()
        {
            return new DailyDatesEnumerator(Current, MinValue, MaxValue);
        }

        private static DateTime GetNextBusinessDay(DateTime t)
        {
            DateTime res = t;
            while (!IsBusinessDay(res))
                res = res.AddDays(1);
            
            return res;
        }

        private static DateTime GetPreviousBusinessDay(DateTime t)
        {
            DateTime res = t;
            while (!IsBusinessDay(res))
                res = res.AddDays(-1);

            return res;
        }

        private static bool IsBusinessDay(DateTime t)
        {
            return t.DayOfWeek != DayOfWeek.Sunday && t.DayOfWeek != DayOfWeek.Saturday;
        }

        public override bool IsPossible(DateTime t)
        {
            return base.IsPossible(t) && IsBusinessDay(t);
        }
    }
}
