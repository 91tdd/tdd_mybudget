using System;

namespace TDD_MyBudget
{
    public class Budget
    {
        public int Amount { get; set; }

        public int DaysInMonth
        {
            get
            {
                return DateTime.DaysInMonth(FirstDay.Year, FirstDay.Month);
            }
        }

        public DateTime FirstDay
        {
            get
            {
                return DateTime.ParseExact(Month + "01", "yyyyMMdd", null);
            }
        }

        public string Month { get; set; }
    }
}