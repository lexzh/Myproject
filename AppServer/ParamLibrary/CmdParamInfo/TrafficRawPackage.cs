using ParamLibrary.Application;
using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class TrafficRawPackage : CmdParamBase
	{
		public CmdParam.CmdCode CmdCode
		{
			get
			{
                return (CmdParam.CmdCode)this.SubOrderCode;
			}
		}

		public object pvArg
		{
			get;
			set;
		}

		[TrafficProtocol("Text", false, ",2563,", "")]
		public string strText
		{
			get;
			set;
		}

		[TrafficProtocol("SubCmdCode", true, ",2563,", "")]
		public CmdParam.OrderCode SubOrderCode
		{
			get;
			set;
		}

		public TrafficRawPackage()
		{
		}
	}
}