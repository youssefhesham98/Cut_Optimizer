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
        public double TotalBarLength { get; set; }
        public int NoOfBars { get; set; }
        public string Weight { get; set; }
        public string WeightTon { get; set; }
        public string Date { get; set; }
        public string Label { get; set; }

        public static void Intialize()
        {
            //XXXX = new List<Element>();
        }
    }
}
