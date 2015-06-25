

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Timers;
using ParamLibrary.CmdParamInfo;
using Library;
using ParamLibrary.Application;

namespace Bussiness
{
	public class LCSTimer : ProcessBase
	{
		private DownData myDownData = new DownData(0);

		private List<string> LCSList = new List<string>();

		private Timer tLCSPosTimer;

		private int iLCSTime = 60000 * ReadDataFromXml.LCSTime;

		private PosReport m_PosReport = new PosReport();

		private int iLCSPosTime = ReadDataFromXml.LCSPosTime;

		private DateTime dLCSPosTime;

		public LCSTimer()
		{
		}

		private void addLCSTime()
		{
			try
			{
				this.dLCSPosTime = this.dLCSPosTime.AddMinutes((double)this.iLCSPosTime);
			}
			catch
			{
			}
		}

		private void getLBSPosParam()
		{
			this.m_PosReport.OrderCode = CmdParam.OrderCode.位置查询;
			this.m_PosReport.CompressionUpTime = 0;
			this.m_PosReport.isCompressed = CmdParam.IsCompressed.单次传送;
			this.m_PosReport.LowReportCycle = 60;
			this.m_PosReport.ReportCycle = 60;
			this.m_PosReport.ReportTiming = 1;
			this.m_PosReport.ReportWhenStop = CmdParam.ReportWhenStop.汇报;
			this.m_PosReport.ReportType = CmdParam.ReportType.定次汇报;
		}

		private int GetLCSPosData(string sLCSDay, string sLCSTime, out DataTable dtLCSData)
		{
			int num;
			try
			{
				string str = "GpsPicServer_GetLCSPosData";
				string lBSType = ReadDataFromXml.LBSType;
				SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@LCSDay", sLCSDay), new SqlParameter("@LCSTime", sLCSTime) };
				dtLCSData = SqlDataAccess.getDataBySP(str, sqlParameter);
				return 0;
			}
			catch (Exception exception)
			{
				ErrorMsg errorMsg = new ErrorMsg("LCSTimer", "GetLCSPosData", exception.Message);
				this.logHelper.WriteError(errorMsg);
				dtLCSData = null;
				num = -1;
			}
			return num;
		}

		private void InitLCSTime()
		{
			DateTime now = DateTime.Now;
			int minute = this.iLCSPosTime - now.Minute % this.iLCSPosTime;
			this.dLCSPosTime = now.AddMinutes((double)minute);
		}

		private void LCSPosTimer()
		{
			ErrorMsg errorMsg = new ErrorMsg("service", "tLCSPosTimer_Elapsed", "");
			LogMsg logMsg = new LogMsg("service", "tLCSPosTimer_Elapsed", "");
			this.getLBSPosParam();
			string empty = string.Empty;
			int num = 0;
			long num1 = (long)0;
			int num2 = 0;
			while (this.LCSList.Count > 0 && num2 < this.LCSList.Count)
			{
				string[] strArrays = this.LCSList[num2].ToString().Split(new char[] { '|' });
				empty = strArrays[0];
				num = Convert.ToInt32(strArrays[1]);
				if (this.myDownData.iCar_SetPosReport(CmdParam.ParamType.SimNum, empty, "", CmdParam.CommMode.未知方式, this.m_PosReport, num) != (long)-1)
				{
					logMsg.Msg = string.Concat("发送LCS失败队列中的车辆成功，SIM卡：", empty);
					this.logHelper.WriteLog(logMsg);
					this.LCSList.RemoveAt(num2);
				}
				else
				{
					num2++;
					errorMsg.ErrorText = string.Concat("LCS定位,下发失败队列中的车辆失败,SIM卡：", empty);
					this.logHelper.WriteError(errorMsg);
				}
			}
			if (this.LCSList.Count == 0)
			{
				this.LCSList.Clear();
			}
			while (DateTime.Now > this.dLCSPosTime)
			{
				DateTime dateTime = this.dLCSPosTime;
				DataTable dataTable = new DataTable();
				int lCSPosData = this.GetLCSPosData(dateTime.ToString("yyyy/MM/dd"), dateTime.ToString("HH:mm"), out dataTable);
				if (lCSPosData == 0)
				{
					this.addLCSTime();
				}
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					foreach (DataRow row in dataTable.Rows)
					{
						empty = row["SimNum"].ToString();
						num = Convert.ToInt32(row["CarId"]);
						if (this.myDownData.iCar_SetPosReport(CmdParam.ParamType.SimNum, empty, "", CmdParam.CommMode.未知方式, this.m_PosReport, num) != (long)-1)
						{
							logMsg.Msg = string.Concat("下发LCS定位成功，SIM卡：", empty);
							this.logHelper.WriteLog(logMsg);
						}
						else
						{
							if (this.LCSList.Contains(empty))
							{
								errorMsg.ErrorText = string.Concat("LCS定位,下发指令失败,失败队列中已存在该车，上次定位丢失，SIM卡：", empty);
							}
							else if (this.LCSList.Count < ReadDataFromXml.LCSCount)
							{
								this.LCSList.Add(string.Concat(empty, "|", num));
								errorMsg.ErrorText = string.Concat("LCS定位,下发指令失败,加入到失败队列中等待重新下发，SIM卡：", empty);
							}
							else
							{
								errorMsg.ErrorText = string.Concat("LCS定位,下发指令失败,队列已满，未加入到失败队列中，SIM卡：", empty);
							}
							this.logHelper.WriteError(errorMsg);
						}
					}
				}
				if (lCSPosData == 0)
				{
					continue;
				}
				return;
			}
		}

		public override void start()
		{
			try
			{
				this.InitLCSTime();
				this.tLCSPosTimer = new Timer((double)this.iLCSTime);
				this.tLCSPosTimer.Elapsed += new ElapsedEventHandler(this.tLCSPosTimer_Elapsed);
				this.tLCSPosTimer.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("LCSTimer", "start", string.Concat("启动LCS定位服务失败", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.tLCSPosTimer.Stop();
		}

		private void tLCSPosTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tLCSPosTimer.Enabled = false;
			try
			{
				try
				{
					this.LCSPosTimer();
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("LCSTimer", "tLBSPosTimer_Elapsed", string.Concat("LCS定位异常", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tLCSPosTimer.Enabled = true;
			}
		}
	}
}