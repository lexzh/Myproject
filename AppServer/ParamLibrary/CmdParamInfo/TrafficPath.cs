using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class TrafficPath
	{
		public string[][] PathSegments;

		public string buildingSitName
		{
			get;
			set;
		}

		public string factoryName
		{
			get;
			set;
		}

		public bool isNewPath
		{
			get;
			set;
		}

		public double lat_BuildingSit
		{
			get;
			set;
		}

		public double lat_Factory
		{
			get;
			set;
		}

		public double lon_BuildingSit
		{
			get;
			set;
		}

		public double lon_Factory
		{
			get;
			set;
		}

		public int pathgroupID
		{
			get;
			set;
		}

		public int PathId
		{
			get;
			set;
		}

		public string pathName
		{
			get;
			set;
		}

		public string pathStr
		{
			get;
			set;
		}

		public int pathType
		{
			get;
			set;
		}

		public int region_Radius
		{
			get;
			set;
		}

		public string remark
		{
			get;
			set;
		}

		public TrafficPath()
		{
		}
	}
}