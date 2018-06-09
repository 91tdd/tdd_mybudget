using System;
using System.Collections.Generic;
using System.Linq;

namespace TDD_MyBudget
{
    public class BudgetCalculator
    {
        private readonly IRepository<Budget> _repo;

        public BudgetCalculator(IRepository<Budget> repo)
        {
            _repo = repo;
        }

        public Decimal TotalAmount(DateTime start, DateTime end)
        {
            var period = new Period(start, end);
            var budgets = _repo.GetBudget();
            if (period.IsSameMonth())
            {
                return EffectiveAmount(budgets, period);
            }

            decimal totalBudget = 0;
            for (int year = period.Start.Year; year <= period.End.Year; year++)
            {
                for (int month = period.Start.Month; month <= period.End.Month; month++)
                {
                    DateTime startDate = period.Start.Year == year && period.Start.Month == month
                        ? new DateTime(year, month, period.Start.Day)
                        : new DateTime(year, month, 1);
                    DateTime endDate = period.End.Year == year && period.End.Month == month
                        ? new DateTime(year, month, period.End.Day)
                        : new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    totalBudget += EffectiveAmount(budgets, new Period(startDate, endDate));
                }
            }

            return totalBudget;
        }

        private decimal EffectiveAmount(List<Budget> budgets, Period period)
        {
            var budget = budgets.FirstOrDefault(x => x.Month == $"{period.Start:yyyyMM}");
            if (budget == null)
            {
                return 0;
            }

            return period.Days() * budget.DailyAmount();
        }
    }
}