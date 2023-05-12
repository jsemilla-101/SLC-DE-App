using DevExpress.LookAndFeel;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.XtraSplashScreen;
using System;
using System.Windows.Forms;

namespace SLC_DE_App
{
    public partial class rbnMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Timer t = null;

        public rbnMain()
        {
            InitializeComponent();
            UserLookAndFeel.Default.SetSkinStyle(SkinStyle.WXICompact);
            UserLookAndFeel.Default.SetSkinStyle(SkinSvgPalette.WXICompact.Sharpness);
        }
        private void StartTimer()
        {
            t = new System.Windows.Forms.Timer();
            t.Interval = 1000;
            t.Tick += new EventHandler(t_Tick);
            t.Enabled = true;
        }

        void t_Tick(object sender, EventArgs e)
        {
            //gbtn.Text = DateTime.Now.ToString();
        }

        private void ReloadData()
        {
            //using (var handle = SplashScreenManager.ShowOverlayForm(gridControl1))
            //{
            //    handle.QueueFocus(IntPtr.Zero);
            //
            //}
            this.tblslcrecordsTableAdapter.Fill(this.sLCDBDataSet.tblslcrecords);
        }
        private void rbnMain_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'sLCDBDataSet.tbluser' table. You can move, or remove it, as needed.
            this.tbluserTableAdapter.Fill(this.sLCDBDataSet.tbluser);
            ReloadData();
        }
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (rbnUploadData frm = new rbnUploadData())
            {
                frm.ShowInTaskbar = false;
                frm.ShowIcon = false;
                frm.ShowDialog();
            }
            ReloadData();
        }
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            ReloadData();
        }
        private void rbnMain_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            this.Dispose();
        }
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView1.RowCount >= 5000)
            {
                if (XtraMessageBox.Show("Large data displayed on the grid, Print preview will take longer than expected." 
                    + "\n" + "\n" 
                    + "Do you want to continue?", "SLC DE Main", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    PrintPreview();
                }
            }
            else
            {
                PrintPreview();
            }

        }
        public void PrintPreview()
        {
            using (var handle = SplashScreenManager.ShowOverlayForm(gridControl1))
            {
                string DefaultPrinterName;
                System.Drawing.Printing.PrinterSettings oPS = new System.Drawing.Printing.PrinterSettings();
                System.Drawing.Printing.Margins mg = new System.Drawing.Printing.Margins(Convert.ToInt16(0.4), Convert.ToInt16(0.4), Convert.ToInt16(0.4), Convert.ToInt16(0.4));
                try
                {
                    DefaultPrinterName = oPS.PrinterName;
                }
                catch (Exception ex)
                {
                    DefaultPrinterName = "";
                    XtraMessageBox.Show(ex.Message.ToString());
                }
                finally
                {
                    oPS = null;
                }

                PrintableComponentLink PrintableComponentLink1 = new PrintableComponentLink(new PrintingSystem()) { Component = gridControl1, Landscape = true, Margins = mg, PaperKind = System.Drawing.Printing.PaperKind.Legal };
                PrintableComponentLink1.CreateDocument();
                PrintableComponentLink1.ShowPreview();
            }
        }

        private void barbtnclearfilter_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridView1.ActiveFilterString = String.Empty;
        }

        private void barbtncleargroup_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridView1.ClearGrouping();
        }

        private void barbtnupdaterecord_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (xfrmUpdateTask frm = new xfrmUpdateTask())
            {
                frm.custCode = Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, colslc_customercode));
                frm.taskStatus = Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, colslc_status));
                frm.ShowInTaskbar = false;
                frm.ShowIcon = false;
                frm.ShowDialog();
            }
            ReloadData();
        }

        private void barbtnTaskHistory_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (xfrmShowCallHistory frm = new xfrmShowCallHistory())
            {
                frm.custCode = Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, colslc_customercode));
                frm.slcid = Convert.ToInt64(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, colslc_id));

                frm.ShowInTaskbar = false;
                frm.ShowIcon = false;
                frm.ShowDialog();
            }
            ReloadData();
        }

        private void barbtntaskstatuslist_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            using (xfrmStatusList frm = new xfrmStatusList())
            {
                frm.ShowInTaskbar = false;
                frm.ShowIcon = false;
                frm.ShowDialog();
            }
        }

        private void rbnMain_FormClosed(object sender, FormClosedEventArgs e)
        {
           // this.Dispose();
        }

        private void officeNavigationBar1_Click(object sender, EventArgs e)
        {

        }

        private void navigationPageTask_Paint(object sender, PaintEventArgs e)
        {

        }

        private void navigationPageReports_Paint(object sender, PaintEventArgs e)
        {

        }

        private void barStaticItem1_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void skinDropDownButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barbtndistribute_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (rbnDistribute frm = new rbnDistribute())
            {
                
                frm.ShowInTaskbar = false;
                frm.ShowIcon = false;
                frm.ShowDialog();
            }
            ReloadData();
        }
    }
}