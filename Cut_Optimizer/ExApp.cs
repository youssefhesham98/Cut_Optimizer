using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Cut_Optimizer
{
    public class ExApp : IExternalApplication
    {
        public UIControlledApplication uicapp { get; set; }
        public Result OnShutdown(UIControlledApplication uicapp)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication uicapp)
        {
            // Attach once here
            //uicapp.ControlledApplication.DocumentChanged += DeletedPipeTracker.OnDocumentChanged;

            this.uicapp = uicapp;
            CreateBushButton();
            return Result.Succeeded;

            //LicenseCheck check = new LicenseCheck();
            //var begin = check.Check();
            //if (begin)
            //{
            //    try
            //    {
            //        this.uicapp = uicapp;
            //        CreateBushButton();
            //    }
            //    catch (Exception ex)
            //    {
            //        var message = ex.Message;
            //        TaskDialog.Show("Error", message);
            //    }
            //}
            //return Result.Succeeded;
        }
        private void CreateBushButton()
        {
            try
            {
                var tab_name = "EDECS TOOLKIT";
                var pnl_name = "Value Engineering";
                var btn_name = "Cut Optimizer";
                try
                {
                    uicapp.CreateRibbonTab(tab_name);
                }
                catch (Exception ex) { /*TaskDialog.Show("Failed", ex.Message.ToString());*/ }
                List<RibbonPanel> panels = uicapp.GetRibbonPanels(tab_name);
                RibbonPanel panel = panels.FirstOrDefault(p => p.Name == pnl_name);
                if (panel == null)
                {
                    panel = uicapp.CreateRibbonPanel(tab_name, pnl_name);
                }
                Assembly assembly = Assembly.GetExecutingAssembly();
                // @"Directory\XXXX.dll"
                PushButtonData pd_data = new PushButtonData(btn_name, btn_name, assembly.Location, "Cut_Optimizer.ExCmd");
                PushButton pb = panel.AddItem(pd_data) as PushButton;
                if (pb != null)
                {
                    pb.ToolTip = "Description.";
                    //pb.LargeImage = new BitmapImage(new Uri($@"{Path.GetDirectoryName(assembly.Location)}\pb.png"));
                    //Stream stream = assembly.GetManifestResourceStream("Naming_Convention_Tester.bin.Resources.pb.png");
                    //PngBitmapDecoder decoder = new PngBitmapDecoder(stream , BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    //pb.LargeImage = decoder.Frames[0];
                    pb.LargeImage = GetImageSource("Directory.ICON_NAME96.png");
                }
            }
            catch (Exception ex) { /*TaskDialog.Show("Failed", ex.Message.ToString());*/ }
        }
        private ImageSource GetImageSource(string ImageFullname)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ImageFullname);
            PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            // use the extension related to the image extension like PngBitmapDecoder for PNG Image
            return decoder.Frames[0];
        }
    }
}
