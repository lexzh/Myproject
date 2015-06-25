namespace Bussiness
{
    using DataAccess;
    using Library;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class TrackReplay : ReceiveDataBase
    {
        public DataTable GetReplayData(string BeginTime, string EndTime, string Tele, int RecordCount, int PageNum, int PageCount, int IsComputeMile)
        {
            return this.GetReplayData(BeginTime, EndTime, Tele, RecordCount, PageNum, PageCount, IsComputeMile, 1);
        }
       
        public DataTable GetReplayData(string BeginTime, string EndTime, string Tele, int RecordCount, int PageNum, int PageCount, int IsComputeMile, int IsQueryPic)
        {
            DataTable table = null;
            try
            {
                table = this.method_1(BeginTime, EndTime, Tele, RecordCount, PageNum, PageCount, IsComputeMile, IsQueryPic);
                table.Columns.Add(new DataColumn("StatuList"));
                table.Columns.Add(new DataColumn("AlarmType"));
                table.Columns.Add(new DataColumn("IsFill"));
                table.Columns.Add(new DataColumn("AccOn"));
                table.Columns.Add(new DataColumn("GpsValid"));
                int num = 0;
                int num2 = 0;
                long num3 = 0L;
                string str = string.Empty;
                CarAlarmType type = new CarAlarmType();
                foreach (DataRow row in table.Rows)
                {
                    str = row["SimNum"].ToString();
                    row["CarNum"].ToString();
                    num = Convert.ToInt32(row["reserved"]);
                    num2 = int.Parse(row["carStatu"].ToString());
                    if (table.Columns.Contains("CarStatuEx"))
                    {
                        num3 = long.Parse(row["CarStatuEx"].ToString());
                    }
                    row["StatuList"] = AlamStatus.GetStatusNameByCarStatu((long) num2) + AlamStatus.GetStatusNameByCarStatuExt(num3) + type.GetCustAlarmName(str, num2);
                    if (AlamStatus.IsAlarmReport(num))
                    {
                        row["carStatu"] = 1;
                    }
                    else
                    {
                        row["carStatu"] = 2;
                    }
                    row["AlarmType"] = type.GetAlarmTypeValue(Tele, num2, num3);
                    int result = 0;
                    int.TryParse(row["TransportStatus"].ToString(), out result);
                    if (result == 3)
                    {
                        row["IsFill"] = 1;
                    }
                    else
                    {
                        row["IsFill"] = 0;
                    }
                    if (base.isPosStatus(num2))
                    {
                        row["GpsValid"] = 1;
                    }
                    else
                    {
                        row["GpsValid"] = 0;
                    }
                    if ((num2 & 0x4000) == 0)
                    {
                        row["AccOn"] = 0;
                    }
                    else
                    {
                        row["AccOn"] = 1;
                    }
                }
                return table;
            }
            catch (Exception exception)
            {
                LogHelper helper = new LogHelper();
                ErrorMsg msg = new ErrorMsg("TrackReplay", helper.GetCallFunction(), helper.GetExceptionMsg(exception));
                helper.WriteError(msg);
            }
            return table;
        }

        public DataTable GetReplayData(string string_0, string string_1, string string_2, int int_0, int int_1, int int_2, int int_3, string string_3, string string_4)
        {
            DataTable table = this.method_2();
            try
            {
                new TrackReplay();
                int num = int_0;
                DataTable table2 = this.method_0(string_0, string_1, string_2, num, int_1, int_2, 0);
                DataView defaultView = table2.DefaultView;
                int num2 = 0;
                int num3 = 0;
                long num4 = 0L;
                int result = 0;
                string str = string.Empty;
                string str2 = string.Empty;
                CarAlarmType type = new CarAlarmType();
                Car car = new Car();
                for (int i = 0; i < defaultView.Count; i++)
                {
                    DataRow row = table.NewRow();
                    row["CarNum"] = defaultView[i]["carNum"].ToString();
                    row["CarId"] = defaultView[i]["carid"].ToString();
                    row["Longitude"] = defaultView[i]["Longitude"].ToString();
                    row["Latitude"] = defaultView[i]["Latitude"].ToString();
                    row["Speed"] = defaultView[i]["speed"].ToString();
                    str = Convert.ToString(defaultView[i]["SimNum"]);
                    num2 = Convert.ToInt32(defaultView[i]["reserved"]);
                    row["Reserved"] = num2;
                    if (num2 == 0x28b)
                    {
                        row["Reserved"] = "是";
                    }
                    else
                    {
                        row["Reserved"] = "否";
                    }
                    num3 = Convert.ToInt32(defaultView[i]["CarStatu"]);
                    if (table2.Columns.Contains("CarStatuEx"))
                    {
                        num4 = long.Parse(defaultView[i]["CarStatuEx"].ToString());
                    }
                    str2 = AlamStatus.GetStatusNameByCarStatu((long) num3) + AlamStatus.GetStatusNameByCarStatuExt(num4) + type.GetCustAlarmName(str, num3);
                    float num7 = float.Parse(defaultView[i]["distanceDiff"].ToString()) / 1000f;
                    if (num7 < 0f)
                    {
                        num7 = 0f;
                    }
                    row["Distance"] = string.Format("{0:F2}", num7);
                    row["GpsTime"] = defaultView[i]["gpstime"].ToString();
                    row["CarStatusList"] = str2;
                    row["CarStatus"] = AlamStatus.IsAlarmReport(num2) ? 1 : 2;
                    row["AlarmType"] = type.GetAlarmTypeValue(string_2, num3, num4);
                    int.TryParse(defaultView[i]["TransportStatus"].ToString(), out result);
                    row["IsFill"] = (result == 3) ? 1 : 0;
                    row["GpsValid"] = car.isPosStatus(num3) ? 1 : 0;
                    row["AccOn"] = ((num3 & 0x4000) == 0) ? 0 : 1;
                    row["Direct"] = defaultView[i]["Direct"].ToString();
                    table.Rows.Add(row);
                }
                return table;
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("TrackReplay", "GetTrackData", exception.Message);
                new LogHelper().WriteError(msg);
                DataRow row2 = table.NewRow();
                table.Clear();
                row2["Error"] = exception.Message;
                row2["Hand"] = string_3;
                table.Rows.Add(row2);
                return table;
            }
        }

        public DataTable GetReplayDataCount(string string_0, string string_1, string string_2)
        {
            DataTable table2;
            try
            {
                SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@BeginTime", string_0), new SqlParameter("@EndTime", string_1), new SqlParameter("@Tele", string_2) };
                DataTable table = new SqlDataAccess().getDataBySP("WebGpsClient_spGisGetTrackCount", parameterArray);
                if ((table != null) && (table.Rows.Count > 0))
                {
                    LogMsg msg = new LogMsg {
                        ClassName = "TrackReplay",
                        FunctionName = "GetReplayDataCount",
                        Msg = string.Concat(new object[] { @"开始时间\结束时间\SimNum\条数:", string_0, @"\", string_1, @"\", string_2, @"\", table.Rows[0]["countData"] })
                    };
                    new LogHelper().WriteLog(msg);
                }
                table2 = table;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return table2;
        }

        public DataTable GetReplayPicDataFromDB(string string_0, string string_1, string string_2)
        {
            return new SqlDataAccess().getDataBySql(string.Format("select gpstime,pic from GpsReceRealTime where telephone='{0}' and ispic=1 and gpstime>='{1}' and gpstime<='{2}'", string_2, string_0, string_1));
        }

        public DataTable GetReplayPicDataFromDBByGpsRece(string string_0, string string_1, string string_2)
        {
            return new SqlDataAccess().getDataBySql(string.Format("select gpstime,pic from GpsReceRealTime where telephone='{0}' and ispic=1 and gpstime='{1}' and ReceTime='{2}'", string_2, string_0, string_1));
        }

        private DataTable method_0(string string_0, string string_1, string string_2, int int_0, int int_1, int int_2, int int_3)
        {
            return this.method_1(string_0, string_1, string_2, int_0, int_1, int_2, int_3, 1);
        }

        private DataTable method_1(string BeginTime, string EndTime, string Tele, int RecordCount, int PageNum, int PageCount, int IsComputeMile, int IsQueryPic)
        {
            DataTable table2;
            try
            {
                DataTable table = null;
                if (IsQueryPic == 1)
                {
                    SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@BeginTime", BeginTime), new SqlParameter("@EndTime", EndTime), new SqlParameter("@Tele", Tele), new SqlParameter("@RecordCount", RecordCount), new SqlParameter("@PageNum", PageNum), new SqlParameter("@PageCount", PageCount), new SqlParameter("@IsComputeMile", IsComputeMile) };
                    table = new SqlDataAccess().getDataBySP("WebGpsClient_spGisGetTrack", parameterArray);
                }
                else
                {
                    SqlParameter[] parameterArray2 = new SqlParameter[] { new SqlParameter("@BeginTime", BeginTime), new SqlParameter("@EndTime", EndTime), new SqlParameter("@Tele", Tele), new SqlParameter("@RecordCount", RecordCount), new SqlParameter("@PageNum", PageNum), new SqlParameter("@PageCount", PageCount), new SqlParameter("@IsComputeMile", IsComputeMile), new SqlParameter("@isQueryPic", IsQueryPic) };
                    table = new SqlDataAccess().getDataBySP("WebGpsClient_spGisGetTrack", parameterArray2);
                }
                table2 = table;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return table2;
        }

        private DataTable method_2()
        {
            DataTable table = new DataTable("GpsRealTimePos");
            table.Columns.Add(new DataColumn("GpsTime"));
            table.Columns.Add(new DataColumn("Longitude"));
            table.Columns.Add(new DataColumn("Latitude"));
            table.Columns.Add(new DataColumn("Distance"));
            table.Columns.Add(new DataColumn("Reserved"));
            table.Columns.Add(new DataColumn("Speed"));
            table.Columns.Add(new DataColumn("CarNum"));
            table.Columns.Add(new DataColumn("CarId"));
            table.Columns.Add(new DataColumn("CarStatus"));
            table.Columns.Add(new DataColumn("CarStatusList"));
            table.Columns.Add(new DataColumn("TotalCount"));
            table.Columns.Add(new DataColumn("TotalDis"));
            table.Columns.Add(new DataColumn("IsLast"));
            table.Columns.Add(new DataColumn("Error"));
            table.Columns.Add(new DataColumn("Hand"));
            table.Columns.Add(new DataColumn("AlarmType"));
            table.Columns.Add(new DataColumn("AccOn"));
            table.Columns.Add(new DataColumn("IsFill"));
            table.Columns.Add(new DataColumn("GpsValid"));
            table.Columns.Add(new DataColumn("Direct"));
            return table.Clone();
        }
    }
}

