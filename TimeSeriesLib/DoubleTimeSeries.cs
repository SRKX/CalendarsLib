using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeSeriesLib
{
    public class DoubleTimeSeries:SortedDictionary<DateTime,double?>
    {
        #region Constructors
        public DoubleTimeSeries()
        {

        }

        public DoubleTimeSeries(DoubleTimeSeries ts)
        {
            foreach (var pair in ts)
            {
                this.Add(pair.Key, pair.Value);
            }
        }
        #endregion

        #region Null Statistics
        public bool HasNull
        {
            get { return this.NbrNull == 0; }
        }

        public int NbrNull
        {
            get { return this.Count(p => !p.Value.HasValue); }
        }

        public double NullRatio
        {
            get { return (this.Count == 0) ? 0.0 : Convert.ToDouble(this.NbrNull)/Convert.ToDouble(this.Count); }
        }
        #endregion

        #region Utils
        public DoubleTimeSeries Clone()
        {
            return new DoubleTimeSeries(this);
        }
        #endregion

    }
}
