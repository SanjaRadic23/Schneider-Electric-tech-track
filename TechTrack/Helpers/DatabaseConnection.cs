using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;


namespace TechTrack.Helpers
{
    public class DatabaseConnection
    {
        
        public static readonly string LOCAL_DATA_SOURCE = "//localhost:1521/xepdb1";

        public static readonly string USER_ID = "system";
        public static readonly string PASSWORD = "1234";

    }
}
