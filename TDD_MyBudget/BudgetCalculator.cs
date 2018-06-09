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
                return EffectiveAmount(period.Start, period.End, budgets);
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
                    totalBudget += EffectiveAmount(startDate, endDate, budgets);
                }
            }

            return totalBudget;
        }

        private decimal EffectiveAmount(DateTime startDate, DateTime endDate, List<Budget> budgets)
        {
            var budgetResult = budgets.FirstOrDefault(x => x.Month == $"{startDate:yyyyMM}");

            var budget = budgetResult?.Amount ?? 0;

            var amount = budget * ((endDate - startDate).Days + 1) / DateTime.DaysInMonth(startDate.Year, startDate.Month);

            return amount;
        }
    }
}