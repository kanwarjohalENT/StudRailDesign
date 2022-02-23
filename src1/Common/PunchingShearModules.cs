using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class PunchingShearModules
    {
        public static double CheckUnreinforcedPunchingShearUtilization(
            double columnWidth,
            double columnDepth,
            double effectiveSlabDepth,
            double concreteCompressiveStrength,
            int columnLocationType,
            double concreteDensityRatio,
            double concreteMaterialResistenceFactor,
            double factoredMomentX,
            double factoredMomentY,
            double factoredShearForce)
        {
            // Calculates punching shear stress and compares to punching shear resistance and returns a utilization
            double shearLengthX = Tools.ShearLength(columnWidth, effectiveSlabDepth);
            double shearLengthY = Tools.ShearLength(columnDepth, effectiveSlabDepth);
            double shearPerimeter = Tools.ShearPerimeter(shearLengthX, shearLengthY);
            double shearArea = Tools.ShearArea(shearPerimeter, effectiveSlabDepth);
            double gammaVX = Tools.GammaV(shearLengthX, shearLengthY);
            double gammaVY = Tools.GammaV(shearLengthY, shearLengthX);
            double eccentricityX = Tools.Eccentricity(shearLengthX);
            double eccentricityY = Tools.Eccentricity(shearLengthY);
            double polarMomentX = Tools.PolarMoment(effectiveSlabDepth, shearLengthY, shearLengthX);
            double polarMomentY = Tools.PolarMoment(effectiveSlabDepth, shearLengthX, shearLengthY);
            double factoredShearStress = Tools.PunchingShearStressWithMoments(factoredShearForce, shearArea, gammaVX, factoredMomentX, eccentricityX, polarMomentX, gammaVY, factoredMomentY, eccentricityY, polarMomentY);
            double unreinforcedShearStressCapacity = Tools.UnreinforcedShearStressCapacity(effectiveSlabDepth, columnWidth, columnDepth, concreteMaterialResistenceFactor, concreteCompressiveStrength, concreteDensityRatio, columnLocationType, shearPerimeter);

            return factoredShearStress / unreinforcedShearStressCapacity;
        }
    }
}
