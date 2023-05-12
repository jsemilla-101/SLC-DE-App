using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace SLC_DE_App
{
    public partial class xfrmStatusList : DevExpress.XtraEditors.XtraForm
    {
        public xfrmStatusList()
        {
            InitializeComponent();
        }

        private void xfrmStatusList_Load(object sender, EventArgs e)
        {
            this.tblslctaskstatusTableAdapter.Fill(this.sLCDBDataSet.tblslctaskstatus);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2Button1.Text == "ADD")
            {
                CreatNewRow();
            }
            else
            {
                using (SLCDBDataSetTableAdapters.tblslctaskstatusTableAdapter adapCompet =
                                 new SLCDBDataSetTableAdapters.tblslctaskstatusTableAdapter())
                {
                    adapCompet.Update(this.sLCDBDataSet.tblslctaskstatus);
                }
                XtraMessageBox.Show("Status successfully Added", "Status List", MessageBoxButtons.OK, MessageBoxIcon.Information);
                gridControl1.RefreshDataSource();
                guna2Button1.Text = "ADD";
            }
        }

        private void CreatNewRow()
        {
            gridView1.AddNewRow();

            int rowHandle = gridView1.GetRowHandle(gridView1.DataRowCount);
            if (gridView1.IsNewItemRow(rowHandle))
            {
                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[1], DBNull.Value);
                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[2], Globals.userempid);
                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[3], DateTime.Now);
                gridView1.SetRowCellValue(rowHandle, gridView1.Columns[4], true);
            }
            guna2Button1.Text = "SAVE";
        }

        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            {
                GridView view = sender as GridView;
                if (view.FocusedRowHandle == GridControl.NewItemRowHandle)
                {
                    e.Cancel = false; // show editor if focused row is the new item row
                }
                else
                {
                    if (view.FocusedColumn.FieldName == "taskstatname" || view.FocusedColumn.FieldName == "taskstatisactive" ||
                        view.FocusedColumn.FieldName == "taskstatid" || view.FocusedColumn.FieldName == "taskstatdateadded" ||
                        view.FocusedColumn.FieldName == "taskstataddedby")
                    {
                        e.Cancel = true; // do not show editor when the focused column is either A or B
                    }
                }
            }
        }
    }
}