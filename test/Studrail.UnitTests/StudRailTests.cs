using NUnit.Framework;
//using StudRail;
using System;

namespace StudRail//.UnitTests
{
    [TestFixture]
    public class StudRailTests
    {
        [TestCase(20.0, 6.625, ExpectedResult=26.625)]
        [TestCase(20.0, 5.0, ExpectedResult=25.0)]
        public double Check_L(double columnSize, double slabDepth)
        {
            return L(columnSize, slabDepth);
        }

        [Test]
        public void Check_bo()
        {
            var result = Tools.bo(26.625, 26.625);
            
            Assert.That(result == 106.5);
        }

        [Test]
        public void Check_Ac()
        {
            var result = Tools.Ac(106.5, 6.625);

            Assert.That(result == 705.5625);
        }

        [Test]
        public void Check_GammaV()
        {
            var result = Tools.GammaV(20, 20);

            Assert.That(result == 0.40);
        }



    }
}