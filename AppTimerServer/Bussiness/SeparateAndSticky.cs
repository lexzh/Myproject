using GisServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Timers;
using System.Xml;
using Library;

namespace Bussiness
{
	public class SeparateAndSticky : ProcessBase
	{
		private Timer tCheckSeparateSticky;

		private DateTime dtNow = ReadDataFromDB.GetSvrTime();

		private XmlDocument xDoc;

		private int SeparateAndStickyInterval = 1000 * ReadDataFromXml.iSeparateAndSticky;

		private DataTable dtBusesPosInfo;

		private List<AlarmBus> ltAlarmBus;

		private Hashtable htRouteList = new Hashtable();

		private int AlarmInterval = ReadDataFromXml.iAlarmInterval;

		private Hashtable htAlarmList = new Hashtable();

		public SeparateAndSticky()
		{
		}

		private void AnalysisOutputXML(string outXML)
		{
			try
			{
				DateTime now = DateTime.Now;
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(outXML);
				XmlNode xmlNodes = xmlDocument.SelectSingleNode("//BusRoutes");
				if (xmlNodes == null)
				{
					ErrorMsg errorMsg = new ErrorMsg("SeparateAndSticky", "AnalysisOutputXML", "")
					{
						ErrorText = "返回的xml错误，不包含BusRoutes节点"
					};
					this.logHelper.WriteError(errorMsg);
				}
				else
				{
					bool flag = false;
					string value = "";
					string str = "";
					int num = 0;
					int num1 = 0;
					string value1 = "";
					foreach (XmlElement childNode in xmlNodes.ChildNodes)
					{
						value = childNode.Attributes["id"].Value;
						XmlElement itemOf = (XmlElement)childNode.GetElementsByTagName("Buses")[0];
						if (itemOf == null)
						{
							continue;
						}
						foreach (XmlElement xmlElement in itemOf.ChildNodes)
						{
							str = xmlElement.Attributes["simNum"].Value;
							value1 = xmlElement.Attributes["lineId"].Value;
							if (xmlElement.Attributes["frontAlarmType"].Value != "0" && (this.htAlarmList[string.Concat(str, "-0")] == null || now.Subtract(Convert.ToDateTime(this.htAlarmList[string.Concat(str, "-0")])).TotalSeconds >= (double)this.AlarmInterval))
							{
								flag = true;
								num = Convert.ToInt32(xmlElement.Attributes["frontAlarmType"].Value);
								this.htAlarmList[string.Concat(str, "-0")] = now;
							}
							if (xmlElement.Attributes["backAlarmType"].Value != "0" && (this.htAlarmList[string.Concat(str, "-1")] == null || now.Subtract(Convert.ToDateTime(this.htAlarmList[string.Concat(str, "-1")])).TotalSeconds >= (double)this.AlarmInterval))
							{
								flag = true;
								num1 = Convert.ToInt32(xmlElement.Attributes["backAlarmType"].Value);
								this.htAlarmList[string.Concat(str, "-1")] = now;
							}
							if (!flag)
							{
								continue;
							}
							flag = false;
							this.ltAlarmBus.Add(AlarmBus.getAlarmBus(value, str, num, num1, value1));
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg1 = new ErrorMsg("SeparateAndSticky", "AnalysisOutputXML", string.Concat("解析返回的xml错误,", exception.Message));
				this.logHelper.WriteError(errorMsg1);
			}
		}

		private void clearHashT()
		{
			DataTable dataBySql = SqlDataAccess.getDataBySql(" select b.SimNum from GpsBusRouteOptions a INNER JOIN GisCar b on a.CarId = b.CarId ");
			if (dataBySql != null)
			{
				List<string> strs = new List<string>();
				foreach (string key in this.htAlarmList.Keys)
				{
					char[] chrArray = new char[] { '-' };
					if ((int)dataBySql.Select(string.Concat("SimNum = '", key.Split(chrArray)[0], "'")).Length > 0)
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
		}

		private XmlElement getBusesInfo(DataTable dtBusInfo)
		{
			XmlElement xmlElement;
			try
			{
				if (this.dtBusesPosInfo == null)
				{
					this.dtBusesPosInfo = new DataTable();
					this.dtBusesPosInfo = dtBusInfo.Clone();
				}
				XmlElement itemOf = (XmlElement)this.xDoc.GetElementsByTagName("BusRoutes")[0];
				XmlElement xmlElement1 = this.xDoc.CreateElement("Buses");
				string str = "";
				string str1 = "";
				string str2 = "";
				string str3 = "";
				foreach (DataRow row in dtBusInfo.Rows)
				{
					str = row["telephone"].ToString();
					str1 = row["Longitude"].ToString();
					str2 = row["Latitude"].ToString();
					str3 = row["direct"].ToString();
					if (double.Parse(str1) < 0.001 || double.Parse(str2) < 0.001)
					{
						continue;
					}
					XmlElement busInfo = this.getBusInfo(str, str1, str2, str3);
					xmlElement1.AppendChild(busInfo);
					this.dtBusesPosInfo.ImportRow(row);
				}
				xmlElement = xmlElement1;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("SeparateAndSticky", "getBusesInfo", string.Concat("获取Buses节点信息错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				xmlElement = null;
			}
			return xmlElement;
		}

		private XmlElement getBusInfo(string simNum, string lon, string lat, string direction)
		{
			XmlElement xmlElement;
			try
			{
				XmlElement xmlElement1 = this.xDoc.CreateElement("Bus");
				xmlElement1.SetAttribute("simNum", simNum);
				xmlElement1.SetAttribute("lon", lon);
				xmlElement1.SetAttribute("lat", lat);
				xmlElement1.SetAttribute("direction", direction);
				xmlElement = xmlElement1;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("SeparateAndSticky", "getBusInfo", string.Concat("获取Bus节点信息错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				xmlElement = null;
			}
			return xmlElement;
		}

		private DataTable getBusPosInfo(string RouteId, DateTime dtNewTime)
		{
			DataTable dataTable;
			try
			{
				string str = " select a.CarId, a.RouteId, c.* from GpsBusRouteOptions a INNER JOIN Giscar b on a.CarId = b.CarId INNER JOIN GpsCarCurrentPosInfo c WITH(NOLOCK) on b.simnum = c.telephone where a.RouteId = '{0}' and c.lastupdatetime between '{1}' and '{2}' ";
				DataTable dataBySql = SqlDataAccess.getDataBySql(string.Format(str, RouteId, this.dtNow.ToString(), dtNewTime.ToString()));
				dataTable = dataBySql;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("SeparateAndSticky", "getBusPosInfo", string.Concat("获取车辆最新位置信息错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				dataTable = null;
			}
			return dataTable;
		}

		private DataTable getBusRouteConfig()
		{
			DataTable dataBySql;
			try
			{
				dataBySql = SqlDataAccess.getDataBySql(" SELECT a.RouteID, b.RouteName, b.Separate, b.Sticky, b.PathDotUp, b.PathDotDown FROM GpsBusRouteOptions a INNER JOIN GpsBusRoute b ON a.RouteId = b.RouteId ");
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("SeparateAndSticky", "getBusRouteConfig", string.Concat("获取公交路线配置信息,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				return null;
			}
			return dataBySql;
		}

		private DataTable getRouteLine(string RouteId, string Type)
		{
			DataTable dataBySql;
			try
			{
				string str = " SELECT a.*, b.StationName, b.StationDot FROM GpsBusRouteRelation a INNER JOIN GpsBusStop b on a.StationId = b.StationId where a.RouteId = '{0}' and a.Type = '{1}' order by a.StationIndex ";
				dataBySql = SqlDataAccess.getDataBySql(string.Format(str, RouteId, Type));
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("SeparateAndSticky", "getRouteLines", string.Concat("获取公交路线信息,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				dataBySql = null;
			}
			return dataBySql;
		}

		private string[] getRouteLines(string RouteId)
		{
			string[] strArrays = new string[2];
			DataTable routeLine = this.getRouteLine(RouteId, "0");
			if (routeLine == null || routeLine.Rows.Count <= 0)
			{
				strArrays[0] = "";
			}
			else
			{
				foreach (DataRow row in routeLine.Rows)
				{
					string[] strArrays1 = strArrays;
					string[] strArrays2 = strArrays1;
					strArrays1[0] = string.Concat(strArrays2[0], row["StationDot"].ToString().Replace("*", ","), ";");
				}
				string str = strArrays[0];
				char[] chrArray = new char[] { ';' };
				strArrays[0] = str.Trim(chrArray);
			}
			DataTable dataTable = this.getRouteLine(RouteId, "1");
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				strArrays[1] = "";
			}
			else
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					string[] strArrays3 = strArrays;
					string[] strArrays4 = strArrays3;
					strArrays3[1] = string.Concat(strArrays4[1], dataRow["StationDot"].ToString().Replace("*", ","), ";");
				}
				string str1 = strArrays[1];
				char[] chrArray1 = new char[] { ';' };
				strArrays[1] = str1.Trim(chrArray1);
			}
			return strArrays;
		}

		private void initInputXML()
		{
			try
			{
				this.xDoc = new XmlDocument();
				XmlDeclaration xmlDeclaration = this.xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
				this.xDoc.AppendChild(xmlDeclaration);
				XmlElement xmlElement = this.xDoc.CreateElement("BusRoutes");
				this.xDoc.AppendChild(xmlElement);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("SeparateAndSticky", "initInputXML", string.Concat("初始化要传入的XML错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		private void InsertAlarmInfo(List<AlarmBus> ltAlarmBus)
		{
			if (this.dtBusesPosInfo == null || this.dtBusesPosInfo.Rows.Count <= 0 || ltAlarmBus == null || ltAlarmBus.Count <= 0)
			{
				return;
			}
			foreach (AlarmBus ltAlarmBu in ltAlarmBus)
			{
				DataRow[] dataRowArray = this.dtBusesPosInfo.Select(string.Concat("telephone like '", ltAlarmBu.SimNum, "'"));
				if ((int)dataRowArray.Length <= 0)
				{
					continue;
				}
				this.InsertAlarmInfo(dataRowArray[0], ltAlarmBu);
			}
		}

		private void InsertAlarmInfo(DataRow dr, AlarmBus ab)
		{
			try
			{
				int num = 0;
				string empty = string.Empty;
				int num1 = Convert.ToInt32(dr["carstatu"]);
				long num2 = (long)0;
				num2 = (dr["carstatuex"] == DBNull.Value || dr["carstatuex"].ToString().Equals("") ? 18014398509481984L : 18014398509481984L | Convert.ToInt64(dr["carstatuex"]));
				int num3 = 1154;
				int num4 = 65;
				string str = string.Empty;
				string str1 = "M13";
				string str2 = "";
				if (ab.FrontAlarmType == 1 || ab.BackAlarmType == 1)
				{
					str2 = string.Concat(str2, "脱车、");
				}
				if (ab.FrontAlarmType == 2 || ab.BackAlarmType == 2)
				{
					str2 = string.Concat(str2, "粘车");
				}
				string str3 = "";
				string[] strArrays = new string[5];
				char[] chrArray = new char[] { '、' };
				strArrays[0] = str2.Trim(chrArray);
				strArrays[1] = ",路线: ";
				strArrays[2] = this.htRouteList[ab.RouteId].ToString();
				strArrays[3] = (ab.LineId.Equals("0") ? " 上行" : " 下行");
				strArrays[4] = "方向";
				str2 = string.Concat(strArrays);
				byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(str2);
				for (int i = 0; i < (int)bytes.Length; i++)
				{
					str3 = string.Concat(str3, bytes[i].ToString("X2"));
				}
				str1 = string.Concat(str1, str3);
				string str4 = null;
				bool flag = false;
				string str5 = null;
				int num5 = 0;
				try
				{
					SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@wrkid", (object)num), new SqlParameter("@orderid", dr["OrderId"]), new SqlParameter("@userid", empty), new SqlParameter("@telephone", dr["telephone"]), new SqlParameter("@msgType", (object)num4), new SqlParameter("@recetime", dr["ReceTime"]), new SqlParameter("@gpstime", dr["GpsTime"]), new SqlParameter("@starCondition", dr["StarCondition"]), new SqlParameter("@starNum", dr["StarNum"]), new SqlParameter("@carStatu", (object)num1), new SqlParameter("@carStatuEx", (object)num2), new SqlParameter("@carCondition", dr["CarCondition"]), new SqlParameter("@Longitude", dr["Longitude"]), new SqlParameter("@Latitude", dr["Latitude"]), new SqlParameter("@direct", dr["Direct"]), new SqlParameter("@speed", dr["Speed"]), new SqlParameter("@Reserved", (object)num3), new SqlParameter("@TransportStatus", dr["TransportStatus"]), new SqlParameter("@Accelerration", dr["Accelerration"]), new SqlParameter("@Altitude", dr["Altitude"]), new SqlParameter("@DistanceDiff", dr["DistanceDiff"]), new SqlParameter("@commflag", dr["CommFlag"]), new SqlParameter("@addType", dr["AddMsgType"]), new SqlParameter("@addTxt", str1), new SqlParameter("@DutyStr", str4), new SqlParameter("@isPic", (object)flag), new SqlParameter("@pic", str5), new SqlParameter("@alarmInfo", str), new SqlParameter("@cameraID", (object)num5) };
					string str6 = "GpsPicServer_ReceBuffer_Insert";
					string str7 = "GpsPicServer_RealTime_Insert";
					int num6 = SqlDataAccess.insertBySp(str6, sqlParameter);
					if (num6 > 0)
					{
						LogMsg logMsg = new LogMsg("SeparateAndSticky", "InsertAlarmInfo", "")
						{
							Msg = string.Concat("车载电话为：", dr["telephone"].ToString(), "的报警报文已插入gpsrecebuffer")
						};
						this.logHelper.WriteLog(logMsg);
					}
					else
					{
						ErrorMsg errorMsg = new ErrorMsg("SeparateAndSticky", "InsertAlarmInfo", string.Concat("将报警报文插入gpsrecbuffer表错误，返回值!", num6.ToString()));
						this.logHelper.WriteError(errorMsg);
					}
					int num7 = SqlDataAccess.insertBySp(str7, sqlParameter);
					if (num7 > 0)
					{
						LogMsg logMsg1 = new LogMsg("PlatformAlarmPathAlarm", "InsertAlarmInfo", string.Concat("车载电话为：", dr["telephone"].ToString(), "的脱车粘车报警报文已插入gpsrecerealtime"))
						{
							Msg = string.Concat("车载电话为：", dr["telephone"].ToString(), "的脱车粘车报警报文已插入gpsrecerealtime")
						};
						this.logHelper.WriteLog(logMsg1);
					}
					else
					{
						ErrorMsg errorMsg1 = new ErrorMsg("PlatformAlarmPathAlarm", "InsertAlarmInfo", string.Concat("将脱车粘车报警报文插入gpsrecerealtime_buffer表发生错误，返回值!", num7.ToString()));
						this.logHelper.WriteError(errorMsg1);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg2 = new ErrorMsg("SeparateAndSticky", "InsertAlarmInfo", string.Concat("车载电话为：", dr["telephone"].ToString(), "的报警报文插入数据库发生错误! 信息：", exception.Message));
					this.logHelper.WriteError(errorMsg2);
				}
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				LogHelper logHelper = new LogHelper();
				ErrorMsg errorMsg3 = new ErrorMsg("SeparateAndSticky", "InsertAlarmInfo", string.Concat("将报警报文插入gpsrecbuffer表 、gpsrecerealtime_buffer表发生错误!", exception2.Message));
				logHelper.WriteError(errorMsg3);
			}
		}

		private void setRouteLine(string RouteId, string Separate, string Sticky, string Line0, string Line1, XmlElement xBuses)
		{
			try
			{
				XmlElement itemOf = (XmlElement)this.xDoc.GetElementsByTagName("BusRoutes")[0];
				XmlElement xmlElement = this.xDoc.CreateElement("Route");
				xmlElement.SetAttribute("id", RouteId);
				xmlElement.SetAttribute("stickyLen", Sticky);
				xmlElement.SetAttribute("separateLen", Separate);
				XmlElement xmlElement1 = this.xDoc.CreateElement("RouteLines");
				if (!string.IsNullOrEmpty(Line0))
				{
					XmlElement xmlElement2 = this.xDoc.CreateElement("Line");
					xmlElement2.SetAttribute("id", "0");
					xmlElement2.SetAttribute("lineString", Line0);
					xmlElement1.AppendChild(xmlElement2);
				}
				if (!string.IsNullOrEmpty(Line1))
				{
					XmlElement xmlElement3 = this.xDoc.CreateElement("Line");
					xmlElement3.SetAttribute("id", "1");
					xmlElement3.SetAttribute("lineString", Line1);
					xmlElement1.AppendChild(xmlElement3);
				}
				xmlElement.AppendChild(xmlElement1);
				xmlElement.AppendChild(xBuses);
				itemOf.AppendChild(xmlElement);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("SeparateAndSticky", "setRouteLine", string.Concat("设置XML中的路线信息错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void start()
		{
			try
			{
				this.tCheckSeparateSticky = new Timer((double)this.SeparateAndStickyInterval);
				this.tCheckSeparateSticky.Elapsed += new ElapsedEventHandler(this.tCheckSeparateSticky_Elapsed);
				this.tCheckSeparateSticky.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("SeparateAndSticky", "start", string.Concat("开启检测脱车粘车报警错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.tCheckSeparateSticky.Stop();
		}

		private void tCheckSeparateSticky_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tCheckSeparateSticky.Enabled = false;
			try
			{
				try
				{
					DataTable busRouteConfig = this.getBusRouteConfig();
					this.clearHashT();
					if (busRouteConfig != null && busRouteConfig.Rows.Count > 0)
					{
						DateTime svrTime = ReadDataFromDB.GetSvrTime();
						this.initInputXML();
						string str = "";
						string str1 = "";
						string str2 = "";
						string[] strArrays = new string[2];
						bool flag = false;
						List<string> strs = new List<string>();
						foreach (DataRow row in busRouteConfig.Rows)
						{
							str = row["RouteId"].ToString();
							if (strs.Contains(str))
							{
								continue;
							}
							strs.Add(str);
							this.htRouteList[str] = row["RouteName"].ToString();
							str1 = row["Separate"].ToString();
							str2 = row["Sticky"].ToString();
							strArrays[0] = row["PathDotUp"].ToString();
							strArrays[1] = row["PathDotDown"].ToString();
							if (string.IsNullOrEmpty(strArrays[0]) && string.IsNullOrEmpty(strArrays[1]))
							{
								continue;
							}
							DataTable busPosInfo = this.getBusPosInfo(str, svrTime);
							if (busPosInfo == null || busPosInfo.Rows.Count <= 0)
							{
								continue;
							}
							XmlElement busesInfo = this.getBusesInfo(busPosInfo);
							this.setRouteLine(str, str1, str2, strArrays[0], strArrays[1], busesInfo);
							flag = true;
						}
						this.dtNow = svrTime;
						if (flag)
						{
							LogMsg logMsg = new LogMsg("PlatformAlarmPathAlarm", "tCheckSeparateSticky_Elapsed", "")
							{
								Msg = string.Concat("脱车粘车传 入 的xml：\r\n", this.xDoc.OuterXml)
							};
							this.logHelper.WriteLog(logMsg);
							string separateAndStickyCars = ReadDataFromGis.GetSeparateAndStickyCars(this.xDoc.OuterXml);
							logMsg.Msg = string.Concat("脱车粘车传 回 的xml：\r\n", separateAndStickyCars);
							this.logHelper.WriteLog(logMsg);
							if (!string.IsNullOrEmpty(separateAndStickyCars))
							{
								this.ltAlarmBus = new List<AlarmBus>();
								this.AnalysisOutputXML(separateAndStickyCars);
							}
							this.InsertAlarmInfo(this.ltAlarmBus);
						}
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("SeparateAndSticky", "tCheckSeparateSticky_Elapsed", string.Concat("定时检测脱车粘车报警错误,", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				if (this.ltAlarmBus != null)
				{
					this.ltAlarmBus.Clear();
					this.ltAlarmBus = null;
				}
				if (this.dtBusesPosInfo != null)
				{
					this.dtBusesPosInfo.Rows.Clear();
					this.dtBusesPosInfo = null;
				}
				this.tCheckSeparateSticky.Enabled = true;
			}
		}
	}
}