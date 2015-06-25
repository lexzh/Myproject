using System;
using System.Collections;
using System.Data;
using System.Timers;
using Library;

namespace Bussiness
{
	public class JTBOnOffLineNotice : ProcessBase
	{
		private Timer OffLineNotice;

		private Timer OnLineNotice;

		private int NoticeInterval = ReadDataFromXml.JTBOnOffInterval * 1000;

		private int OffLineTime = ReadDataFromXml.JTBOffLineTime;

		private DateTime dtNow = ReadDataFromDB.GetSvrTime();

		private DateTime dtServiceStart = ReadDataFromDB.GetSvrTime();

		private DateTime dtOffLineTime = ReadDataFromDB.GetSvrTime();

		public JTBOnOffLineNotice()
		{
		}

		private DataTable getOffLineData2()
		{
			DataTable dataTable;
			try
			{
				DateTime svrTime = ReadDataFromDB.GetSvrTime();
				string str = " exec GpsPicServer_ExecCuffNoticeJTB '{0}', '{1}' ";
				DataTable dataBySql = SqlDataAccess.getDataBySql(string.Format(str, this.dtOffLineTime.ToString(), svrTime));
				this.dtOffLineTime = svrTime;
				dataTable = dataBySql;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("JTBOnOffLineNotice", "getOffLineData2", string.Concat("获取掉线车辆信息错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				dataTable = null;
			}
			return dataTable;
		}

		private DataTable getOnLineData()
		{
			DataTable dataTable;
			try
			{
				DateTime svrTime = ReadDataFromDB.GetSvrTime();
				string str = " exec GpsPicServer_ExecOnLineNoticeJTB '{0}', '{1}' ";
				DataTable dataBySql = SqlDataAccess.getDataBySql(string.Format(str, this.dtNow.ToString(), svrTime.ToString()));
				this.dtNow = svrTime;
				dataTable = dataBySql;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("JTBOnOffLineNotice", "getOnLineData", string.Concat("获取上线车辆信息错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				dataTable = null;
			}
			return dataTable;
		}

		private void OffLineNotice_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.OffLineNotice.Enabled = false;
			try
			{
				try
				{
					DataTable offLineData2 = this.getOffLineData2();
					LogMsg logMsg = new LogMsg("JTBOnOffLineNotice", "OffLineNotice_Elapsed", "");
					if (offLineData2 != null && offLineData2.Rows.Count > 0)
					{
						foreach (DataRow row in offLineData2.Rows)
						{
							logMsg.Msg = string.Concat("交通部车辆掉线通知，simnum：", row["simnum"].ToString());
							this.logHelper.WriteLog(logMsg);
						}
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("JTBOnOffLineNotice", "OffLineNotice_Elapsed", string.Concat("检测掉线通知错误,", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.OffLineNotice.Enabled = true;
			}
		}

		private void OnLineNotice_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.OnLineNotice.Enabled = false;
			LogMsg logMsg = new LogMsg("JTBOnOffLineNotice", "OnLineNotice_Elapsed", "");
			try
			{
				try
				{
					DataTable onLineData = this.getOnLineData();
					if (onLineData != null && onLineData.Rows.Count > 0)
					{
						foreach (DataRow row in onLineData.Rows)
						{
							logMsg.Msg = string.Concat("交通部车辆上线通知，simnum：", row["simnum"].ToString());
							this.logHelper.WriteLog(logMsg);
						}
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("JTBOnOffLineNotice", "OnLineNotice_Elapsed", string.Concat("检测上线通知错误,", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.OnLineNotice.Enabled = true;
			}
		}

		public override void start()
		{
			try
			{
				this.OffLineNotice = new Timer((double)this.NoticeInterval);
				this.OffLineNotice.Elapsed += new ElapsedEventHandler(this.OffLineNotice_Elapsed);
				this.OffLineNotice.Enabled = true;
				this.OnLineNotice = new Timer((double)this.NoticeInterval);
				this.OnLineNotice.Elapsed += new ElapsedEventHandler(this.OnLineNotice_Elapsed);
				this.OnLineNotice.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("JTBOnOffLineNotice", "start", string.Concat("开启交通部上下线通知错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.OnLineNotice.Stop();
			this.OffLineNotice.Stop();
		}
	}
}