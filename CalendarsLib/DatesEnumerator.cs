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
        {

        }

        public DatesEnumerator(DateTime startingDate, DateTime minValue, DateTime maxValue)
        {
            Current = startingDate;
            if (minValue <= maxValue)
                throw new ArgumentException("MinValue cannot be later than MaxValue");
            MinValue = minValue;
            MaxValue = maxValue;
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
            DateTime oldCurrent = Current;
            bool moveNextResult = _InternalMoveNext();
            //Checks that the date is within the bounds.
            if (Current > MaxValue)
            {
                Current = oldCurrent;
                moveNextResult = false;
            }
            return moveNextResult;
        }

        public bool MovePrevious()
        {
            DateTime oldCurrent = Current;
            bool movePreviousResult = _InternalMovePrevious();
            //Checks that the date is within the bounds.
            if (Current < MinValue)
            {
                Current = oldCurrent;
                movePreviousResult = false;
            }
            return movePreviousResult;
        }

        protected abstract bool _InternalMoveNext();
        protected abstract bool _InternalMovePrevious();

        public virtual void Reset()
        {
            Current = MinValue;
        }
    }
}
