using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Cut_Optimizer.UI;

namespace Cut_Optimizer
{
    public class ExEvt : IExternalEventHandler
    {
        public Request request { get; set; }
        public void Execute(UIApplication app)
        {
            switch (request)
            {
                case Request.Report:
                    RvtUtils.Collector(ExCmd.doc,Mainform.fromDate,Mainform.toDate,Mainform.ttle,Mainform.directory);
                    break;
                case Request.Assign:
                    RvtUtils.GetRebarActivityIDs(ExCmd.doc, Mainform.excel);
                    break;
            }
        }

        public string GetName()
        {
            return "EDECS Toolkit";
        }
        public enum Request
        {
            Report,
            Assign
        }
    }
}
