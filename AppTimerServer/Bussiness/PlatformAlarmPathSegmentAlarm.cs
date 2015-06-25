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
	public class PlatformAlarmPathSegmentAlarm : ProcessBase
	{
		private DownData myDownData = new DownData(0);

		private TxtMsg txtMsg = new TxtMsg();

		private Timer tCheckPathSegmentAlarm;

		private DateTime dtNow = ReadDataFromDB.GetSvrTime();

		private int iCheckPathSegmentAlarm = 60000 * ReadDataFromXml.PathAlarmInterval;

		private XmlDocument xDoc;

		private Hashtable htAlarmList = new Hashtable();

		private List<string> AlarmListFirst;

		private List<string> AlarmListMid;

		private List<string> AlarmListEnd;

		public PlatformAlarmPathSegmentAlarm()
		{
		}

		private void AnalysisOutputXML(string outXML)
		{
			try
			{
				this.AlarmListFirst = new List<string>();
				this.AlarmListMid = new List<string>();
				this.AlarmListEnd = new List<string>();
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(outXML);
				foreach (XmlElement childNode in xmlDocument.SelectSingleNode("//Cars").ChildNodes)
				{
					if (childNode.Attributes["onWhichRoad"].Value.ToString().Equals(""))
					{
						continue;
					}
					int num = Convert.ToInt32(childNode.Attributes["onWhichRoad"].Value);
					if (childNode.Attributes["isOverSpeed"].Value.Equals("0"))
					{
						if (!childNode.Attributes["isOverSpeed"].Value.Equals("0") || Convert.ToInt32(this.htAlarmList[string.Concat(childNode.Attributes["id"].Value, "-", num)]) != 1)
						{
							continue;
						}
						this.AlarmListEnd.Add(childNode.Attributes["id"].Value);
						this.htAlarmList[string.Concat(childNode.Attributes["id"].Value, "-", num)] = 0;
					}
					else
					{
						if (this.htAlarmList[string.Concat(childNode.Attributes["id"].Value, "-", num)] == null || Convert.ToInt32(this.htAlarmList[string.Concat(childNode.Attributes["id"].Value, "-", num)]) != 1)
						{
							this.AlarmListFirst.Add(childNode.Attributes["id"].Value);
						}
						else if (ReadDataFromXml.IsContinuousAlarm)
						{
							this.AlarmListMid.Add(childNode.Attributes["id"].Value);
						}
						this.htAlarmList[string.Concat(childNode.Attributes["id"].Value, "-", num)] = 1;
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmPathSegmentAlarm", "AnalysisOutputXML", string.Concat("解析返回的XML错误,", exception.Message));
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
				string str = " select b.CarId, b.simnum,c.* from giscar b INNER join gpscarcurrentposinfo c WITH(NOLOCK) on b.simnum = c.telephone {2} where c.lastupdatetime between '{0}' and '{1}' ";
				string str1 = "";
				if (ReadDataFromXml.IsOnlyFillCheck)
				{
					str1 = "and c.TransportStatus = '3'";
				}
				DataTable dataBySql = SqlDataAccess.getDataBySql(string.Format(str, this.dtNow.ToString(), svrTime.ToString(), str1));
				this.dtNow = svrTime;
				dataTable = dataBySql;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmPathSegmentAlarm", "getConfigInfo", string.Concat("获取分路段超速报警配置信息错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				dataTable = null;
			}
			return dataTable;
		}

		private XmlElement getPaths(int CarId)
		{
			XmlElement xmlElement;
			try
			{
				string str = " select * from GpsJtbCarPathSegmentAlarm_Platform a INNER JOIN GpsPathSegment b on a.PathSegmentID = b.PathSegmentID where a.CarId = '{0}' ";
				DataTable dataBySql = SqlDataAccess.getDataBySql(string.Format(str, CarId));
				if (dataBySql == null || dataBySql.Rows.Count <= 0)
				{
					xmlElement = null;
				}
				else
				{
					XmlElement xmlElement1 = this.xDoc.CreateElement("Paths");
					foreach (DataRow row in dataBySql.Rows)
					{
						XmlElement xmlElement2 = this.xDoc.CreateElement("Path");
						xmlElement2.SetAttribute("id", row["PathSegmentID"].ToString());
						string str1 = row["alarmSegmentDot"].ToString().Replace("*", ",").Replace("/", ";");
						char[] chrArray = new char[] { ';' };
						xmlElement2.SetAttribute("value", str1.Trim(chrArray));
						xmlElement2.SetAttribute("speed", row["Speed"].ToString());
						xmlElement1.AppendChild(xmlElement2);
					}
					xmlElement = xmlElement1;
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				object[] carId = new object[] { "获取路线信息错误,Carid：", CarId, ",", exception.Message };
				ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmPathSegmentAlarm", "getPaths", string.Concat(carId));
				this.logHelper.WriteError(errorMsg);
				xmlElement = null;
			}
			return xmlElement;
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
				long num2 = Convert.ToInt64(dr["carstatuex"]);
				int num3 = 1154;
				int num4 = 65;
				string str = string.Empty;
				string str1 = dr["AddMsgTxt"].ToString();
				str1 = (!string.IsNullOrEmpty(str1) ? string.Concat(str1, "/MEE0000000000000001") : "MEE0000000000000001");
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
							LogMsg logMsg = new LogMsg("PlatformAlarmPathSegmentAlarm", "InsertAlarmInfo", "");
							string[] strArrays = new string[] { "车载电话为：", dr["telephone"].ToString(), "的平台检测", carAlarmState.ToString(), "报警报文已插入gpsrecebuffer" };
							logMsg.Msg = string.Concat(strArrays);
							this.logHelper.WriteLog(logMsg);
						}
						else
						{
							ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmPathSegmentAlarm", "InsertAlarmInfo", string.Concat("将平台检测", carAlarmState.ToString(), "报警报文插入gpsrecbuffer表错误，返回值!", num6.ToString()));
							this.logHelper.WriteError(errorMsg);
						}
					}
					int num7 = SqlDataAccess.insertBySp(str5, sqlParameter);
					if (num7 > 0)
					{
						string[] strArrays1 = new string[] { "车载电话为：", dr["telephone"].ToString(), "的平台检测", carAlarmState.ToString(), "报警报文已插入gpsrecerealtime" };
						LogMsg logMsg1 = new LogMsg("PlatformAlarmPathSegmentAlarm", "InsertAlarmInfo", string.Concat(strArrays1));
						string[] strArrays2 = new string[] { "车载电话为：", dr["telephone"].ToString(), "的平台检测", carAlarmState.ToString(), "报警报文已插入gpsrecerealtime" };
						logMsg1.Msg = string.Concat(strArrays2);
						this.logHelper.WriteLog(logMsg1);
					}
					else
					{
						ErrorMsg errorMsg1 = new ErrorMsg("PlatformAlarmPathSegmentAlarm", "InsertAlarmInfo", string.Concat("将平台检测", carAlarmState.ToString(), "报警报文插入gpsrecerealtime_buffer表发生错误，返回值!", num7.ToString()));
						this.logHelper.WriteError(errorMsg1);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					string[] strArrays3 = new string[] { "车载电话为：", dr["telephone"].ToString(), "的平台检测", carAlarmState.ToString(), "报警报文插入数据库发生错误! 信息：", exception.Message };
					ErrorMsg errorMsg2 = new ErrorMsg("PlatformAlarmPathSegmentAlarm", "InsertAlarmInfo", string.Concat(strArrays3));
					this.logHelper.WriteError(errorMsg2);
				}
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg3 = new ErrorMsg("PlatformAlarmPathSegmentAlarm", "InsertAlarmInfo", string.Concat("将平台检测", carAlarmState.ToString(), "报警报文插入gpsrecbuffer表 、gpsrecerealtime_buffer表发生错误!", exception2.Message));
				logHelper.WriteError(errorMsg3);
			}
		}

		private void ReportAlarmInfo(string simnum, string alarmType, string alarmTime, string alarmInfo)
		{
		}

		private void setInputXML(int carid, string Lon, string Lat, string Speed, XmlElement xPaths)
		{
			try
			{
				XmlElement xmlElement = this.xDoc.CreateElement("Car");
				xmlElement.SetAttribute("id", carid.ToString());
				xmlElement.SetAttribute("lon", Lon);
				xmlElement.SetAttribute("lat", Lat);
				xmlElement.SetAttribute("speed", Speed);
				xmlElement.AppendChild(xPaths);
				XmlElement itemOf = (XmlElement)this.xDoc.GetElementsByTagName("Cars")[0];
				itemOf.AppendChild(xmlElement);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmPathSegmentAlarm", "setInputXML", string.Concat("设置输入XML错误,", exception.Message));
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
				this.tCheckPathSegmentAlarm = new Timer((double)this.iCheckPathSegmentAlarm);
				this.tCheckPathSegmentAlarm.Elapsed += new ElapsedEventHandler(this.tCheckPathSegmentAlarm_Elapsed);
				this.tCheckPathSegmentAlarm.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmPathSegmentAlarm", "start", string.Concat("开启检测分路段超速报警错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.tCheckPathSegmentAlarm.Stop();
		}

		private void tCheckPathSegmentAlarm_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tCheckPathSegmentAlarm.Enabled = false;
			try
			{
				try
				{
					DataTable configInfo = this.getConfigInfo();
					bool flag = false;
					if (configInfo != null && configInfo.Rows.Count > 0)
					{
						this.initInputXML();
						foreach (DataRow row in configInfo.Rows)
						{
							int num = Convert.ToInt32(row["carid"]);
							if (row["Longitude"] == DBNull.Value || row["Latitude"] == DBNull.Value || double.Parse(row["Longitude"].ToString()) < 0.001 || double.Parse(row["Latitude"].ToString()) < 0.001)
							{
								LogMsg logMsg = new LogMsg("PlatformAlarmPathSegmentAlarm", "tCheckPathSegmentAlarm_Elapsed", "");
								object[] str = new object[] { "carid:", num, ",simnum:", row["simnum"].ToString(), ",轨迹不存在或为0" };
								logMsg.Msg = string.Concat(str);
								this.logHelper.WriteLog(logMsg);
							}
							else
							{
								string str1 = row["Longitude"].ToString();
								string str2 = row["Latitude"].ToString();
								string str3 = row["Speed"].ToString();
								XmlElement paths = this.getPaths(num);
								if (paths == null)
								{
									continue;
								}
								flag = true;
								this.setInputXML(num, str1, str2, str3, paths);
							}
						}
						if (flag)
						{
							LogMsg logMsg1 = new LogMsg("PlatformAlarmPathSegmentAlarm", "tCheckPathSegmentAlarm_Elapsed", "")
							{
								Msg = string.Concat("分路段超速报警传 入 的xml：\r\n", this.xDoc.OuterXml)
							};
							this.logHelper.WriteLog(logMsg1);
							string str4 = ReadDataFromGis.ChackRoadSegAlarmCustom(this.xDoc.OuterXml);
							logMsg1.Msg = string.Concat("分路段超速报警传 回 的xml：\r\n", str4);
							this.logHelper.WriteLog(logMsg1);
							if (!string.IsNullOrEmpty(str4))
							{
								this.AnalysisOutputXML(str4);
								if (this.AlarmListFirst != null && this.AlarmListFirst.Count > 0)
								{
									foreach (string alarmListFirst in this.AlarmListFirst)
									{
										DataRow[] dataRowArray = configInfo.Select(string.Concat("carid = ", alarmListFirst));
										this.InsertAlarmInfo(dataRowArray[0], CmdParam.CarAlarmState.超速, -99999);
										if (!ReadDataFromXml.IschkDownMsg)
										{
											continue;
										}
										this.DownToTerminal(dataRowArray[0]["simnum"].ToString(), "0001", dataRowArray[0]["gpsTime"].ToString(), "您的车辆发生超速报警。", alarmListFirst);
									}
									this.AlarmListFirst.Clear();
									this.AlarmListFirst = null;
								}
								if (this.AlarmListMid != null && this.AlarmListMid.Count > 0)
								{
									foreach (string alarmListMid in this.AlarmListMid)
									{
										DataRow[] dataRowArray1 = configInfo.Select(string.Concat("carid = ", alarmListMid));
										this.InsertAlarmInfo(dataRowArray1[0], CmdParam.CarAlarmState.超速, -99998);
										if (!ReadDataFromXml.IschkDownMsg)
										{
											continue;
										}
										this.DownToTerminal(dataRowArray1[0]["simnum"].ToString(), "0001", dataRowArray1[0]["gpsTime"].ToString(), "您的车辆发生超速报警。", alarmListMid);
									}
									this.AlarmListMid.Clear();
									this.AlarmListMid = null;
								}
								if (this.AlarmListEnd != null && this.AlarmListEnd.Count > 0)
								{
									foreach (string alarmListEnd in this.AlarmListEnd)
									{
										DataRow[] dataRowArray2 = configInfo.Select(string.Concat("carid = ", alarmListEnd));
										this.InsertAlarmInfo(dataRowArray2[0], CmdParam.CarAlarmState.超速, -99997);
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
					ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmPathSegmentAlarm", "tCheckPathSegmentAlarm_Elapsed", string.Concat("检测分路段超速报警错误,", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.xDoc = null;
				this.tCheckPathSegmentAlarm.Enabled = true;
			}
		}
	}
}