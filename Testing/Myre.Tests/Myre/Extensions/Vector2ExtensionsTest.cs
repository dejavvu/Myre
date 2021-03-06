﻿using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwizzleMyVectors;

namespace Myre.Tests.Myre.Extensions
{
    [TestClass]
    public class Vector2ExtensionsTest
    {
        [TestMethod]
        public void AreaOfAntiClockwiseWindingIsNegative()
        {
            var shape = new[] {new Vector2(0, 0), new Vector2(10, 0), new Vector2(10, 10), new Vector2(0, 10)};
            var area = shape.Area();
            Assert.AreEqual(-100f, area);
        }

        [TestMethod]
        public void AreaOfClockwiseWindingIsPositive()
        {
            var shape = new[] { new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10), new Vector2(10, 0) };
            var area = shape.Area();
            Assert.AreEqual(100f, area);
        }
    }
}
