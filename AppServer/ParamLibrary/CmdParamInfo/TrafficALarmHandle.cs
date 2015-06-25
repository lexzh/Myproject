using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class TrafficALarmHandle
	{
		public string CarId
		{
			get;
			set;
		}

		public string CreateTime
		{
			get;
			set;
		}

		public string GpsTime
		{
			get;
			set;
		}

		public int iProcMode
		{
			get;
			set;
		}

		public int OrderID
		{
			get;
			set;
		}

		public string ProcMode
		{
			get;
			set;
		}

		public string Remark
		{
			get;
			set;
		}

		public int WorkID
		{
			get;
			set;
		}

		public TrafficALarmHandle()
		{
		}
	}
}