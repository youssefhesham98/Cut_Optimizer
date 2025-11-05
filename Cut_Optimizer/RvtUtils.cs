using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        /// <summary>
        /// Collects rebar data from the main document and linked documents,
        /// </summary>
        /// <param name="doc"></param>
        public static void Collector(Document doc, List<string> selectedDates)
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

                    ExportRebarSummaryToExcel(aggregated,selectedDates, excelPath);

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
        /// Gets rebar data from a given document.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="sourceLabel"></param>
        /// <returns></returns>
        private static List<Data> GetRebarsFromDocument(Document doc, string sourceLabel)
        {
            List<Data> list = new List<Data>();

            FilteredElementCollector rebarCollector = new FilteredElementCollector(doc)
                .OfClass(typeof(Rebar))
                .WhereElementIsNotElementType();

            foreach (Rebar rebar in rebarCollector)
            {
                var dia = Math.Ceiling(UnitUtils.ConvertFromInternalUnits(double.Parse(GetParameterValue(rebar, "Bar Diameter")),UnitTypeId.Millimeters));
                var len = Math.Ceiling(UnitUtils.ConvertFromInternalUnits(double.Parse(GetParameterValue(rebar, "Bar Length")),UnitTypeId.Millimeters));

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
                    TotalBarLength = Math.Ceiling(totalLen),
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
        /// Gets the string representation of a parameter's value.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
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
        /// <summary>
        /// aggregates rebar data by diameter and length.
        /// </summary>
        /// <param name="allRebars"></param>
        /// <returns></returns>
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
        private static void ExportRebarSummaryToExcel(List<Data> rebarDataList,List<string> selectedDates, string savePath)
        {
            if (rebarDataList == null || rebarDataList.Count == 0)
            {
                TaskDialog.Show("Export Rebars", "No data to export.");
                return;
            }

            if (selectedDates == null || selectedDates.Count == 0)
            {
                TaskDialog.Show("Export Rebars", "No date range provided for export.");
                return;
            }

            // Parse and sort the date range safely (input format dd/MM/yyyy → output MM/dd/yyyy)
            var parsedDates = selectedDates
                .Select(d =>
                {
                    DateTime.TryParseExact(d, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed);
                    return parsed;
                })
                .Where(dt => dt != DateTime.MinValue)
                .OrderBy(dt => dt)
                .Select(d => d.ToString("mm/dd/yyyy"))
                .ToList();

            if (parsedDates.Count == 0)
            {
                TaskDialog.Show("Export Rebars", "No valid dates found in the provided list.");
                return;
            }

            // Convert parsed dates back to MM/dd/yyyy for display
            //string fromDate = parsedDates.First().ToString("MM/dd/yyyy");
            //string toDate = parsedDates.Last().ToString("MM/dd/yyyy");

            //// Filter the rebar data by comparing parsed DateTime values
            //var filteredData = rebarDataList
            //    .Where(d =>
            //    {
            //        if (DateTime.TryParseExact(d.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime rebarDate) ||
            //            DateTime.TryParseExact(d.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out rebarDate))
            //        {
            //            return rebarDate >= fromDate && rebarDate <= toDate;
            //        }
            //        return false;
            //    })
            //    .ToList();

            // Filter the data list by selected dates
            var filteredData = rebarDataList
                .Where(d => parsedDates.Contains(d.Date))
                .ToList();

            if (filteredData.Count == 0)
            {
                TaskDialog.Show("Export Rebars", "No rebar data found for the selected dates.");
                return;
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Rebar Summary");

                // --- Add Export Date Range Info ---
                ws.Cells[1, 1].Value = "Export From:";
                ws.Cells[1, 2].Value = selectedDates.First();
                ws.Cells[2, 1].Value = "Export To:";
                ws.Cells[2, 2].Value = selectedDates.Last();

                string[] headers = { "Source Model","Rebar ID", "Bar Diameter", "Bar Length", "Total Bar Length", "No. of Bars","Weight","Weight (ton)","Date","Rebar Label"};
                int headerRow = 4; // Start headers below the export info
                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cells[headerRow, i + 1].Value = headers[i];
                    ws.Cells[headerRow, i + 1].Style.Font.Bold = true;
                    ws.Cells[headerRow, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[headerRow, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                int row = headerRow + 1;
                foreach (var item in filteredData)
                {
                    ws.Cells[row, 1].Value = item.Source;
                    ws.Cells[row, 2].Value = item.RebarId;
                    ws.Cells[row, 3].Value = item.BarDiameter;
                    ws.Cells[row, 4].Value = item.BarLength;
                    ws.Cells[row, 5].Value = item.TotalBarLength;
                    ws.Cells[row, 6].Value = item.NoOfBars;
                    ws.Cells[row, 7].Value = item.Weight;
                    ws.Cells[row, 8].Value = item.WeightTon;
                    ws.Cells[row, 9].Value = item.Date;
                    ws.Cells[row, 10].Value = item.Label;
                    row++;
                }

                ws.Cells.AutoFitColumns();

                package.SaveAs(new FileInfo(savePath));

                // Automatically open the Excel file after saving
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = savePath,
                        UseShellExecute = true // required to open with default app (Excel)
                    });
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Export Rebars", $"File saved but could not be opened automatically.\n{ex.Message}");
                }
            }
        }
        private string ExtractValue(string input, string pattern)
        {
            var match = Regex.Match(input, pattern);
            return match.Success ? match.Groups[1].Value.Trim() : "N/A";
        }
    }

}

