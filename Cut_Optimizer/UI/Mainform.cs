using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Cut_Optimizer.ExEvt;

namespace Cut_Optimizer.UI
{
    public partial class Mainform : Form
    {
        public static List<string> selectedDates { get; set; }
        public static string fromDate { get; set; }
        public static string toDate { get; set; }
        public Mainform()
        {
            InitializeComponent();
            fromdate.Format = DateTimePickerFormat.Custom;
            fromdate.CustomFormat = "dd/MM/yyyy";
            todate.Format = DateTimePickerFormat.Custom;
            todate.CustomFormat = "dd/MM/yyyy";
        }

        private void test_Click(object sender, EventArgs e)
        {
            DateTime startDate = fromdate.Value.Date;
            DateTime endDate = todate.Value.Date;
            fromDate = fromdate.Value.ToString("dd/MM/yyyy");
            toDate = todate.Value.ToString("dd/MM/yyyy");
            if (startDate > endDate)
            {
                TaskDialog.Show("Error", "Start date cannot be after end date!");
                return;
            }
            selectedDates = GetDateRangeAsStrings(startDate, endDate);
            string allDates = string.Join("\n", selectedDates);
            ExCmd.exevt.request = Request.Test;
            ExCmd.exevthan.Raise();

            // Example:
            // var filteredItems = allItems.Where(x => x.Date >= startDate && x.Date <= endDate).ToList();
        }

        private void fromdate_ValueChanged(object sender, EventArgs e)
        {
            todate.MinDate = fromdate.Value;
        }
        private List<string> GetDateRangeAsStrings(DateTime startDate, DateTime endDate)
        {
            List<string> dates = new List<string>();

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                dates.Add(date.ToString("MM/dd/yyyy")); // Format to mm/dd/yyyy
            }

            return dates;
        }
    }
}
