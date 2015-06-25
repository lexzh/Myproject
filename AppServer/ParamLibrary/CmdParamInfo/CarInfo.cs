using ParamLibrary.Application;
using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class CarInfo : CmdParamBase
	{
		public new string CarType
		{
			get;
			set;
		}

		public string CarVer
		{
			get;
			set;
		}

		public CmdParam.CommModule CommModule
		{
			get;
			set;
		}

		public string ReportCenter
		{
			get;
			set;
		}

		public string Reserve
		{
			get;
			set;
		}

		public CarInfo()
		{
		}

		public override int CheckData(out string strErrorMsg)
		{
			return base.CheckData(out strErrorMsg);
		}
	}
}