using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.Entity
{
	[Serializable]
	public class Response
	{
		private long _ResultCode = (long)-1;

		public string ErrorMsg
		{
			get;
			set;
		}

		public string OrderIDParam
		{
			get;
			set;
		}

		public long ResultCode
		{
			get
			{
				return this._ResultCode;
			}
			set
			{
				this._ResultCode = value;
			}
		}

		public string SvcContext
		{
			get;
			set;
		}

		public Response()
		{
		}
	}
}