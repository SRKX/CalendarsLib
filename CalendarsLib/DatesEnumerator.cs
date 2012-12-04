using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalendarsLib
{
    public abstract class DatesEnumerator:IEnumerator<DateTime>
    {
        public DateTime MinValue { get; private set; }

        public DateTime MaxValue { get; private set; }

        public DatesEnumerator()
            :this(DateTime.Now)
        {

        }

        public DatesEnumerator(DateTime startingDate)
            :this(startingDate,DateTime.MinValue,DateTime.MaxValue)
        { }

        public DatesEnumerator(DateTime minValue, DateTime maxValue)
            :this(minValue,minValue,maxValue)
        { }

        public DatesEnumerator(DateTime startingDate, DateTime minValue, DateTime maxValue)
        {
            if (minValue >= maxValue)
                throw new ArgumentException("MinValue cannot be later than MaxValue","minValue");
            MinValue = minValue;
            MaxValue = maxValue;
            if (IsPossible(startingDate))
                Current = startingDate;
            else
                throw new ArgumentOutOfRangeException("startingDate");
        }

        public DateTime Current
        {
            get;
            protected set;
        }

        public void Dispose()
        {
            
        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            DateTime? nextDate = _InternalMoveNext();
            bool moveNextResult;
            //Checks that the date is within the bounds.
            if (nextDate.HasValue && nextDate.Value > MaxValue)
            {
                moveNextResult = false;
            }
            else if (nextDate.HasValue)
            {
                Current = nextDate.Value;
                moveNextResult = true;
            }
            else
            {
                moveNextResult = false;
            }
            return moveNextResult;
        }

        public bool MovePrevious()
        {
            //DateTime oldCurrent = Current;
            DateTime? previousDate =_InternalMovePrevious();
            bool movePreviousResult;
            //Checks that the date is within the bounds.
            if (previousDate.HasValue && previousDate.Value < MinValue)
            {
                movePreviousResult = false;
            }
            else if (previousDate.HasValue)
            {
                Current = previousDate.Value;
                movePreviousResult = true;
            }
            else
            {
                movePreviousResult = false;
            }
            return movePreviousResult;
        }


        protected abstract DateTime? _InternalMoveNext();
        protected abstract DateTime? _InternalMovePrevious();

        public virtual void Reset()
        {
            Current = MinValue;
        }

        public virtual bool IsPossible(DateTime t)
        {
            return t >= MinValue && t <= MaxValue;
        }


        public abstract DatesEnumerator Clone();
    }
}
