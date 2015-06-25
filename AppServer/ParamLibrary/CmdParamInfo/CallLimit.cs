using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class CallLimit : CmdParamBase
	{
		public ArrayList CallInList
		{
			get;
			set;
		}

		public object CallInPhone
		{
			get
			{
				return this.getPhoneList(this.CallInList);
			}
		}

		public string CallInPhoneString
		{
			get
			{
				return this.getPhoneString(this.CallInList);
			}
		}

		public ArrayList CallOutList
		{
			get;
			set;
		}

		public object CallOutPhone
		{
			get
			{
				return this.getPhoneList(this.CallOutList);
			}
		}

		public string CallOutPhoneString
		{
			get
			{
				return this.getPhoneString(this.CallOutList);
			}
		}

		public int FlagMask
		{
			get
			{
				return this.GetOrValue(this.FlagMaskList);
			}
		}

		public ArrayList FlagMaskList
		{
			get;
			set;
		}

		public int FlagValue
		{
			get
			{
				return this.GetOrValue(this.FlagValueList);
			}
		}

		public ArrayList FlagValueList
		{
			get;
			set;
		}

		public CallLimit()
		{
			this.FlagValueList = new ArrayList();
			this.FlagMaskList = new ArrayList();
			this.CallInList = new ArrayList();
			this.CallOutList = new ArrayList();
		}

		public override int CheckData(out string strErrorMsg)
		{
			return base.CheckData(out strErrorMsg);
		}

		private int GetOrValue(ArrayList list)
		{
			int num;
			int item = 0;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					item = item | (int)list[i];
				}
				num = item;
			}
			else
			{
				num = item;
			}
			return num;
		}

		private object getPhoneList(ArrayList phoneList)
		{
			object obj;
			if ((phoneList == null ? false : phoneList.Count != 0))
			{
				byte[] numArray = new byte[phoneList.Count * 15];
				for (int i = 0; i < phoneList.Count; i++)
				{
					byte[] bytes = new byte[15];
					bytes = Encoding.GetEncoding("gb2312").GetBytes((string)phoneList[i]);
					bytes.CopyTo(numArray, i * 15);
					for (int j = i * 15 + (int)bytes.Length; j < (i + 1) * 15 - 1; j++)
					{
						numArray[j] = 0;
					}
				}
				obj = numArray;
			}
			else
			{
				obj = null;
			}
			return obj;
		}

		private string getPhoneString(ArrayList phoneList)
		{
			string str;
			string str1 = "";
			if ((phoneList == null ? false : phoneList.Count != 0))
			{
				for (int i = 0; i < phoneList.Count; i++)
				{
					str1 = string.Concat(str1, (string)phoneList[i], "/");
				}
				str = str1.Trim(new char[] { '/' });
			}
			else
			{
				str = str1;
			}
			return str;
		}
	}
}