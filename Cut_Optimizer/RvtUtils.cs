using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Cut_Optimizer
{
    public class RvtUtils
    {
        public static void Collector(Document doc)
        {
            try
            {
                // Collect all Structural Rebar elements in the active document
                FilteredElementCollector rebarCollector = new FilteredElementCollector(doc)
                    .OfClass(typeof(Rebar))
                    .WhereElementIsNotElementType();

                List<string> rebarInfoList = new List<string>();

                foreach (Rebar rebar in rebarCollector)
                {
                    string rebarName = rebar.Name;

                    // Try to get parameters safely
                    string barDiameter = GetParameterValue(rebar, "Bar Diameter");
                    string barLength = GetParameterValue(rebar, "Bar Length");
                    string totalBarLength = GetParameterValue(rebar, "Total Bar Length");
                    string weight = GetParameterValue(rebar, "Weight");
                    string weightTon = GetParameterValue(rebar, "Weight (ton)");
                    string numOfBars = GetParameterValue(rebar, "No. of Bars");
                    string date = GetParameterValue(rebar, "Date");
                    string label = GetParameterValue(rebar, "Rebar Label");

                    // Combine all values in one readable line
                    string info = $"Rebar ID: {rebar.Id.IntegerValue} | " +
                                  $"Diameter: {barDiameter} | Length: {barLength} | Total Length: {totalBarLength} | " +
                                  $"Weight: {weight} | Weight (ton): {weightTon} | Count: {numOfBars} | " +
                                  $"Date: {date} | Label: {label}";

                    rebarInfoList.Add(info);
                }

                // Show summary
                TaskDialog.Show("Rebar Collector",
                    $"Found {rebarInfoList.Count} rebars.\n\n" +
                    string.Join("\n", rebarInfoList.Take(30)) + // display only first 30 for safety
                    (rebarInfoList.Count > 30 ? "\n... (more rebars not shown)" : "")
                );

            }
            catch (Exception ex)
            {
              TaskDialog.Show("Error", $"An error occurred: {ex.Message}");
            }
        }

        // HELPERS
        private static string GetParameterValue(Element element, string paramName)
        {
            Parameter param = element.LookupParameter(paramName);
            if (param == null) return "N/A";

            switch (param.StorageType)
            {
                case StorageType.String:
                    return param.AsString();
                case StorageType.Double:
                    // Convert from internal Revit units to millimeters/meters as needed
                    double val = param.AsDouble();
                    return UnitUtils.ConvertFromInternalUnits(val, UnitTypeId.Millimeters).ToString("0.##");
                case StorageType.Integer:
                    return param.AsInteger().ToString();
                case StorageType.ElementId:
                    ElementId id = param.AsElementId();
                    return id.IntegerValue.ToString();
                default:
                    return "N/A";
            }
        }
    }
}

