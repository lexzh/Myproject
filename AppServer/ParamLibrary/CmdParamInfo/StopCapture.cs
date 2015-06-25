using ParamLibrary.Application;
using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class StopCapture : CmdParamBase
	{
		public CarProtocolType protocolType = CarProtocolType.非交通厅;

		[TrafficProtocol("ID")]
		public byte CamerasID
		{
			get;
			set;
		}

		[TrafficProtocol("Flag")]
		public int Flag1
		{
			get;
			set;
		}

		[TrafficProtocol("FlagEx")]
		public int Flag2
		{
			get;
			set;
		}

		public StopCapture()
		{
		}
	}
}