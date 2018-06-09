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
            return budgets.Sum(b => b.EffectiveAmount(period));
        }
    }
}