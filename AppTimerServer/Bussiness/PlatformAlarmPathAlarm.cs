using GisServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Timers;
using System.Xml;
using ParamLibrary.CmdParamInfo;
using Library;
using ParamLibrary.Application;

namespace Bussiness
{
	public class PlatformAlarmPathAlarm : ProcessBase
	{
		private DownData myDownData = new DownData(0);

		private TxtMsg txtMsg = new TxtMsg();

		private Timer tCheckPathAlarm;

		private DateTime dtNow = ReadDataFromDB.GetSvrTime();

		private int iCheckPathAlarm = 60000 * ReadDataFromXml.PathAlarmInterval;

		private XmlDocument xDoc;

		private Hashtable htCarPathInfo = new Hashtable();

		private Hashtable htCarPosInfo = new Hashtable();

		private Hashtable htAlarmList = new Hashtable();

		private bool IsJudge;

		private DataTable dtConfigInfo;

		private List<string> AlarmListFirst;

		private List<string> AlarmListMid;

		private List<string> AlarmListEnd;

		public PlatformAlarmPathAlarm()
		{
		}

		private bool AnalysisOutputXML(string outXML)
		{
			bool flag;
			try
			{
				this.AlarmListFirst = new List<string>();
				this.AlarmListMid = new List<string>();
				this.AlarmListEnd = new List<string>();
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(outXML);
				XmlNode xmlNodes = xmlDocument.SelectSingleNode("//Cars");
				if (xmlNodes == null)
				{
					ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmPathAlarm", "AnalysisOutputXML", "")
					{
						ErrorText = "返回的xml错误，不包含Cars节点"
					};
					this.logHelper.WriteError(errorMsg);
					flag = false;
				}
				else
				{
					foreach (XmlElement childNode in xmlNodes.ChildNodes)
					{
						XmlElement itemOf = (XmlElement)childNode.GetElementsByTagName("IsOnRoad")[0];
						XmlElement xmlElement = (XmlElement)childNode.GetElementsByTagName("PathId")[0];
						int num = Convert.ToInt32(childNode.Attributes["value"].Value);
						if (!itemOf.Attributes["value"].Value.Equals("0") || !ReadDataFromXml.IsContinuousAlarm && this.htAlarmList[num] != null && (this.htAlarmList[num] as AlarmCarInfo).IsAlarm)
						{
							if (itemOf.Attributes["value"].Value.Equals("0") || !(this.htAlarmList[num] as AlarmCarInfo).IsAlarm)
							{
								continue;
							}
							(this.htAlarmList[num] as AlarmCarInfo).IsAlarm = false;
							this.AlarmListEnd.Add(childNode.Attributes["value"].Value);
						}
						else
						{
							if ((this.htAlarmList[num] as AlarmCarInfo).IsAlarm)
							{
								this.AlarmListMid.Add(childNode.Attributes["value"].Value);
							}
							else
							{
								this.AlarmListFirst.Add(childNode.Attributes["value"].Value);
							}
							(this.htAlarmList[num] as AlarmCarInfo).IsAlarm = true;
						}
					}
					flag = true;
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg1 = new ErrorMsg("PlatformAlarmPathAlarm", "AnalysisOutputXML", string.Concat("解析返回XML错误,", exception.Message));
				this.logHelper.WriteError(errorMsg1);
				flag = false;
			}
			return flag;
		}

		private void CheckAndAnalysis()
		{
			if (this.IsJudge)
			{
				LogMsg logMsg = new LogMsg("PlatformAlarmPathAlarm", "tCheckPathAlarm_Elapsed", "")
				{
					Msg = string.Concat("偏移路线报警传 入 的xml：\r\n", this.xDoc.OuterXml)
				};
				this.logHelper.WriteLog(logMsg);
				string str = ReadDataFromGis.IsCarsOnRoad(this.xDoc.OuterXml);
				logMsg.Msg = string.Concat("偏移路线报警传 回 的xml：\r\n", str);
				this.logHelper.WriteLog(logMsg);
				if (!string.IsNullOrEmpty(str) && this.AnalysisOutputXML(str))
				{
					if (this.AlarmListFirst != null && this.AlarmListFirst.Count > 0)
					{
						foreach (string alarmListFirst in this.AlarmListFirst)
						{
							DataRow[] dataRowArray = this.dtConfigInfo.Select(string.Concat("carid = ", alarmListFirst));
							this.InsertAlarmInfo(dataRowArray[0], CmdParam.CarAlarmState.偏移路线, -99999);
							if (!ReadDataFromXml.IschkDownMsg)
							{
								continue;
							}
							this.DownToTerminal(dataRowArray[0]["simnum"].ToString(), "0001", dataRowArray[0]["gpsTime"].ToString(), "您的车辆发生偏移路线报警。", alarmListFirst);
						}
						this.AlarmListFirst.Clear();
						this.AlarmListFirst = null;
					}
					if (this.AlarmListMid != null && this.AlarmListMid.Count > 0)
					{
						foreach (string alarmListMid in this.AlarmListMid)
						{
							DataRow[] dataRowArray1 = this.dtConfigInfo.Select(string.Concat("carid = ", alarmListMid));
							this.InsertAlarmInfo(dataRowArray1[0], CmdParam.CarAlarmState.偏移路线, -99998);
							if (!ReadDataFromXml.IschkDownMsg)
							{
								continue;
							}
							this.DownToTerminal(dataRowArray1[0]["simnum"].ToString(), "0001", dataRowArray1[0]["gpsTime"].ToString(), "您的车辆发生偏移路线报警。", alarmListMid);
						}
						this.AlarmListMid.Clear();
						this.AlarmListMid = null;
					}
					if (this.AlarmListEnd != null && this.AlarmListEnd.Count > 0)
					{
						foreach (string alarmListEnd in this.AlarmListEnd)
						{
							DataRow[] dataRowArray2 = this.dtConfigInfo.Select(string.Concat("carid = ", alarmListEnd));
							this.InsertAlarmInfo(dataRowArray2[0], CmdParam.CarAlarmState.偏移路线, -99997);
						}
						this.AlarmListEnd.Clear();
						this.AlarmListEnd = null;
					}
				}
				this.IsJudge = false;
			}
		}

		private void clearAlarmCar()
		{
			try
			{
				DataTable dataBySql = SqlDataAccess.getDataBySql(" select * from GpsJtbCarPathAlarm_Platform ");
				if (dataBySql == null || dataBySql.Rows.Count <= 0)
				{
					this.htAlarmList.Clear();
				}
				List<int> nums = new List<int>();
				Hashtable hashtables = new Hashtable();
				foreach (int key in this.htAlarmList.Keys)
				{
					if (hashtables[key] == null)
					{
						hashtables[key] = "";
					}
					List<DateTime> startTime = (this.htAlarmList[key] as AlarmCarInfo).StartTime;
					List<DateTime> endTime = (this.htAlarmList[key] as AlarmCarInfo).EndTime;
					if ((int)dataBySql.Select(string.Concat("carid = ", key)).Length <= 0 || !this.getIsInTime(startTime, endTime))
					{
						nums.Add(key);
					}
					else
					{
						foreach (int item in (this.htAlarmList[key] as AlarmCarInfo).pathid)
						{
							object[] objArray = new object[] { "pathid = ", item, " and carid = ", key };
							if ((int)dataBySql.Select(string.Concat(objArray)).Length > 0)
							{
								continue;
							}
							Hashtable hashtables1 = hashtables;
							Hashtable hashtables2 = hashtables1;
							object obj = key;
							object obj1 = obj;
							hashtables1[obj] = string.Concat(hashtables2[obj1], item.ToString(), ",");
						}
						if (hashtables[key] == null)
						{
							continue;
						}
						object obj2 = key;
						string str = hashtables[key].ToString();
						char[] chrArray = new char[] { ',' };
						hashtables[obj2] = str.Trim(chrArray);
					}
				}
				foreach (int num in nums)
				{
					if (!this.htAlarmList.Contains(num))
					{
						continue;
					}
					this.htAlarmList.Remove(num);
				}
				foreach (int key1 in hashtables.Keys)
				{
					string str1 = hashtables[key1].ToString();
					char[] chrArray1 = new char[] { ',' };
					string[] strArrays = str1.Split(chrArray1, StringSplitOptions.RemoveEmptyEntries);
					AlarmCarInfo alarmCarInfo = this.htAlarmList[key1] as AlarmCarInfo;
					if (alarmCarInfo == null)
					{
						continue;
					}
					string[] strArrays1 = strArrays;
					for (int i = 0; i < (int)strArrays1.Length; i++)
					{
						int num1 = Convert.ToInt32(strArrays1[i]);
						if (alarmCarInfo.pathid.Contains(num1))
						{
							alarmCarInfo.pathid.Remove(num1);
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmPathAlarm", "clearAlarmCar", string.Concat("清除内存数据,", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		private void DownToTerminal(string simnum, string alarmType, string alarmTime, string alarmInfo, string carid)
		{
			this.setParam(alarmInfo);
			this.txtMsg.CarId = carid;
			this.myDownData.icar_SendTxtMsg(CmdParam.ParamType.SimNum, simnum, "", CmdParam.CommMode.未知方式, this.txtMsg, 0, alarmInfo);
		}

		private DataTable getConfigInfo()
		{
			DataTable dataTable;
			try
			{
				DateTime svrTime = ReadDataFromDB.GetSvrTime();
				string str = " select a.*,b.simnum,c.alarmpathdot,c.pathname,d.* from GpsJtbCarPathAlarm_Platform a inner join giscar b on a.carid = b.carid left join GpsPathType c on a.pathid = c.pathid INNER join gpscarcurrentposinfo d WITH(NOLOCK) on b.simnum = d.telephone {2} where d.lastupdatetime between '{0}' and '{1}' ";
				string str1 = "";
				if (ReadDataFromXml.IsOnlyFillCheck)
				{
					str1 = "and d.TransportStatus = '3'";
				}
				DataTable dataBySql = SqlDataAccess.getDataBySql(string.Format(str, this.dtNow.ToString(), svrTime.ToString(), str1));
				this.dtNow = svrTime;
				dataTable = dataBySql;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmPathAlarm", "getConfigInfo", string.Concat("获取偏移路线报警信息错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				dataTable = null;
			}
			return dataTable;
		}

		private bool getIsInTime(List<DateTime> L1, List<DateTime> L2)
		{
			PlatformAlarmRegionAlarm platformAlarmRegionAlarm = new PlatformAlarmRegionAlarm();
			DateTime svrTime = ReadDataFromDB.GetSvrTime();
			for (int i = 0; i < L1.Count; i++)
			{
				if (platformAlarmRegionAlarm.IsInConfigTime(L1[i], L2[i], svrTime))
				{
					return true;
				}
			}
			return false;
		}

		private void initInputXML()
		{
			this.xDoc = new XmlDocument();
			XmlDeclaration xmlDeclaration = this.xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
			this.xDoc.AppendChild(xmlDeclaration);
			XmlElement xmlElement = this.xDoc.CreateElement("Cars");
			this.xDoc.AppendChild(xmlElement);
		}

		private void InsertAlarmInfo(DataRow dr, CmdParam.CarAlarmState carAlarmState, int AddMsgType)
		{
			try
			{
				int num = 0;
				string empty = string.Empty;
				int num1 = Convert.ToInt32(dr["carstatu"]);
				long num2 = (long)0;
				num2 = (dr["carstatuex"] == DBNull.Value || dr["carstatuex"].ToString().Equals("") ? 2251799813685248L : 2251799813685248L | Convert.ToInt64(dr["carstatuex"]));
				int num3 = 1154;
				int num4 = 65;
				string str = string.Empty;
				string str1 = dr["AddMsgTxt"].ToString();
				string str2 = null;
				bool flag = false;
				string str3 = null;
				int num5 = 0;
				try
				{
					SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@wrkid", (object)num), new SqlParameter("@orderid", dr["OrderId"]), new SqlParameter("@userid", empty), new SqlParameter("@telephone", dr["telephone"]), new SqlParameter("@msgType", (object)num4), new SqlParameter("@recetime", dr["ReceTime"]), new SqlParameter("@gpstime", dr["GpsTime"]), new SqlParameter("@starCondition", dr["StarCondition"]), new SqlParameter("@starNum", dr["StarNum"]), new SqlParameter("@carStatu", (object)num1), new SqlParameter("@carStatuEx", (object)num2), new SqlParameter("@carCondition", dr["CarCondition"]), new SqlParameter("@Longitude", dr["Longitude"]), new SqlParameter("@Latitude", dr["Latitude"]), new SqlParameter("@direct", dr["Direct"]), new SqlParameter("@speed", dr["Speed"]), new SqlParameter("@Reserved", (object)num3), new SqlParameter("@TransportStatus", dr["TransportStatus"]), new SqlParameter("@Accelerration", dr["Accelerration"]), new SqlParameter("@Altitude", dr["Altitude"]), new SqlParameter("@DistanceDiff", dr["DistanceDiff"]), new SqlParameter("@commflag", dr["CommFlag"]), new SqlParameter("@addType", (object)AddMsgType), new SqlParameter("@addTxt", str1), new SqlParameter("@DutyStr", str2), new SqlParameter("@isPic", (object)flag), new SqlParameter("@pic", str3), new SqlParameter("@alarmInfo", str), new SqlParameter("@cameraID", (object)num5) };
					string str4 = "GpsPicServer_Alarm_Insert";
					string str5 = "GpsPicServer_RealTime_Insert";
					if (AddMsgType != -99997)
					{
						int num6 = SqlDataAccess.insertBySp(str4, sqlParameter);
						if (num6 > 0)
						{
							LogMsg logMsg = new LogMsg("PlatformAlarmPathAlarm", "InsertAlarmInfo", "");
							string[] strArrays = new string[] { "车载电话为：", dr["telephone"].ToString(), "的平台检测", carAlarmState.ToString(), "报警报文已插入gpsrecebuffer" };
							logMsg.Msg = string.Concat(strArrays);
							this.logHelper.WriteLog(logMsg);
						}
						else
						{
							ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmPathAlarm", "InsertAlarmInfo", string.Concat("将平台检测", carAlarmState.ToString(), "报警报文插入gpsrecbuffer表错误，返回值!", num6.ToString()));
							this.logHelper.WriteError(errorMsg);
						}
					}
					int num7 = SqlDataAccess.insertBySp(str5, sqlParameter);
					if (num7 > 0)
					{
						string[] strArrays1 = new string[] { "车载电话为：", dr["telephone"].ToString(), "的平台检测", carAlarmState.ToString(), "报警报文已插入gpsrecerealtime" };
						LogMsg logMsg1 = new LogMsg("PlatformAlarmPathAlarm", "InsertAlarmInfo", string.Concat(strArrays1));
						string[] strArrays2 = new string[] { "车载电话为：", dr["telephone"].ToString(), "的平台检测", carAlarmState.ToString(), "报警报文已插入gpsrecerealtime" };
						logMsg1.Msg = string.Concat(strArrays2);
						this.logHelper.WriteLog(logMsg1);
					}
					else
					{
						ErrorMsg errorMsg1 = new ErrorMsg("PlatformAlarmPathAlarm", "InsertAlarmInfo", string.Concat("将平台检测", carAlarmState.ToString(), "报警报文插入gpsrecerealtime_buffer表发生错误，返回值!", num7.ToString()));
						this.logHelper.WriteError(errorMsg1);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					string[] strArrays3 = new string[] { "车载电话为：", dr["telephone"].ToString(), "的平台检测", carAlarmState.ToString(), "报警报文插入数据库发生错误! 信息：", exception.Message };
					ErrorMsg errorMsg2 = new ErrorMsg("PlatformAlarmPathAlarm", "InsertAlarmInfo", string.Concat(strArrays3));
					this.logHelper.WriteError(errorMsg2);
				}
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg3 = new ErrorMsg("PlatformAlarmPathAlarm", "InsertAlarmInfo", string.Concat("将平台检测", carAlarmState.ToString(), "报警报文插入gpsrecbuffer表 、gpsrecerealtime_buffer表发生错误!", exception2.Message));
				logHelper.WriteError(errorMsg3);
			}
		}

		private void setInputXML()
		{
			try
			{
				this.initInputXML();
				int num = 0;
				foreach (int key in this.htCarPathInfo.Keys)
				{
					XmlElement xmlElement = this.xDoc.CreateElement("Car");
					xmlElement.SetAttribute("value", key.ToString());
					string[] strArrays = this.htCarPosInfo[key].ToString().Split(new char[] { '*' });
					XmlElement xmlElement1 = this.xDoc.CreateElement("Lon");
					xmlElement1.SetAttribute("value", strArrays[0]);
					XmlElement xmlElement2 = this.xDoc.CreateElement("Lat");
					xmlElement2.SetAttribute("value", strArrays[1]);
					XmlElement xmlElement3 = this.xDoc.CreateElement("Paths");
					string[] strArrays1 = this.htCarPathInfo[key].ToString().Split(new char[] { ';' });
					for (int i = 0; i < (int)strArrays1.Length; i++)
					{
						string str = strArrays1[i];
						if (!string.IsNullOrEmpty(str))
						{
							string[] strArrays2 = str.Split(new char[] { ':' });
							XmlElement xmlElement4 = this.xDoc.CreateElement("Path");
							xmlElement4.SetAttribute("id", strArrays2[0]);
							string str1 = strArrays2[1].Replace("*", ",").Replace("/", ";");
							char[] chrArray = new char[] { ';' };
							xmlElement4.SetAttribute("value", str1.Trim(chrArray));
							xmlElement3.AppendChild(xmlElement4);
							num++;
						}
					}
					xmlElement.AppendChild(xmlElement1);
					xmlElement.AppendChild(xmlElement2);
					xmlElement.AppendChild(xmlElement3);
					XmlElement itemOf = (XmlElement)this.xDoc.GetElementsByTagName("Cars")[0];
					itemOf.AppendChild(xmlElement);
					this.IsJudge = true;
					if (num < 100)
					{
						continue;
					}
					this.CheckAndAnalysis();
					this.initInputXML();
					this.IsJudge = false;
					num = 0;
				}
				this.CheckAndAnalysis();
				this.IsJudge = false;
				num = 0;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmPathAlarm", "setInputXML", string.Concat("设置输入XML错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
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
				this.tCheckPathAlarm = new Timer((double)this.iCheckPathAlarm);
				this.tCheckPathAlarm.Elapsed += new ElapsedEventHandler(this.tCheckPathAlarm_Elapsed);
				this.tCheckPathAlarm.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmPathAlarm", "start", string.Concat("开启检测偏移路线报警错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.tCheckPathAlarm.Stop();
		}

		private void tCheckPathAlarm_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tCheckPathAlarm.Enabled = false;
			try
			{
				try
				{
					PlatformAlarmRegionAlarm platformAlarmRegionAlarm = new PlatformAlarmRegionAlarm();
					this.clearAlarmCar();
					this.dtConfigInfo = this.getConfigInfo();
					if (this.dtConfigInfo != null && this.dtConfigInfo.Rows.Count > 0)
					{
						foreach (DataRow row in this.dtConfigInfo.Rows)
						{
							DateTime dateTime = Convert.ToDateTime(row["BeginTime"]);
							DateTime dateTime1 = Convert.ToDateTime(row["EndTime"]);
							DateTime dateTime2 = Convert.ToDateTime(row["gpsTime"]);
							if (!platformAlarmRegionAlarm.IsInConfigTime(dateTime, dateTime1, this.dtNow) || !platformAlarmRegionAlarm.IsInConfigTime(dateTime, dateTime1, dateTime2) || dateTime2.Date != this.dtNow.Date)
							{
								continue;
							}
							int num = Convert.ToInt32(row["carid"]);
							int num1 = Convert.ToInt32(row["PathID"]);
							if (this.htCarPathInfo[num] == null)
							{
								this.htCarPathInfo[num] = "";
							}
							try
							{
								if (this.htAlarmList[num] != null)
								{
									this.htAlarmList[num] = (this.htAlarmList[num] as AlarmCarInfo).updateAlarmCar(num, num1, dateTime, dateTime1);
								}
								else
								{
									this.htAlarmList[num] = AlarmCarInfo.getAlarmCar(num, num1, dateTime, dateTime1);
								}
							}
							catch (Exception exception1)
							{
								Exception exception = exception1;
								ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmPathAlarm", "tCheckPathAlarm_Elapsed", string.Concat("保存报警车辆信息,", exception.Message));
								this.logHelper.WriteError(errorMsg);
							}
							if (row["Longitude"] == DBNull.Value || row["Latitude"] == DBNull.Value || double.Parse(row["Longitude"].ToString()) < 0.001 || double.Parse(row["Latitude"].ToString()) < 0.001)
							{
								if (this.htCarPathInfo.Contains(num))
								{
									this.htCarPathInfo.Remove(num);
								}
								LogMsg logMsg = new LogMsg("PlatformAlarmPathAlarm", "tCheckPathAlarm_Elapsed", "");
								object[] str = new object[] { "carid:", num, ",pathId:", num1.ToString(), ",simnum:", row["simnum"].ToString(), ",轨迹不存在或为0" };
								logMsg.Msg = string.Concat(str);
								this.logHelper.WriteLog(logMsg);
							}
							else if (row["alarmpathdot"] == DBNull.Value || string.IsNullOrEmpty(row["alarmpathdot"].ToString()))
							{
								if (this.htCarPathInfo.Contains(num))
								{
									this.htCarPathInfo.Remove(num);
								}
								LogMsg logMsg1 = new LogMsg("PlatformAlarmPathAlarm", "tCheckPathAlarm_Elapsed", "");
								object[] objArray = new object[] { "carid:", num, ",pathId:", num1.ToString(), ",simnum:", row["simnum"].ToString(), ",路线不存在" };
								logMsg1.Msg = string.Concat(objArray);
								this.logHelper.WriteLog(logMsg1);
							}
							else
							{
								Hashtable hashtables = this.htCarPathInfo;
								Hashtable hashtables1 = hashtables;
								object obj = num;
								object item = hashtables1[obj];
								object[] str1 = new object[] { item, num1, ":", row["alarmpathdot"].ToString(), ";" };
								hashtables[obj] = string.Concat(str1);
								this.htCarPosInfo[num] = string.Concat(row["Longitude"].ToString(), "*", row["Latitude"].ToString());
							}
						}
						this.setInputXML();
						this.CheckAndAnalysis();
					}
				}
				catch (Exception exception3)
				{
					Exception exception2 = exception3;
					ErrorMsg errorMsg1 = new ErrorMsg("PlatformAlarmPathAlarm", "tCheckPathAlarm_Elapsed", string.Concat("检测偏移路线报警错误,", exception2.Message));
					this.logHelper.WriteError(errorMsg1);
				}
			}
			finally
			{
				this.htCarPathInfo.Clear();
				this.htCarPosInfo.Clear();
				this.xDoc = null;
				this.tCheckPathAlarm.Enabled = true;
			}
		}
	}
}