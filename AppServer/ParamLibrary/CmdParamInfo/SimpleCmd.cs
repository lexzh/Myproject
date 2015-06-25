using ParamLibrary.Application;
using PublicClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using Library;

namespace ParamLibrary.CmdParamInfo
{
	[Serializable]
	public class SimpleCmd : CmdParamBase
	{
		private static string RegionIdsError;

		private static string PathIdError;

		[TrafficProtocol("StartTime", false, ",8195,")]
		public string accidentBeginTime
		{
			get;
			set;
		}

		[TrafficProtocol("EndTime", false, ",8195,")]
		public string accidentEndTime
		{
			get;
			set;
		}

		public byte[] AreaIdOrPathIdList
		{
			get;
			set;
		}

		public DateTime BeginTime
		{
			get;
			set;
		}

		[TrafficProtocol("Time", false, ",291,")]
		public int CallTimeLimit
		{
			get;
			set;
		}

		[TrafficProtocol("Type", false, ",291,")]
		public int CallTimeLimitType
		{
			get;
			set;
		}

		public byte CamaraId
		{
			get;
			set;
		}

		public string CancleAlarmParm
		{
			get;
			set;
		}

		[TrafficProtocol("CarType", false, ",1040,")]
		public new string CarType
		{
			get;
			set;
		}

		public byte ClearHistory
		{
			get;
			set;
		}

		public int CloseDail
		{
			get;
			set;
		}

		public int CloseGSM
		{
			get;
			set;
		}

		public CmdParam.CmdCode CmdCode
		{
			get
			{
				CmdParam.CmdCode orderCode;
				CmdParam.OrderCode orderCode1 = base.OrderCode;
				if (orderCode1 <= CmdParam.OrderCode.设置温度报警)
				{
					if (orderCode1 > CmdParam.OrderCode.锁车)
					{
						switch (orderCode1)
						{
							case CmdParam.OrderCode.取消进入报警区域值:
							{
								orderCode = CmdParam.CmdCode.取消区域报警;
								break;
							}
							case CmdParam.OrderCode.设置偏移路线报警:
							{
								orderCode = (CmdParam.CmdCode)base.OrderCode;
								return orderCode;
							}
							case CmdParam.OrderCode.取消偏移路线报警:
							{
								orderCode = CmdParam.CmdCode.取消路线报警;
								break;
							}
							default:
							{
								if (orderCode1 == CmdParam.OrderCode.取消超速报警)
								{
									orderCode = CmdParam.CmdCode.取消超速报警;
									break;
								}
								else
								{
									switch (orderCode1)
									{
										case CmdParam.OrderCode.设置超时停车报警:
										{
											orderCode = CmdParam.CmdCode.设置超时停车报警;
											break;
										}
										case CmdParam.OrderCode.设置超时驾驶报警:
										{
											orderCode = CmdParam.CmdCode.设置超时驾驶报警;
											break;
										}
										case CmdParam.OrderCode.设置通话时间限制:
										{
											orderCode = CmdParam.CmdCode.设置通话时间限制;
											break;
										}
										case CmdParam.OrderCode.设置盲区补偿:
										{
											orderCode = CmdParam.CmdCode.设置盲区补偿;
											break;
										}
										case CmdParam.OrderCode.实时图像监控:
										case CmdParam.OrderCode.实时监控 | CmdParam.OrderCode.停止监控 | CmdParam.OrderCode.设置越出报警区域值 | CmdParam.OrderCode.远程升级车台软件 | CmdParam.OrderCode.多种条件图像监控 | CmdParam.OrderCode.停止黑匣子采样 | CmdParam.OrderCode.终端最小汇报间隔 | CmdParam.OrderCode.设置超时驾驶报警 | CmdParam.OrderCode.设置盲区补偿 | CmdParam.OrderCode.设置限拨的电话号码JTB | CmdParam.OrderCode.查询终端参数配置:
										case CmdParam.OrderCode.自检 | CmdParam.OrderCode.远程升级车台软件 | CmdParam.OrderCode.设置移动监控中心号码:
										case CmdParam.OrderCode.实时监控 | CmdParam.OrderCode.自检 | CmdParam.OrderCode.断电恢复 | CmdParam.OrderCode.远程升级车台软件 | CmdParam.OrderCode.多种条件图像监控 | CmdParam.OrderCode.设置移动监控中心号码 | CmdParam.OrderCode.设置超时驾驶报警 | CmdParam.OrderCode.设置限拨的电话号码JTB:
										case CmdParam.OrderCode.初始化命令 | CmdParam.OrderCode.实时监控 | CmdParam.OrderCode.压缩监控 | CmdParam.OrderCode.自检 | CmdParam.OrderCode.断电 | CmdParam.OrderCode.断电恢复 | CmdParam.OrderCode.解锁 | CmdParam.OrderCode.远程升级车台软件 | CmdParam.OrderCode.设置车台呼叫限制 | CmdParam.OrderCode.多种条件图像监控 | CmdParam.OrderCode.设置轮询监控 | CmdParam.OrderCode.设置移动监控中心号码 | CmdParam.OrderCode.设置超时停车报警 | CmdParam.OrderCode.设置超时驾驶报警 | CmdParam.OrderCode.设置通话时间限制 | CmdParam.OrderCode.设置车台参数 | CmdParam.OrderCode.设置限拨的电话号码JTB | CmdParam.OrderCode.查询终端属性 | CmdParam.OrderCode.上报驾驶员身份信息请求:
										case CmdParam.OrderCode.设置自定义报警器:
										case CmdParam.OrderCode.实时监控 | CmdParam.OrderCode.停止监控 | CmdParam.OrderCode.设置越出报警区域值 | CmdParam.OrderCode.自检 | CmdParam.OrderCode.断电恢复 | CmdParam.OrderCode.锁车 | CmdParam.OrderCode.取消越出报警区域值 | CmdParam.OrderCode.远程升级车台软件 | CmdParam.OrderCode.多种条件图像监控 | CmdParam.OrderCode.停止黑匣子采样 | CmdParam.OrderCode.终端最小汇报间隔 | CmdParam.OrderCode.设置移动监控中心号码 | CmdParam.OrderCode.设置超时驾驶报警 | CmdParam.OrderCode.设置盲区补偿 | CmdParam.OrderCode.设置自定义报警器 | CmdParam.OrderCode.设置限拨的电话号码JTB | CmdParam.OrderCode.查询终端参数配置:
										{
											orderCode = (CmdParam.CmdCode)base.OrderCode;
											return orderCode;
										}
										case CmdParam.OrderCode.获得车台终端的软件版本:
										{
											orderCode = CmdParam.CmdCode.获取终端版本;
											break;
										}
										case CmdParam.OrderCode.设置车台参数:
										{
											orderCode = CmdParam.CmdCode.设置签到汇报;
											break;
										}
										case CmdParam.OrderCode.设置车台总里程:
										{
											orderCode = CmdParam.CmdCode.修改终端总里程值;
											break;
										}
										case CmdParam.OrderCode.设置温度报警:
										{
											orderCode = CmdParam.CmdCode.设置车台温度报警;
											break;
										}
										default:
										{
											orderCode = (CmdParam.CmdCode)base.OrderCode;
											return orderCode;
										}
									}
								}
								break;
							}
						}
					}
					else if (orderCode1 == CmdParam.OrderCode.取消多功能区域报警)
					{
						orderCode = CmdParam.CmdCode.取消区域报警;
					}
					else
					{
						if (orderCode1 == CmdParam.OrderCode.关闭报警路线动态下载)
						{
							orderCode = CmdParam.CmdCode.配置功能开关;
							return orderCode;
						}
						switch (orderCode1)
						{
							case CmdParam.OrderCode.短信强制复位:
							{
								orderCode = CmdParam.CmdCode.复位;
								break;
							}
							case CmdParam.OrderCode.末次位置查询:
							case CmdParam.OrderCode.位置查询:
							case CmdParam.OrderCode.初始化命令:
							case CmdParam.OrderCode.实时监控:
							case CmdParam.OrderCode.压缩监控:
							case CmdParam.OrderCode.设置越出报警区域值:
							case CmdParam.OrderCode.设置进入报警区域值:
							{
								orderCode = (CmdParam.CmdCode)base.OrderCode;
								return orderCode;
							}
							case CmdParam.OrderCode.停止监控:
							{
								orderCode = CmdParam.CmdCode.停止监控;
								break;
							}
							case CmdParam.OrderCode.停止报警:
							{
								orderCode = CmdParam.CmdCode.结束报警;
								break;
							}
							case CmdParam.OrderCode.自检:
							{
								orderCode = CmdParam.CmdCode.自检;
								break;
							}
							case CmdParam.OrderCode.断电:
							{
								orderCode = CmdParam.CmdCode.断电;
								break;
							}
							case CmdParam.OrderCode.断电恢复:
							{
								orderCode = CmdParam.CmdCode.断电恢复;
								break;
							}
							case CmdParam.OrderCode.解锁:
							{
								orderCode = CmdParam.CmdCode.开锁;
								break;
							}
							case CmdParam.OrderCode.锁车:
							{
								orderCode = CmdParam.CmdCode.闭锁;
								break;
							}
							default:
							{
								orderCode = (CmdParam.CmdCode)base.OrderCode;
								return orderCode;
							}
						}
					}
				}
				else if (orderCode1 <= CmdParam.OrderCode.设置个人定位亲情号码)
				{
					switch (orderCode1)
					{
						case CmdParam.OrderCode.获得当前车台温度:
						{
							orderCode = CmdParam.CmdCode.获取车台温度;
							break;
						}
						case CmdParam.OrderCode.下载兴趣点:
						{
							orderCode = CmdParam.CmdCode.设置兴趣点信息;
							break;
						}
						case CmdParam.OrderCode.停止监控 | CmdParam.OrderCode.自检 | CmdParam.OrderCode.锁车 | CmdParam.OrderCode.设置偏移路线报警 | CmdParam.OrderCode.点对点电召 | CmdParam.OrderCode.设置车台报警器 | CmdParam.OrderCode.设置车台复位 | CmdParam.OrderCode.实时点名查询 | CmdParam.OrderCode.远程升级车台软件 | CmdParam.OrderCode.停止黑匣子采样 | CmdParam.OrderCode.设置移动监控中心号码 | CmdParam.OrderCode.呼外线 | CmdParam.OrderCode.设置盲区补偿 | CmdParam.OrderCode.设置出租车监控 | CmdParam.OrderCode.设置自定义报警器:
						case CmdParam.OrderCode.设置多功能区域报警:
						case CmdParam.OrderCode.初始化命令 | CmdParam.OrderCode.设置调度汇报中心号 | CmdParam.OrderCode.配置串口参数:
						{
							orderCode = (CmdParam.CmdCode)base.OrderCode;
							return orderCode;
						}
						case CmdParam.OrderCode.设置油量报警阈值:
						{
							orderCode = CmdParam.CmdCode.设置油量报警阈值;
							break;
						}
						case CmdParam.OrderCode.配置油量检测参考值:
						{
							orderCode = CmdParam.CmdCode.配置油量检测参考值;
							break;
						}
						case CmdParam.OrderCode.配置串口参数:
						{
							orderCode = CmdParam.CmdCode.配置串口参数;
							break;
						}
						case CmdParam.OrderCode.开启报警路线动态下载:
						{
							orderCode = CmdParam.CmdCode.配置功能开关;
							return orderCode;
						}
						default:
						{
							if (orderCode1 == CmdParam.OrderCode.驾培信息查询)
							{
								orderCode = CmdParam.CmdCode.驾培查询;
								break;
							}
							else if (orderCode1 == CmdParam.OrderCode.设置个人定位亲情号码)
							{
								orderCode = CmdParam.CmdCode.设置个人定位亲情号码;
								break;
							}
							else
							{
								orderCode = (CmdParam.CmdCode)base.OrderCode;
								return orderCode;
							}
						}
					}
				}
				else if (orderCode1 > CmdParam.OrderCode.设置被动监听)
				{
					switch (orderCode1)
					{
						case CmdParam.OrderCode.取消报警区域值:
						{
							orderCode = CmdParam.CmdCode.取消区域报警;
							break;
						}
						case CmdParam.OrderCode.实时监控 | CmdParam.OrderCode.停止监控 | CmdParam.OrderCode.设置越出报警区域值 | CmdParam.OrderCode.设置被动监听:
						case CmdParam.OrderCode.初始化命令 | CmdParam.OrderCode.实时监控 | CmdParam.OrderCode.压缩监控 | CmdParam.OrderCode.停止监控 | CmdParam.OrderCode.停止报警 | CmdParam.OrderCode.设置越出报警区域值 | CmdParam.OrderCode.设置进入报警区域值 | CmdParam.OrderCode.设置被动监听 | CmdParam.OrderCode.取消报警区域值:
                        case (CmdParam.OrderCode)1032:
						case CmdParam.OrderCode.超时驾驶时间清零:
						{
							orderCode = (CmdParam.CmdCode)base.OrderCode;
							return orderCode;
						}
						case CmdParam.OrderCode.修改车台超级密码:
						{
							orderCode = CmdParam.CmdCode.重置车台超级密码;
							break;
						}
						case CmdParam.OrderCode.重置车台管理密码:
						{
							orderCode = CmdParam.CmdCode.重置车台管理密码;
							break;
						}
						case CmdParam.OrderCode.重置车台通话密码:
						{
							orderCode = CmdParam.CmdCode.重置车台通话密码;
							break;
						}
						case CmdParam.OrderCode.重置车台防盗密码:
						{
							orderCode = CmdParam.CmdCode.重置车台防盗密码;
							break;
						}
						default:
						{
							switch (orderCode1)
							{
								case CmdParam.OrderCode.停止图像监控:
								{
									orderCode = CmdParam.CmdCode.停止摄像头监控;
									break;
								}
								case CmdParam.OrderCode.摄像头休眠:
								{
									orderCode = (CmdParam.CmdCode)base.OrderCode;
									return orderCode;
								}
								case CmdParam.OrderCode.终端图片上传:
								{
									orderCode = CmdParam.CmdCode.上传某时间段某状态的条件监控数据;
									break;
								}
								case CmdParam.OrderCode.根据条件删除图片:
								{
									orderCode = CmdParam.CmdCode.删除某时间之前的图片;
									break;
								}
								case CmdParam.OrderCode.删除所有图片:
								{
									orderCode = CmdParam.CmdCode.清除所有图片;
									break;
								}
								case CmdParam.OrderCode.根据条件获得黑匣子图片:
								{
									orderCode = CmdParam.CmdCode.上传某时间段某状态的条件监控数据;
									break;
								}
								default:
								{
									orderCode = (CmdParam.CmdCode)base.OrderCode;
									return orderCode;
								}
							}
							break;
						}
					}
				}
				else if (orderCode1 == CmdParam.OrderCode.停止轮询监控)
				{
					orderCode = CmdParam.CmdCode.停止轮询监控;
				}
				else
				{
					if (orderCode1 != CmdParam.OrderCode.设置被动监听)
					{
						orderCode = (CmdParam.CmdCode)base.OrderCode;
						return orderCode;
					}
					orderCode = CmdParam.CmdCode.被动监听;
				}
				return orderCode;
			}
		}

		[TrafficProtocolAttrForArray(",335,8199,8273,8274,")]
		public ArrayList CmdParams
		{
			get;
			set;
		}

		public string Com1Device
		{
			get;
			set;
		}

		public string Com2Device
		{
			get;
			set;
		}

		public int DevType1
		{
			get;
			set;
		}

		public int DevType2
		{
			get;
			set;
		}

		[TrafficProtocolAttrGroup("Content", "8200", "Set")]
		public string DriverSpeedGetContent
		{
			get
			{
				string str = ",";
				for (int i = 0; i < this.CmdParams.Count; i++)
				{
					string[] item = this.CmdParams[i] as string[];
					str = string.Concat(str, item[1], ",");
				}
				return str;
			}
		}

		[TrafficProtocol("Type", false, ",8275,")]
		public int DriverSpeedGetType
		{
			get;
			set;
		}

		[TrafficProtocolAttrGroup("Type", "8200", "Set")]
		public string DriverSpeedType
		{
			get
			{
				string str = ",";
				for (int i = 0; i < this.CmdParams.Count; i++)
				{
					string[] item = this.CmdParams[i] as string[];
					str = string.Concat(str, item[0], ",");
				}
				return str;
			}
		}

		public byte DvrFrame
		{
			get;
			set;
		}

		public byte DvrImgQulity
		{
			get;
			set;
		}

		public int EmptyAD
		{
			get;
			set;
		}

		public DateTime EndTime
		{
			get;
			set;
		}

		public string FirstPwd
		{
			get;
			set;
		}

		public int FullAD
		{
			get;
			set;
		}

		public bool HavePtp1
		{
			get;
			set;
		}

		public bool HavePtp2
		{
			get;
			set;
		}

		public double HighTemprature
		{
			get;
			set;
		}

		public int ImageCnt
		{
			get;
			set;
		}

		public DataTable InsterestPoints
		{
			get;
			set;
		}

		public byte IntervalTime
		{
			get;
			set;
		}

		public byte IsClearHistory
		{
			get;
			set;
		}

		[TrafficProtocol("Type", false, ",16391,")]
		public int ListenModeStrategy
		{
			get;
			set;
		}

		[TrafficProtocol("Phone", false, ",1026,")]
		public string ListenTel
		{
			get;
			set;
		}

		public byte LocaleGuideSwitch
		{
			get;
			set;
		}

		[TrafficProtocol("Type", false, ",1040,")]
		public string LockCarValue
		{
			get;
			set;
		}

		public double LowTemprature
		{
			get;
			set;
		}

		public string MapTypes
		{
			get;
			set;
		}

		[TrafficProtocol("Mileage", false, ",301,")]
		public long MileageCnt
		{
			get;
			set;
		}

		public int OilAlarmValue
		{
			get;
			set;
		}

		public int OilBoxVol
		{
			get;
			set;
		}

		public int OnDuty
		{
			get;
			set;
		}

		public int PhotoState
		{
			get;
			set;
		}

		public byte PowerType
		{
			get;
			set;
		}

		public byte[] PPSFamilyNum
		{
			get;
			set;
		}

		[TrafficProtocol("PTime", false, ",290,")]
		public int PreAlarmTime
		{
			get;
			set;
		}

		[TrafficProtocol("STime", false, ",290,")]
		public int PreInterval
		{
			get;
			set;
		}

		public object pvArg
		{
			get
			{
				return this.GetArg();
			}
		}

		[TrafficProtocol("RCycle", false, ",16403,")]
		public int RCycle
		{
			get;
			set;
		}

		public int RealseaseTime
		{
			get;
			set;
		}

		public string RefAD
		{
			get;
			set;
		}

		public int RefCount
		{
			get;
			set;
		}

		public string RefPercentage
		{
			get;
			set;
		}

		public string RegionIds
		{
			get;
			set;
		}

		public string RegionTypes
		{
			get;
			set;
		}

		[TrafficProtocol("Type", false, ",272,")]
		public string ResetParam
		{
			get;
			set;
		}

		[TrafficProtocol("RTime", false, ",290,")]
		public int RestTime
		{
			get;
			set;
		}

		[TrafficProtocol("RTiming", false, ",16403,")]
		public int RTiming
		{
			get;
			set;
		}

		public string SecondPwd
		{
			get;
			set;
		}

		public string SecurityPwd
		{
			get;
			set;
		}

		public byte SecuritySwitch
		{
			get;
			set;
		}

		public byte SecurityType
		{
			get;
			set;
		}

		public string SeftParam
		{
			get;
			set;
		}

		public string StarDateTime
		{
			get;
			set;
		}

		[TrafficProtocol("StopID", false, ",8195,")]
		public string StopID
		{
			get
			{
				string str;
				if (this.CmdParams.Count > 0)
				{
					string[] item = this.CmdParams[0] as string[];
					this.accidentBeginTime = item[1];
					this.accidentEndTime = item[2];
					str = item[0];
				}
				else
				{
					str = "";
				}
				return str;
			}
		}

		[TrafficProtocol("OTime,Time", false, ",290,289,")]
		public int TimeOutTime
		{
			get;
			set;
		}

		[TrafficProtocolAttrForArray(",1030,")]
		public ArrayList TrafficPathID
		{
			get
			{
				string[] strArrays = new string[] { "," };
				ArrayList arrayLists = new ArrayList();
				string[] strArrays1 = this.RegionIds.Replace("\\", ",").Split(strArrays, StringSplitOptions.RemoveEmptyEntries);
				string[] strArrays2 = strArrays1;
				for (int i = 0; i < (int)strArrays2.Length; i++)
				{
					string str = strArrays2[i];
					arrayLists.Add(new string[] { str });
				}
				return arrayLists;
			}
		}

		[TrafficProtocolAttrGroup("ID", "1029", "Path")]
		public string TrafficRegionIds
		{
			get
			{
				return this.RegionIds.Replace('\\', ',');
			}
		}

		[TrafficProtocolAttrGroup("Type", "1029", "Path")]
		public string TrafficRegionTypes
		{
			get
			{
				return this.RegionTypes.Replace('\\', ',');
			}
		}

		public byte UnknownAreaCompensation
		{
			get;
			set;
		}

		public byte WaitingTime
		{
			get;
			set;
		}

		static SimpleCmd()
		{
			SimpleCmd.RegionIdsError = "区域ID超过指定范围！(0=<x<=255)";
			SimpleCmd.PathIdError = "线路ID超过指定范围！(0=<x<=255)";
		}

		public SimpleCmd()
		{
		}

		public override int CheckData(out string strErrorMsg)
		{
			int num;
			int num1 = -1;
			strErrorMsg = "";
			CmdParam.OrderCode orderCode = base.OrderCode;
			if (orderCode > CmdParam.OrderCode.设置温度报警)
			{
				if (orderCode > CmdParam.OrderCode.设置被动监听)
				{
					switch (orderCode)
					{
						case CmdParam.OrderCode.取消报警区域值:
						{
							if (this.CheckRegionIds(out strErrorMsg))
							{
								num1 = 0;
							}
							num = num1;
							return num;
						}
						case CmdParam.OrderCode.实时监控 | CmdParam.OrderCode.停止监控 | CmdParam.OrderCode.设置越出报警区域值 | CmdParam.OrderCode.设置被动监听:
						case CmdParam.OrderCode.初始化命令 | CmdParam.OrderCode.实时监控 | CmdParam.OrderCode.压缩监控 | CmdParam.OrderCode.停止监控 | CmdParam.OrderCode.停止报警 | CmdParam.OrderCode.设置越出报警区域值 | CmdParam.OrderCode.设置进入报警区域值 | CmdParam.OrderCode.设置被动监听 | CmdParam.OrderCode.取消报警区域值:
                        case (CmdParam.OrderCode)1032:
						case CmdParam.OrderCode.超时驾驶时间清零:
						{
							num1 = 0;
							num = num1;
							return num;
						}
						case CmdParam.OrderCode.修改车台超级密码:
						case CmdParam.OrderCode.重置车台管理密码:
						case CmdParam.OrderCode.重置车台通话密码:
						case CmdParam.OrderCode.重置车台防盗密码:
						{
							if (!(!Check.CheckIsDigit(this.FirstPwd, 8) ? false : Check.CheckIsDigit(this.SecondPwd, 8)))
							{
								strErrorMsg = "密码必须为8位数字！";
								num1 = -1;
								break;
							}
							else if (this.CheckStringQuery(this.FirstPwd, this.SecondPwd, out strErrorMsg))
							{
								num1 = 0;
								break;
							}
							else
							{
								num1 = -1;
								break;
							}
						}
						default:
						{
							switch (orderCode)
							{
								case CmdParam.OrderCode.根据条件删除图片:
								case CmdParam.OrderCode.根据条件获得黑匣子图片:
								{
									if (!(this.BeginTime > this.EndTime))
									{
										num1 = 0;
									}
									else
									{
										strErrorMsg = "开始时间大于结束时间";
										num1 = -1;
									}
									break;
								}
								case CmdParam.OrderCode.删除所有图片:
								{
									num1 = 0;
									num = num1;
									return num;
								}
								default:
								{
									num1 = 0;
									num = num1;
									return num;
								}
							}
							break;
						}
					}
				}
				else
				{
					switch (orderCode)
					{
						case CmdParam.OrderCode.设置油量报警阈值:
						{
							if (!this.CheckValue(out strErrorMsg, 65535, 0, this.OilAlarmValue, "油量异常变化阈值"))
							{
								num1 = -1;
								break;
							}
							else if (this.CheckValue(out strErrorMsg, 65535, 0, this.TimeOutTime, "持续时间"))
							{
								num1 = 0;
								break;
							}
							else
							{
								num1 = -1;
								break;
							}
						}
						case CmdParam.OrderCode.配置油量检测参考值:
						{
							if ((this.PowerType == 0 ? true : this.PowerType == 1))
							{
								num1 = 0;
								break;
							}
							else
							{
								strErrorMsg = "电瓶类型错误";
								num1 = -1;
								break;
							}
						}
						default:
						{
							if (orderCode != CmdParam.OrderCode.设置被动监听)
							{
								num1 = 0;
								num = num1;
								return num;
							}
							else if (Check.CheckIsDigit(this.ListenTel, 15))
							{
								num1 = 0;
								break;
							}
							else
							{
								strErrorMsg = "电话号码必须为15个字符以内的数字！";
								num1 = -1;
								break;
							}
						}
					}
				}
			}
			else if (orderCode > CmdParam.OrderCode.取消偏移路线报警)
			{
				switch (orderCode)
				{
					case CmdParam.OrderCode.设置超时停车报警:
					{
						num1 = (this.CheckValue(out strErrorMsg, 65535, 0, this.TimeOutTime, "驾驶持续时长") ? 0 : -1);
						break;
					}
					case CmdParam.OrderCode.设置超时驾驶报警:
					{
						if (!(!this.CheckValue(out strErrorMsg, 65535, 0, this.TimeOutTime, "驾驶持续时长") || !this.CheckValue(out strErrorMsg, 65535, 0, this.PreAlarmTime, "预警时长") || !this.CheckValue(out strErrorMsg, 65535, 0, this.PreInterval, "预警间隔") ? false : this.CheckValue(out strErrorMsg, 65535, 0, this.RestTime, "休息时长")))
						{
							num1 = -1;
						}
						else if (this.PreAlarmTime <= this.TimeOutTime)
						{
							num1 = 0;
						}
						else
						{
							strErrorMsg = "预警时间不能大于持续驾驶时间!";
							num1 = -1;
						}
						break;
					}
					case CmdParam.OrderCode.设置通话时间限制:
					{
						if (this.CheckValue(out strErrorMsg, 65535, 0, this.CallTimeLimit, "通话时长"))
						{
							num1 = 0;
							break;
						}
						else
						{
							num1 = -1;
							break;
						}
					}
					default:
					{
						if (orderCode != CmdParam.OrderCode.设置温度报警)
						{
							num1 = 0;
							num = num1;
							return num;
						}
						else if (this.LowTemprature > this.HighTemprature)
						{
							strErrorMsg = "最低温度不能大于最高温度";
							num1 = -1;
							break;
						}
						else if (!(this.LowTemprature > 327.67 ? false : this.LowTemprature >= -327.67))
						{
							strErrorMsg = "最低温度超出范围(-327到327)";
							num1 = -1;
							break;
						}
						else if ((this.HighTemprature > 327.67 ? false : this.HighTemprature >= -327.67))
						{
							num1 = 0;
							break;
						}
						else
						{
							strErrorMsg = "最高温度超出范围(-327到327)";
							num1 = -1;
							break;
						}
					}
				}
			}
			else if (orderCode != CmdParam.OrderCode.取消多功能区域报警)
			{
				switch (orderCode)
				{
					case CmdParam.OrderCode.取消进入报警区域值:
					case CmdParam.OrderCode.取消偏移路线报警:
					{
						if (this.CheckRegionIds(out strErrorMsg))
						{
							num1 = 0;
						}
						num = num1;
						return num;
					}
					case CmdParam.OrderCode.设置偏移路线报警:
					{
						num1 = 0;
						num = num1;
						return num;
					}
					default:
					{
						num1 = 0;
						num = num1;
						return num;
					}
				}
			}
			else
			{
				if (this.CheckRegionIds(out strErrorMsg))
				{
					num1 = 0;
				}
				num = num1;
				return num;
			}
			num = num1;
			return num;
		}

		private bool CheckRegionIds(out string strErrorMsg)
		{
			bool flag;
			strErrorMsg = "";
			string empty = string.Empty;
			CmdParam.OrderCode orderCode = base.OrderCode;
			if (orderCode == CmdParam.OrderCode.取消多功能区域报警)
			{
				empty = SimpleCmd.RegionIdsError;
			}
			else
			{
				switch (orderCode)
				{
					case CmdParam.OrderCode.取消进入报警区域值:
					{
						empty = SimpleCmd.RegionIdsError;
						break;
					}
					case CmdParam.OrderCode.设置偏移路线报警:
					{
						break;
					}
					case CmdParam.OrderCode.取消偏移路线报警:
					{
						empty = SimpleCmd.PathIdError;
						break;
					}
					default:
					{
						if (orderCode == CmdParam.OrderCode.取消报警区域值)
						{
							empty = SimpleCmd.RegionIdsError;
							break;
						}
						else
						{
							break;
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(this.RegionIds))
			{
				if (!this.RegionIds.EndsWith("\\"))
				{
					SimpleCmd simpleCmd = this;
					simpleCmd.RegionIds = string.Concat(simpleCmd.RegionIds, "\\");
				}
				string str = string.Concat(this.RegionIds, "end");
				string[] strArrays = str.Split(new char[] { '\\' });
				if ("end".Equals(strArrays[(int)strArrays.Length - 1]))
				{
					int num = 0;
					while (num < (int)strArrays.Length - 1)
					{
                        if (Check.isNumeric(strArrays[num].ToString(), Check.NumType.sInt))
						{
							int num1 = int.Parse(strArrays[num]);
							if ((num1 > 255 ? false : num1 >= 0))
							{
								num++;
							}
							else
							{
								strErrorMsg = empty;
								flag = false;
								return flag;
							}
						}
						else
						{
							strErrorMsg = empty;
							flag = false;
							return flag;
						}
					}
					flag = true;
				}
				else
				{
					strErrorMsg = empty;
					flag = false;
				}
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		private bool CheckStringQuery(string first, string second, out string strErrorMsg)
		{
			bool flag;
			strErrorMsg = "";
			if (first.Equals(second))
			{
				flag = true;
			}
			else
			{
				strErrorMsg = "两次输入的密码不一样，请重新输入！";
				flag = false;
			}
			return flag;
		}

		private bool CheckValue(out string strErrorMsg, int Max, int Min, int iValue, string Name)
		{
			bool flag;
			strErrorMsg = "";
			if ((iValue > Max ? false : iValue >= Min))
			{
				flag = true;
			}
			else
			{
				object[] name = new object[] { Name, "必须为", Min, "-", Max, "范围内的数字" };
				strErrorMsg = string.Concat(name);
				flag = false;
			}
			return flag;
		}

		public static byte[] ConvertLatAndLon(string pLatOrLon)
		{
			byte[] numArray = new byte[4];
			if (!string.IsNullOrEmpty(pLatOrLon))
			{
				double num = Convert.ToDouble(pLatOrLon);
				int num1 = (int)num;
				numArray[0] = (byte)num1;
				double num2 = (num - (double)num1) * 60;
				int num3 = (int)num2;
				if ((num3 < 0 ? false : num3 <= 60))
				{
					numArray[1] = (byte)num3;
					num2 = (num2 - (double)num3) * 100;
					int num4 = (int)num2;
					if ((num4 < 0 ? false : num4 <= 99))
					{
						numArray[2] = (byte)num4;
						num2 = (num2 - (double)num4) * 100;
						int num5 = (int)num2;
						if ((num5 < 0 ? false : num5 <= 99))
						{
							numArray[3] = (byte)num5;
						}
					}
				}
			}
			return numArray;
		}

		private int FillComConfigByteArr(int iComNum, int iDevType, bool bHavePtp, ref byte[] arr)
		{
			int num;
			int num1 = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			string str = "";
			string str1 = "";
			if (iDevType != 0)
			{
				if (bHavePtp)
				{
					num1 = 0;
					num2 = 0;
					num3 = 0;
					num4 = 0;
					num5 = 0;
					num6 = 0;
					num7 = 0;
					num8 = 0;
					num9 = 0;
					str = "";
					str1 = "";
				}
			}
			try
			{
				int length = (int)arr.Length;
				byte[] numArray = new byte[length + 19 + num8 + num9];
				arr.CopyTo(numArray, 0);
				numArray[length] = (byte)iComNum;
				numArray[length + 1] = (byte)iDevType;
				numArray[length + 2] = (byte)(iDevType >> 8);
				numArray[length + 3] = (byte)(iDevType >> 16);
				numArray[length + 4] = (byte)(iDevType >> 24);
				numArray[length + 5] = (byte)num1;
				numArray[length + 6] = (byte)num2;
				numArray[length + 7] = (byte)(num2 >> 8);
				numArray[length + 8] = (byte)(num2 >> 16);
				numArray[length + 9] = (byte)(num2 >> 24);
				numArray[length + 10] = (byte)num3;
				numArray[length + 11] = (byte)num4;
				numArray[length + 12] = (byte)num5;
				numArray[length + 13] = (byte)num6;
				numArray[length + 14] = (byte)(num6 >> 8);
				numArray[length + 15] = (byte)num7;
				numArray[length + 16] = (byte)(num7 >> 8);
				numArray[length + 17] = (byte)num8;
				if (num8 != 0)
				{
					Encoding.GetEncoding("UTF-16").GetBytes(str).CopyTo(numArray, length + 18);
					numArray[length + 18 + str.Length] = (byte)num9;
					Encoding.GetEncoding("UTF-16").GetBytes(str1).CopyTo(numArray, length + 18 + str.Length + 1);
				}
				else
				{
					numArray[length + 18] = (byte)num9;
				}
				arr = numArray;
			}
			catch
			{
				num = -1;
				return num;
			}
			num = 0;
			return num;
		}

		private object GetArg()
		{
			object obj;
			object regionIdsObj = null;
			CmdParam.OrderCode orderCode = base.OrderCode;
			if (orderCode <= CmdParam.OrderCode.设置温度报警)
			{
				if (orderCode <= CmdParam.OrderCode.断电)
				{
					if (orderCode == CmdParam.OrderCode.取消多功能区域报警)
					{
						regionIdsObj = this.GetRegionIdsObj();
						obj = regionIdsObj;
						return obj;
					}
					if (orderCode == CmdParam.OrderCode.关闭报警路线动态下载)
					{
						regionIdsObj = this.getDynPathSwitch(0);
					}
					else
					{
						switch (orderCode)
						{
							case CmdParam.OrderCode.停止监控:
							case CmdParam.OrderCode.停止报警:
							{
								regionIdsObj = this.ClearHistory;
								obj = regionIdsObj;
								return obj;
							}
							case CmdParam.OrderCode.设置越出报警区域值:
							case CmdParam.OrderCode.设置进入报警区域值:
							{
								obj = regionIdsObj;
								return obj;
							}
							case CmdParam.OrderCode.自检:
							{
								regionIdsObj = new object[] { this.SeftParam };
								break;
							}
							case CmdParam.OrderCode.断电:
							{
								regionIdsObj = new object[] { this.StarDateTime };
								break;
							}
							default:
							{
								obj = regionIdsObj;
								return obj;
							}
						}
					}
				}
				else if (orderCode > CmdParam.OrderCode.设置车台复位)
				{
					switch (orderCode)
					{
						case CmdParam.OrderCode.设置超时停车报警:
						{
							regionIdsObj = this.TimeOutTime;
							break;
						}
						case CmdParam.OrderCode.设置超时驾驶报警:
						{
							object[] timeOutTime = new object[] { this.TimeOutTime, this.PreAlarmTime, this.PreInterval, this.RestTime };
							regionIdsObj = timeOutTime;
							break;
						}
						case CmdParam.OrderCode.设置通话时间限制:
						{
							regionIdsObj = this.CallTimeLimit;
							break;
						}
						case CmdParam.OrderCode.设置盲区补偿:
						{
							regionIdsObj = this.UnknownAreaCompensation;
							break;
						}
						case CmdParam.OrderCode.实时图像监控:
						case CmdParam.OrderCode.实时监控 | CmdParam.OrderCode.停止监控 | CmdParam.OrderCode.设置越出报警区域值 | CmdParam.OrderCode.远程升级车台软件 | CmdParam.OrderCode.多种条件图像监控 | CmdParam.OrderCode.停止黑匣子采样 | CmdParam.OrderCode.终端最小汇报间隔 | CmdParam.OrderCode.设置超时驾驶报警 | CmdParam.OrderCode.设置盲区补偿 | CmdParam.OrderCode.设置限拨的电话号码JTB | CmdParam.OrderCode.查询终端参数配置:
						case CmdParam.OrderCode.获得车台终端的软件版本:
						case CmdParam.OrderCode.自检 | CmdParam.OrderCode.远程升级车台软件 | CmdParam.OrderCode.设置移动监控中心号码:
						{
							obj = regionIdsObj;
							return obj;
						}
						case CmdParam.OrderCode.设置车台参数:
						{
							regionIdsObj = this.GetOnDuty();
							break;
						}
						default:
						{
							switch (orderCode)
							{
								case CmdParam.OrderCode.设置车台总里程:
								{
									regionIdsObj = (double)this.MileageCnt;
									break;
								}
								case CmdParam.OrderCode.实时监控 | CmdParam.OrderCode.停止监控 | CmdParam.OrderCode.设置越出报警区域值 | CmdParam.OrderCode.自检 | CmdParam.OrderCode.断电恢复 | CmdParam.OrderCode.锁车 | CmdParam.OrderCode.取消越出报警区域值 | CmdParam.OrderCode.远程升级车台软件 | CmdParam.OrderCode.多种条件图像监控 | CmdParam.OrderCode.停止黑匣子采样 | CmdParam.OrderCode.终端最小汇报间隔 | CmdParam.OrderCode.设置移动监控中心号码 | CmdParam.OrderCode.设置超时驾驶报警 | CmdParam.OrderCode.设置盲区补偿 | CmdParam.OrderCode.设置自定义报警器 | CmdParam.OrderCode.设置限拨的电话号码JTB | CmdParam.OrderCode.查询终端参数配置:
								{
									obj = regionIdsObj;
									return obj;
								}
								case CmdParam.OrderCode.设置温度报警:
								{
									object[] lowTemprature = new object[] { this.LowTemprature, this.HighTemprature };
									regionIdsObj = lowTemprature;
									break;
								}
								default:
								{
									obj = regionIdsObj;
									return obj;
								}
							}
							break;
						}
					}
				}
				else
				{
					switch (orderCode)
					{
						case CmdParam.OrderCode.取消进入报警区域值:
						case CmdParam.OrderCode.取消偏移路线报警:
						{
							regionIdsObj = this.GetRegionIdsObj();
							obj = regionIdsObj;
							return obj;
						}
						case CmdParam.OrderCode.设置偏移路线报警:
						{
							obj = regionIdsObj;
							return obj;
						}
						default:
						{
							if (orderCode == CmdParam.OrderCode.设置车台复位)
							{
								regionIdsObj = new object[] { this.ResetParam };
								break;
							}
							else
							{
								obj = regionIdsObj;
								return obj;
							}
						}
					}
				}
			}
			else if (orderCode <= CmdParam.OrderCode.停止轮询监控)
			{
				if (orderCode > CmdParam.OrderCode.设置个人定位亲情号码)
				{
					switch (orderCode)
					{
						case CmdParam.OrderCode.设置出入口分段超速报警:
						{
							regionIdsObj = this.GetPassWayParam();
							break;
						}
						case CmdParam.OrderCode.设置夜间时间:
						{
							obj = regionIdsObj;
							return obj;
						}
						case CmdParam.OrderCode.设置出入口分段超速报警_公里:
						{
							regionIdsObj = this.GetPassWayParam_New();
							break;
						}
						default:
						{
							if (orderCode == CmdParam.OrderCode.停止轮询监控)
							{
								regionIdsObj = this.ClearHistory;
								obj = regionIdsObj;
								return obj;
							}
							obj = regionIdsObj;
							return obj;
						}
					}
				}
				else
				{
					switch (orderCode)
					{
						case CmdParam.OrderCode.获得当前车台温度:
						{
							regionIdsObj = 0;
							break;
						}
						case CmdParam.OrderCode.下载兴趣点:
						{
							regionIdsObj = this.GetInsteresPointArg();
							break;
						}
						case CmdParam.OrderCode.停止监控 | CmdParam.OrderCode.自检 | CmdParam.OrderCode.锁车 | CmdParam.OrderCode.设置偏移路线报警 | CmdParam.OrderCode.点对点电召 | CmdParam.OrderCode.设置车台报警器 | CmdParam.OrderCode.设置车台复位 | CmdParam.OrderCode.实时点名查询 | CmdParam.OrderCode.远程升级车台软件 | CmdParam.OrderCode.停止黑匣子采样 | CmdParam.OrderCode.设置移动监控中心号码 | CmdParam.OrderCode.呼外线 | CmdParam.OrderCode.设置盲区补偿 | CmdParam.OrderCode.设置出租车监控 | CmdParam.OrderCode.设置自定义报警器:
						case CmdParam.OrderCode.设置多功能区域报警:
						case CmdParam.OrderCode.初始化命令 | CmdParam.OrderCode.设置调度汇报中心号 | CmdParam.OrderCode.配置串口参数:
						{
							obj = regionIdsObj;
							return obj;
						}
						case CmdParam.OrderCode.设置油量报警阈值:
						{
							regionIdsObj = this.getOilVar();
							break;
						}
						case CmdParam.OrderCode.配置油量检测参考值:
						{
							regionIdsObj = this.getOilRefValue();
							break;
						}
						case CmdParam.OrderCode.配置串口参数:
						{
							regionIdsObj = this.getComConfig();
							break;
						}
						case CmdParam.OrderCode.开启报警路线动态下载:
						{
							regionIdsObj = this.getDynPathSwitch(1);
							break;
						}
						default:
						{
							if (orderCode == CmdParam.OrderCode.设置个人定位亲情号码)
							{
								regionIdsObj = this.PPSFamilyNum;
								break;
							}
							else
							{
								obj = regionIdsObj;
								return obj;
							}
						}
					}
				}
			}
			else if (orderCode <= CmdParam.OrderCode.重置车台防盗密码)
			{
				if (orderCode == CmdParam.OrderCode.设置被动监听)
				{
					regionIdsObj = this.ListenTel;
				}
				else
				{
					switch (orderCode)
					{
						case CmdParam.OrderCode.取消报警区域值:
						{
							regionIdsObj = this.GetRegionIdsObj();
							obj = regionIdsObj;
							return obj;
						}
						case CmdParam.OrderCode.实时监控 | CmdParam.OrderCode.停止监控 | CmdParam.OrderCode.设置越出报警区域值 | CmdParam.OrderCode.设置被动监听:
						case CmdParam.OrderCode.初始化命令 | CmdParam.OrderCode.实时监控 | CmdParam.OrderCode.压缩监控 | CmdParam.OrderCode.停止监控 | CmdParam.OrderCode.停止报警 | CmdParam.OrderCode.设置越出报警区域值 | CmdParam.OrderCode.设置进入报警区域值 | CmdParam.OrderCode.设置被动监听 | CmdParam.OrderCode.取消报警区域值:
                        case (CmdParam.OrderCode)1032:
						case CmdParam.OrderCode.超时驾驶时间清零:
						{
							obj = regionIdsObj;
							return obj;
						}
						case CmdParam.OrderCode.修改车台超级密码:
						case CmdParam.OrderCode.重置车台管理密码:
						case CmdParam.OrderCode.重置车台通话密码:
						case CmdParam.OrderCode.重置车台防盗密码:
						{
							regionIdsObj = this.FirstPwd;
							break;
						}
						default:
						{
							obj = regionIdsObj;
							return obj;
						}
					}
				}
			}
			else if (orderCode == CmdParam.OrderCode.重工项目锁车)
			{
				object[] objArray = new object[] { (byte)int.Parse(this.LockCarValue), (byte)1 };
				regionIdsObj = objArray;
			}
			else
			{
				switch (orderCode)
				{
					case CmdParam.OrderCode.根据条件删除图片:
					{
						regionIdsObj = this.getBlackBoxDel();
						break;
					}
					case CmdParam.OrderCode.删除所有图片:
					{
						obj = regionIdsObj;
						return obj;
					}
					case CmdParam.OrderCode.根据条件获得黑匣子图片:
					{
						regionIdsObj = this.getBlackBoxUp();
						break;
					}
					default:
					{
						obj = regionIdsObj;
						return obj;
					}
				}
			}
			obj = regionIdsObj;
			return obj;
		}

		private object getBlackBoxDel()
		{
			byte[] numArray = new byte[28];
			byte[] bytes = new byte[14];
			Encoding encoding = Encoding.GetEncoding("gb2312");
			DateTime beginTime = this.BeginTime;
			bytes = encoding.GetBytes(beginTime.ToString("yyyyMMddHHmmss"));
			bytes.CopyTo(numArray, 0);
			Encoding encoding1 = Encoding.GetEncoding("gb2312");
			beginTime = this.EndTime;
			bytes = encoding1.GetBytes(beginTime.ToString("yyyyMMddHHmmss"));
			bytes.CopyTo(numArray, 14);
			return numArray;
		}

		private object getBlackBoxUp()
		{
			byte[] photoState = new byte[34];
			byte[] bytes = new byte[14];
			Encoding encoding = Encoding.GetEncoding("gb2312");
			DateTime beginTime = this.BeginTime;
			bytes = encoding.GetBytes(beginTime.ToString("yyyyMMddHHmmss"));
			bytes.CopyTo(photoState, 0);
			Encoding encoding1 = Encoding.GetEncoding("gb2312");
			beginTime = this.EndTime;
			bytes = encoding1.GetBytes(beginTime.ToString("yyyyMMddHHmmss"));
			bytes.CopyTo(photoState, 14);
			photoState[28] = (byte)(this.PhotoState >> 24);
			photoState[29] = (byte)(this.PhotoState >> 16);
			photoState[30] = (byte)(this.PhotoState >> 8);
			photoState[31] = (byte)this.PhotoState;
			photoState[32] = (byte)(this.ImageCnt >> 8);
			photoState[33] = (byte)this.ImageCnt;
			return photoState;
		}

		private object getComConfig()
		{
			byte[] numArray = new byte[0];
			this.FillComConfigByteArr(1, this.DevType1, this.HavePtp1, ref numArray);
			this.FillComConfigByteArr(2, this.DevType2, this.HavePtp2, ref numArray);
			return numArray;
		}

		private object getDynPathSwitch(int iFlag)
		{
			int networkOrder = IPAddress.HostToNetworkOrder(1);
			int num = IPAddress.HostToNetworkOrder(iFlag);
			byte[] numArray = new byte[] { (byte)num, (byte)(num >> 8), (byte)(num >> 16), (byte)(num >> 24), (byte)networkOrder, (byte)(networkOrder >> 8), (byte)(networkOrder >> 16), (byte)(networkOrder >> 24) };
			return numArray;
		}

		private object GetInsteresPointArg()
		{
			object[] objArray = new object[this.InsterestPoints.Rows.Count * 6];
			for (int i = 0; i < this.InsterestPoints.Rows.Count; i++)
			{
				string item = this.InsterestPoints.Rows[i]["PromptText"] as string;
				if (!string.IsNullOrEmpty(item))
				{
					objArray[i * 6] = item;
					objArray[i * 6 + 1] = 1;
					objArray[i * 6 + 2] = int.Parse(this.InsterestPoints.Rows[i]["PromptID"].ToString());
					objArray[i * 6 + 3] = double.Parse(this.InsterestPoints.Rows[i]["lon"].ToString());
					objArray[i * 6 + 4] = double.Parse(this.InsterestPoints.Rows[i]["lat"].ToString());
					objArray[i * 6 + 5] = int.Parse(this.InsterestPoints.Rows[i]["PromptRad"].ToString());
				}
			}
			return objArray;
		}

		private object getOilRefValue()
		{
			int networkOrder = IPAddress.HostToNetworkOrder(this.EmptyAD);
			int num = IPAddress.HostToNetworkOrder(this.FullAD);
			int networkOrder1 = IPAddress.HostToNetworkOrder(this.OilBoxVol);
			int refCount = 14 + this.RefCount * 5;
			byte[] powerType = new byte[refCount];
			powerType[0] = this.PowerType;
			powerType[1] = (byte)networkOrder;
			powerType[2] = (byte)(networkOrder >> 8);
			powerType[3] = (byte)(networkOrder >> 16);
			powerType[4] = (byte)(networkOrder >> 24);
			powerType[5] = (byte)num;
			powerType[6] = (byte)(num >> 8);
			powerType[7] = (byte)(num >> 16);
			powerType[8] = (byte)(num >> 24);
			powerType[9] = (byte)networkOrder1;
			powerType[10] = (byte)(networkOrder1 >> 8);
			powerType[11] = (byte)(networkOrder1 >> 16);
			powerType[12] = (byte)(networkOrder1 >> 24);
			powerType[13] = (byte)this.RefCount;
			string refPercentage = this.RefPercentage;
			char[] chrArray = new char[] { ',' };
			string[] strArrays = refPercentage.Split(chrArray);
			string refAD = this.RefAD;
			chrArray = new char[] { ',' };
			string[] strArrays1 = refAD.Split(chrArray);
			for (int i = 0; i < this.RefCount; i++)
			{
				int num1 = int.Parse(strArrays[i]);
				int networkOrder2 = IPAddress.HostToNetworkOrder(int.Parse(strArrays1[i]));
				powerType[i * 5 + 14] = (byte)num1;
				powerType[i * 5 + 15] = (byte)networkOrder2;
				powerType[i * 5 + 16] = (byte)(networkOrder2 >> 8);
				powerType[i * 5 + 17] = (byte)(networkOrder2 >> 16);
				powerType[i * 5 + 18] = (byte)(networkOrder2 >> 24);
			}
			return powerType;
		}

		private object getOilVar()
		{
			int networkOrder = IPAddress.HostToNetworkOrder(this.OilAlarmValue);
			int num = IPAddress.HostToNetworkOrder(this.TimeOutTime);
			byte[] numArray = new byte[] { (byte)networkOrder, (byte)(networkOrder >> 8), (byte)num, (byte)(num >> 8) };
			return numArray;
		}

		private object GetOnDuty()
		{
			int networkOrder = IPAddress.HostToNetworkOrder(this.CloseGSM);
			int num = IPAddress.HostToNetworkOrder(this.CloseDail);
			byte[] onDuty = new byte[] { (byte)this.OnDuty, (byte)networkOrder, (byte)(networkOrder >> 8), (byte)(networkOrder >> 16), (byte)(networkOrder >> 24), (byte)num, (byte)(num >> 8), (byte)(num >> 16), (byte)(num >> 24) };
			return onDuty;
		}

		public string GetParamDisc()
		{
			string str;
			DateTime beginTime;
			string empty = string.Empty;
			empty = string.Concat("命令码-", this.CmdCode.ToString());
			CmdParam.OrderCode orderCode = base.OrderCode;
			if (orderCode <= CmdParam.OrderCode.下载兴趣点)
			{
				if (orderCode <= CmdParam.OrderCode.取消偏移路线报警)
				{
					if (orderCode != CmdParam.OrderCode.取消多功能区域报警)
					{
						switch (orderCode)
						{
							case CmdParam.OrderCode.停止监控:
							case CmdParam.OrderCode.停止报警:
							{
								empty = string.Concat(empty, ",是否强制停止-");
								empty = (this.ClearHistory != 1 ? string.Concat(empty, "不强制停止") : string.Concat(empty, "强制停止"));
								str = empty;
								return str;
							}
							case CmdParam.OrderCode.设置越出报警区域值:
							case CmdParam.OrderCode.设置进入报警区域值:
							{
								str = empty;
								return str;
							}
							case CmdParam.OrderCode.自检:
							{
								empty = string.Concat(empty, ",自检指令:", this.SeftParam);
								break;
							}
							default:
							{
								switch (orderCode)
								{
									case CmdParam.OrderCode.取消进入报警区域值:
									{
										empty = string.Concat(empty, "，取消区域ID-", this.RegionIds);
										str = empty;
										return str;
									}
									case CmdParam.OrderCode.设置偏移路线报警:
									{
										str = empty;
										return str;
									}
									case CmdParam.OrderCode.取消偏移路线报警:
									{
										empty = string.Concat(empty, ",取消线路ID-", this.RegionIds);
										break;
									}
									default:
									{
										str = empty;
										return str;
									}
								}
								break;
							}
						}
					}
					else
					{
						empty = string.Concat(empty, "，取消区域ID-", this.RegionIds);
						str = empty;
						return str;
					}
				}
				else if (orderCode > CmdParam.OrderCode.设置盲区补偿)
				{
					switch (orderCode)
					{
						case CmdParam.OrderCode.设置车台总里程:
						{
							empty = string.Concat(empty, ",里程数-", this.MileageCnt);
							break;
						}
						case CmdParam.OrderCode.实时监控 | CmdParam.OrderCode.停止监控 | CmdParam.OrderCode.设置越出报警区域值 | CmdParam.OrderCode.自检 | CmdParam.OrderCode.断电恢复 | CmdParam.OrderCode.锁车 | CmdParam.OrderCode.取消越出报警区域值 | CmdParam.OrderCode.远程升级车台软件 | CmdParam.OrderCode.多种条件图像监控 | CmdParam.OrderCode.停止黑匣子采样 | CmdParam.OrderCode.终端最小汇报间隔 | CmdParam.OrderCode.设置移动监控中心号码 | CmdParam.OrderCode.设置超时驾驶报警 | CmdParam.OrderCode.设置盲区补偿 | CmdParam.OrderCode.设置自定义报警器 | CmdParam.OrderCode.设置限拨的电话号码JTB | CmdParam.OrderCode.查询终端参数配置:
						{
							str = empty;
							return str;
						}
						case CmdParam.OrderCode.设置温度报警:
						{
							object obj = empty;
							object[] lowTemprature = new object[] { obj, ",最低温度-", this.LowTemprature, ",最高温度-", this.HighTemprature };
							empty = string.Concat(lowTemprature);
							break;
						}
						default:
						{
							if (orderCode == CmdParam.OrderCode.下载兴趣点)
							{
								empty = string.Concat(empty, ",兴趣点类别-", this.MapTypes);
								break;
							}
							else
							{
								str = empty;
								return str;
							}
						}
					}
				}
				else if (orderCode == CmdParam.OrderCode.设置车台复位)
				{
					empty = string.Concat(empty, ",复位参数:", this.ResetParam);
				}
				else
				{
					switch (orderCode)
					{
						case CmdParam.OrderCode.设置超时停车报警:
						{
							empty = string.Concat(empty, ",驾驶持续时长-", this.TimeOutTime);
							break;
						}
						case CmdParam.OrderCode.设置超时驾驶报警:
						{
							empty = string.Concat(empty, ",驾驶持续时长-", this.TimeOutTime);
							empty = string.Concat(empty, "，预警时长-", this.PreAlarmTime);
							empty = string.Concat(empty, ",预警间隔-", this.PreInterval);
							empty = string.Concat(empty, ",休息时长-", this.RestTime);
							break;
						}
						case CmdParam.OrderCode.设置通话时间限制:
						{
							empty = string.Concat(empty, ",通话限制类别-", this.CallTimeLimitType);
							empty = string.Concat(empty, ",通话时长控制-", this.CallTimeLimit);
							break;
						}
						case CmdParam.OrderCode.设置盲区补偿:
						{
							empty = string.Concat(empty, ",盲区是否补偿-");
							empty = (this.UnknownAreaCompensation != 1 ? string.Concat(empty, "不补偿") : string.Concat(empty, "补偿"));
							break;
						}
						default:
						{
							str = empty;
							return str;
						}
					}
				}
			}
			else if (orderCode <= CmdParam.OrderCode.设置被动监听)
			{
				if (orderCode == CmdParam.OrderCode.设置油量报警阈值)
				{
					empty = string.Concat(empty, ",油量异常变化阈值-", this.OilAlarmValue);
					empty = string.Concat(empty, ",持续时间-", this.TimeOutTime);
				}
				else
				{
					if (orderCode == CmdParam.OrderCode.停止轮询监控)
					{
						empty = string.Concat(empty, ",是否强制停止-");
						empty = (this.ClearHistory != 1 ? string.Concat(empty, "不强制停止") : string.Concat(empty, "强制停止"));
						str = empty;
						return str;
					}
					if (orderCode != CmdParam.OrderCode.设置被动监听)
					{
						str = empty;
						return str;
					}
					empty = string.Concat(empty, ",电话号码-", this.ListenTel);
				}
			}
			else if (orderCode <= CmdParam.OrderCode.根据条件获得黑匣子图片)
			{
				if (orderCode == CmdParam.OrderCode.取消报警区域值)
				{
					empty = string.Concat(empty, "，取消区域ID-", this.RegionIds);
					str = empty;
					return str;
				}
				switch (orderCode)
				{
					case CmdParam.OrderCode.根据条件删除图片:
					{
						beginTime = this.BeginTime;
						empty = string.Concat(empty, ",开始时间-", beginTime.ToString("yyyyMMddHHmmss"));
						beginTime = this.EndTime;
						empty = string.Concat(empty, ",结束时间-", beginTime.ToString("yyyyMMddHHmmss"));
						break;
					}
					case CmdParam.OrderCode.删除所有图片:
					{
						str = empty;
						return str;
					}
					case CmdParam.OrderCode.根据条件获得黑匣子图片:
					{
						beginTime = this.BeginTime;
						empty = string.Concat(empty, ",开始时间-", beginTime.ToString("yyyyMMddHHmmss"));
						beginTime = this.EndTime;
						empty = string.Concat(empty, ",结束时间-", beginTime.ToString("yyyyMMddHHmmss"));
						int imageCnt = this.ImageCnt;
						empty = string.Concat(empty, ",数量-", imageCnt.ToString());
						break;
					}
					default:
					{
						str = empty;
						return str;
					}
				}
			}
			else if (orderCode == CmdParam.OrderCode.终端电话接听策略)
			{
				empty = string.Concat(empty, ",终端电话接听策略:", this.ListenModeStrategy);
			}
			else
			{
				if (orderCode != CmdParam.OrderCode.临时位置跟踪控制)
				{
					str = empty;
					return str;
				}
				empty = string.Concat(empty, ",有效时间间隔:", this.RCycle);
				empty = string.Concat(empty, ",有效期:", this.RTiming);
			}
			str = empty;
			return str;
		}

		private object GetPassWayParam()
		{
			int i;
			double num = 0;
			Hashtable hashtables = new Hashtable();
			List<string> strs = new List<string>();
			string[] item = this.CmdParams[0] as string[];
			for (i = 0; i < (int)item.Length / 8; i++)
			{
				if (!hashtables.ContainsKey(item[i * 8 + 1]))
				{
					hashtables.Add(item[i * 8 + 1], null);
					for (int j = i * 8 + 1; j < i * 8 + 8; j++)
					{
						if (!double.TryParse(item[j].ToString(), out num))
						{
							throw new Exception("输入数据不合法,请检查!");
						}
						strs.Add(item[j].ToString());
					}
				}
			}
			object[] objArray = new object[strs.Count];
			byte[] numArray = new byte[22 * strs.Count / 7];
			int num1 = 0;
			for (i = 0; i < strs.Count / 7; i++)
			{
				BitConverter.GetBytes((int)Convert.ToDouble(strs[i * 7])).CopyTo(numArray, num1);
				num1 = num1 + 4;
				SimpleCmd.ConvertLatAndLon(strs[i * 7 + 1]).CopyTo(numArray, num1);
				num1 = num1 + 4;
				SimpleCmd.ConvertLatAndLon(strs[i * 7 + 2]).CopyTo(numArray, num1);
				num1 = num1 + 4;
				SimpleCmd.ConvertLatAndLon(strs[i * 7 + 3]).CopyTo(numArray, num1);
				num1 = num1 + 4;
				SimpleCmd.ConvertLatAndLon(strs[i * 7 + 4]).CopyTo(numArray, num1);
				num1 = num1 + 4;
				numArray[num1] = (byte)(Convert.ToDouble(strs[i * 7 + 5]) / 1.852 + 0.5);
				num1++;
				numArray[num1] = (byte)(Convert.ToDouble(strs[i * 7 + 6]) / 1.852 + 0.5);
				num1++;
			}
			return numArray;
		}

		private object GetPassWayParam_New()
		{
			int i;
			double num = 0;
			Hashtable hashtables = new Hashtable();
			List<string> strs = new List<string>();
			string[] item = this.CmdParams[0] as string[];
			for (i = 0; i < (int)item.Length / 8; i++)
			{
				if (!hashtables.ContainsKey(item[i * 8 + 1]))
				{
					hashtables.Add(item[i * 8 + 1], null);
					for (int j = i * 8 + 1; j < i * 8 + 8; j++)
					{
						if (!double.TryParse(item[j].ToString(), out num))
						{
							throw new Exception("输入数据不合法,请检查!");
						}
						strs.Add(item[j].ToString());
					}
				}
			}
			object[] objArray = new object[strs.Count];
			byte[] numArray = new byte[22 * strs.Count / 7];
			int num1 = 0;
			for (i = 0; i < strs.Count / 7; i++)
			{
				BitConverter.GetBytes((int)Convert.ToDouble(strs[i * 7])).CopyTo(numArray, num1);
				num1 = num1 + 4;
				SimpleCmd.ConvertLatAndLon(strs[i * 7 + 1]).CopyTo(numArray, num1);
				num1 = num1 + 4;
				SimpleCmd.ConvertLatAndLon(strs[i * 7 + 2]).CopyTo(numArray, num1);
				num1 = num1 + 4;
				SimpleCmd.ConvertLatAndLon(strs[i * 7 + 3]).CopyTo(numArray, num1);
				num1 = num1 + 4;
				SimpleCmd.ConvertLatAndLon(strs[i * 7 + 4]).CopyTo(numArray, num1);
				num1 = num1 + 4;
				numArray[num1] = (byte)Convert.ToDouble(strs[i * 7 + 5]);
				num1++;
				numArray[num1] = (byte)Convert.ToDouble(strs[i * 7 + 6]);
				num1++;
			}
			return numArray;
		}

		private object GetRegionIdsObj()
		{
			byte[] numArray;
			if (!string.IsNullOrEmpty(this.RegionIds))
			{
				string[] strArrays = this.RegionIds.Split(new char[] { '\\' });
				numArray = new byte[this.RegionIds.Length];
				for (int i = 0; i < (int)strArrays.Length - 1; i++)
				{
					numArray[i] = byte.Parse(strArrays[i]);
				}
			}
			else
			{
				numArray = new byte[0];
			}
			return numArray;
		}
	}
}