using ParamLibrary.Application;
using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class BlackBox : CmdParamBase
	{
		private static int Max_Interval;

		private static int Min_Interval;

		private static byte Max_AutoCalArc;

		private static byte Min_AutoCalArc;

		public static string AutoCalArcError;

		public static string ReportCycleTimeError;

		public static string ReportCycleDiscError;

		public int Flag
		{
			get;
			set;
		}

		[TrafficProtocol("IsAutoCalArc")]
		public byte IsAutoCalArc
		{
			get;
			set;
		}

		public new CmdParam.OrderCode OrderCode
		{
			get;
			set;
		}

		public virtual int ReportCycle
		{
			get;
			set;
		}

		public CmdParam.ReportType ReportType
		{
			get;
			set;
		}

		static BlackBox()
		{
			BlackBox.Max_Interval = 65535;
			BlackBox.Min_Interval = 0;
			BlackBox.Max_AutoCalArc = 180;
			BlackBox.Min_AutoCalArc = 0;
			BlackBox.AutoCalArcError = "拐点补偿必须为0-180之间的数字";
			BlackBox.ReportCycleTimeError = "间隔时间必须为0-65535之间的数字";
			BlackBox.ReportCycleDiscError = "间隔距离必须为0-65535之间的数字";
		}

		public BlackBox()
		{
		}

		protected bool CheckAutoCalArc(out string strErrorMsg)
		{
			bool flag;
			strErrorMsg = string.Empty;
			if ((this.IsAutoCalArc > BlackBox.Max_AutoCalArc ? false : this.IsAutoCalArc >= BlackBox.Min_AutoCalArc))
			{
				flag = true;
			}
			else
			{
				strErrorMsg = BlackBox.AutoCalArcError;
				flag = false;
			}
			return flag;
		}

		public override int CheckData(out string strErrorMsg)
		{
			return ((!this.CheckAutoCalArc(out strErrorMsg) ? false : this.CheckReportCycle(out strErrorMsg)) ? 0 : -1);
		}

		protected bool CheckReportCycle(out string strErrorMsg)
		{
			bool flag;
			strErrorMsg = string.Empty;
			if ((this.ReportCycle > BlackBox.Max_Interval ? false : this.ReportCycle >= BlackBox.Min_Interval))
			{
				flag = true;
			}
			else
			{
				if (this.ReportType != CmdParam.ReportType.定距汇报)
				{
					strErrorMsg = BlackBox.ReportCycleTimeError;
				}
				else
				{
					strErrorMsg = BlackBox.ReportCycleDiscError;
				}
				flag = false;
			}
			return flag;
		}
	}
}