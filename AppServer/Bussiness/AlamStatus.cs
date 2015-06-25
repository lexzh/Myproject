namespace Bussiness
{
    using DataAccess;
    using Library;
    using System;
    using System.Collections;
    using System.Data;
    using System.Text;

    public class AlamStatus
    {
        private static Hashtable _AlarmStatuBuffer;
        private static Hashtable _AlarmStatuBufferExt;
        private static DataTable _CarStatusList;
        private static DataTable _CarStatusListExt;

        static AlamStatus()
        {
            _CarStatusListExt = new DataTable();
            _CarStatusList = new DataTable();
            _AlarmStatuBuffer = Hashtable.Synchronized(new Hashtable(0xa3));
            _AlarmStatuBufferExt = Hashtable.Synchronized(new Hashtable(0xa3));
        }

        private static void AddBufferStatuName(long status, string statuName)
        {
            if ((_AlarmStatuBuffer != null) && !_AlarmStatuBuffer.ContainsKey(status))
            {
                if (_AlarmStatuBuffer.Count >= 100)
                {
                    _AlarmStatuBuffer.Clear();
                }
                _AlarmStatuBuffer.Add(status, statuName);
            }
        }

        private static void AddBufferStatuNameExt(long long_0, string string_0)
        {
            if ((_AlarmStatuBufferExt != null) && !_AlarmStatuBufferExt.ContainsKey(long_0))
            {
                if (_AlarmStatuBufferExt.Count >= 100)
                {
                    _AlarmStatuBufferExt.Clear();
                }
                _AlarmStatuBufferExt.Add(long_0, string_0);
            }
        }

        public static ArrayList GetAlarmCodeExList(long long_0)
        {
            ArrayList list = new ArrayList();
            if ((_CarStatusListExt != null) && (_CarStatusListExt.Rows.Count > 0))
            {
                long num = 0L;
                foreach (DataRow row in _CarStatusListExt.Rows)
                {
                    num = Convert.ToInt64(row["CarStatuEx"]);
                    if ((num & long_0) != 0L)
                    {
                        list.Add(num);
                    }
                }
            }
            return list;
        }

        public static ArrayList GetAlarmCodeList(int int_0)
        {
            ArrayList list = new ArrayList();
            if ((_CarStatusList != null) && (_CarStatusList.Rows.Count > 0))
            {
                long num = 0L;
                foreach (DataRow row in _CarStatusList.Rows)
                {
                    num = Convert.ToInt64(row["CarStatu"]);
                    if ((num != 1L) && ((num & int_0) != 0L))
                    {
                        list.Add(num);
                    }
                }
            }
            return list;
        }

        private static string GetBufferStatuName(long status)
        {
            string str = string.Empty;
            if ((_AlarmStatuBuffer != null) && _AlarmStatuBuffer.ContainsKey(status))
            {
                str = (string) _AlarmStatuBuffer[status];
            }
            return str;
        }

        private static string GetBufferStatuNameExt(long long_0)
        {
            string str = string.Empty;
            if ((_AlarmStatuBufferExt != null) && _AlarmStatuBufferExt.ContainsKey(long_0))
            {
                str = (string) _AlarmStatuBufferExt[long_0];
            }
            return str;
        }

        /// <summary>
        /// 获取车辆状态名称
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetStatusNameByCarStatu(long status)
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                //从状态列表中查询该状态值是否有对应的状态名称
                string bufferStatuName = GetBufferStatuName(status);
                if (!string.IsNullOrEmpty(bufferStatuName))
                {
                    return bufferStatuName;
                }

                if ((1L & status) == 0L && (0x4000L & status) == 0)
                {
                    builder.Append("停车在线;");
                }
                else if ((1L & status) == 0 && (0x4000L & status) != 0)
                {
                    builder.Append("定位无效;");
                }

                if (!IsNull())
                {
                    long num = 0L;
                    foreach (DataRow row in _CarStatusList.Rows)
                    {
                        num = Convert.ToInt64(row["CarStatu"]);
                        if ((num != 0x4000L) && (num != 1L))
                        {
                            if ((num == 0x20000L) && ((num & status) != 0L))
                            {
                                builder.Append(Convert.ToString(row["CarStatuName"]) + ":开;");
                            }
                            else if ((num == 0x40000L) && ((num & status) != 0L))
                            {
                                builder.Append(Convert.ToString(row["CarStatuName"]) + ":开;");
                            }
                            else if ((num == 0x80000L) && ((num & status) != 0L))
                            {
                                builder.Append(Convert.ToString(row["CarStatuName"]) + ":开;");
                            }
                            else if ((num == 0x400000L) && ((num & status) != 0L))
                            {
                                builder.Append(Convert.ToString(row["CarStatuName"]) + ":开;");
                            }
                            else if ((num & status) != 0L)
                            {
                                builder.Append(Convert.ToString(row["CarStatuName"]) + ";");
                                //修改状态避免同时出现定位无效和定位状态 huzh 2014.2.18 
                                //根据ACC状态和定位状态判断车辆状态
                                //由于华宝设备在ACC关闭后进入休眠，会将定位状态置为无效
                                //所以当ACC关且定位状态无效时，视为停车在线状态
                            }

                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("AlamStatus", "GetStatusNameByCarStatu", exception.Message);
                new LogHelper().WriteError(msg);
            }
            if (string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("卫星定位;");
            }
            else
            {
                AddBufferStatuName(status, builder.ToString());
            }
            //System.Diagnostics.Trace.Write(builder.ToString() + status.ToString() +  "\r\n");
            return builder.ToString();
        }

        public static string GetStatusNameByCarStatuExt(long long_0)
        {
            if (long_0 == 0L)
            {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            try
            {
                string bufferStatuNameExt = GetBufferStatuNameExt(long_0);
                if (!string.IsNullOrEmpty(bufferStatuNameExt))
                {
                    return bufferStatuNameExt;
                }
                if (!IsNullExt())
                {
                    long num = 0L;
                    foreach (DataRow row in _CarStatusListExt.Rows)
                    {
                        if (row["carStatuEx"] != DBNull.Value)
                        {
                            num = Convert.ToInt64(row["carStatuEx"]);
                            if ((num == 0x2000000000L) && ((num & long_0) != 0L))
                            {
                                builder.Append(Convert.ToString(row["carStatuExName"]) + ";");
                            }
                            else if ((num & long_0) != 0L)
                            {
                                builder.Append(Convert.ToString(row["carStatuExName"]) + ";");
                            }
                        }
                    }
                    AddBufferStatuNameExt(long_0, builder.ToString());
                }
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("AlamStatus", "GetStatusNameByCarStatuExt", exception.Message);
                new LogHelper().WriteError(msg);
            }
            return builder.ToString();
        }

        public static int IsAlarm(string string_0, int int_0, long long_0)
        {
            CarAlarmType type = new CarAlarmType();
            return type.GetAlarmTypeValue(string_0, int_0, long_0);
        }

        public static bool IsAlarmReport(int int_0)
        {
            if ((int_0 & 0x482) != 0x482)
            {
                return false;
            }
            return true;
        }

        private static bool IsNull()
        {
            bool flag = false;
            if ((_CarStatusList != null) && (_CarStatusList.Rows.Count > 0))
            {
                return flag;
            }
            LoadAllAlarmStatu();
            if ((_CarStatusList != null) && (_CarStatusList.Rows.Count > 0))
            {
                return flag;
            }
            return true;
        }

        private static bool IsNullExt()
        {
            bool flag = false;
            if ((_CarStatusListExt != null) && (_CarStatusListExt.Rows.Count > 0))
            {
                return flag;
            }
            LoadAllAlarmStatu();
            if ((_CarStatusListExt != null) && (_CarStatusListExt.Rows.Count > 0))
            {
                return flag;
            }
            return true;
        }

        public static void LoadAllAlarmStatu()
        {
            SqlDataAccess access = new SqlDataAccess();
            _CarStatusList = access.getDataBySql("select a.CarStatu,a.CarStatuName from CarStatuTable a");
            _CarStatusListExt = access.getDataBySql("select carStatuEx,carStatuExName from CarStatuExTable");
        }

    }
}

