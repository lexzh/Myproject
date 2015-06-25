using ParamLibrary.Application;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class AlarmEntity : CmdParamBase
	{
		public long AlarmFlagEx
		{
			get;
			set;
		}

		public CmdParam.CarAlarmTypeTag AlarmFlagType
		{
			get;
			set;
		}

		[TrafficProtocol("AlarmFlag")]
		public long CarAlarmFlag
		{
			get;
			set;
		}

		[TrafficProtocol("AlarmSwitch")]
		public long CarAlarmSwitch
		{
			get;
			set;
		}

		[TrafficProtocol("AlarmFlagEx")]
		public long CarAlarmSwitchEx
		{
			get;
			set;
		}

		public long CarShowAlarm
		{
			get;
			set;
		}

		public long CarShowAlarmEx
		{
			get;
			set;
		}

		public string CustName
		{
			get;
			set;
		}

		[TrafficProtocol("Level")]
		public long Level
		{
			get;
			set;
		}

		public AlarmEntity()
		{
		}

		public override int CheckData(out string strErrorMsg)
		{
			return base.CheckData(out strErrorMsg);
		}

		private int GetOrValue(ArrayList list)
		{
			int num;
			int item = 0;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					item = item | (int)list[i];
				}
				num = item;
			}
			else
			{
				num = item;
			}
			return num;
		}
	}
}