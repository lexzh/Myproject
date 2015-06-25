using ParamLibrary.Application;
using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class TransportReport : CmdParamBase
	{
		private static int Max;

		private static int Min;

		public static string StatuFreeError;

		public static string StatuBusyError;

		[TrafficProtocol("StatuBusy")]
		public int nStatuBusy
		{
			get;
			set;
		}

		[TrafficProtocol("StatuFree")]
		public int nStatuFree
		{
			get;
			set;
		}

		[TrafficProtocol("StatuTask")]
		public int nStatuTask
		{
			get;
			set;
		}

		[TrafficProtocol("RType")]
		public byte ReportFlag
		{
			get;
			set;
		}

		static TransportReport()
		{
			TransportReport.Max = 65535;
			TransportReport.Min = 0;
			object[] min = new object[] { "空车汇报间隔必须为", TransportReport.Min, "-", TransportReport.Max, "范围内的数字" };
			TransportReport.StatuFreeError = string.Concat(min);
			min = new object[] { "重车汇报间隔必须为", TransportReport.Min, "-", TransportReport.Max, "范围内的数字" };
			TransportReport.StatuBusyError = string.Concat(min);
		}

		public TransportReport()
		{
		}

		public override int CheckData(out string strErrorMsg)
		{
			strErrorMsg = "";
			int num = -1;
			if (base.OrderCode == CmdParam.OrderCode.设置出租车监控)
			{
				num = ((!this.CheckData(out strErrorMsg, this.nStatuFree, "空车汇报间隔") ? false : this.CheckData(out strErrorMsg, this.nStatuBusy, "重车汇报间隔")) ? 0 : -1);
			}
			return num;
		}

		private bool CheckData(out string strErrorMsg, int iValue, string strType)
		{
			bool flag;
			strErrorMsg = "";
			if ((iValue > TransportReport.Max ? false : iValue >= TransportReport.Min))
			{
				flag = true;
			}
			else
			{
				object[] objArray = new object[] { strType, "必须为", TransportReport.Min, "-", TransportReport.Max, "范围内的数字！" };
				strErrorMsg = string.Concat(objArray);
				flag = false;
			}
			return flag;
		}

		public string GetParamDisc()
		{
			string empty = string.Empty;
			if (base.OrderCode == CmdParam.OrderCode.设置出租车监控)
			{
				empty = string.Concat(empty, "空车汇报间隔-", this.nStatuFree);
				empty = string.Concat(empty, ",重车汇报间隔-", this.nStatuBusy);
			}
			return empty;
		}
	}
}