using System;

namespace ParamLibrary.Bussiness
{
	[Serializable]
	public class AppRespone
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

		private int resultCodeField;

		private string resultMsgField;

		private string svcContField;

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

		public int ResultCode
		{
			get
			{
				return this.resultCodeField;
			}
			set
			{
				this.resultCodeField = value;
			}
		}

		public string ResultMsg
		{
			get
			{
				return this.resultMsgField;
			}
			set
			{
				this.resultMsgField = value;
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

		public AppRespone()
		{
			this.ActionCode = 2;
			this.TestFlag = 0;
			this.ResultCode = 0;
		}

		public class Result
		{
			public const int Fail = -1;

			public const int Success = 0;

			public Result()
			{
			}
		}
	}
}