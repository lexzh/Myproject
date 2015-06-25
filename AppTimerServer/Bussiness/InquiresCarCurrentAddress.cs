using GisServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Timers;
using ParamLibrary.CmdParamInfo;
using Library;
using ParamLibrary.Application;

namespace Bussiness
{
	public class InquiresCarCurrentAddress : ProcessBase
	{
		private DownData myDownData = new DownData(0);

		private TrafficRawPackage trafficRawPackage = new TrafficRawPackage();

		private Timer tSendCurrentAddress;

		private int iSendCurrentAddress = ReadDataFromXml.iCurrentAddressInterval * 1000;

		private Dictionary<string, string> dFailed = new Dictionary<string, string>();

		private DateTime dtReadTime = DateTime.Now;

		public InquiresCarCurrentAddress()
		{
		}

		public string GetGB2312(string source)
		{
			string empty = string.Empty;
			byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(source);
			for (int i = 0; i < (int)bytes.Length; i++)
			{
				byte num = bytes[i];
				empty = string.Concat(empty, num.ToString("X2"));
			}
			return empty;
		}

		private DataTable getLonLatInfo()
		{
			DataTable dataTable;
			try
			{
				DateTime now = DateTime.Now;
				string str = " select a.simnum, a.propertyData, b.Carid from GpsOutEquipmentHistory_buff a inner join giscar b on b.simnum = a.simNum where a.Equipmentid = 36 and a.InsTime between '{0}' and '{1}' UNION ALL select c.simnum, c.propertyData, d.carid from GpsOutEquipmentHistory c inner join giscar d on c.simnum = d.simNum where c.Equipmentid = 36 and c.InsTime between '{0}' and '{1}' ";
				DataTable dataBySql = SqlDataAccess.getDataBySql(string.Format(str, this.dtReadTime, now));
				this.dtReadTime = now;
				dataTable = dataBySql;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("InquiresCarCurrentAddress", "getLonLatInfo", string.Concat("获取经纬度信息异常，", exception.Message));
				this.logHelper.WriteError(errorMsg);
				dataTable = null;
			}
			return dataTable;
		}

		private void SendFailedInfo()
		{
			try
			{
				if (this.dFailed.Keys.Count > 0)
				{
					foreach (string key in this.dFailed.Keys)
					{
						string[] strArrays = this.dFailed[key].Split(new char[] { ',' });
						string str = strArrays[0];
						string str1 = strArrays[1];
						string str2 = strArrays[2];
						string str3 = ReadDataFromGis.QueryAllLayerByPoint(str, str1);
						if (!string.IsNullOrEmpty(str3))
						{
							this.SendMsg(key, str3, str2);
						}
						else
						{
							ErrorMsg errorMsg = new ErrorMsg("InquiresCarCurrentAddress", "tSendCurrentAddress_Elapsed", string.Concat("地址解析返回为空，", str, ",", str1));
							this.logHelper.WriteError(errorMsg);
						}
					}
					this.dFailed.Clear();
				}
			}
			catch (Exception exception)
			{
				ErrorMsg errorMsg1 = new ErrorMsg("InquiresCarCurrentAddress", "SendFailedInfo", exception.Message);
				this.logHelper.WriteError(errorMsg1);
			}
		}

		private bool SendMsg(string simnum, string Msg, string CarID)
		{
			this.trafficRawPackage.OrderCode = CmdParam.OrderCode.命令透传;
			this.trafficRawPackage.SubOrderCode = CmdParam.OrderCode.下发终端详细地址信息;
			this.trafficRawPackage.strText = this.GetGB2312(Msg);
			if (this.myDownData.icar_SendRawPackage(CmdParam.ParamType.SimNum, simnum, "", CmdParam.CommMode.未知方式, this.trafficRawPackage, CarID) != (long)0)
			{
				return false;
			}
			return true;
		}

		private void SendNewInfo()
		{
			try
			{
				DataTable lonLatInfo = this.getLonLatInfo();
				if (lonLatInfo != null && lonLatInfo.Rows.Count > 0)
				{
					foreach (DataRow row in lonLatInfo.Rows)
					{
						try
						{
							string str = row["simnum"].ToString();
							string str1 = row["propertyData"].ToString();
							string str2 = row["CarId"].ToString();
							if (str1.Substring(0, 4).Equals("4182"))
							{
								if (str1.Substring(4, 4).Equals("0008"))
								{
									string str3 = str1.Substring(8, 8);
									string str4 = str1.Substring(16, 8);
									double num = (double)Convert.ToInt32(str3, 16) * 1E-06;
									string str5 = num.ToString();
									double num1 = (double)Convert.ToInt32(str4, 16) * 1E-06;
									string str6 = num1.ToString();
									string str7 = ReadDataFromGis.QueryAllLayerByPoint(str5, str6);
									if (string.IsNullOrEmpty(str7))
									{
										this.dFailed[str] = string.Concat(str5, ",", str6);
										ErrorMsg errorMsg = new ErrorMsg("InquiresCarCurrentAddress", "tSendCurrentAddress_Elapsed", string.Concat("地址解析返回为空，", str5, ",", str6));
										this.logHelper.WriteError(errorMsg);
									}
									else if (!this.SendMsg(str, str7, str2))
									{
										Dictionary<string, string> strs = this.dFailed;
										string[] strArrays = new string[] { str5, ",", str6, ",", str2 };
										strs[str] = string.Concat(strArrays);
									}
								}
								else
								{
									ErrorMsg errorMsg1 = new ErrorMsg("InquiresCarCurrentAddress", "tSendCurrentAddress_Elapsed", string.Concat("详细位置解析长度不正确，simnum:", str));
									this.logHelper.WriteError(errorMsg1);
								}
							}
						}
						catch (Exception exception)
						{
							ErrorMsg errorMsg2 = new ErrorMsg("InquiresCarCurrentAddress", "tSendCurrentAddress_Elapsed", string.Concat("详细位置解析异常，", row["propertyData"].ToString()));
							this.logHelper.WriteError(errorMsg2);
						}
					}
				}
			}
			catch (Exception exception1)
			{
				ErrorMsg errorMsg3 = new ErrorMsg("InquiresCarCurrentAddress", "SendNewInfo", exception1.Message);
				this.logHelper.WriteError(errorMsg3);
			}
		}

		public override void start()
		{
			try
			{
				this.tSendCurrentAddress = new Timer(10000);
				this.tSendCurrentAddress.Elapsed += new ElapsedEventHandler(this.tSendCurrentAddress_Elapsed);
				this.tSendCurrentAddress.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("InquiresCarCurrentAddress", "start", string.Concat("启动查询车辆当前地址信息失败", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.tSendCurrentAddress.Stop();
		}

		private void tSendCurrentAddress_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tSendCurrentAddress.Enabled = false;
			this.tSendCurrentAddress.Interval = (double)this.iSendCurrentAddress;
			try
			{
				try
				{
					this.SendFailedInfo();
					this.SendNewInfo();
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("InquiresCarCurrentAddress", "tSendCurrentAddress_Elapsed", string.Concat("查询车辆当前地址", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tSendCurrentAddress.Enabled = true;
			}
		}
	}
}