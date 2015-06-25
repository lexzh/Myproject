using ParamLibrary.Application;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class CmdParamBase
	{
		public const string Enocoding = "UTF-16";

		public const string Gb2312 = "gb2312";

		public int TransformCode = -1;

		private int _Orderid;

		private string _Sim;

		private string _CarType = "";

		private int _CommFlag;

		private string _FunctionName;

		public string CarType
		{
			get
			{
				return this._CarType;
			}
			set
			{
				this._CarType = value;
			}
		}

		public int CommFlag
		{
			get
			{
				return this._CommFlag;
			}
			set
			{
				this._CommFlag = value;
			}
		}

		public string FunctionName
		{
			get
			{
				return this._FunctionName;
			}
			set
			{
				this._FunctionName = value;
			}
		}

		public CmdParam.OrderCode OrderCode
		{
			get;
			set;
		}

		public int OrderID
		{
			get
			{
				return this._Orderid;
			}
			set
			{
				this._Orderid = value;
			}
		}

		public string Sim
		{
			get
			{
				return this._Sim;
			}
			set
			{
				this._Sim = value;
			}
		}

		public CmdParamBase()
		{
		}

		public virtual int CheckData(out string strErrorMsg)
		{
			strErrorMsg = "";
			return 0;
		}

		private string GetArrayProperty(TrafficProtocolAttrForArrayAttribute attr, MemberInfo m, int tmpOrderCoder, ref string content)
		{
			string str;
			if (attr.OrderCodelist.IndexOf(string.Concat(",", tmpOrderCoder.ToString(), ",")) >= 0)
			{
				ArrayList value = this.GetType().GetProperty(m.Name).GetValue(this, null) as ArrayList;
				StringBuilder stringBuilder = new StringBuilder();
				string item = TrafficProtocolAttrForArrayAttribute.DicOrderCode2XmlTag[tmpOrderCoder];
				string[] strArrays = new string[] { "," };
				string[] strArrays1 = item.Split(strArrays, StringSplitOptions.RemoveEmptyEntries);
				if (value != null)
				{
					for (int i = 0; i < value.Count; i++)
					{
						string[] item1 = value[i] as string[];
						int num = 0;
						while (true)
						{
							if ((num >= (int)item1.Length ? true : num >= (int)strArrays1.Length))
							{
								break;
							}
							strArrays = new string[] { "<", strArrays1[num], ">", item1[num], "</", strArrays1[num], ">" };
							stringBuilder.Append(string.Concat(strArrays));
							string str1 = content;
							strArrays = new string[] { str1, strArrays1[num], "-", item1[num], "," };
							content = string.Concat(strArrays);
							num++;
						}
					}
				}
				str = stringBuilder.ToString();
			}
			else
			{
				str = "";
			}
			return str;
		}

		private string GetGroupAttr(Dictionary<string, string> Group, string GroupName, ref string content)
		{
			string current;
			string str;
			if (Group.Count != 0)
			{
				string[] strArrays = new string[] { "," };
				string[] strArrays1 = strArrays;
				StringBuilder stringBuilder = new StringBuilder();
				string str1 = "";
				Dictionary<string, string>.KeyCollection.Enumerator enumerator = Group.Keys.GetEnumerator();
				try
				{
					if (enumerator.MoveNext())
					{
						current = enumerator.Current;
						str1 = current;
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				string[] strArrays2 = Group[str1].ToString().Split(strArrays1, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < (int)strArrays2.Length; i++)
				{
					stringBuilder.Append(string.Concat("<", GroupName, ">"));
					foreach (string currentKey in Group.Keys)
					{
                        string[] strArrays3 = Group[currentKey].ToString().Split(strArrays1, StringSplitOptions.RemoveEmptyEntries);
                        strArrays = new string[] { "<", currentKey, ">", strArrays3[i], "</", currentKey, ">" };
						stringBuilder.Append(string.Concat(strArrays));
					}
					stringBuilder.Append(string.Concat("</", GroupName, ">"));
				}
				str = stringBuilder.ToString();
			}
			else
			{
				str = "";
			}
			return str;
		}

		public virtual string GetMembersXml(ref string content)
		{
			int orderCode;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<Parameter>");
			MemberInfo[] members = this.GetType().GetMembers();
			if (this.TransformCode == -1)
			{
				orderCode = (int)this.OrderCode;
			}
			else
			{
				orderCode = this.TransformCode;
			}
			int num = orderCode;
			Dictionary<string, string> strs = new Dictionary<string, string>();
			string groupName = "";
			MemberInfo[] memberInfoArray = members;
			for (int i = 0; i < (int)memberInfoArray.Length; i++)
			{
				MemberInfo memberInfo = memberInfoArray[i];
				TrafficProtocolAttribute customAttribute = Attribute.GetCustomAttribute(memberInfo, typeof(TrafficProtocolAttribute)) as TrafficProtocolAttribute;
				TrafficProtocolAttrForArrayAttribute trafficProtocolAttrForArrayAttribute = Attribute.GetCustomAttribute(memberInfo, typeof(TrafficProtocolAttrForArrayAttribute)) as TrafficProtocolAttrForArrayAttribute;
				TrafficProtocolAttrGroupAttribute trafficProtocolAttrGroupAttribute = Attribute.GetCustomAttribute(memberInfo, typeof(TrafficProtocolAttrGroupAttribute)) as TrafficProtocolAttrGroupAttribute;
				if (customAttribute != null)
				{
					stringBuilder.Append(this.GetSingleProperty(customAttribute, memberInfo, num, ref content));
				}
				else if (trafficProtocolAttrForArrayAttribute != null)
				{
					stringBuilder.Append(this.GetArrayProperty(trafficProtocolAttrForArrayAttribute, memberInfo, num, ref content));
				}
				else if (trafficProtocolAttrGroupAttribute != null)
				{
					if (trafficProtocolAttrGroupAttribute.HasContainOrderCoder(num.ToString()))
					{
                        groupName = trafficProtocolAttrGroupAttribute.GroupName;
                        string str = this.GetType().GetProperty(memberInfo.Name).GetValue(this, null).ToString();
                        strs.Add(trafficProtocolAttrGroupAttribute.XmlTag, str);
                    }
				}
			}
			stringBuilder.Append(this.GetGroupAttr(strs, groupName, ref content));
			stringBuilder.Append("</Parameter>");
			return stringBuilder.ToString();
		}

		private string GetSingleProperty(TrafficProtocolAttribute attr, MemberInfo m, int tmpOrderCoder, ref string content)
		{
			string str;
			string[] strArrays;
			bool flag;
			if (string.IsNullOrEmpty(attr.OrderCodeList))
			{
				flag = true;
			}
			else
			{
				flag = (attr.OrderCodeList.IndexOf(string.Concat(",", tmpOrderCoder.ToString(), ",")) >= 0 ? true : attr.OrderCodeList == tmpOrderCoder.ToString());
			}
			if (flag)
			{
				string str1 = "";
				string str2 = "";
				string str3 = "";
				if (!attr.isEnum)
				{
					str2 = (this.GetType().GetProperty(m.Name).GetValue(this, null) == null ? "" : this.GetType().GetProperty(m.Name).GetValue(this, null).ToString());
				}
				else
				{
					str2 = ((this.GetType().GetProperty(m.Name).GetValue(this, null) == null ? 0 : (int)this.GetType().GetProperty(m.Name).GetValue(this, null))).ToString();
				}
				str1 = ((attr.OrderCodeList == string.Concat(",", tmpOrderCoder.ToString(), ",") ? false : !string.IsNullOrEmpty(attr.OrderCodeList)) ? this.GetXmlTag(attr.XmlTag, attr.OrderCodeList, tmpOrderCoder.ToString()) : attr.XmlTag);
				if (attr.SourceValueField.Length == 0)
				{
					str3 = str2;
				}
				else
				{
					str3 = (this.GetType().GetProperty(attr.SourceValueField).GetValue(this, null) == null ? "" : this.GetType().GetProperty(attr.SourceValueField).GetValue(this, null).ToString());
				}
				if (!string.IsNullOrEmpty(attr.desc))
				{
					string str4 = content;
					strArrays = new string[] { str4, attr.desc, "-", str3, "," };
					content = string.Concat(strArrays);
				}
				strArrays = new string[] { "<", str1, ">", str2, "</", str1, ">" };
				str = string.Concat(strArrays);
			}
			else
			{
				str = "";
			}
			return str;
		}

		public string GetXmlTag(string XmlTag, string OrderCodeList, string OrderCoder)
		{
			string str;
			string[] strArrays = new string[] { "," };
			string[] strArrays1 = XmlTag.Split(strArrays, StringSplitOptions.RemoveEmptyEntries);
			string[] strArrays2 = OrderCodeList.Split(strArrays, StringSplitOptions.RemoveEmptyEntries);
			int num = 0;
			while (true)
			{
				if (num >= (int)strArrays2.Length)
				{
					str = "";
					break;
				}
				else if (!(OrderCoder == strArrays2[num]))
				{
					num++;
				}
				else
				{
					str = strArrays1[num];
					break;
				}
			}
			return str;
		}

		public virtual string ToXmlString(int OrderID, string Sim, string CarType, int CommFlag, string FunctionName, ref string conntent)
		{
			this.OrderID = OrderID;
			this.Sim = Sim;
			this.CarType = CarType;
			this.CommFlag = CommFlag;
			this.FunctionName = FunctionName;
			return this.ToXmlString(ref conntent);
		}

		public virtual string ToXmlString(ref string content)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<?xml version='1.0' encoding='UTF-8'?>");
			stringBuilder.Append(string.Concat("<function name='", this.FunctionName, "' version='1.0.0'>"));
			stringBuilder.Append("<body>");
			int orderID = this.OrderID;
			stringBuilder.Append(string.Concat("<OrderID>", orderID.ToString(), "</OrderID>"));
			stringBuilder.Append(string.Concat("<Sim>", this.Sim.ToString(), "</Sim>"));
			stringBuilder.Append(string.Concat("<CarType>", this.CarType.ToString(), "</CarType>"));
			if (this.TransformCode == -1)
			{
				stringBuilder.Append(string.Concat("<CmdCode>", (int)this.OrderCode, "</CmdCode>"));
			}
			else
			{
				stringBuilder.Append(string.Concat("<CmdCode>", this.TransformCode, "</CmdCode>"));
			}
			orderID = this.CommFlag;
			stringBuilder.Append(string.Concat("<CommFlag>", orderID.ToString(), "</CommFlag>"));
			stringBuilder.Append(this.GetMembersXml(ref content));
			stringBuilder.Append("</body>");
			stringBuilder.Append("</function>");
			return stringBuilder.ToString();
		}
	}
}