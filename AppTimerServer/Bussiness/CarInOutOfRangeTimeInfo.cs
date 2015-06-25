using System;

namespace Bussiness
{
	internal class CarInOutOfRangeTimeInfo
	{
		private int iD;

		private string simNum;

		private string startTime;

		private string endTime;

		private int regionID;

		private string regionDot;

		private string regionName;

		private bool isInAlarm;

		private bool isOutAlarm;

		private string midTime;

		private int regionIndex;

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

		public bool IsInAlarm
		{
			get
			{
				return this.isInAlarm;
			}
			set
			{
				this.isInAlarm = value;
			}
		}

		public bool IsOutAlarm
		{
			get
			{
				return this.isOutAlarm;
			}
			set
			{
				this.isOutAlarm = value;
			}
		}

		public string MidTime
		{
			get
			{
				return this.midTime;
			}
			set
			{
				this.midTime = value;
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

		public int RegionIndex
		{
			get
			{
				return this.regionIndex;
			}
			set
			{
				this.regionIndex = value;
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

		public string StartTime
		{
			get
			{
				return this.startTime;
			}
			set
			{
				this.startTime = value;
			}
		}

		public CarInOutOfRangeTimeInfo()
		{
		}

		public string getMidTime()
		{
			if (Convert.ToDateTime(string.Concat("1970-01-01 ", this.StartTime)).CompareTo(Convert.ToDateTime(string.Concat("1970-01-01 ", this.EndTime))) <= 0)
			{
				string[] strArrays = this.StartTime.Split(new char[] { ':' });
				string[] strArrays1 = this.EndTime.Split(new char[] { ':' });
				int[] numArray = new int[3];
				int num = int.Parse(strArrays[0]) + int.Parse(strArrays1[0]);
				bool flag = false;
				bool flag1 = num % 2 == 1;
				numArray[0] = num / 2;
				if (!flag1)
				{
					numArray[1] = (int.Parse(strArrays[1]) + int.Parse(strArrays1[1])) / 2;
				}
				else
				{
					numArray[1] = (int.Parse(strArrays[1]) + int.Parse(strArrays1[1])) / 2 + 30;
					flag = numArray[1] >= 60;
				}
				if (flag)
				{
					numArray[0] = numArray[0] + 1;
					numArray[1] = numArray[1] - 60;
				}
				numArray[2] = (int.Parse(strArrays[2]) + int.Parse(strArrays1[2])) / 2;
				object[] objArray = new object[] { numArray[0], ":", numArray[1], ":", numArray[2] };
				return string.Concat(objArray);
			}
			string[] strArrays2 = this.StartTime.Split(new char[] { ':' });
			string[] strArrays3 = this.EndTime.Split(new char[] { ':' });
			int[] numArray1 = new int[3];
			int num1 = int.Parse(strArrays2[0]) + int.Parse(strArrays3[0]);
			bool flag2 = false;
			bool flag3 = num1 % 2 == 1;
			numArray1[0] = (num1 + 24) / 2;
			if (!flag3)
			{
				numArray1[1] = (int.Parse(strArrays2[1]) + int.Parse(strArrays3[1])) / 2;
			}
			else
			{
				numArray1[1] = (int.Parse(strArrays2[1]) + int.Parse(strArrays3[1])) / 2 + 30;
				flag2 = numArray1[1] >= 60;
			}
			if (flag2)
			{
				numArray1[0] = numArray1[0] + 1;
				numArray1[1] = numArray1[1] - 60;
			}
			numArray1[0] = numArray1[0] % 24;
			numArray1[2] = (int.Parse(strArrays2[2]) + int.Parse(strArrays3[2])) / 2;
			object[] objArray1 = new object[] { numArray1[0], ":", numArray1[1], ":", numArray1[2] };
			return string.Concat(objArray1);
		}
	}
}