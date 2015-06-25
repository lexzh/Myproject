using PublicClass;
using System;
using System.Runtime.CompilerServices;
using Library;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class RemoteDial : CmdParamBase
	{
		private static string MSGERROR;

		public object Phone
		{
			get
			{
				return this.getPhone();
			}
		}

		public string strMsg
		{
			get;
			set;
		}

		public string strPhone
		{
			get;
			set;
		}

		static RemoteDial()
		{
			RemoteDial.MSGERROR = "调度信息必须为1-175个以内的汉字和字符！";
		}

		public RemoteDial()
		{
		}

		public override int CheckData(out string strErrorMsg)
		{
			int num;
			strErrorMsg = "";
			if (!Check.CheckIsDigit(this.strPhone, 15))
			{
				strErrorMsg = "电话号码必须为15个字符以内的数字！";
				num = -1;
			}
			else if ((this.strMsg == null || string.IsNullOrEmpty(this.strMsg.Trim()) || this.strMsg.Trim().Length <= 0 ? false : this.strMsg.Trim().Length <= 175))
			{
				num = 0;
			}
			else
			{
				strErrorMsg = RemoteDial.MSGERROR;
				num = -1;
			}
			return num;
		}

		private string getPhone()
		{
			return this.strPhone.PadRight(15, '\0');
		}
	}
}