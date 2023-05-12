using Npgsql;
using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.DirectoryServices;
using DevExpress.XtraEditors;

namespace SLC_DE_App
{
    public partial class frmLogin : Form
    {
        int isLogin = 0;
        //private NpgsqlConnection conn;
        //private NpgsqlCommand cmd;
        //private DataTable dt;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void ADLogin(string username, string password, string domain)
        {
            DirectoryEntry entry = new DirectoryEntry($"LDAP://{domain}", username, password);
            try
            {
                // Authenticate the user's credentials
                object nativeObject = entry.NativeObject;

                // Retrieve the user's display name
                DirectorySearcher searcher = new DirectorySearcher(entry);
                searcher.Filter = $"(&(objectClass=user)(sAMAccountName={username}))";
                SearchResult result = searcher.FindOne();
                string displayName = result.Properties["displayName"][0].ToString();

                isLogin = 1;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error authenticating user: {0}", ex.Message);
                XtraMessageBox.Show(ex.Message.ToString());
                isLogin = 0;
            }
        }
        //Log in Button
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (gtxtIDnumber.Text == String.Empty || gtxtPassword.Text == String.Empty)
            {
                guna2MessageDialog1.Show("Fields cannot be empty: please input data", "Log in");
                return;
            }

            ADLogin(gtxtIDnumber.Text, gtxtPassword.Text, "officepartners360.com");

            if (isLogin == 1)
            {
                ObtainIPAddr(); // get IP address
                                // Functions.trailuserlog(conn, Globals.userempid, Globals.uipaddr, btnOk.Text, "ATT".ToString(), Globals._uversion); // Log timestamp everytime user login and logout
                this.Hide();
                //Globals._frmMain = new Main(txtUsername.Text, txtPassword.Text);
                //Globals._frmMain.Show();

                //check if log in exist
                rbnMain frm = new rbnMain();
               
                frm.ShowIcon = false;
                frm.Show();
            }
            ////check if empty
            //if (gtxtIDnumber.Text == String.Empty || gtxtPassword.Text == String.Empty)
            //{
            //    guna2MessageDialog1.Show("Fields cannot be empty: please input data", "Log in");
            //    return;
            //}

            ////check log in
            //else
            //{
            //    //go log in
            //    conn = new NpgsqlConnection(Globals.connstring);
            //    cmd = new NpgsqlCommand(string.Format(@"SELECT * FROM tbluser WHERE userzohoid ='{0}' AND userpass ='{1}'", gtxtIDnumber.Text, gtxtPassword.Text), conn);
            //    dt = new DataTable();
            //    conn.Open();
            //    dt.Load(cmd.ExecuteReader());
            //    conn.Close();

            //    if (dt.Rows.Count > 0)
            //    {
            //        Globals.userid = dt.Rows[0].Field<Int64>(0); // ID
            //        Globals.userempid = dt.Rows[0].Field<string>(1); // EMPID
            //        Globals.userpassword = dt.Rows[0].Field<string>(3); //UPASSWORD

            //        ObtainIPAddr(); // get IP address
            //        // Functions.trailuserlog(conn, Globals.userempid, Globals.uipaddr, btnOk.Text, "ATT".ToString(), Globals._uversion); // Log timestamp everytime user login and logout
            //        this.Hide();

            //        //Globals._frmMain = new Main(txtUsername.Text, txtPassword.Text);
            //        //Globals._frmMain.Show();

            //        //check if log in exist
            //        rbnMain frm = new rbnMain();
            //        //rbnMainFormUI frm = new rbnMainFormUI();
            //        frm.ShowIcon = false;
            //        frm.Show();

            //    }
            //    else
            //    {
            //        guna2MessageDialog1.Show("Username and password does not exist.", "Login fail");
            //        conn.Close();
            //    }


            //    conn.Dispose();
            //}
        }

        //Sign Up Button
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            using (frmSignUp frm = new frmSignUp())
            {
                frm.ShowInTaskbar = false;
                frm.ShowIcon = false;
                frm.ShowDialog();
            }
        }

        private void ObtainIPAddr()
        {
            IPHostEntry ipaddr;
            string localIP = "?";
            ipaddr = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in ipaddr.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            Globals.uipaddr = localIP;
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();
        }
    }
}
