using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Timers;
using ParamLibrary.CmdParamInfo;
using Library;
using ParamLibrary.Application;

namespace Bussiness
{
	public class CarInOutOfRangeOnTime : ProcessBase
	{
		private DownData myDownData = new DownData(0);

		private TxtMsg txtMsg = new TxtMsg();

		private Timer tGetConfigInfo;

		private Timer tGetCurrentPosInfo;

		private int iGetConfigInfo = 60000 * ReadDataFromXml.GetInOutConfig;

		private int iGetCurrentPosInfo = 60000 * ReadDataFromXml.GetInOutPos;

		private Dictionary<int, CarInOutOfRangeTimeInfo> infoList = new Dictionary<int, CarInOutOfRangeTimeInfo>();

		public CarInOutOfRangeOnTime()
		{
		}

		private void DownToTerminal(string simnum, string alarmType, string alarmTime, string alarmInfo)
		{
			this.setParam(alarmInfo);
			this.myDownData.icar_SendTxtMsg(CmdParam.ParamType.SimNum, simnum, "", CmdParam.CommMode.未知方式, this.txtMsg, 0, alarmInfo);
		}

		private DataTable getConfigInfo(string tTime)
		{
			DataTable dataBySql;
			try
			{
				string str = " select * from GpsCarInOutOfRangeTime a INNER JOIN GpsRegionType b on a.RegionID = b.RegionID where (datediff(ss, a.StartTime, '{0}') > 0 and datediff(ss, a.EndTime, '{1}') < 0) or (datediff(ss, a.EndTime, a.StartTime) > 0 and (datediff(ss, a.StartTime, '{2}') > 0 or datediff(ss, a.EndTime, '{3}') < 0)) ";
				object[] objArray = new object[] { tTime, tTime, tTime, tTime };
				dataBySql = SqlDataAccess.getDataBySql(string.Format(str, objArray));
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "CarInOutOfRangeOnTime",
					FunctionName = "getConfigInfo",
					ErrorText = string.Concat("取得按时进出站配置信息错误，", exception.Message)
				};
				this.logHelper.WriteError(errorMsg);
				dataBySql = null;
			}
			return dataBySql;
		}

		private CarInOutOfRangeTimeInfo getInOutOfRangeTimeInfo(DataRow dr)
		{
			CarInOutOfRangeTimeInfo carInOutOfRangeTimeInfo;
			try
			{
				CarInOutOfRangeTimeInfo str = new CarInOutOfRangeTimeInfo()
				{
					ID = Convert.ToInt32(dr["ID"]),
					SimNum = dr["SimNum"].ToString()
				};
				DateTime dateTime = Convert.ToDateTime(dr["startTime"]);
				str.StartTime = dateTime.ToString("HH:mm:ss");
				DateTime dateTime1 = Convert.ToDateTime(dr["endTime"]);
				str.EndTime = dateTime1.ToString("HH:mm:ss");
				str.MidTime = str.getMidTime();
				str.RegionID = Convert.ToInt32(dr["regionID"]);
				str.RegionDot = dr["regionDot"].ToString();
				str.RegionName = dr["regionName"].ToString();
				str.RegionIndex = Convert.ToInt32(dr["regionIndex"]);
				carInOutOfRangeTimeInfo = str;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "CarInOutOfRangeOnTime",
					FunctionName = "getInOutOfRangeTimeInfo",
					ErrorText = string.Concat("将DataRow信息转换成CarInOutOfRangeTimeInfo错误，", exception.Message)
				};
				this.logHelper.WriteError(errorMsg);
				carInOutOfRangeTimeInfo = null;
			}
			return carInOutOfRangeTimeInfo;
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
					ClassName = "CarInOutOfRangeOnTime",
					FunctionName = "getPosInfo",
					ErrorText = string.Concat("根据simnum取得末次位置信息错误，", exception.Message)
				};
				this.logHelper.WriteError(errorMsg);
				dataBySql = null;
			}
			return dataBySql;
		}

		private void InsertAlarmInfo(DataRow dr, string regionId, int type, string StartTime, string EndTime)
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
				string str1 = "";
				if (type == 1)
				{
					str1 = string.Concat("M150001", regionId);
				}
				else if (type == 2)
				{
					str1 = string.Concat("M150002", regionId);
				}
				string str2 = str1;
				string[] startTime = new string[] { str2, ",", StartTime, ",", EndTime };
				str1 = string.Concat(startTime);
				string str3 = null;
				bool flag = false;
				string str4 = null;
				int num5 = 0;
				try
				{
					SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@wrkid", (object)num), new SqlParameter("@orderid", dr["OrderId"]), new SqlParameter("@userid", empty), new SqlParameter("@telephone", dr["telephone"]), new SqlParameter("@msgType", (object)num2), new SqlParameter("@recetime", dr["ReceTime"]), new SqlParameter("@gpstime", dr["GpsTime"]), new SqlParameter("@starCondition", dr["StarCondition"]), new SqlParameter("@starNum", dr["StarNum"]), new SqlParameter("@carStatu", (object)num3), new SqlParameter("@carStatuEx", (object)num4), new SqlParameter("@carCondition", dr["CarCondition"]), new SqlParameter("@Longitude", dr["Longitude"]), new SqlParameter("@Latitude", dr["Latitude"]), new SqlParameter("@direct", dr["Direct"]), new SqlParameter("@speed", dr["Speed"]), new SqlParameter("@Reserved", (object)num1), new SqlParameter("@TransportStatus", dr["TransportStatus"]), new SqlParameter("@Accelerration", dr["Accelerration"]), new SqlParameter("@Altitude", dr["Altitude"]), new SqlParameter("@DistanceDiff", dr["DistanceDiff"]), new SqlParameter("@commflag", dr["CommFlag"]), new SqlParameter("@addType", dr["AddMsgType"]), new SqlParameter("@addTxt", str1), new SqlParameter("@DutyStr", str3), new SqlParameter("@isPic", (object)flag), new SqlParameter("@pic", str4), new SqlParameter("@alarmInfo", str), new SqlParameter("@cameraID", (object)num5) };
					string str5 = "GpsPicServer_Alarm_Insert";
					string str6 = "GpsPicServer_RealTime_Insert";
					int num6 = SqlDataAccess.insertBySp(str5, sqlParameter);
					if (num6 > 0)
					{
						LogMsg logMsg = new LogMsg("CarInOutOfRangeOnTime", "InsertAlarmInfo", "");
						if (type != 1)
						{
							logMsg.Msg = string.Concat("车载电话为：", dr["telephone"].ToString(), "的未按时 出 站报警报文已插入gpsrecebuffer");
						}
						else
						{
							logMsg.Msg = string.Concat("车载电话为：", dr["telephone"].ToString(), "的未按时 入 站报警报文已插入gpsrecebuffer");
						}
						this.logHelper.WriteLog(logMsg);
					}
					else
					{
						ErrorMsg errorMsg = new ErrorMsg("CarInOutOfRangeOnTime", "InsertAlarmInfo", string.Concat("将未按时出入站报警报文插入gpsrecbuffer表错误，返回值!", num6.ToString()));
						this.logHelper.WriteError(errorMsg);
					}
					int num7 = SqlDataAccess.insertBySp(str6, sqlParameter);
					if (num7 > 0)
					{
						LogMsg logMsg1 = new LogMsg("CarInOutOfRangeOnTime", "InsertAlarmInfo", string.Concat("车载电话为：", dr["telephone"].ToString(), "的未按时出入站报警报文已插入gpsrecerealtime"));
						if (type != 1)
						{
							logMsg1.Msg = string.Concat("车载电话为：", dr["telephone"].ToString(), "的未按时 出 站报警报文已插入gpsrecerealtime");
						}
						else
						{
							logMsg1.Msg = string.Concat("车载电话为：", dr["telephone"].ToString(), "的未按时 入 站报警报文已插入gpsrecerealtime");
						}
						this.logHelper.WriteLog(logMsg1);
					}
					else
					{
						ErrorMsg errorMsg1 = new ErrorMsg("CarInOutOfRangeOnTime", "InsertAlarmInfo", string.Concat("将未按时出入站报警报文插入gpsrecerealtime_buffer表发生错误，返回值!", num7.ToString()));
						this.logHelper.WriteError(errorMsg1);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg2 = new ErrorMsg("CarInOutOfRangeOnTime", "InsertAlarmInfo", string.Concat("车载电话为：", dr["telephone"].ToString(), "的未按时出入站报警报文插入数据库发生错误! 信息：", exception.Message));
					this.logHelper.WriteError(errorMsg2);
				}
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg3 = new ErrorMsg("CarInOutOfRangeOnTime", "InsertAlarmInfo", string.Concat("将未按时出入站报警报文插入gpsrecbuffer表 、gpsrecerealtime_buffer表发生错误!", exception2.Message));
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
					ClassName = "CarInOutOfRangeOnTime",
					FunctionName = "IsInRegion",
					ErrorText = string.Concat("判断是否在区域内错误，", exception.Message)
				};
				this.logHelper.WriteError(errorMsg);
				flag = false;
			}
			return flag;
		}

		private void ReportAlarmInfo(string simnum, string alarmType, string alarmTime, string alarmInfo)
		{
			try
			{
				string str = "0";
				string str1 = "16901";
				string str2 = "";
				str2 = "02";
				str2 = string.Concat(str2, alarmType);
				DateTime universalTime = Convert.ToDateTime(alarmTime).ToUniversalTime();
				TimeSpan timeSpan = universalTime.Subtract(new DateTime(1970, 1, 1));
				str2 = string.Concat(str2, Convert.ToString((long)timeSpan.TotalSeconds, 16).PadLeft(16, '0'));
				str2 = string.Concat(str2, "00001402");
				string str3 = "";
				byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(alarmInfo.Trim());
				str2 = string.Concat(str2, Convert.ToString((int)bytes.Length, 16).PadLeft(8, '0'));
				for (int i = 0; i < (int)bytes.Length; i++)
				{
					str3 = string.Concat(str3, bytes[i].ToString("X2"));
				}
				str2 = string.Concat(str2, str3);
				string str4 = "insert into GPSJTBSysSndCmd(OrderId, SimNum, CmdCode, CmdContent, AddTime, bSend) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}') ";
				object[] objArray = new object[] { str, simnum, str1, str2, ReadDataFromDB.GetSvrTime(), 0 };
				int num = SqlDataAccess.insertBySql(string.Format(str4, objArray));
				if (num > 0)
				{
					LogMsg logMsg = new LogMsg("CarInOutOfRangeOnTime", "ReportAlarmInfo", "")
					{
						Msg = string.Concat("将平台检测超速报警报文插入GPSJTBSysSndCmd表，AlarmType：", alarmType, "，simnum：", simnum)
					};
					this.logHelper.WriteLog(logMsg);
				}
				else
				{
					ErrorMsg errorMsg = new ErrorMsg("CarInOutOfRangeOnTime", "ReportAlarmInfo", string.Concat("将平台检测超速报警报文插入GPSJTBSysSndCmd表错误，AlarmType：", alarmType, ",返回值!", num.ToString()));
					this.logHelper.WriteError(errorMsg);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg1 = new ErrorMsg("CarInOutOfRangeOnTime", "ReportAlarmInfo", string.Concat("上报警情信息入库错误，", exception.Message));
				this.logHelper.WriteError(errorMsg1);
			}
		}

		private bool setParam(string sMsg)
		{
			this.txtMsg.MsgType = CmdParam.MsgType.详细调度信息;
			this.txtMsg.strMsg = sMsg;
			return true;
		}

		public override void start()
		{
			try
			{
				this.tGetConfigInfo = new Timer(100);
				this.tGetConfigInfo.Elapsed += new ElapsedEventHandler(this.tGetConfigInfo_Elapsed);
				this.tGetConfigInfo.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("CarInOutOfRangeOnTime", "start", string.Concat("开启获取按时进出站配置信息错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
			try
			{
				this.tGetCurrentPosInfo = new Timer((double)this.iGetCurrentPosInfo);
				this.tGetCurrentPosInfo.Elapsed += new ElapsedEventHandler(this.tGetCurrentPosInfo_Elapsed);
				this.tGetCurrentPosInfo.Enabled = true;
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				ErrorMsg errorMsg1 = new ErrorMsg("CarInOutOfRangeOnTime", "start", string.Concat("开启检测是否按时进出站错误,", exception2.Message));
				this.logHelper.WriteError(errorMsg1);
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
						foreach (KeyValuePair<int, CarInOutOfRangeTimeInfo> keyValuePair in this.infoList)
						{
							if (!keyValuePair.Value.IsInAlarm || !keyValuePair.Value.IsOutAlarm)
							{
								continue;
							}
							nums.Add(keyValuePair.Key);
							LogMsg logMsg = new LogMsg("CarInOutOfRangeOnTime", "tGetConfigInfo_Elapsed", "");
							object[] d = new object[] { "删除未按时进出站都已检测过的配置信息ID：", keyValuePair.Value.ID, "，simnum：", keyValuePair.Value.SimNum, "，起始时间：", keyValuePair.Value.StartTime, "，终止时间： ", keyValuePair.Value.EndTime, "，区域ID：", keyValuePair.Value.RegionID, ",Index:", keyValuePair.Value.RegionIndex };
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
								CarInOutOfRangeTimeInfo inOutOfRangeTimeInfo = this.getInOutOfRangeTimeInfo(row);
								if (inOutOfRangeTimeInfo == null)
								{
									continue;
								}
								this.infoList.Add(num1, inOutOfRangeTimeInfo);
								LogMsg logMsg1 = new LogMsg("CarInOutOfRangeOnTime", "tGetConfigInfo_Elapsed", "");
								object[] simNum = new object[] { "增加配置信息ID：", num1, "，simnum：", inOutOfRangeTimeInfo.SimNum, "，起始时间：", inOutOfRangeTimeInfo.StartTime, "，终止时间： ", inOutOfRangeTimeInfo.EndTime, "，区域ID：", inOutOfRangeTimeInfo.RegionID, ",Index:", inOutOfRangeTimeInfo.RegionIndex };
								logMsg1.Msg = string.Concat(simNum);
								this.logHelper.WriteLog(logMsg1);
							}
						}
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("CarInOutOfRangeOnTime", "tGetConfigInfo_Elapsed", string.Concat("获取按时进出站配置信息错误,", exception.Message));
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
					string str = ReadDataFromDB.GetSvrTime().ToString("HH:mm:ss");
					lock (this.infoList)
					{
						foreach (KeyValuePair<int, CarInOutOfRangeTimeInfo> keyValuePair in this.infoList)
						{
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
							DateTime svrTime = ReadDataFromDB.GetSvrTime();
							if (!this.IsInConfigTime(keyValuePair.Value.StartTime, keyValuePair.Value.EndTime, str) || !this.IsInConfigTime(keyValuePair.Value.StartTime, keyValuePair.Value.EndTime, str3) || !(dateTime1.Date == svrTime.Date) || keyValuePair.Value.IsInAlarm)
							{
								if (this.TimeCompareTo(str, keyValuePair.Value.EndTime) <= 0 || this.TimeCompareTo(str3, keyValuePair.Value.EndTime) <= 0 || !(dateTime1.Date == svrTime.Date) || keyValuePair.Value.IsOutAlarm)
								{
									continue;
								}
								if (this.IsInRegion(str1, str2, regionDot))
								{
									DataRow item = posInfo.Rows[0];
									int regionID = keyValuePair.Value.RegionID;
									this.InsertAlarmInfo(item, regionID.ToString(), 2, keyValuePair.Value.StartTime, keyValuePair.Value.EndTime);
								}
								keyValuePair.Value.IsOutAlarm = true;
							}
							else
							{
								if (!this.IsInRegion(str1, str2, regionDot))
								{
									DataRow dataRow = posInfo.Rows[0];
									int num = keyValuePair.Value.RegionID;
									this.InsertAlarmInfo(dataRow, num.ToString(), 1, keyValuePair.Value.StartTime, keyValuePair.Value.EndTime);
								}
								keyValuePair.Value.IsInAlarm = true;
							}
						}
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg()
					{
						ClassName = "CarInOutOfRangeOnTime",
						FunctionName = "tGetCurrentPosInfo_Elapsed",
						ErrorText = string.Concat("检测是否按时进出站错误，", exception.Message)
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