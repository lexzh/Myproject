using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class MinSMSReportInterval : CmdParamBase
	{
		private static int MaxValue;

		private static int MinValue;

		public static string ErrorMsgSMS;

		public static string ErrorMsgMIX;

		public int MixedMode
		{
			get;
			set;
		}

		public int SMSMode
		{
			get;
			set;
		}

		static MinSMSReportInterval()
		{
			MinSMSReportInterval.MaxValue = 65535;
			MinSMSReportInterval.MinValue = 30;
			object[] minValue = new object[] { "纯短信模式下以短信方式汇报的最小时间间隔必须为", MinSMSReportInterval.MinValue, "-", MinSMSReportInterval.MaxValue, "之间的数字" };
			MinSMSReportInterval.ErrorMsgSMS = string.Concat(minValue);
			minValue = new object[] { "混合模式下以短信方式汇报的最小时间间隔必须为", MinSMSReportInterval.MinValue, "-", MinSMSReportInterval.MaxValue, "之间的数字" };
			MinSMSReportInterval.ErrorMsgMIX = string.Concat(minValue);
		}

		public MinSMSReportInterval()
		{
		}

		public override int CheckData(out string strErrorMsg)
		{
			int num;
			strErrorMsg = string.Empty;
			if (!(this.SMSMode > MinSMSReportInterval.MaxValue ? false : this.SMSMode >= MinSMSReportInterval.MinValue))
			{
				strErrorMsg = MinSMSReportInterval.ErrorMsgSMS;
				num = -1;
			}
			else if ((this.MixedMode > MinSMSReportInterval.MaxValue ? false : this.MixedMode >= MinSMSReportInterval.MinValue))
			{
				num = 0;
			}
			else
			{
				strErrorMsg = MinSMSReportInterval.ErrorMsgMIX;
				num = -1;
			}
			return num;
		}
	}
}