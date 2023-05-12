using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;


namespace SLC_DE_App
{
    public partial class xfrmShowCallHistory : XtraForm
    {
        public string custCode;
        public Int64 slcid;
        Int64 maxi, maxlatest;

        DateTime oldValStart, newValStart;
        DateTime oldValStop, newValStop;

        string oldValStatus, newValStatus;
        string oldValRemarks, newValRemarks;

        DateTime start, stop;
            
        string status, remarks;

        public xfrmShowCallHistory()
        {
            InitializeComponent();
        }

        private void xfrmShowCallHistory_Load(object sender, EventArgs e)
        {
            this.tblslctaskstatusTableAdapter.Fill(this.sLCDBDataSet.tblslctaskstatus);
            this.tbluserTableAdapter.Fill(this.sLCDBDataSet.tbluser);
            this.getaskhistoryTableAdapter.Fill_SPGetTaskHistory(this.sLCDBDataSet.getaskhistory, custCode);
            this.tblslcrecordstaskhistoryeditsTableAdapter1.Fill(this.sLCDBDataSet.tblslcrecordstaskhistoryedits);

            using (SLCDBDataSetTableAdapters.tblslcrecordstaskhistoryTableAdapter adap = new SLCDBDataSetTableAdapters.tblslcrecordstaskhistoryTableAdapter())
            {
                maxlatest = Convert.ToInt64(Functions.ReplaceNull(adap.ScalarQuery_GetMaxByCustCode(custCode)));
            }
        }

        private void ClearInitialString()
        {
            start = DateTime.Now;
            stop = DateTime.Now;
            status = String.Empty;
            remarks = String.Empty;
        }

        private void repdteditTaskStart_QueryPopUp(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }


        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            {
                GridView view = sender as GridView;
                GridViewInfo viewInfo = view.GetViewInfo() as GridViewInfo;
                
                if (view.FocusedRowHandle == viewInfo.RowsInfo.Last().RowHandle)
                {
                    e.Cancel = false; // show editor if focused row is the new item row
                }

                else
                {
                    if (view.FocusedColumn.FieldName == "slctaskstart" || view.FocusedColumn.FieldName == "slctaskend" ||
                          view.FocusedColumn.FieldName == "slctaskstatus" || view.FocusedColumn.FieldName == "slctaskremarks" ||
                          view.FocusedColumn.FieldName == "slctaskduration")
                    {
                        e.Cancel = true; // do not show editor when the focused column is either A or B}
                    }
                }
            }
        }

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            maxi = Convert.ToInt64(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "slctaskhistid"));
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
       
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void repdteditTaskEnd_QueryPopUp(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
        
        private void repdteditTaskStart_EditValueChanged(object sender, EventArgs e)
        {
            DateEdit dateeditStart = (sender as DateEdit) ;
            oldValStart = Convert.ToDateTime(dateeditStart.OldEditValue);
            newValStart = Convert.ToDateTime(dateeditStart.EditValue);
        }

        private void repdteditTaskEnd_EditValueChanged(object sender, EventArgs e)
        {
            DateEdit dateeditStop = (sender as DateEdit);
            oldValStop = Convert.ToDateTime(dateeditStop.OldEditValue);
            newValStop = Convert.ToDateTime(dateeditStop.EditValue);
        }
        private void replookupStatus_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit lookUpEditStatus = (sender as LookUpEdit);
            oldValStatus = Convert.ToString(lookUpEditStatus.OldEditValue);
            newValStatus = Convert.ToString(lookUpEditStatus.Text);
        }

        private void reptxtRemarks_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit texteditRemarks = (sender as TextEdit);
            oldValRemarks = Convert.ToString(texteditRemarks.OldEditValue);
            newValRemarks = Convert.ToString(texteditRemarks.EditValue);
        }
                
        void SaveInsert()
        {
            if (oldValStart != newValStart)
            {
                start = newValStart;
                using (SLCDBDataSetTableAdapters.tblslcrecordstaskhistoryeditsTableAdapter adap = new SLCDBDataSetTableAdapters.tblslcrecordstaskhistoryeditsTableAdapter())
                {
                    Int64 maxeditno;
                    maxeditno = Convert.ToInt64(Functions.ReplaceNull(adap.ScalarQuery_GetMaxEditNo())) + 1;
                    adap.Insert(maxeditno, maxlatest, Globals.userempid, DateTime.Now, "TASK START", Convert.ToString(oldValStart),
                        Convert.ToString(newValStart), gtxtEditReason.Text);
                }
            }

            else { start = oldValStart; }

            if (oldValStop != newValStop)
            {
                stop = newValStop;
                using (SLCDBDataSetTableAdapters.tblslcrecordstaskhistoryeditsTableAdapter adap = new SLCDBDataSetTableAdapters.tblslcrecordstaskhistoryeditsTableAdapter())
                {
                    Int64 maxeditno;
                    maxeditno = Convert.ToInt64(Functions.ReplaceNull(adap.ScalarQuery_GetMaxEditNo())) + 1;
                    adap.Insert(maxeditno, maxlatest, Globals.userempid, DateTime.Now, "TASK STOP", Convert.ToString(oldValStop),
                        Convert.ToString(newValStop), gtxtEditReason.Text);
                }
            }
            else { stop = oldValStop; }

            if (oldValStatus != newValStatus)
            {
                status = newValStatus;
                using (SLCDBDataSetTableAdapters.tblslcrecordstaskhistoryeditsTableAdapter adap = new SLCDBDataSetTableAdapters.tblslcrecordstaskhistoryeditsTableAdapter())
                {
                    Int64 maxeditno;
                    maxeditno = Convert.ToInt64(Functions.ReplaceNull(adap.ScalarQuery_GetMaxEditNo())) + 1;
                    adap.Insert(maxeditno, maxlatest, Globals.userempid, DateTime.Now, "STATUS", oldValStatus, newValStatus, gtxtEditReason.Text);
                }
            }
            else {status = oldValStatus; }

            if (oldValRemarks != newValRemarks)
            {
                remarks = newValRemarks;
                using (SLCDBDataSetTableAdapters.tblslcrecordstaskhistoryeditsTableAdapter adap = new SLCDBDataSetTableAdapters.tblslcrecordstaskhistoryeditsTableAdapter())
                {
                    Int64 maxeditno;
                    maxeditno = Convert.ToInt64(Functions.ReplaceNull(adap.ScalarQuery_GetMaxEditNo())) + 1;
                    adap.Insert(maxeditno, maxlatest, Globals.userempid, DateTime.Now, "REMARKS", oldValRemarks, newValRemarks, gtxtEditReason.Text);
                }
            }
            else { remarks = oldValRemarks; }
        }
        public static bool checkcont(string s)
        {
            return (s == null || s == String.Empty) ? true : false;
        }
        
        private void gbuttonUpdate_Click(object sender, EventArgs e)
        {
            SaveInsert();

            if (start == Convert.ToDateTime("01-Jan-0001 12:00:00 AM"))
            { start = Convert.ToDateTime(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "slctaskstart")); }

            if (stop == Convert.ToDateTime("01-Jan-0001 12:00:00 AM"))
            { stop = Convert.ToDateTime(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "slctaskend")); }

            if (checkcont(status) == true)
            { status = Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "slctaskstatus")); }

            if (checkcont(remarks) == true)
            { remarks = Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "slctaskremarks")); }


            using (SLCDBDataSetTableAdapters.tblslcrecordstaskhistoryTableAdapter adap = new SLCDBDataSetTableAdapters.tblslcrecordstaskhistoryTableAdapter())
            {
                adap.UpdateQuery_UpdateTimeStatusRemarks(start, stop, Functions.NullorEmpty(status), Functions.NullorEmpty(remarks), maxlatest);
            }

            using (SLCDBDataSetTableAdapters.tblslcrecordsTableAdapter adap = new SLCDBDataSetTableAdapters.tblslcrecordsTableAdapter())
            {
               adap.UpdateQuery_UpdateStatusRemarks(Functions.NullorEmpty(status), Functions.NullorEmpty(remarks), custCode);
            }

            XtraMessageBox.Show("Call and Task History successfully edited", "Edit Update Call Task", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            ClearInitialString();
            gridControl1.RefreshDataSource();
        }
    }
}