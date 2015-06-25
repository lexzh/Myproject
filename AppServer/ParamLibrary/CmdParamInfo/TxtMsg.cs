using ParamLibrary.Application;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class TxtMsg : CmdParamBase
	{
		private static string MSGERROR;

		[TrafficProtocol("LLat,LLat,LLat", false, ",768,770,773,", "左上角纬度")]
		public string LLat
		{
			get;
			set;
		}

		[TrafficProtocol("LLon,LLon,LLon", false, ",768,770,773,", "左上角经度")]
		public string LLon
		{
			get;
			set;
		}

		[TrafficProtocol("MsgType,MsgType,MsgType", true, ",768,770,773,")]
		public CmdParam.MsgType MsgType
		{
			get;
			set;
		}

		[TrafficProtocol("Orderid,Orderid,Orderid", false, ",768,770,773,", "定单号")]
		public string Orderid
		{
			get;
			set;
		}

		[TrafficProtocol("RLat,RLat,RLat", false, ",768,770,773,", "右上角纬度")]
		public string RLat
		{
			get;
			set;
		}

		[TrafficProtocol("RLon,RLon,RLon", false, ",768,770,773,", "右上角经度")]
		public string RLon
		{
			get;
			set;
		}

        public string CarId { get; set; }

		public string sCarName
		{
			get;
			set;
		}

		public string sPhone
		{
			get;
			set;
		}

		public string strMsg
		{
			get;
			set;
		}

		[TrafficProtocol("Text,Text,Text", false, ",1538,768,16404,")]
		public string strMsgTarget
		{
			get
			{
				return this.GetGB2312(this.strMsg);
			}
		}

		[TrafficProtocol("TelNumber,TelNumber", false, ",768,773,", "")]
		public string TelNumber
		{
			get;
			set;
		}

		[TrafficProtocol("Text,Text,Text", false, ",768,770,773,", "信息内容")]
		public string Text
		{
			get
			{
				return this.strMsgTarget;
			}
		}

		[TrafficProtocol("Way,Way,Way", false, ",768,770,773,", "抢答方式")]
		public string Way
		{
			get;
			set;
		}

		static TxtMsg()
		{
			TxtMsg.MSGERROR = "调度信息必须为1-175个以内的汉字和字符！";
		}

		public TxtMsg()
		{
		}

		public override int CheckData(out string strErrorMsg)
		{
			return this.CheckParam(out strErrorMsg);
		}

		public int CheckParam(out string strErrorMsg)
		{
			int num;
			strErrorMsg = string.Empty;
			int num1 = 0;
			if ((this.strMsg == null || string.IsNullOrEmpty(this.strMsg.Trim()) || this.strMsg.Trim().Length <= 0 ? false : this.strMsg.Trim().Length <= 175))
			{
				num = num1;
			}
			else
			{
				strErrorMsg = TxtMsg.MSGERROR;
				int num2 = -1;
				num1 = num2;
				num = num2;
			}
			return num;
		}

		public string GetGB2312(string source)
		{
			StringBuilder stringBuilder = new StringBuilder();
			byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(source);
			for (int i = 0; i < (int)bytes.Length; i++)
			{
				byte num = bytes[i];
				stringBuilder.Append(num.ToString("X2"));
			}
			return stringBuilder.ToString();
		}

        public string SimNum { get; set; }
    }
}