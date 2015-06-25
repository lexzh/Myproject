using System.Diagnostics;
namespace Bussiness
{
    using DataAccess;
    using Library;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading;

    public class UpDataOtherData : ReceiveDataBase
    {
        private bool bool_0;
        private Dictionary<string, string> dictionary_0 = new Dictionary<string, string>();
        private ManualResetEvent manualResetEvent_0 = new ManualResetEvent(false);
        private UpBuffer upBuffer_0 = new UpBuffer();
        private UpBuffer upBuffer_1 = new UpBuffer();

        public DataTable GetData(OnlineUserInfo onlineUserInfo_0)
        {
            if (this.bool_0)
            {
                this.manualResetEvent_0.WaitOne();
                this.bool_0 = false;
            }
            if ((this.dictionary_0.Count <= 0) || (this.dictionary_0[onlineUserInfo_0.UserId] == null))
            {
                return null;
            }
            DataTable table = null;
            DateTime newOtherDataTime = onlineUserInfo_0.NewOtherDataTime;
            DataTable data = this.upBuffer_1.GetData(ref newOtherDataTime);
            onlineUserInfo_0.NewOtherDataTime = newOtherDataTime;
            table = data.Clone();
            foreach (DataRow row in data.Rows)
            {
                int num = Convert.ToInt32(row["CarId"]);
                if (this.dictionary_0[onlineUserInfo_0.UserId].ToString().IndexOf("," + num + ",") >= 0)
                {
                    table.ImportRow(row);
                }
            }
            return table;
        }

        private void method_0(Exception exception_0)
        {
            Thread.Sleep(0x1388);
            LogHelper helper = new LogHelper();
            ErrorMsg msg = new ErrorMsg("UpDataOtherData", helper.GetCallFunction(), helper.GetExceptionMsg(exception_0));
            helper.WriteError(msg);
        }

        private void method_1()
        {
            Trace.Write("appserver - Thread upOtherData, WebGpsClient_GetOtherData start!");
            SqlDataAccess access = new SqlDataAccess();
            DateTime dbTime = base.GetDbTime(access);
            DateTime now = DateTime.Now;
            DataTable cloneDataTableColumn = UpdataStruct.CloneDataTableColumn;
            DataRow row = cloneDataTableColumn.NewRow();
            while (true)
            {
                DataTable table2 = null;
                try
                {
                    SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@PreReadTime", dbTime) };
                    table2 = access.getDataBySP("WebGpsClient_GetOtherData", parameterArray);
                }
                catch (SqlException exception)
                {
                    this.method_0(exception);
                }
                if ((table2 != null) && (table2.Rows.Count > 0))
                {
                    this.method_2();
                    foreach (DataRow row2 in table2.Rows)
                    {
                        try
                        {
                            row["OrderType"] = "信息";
                            row["msgType"] = -1;
                            row["OrderResult"] = "";
                            row["CommFlag"] = "";
                            row["GpsTime"] = Convert.ToString(row2["svrTime"]);
                            row["ReceTime"] = Convert.ToString(row2["svrTime"]);
                            row["CarNum"] = "";
                            row["CarId"] = Convert.ToInt32(row2["RegionID"]);
                            row["WrkId"] = 0;
                            row["SimNum"] = "0";
                            row["OrderID"] = "0";
                            row["OrderName"] = "平台报警";
                            row["Describe"] = "区域：" + row2["RegionName"].ToString() + "，允许最大车辆：" + row2["AllowLargest"].ToString() + "，当前车辆数：" + row2["CurrentCount"].ToString() + "，在" + row2["instime"].ToString() + "时发生聚集报警。";
                            cloneDataTableColumn.Rows.Add(row.ItemArray);
                        }
                        catch (Exception exception2)
                        {
                            this.method_0(exception2);
                        }
                    }
                    try
                    {
                        dbTime = Convert.ToDateTime(table2.Rows[0]["svrTime"]);
                    }
                    catch (Exception exception3)
                    {
                        this.method_0(exception3);
                    }
                    if ((cloneDataTableColumn != null) && (cloneDataTableColumn.Rows.Count > 0))
                    {
                        this.upBuffer_1.Add(dbTime, cloneDataTableColumn.Copy());
                        cloneDataTableColumn.Clear();
                    }
                    Thread.Sleep(3000);
                    if (this.method_3(now))
                    {
                        now = DateTime.Now;
                    }
                }
                else
                {
                    Thread.Sleep(600000);
                }
            }
        }

        private void method_2()
        {
            try
            {
                string str = " select a.RegionID, c.UserID from gpsRegionType a INNER JOIN GpsPathInGroup b on a.RegionID = b.RegionID INNER JOIN GpsPathGroupAuth c on b.pathgroupID = c.pathgroupID ";
                DataTable table = new SqlDataAccess().getDataBySql(str);
                if ((table != null) && (table.Rows.Count > 0))
                {
                    this.manualResetEvent_0.Reset();
                    this.bool_0 = true;
                    this.dictionary_0.Clear();
                    foreach (DataRow row in table.Rows)
                    {
                        int num = Convert.ToInt32(row["RegionID"]);
                        string str2 = row["UserID"].ToString();
                        if (this.dictionary_0.Keys.Contains<string>(str2) && (this.dictionary_0[str2] != null))
                        {
                            this.dictionary_0[str2] = this.dictionary_0[str2].ToString() + num + ",";
                        }
                        else
                        {
                            this.dictionary_0[str2] = "," + num + ",";
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.method_0(exception);
            }
            finally
            {
                this.manualResetEvent_0.Set();
            }
        }

        private bool method_3(DateTime dateTime_0)
        {
            bool flag = false;
            TimeSpan span = (TimeSpan) (DateTime.Now - dateTime_0);
            if (span.Minutes > 30)
            {
                this.upBuffer_1.Delete();
                flag = true;
            }
            return flag;
        }

        public void Start()
        {
            Thread thread = new Thread(new ThreadStart(this.method_1)) {
                IsBackground = true
            };
            thread.Priority = ThreadPriority.AboveNormal;
            thread.Start();
        }
    }
}

