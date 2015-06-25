using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class MultiPathInfo : PathOverSpeedAlarmList
	{
		public double dMaxLat
		{
			get;
			set;
		}

		public double dMaxLon
		{
			get;
			set;
		}

		public double dMinLat
		{
			get;
			set;
		}

		public double dMinLon
		{
			get;
			set;
		}

		public MultiPathInfo()
		{
		}

		public new int CheckData(out string strErrorMsg)
		{
			strErrorMsg = "";
			return 0;
		}

		private byte[] SealData(int regionId, ArrayList pointsList, byte speed, byte time)
		{
			int num = 0;
			byte[] numArray = new byte[3 + pointsList.Count * 20 + 2];
			int num1 = num;
			num = num1 + 1;
			numArray[num1] = (byte)(regionId >> 8);
			int num2 = num;
			num = num2 + 1;
			numArray[num2] = (byte)(regionId & 255);
			int count = pointsList.Count;
			int num3 = num;
			num = num3 + 1;
			numArray[num3] = (byte)(count >> 8);
			int num4 = num;
			num = num4 + 1;
			numArray[num4] = (byte)(count & 255);
			for (int i = 0; i < pointsList.Count; i++)
			{
				Point item = (Point)pointsList[i];
				item.Longitude.ToString();
				item.Latitude.ToString();
				char[] charArray = base.GetTenBitData(item.Longitude.ToString()).ToCharArray();
				char[] chrArray = base.GetTenBitData(item.Latitude.ToString()).ToCharArray();
				charArray.CopyTo(numArray, num);
				num = num + 10;
				chrArray.CopyTo(numArray, num);
			}
			int num5 = num;
			num = num5 + 1;
			numArray[num5] = speed;
			int num6 = num;
			num = num6 + 1;
			numArray[num6] = time;
			return numArray;
		}
	}
}