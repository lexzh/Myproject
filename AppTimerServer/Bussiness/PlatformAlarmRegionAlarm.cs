using GisServices;
using System;
using System.Collections;
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
	public class PlatformAlarmRegionAlarm : ProcessBase
	{
		private const double EARTH_RADIUS = 6378.137;

		private DownData myDownData = new DownData(0);

		private TxtMsg txtMsg = new TxtMsg();

		private Timer tCheckRegionAlarm;

		private Hashtable htInfo = new Hashtable();

		private DateTime dtNow = ReadDataFromDB.GetSvrTime();

		private int iCheckRegionAlarm = 60000 * ReadDataFromXml.PathAlarmInterval;

		public PlatformAlarmRegionAlarm()
		{
		}

		private void DownToTerminal(string simnum, string alarmType, string alarmTime, string alarmInfo)
		{
			this.setParam(alarmInfo);
			this.myDownData.icar_SendTxtMsg(CmdParam.ParamType.SimNum, simnum, "", CmdParam.CommMode.未知方式, this.txtMsg, 0, alarmInfo);
		}

		private DataTable getConfigInfo()
		{
			DataTable dataTable;
			try
			{
				DateTime svrTime = ReadDataFromDB.GetSvrTime();
				string str = " select a.*,b.simnum,c.regiondot,c.regionname,c.regionfeature,d.* from GpsJtbCarRegionAlarm_Platform a inner join giscar b on a.carid = b.carid left join GpsRegionType c on a.regionid = c.regionid left join gpscarcurrentposinfo d WITH(NOLOCK) on b.simnum = d.telephone where d.lastupdatetime between '{0}' and '{1}' and (a.stopAlarmTime is null or a.stopAlarmTime < getdate()) ";
				DataTable dataBySql = SqlDataAccess.getDataBySql(string.Format(str, this.dtNow.ToString(), svrTime.ToString()));
				this.dtNow = svrTime;
				dataTable = dataBySql;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmRegionAlarm", "getConfigInfo", string.Concat("获取平台报警区域报警配置信息错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				dataTable = null;
			}
			return dataTable;
		}

		private double GetDistance(double lat1, double lng1, double lat2, double lng2)
		{
			double num;
			try
			{
				double num1 = this.rad(lat1);
				double num2 = this.rad(lat2);
				double num3 = num1 - num2;
				double num4 = this.rad(lng1) - this.rad(lng2);
				double num5 = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(num3 / 2), 2) + Math.Cos(num1) * Math.Cos(num2) * Math.Pow(Math.Sin(num4 / 2), 2)));
				num5 = num5 * 6378.137;
				num = Math.Round(num5 * 10000) / 10;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmRegionAlarm", "InsertAlarmInfo", string.Concat("判断两坐标点之间的距离错误，", exception.Message));
				this.logHelper.WriteError(errorMsg);
				num = 0;
			}
			return num;
		}

		private XmlDocument getInputXML(string Lon, string Lat, string regionDot)
		{
			XmlDocument xmlDocument;
			try
			{
				XmlDocument xmlDocument1 = new XmlDocument();
				XmlDeclaration xmlDeclaration = xmlDocument1.CreateXmlDeclaration("1.0", "UTF-8", null);
				xmlDocument1.AppendChild(xmlDeclaration);
				XmlElement xmlElement = xmlDocument1.CreateElement("Cars");
				XmlElement xmlElement1 = xmlDocument1.CreateElement("Car");
				xmlElement1.SetAttribute("id", "1");
				xmlElement1.SetAttribute("lon", Lon);
				xmlElement1.SetAttribute("lat", Lat);
				XmlElement xmlElement2 = xmlDocument1.CreateElement("Areas");
				XmlElement xmlElement3 = xmlDocument1.CreateElement("Area");
				xmlElement3.SetAttribute("id", "1");
				string str = regionDot.Replace("*", ";").Replace("\\", ",");
				char[] chrArray = new char[] { ';' };
				xmlElement3.SetAttribute("value", str.Trim(chrArray));
				xmlElement2.AppendChild(xmlElement3);
				xmlElement1.AppendChild(xmlElement2);
				xmlElement.AppendChild(xmlElement1);
				xmlDocument1.AppendChild(xmlElement);
				xmlDocument = xmlDocument1;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmRegionAlarm", "getInputXML", string.Concat("获取平台报警区域报警输入XML错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				xmlDocument = null;
			}
			return xmlDocument;
		}

		private int getResult(string OutXML)
		{
			int num;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(OutXML);
				XmlNode xmlNodes = xmlDocument.SelectSingleNode("//Cars");
				if (xmlNodes == null)
				{
					num = -1;
				}
				else
				{
					XmlElement itemOf = (XmlElement)xmlNodes.ChildNodes[0];
					XmlElement xmlElement = (XmlElement)itemOf.GetElementsByTagName("Areas")[0];
					if (xmlElement == null || xmlElement.ChildNodes.Count <= 0)
					{
						num = -1;
					}
					else
					{
						num = (!xmlElement.ChildNodes[0].Attributes["value"].Value.ToString().Equals("1") ? 0 : 1);
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmRegionAlarm", "getInputXML", string.Concat("获取平台报警区域报警解析返回XML错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				num = -1;
			}
			return num;
		}

		private void InsertAlarmInfo(DataRow dr, CmdParam.CarAlarmState carAlarmState, int regionID)
		{
			try
			{
				int num = 0;
				string empty = string.Empty;
				int num1 = 1;
				long num2 = 4503599627370496L;
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
					SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@wrkid", (object)num), new SqlParameter("@orderid", dr["OrderId"]), new SqlParameter("@userid", empty), new SqlParameter("@telephone", dr["telephone"]), new SqlParameter("@msgType", (object)num4), new SqlParameter("@recetime", dr["ReceTime"]), new SqlParameter("@gpstime", dr["GpsTime"]), new SqlParameter("@starCondition", dr["StarCondition"]), new SqlParameter("@starNum", dr["StarNum"]), new SqlParameter("@carStatu", (object)num1), new SqlParameter("@carStatuEx", (object)num2), new SqlParameter("@carCondition", dr["CarCondition"]), new SqlParameter("@Longitude", dr["Longitude"]), new SqlParameter("@Latitude", dr["Latitude"]), new SqlParameter("@direct", dr["Direct"]), new SqlParameter("@speed", dr["Speed"]), new SqlParameter("@Reserved", (object)num3), new SqlParameter("@TransportStatus", dr["TransportStatus"]), new SqlParameter("@Accelerration", dr["Accelerration"]), new SqlParameter("@Altitude", dr["Altitude"]), new SqlParameter("@DistanceDiff", dr["DistanceDiff"]), new SqlParameter("@commflag", dr["CommFlag"]), new SqlParameter("@addType", dr["AddMsgType"]), new SqlParameter("@addTxt", str1), new SqlParameter("@DutyStr", str2), new SqlParameter("@isPic", (object)flag), new SqlParameter("@pic", str3), new SqlParameter("@alarmInfo", str), new SqlParameter("@cameraID", (object)num5) };
					string str4 = "GpsPicServer_Alarm_Insert";
					string str5 = "GpsPicServer_RealTime_Insert";
					int num6 = SqlDataAccess.insertBySp(str4, sqlParameter);
					if (num6 > 0)
					{
						LogMsg logMsg = new LogMsg("PlatformAlarmRegionAlarm", "InsertAlarmInfo", "");
						object[] objArray = new object[] { "车载电话为：", dr["telephone"].ToString(), "的平台检测", carAlarmState.ToString(), "报警报文已插入gpsrecebuffer,区域ID：", regionID };
						logMsg.Msg = string.Concat(objArray);
						this.logHelper.WriteLog(logMsg);
					}
					else
					{
						object[] objArray1 = new object[] { "将平台检测", carAlarmState.ToString(), "报警报文插入gpsrecbuffer表错误，返回值!", num6.ToString(), ",区域ID：", regionID };
						ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmRegionAlarm", "InsertAlarmInfo", string.Concat(objArray1));
						this.logHelper.WriteError(errorMsg);
					}
					int num7 = SqlDataAccess.insertBySp(str5, sqlParameter);
					if (num7 > 0)
					{
						string[] strArrays = new string[] { "车载电话为：", dr["telephone"].ToString(), "的平台检测", carAlarmState.ToString(), "报警报文已插入gpsrecerealtime" };
						LogMsg logMsg1 = new LogMsg("PlatformAlarmRegionAlarm", "InsertAlarmInfo", string.Concat(strArrays));
						object[] objArray2 = new object[] { "车载电话为：", dr["telephone"].ToString(), "的平台检测", carAlarmState.ToString(), "报警报文已插入gpsrecerealtime,区域ID：", regionID };
						logMsg1.Msg = string.Concat(objArray2);
						this.logHelper.WriteLog(logMsg1);
					}
					else
					{
						object[] objArray3 = new object[] { "将平台检测", carAlarmState.ToString(), "报警报文插入gpsrecerealtime_buffer表发生错误，返回值!", num7.ToString(), ",区域ID：", regionID };
						ErrorMsg errorMsg1 = new ErrorMsg("PlatformAlarmRegionAlarm", "InsertAlarmInfo", string.Concat(objArray3));
						this.logHelper.WriteError(errorMsg1);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					object[] objArray4 = new object[] { "车载电话为：", dr["telephone"].ToString(), "的平台检测", carAlarmState.ToString(), ",区域ID：", regionID, "报警报文插入数据库发生错误! 信息：", exception.Message };
					ErrorMsg errorMsg2 = new ErrorMsg("PlatformAlarmRegionAlarm", "InsertAlarmInfo", string.Concat(objArray4));
					this.logHelper.WriteError(errorMsg2);
				}
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				LogHelper logHelper = new LogHelper();
				object[] objArray5 = new object[] { "将平台检测", carAlarmState.ToString(), ",区域ID：", regionID, "报警报文插入gpsrecbuffer表 、gpsrecerealtime_buffer表发生错误!", exception2.Message };
				ErrorMsg errorMsg3 = new ErrorMsg("PlatformAlarmRegionAlarm", "InsertAlarmInfo", string.Concat(objArray5));
				logHelper.WriteError(errorMsg3);
			}
		}

		private bool IsInArea(string Lon, string Lat, string regionDot)
		{
			XmlDocument inputXML = this.getInputXML(Lon, Lat, regionDot);
			if (inputXML != null)
			{
				LogMsg logMsg = new LogMsg("PlatformAlarmRegionAlarm", "IsInArea", "")
				{
					Msg = string.Concat("平台区域报警传 入 的xml：\r\n", inputXML.OuterXml)
				};
				this.logHelper.WriteLog(logMsg);
				string str = ReadDataFromGis.IsInArea(inputXML.OuterXml);
				logMsg.Msg = string.Concat("平台区域报警传 回 的xml：\r\n", str);
				this.logHelper.WriteLog(logMsg);
				int result = this.getResult(str);
				if (result == 1)
				{
					return true;
				}
				if (result == 0)
				{
					return false;
				}
			}
			throw new Exception("XML格式错误");
		}

		public bool IsInConfigTime(DateTime beginTime, DateTime endTime, DateTime dtNow)
		{
			if (beginTime.Year != 1900 || endTime.Year != 1900)
			{
				if (!(beginTime > dtNow) && !(endTime < dtNow))
				{
					return true;
				}
				return false;
			}
			if (beginTime < endTime && (endTime < Convert.ToDateTime(string.Concat("1900-01-01 ", dtNow.ToString("HH:mm:ss"))) || Convert.ToDateTime(string.Concat("1900-01-01 ", dtNow.ToString("HH:mm:ss"))) < beginTime) || beginTime > endTime && beginTime > Convert.ToDateTime(string.Concat("1900-01-01 ", dtNow.ToString("HH:mm:ss"))) && endTime < Convert.ToDateTime(string.Concat("1900-01-01 ", dtNow.ToString("HH:mm:ss"))))
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
				if (this.IsRectangle(regionDot))
				{
					string str = regionDot.Replace("*", "\\");
					char[] chrArray = new char[] { '\\' };
					string[] strArrays = str.Trim(chrArray).Split(new char[] { '\\' });
					flag = (double.Parse(Lon) <= double.Parse(strArrays[0]) || double.Parse(Lon) >= double.Parse(strArrays[4]) || double.Parse(Lat) >= double.Parse(strArrays[5]) || double.Parse(Lat) <= double.Parse(strArrays[1]) ? false : true);
				}
				else if (!this.IsRoundness(regionDot))
				{
					flag = this.IsInArea(Lon, Lat, regionDot);
				}
				else
				{
					string str1 = regionDot.Replace("*", "\\");
					char[] chrArray1 = new char[] { '\\' };
					string[] strArrays1 = str1.Trim(chrArray1).Split(new char[] { '\\' });
					flag = (this.GetDistance(double.Parse(Lat), double.Parse(Lon), double.Parse(strArrays1[1]), double.Parse(strArrays1[0])) <= double.Parse(strArrays1[2]) ? true : false);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "PlatformAlarmRegionAlarm",
					FunctionName = "IsInRegion",
					ErrorText = string.Concat("判断是否在区域内错误，", exception.Message)
				};
				this.logHelper.WriteError(errorMsg);
				flag = false;
			}
			return flag;
		}

		private bool IsProvideRegion(string regionDot)
		{
			return true;
		}

		private bool IsRectangle(string sRegionDot)
		{
			bool flag;
			try
			{
				string str = sRegionDot.Replace("*", "\\");
				char[] chrArray = new char[] { '\\' };
				string[] strArrays = str.Trim(chrArray).Split(new char[] { '\\' });
				if ((int)strArrays.Length == 8)
				{
					flag = ((!(strArrays[0] == strArrays[2]) || !(strArrays[1] == strArrays[7]) || !(strArrays[3] == strArrays[5]) || !(strArrays[4] == strArrays[6])) && (!(strArrays[0] == strArrays[6]) || !(strArrays[1] == strArrays[3]) || !(strArrays[2] == strArrays[4]) || !(strArrays[5] == strArrays[7])) ? false : true);
				}
				else
				{
					flag = false;
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		public bool IsRoundness(string sRegionDot)
		{
			bool flag;
			try
			{
				string str = sRegionDot.Replace("*", "\\").Trim(new char[] { '\\' });
				char[] chrArray = new char[] { '\\' };
				flag = ((int)str.Split(chrArray).Length == 3 ? true : false);
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		private double rad(double d)
		{
			return d * 3.14159265358979 / 180;
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
				object[] objArray = new object[] { str, simnum, str1, str2, DateTime.Now, 0 };
				int num = SqlDataAccess.insertBySql(string.Format(str4, objArray));
				if (num > 0)
				{
					LogMsg logMsg = new LogMsg("PlatformAlarmRegionAlarm", "ReportAlarmInfo", "")
					{
						Msg = string.Concat("将平台检测区域报警报文插入GPSJTBSysSndCmd表，AlarmType：", alarmType, "，simnum：", simnum)
					};
					this.logHelper.WriteLog(logMsg);
				}
				else
				{
					ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmRegionAlarm", "ReportAlarmInfo", string.Concat("将平台检测区域报警报文插入GPSJTBSysSndCmd表错误，AlarmType：", alarmType, ",返回值!", num.ToString()));
					this.logHelper.WriteError(errorMsg);
				}
				this.DownToTerminal(simnum, alarmType, alarmTime, alarmInfo);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg1 = new ErrorMsg("PlatformAlarmRegionAlarm", "ReportAlarmInfo", string.Concat("上报警情信息入库错误，", exception.Message));
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
				this.tCheckRegionAlarm = new Timer((double)this.iCheckRegionAlarm);
				this.tCheckRegionAlarm.Elapsed += new ElapsedEventHandler(this.tCheckRegionAlarm_Elapsed);
				this.tCheckRegionAlarm.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmRegionAlarm", "start", string.Concat("开启检测区域报警错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.tCheckRegionAlarm.Stop();
		}

		private void tCheckRegionAlarm_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tCheckRegionAlarm.Enabled = false;
			try
			{
				try
				{
					DataTable configInfo = this.getConfigInfo();
					if (configInfo != null && configInfo.Rows.Count > 0)
					{
						DateTime now = DateTime.Now;
						foreach (DataRow row in configInfo.Rows)
						{
							int num = Convert.ToInt32(row["carid"]);
							int num1 = Convert.ToInt32(row["regionID"]);
							if (row["Longitude"] == DBNull.Value || row["Latitude"] == DBNull.Value || double.Parse(row["Longitude"].ToString()) < 0.001 || double.Parse(row["Latitude"].ToString()) < 0.001)
							{
								continue;
							}
							if (row["regionDot"] == DBNull.Value || string.IsNullOrEmpty(row["regionDot"].ToString()))
							{
								LogMsg logMsg = new LogMsg("PlatformAlarmRegionAlarm", "tCheckRegionAlarm_Elapsed", "");
								object[] str = new object[] { "carid:", num, ",regionID:", num1.ToString(), ",simnum:", row["simnum"].ToString(), ",区域不存在" };
								logMsg.Msg = string.Concat(str);
								this.logHelper.WriteLog(logMsg);
							}
							else
							{
								DateTime dateTime = Convert.ToDateTime(row["beginTime"]);
								DateTime dateTime1 = Convert.ToDateTime(row["endTime"]);
								DateTime dateTime2 = Convert.ToDateTime(row["gpsTime"]);
								if (!this.IsInConfigTime(dateTime, dateTime1, now) || !this.IsInConfigTime(dateTime, dateTime1, dateTime2) || dateTime2.Date != now.Date)
								{
									if (!this.htInfo.Contains(string.Concat(num, "-", num1)))
									{
										continue;
									}
									this.htInfo.Remove(string.Concat(num, "-", num1));
									LogMsg logMsg1 = new LogMsg("PlatformAlarmRegionAlarm", "tCheckRegionAlarm_Elapsed", "");
									object[] objArray = new object[] { "carid:", num, ",regionID:", num1.ToString(), ",simnum:", row["simnum"].ToString(), ",未在设置时间段内，将车辆所在区域信息从内存移除" };
									logMsg1.Msg = string.Concat(objArray);
									this.logHelper.WriteLog(logMsg1);
								}
								else
								{
									string str1 = row["Longitude"].ToString();
									string str2 = row["Latitude"].ToString();
									string str3 = row["regionDot"].ToString();
									int num2 = Convert.ToInt32(row["regionType"]);
									if (this.IsProvideRegion(str3))
									{
										bool flag = this.IsInRegion(str1, str2, str3);
										if ((num2 & 1) == 1 && flag && this.htInfo[string.Concat(num, "-", num1)] != null && (int)this.htInfo[string.Concat(num, "-", num1)] == 2)
										{
											this.InsertAlarmInfo(row, CmdParam.CarAlarmState.入区域, num1);
											this.ReportAlarmInfo(row["simnum"].ToString(), "0004", row["gpsTime"].ToString(), "您的车辆发生进入区域报警");
										}
										if ((num2 & 2) == 2 && !flag && this.htInfo[string.Concat(num, "-", num1)] != null && (int)this.htInfo[string.Concat(num, "-", num1)] == 1)
										{
											this.InsertAlarmInfo(row, CmdParam.CarAlarmState.出区域, num1);
											this.ReportAlarmInfo(row["simnum"].ToString(), "0005", row["gpsTime"].ToString(), "您的车辆发生越出区域报警");
										}
										if (!flag)
										{
											this.htInfo[string.Concat(num, "-", num1)] = 2;
										}
										else
										{
											this.htInfo[string.Concat(num, "-", num1)] = 1;
										}
									}
									else
									{
										LogMsg logMsg2 = new LogMsg("PlatformAlarmRegionAlarm", "tCheckRegionAlarm_Elapsed", "");
										object[] objArray1 = new object[] { "carid:", num, ",regionID:", num1.ToString(), ",simnum:", row["simnum"].ToString(), ",区域不是圆形或矩形" };
										logMsg2.Msg = string.Concat(objArray1);
										this.logHelper.WriteLog(logMsg2);
									}
								}
							}
						}
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("PlatformAlarmRegionAlarm", "tCheckRegionAlarm_Elapsed", string.Concat("平台报警检测是否区域报警错误，", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tCheckRegionAlarm.Enabled = true;
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