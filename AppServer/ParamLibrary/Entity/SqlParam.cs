using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.Entity
{
	[Serializable]
	public class SqlParam
	{
		public string name
		{
			get;
			set;
		}

		public object @value
		{
			get;
			set;
		}

		public SqlParam()
		{
		}
	}
}