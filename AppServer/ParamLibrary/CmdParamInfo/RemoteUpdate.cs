using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class RemoteUpdate : CmdParamBase
	{
		public byte[] pvDataPack
		{
			get;
			set;
		}

		public string strFileID
		{
			get;
			set;
		}

		public RemoteUpdate()
		{
		}

		public override int CheckData(out string strErrorMsg)
		{
			return base.CheckData(out strErrorMsg);
		}
	}
}