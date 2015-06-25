using System;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public struct Point
	{
		public double Longitude;

		public double Latitude;

		public override string ToString()
		{
			string[] str = new string[] { "<Point><Lon>", this.Longitude.ToString(), "</Lon><Lat>", this.Latitude.ToString(), "</Lat></Point>" };
			return string.Concat(str);
		}
	}
}