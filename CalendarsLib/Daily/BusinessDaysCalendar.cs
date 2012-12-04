using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalendarsLib.Daily
{
    public class BusinessDaysCalendar:Calendar
    {
        public BusinessDaysCalendar()
            :base(new BusinessDaysDatesEnumerator())
        {}

        public BusinessDaysCalendar(DateTime startingTime)
            : base(new BusinessDaysDatesEnumerator(startingTime, DateTime.MinValue, DateTime.MaxValue))
        {}

        public BusinessDaysCalendar(DateTime startingTime, DateTime minValue, DateTime maxValue)
            : base(new BusinessDaysDatesEnumerator(startingTime, minValue, maxValue))
        {}
        public override IEnumerator<DateTime> GetEnumerator(DateTime startingDate, DateTime minValue, DateTime maxValue)
        {
            return new BusinessDaysDatesEnumerator(startingDate, minValue, maxValue);
        }

        public override string Name
        {
            get { return "Business Days Calendar"; }
        }

        public override Calendar Clone(DateTime currentDate, DateTime minValue, DateTime maxValue)
        {
            return new BusinessDaysCalendar(currentDate, minValue, maxValue);
        }
    }
}
