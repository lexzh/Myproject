using ParamLibrary.Application;
using System;

namespace ParamLibrary.Bussiness
{
	[Serializable]
	public class AppRequest
	{
		private string bizCodeField;

		private string transIDField;

		private int actionCodeField;

		private string timeStampField;

		private string sIAppIDField;

		private int testFlagField;

		private int dealkindField;

		private int priorityField;

		private string versionField;

		private string svcContField;

		private string[] paramContField;

		private CmdParam.ParamType paramTypeField;

		private string carValuesField;

		private string carPwField;

		private CmdParam.CommMode commModeField;

		private CmdParam.OrderCode orderCodeField;

		private string cmdDesField = string.Empty;

		public int ActionCode
		{
			get
			{
				return this.actionCodeField;
			}
			set
			{
				this.actionCodeField = value;
			}
		}

		public string BizCode
		{
			get
			{
				return this.bizCodeField;
			}
			set
			{
				this.bizCodeField = value;
			}
		}

		public string CarPw
		{
			get
			{
				return this.carPwField;
			}
			set
			{
				this.carPwField = value;
			}
		}

		public string CarValues
		{
			get
			{
				return this.carValuesField;
			}
			set
			{
				this.carValuesField = value;
			}
		}

		public CmdParam.CmdCode CmdCode
		{
			get
			{
                return (CmdParam.CmdCode)this.OrderCode;
			}
		}

		public string CmdCodeDes
		{
			get
			{
				return this.OrderCode.ToString();
			}
		}

		public CmdParam.CommMode CommMode
		{
			get
			{
				return this.commModeField;
			}
			set
			{
				this.commModeField = value;
			}
		}

		public int Dealkind
		{
			get
			{
				return this.dealkindField;
			}
			set
			{
				this.dealkindField = value;
			}
		}

		public CmdParam.OrderCode OrderCode
		{
			get
			{
				return this.orderCodeField;
			}
			set
			{
				this.orderCodeField = value;
			}
		}

		public string[] ParamCont
		{
			get
			{
				return this.paramContField;
			}
			set
			{
				this.paramContField = value;
			}
		}

		public CmdParam.ParamType ParamType
		{
			get
			{
				return this.paramTypeField;
			}
			set
			{
				this.paramTypeField = value;
			}
		}

		public int Priority
		{
			get
			{
				return this.priorityField;
			}
			set
			{
				this.priorityField = value;
			}
		}

		public string SIAppID
		{
			get
			{
				return this.sIAppIDField;
			}
			set
			{
				this.sIAppIDField = value;
			}
		}

		public string SvcCont
		{
			get
			{
				return this.svcContField;
			}
			set
			{
				this.svcContField = value;
			}
		}

		public int TestFlag
		{
			get
			{
				return this.testFlagField;
			}
			set
			{
				this.testFlagField = value;
			}
		}

		public string TimeStamp
		{
			get
			{
				return this.timeStampField;
			}
			set
			{
				this.timeStampField = value;
			}
		}

		public string TransID
		{
			get
			{
				return this.transIDField;
			}
			set
			{
				this.transIDField = value;
			}
		}

		public string Version
		{
			get
			{
				return this.versionField;
			}
			set
			{
				this.versionField = value;
			}
		}

		public AppRequest()
		{
			this.ActionCode = 1;
			this.TestFlag = 0;
		}
	}
}