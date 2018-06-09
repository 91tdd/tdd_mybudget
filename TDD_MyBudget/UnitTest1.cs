using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace TDD_MyBudget
{
    [TestClass]
    public class UnitTest1
    {
        public IRepository<Budget> repo = Substitute.For<IRepository<Budget>>();
        public BudgetCalculator BC;

        public List<Budget> ListofBudget = new List<Budget>
        {
            new Budget {Month = "201806", Amount = 300},
            new Budget {Month = "201807", Amount = 310},
            new Budget {Month = "201602", Amount = 2900},
        };

        [TestInitialize]
        public void init()
        {
            repo.GetBudget().Returns(ListofBudget);
            BC = new BudgetCalculator(repo);
        }

        [TestMethod]
        public void GetFullMonthResult()
        {
            CultureInfo prov = CultureInfo.InvariantCulture; ;

            Assert.AreEqual(300, BC.TotalAmount(DateTime.ParseExact("20180601", "yyyyMMdd", prov),
                    DateTime.ParseExact("20180630", "yyyyMMdd", prov)));
        }

        [TestMethod]
        public void WrongDatEntered()
        {
            CultureInfo prov = CultureInfo.InvariantCulture; ;

            Assert.ThrowsException<Exception>(() => BC.TotalAmount(DateTime.ParseExact("20190601", "yyyyMMdd", prov),
                DateTime.ParseExact("20180601", "yyyyMMdd", prov)));
        }

        [TestMethod]
        public void GetOneDayResult()
        {
            CultureInfo prov = CultureInfo.InvariantCulture; ;

            Assert.AreEqual(10, BC.TotalAmount(DateTime.ParseExact("20180601", "yyyyMMdd", prov),
                   DateTime.ParseExact("20180601", "yyyyMMdd", prov)));
        }

        [TestMethod]
        public void WithOverlappedMonth()
        {
            CultureInfo prov = CultureInfo.InvariantCulture; ;

            Assert.AreEqual(610, BC.TotalAmount(DateTime.ParseExact("20180601", "yyyyMMdd", prov),
                DateTime.ParseExact("20180731", "yyyyMMdd", prov)));
        }

        [TestMethod]
        public void GetZeroResult()
        {
            CultureInfo prov = CultureInfo.InvariantCulture; ;

            Assert.AreEqual(0, BC.TotalAmount(DateTime.ParseExact("20180801", "yyyyMMdd", prov),
                DateTime.ParseExact("20180815", "yyyyMMdd", prov)));
        }

        [TestMethod]
        public void GetSpecialFebResult()
        {
            CultureInfo prov = CultureInfo.InvariantCulture; ;

            Assert.AreEqual(2900, BC.TotalAmount(DateTime.ParseExact("20160101", "yyyyMMdd", prov),
                DateTime.ParseExact("20160330", "yyyyMMdd", prov)));
        }

        [TestMethod]
        public void GetSpecialFebResult_1()
        {
            CultureInfo prov = CultureInfo.InvariantCulture; ;

            Assert.AreEqual(1400, BC.TotalAmount(DateTime.ParseExact("20160203", "yyyyMMdd", prov),
                DateTime.ParseExact("20160216", "yyyyMMdd", prov)));
        }

        [TestMethod]
        public void GetFullResult()
        {
            CultureInfo prov = CultureInfo.InvariantCulture; ;

            Assert.AreEqual(3510, BC.TotalAmount(DateTime.ParseExact("20160101", "yyyyMMdd", prov),
                DateTime.ParseExact("20181231", "yyyyMMdd", prov)));
        }
    }
}