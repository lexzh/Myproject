using System;

namespace ParamLibrary.Bussiness
{
	[Serializable]
	public struct CarTrackInfo
	{
		public int WorkId;

		public string SimNum;

		public bool IsShowTrack;

		public int ReportType;

		public string CarColor;

		public bool IsTracking;

		public int status;
	}
}