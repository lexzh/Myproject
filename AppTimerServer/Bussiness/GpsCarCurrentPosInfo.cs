using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Timers;
using Library;

namespace Bussiness
{
	public class GpsCarCurrentPosInfo : ProcessBase
	{
		public static Dictionary<int, DataRow> dicCarList = new Dictionary<int, DataRow>();

		private string sql = "select b.carid, a.*\r\n\tFROM GpsCarCurrentPosInfo a WITH(NOLOCK)\r\n\tinner join giscar b on a.telephone = b.simnum\r\n\tWHERE a.LastUpdateTime >= '{0}' and a.LastUpdateTime < '{1}' ";

		private Timer tGetCarCurrentPosInfo;

		private DateTime dtNow = ReadDataFromDB.GetSvrTime();

		public GpsCarCurrentPosInfo()
		{
		}

		private DataTable getCarCurrentPosInfo()
		{
			DataTable dataTable;
			try
			{
				DateTime svrTime = ReadDataFromDB.GetSvrTime();
				DataTable dataBySql = SqlDataAccess.getDataBySql(string.Format(this.sql, this.dtNow, svrTime));
				this.dtNow = svrTime;
				dataTable = dataBySql;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("GpsCarCurrentPosInfo", "getCarCurrentPosInfo", string.Concat("获取末次位置信息错误,", exception.Message));
				this.logHelper.WriteError(errorMsg);
				dataTable = null;
			}
			return dataTable;
		}

		public override void start()
		{
			this.tGetCarCurrentPosInfo = new Timer();
			this.tGetCarCurrentPosInfo.Elapsed += new ElapsedEventHandler(this.tGetCarCurrentPosInfo_Elapsed);
			this.tGetCarCurrentPosInfo.Enabled = true;
		}

		public override void stop()
		{
			if (this.tGetCarCurrentPosInfo != null)
			{
				this.tGetCarCurrentPosInfo.Stop();
			}
		}

		private void tGetCarCurrentPosInfo_Elapsed(object sender, ElapsedEventArgs e)
		{
			try
			{
				try
				{
					this.tGetCarCurrentPosInfo.Enabled = false;
					this.tGetCarCurrentPosInfo.Interval = 30000;
					DataTable carCurrentPosInfo = this.getCarCurrentPosInfo();
					if (carCurrentPosInfo != null && carCurrentPosInfo.Rows.Count > 0)
					{
						lock (dicCarList)
						{
							foreach (DataRow row in carCurrentPosInfo.Rows)
							{
								dicCarList[int.Parse(row["CarId"].ToString())] = row;
							}
						}
					}
				}
				catch (Exception exception)
				{
					ErrorMsg errorMsg = new ErrorMsg("GpsCarCurrentPosInfo", "tGetCarCurrentPosInfo_Elapsed", exception.Message);
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tGetCarCurrentPosInfo.Enabled = true;
			}
		}
	}
}