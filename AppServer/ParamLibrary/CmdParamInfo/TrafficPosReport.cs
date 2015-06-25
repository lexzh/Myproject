using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class TrafficPosReport : CmdParamBase
	{
		[TrafficProtocol("AlarmCycleDis", false, "512", "紧急报警时的汇报距离间隔")]
		public int AlarmCycleDis
		{
			get;
			set;
		}

		[TrafficProtocol("AlarmCycleTime", false, "512", "紧急报警时的汇报时间间隔")]
		public int AlarmCycleTime
		{
			get;
			set;
		}

		[TrafficProtocol("DefCycleDis", false, "512", "缺省距离汇报间隔")]
		public int DefCycleDis
		{
			get;
			set;
		}

		[TrafficProtocol("DefCycleTime", false, "512", "缺省时间汇报间隔")]
		public int DefCycleTime
		{
			get;
			set;
		}

		[TrafficProtocol("IsAutoCalArc", false, "512", "拐点补偿")]
		public int IsAutoCalArc
		{
			get;
			set;
		}

		[TrafficProtocol("NLoginCycleDis", false, "512", "未登录时的汇报距离间隔")]
		public int NLoginCycleDis
		{
			get;
			set;
		}

		[TrafficProtocol("NLoginCycleTime", false, "512", "未登录时的汇报时间间隔")]
		public int NLoginCycleTime
		{
			get;
			set;
		}

		[TrafficProtocol("RestCycleDis", false, "512", "休眠时的汇报距离间隔")]
		public int RestCycleDis
		{
			get;
			set;
		}

		[TrafficProtocol("RestCycleTime", false, "512", "休眠时的汇报时间间隔")]
		public int RestCycleTime
		{
			get;
			set;
		}

		[TrafficProtocol("RProject", false, "512", "位置汇报方案")]
		public int RProject
		{
			get;
			set;
		}

		[TrafficProtocol("RType", false, "512", "位置汇报策略")]
		public int RType
		{
			get;
			set;
		}

		public TrafficPosReport()
		{
		}
	}
}