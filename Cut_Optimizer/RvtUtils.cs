using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using LicenseContext = OfficeOpenXml.LicenseContext;
using Rebar = Autodesk.Revit.DB.Structure.Rebar;

namespace Cut_Optimizer
{
    public class RvtUtils
    {
        public static void Collector(Document doc)
        {
            using (Transaction tns = new Transaction(doc, "rebarring"))
            {
                tns.Start();
                try
                {
                    List<Data> allRebars = new List<Data>();

                    // Collect from main document
                    allRebars.AddRange(GetRebarsFromDocument(doc, "Main Model"));

                    // Collect from linked docs
                    var linkCollector = new FilteredElementCollector(doc).OfClass(typeof(RevitLinkInstance));
                    foreach (RevitLinkInstance linkInstance in linkCollector)
                    {
                        Document linkDoc = linkInstance.GetLinkDocument();
                        if (linkDoc == null) continue;

                        allRebars.AddRange(GetRebarsFromDocument(linkDoc, linkInstance.Name));
                    }

                    // --- Aggregate by diameter & length ---
                    List<Data> aggregated = AggregateRebarData(allRebars);

                    // --- Export to Excel ---
                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string excelPath = Path.Combine(desktopPath, $"RebarSummary_{DateTime.Now:yyyyMMdd_HHmm}.xlsx");

                    FilteredElementCollector rebarCollector = new FilteredElementCollector(doc)
                                            .OfClass(typeof(Rebar))
                                            .WhereElementIsNotElementType();
                    int x = 0;
                    foreach (var bar in rebarCollector)
                    {
                        Parameter NoofBars = bar.LookupParameter("No. of Bars");
                        NoofBars.Set(aggregated[x].NoOfBars);
                    }

                    ExportRebarSummaryToExcel(aggregated, excelPath);

                    TaskDialog.Show("Rebar Summary", $"Export completed successfully.\nFile saved at:\n{excelPath}");
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", ex.Message);
                }
                tns.Commit();
            }
        }

        // HELPERS
        /// <summary>
        /// Collects and formats rebar data from a given document.
        /// </summary>
        private static List<Data> GetRebarsFromDocument(Document doc, string sourceLabel)
        {
            List<Data> list = new List<Data>();

            FilteredElementCollector rebarCollector = new FilteredElementCollector(doc)
                .OfClass(typeof(Rebar))
                .WhereElementIsNotElementType();

            foreach (Rebar rebar in rebarCollector)
            {
                var dia = Math.Round(UnitUtils.ConvertFromInternalUnits(double.Parse(GetParameterValue(rebar, "Bar Diameter")),UnitTypeId.Millimeters));
                var len = Math.Round(UnitUtils.ConvertFromInternalUnits(double.Parse(GetParameterValue(rebar, "Bar Length")),UnitTypeId.Millimeters));

                double totalLen = 0;
                try
                {
                    // Try to read numerical total bar length if present
                    string totalStr = GetParameterValue(rebar, "Total Bar Length");
                    double.TryParse(totalStr, out totalLen);
                }
                catch { }

                Data data = new Data
                {
                    Source = sourceLabel,
                    RebarId = rebar.Id.IntegerValue,
                    BarDiameter = dia,
                    BarLength = len,
                    TotalBarLength = Math.Round(totalLen),
                    NoOfBars = 1, // We'll calculate grouped totals later
                    Weight = GetParameterValue(rebar, "Weight"),
                    WeightTon = GetParameterValue(rebar, "Weight (ton)"),
                    Date = GetParameterValue(rebar, "Date"),
                    Label = GetParameterValue(rebar, "Rebar Label")
                };

                list.Add(data);
            }

            return list;
        }
        /// <summary>
        /// Safely extracts a parameter's value as string (Revit 2024 compatible).
        /// </summary>
        private static string GetParameterValue(Element element, string paramName)
        {
            Parameter param = element.LookupParameter(paramName);
            if (param == null) return "N/A";

            switch (param.StorageType)
            {
                case StorageType.String:
                    return param.AsString();
                case StorageType.Double:
                    return param.AsDouble().ToString("0.##");
                case StorageType.Integer:
                    return param.AsInteger().ToString();
                case StorageType.ElementId:
                    return param.AsElementId().IntegerValue.ToString();
                default:
                    return "N/A";
            }
        }
        private static List<Data> AggregateRebarData(List<Data> allRebars)
        {
            var grouped = allRebars
                .Where(r => !string.IsNullOrEmpty(r.BarDiameter.ToString()) && !string.IsNullOrEmpty(r.BarLength.ToString()))
                .GroupBy(r => new { r.BarDiameter, r.BarLength, r.Source })
                .Select(g => new Data
                {
                    Source = g.Key.Source,
                    BarDiameter = g.Key.BarDiameter,
                    BarLength = g.Key.BarLength,
                    NoOfBars = g.Count(),
                    TotalBarLength = g.Sum(x => x.TotalBarLength),
                    Weight = "-",     // optional: average or total weight
                    WeightTon = "-",
                    Date = "-",
                    Label = "-"
                })
                .OrderBy(r => r.Source)
                .ThenBy(r => r.BarDiameter)
                .ThenBy(r => r.BarLength)
                .ToList();

            return grouped;
        }
        private static void ExportRebarSummaryToExcel(List<Data> rebarDataList, string savePath)
        {
            if (rebarDataList == null || rebarDataList.Count == 0)
            {
                TaskDialog.Show("Export Rebars", "No data to export.");
                return;
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Rebar Summary");

                string[] headers = { "Source Model", "Bar Diameter", "Bar Length", "No. of Bars", "Total Bar Length" };
                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cells[1, i + 1].Value = headers[i];
                    ws.Cells[1, i + 1].Style.Font.Bold = true;
                }

                int row = 2;
                foreach (var item in rebarDataList)
                {
                    ws.Cells[row, 1].Value = item.Source;
                    ws.Cells[row, 2].Value = item.BarDiameter;
                    ws.Cells[row, 3].Value = item.BarLength;
                    ws.Cells[row, 4].Value = item.NoOfBars;
                    ws.Cells[row, 5].Value = item.TotalBarLength;
                    row++;
                }

                ws.Cells.AutoFitColumns();

                package.SaveAs(new FileInfo(savePath));
            }
        }
        private string ExtractValue(string input, string pattern)
        {
            var match = Regex.Match(input, pattern);
            return match.Success ? match.Groups[1].Value.Trim() : "N/A";
        }
    }

}

