using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudRail
{
    class Program
    {
        static void Main(string[] args)
        {
            double columnSizex = 20.0;
            double columnSizey = 20.0;
            double d = 6.625;

            double Lx = Tools.L(columnSizex, d);
            double Ly = Tools.L(columnSizey, d);
            double bo = Tools.bo(Lx, Ly);
            double Ac = Tools.Ac(bo, d);
            double GammaVx = Tools.GammaV(Lx, Ly);
            double GammaVy = Tools.GammaV(Ly, Lx);

        }
    }
}
