using System;

namespace Bussiness
{
	internal class CarBeBackTimeInfo
	{
		private int iD;

		private string simNum;

		private string beginTime;

		private string endTime;

		private int regionID;

		private string regionDot;

		private string regionName;

		private bool isAlarm;

		public string BeginTime
		{
			get
			{
				return this.beginTime;
			}
			set
			{
				this.beginTime = value;
			}
		}

		public string EndTime
		{
			get
			{
				return this.endTime;
			}
			set
			{
				this.endTime = value;
			}
		}

		public int ID
		{
			get
			{
				return this.iD;
			}
			set
			{
				this.iD = value;
			}
		}

		public bool IsAlarm
		{
			get
			{
				return this.isAlarm;
			}
			set
			{
				this.isAlarm = value;
			}
		}

		public string RegionDot
		{
			get
			{
				return this.regionDot;
			}
			set
			{
				this.regionDot = value;
			}
		}

		public int RegionID
		{
			get
			{
				return this.regionID;
			}
			set
			{
				this.regionID = value;
			}
		}

		public string RegionName
		{
			get
			{
				return this.regionName;
			}
			set
			{
				this.regionName = value;
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

		public CarBeBackTimeInfo()
		{
		}

		public CarBeBackTimeInfo(int _id, string _simNum, string _beginTime, string _endTime, int _regionID, string _regionDot, string _regionName)
		{
			this.ID = _id;
			this.SimNum = _simNum;
			this.BeginTime = _beginTime;
			this.EndTime = _endTime;
			this.RegionID = _regionID;
			this.RegionDot = _regionDot;
			this.RegionName = _regionName;
			this.IsAlarm = false;
		}
	}
}