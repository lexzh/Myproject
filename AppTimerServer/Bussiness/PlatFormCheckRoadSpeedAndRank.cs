using GisServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Timers;
using System.Xml;
using ParamLibrary.CmdParamInfo;
using Library;
using ParamLibrary.Application;

namespace Bussiness
{
	public class PlatFormCheckRoadSpeedAndRank : ProcessBase
	{
		private DownData myDownData = new DownData(0);

		private TxtMsg txtMsg = new TxtMsg();

		private Timer tCheckRoadSpeedAndRan;

		private DateTime dtNow = ReadDataFromDB.GetSvrTime();

		private int iCheckRoadSpeedAndRank = 60000 * ReadDataFromXml.numRoadSpeedAndRankInterval;

		private XmlDocument xDoc;

		private Hashtable htAlarmList = new Hashtable();

		private string sSendMsg = ReadDataFromXml.SendMessage;

		private string sPw = "";

		private List<string> AlarmListFirst;

		private List<string> AlarmListMid;

		private List<string> AlarmListEnd;

		private Hashtable AlarmMidInfo = new Hashtable();

		public PlatFormCheckRoadSpeedAndRank()
		{
		}

		private void AnalysisOutputXML(string outXML)
		{
			try
			{
				this.AlarmListFirst = new List<string>();
				this.AlarmListMid = new List<string>();
				this.AlarmListEnd = new List<string>();
				if (!string.IsNullOrEmpty(outXML))
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(outXML);
					foreach (XmlElement childNode in xmlDocument.SelectSingleNode("//PosMsgs").ChildNodes)
					{
						string attribute = childNode.GetAttribute("id");
						XmlElement itemOf = (XmlElement)childNode.GetElementsByTagName("Status")[0];
						XmlElement xmlElement = (XmlElement)childNode.GetElementsByTagName("RoadRank")[0];
						XmlElement itemOf1 = (XmlElement)childNode.GetElementsByTagName("LimitSpeed")[0];
						string str = xmlElement.GetAttribute("value");
						string attribute1 = itemOf1.GetAttribute("value");
						if (!itemOf.GetAttribute("value").Equals("4096"))
						{
							if (this.htAlarmList[attribute] == null || !bool.Parse(this.htAlarmList[attribute].ToString()))
							{
								continue;
							}
							this.htAlarmList[attribute] = false;
							List<string> alarmListEnd = this.AlarmListEnd;
							string[] strArrays = new string[] { attribute, "-", str, "-", attribute1 };
							alarmListEnd.Add(string.Concat(strArrays));
							this.AlarmMidInfo.Remove(attribute);
						}
						else if (this.htAlarmList[attribute] == null || !bool.Parse(this.htAlarmList[attribute].ToString()))
						{
							List<string> alarmListFirst = this.AlarmListFirst;
							string[] strArrays1 = new string[] { attribute, "-", str, "-", attribute1 };
							alarmListFirst.Add(string.Concat(strArrays1));
							this.htAlarmList[attribute] = true;
							this.AlarmMidInfo.Add(attribute, attribute1);
						}
						else if (this.AlarmMidInfo[attribute] == null || !(this.AlarmMidInfo[attribute] as string != attribute1))
						{
							List<string> alarmListMid = this.AlarmListMid;
							string[] strArrays2 = new string[] { attribute, "-", str, "-", attribute1 };
							alarmListMid.Add(string.Concat(strArrays2));
						}
						else
						{
							if (this.htAlarmList[attribute] == null || !bool.Parse(this.htAlarmList[attribute].ToString()))
							{
								continue;
							}
							List<string> strs = this.AlarmListFirst;
							string[] strArrays3 = new string[] { attribute, "-", str, "-", attribute1 };
							strs.Add(string.Concat(strArrays3));
							this.htAlarmList[attribute] = true;
							this.AlarmMidInfo[attribute] = attribute1;
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatFormCheckRoadSpeedAndRank", "AnalysisOutputXML", string.Concat("判断车自定义分段超速报警和道路等级,", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		private void clearAlarmCar()
		{
			try
			{
				DataTable dataBySql = SqlDataAccess.getDataBySql(" select * from GpsCarCheckRoadSpeedAndRank ");
				if (dataBySql == null || dataBySql.Rows.Count <= 0)
				{
					this.htAlarmList.Clear();
				}
				List<string> strs = new List<string>();
				foreach (string key in this.htAlarmList.Keys)
				{
					if ((int)dataBySql.Select(string.Concat("carid = ", key)).Length > 0)
					{
						continue;
					}
					strs.Add(key);
				}
				foreach (string str in strs)
				{
					if (!this.htAlarmList.Contains(str))
					{
						continue;
					}
					this.htAlarmList.Remove(str);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatFormCheckRoadSpeedAndRank", "clearAlarmCar", string.Concat("清除内存数据,", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		private string EnCodeString(string tmp1, string tmp2)
		{
			byte[] bytes;
			string str = "M13";
			string str1 = string.Concat("{0},限速值:", tmp2);
			if (!string.IsNullOrEmpty(tmp1))
			{
				string str2 = "";
				string str3 = tmp1;
				string str4 = str3;
				str2 = (str3 == null || !(str4 == "1") ? "非高速道路" : "高速道路");
				bytes = Encoding.Default.GetBytes(string.Format(str1, str2));
			}
			else
			{
				bytes = Encoding.Default.GetBytes(string.Format(str1, "非高速道路"));
			}
			return string.Concat(str, BitConverter.ToString(bytes).Replace("-", ""));
		}

		private void execSendMsg(DataRow dr)
		{
			try
			{
				DateTime dateTime = DateTime.Parse(dr["gpsTime"].ToString());
				string str = dateTime.ToString("M月d日H时m分s秒");
				this.sSendMsg = this.sSendMsg.Replace("(A)", str);
				this.txtMsg.MsgType = CmdParam.MsgType.固定信息点播;
				this.txtMsg.strMsg = this.sSendMsg;
				this.myDownData.icar_SendCmdXML(CmdParam.ParamType.SimNum, dr["telephone"].ToString(), dr["carid"].ToString(), this.sPw, dr["ProtocolName"].ToString(), CmdParam.CommMode.未知方式, this.txtMsg, "分道路等级超速报警通知");
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatFormCheckRoadSpeedAndRank", "execSendMsg", string.Concat("下发播报信息,", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		private DataTable getData()
		{
			DataTable dataTable;
			try
			{
				DateTime svrTime = ReadDataFromDB.GetSvrTime();
				string str = "select a.*, c.*,e.[ProtocolName] from GpsCarCheckRoadSpeedAndRank a INNER JOIN GisCar b on a.Carid = b.CarId INNER JOIN GpsCarCurrentPosInfo c on c.telephone = b.simnum inner join GpsTerminalType d on  b.[TerminalTypeID]=d.[TerminalTypeID] inner join [GpsProtocol] e on e.[ProtocolCode]=d.[ProtocolCode] where c.LastUpdateTime between '{0}' and '{1}'";
				DataTable dataBySql = SqlDataAccess.getDataBySql(string.Format(str, this.dtNow, svrTime));
				this.dtNow = svrTime;
				dataTable = dataBySql;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatFormCheckRoadSpeedAndRank", "getData", string.Concat("判断车自定义分段超速报警和道路等级,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				dataTable = null;
			}
			return dataTable;
		}

		private void initInputXML(int Count)
		{
			this.xDoc = new XmlDocument();
			XmlDeclaration xmlDeclaration = this.xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
			this.xDoc.AppendChild(xmlDeclaration);
			XmlElement xmlElement = this.xDoc.CreateElement("PosMsgs");
			xmlElement.SetAttribute("nums", Count.ToString());
			this.xDoc.AppendChild(xmlElement);
		}

		private void InsertAlarmInfo(DataRow dr, string carAlarmState, int AddMsgType, string AddMsgTxt)
		{
			try
			{
				int num = 0;
				string empty = string.Empty;
				int num1 = Convert.ToInt32(dr["carstatu"]);
				long num2 = Convert.ToInt64(dr["carStatuex"]) | 9007199254740992L;
				int num3 = 1154;
				int num4 = 65;
				string str = string.Empty;
				AddMsgTxt = string.Concat(AddMsgTxt, "/", dr["AddMsgTxt"].ToString());
				string str1 = null;
				bool flag = false;
				string str2 = null;
				int num5 = 0;
				try
				{
					SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@wrkid", (object)num), new SqlParameter("@orderid", dr["OrderId"]), new SqlParameter("@userid", empty), new SqlParameter("@telephone", dr["telephone"]), new SqlParameter("@msgType", (object)num4), new SqlParameter("@recetime", dr["ReceTime"]), new SqlParameter("@gpstime", dr["GpsTime"]), new SqlParameter("@starCondition", dr["StarCondition"]), new SqlParameter("@starNum", dr["StarNum"]), new SqlParameter("@carStatu", (object)num1), new SqlParameter("@carStatuEx", (object)num2), new SqlParameter("@carCondition", dr["CarCondition"]), new SqlParameter("@Longitude", dr["Longitude"]), new SqlParameter("@Latitude", dr["Latitude"]), new SqlParameter("@direct", dr["Direct"]), new SqlParameter("@speed", dr["Speed"]), new SqlParameter("@Reserved", (object)num3), new SqlParameter("@TransportStatus", dr["TransportStatus"]), new SqlParameter("@Accelerration", dr["Accelerration"]), new SqlParameter("@Altitude", dr["Altitude"]), new SqlParameter("@DistanceDiff", dr["DistanceDiff"]), new SqlParameter("@commflag", dr["CommFlag"]), new SqlParameter("@addType", (object)AddMsgType), new SqlParameter("@addTxt", AddMsgTxt), new SqlParameter("@DutyStr", str1), new SqlParameter("@isPic", (object)flag), new SqlParameter("@pic", str2), new SqlParameter("@alarmInfo", str), new SqlParameter("@cameraID", (object)num5) };
					string str3 = "GpsPicServer_Alarm_Insert";
					string str4 = "GpsPicServer_RealTime_Insert";
					if (AddMsgType != -99997)
					{
						int num6 = SqlDataAccess.insertBySp(str3, sqlParameter);
						if (num6 > 0)
						{
							LogMsg logMsg = new LogMsg("PlatFormCheckRoadSpeedAndRank", "InsertAlarmInfo", "");
							string[] strArrays = new string[] { "车载电话为：", dr["telephone"].ToString(), "的平台检测", carAlarmState.ToString(), "报警报文已插入gpsrecebuffer" };
							logMsg.Msg = string.Concat(strArrays);
							this.logHelper.WriteLog(logMsg);
						}
						else
						{
							ErrorMsg errorMsg = new ErrorMsg("PlatFormCheckRoadSpeedAndRank", "InsertAlarmInfo", string.Concat("将平台检测", carAlarmState.ToString(), "报警报文插入gpsrecbuffer表错误，返回值!", num6.ToString()));
							this.logHelper.WriteError(errorMsg);
						}
					}
					int num7 = SqlDataAccess.insertBySp(str4, sqlParameter);
					if (num7 > 0)
					{
						string[] strArrays1 = new string[] { "车载电话为：", dr["telephone"].ToString(), "的平台检测", carAlarmState.ToString(), "报警报文已插入gpsrecerealtime" };
						LogMsg logMsg1 = new LogMsg("PlatFormCheckRoadSpeedAndRank", "InsertAlarmInfo", string.Concat(strArrays1));
						string[] strArrays2 = new string[] { "车载电话为：", dr["telephone"].ToString(), "的平台检测", carAlarmState.ToString(), "报警报文已插入gpsrecerealtime" };
						logMsg1.Msg = string.Concat(strArrays2);
						this.logHelper.WriteLog(logMsg1);
					}
					else
					{
						ErrorMsg errorMsg1 = new ErrorMsg("PlatFormCheckRoadSpeedAndRank", "InsertAlarmInfo", string.Concat("将平台检测", carAlarmState.ToString(), "报警报文插入gpsrecerealtime_buffer表发生错误，返回值!", num7.ToString()));
						this.logHelper.WriteError(errorMsg1);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					string[] strArrays3 = new string[] { "车载电话为：", dr["telephone"].ToString(), "的平台检测", carAlarmState.ToString(), "报警报文插入数据库发生错误! 信息：", exception.Message };
					ErrorMsg errorMsg2 = new ErrorMsg("PlatFormCheckRoadSpeedAndRank", "InsertAlarmInfo", string.Concat(strArrays3));
					this.logHelper.WriteError(errorMsg2);
				}
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg3 = new ErrorMsg("PlatFormCheckRoadSpeedAndRank", "InsertAlarmInfo", string.Concat("将平台检测", carAlarmState.ToString(), "报警报文插入gpsrecbuffer表 、gpsrecerealtime_buffer表发生错误!", exception2.Message));
				logHelper.WriteError(errorMsg3);
			}
		}

		private void setInputXML(int carId, string Lon, string Lat, string Speed)
		{
			try
			{
				XmlElement xmlElement = this.xDoc.CreateElement("PosMsg");
				xmlElement.SetAttribute("id", carId.ToString());
				XmlElement xmlElement1 = this.xDoc.CreateElement("Lon");
				xmlElement1.SetAttribute("value", Lon);
				XmlElement xmlElement2 = this.xDoc.CreateElement("Lat");
				xmlElement2.SetAttribute("value", Lat);
				XmlElement xmlElement3 = this.xDoc.CreateElement("Speed");
				xmlElement3.SetAttribute("value", Speed);
				xmlElement.AppendChild(xmlElement1);
				xmlElement.AppendChild(xmlElement2);
				xmlElement.AppendChild(xmlElement3);
				XmlElement itemOf = (XmlElement)this.xDoc.GetElementsByTagName("PosMsgs")[0];
				itemOf.AppendChild(xmlElement);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatFormCheckRoadSpeedAndRank", "setInputXML", string.Concat("判断车自定义分段超速报警和道路等级,", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void start()
		{
			try
			{
				this.tCheckRoadSpeedAndRan = new Timer(1000);
				this.tCheckRoadSpeedAndRan.Elapsed += new ElapsedEventHandler(this.tCheckRoadSpeedAndRan_Elapsed);
				this.tCheckRoadSpeedAndRan.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatFormCheckRoadSpeedAndRank", "start", string.Concat("判断车自定义分段超速报警和道路等级,", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.tCheckRoadSpeedAndRan.Stop();
		}

		private void tCheckRoadSpeedAndRan_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tCheckRoadSpeedAndRan.Enabled = false;
			this.tCheckRoadSpeedAndRan.Interval = (double)this.iCheckRoadSpeedAndRank;
			try
			{
				this.clearAlarmCar();
				DataTable data = this.getData();
				bool flag = false;
				if (data != null && data.Rows.Count > 0)
				{
					this.initInputXML(data.Rows.Count);
					foreach (DataRow row in data.Rows)
					{
						int num = Convert.ToInt32(row["carid"]);
						string str = row["Longitude"].ToString();
						string str1 = row["Latitude"].ToString();
						string str2 = row["Speed"].ToString();
						this.setInputXML(num, str, str1, str2);
						flag = true;
					}
					if (flag)
					{
						LogMsg logMsg = new LogMsg("PlatFormCheckRoadSpeedAndRank", "tCheckRoadSpeedAndRan_Elapsed", "")
						{
							Msg = string.Concat("分道路等级超速报警传 入 的xml：\r\n", this.xDoc.OuterXml)
						};
						this.logHelper.WriteLog(logMsg);
						string empty = string.Empty;
						empty = (!ReadDataFromXml.RoadSpeedAndRankGisType.Equals("0") ? ReadDataFromOtherGis.checkRoadSpeedAndRank(this.xDoc.OuterXml) : ReadDataFromGis.checkRoadSpeedAndRank(this.xDoc.OuterXml));
						logMsg.Msg = string.Concat("分道路等级超速报警传 回 的xml：\r\n", empty);
						this.logHelper.WriteLog(logMsg);
						if (!string.IsNullOrEmpty(empty))
						{
							this.AnalysisOutputXML(empty);
							if (this.AlarmListFirst != null && this.AlarmListFirst.Count > 0)
							{
								foreach (string alarmListFirst in this.AlarmListFirst)
								{
									string[] strArrays = alarmListFirst.Split(new char[] { '-' });
									DataRow[] dataRowArray = data.Select(string.Concat("carid = ", strArrays[0]));
									this.InsertAlarmInfo(dataRowArray[0], "分道路等级超速", -99999, this.EnCodeString(strArrays[1], strArrays[2]));
									if (!ReadDataFromXml.IsSend)
									{
										continue;
									}
									this.execSendMsg(dataRowArray[0]);
								}
								this.AlarmListFirst.Clear();
								this.AlarmListFirst = null;
							}
							if (this.AlarmListMid != null && this.AlarmListMid.Count > 0)
							{
								foreach (string alarmListMid in this.AlarmListMid)
								{
									string[] strArrays1 = alarmListMid.Split(new char[] { '-' });
									DataRow[] dataRowArray1 = data.Select(string.Concat("carid = ", strArrays1[0]));
									this.InsertAlarmInfo(dataRowArray1[0], "分道路等级超速", -99998, this.EnCodeString(strArrays1[1], strArrays1[2]));
								}
								this.AlarmListMid.Clear();
								this.AlarmListMid = null;
							}
							if (this.AlarmListEnd != null && this.AlarmListEnd.Count > 0)
							{
								foreach (string alarmListEnd in this.AlarmListEnd)
								{
									string[] strArrays2 = alarmListEnd.Split(new char[] { '-' });
									DataRow[] dataRowArray2 = data.Select(string.Concat("carid = ", strArrays2[0]));
									this.InsertAlarmInfo(dataRowArray2[0], "分道路等级超速", -99997, this.EnCodeString(strArrays2[1], strArrays2[2]));
								}
								this.AlarmListEnd.Clear();
								this.AlarmListEnd = null;
							}
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatFormCheckRoadSpeedAndRank", "tCheckRoadSpeedAndRan_Elapsed", string.Concat("判断车自定义分段超速报警和道路等级,", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
			this.tCheckRoadSpeedAndRan.Enabled = true;
		}
	}
}