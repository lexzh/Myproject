using System.Diagnostics;
namespace Bussiness
{
    using DataAccess;
    using Library;
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;

    public class UpdataResponseData : ReceiveDataBase
    {
        private UpAlarm upAlarm_0 = new UpAlarm();
        private UpBuffer upBuffer_0 = new UpBuffer();
        private UpBuffer upBuffer_1 = new UpBuffer();
        private UpBuffer upBuffer_2 = new UpBuffer();
        private UpBuffer upBuffer_3 = new UpBuffer(Const.AlarmCodeList);
        private UpPic upPic_0 = new UpPic();
        private UpResponse upResponse_0 = new UpResponse();

        public void Delete(Hashtable hashtable_0)
        {
            this.upBuffer_2.Delete(hashtable_0);
        }

        public DataTable GetAlarmData(OnlineUserInfo onlineUserInfo_0)
        {
            return this.upBuffer_1.GetAlarmData(onlineUserInfo_0);
        }

        public DataTable GetNewLog(OnlineUserInfo onlineUserInfo_0)
        {
            DateTime newLogTime = onlineUserInfo_0.NewLogTime;
            DataTable data = this.upBuffer_0.GetData(ref newLogTime);
            onlineUserInfo_0.NewLogTime = newLogTime;
            return data;
        }

        public DataTable GetNewLogExt(OnlineUserInfo onlineUserInfo_0)
        {
            return this.upBuffer_2.GetDataByWorkId(onlineUserInfo_0.WorkId);
        }

        public DataTable GetPictureData(OnlineUserInfo onlineUserInfo_0)
        {
            return this.upBuffer_3.GetPictureData(onlineUserInfo_0);
        }

        public DataTable GetPictureData(OnlineUserInfo onlineUserInfo_0, ref DateTime dateTime_0)
        {
            onlineUserInfo_0.NewPicTime = dateTime_0;
            DataTable pictureData = this.upBuffer_3.GetPictureData(onlineUserInfo_0);
            dateTime_0 = onlineUserInfo_0.NewPicTime;
            return pictureData;
        }

        private bool method_0(DateTime dateTime_0)
        {
            bool flag = false;
            TimeSpan span = (TimeSpan) (DateTime.Now - dateTime_0);
            if (span.TotalSeconds > 30.0)
            {
                this.upBuffer_0.Delete();
                this.upBuffer_1.Delete();
                this.upBuffer_3.Delete();
                flag = true;
            }
            return flag;
        }

        private void method_1()
        {
            //Trace.Write("appserver - Thread upResponse, WebGpsClient_GetResponseData start!");
            SqlDataAccess access = new SqlDataAccess();
            DateTime dbTime = base.GetDbTime(access);
            DateTime now = DateTime.Now;
            DataRow row = UpdataStruct.CloneDataTableColumn.NewRow();
            while (true)
            {
                try
                {
                    SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@PreReadTime", dbTime) };
                    DataTable table = access.getDataBySP("WebGpsClient_GetResponseData", parameterArray);
                    if ((table != null) && (table.Rows.Count > 0))
                    {
                        dbTime = Convert.ToDateTime(table.Rows[0]["svrTime"]);
                        this.method_2(row, table);
                    }
                    else
                    {
                        Thread.Sleep(0xbb8);
                    }
                    if (this.method_0(now))
                    {
                        now = DateTime.Now;
                    }
                }
                catch (Exception exception)
                {
                    Thread.Sleep(0x1388);
                    LogHelper helper = new LogHelper();
                    ErrorMsg msg = new ErrorMsg("UpdataResponseData", helper.GetCallFunction(), helper.GetExceptionMsg(exception));
                    helper.WriteError(msg);
                }
                Thread.Sleep(0x3e8);
            }
        }

        private void method_2(DataRow dataRow_0, DataTable dataTable_0)
        {
            DataTable cloneDataTableColumn = UpdataStruct.CloneDataTableColumn;
            DataTable table2 = UpdataStruct.CloneDataTableColumn;
            DataTable table3 = UpdataStruct.CloneDataTableColumn;
            DataTable table4 = UpdataStruct.CloneDataTableColumn;
            CarPartInfo info = new CarPartInfo();
            foreach (DataRow row in dataTable_0.Rows)
            {
                DataRowData data = new DataRowData(row);
                if (data.IsAlamType)
                {
                    this.upAlarm_0.CalAlarmData(row, dataRow_0, info, cloneDataTableColumn);
                }
                else if (data.IsHadBitData)
                {
                    this.upPic_0.CalPicData(row, info, table4);
                }
                else if (data.WorkId == 0)
                {
                    this.upResponse_0.CalNewLog(row, table2);
                }
                else
                {
                    this.upResponse_0.CalNewLog(row, table3);
                    this.upBuffer_2.AddByWorkId(data.WorkId, table3.Copy());
                    table3.Clear();
                }
            }
            if (cloneDataTableColumn.Rows.Count > 0)
            {
                this.upBuffer_1.Add(DateTime.Now, cloneDataTableColumn);
            }
            if (table2.Rows.Count > 0)
            {
                this.upBuffer_0.Add(DateTime.Now, table2);
            }
            if (table4.Rows.Count > 0)
            {
                this.upBuffer_3.Add(DateTime.Now, table4);
            }
        }

        public void Remove(int int_0)
        {
            this.upBuffer_2.RemoveAt(int_0);
        }

        public void Start()
        {
            Thread thread = new Thread(new ThreadStart(this.method_1)) {
                IsBackground = true
            };
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }

        public int AlarmListSize
        {
            get
            {
                return this.upBuffer_1.Count;
            }
        }

        public int RespListByWorkId
        {
            get
            {
                return this.upBuffer_2.Count;
            }
        }

        public int RespListSize
        {
            get
            {
                return this.upBuffer_0.Count;
            }
        }

        public class DataRowData
        {
            private DataRow dataRow_0;

            public DataRowData(DataRow dataRow_1)
            {
                this.dataRow_0 = dataRow_1;
            }

            public bool IsAlamType
            {
                get
                {
                    return (Convert.ToInt32(this.dataRow_0["Reserved"]) == 0x482);
                }
            }

            public bool IsHadBitData
            {
                get
                {
                    int num = 0;
                    try
                    {
                        num = Convert.ToInt32(this.dataRow_0["PicDataType"]);
                    }
                    catch
                    {
                    }
                    return (num != 0);
                }
            }

            public int WorkId
            {
                get
                {
                    return Convert.ToInt32(this.dataRow_0["wrkId"]);
                }
            }
        }
    }
}

