using ParamLibrary.Application;
using PublicClass;
using System;
using System.Runtime.CompilerServices;
using Library;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class CommArgs : CmdParamBase
	{
		[TrafficProtocol("CommMode", true, ",264,")]
		public CmdParam.CommMode CommMode
		{
			get;
			set;
		}

		[TrafficProtocol("UseProxy", true, ",264,")]
		public CmdParam.IsUseProxy IsUseProxy
		{
			get;
			set;
		}

		[TrafficProtocol("ProxyPort", false, ",264,")]
		public int ProxyPort
		{
			get;
			set;
		}

		[TrafficProtocol("ServerType", false, ",264,")]
		public int ServerType
		{
			get;
			set;
		}

		[TrafficProtocol("APNAddr", false, ",264,")]
		public string strAPNAddr
		{
			get;
			set;
		}

		[TrafficProtocol("Password", false, ",264,")]
		public string strPassword
		{
			get;
			set;
		}

		[TrafficProtocol("ProxyIP", false, ",264,")]
		public string strProxyIP
		{
			get;
			set;
		}

		[TrafficProtocol("TCPIP", false, ",264,")]
		public string strTCPIP
		{
			get;
			set;
		}

		[TrafficProtocol("UDPIP", false, ",264,")]
		public string strUDPIP
		{
			get;
			set;
		}

		[TrafficProtocol("User", false, ",264,")]
		public string strUser
		{
			get;
			set;
		}

		[TrafficProtocol("TCPPort", false, ",264,")]
		public int TCPPort
		{
			get;
			set;
		}

		[TrafficProtocol("UDPPort", false, ",264,")]
		public int UDPPort
		{
			get;
			set;
		}

		public CommArgs()
		{
		}

		public override int CheckData(out string strErrorMsg)
		{
			int num;
			strErrorMsg = "";
			if (this.CommMode == CmdParam.CommMode.短信)
			{
				num = 0;
			}
			else if ((!string.IsNullOrEmpty(this.strTCPIP) ? true : !string.IsNullOrEmpty(this.strUDPIP)))
			{
				if (!string.IsNullOrEmpty(this.strTCPIP))
				{
					if (!Check.CheckIpAddress(this.strTCPIP))
					{
						strErrorMsg = "TCPIP地址格式有误";
						num = -1;
						return num;
					}
					else if ((this.TCPPort <= 0 ? true : this.TCPPort > 65535))
					{
						strErrorMsg = "TCP端口超出指定范围1-65535！";
						num = -1;
						return num;
					}
				}
				if (!string.IsNullOrEmpty(this.strUDPIP))
				{
					if (!Check.CheckIpAddress(this.strUDPIP))
					{
						strErrorMsg = "UDPIP地址格式有误";
						num = -1;
						return num;
					}
					else if ((this.UDPPort <= 0 ? true : this.UDPPort > 65535))
					{
						strErrorMsg = "UDP端口超出指定范围1-65535！";
						num = -1;
						return num;
					}
				}
				if (!(string.IsNullOrEmpty(this.strUser) ? true : this.strUser.Length <= 16))
				{
					strErrorMsg = "GPRS拨号用户名，不超过16个字符";
					num = -1;
				}
				else if ((string.IsNullOrEmpty(this.strPassword) ? true : this.strPassword.Length <= 16))
				{
					if (this.IsUseProxy == CmdParam.IsUseProxy.使用代理)
					{
						if (!string.IsNullOrEmpty(this.strProxyIP))
						{
							if (!Check.CheckIpAddress(this.strProxyIP))
							{
								strErrorMsg = "代理服务器IP地址格式有误";
								num = -1;
								return num;
							}
							else if ((this.ProxyPort <= 0 ? true : this.ProxyPort > 65535))
							{
								strErrorMsg = "代理服务器端口超出指定范围1-65535！";
								num = -1;
								return num;
							}
						}
					}
					num = 0;
				}
				else
				{
					strErrorMsg = "GPRS拨号用户密码，不超过16个字符";
					num = -1;
				}
			}
			else
			{
				strErrorMsg = "请选择TCP/UDP地址！";
				num = -1;
			}
			return num;
		}
	}
}