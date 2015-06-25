using ParamLibrary.Application;
using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class PosReport : BlackBox
	{
		private static int Max_Timming;

		private static int Min_Timming;

		public static string ReporTimmingTimesError;

		public static string ReportTimmingTimeError;

		public CarProtocolType protocolType = CarProtocolType.非交通厅;

		public int CompressionUpTime
		{
			get;
			set;
		}

		[TrafficProtocol("IsCompressed", true)]
		public CmdParam.IsCompressed isCompressed
		{
			get;
			set;
		}

		public int LowReportCycle
		{
			get;
			set;
		}

		[TrafficProtocol("RCycle")]
		public override int ReportCycle
		{
			get
			{
				return this.CompressionUpTime << 8 | this.LowReportCycle;
			}
		}

		[TrafficProtocol("RTiming")]
		public int ReportTiming
		{
			get;
			set;
		}

		[TrafficProtocol("RWhenStp", true)]
		public CmdParam.ReportWhenStop ReportWhenStop
		{
			get;
			set;
		}

		static PosReport()
		{
			PosReport.Max_Timming = 65535;
			PosReport.Min_Timming = 0;
			PosReport.ReporTimmingTimesError = "回传次数必须为0-65535之间的数字";
			PosReport.ReportTimmingTimeError = "监控时长必须为0-65535之间的数字";
		}

		public PosReport()
		{
		}

		public new int CheckData(out string strErrorMsg)
		{
			return ((!base.CheckAutoCalArc(out strErrorMsg) || !this.CheckReportTimming(out strErrorMsg) ? false : base.CheckReportCycle(out strErrorMsg)) ? 0 : -1);
		}

		protected bool CheckReportTimming(out string strErrorMsg)
		{
			bool flag;
			strErrorMsg = string.Empty;
			if ((this.ReportTiming > PosReport.Max_Timming ? false : this.ReportTiming >= PosReport.Min_Timming))
			{
				flag = true;
			}
			else
			{
				if (base.ReportType != CmdParam.ReportType.定次汇报)
				{
					strErrorMsg = PosReport.ReportTimmingTimeError;
				}
				else
				{
					strErrorMsg = PosReport.ReporTimmingTimesError;
				}
				flag = false;
			}
			return flag;
		}
	}
}