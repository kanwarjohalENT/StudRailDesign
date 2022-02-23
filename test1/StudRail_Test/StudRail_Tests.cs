// <copyright file="StudRail_Tests.cs" company="Entuitive Corporation">
// Copyright (c) Entuitive Corporation. All rights reserved.
// </copyright>

using System;
using Common;
using NUnit.Framework;

namespace StudRail_Test
{
    [TestFixture]
    public class StudRail_Tests
    {
        [TestCase(20.0, 6.625, ExpectedResult = 26.625)]
        public double Check_ShearLength(double columnSize, double effectiveSlabDepth)
        {
            return Tools.ShearLength(columnSize, effectiveSlabDepth);
        }

        [TestCase(-1, 200.0)]
        [TestCase(0, 150.0)]
        [TestCase(150, 0)]
        [TestCase(150, -1)]
        public void TestException_ShearLength(double columnSize, double effectiveSlabDepth)
        {
            Assert.Throws<ArgumentException>(() => Tools.ShearLength(columnSize, effectiveSlabDepth));
        }

        [TestCase(26.625, 26.625, ExpectedResult = 106.5)]
        [DefaultFloatingPointTolerance(0.02)]
        public double Check_ShearPerimeter(double shearLength1, double shearLength2)
        {
            return Tools.ShearPerimeter(shearLength1, shearLength2);
        }

        [Test]
        public void Check_ShearArea()
        {
            // Arrange
            double expected = 705.5625;

            // Act
            double actual = Tools.ShearArea(106.5, 6.625);

            // Assert
            Assert.That(expected, Is.EqualTo(actual).Within(1).Percent);
        }

        [Test]
        public void Check_GammaV()
        {
            // Arrange
            double expected = 0.40;

            // Act
            double actual = Tools.GammaV(20, 20);

            // Assert
            Assert.That(expected, Is.EqualTo(actual).Within(1).Percent);

            // Arrange
            expected = 0.434;

            // Act
            actual = Tools.GammaV(610, 805);

            // Assert
            Assert.That(expected, Is.EqualTo(actual).Within(1).Percent);
        }

        [Test]
        public void Check_Eccentricity()
        {
            // Arrange
            double expected = 405;

            // Act
            double actual = Tools.Eccentricity(810);

            // Assert
            Assert.That(expected, Is.EqualTo(actual).Within(1).Percent);

        }

        [Test]
        public void Check_PolarMoment()
        {
            // Arrange
            double expected = 83360;

            // Act
            double actual = Tools.PolarMoment(6.625, 26.625, 26.625);

            // Assert
            Assert.That(expected, Is.EqualTo(actual).Within(5).Percent);

            // Arrange
            expected = 40.53 * Math.Pow(10, 9);

            // Act
            actual = Tools.PolarMoment(210, 610, 810);

            // Assert
            Assert.That(expected, Is.EqualTo(actual).Within(5).Percent);

        }

        [Test]
        public void Check_PunchingShearStressWithMoments()
        {
            // Arrange
            double expected = 1.12;

            // Act
            double actual = Tools.PunchingShearStressWithMoments(543576, 596400, 0.434, 73.4 * Math.Pow(10, 6), 405.0, 61.87 * Math.Pow(10, 9), 0.0, 0.0, 0.0, 0.1);

            // Assert
            Assert.That(expected, Is.EqualTo(actual).Within(1).Percent);
        }

        [Test]
        public void Check_UnreinforcedShearStressCapacity()
        {
            // Arrange
            double expected = 1.23;

            // Act
            double actual = Tools.UnreinforcedShearStressCapacity(210, 400, 600, 0.65, 25, 1.0, (int)ColumnLocationType.Interior, 2840);

            // Assert
            Assert.That(expected, Is.EqualTo(actual).Within(1).Percent);

            // Arrange
            double expected2 = 1.28;

            // Act
            double actual2 = Tools.UnreinforcedShearStressCapacity(400, 1000, 200, 0.65, 90, 1.0, (int)ColumnLocationType.Edge, 2200);

            // Assert
            Assert.That(expected2, Is.EqualTo(actual2).Within(1).Percent);

            // Arrange
            double expected3 = 1.657;

            // Act
            double actual3 = Tools.UnreinforcedShearStressCapacity(200, 800, 1000, 0.65, 45, 1.0, (int)ColumnLocationType.Corner, 2000);

            // Assert
            Assert.That(expected3, Is.EqualTo(actual3).Within(1).Percent);
        }

        [Test]
        public void Check_ShearStressLimitOutsideReinforcedRegion()
        {
            // Arrange
            double expected = 0.741;

            // Act
            double actual = Tools.ShearStressLimitOutsideReinforcedRegion(1.0, 0.65, 36);

            // Assert
            Assert.That(expected, Is.EqualTo(actual).Within(1).Percent);
        }

        [Test]
        public void Check_ShearStressCapacityLimitInReinforcedRegion()
        {
            // Arrange
            double expected = 2.92;

            // Act
            double actual = Tools.ShearStressCapacityLimitInReinforcedRegion(1.0, 0.65, 36);

            // Assert
            Assert.That(expected, Is.EqualTo(actual).Within(1).Percent);
        }

        [Test]
        public void Check_ConcreteShearStressCapacityInReinforcedRegion()
        {
            // Arrange
            double expected = 1.092;

            // Act
            double actual = Tools.ConcreteShearStressCapacityInReinforcedRegion(1.0, 0.65, 36);

            // Assert
            Assert.That(expected, Is.EqualTo(actual).Within(1).Percent);
        }

        [Test]
        public void Check_ShearStudStressCapacityInReinforcedRegion()
        {
            // Arrange
            double expected = 1.676;

            // Act
            double actual = Tools.ShearStudStressCapacityInReinforcedRegion(0.9, 12.0 * 127.0, 345, 2800, 100);

            // Assert
            Assert.That(expected, Is.EqualTo(actual).Within(1).Percent);
        }

        [Test]
        public void Check_PunchingShearCalculatorUnreinforcedCheck()
        {
            // Arrange
            double expected = 1.676;

            // Act
            double actual = Tools.ShearStudStressCapacityInReinforcedRegion(0.9, 12.0 * 127.0, 345, 2800, 100);

            // Assert
            Assert.That(expected, Is.EqualTo(actual).Within(1).Percent);
        }

        [Test]
        public void Check_PunchingShearUtilizationUnreinforcedCheck()
        {
            // Arrange
            double expected = 0.9105;

            // Act
            double actual = PunchingShearModules.CheckUnreinforcedPunchingShearUtilization(600, 400, 210, 25, (int)ColumnLocationType.Interior, 1.0, 0.65, 0, 73400000, 543576);

            // Assert
            Assert.That(expected, Is.EqualTo(actual).Within(3).Percent);

            // Arrange
            double expected2 = 0.859;

            // Act
            double actual2 = PunchingShearModules.CheckUnreinforcedPunchingShearUtilization(500, 1000, 250, 35, (int)ColumnLocationType.Interior, 1.0, 0.65, 500000000, 100000000, 500000);

            // Assert
            Assert.That(expected2, Is.EqualTo(actual2).Within(3).Percent);
        }

        [Test]
        public void Check_StudRailConfiguration()
        {
            // Arrange
            double expected = 0.859;
            var studRailConfiguration = new StudRailConfiguration(
                1,
                "G-8",
                500,
                1000,
                250,
                35,
                (int)ColumnLocationType.Interior,
                1.0,
                0.65,
                500000000,
                100000000,
                500000);

            // Act
            studRailConfiguration.CheckUnreinforcedPunchingShearUtilization();

            // Assert
            Assert.That(expected, Is.EqualTo(studRailConfiguration.UnreinforcedPunchingShearUtilization).Within(3).Percent);
        }
        }
}