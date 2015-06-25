using GisServices;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Timers;
using Library;

namespace Bussiness
{
	public class IORegionTimer : ProcessBase
	{
		private Timer tGetCurrentPosInfoTimer;

		private int iGetCurrentPosInfoInterval = ReadDataFromXml.GetCurrentPosInfo * 1000 * 60;

		private DataTable dtAdminRegionList = new DataTable();

		private string sAdminRegionPreTime = "";

		public IORegionTimer()
		{
		}

		private void AdminRegionAlarmInsert(DataTable dt)
		{
			if (dt == null || dt.Rows.Count <= 0)
			{
				return;
			}
			try
			{
				LogHelper logHelper = new LogHelper();
				int num = 0;
				string empty = string.Empty;
				int num1 = 1154;
				int num2 = 65;
				string str = string.Empty;
				string str1 = null;
				bool flag = false;
				string str2 = null;
				int num3 = 0;
				foreach (DataRow row in dt.Rows)
				{
					try
					{
						SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@wrkid", (object)num), new SqlParameter("@orderid", row["OrderId"]), new SqlParameter("@userid", empty), new SqlParameter("@telephone", row["SimNum"]), new SqlParameter("@msgType", (object)num2), new SqlParameter("@recetime", row["ReceTime"]), new SqlParameter("@gpstime", row["GpsTime"]), new SqlParameter("@starCondition", row["StarCondition"]), new SqlParameter("@starNum", row["StarNum"]), new SqlParameter("@carStatu", row["carStatu"]), new SqlParameter("@carStatuEx", row["CarStatuEx"]), new SqlParameter("@carCondition", row["CarCondition"]), new SqlParameter("@Longitude", row["Longitude"]), new SqlParameter("@Latitude", row["Latitude"]), new SqlParameter("@direct", row["Direct"]), new SqlParameter("@speed", row["Speed"]), new SqlParameter("@Reserved", (object)num1), new SqlParameter("@TransportStatus", row["TransportStatus"]), new SqlParameter("@Accelerration", row["Accelerration"]), new SqlParameter("@Altitude", row["Altitude"]), new SqlParameter("@DistanceDiff", row["DistanceDiff"]), new SqlParameter("@commflag", row["CommFlag"]), new SqlParameter("@addType", row["AddMsgType"]), new SqlParameter("@addTxt", row["AddMsgTxt"]), new SqlParameter("@DutyStr", str1), new SqlParameter("@isPic", (object)flag), new SqlParameter("@pic", str2), new SqlParameter("@alarmInfo", row["AdminRegionName"].ToString()), new SqlParameter("@cameraID", (object)num3) };
						string str3 = "GpsPicServer_Alarm_Insert";
						string str4 = "GpsPicServer_RealTime_Insert";
						int num4 = SqlDataAccess.insertBySp(str3, sqlParameter);
						if (num4 > 0)
						{
							LogMsg logMsg = new LogMsg("", "", string.Concat("车载电话为：", row["SimNum"].ToString(), "的出入行政区报警报文已插入gpsrecebuffer"));
							logHelper.WriteLog(logMsg);
						}
						else
						{
							ErrorMsg errorMsg = new ErrorMsg("ReadDataFromDB", "AdminRegionAlarmInsert", string.Concat("将出入行政区报警报文插入gpsrecbuffer表错误，返回值!", num4.ToString()));
							logHelper.WriteError(errorMsg);
						}
						int num5 = SqlDataAccess.insertBySp(str4, sqlParameter);
						if (num5 > 0)
						{
							LogMsg logMsg1 = new LogMsg("", "", string.Concat("车载电话为：", row["SimNum"].ToString(), "的出入行政区报警报文已插入gpsrecerealtime"));
							logHelper.WriteLog(logMsg1);
						}
						else
						{
							ErrorMsg errorMsg1 = new ErrorMsg("ReadDataFromDB", "AdminRegionAlarmInsert", string.Concat("将出入行政区报警报文插入gpsrecerealtime_buffer表发生错误，返回值!", num5.ToString()));
							logHelper.WriteError(errorMsg1);
						}
					}
					catch (Exception exception1)
					{
						Exception exception = exception1;
						ErrorMsg errorMsg2 = new ErrorMsg("IORegionTimer", "AdminRegionAlarmInsert", string.Concat("车载电话为：", row["SimNum"].ToString(), "的出入行政区报警报文插入数据库发生错误! 信息：", exception.Message));
						logHelper.WriteError(errorMsg2);
					}
				}
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				LogHelper logHelper1 = new LogHelper();
				ErrorMsg errorMsg3 = new ErrorMsg("IORegionTimer", "AdminRegionAlarmInsert", string.Concat("将出入行政区报警报文插入gpsrecbuffer表 、gpsrecerealtime_buffer表发生错误!", exception2.Message));
				logHelper1.WriteError(errorMsg3);
			}
		}

		private DataTable GetCurrentPosInfoData(string sPresTime)
		{
			DataTable dataBySP;
			try
			{
				string str = "GpsPicServer_GetAdminRegionAlarmSet";
				SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@preTime", sPresTime) };
				dataBySP = SqlDataAccess.getDataBySP(str, sqlParameter);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg = new ErrorMsg("IORegionTimer", "GetCurrentPosInfoData", string.Concat("获取基础行政区报警报文发生错误!", exception.Message));
				logHelper.WriteError(errorMsg);
				dataBySP = null;
			}
			return dataBySP;
		}

		private void GetCurrentPosInfoTimer()
		{
			this.dtAdminRegionList = this.GetCurrentPosInfoData(this.sAdminRegionPreTime);
			DataTable dataTable = this.dtAdminRegionList.Clone();
			if (this.dtAdminRegionList != null && this.dtAdminRegionList.Rows.Count > 0)
			{
				dataTable.Clear();
				int count = this.dtAdminRegionList.Rows.Count;
				string[] strArrays = new string[count];
				this.sAdminRegionPreTime = this.dtAdminRegionList.Rows[0]["preTime"].ToString();
				for (int i = 0; i < count; i++)
				{
					double num = 0;
					double num1 = 0;
					string empty = string.Empty;
					string str = string.Empty;
					bool flag = false;
					DataRow item = this.dtAdminRegionList.Rows[i];
					if (item["longitude"] == DBNull.Value || item["latitude"] == DBNull.Value)
					{
						ErrorMsg errorMsg = new ErrorMsg("出入行政区报警", "解析经纬度", "经纬度为null")
						{
							ClassName = "service",
							FunctionName = "tGetCurrentPosInfoTimer_Elapsed"
						};
						(new LogHelper()).WriteError(errorMsg);
					}
					else
					{
						num = double.Parse(item["longitude"].ToString());
						num1 = double.Parse(item["latitude"].ToString());
						empty = item["adminregionid"].ToString();
						str = item["SimNum"].ToString();
						flag = (int.Parse(item["AlarmStatus"].ToString()) != 0 ? false : true);
						string[] str1 = new string[] { num.ToString(), ",", num1.ToString(), ",", empty, ",", str.ToString(), "$", flag.ToString() };
						strArrays[i] = string.Concat(str1);
					}
				}
				string[] strArrays1 = ReadDataFromGis.servicerIsInRegions(strArrays);
				if (strArrays1 == null)
				{
					ErrorMsg errorMsg1 = new ErrorMsg("IORegionTimer", "GetCurrentPosInfoTimer", "调用webservice查询是否在区域内返回null");
					this.logHelper.WriteError(errorMsg1);
					return;
				}
				for (int j = 0; j < (int)strArrays1.Length; j++)
				{
					string str2 = strArrays1[j];
					string[] strArrays2 = str2.Split(new char[] { ',' });
					string str3 = strArrays2[1];
					string[] strArrays3 = str3.Split(new char[] { '$' });
					if (bool.Parse(strArrays2[0]) == bool.Parse(strArrays3[1]))
					{
						foreach (DataRow row in this.dtAdminRegionList.Rows)
						{
							if (!strArrays3[0].ToString().Equals(row["SimNum"].ToString()))
							{
								continue;
							}
							DataRow itemArray = dataTable.NewRow();
							itemArray.ItemArray = row.ItemArray;
							dataTable.Rows.Add(itemArray);
							break;
						}
					}
				}
				this.AdminRegionAlarmInsert(dataTable);
			}
		}

		public override void start()
		{
			try
			{
				this.tGetCurrentPosInfoTimer = new Timer((double)this.iGetCurrentPosInfoInterval);
				this.tGetCurrentPosInfoTimer.Elapsed += new ElapsedEventHandler(this.tGetCurrentPosInfoTimer_Elapsed);
				this.tGetCurrentPosInfoTimer.Enabled = true;
				this.sAdminRegionPreTime = DateTime.Now.ToString();
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("IORegionTimer", "start", string.Concat("启动出入行政区报警服务失败", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.tGetCurrentPosInfoTimer.Stop();
		}

		private void tGetCurrentPosInfoTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tGetCurrentPosInfoTimer.Enabled = false;
			try
			{
				try
				{
					this.GetCurrentPosInfoTimer();
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("IORegionTimer", "tGetCurrentPosInfoTimer_Elapsed", string.Concat("出入行政区报警例外", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tGetCurrentPosInfoTimer.Interval = (double)this.iGetCurrentPosInfoInterval;
				this.tGetCurrentPosInfoTimer.Enabled = true;
			}
		}
	}
}