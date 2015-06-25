namespace Bussiness
{
    using DataAccess;
    using Library;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public sealed class LoginUserInfo
    {
        private SqlDataAccess sqlDataAccess_0;
        private string string_0;
        private string string_1;

        public LoginUserInfo()
        {
            this.sqlDataAccess_0 = new SqlDataAccess();
        }

        public LoginUserInfo(string string_2)
        {
            this.sqlDataAccess_0 = new SqlDataAccess();
            this.UserId = string_2;
        }

        public LoginUserInfo(string string_2, string string_3) : this(string_2)
        {
            this.UserPassword = string_3;
        }

        public string ChangePassword(string string_2)
        {
            DataTable table = this.sqlDataAccess_0.getDataBySql(this.method_2());
            if ((table != null) && (table.Rows.Count > 0))
            {
                if (this.sqlDataAccess_0.updateBySql(this.method_4(string_2)) <= 0)
                {
                    return "访问数据库错误!";
                }
                return "";
            }
            DataTable table2 = this.sqlDataAccess_0.getDataBySql(this.method_3());
            if ((table2 == null) || (table2.Rows.Count <= 0))
            {
                return "用户密码错误!";
            }
            if (this.sqlDataAccess_0.updateBySql(this.method_4(SecurityHelper.EncryptMD5String(string_2))) <= 0)
            {
                return "访问数据库错误!";
            }
            return "";
        }

        public DataTable CreateUserAuthorInfomation()
        {
            return this.sqlDataAccess_0.getDataBySql(this.method_6());
        }

        public DataTable CreateUserInfomation(string string_2)
        {
            string str = this.method_5(string_2);
            DataTable table = this.method_7(str);
            if ((table != null) && (table.Rows.Count > 0))
            {
                return table;
            }
            return this.method_8(str);
        }

        public int GetPasswordOutDay()
        {
            int days = -1;
            DataTable table = this.sqlDataAccess_0.getDataBySql(this.method_0());
            if ((table != null) && (table.Rows.Count > 0))
            {
                DateTime time = Convert.ToDateTime(table.Rows[0]["pwdUpdate"]);
                TimeSpan span = (TimeSpan) (DateTime.Now - time);
                days = span.Days;
            }
            return days;
        }

        public string GetUserName()
        {
            DataTable table = this.sqlDataAccess_0.getDataBySql(this.method_1());
            if ((table != null) && (table.Rows.Count > 0))
            {
                return table.Rows[0]["UserName"].ToString();
            }
            return string.Empty;
        }

        public string GetUserPassword(string string_2)
        {
            string format = "select password from gpsUser where userId= '{0}'";
            DataTable table = this.sqlDataAccess_0.getDataBySql(string.Format(format, string_2));
            if ((table != null) && (table.Rows.Count > 0))
            {
                return table.Rows[0]["password"].ToString();
            }
            return string.Empty;
        }

        public bool IsUserButCUser()
        {
            bool flag;
            try
            {
                string format = "SELECT UserID FROM GpsUser WHERE IsStop = 0 and UserID= '{0}' and PassWord ='{1}'";
                format = string.Format(format, this.UserId, this.UserPassword);
                if (this.sqlDataAccess_0.getDataBySql(format).Rows.Count != 0)
                {
                    return true;
                }
                flag = false;
            }
            catch (Exception exception)
            {
                LogHelper helper = new LogHelper();
                ErrorMsg msg = new ErrorMsg("login", helper.GetCallFunction(), helper.GetExceptionMsg(exception));
                helper.WriteError(msg, exception);
                throw exception;
            }
            return flag;
        }

        private string method_0()
        {
            string format = "Select a.pwdUpdate From GpsUser a where a.UserId ='{0}' and a.Password = '{1}' ";
            return string.Format(format, this.UserId, this.UserPassword);
        }

        private string method_1()
        {
            return string.Format("Select a.UserId, a.UserName From GpsUser a where a.UserId='{0}'", this.string_0);
        }

        private string method_2()
        {
            return string.Format("select userId from gpsUser where userId= '{0}'and password='{1}'", this.UserId, this.UserPassword);
        }

        private string method_3()
        {
            return string.Format("select userId from gpsUser where userId= '{0}'and password='{1}'", this.UserId, SecurityHelper.EncryptMD5String(this.string_1));
        }

        private string method_4(string string_2)
        {
            return string.Format("update gpsUser set password='{0}',pwdUpdate = getdate() where userid='{1}'", string_2, this.UserId);
        }

        private string method_5(string string_2)
        {
            string format = "Select UserId, UserName, GroupId, b.AreaCode,isnull(b.RoadTransportID,'') as RoadTransportID,isnull(IsStop, 0) as IsStop,UserModule&{0} as UserModule From GpsUser a left join GpsArea b on a.AreaId=b.AreaId where UserId=@UserId and Password=@Password ";
            return string.Format(format, string_2);
        }

        private string method_6()
        {
            string format = "SELECT isnull(MultiplySend,0) as MultiplySend, isnull(isNeedPass,0) as isNeedPass, isnull(AllowSetParm,0) as AllowSetParm, UserModule, areaCode, groupID,isnull(SudoOverDue,0) as SudoOverDue FROM view_GpsUser where userId='{0}'";
            return string.Format(format, this.UserId);
        }

        private DataTable method_7(string string_2)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@UserId", this.UserId), new SqlParameter("@Password", this.UserPassword) };
            return this.sqlDataAccess_0.getDataBySqlParam(string_2, parameterArray);
        }

        private DataTable method_8(string string_2)
        {
            SqlParameter[] parameterArray = new SqlParameter[2];
            parameterArray[0] = new SqlParameter("@UserId", this.UserId);
            string str = SecurityHelper.EncryptMD5String(this.string_1);
            parameterArray[1] = new SqlParameter("@Password", str);
            return this.sqlDataAccess_0.getDataBySqlParam(string_2, parameterArray);
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

        public string UserPassword
        {
            get
            {
                return this.string_1;
            }
            set
            {
                this.string_1 = value;
            }
        }
    }
}

