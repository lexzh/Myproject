using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Timers;
using Library;
using ParamLibrary.CmdParamInfo;
using ParamLibrary.Application;

namespace Bussiness
{
	public class SendPZMessage : ProcessBase
	{
		private Timer tTimer1;

		private DownData myDownData = new DownData(0);

		private string PZMsg = ReadDataFromXml.PZSendMsg;

		private List<string> ltSendFailed = new List<string>();

		private int terminalTypeID = ReadDataFromXml.iPZType;

		private int SendHour = ReadDataFromXml.iPZInterval;

		private DateTime dtTime
		{
			get
			{
				DateTime dateTime;
				try
				{
					dateTime = Convert.ToDateTime(FileHelper.ReadXmlEveryOne("SendPZMessageLastTime"));
				}
				catch (Exception exception)
				{
					dateTime = DateTime.MinValue;
				}
				return dateTime;
			}
			set
			{
				FileHelper.setConfig("SendPZMessageLastTime", value.ToString());
			}
		}

		public SendPZMessage()
		{
		}

		private DataTable getCars()
		{
			DataTable dataTable;
			try
			{
				string str = "exec GpsPicServer_getSendPZCars {0}";
				DataTable dataBySql = SqlDataAccess.getDataBySql(string.Format(str, this.terminalTypeID));
				dataTable = dataBySql;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("SendPZMessage", "getCars", string.Concat(",", exception.Message));
				this.logHelper.WriteError(errorMsg);
				dataTable = null;
			}
			return dataTable;
		}

		private void SendFailedMsg(DataTable dtInfo)
		{
			try
			{
				try
				{
					foreach (string str in this.ltSendFailed)
					{
						char[] chrArray = new char[] { '|' };
						string[] strArrays = str.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
						string str1 = strArrays[0];
						string str2 = strArrays[1];
						if ((int)dtInfo.Select(string.Concat("CarId = '", str2, "'")).Length <= 0)
						{
							continue;
						}
						this.SendMsg(str2, str1, this.PZMsg.Replace("[A]", str1), "平台发送配置短信");
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("SendPZMessage", "SendFailedMsg", string.Concat(",", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.ltSendFailed.Clear();
			}
		}

		private void SendMsg(DataTable dtInfo)
		{
			try
			{
				foreach (DataRow row in dtInfo.Rows)
				{
					if (this.SendMsg(row["CarId"].ToString(), row["SimNum"].ToString(), this.PZMsg.Replace("[A]", row["SimNum"].ToString()), "平台发送配置短信") == (long)0)
					{
						continue;
					}
					this.ltSendFailed.Add(string.Concat(row["SimNum"].ToString(), "|", row["CarId"].ToString()));
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("SendPZMessage", "SendMsg", string.Concat(",", exception.Message));
				this.logHelper.WriteError(errorMsg);
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
				ErrorMsg errorMsg = new ErrorMsg("SendPZMessage", "SendMsg", "")
				{
					ErrorText = exception.Message
				};
				this.logHelper.WriteError(errorMsg);
				num = (long)-1;
			}
			return num;
		}

		public override void start()
		{
			try
			{
				this.tTimer1 = new Timer(1000);
				this.tTimer1.Elapsed += new ElapsedEventHandler(this.tTimer1_Elapsed);
				this.tTimer1.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("SendPZMessage", "start", string.Concat(",", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.tTimer1.Stop();
		}

		private void tTimer1_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tTimer1.Enabled = false;
			this.tTimer1.Interval = 3600000;
			try
			{
				try
				{
					if (DateTime.Now.Date != this.dtTime.Date && DateTime.Now.Hour == this.SendHour)
					{
						DataTable cars = this.getCars();
						if (cars != null && cars.Rows.Count > 0)
						{
							this.SendMsg(cars);
							this.dtTime = DateTime.Now;
						}
					}
					else if (this.ltSendFailed.Count > 0)
					{
						DataTable dataTable = this.getCars();
						if (dataTable == null || dataTable.Rows.Count <= 0)
						{
							this.ltSendFailed.Clear();
						}
						else
						{
							this.SendFailedMsg(dataTable);
						}
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("SendPZMessage", "tTimer1_Elapsed", string.Concat(",", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tTimer1.Enabled = true;
			}
		}
	}
}