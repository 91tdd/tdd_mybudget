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
            for (int year = period.Start.Year; year <= period.End.Year; year++)
            {
                for (int month = period.Start.Month; month <= period.End.Month; month++)
                {
                    var budget = budgets.FirstOrDefault(x => x.Month == $"{year:0000}{month:00}");
                    if (budget != null)
                    {
                        DateTime overlapStartDate = period.Start > budget.FirstDay
                            ? period.Start
                            : budget.FirstDay;

                        DateTime overlapEndDate = period.End < budget.LastDay
                            ? period.End
                            : budget.LastDay;

                        Period overlapPeriod = new Period(overlapStartDate, overlapEndDate);
                        var effectiveAmount = EffectiveAmount(overlapPeriod, budget);
                        totalBudget += effectiveAmount;
                    }
                }
            }

            return totalBudget;
        }

        private decimal EffectiveAmount(Period period, Budget budget)
        {
            return period.Days() * budget.DailyAmount();
        }
    }
}