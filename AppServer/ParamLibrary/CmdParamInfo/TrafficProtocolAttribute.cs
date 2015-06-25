using System;

namespace ParamLibrary.CmdParamInfo
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	internal class TrafficProtocolAttribute : Attribute
	{
		public string XmlTag = "";

		public bool isEnum = false;

		public string desc = "";

		public string SourceValueField = "";

		public string OrderCodeList = "";

		public TrafficProtocolAttribute(string XmlTag)
		{
			this.XmlTag = XmlTag;
		}

		public TrafficProtocolAttribute(string XmlTag, bool isEnum)
		{
			this.XmlTag = XmlTag;
			this.isEnum = isEnum;
		}

		public TrafficProtocolAttribute(string XmlTag, bool isEnum, string OrderCodeList)
		{
			this.XmlTag = XmlTag;
			this.isEnum = isEnum;
			this.OrderCodeList = OrderCodeList;
		}

		public TrafficProtocolAttribute(string XmlTag, bool isEnum, string OrderCodeList, string desc)
		{
			this.XmlTag = XmlTag;
			this.isEnum = isEnum;
			this.OrderCodeList = OrderCodeList;
			this.desc = desc;
		}

		public TrafficProtocolAttribute(string XmlTag, bool isEnum, string OrderCodeList, string desc, string SourceValueField)
		{
			this.XmlTag = XmlTag;
			this.isEnum = isEnum;
			this.OrderCodeList = OrderCodeList;
			this.desc = desc;
			this.SourceValueField = SourceValueField;
		}
	}
}