using ParamLibrary.Application;
using PublicClass;
using System;
using System.Runtime.CompilerServices;
using Library;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class SetPhone : CmdParamBase
	{
		public CmdParam.PhoneType PhoneType
		{
			get
			{
				CmdParam.PhoneType orderCode;
				CmdParam.OrderCode orderCode1 = base.OrderCode;
				if (orderCode1 == CmdParam.OrderCode.设置移动监控中心号码)
				{
					orderCode = CmdParam.PhoneType.移动监控中心号码;
				}
				else
				{
					switch (orderCode1)
					{
						case CmdParam.OrderCode.修改监控中心号码:
						{
							orderCode = CmdParam.PhoneType.调度汇报中心号码;
							break;
						}
						case CmdParam.OrderCode.设置语音报警电话号码:
						{
							orderCode = CmdParam.PhoneType.报警号码;
							break;
						}
						case CmdParam.OrderCode.设置中心监听号码:
						{
							orderCode = CmdParam.PhoneType.中心监听电话号码;
							break;
						}
						case CmdParam.OrderCode.设置监控中心号码:
						{
							orderCode = CmdParam.PhoneType.初始化监控中心号码;
							break;
						}
						case CmdParam.OrderCode.设置医疗服务电话号码:
						{
							orderCode = CmdParam.PhoneType.医疗救护电话号码;
							break;
						}
						case CmdParam.OrderCode.设置维修服务电话号码:
						{
							orderCode = CmdParam.PhoneType.维修电话号码;
							break;
						}
						case CmdParam.OrderCode.设置救助服务电话号码:
						{
							orderCode = CmdParam.PhoneType.求助号码;
							break;
						}
						default:
						{
							switch (orderCode1)
							{
								case CmdParam.OrderCode.监控平台电话号码:
								{
									orderCode = CmdParam.PhoneType.监控平台电话号码;
									break;
								}
								case CmdParam.OrderCode.复位电话号码:
								{
									orderCode = CmdParam.PhoneType.复位电话号码;
									break;
								}
								case CmdParam.OrderCode.恢复出厂设置电话号码:
								{
									orderCode = CmdParam.PhoneType.恢复出厂设置电话号码;
									break;
								}
								case CmdParam.OrderCode.接收终端SMS文本报警号码:
								{
									orderCode = CmdParam.PhoneType.接收终端SMS文本报警号码;
									break;
								}
								default:
								{
									orderCode = (CmdParam.PhoneType)base.OrderCode;
									break;
								}
							}
							break;
						}
					}
				}
				return orderCode;
			}
		}

		[TrafficProtocol("Phone")]
		public string strPhone
		{
			get;
			set;
		}

		public SetPhone()
		{
		}

		public override int CheckData(out string strErrorMsg)
		{
			int num;
			strErrorMsg = "";
			if (Check.CheckIsDigit(this.strPhone, 15))
			{
				num = 0;
			}
			else
			{
				strErrorMsg = "电话号码必须为15个字符以内的数字！";
				num = -1;
			}
			return num;
		}
	}
}