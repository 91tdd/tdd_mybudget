using System;

namespace TDD_MyBudget
{
    public class Period
    {
        public Period(DateTime start, DateTime end)
        {
            if (DateTime.Compare(end, start) < 0)
            { 
                throw new Exception("Illegal date");
            }
            Start = start;
            End = end;
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public bool IsSameMonth()
        {
            return Start.Year == End.Year && Start.Month == End.Month;
        }
    }
}