namespace Protocol
{
    using DataAccess;
    using Library;
    using System;
    using System.Collections;
    using System.Data;

    public class AskConfigerParameter
    {
        private static Hashtable _userAreacodeList;
        /// <summary>
        /// 营运证表
        /// </summary>
        private static Hashtable _userIDCarList;

        static AskConfigerParameter()
        {
            _userAreacodeList = new Hashtable(100);
            _userIDCarList = new Hashtable(100);
            DataSet set = FileHelper.ReadDataXml("AckList.xml");
            if (((set != null) && (set.Tables.Count > 0)) && (set.Tables[0].Rows.Count > 0))
            {
                string str = string.Empty;
                string str2 = string.Empty;
                string str3 = string.Empty;
                try
                {
                    foreach (DataRow row in set.Tables[0].Rows)
                    {
                        str = row["name"].ToString();
                        str2 = row["value1"].ToString().Trim();
                        str3 = row["value2"].ToString().Trim();
                        if (!string.IsNullOrEmpty(str) && !_userIDCarList.ContainsKey(str))
                        {
                            _userIDCarList.Add(str, str2);
                        }
                        if (!string.IsNullOrEmpty(str3) && !_userAreacodeList.ContainsKey(str3))
                        {
                            _userAreacodeList.Add(str3, str);
                        }
                    }
                }
                catch (Exception exception)
                {
                    string message = exception.Message;
                }
            }
        }

        public static string GetAutoRelyInfoString()
        {
            string str = "select distinct a.userId from GpsUser_Online a";
            SqlDataAccess access = new SqlDataAccess();
            DataTable table = null;
            try
            {
                table = access.getDataBySql(str);
            }
            catch
            {
            }
            if ((table == null) || (table.Rows.Count <= 0))
            {
                return string.Empty;
            }
            string str2 = string.Empty;
            string key = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                key = row["userid"] as string;
                if (_userIDCarList.ContainsKey(key))
                {
                    string str4 = str2;
                    str2 = str4 + _userIDCarList[key].ToString() + "|" + key + " ";
                }
            }
            return str2;
        }

        public static string GetUserIdByRegionCode(string string_0)
        {
            return (_userAreacodeList[string_0] as string);
        }

        public static string GetUserIdByYYZCarId(string string_0)
        {
            string str = string.Empty;
            //string str2 = string.Empty;
            //using (IDictionaryEnumerator enumerator = _userIDCarList.GetEnumerator())
            ////IEnumerator enumerator = _userIDCarList.Keys.GetEnumerator();
            //{
            //    string current;
            //    while (enumerator.MoveNext())
            //    {
            //        current = (string) enumerator.Current;
            //        str2 = _userIDCarList[current] as string;
            //        if (string_0.Equals(str2))
            //        {
            //            str = current;
            //            break;
            //        }
            //    }            
            //}

            //System.Diagnostics.Trace.Write("appserver - name: " + string_0);

            foreach (var key in _userIDCarList.Keys)
            {
                //System.Diagnostics.Trace.Write("appserver - key: " + key.ToString() + " value: " + _userIDCarList[key].ToString());
                if (string_0.Equals(_userIDCarList[key].ToString()))
                {
                    str = key.ToString();
                    //LogMsg log = new LogMsg("AskConfigerParameter", "GetUserIdByYYZCarId", "key: " + str);
                    //new LogHelper().WriteLog(log);
                    break;
                }
            }
            
            //foreach (DictionaryEntry de in _userIDCarList)
            //{
            //    System.Diagnostics.Trace.Write("appserver - key: " + de.Key.ToString() + " value: " + de.Value.ToString());
            //    if (string_0.Equals(de.Value.ToString()))
            //    {
            //        str = de.Key.ToString();
            //        LogMsg log = new LogMsg("AskConfigerParameter", "GetUserIdByYYZCarId", "key: " + str);
            //        new LogHelper().WriteLog(log);
            //    }
            //}
               
            return str;
        }

        public static string GetWorkIdByUserId(string string_0)
        {
            string format = "select top 1 a.Id from GpsUser_Online a where a.userId = '{0}' order by logintime desc";
            format = string.Format(format, string_0);
            SqlDataAccess access = new SqlDataAccess();
            DataTable table = null;
            try
            {
                table = access.getDataBySql(format);
            }
            catch
            {
            }
            if ((table != null) && (table.Rows.Count > 0))
            {
                return table.Rows[0]["Id"].ToString();
            }
            return string.Empty;
        }

        public static int InsertCommandParameterToDB(string string_0, int int_0, int int_1, string string_1)
        {
            string format = "insert into GPSJTBSysSndCmd(SimNum,CmdCode,CmdContent,OrderId,AddTime,bSend) values('{0}', {1}, '{2}',{3}, getdate(),-1)";
            string str2 = string.Format(format, new object[] { string_0, int_1, string_1, int_0 });
            SqlDataAccess access = new SqlDataAccess();
            return access.insertBySql(str2);
        }

    }
}

