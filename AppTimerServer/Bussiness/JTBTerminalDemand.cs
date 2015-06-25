using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Timers;
using System.Xml;
using Library;

namespace Bussiness
{
	public class JTBTerminalDemand : ProcessBase
	{
		private DownData myDownData = new DownData(0);

		private Timer tTerminalDemand;

		private int iTerminalDemand = 10000;

		private string OrderId = "0";

		private string CarType = "JTBGPS";

		private string CmdCode = "1542";

		private string CommFlag = "-1";

		private Hashtable htDownInterval = new Hashtable();

		public JTBTerminalDemand()
		{
		}

		private void clearDownList(DataTable dtDemand)
		{
			List<string> strs = new List<string>();
			foreach (string key in this.htDownInterval.Keys)
			{
				char[] chrArray = new char[] { '-' };
				string str = key.Split(chrArray)[0];
				char[] chrArray1 = new char[] { '-' };
				string str1 = key.Split(chrArray1)[1];
				string[] strArrays = new string[] { "simnum = '", str, "' and MsgID = '", str1, "'" };
				if ((int)dtDemand.Select(string.Concat(strArrays)).Length > 0)
				{
					continue;
				}
				strs.Add(key);
			}
			foreach (string str2 in strs)
			{
				this.htDownInterval.Remove(str2);
			}
		}

		private DataTable getConfigInfo()
		{
			DataTable dataBySql;
			try
			{
				dataBySql = SqlDataAccess.getDataBySql(" select a.SimNum,a.MsgID,c.*,d.Param,b.CarId from GpsJTBMsgHistory a INNER JOIN GisCar b on a.SimNum = b.SimNum INNER JOIN GpsJTBMsgParam c on a.MsgType = c.MsgType and a.MsgID = c.ID INNER JOIN GpsCarSetParamEx d on b.CarID = d.carId where a.MsgType = '2' ");
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("JTBTerminalDemand", "getConfigInfo", string.Concat("获取终端点播配置信息失败", exception.Message));
				this.logHelper.WriteError(errorMsg);
				return null;
			}
			return dataBySql;
		}

		private bool IsDown(string p, string p_2, DateTime dtNow, string Interval)
		{
			int num;
			bool flag;
			try
			{
				try
				{
					num = Convert.ToInt32(Interval);
				}
				catch
				{
					num = 60;
				}
				if (this.htDownInterval[string.Concat(p, "-", p_2)] == null)
				{
					this.htDownInterval[string.Concat(p, "-", p_2)] = dtNow;
					flag = true;
				}
				else if (dtNow.Subtract((DateTime)this.htDownInterval[string.Concat(p, "-", p_2)]).TotalSeconds < (double)num)
				{
					flag = false;
				}
				else
				{
					this.htDownInterval[string.Concat(p, "-", p_2)] = dtNow;
					flag = true;
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("JTBTerminalDemand", "IsDown", string.Concat("判断是否应下发错误，", exception.Message));
				this.logHelper.WriteError(errorMsg);
				flag = false;
			}
			return flag;
		}

		private XmlDocument SetXML(string simnum, string type, string text)
		{
			XmlDocument xmlDocument;
			try
			{
				XmlDocument xmlDocument1 = new XmlDocument();
				XmlDeclaration xmlDeclaration = xmlDocument1.CreateXmlDeclaration("1.0", "UTF-8", null);
				xmlDocument1.AppendChild(xmlDeclaration);
				XmlElement xmlElement = xmlDocument1.CreateElement("function");
				xmlElement.SetAttribute("name", "SimpleCmd");
				xmlElement.SetAttribute("version", "1.0.0");
				XmlElement xmlElement1 = xmlDocument1.CreateElement("body");
				XmlElement orderId = xmlDocument1.CreateElement("OrderID");
				orderId.InnerText = this.OrderId;
				XmlElement xmlElement2 = xmlDocument1.CreateElement("Sim");
				xmlElement2.InnerText = simnum;
				XmlElement carType = xmlDocument1.CreateElement("CarType");
				carType.InnerText = this.CarType;
				XmlElement cmdCode = xmlDocument1.CreateElement("CmdCode");
				cmdCode.InnerText = this.CmdCode;
				XmlElement commFlag = xmlDocument1.CreateElement("CommFlag");
				commFlag.InnerText = this.CommFlag;
				XmlElement xmlElement3 = xmlDocument1.CreateElement("Parameter");
				XmlElement xmlElement4 = xmlDocument1.CreateElement("Type");
				xmlElement4.InnerText = type;
				XmlElement xmlElement5 = xmlDocument1.CreateElement("Text");
				xmlElement5.InnerText = text;
				xmlElement3.AppendChild(xmlElement4);
				xmlElement3.AppendChild(xmlElement5);
				xmlElement1.AppendChild(orderId);
				xmlElement1.AppendChild(xmlElement2);
				xmlElement1.AppendChild(carType);
				xmlElement1.AppendChild(cmdCode);
				xmlElement1.AppendChild(commFlag);
				xmlElement1.AppendChild(xmlElement3);
				xmlElement.AppendChild(xmlElement1);
				xmlDocument1.AppendChild(xmlElement);
				xmlDocument = xmlDocument1;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("JTBTerminalDemand", "SetXML", "")
				{
					ErrorText = string.Concat("设置XML错误，sim:", simnum, ",", exception.Message)
				};
				this.logHelper.WriteError(errorMsg);
				xmlDocument = null;
			}
			return xmlDocument;
		}

		public override void start()
		{
			try
			{
				this.tTerminalDemand = new Timer((double)this.iTerminalDemand);
				this.tTerminalDemand.Elapsed += new ElapsedEventHandler(this.tTerminalDemand_Elapsed);
				this.tTerminalDemand.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("JTBTerminalDemand", "start", string.Concat("启动定时下发终端点播失败", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.tTerminalDemand.Stop();
		}

		private void tTerminalDemand_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tTerminalDemand.Enabled = false;
			try
			{
				try
				{
					DataTable configInfo = this.getConfigInfo();
					this.clearDownList(configInfo);
					DateTime now = DateTime.Now;
					if (configInfo != null && configInfo.Rows.Count > 0)
					{
						foreach (DataRow row in configInfo.Rows)
						{
							string str = "";
							string str1 = row["CarID"].ToString();
							try
							{
								str = row["MsgContent"].ToString();
							}
							catch
							{
								str = row["MsgName"].ToString();
							}
							byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(str);
							if (bytes == null)
							{
								continue;
							}
							StringBuilder stringBuilder = new StringBuilder();
							string str2 = "";
							for (int i = 0; i < (int)bytes.Length; i++)
							{
								stringBuilder.Append(bytes[i].ToString("X2"));
							}
							str2 = stringBuilder.ToString();
							XmlDocument xmlDocument = this.SetXML(row["SimNum"].ToString(), row["MsgId"].ToString(), str2);
							if (xmlDocument == null || !this.IsDown(row["SimNum"].ToString(), row["MsgId"].ToString(), now, row["Param"].ToString()))
							{
								continue;
							}
							this.myDownData.icar_SetCmdXML(this.OrderId, row["SimNum"].ToString(), this.CarType, this.CmdCode, xmlDocument.OuterXml, this.CommFlag, str1);
						}
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("JTBTerminalDemand", "tTerminalDemand_Elapsed", string.Concat("定时下发终端点播失败", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tTerminalDemand.Enabled = true;
			}
		}
	}
}