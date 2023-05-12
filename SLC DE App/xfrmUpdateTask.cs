using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SLC_DE_App
{
    public partial class xfrmUpdateTask : DevExpress.XtraEditors.XtraForm
    {
        public string custCode;
        public string taskStatus;

        Timer t = null;
        bool isStarted = false;
        public xfrmUpdateTask()
        {
            InitializeComponent();
        }

        private void xfrmUpdateTask_Load(object sender, EventArgs e)
        {
            this.tblslcrecordsTableAdapter.FillBy_CustomerCode(this.sLCDBDataSet.tblslcrecords, custCode);
            ReloadTaskStatus();
            gcomboxTaskStatus.SelectedIndex = -1;
            if (taskStatus == "Closed Account")
            {
                gbtnSave.Enabled = false;
                gcomboxTaskStatus.Text = taskStatus;
                gcomboxTaskStatus.Enabled = false;
                gtxtmultiRemarks.Enabled = false;
                gbtnTaskStart.Enabled = false;
                gbtnTaskStop.Enabled = false;
                windowsUIButtonPanel1.Enabled = false;
            }
        }

        private void ReloadTaskStatus()
        {
            this.tblslctaskstatusTableAdapter.Fill(this.sLCDBDataSet.tblslctaskstatus);
        }
         
        void t_Tick(object sender, EventArgs e)
        {

            //gbtnTaskStart.Text = "Task Started" + "\n" + DateTime.Now.ToString("hh:mm:ss tt");           
            gbtnTaskStop.Text = "Stop" + "\n" + DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void StartTimer()
        {
            t = new Timer();
            t.Interval = 1000;
            t.Tick += new EventHandler(t_Tick);
            t.Enabled = true;
        }
        private void gbtnTaskStart_Click(object sender, EventArgs e)
        {   
          
            txtTaskStart.Text = DateTime.Now.ToString();
            gbtnTaskStart.Text = "Task Started" + "\n" + DateTime.Now.ToString("hh:mm:ss tt");

            StartTimer();

            isStarted = true;
            gbtnTaskStart.Enabled = false;
        }
        private void gbtnTaskStop_Click(object sender, EventArgs e)
        {
           
            t.Stop();
            txtTaskEnd.EditValue = DateTime.Now.ToString();
          
            gbtnTaskStop.Text = "Task Stopped" + "\n" + DateTime.Now.ToString("hh:mm:ss tt");
            isStarted = false;
            gbtnTaskStop.Enabled = false;
        }
             
        private void gcomboxtaskstatus_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                gcomboxTaskStatus.SelectedIndex = -1;
            }
        }

        private void layoutControlItem4_DoubleClick(object sender, EventArgs e)
        {
            using (xfrmStatusList frm = new xfrmStatusList())
            {
                frm.ShowInTaskbar = false;
                frm.ShowIcon = false;
                frm.ShowDialog();
            }

            ReloadTaskStatus();
        }

        
        private void xfrmUpdateTask_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isStarted == true)
            {
                XtraMessageBox.Show("Task was already started please." + "\n \n" + "Please click Stop and Update button before closing", "Update Task", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
        }

        private void windowsUIbtnResetTime_Click(object sender, EventArgs e)
        {

        }

        private void gbtnSave_Click(object sender, EventArgs e)
        {

            if (txtTaskStart.Text == String.Empty && txtTaskEnd.Text == String.Empty)
            {
                XtraMessageBox.Show("Please click Task Start button", "Update Task", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtTaskEnd.Text == String.Empty)
            {
                XtraMessageBox.Show("Please click Stop button", "Update Task", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (gcomboxTaskStatus.SelectedIndex < 0)
            {
                XtraMessageBox.Show("Please Select Task Status", "Update Task", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            using (SLCDBDataSetTableAdapters.tblslcrecordsTableAdapter adap = new SLCDBDataSetTableAdapters.tblslcrecordsTableAdapter())
            {
                adap.UpdateQuery_TaskUpdate(gcomboxTaskStatus.Text, gtxtmultiRemarks.Text, Globals.userempid, DateTime.Parse(txtTaskEnd.Text), DateTime.Now, custCode);
            }

            using (SLCDBDataSetTableAdapters.tblslcrecordstaskhistoryTableAdapter adap = new SLCDBDataSetTableAdapters.tblslcrecordstaskhistoryTableAdapter())
            {
                Int64 maxtaskhistid;
                maxtaskhistid = Convert.ToInt64(Functions.ReplaceNull(adap.ScalarQuery_GetMaxTaskHistID())) + 1;
                adap.Insert(maxtaskhistid, custCode, DateTime.Parse(txtTaskStart.Text), DateTime.Parse(txtTaskEnd.Text),gcomboxTaskStatus.Text, gtxtmultiRemarks.Text, Globals.userempid);
            }

            XtraMessageBox.Show("Successfully Updated", "Update Task", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            ResetTimeButton();
            clear();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //lblTime.Text = DateTime.Now.ToString("hh:mm:ss");
        }
        private void ResetTimeButton()
        {
            gbtnTaskStart.Enabled = true;
            gbtnTaskStart.Text = "Task Start";
            txtTaskStart.Text = String.Empty;

            gbtnTaskStop.Enabled = true;
            gbtnTaskStop.Text = "Task Stop";
            txtTaskEnd.Text = String.Empty;

            isStarted = false;
        }

        private void windowsUIButtonPanel1_Click(object sender, EventArgs e)
        {
            ResetTimeButton();
        }

        private void clear()
        {
            ResetTimeButton();
            gcomboxTaskStatus.Text = String.Empty;
            gtxtmultiRemarks.Text = String.Empty;
        }
    }
}