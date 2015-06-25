using System;
using System.Collections;
using System.Data;
using System.Timers;
using ParamLibrary.CmdParamInfo;
using Library;
using ParamLibrary.Application;

namespace Bussiness
{
	public class DWLBSPos : ProcessBase
	{
		private DownData myDownData = new DownData(0);

		private Timer DWLBSPosTimer;

		private int DWLBSPosInterval = 1000 * ReadDataFromXml.DWLBSTime;

		private string dtTime = DateTime.Now.ToString();

		private int DWLBSPosInfoID;

		private int DWLBSPosInfoIDBuff;

		private PosReport m_PosReport = new PosReport();

		public DWLBSPos()
		{
		}

		private void DWLBSPosTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.DWLBSPosTimer.Enabled = false;
			try
			{
				try
				{
					DataTable dWLBSInfo = this.getDWLBSInfo();
					if (dWLBSInfo != null && dWLBSInfo.Rows.Count > 0)
					{
						LogMsg logMsg = new LogMsg("DWLBSPos", "DWLBSPosTimer_Elapsed", "")
						{
							Msg = string.Concat("获取需定位的信息数量为：", dWLBSInfo.Rows.Count)
						};
						this.logHelper.WriteLog(logMsg);
						int num1 = 0;
						foreach (DataRow row in dWLBSInfo.Rows)
						{
							string str = row["SimNum"].ToString();
							int num2 = Convert.ToInt32(row["CarId"]);
							if (this.myDownData.iCar_SetPosReport(CmdParam.ParamType.SimNum, str, "", CmdParam.CommMode.未知方式, this.m_PosReport, num2) != (long)-1)
							{
								num1++;
							}
							else
							{
								ErrorMsg errorMsg = new ErrorMsg("DWLBSPos", "DWLBSPosTimer_Elapsed", string.Concat("通讯返回失败，SIM卡：", str));
								this.logHelper.WriteError(errorMsg);
							}
						}
						logMsg.Msg = string.Concat("实际定位成功的信息数量为：", num1);
						this.logHelper.WriteLog(logMsg);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg1 = new ErrorMsg("DWLBSPos", "DWLBSPosTimer_Elapsed", string.Concat("DWLBS定位失败", exception.Message));
					this.logHelper.WriteError(errorMsg1);
				}
			}
			finally
			{
				this.DWLBSPosTimer.Enabled = true;
			}
		}

		private DataTable getDWLBSInfo()
		{
			DataTable dataTable;
			try
			{
				DataTable dataBySql = new DataTable();
				DataTable dataTable1 = new DataTable();
				string str = " select a.id,a.simNum,a.EquipmentId,a.gpstime,a.recetime,a.AccStatus,a.PropertyData,a.InsTime,b.CarId from GpsOutEquipmentHistory a INNER JOIN GisCar b on a.simNum = b.simNum where a.ID > '{0}' and a.InsTime >= '{1}' and a.EquipmentId = '30' and (a.PropertyData like '6457%' or a.PropertyData like '4457%' or a.PropertyData like '6477%' or a.PropertyData like '4477%') order by a.ID ";
				string str1 = " select a.id,a.simNum,a.EquipmentId,a.gpstime,a.recetime,a.AccStatus,a.PropertyData,a.InsTime,b.CarId from GpsOutEquipmentHistory_buff a INNER JOIN GisCar b on a.simNum = b.simNum where a.ID > '{0}' and a.InsTime >= '{1}' and a.EquipmentId = '30' and (a.PropertyData like '6457%' or a.PropertyData like '4457%' or a.PropertyData like '6477%' or a.PropertyData like '4477%') order by a.ID ";
				DateTime dateTime = DateTime.Now.AddSeconds(-1);
				dataBySql = SqlDataAccess.getDataBySql(string.Format(str, this.DWLBSPosInfoID, this.dtTime.ToString()));
				if (dataBySql != null && dataBySql.Rows.Count > 0)
				{
					this.DWLBSPosInfoID = Convert.ToInt32(dataBySql.Rows[dataBySql.Rows.Count - 1]["id"]);
				}
				dataTable1 = SqlDataAccess.getDataBySql(string.Format(str1, this.DWLBSPosInfoIDBuff, this.dtTime.ToString()));
				if (dataTable1 != null && dataTable1.Rows.Count > 0)
				{
					this.DWLBSPosInfoIDBuff = Convert.ToInt32(dataTable1.Rows[dataTable1.Rows.Count - 1]["id"]);
				}
				this.dtTime = dateTime.ToString();
				if (dataBySql != null && dataTable1 != null)
				{
					object[] objArray = new object[dataTable1.Columns.Count];
					for (int i = 0; i < dataTable1.Rows.Count; i++)
					{
						dataTable1.Rows[i].ItemArray.CopyTo(objArray, 0);
						dataBySql.Rows.Add(objArray);
					}
				}
				dataTable = (dataBySql == null ? dataTable1 : dataBySql);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("DWLBSPos", "getDWLBSInfo", string.Concat("获取DWLBS定位信息失败,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				dataTable = null;
			}
			return dataTable;
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

		public override void start()
		{
			try
			{
				this.DWLBSPosTimer = new Timer((double)this.DWLBSPosInterval);
				this.DWLBSPosTimer.Elapsed += new ElapsedEventHandler(this.DWLBSPosTimer_Elapsed);
				this.DWLBSPosTimer.Enabled = true;
				this.getLBSPosParam();
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("DWLBSPos", "start", string.Concat("启动DWLBS定位服务失败", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.DWLBSPosTimer.Stop();
		}
	}
}