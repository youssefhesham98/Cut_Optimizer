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
        public Mainform()
        {
            InitializeComponent();
        }

        private void test_Click(object sender, EventArgs e)
        {
            ExCmd.exevt.request = Request.Test;
            ExCmd.exevthan.Raise();
         
        }
    }
}
