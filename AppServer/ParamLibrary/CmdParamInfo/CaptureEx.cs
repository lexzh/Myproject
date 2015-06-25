using ParamLibrary.Application;
using System;
using System.Runtime.CompilerServices;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class CaptureEx : Capture
	{
		public static string MoniterTimes;

		public static string MoniterInterval;

		public static string ImgQuality;

		public static string ImgLight;

		public static string ImgContrast;

		public static string ImgSaturation;

		public static string ImgColor;

		public CarProtocolType protocolType = CarProtocolType.非交通厅;

		public string BeginTime
		{
			get;
			set;
		}

		[TrafficProtocol("CapWhenStop")]
		public int CapWhenStop
		{
			get;
			set;
		}

		public string EndTime
		{
			get;
			set;
		}

		public string IsCapWhenStop
		{
			get
			{
				return (this.CapWhenStop != 1 ? "是" : "否");
			}
		}

		public string IsMulitFramebool
		{
			get
			{
				return (base.IsMultiFrame != 1 ? "否" : "是");
			}
		}

		[TrafficProtocol("PSize")]
		public int PSize
		{
			get;
			set;
		}

		[TrafficProtocol("Type")]
		public int Type
		{
			get;
			set;
		}

		static CaptureEx()
		{
			CaptureEx.MoniterTimes = "监控次数必须为0-65535之间的数字！";
			CaptureEx.MoniterInterval = "监控时间间隔必须为1-65535之间的数字!";
			CaptureEx.ImgQuality = "图像质量必须为0-5之间的数字";
			CaptureEx.ImgLight = "图像亮度必须为0-255之间的数字";
			CaptureEx.ImgContrast = "对比度必须为0-127之间的数字";
			CaptureEx.ImgSaturation = "饱和度必须为0-127之间的数字";
			CaptureEx.ImgColor = "色度必须为0-255之间的数字";
		}

		public CaptureEx()
		{
		}

		public override int CheckData(out string strErrorMsg)
		{
			int num;
			strErrorMsg = string.Empty;
			if (!(base.Times < 0 ? false : base.Times <= 65535))
			{
				strErrorMsg = CaptureEx.MoniterTimes;
				num = -1;
			}
			else if (!(base.Interval < 0 ? false : base.Interval <= 65535))
			{
				strErrorMsg = CaptureEx.MoniterInterval;
				num = -1;
			}
			else if (!(base.Brightness < 0 ? false : base.Brightness <= 255))
			{
				strErrorMsg = CaptureEx.ImgLight;
				num = -1;
			}
			else if (!(base.Contrast < 0 ? false : base.Contrast <= 127))
			{
				strErrorMsg = CaptureEx.ImgContrast;
				num = -1;
			}
			else if (!(base.Saturation < 0 ? false : base.Saturation <= 127))
			{
				strErrorMsg = CaptureEx.ImgSaturation;
				num = -1;
			}
			else if ((base.Chroma < 0 ? false : base.Chroma <= 255))
			{
				num = 0;
			}
			else
			{
				strErrorMsg = CaptureEx.ImgColor;
				num = -1;
			}
			return num;
		}

        public string ProtocolName { get; set; }
    }
}