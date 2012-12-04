using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalendarsLib.Daily
{
    class DailyDatesEnumerator:DatesEnumerator
    {

        public DailyDatesEnumerator()
            :base(DateTime.Today)
        {

        }

        public DailyDatesEnumerator(DateTime minValue, DateTime maxValue)
            : base(minValue, maxValue)
        { }

        public DailyDatesEnumerator(DateTime startingDate, DateTime minValue, DateTime maxValue)
            :base(startingDate.Date,minValue.Date,maxValue.Date)
        {
        }

        protected override DateTime? _InternalMoveNext()
        {
            return Current.AddDays(1);
        }

        protected override DateTime? _InternalMovePrevious()
        {
            return Current.AddDays(-1);
        }

        public override DatesEnumerator Clone()
        {
            return new DailyDatesEnumerator(Current, MinValue, MaxValue);
        }
    }
}
