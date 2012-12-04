using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalendarsLib
{
    /// <summary>
    /// <c>Calendar</c> is an abastract representation of a continous collection of dates (represented as <see cref="DateTime"/>).
    /// It allows user to move through the calendar either day by day or to request a period of the current calendar to which the user can iterate.
    /// The <c>Calendar</c> is setup with a <see cref="DatesEnumerator"/> which will determine how the calendar evolves through time.
    /// </summary>
    /// <remarks>
    /// <c>Calendar</c> is a lazy structure; dates are not computed unless they are specifically requested.
    /// </remarks>
    public abstract class Calendar:IEnumerable<DateTime>
    {

        private DatesEnumerator _currentEnumerator;


        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// The furthest date in the past where the calendar can go.
        /// </summary>
        public DateTime MinValue
        {
            get { return _currentEnumerator.MinValue; }
        }

        /// <summary>
        /// The furthest date in the future where the calendar can go.
        /// </summary>
        public DateTime MaxValue
        {
            get { return _currentEnumerator.MaxValue; }
        }


        /// <summary>
        /// The current date of the calendar.
        /// This is the date from which the calendar will move forwards or backwards.
        /// </summary>
        public DateTime CurrentDate
        {
            get { return _currentEnumerator.Current; }
        }

        public Calendar(DatesEnumerator enumerator)
        {
            _currentEnumerator = enumerator;
        }

        //public void ReConfigure(DateTime startingDate)
        //{
        //    ReConfigure(startingDate, DateTime.MinValue, DateTime.MaxValue);
        //}

        //public void ReConfigure(DateTime startingDate, DateTime minValue, DateTime maxValue)
        //{
        //    MinValue = minValue;
        //    MaxValue = maxValue;
        //}

        public IEnumerator<DateTime> GetEnumerator()
        {
            return ToList(MinValue, MaxValue).GetEnumerator();
        }

        public abstract IEnumerator<DateTime> GetEnumerator(DateTime startingDate, DateTime minValue, DateTime maxValue);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ToList(MinValue, MaxValue).GetEnumerator();
        }

        public List<DateTime> ToList(DateTime from, DateTime to)
        {
            //Creates an enumerator with the right boundaries starting from minimal point.
            IEnumerator<DateTime> enumerator = GetEnumerator(from, from, to);
            //Creates the resulting list, with the initial date.
            List<DateTime> result = new List<DateTime>() {enumerator.Current};
            //Fills the list.
            while (enumerator.MoveNext())
            {
                result.Add(enumerator.Current);
            }
            return result;
        }

        public DateTime? Next()
        {
            bool iterNext = _currentEnumerator.MoveNext();
            return  (iterNext) ? (DateTime?) _currentEnumerator.Current :null;
        }

        public DateTime? Previous()
        {
            bool iterPrevious = _currentEnumerator.MovePrevious();
            return (iterPrevious) ? (DateTime?)_currentEnumerator.Current : null;
        }


        /// <summary>
        /// Moves the calendar to the provided date.
        /// </summary>
        /// <param name="t">The date to which the calendar should go.</param>
        /// <returns></returns>
        public void GoTo(DateTime t)
        {
            if (t > CurrentDate)
            {
                while (t > CurrentDate)
                {
                    DateTime? nextPossible = Next();
                    if (!nextPossible.HasValue) throw new ArgumentOutOfRangeException("t");
                }
            }
            else if (t < CurrentDate)
            {
                while (t < CurrentDate)
                {
                    DateTime? previousPossible = Previous();
                    if (!previousPossible.HasValue) throw new ArgumentOutOfRangeException("t");
                }
            }
            else if (t != CurrentDate) throw new ArgumentOutOfRangeException("t");
        }

        public virtual bool IsPossible(DateTime t)
        {
            return _currentEnumerator.IsPossible(t);
        }

        public Calendar GetSubCalendar(DateTime start, DateTime end)
        {
            //First, we perform some checks on the feasibility of the operation
            if (start > end)
                throw new ArgumentException("Start cannot be after end", "start");
            else if (!IsPossible(start))
                throw new ArgumentOutOfRangeException("start");
            else if (!IsPossible(end))
                throw new ArgumentOutOfRangeException("end");

            return Clone(start, end);
        }

        public Calendar Clone()
        {
            return Clone(CurrentDate, MinValue, MaxValue);
        }

        public Calendar Clone(DateTime minValue, DateTime maxValue)
        {
            return Clone(minValue, minValue, maxValue);
        }

        public abstract Calendar Clone(DateTime currentDate, DateTime minValue, DateTime maxValue);

        public override string ToString()
        {
            return Name + "(" + MinValue.ToShortDateString() + " ... " + MaxValue.ToShortDateString() + ")";
        }
    }
}
