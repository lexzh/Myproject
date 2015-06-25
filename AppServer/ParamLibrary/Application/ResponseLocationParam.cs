using System;

namespace ParamLibrary.Application
{
	[Serializable]
	public class ResponseLocationParam
	{
		public double dLongitude = 0;

		public double dLatitude = 0;

		public string sUserName = "";

		public string sAddress = "";

		public ResponseLocationParam()
		{
		}
	}
}