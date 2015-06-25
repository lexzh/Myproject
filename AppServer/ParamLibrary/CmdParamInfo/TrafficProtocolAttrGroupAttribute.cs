using System;

namespace ParamLibrary.CmdParamInfo
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class TrafficProtocolAttrGroupAttribute : Attribute
	{
		public string XmlTag = "";

		public string GroupName = "";

		public string OrderCodeList = "";

		public TrafficProtocolAttrGroupAttribute(string XmlTag, string OrderCodeList, string GroupName)
		{
			this.XmlTag = XmlTag;
			this.OrderCodeList = OrderCodeList;
			this.GroupName = GroupName;
		}

		public bool HasContainOrderCoder(string OrderCodeList)
		{
			return (this.OrderCodeList == OrderCodeList ? true : false);
		}
	}
}