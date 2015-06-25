using GisServices;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Timers;
using Library;
using ParamLibrary.CmdParamInfo;
using ParamLibrary.Application;

namespace Bussiness
{
	public class CuffTimer : ProcessBase
	{
		private DownData myDownData = new DownData(0);

		private Timer tCuffTime;

		private int iCuffTime = 60000 * ReadDataFromXml.CuffDiff;

		private string sPreTime = ReadDataFromXml.CuffBeginTime;

		public CuffTimer()
		{
		}

		private DataTable execCuffNotice(string sBeginTime)
		{
			DataTable dataBySP;
			try
			{
				string str = "GpsPicServer_ExecCuffNoticeEx";
				SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@BgnTime", sBeginTime) };
				dataBySP = SqlDataAccess.getDataBySP(str, sqlParameter);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "CuffTimer",
					FunctionName = "execCuffNotice",
					ErrorText = string.Concat("获取掉线通知数据时发生错误!", exception.Message)
				};
				this.logHelper.WriteError(errorMsg, exception);
				dataBySP = null;
			}
			return dataBySP;
		}

		private void execSendMsg(DataTable dtSendCar)
		{
			LogMsg logMsg = new LogMsg();
			if (string.IsNullOrEmpty(ReadDataFromXml.SendMsg))
			{
				logMsg.Msg = "短息内容为空，不下发短信通知。";
				this.logHelper.WriteLog(logMsg);
				return;
			}
			if (dtSendCar == null || dtSendCar.Rows.Count <= 0)
			{
				logMsg.Msg = "未获取到掉线数据 ";
				this.logHelper.WriteLog(logMsg);
				return;
			}
			int count = dtSendCar.Rows.Count;
			logMsg.Msg = string.Concat("获取到掉线数据 ", count.ToString(), " 条");
			this.logHelper.WriteLog(logMsg);
			logMsg.Msg = "开始发送短信通信通知...";
			this.logHelper.WriteLog(logMsg);
			foreach (DataRow row in dtSendCar.Rows)
			{
				string sendMsg = ReadDataFromXml.SendMsg;
				bool flag = sendMsg.IndexOf("(C)") >= 0;
				string str = row["StarCondition"].ToString();
				string str1 = row["Longitude"].ToString();
				string str2 = row["Latitude"].ToString();
				string str3 = row["CarNum"].ToString();
				string str4 = row["CarId"].ToString();
				string str5 = row["SimNum"].ToString();
				string str6 = row["OfflineSMSUsers"].ToString();
				string str7 = "";
				string str8 = "";
				TxtMsg txtMsg = new TxtMsg();
				try
				{
					str1 = str1.Substring(0, str1.IndexOf('.') + 7);
					str2 = str2.Substring(0, str2.IndexOf('.') + 7);
				}
				catch
				{
				}
				if (flag)
				{
					str7 = ReadDataFromGis.QueryAllLayerByPoint(str1, str2);
				}
				str8 = (str != "1" ? "未定位" : "定位");
				sendMsg = sendMsg.Replace("|", " ");
				sendMsg = sendMsg.Replace("(A)", str3);
				DateTime now = DateTime.Now;
				sendMsg = sendMsg.Replace("(B)", now.ToString("yyyyMMdd HH:mm:ss"));
				sendMsg = sendMsg.Replace("(C)", str7);
				sendMsg = sendMsg.Replace("(D)", string.Format("{0},{1}", str1, str2));
				sendMsg = sendMsg.Replace("(E)", "掉线报警");
				sendMsg = sendMsg.Replace("(F)", str8);
				txtMsg.strMsg = sendMsg;
				txtMsg.MsgType = CmdParam.MsgType.掉线短信通知;
				txtMsg.OrderCode = CmdParam.OrderCode.调度;
				txtMsg.CarId = str4;
				txtMsg.SimNum = str5;
				char[] chrArray = new char[] { ';' };
				string[] strArrays = str6.Trim(chrArray).Split(new char[] { ';' });
				for (int i = 0; i < (int)strArrays.Length; i++)
				{
					string str9 = strArrays[i];
					if (!string.IsNullOrEmpty(str9))
					{
						this.myDownData.icar_SendTxtMsg(CmdParam.ParamType.SimNum, str9, "", CmdParam.CommMode.短信, txtMsg, 0, "掉线报警通知");
					}
				}
			}
		}

		private void onCuffTimerMain(object sender, ElapsedEventArgs e)
		{
			this.tCuffTime.Enabled = false;
			try
			{
				try
				{
					DataTable dataTable = this.execCuffNotice(this.sPreTime);
					if (ReadDataFromXml.IsSendMsg)
					{
						this.execSendMsg(dataTable);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("Service", "onCuffTimerMain", string.Concat("获取掉线通知数据", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tCuffTime.Enabled = true;
			}
		}

		public override void start()
		{
			try
			{
				this.tCuffTime = new Timer((double)this.iCuffTime);
				this.tCuffTime.Elapsed += new ElapsedEventHandler(this.onCuffTimerMain);
				this.tCuffTime.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				this.tCuffTime.Enabled = true;
				ErrorMsg errorMsg = new ErrorMsg("CuffTimer", "start", string.Concat("启动掉线通知失败", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.tCuffTime.Stop();
		}
	}
}