using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace SLC_DE_App
{
    public partial class rbnUploadData : DevExpress.XtraBars.Ribbon.RibbonForm
    {

        string fulldirectory;
        public rbnUploadData()
        {
            InitializeComponent();
        }
        private void barbtnupload_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (CheckGridHasDataLoaded() == true)
                {
                    XtraMessageBox.Show("Grid has data");
                    return;
                }
                else
                {
                    OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
                    {
                        var withBlock = OpenFileDialog1;
                        withBlock.CheckFileExists = true;
                        withBlock.ShowReadOnly = false;

                        withBlock.FilterIndex = 1;
                        if (withBlock.ShowDialog() == DialogResult.OK)
                        {
                            string filename;
                            string filex;

                            fulldirectory = OpenFileDialog1.FileName.ToString();
                            filename = Path.GetFileName(OpenFileDialog1.FileName.ToString());
                            filex = Path.GetExtension(filename);

                            using (var handle = SplashScreenManager.ShowOverlayForm(gridControl1))
                            {
                                gridControl1.DataSource = GetDataTabletFromCSVFile(fulldirectory);
                            }

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString());
                return;
            }
        }


        private bool CheckGridHasDataLoaded()

        {
            if (gridView1.RowCount > 0)
                return true;
            else
                return false;
        }
        private DataTable RenameTableHeader_ForDBSave(DataTable dt)
        {
            //dt = gridView1.GridControl.DataSource as DataTable;

            //change grid view data table column names to data base column names
            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName == "Upload ID") //Serial ID
                {
                    col.ColumnName = "slc_ID";
                }
                else if (col.ColumnName == "CustomerCode")
                {
                    col.ColumnName = "slc_customercode";
                }
                else if (col.ColumnName == "CustomerName")
                {
                    col.ColumnName = "slc_customername";
                }
                else if (col.ColumnName == "SalesOrderCode")
                {
                    col.ColumnName = "slc_salesordercode";
                }
                else if (col.ColumnName == "InvoiceCode")
                {
                    col.ColumnName = "slc_invoicecode";
                }
                else if (col.ColumnName == "DispatchDate")
                {
                    col.ColumnName = "slc_dispatchdate";
                }
                else if (col.ColumnName == "DueDate")
                {
                    col.ColumnName = "slc_duedate";
                }
                else if (col.ColumnName == "SalesAmount")
                {
                    col.ColumnName = "slc_salesamount";
                }
                else if (col.ColumnName == "RecieptAmount")
                {
                    col.ColumnName = "slc_recieptamount";
                }
                else if (col.ColumnName == "ReturnAmount")
                {
                    col.ColumnName = "slc_returnamount";
                }
                else if (col.ColumnName == "RefundAmount")
                {
                    col.ColumnName = "slc_refundamount";
                }
                else if (col.ColumnName == "StoreCreditBalance")
                {
                    col.ColumnName = "slc_storecreditbalance";
                }
                else if (col.ColumnName == "PendingBalance")
                {
                    col.ColumnName = "slc_pendingbalance";
                }
                else if (col.ColumnName == "CustomerPendingAmount") //#
                {
                    col.ColumnName = "slc_customerpendingamount";
                }
                else if (col.ColumnName == "IsEmailExist") //Date Assigned
                {
                    col.ColumnName = "slc_isemailexist";
                }
            }
            return dt;
        }

        private static DataTable GetDataTabletFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    //read column names

                    string[] colFields = csvReader.ReadFields();

                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }

                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
                //csvData.Columns[6].DataType = Type.GetType("System.Double");

                DataColumn dValue1 = new DataColumn();
                dValue1.ColumnName = "Upload ID";
                dValue1.DataType = Type.GetType("System.Int64");
                csvData.Columns.Add(dValue1);
                csvData.Columns["Upload ID"].SetOrdinal(0);
                csvData.Columns["Upload ID"].DefaultValue = 1;


                using (SLCDBDataSetTableAdapters.tblslcrecordsTableAdapter adap =
                    new SLCDBDataSetTableAdapters.tblslcrecordsTableAdapter())
                {
                    long slc_id = Convert.ToInt64(adap.ScalarQuery_MaxSLCID());
                    Int64 ciwbcmdindexer = slc_id;
                    adap.Dispose();
                    //loop for assigning the current and accumulative data for the new generated columns
                    foreach (DataRow row in csvData.Rows)
                    {
                        slc_id += 1;
                        row.SetField("Upload ID", slc_id);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString());
                return null;
            }

            return csvData;
        }

        private void barbtnsave_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (gridView1.RowCount == 0)
                {
                    XtraMessageBox.Show("No row data found in the Grid view", "Spreadsheet Upload Process", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    SaveData();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString());
                return;
            }
        }

        private void SaveData()
        {
            try
            {
                using (var handle = SplashScreenManager.ShowOverlayForm(gridControl1))
                {
                    handle.QueueFocus(IntPtr.Zero);
                    DataTable dt = gridControl1.DataSource as DataTable;
                    Functions.SaveToDB(Properties.Settings.Default.SLCDBConnectionString, RenameTableHeader_ForDBSave(dt), "tblslcrecords");

                    if (XtraMessageBox.Show("Data was successfully processed", "Spreadsheet Upload Process", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        clearGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString());
                return;
            }
        }

        private void clearGrid()
        {
            object ds;  // Data source

            // Clear  
            ds = gridControl1.DataSource;
            gridControl1.DataSource = null;
            GridView view = new GridView(gridControl1);
            view.OptionsView.ShowGroupPanel = false;
            view.OptionsView.ShowColumnHeaders = false;
            gridControl1.MainView = view;


            // Restore  
            gridControl1.MainView = gridView1;
            //gridControl1.DataSource = ds;
        }
        private void rbnUploadData_Load(object sender, EventArgs e)
        {

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            clearGrid();
        }
    }
}

