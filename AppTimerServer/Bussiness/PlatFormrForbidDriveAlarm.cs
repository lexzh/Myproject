using GisServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Timers;
using Library;
using ParamLibrary.CmdParamInfo;
using ParamLibrary.Application;

namespace Bussiness
{
	public class PlatFormrForbidDriveAlarm : ProcessBase
	{
		private Timer tCheckForbidDriveAlarm;

		private int iCheckInterval = 60000 * ReadDataFromXml.numForbidDriveInterval;

		private DateTime dtNow = ReadDataFromDB.GetSvrTime();

		private DateTime dtTmp;

		private DataTable dtConfig = new DataTable();

		private Hashtable htCaridStat = new Hashtable();

		private List<string> AlarmListFirst = new List<string>();

		private List<string> AlarmListMid = new List<string>();

		private List<string> AlarmListEnd = new List<string>();

		private string sSendMsg = ReadDataFromXml.SendDriverAlarmMsg;

		private DownData myDownData = new DownData(0);

		public PlatFormrForbidDriveAlarm()
		{
		}

		private void CheckAndInsert()
		{
			try
			{
				if (this.AlarmListFirst != null && this.AlarmListFirst.Count > 0)
				{
					foreach (string alarmListFirst in this.AlarmListFirst)
					{
						DataRow[] dataRowArray = this.dtConfig.Select(string.Concat("CarID=", alarmListFirst));
						this.InsertAlarmInfo(dataRowArray[0], -99999);
						if (!ReadDataFromXml.IsSendDriverAlarm)
						{
							continue;
						}
						this.execSendMsg(dataRowArray[0]);
					}
				}
				this.AlarmListFirst.Clear();
				if (this.AlarmListMid != null && this.AlarmListMid.Count > 0)
				{
					foreach (string alarmListMid in this.AlarmListMid)
					{
						DataRow[] dataRowArray1 = this.dtConfig.Select(string.Concat("CarID=", alarmListMid));
						this.InsertAlarmInfo(dataRowArray1[0], -99998);
					}
				}
				this.AlarmListMid.Clear();
				if (this.AlarmListEnd != null && this.AlarmListEnd.Count > 0)
				{
					foreach (string alarmListEnd in this.AlarmListEnd)
					{
						DataRow[] dataRowArray2 = this.dtConfig.Select(string.Concat("CarID=", alarmListEnd));
						this.InsertAlarmInfo(dataRowArray2[0], -99997);
					}
				}
				this.AlarmListEnd.Clear();
			}
			catch (Exception exception)
			{
				ErrorMsg errorMsg = new ErrorMsg("PlatFormrForbidDriveAlarm", "CheckAndInsert", exception.Message);
				this.logHelper.WriteError(errorMsg);
			}
		}

		private void execSendMsg(DataRow dr)
		{
			LogMsg logMsg = new LogMsg();
			if (string.IsNullOrEmpty(this.sSendMsg))
			{
				logMsg.Msg = "短息内容为空，不下发短信通知。";
				this.logHelper.WriteLog(logMsg);
				return;
			}
			logMsg.Msg = "开始发送短信通信通知...";
			this.logHelper.WriteLog(logMsg);
			bool flag = this.sSendMsg.IndexOf("(C)") >= 0;
			string str = dr["StarCondition"].ToString();
			string str1 = dr["Longitude"].ToString();
			string str2 = dr["Latitude"].ToString();
			string str3 = dr["CarNum"].ToString();
			string str4 = dr["CarId"].ToString();
			string str5 = dr["telephone"].ToString();
			dr["FirstConnectorName"].ToString();
			string str6 = dr["FirstConnectTele"].ToString();
			dr["ConnectorName"].ToString();
			string str7 = dr["ConnectTele"].ToString();
			string str8 = "";
			string str9 = "";
			TxtMsg txtMsg = new TxtMsg();
			try
			{
				str1 = str1.Substring(0, str1.IndexOf('.') + 7);
				str2 = str2.Substring(0, str2.IndexOf('.') + 7);
			}
			catch
			{
			}
			if (flag)
			{
				str8 = ReadDataFromGis.QueryAllLayerByPoint(str1, str2);
			}
			str9 = (str != "1" ? "未定位" : "定位");
			this.sSendMsg = this.sSendMsg.Replace("(A)", str3);
			string str10 = this.sSendMsg;
			DateTime now = DateTime.Now;
			this.sSendMsg = str10.Replace("(B)", now.ToString("yyyyMMdd HH:mm:ss"));
			this.sSendMsg = this.sSendMsg.Replace("(C)", str8);
			this.sSendMsg = this.sSendMsg.Replace("(D)", str9);
			txtMsg.strMsg = this.sSendMsg;
			txtMsg.MsgType = CmdParam.MsgType.UCS2手机短信;
			txtMsg.OrderCode = CmdParam.OrderCode.调度;
			txtMsg.CarId = str4;
			txtMsg.SimNum = str5;
			if (!string.IsNullOrEmpty(str6))
			{
				this.myDownData.icar_SendTxtMsg(CmdParam.ParamType.SimNum, str6, "", CmdParam.CommMode.短信, txtMsg, 0, "禁驾报警通知第一联系人");
			}
			if (!string.IsNullOrEmpty(str7))
			{
				this.myDownData.icar_SendTxtMsg(CmdParam.ParamType.SimNum, str7, "", CmdParam.CommMode.短信, txtMsg, 0, "禁驾报警通知第二联系人");
			}
		}

		private DataTable getConfig()
		{
			DataTable dataBySP;
			try
			{
				SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@preTime", (object)this.dtNow), new SqlParameter("@currentTime", (object)this.dtTmp) };
				dataBySP = SqlDataAccess.getDataBySP("GpsPicServer_GetForbidDriveAlarmConfig_new", sqlParameter);
			}
			catch (Exception exception)
			{
				ErrorMsg errorMsg = new ErrorMsg("PlatFormrForbidDriveAlarm", "getConfig", exception.Message);
				this.logHelper.WriteError(errorMsg);
				dataBySP = null;
			}
			return dataBySP;
		}

		private void InsertAlarmInfo(DataRow dr, int AddMsgType)
		{
			ErrorMsg errorMsg = new ErrorMsg("PlatFormrForbidDriveAlarm", "InsertAlarmInfo", "");
			LogMsg logMsg = new LogMsg("PlatFormrForbidDriveAlarm", "InsertAlarmInfo", "");
			try
			{
				int num = 0;
				string empty = string.Empty;
				int num1 = int.Parse(dr["carstatu"].ToString());
				long num2 = 36028797018963968L | long.Parse(dr["carStatuex"].ToString());
				int num3 = 1154;
				int num4 = 65;
				string str = string.Empty;
				string str1 = null;
				bool flag = false;
				string str2 = null;
				int num5 = 0;
				try
				{
					SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@wrkid", (object)num), new SqlParameter("@orderid", dr["OrderId"]), new SqlParameter("@userid", empty), new SqlParameter("@telephone", dr["telephone"]), new SqlParameter("@msgType", (object)num4), new SqlParameter("@recetime", dr["ReceTime"]), new SqlParameter("@gpstime", dr["GpsTime"]), new SqlParameter("@starCondition", dr["StarCondition"]), new SqlParameter("@starNum", dr["StarNum"]), new SqlParameter("@carStatu", (object)num1), new SqlParameter("@carStatuEx", (object)num2), new SqlParameter("@carCondition", dr["CarCondition"]), new SqlParameter("@Longitude", dr["Longitude"]), new SqlParameter("@Latitude", dr["Latitude"]), new SqlParameter("@direct", dr["Direct"]), new SqlParameter("@speed", dr["Speed"]), new SqlParameter("@Reserved", (object)num3), new SqlParameter("@TransportStatus", dr["TransportStatus"]), new SqlParameter("@Accelerration", dr["Accelerration"]), new SqlParameter("@Altitude", dr["Altitude"]), new SqlParameter("@DistanceDiff", dr["DistanceDiff"]), new SqlParameter("@commflag", dr["CommFlag"]), new SqlParameter("@addType", (object)AddMsgType), new SqlParameter("@addTxt", dr["AddMsgTxt"]), new SqlParameter("@DutyStr", str1), new SqlParameter("@isPic", (object)flag), new SqlParameter("@pic", str2), new SqlParameter("@alarmInfo", str), new SqlParameter("@cameraID", (object)num5) };
					string str3 = "GpsPicServer_ReceBuffer_Insert";
					string str4 = "GpsPicServer_RealTime_Insert";
					if (AddMsgType != -99997)
					{
						if (SqlDataAccess.insertBySp(str3, sqlParameter) > 0)
						{
							logMsg.Msg = string.Concat("车载电话为：", dr["telephone"].ToString(), "的报警报文已插入gpsrecebuffer");
							this.logHelper.WriteLog(logMsg);
						}
						else
						{
							this.logHelper.WriteError(errorMsg);
						}
					}
					int num6 = SqlDataAccess.insertBySp(str4, sqlParameter);
					if (num6 > 0)
					{
						logMsg.Msg = string.Concat("车载电话为：", dr["telephone"].ToString(), "的平台禁驾报警报文已插入gpsrecerealtime");
						this.logHelper.WriteLog(logMsg);
					}
					else
					{
						errorMsg.ErrorText = string.Concat("将平台禁驾报警报文插入gpsrecerealtime_buffer表发生错误，返回值!", num6.ToString());
						this.logHelper.WriteError(errorMsg);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					errorMsg.ErrorText = string.Concat("车载电话为：", dr["telephone"].ToString(), "的报警报文插入数据库发生错误! 信息：", exception.Message);
					this.logHelper.WriteError(errorMsg);
				}
			}
			catch (Exception exception2)
			{
				errorMsg.ErrorText = string.Concat("将报警报文插入gpsrecbuffer表 、gpsrecerealtime_buffer表发生错误!", exception2.Message);
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void start()
		{
			this.tCheckForbidDriveAlarm = new Timer((double)this.iCheckInterval);
			this.tCheckForbidDriveAlarm.Elapsed += new ElapsedEventHandler(this.tCheckForbidDriveAlarm_Elapsed);
			this.tCheckForbidDriveAlarm.Enabled = true;
		}

		public override void stop()
		{
			if (this.tCheckForbidDriveAlarm != null)
			{
				this.tCheckForbidDriveAlarm.Stop();
			}
		}

		private void tCheckForbidDriveAlarm_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tCheckForbidDriveAlarm.Enabled = false;
			this.dtTmp = ReadDataFromDB.GetSvrTime();
			DataTable dataBySP = null;
			SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@CurrentTime", (object)this.dtTmp) };
			dataBySP = SqlDataAccess.getDataBySP("GpsPicServer_GetForbidDriveAlarmConfig", sqlParameter);
			ArrayList arrayLists = new ArrayList();
			foreach (int key in this.htCaridStat.Keys)
			{
				if ((int)dataBySP.Select(string.Concat("Carid=", key)).Length != 0)
				{
					continue;
				}
				arrayLists.Add(key);
			}
			foreach (int arrayList in arrayLists)
			{
				this.htCaridStat.Remove(arrayList);
			}
			try
			{
				try
				{
					this.dtConfig = this.getConfig();
					if (this.dtConfig != null && this.dtConfig.Rows.Count > 0)
					{
						foreach (DataRow row in this.dtConfig.Rows)
						{
							int num = int.Parse(row["CarId"].ToString());
							if (!this.htCaridStat.Contains(num))
							{
								this.htCaridStat[num] = 0;
							}
							double num1 = double.Parse(row["Speed"].ToString());
							int num2 = int.Parse(this.htCaridStat[num].ToString());
							DateTime dateTime = DateTime.Parse(row["GpsTime"].ToString());
							string str = dateTime.ToString("HH:mm");
							DateTime dateTime1 = DateTime.Parse(row["StartTime"].ToString());
							string str1 = dateTime1.ToString("HH:mm");
							DateTime dateTime2 = DateTime.Parse(row["EndTime"].ToString());
							string str2 = dateTime2.ToString("HH:mm");
							if ((str1.CompareTo(str2) >= 0 || str.CompareTo(str1) <= 0 || str.CompareTo(str2) >= 0) && (str1.CompareTo(str2) <= 0 || str.CompareTo(str1) <= 0 && str.CompareTo(str2) >= 0))
							{
								if (num2 != 1)
								{
									continue;
								}
								this.AlarmListEnd.Add(num.ToString());
								this.htCaridStat[num] = 0;
							}
							else if (num1 <= 0.001)
							{
								if (num2 != 1)
								{
									continue;
								}
								this.AlarmListEnd.Add(num.ToString());
								this.htCaridStat[num] = 0;
							}
							else if (num2 != 0)
							{
								this.AlarmListMid.Add(num.ToString());
							}
							else
							{
								this.AlarmListFirst.Add(num.ToString());
								this.htCaridStat[num] = 1;
							}
						}
						this.dtNow = this.dtTmp;
					}
					this.CheckAndInsert();
				}
				catch (Exception exception)
				{
					ErrorMsg errorMsg = new ErrorMsg("PlatFormrForbidDriveAlarm", "tCheckForbidDrivingAlarm_Elapsed", exception.Message);
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tCheckForbidDriveAlarm.Enabled = true;
			}
		}
	}
}