namespace Bussiness
{
    using Library;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    public sealed class UserLoginInterface
    {
        public string Ip;
        private LoginOnlieUserInfo loginOnlieUserInfo;
        private LoginUserInfo loginUserInfo;
        public string ModuleId;
        private const int offDays = 5;
        public int PreWorkId;
        private string userId;
        private string userPw;

        public UserLoginInterface(string userId, string userPw)
        {
            this.Ip = string.Empty;
            this.PreWorkId = -1;
            this.ModuleId = string.Empty;
            this.userId = string.Empty;
            this.userPw = string.Empty;
            this.userId = userId;
            this.userPw = userPw;
            this.loginUserInfo = new LoginUserInfo(userId, userPw);
            this.loginOnlieUserInfo = new LoginOnlieUserInfo(userId);
        }

        public UserLoginInterface(string userId, string userPw, string moduleId) : this(userId, userPw)
        {
            this.ModuleId = moduleId;
        }

        public UserLoginInterface(string userId, string userPw, string sIp, string moduleId, int PreWorkId) : this(userId, userPw)
        {
            this.Ip = sIp;
            this.PreWorkId = PreWorkId;
            this.ModuleId = moduleId;
        }

        public bool CheckUserPasswordOutDays(out string string_2)
        {
            string_2 = string.Empty;
            int configPasswordOutDays = SystemConfiger.GetConfigPasswordOutDays();
            int passwordOutDay = this.loginUserInfo.GetPasswordOutDay();
            if ((passwordOutDay >= (configPasswordOutDays - 5)) && (passwordOutDay < configPasswordOutDays))
            {
                string_2 = "用户密码即将过期，请修改密码！";
            }
            if (passwordOutDay >= configPasswordOutDays)
            {
                string_2 = "用户密码过期，请修改密码！";
                return false;
            }
            return true;
        }

        public bool IsCutUser()
        {
            DataTable table = this.method_1();
            if ((table == null) || (table.Rows.Count <= 0))
            {
                return false;
            }
            if (table.Rows[0]["IsStop"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (table.Rows[0]["UserModule"].ToString().Equals("0", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            DataTable table2 = this.method_2();
            return ((table2 != null) && (table2.Rows.Count > 0));
        }

        public bool Login(ref UserInfoEntity userInfoEntity_0, ref string string_2)
        {
            DataTable table = this.method_1();
            if ((table != null) && (table.Rows.Count > 0))
            {
                if (table.Rows[0]["IsStop"].ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    string_2 = "该用户已停用，请检查!";
                    return false;
                }
                if (table.Rows[0]["UserModule"].ToString().Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    string_2 = "该用户没有访问该系统权限，请检查!";
                    return false;
                }
                DataTable table2 = this.method_2();
                if ((table2 != null) && (table2.Rows.Count > 0))
                {
                    int num = this.loginOnlieUserInfo.CreateLoginUserWorkId(this.Ip, this.PreWorkId);
                    userInfoEntity_0 = this.method_0(table, table2, num);
                    return true;
                }
                string_2 = "读取用户权限参数为空！";
                return false;
            }
            string_2 = "用户名或密码错误，请检查!";
            return false;
        }

        private UserInfoEntity method_0(DataTable dataTable_0, DataTable dataTable_1, int int_0)
        {
            UserInfoEntity entity = new UserInfoEntity {
                UserName = Convert.ToString(dataTable_0.Rows[0]["userName"]),
                AreaCode = Convert.ToString(dataTable_0.Rows[0]["AreaCode"]),
                RoadTransportID = Convert.ToString(dataTable_0.Rows[0]["RoadTransportID"])
            };
            int num = Convert.ToInt32(dataTable_1.Rows[0]["MultiplySend"]);
            entity.AllowSelMutil = num == 0;
            int num2 = Convert.ToInt32(dataTable_1.Rows[0]["isNeedPass"]);
            entity.AllowEmptyPw = num2 != 0;
            int num3 = Convert.ToInt32(dataTable_1.Rows[0]["SudoOverDue"]);
            entity.SudoOverDue = num3 == 1;
            entity.GroupId = Convert.ToInt32(dataTable_1.Rows[0]["groupID"]);
            entity.WorkId = int_0;
            return entity;
        }

        private DataTable method_1()
        {
            DataTable table = null;
            try
            {
                table = this.loginUserInfo.CreateUserInfomation(this.ModuleId);
            }
            catch (Exception exception)
            {
                LogHelper helper = new LogHelper();
                ErrorMsg msg = new ErrorMsg("login", helper.GetCallFunction(), helper.GetExceptionMsg(exception));
                helper.WriteError(msg, exception);
                throw exception;
            }
            return table;
        }

        private DataTable method_2()
        {
            DataTable table = null;
            try
            {
                table = this.loginUserInfo.CreateUserAuthorInfomation();
            }
            catch (Exception exception)
            {
                LogHelper helper = new LogHelper();
                ErrorMsg msg = new ErrorMsg("login", helper.GetCallFunction(), helper.GetExceptionMsg(exception));
                helper.WriteError(msg);
                throw exception;
            }
            return table;
        }
    }
}

