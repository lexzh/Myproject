using ParamLibrary.Application;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class PathAlarmList : ArrayList
	{
		private int _PointCount = 0;

		public CarProtocolType protocolType = CarProtocolType.非交通厅;

		public string AlarmPathDot
		{
			get;
			set;
		}

		public string BeginTime
		{
			get;
			set;
		}

		public string EndTime
		{
			get;
			set;
		}

		public CmdParam.OrderCode OrderCode
		{
			get;
			set;
		}

		public int PathFlag
		{
			get;
			set;
		}

		public int PathIDTraffic
		{
			get;
			set;
		}

		public string PathNameTraffic
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

		public PathAlarmList()
		{
		}

		public int CheckData(out string strErrorMsg)
		{
			strErrorMsg = "";
			return 0;
		}

		private string GetMembersXml(ref string content)
		{
			StringBuilder stringBuilder = new StringBuilder();
			this._PointCount = 0;
			for (int i = 0; i < this.Count; i++)
			{
				PathAlarm item = this[i] as PathAlarm;
				stringBuilder.Append(item.GetMembersXml(ref content));
				PathAlarmList count = this;
				count._PointCount = count._PointCount + item.Points.Count;
			}
			return stringBuilder.ToString();
		}

		private object GetRegions()
		{
			byte[] speed = new byte[2048];
			int num = 0;
			for (int i = 0; i < this.Count; i++)
			{
				PathAlarm item = (PathAlarm)this[i];
				int d = item.ID;
				PathAlarmList pathAlarmList = this;
				pathAlarmList.AlarmPathDot = string.Concat(pathAlarmList.AlarmPathDot, d);
				byte[] numArray = this.SealData(d, item.Points);
				num = (this.OrderCode != CmdParam.OrderCode.设置分路段超速报警 ? num + (int)numArray.Length : num + (int)numArray.Length + 26);
				Array.Resize<byte>(ref speed, num);
				if (this.OrderCode != CmdParam.OrderCode.设置分路段超速报警)
				{
					numArray.CopyTo(speed, num - (int)numArray.Length);
				}
				else
				{
					numArray.CopyTo(speed, num - (int)numArray.Length - 26);
				}
				if (this.OrderCode == CmdParam.OrderCode.设置分路段超速报警)
				{
					speed[num - 26] = item.Speed;
					speed[num - 25] = item.Time;
				}
				byte[] bytes = new byte[24];
				bytes = Encoding.GetEncoding("gb2312").GetBytes(string.Concat(item.sBeginTime, item.sEndTime));
				Array.Copy(bytes, 0, speed, num - (int)bytes.Length, (int)bytes.Length);
			}
			return speed;
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

		private byte[] SealData(int regionId, ArrayList pointsList)
		{
			int num = 0;
			byte[] numArray = new byte[3 + pointsList.Count * 20];
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
				string tenBitData = this.GetTenBitData(item.Longitude.ToString());
				string str = this.GetTenBitData(item.Latitude.ToString());
				byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(tenBitData);
				byte[] bytes1 = Encoding.GetEncoding("gb2312").GetBytes(str);
				PathAlarmList pathAlarmList = this;
				string alarmPathDot = pathAlarmList.AlarmPathDot;
				string[] strArrays = new string[] { alarmPathDot, "\\", tenBitData, "\\", str };
				pathAlarmList.AlarmPathDot = string.Concat(strArrays);
				bytes.CopyTo(numArray, num);
				num = num + 10;
				bytes1.CopyTo(numArray, num);
				num = num + 10;
			}
			PathAlarmList pathAlarmList1 = this;
			pathAlarmList1.AlarmPathDot = string.Concat(pathAlarmList1.AlarmPathDot, "*");
			return numArray;
		}

		public string ToXmlString(int OrderID, string Sim, string CarType, int CommFlag, string FunctionName, ref string content)
		{
			DateTime dateTime;
			DateTime dateTime1;
			StringBuilder stringBuilder = new StringBuilder();
			string membersXml = this.GetMembersXml(ref content);
			stringBuilder.Append("<?xml version='1.0' encoding='UTF-8'?>");
			stringBuilder.Append(string.Concat("<function name='", FunctionName, "' version='1.0.0'>"));
			stringBuilder.Append("<body>");
			stringBuilder.Append(string.Concat("<OrderID>", OrderID.ToString(), "</OrderID>"));
			stringBuilder.Append(string.Concat("<Sim>", Sim.ToString(), "</Sim>"));
			stringBuilder.Append(string.Concat("<CarType>", CarType.ToString(), "</CarType>"));
			stringBuilder.Append(string.Concat("<CmdCode>", (int)this.OrderCode, "</CmdCode>"));
			stringBuilder.Append(string.Concat("<CommFlag>", CommFlag.ToString(), "</CommFlag>"));
			stringBuilder.Append("<Parameter>");
			stringBuilder.Append(string.Concat("<PathNums>", this.Count, "</PathNums>"));
			stringBuilder.Append(string.Concat("<TotalNum>", this._PointCount, "</TotalNum>"));
			stringBuilder.Append(string.Concat("<PathFlag>", this.PathFlag, "</PathFlag>"));
			DateTime.TryParse(this.BeginTime, out dateTime);
			DateTime.TryParse(this.BeginTime, out dateTime1);
			stringBuilder.Append(string.Concat("<StarTime>", dateTime.ToString("yyMMddhhmmss"), "</StarTime>"));
			stringBuilder.Append(string.Concat("<EndTime>", dateTime1.ToString("yyMMddhhmmss"), "</EndTime>"));
			stringBuilder.Append(membersXml);
			stringBuilder.Append("</Parameter>");
			stringBuilder.Append("</body>");
			stringBuilder.Append("</function>");
			return stringBuilder.ToString();
		}
	}
}