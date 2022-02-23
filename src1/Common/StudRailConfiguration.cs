using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    class StudRailConfiguration
    {
        private int ID;
        private string Gridlines;
        private double ColumnWidth;
        private double ColumnDepth;
        private double EffectiveSlabDepth;
        private double ConcreteCompressiveStrength;
        private int ColumnLocationType;
        private double ConcreteDensityRatio;
        private double ConcreteMaterialResistenceFactor;
        private double FactoredMomentX;
        private double FactoredMomentY;
        private double FactoredShearForce;
        public double UnreinforcedPunchingShearUtilization;

        public StudRailConfiguration(
            int id,
            string gridlines,
            double columnwidth,
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
            this.ID = id;
            this.Gridlines = gridlines;
            this.ColumnWidth = columnwidth;
            this.ColumnDepth = columnDepth;
            this.EffectiveSlabDepth = effectiveSlabDepth;
            this.ConcreteCompressiveStrength = concreteCompressiveStrength;
            this.ColumnLocationType = columnLocationType;
            this.ConcreteDensityRatio = concreteDensityRatio;
            this.ConcreteMaterialResistenceFactor = concreteMaterialResistenceFactor;
            this.FactoredMomentX = factoredMomentX;
            this.FactoredMomentY = factoredMomentY;
            this.FactoredShearForce = factoredShearForce;
        }

        public void CheckUnreinforcedPunchingShearUtilization()
        {
            double shearLengthX = Tools.ShearLength(this.ColumnWidth, this.EffectiveSlabDepth);
            double shearLengthY = Tools.ShearLength(this.ColumnDepth, this.EffectiveSlabDepth);
            double shearPerimeter = Tools.ShearPerimeter(shearLengthX, shearLengthY);
            double shearArea = Tools.ShearArea(shearPerimeter, this.EffectiveSlabDepth);
            double gammaVX = Tools.GammaV(shearLengthX, shearLengthY);
            double gammaVY = Tools.GammaV(shearLengthY, shearLengthX);
            double eccentricityX = Tools.Eccentricity(shearLengthX);
            double eccentricityY = Tools.Eccentricity(shearLengthY);
            double polarMomentX = Tools.PolarMoment(this.EffectiveSlabDepth, shearLengthY, shearLengthX);
            double polarMomentY = Tools.PolarMoment(this.EffectiveSlabDepth, shearLengthX, shearLengthY);
            double factoredShearStress = Tools.PunchingShearStressWithMoments(
                this.FactoredShearForce,
                shearArea,
                gammaVX,
                this.FactoredMomentX,
                eccentricityX,
                polarMomentX,
                gammaVY,
                this.FactoredMomentY,
                eccentricityY,
                polarMomentY);
            double unreinforcedShearStressCapacity = Tools.UnreinforcedShearStressCapacity(
                this.EffectiveSlabDepth,
                this.ColumnWidth,
                this.ColumnDepth,
                this.ConcreteMaterialResistenceFactor,
                this.ConcreteCompressiveStrength,
                this.ConcreteDensityRatio,
                this.ColumnLocationType,
                shearPerimeter);

            this.UnreinforcedPunchingShearUtilization = factoredShearStress / unreinforcedShearStressCapacity;
        }

        // Main controller to iterate lengths and find extent of rails
        public void IterateStudRailLengths() {
        }

        // Main controller to iterate spacing and stud rail lengths
        public void IterateStudRailSpacingAndDiameter() {
        }

        //check if stud is needed > 1



        //check how far stud need to go 
        //guess a length, calculate polar moment of intertia, check shear stress at extremeties,

        //iterate on stud spacing to get shear stress to work internally

        //set configuration
        public int MyProperty { get; set; }
    }
}
