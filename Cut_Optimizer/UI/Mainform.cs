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
        public Mainform()
        {
            InitializeComponent();
        }

        private void test_Click(object sender, EventArgs e)
        {
            DateTime startDate = fromdate.Value.Date;
            DateTime endDate = todate.Value.Date;
            if (startDate > endDate)
            {
                TaskDialog.Show("Error", "Start date cannot be after end date!");
                return;
            }
            selectedDates = GetDateRangeAsStrings(startDate, endDate);
            ExCmd.exevt.request = Request.Test;
            ExCmd.exevthan.Raise();

            string allDates = string.Join("\n", selectedDates);
            //TaskDialog.Show("Date",allDates);

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
