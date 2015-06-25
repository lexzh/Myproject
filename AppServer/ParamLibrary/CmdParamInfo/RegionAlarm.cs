using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class RegionAlarm : PathAlarm
	{
		public long AlarmCondition
		{
			get;
			set;
		}

		public string AlarmRegionDot
		{
			get;
			set;
		}

		public int newRegionId
		{
			get;
			set;
		}

		public int param
		{
			get;
			set;
		}

		public string PlanDownTime
		{
			get;
			set;
		}

		public string PlanUpTime
		{
			get;
			set;
		}

		public string RegionDot
		{
			get;
			set;
		}

		public int RegionID
		{
			get;
			set;
		}

		public int RegionType
		{
			get;
			set;
		}

		public int ShapeType
		{
			get;
			set;
		}

		public float toBackTime
		{
			get;
			set;
		}

		public float toEndTime
		{
			get;
			set;
		}

		public RegionAlarm()
		{
		}

		public new int CheckData(out string strErrorMsg)
		{
			return base.CheckData(out strErrorMsg);
		}

		public override string GetMembersXml(ref string content)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			string pointList = this.GetPointList(this.AlarmRegionDot, out num);
			if (this.ShapeType == 0)
			{
				content = string.Concat(content, base.PathName, ",");
			}
			stringBuilder.Append("<Region>");
			stringBuilder.Append(string.Concat("<RegionID>", this.newRegionId, "</RegionID>"));
			stringBuilder.Append(string.Concat("<RegionType>", this.ShapeType, "</RegionType>"));
			stringBuilder.Append(string.Concat("<AlarmFlag>", base.AlarmFlag, "</AlarmFlag>"));
			stringBuilder.Append(string.Concat("<MaxSpeed>", base.MaxSpeed, "</MaxSpeed>"));
			stringBuilder.Append(string.Concat("<HodeTime>", base.HodeTime, "</HodeTime>"));
			stringBuilder.Append(pointList);
			stringBuilder.Append(string.Concat("<Radius>", num, "</Radius>"));
			stringBuilder.Append(string.Concat("<StarTime>", base.BeginTime, "</StarTime>"));
			stringBuilder.Append(string.Concat("<EndTime>", base.EndTime, "</EndTime>"));
			stringBuilder.Append("</Region>");
			return stringBuilder.ToString();
		}

		private string GetPointList(string RegionDot, out int Radius)
		{
			string str;
			Radius = 0;
			StringBuilder stringBuilder = new StringBuilder();
			string[] strArrays = RegionDot.Split(new char[] { '\\' });
			if ((int)strArrays.Length == 4)
			{
				stringBuilder.Append("<Point>");
				stringBuilder.Append(string.Concat("<Lon>", strArrays[1], "</Lon>"));
				stringBuilder.Append(string.Concat("<Lat>", strArrays[2], "</Lat>"));
				stringBuilder.Append("</Point>");
				Radius = int.Parse(strArrays[3]);
				this.ShapeType = 1;
				str = stringBuilder.ToString();
			}
			else if (((int)strArrays.Length != 9 || !(strArrays[1] == strArrays[3]) || !(strArrays[4] == strArrays[6]) || !(strArrays[5] == strArrays[7]) ? true : !(strArrays[8] == strArrays[2])))
			{
				string empty = string.Empty;
				for (int i = 1; i <= (int)strArrays.Length - 2; i = i + 2)
				{
					stringBuilder.Append("<Point>");
					stringBuilder.Append(string.Concat("<Lon>", strArrays[i], "</Lon>"));
					stringBuilder.Append(string.Concat("<Lat>", strArrays[i + 1], "</Lat>"));
					stringBuilder.Append("</Point>");
				}
				Radius = 0;
				this.ShapeType = 3;
				str = stringBuilder.ToString();
			}
			else
			{
				this.ShapeType = 2;
				stringBuilder.Append("<Point>");
				stringBuilder.Append(string.Concat("<Lon>", strArrays[3], "</Lon>"));
				stringBuilder.Append(string.Concat("<Lat>", strArrays[4], "</Lat>"));
				stringBuilder.Append("</Point>");
				stringBuilder.Append("<Point>");
				stringBuilder.Append(string.Concat("<Lon>", strArrays[7], "</Lon>"));
				stringBuilder.Append(string.Concat("<Lat>", strArrays[8], "</Lat>"));
				stringBuilder.Append("</Point>");
				Radius = 0;
				this.ShapeType = 2;
				str = stringBuilder.ToString();
			}
			return str;
		}

		public int GetSharpe()
		{
			int num;
			string[] strArrays = this.AlarmRegionDot.Split(new char[] { '\\' });
			if ((int)strArrays.Length != 4)
			{
				if ((int)strArrays.Length != 9)
				{
					if ((int)strArrays.Length <= 5)
					{
						goto Label1;
					}
					num = 3;
					return num;
				}
				else if ((!(strArrays[1] == strArrays[3]) || !(strArrays[4] == strArrays[6]) || !(strArrays[5] == strArrays[7]) ? false : strArrays[8] == strArrays[2]))
				{
					num = 2;
					return num;
				}
			Label1:
				num = 3;
			}
			else
			{
				num = 1;
			}
			return num;
		}

		public override string ToXmlString(int OrderID, string Sim, string CarType, int CommFlag, string FunctionName, ref string content)
		{
			base.OrderID = OrderID;
			base.Sim = Sim;
			base.CarType = CarType;
			base.CommFlag = CommFlag;
			base.FunctionName = FunctionName;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<?xml version='1.0' encoding='UTF-8'?>");
			stringBuilder.Append(string.Concat("<function name='", base.FunctionName, "' version='1.0.0'>"));
			stringBuilder.Append("<body>");
			int orderID = base.OrderID;
			stringBuilder.Append(string.Concat("<OrderID>", orderID.ToString(), "</OrderID>"));
			stringBuilder.Append(string.Concat("<Sim>", base.Sim.ToString(), "</Sim>"));
			stringBuilder.Append(string.Concat("<CarType>", base.CarType.ToString(), "</CarType>"));
			stringBuilder.Append(string.Concat("<CmdCode>", (int)base.OrderCode, "</CmdCode>"));
			orderID = base.CommFlag;
			stringBuilder.Append(string.Concat("<CommFlag>", orderID.ToString(), "</CommFlag>"));
			stringBuilder.Append("<Parameter>");
			stringBuilder.Append("<Type>0</Type>");
			stringBuilder.Append(this.GetMembersXml(ref content));
			stringBuilder.Append("</Parameter>");
			stringBuilder.Append("</body>");
			stringBuilder.Append("</function>");
			return stringBuilder.ToString();
		}
	}
}