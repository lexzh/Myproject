using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Timers;
using ParamLibrary.CmdParamInfo;
using Library;
using ParamLibrary.Application;

namespace Bussiness
{
	public class PicTimer : ProcessBase
	{
		private DownData myDownData = new DownData(0);

		private CaptureEx m_CaptureEx = new CaptureEx();

		private string sPw = "";

		private System.Timers.Timer tRecordTimer;

		private int iRecordTime = 60000 * ReadDataFromXml.PicTimeDiff;

		public PicTimer()
		{
		}

		private bool getParam(DataRow myRow)
		{
			bool flag;
			byte num = 0;
			byte num1 = 1;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			byte num5 = 0;
			byte num6 = 0;
			byte num7 = 0;
			byte num8 = 0;
			byte num9 = 0;
			int num10 = 0;
			int num11 = 0;
			string empty = string.Empty;
			try
			{
				num = Convert.ToByte(myRow["CamerasID"]);
				num1 = Convert.ToByte(myRow["IsMultiFrame"]);
				if (!string.IsNullOrEmpty(myRow["CapWhenStop"].ToString()))
				{
					num2 = Convert.ToInt32(myRow["CapWhenStop"]);
				}
				num3 = Convert.ToInt32(myRow["Times"]);
				num4 = Convert.ToInt32(myRow["CatchInterval"]);
				num5 = Convert.ToByte(myRow["Quality"]);
				num6 = Convert.ToByte(myRow["Brightness"]);
				num7 = Convert.ToByte(myRow["Contrast"]);
				num8 = Convert.ToByte(myRow["Saturation"]);
				num9 = Convert.ToByte(myRow["Chroma"]);
				num10 = Convert.ToInt32(myRow["CaptureFlag"]);
				num11 = Convert.ToInt32(myRow["CaptureMask"]);
				empty = myRow["SimNum"].ToString();
				this.m_CaptureEx.ProtocolName = "OTHER";
				try
				{
					if (SqlDataAccess.getDataBySql(string.Format("select gp.ProtocolName from giscar as gc inner join GpsTerminalType as gt on gc.terminaltypeid=gt.terminaltypeid inner join gpsprotocol as gp on gt.ProtocolCode=gp.ProtocolCode where SimNum = '{0}'", empty)).Rows[0][0].ToString() == "JTBGPS")
					{
						this.m_CaptureEx.ProtocolName = "JTBGPS";
					}
				}
				catch
				{
				}
				this.m_CaptureEx.CamerasID = num;
				this.m_CaptureEx.IsMultiFrame = num1;
				this.m_CaptureEx.CapWhenStop = num2;
				this.m_CaptureEx.Times = num3;
				this.m_CaptureEx.Interval = num4;
				this.m_CaptureEx.Quality = num5;
				this.m_CaptureEx.Brightness = num6;
				this.m_CaptureEx.Contrast = num7;
				this.m_CaptureEx.Saturation = num8;
				this.m_CaptureEx.Chroma = num9;
				this.m_CaptureEx.CaptureFlag = num10;
				this.m_CaptureEx.CaptureCache = num11;
				this.m_CaptureEx.OrderCode = CmdParam.OrderCode.定时抓拍图像监控;
				return true;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		private DataTable GetPicDataSet(string sPicTime)
		{
			DataTable dataBySP;
			try
			{
				string str = "GpsPicServer_GetCarPicParam";
				SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@PicTime", sPicTime) };
				dataBySP = SqlDataAccess.getDataBySP(str, sqlParameter);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "PicTimer",
					FunctionName = "GetPicDataSet",
					ErrorText = string.Concat("获取定时拍照数据发生错误!", exception.Message)
				};
				this.logHelper.WriteError(errorMsg);
				return null;
			}
			return dataBySP;
		}

		private string GetPicTime()
		{
			string str = "";
			int picTimeDiff = ReadDataFromXml.PicTimeDiff;
			DateTime now = DateTime.Now;
			for (int i = 0; i < picTimeDiff; i++)
			{
				string str1 = "00";
				string str2 = "00";
				DateTime dateTime = now.AddMinutes((double)i);
				try
				{
					if (dateTime.Hour >= 10)
					{
						str1 = dateTime.Hour.ToString();
					}
					else
					{
						int hour = dateTime.Hour;
						str1 = string.Concat("0", hour.ToString());
					}
					if (dateTime.Minute >= 10)
					{
						str2 = dateTime.Minute.ToString();
					}
					else
					{
						int minute = dateTime.Minute;
						str2 = string.Concat("0", minute.ToString());
					}
					if (i != picTimeDiff - 1)
					{
						string str3 = str;
						string[] strArrays = new string[] { str3, str1, ":", str2, "," };
						str = string.Concat(strArrays);
					}
					else
					{
						str = string.Concat(str, str1, ":", str2);
					}
				}
				catch
				{
				}
			}
			return str;
		}

		private void onRecordTimerMain(object sender, ElapsedEventArgs e)
		{
			this.tRecordTimer.Enabled = false;
			try
			{
				try
				{
					this.RecordTimerMain();
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("PicTimer", "onRecordTimerMain", string.Concat("定时拍照", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tRecordTimer.Enabled = true;
			}
		}

		private void RecordTimerMain()
		{
			string picTime = this.GetPicTime();
			DataTable picDataSet = this.GetPicDataSet(picTime);
			if (picDataSet == null)
			{
				return;
			}
			string[] strArrays = picTime.Split(new char[] { ',' });
			DataRow[] dataRowArray = null;
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str = string.Concat("PicTime like '%", strArrays[i], ":%'");
				try
				{
					dataRowArray = picDataSet.Select(str);
					if (dataRowArray != null && (int)dataRowArray.Length > 0)
					{
						DataRow[] dataRowArray1 = dataRowArray;
						for (int j = 0; j < (int)dataRowArray1.Length; j++)
						{
							DataRow dataRow = dataRowArray1[j];
							if (this.getParam(dataRow))
							{
								string str1 = dataRow["SimNum"].ToString();
								string str2 = dataRow["CarId"].ToString();
								this.myDownData.icar_SetCaptureEx(CmdParam.ParamType.SimNum, str1, this.sPw, CmdParam.CommMode.未知方式, this.m_CaptureEx, strArrays[i], str2);
							}
						}
					}
				}
				catch
				{
				}
				if (i != (int)strArrays.Length - 1)
				{
					Thread.Sleep(60000);
				}
			}
			dataRowArray = null;
			picDataSet.Clear();
		}

		public override void start()
		{
			try
			{
				this.tRecordTimer = new System.Timers.Timer((double)this.iRecordTime);
				this.tRecordTimer.Elapsed += new ElapsedEventHandler(this.onRecordTimerMain);
				this.tRecordTimer.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				this.tRecordTimer.Enabled = true;
				ErrorMsg errorMsg = new ErrorMsg("PicTimer", "start", string.Concat("启动定时拍照失败", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.tRecordTimer.Stop();
		}
	}
}