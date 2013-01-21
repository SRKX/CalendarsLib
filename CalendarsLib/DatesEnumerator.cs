using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalendarsLib
{
    /// <summary>
    /// Represents and enumerator of <see cref="System.DateTime"/> which allows to navigate through time.
    /// </summary>
    public abstract class DatesEnumerator:IEnumerator<DateTime>
    {
        /// <summary>
        /// The lower bound of the enumerator. It will never return a date before that.
        /// </summary>
        public DateTime MinValue { get; private set; }

        /// <summary>
        /// The upper bound of the enumerator. It will never return a date after that.
        /// </summary>
        public DateTime MaxValue { get; private set; }


        /// <summary>
        /// Initializes the enumerator with the cursor set to the actual date and time and the bounds set to the minimum and maximum date from <see cref="System.DateTime"/>.
        /// </summary>
        public DatesEnumerator()
            :this(DateTime.Now)
        { }

        /// <summary>
        /// Initializes the enumerator with the cursor set to the provided <paramref name="initialDate"/> and the bounds set to the minimum and maximum date from <see cref="System.DateTime"/>.
        /// </summary>
        /// <param name="initialDate">The date at which the cursor will be initialized at.</param>
        public DatesEnumerator(DateTime initialDate)
            :this(initialDate,DateTime.MinValue,DateTime.MaxValue)
        { }

        /// <summary>
        /// Initializes the enumerator with a lower bound <paramref name="minValue"/>, a higher bound <paramref name="maxValue"/> and the cursor set to the lower bound.
        /// </summary>
        /// <param name="minValue">The lower bound of the enumerator.</param>
        /// <param name="maxValue">The upper bound of the enumerator.</param>
        public DatesEnumerator(DateTime minValue, DateTime maxValue)
            :this(minValue,minValue,maxValue)
        { }

        /// <summary>
        /// Initializes the enumerator with a lower bound <paramref name="minValue"/>, a higher bound <paramref name="maxValue"/> and the cursor set to <paramref name="initialDate"/>.
        /// </summary>
        /// <param name="initialDate">The date at which the cursor is initially set to.</param>
        /// <param name="minValue">The lower bound of the enumerator.</param>
        /// <param name="maxValue">The upper bound of the enumerator.</param>
        public DatesEnumerator(DateTime initialDate, DateTime minValue, DateTime maxValue)
        {
            if (minValue >= maxValue)
                throw new ArgumentException("minValue", "MinValue cannot be later than MaxValue");
            MinValue = minValue;
            MaxValue = maxValue;
            if (IsPossible(initialDate))
                Current = initialDate;
            else
                throw new ArgumentOutOfRangeException("startingDate","Starting date detected as impossible by the enumerator.");
        }

        /// <summary>
        /// The date at which the cursor of the enumerator points to.
        /// </summary>
        public DateTime Current
        {
            get;
            protected set;
        }

        /// <summary>
        /// Disposes the enumerator.
        /// </summary>
        public void Dispose()
        {
            
        }

        /// <summary>
        /// The date at which the cursor of the enumerator points to.
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }


        /// <summary>
        /// Moves to the next date available in the calendar.
        /// </summary>
        /// <returns><c>True</c> if there is a next date available to the enumerator.</returns>
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

        /// <summary>
        /// Moves to the previous date in the enumerator.
        /// </summary>
        /// <returns><c>True</c> if there is a previous date available to the enumerator.</returns>
        public bool MovePrevious()
        {
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

        /// <summary>
        /// Internal logic for determining the next date of the enumerator.
        /// </summary>
        /// <returns>The next date of the enumerator.</returns>
        /// <remarks>If the enumerator cannot return a value, it should return <c>null</c>.</remarks>
        protected abstract DateTime? _InternalMoveNext();

        /// <summary>
        /// Internal logic for determining the previous date of the enumerator.
        /// </summary>
        /// <returns>The previous date of the enumerator.</returns>
        /// <remarks>If the enumerator cannot return a value, it should return <c>null</c>.</remarks>
        protected abstract DateTime? _InternalMovePrevious();

        /// <summary>
        /// Place the cursor to the minimum date.
        /// </summary>
        public virtual void Reset()
        {
            Current = MinValue;
        }

        /// <summary>
        /// Determines whether the provided <paramref name="date"/> can possibly be in the enumerator.
        /// </summary>
        /// <param name="date">The date to be tested.</param>
        /// <returns><c>True</c> if the <paramref name="date"/> can be in the enumerator.</returns>
        public virtual bool IsPossible(DateTime date)
        {
            return date >= MinValue && date <= MaxValue;
        }

        /// <summary>
        /// Clones the current enumerator.
        /// </summary>
        /// <returns>A clone of the enumerator.</returns>
        public abstract DatesEnumerator Clone();
    }
}
