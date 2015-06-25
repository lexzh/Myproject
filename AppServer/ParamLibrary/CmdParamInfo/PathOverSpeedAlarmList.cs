using ParamLibrary.Application;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class PathOverSpeedAlarmList : ArrayList
	{
		public CmdParam.OrderCode OrderCode
		{
			get;
			set;
		}

		public object pvRegions
		{
			get
			{
				return this.GetRegions();
			}
		}

		public PathOverSpeedAlarmList()
		{
		}

		public int CheckData(out string strErrorMsg)
		{
			strErrorMsg = "";
			return 0;
		}

		private object GetRegions()
		{
			byte[] numArray = new byte[2048];
			int length = 0;
			foreach (PathAlarm pathAlarm in this)
			{
				int d = pathAlarm.ID;
				ArrayList points = pathAlarm.Points;
				byte[] numArray1 = this.SealData(d, points, pathAlarm.Speed, pathAlarm.Time);
				length = length + (int)numArray1.Length;
				if (length > (int)numArray.Length)
				{
					byte[] numArray2 = new byte[(int)numArray.Length];
					numArray.CopyTo(numArray2, 0);
					numArray = new byte[length * 2];
					numArray2.CopyTo(numArray, 0);
				}
				numArray1.CopyTo(numArray, length - (int)numArray1.Length);
			}
			return numArray;
		}

		protected string GetTenBitData(string str)
		{
			int num = str.IndexOf('.');
			string empty = string.Empty;
			string empty1 = string.Empty;
			if (num != -1)
			{
				empty = str.Substring(0, num);
				empty1 = str.Substring(num + 1, str.Length - num - 1);
			}
			else
			{
				empty = str;
				empty1 = "0";
			}
			string str1 = string.Concat(empty.PadLeft(3, '0'), ".", empty1.PadRight(6, '0'));
			return str1;
		}

		private byte[] SealData(int regionId, ArrayList pointsList, byte speed, byte time)
		{
			int num = 0;
			byte[] numArray = new byte[3 + pointsList.Count * 20 + 2];
			int num1 = num;
			num = num1 + 1;
			numArray[num1] = (byte)regionId;
			int count = pointsList.Count;
			int num2 = num;
			num = num2 + 1;
			numArray[num2] = (byte)(count >> 8);
			int num3 = num;
			num = num3 + 1;
			numArray[num3] = (byte)(count & 255);
			for (int i = 0; i < pointsList.Count; i++)
			{
				Point item = (Point)pointsList[i];
				item.Longitude.ToString();
				item.Latitude.ToString();
				char[] charArray = this.GetTenBitData(item.Longitude.ToString()).ToCharArray();
				char[] chrArray = this.GetTenBitData(item.Latitude.ToString()).ToCharArray();
				charArray.CopyTo(numArray, num);
				num = num + 10;
				chrArray.CopyTo(numArray, num);
			}
			int num4 = num;
			num = num4 + 1;
			numArray[num4] = speed;
			int num5 = num;
			num = num5 + 1;
			numArray[num5] = time;
			return numArray;
		}
	}
}