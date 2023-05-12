using Npgsql;
using System;
using System.Linq;

namespace SLC_DE_App
{
    public static class Globals
    {
        public static NpgsqlConnection connTerminal;
        public static NpgsqlTransaction transact;
        public static string ServerIP;

        //public static string connstring = string.Format("Server={0};Port={1}; User Id={2};Password={3};Database={4};", "192.168.128.175", 5432, "postgres", "Password1", "OP360");
        // static string connstring = String.Format("User Id=postgres;Password=Password1;Host=localhost;Database=SLCDB;Initial Schema=public");

        //public static string connstring = string.Format("Server={0};Port={1};" +
        //                                                "User Id={2};Password={3};Database={4};",
        //    "127.0.0.1", 5432, "postgres", "abcde", "OP360");

        public static string connstring = string.Format("Server={0};Port={1};" +
                                                        "User Id={2};Password={3};Database={4};",
                                                        "127.0.0.1", 5432, "postgres", "Password1", "SLCDB");


        public static Int64 userid;//0
        public static string username;//1
        public static string userpassword;//2
        public static string userrole;//3
        public static string userempid;//4
        public static string userfullname;//5
        public static string useraccntid;//6
        public static string userACCNT;//7
        public static string userdeptid;//8
        public static string userdeptname;//9
        public static string userlob1;//10
        public static string userLOBname;//11
        public static string userlob2;//12
        public static string userlobname2;//13
        public static string userlob3;//14
        public static string userlobname3;//15
        public static string usergroupid;//16
        public static string useremptype;//17
        public static string usertl;//18
        public static string usertlname;//19
        public static string usershiftype;//20
        public static string usershiftsched;//21
        public static string usershiftrd;//22
        public static bool userstatus;//23
        public static string empopid;//24
        public static string empoldid;//25
        public static bool? leavestatus;//26
        public static int? leaves_ID;//27
        public static string uipaddr;
        public static DateTime globalShiftDate;
        public static string BioDevice;
        public static bool isLocal = false;


        // Assign Global User Access
        public static bool isCreate;
        public static bool isEdit;
        public static bool isDelete;
        public static bool isCancel;
        public static bool isView;

        public static DateTime mainDateTime;
        // Assign Global LOB name

        public static bool isAllowOnline = true;
        //public static Main _frmMain;

        public static Int16 comAct1;
        public static Int16 comAct2;
        public static Int16 comAct3;
        public static Int16 comAct4;

        public static string comActDesc1;
        public static string comActDesc2;
        public static string comActDesc3;
        public static string comActDesc4;

        //public static ConnectionInfo _connectionInfo;
        //public static ForwardedPort port;
        //public static SshClient _sshClient;

        public static string _host;
        public static int _portno;
        public static string _username;
        public static string _password;
        public static uint _fport;
        public static string _localhost;
        public static uint _DBport;
        public static string _pingadd;
        public static uint _connectAttempt;

        public static string _hiddenpath;
        public static int _tlDue;
        public static int _stlDue;
        public static int _hopsDue;
        public static int _retention;
        public static int _SSTimer;
        public static bool _KillSS;

        public static string _ssact;
        public static string _uversion;
    }
}
