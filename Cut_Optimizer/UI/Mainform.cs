using Autodesk.Revit.UI;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Cut_Optimizer.ExEvt;
using TextBox = System.Windows.Forms.TextBox;

namespace Cut_Optimizer.UI
{
    public partial class Mainform : Form
    {
        public static List<string> selectedDates { get; set; }
        public static string fromDate { get; set; }
        public static string toDate { get; set; }
        public static string directory { get; set; }
        public static string ttle { get; set; }
        public static string excel { get; set; }
        public Mainform()
        {
            InitializeComponent();
            InitializePlaceholders();
            #region Configuration
            // Set alignment (center or left)
            title.TextAlign = HorizontalAlignment.Center;
            exportpath.TextAlign = HorizontalAlignment.Center;

            title.ShortcutsEnabled = true;
            exportpath.ShortcutsEnabled = true;

            title.ReadOnly = false;
            exportpath.ReadOnly = false;

            title.Enabled = true;
            exportpath.Enabled = true;

            // If you have custom ContextMenuStrip that removed default menu, reset it:
            title.ContextMenuStrip = null;
            exportpath.ContextMenuStrip = null;

            //fromdate.Format = DateTimePickerFormat.Custom;
            //fromdate.CustomFormat = "dd/MM/yyyy";
            //todate.Format = DateTimePickerFormat.Custom;
            //todate.CustomFormat = "dd/MM/yyyy";
            #endregion
            // Hook Events
            title.Enter += TextBox_Enter;
            title.Leave += TextBox_Leave;
            exportpath.Enter += TextBox_Enter;
            exportpath.Leave += TextBox_Leave;
            title.KeyDown += TextBox_GlobalKeyDown;
            export.KeyDown += TextBox_GlobalKeyDown;
        }
        // EVENTS
        private void TextBox_GlobalKeyDown(object sender, KeyEventArgs e)
        {

        }
        private void InitializePlaceholders()
        {
            SetPlaceholder(title, "Export Title");
            SetPlaceholder(exportpath, "Export Directory");
        }
        private void SetPlaceholder(TextBox tb, string placeholder)
        {
            if (string.IsNullOrEmpty(tb.Text) || tb.Text == tb.Tag as string)
            {
                tb.ForeColor = Color.SeaGreen;
                tb.Font = new Font(tb.Font, FontStyle.Italic);
                tb.Text = placeholder;
                tb.Tag = placeholder; // remember the placeholder text
            }
        }
        private void RemovePlaceholder(TextBox tb)
        {
            if (tb.Text == tb.Tag as string)
            {
                tb.Text = "";
                tb.ForeColor = Color.FromArgb(0, 101, 96);
            }
        }
        private void TextBox_Enter(object sender, EventArgs e)
        {
            RemovePlaceholder((TextBox)sender);
        }
        private void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (string.IsNullOrEmpty(tb.Text))
            {
                SetPlaceholder(tb, tb.Tag as string);
            }
        }
        // EVENTS
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

        private void export_Click(object sender, EventArgs e)
        {
            if (exportbrws.ShowDialog() == DialogResult.OK)
            {
                exportpath.Text = exportbrws.SelectedPath;
            }

            string Title = title.Text.Trim();
            string folder = exportpath.Text.Trim();

            if (string.IsNullOrEmpty(Title))
            {
                TaskDialog.Show("Error", "Please enter a document title.");
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                TaskDialog.Show("Error", "Please enter a path.");
                return;
            }
            directory = folder;
            ttle = Title;
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
            ExCmd.exevt.request = Request.Report;
            ExCmd.exevthan.Raise();

            // Example:
            // var filteredItems = allItems.Where(x => x.Date >= startDate && x.Date <= endDate).ToList();
        }

        private void exportpath_TextChanged(object sender, EventArgs e)
        {

        }

        private void title_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (tb == null) return;

            int selStart = tb.SelectionStart; // remember cursor position

            // Keep only letters, dash, underscore
            string filtered = new string(tb.Text
                .Where(c => char.IsLetter(c) || c == '-' || c == '_' || c == ' ')
                .ToArray());

            if (tb.Text != filtered)
            {
                tb.Text = filtered;
                tb.SelectionStart = Math.Min(selStart, tb.Text.Length); // restore cursor position
            }
        }

        private void cls_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lnkd_Click(object sender, EventArgs e)
        {
            string url = @"https://www.linkedin.com/in/youssef-hesham/";

            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    // Open the URL in the default browser
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true // Required to use the default browser
                    });

                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", $"Error opening the URL: {ex.Message}");
                }
            }
            else
            {
                TaskDialog.Show("Error", "No URL entered.");
            }
        }

        private void edecs_Click(object sender, EventArgs e)
        {
            string url = @"https://www.edecs.com/";

            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    // Open the URL in the default browser
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true // Required to use the default browser
                    });

                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", $"Error opening the URL: {ex.Message}");
                }
            }
            else
            {
                TaskDialog.Show("Error", "No URL entered.");
            }
        }

        private void assign_Click(object sender, EventArgs e)
        {
            excelfile.Title = "Select Rebar Activity Schedule";
            excelfile.Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
            excelfile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (excelfile.ShowDialog() != DialogResult.OK)
            {
                TaskDialog.Show("Cancelled", "Operation cancelled. No Excel file selected.");
                return;
            }
            string excelPath = excelfile.FileName;
            excel = excelPath;

            ExCmd.exevt.request = Request.Assign;
            ExCmd.exevthan.Raise();
        }
    }
}
