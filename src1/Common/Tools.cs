// <copyright file="Tools.cs" company="Entuitive Corporation">
// Copyright (c) Entuitive Corporation. All rights reserved.
// </copyright>

using System;

namespace Common
{
    public enum ColumnLocationType
    {
        // ENUMS CORRESSPOND TO VALUES IN CSA A23.3 CLAUSE 13.3.4
        Interior = 4,
        Edge = 3,
        Corner = 2
    }

    public enum ColumnType
    {
        Rectangular,
        Circular,
    }

    public static class Tools
    {
        public static double ShearLength(double columnSize, double effectiveSlabDepth)
        {
#pragma warning disable SA1503 // Braces should not be omitted
            if (columnSize <= 0) throw new ArgumentException("Value must be positive", nameof(columnSize));
            if (effectiveSlabDepth <= 0) throw new ArgumentException("Value must be positive", nameof(effectiveSlabDepth));
#pragma warning restore SA1503 // Braces should not be omitted
            return columnSize + effectiveSlabDepth;
        }

        public static double ShearPerimeter(double shearLength1, double shearLength2) {
#pragma warning disable SA1503 // Braces should not be omitted
            if (shearLength1 <= 0) throw new ArgumentException("Value must be positive", nameof(shearLength1));
            if (shearLength2 <= 0) throw new ArgumentException("Value must be positive", nameof(shearLength2));
#pragma warning restore SA1503 // Braces should not be omitted

            return (2 * shearLength1) + (2 * shearLength2);
        }

        public static double ShearArea(double shearPerimeter, double effectiveSlabDepth) =>
            shearPerimeter * effectiveSlabDepth;

        public static double GammaV(double shearLength1, double shearLength2) =>
            1.0 - (1.0 / (1.0 + ((2.0 / 3.0) * Math.Sqrt(shearLength2 / shearLength1))));

        public static double Eccentricity(double shearLength)
        {
            // This returns interior column eccentricity as per CSA A23.3 Page 5-42. Edge, corner solution to be added.
#pragma warning disable SA1503 // Braces should not be omitted
            if (shearLength <= 0) throw new ArgumentException("Value must be positive", nameof(shearLength));
#pragma warning restore SA1503 // Braces should not be omitted

            return shearLength * 0.5;
        }

        public static double PolarMoment(double effectiveSlabDepth, double shearLength1, double shearLength2)
        {
            // This returns interior column polar moment of intertia as per CSA A23.3 Page 5-42. Edge, corner and ACI 421.1 method to be added.
            return (shearLength1 * Math.Pow(effectiveSlabDepth, 3) / 6) + (Math.Pow(shearLength1, 3) * effectiveSlabDepth / 6) + (effectiveSlabDepth*shearLength2 * Math.Pow(shearLength1, 2) / 2);
        }

        public static double PunchingShearStressWithMoments(
            double factoredShearForce,
            double shearArea,
            double gammaV1,
            double factoredMoment1,
            double eccentricity1,
            double polarMoment1,
            double gammaV2,
            double factoredMoment2,
            double eccentricity2,
            double polarMoment2)
        {
            // CSA A23.3 EQN 13.9
            // Principal and geometric axis are same if angle of rotation is 0, typical if double symmetric section.
            // Therefore Axis 1 and 2 are equivalent to Axis x and y
#pragma warning disable SA1503 // Braces should not be omitted
            if (polarMoment1 <= 0) throw new ArgumentException("Value must be positive", nameof(polarMoment1));
            if (polarMoment2 <= 0) throw new ArgumentException("Value must be positive", nameof(polarMoment2));
#pragma warning restore SA1503 // Braces should not be omitted

            double punchingShearStressWithMoments = (factoredShearForce / shearArea) +
                (gammaV1 * factoredMoment1 * eccentricity2 / polarMoment1) +
                (gammaV2 * factoredMoment2 * eccentricity1 / polarMoment2);

            return punchingShearStressWithMoments;
        }

        public static double UnreinforcedShearStressCapacity(
            double effectiveSlabDepth,
            double columnWidth,
            double columnDepth,
            double concreteMaterialResistenceFactor,
            double concreteCompressiveStrength,
            double concreteDensityRatio,
            int columnLocationType,
            double shearPerimeter)
        {
            // CSA 13.3.4.1 / 13.3.4.2 / 13.3.4.3
            // This returns shear stress capacity for unreinforced concrete sections, edge, corner, interior, and accounts for max Vc, and deep sections ...
#pragma warning disable SA1503 // Braces should not be omitted
            if (concreteMaterialResistenceFactor <= 0) throw new ArgumentException("Value must be positive", nameof(concreteMaterialResistenceFactor));
            if (concreteDensityRatio <= 0) throw new ArgumentException("Value must be positive", nameof(concreteDensityRatio));
#pragma warning restore SA1503 // Braces should not be omitted

            double effectiveConcreteStrength = Math.Min(Math.Pow(concreteCompressiveStrength, 0.5), 8);
            double concreteStrengthDepthModifier = 1.0;
            double longToShortColumnDimensionRatio = Math.Max(columnWidth / columnDepth, columnDepth / columnWidth);

            if (effectiveSlabDepth > 300) {
                concreteStrengthDepthModifier = 1300 / (1000 + effectiveSlabDepth);
            }

            double shearStressCapacityA = (1 + (2 / longToShortColumnDimensionRatio)) * 0.19 * concreteDensityRatio * concreteMaterialResistenceFactor * effectiveConcreteStrength;
            double shearStressCapacityB = ((columnLocationType * effectiveSlabDepth / shearPerimeter) + 0.19) * concreteDensityRatio * concreteMaterialResistenceFactor * effectiveConcreteStrength;
            double shearStressCapacityC = 0.38 * concreteDensityRatio * concreteMaterialResistenceFactor * effectiveConcreteStrength;

            double effectiveShearStressCapacity = concreteStrengthDepthModifier * Math.Min(Math.Min(shearStressCapacityA, shearStressCapacityB), shearStressCapacityC);

            return effectiveShearStressCapacity;
        }

        public static double ShearStressLimitOutsideReinforcedRegion(double concreteDensityRatio, double concreteMaterialResistenceFactor, double concreteCompressiveStrength)
        {
            // Clause 13.3.7.4
            // Maximum shear stress allowed outside a reinforced region
            return 0.19 * concreteDensityRatio * concreteMaterialResistenceFactor * Math.Pow(concreteCompressiveStrength, 0.5);
        }


        /// <summary>
        /// Concrete shear stress capacity limit in shear stud reinforced region. Clause 13.3.8.2.
        /// </summary>
        /// <param name="concreteDensityRatio"> Lambda.</param>
        /// <param name="concreteMaterialResistenceFactor">phic.</param>
        /// <param name="concreteCompressiveStrength">f'c.</param>
        /// <returns>Maximum allowable shear stress capacity in stud rail reinforced slab</returns>
        public static double ShearStressCapacityLimitInReinforcedRegion(
            double concreteDensityRatio,
            double concreteMaterialResistenceFactor,
            double concreteCompressiveStrength)
        {
            return 0.75 * concreteDensityRatio * concreteMaterialResistenceFactor * Math.Pow(concreteCompressiveStrength, 0.5);
        }

        /// <summary>
        /// Concrete shear stress capacity limit in shear stud reinforced region.Clause 13.3.8.3.
        /// </summary>
        /// <param name="concreteDensityRatio">lambda.</param>
        /// <param name="concreteMaterialResistenceFactor">phiC.</param>
        /// <param name="concreteCompressiveStrength">f'c.</param>
        /// <returns></returns>
        public static double ConcreteShearStressCapacityInReinforcedRegion(double concreteDensityRatio, double concreteMaterialResistenceFactor, double concreteCompressiveStrength)
        {
            return 0.28 * concreteDensityRatio * concreteMaterialResistenceFactor * Math.Pow(concreteCompressiveStrength, 0.5);
        }

        public static double ShearStudStressCapacityInReinforcedRegion(
            double steelMaterialResistenceFactor,
            double shearStudCrossSectionalArea,
            double shearStudTensileStrength,
            double shearPerimeter,
            double studSpacing)
        {
            // Clause 13.3.8.5
            // Concrete shear stress capacity limit in shear stud reinforced region
            return (steelMaterialResistenceFactor * shearStudCrossSectionalArea * shearStudTensileStrength) / (shearPerimeter * studSpacing);
        }

        public static double CalculatePolarMomentOfInertia()
        {
            return 0;
        }

        public static double CheckShearStressAtExtentsOfStudRails()
        {
            return 0;
        }
    }
}
