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
            if (DateTime.Compare(end, start) < 0)
            {
                throw new Exception("Illegal date");
            }

            if (start.Year == end.Year && start.Month == end.Month)
            {
                return GetRsult(start, end);
            }

            decimal totalBudget = 0;
            for (int year = start.Year; year <= end.Year; year++)
            {
                for (int month = start.Month; month <= end.Month; month++)
                {
                    DateTime startDate = start.Year == year && start.Month == month
                        ? new DateTime(year, month, start.Day)
                        : new DateTime(year, month, 1);
                    DateTime endDate = end.Year == year && end.Month == month
                        ? new DateTime(year, month, end.Day)
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