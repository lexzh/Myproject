using System;
using System.Collections.Generic;

namespace ParamLibrary.CmdParamInfo
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	internal class TrafficProtocolAttrForArrayAttribute : Attribute
	{
		public static Dictionary<int, string> DicOrderCode2XmlTag;

		public string OrderCodelist = "";

		static TrafficProtocolAttrForArrayAttribute()
		{
			TrafficProtocolAttrForArrayAttribute.DicOrderCode2XmlTag = new Dictionary<int, string>();
			if (TrafficProtocolAttrForArrayAttribute.DicOrderCode2XmlTag.Count == 0)
			{
				TrafficProtocolAttrForArrayAttribute.DicOrderCode2XmlTag.Add(335, "Phone");
				TrafficProtocolAttrForArrayAttribute.DicOrderCode2XmlTag.Add(8199, "Type");
				TrafficProtocolAttrForArrayAttribute.DicOrderCode2XmlTag.Add(8273, "Type");
				TrafficProtocolAttrForArrayAttribute.DicOrderCode2XmlTag.Add(8274, "V");
				TrafficProtocolAttrForArrayAttribute.DicOrderCode2XmlTag.Add(1030, "ID");
				TrafficProtocolAttrForArrayAttribute.DicOrderCode2XmlTag.Add(16422, "ID");
			}
		}

		public TrafficProtocolAttrForArrayAttribute(string OrderCodelist)
		{
			this.OrderCodelist = OrderCodelist;
		}
	}
}