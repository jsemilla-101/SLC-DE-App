using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLC_DE_App
{
    public partial class frmSignUp : Form
    {
        public frmSignUp()
        {
            InitializeComponent();
        }    

        private void frmSignUp_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'sLCDBDataSet.tbluser' table. You can move, or remove it, as needed.
            this.tbluserTableAdapter.Fill(this.sLCDBDataSet.tbluser);
            this.tbldesignationTableAdapter.Fill(this.sLCDBDataSet.tbldesignation);

            gcombodesig.SelectedIndex = -1;
            gcomborepto.SelectedIndex = -1;
        }

        private void gbtnRegister_Click(object sender, EventArgs e)
        {
            //check if id exist
            using (SLCDBDataSetTableAdapters.tbluserTableAdapter adap = new SLCDBDataSetTableAdapters.tbluserTableAdapter())
            {
                string id = Convert.ToString(adap.ScalarQuery_CheckIfExist(gtxtIDnumber.Text));

                if (id == gtxtIDnumber.Text) 
                {
                    guna2MessageDialog1.Show("User Zoho ID already registered", "User Account Registration");
                    return;
                }           
            }
            
            //check empty
            if (gtxtIDnumber.Text == String.Empty || gtxtemail.Text == String.Empty
                || gtxtpassword.Text == String.Empty || gtxtconpassword.Text == String.Empty
                || gtxtfname.Text == String.Empty || gtxtlastname.Text == String.Empty || gcombodesig.Text == String.Empty)
            {
                guna2MessageDialog1.Show("Please fill in data needed", "User Account Registration");
                return;
            }

            //check password is equal
            else if (gtxtpassword.Text != gtxtconpassword.Text)
            {
                guna2MessageDialog1.Show("Password not confirm as equal", "User Account Registration");
                return;
            }
            else
            {
                using (SLCDBDataSetTableAdapters.tbluserTableAdapter adap = new SLCDBDataSetTableAdapters.tbluserTableAdapter())
                {
                     Int64 maxuserid;
                     maxuserid = Convert.ToInt64(Functions.ReplaceNull(adap.ScalarQuery_GetMaxUserID())) + 1;
                    adap.InsertQuery_UserRegister(maxuserid, gtxtIDnumber.Text, gtxtemail.Text, gtxtpassword.Text, gtxtfname.Text, gtxtlastname.Text, Convert.ToInt64(gcombodesig.SelectedValue), Convert.ToInt64(gcomborepto.SelectedValue));
                    guna2MessageDialog1.Show("Successfully registered: Account must be activated before log in", "User Account Registration");
                    gbtnclear.PerformClick();
                    
                }
            }  
        }

        private void gbtnclear_Click(object sender, EventArgs e)
        {
        
            gtxtIDnumber.Clear();
            gtxtemail.Clear();
            gtxtfname.Clear();
            gtxtlastname.Clear();
            gtxtpassword.Clear();
            gtxtconpassword.Clear();

            gcombodesig.SelectedIndex = -1;
            gcomborepto.SelectedIndex = -1;
        }

        private void gcombodesig_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void gcombodesig_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                gcombodesig.SelectedIndex = -1;
            }
        }

        private void gcomborepto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                gcomborepto.SelectedIndex = -1;
            }
        }
    }
}
