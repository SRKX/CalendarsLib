using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalendarsLib;

namespace TimeSeriesLib
{
    public class CalendarDoubleTimeSeries:IEnumerable<KeyValuePair<DateTime,double?>>
    {
        private readonly DoubleTimeSeries _tSeries =new DoubleTimeSeries();

        private readonly Calendar _calendar;

        #region Constructors
        public CalendarDoubleTimeSeries(Calendar calendar)
        {
            if (calendar == null)
                throw new ArgumentNullException("calendar", "Cannot assign a null Calendar to a CalendarTimeSeries");
            _calendar = calendar;
        }
        #endregion

        #region Indexers
        public double? this[string date]
        {
            get { return this[DateTime.Parse(date)]; }
            set { this[DateTime.Parse(date)]=value; }
        }

        public double? this[DateTime date]
        {
            get { return Get(date); }
            set { AddOrUpdate(date, value); }
        }
        #endregion

        #region Insertion/Removal Handling
        public void AddOrUpdate(DateTime date, double? value)
        {
            if (_calendar.Contains(date))
            {
                if (_tSeries.Count > 0 && !_tSeries.ContainsKey(date) && !(_tSeries.Keys.Any(k=>k<date) && _tSeries.Keys.Any(k=>k>date)))
                {
                    DateTime minDate = _tSeries.Keys.Min();
                    DateTime maxDate = _tSeries.Keys.Max();
                    Calendar subCalendar;
                    if (date < minDate)
                        subCalendar = _calendar.GetSubCalendar(date, minDate);
                    else
                        subCalendar = _calendar.GetSubCalendar(maxDate, date);

                    foreach (var d in subCalendar)
                    {
                        if (d!=minDate && d!=maxDate)
                            _tSeries[d] = null;
                    }
                }
                _tSeries[date] = value;
                
            }
            else
                throw CalendarException.Create(_calendar, date);
        }

        private void _addOrUpdate(KeyValuePair<DateTime, double?> pair)
        {
            AddOrUpdate(pair.Key, pair.Value);
        }

        public void AddOrUpdate(IEnumerable<KeyValuePair<DateTime, double?>> pairs)
        {
            foreach (var pair in pairs)
            {
                _addOrUpdate(pair);
            }
        }

        public double? Get(DateTime date)
        {
            if (_calendar.Contains(date))
                return _tSeries[date];
            else
                throw CalendarException.Create(_calendar, date);
        }

        public void Remove(string date)
        {
            Remove(DateTime.Parse(date));
        }

        public void Remove(DateTime date)
        {
            if (_calendar.Contains(date))
            {
                if (!_tSeries.ContainsKey(date))
                    throw new ArgumentException(date.ToShortDateString() + " is not in the time series");
                else
                {
                    if (_tSeries.Count==1)
                        _tSeries.Remove(date);
                    else
                    {
                        _tSeries[date] = null;
                        DateTime minDate = _tSeries.First(p => p.Value.HasValue).Key;
                        DateTime maxDate = _tSeries.Last(p => p.Value.HasValue).Key;
                        IEnumerable<DateTime> toRemove = _tSeries.Keys.Where(k => k < minDate || k > maxDate).ToList();
                        foreach (var d in toRemove)
                        {
                            _tSeries.Remove(d);
                        }
                    }
                }
                
            }
            else
                throw CalendarException.Create(_calendar, date);
        }
        #endregion

        #region Utils
        public enum ExportMode
        {
            FULL,
            LIMITED
        }


        public DoubleTimeSeries GetDoubleTimeSeries(ExportMode mode)
        {
            switch (mode)
            {
                case ExportMode.FULL:
                    return GetDoubleTimeSeries(_calendar.MinValue, _calendar.MaxValue);
                case ExportMode.LIMITED:
                    return _tSeries.Clone();
                default:
                    throw new ArgumentException("Unknown export mode", "mode");
            }
        }

        public DoubleTimeSeries GetDoubleTimeSeries(DateTime start, DateTime end)
        {
            if (start > end)
                throw new ArgumentException("Starting date cannot be before ending date", "start");
            else if (start < _calendar.MinValue)
                throw CalendarException.Create(_calendar, start);
            else if (end > _calendar.MaxValue)
                throw CalendarException.Create(_calendar, end);
            else
            {
                Calendar subCalendar = _calendar.GetSubCalendar(start, end);
                DoubleTimeSeries res = new DoubleTimeSeries();
                foreach (var date in subCalendar)
                {
                    res[date] =_tSeries.ContainsKey(date) ? _tSeries[date] : null;
                }
                return res;
            }
        }

        public List<Hole> FindHoles(ExportMode mode=ExportMode.LIMITED)
        {
            List<Hole> holes = new List<Hole>();

            DoubleTimeSeries ts = GetDoubleTimeSeries(mode);

            DateTime? holeStart = null;
            DateTime? holeEnd = null;
            foreach (var pair in ts)
            {
                if (pair.Value.HasValue && holeStart.HasValue)
                {
                    holes.Add(new Hole(holeStart.Value, holeEnd.Value));
                    holeStart = null;
                    holeEnd = null;
                }
                if (!pair.Value.HasValue)
                {
                    if (!holeStart.HasValue)
                    {
                        holeStart = pair.Key;
                    }
                    holeEnd = pair.Key;
                }
            }

            if (holeStart.HasValue)
                holes.Add(new Hole(holeStart.Value, holeEnd.Value));

            return holes;

        }
        #endregion


        #region IEnumerable Implementation
        public IEnumerator<KeyValuePair<DateTime, double?>> GetEnumerator()
        {
            return _tSeries.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _tSeries.GetEnumerator();
        }
        #endregion

    }
}
