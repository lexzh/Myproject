using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class SpeedAlarm : CmdParamBase
	{
		public static string ErrorMaxSpeed;

		public static string ErrorHoldTime;

		public int HoldTime
		{
			get;
			set;
		}

		[TrafficProtocol("MaxSpeed")]
		public int MaxSpeed
		{
			get;
			set;
		}

		[TrafficProtocol("HoldTime")]
		public int RealHoldTime
		{
			get;
			set;
		}

		static SpeedAlarm()
		{
			SpeedAlarm.ErrorMaxSpeed = "最高限速超过指定的范围0-255！";
			SpeedAlarm.ErrorHoldTime = "持续时间超过指定的范围0-250！";
		}

		public SpeedAlarm()
		{
		}

		public override int CheckData(out string strErrorMsg)
		{
			int num;
			if (!(this.MaxSpeed < 0 ? false : this.MaxSpeed <= 255))
			{
				strErrorMsg = SpeedAlarm.ErrorMaxSpeed;
				num = -1;
			}
			else if ((this.HoldTime < 0 ? false : this.HoldTime <= 255))
			{
				strErrorMsg = "";
				num = 0;
			}
			else
			{
				strErrorMsg = SpeedAlarm.ErrorHoldTime;
				num = -1;
			}
			return num;
		}
	}
}