namespace Bussiness
{
    using DataAccess;
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;

    public class CarFilter
    {
        private Hashtable hashtable_0 = Hashtable.Synchronized(new Hashtable(0xaa9));

        public void AddCarFilterList(CarInfo carInfo_0)
        {
            if ((this.hashtable_0 != null) && !this.hashtable_0.ContainsKey(carInfo_0.SimNum))
            {
                CarFilterInfo info = new CarFilterInfo {
                    SimNum = carInfo_0.SimNum,
                    CarInfoData = carInfo_0,
                    PosReadTime = carInfo_0.IsNewPosTime,
                    PicReadTime = carInfo_0.IsNewPicTime
                };
                lock (this.hashtable_0.SyncRoot)
                {
                    this.hashtable_0.Add(info.SimNum, info);
                }
            }
        }

        public Hashtable GpsCarFilterToHashTable(int int_0)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@workId", int_0) };
            DataTable table = new SqlDataAccess().getDataBySP("WebGpsClient_GetCarFilter", parameterArray);
            Hashtable hashtable = new Hashtable();
            if (hashtable.Count > 0)
            {
                hashtable.Clear();
            }
            CarFilterInfo info = null;
            foreach (DataRow row in table.Rows)
            {
                if (row["simNum"] != DBNull.Value)
                {
                    info = new CarFilterInfo {
                        SimNum = Convert.ToString(row["simNum"]),
                        CarInfoData = CarDataInfoBuffer.GetDataCarInfoBySimNum(info.SimNum)
                    };
                    if (info.CarInfoData != null)
                    {
                        info.PosReadTime = info.CarInfoData.IsNewPosTime;
                        info.PicReadTime = info.CarInfoData.IsNewPicTime;
                        hashtable.Add(info.SimNum, info);
                    }
                }
            }
            return hashtable;
        }

        private CarFilterInfo method_0(string string_0)
        {
            string simNum = CarDataInfoBuffer.GetDataCarInfoByCarId(string_0).SimNum;
            if ((this.hashtable_0 != null) && this.hashtable_0.ContainsKey(simNum))
            {
                return (this.hashtable_0[simNum] as CarFilterInfo);
            }
            return null;
        }

        private int method_1(int int_0, int int_1, string string_0)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@wrkid", int_0), new SqlParameter("@carid", string_0), new SqlParameter("@flg", int_1) };
            int num = 0;
            try
            {
                new SqlDataAccess().insertBySp("WebGpsClient_WatchCarInfo", parameterArray);
                if (int_1 == 0)
                {
                    CarInfo info = CarDataInfoBuffer.GetDataCarInfoByCarId(string_0);
                    if (info != null)
                    {
                        this.AddCarFilterList(info);
                    }
                    return num;
                }
                CarInfo dataCarInfoByCarId = CarDataInfoBuffer.GetDataCarInfoByCarId(string_0);
                if (dataCarInfoByCarId != null)
                {
                    this.RomoveCarFilterList(dataCarInfoByCarId.SimNum);
                }
            }
            catch
            {
                num = -1;
            }
            return num;
        }

        private int method_2(int int_0, int int_1, string string_0)
        {
            int num = 0;
            try
            {
                SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@wrkid", int_0), new SqlParameter("@AreaCode", string_0), new SqlParameter("@flg", int_1) };
                string str = "WebGpsClient_SetCheckedCarInfoByArea";
                new SqlDataAccess().insertBySp(str, parameterArray);
            }
            catch
            {
                num = -1;
            }
            return num;
        }

        private void method_3(int int_0)
        {
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@workId", int_0) };
            DataTable table = new SqlDataAccess().getDataBySP("WebGpsClient_GetCarFilter", parameterArray);
            this.method_4(table);
        }

        private void method_4(DataTable dataTable_0)
        {
            lock (this.hashtable_0.SyncRoot)
            {
                if (this.hashtable_0.Count > 0)
                {
                    this.hashtable_0.Clear();
                }
                CarFilterInfo info = null;
                foreach (DataRow row in dataTable_0.Rows)
                {
                    if (row["simNum"] != DBNull.Value)
                    {
                        info = new CarFilterInfo
                        {
                            SimNum = Convert.ToString(row["simNum"]),
                            CarInfoData = CarDataInfoBuffer.GetDataCarInfoBySimNum(Convert.ToString(row["simNum"]))
                        };
                        if (info.CarInfoData != null)
                        {
                            info.PosReadTime = info.CarInfoData.IsNewPosTime;
                            info.PicReadTime = info.CarInfoData.IsNewPicTime;
                            this.hashtable_0.Add(info.SimNum, info);
                        }
                    }
                }
            }
        }

        public void RomoveCarFilterList(string string_0)
        {
            if ((this.hashtable_0 != null) && this.hashtable_0.ContainsKey(string_0))
            {
                lock (this.hashtable_0.SyncRoot)
                {
                    this.hashtable_0.Remove(string_0);
                }
            }
        }

        public int SetSelectCar(int WorkId, string AreaCodeOrCarId, bool isRegion, bool isAdd)
        {
            int num = 0;
            if (!isAdd)
            {
                num = 1;
            }
            int num2 = -1;
            if (!isRegion)
            {
                string[] strArray2 = AreaCodeOrCarId.Split(new char[] { ',' });
                for (int j = 0; j <= (strArray2.Length - 1); j++)
                {
                    num2 = this.method_1(WorkId, num, strArray2[j]);
                    if (num2 == -1)
                    {
                        return num2;
                    }
                }
                return num2;
            }
            string[] strArray = AreaCodeOrCarId.Split(new char[] { ',' });
            for (int i = 0; i < strArray.Length; i++)
            {
                num2 = this.method_2(WorkId, num, strArray[i]);
                if (num2 == -1)
                {
                    break;
                }
            }
            if (num2 == 0)
            {
                this.method_3(WorkId);
            }
            return num2;
        }

        public void UpdatePosSearchFlag(string string_0, int int_0, string string_1)
        {
            if (("位置查询".Equals(string_0) || "实时点名查询".Equals(string_0)) || ("LBS位置查询".Equals(string_0) || "获得当前车台温度".Equals(string_0)))
            {
                CarFilterInfo info = this.method_0(string_1);
                if (info != null)
                {
                    info.IsPosSearchFlag = true;
                    info.OrderId = int_0;
                }
            }
        }

        public Hashtable CarFilterList
        {
            get
            {
                return this.hashtable_0;
            }
            set
            {
                this.hashtable_0 = value;
            }
        }
    }
}

