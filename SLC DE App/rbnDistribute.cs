using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace SLC_DE_App
{
    public partial class rbnDistribute : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private Int32 totalrowupdated = 0;

        public rbnDistribute()
        {
            InitializeComponent();
        }

        private void rbnDistribute_Load(object sender, EventArgs e)
        {
            this.tbluserTableAdapter.FillBy_ReportingTo(this.sLCDBDataSet.tbluser, Globals.userid);
        }
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.getslctopcustomerpendingamountTableAdapter.Fill(this.sLCDBDataSet.getslctopcustomerpendingamount);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
           
           string agemtzohoid = Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, coluserzohoid));

            SetWorkLoads(100, agemtzohoid, totalrowupdated);
        }

        private void SetWorkLoads(int agenttotaltask, string agemtzohoid, int totalrow)
        {
            int indexer = 0;
            Int32 row;

            for (row = totalrow; indexer < agenttotaltask; row++)
            {
                indexer += 1;
                gridView2.SetRowCellValue(row, "slc_assignee", agemtzohoid);
                             
            }

            if (indexer == agenttotaltask)
            {
                totalrowupdated = row;
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                using (SLCDBDataSetTableAdapters.tblslcrecordsTableAdapter adap =
                    new SLCDBDataSetTableAdapters.tblslcrecordsTableAdapter())
                {
                    string custcode;
                    string agentzoho;

                    custcode = Convert.ToString(gridView2.GetRowCellValue(i, colslc_customercode));
                    agentzoho = Convert.ToString(gridView2.GetRowCellValue(i, colslc_assignee));
                    adap.UpdateQuery_UpdateAssignee(agentzoho,custcode);
                    adap.Dispose();
                    MessageBox.Show("Successfully assigned");
                }
            }
        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void gridControl2_Click(object sender, EventArgs e)
        {

        }
    }
}