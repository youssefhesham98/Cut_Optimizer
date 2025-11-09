using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cut_Optimizer
{
    public class Data
    {
        public string Source { get; set; }
        public int RebarId { get; set; }
        public double BarDiameter { get; set; }
        public double BarLength { get; set; }
        public double BarCuts { get; set; }
        public double TotalBarLength { get; set; }
        public double NoOfBars { get; set; }
        public double Weight { get; set; }
        public double WeightTon { get; set; }
        public string Date { get; set; }
        public string Label { get; set; }

        public static void Intialize()
        {
            //XXXX = new List<Element>();
        }
    }
}
