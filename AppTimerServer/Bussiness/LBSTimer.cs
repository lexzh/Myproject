

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Timers;
using ParamLibrary.CmdParamInfo;
using Library;
using ParamLibrary.Application;

namespace Bussiness
{
	public class LBSTimer : ProcessBase
	{
		private DownData myDownData = new DownData(0);

		private System.Timers.Timer tLBSPosTimer;

		private System.Timers.Timer tGetLBSPosDataTimer;

		private int iLBSPosInterval = 60000 * ReadDataFromXml.LBSPosInterval;

		private int iGetLBSPosDataInterval = 60000 * ReadDataFromXml.GetLBSPosDataInterval;

		private Hashtable htLBSPos = new Hashtable();

		private PosReport m_PosReport = new PosReport();

		private string preGetLBSTime = "";

		public LBSTimer()
		{
		}

		private void GetLBSDataTimer()
		{
			DateTime localTime = DateTime.Now.ToLocalTime();
			DateTime dateTime = DateTime.Parse(ReadDataFromXml.LBSStarTime);
			DateTime dateTime1 = DateTime.Parse(ReadDataFromXml.LBSEndTime);
			if (dateTime > localTime || localTime > dateTime1)
			{
				this.htLBSPos.Clear();
				return;
			}
			DataTable lBSPosData = this.GetLBSPosData(this.preGetLBSTime);
			if (lBSPosData != null && lBSPosData.Rows.Count > 0)
			{
				string empty = string.Empty;
				lock (this.htLBSPos)
				{
					this.htLBSPos.Clear();
					this.htLBSPos = null;
					this.htLBSPos = new Hashtable();
					foreach (DataRow row in lBSPosData.Rows)
					{
						empty = string.Concat(row["SimNum"].ToString(), "|", row["CarId"].ToString());
						this.htLBSPos.Add(empty, null);
					}
				}
				lBSPosData.Clear();
				lBSPosData = null;
			}
		}

		private DataTable GetLBSPosData(string sPreGetLBSTime)
		{
			DataTable dataBySP;
			try
			{
				string str = "GpsPicServer_GetLBSPosData";
				string lBSType = ReadDataFromXml.LBSType;
				SqlParameter[] sqlParameter = new SqlParameter[] { new SqlParameter("@TerminalTypeId", lBSType), new SqlParameter("@preGetLBSTime", sPreGetLBSTime) };
				dataBySP = SqlDataAccess.getDataBySP(str, sqlParameter);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg()
				{
					ClassName = "LBSTimer",
					FunctionName = "GetLBSPosData",
					ErrorText = string.Concat("从数据库读取需要进行LBS定位数据发生错误!", exception.Message)
				};
				(new LogHelper()).WriteError(errorMsg);
				return null;
			}
			return dataBySP;
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

		private object getPhoneList(ArrayList phoneList)
		{
			if (phoneList == null || phoneList.Count == 0)
			{
				return null;
			}
			byte[] numArray = new byte[phoneList.Count * 15];
			for (int i = 0; i < phoneList.Count; i++)
			{
				byte[] bytes = new byte[15];
				Encoding encoding = Encoding.GetEncoding(CmdParamBase.Gb2312);
				string str = phoneList[i].ToString();
				char[] chrArray = new char[] { '|' };
				bytes = encoding.GetBytes(str.Split(chrArray)[0]);
				bytes.CopyTo(numArray, i * 15);
				for (int j = i * 15 + (int)bytes.Length; j < (i + 1) * 15 - 1; j++)
				{
					numArray[j] = 0;
				}
			}
			return numArray;
		}

		private StringBuilder getSimNums()
		{
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				int num = 0;
				lock (this.htLBSPos)
				{
					IDictionaryEnumerator enumerator = this.htLBSPos.GetEnumerator();
					while (enumerator.MoveNext())
					{
						if (num >= ReadDataFromXml.LBSPosMaxNum)
						{
							stringBuilder.Remove(stringBuilder.Length - 1, 1);
							stringBuilder.Append(';');
							stringBuilder.Append(enumerator.Key.ToString());
							stringBuilder.Append(",");
							num = 1;
						}
						else
						{
							stringBuilder.Append(enumerator.Key.ToString());
							stringBuilder.Append(",");
							num++;
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				LogHelper logHelper = new LogHelper();
				logHelper.WriteError(new ErrorMsg("LBSTimer", "组合simnum为byte[]出错", exception.Message));
			}
			return stringBuilder;
		}

		private void LBSMuliPos()
		{
			if (this.htLBSPos == null || this.htLBSPos.Count <= 0)
			{
				return;
			}
			LogHelper logHelper = new LogHelper();
			string empty = string.Empty;
			long num = (long)0;
			try
			{
				empty = this.getSimNums().ToString();
				string[] strArrays = empty.Split(new char[] { ';' });
				for (int i = 0; i < (int)strArrays.Length; i++)
				{
					string str = strArrays[i];
					char[] chrArray = new char[] { ',' };
					ArrayList arrayLists = new ArrayList(str.Split(chrArray));
					if (arrayLists[arrayLists.Count - 1].Equals(""))
					{
						arrayLists.RemoveAt(arrayLists.Count - 1);
					}
					string[] strArrays1 = arrayLists[0].ToString().Split(new char[] { '|' });
					string str1 = strArrays1[0];
					int num1 = Convert.ToInt32(strArrays1[1]);
					object phoneList = this.getPhoneList(arrayLists);
					if (this.myDownData.icar_SendRawPackage(str1, phoneList, num1) != (long)0)
					{
						int count = arrayLists.Count;
						logHelper.WriteLog(new LogMsg("Service", "LBSPos", string.Concat("指令下发： 串头： ", str1, " 失败   下发车数：", count.ToString())));
					}
					else
					{
						int count1 = arrayLists.Count;
						logHelper.WriteLog(new LogMsg("Service", "LBSPos", string.Concat("指令下发： 串头： ", str1, " 成功   下发车数：", count1.ToString())));
					}
					if (ReadDataFromXml.LBSPosSleepTime != 0)
					{
						Thread.Sleep(1000 * ReadDataFromXml.LBSPosSleepTime);
					}
				}
			}
			catch (Exception exception)
			{
				logHelper.WriteError(new ErrorMsg("LBSTimer", "LBSLBSMuliPos", exception.Message));
				Thread.Sleep(60000);
			}
		}

		private void LBSPosTimer()
		{
			bool flag = false;
			try
			{
				string lBSMuliPosTypes = ReadDataFromXml.LBSMuliPosTypes;
				if (!string.IsNullOrEmpty(lBSMuliPosTypes))
				{
					string[] strArrays = lBSMuliPosTypes.Split(new char[] { ',' });
					int num = 0;
					while (num < (int)strArrays.Length)
					{
						string str = strArrays[num];
						if (int.Parse(ReadDataFromXml.LBSType) != int.Parse(str))
						{
							num++;
						}
						else
						{
							flag = true;
							break;
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				flag = false;
				ErrorMsg errorMsg = new ErrorMsg("LBS定位", "LBS定位是否批量下发终端类型解析错误", exception.Message);
				this.logHelper.WriteError(errorMsg);
			}
			if (!flag)
			{
				this.LBSSigleCarPos();
				return;
			}
			flag = false;
			this.LBSMuliPos();
		}

		private void LBSSigleCarPos()
		{
			if (this.htLBSPos == null || this.htLBSPos.Count <= 0)
			{
				return;
			}
			LogHelper logHelper = new LogHelper();
			string[] strArrays = null;
			string str = "";
			int num = 0;
			long num1 = (long)0;
			this.getLBSPosParam();
			try
			{
				lock (this.htLBSPos)
				{
					foreach (object key in this.htLBSPos.Keys)
					{
						strArrays = key.ToString().Split(new char[] { '|' });
						str = strArrays[0];
						num = Convert.ToInt32(strArrays[1]);
						if (this.myDownData.iCar_SetPosReport(CmdParam.ParamType.SimNum, str, "", CmdParam.CommMode.未知方式, this.m_PosReport, num) == (long)-1)
						{
							ErrorMsg errorMsg = new ErrorMsg("LBS定位", "LBS定位下发指令失败，", string.Concat("SIM卡：", str))
							{
								ClassName = "service",
								FunctionName = "LBSPos"
							};
							logHelper.WriteError(errorMsg);
						}
						if (ReadDataFromXml.LBSPosSleepTime == 0)
						{
							continue;
						}
						Thread.Sleep(ReadDataFromXml.LBSPosSleepTime);
					}
				}
			}
			catch (Exception exception)
			{
				logHelper.WriteError(new ErrorMsg("LBSTimer", "LBSSigleCarPos", exception.Message));
				Thread.Sleep(60000);
			}
		}

		public override void start()
		{
			try
			{
				this.preGetLBSTime = "";
				this.tGetLBSPosDataTimer = new System.Timers.Timer((double)this.iGetLBSPosDataInterval);
				this.tGetLBSPosDataTimer.Elapsed += new ElapsedEventHandler(this.tGetLBSDataTimer_Elapsed);
				this.tGetLBSPosDataTimer.Enabled = true;
				this.tLBSPosTimer = new System.Timers.Timer((double)this.iGetLBSPosDataInterval);
				this.tLBSPosTimer.Elapsed += new ElapsedEventHandler(this.tLBSPosTimer_Elapsed);
				this.tLBSPosTimer.Enabled = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ErrorMsg errorMsg = new ErrorMsg("LBSTimer", "start", string.Concat("启动LBS定位服务失败", exception.Message));
				this.logHelper.WriteError(errorMsg);
			}
		}

		public override void stop()
		{
			this.tGetLBSPosDataTimer.Stop();
			this.tLBSPosTimer.Stop();
		}

		private void tGetLBSDataTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tGetLBSPosDataTimer.Enabled = false;
			try
			{
				try
				{
					this.GetLBSDataTimer();
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("LBSTimer", "tGetLBSDataTimer_Elapsed", string.Concat("读取LBS定位数据出错", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tGetLBSPosDataTimer.Interval = (double)this.iGetLBSPosDataInterval;
				this.tGetLBSPosDataTimer.Enabled = true;
			}
		}

		private void tLBSPosTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.tLBSPosTimer.Enabled = false;
			try
			{
				try
				{
					this.LBSPosTimer();
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg("LBSTimer", "tLBSPosTimer_Elapsed", string.Concat("LBS定位例外", exception.Message));
					this.logHelper.WriteError(errorMsg);
				}
			}
			finally
			{
				this.tLBSPosTimer.Interval = (double)this.iLBSPosInterval;
				this.tLBSPosTimer.Enabled = true;
			}
		}
	}
}