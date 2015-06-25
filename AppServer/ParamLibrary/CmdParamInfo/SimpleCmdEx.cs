using ParamLibrary.Application;
using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class SimpleCmdEx : SimpleCmd
	{
		public new CmdParam.CommMode CommFlag
		{
			get;
			set;
		}

		public SimpleCmdEx()
		{
		}
	}
}