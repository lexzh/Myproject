using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class TrafficSimpleCmd : CmdParamBase
	{
		[TrafficProtocol("Flag", false, ",16430,", "确认报警标志")]
		public int AlarmFlagNeedConfirm
		{
			get;
			set;
		}

		[TrafficProtocol("ID", false, ",16430,", "报警消息流水号")]
		public int AlarmIDNeedConfirm
		{
			get;
			set;
		}

		[TrafficProtocol("Angle", false, ",16427,", "角度")]
		public int Angle
		{
			get;
			set;
		}

		[TrafficProtocol("APN", false, ",16400,", "APN")]
		public string APN
		{
			get;
			set;
		}

		[TrafficProtocol("APN", false, ",16414,", " 拨号点名称")]
		public string APNAddr
		{
			get;
			set;
		}

		[TrafficProtocol("AuthCode", false, ",16414,", "鉴权码")]
		public string AuthCode
		{
			get;
			set;
		}

		[TrafficProtocol(",BeginTime,BeginTime,BeginTime,BeginTime,BeginTime,BeginTime,BeginTime,BeginTime,", false, ",8451,8456,8457,8458,8459,8460,8461,8462,", "开始时间")]
		public string BeginTime2012
		{
			get;
			set;
		}

		[TrafficProtocol("CAN1", false, ",16420,", "CAN1采集间隔")]
		public uint CAN1AcSpan
		{
			get;
			set;
		}

		[TrafficProtocol("CAN2", false, ",16420,", "CAN1上传间隔")]
		public int CAN1UpSpan
		{
			get;
			set;
		}

		[TrafficProtocol("CAN3", false, ",16420,", "CAN2采集间隔")]
		public uint CAN2AcSpan
		{
			get;
			set;
		}

		[TrafficProtocol("CAN4", false, ",16420,", "CAN2上传间隔")]
		public int CAN2UpSpan
		{
			get;
			set;
		}

		[TrafficProtocol("Time", false, ",16423,", "CANID采集时间间隔")]
		public uint CanIDAcSpan
		{
			get;
			set;
		}

		[TrafficProtocol("CanIDFlag", false, ",16423,", "CANID页属性")]
		public uint CanIDFlag
		{
			get;
			set;
		}

		[TrafficProtocol("CarNUM", false, "8706", "车牌号")]
		public string CarNUM
		{
			get;
			set;
		}

		[TrafficProtocol("CarNum", false, ",8708,", "车牌号")]
		public string CarNum2012
		{
			get;
			set;
		}

		[TrafficProtocol("CarNum", false, ",16399,", "车牌")]
		public string CarNumber
		{
			get;
			set;
		}

		[TrafficProtocol("CColor", false, ",16399,", "车牌颜色")]
		public int CColor
		{
			get;
			set;
		}

		[TrafficProtocol("ID", false, ",16411,", "媒体类型通道号")]
		public int ChanelID
		{
			get;
			set;
		}

		[TrafficProtocol("Type", false, ",16386,", "通道类型")]
		public int ChannelType
		{
			get;
			set;
		}

		[TrafficProtocol("CID", false, ",16398,", "市ID")]
		public int CID
		{
			get;
			set;
		}

		public object CommonArgs
		{
			get;
			set;
		}

		[TrafficProtocol("IP", false, ",16414,", " 服务器地址")]
		public string ConnectionIP
		{
			get;
			set;
		}

		[TrafficProtocol("Pwd", false, ",16414,", " 拨号密码")]
		public string ConnectionPassword
		{
			get;
			set;
		}

		[TrafficProtocol("TOut", false, ",16414,", "有效时间")]
		public int ConnectionTout
		{
			get;
			set;
		}

		[TrafficProtocol("Type", false, ",16414,", "连接控制")]
		public int ConnectionType
		{
			get;
			set;
		}

		[TrafficProtocol("User", false, ",16414,", " 拨号用户名")]
		public string ConnectionUser
		{
			get;
			set;
		}

		[TrafficProtocol("DCode", false, "8705", "驾驶员代码")]
		public long DCode
		{
			get;
			set;
		}

		[TrafficProtocol("Flag", false, "16418", "删除标志")]
		public int DeleteFlag
		{
			get;
			set;
		}

		[TrafficProtocol("DIF", false, ",16426,", "预警差值")]
		public int DIF
		{
			get;
			set;
		}

		[TrafficProtocol("DIndexKey", false, "8705", "驾驶证号码")]
		public string DIndexKey
		{
			get;
			set;
		}

		[TrafficProtocol("BackupsIP", false, ",16687,", ",备份服务器IP,")]
		public string DRIC_BackupsIP
		{
			get;
			set;
		}

		[TrafficProtocol("TCPIP", false, ",16687,", "认证主服务器IP")]
		public string DRIC_TCPIP
		{
			get;
			set;
		}

		[TrafficProtocol("TCPPort", false, ",16687,", "TCP端口")]
		public int DRIC_TCPPort
		{
			get;
			set;
		}

		[TrafficProtocol("UDPPort", false, ",16687,", "UDP端口")]
		public int DRIC_UDPPort
		{
			get;
			set;
		}

		[TrafficProtocol("Time", false, ",16396,", "当天累计驾驶门限")]
		public int DriveDoorLimit
		{
			get;
			set;
		}

		[TrafficProtocol("Type", false, ",8275,", "查询类型")]
		public int DriveSpeedType
		{
			get;
			set;
		}

		[TrafficProtocol("EFlag", false, ",16411,", "事件项编码")]
		public int EFlag
		{
			get;
			set;
		}

		[TrafficProtocol("Time", false, ",16385,", "心跳间隔")]
		public int EndPointHeartInterval
		{
			get;
			set;
		}

		[TrafficProtocol("EndTime", false, ",16411,", "结束时间")]
		public string EndTime
		{
			get;
			set;
		}

		[TrafficProtocol(",EndTime,EndTime,EndTime,EndTime,EndTime,EndTime,EndTime,EndTime,", false, ",8451,8456,8457,8458,8459,8460,8461,8462,", "结束时间")]
		public string EndTime2012
		{
			get;
			set;
		}

		[TrafficProtocol("Revolution", false, ",354,", "发动机超转阀值")]
		public string EngineRevolution
		{
			get;
			set;
		}

		[TrafficProtocol("Times", false, ",354,", "发动机超转允许时长")]
		public string EngineTimes
		{
			get;
			set;
		}

		[TrafficProtocol("ENums", false, ",16405,", "事件项数")]
		public int ENums
		{
			get;
			set;
		}

		public string EventContent
		{
			get;
			set;
		}

		[TrafficProtocolAttrGroup("Content", "16405", "Event")]
		public string EventContentTarget
		{
			get
			{
				return this.GetGroupGB2312(this.EventContent);
			}
		}

		[TrafficProtocolAttrGroup("ID", "16405", "Event")]
		public string EventID
		{
			get;
			set;
		}

		[TrafficProtocol("Type", false, ",16405,", "设置类型")]
		public int EventType
		{
			get;
			set;
		}

		[TrafficProtocol(",FirstTime,FirstTime", false, ",8709,8713,", "安装时间")]
		public string FirstTime2012
		{
			get;
			set;
		}

		[TrafficProtocol("GetInterval", false, ",341,", "CAN数据采样间隔")]
		public string GetInterval
		{
			get;
			set;
		}

		[TrafficProtocol("GFee", false, ",16413,", "音频采样率")]
		public int GFee
		{
			get;
			set;
		}

		[TrafficProtocol("FTP", false, ",16419,", "GNSS模块详细定位数据上传方式")]
		public int GNSSFTP
		{
			get;
			set;
		}

		[TrafficProtocol("Mode", false, ",16419,", "GNSS定位模式")]
		public int GNSSMode
		{
			get;
			set;
		}

		[TrafficProtocol("NMEAHz", false, ",16419,", "GNSS采集NMEA数据频率")]
		public uint GNSSNMEAHz
		{
			get;
			set;
		}

		[TrafficProtocol("NMEAOut", false, ",16419,", "GNSSNMEA输出更新率")]
		public int GNSSNMEAOut
		{
			get;
			set;
		}

		[TrafficProtocol("Rate", false, ",16419,", "GNSS波特率")]
		public int GNSSRate
		{
			get;
			set;
		}

		[TrafficProtocol("Unit", false, ",16419,", "上传设置")]
		public uint GNSSUnit
		{
			get;
			set;
		}

		[TrafficProtocol("HVer", false, ",16400,", "硬件版本")]
		public string HVer
		{
			get;
			set;
		}

		[TrafficProtocol("IFDel", false, ",16412,", "删除标志")]
		public int IFDel
		{
			get;
			set;
		}

		[TrafficProtocol("IFSave", false, ",16413,", "保存标志")]
		public int IFSave
		{
			get;
			set;
		}

		[TrafficProtocol("ItemNums", false, ",1537,", "信息项数目")]
		public int InforItemNums
		{
			get;
			set;
		}

		[TrafficProtocolAttrGroup("Content", "1537", "Item")]
		public string InforMenuContent
		{
			get;
			set;
		}

		[TrafficProtocolAttrGroup("IType", "1537", "Item")]
		public string InforMenuIType
		{
			get;
			set;
		}

		[TrafficProtocol("Type", false, ",1537,", "设置类型")]
		public int InforMenuType
		{
			get;
			set;
		}

		public string InforServiceText
		{
			get;
			set;
		}

		[TrafficProtocol("Text", false, "1542", "信息服务内容", "InforServiceText")]
		public string InforServiceTextTarget
		{
			get
			{
				return this.GetGroupGB2312(this.InforServiceText);
			}
		}

		[TrafficProtocol("Type", false, "1542", "信息服务类型")]
		public int InfoServiceType
		{
			get;
			set;
		}

		[TrafficProtocol("Initial", false, ",8713,", "初始里程")]
		public string InitialMileage2012
		{
			get;
			set;
		}

		[TrafficProtocol("IP", false, ",16400,", "升级服务器地址IP")]
		public string IP
		{
			get;
			set;
		}

		[TrafficProtocol("KnockSet", false, ",16421,", "碰撞参数")]
		public int KnockSet
		{
			get;
			set;
		}

		public string m_Params
		{
			get;
			set;
		}

		public string m_ParamsReport
		{
			get;
			set;
		}

		[TrafficProtocol("ID", false, ",16431,", "制造商ID")]
		public string ManufacturerID
		{
			get;
			set;
		}

		[TrafficProtocol(",MaxBlockNum,MaxBlockNum,MaxBlockNum,MaxBlockNum,MaxBlockNum,MaxBlockNum,MaxBlockNum,MaxBlockNum", false, ",8451,8456,8457,8458,8459,8460,8461,8462,", "最大数据块")]
		public int MaxBlockNum2012
		{
			get;
			set;
		}

		[TrafficProtocol("Flag", false, ",16411,", "多媒体类型")]
		public int MediaFlag
		{
			get;
			set;
		}

		[TrafficProtocol("ID", false, "16418", "多媒体ID")]
		public long MediaID
		{
			get;
			set;
		}

		[TrafficProtocol("MID", false, ",16400,", "制造商ID")]
		public string MID
		{
			get;
			set;
		}

		[TrafficProtocol("Times", false, ",355,", "超长怠速允许时长")]
		public string OverRunIdleSpeedTimes
		{
			get;
			set;
		}

		[TrafficProtocolAttrForArray(",16422,")]
		public ArrayList ParamIDs
		{
			get
			{
				string[] strArrays = new string[] { "," };
				ArrayList arrayLists = new ArrayList();
				string[] strArrays1 = this.sParamID.Split(strArrays, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < (int)strArrays1.Length; i++)
				{
					string str = strArrays1[i];
					arrayLists.Add(new string[] { str });
				}
				return arrayLists;
			}
		}

		[TrafficProtocol("Flag", false, ",16428,16429,", ",定时参数,定距参数,")]
		public uint PhotoFlag
		{
			get;
			set;
		}

		[TrafficProtocol("Brightness", false, ",16397,", "亮度")]
		public int PicBrightness
		{
			get;
			set;
		}

		[TrafficProtocol("Chroma", false, ",16397,", "色度")]
		public int PicChroma
		{
			get;
			set;
		}

		[TrafficProtocol("Contrast", false, ",16397,", "对比度")]
		public int PicContrast
		{
			get;
			set;
		}

		[TrafficProtocol("Quality", false, ",16397,", "图片质量图像质量")]
		public int PicQuality
		{
			get;
			set;
		}

		[TrafficProtocol("Saturation", false, ",16397,", "饱和度")]
		public int PicSaturation
		{
			get;
			set;
		}

		[TrafficProtocol("PID", false, ",16398,", "省ID")]
		public int PID
		{
			get;
			set;
		}

		[TrafficProtocol("PlateType", false, "8706", "车牌分类")]
		public string PlateType
		{
			get;
			set;
		}

		[TrafficProtocol("PlateType", false, ",8708,", "车牌分类")]
		public string PlateType2012
		{
			get;
			set;
		}

		[TrafficProtocol("V", false, ",8712,", "脉冲系数")]
		public string PulseFactor2012
		{
			get;
			set;
		}

		[TrafficProtocol("Pwd", false, ",16400,", "拨号密码")]
		public string Pwd
		{
			get;
			set;
		}

		[TrafficProtocol("Question", false, ",16406,", "问题内容")]
		public string Question
		{
			get;
			set;
		}

		[TrafficProtocol("Flag", false, ",16406,", "提问下发标志位")]
		public int QuestionFlag
		{
			get;
			set;
		}

		[TrafficProtocol("R", false, "16417", "电子围栏半径")]
		public long R
		{
			get;
			set;
		}

		public string RawPackageText
		{
			get;
			set;
		}

		[TrafficProtocol("Text", false, "2562", "透传文本")]
		public string RawPackageTextTarget
		{
			get
			{
				string str = "";
				byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(this.RawPackageText);
				for (int i = 0; i < (int)bytes.Length; i++)
				{
					str = string.Concat(str, bytes[i].ToString("X2"));
				}
				return str;
			}
		}

		[TrafficProtocol(",Time,Time,Time,", false, ",8711,8712,8713,", "记录仪实时时钟")]
		public string RealTime2012
		{
			get;
			set;
		}

		[TrafficProtocol("Time", false, "8707", "记录仪实时时钟")]
		public string RecordTime
		{
			get;
			set;
		}

		[TrafficProtocol("Times", false, ",16386,", "重传次数")]
		public int RepeatTimes
		{
			get;
			set;
		}

		[TrafficProtocol("TimeOut", false, ",16386,", "应答超时时间")]
		public int ReplyTimeOut
		{
			get;
			set;
		}

		public string ResultContent
		{
			get;
			set;
		}

		[TrafficProtocolAttrGroup("Content", "16406", "Result")]
		public string ResultContentTarget
		{
			get
			{
				return this.GetGroupGB2312(this.ResultContent);
			}
		}

		[TrafficProtocolAttrGroup("ID", "16406", "Result")]
		public string ResultID
		{
			get;
			set;
		}

		[TrafficProtocol("ServerType", false, ",16426,", "预警类型")]
		public int ServerType
		{
			get;
			set;
		}

		[TrafficProtocol("Time", false, ",16413,", "录音时间")]
		public int SoundRecordistTime
		{
			get;
			set;
		}

		[TrafficProtocol("Type", false, ",16413,", "录音命令")]
		public int SoundRecordistType
		{
			get;
			set;
		}

		public string sParamID
		{
			get;
			set;
		}

		[TrafficProtocol("Acceleration", false, ",353,", "急减速阀值")]
		public string SpeedDownAcceleration
		{
			get;
			set;
		}

		[TrafficProtocol("AcceTime", false, ",353,", "急减速允许时长")]
		public string SpeedDownAcceTime
		{
			get;
			set;
		}

		[TrafficProtocol("Acceleration", false, ",352,", "急加速阀值")]
		public string SpeedUpAcceleration
		{
			get;
			set;
		}

		[TrafficProtocol("AcceTime", false, ",352,", "急加速允许时长")]
		public string SpeedUpAcceTime
		{
			get;
			set;
		}

		[TrafficProtocol("AccPercentage", false, ",352,", "油门开度")]
		public string SpeedUpAccPercentage
		{
			get;
			set;
		}

		[TrafficProtocol("StartTime", false, ",16411,", "启始时间")]
		public string StartTime
		{
			get;
			set;
		}

		[TrafficProtocol("name0", false, ",8710,", "D0状态名称")]
		public string StaSigName0
		{
			get;
			set;
		}

		[TrafficProtocol("name1", false, ",8710,", "D1状态名称")]
		public string StaSigName1
		{
			get;
			set;
		}

		[TrafficProtocol("name2", false, ",8710,", "D2状态名称")]
		public string StaSigName2
		{
			get;
			set;
		}

		[TrafficProtocol("name3", false, ",8710,", "D3状态名称")]
		public string StaSigName3
		{
			get;
			set;
		}

		[TrafficProtocol("name4", false, ",8710,", "D4状态名称")]
		public string StaSigName4
		{
			get;
			set;
		}

		[TrafficProtocol("name5", false, ",8710,", "D5状态名称")]
		public string StaSigName5
		{
			get;
			set;
		}

		[TrafficProtocol("name6", false, ",8710,", "D6状态名称")]
		public string StaSigName6
		{
			get;
			set;
		}

		[TrafficProtocol("name7", false, ",8710,", "D7状态名称")]
		public string StaSigName7
		{
			get;
			set;
		}

		[TrafficProtocol("SVer", false, ",16400,", "固件版本")]
		public string SVer
		{
			get;
			set;
		}

		[TrafficProtocol("TCPPort", false, ",16414,", " 服务器TCP端口")]
		public string TCPPort
		{
			get;
			set;
		}

		[TrafficProtocol("Total", false, ",8713,", "累计里程")]
		public string TotalMileage2012
		{
			get;
			set;
		}

		[TrafficProtocol("TOut", false, ",16400,", "时限")]
		public int Tout
		{
			get;
			set;
		}

		[TrafficProtocol("UDPPort", false, ",16414,", "服务器UDP端口")]
		public string UDPPort
		{
			get;
			set;
		}

		[TrafficProtocol("UpInterval", false, ",341,", "CAN数据上报间隔")]
		public string UpInterval
		{
			get;
			set;
		}

		[TrafficProtocol("ID", false, ",16412,", "媒体类型通道号")]
		public int UpMediaChanelID
		{
			get;
			set;
		}

		[TrafficProtocol("EFlag", false, ",16412,", "事件项编码")]
		public int UpMediaEFlag
		{
			get;
			set;
		}

		[TrafficProtocol("EndTime", false, ",16412,", "结束时间")]
		public string UpMediaEndTime
		{
			get;
			set;
		}

		[TrafficProtocol("Flag", false, ",16412,", "多媒体类型")]
		public int UpMediaFlag
		{
			get;
			set;
		}

		[TrafficProtocol("StartTime", false, ",16412,", "启始时间")]
		public string UpMediaStartTime
		{
			get;
			set;
		}

		[TrafficProtocol("Text", false, ",16431,", ",升级数据包,")]
		public string UpText
		{
			get;
			set;
		}

		[TrafficProtocol("UpType", false, ",16431,", "升级类型")]
		public byte UpType
		{
			get;
			set;
		}

		[TrafficProtocol("VER", false, ",16431,", "版本号")]
		public string UpVer
		{
			get;
			set;
		}

		[TrafficProtocol("User", false, ",16400,", "拨号用户名")]
		public string User
		{
			get;
			set;
		}

		[TrafficProtocol("UURL", false, ",16400,", "升级URL地址")]
		public string UURL
		{
			get;
			set;
		}

		[TrafficProtocol("VIN", false, "8706", "车辆VIN号")]
		public string VIN
		{
			get;
			set;
		}

		[TrafficProtocol("VIN", false, ",8708,", "车辆VIN号")]
		public string VIN2012
		{
			get;
			set;
		}

		[TrafficProtocol("TCPPort", false, ",16400,", "升级服务器地址TCP端口")]
		public string WirelessTCPPort
		{
			get;
			set;
		}

		[TrafficProtocol("UDPPort", false, ",16400,", "升级服务器地址UDP端口")]
		public string WirelessUDPPort
		{
			get;
			set;
		}

		public TrafficSimpleCmd()
		{
		}

		public string GetGB2312(string source)
		{
			string empty = string.Empty;
			byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(source);
			for (int i = 0; i < (int)bytes.Length; i++)
			{
				byte num = bytes[i];
				empty = string.Concat(empty, num.ToString("X2"));
			}
			return empty;
		}

		public string GetGroupGB2312(string groupSource)
		{
			string empty = string.Empty;
			string[] strArrays = new string[] { "," };
			string[] strArrays1 = groupSource.Split(strArrays, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < (int)strArrays1.Length; i++)
			{
				empty = (i != 0 ? string.Concat(empty, ",", this.GetGB2312(strArrays1[i])) : string.Concat(empty, this.GetGB2312(strArrays1[i])));
			}
			return empty;
		}
	}
}