using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeSeriesLib
{
    public class Hole
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public Hole(DateTime start, DateTime end)
        {
            if (start > end)
                throw new ArgumentException("Starting dates should be before ending date.", "start");
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            if (Start==End)
                return "Missing data on " + Start.ToShortDateString();
            else
                return "Missing data from " + Start.ToShortDateString() + " to " + End.ToShortDateString();
        }
    }
}
