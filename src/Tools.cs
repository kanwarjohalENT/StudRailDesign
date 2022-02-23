using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudRail
{
    public static class Tools
    {
        public static double L(double columnSize, double slabDepth)
        {
            if (columnSize <= 0) throw new ArgumentException("Value must be positive", nameof(columnSize));
            if (slabDepth <= 0) throw new ArgumentException("Value must be positive", nameof(columnSize));

            return columnSize + slabDepth;
        }

        public static double bo(double Lx, double Ly) => (2*Lx) + (2*Ly);

        public static double Ac(double bo, double d) => bo * d;

        public static double GammaV(double L1, double L2) => 1.0 - (1.0 / (1.0 + ((2.0 / 3.0) * (Math.Sqrt(L2 / L1)))));
    }
}
