using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class PathSegmentAlarm
	{
        public int PathSegmentDataBaseID;
		public int? DriEnough
		{
			get;
			set;
		}

		public int? DriNoEnough
		{
			get;
			set;
		}

		public int Flag
		{
			get;
			set;
		}

		public int? HoldTime
		{
			get;
			set;
		}

		public int PathId
		{
			get;
			set;
		}

		public int PathSegmentID
		{
			get;
			set;
		}

		public int PathWidth
		{
			get;
			set;
		}

		public ArrayList Points
		{
			get;
			set;
		}

		public int? TopSpeed
		{
			get;
			set;
		}

		public PathSegmentAlarm()
		{
		}
	}
}