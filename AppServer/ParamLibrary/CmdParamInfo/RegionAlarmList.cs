using ParamLibrary.Application;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class RegionAlarmList : ArrayList
	{
		public CarProtocolType protocolType = CarProtocolType.非交通厅;
        public int? OperationType;

		public string AlarmRegionDot
		{
			get;
			set;
		}

		public CmdParam.CmdCode CmdCode
		{
			get
			{
				CmdParam.CmdCode orderCode;
				CmdParam.OrderCode orderCode1 = this.OrderCode;
				if (orderCode1 == CmdParam.OrderCode.设置区域报警)
				{
					orderCode = CmdParam.CmdCode.设置区域;
				}
				else if (orderCode1 == CmdParam.OrderCode.设置多功能区域报警)
				{
					orderCode = CmdParam.CmdCode.设置多功能区域报警;
				}
				else
				{
					orderCode = (CmdParam.CmdCode)this.OrderCode;
				}
				return orderCode;
			}
		}

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

		public int RegionFeature
		{
			get;
			set;
		}

		public RegionAlarmList()
		{
		}

		public int CheckData(out string strErrorMsg)
		{
			strErrorMsg = "";
			return 0;
		}

		private byte[] GetBitData(string str)
		{
			byte[] numArray = new byte[4];
			int num = str.IndexOf('.');
			string empty = string.Empty;
			string empty1 = string.Empty;
			if (num != -1)
			{
				empty = str.Substring(0, num);
				empty1 = str.Substring(num + 1, str.Length - num - 1).PadRight(6, '0');
			}
			else
			{
				empty = str;
				empty1 = "0";
			}
			numArray[0] = (byte)int.Parse(empty);
			numArray[1] = (byte)int.Parse(empty1.Substring(0, 2));
			numArray[2] = (byte)int.Parse(empty1.Substring(2, 2));
			numArray[3] = (byte)int.Parse(empty1.Substring(4, 2));
			return numArray;
		}

		private string GetMembersXml(ref string content)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.Count; i++)
			{
				RegionAlarm item = this[i] as RegionAlarm;
				stringBuilder.Append(item.GetMembersXml(ref content));
			}
			return stringBuilder.ToString();
		}

		private object GetRegions()
		{
			byte[] numArray = new byte[2048];
			int length = 0;
			this.AlarmRegionDot = string.Empty;
			foreach (RegionAlarm regionAlarm in this)
			{
				int num = regionAlarm.newRegionId;
				RegionAlarmList regionAlarmList = this;
				regionAlarmList.AlarmRegionDot = string.Concat(regionAlarmList.AlarmRegionDot, num);
				byte regionType = (byte)regionAlarm.RegionType;
				RegionAlarmList regionAlarmList1 = this;
				regionAlarmList1.AlarmRegionDot = string.Concat(regionAlarmList1.AlarmRegionDot, "\\", regionType);
				ArrayList points = regionAlarm.Points;
				long alarmCondition = regionAlarm.AlarmCondition;
				string beginTime = regionAlarm.BeginTime;
				string endTime = regionAlarm.EndTime;
				byte[] numArray1 = this.SealData(num, regionType, points, alarmCondition, beginTime, endTime);
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
			Array.Resize<byte>(ref numArray, length);
			return numArray;
		}

		private string GetTenBitData(string str)
		{
			int num = str.IndexOf('.');
			string empty = string.Empty;
			string str1 = str;
			if (num == -1)
			{
				empty = str;
				str1 = string.Concat(str, ".0");
			}
			string str2 = str1.PadRight(10, '0').Substring(0, 10);
			return str2;
		}

		private byte[] SealData(int regionId, byte regionType, ArrayList pointsList, long alarmCondition, string beginTime, string endTime)
		{
			byte[] numArray;
			int num = 0;
			numArray = (this.OrderCode != CmdParam.OrderCode.设置区域报警 ? new byte[4 + pointsList.Count * 20 + 4 + 12 + 12] : new byte[4 + pointsList.Count * 20]);
			int num1 = num;
			num = num1 + 1;
			numArray[num1] = (byte)regionId;
			int num2 = num;
			num = num2 + 1;
			numArray[num2] = regionType;
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
				string tenBitData = this.GetTenBitData(item.Longitude.ToString());
				string str = this.GetTenBitData(item.Latitude.ToString());
				byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(tenBitData);
				byte[] bytes1 = Encoding.GetEncoding("gb2312").GetBytes(str);
				RegionAlarmList regionAlarmList = this;
				string alarmRegionDot = regionAlarmList.AlarmRegionDot;
				string[] strArrays = new string[] { alarmRegionDot, "\\", tenBitData, "\\", str };
				regionAlarmList.AlarmRegionDot = string.Concat(strArrays);
				bytes.CopyTo(numArray, num);
				num = num + 10;
				bytes1.CopyTo(numArray, num);
				num = num + 10;
			}
			RegionAlarmList regionAlarmList1 = this;
			regionAlarmList1.AlarmRegionDot = string.Concat(regionAlarmList1.AlarmRegionDot, "*");
			if (this.OrderCode == CmdParam.OrderCode.设置多功能区域报警)
			{
				int num5 = num;
				num = num5 + 1;
				numArray[num5] = (byte)(alarmCondition >> 24);
				int num6 = num;
				num = num6 + 1;
				numArray[num6] = (byte)(alarmCondition >> 16);
				int num7 = num;
				num = num7 + 1;
				numArray[num7] = (byte)(alarmCondition >> 8);
				int num8 = num;
				num = num8 + 1;
				numArray[num8] = (byte)alarmCondition;
				byte[] numArray1 = Encoding.GetEncoding("gb2312").GetBytes(beginTime.PadRight(12, '0'));
				byte[] bytes2 = Encoding.GetEncoding("gb2312").GetBytes(endTime.PadRight(12, '0'));
				numArray1.CopyTo(numArray, num);
				num = num + 12;
				bytes2.CopyTo(numArray, num);
				num = num + 12;
			}
			return numArray;
		}

		public string ToXmlString(int OrderID, string Sim, string CarType, int CommFlag, string FunctionName, ref string content)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<?xml version='1.0' encoding='UTF-8'?>");
			stringBuilder.Append(string.Concat("<function name='", FunctionName, "' version='1.0.0'>"));
			stringBuilder.Append("<body>");
			stringBuilder.Append(string.Concat("<OrderID>", OrderID.ToString(), "</OrderID>"));
			stringBuilder.Append(string.Concat("<Sim>", Sim.ToString(), "</Sim>"));
			stringBuilder.Append(string.Concat("<CarType>", CarType.ToString(), "</CarType>"));
			stringBuilder.Append(string.Concat("<CmdCode>", (int)this.OrderCode, "</CmdCode>"));
			stringBuilder.Append(string.Concat("<CommFlag>", CommFlag.ToString(), "</CommFlag>"));
			stringBuilder.Append("<Parameter>");
			stringBuilder.Append(string.Concat("<RegionNums>", this.Count, "</RegionNums>"));
			stringBuilder.Append("<Type>0</Type>");
			stringBuilder.Append(this.GetMembersXml(ref content));
			stringBuilder.Append("</Parameter>");
			stringBuilder.Append("</body>");
			stringBuilder.Append("</function>");
			return stringBuilder.ToString();
		}
	}
}