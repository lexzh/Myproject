namespace Bussiness
{
    using DataAccess;
    using Library;
    using System;
    using System.Data;

    public class ReceiveDataBase
    {
        public string GetACCStatus(int int_0)
        {
            string str = "关";
            if ((int_0 & 0x4000) != 0)
            {
                str = "开";
            }
            return str;
        }

        protected string GetAddTextContext(string string_0)
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(string_0))
            {
                return str;
            }
            string[] strArray = string_0.Split(new char[] { '/' });
            int num = -1;
            float num2 = -1f;
            int index = 0;
        Label_0032:
            if (index > (strArray.Length - 1))
            {
                return str;
            }
            try
            {
                num = int.Parse(strArray[index].Substring(0, 1));
                num2 = float.Parse(strArray[index].Substring(1));
                switch (num)
                {
                    case 0:
                        str = str + string.Format("[下车：{0}],", num2);
                        goto Label_0107;

                    case 1:
                        str = str + string.Format("[当前油量：{0}升],", num2 / 100f);
                        goto Label_0107;

                    case 2:
                        str = str + string.Format("[当前温度：{0}度],", num2 / 100f);
                        goto Label_0107;

                    case 9:
                        str = str + string.Format("[上车：{0}],", num2);
                        goto Label_0107;
                }
                str = str + string.Format("[未知值：{0}],", num2);
            }
            catch
            {
            }
        Label_0107:
            index++;
            goto Label_0032;
        }

        protected string GetCommFlagName(int int_0)
        {
            string str = string.Empty;
            switch (int_0)
            {
                case 0:
                    return "短信";

                case 1:
                    return "GPRS/CDMA";

                case 2:
                    return "混合方式";
            }
            return str;
        }

        public string GetDBCurrentDateTime()
        {
            SqlDataAccess access = new SqlDataAccess();
            return access.getSystemDate().ToString("yyyy-MM-dd HH:mm:ss");
        }

        protected DateTime GetDbTime(SqlDataAccess sqlDataAccess_0)
        {
            DateTime now = DateTime.Now;
            try
            {
                now = sqlDataAccess_0.getSystemDate();
            }
            catch
            {
            }
            return now;
        }

        public int GetDrInt(DataRow dataRow_0, string string_0)
        {
            return Convert.ToInt32(dataRow_0[string_0]);
        }

        protected long GetDrInt64(DataRow dataRow_0, string string_0)
        {
            return Convert.ToInt64(dataRow_0[string_0]);
        }

        protected string GetDrStr(DataRow dr, string name)
        {
            return Convert.ToString(dr[name]);
        }

        protected string GetOrderName(int int_0)
        {
            switch (int_0)
            {
                case 0x41:
                    return "实时定位信息报文";

                case 0x42:
                    return "压缩定位信息报文";
            }
            return "未知报文类型";
        }

        protected string GetSysDateIsNull(string string_0)
        {
            string currentDateTime = string_0;
            DateHelper helper = new DateHelper();
            if (string.IsNullOrEmpty(string_0))
            {
                currentDateTime = helper.GetCurrentDateTime();
            }
            return currentDateTime;
        }

        public DateTime getSystemDate()
        {
            SqlDataAccess access = new SqlDataAccess();
            return access.getSystemDate();
        }

        public string GetTransportStatus(int int_0)
        {
            switch (int_0)
            {
                case 0:
                    return "空车";

                case 1:
                    return "停运";

                case 2:
                    return "预约";

                case 3:
                    return "重车";

                case 4:
                    return "营运";
            }
            return "关";
        }

        protected bool IsCancelAlarm(int int_0)
        {
            if ((((int_0 != 8) && (int_0 != 9)) && ((int_0 != 10) && (int_0 != 0x485))) && ((int_0 != 0x1d3) && (int_0 != 0x1d2)))
            {
                return false;
            }
            return true;
        }

        public bool isPosStatus(int int_0)
        {
            return ((int_0 & 1) != 0);
        }
    }
}

