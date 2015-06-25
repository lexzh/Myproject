

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Timers;
using Library;
using ParamLibrary.CmdParamInfo;
using ParamLibrary.Application;

namespace Bussiness
{
	public class TerminalOffLineMessageRemind : ProcessBase
	{
		private System.Timers.Timer tTimer1;

		private System.Timers.Timer tTimer2;

		private int MessageRemindInterval1 = 3600000;

		private int MessageRemindInterval2 = 3600000;

		private DownData myDownData = new DownData(0);

		private string WarnMsg1 = ReadDataFromXml.WarnMsg1;

		private string WarnMsg2 = ReadDataFromXml.WarnMsg2;

		private string WarnMsg3 = ReadDataFromXml.WarnMsg3;

		private int appointedTime = 60 * ReadDataFromXml.iAppointedTime;

		private int terminalTypeID = ReadDataFromXml.iTerminalTypeID;

		private List<string> ltMsgSended = new List<string>();

		private List<string> NewsSMSFailure = new List<string>();

		private DateTime dtTime
		{
			get
			{
				DateTime dateTime;
				try
				{
					dateTime = Convert.ToDateTime(FileHelper.ReadXmlEveryOne("TerminalOffLineLastTime"));
				}
				catch (Exception exception)
				{
					dateTime = DateTime.Now;
				}
				return dateTime;
			}
			set
			{
				FileHelper.setConfig("TerminalOffLineLastTime", value.ToString());
			}
		}

		private bool IsSent
		{
			get
			{
				bool flag;
				try
				{
					flag = Convert.ToBoolean(FileHelper.ReadXmlEveryOne("TerminalOffLineIsSent"));
				}
				catch (Exception exception)
				{
					flag = false;
				}
				return flag;
			}
			set
			{
				FileHelper.setConfig("TerminalOffLineIsSent", value.ToString());
			}
		}

		public TerminalOffLineMessageRemind()
		{
		}

		private void clearHtCarsInfo(DataTable dtSendCars)
		{
			try
			{
				List<string> strs = new List<string>();
				foreach (string str in this.ltMsgSended)
				{
					if ((int)dtSendCars.Select(string.Concat("SimNum = '", str, "'")).Length > 0)
					{
						continue;
					}
					strs.Add(str);
				}
				foreach (string str1 in strs)
				{
					this.ltMsgSended.Remove(str1);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("TerminalOffLineMessageRemind", "clearHtCarsInfo", "")
				{
					ErrorText = exception.Message
				};
				this.logHelper.WriteError(errorMsg);
			}
		}

		private string GetAssemblyPath()
		{
			string codeBase = Assembly.GetExecutingAssembly().CodeBase;
			codeBase = codeBase.Substring(8, codeBase.Length - 8);
			string[] strArrays = codeBase.Split(new char[] { '/' });
			string str = "";
			for (int i = 0; i < (int)strArrays.Length - 1; i++)
			{
				str = string.Concat(str, strArrays[i], "/");
			}
			return str;
		}

		private DataTable getNewCustomers()
		{
			DataTable dataTable;
			try
			{
				DateTime now = DateTime.Now;
				string str = " exec GpsPicServer_getNewCustomers '{0}', '{1}' ";
				DataTable dataBySql = SqlDataAccess.getDataBySql(string.Format(str, this.dtTime, now));
				this.dtTime = now;
				dataTable = dataBySql;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("TerminalOffLineMessageRemind", "getNewCustomers", "")
				{
					ErrorText = exception.Message
				};
				this.logHelper.WriteError(errorMsg);
				dataTable = null;
			}
			return dataTable;
		}

		private DataTable getSendCars()
		{
			DataTable dataTable;
			try
			{
				string str = " exec GpsPicServer_getSendCars {0}, {1}";
				DataTable dataBySql = SqlDataAccess.getDataBySql(string.Format(str, this.terminalTypeID, this.appointedTime));
				dataTable = dataBySql;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("TerminalOffLineMessageRemind", "getSendCars", "")
				{
					ErrorText = exception.Message
				};
				this.logHelper.WriteError(errorMsg);
				dataTable = null;
			}
			return dataTable;
		}

		private List<string> getSendedMsg()
		{
			List<string> strs = new List<string>();
			try
			{
				string str = string.Concat(this.GetAssemblyPath(), "/SendedMsg.txt");
				if ((new FileInfo(string.Concat(this.GetAssemblyPath(), "/SendedMsg.txt"))).Exists)
				{
					StreamReader streamReader = new StreamReader(str);
					string end = streamReader.ReadToEnd();
					streamReader.Close();
					char[] chrArray = new char[] { ',' };
					strs = end.Split(chrArray, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("TerminalOffLineMessageRemind", "getSendedMsg", "")
				{
					ErrorText = exception.Message
				};
				this.logHelper.WriteError(errorMsg);
			}
			return strs;
		}

		private void MsgRemind1()
		{
			try
			{
				DataTable sendCars = this.getSendCars();
				if (sendCars != null && sendCars.Rows.Count > 0)
				{
					this.clearHtCarsInfo(sendCars);
					this.SendWarnMsg(sendCars);
				}
				else if (sendCars != null)
				{
					this.ltMsgSended.Clear();
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("TerminalOffLineMessageRemind", "MsgRemind1", "")
				{
					ErrorText = exception.Message
				};
				this.logHelper.WriteError(errorMsg);
			}
		}

		private void MsgRemind2()
		{
			try
			{
				DataTable sendCars = this.getSendCars();
				if (sendCars != null && sendCars.Rows.Count > 0)
				{
					foreach (DataRow row in sendCars.Rows)
					{
						string str = row["CarId"].ToString();
						string str1 = row["simnum"].ToString();
						string str2 = this.WarnMsg2.Replace("[A]", row["carNum"].ToString());
						DateTime date = DateTime.Now.Date;
						this.SendMsg(str, str1, str2.Replace("[B]", date.ToString()), "每月一号检测车卫士上线情况并通知用户");
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("TerminalOffLineMessageRemind", "MsgRemind2", "")
				{
					ErrorText = exception.Message
				};
				this.logHelper.WriteError(errorMsg);
			}
		}

		private void saveSendedMsg()
		{
			try
			{
				string str = string.Concat(this.GetAssemblyPath(), "/SendedMsg.txt");
				FileInfo fileInfo = new FileInfo(string.Concat(this.GetAssemblyPath(), "/SendedMsg.txt"));
				int num = 0;
				while (fileInfo.Exists && num < 5)
				{
					try
					{
						fileInfo.Delete();
						num++;
					}
					catch (Exception exception1)
					{
						Exception exception = exception1;
						ErrorMsg errorMsg = new ErrorMsg("TerminalOffLineMessageRemind", "saveSendedMsg", "")
						{
							ErrorText = string.Concat("删除已发送车辆记录文件异常-", num, exception.Message)
						};
						this.logHelper.WriteError(errorMsg);
						Thread.Sleep(10000);
					}
				}
				if (this.ltMsgSended.Count > 0)
				{
					string str1 = "";
					foreach (string str2 in this.ltMsgSended)
					{
						str1 = string.Concat(str1, str2, ",");
					}
					StreamWriter streamWriter = new StreamWriter(str);
					char[] chrArray = new char[] { ',' };
					streamWriter.Write(str1.Trim(chrArray));
					streamWriter.Close();
					this.ltMsgSended.Clear();
					this.ltMsgSended = null;
				}
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				ErrorMsg errorMsg1 = new ErrorMsg("TerminalOffLineMessageRemind", "saveSendedMsg", "")
				{
					ErrorText = exception2.Message
				};
				this.logHelper.WriteError(errorMsg1);
			}
		}

		private long SendMsg(string carId, string simnum, string MsgContent, string MsgType)
		{
			long num;
			try
			{
				TxtMsg txtMsg = new TxtMsg()
				{
					strMsg = MsgContent,
					MsgType = CmdParam.MsgType.UCS2手机短信,
					OrderCode = CmdParam.OrderCode.调度,
					CarId = carId,
					SimNum = simnum
				};
				long num1 = this.myDownData.icar_SendTxtMsg(CmdParam.ParamType.SimNum, simnum, "", CmdParam.CommMode.短信, txtMsg, 0, MsgType);
				num = num1;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("TerminalOffLineMessageRemind", "SendMsg", "")
				{
					ErrorText = exception.Message
				};
				this.logHelper.WriteError(errorMsg);
				num = (long)-1;
			}
			return num;
		}

		private void SendWarnMsg(DataTable dtSendCars)
		{
			try
			{
				foreach (DataRow row in dtSendCars.Rows)
				{
					if (this.ltMsgSended.Contains(row["Simnum"].ToString()) || this.SendMsg(row["CarId"].ToString(), row["SimNum"].ToString(), this.WarnMsg1.Replace("[A]", row["CarNum"].ToString()), "48小时未上线短信提醒") != (long)0)
					{
						continue;
					}
					this.ltMsgSended.Add(row["SimNum"].ToString());
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("TerminalOffLineMessageRemind", "SendWarnMsg", "")
				{
					ErrorText = exception.Message
				};
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void start()
		{
			try
			{
				this.tTimer1 = new System.Timers.Timer(1000);
				this.tTimer1.Elapsed += new ElapsedEventHandler(this.tTimer1_Elapsed);
				this.tTimer1.Enabled = true;
				this.tTimer2 = new System.Timers.Timer(10000);
				this.tTimer2.Elapsed += new ElapsedEventHandler(this.tTimer2_Elapsed);
				this.tTimer2.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("TerminalOffLineMessageRemind", "start", string.Concat(",", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.tTimer1.Stop();
			this.tTimer2.Stop();
		}

		private void tTimer1_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tTimer1.Enabled = false;
			this.tTimer1.Interval = (double)this.MessageRemindInterval1;
			if (DateTime.Now.Day != 1 || DateTime.Now.Hour != 9 || this.IsSent)
			{
				this.ltMsgSended = this.getSendedMsg();
				this.MsgRemind1();
				this.saveSendedMsg();
			}
			else
			{
				this.MsgRemind2();
				this.IsSent = true;
			}
			if (DateTime.Now.Hour != 9 && this.IsSent)
			{
				this.IsSent = false;
			}
			this.tTimer1.Enabled = true;
		}

		private void tTimer2_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tTimer2.Enabled = false;
			this.tTimer2.Interval = (double)this.MessageRemindInterval2;
			try
			{
				try
				{
					DataTable newCustomers = this.getNewCustomers();
					if (newCustomers != null && newCustomers.Rows.Count > 0)
					{
						foreach (DataRow row in newCustomers.Rows)
						{
							this.SendMsg("0", row["SimNum"].ToString(), this.WarnMsg3, "新开户车卫士用户提醒");
						}
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("TerminalOffLineMessageRemind", "tTimer2_Elapsed", "")
					{
						ErrorText = exception.Message
					};
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tTimer2.Enabled = true;
			}
		}
	}
}