using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class Capture : CmdParamBase
	{
		[TrafficProtocol("Brightness")]
		public byte Brightness
		{
			get;
			set;
		}

		[TrafficProtocol("CamerasID")]
		public byte CamerasID
		{
			get;
			set;
		}

		[TrafficProtocol("CaptureCache")]
		public int CaptureCache
		{
			get;
			set;
		}

		[TrafficProtocol("CaptureFlag")]
		public int CaptureFlag
		{
			get;
			set;
		}

		[TrafficProtocol("Chroma")]
		public byte Chroma
		{
			get;
			set;
		}

		[TrafficProtocol("Contrast")]
		public byte Contrast
		{
			get;
			set;
		}

		public int Interval
		{
			get;
			set;
		}

		[TrafficProtocol("Interval")]
		public int IntervalTarget
		{
			get
			{
				return this.Interval / 10;
			}
		}

		[TrafficProtocol("IsMulFrame")]
		public byte IsMultiFrame
		{
			get;
			set;
		}

		[TrafficProtocol("Quality")]
		public byte Quality
		{
			get;
			set;
		}

		public int ReserveFlag
		{
			get;
			set;
		}

		[TrafficProtocol("Saturation")]
		public byte Saturation
		{
			get;
			set;
		}

		[TrafficProtocol("Times")]
		public int Times
		{
			get;
			set;
		}

		public Capture()
		{
		}

		public override int CheckData(out string strErrorMsg)
		{
			return base.CheckData(out strErrorMsg);
		}
	}
}