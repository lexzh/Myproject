using GisServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Timers;
using System.Xml;
using Library;

namespace Bussiness
{
	public class PlatFormAlarmThreeLevelRoadAlarm : ProcessBase
	{
		private Timer tCheckThreeLevelRoadAlarm;

		private int iCheckInterval = 60000 * ReadDataFromXml.numThreeLevelRoadInterval;

		private DateTime dtNow = ReadDataFromDB.GetSvrTime();

		private DataTable dtConfiginfo;

		private XmlDocument xDoc;

		private Hashtable htThreeLevelRoad = new Hashtable();

		private Hashtable htNumCarid = new Hashtable();

		private Hashtable htCaridStat = new Hashtable();

		private List<string> AlarmListFirst;

		private List<string> AlarmListMid;

		private List<string> AlarmListEnd;

		public PlatFormAlarmThreeLevelRoadAlarm()
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
				XmlNode xmlNodes = xmlDocument.SelectSingleNode("//PosMsgs");
				if (xmlNodes == null)
				{
					ErrorMsg errorMsg = new ErrorMsg("PlatFormAlarmThreeLevelRoadAlarm", "AnalysisOutputXML", "")
					{
						ErrorText = "返回的xml错误，不包含PosMsgs 节点"
					};
					this.logHelper.WriteError(errorMsg);
					flag = false;
				}
				else
				{
					foreach (XmlElement childNode in xmlNodes.ChildNodes)
					{
						int num = int.Parse(childNode.Attributes["id"].Value);
						int num1 = int.Parse(this.htNumCarid[num].ToString());
						int num2 = int.Parse(this.htCaridStat[num1].ToString());
						if (!((XmlElement)childNode.GetElementsByTagName("Status")[0]).Attributes["value"].Value.Equals("536870912"))
						{
							if (num2 != 1)
							{
								continue;
							}
							this.AlarmListEnd.Add(num1.ToString());
							this.htCaridStat[num1] = 0;
						}
						else
						{
							if (num2 != 0)
							{
								this.AlarmListMid.Add(num1.ToString());
							}
							else
							{
								this.AlarmListFirst.Add(num1.ToString());
							}
							this.htCaridStat[num1] = 1;
						}
					}
					flag = true;
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg1 = new ErrorMsg("PlatFormAlarmThreeLevelRoadAlarm", "AnalysisOutputXML", string.Concat("解析返回XML错误,", exception.Message));
				this.logHelper.WriteError(errorMsg1);
				flag = false;
			}
			return flag;
		}

		private void CheckAndInsert()
		{
			LogMsg logMsg = new LogMsg("PatFormAlarmThreeLevelRoadAlarm", "tCheckThreeLevelRoadAlarm_Elapsed", "")
			{
				Msg = string.Concat("三级路面报警传入的的xml：\r\n", this.xDoc.OuterXml)
			};
			this.logHelper.WriteLog(logMsg);
			string alarmState = ReadDataFromGis.GetAlarmState(this.xDoc.OuterXml);
			logMsg.Msg = string.Concat("三级路面报警传 回 的xml：\r\n", alarmState);
			this.logHelper.WriteLog(logMsg);
			if (!string.IsNullOrEmpty(alarmState) && this.AnalysisOutputXML(alarmState))
			{
				if (this.AlarmListFirst != null && this.AlarmListFirst.Count > 0)
				{
					foreach (string alarmListFirst in this.AlarmListFirst)
					{
						DataRow[] dataRowArray = this.dtConfiginfo.Select(string.Concat("carid=", alarmListFirst));
						this.InsertAlarmInfo(dataRowArray[0], -99999);
					}
				}
				this.AlarmListFirst.Clear();
				this.AlarmListFirst = null;
				if (this.AlarmListMid != null && this.AlarmListMid.Count > 0)
				{
					foreach (string alarmListMid in this.AlarmListMid)
					{
						DataRow[] dataRowArray1 = this.dtConfiginfo.Select(string.Concat("carid = ", alarmListMid));
						this.InsertAlarmInfo(dataRowArray1[0], -99998);
					}
				}
				this.AlarmListMid.Clear();
				this.AlarmListMid = null;
				if (this.AlarmListEnd != null && this.AlarmListEnd.Count > 0)
				{
					foreach (string alarmListEnd in this.AlarmListEnd)
					{
						DataRow[] dataRowArray2 = this.dtConfiginfo.Select(string.Concat("carid = ", alarmListEnd));
						this.InsertAlarmInfo(dataRowArray2[0], -99997);
					}
				}
				this.AlarmListEnd.Clear();
				this.AlarmListEnd = null;
			}
		}

		private DataTable getConfigInfo()
		{
			DataTable dataTable;
			try
			{
				DateTime svrTime = ReadDataFromDB.GetSvrTime();
				string str = "select a.carid,c.* from  gpscarlevelthreeroad a inner join giscar b  on a.carid=b.carid inner join GpsCarCurrentPosInfo c with(nolock) on b.simnum=c.telephone where c.lastupdatetime between '{0}' and '{1}'";
				DataTable dataBySql = SqlDataAccess.getDataBySql(string.Format(str, this.dtNow, svrTime));
				this.dtNow = svrTime;
				dataTable = dataBySql;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatFormAlarmThreeLevelRoadAlarm", "getConfigInfo", string.Concat("获取三级路面报警配置信息错误", exception.Message));
				this.logHelper.WriteError(errorMsg);
				dataTable = null;
			}
			return dataTable;
		}

		private void initInputXML()
		{
			this.xDoc = new XmlDocument();
			XmlDeclaration xmlDeclaration = this.xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
			this.xDoc.AppendChild(xmlDeclaration);
			XmlElement xmlElement = this.xDoc.CreateElement("PosMsgs");
			this.xDoc.AppendChild(xmlElement);
		}

		private void InsertAlarmInfo(DataRow dr, int AddMsgType)
		{
			try
			{
				int num = 0;
				string empty = string.Empty;
				int num1 = Convert.ToInt32(dr["carstatu"]);
				long num2 = (long)0;
				num2 = (dr["carStatuEx"] == DBNull.Value || dr["carstatuex"].ToString().Equals("") ? 274877906944L : 274877906944L | Convert.ToInt64(dr["carstatuex"]));
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
							LogMsg logMsg = new LogMsg("PlatFormAlarmThreeLevelRoadAlarm", "InsertAlarmInfo", "")
							{
								Msg = string.Concat("车载电话为：", dr["telephone"].ToString(), "的平台检测三级路面报警报文已插入gpsrecebuffer")
							};
							this.logHelper.WriteLog(logMsg);
						}
						else
						{
							ErrorMsg errorMsg = new ErrorMsg("PlatFormAlarmThreeLevelRoadAlarm", "InsertAlarmInfo", string.Concat("将平台检测三级路面报警报文插入gpsrecbuffer表错误，返回值!", num6.ToString()));
							this.logHelper.WriteError(errorMsg);
						}
					}
					int num7 = SqlDataAccess.insertBySp(str5, sqlParameter);
					if (num7 > 0)
					{
						LogMsg logMsg1 = new LogMsg("PlatFormAlarmThreeLevelRoadAlarm", "InsertAlarmInfo", string.Concat("车载电话为：", dr["telephone"].ToString(), "的平台检测三级路面报警报文已插入gpsrecerealtime"))
						{
							Msg = string.Concat("车载电话为：", dr["telephone"].ToString(), "的平台检测三级路面报警报文已插入gpsrecerealtime")
						};
						this.logHelper.WriteLog(logMsg1);
					}
					else
					{
						ErrorMsg errorMsg1 = new ErrorMsg("PlatFormAlarmThreeLevelRoadAlarm", "InsertAlarmInfo", string.Concat("将平台检测三级路面报警报文插入gpsrecerealtime_buffer表发生错误，返回值!", num7.ToString()));
						this.logHelper.WriteError(errorMsg1);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg2 = new ErrorMsg("PlatFormAlarmThreeLevelRoadAlarm", "InsertAlarmInfo", string.Concat("车载电话为：", dr["telephone"].ToString(), "的平台检测三级路面报警报文插入数据库发生错误! 信息：", exception.Message));
					this.logHelper.WriteError(errorMsg2);
				}
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				ErrorMsg errorMsg3 = new ErrorMsg("PlatformAlarmPathAlarm", "InsertAlarmInfo", string.Concat("将平台检测三级路面报警报文插入gpsrecbuffer表 、gpsrecerealtime_buffer表发生错误!", exception2.Message));
				this.logHelper.WriteError(errorMsg3);
			}
		}

		private void setInputXML()
		{
			try
			{
				this.initInputXML();
				int num = 0;
				XmlElement itemOf = (XmlElement)this.xDoc.GetElementsByTagName("PosMsgs")[0];
				foreach (int key in this.htThreeLevelRoad.Keys)
				{
					string[] strArrays = this.htThreeLevelRoad[key].ToString().Split(new char[] { '*' });
					XmlElement xmlElement = this.xDoc.CreateElement("PosMsg");
					xmlElement.SetAttribute("id", strArrays[0].ToString());
					XmlElement xmlElement1 = this.xDoc.CreateElement("Lon");
					xmlElement1.SetAttribute("value", strArrays[1].ToString());
					XmlElement xmlElement2 = this.xDoc.CreateElement("Lat");
					xmlElement2.SetAttribute("value", strArrays[2].ToString());
					XmlElement xmlElement3 = this.xDoc.CreateElement("Speed");
					xmlElement3.SetAttribute("value", strArrays[3].ToString());
					xmlElement.AppendChild(xmlElement1);
					xmlElement.AppendChild(xmlElement2);
					xmlElement.AppendChild(xmlElement3);
					itemOf.AppendChild(xmlElement);
					num++;
					if (num < 100)
					{
						continue;
					}
					itemOf.SetAttribute("nums", num.ToString());
					this.CheckAndInsert();
					this.initInputXML();
					itemOf = (XmlElement)this.xDoc.GetElementsByTagName("PosMsgs")[0];
					num = 0;
				}
				itemOf.SetAttribute("nums", num.ToString());
				this.xDoc.AppendChild(itemOf);
				this.CheckAndInsert();
				this.initInputXML();
				itemOf = (XmlElement)this.xDoc.GetElementsByTagName("PosMsgs")[0];
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("PlatFormAlarmThreeLevelRoadAlarm", "setInputXML", string.Concat("组合XML出错", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void start()
		{
			this.tCheckThreeLevelRoadAlarm = new Timer((double)this.iCheckInterval);
			this.tCheckThreeLevelRoadAlarm.Elapsed += new ElapsedEventHandler(this.tCheckThreeLevelRoadAlarm_Elapsed);
			this.tCheckThreeLevelRoadAlarm.Enabled = true;
		}

		public override void stop()
		{
			this.tCheckThreeLevelRoadAlarm.Stop();
		}

		private void tCheckThreeLevelRoadAlarm_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tCheckThreeLevelRoadAlarm.Enabled = false;
			DataTable dataBySql = null;
			dataBySql = SqlDataAccess.getDataBySql("select carid from GpsCarLevelThreeRoad");
			ArrayList arrayLists = new ArrayList();
			foreach (int key in this.htCaridStat.Keys)
			{
				if ((int)dataBySql.Select(string.Concat("CarId=", key)).Length > 0)
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
					this.dtConfiginfo = this.getConfigInfo();
					if (this.dtConfiginfo != null && this.dtConfiginfo.Rows.Count > 0)
					{
						int num = 1;
						foreach (DataRow row in this.dtConfiginfo.Rows)
						{
							int num1 = int.Parse(row["carid"].ToString());
							DateTime.Parse(row["gpstime"].ToString());
							Hashtable hashtables = this.htThreeLevelRoad;
							object obj = num1;
							string[] str = new string[] { num.ToString(), "*", row["longitude"].ToString(), "*", row["latitude"].ToString(), "*", row["speed"].ToString() };
							hashtables[obj] = string.Concat(str);
							if (!this.htCaridStat.Contains(num1))
							{
								this.htCaridStat[num1] = 0;
							}
							this.htNumCarid[num] = num1;
							num++;
						}
						this.setInputXML();
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("PlatFormAlarmThreeLevelRoadAlarm", "tCheckThreeLevelRoadAlarm_Elapsed", string.Concat("检测三级路面报警错误", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.htNumCarid.Clear();
				this.htThreeLevelRoad.Clear();
				this.tCheckThreeLevelRoadAlarm.Enabled = true;
			}
		}
	}
}