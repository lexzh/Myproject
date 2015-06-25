using ParamLibrary.Application;
using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class CmdBase
	{
		public ParamLibrary.CmdParamInfo.BlackBox BlackBox
		{
			get;
			set;
		}

		public string carId
		{
			get;
			set;
		}

		public string carNum
		{
			get;
			set;
		}

		public CommArgs Comm
		{
			get;
			set;
		}

		public string Info
		{
			get;
			set;
		}

		public string LogInfo
		{
			get
			{
				string[] str = new string[] { "OrderId:", null, null, null, null, null };
				str[1] = this.OrderId.ToString();
				str[2] = " SimNum:";
				str[3] = this.SimNum;
				str[4] = " ";
				str[5] = this.Info;
				return string.Concat(str);
			}
		}

		public int OrderId
		{
			get;
			set;
		}

		public string PhoneNum
		{
			get;
			set;
		}

		public CmdParam.PhoneType PhoneType
		{
			get;
			set;
		}

		public string SimNum
		{
			get;
			set;
		}

		public byte Type
		{
			get;
			set;
		}

		public CmdBase()
		{
		}
	}
}