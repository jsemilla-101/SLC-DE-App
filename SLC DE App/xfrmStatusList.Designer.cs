namespace SLC_DE_App
{
    partial class xfrmStatusList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tblslctaskstatusBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sLCDBDataSet = new SLC_DE_App.SLCDBDataSet();
            this.tblslctaskstatusTableAdapter = new SLC_DE_App.SLCDBDataSetTableAdapters.tblslctaskstatusTableAdapter();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.guna2ShadowForm1 = new Guna.UI2.WinForms.Guna2ShadowForm(this.components);
            this.coltaskstatid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltaskstatname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltaskstataddedby = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltaskstatdateadded = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltaskstatisactive = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.coltaskstatid1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltaskstatname1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltaskstataddedby1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltaskstatdateadded1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltaskstatisactive1 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.tblslctaskstatusBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLCDBDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tblslctaskstatusBindingSource
            // 
            this.tblslctaskstatusBindingSource.DataMember = "tblslctaskstatus";
            this.tblslctaskstatusBindingSource.DataSource = this.sLCDBDataSet;
            // 
            // sLCDBDataSet
            // 
            this.sLCDBDataSet.DataSetName = "SLCDBDataSet";
            this.sLCDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tblslctaskstatusTableAdapter
            // 
            this.tblslctaskstatusTableAdapter.ClearBeforeFill = true;
            // 
            // guna2Button1
            // 
            this.guna2Button1.BorderRadius = 16;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Location = new System.Drawing.Point(289, 365);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(103, 36);
            this.guna2Button1.TabIndex = 1;
            this.guna2Button1.Text = "ADD";
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 12;
            this.guna2Elipse1.TargetControl = this;
            // 
            // guna2ShadowForm1
            // 
            this.guna2ShadowForm1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.guna2ShadowForm1.TargetForm = this;
            // 
            // coltaskstatid
            // 
            this.coltaskstatid.FieldName = "taskstatid";
            this.coltaskstatid.Name = "coltaskstatid";
            this.coltaskstatid.OptionsColumn.AllowEdit = false;
            this.coltaskstatid.Width = 50;
            // 
            // coltaskstatname
            // 
            this.coltaskstatname.FieldName = "taskstatname";
            this.coltaskstatname.Name = "coltaskstatname";
            this.coltaskstatname.OptionsFilter.AllowAutoFilter = false;
            this.coltaskstatname.OptionsFilter.AllowFilter = false;
            this.coltaskstatname.Visible = true;
            this.coltaskstatname.VisibleIndex = 0;
            this.coltaskstatname.Width = 269;
            // 
            // coltaskstataddedby
            // 
            this.coltaskstataddedby.FieldName = "taskstataddedby";
            this.coltaskstataddedby.Name = "coltaskstataddedby";
            // 
            // coltaskstatdateadded
            // 
            this.coltaskstatdateadded.FieldName = "taskstatdateadded";
            this.coltaskstatdateadded.Name = "coltaskstatdateadded";
            // 
            // coltaskstatisactive
            // 
            this.coltaskstatisactive.FieldName = "taskstatisactive";
            this.coltaskstatisactive.Name = "coltaskstatisactive";
            this.coltaskstatisactive.Visible = true;
            this.coltaskstatisactive.VisibleIndex = 1;
            this.coltaskstatisactive.Width = 94;
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.tblslctaskstatusBindingSource;
            this.gridControl1.Location = new System.Drawing.Point(12, 12);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(380, 347);
            this.gridControl1.TabIndex = 2;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.coltaskstatid1,
            this.coltaskstatname1,
            this.coltaskstataddedby1,
            this.coltaskstatdateadded1,
            this.coltaskstatisactive1});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridView1_ShowingEditor);
            // 
            // coltaskstatid1
            // 
            this.coltaskstatid1.FieldName = "taskstatid";
            this.coltaskstatid1.Name = "coltaskstatid1";
            this.coltaskstatid1.Visible = true;
            this.coltaskstatid1.VisibleIndex = 0;
            this.coltaskstatid1.Width = 41;
            // 
            // coltaskstatname1
            // 
            this.coltaskstatname1.FieldName = "taskstatname";
            this.coltaskstatname1.Name = "coltaskstatname1";
            this.coltaskstatname1.Visible = true;
            this.coltaskstatname1.VisibleIndex = 1;
            this.coltaskstatname1.Width = 202;
            // 
            // coltaskstataddedby1
            // 
            this.coltaskstataddedby1.FieldName = "taskstataddedby";
            this.coltaskstataddedby1.Name = "coltaskstataddedby1";
            // 
            // coltaskstatdateadded1
            // 
            this.coltaskstatdateadded1.FieldName = "taskstatdateadded";
            this.coltaskstatdateadded1.Name = "coltaskstatdateadded1";
            // 
            // coltaskstatisactive1
            // 
            this.coltaskstatisactive1.FieldName = "taskstatisactive";
            this.coltaskstatisactive1.Name = "coltaskstatisactive1";
            this.coltaskstatisactive1.Visible = true;
            this.coltaskstatisactive1.VisibleIndex = 2;
            this.coltaskstatisactive1.Width = 85;
            // 
            // xfrmStatusList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(416, 415);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.guna2Button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "xfrmStatusList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "STATUS LIST";
            this.Load += new System.EventHandler(this.xfrmStatusList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tblslctaskstatusBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sLCDBDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private SLCDBDataSet sLCDBDataSet;
        private System.Windows.Forms.BindingSource tblslctaskstatusBindingSource;
        private SLCDBDataSetTableAdapters.tblslctaskstatusTableAdapter tblslctaskstatusTableAdapter;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2ShadowForm guna2ShadowForm1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn coltaskstatid1;
        private DevExpress.XtraGrid.Columns.GridColumn coltaskstatname1;
        private DevExpress.XtraGrid.Columns.GridColumn coltaskstataddedby1;
        private DevExpress.XtraGrid.Columns.GridColumn coltaskstatdateadded1;
        private DevExpress.XtraGrid.Columns.GridColumn coltaskstatisactive1;
        private DevExpress.XtraGrid.Columns.GridColumn coltaskstatid;
        private DevExpress.XtraGrid.Columns.GridColumn coltaskstatname;
        private DevExpress.XtraGrid.Columns.GridColumn coltaskstataddedby;
        private DevExpress.XtraGrid.Columns.GridColumn coltaskstatdateadded;
        private DevExpress.XtraGrid.Columns.GridColumn coltaskstatisactive;
    }
}