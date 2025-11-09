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
        ///
        /// <summary>
        /// Collects rebar data from the main document and linked documents,
        /// </summary>
        /// <param name="doc"></param>
        public static void Collector(Document doc,string fromdate ,string todate,string title,string path)
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

                    // --- Export to Excel ---
                    //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string excelPath = Path.Combine(path, $"{title}_RebarSummary_{DateTime.Now:yyyyMMdd_HHmm}.xlsx");

                    ExportRebarSummaryToExcel(fromdate,todate,allRebars,excelPath);

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
                ElementId typeId = rebar.GetTypeId();
                Element rebarType = rebar.Document.GetElement(typeId);
                Parameter typeParam = rebarType.LookupParameter("Bar Diameter");
                double diameterr = UnitUtils.ConvertFromInternalUnits(typeParam.AsDouble(), UnitTypeId.Millimeters);
                double len = Math.Ceiling(UnitUtils.ConvertFromInternalUnits(rebar.LookupParameter("Bar Length").AsDouble(),UnitTypeId.Millimeters));
                double totallen = Math.Ceiling(UnitUtils.ConvertFromInternalUnits(rebar.LookupParameter("Total Bar Length").AsDouble(), UnitTypeId.Millimeters));
                double noofbars = Math.Ceiling(totallen / 12000);
                string dateStr = rebar.LookupParameter("Date").AsString();
                string labelStr = rebar.LookupParameter("Rebar Label").AsString();
                double weight = Math.Ceiling((diameterr * diameterr) /162.28 * totallen); // in kg
                double weightton = weight / 1000;// in ton
             
                Data data = new Data
                {
                    Source = sourceLabel,
                    RebarId = rebar.Id.IntegerValue,
                    BarDiameter = diameterr,
                    BarLength = len,
                    BarCuts = Math.Ceiling(totallen/len),
                    TotalBarLength = totallen,
                    NoOfBars = noofbars, // We'll calculate grouped totals later
                    Weight = weight,
                    WeightTon = weightton,
                    Date = dateStr,
                    Label = labelStr
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
            //return grouped;
            if (allRebars == null || allRebars.Count == 0)
                return new List<Data>();

            // Group to calculate aggregate values
            var groupSums = allRebars
                .GroupBy(r => new { r.BarDiameter, r.BarLength, r.Source })
                .ToDictionary(
                    g => g.Key,
                    g => new
                    {
                        TotalBarLength = g.Sum(x => x.TotalBarLength),
                        NoOfBars = g.Count()
                    });

            // Update each item with the corresponding aggregated values
            foreach (var rebar in allRebars)
            {
                var key = new { rebar.BarDiameter, rebar.BarLength, rebar.Source };
                if (groupSums.ContainsKey(key))
                {
                    rebar.TotalBarLength = groupSums[key].TotalBarLength;
                    rebar.NoOfBars = groupSums[key].NoOfBars;
                }
            }

            // Return updated list (keeps each item’s own Weight, WeightTon, Date, Label)
            return allRebars
                .OrderBy(r => r.Source)
                .ThenBy(r => r.BarDiameter)
                .ThenBy(r => r.BarLength)
                .ToList();
        }
        private static void ExportRebarSummaryToExcel(string fromdate,string todate,List<Data> rebarDataList,/*List<string> selectedDates,*/ string savePath)
        {
            if (rebarDataList == null || rebarDataList.Count == 0)
            {
                TaskDialog.Show("Export Rebars", "No data to export.");
                return;
            }
         
            var selectedDateSet = GetDateRangeList(fromdate, todate);

            // Filter the data list by selected dates
            var filteredData = rebarDataList
                .Where(d => selectedDateSet.Contains(d.Date.ToString()))
                .OrderBy(d => d.BarDiameter)   
                .ThenBy(d => d.Date)
                .ThenBy(d => d.Label)
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
                ws.Cells[1, 2].Value = int.Parse(selectedDateSet.First());
                ws.Cells[2, 1].Value = "Export To:";
                ws.Cells[2, 2].Value = int.Parse(selectedDateSet.Last());

                string[] headers = { "Source Model","Rebar ID", "Bar Diameter", "Bar Length","No. of Bar Cuts", "Total Bar Length", "No. of Bars","Weight","Weight (ton)","Date","Rebar Label"};
                int headerRow = 4; // Start headers below the export info
                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cells[headerRow, i + 1].Value = headers[i];
                    ws.Cells[headerRow, i + 1].Style.Font.Bold = true;
                    ws.Cells[headerRow, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[headerRow, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                int row = headerRow + 1;

                // Group by Bar Diameter for summary
                var groupedData = filteredData
                    .GroupBy(d => d.BarDiameter)
                    .OrderBy(g => g.Key); // ensure ascending diameter order
                foreach (var group in groupedData)
                {
                    foreach (var item in filteredData)
                    {
                        ws.Cells[row, 1].Value = item.Source;
                        ws.Cells[row, 2].Value = item.RebarId;
                        ws.Cells[row, 3].Value = item.BarDiameter;
                        ws.Cells[row, 4].Value = item.BarLength;
                        ws.Cells[row, 5].Value = item.BarCuts;
                        ws.Cells[row, 6].Value = item.TotalBarLength;
                        ws.Cells[row, 7].Value = item.NoOfBars;
                        ws.Cells[row, 8].Value = item.Weight;
                        ws.Cells[row, 9].Value = item.WeightTon;
                        ws.Cells[row, 10].Value = int.Parse(item.Date);
                        ws.Cells[row, 11].Value = item.Label;
                        row++;
                    }

                    // --- Add summary row for this diameter ---
                    ws.Cells[row, 3].Value = $"{group.Key} mm Total";
                    ws.Cells[row, 3].Style.Font.Bold = true;

                    ws.Cells[row, 5].Value = group.Sum(x => x.TotalBarLength);
                    ws.Cells[row, 6].Value = group.Sum(x => x.NoOfBars);
                    ws.Cells[row, 7].Value = group.Sum(x => x.Weight);
                    ws.Cells[row, 8].Value = group.Sum(x => x.WeightTon);

                    // Format summary row
                    ws.Cells[row, 5, row, 8].Style.Font.Bold = true;
                    ws.Cells[row, 1, row, headers.Length].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[row, 1, row, headers.Length].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);
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
        public static List<string> GetDateRangeList(string fromDateStr, string toDateStr)
        {
            string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd/M/yyyy" };

            if (!DateTime.TryParseExact(fromDateStr, formats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime fromDate) ||
                !DateTime.TryParseExact(toDateStr, formats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime toDate))
            {
                throw new ArgumentException("Invalid date format. Please use dd/MM/yyyy or d/M/yyyy.");
            }

            if (fromDate > toDate)
            {
                (fromDate, toDate) = (toDate, fromDate); // swap if reversed
            }

            List<string> dateList = new List<string>();
            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                dateList.Add(date.ToString("yyyyMMdd"));
            }

            return dateList;
        }
    }

}

