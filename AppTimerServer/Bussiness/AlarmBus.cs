using System;

namespace Bussiness
{
	internal class AlarmBus
	{
		private string simNum = "";

		private int frontAlarmType;

		private int backAlarmType;

		private string lineId = "";

		private string routeId = "";

		public int BackAlarmType
		{
			get
			{
				return this.backAlarmType;
			}
			set
			{
				this.backAlarmType = value;
			}
		}

		public int FrontAlarmType
		{
			get
			{
				return this.frontAlarmType;
			}
			set
			{
				this.frontAlarmType = value;
			}
		}

		public string LineId
		{
			get
			{
				return this.lineId;
			}
			set
			{
				this.lineId = value;
			}
		}

		public string RouteId
		{
			get
			{
				return this.routeId;
			}
			set
			{
				this.routeId = value;
			}
		}

		public string SimNum
		{
			get
			{
				return this.simNum;
			}
			set
			{
				this.simNum = value;
			}
		}

		public AlarmBus()
		{
		}

		public static AlarmBus getAlarmBus(string routeId, string simnum, int fat, int bat, string lineId)
		{
			AlarmBus alarmBu = new AlarmBus()
			{
				RouteId = routeId,
				SimNum = simnum,
				FrontAlarmType = fat,
				BackAlarmType = bat,
				LineId = lineId
			};
			return alarmBu;
		}
	}
}