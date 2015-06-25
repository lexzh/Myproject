
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Timers;
using Library;

namespace Bussiness
{
	public class CarBeBackOnTime : ProcessBase
	{
		private Timer tGetConfigInfo;

		private Timer tGetCurrentPosInfo;

		private int iGetConfigInfo = 60000 * ReadDataFromXml.GetBeBackConfig;

		private int iGetCurrentPosInfo = 60000 * ReadDataFromXml.GetBeBackPos;

		private Dictionary<int, CarBeBackTimeInfo> infoList = new Dictionary<int, CarBeBackTimeInfo>();

		public CarBeBackOnTime()
		{
		}

		private CarBeBackTimeInfo getCarBeBackInfo(DataRow dr)
		{
			CarBeBackTimeInfo carBeBackTimeInfo;
			try
			{
				CarBeBackTimeInfo str = new CarBeBackTimeInfo()
				{
					ID = Convert.ToInt32(dr["ID"]),
					SimNum = dr["SimNum"].ToString()
				};
				DateTime dateTime = Convert.ToDateTime(dr["beginTime"]);
				str.BeginTime = dateTime.ToString("HH:mm:ss");
				DateTime dateTime1 = Convert.ToDateTime(dr["endTime"]);
				str.EndTime = dateTime1.ToString("HH:mm:ss");
				str.RegionID = Convert.ToInt32(dr["regionID"]);
				str.RegionDot = dr["regionDot"].ToString();
				str.RegionName = dr["regionName"].ToString();
				carBeBackTimeInfo = str;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "CarBeBackOnTime",
					FunctionName = "getCarBeBackInfo",
					ErrorText = string.Concat("将DataRow信息转换成CarBeBackTimeInfo错误，", exception.Message)
				};
				this.logHelper.WriteError(errorMsg);
				carBeBackTimeInfo = null;
			}
			return carBeBackTimeInfo;
		}

		private DataTable getConfigInfo(string tTime)
		{
			DataTable dataBySql;
			try
			{
				string str = " select * from GpsCarBeBackTime a INNER JOIN GpsRegionType b on a.RegionID = b.RegionID where (datediff(ss, a.BeginTime, '{0}') > 0 and datediff(ss, a.EndTime, '{1}') < 0) or (datediff(ss, a.EndTime, a.BeginTime) > 0 and (datediff(ss, a.BeginTime, '{2}') > 0 or datediff(ss, a.EndTime, '{3}') < 0)) ";
				object[] objArray = new object[] { tTime, tTime, tTime, tTime };
				dataBySql = SqlDataAccess.getDataBySql(string.Format(str, objArray));
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "CarBeBackOnTime",
					FunctionName = "getConfigInfo",
					ErrorText = string.Concat("取得按时归班配置信息错误，", exception.Message)
				};
				this.logHelper.WriteError(errorMsg);
				dataBySql = null;
			}
			return dataBySql;
		}

		private DataTable getPosInfo(string simnum)
		{
			DataTable dataBySql;
			try
			{
				dataBySql = SqlDataAccess.getDataBySql(string.Format(" select * from GpsCarCurrentPosInfo WITH(NOLOCK) where telephone = '{0}' ", simnum));
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "CarBeBackOnTime",
					FunctionName = "getPosInfo",
					ErrorText = string.Concat("根据simnum取得末次位置信息错误，", exception.Message)
				};
				this.logHelper.WriteError(errorMsg);
				dataBySql = null;
			}
			return dataBySql;
		}

		private void InsertAlarmInfo(DataRow dr, string regionId)
		{
			try
			{
				int num = 0;
				string empty = string.Empty;
				int num1 = 1154;
				int num2 = 65;
				string str = string.Empty;
				int num3 = Convert.ToInt32(dr["carstatu"]);
				long num4 = (long)0;
				num4 = (dr["carstatuex"] == DBNull.Value || dr["carstatuex"].ToString().Equals("") ? 1125899906842624L : 1125899906842624L | Convert.ToInt64(dr["carstatuex"]));
				string str1 = string.Concat("M150004", regionId);
				string str2 = null;
				bool flag = false;
				string str3 = null;
				int num5 = 0;
				try
				{
					SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@wrkid", (object)num), new SqlParameter("@orderid", dr["OrderId"]), new SqlParameter("@userid", empty), new SqlParameter("@telephone", dr["telephone"]), new SqlParameter("@msgType", (object)num2), new SqlParameter("@recetime", dr["ReceTime"]), new SqlParameter("@gpstime", dr["GpsTime"]), new SqlParameter("@starCondition", dr["StarCondition"]), new SqlParameter("@starNum", dr["StarNum"]), new SqlParameter("@carStatu", (object)num3), new SqlParameter("@carStatuEx", (object)num4), new SqlParameter("@carCondition", dr["CarCondition"]), new SqlParameter("@Longitude", dr["Longitude"]), new SqlParameter("@Latitude", dr["Latitude"]), new SqlParameter("@direct", dr["Direct"]), new SqlParameter("@speed", dr["Speed"]), new SqlParameter("@Reserved", (object)num1), new SqlParameter("@TransportStatus", dr["TransportStatus"]), new SqlParameter("@Accelerration", dr["Accelerration"]), new SqlParameter("@Altitude", dr["Altitude"]), new SqlParameter("@DistanceDiff", dr["DistanceDiff"]), new SqlParameter("@commflag", dr["CommFlag"]), new SqlParameter("@addType", dr["AddMsgType"]), new SqlParameter("@addTxt", str1), new SqlParameter("@DutyStr", str2), new SqlParameter("@isPic", (object)flag), new SqlParameter("@pic", str3), new SqlParameter("@alarmInfo", str), new SqlParameter("@cameraID", (object)num5) };
					string str4 = "GpsPicServer_Alarm_Insert";
					string str5 = "GpsPicServer_RealTime_Insert";
					int num6 = SqlDataAccess.insertBySp(str4, sqlParameter);
					if (num6 > 0)
					{
						LogMsg logMsg = new LogMsg("CarBeBackOnTime", "InsertAlarmInfo", string.Concat("车载电话为：", dr["telephone"].ToString(), "的未按时归班报警报文已插入gpsrecebuffer"));
						this.logHelper.WriteLog(logMsg);
					}
					else
					{
						ErrorMsg errorMsg = new ErrorMsg("CarBeBackOnTime", "InsertAlarmInfo", string.Concat("将未按时归班报警报文插入gpsrecbuffer表错误，返回值!", num6.ToString()));
						this.logHelper.WriteError(errorMsg);
					}
					int num7 = SqlDataAccess.insertBySp(str5, sqlParameter);
					if (num7 > 0)
					{
						LogMsg logMsg1 = new LogMsg("CarBeBackOnTime", "InsertAlarmInfo", string.Concat("车载电话为：", dr["telephone"].ToString(), "的未按时归班报警报文已插入gpsrecerealtime"));
						this.logHelper.WriteLog(logMsg1);
					}
					else
					{
						ErrorMsg errorMsg1 = new ErrorMsg("CarBeBackOnTime", "InsertAlarmInfo", string.Concat("将未按时归班报警报文插入gpsrecerealtime_buffer表发生错误，返回值!", num7.ToString()));
						this.logHelper.WriteError(errorMsg1);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg2 = new ErrorMsg("CarBeBackOnTime", "InsertAlarmInfo", string.Concat("车载电话为：", dr["telephone"].ToString(), "的未按时归班报警报文插入数据库发生错误! 信息：", exception.Message));
					this.logHelper.WriteError(errorMsg2);
				}
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg3 = new ErrorMsg("CarBeBackOnTime", "InsertAlarmInfo", string.Concat("将未按时归班报警报文插入gpsrecbuffer表 、gpsrecerealtime_buffer表发生错误!", exception2.Message));
				logHelper.WriteError(errorMsg3);
			}
		}

		private bool IsInConfigTime(string beginTime, string endTime, string nowTime)
		{
			if (this.TimeCompareTo(beginTime, endTime) < 0 && (this.TimeCompareTo(endTime, nowTime) < 0 || this.TimeCompareTo(nowTime, beginTime) < 0) || this.TimeCompareTo(beginTime, endTime) > 0 && this.TimeCompareTo(beginTime, nowTime) > 0 && this.TimeCompareTo(endTime, nowTime) < 0)
			{
				return false;
			}
			return true;
		}

		private bool IsInRegion(string Lon, string Lat, string regionDot)
		{
			bool flag;
			try
			{
				string str = regionDot.Replace("*", "\\");
				char[] chrArray = new char[] { '\\' };
				string[] strArrays = str.Trim(chrArray).Split(new char[] { '\\' });
				flag = (double.Parse(Lon) <= double.Parse(strArrays[0]) || double.Parse(Lon) >= double.Parse(strArrays[4]) || double.Parse(Lat) >= double.Parse(strArrays[5]) || double.Parse(Lat) <= double.Parse(strArrays[1]) ? false : true);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "CarBeBackOnTime",
					FunctionName = "IsInRegion",
					ErrorText = string.Concat("判断是否在区域内错误，", exception.Message)
				};
				this.logHelper.WriteError(errorMsg);
				flag = false;
			}
			return flag;
		}

		public override void start()
		{
			try
			{
				this.tGetConfigInfo = new Timer(100);
				this.tGetConfigInfo.Elapsed += new ElapsedEventHandler(this.tGetConfigInfo_Elapsed);
				this.tGetConfigInfo.Enabled = true;
				this.tGetCurrentPosInfo = new Timer((double)this.iGetCurrentPosInfo);
				this.tGetCurrentPosInfo.Elapsed += new ElapsedEventHandler(this.tGetCurrentPosInfo_Elapsed);
				this.tGetCurrentPosInfo.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "CarBeBackOnTime",
					FunctionName = "start",
					ErrorText = string.Concat("开启检测是否按时归班错误，", exception.Message)
				};
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.tGetConfigInfo.Stop();
			this.tGetCurrentPosInfo.Stop();
		}

		private void tGetConfigInfo_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tGetConfigInfo.Enabled = false;
			this.tGetConfigInfo.Interval = (double)this.iGetConfigInfo;
			try
			{
				try
				{
					string str = ReadDataFromDB.GetSvrTime().ToString("HH:mm:ss");
					lock (this.infoList)
					{
						List<int> nums = new List<int>();
						foreach (KeyValuePair<int, CarBeBackTimeInfo> keyValuePair in this.infoList)
						{
							if (this.IsInConfigTime(keyValuePair.Value.BeginTime, keyValuePair.Value.EndTime, str))
							{
								continue;
							}
							nums.Add(keyValuePair.Key);
							LogMsg logMsg = new LogMsg("CarBeBackOnTime", "tGetConfigInfo_Elapsed", "");
							object[] d = new object[] { "删除设置时间段外的配置信息ID：", keyValuePair.Value.ID, "，simnum：", keyValuePair.Value.SimNum, "，起始时间：", keyValuePair.Value.BeginTime, "，终止时间： ", keyValuePair.Value.EndTime, "，区域ID：", keyValuePair.Value.RegionID };
							logMsg.Msg = string.Concat(d);
							this.logHelper.WriteLog(logMsg);
						}
						foreach (int num in nums)
						{
							this.infoList.Remove(num);
						}
						DataTable configInfo = this.getConfigInfo(str);
						if (configInfo != null && configInfo.Rows.Count > 0)
						{
							foreach (DataRow row in configInfo.Rows)
							{
								int num1 = Convert.ToInt32(row["ID"]);
								if (this.infoList.Keys.Contains<int>(num1))
								{
									continue;
								}
								CarBeBackTimeInfo carBeBackInfo = this.getCarBeBackInfo(row);
								if (carBeBackInfo == null)
								{
									continue;
								}
								this.infoList.Add(num1, carBeBackInfo);
								LogMsg logMsg1 = new LogMsg("CarBeBackOnTime", "tGetConfigInfo_Elapsed", "");
								object[] simNum = new object[] { "增加配置信息ID：", num1, "，simnum：", carBeBackInfo.SimNum, "，起始时间：", carBeBackInfo.BeginTime, "，终止时间： ", carBeBackInfo.EndTime, "，区域ID：", carBeBackInfo.RegionID };
								logMsg1.Msg = string.Concat(simNum);
								this.logHelper.WriteLog(logMsg1);
							}
						}
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg()
					{
						ClassName = "CarBeBackOnTime",
						FunctionName = "tGetConfigInfo_Elapsed",
						ErrorText = string.Concat("获取配置信息错误，", exception.Message)
					};
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tGetConfigInfo.Enabled = true;
			}
		}

		private void tGetCurrentPosInfo_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tGetCurrentPosInfo.Enabled = false;
			try
			{
				try
				{
					string str = DateTime.Now.ToString("HH:mm:ss");
					lock (this.infoList)
					{
						foreach (KeyValuePair<int, CarBeBackTimeInfo> keyValuePair in this.infoList)
						{
							if (!this.IsInConfigTime(keyValuePair.Value.BeginTime, keyValuePair.Value.EndTime, str) || keyValuePair.Value.IsAlarm)
							{
								continue;
							}
							DataTable posInfo = this.getPosInfo(keyValuePair.Value.SimNum);
							if (posInfo == null || posInfo.Rows.Count <= 0)
							{
								continue;
							}
							string str1 = posInfo.Rows[0]["Longitude"].ToString();
							string str2 = posInfo.Rows[0]["Latitude"].ToString();
							string regionDot = keyValuePair.Value.RegionDot;
							DateTime dateTime = Convert.ToDateTime(posInfo.Rows[0]["gpsTime"]);
							string str3 = dateTime.ToString("HH:mm:ss");
							DateTime dateTime1 = Convert.ToDateTime(posInfo.Rows[0]["gpsTime"]);
							if (this.IsInRegion(str1, str2, regionDot) || !this.IsInConfigTime(keyValuePair.Value.BeginTime, keyValuePair.Value.EndTime, str3) || !(dateTime1.Date == DateTime.Now.Date))
							{
								continue;
							}
							this.InsertAlarmInfo(posInfo.Rows[0], keyValuePair.Value.RegionID.ToString());
							keyValuePair.Value.IsAlarm = true;
						}
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg()
					{
						ClassName = "CarBeBackOnTime",
						FunctionName = "tGetCurrentPosInfo_Elapsed",
						ErrorText = string.Concat("检测是否按时归班错误，", exception.Message)
					};
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tGetCurrentPosInfo.Enabled = true;
			}
		}

		private int TimeCompareTo(string time1, string time2)
		{
			DateTime dateTime = Convert.ToDateTime(string.Concat("1970-01-01 ", time1));
			DateTime dateTime1 = Convert.ToDateTime(string.Concat("1970-01-01 ", time2));
			return dateTime.CompareTo(dateTime1);
		}
	}
}