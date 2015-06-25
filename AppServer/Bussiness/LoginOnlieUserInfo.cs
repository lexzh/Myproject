namespace Bussiness
{
    using DataAccess;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public sealed class LoginOnlieUserInfo
    {
        private int int_0;
        private static object lockObj;
        private const string sp_GetWorkId = "WebGpsClient_GetWorkId";
        private SqlDataAccess sqlDataAccess_0;
        private string string_0;
        private const string strUpdateSql = "WebGpsClient_UpdateUserOnline";

        static LoginOnlieUserInfo()
        {
            old_acctor_mc();
        }

        public LoginOnlieUserInfo()
        {
            this.int_0 = -1;
            this.sqlDataAccess_0 = new SqlDataAccess();
        }

        public LoginOnlieUserInfo(int int_1)
        {
            this.int_0 = -1;
            this.sqlDataAccess_0 = new SqlDataAccess();
            this.WorkId = int_1;
        }

        public LoginOnlieUserInfo(string string_1)
        {
            this.int_0 = -1;
            this.sqlDataAccess_0 = new SqlDataAccess();
            this.UserId = string_1;
        }

        public int CreateLoginUserWorkId(string string_1, int int_1)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@UserId", this.UserId), new SqlParameter("@Ip", string_1), new SqlParameter("@PreWorkId", int_1) };
            int num = 0;
            lock (lockObj)
            {
                DataTable table = this.sqlDataAccess_0.getDataBySP("WebGpsClient_GetWorkId", parameterArray);
                if ((table != null) && (table.Rows.Count > 0))
                {
                    num = Convert.ToInt32(table.Rows[0]["wrkid"]);
                }
            }
            return num;
        }

        public bool IsExistUser()
        {
            if (this.sqlDataAccess_0.GetReturnBySql(this.method_0()) == null)
            {
                return false;
            }
            return true;
        }

        private string method_0()
        {
            return ("select id from gpsuser_online where id = " + this.WorkId.ToString());
        }

        private static void old_acctor_mc()
        {
            lockObj = new object();
        }

        public int UpdateUserOnline()
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@WrokId", this.WorkId) };
            return this.sqlDataAccess_0.updateBySp("WebGpsClient_UpdateUserOnline", parameterArray);
        }

        public string UserId
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }

        public int WorkId
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }
    }
}

