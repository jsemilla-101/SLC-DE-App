using Devart.Data.PostgreSql;
using DevExpress.XtraEditors;
using Npgsql;
using System;
using System.Data;
using System.Data.Linq.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SLC_DE_App
{
    public static class Functions
    {
        public static bool CommitTransaction()
        {
            bool commitstat;
            commitstat = false;
            try
            {
                if (Globals.connTerminal.State == ConnectionState.Open)
                {
                    Globals.transact.Commit();
                    commitstat = true;
                    if (Globals.connTerminal.State == ConnectionState.Open)
                    {
                        Globals.connTerminal.Close();
                        Globals.connTerminal.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString());
                commitstat = false;
            }
            return commitstat;
        }

        public static NpgsqlTransaction ResetTransaction(int timeout = 500)
        {
            if (Globals.connTerminal != null)
            {
                Globals.connTerminal.Dispose();
                Globals.connTerminal = null;
            }

            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();
            builder.Timeout = timeout;
            Globals.connTerminal = new NpgsqlConnection(builder.ToString());

            if (Globals.connTerminal.State == ConnectionState.Closed || Globals.connTerminal.State == ConnectionState.Broken)
            {
                Globals.connTerminal.ConnectionString = Globals.connstring;
                Globals.connTerminal.Open();
                Globals.transact = Globals.connTerminal.BeginTransaction(IsolationLevel.Serializable);
            }

            return Globals.transact;
        }

        public static void RollBackTransaction()
        {
            if (Globals.connTerminal != null)
            {
                if (Globals.connTerminal.State == ConnectionState.Open)
                {
                    Globals.transact.Rollback();
                    Globals.connTerminal.Close();
                    Globals.connTerminal.Dispose();
                    Globals.connTerminal = null;
                    Globals.transact.Dispose();
                }
            }
        }

        public static bool CheckDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void CheckNumeric(System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Back))
            {
                return;
            }
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        public static Boolean CheckEmpRole(string refid, string refdes)
        {
            NpgsqlConnection conn;
            NpgsqlCommand cmd;
            string sql = "";

            string _role = string.Empty;
            conn = new NpgsqlConnection(Globals.connstring);
            sql = @"SELECT _role FROM roster_select() WHERE _id='" + refid + "' AND designation='" + refdes + "'";
            cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader myreader;
            try
            {
                conn.Open();
                myreader = cmd.ExecuteReader();
                while (myreader.Read())
                {
                    // string _id = myreader.GetString(0);
                    _role = myreader.GetString(0);
                }
                conn.Close();
                conn.Dispose();
                if (_role == null || _role == "" || _role.Contains("Senior") || _role.Contains("Director") || _role.Contains("Admin") || _role.Contains("Manager"))
                { return true; }
                else
                { return false; }
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        public static string collatestr(DataTable dtdata, int val)
        {
            string hold = "";
            if (dtdata.Rows.Count != 0)
            {
                foreach (DataRow dr in dtdata.Rows)
                {
                    if (val == 0)
                    {
                        if (hold == "")
                        { hold = "'" + dr["lobname"].ToString() + "'"; }
                        else { hold = hold + ", '" + dr["lobname"].ToString() + "'"; }
                    }
                    else if (val == 1)
                    {
                        if (hold == "")
                        { hold = "'" + dr["_tlead"].ToString() + "'"; }
                        else { hold = hold + ", '" + dr["_tlead"].ToString() + "'"; }
                    }
                    else if (val == 2)
                    {
                        if (hold == "")
                        { hold = "'" + dr["_empid"].ToString() + "'"; }
                        else { hold = hold + ", '" + dr["_empid"].ToString() + "'"; }
                    }
                    else if (val == 3)
                    {
                        if (hold == "")
                        { hold = "'" + dr["_empid"].ToString() + "'" + ";" + dr["_lobid"].ToString() + ";" + dr["_tlead"].ToString(); }
                        else { hold = hold + ", '" + dr["_empid"].ToString() + "'" + ";" + dr["_lobid"].ToString() + ";" + dr["_tlead"].ToString(); }
                    }
                    else if (val == 4)
                    {
                        if (hold == "")
                        { hold = "'" + dr["_name"].ToString() + "'"; }
                        else { hold = hold + ", '" + dr["_name"].ToString() + "'"; }
                    }
                    else if (val == 5)
                    {
                        if (hold == "")
                        { hold = "'" + dr["_lobid"].ToString() + "'"; }
                        else { hold = hold + ", '" + dr["_lobid"].ToString() + "'"; }
                    }
                    else if (val == 6)
                    {
                        if (hold == "")
                        { hold = "'" + dr["act_name"].ToString() + "'"; }
                        else { hold = hold + ", '" + dr["act_name"].ToString() + "'"; }
                    }
                    else if (val == 7)
                    {
                        if (hold == "")
                        { hold = "'" + dr["_lobclstr"].ToString() + "'"; }
                        else { hold = hold + ", '" + dr["_lobclstr"].ToString() + "'"; }
                    }
                }
            }
            return hold;
        }

        public static Boolean getAuthorityAccess(string _role)
        {
            bool boolvale = false;
            if (_role.Contains("Director") || _role.Contains("Admin") || _role.Contains("Manager"))
            { boolvale = true; }
            else if (_role.Contains("Senior"))
            { boolvale = false; }
            return boolvale;
        }

        public static string NullorEmptyLOBEdit(string refval)
        {
            switch (refval)
            {
                case null:
                    return "";
                case "":
                    return "";
                default:
                    return string.Format(" and  _lobid in ({0})", refval);
            }
        }

        public static string NullorEmptyLOB(string refval)
        {
            switch (refval)
            {
                case null:
                    return "";
                case "":
                    return "";
                default:
                    return string.Format(" and  _lob in ({0})", refval);
            }
        }

        public static string NullorEmpty(string refval)
        {
            switch (refval)
            {
                case null:
                    return "";
                case "":
                    return "";
                default:
                    return refval;
            }
        }

        public static bool checkDateFile(DateTime date1, DateTime date2)
        {
            if (date1 == date2)
            { return false; }
            else
            { return true; }
        }

        public static bool chkAbsType(string stype)
        {
            switch (stype)
            {
                case "VL":
                    return false;
                case "HD-VL":
                    return false;
                default:
                    return true;
            }
        }

        public static string checkisTrue(bool statval, string val1, string val2)
        {
            if (statval == true)
            { return val1; }
            else { return val2; }
        }

        public static bool trailuserlog(NpgsqlConnection conn, string uid, string uipaddr, string uactivity, string umodule, string uversion)
        {
            int result = 0;
            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                conn = new NpgsqlConnection(Globals.connstring);
                conn.Open();
                string sql = @"select * from traillog_insert(:uid, :uipaddr, :uactivity, :umodule, :uversion)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("uid", uid);
                cmd.Parameters.AddWithValue("uipaddr", uipaddr);
                cmd.Parameters.AddWithValue("uactivity", uactivity);
                cmd.Parameters.AddWithValue("umodule", umodule);
                cmd.Parameters.AddWithValue("uversion", uversion);
                // Timestamp value to be inserted in stored procedure PostgreSQL
                result = (int)cmd.ExecuteScalar();
                conn.Close();
                conn.Dispose();
                if (result != 1)
                { MessageBox.Show("Something went wrong tagging the Ticket.", "Ticket Tracker", MessageBoxButtons.OK, MessageBoxIcon.Stop); return false; }
                else
                { return true; }
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static DateTime servertime(NpgsqlConnection conn)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            conn = new NpgsqlConnection(Globals.connstring);
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "servertime";
            cmd.Connection = conn;
            var obj = cmd.ExecuteScalar();
            conn.Close();
            conn.Dispose();
            return Convert.ToDateTime(obj);
        }

        public static string getTeamID(NpgsqlConnection conn, string subquery)
        {
            NpgsqlCommand cmd;
            string retval = "";
            conn = new NpgsqlConnection(Globals.connstring);
            string sql = string.Format("select team_id from tblteam_master where {0}", subquery);
            cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader myreader;
            conn.Open();

            myreader = cmd.ExecuteReader();
            while (myreader.Read())
            {
                if (myreader.GetValue(0) != DBNull.Value)
                { retval = myreader.GetInt32(0).ToString(); }
                else
                { retval = ""; }
            }
            conn.Dispose();
            return retval;
        }

        public static string getDTRLog(string empid, string dtrtype, DateTime shiftdate)
        {
            NpgsqlConnection conn = new NpgsqlConnection(Globals.connstring);
            NpgsqlCommand cmd;
            string retval = "";
            string sql = string.Format("select dtr_clocktime from tbldtr where dtr_empid='{0}' and dtr_clocktype='{1}' and dtr_clocktime::date='{2}'::date order by dtr_clocktime desc limit 1", empid, dtrtype, shiftdate);
            cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader myreader;
            conn.Open();
            myreader = cmd.ExecuteReader();
            while (myreader.Read())
            {
                if (myreader.GetValue(0) != DBNull.Value)
                { retval = myreader.GetDateTime(0).ToString(); }
                else
                { retval = ""; }
            }
            conn.Close();
            conn.Dispose();
            return retval;
        }

        public static DataTable FetchDTRpairing(DateTime _startdate, DateTime _enddate, string _empid)
        {
            NpgsqlConnection conn = new NpgsqlConnection(Globals.connstring);
            NpgsqlCommand cmd;
            DataTable retval = new DataTable();
            string sql = string.Format(@"SELECT * FROM crosstab
                (
	                'select dtr_empid, dtr_clocktype, dtr_clocktime from tbldtr 
	                where dtr_clocktime >= ''{0}'' and dtr_clocktime <= ''{1}'' and dtr_empid=''{2}''
	                order by 1,3,2 asc', 'select distinct dtr_clocktype from tbldtr order by 1'
                ) as ( dtr_empid character varying, typein text, Login timestamp, typeout text, Logout timestamp )", _startdate, _enddate, _empid);
            cmd = new NpgsqlCommand(sql, conn);
            conn.Open();
            retval.Load(cmd.ExecuteReader());
            conn.Close();
            conn.Dispose();
            return retval;
        }

        public static DataTable DTRpairing(DateTime _startdate, DateTime _enddate, DateTime _USenddate)
        {
            NpgsqlConnection conn = new NpgsqlConnection(Globals.connstring);
            NpgsqlCommand cmd;
            DataTable retval = new DataTable();
            string sql = string.Format(@"SELECT dtr_empid, typein, login, typeout, logout FROM crosstab
                (
	                'select dtr_empid, dtr_clocktype, dtr_clocktime from tbldtr
	                left join tbluser on userempid=dtr_empid
	                where dtr_clocktime between ''{0} 00:00:00'' and ''{1} 23:59:59''
	                and usershifttype=''PH'' union select dtr_empid, dtr_clocktype, dtr_clocktime from tbldtr
	                left join tbluser on userempid=dtr_empid
	                where dtr_clocktime between ''{0} 13:00:00'' and ''{2} 12:00:00''
	                and usershifttype in (''US'', ''Swing'') union select dtr_empid, dtr_clocktype, 
					dtr_clocktime from tbldtr left join tbluser on userempid=dtr_empid
	                where usershifttype in (''Rotational'')  and 
                    (dtr_clocktime between ''{0} 00:00:00'' and ''{1} 23:00:00'' or
                    dtr_clocktime between ''{0} 17:00:00'' and ''{2} 10:00:00'') order by 1,3,2 asc', 
                    'select distinct dtr_clocktype from tbldtr order by 1'
                ) as (dtr_empid text, dtr_empid2 text, typein text, Login timestamp, typeout text, Logout timestamp)", string.Format("{0:yyyy-MM-dd}", _startdate.Date), string.Format("{0:yyyy-MM-dd}"
                , _enddate.Date), string.Format("{0:yyyy-MM-dd}", _USenddate));
            cmd = new NpgsqlCommand(sql, conn);
            conn.Open();
            retval.Load(cmd.ExecuteReader());
            conn.Close();
            conn.Dispose();
            return retval;
        }

        public static int splitTime(string timestr)
        {
            string[] timesplit = timestr.Split(new[] { ":" }, StringSplitOptions.None);
            return int.Parse(timesplit[0]);
        }

        public static int getHighLigthsValue(string IN, string OUT, string rddays)
        {
            int hl_col = 0;
            if (rddays != "RD" || rddays != "")
            {
                int cntr = 0;
                if (IN != "" | IN != null)
                { cntr += 1; }
                if (OUT != "" | OUT != null)
                { cntr += 1; }

                //if (current_timestamp <= servertime(new NpgsqlConnection(Globals.connstring)))
                //{
                if (cntr == 1)
                { hl_col = 1; }
                else
                { hl_col = 0; }
                //}
                //else { hl_col = 0; }
            }
            return hl_col;
        }

        public static bool IsNumeric(this String s) => s.All(Char.IsDigit);


        public static string GetTimeSpanString(TimeSpan ts)
        {
            StringBuilder output = new StringBuilder();
            bool needsComma = false;

            if (ts == null)
            { return "00:00:00"; }

            if (ts.TotalHours >= 1)
            {
                output.AppendFormat("{0}", Math.Truncate(ts.TotalHours));
                if (ts.TotalHours > 1)
                { output.Append("s"); }
                needsComma = true;
            }

            if (ts.Minutes > 0)
            {
                if (needsComma)
                {
                    output.Append(", ");
                }
                output.AppendFormat("{0} m", ts.Minutes);
                needsComma = true;
            }

            return output.ToString();
        }

        // Check Activity without Sign Off in between Shift Date
        public static bool checkSignOffShift(bool isShiftType, DateTime Shift_DATE, int Shift_Time_Out, DateTime Last_Activity_DateTime, DateTime SignOff_Activity_DateTime, DateTime CURRENT_DATE)
        {
            bool retvalue = false;
            DateTime shiftOUT = new DateTime(SignOff_Activity_DateTime.Year, SignOff_Activity_DateTime.Month, SignOff_Activity_DateTime.Day, Shift_Time_Out, 0, 0);

            if (Shift_DATE.Date < CURRENT_DATE.Date)
            {
                retvalue = true;
            }
            else
            {
                if (Last_Activity_DateTime < shiftOUT)
                { retvalue = false; }
                else { retvalue = true; }
            }
            return retvalue;
        }

        public static DateTime setShiftDateGlobal(string shifttype)
        {
            DateTime serverDate = servertime(new NpgsqlConnection(Globals.connstring));
            //if (shifttype == "PH")
            //{
            //    // PH shift
            //    Globals.globalShiftDate = serverDate;
            //}
            //else
            //{
            //    // US or Swing shift
            //    if (shifttype == "Rotational")
            //    {
            //        //split shift here
            //        string[] shiftsplit = Globals.usershiftsched.Split(new[] { " - " }, StringSplitOptions.None);
            //        string[] tslogin = shiftsplit[0].Split(new[] { ":" }, StringSplitOptions.None);
            //        string[] tslogout = shiftsplit[1].Split(new[] { ":" }, StringSplitOptions.None);
            //        if (int.Parse(tslogin[0]) > int.Parse(tslogout[0]))
            //        {
            //            if (serverDate.Hour < 12)
            //            { Globals.globalShiftDate = serverDate.AddDays(-1); }
            //            else { Globals.globalShiftDate = serverDate; }
            //        }
            //        else { Globals.globalShiftDate = serverDate; }
            //    }
            //    else
            //    {
            //        if (serverDate.Hour < 12)
            //        { Globals.globalShiftDate = serverDate.AddDays(-1); }
            //        else { Globals.globalShiftDate = serverDate; }
            //    }
            //}
            //return Globals.globalShiftDate;

            string[] shiftsplit = Globals.usershiftsched.Split(new[] { " - " }, StringSplitOptions.None);
            string[] tslogin = shiftsplit[0].Split(new[] { ":" }, StringSplitOptions.None);
            //string[] tslogout = shiftsplit[1].Split(new[] { ":" }, StringSplitOptions.None);

            if (int.Parse(tslogin[0]) > 14)
            {
                if (serverDate.Hour < 12)
                { Globals.globalShiftDate = serverDate.AddDays(-1); }
                else { Globals.globalShiftDate = serverDate; }
            }
            else { Globals.globalShiftDate = serverDate; }

            return Globals.globalShiftDate;
        }

        public static Double returnZero(Double retval)
        {
            double value = 0;
            if (retval < Double.Parse("0.01"))
            { value = 0; }
            else { value = retval; }
            return value;
        }

        public static string getTableID(NpgsqlConnection conn, string column, string table, string index)
        {
            NpgsqlCommand cmd;
            string retval = "";
            string sql = string.Format("select {0} from {1} where {2}", column, table, index);
            cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader myreader;
            conn.Open();

            myreader = cmd.ExecuteReader();
            while (myreader.Read())
            {
                if (myreader.GetValue(0) != DBNull.Value)
                { retval = myreader.GetString(0); }
                else
                { retval = ""; }
            }
            conn.Dispose();
            return retval;
        }

        public static int returnCount(NpgsqlConnection conn, string table, string clause)
        {
            NpgsqlCommand cmd;
            int retval = 0;
            string sql = string.Format("select count(*) from {0} where {1}", table, clause);
            cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader myreader;
            conn.Open();

            myreader = cmd.ExecuteReader();
            while (myreader.Read())
            {
                if (myreader.GetValue(0) != DBNull.Value)
                { retval = myreader.GetInt32(0); }
                else
                { retval = 0; }
            }
            conn.Close();
            conn.Dispose();
            return retval;
        }

        public static bool getDaysDiff(DateTime value1, DateTime value2)
        {
            // Get the difference between to dates
            // Allowable 3 days for Team Leads and 7 days for STL up
            int daydiff = 0;

            daydiff = SqlMethods.DateDiffDay(value1.Date, value2.Date);
            if (Convert.ToInt16(Globals.usergroupid) == 37)
            {
                if (Globals.userrole.Contains("Supervisor"))
                { if (daydiff >= 0 && daydiff <= Globals._stlDue) { return true; } else { return false; } }
                else { return true; }
            }
            else { if (daydiff >= 0 && daydiff <= Globals._tlDue) { return true; } else { return false; } }
        }

        public static DateTime checkValidRangeDate(DateTime startvalue, DateTime endvalue)
        {
            if (startvalue > endvalue)
            { return servertime(new NpgsqlConnection(Globals.connstring)); }
            else { return endvalue; }
        }

        public static string getColumnValue(NpgsqlConnection conn, string column, string table, string condition)
        {
            NpgsqlCommand cmd;
            string retval = "";
            conn.Open();
            string sql = string.Format("select {0} from {1} where {2}", column, table, condition);
            cmd = new NpgsqlCommand(sql, conn);
            //NpgsqlDataReader myreader;
            DataTable dtitem = new DataTable();
            dtitem.Load(cmd.ExecuteReader());

            if (dtitem.Rows.Count != 0)
            { retval = dtitem.Rows[0][0].ToString(); }
            else { retval = ""; }

            conn.Close();
            conn.Dispose();
            return retval;
        }

        public static DataTable getDataTable(NpgsqlConnection conn, string column, string table, string condition)
        {
            NpgsqlCommand cmd;
            conn.Open();
            string sql = string.Format("select {0} from {1} where {2}", column, table, condition);
            cmd = new NpgsqlCommand(sql, conn);
            //NpgsqlDataReader myreader;
            DataTable dtitem = new DataTable();
            dtitem.Load(cmd.ExecuteReader());
            conn.Close();
            conn.Dispose();
            return dtitem;
        }

        public static DataTable getDataTable(NpgsqlConnection conn, string column, string table)
        {
            NpgsqlCommand cmd;
            conn.Open();
            string sql = string.Format("select {0} from {1}", column, table);
            cmd = new NpgsqlCommand(sql, conn);
            //NpgsqlDataReader myreader;
            DataTable dtitem = new DataTable();
            dtitem.Load(cmd.ExecuteReader());
            conn.Close();
            conn.Dispose();
            return dtitem;
        }

        public static bool checkOTremoveLate(string OTcat)
        {
            switch (OTcat)
            {
                case "OT RD":
                    return true;
                case "OT RDH":
                    return true;
                case "OT LH":
                    return true;
                case "OTSPH":
                    return true;
                default:
                    return false;
            }
        }

        public static string CorrectFormatID(string idincorrect)
        {
            if (idincorrect.Length == 2)
            { return "0" + idincorrect.ToString(); }
            else { return idincorrect.ToString(); }
        }

        public static double retZeroVal(double value)
        {
            if (value < 0.0)
            { return 0.0; }
            else { return value; }
        }

        public static double returnLegal(string _holtype, bool _holispay, double staffhour)
        {
            double returnvalue = 0;
            if (_holtype == "REGULAR")
            {
                if (_holispay == true)
                {
                    if (staffhour != 0)
                    {
                        if (staffhour > 7.59)
                        { returnvalue = 8.00; }
                        else { returnvalue = staffhour; }
                    }
                    //else { returnvalue = 8.00; }
                }
            }
            else { returnvalue = 0.00; }

            return returnvalue;
        }

        public static double returnSpecial(string _holtype, bool _holispay, double staffhour)
        {
            double returnvalue = 0;
            if (_holtype == "SPECIAL")
            {
                if (staffhour != 0)
                {
                    if (staffhour > 8)
                    { returnvalue = Math.Truncate(staffhour - 1); }
                    else { returnvalue = staffhour; }
                }
                else { returnvalue = 0.00; }
            }
            else { returnvalue = 0.00; }

            return returnvalue;
        }


        public static SaveInfo SaveToDB(string strconnection, DataTable dt, string tabname)
        {


            SaveInfo sinfo = new SaveInfo();
            string sql;
            string[] column;
            string insert;
            string values;
            int i = 0;
            int x = 0;
            PgSqlCommand cmd = new PgSqlCommand();
            int ctr = 0;
            PgSqlTransaction trans = null /* TODO Change to default(_) if this is not a reference type */;

            column = new string[dt.Columns.Count + 1];
            // ReDim insert(dt.rows.count)

            foreach (DataColumn col in dt.Columns)
            {
                column[x] = col.ColumnName;
                x += 1;
            }

            insert = string.Format("INSERT INTO {0} (", tabname);
            values = "VALUES(";

            for (i = 0; i <= column.GetUpperBound(0) - 1; i++)
            {
                if (i == column.GetUpperBound(0) - 1)
                {
                    insert += column[i] + ") ";
                    values += string.Format(":{0})", column[i]);
                }
                else
                {
                    insert += column[i] + ", ";
                    values += string.Format(":{0}, ", column[i]);
                }
            }

            using (PgSqlConnection conn = new PgSqlConnection(strconnection))
            {
                try
                {
                    sql = string.Format("{0}{1} ON CONFLICT DO NOTHING", insert, values);
                    conn.Open();
                    trans = conn.BeginTransaction();
                    cmd.Transaction = trans;
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    foreach (DataRow r in dt.Rows)
                    {
                        cmd.Parameters.Clear();

                        foreach (DataColumn c in dt.Columns)
                            cmd.Parameters.Add(c.ColumnName, r[c.ColumnName]);
                        ctr += cmd.ExecuteNonQuery();
                    }
                }
                catch (PgSqlException pgex)
                {
                    if (pgex.ErrorCode == "23505")
                    {
                        sinfo.ErrFound = true;
                        sinfo.Message = string.Format("Error in saving: it may create duplicate value in table {0}. " + "Please contact Database Administrator.", dt.TableName.ToString());
                        trans.Rollback();
                        return sinfo;
                    }
                    else
                    {
                        sinfo.ErrFound = true;
                        sinfo.Message = pgex.Message.ToString();
                    }
                }
                catch (Exception ex)
                {
                    sinfo.ErrFound = true;
                    sinfo.Message = ex.Message.ToString();
                }
                if (ctr != 0)
                    trans.Commit();
                else
                    trans.Rollback();
            }
            return sinfo;
        }


        public static void CopyExcelDocuments(string filedirectory, string filename)
        {
            //save a copy of the file to shareddirectory
            string sourcefile;
            string destfile;

            if (Directory.Exists(Properties.Settings.Default.SharedRepositoryFileDirectory) == true)
            {
                sourcefile = Path.Combine(System.IO.Path.GetDirectoryName(filedirectory), filename);
                destfile = Path.Combine(Properties.Settings.Default.SharedRepositoryFileDirectory, filename);

                if (File.Exists(destfile) == true)
                {
                    XtraMessageBox.Show("File already exists in shared folder directory", "Attachment Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                    File.Copy(sourcefile, destfile, false);
            }
            else
            {
                XtraMessageBox.Show(String.Format("Destination shared folder did not exist: Please create the directory in the target computer as indicated"), "Attachment Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }


        public static object getMaxNumber(string strconnection, string tablename, string fieldname)
        {
            string sql;
            NpgsqlCommand cmd = new NpgsqlCommand();
            using (NpgsqlConnection conn = new NpgsqlConnection(strconnection))
            {
                conn.Open();
                sql = string.Format("select max({0}) FROM {1}", fieldname, tablename);
                try
                {
                    cmd = new NpgsqlCommand(sql, conn);
                    //return cmd.ExecuteScalar() as int? ?? 0;
                    return cmd.ExecuteScalar() as object ?? 0;
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message.ToString());
                    return 0;
                }
            }
        }

        public static bool IsDBNull(object value)
        {
            if (value == System.DBNull.Value) return true;
            IConvertible convertible = value as IConvertible;
            return convertible != null ? convertible.GetTypeCode() == TypeCode.DBNull : false;
        }

        public static object ReplaceNull(object value)
        {
            if (IsDBNull(value) == true)
            {
                return 0;
            }
            else
            {
                return value;
            }
        }

        // Compute the Billable and Non-billable hour of the Agent
        // Parameter Records, AgentID, LOBID, TLID, Date Start, Date End
        //public static string ComputeAgentProd(DataRow[] AATrow, string AgentID, string LOBDID, string TLID, DateTime DateStart, DateTime DateEnd)
        public static string ComputeAgentProd(DataRow[] AATrow, string _shiftday)
        {
            string computedBB = string.Empty;
            bool getstart = false;
            bool getend = false;
            bool Act_SignOff = true;
            DateTime shiftDate;
            DateTime _start;
            DateTime _end;
            DateTime holdBillable;
            DateTime holdNBillable;
            string tempB = string.Empty;
            string tempNB = string.Empty;
            string acttype = string.Empty;
            string acttype2 = string.Empty;
            string actdesc = string.Empty;
            string actdesc2 = string.Empty;
            string descact = string.Empty;
            string agentshift = string.Empty;
            int cntr = 0;

            _start = new DateTime();
            _end = new DateTime();
            getstart = false;
            getend = false;
            holdBillable = new DateTime();
            holdNBillable = new DateTime();
            agentshift = _shiftday;

            foreach (DataRow dr in AATrow)
            {
                cntr += 1;
                if (getstart == false)
                {
                    getstart = true;
                    actdesc = dr["_desc"].ToString();
                    acttype = dr["_type"].ToString();
                    shiftDate = (DateTime)dr["_shiftdate"];
                    _start = DateTime.Parse(dr["_timeexec"].ToString());
                    //stID = Int64.Parse(dr["_aaid"].ToString());
                }
                else
                {
                    getend = true;
                    actdesc2 = dr["_desc"].ToString();
                    acttype2 = dr["_type"].ToString();
                    shiftDate = (DateTime)dr["_shiftdate"];
                    _end = DateTime.Parse(dr["_timeexec"].ToString());
                    //etID = Int64.Parse(dr["_aaid"].ToString());
                }

                if (getend == true)
                {
                    if (actdesc.Contains("Sign"))
                    {
                        if (cntr != AATrow.Length)
                        {
                            _end = Functions.checkValidRangeDate(_start, _end);
                            if (getActType(acttype, acttype2) == "Billable")
                            { holdBillable = holdBillable.Add(_end.Subtract(_start)); }
                            else
                            { holdNBillable = holdNBillable.Add(_end.Subtract(_start)); }

                            _start = _end;
                            actdesc = actdesc2;
                            acttype = acttype2;
                        }
                        else
                        {
                            _start = _end;
                            actdesc = actdesc2;
                            acttype = acttype2;
                        }
                    }
                    else
                    {
                        _end = Functions.checkValidRangeDate(_start, _end);
                        if (getActType(acttype, acttype2) == "Billable")
                        { holdBillable = holdBillable.Add(_end.Subtract(_start)); }
                        else
                        { holdNBillable = holdNBillable.Add(_end.Subtract(_start)); }

                        _start = _end;
                        actdesc = actdesc2;
                        acttype = acttype2;
                    }
                }

                if (cntr == AATrow.Length)
                {
                    if (!actdesc.Contains("Sign"))
                    {
                        string[] lenstr = agentshift.Split(new[] { " - " }, StringSplitOptions.None);
                        string[] tslogin = lenstr[0].Split(new[] { ":" }, StringSplitOptions.None);
                        string[] tslogout = lenstr[1].Split(new[] { ":" }, StringSplitOptions.None);
                        DateTime _newEnd = new DateTime();

                        // Note: Last row activity has to be check had sign off per shift date
                        // Check Activity without sign off in between shift date [PH, US / Swing]
                        // If current/present day occurs {get; set} server time
                        // Else aging {set; } actual logout or new datetime based on end shift time
                        // Variable : Shift_Type, Shift_Time_Out, Last_Activity_DateTime, SignOff_Activity_DateTime, Return Status TRUE / FALSE
                        // -------------------------------------------------------------------------
                        // Return value TRUE - succeed insert dataset dtAgentSummary else FALSE - skip insert dataset dtAgentSummary

                        if (int.Parse(tslogin[0]) > int.Parse(tslogout[0]))
                        {
                            //US shift
                            string holdlogin = getDTRLog(dr["_agentid"].ToString(), "O", shiftDate.Date.AddDays(1));
                            if (holdlogin != "")
                            {
                                Act_SignOff = checkSignOffShift(false, shiftDate, int.Parse(tslogout[0]), _start, _newEnd, Globals.globalShiftDate);
                                _newEnd = Convert.ToDateTime(holdlogin);
                            }
                            else
                            {
                                Act_SignOff = false;
                            }
                        }
                        else
                        {
                            //PH shift
                            string holdlogin = getDTRLog(dr["_agentid"].ToString(), "O", shiftDate.Date);
                            if (holdlogin != "")
                            {
                                Act_SignOff = checkSignOffShift(false, shiftDate, int.Parse(tslogout[0]), _start, _newEnd, Globals.globalShiftDate);
                                _newEnd = Convert.ToDateTime(holdlogin);
                            }
                            else
                            {
                                Act_SignOff = false;
                            }
                        }
                        // Check value = TRUE to go here to compute time of Billable and Non-billable
                        if (Act_SignOff == true)
                        {
                            _newEnd = Functions.checkValidRangeDate(_start, _newEnd);
                            if (getActType(acttype, acttype2) == "Billable")
                            { holdBillable = holdBillable.Add(_newEnd.Subtract(_start)); }
                            else
                            { holdNBillable = holdNBillable.Add(_newEnd.Subtract(_start)); }
                            Act_SignOff = true;
                        }
                    }
                }
            }
            tempB = string.Format("{0:HH:mm:ss}", holdBillable);
            tempNB = string.Format("{0:HH:mm:ss}", holdNBillable);
            return computedBB = tempB + ";" + tempNB;
        }

        private static string getActType(string refval1, string refval2)
        {
            if (refval1 == refval2)
            { return refval2; }
            else
            { return refval1; }
        }

        public static DateTime getFirstDay(string _day, DateTime dtpfrom)
        {
            DateTime _getFirstday = new DateTime();
            switch (_day)
            {
                case "Tue":
                    _getFirstday = dtpfrom.AddDays(-1);
                    break;
                case "Wed":
                    _getFirstday = dtpfrom.AddDays(-2);
                    break;
                case "Thu":
                    _getFirstday = dtpfrom.AddDays(-3);
                    break;
                case "Fri":
                    _getFirstday = dtpfrom.AddDays(-4);
                    break;
                case "Sat":
                    _getFirstday = dtpfrom.AddDays(-5);
                    break;
                case "Sun":
                    _getFirstday = dtpfrom.AddDays(-6);
                    break;
            }
            return _getFirstday;
        }

        public static DateTime getLastDay(string _day, DateTime dtpto)
        {
            DateTime _getLastday = new DateTime();
            switch (_day)
            {
                case "Mon":
                    _getLastday = dtpto.AddDays(6);
                    break;
                case "Tue":
                    _getLastday = dtpto.AddDays(5);
                    break;
                case "Wed":
                    _getLastday = dtpto.AddDays(4);
                    break;
                case "Thu":
                    _getLastday = dtpto.AddDays(3);
                    break;
                case "Fri":
                    _getLastday = dtpto.AddDays(2);
                    break;
                case "Sat":
                    _getLastday = dtpto.AddDays(1);
                    break;
            }
            return _getLastday;
        }

        public static string getRequestCalendar()
        {
            string _id = string.Empty;
            string value = string.Empty;
            Int32 result = 0;
            Int32 result2 = 0;
            Int32 result3 = 0;
            string sqlOT = string.Empty;
            string sqlAB = string.Empty;
            string sqlSC = string.Empty;

            if (CheckEmpRole(Globals.userempid, Globals.userrole) == true)
            {
                sqlOT = string.Format("SELECT COUNT(_id) FROM forapprovalovertimelist WHERE _isapproved=false");
                sqlAB = string.Format("SELECT COUNT(_id) FROM forapprovalleavedlist WHERE _isapproved=false");
                sqlSC = string.Format("SELECT COUNT(_id) FROM forapprovalschedchange WHERE _isapproved=false");
            }
            else
            {
                sqlOT = string.Format("SELECT COUNT(_id) FROM forapprovalovertimelist WHERE _isapproved=false AND _tl = '{0}'", Globals.userempid);
                sqlAB = string.Format("SELECT COUNT(_id) FROM forapprovalleavedlist WHERE _isapproved=false AND _tl = '{0}'", Globals.userempid);
                sqlSC = string.Format("SELECT COUNT(_id) FROM forapprovalschedchange WHERE _isapproved=false AND _tl = '{0}'", Globals.userempid);
            }

            using (var conn = new NpgsqlConnection(Globals.connstring))
            {
                conn.Open();
                {
                    using (var command = new NpgsqlCommand(sqlOT, conn))
                    {
                        var obj = command.ExecuteScalar();
                        result = Convert.ToInt32(obj);
                    }

                    using (var command2 = new NpgsqlCommand(sqlAB, conn))
                    {
                        var obj2 = command2.ExecuteScalar();
                        result2 = Convert.ToInt32(obj2);
                    }

                    using (var command3 = new NpgsqlCommand(sqlSC, conn))
                    {
                        var obj3 = command3.ExecuteScalar();
                        result3 = Convert.ToInt32(obj3);
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return result + ";" + result2 + ";" + result3;
        }

        public static int getIRForApprovalSum()
        {
            string _id = string.Empty;
            string value = string.Empty;
            Int32 result = 0;
            Int32 rscrb = 0;
            string sqllob = string.Empty;
            string sqlpreapp = string.Empty;
            bool isAuthorized = CheckEmpRole(Globals.userempid, Globals.userrole);
            if (isAuthorized == true)
            {
                sqllob = @"select COUNT(*) from aat_irmaster where irscrbstat='PENDING'";
                sqlpreapp = @"SELECT COUNT(*) FROM aat_irlist WHERE irstatus = true AND irscrbstat='SCRUBBED'";
            }
            else
            {
                sqllob = string.Format(@"select COUNT(*) from aat_irmaster where irscrbstat='PENDING' and irtlid='{0}'", Globals.userempid);
                sqlpreapp = string.Format(@"SELECT COUNT(*) FROM aat_irlist WHERE irstatus = true AND irscrbstat='SCRUBBED'and irtlid='{0}'", Globals.userempid);
            }

            using (var conn = new NpgsqlConnection(Globals.connstring))
            {
                conn.Open();
                {
                    using (var command = new NpgsqlCommand(sqllob, conn))
                    {
                        var obj1 = command.ExecuteScalar();
                        result = Convert.ToInt32(obj1);
                    }

                    using (var command = new NpgsqlCommand(sqlpreapp, conn))
                    {
                        var obj2 = command.ExecuteScalar();
                        rscrb = Convert.ToInt32(obj2);
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            if (isAuthorized == true)
            { return result + rscrb; }
            else { return result; }
        }

        public static int getScrubForApprovalSum()
        {
            string _id = string.Empty;
            string value = string.Empty;
            Int32 result = 0;
            string sqllob = string.Empty;

            sqllob = @"SELECT COUNT(*) FROM IRfiledrequest WHERE irscrbstat='SCRUBBED'";

            using (var conn = new NpgsqlConnection(Globals.connstring))
            {
                conn.Open();
                {
                    using (var command = new NpgsqlCommand(sqllob, conn))
                    {
                        var obj = command.ExecuteScalar();
                        result = Convert.ToInt32(obj);
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return result;
        }

        public static int getForApprovalSum()
        {
            string _id = string.Empty;
            string value = string.Empty;
            Int32 result = 0;
            string sqllob = string.Empty;

            if (CheckEmpRole(Globals.userempid, Globals.userrole) == true)
            {
                sqllob = string.Format("select * from forapprovalsummary", Globals.userempid);
            }
            else
            {
                sqllob = string.Format("select * from forapprovalsummary('{0}')", Globals.userempid);
            }

            using (var conn = new NpgsqlConnection(Globals.connstring))
            {
                conn.Open();
                {
                    using (var command = new NpgsqlCommand(sqllob, conn))
                    {
                        var obj = command.ExecuteScalar();
                        result = Convert.ToInt32(obj);
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return result;
        }

        public static bool IsValidTimeFormat(this string input)
        {
            TimeSpan dummyOutput;
            return TimeSpan.TryParse(input, out dummyOutput);
        }

        public static int UpdateRequestCalendar(int _refID, string _UID, int _tag, string _type, DateTime _StartDate, DateTime _EndDate, decimal _ReqHrsDay, string _Sched, string _Reqby, string _ReasonApp)
        {
            int result = 0;

            NpgsqlConnection conn = new NpgsqlConnection(Globals.connstring);
            NpgsqlCommand cmd;

            try
            {
                string sqlinsert = @"SELECT * FROM _editrequestcalendar(:_id, :_uid, :_tag, :_type, :_datefrom, :_dateto, :_noofdays, :_sched, :_request, :_reason)";
                cmd = new NpgsqlCommand(sqlinsert, conn);
                cmd.Parameters.AddWithValue("_id", _refID);
                cmd.Parameters.AddWithValue("_uid", _UID);
                cmd.Parameters.AddWithValue("_tag", _tag);
                cmd.Parameters.AddWithValue("_type", _type);
                cmd.Parameters.AddWithValue("_datefrom", _StartDate);
                cmd.Parameters.AddWithValue("_dateto", _EndDate);
                cmd.Parameters.AddWithValue("_noofdays", _ReqHrsDay);
                cmd.Parameters.AddWithValue("_sched", _Sched);
                cmd.Parameters.AddWithValue("_request", _Reqby);
                cmd.Parameters.AddWithValue("_reason", _ReasonApp);
                conn.Open();
                result = (int)cmd.ExecuteScalar();
                conn.Close();
                return result;
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }
    }
}
