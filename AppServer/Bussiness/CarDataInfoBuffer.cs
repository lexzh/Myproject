namespace Bussiness
{
    using DataAccess;
    using Library;
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;

    public class CarDataInfoBuffer
    {
        private static Hashtable _keyCarIdList;
        private static Hashtable _keyCarNumList;
        private static Hashtable _keySimNumList;
        private static string _SqlReadSingleCarInfo;

        static CarDataInfoBuffer()
        {
            old_acctor_mc();
        }

        private static void Add(Enum0 enum0_0, string string_0)
        {
            string format = string.Empty;
            if (enum0_0 == Enum0.CarNum)
            {
                format = _SqlReadSingleCarInfo + " where a.carnum='{0}'";
            }
            else if (enum0_0 == Enum0.SimNum)
            {
                format = _SqlReadSingleCarInfo + " where a.simnum='{0}'";
            }
            else if (enum0_0 == Enum0.CarId)
            {
                format = _SqlReadSingleCarInfo + " where a.carid={0}";
            }
            format = string.Format(format, string_0);
            DataTable table = new SqlDataAccess().getDataBySql(format);
            if ((table != null) && (table.Rows.Count > 0))
            {
                AddCarInfoToList(table);
            }
        }

        private static void AddCarInfoToList(DataTable dataTable_0)
        {
            lock (_keyCarIdList.SyncRoot)
            {
                foreach (DataRow row in dataTable_0.Rows)
                {
                    CarInfo info = new CarInfo();
                    FillCarInfo(info, row);
                    if (_keyCarIdList.ContainsKey(info.CarId))
                    {
                        CarInfo info2 = _keyCarIdList[info.CarId] as CarInfo;
                        _keyCarIdList.Remove(info2.CarId);
                        _keyCarIdList.Remove(info.CarId);
                        _keySimNumList.Remove(info2.SimNum);
                        _keyCarNumList.Remove(info2.CarNum);
                    }
                    _keyCarIdList.Add(info.CarId, info);
                    if (_keySimNumList.ContainsKey(info.SimNum))
                    {
                        CarInfo info3 = _keySimNumList[info.SimNum] as CarInfo;
                        _keySimNumList.Remove(info3.SimNum);
                        _keySimNumList.Remove(info.SimNum);
                    }
                    _keySimNumList.Add(info.SimNum, info);
                    if (_keyCarNumList.ContainsKey(info.CarNum))
                    {
                        CarInfo info4 = _keyCarNumList[info.CarNum] as CarInfo;
                        _keyCarNumList.Remove(info4.CarNum);
                        _keyCarNumList.Remove(info.CarNum);
                    }
                    _keyCarNumList.Add(info.CarNum, info);
                }
            }
        }

        private static void FillCarInfo(CarInfo carInfo_0, DataRow dataRow_0)
        {
            carInfo_0.CarId = Convert.ToString(dataRow_0["CarId"]);
            carInfo_0.CarNum = Convert.ToString(dataRow_0["CarNum"]);
            carInfo_0.SimNum = Convert.ToString(dataRow_0["SimNum"]);
            carInfo_0.AreaCode = Convert.ToString(dataRow_0["areacode"]);
            carInfo_0.AreaName = Convert.ToString(dataRow_0["areaname"]);
            carInfo_0.AreaId = Convert.ToInt32(dataRow_0["areaid"]);
            carInfo_0.overTime = Convert.ToInt32(dataRow_0["overTime"]);
            carInfo_0.IsStop = Convert.ToInt32(dataRow_0["isStop"]);
            carInfo_0.ProtocolName = (dataRow_0["ProtocolName"] == DBNull.Value) ? "" : dataRow_0["ProtocolName"].ToString();
        }

        public DataTable GetArarmCarList(Hashtable hashtable_0)
        {
            DataTable cloneDataTableColumn = null;
            try
            {
                cloneDataTableColumn = UpdataStruct.CloneDataTableColumn;
                if ((_keyCarIdList != null) && (_keyCarIdList.Count > 0))
                {
                    if ((hashtable_0 == null) || (hashtable_0.Count <= 0))
                    {
                        return cloneDataTableColumn;
                    }
                    lock (_keyCarIdList.SyncRoot)
                    {
                        foreach (CarInfo info in _keyCarIdList.Values)
                        {
                            if ((info.isAlarm && (info.CarAlarmData != null)) && hashtable_0.ContainsKey(Convert.ToInt32(info.CarId)))
                            {
                                cloneDataTableColumn.LoadDataRow(info.CarAlarmData, false);
                            }
                        }
                        return cloneDataTableColumn;
                    }
                }
                return cloneDataTableColumn;
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("CarDataInfoBuffer", "GetArarmCarList", exception.Message + exception.StackTrace);
                new LogHelper().WriteError(msg);
            }
            return cloneDataTableColumn;
        }

        public ArrayList GetCarInfoByAreaCode(string string_0)
        {
            ArrayList list = new ArrayList();
            lock (_keyCarIdList.SyncRoot)
            {
                foreach (CarInfo info in _keyCarIdList)
                {
                    if (info.AreaCode.Equals(string_0))
                    {
                        list.Add(info);
                    }
                }
            }
            return list;
        }

        public ArrayList GetCarInfoByCarids(string string_0)
        {
            ArrayList list = new ArrayList();
            foreach (string str in string_0.Split(new char[] { ',' }))
            {
                list.Add(GetDataCarInfoByCarId(str));
            }
            return list;
        }

        public int getCarNumByCode(string string_0)
        {
            int num = 0;
            lock (_keyCarIdList.SyncRoot)
            {
                foreach (CarInfo info in _keyCarIdList)
                {
                    if (info.AreaCode.Equals(string_0))
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        public int getCarSubAreaNumByCode(string string_0)
        {
            int num = 0;
            lock (_keyCarIdList.SyncRoot)
            {
                foreach (CarInfo info in _keyCarIdList)
                {
                    if (info.AreaCode.IndexOf(string_0) != -1)
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        public static CarInfo GetDataCarInfoByCarId(string string_0)
        {
            CarInfo info = _keyCarIdList[string_0] as CarInfo;
            if (info == null)
            {
                Add(Enum0.CarId, string_0);
                info = _keyCarIdList[string_0] as CarInfo;
            }
            return info;
        }

        public static CarInfo GetDataCarInfoByCarNum(string string_0)
        {
            if (!_keyCarNumList.ContainsKey(string_0))
            {
                Add(Enum0.CarNum, string_0);
            }
            CarInfo info = _keyCarNumList[string_0] as CarInfo;
            if ((info != null) && !string_0.Equals(info.CarNum))
            {
                Add(Enum0.CarNum, string_0);
                info = _keyCarNumList[string_0] as CarInfo;
            }
            return info;
        }

        public static CarInfo GetDataCarInfoBySimNum(string sSimNum)
        {
            if (!_keySimNumList.ContainsKey(sSimNum))
            {
                Add(Enum0.SimNum, sSimNum);
            }
            CarInfo info = _keySimNumList[sSimNum] as CarInfo;
            if ((info != null) && !sSimNum.Equals(info.SimNum))
            {
                Add(Enum0.SimNum, sSimNum);
                info = _keySimNumList[sSimNum] as CarInfo;
            }
            return info;
        }

        public static void LoadAllCarInfoList()
        {
            DataTable table = new SqlDataAccess().getDataBySP("WebGpsClient_GetCarInfo", null);
            if ((table != null) && (table.Rows.Count > 0))
            {
                AddCarInfoToList(table);
            }
        }

        private static void old_acctor_mc()
        {
            _keySimNumList = new Hashtable(0x5597);
            _keyCarNumList = new Hashtable(0x5597);
            _keyCarIdList = Hashtable.Synchronized(new Hashtable(0x5597));
            _SqlReadSingleCarInfo = "select a.carid , a.carNum, a.simnum, a.password, isnull(a.isStop,0) as isStop, b.areacode, b.areaname, b.areaid, a.SVREndTime, isnull(datediff(day,getdate(),a.SVREndTime),1) as overTime, getdate() as currentTime, d.ProtocolName from giscar a inner join gpsarea b on a.areaid = b.areaid inner join gpsterminaltype c on a.TerminalTypeID = c.TerminalTypeID inner join gpsprotocol d on c.ProtocolCode = d.ProtocolCode ";
        }

        private static void Update(CarInfo carInfo_0, DataRow dataRow_0)
        {
            carInfo_0.AreaCode = Convert.ToString(dataRow_0["areacode"]);
            carInfo_0.AreaName = Convert.ToString(dataRow_0["areaname"]);
            carInfo_0.AreaId = Convert.ToInt32(dataRow_0["areaid"]);
            carInfo_0.overTime = Convert.ToInt32(dataRow_0["overTime"]);
            carInfo_0.IsStop = Convert.ToInt32(dataRow_0["isStop"]);
            carInfo_0.ProtocolName = (dataRow_0["ProtocolName"] == DBNull.Value) ? "" : dataRow_0["ProtocolName"].ToString();
        }

        public static void UpdateLoginUserCarInfo(string string_0)
        {
            try
            {
                SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@UserId", string_0) };
                DataTable table = new SqlDataAccess().getDataBySP("WebGpsClient_GetCarInfoByUserId", parameterArray);
                if ((table != null) && (table.Rows.Count > 0))
                {
                    string key = string.Empty;
                    string str2 = string.Empty;
                    string str3 = string.Empty;
                    foreach (DataRow row in table.Rows)
                    {
                        key = Convert.ToString(row["CarId"]);
                        if (_keyCarIdList.ContainsKey(key))
                        {
                            CarInfo info = _keyCarIdList[key] as CarInfo;
                            str2 = Convert.ToString(row["SimNum"]);
                            str3 = Convert.ToString(row["CarNum"]);
                            if (info.CarNum.Equals(str3) && info.SimNum.Equals(str2))
                            {
                                Update(info, row);
                            }
                            else
                            {
                                lock (_keyCarIdList.SyncRoot)
                                {
                                    if (_keyCarIdList.ContainsKey(key))
                                    {
                                        _keyCarIdList.Remove(key);
                                    }
                                    if (_keyCarNumList.ContainsKey(info.CarNum))
                                    {
                                        _keyCarNumList.Remove(info.CarNum);
                                    }
                                    if (_keySimNumList.ContainsKey(info.SimNum))
                                    {
                                        _keySimNumList.Remove(info.SimNum);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("CarDataInfoBuffer", "UpdateLoginUserCarInfo", exception.Message + exception.StackTrace);
                new LogHelper().WriteError(msg);
            }
        }

        public static int Count
        {
            get
            {
                return _keyCarIdList.Count;
            }
        }

        private enum Enum0
        {
            CarNum,
            SimNum,
            CarId
        }
    }
}

