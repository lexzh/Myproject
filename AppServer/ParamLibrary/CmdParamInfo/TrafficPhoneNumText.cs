using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class TrafficPhoneNumText : CmdParamBase
	{
		[TrafficProtocolAttrGroup("Flag", "16407", "Lman")]
		public string FlagList
		{
			get;
			set;
		}

		public string m_Params
		{
			get;
			set;
		}

		public string m_ParamsReport
		{
			get;
			set;
		}

		[TrafficProtocolAttrGroup("Name", "16407", "Lman")]
		public string NameList
		{
			get;
			set;
		}

		[TrafficProtocol("Nums")]
		public int Nums
		{
			get;
			set;
		}

		[TrafficProtocolAttrGroup("Phone", "16407", "Lman")]
		public string PhoneListList
		{
			get;
			set;
		}

		[TrafficProtocol("Type")]
		public int Type
		{
			get;
			set;
		}

		public TrafficPhoneNumText()
		{
		}
	}
}