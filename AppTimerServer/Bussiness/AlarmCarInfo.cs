using System;
using System.Collections.Generic;

namespace Bussiness
{
	internal class AlarmCarInfo
	{
		public int carid;

		public List<DateTime> StartTime = new List<DateTime>();

		public List<DateTime> EndTime = new List<DateTime>();

		public List<int> pathid = new List<int>();

		public bool IsAlarm;

		public AlarmCarInfo()
		{
		}

		public static AlarmCarInfo getAlarmCar(int carid, int pathid, DateTime StartTime, DateTime EndTime)
		{
			AlarmCarInfo alarmCarInfo = new AlarmCarInfo()
			{
				carid = carid
			};
			alarmCarInfo.StartTime.Add(StartTime);
			alarmCarInfo.EndTime.Add(EndTime);
			alarmCarInfo.pathid.Add(pathid);
			return alarmCarInfo;
		}

		public AlarmCarInfo updateAlarmCar(int carid, int pathid, DateTime StartTime, DateTime EndTime)
		{
			if (!this.pathid.Contains(pathid))
			{
				this.pathid.Add(pathid);
				this.StartTime.Add(StartTime);
				this.EndTime.Add(EndTime);
				this.IsAlarm = false;
			}
			return this;
		}
	}
}