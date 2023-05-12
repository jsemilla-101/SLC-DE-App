using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4.Forms
{
    public partial class ucSLCDB : UserControl
    {
        public ucSLCDB()
        {
            InitializeComponent();
        }

        private void dashboardViewer1_Load(object sender, EventArgs e)
        {
            this.dashboardViewer1.DashboardSource = "C:/Dashboards/SLC_DB.xml";
        }
    }
}
