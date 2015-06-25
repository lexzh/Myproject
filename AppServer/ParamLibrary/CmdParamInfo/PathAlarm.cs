using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class PathAlarm : CmdParamBase
	{
		public List<PathSegmentAlarm> PathSegmentAlarmList = new List<PathSegmentAlarm>();

		public int _PathSegmentPointCount
		{
			get;
			set;
		}

		public int AlarmFlag
		{
			get;
			set;
		}

		public string BeginTime
		{
			get;
			set;
		}

		public int DriEnough
		{
			get;
			set;
		}

		public int DriNoEnough
		{
			get;
			set;
		}

		public string EndTime
		{
			get;
			set;
		}

		public int? HodeTime
		{
			get;
			set;
		}

		public int ID
		{
			get;
			set;
		}

		public int? MaxSpeed
		{
			get;
			set;
		}

		public int ParentID
		{
			get;
			set;
		}

		public string PathDif
		{
			get;
			set;
		}

		public int PathFlag
		{
			get;
			set;
		}

		public string PathName
		{
			get;
			set;
		}

		public object pKeyPosArray
		{
			get
			{
				return this.GetKeyPosArray();
			}
		}

		public int PointCount
		{
			get;
			set;
		}

		public ArrayList Points
		{
			get;
			set;
		}

		public string sBeginTime
		{
			get;
			set;
		}

		public string sEndTime
		{
			get;
			set;
		}

		public byte Speed
		{
			get;
			set;
		}

		public byte Time
		{
			get;
			set;
		}

		public PathAlarm()
		{
		}

		public override int CheckData(out string strErrorMsg)
		{
			return base.CheckData(out strErrorMsg);
		}

		private double[] GetKeyPosArray()
		{
			double[] numArray;
			if (this.Points != null)
			{
				double[] longitude = new double[this.Points.Count * 2];
				for (int i = 0; i < this.Points.Count; i++)
				{
					Point item = (Point)this.Points[i];
					longitude[i * 2] = item.Longitude;
					longitude[i * 2 + 1] = item.Latitude;
				}
				numArray = longitude;
			}
			else
			{
				numArray = new double[0];
			}
			return numArray;
		}

		public override string GetMembersXml(ref string content)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.PathSegmentAlarmList.Count; i++)
			{
				PathSegmentAlarm item = this.PathSegmentAlarmList[i];
				content = string.Concat(content, this.PathName, ",");
				stringBuilder.Append("<Segment>");
				stringBuilder.Append(string.Concat("<SegmentID>", item.PathSegmentID, "</SegmentID>"));
				stringBuilder.Append(string.Concat("<DriEnough>", item.DriEnough, "</DriEnough>"));
				stringBuilder.Append(string.Concat("<DriNoEnough>", item.DriNoEnough, "</DriNoEnough>"));
				stringBuilder.Append(string.Concat("<SegmentPointNums>", item.Points.Count, "</SegmentPointNums>"));
				stringBuilder.Append(string.Concat("<SegmentWidth>", item.PathWidth, "</SegmentWidth>"));
				for (int j = 0; j < item.Points.Count; j++)
				{
					Point point = (Point)item.Points[j];
					stringBuilder.Append(point.ToString());
				}
				PathAlarm count = this;
				count._PathSegmentPointCount = count._PathSegmentPointCount + item.Points.Count;
				stringBuilder.Append("<SegmentType>1</SegmentType>");
				stringBuilder.Append(string.Concat("<AlarmSpeed>", item.TopSpeed, "</AlarmSpeed>"));
				stringBuilder.Append(string.Concat("<HoldTime>", item.HoldTime, "</HoldTime>"));
				stringBuilder.Append(string.Concat("<SegmentFlag>", item.Flag, "</SegmentFlag>"));
				stringBuilder.Append("</Segment>");
			}
			return stringBuilder.ToString();
		}

		public override string ToXmlString(int OrderID, string Sim, string CarType, int CommFlag, string FunctionName, ref string content)
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
			stringBuilder.Append(string.Concat("<CmdCode>", (int)base.OrderCode, "</CmdCode>"));
			stringBuilder.Append(string.Concat("<CommFlag>", CommFlag.ToString(), "</CommFlag>"));
			stringBuilder.Append("<Parameter>");
			stringBuilder.Append(string.Concat("<TotalNum>", this._PathSegmentPointCount, "</TotalNum>"));
			stringBuilder.Append(string.Concat("<PathNums>", this.PathSegmentAlarmList.Count, "</PathNums>"));
			stringBuilder.Append(string.Concat("<PathFlag>", this.PathFlag, "</PathFlag>"));
			stringBuilder.Append(string.Concat("<PathID>", this.ID, "</PathID>"));
			bool flag = DateTime.TryParse(this.BeginTime, out dateTime);
			DateTime.TryParse(this.EndTime, out dateTime1);
			stringBuilder.Append(string.Concat("<StarTime>", (!flag ? "000000000000" : dateTime.ToString("yyMMddHHmmss")), "</StarTime>"));
			stringBuilder.Append(string.Concat("<EndTime>", (!flag ? "000000000000" : dateTime1.ToString("yyMMddHHmmss")), "</EndTime>"));
			stringBuilder.Append(membersXml);
			stringBuilder.Append("</Parameter>");
			stringBuilder.Append("</body>");
			stringBuilder.Append("</function>");
			return stringBuilder.ToString();
		}
	}
}