﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skender.Stock.Indicators;

namespace Internal.Tests
{
    [TestClass]
    public class Ultimate : TestBase
    {

        [TestMethod]
        public void Standard()
        {

            List<UltimateResult> results = quotes.GetUltimate(7, 14, 28)
                .ToList();

            // assertions

            // proper quantities
            // should always be the same number of results as there is quotes
            Assert.AreEqual(502, results.Count);
            Assert.AreEqual(474, results.Where(x => x.Ultimate != null).Count());

            // sample values
            UltimateResult r1 = results[74];
            Assert.AreEqual(51.7770m, Math.Round((decimal)r1.Ultimate, 4));

            UltimateResult r2 = results[249];
            Assert.AreEqual(45.3121m, Math.Round((decimal)r2.Ultimate, 4));

            UltimateResult r3 = results[501];
            Assert.AreEqual(49.5257m, Math.Round((decimal)r3.Ultimate, 4));
        }

        [TestMethod]
        public void BadData()
        {
            IEnumerable<UltimateResult> r = Indicator.GetUltimate(historyBad, 1, 2, 3);
            Assert.AreEqual(502, r.Count());
        }

        [TestMethod]
        public void Removed()
        {
            List<UltimateResult> results = quotes.GetUltimate(7, 14, 28)
                .RemoveWarmupPeriods()
                .ToList();

            // assertions
            Assert.AreEqual(502 - 28, results.Count);

            UltimateResult last = results.LastOrDefault();
            Assert.AreEqual(49.5257m, Math.Round((decimal)last.Ultimate, 4));
        }

        [TestMethod]
        public void Exceptions()
        {
            // bad short period
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                Indicator.GetUltimate(quotes, 0));

            // bad middle period
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                Indicator.GetUltimate(quotes, 7, 6));

            // bad long period
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                Indicator.GetUltimate(quotes, 7, 14, 11));

            // insufficient quotes
            Assert.ThrowsException<BadQuotesException>(() =>
                Indicator.GetUltimate(HistoryTestData.Get(28), 7, 14, 28));
        }
    }
}
