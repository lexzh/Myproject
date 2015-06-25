namespace Bussiness
{
    using DataAccess;
    using Library;
    using PublicClass;
    using System;
    using System.Collections;
    using System.Data;
    using System.Runtime.InteropServices;

    public class CarAlarmType
    {
        private static Hashtable m_CarAlarmTypeList;
        private static string m_StrSql;
        private string string_0 = " and b.SimNum = '{0}'";

        static CarAlarmType()
        {
            old_acctor_mc();
        }

        private static void FillAlarmData(ref AlarmType alarmType_0, DataRow dataRow_0)
        {
            try
            {
                alarmType_0.AlarmSwitch = NumHelper.ConvertToInt(dataRow_0["AlarmSwitch"]);
                alarmType_0.AlarmFlag = NumHelper.ConvertToInt(dataRow_0["AlarmFlag"]);
                alarmType_0.ShowAlarm = NumHelper.ConvertToInt(dataRow_0["ShowAlarm"]);
                alarmType_0.cust_AlarmSwitch = NumHelper.ConvertToInt(dataRow_0["cust_AlarmSwitch"]);
                alarmType_0.cust_AlarmFlag = NumHelper.ConvertToInt(dataRow_0["cust_AlarmFlag"]);
                alarmType_0.cust_ShowAlarm = NumHelper.ConvertToInt(dataRow_0["cust_ShowAlarm"]);
                alarmType_0.cust_AlarmName = Convert.ToString(dataRow_0["AlarmName"]);
                if (dataRow_0["AlarmSwitchExt"] != null)
                {
                    alarmType_0.AlarmSwitchExt = Convert.ToInt64(dataRow_0["AlarmSwitchExt"].ToString());
                }
                if (dataRow_0["AlarmFlagExt"] != null)
                {
                    alarmType_0.AlarmFlagExt = Convert.ToInt64(dataRow_0["AlarmFlagExt"].ToString());
                }
                if (dataRow_0["ShowAlarmExt"] != null)
                {
                    alarmType_0.ShowAlarmExt = Convert.ToInt64(dataRow_0["ShowAlarmExt"].ToString());
                }
                alarmType_0.AlarmSwitch |= alarmType_0.cust_AlarmSwitch;
                alarmType_0.AlarmFlag |= alarmType_0.cust_AlarmFlag;
                alarmType_0.ShowAlarm |= alarmType_0.cust_ShowAlarm;
            }
            catch
            {
            }
        }

        public ArrayList GetAlarmCode(string string_1, int int_0)
        {
            ArrayList alarmCodeList = AlamStatus.GetAlarmCodeList(int_0);
            AlarmType type = this.method_1(string_1);
            if (((type.cust_AlarmSwitch != 0) && (type.cust_AlarmName != null)) && (type.cust_AlarmName.Length > 0))
            {
                if (alarmCodeList == null)
                {
                    alarmCodeList = new ArrayList();
                }
                string[] strArray = type.cust_AlarmName.Split(new char[] { '*' });
                long num = 0L;
                for (int i = 0; i <= (strArray.Length - 2); i++)
                {
                    num = Convert.ToInt32(strArray[i].Substring(0, strArray[i].IndexOf("/")));
                    if ((num & int_0) != 0L)
                    {
                        alarmCodeList.Add(num);
                    }
                }
            }
            return alarmCodeList;
        }

        public string GetAlarmTypeName(int int_0)
        {
            string str = "接收";
            if (int_0 == 1)
            {
                str = "报警";
            }
            if (int_0 == 2)
            {
                return "警告";
            }
            if (int_0 == 3)
            {
                str = "报告";
            }
            return str;
        }

        public int GetAlarmTypeValue(string string_1, int int_0, long long_0)
        {
            AlarmType type = this.method_1(string_1);
            ArrayList alarmCode = this.GetAlarmCode(string_1, int_0);
            ArrayList alarmCodeExList = AlamStatus.GetAlarmCodeExList(long_0);
            int num = 1;
            foreach (long num2 in alarmCode)
            {
                if (num2 == -2147483648L)
                {
                    return 1;
                }
                if ((type.AlarmSwitch & num2) != 0L)
                {
                    if ((type.AlarmFlag & num2) != 0L)
                    {
                        return 1;
                    }
                    if (((type.ShowAlarm & num2) != 0L) && (num != 2))
                    {
                        num = 3;
                    }
                    else
                    {
                        num = 2;
                    }
                }
            }
            foreach (long num3 in alarmCodeExList)
            {
                if ((type.AlarmSwitchExt & num3) != 0L)
                {
                    if ((type.AlarmFlagExt & num3) != 0L)
                    {
                        return 1;
                    }
                    if (((type.ShowAlarmExt & num3) != 0L) && (num != 2))
                    {
                        num = 3;
                    }
                    else
                    {
                        num = 2;
                    }
                }
            }
            return num;
        }

        public string GetCustAlarmName(string string_1, int int_0)
        {
            string str = string.Empty;
            AlarmType type = this.method_1(string_1);
            if (type.cust_AlarmSwitch != 0)
            {
                if ((type.cust_AlarmName == null) || (type.cust_AlarmName.Length <= 0))
                {
                    return str;
                }
                string[] strArray = type.cust_AlarmName.Split(new char[] { '*' });
                for (int i = 0; i <= (strArray.Length - 2); i++)
                {
                    if ((Convert.ToInt32(strArray[i].Substring(0, strArray[i].IndexOf("/"))) & int_0) != 0)
                    {
                        str = str + strArray[i].Substring(strArray[i].IndexOf("/") + 1) + ";";
                    }
                }
            }
            return str;
        }

        public static void LoadAllCarAlarmTypeList()
        {
            if ((m_CarAlarmTypeList == null) || (m_CarAlarmTypeList.Count <= 0))
            {
                DataTable table = new SqlDataAccess().getDataBySql(m_StrSql);
                if ((table != null) && (table.Rows.Count > 0))
                {
                    string key = string.Empty;
                    foreach (DataRow row in table.Rows)
                    {
                        AlarmType type = new AlarmType();
                        FillAlarmData(ref type, row);
                        key = Convert.ToString(row["SimNum"]);
                        if ((m_CarAlarmTypeList != null) && !m_CarAlarmTypeList.ContainsKey(key))
                        {
                            m_CarAlarmTypeList.Add(key, type);
                        }
                    }
                }
            }
        }

        private AlarmType method_0(string string_1)
        {
            string str = string.Format(m_StrSql + this.string_0, string_1);
            AlarmType type = new AlarmType();
            try
            {
                DataTable table = new SqlDataAccess().getDataBySql(str);
                if ((table != null) && (table.Rows.Count > 0))
                {
                    FillAlarmData(ref type, table.Rows[0]);
                }
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("CarAlarmType", "GetSingleRowData", exception.Message);
                new LogHelper().WriteError(msg);
            }
            return type;
        }

        private AlarmType method_1(string string_1)
        {
            if (((m_CarAlarmTypeList != null) && (m_CarAlarmTypeList.Count > 0)) && m_CarAlarmTypeList.ContainsKey(string_1))
            {
                return (AlarmType) m_CarAlarmTypeList[string_1];
            }
            AlarmType type = this.method_0(string_1);
            m_CarAlarmTypeList.Add(string_1, type);
            return type;
        }

        private static void old_acctor_mc()
        {
            m_CarAlarmTypeList = new Hashtable(0x5597);
            m_StrSql = "select b.SimNum,a.cust_name as AlarmName, isnull(a.carAlarmSwitch,0) as AlarmSwitch,isnull(a.carAlarmFlag,0) as AlarmFlag,isNull(a.isShowForm,0) as ShowAlarm,  isnull(a.CarAlarmSwitchEx,0) as AlarmSwitchExt,isnull(a.AlarmFlagEx,0) as AlarmFlagExt,isNull(a.CarShowAlarmEx,0) as ShowAlarmExt,  isnull(a.cust_carAlarmSwitch,0) as cust_AlarmSwitch,isnull(a.cust_carAlarmFlag,0) as cust_AlarmFlag,isNull(a.cust_isShowForm,0) as cust_ShowAlarm  from GisCarInfoTable a  left join giscar b on a.carid = b.carid  where b.SimNum is not null";
        }

        public void UpdateAlarmType(string string_1)
        {
            AlarmType type = this.method_0(string_1);
            if (m_CarAlarmTypeList.ContainsKey(string_1))
            {
                m_CarAlarmTypeList[string_1] = type;
            }
            else
            {
                m_CarAlarmTypeList.Add(string_1, type);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct AlarmType
        {
            public int AlarmSwitch;
            public int AlarmFlag;
            public int ShowAlarm;
            public int cust_AlarmSwitch;
            public int cust_AlarmFlag;
            public int cust_ShowAlarm;
            public string cust_AlarmName;
            public long AlarmSwitchExt;
            public long AlarmFlagExt;
            public long ShowAlarmExt;
        }
    }
}

