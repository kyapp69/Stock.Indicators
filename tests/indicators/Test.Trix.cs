﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skender.Stock.Indicators;

namespace Internal.Tests
{
    [TestClass]
    public class Trix : TestBase
    {

        [TestMethod]
        public void Standard()
        {

            List<TrixResult> results = quotes.GetTrix(20, 5).ToList();

            // assertions

            // proper quantities
            // should always be the same number of results as there is quotes
            Assert.AreEqual(502, results.Count);
            Assert.AreEqual(445, results.Where(x => x.Ema3 != null).Count());
            Assert.AreEqual(444, results.Where(x => x.Trix != null).Count());
            Assert.AreEqual(440, results.Where(x => x.Signal != null).Count());

            // sample values
            TrixResult r1 = results[67];
            Assert.AreEqual(221.6320m, Math.Round((decimal)r1.Ema3, 4));
            Assert.AreEqual(0.055596m, Math.Round((decimal)r1.Trix, 6));
            Assert.AreEqual(0.063512m, Math.Round((decimal)r1.Signal, 6));

            TrixResult r2 = results[249];
            Assert.AreEqual(249.4469m, Math.Round((decimal)r2.Ema3, 4));
            Assert.AreEqual(0.121781m, Math.Round((decimal)r2.Trix, 6));
            Assert.AreEqual(0.119769m, Math.Round((decimal)r2.Signal, 6));

            TrixResult r3 = results[501];
            Assert.AreEqual(263.3216m, Math.Round((decimal)r3.Ema3, 4));
            Assert.AreEqual(-0.230742m, Math.Round((decimal)r3.Trix, 6));
            Assert.AreEqual(-0.204536m, Math.Round((decimal)r3.Signal, 6));
        }

        [TestMethod]
        public void BadData()
        {
            IEnumerable<TrixResult> r = Indicator.GetTrix(historyBad, 15, 2);
            Assert.AreEqual(502, r.Count());
        }

        [TestMethod]
        public void Removed()
        {
            List<TrixResult> results = quotes.GetTrix(20, 5)
                .RemoveWarmupPeriods()
                .ToList();

            // assertions
            Assert.AreEqual(502 - (3 * 20 + 250), results.Count);

            TrixResult last = results.LastOrDefault();
            Assert.AreEqual(263.3216m, Math.Round((decimal)last.Ema3, 4));
            Assert.AreEqual(-0.230742m, Math.Round((decimal)last.Trix, 6));
            Assert.AreEqual(-0.204536m, Math.Round((decimal)last.Signal, 6));
        }

        [TestMethod]
        public void Exceptions()
        {
            // bad lookback period
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                Indicator.GetTrix(quotes, 0));

            // insufficient quotes for 3*N+100
            Assert.ThrowsException<BadQuotesException>(() =>
                Indicator.GetTrix(HistoryTestData.Get(189), 30));

            // insufficient quotes for 4×N
            Assert.ThrowsException<BadQuotesException>(() =>
                Indicator.GetTrix(HistoryTestData.GetLong(999), 250));
        }
    }
}
