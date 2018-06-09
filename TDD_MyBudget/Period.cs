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

        public int Days()
        {
            var days = (End - Start).Days + 1;
            return days;
        }

        public int OverlappingDays(Period otherPeriod)
        {
            if (HasNoOverlap(otherPeriod))
            {
                return 0;
            }

            DateTime overlapStartDate = Start > otherPeriod.Start
                ? Start
                : otherPeriod.Start;

            DateTime overlapEndDate = End < otherPeriod.End
                ? End
                : otherPeriod.End;

            return (overlapEndDate - overlapStartDate).Days + 1;
        }

        private bool HasNoOverlap(Period otherPeriod)
        {
            return otherPeriod.End < Start || otherPeriod.Start > End;
        }
    }
}