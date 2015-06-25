using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Timers;
using Library;
using ParamLibrary.CmdParamInfo;
using ParamLibrary.Application;

namespace Bussiness
{
	public class BroadCastTimer : ProcessBase
	{
		private DownData myDownData = new DownData(0);

		private Timer tBroadCastTimer;

		private Timer tBroadCastUrgentTimer;

		private int iBroadCastTime = 60000 * ReadDataFromXml.BroadCastDiff;

		private TxtMsg txtMsg = new TxtMsg();

		private DateTime BroadPreTime = DateTime.Now;

		private DataTable dtBroadCastUrgent;

		private DataTable dtBroadCastError;

		private DateTime BroadUrgentPreTime = DateTime.Now;

		private Hashtable htBroadCast = new Hashtable();

		private string sPw = "";

		public BroadCastTimer()
		{
		}

		private void AddBroadCastError(DataRow myRow)
		{
			try
			{
				if (this.dtBroadCastError == null)
				{
					this.dtBroadCastError = myRow.Table.Clone();
				}
				this.dtBroadCastError.Rows.Add(myRow.ItemArray);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "BussinessProcess",
					FunctionName = "AddBroadCastError",
					ErrorText = string.Concat("加入错误列表发生错误!", exception.ToString())
				};
				(new LogHelper()).WriteError(errorMsg);
			}
		}

		private void AddBroadCastUrgent(DataRow myRow)
		{
			try
			{
				if (this.dtBroadCastUrgent == null)
				{
					this.dtBroadCastUrgent = myRow.Table.Clone();
				}
				this.dtBroadCastUrgent.Rows.Add(myRow.ItemArray);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "BussinessProcess",
					FunctionName = "AddBroadCastUrgent",
					ErrorText = string.Concat("加入紧急播报列表发生错误!", exception.ToString())
				};
				(new LogHelper()).WriteError(errorMsg);
			}
		}

		private void BroadCastErrorMsg()
		{
			try
			{
				DataTable dataTable = new DataTable();
				lock (this.dtBroadCastError)
				{
					dataTable = this.dtBroadCastError.Copy();
					this.dtBroadCastError.Rows.Clear();
					this.dtBroadCastError = null;
				}
				foreach (DataRow row in dataTable.Rows)
				{
					this.SendBroadCast(row);
				}
				dataTable.Rows.Clear();
				dataTable.Dispose();
				dataTable = null;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "BussinessProcess",
					FunctionName = "BroadCastErrorMsg",
					ErrorText = string.Concat("处理发送错误的信息发生错误!", exception.Message)
				};
				(new LogHelper()).WriteError(errorMsg);
			}
		}

		private void BroadCastMsg()
		{
			try
			{
				DataTable msgParamData = this.GetMsgParamData(this.BroadPreTime.ToString());
				if (msgParamData == null || msgParamData.Rows.Count == 0)
				{
					if (this.dtBroadCastUrgent != null && this.dtBroadCastUrgent.Rows.Count > 0)
					{
						this.BroadCastUrgentMsg();
					}
					if (this.dtBroadCastError != null && this.dtBroadCastError.Rows.Count > 0)
					{
						this.BroadCastErrorMsg();
					}
				}
				else
				{
					this.BroadPreTime = DateTime.Parse(msgParamData.Rows[0]["PreTime"].ToString());
					LogMsg logMsg = new LogMsg();
					LogHelper logHelper = new LogHelper();
					foreach (DataRow row in msgParamData.Rows)
					{
						if (this.dtBroadCastUrgent != null && this.dtBroadCastUrgent.Rows.Count > 0)
						{
							this.BroadCastUrgentMsg();
						}
						this.SendBroadCast(row);
					}
					if (this.dtBroadCastError != null && this.dtBroadCastError.Rows.Count > 0)
					{
						this.BroadCastErrorMsg();
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "BussinessProcess",
					FunctionName = "BroadCastMsg",
					ErrorText = string.Concat("处理普通信息播报发生错误!", exception.Message)
				};
				(new LogHelper()).WriteError(errorMsg);
			}
		}

		private void BroadCastUrgentMsg()
		{
			try
			{
				DataTable dataTable = new DataTable();
				lock (this.dtBroadCastUrgent)
				{
					dataTable = this.dtBroadCastUrgent.Copy();
					this.dtBroadCastUrgent.Rows.Clear();
					this.dtBroadCastUrgent = null;
				}
				foreach (DataRow row in dataTable.Rows)
				{
					this.SendBroadCast(row);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "BussinessProcess",
					FunctionName = "BroadCastUrgentMsg",
					ErrorText = string.Concat("处理播报紧急信息发生错误!", exception.Message)
				};
				(new LogHelper()).WriteError(errorMsg);
			}
		}

		private void BroadCastUrgentTimer()
		{
			DataTable broadCastUrgent = null;
			broadCastUrgent = this.getBroadCastUrgent();
			if (broadCastUrgent != null && broadCastUrgent.Rows.Count > 0)
			{
				this.BroadUrgentPreTime = DateTime.Parse(broadCastUrgent.Rows[0]["PreTime"].ToString());
				LogMsg logMsg = new LogMsg();
				LogHelper logHelper = new LogHelper();
				foreach (DataRow row in broadCastUrgent.Rows)
				{
					this.AddBroadCastUrgent(row);
				}
			}
			if (broadCastUrgent != null)
			{
				broadCastUrgent.Rows.Clear();
				broadCastUrgent.Dispose();
				broadCastUrgent = null;
			}
		}

		private DataTable getBroadCastUrgent()
		{
			DataTable urgentMsgParamData;
			try
			{
				urgentMsgParamData = this.GetUrgentMsgParamData(this.BroadUrgentPreTime.ToString());
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "BussinessProcess",
					FunctionName = "getBroadCastUrgent",
					ErrorText = string.Concat("获取播报紧急信息数据发生错误!", exception.Message)
				};
				(new LogHelper()).WriteError(errorMsg);
				return null;
			}
			return urgentMsgParamData;
		}

		private DataTable GetMsgParamData(string sPreTime)
		{
			DataTable dataBySP;
			try
			{
				string str = "GpsPicServer_GetCarMsgParam";
				SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@PreTime", sPreTime), new SqlParameter("@Level", (object)2) };
				dataBySP = SqlDataAccess.getDataBySP(str, sqlParameter);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "BroadCastTimer",
					FunctionName = "GetMsgParamData",
					ErrorText = string.Concat("获取普通播报信息发生错误!", exception.Message)
				};
				(new LogHelper()).WriteError(errorMsg);
				return null;
			}
			return dataBySP;
		}

		private DataTable GetUrgentMsgParamData(string sPreTime)
		{
			DataTable dataBySP;
			try
			{
				string str = "GpsPicServer_GetCarMsgParam";
				SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@PreTime", sPreTime), new SqlParameter("@Level", (object)1) };
				dataBySP = SqlDataAccess.getDataBySP(str, sqlParameter);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "BroadCastTimer",
					FunctionName = "GetUrgentMsgParamData",
					ErrorText = string.Concat("获取紧急播报信息发生错误!", exception.Message)
				};
				(new LogHelper()).WriteError(errorMsg);
				return null;
			}
			return dataBySP;
		}

		private void SendBroadCast(DataRow myRow)
		{
			try
			{
				string str = myRow["SimNum"].ToString();
				DateTime dateTime = Convert.ToDateTime(myRow["BeginTime"]);
				DateTime dateTime1 = Convert.ToDateTime(myRow["EndTime"]);
				string str1 = myRow["MsgContent"].ToString();
				string str2 = myRow["MsgLevel"].ToString();
				int num = int.Parse(myRow["MsgId"].ToString());
				if (dateTime1.CompareTo(DateTime.Now) < 0)
				{
					string str3 = "普通信息";
					if ("1".Equals(str2))
					{
						str3 = "即时信息";
					}
					LogMsg logMsg = new LogMsg()
					{
						FunctionName = "播报信息"
					};
					string[] strArrays = new string[] { "超时：SimNum-", str, ",信息类型-", str3, ",开始时间-", dateTime.ToString("yyyy-MM-dd HH:mm:ss"), "，结束时间-", dateTime1.ToString("yyyy-MM-dd HH:mm:ss"), ",播报时间-", null, null, null, null };
					strArrays[9] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					strArrays[10] = ",播报消息-";
					strArrays[11] = str1;
					strArrays[12] = "\r\n";
					logMsg.Msg = string.Concat(strArrays);
					(new LogHelper()).WriteLog(logMsg);
				}
				else if (this.setParam(str1))
				{
					if (this.htBroadCast.ContainsKey(str))
					{
						if (DateTime.Parse(this.htBroadCast[str].ToString()).AddSeconds((double)ReadDataFromXml.BroadCastMsgTime).CompareTo(DateTime.Now) < 0)
						{
							this.htBroadCast.Remove(str);
						}
						else
						{
							bool flag = true;
							if (this.dtBroadCastError != null)
							{
								foreach (DataRow row in this.dtBroadCastError.Rows)
								{
									if (!(row["SimNum"].ToString() == myRow["SimNum"].ToString()) || !(row["BeginTime"].ToString() == myRow["BeginTime"].ToString()) || !(row["MsgLevel"].ToString() == myRow["MsgLevel"].ToString()))
									{
										continue;
									}
									flag = false;
									break;
								}
							}
							if (flag)
							{
								if (!"1".Equals(myRow["MsgLevel"].ToString()))
								{
									this.AddBroadCastError(myRow);
								}
								else
								{
									this.AddBroadCastUrgent(myRow);
								}
							}
							return;
						}
					}
					if (this.myDownData.icar_SendTxtMsg(CmdParam.ParamType.SimNum, str, this.sPw, CmdParam.CommMode.未知方式, this.txtMsg, num, "播报信息") != (long)0)
					{
						this.AddBroadCastError(myRow);
						LogMsg logMsg1 = new LogMsg()
						{
							FunctionName = "播报信息"
						};
						string[] strArrays1 = new string[] { "失败：SimNum-", str, ",播报消息-", str1, ",开始时间-", dateTime.ToString("yyyy-MM-dd HH:mm:ss"), "，结束时间-", dateTime1.ToString("yyyy-MM-dd HH:mm:ss"), ",播报时间-", null, null };
						strArrays1[9] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						strArrays1[10] = "\r\n";
						logMsg1.Msg = string.Concat(strArrays1);
						(new LogHelper()).WriteLog(logMsg1);
					}
					else if (!this.htBroadCast.ContainsKey(str))
					{
						this.htBroadCast.Add(str, DateTime.Now);
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "BussinessProcess",
					FunctionName = "SendBroadCast",
					ErrorText = string.Concat("发送播报信息发生错误!", exception.ToString())
				};
				(new LogHelper()).WriteError(errorMsg);
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
				this.tBroadCastTimer = new Timer((double)this.iBroadCastTime);
				this.tBroadCastTimer.Elapsed += new ElapsedEventHandler(this.tBroadCastTimer_Elapsed);
				this.tBroadCastTimer.Enabled = true;
				this.tBroadCastTimer.Start();
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("Service", "GpsPicMain", string.Concat("启动播报普通信息失败", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
			try
			{
				this.tBroadCastUrgentTimer = new Timer((double)this.iBroadCastTime);
				this.tBroadCastUrgentTimer.Elapsed += new ElapsedEventHandler(this.tBroadCastUrgentTimer_Elapsed);
				this.tBroadCastUrgentTimer.Enabled = true;
				this.tBroadCastUrgentTimer.Start();
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				ErrorMsg errorMsg1 = new ErrorMsg("Service", "GpsPicMain", string.Concat("启动播报紧急信息失败", exception2.Message));
				this.logHelper.WriteError(errorMsg1);
			}
		}

		public override void stop()
		{
			this.tBroadCastTimer.Stop();
			this.tBroadCastUrgentTimer.Stop();
		}

		private void tBroadCastTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tBroadCastTimer.Enabled = false;
			try
			{
				try
				{
					this.BroadCastMsg();
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("Service", "tBroadCastTimer_Elapsed", string.Concat("播报普通信息计时器发生错误", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tBroadCastTimer.Enabled = true;
			}
		}

		private void tBroadCastUrgentTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tBroadCastUrgentTimer.Enabled = false;
			try
			{
				try
				{
					this.BroadCastUrgentTimer();
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("Service", "tBroadCastUrgentTimer_Elapsed", string.Concat("播报紧急信息计时器", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tBroadCastUrgentTimer.Enabled = true;
			}
		}
	}
}