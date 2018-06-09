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
                var budget = budgets.FirstOrDefault(x => x.Month == $"{period.Start:yyyyMM}");
                if (budget == null)
                {
                    return 0;
                }

                return period.Days() * budget.DailyAmount();
            }

            var totalBudget = TotalBudgetOfMultiMonthsPeriod(period, budgets);

            return totalBudget;
        }

        private decimal TotalBudgetOfMultiMonthsPeriod(Period period, List<Budget> budgets)
        {
            decimal totalBudget = 0;
            foreach (var budget in budgets)
            {
                totalBudget += budget.EffectiveAmount(period);
            }

            return totalBudget;
            for (int year = period.Start.Year; year <= period.End.Year; year++)
            {
                for (int month = period.Start.Month; month <= period.End.Month; month++)
                {
                    var budget = budgets.FirstOrDefault(x => x.Month == $"{year:0000}{month:00}");
                    if (budget != null)
                    {
                        totalBudget += budget.EffectiveAmount(period);
                    }
                }
            }

            return totalBudget;
        }
    }
}