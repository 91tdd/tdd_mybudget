using System;
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
            if (period.Start.Year == period.End.Year && period.Start.Month == period.End.Month)
            {
                return GetRsult(period.Start, period.End);
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
                    totalBudget += GetRsult(startDate, endDate);
                }
            }

            return totalBudget;
            return 0;
        }

        private decimal GetRsult(DateTime StartDate, DateTime EndDate)
        {
            var budgetResult = _repo.GetBudget().FirstOrDefault(x => x.Month == string.Format("{0:yyyyMM}", StartDate));

            var budget = (budgetResult == null) ? 0 : budgetResult.Amount;

            var amount = budget * ((EndDate - StartDate).Days + 1) / DateTime.DaysInMonth(StartDate.Year, StartDate.Month);

            return amount;
        }
    }
}