
using System;
using Library;
using ParamLibrary.Application;
using ParamLibrary.CmdParamInfo;

namespace Bussiness
{
	public class DownData
	{
		protected int m_WorkId;

		private static CarDownCmd.CarDownCmd CarCmdSend;

		private LogHelper log;

		private LogMsg logMsg;

		protected int WorkId
		{
			get
			{
				return this.m_WorkId;
			}
			set
			{
				this.m_WorkId = value;
			}
		}

		static DownData()
		{
		}

		public DownData(int workId)
		{
			this.log = new LogHelper();
			this.logMsg = new LogMsg()
			{
				ClassName = "GpsPicDownData"
			};
			this.WorkId = workId;
		}

		private int CalOrderId(int wrkId, int orderId)
		{
			return (wrkId & 65535) << 16 | orderId & 65535;
		}

		private string ConParams(string[] parms, int iOrderCode)
		{
			string str = "下发参数：";
			iOrderCode = 0;
			if (iOrderCode != 0)
			{
				str = "下发参数：";
				for (int i = 0; i <= (int)parms.Length - 1; i++)
				{
					str = string.Concat(str, parms[i], ";");
				}
			}
			else
			{
				str = "下发参数：";
				for (int j = 0; j <= (int)parms.Length - 1; j++)
				{
					str = string.Concat(str, parms[j], ";");
				}
			}
			return str.Trim(new char[] { ';' });
		}

		private object[] ConvertObjectArrary(string[] parms)
		{
			object[] objArray = new object[(int)parms.Length];
			for (int i = 0; i <= (int)parms.Length - 1; i++)
			{
				objArray[i] = parms[i];
			}
			return objArray;
		}

		public long icar_SendCmdXML(CmdParam.ParamType ParamType, string SimNum, string CarId, string CarPw, string ProtocolName, CmdParam.CommMode CommMode, TxtMsg MsgContext, string sMsgType)
		{
			long num = (long)0;
			this.logMsg.FunctionName = "icar_SendTxtMsg";
			this.logMsg.Msg = string.Concat("发送：类型-", ParamType.ToString(), ",车辆-", SimNum);
			string str = string.Concat("消息类型-", MsgContext.MsgType.ToString(), string.Format(",{0}-", sMsgType), MsgContext.strMsg);
			LogMsg logMsg = this.logMsg;
			logMsg.Msg = string.Concat(logMsg.Msg, ",", str);
			this.log.WriteLog(this.logMsg);
			if (!this.isStartCommon())
			{
				num = (long)-1;
			}
			else
			{
				try
				{
					int newOrderId = DownData.CarCmdSend.GetNewOrderId();
					if (ProtocolName != "JTBGPS")
					{
						num = DownData.CarCmdSend.icar_SendTxtMsg(this.WorkId, newOrderId, SimNum, MsgContext.MsgType, MsgContext.strMsg);
					}
					else
					{
						MsgContext.TransformCode = CmdParam.TrafficProtocolCodeExchange((int)MsgContext.MsgType);
						string str1 = "";
						string xmlString = MsgContext.ToXmlString(this.CalOrderId(this.WorkId, newOrderId), SimNum, ProtocolName, (int)CommMode, "SimpleCmd", ref str1);
						num = DownData.CarCmdSend.icar_SendCmdXML(this.WorkId, newOrderId, SimNum, ProtocolName, (int)MsgContext.MsgType, (int)CommMode, xmlString);
					}
					if (num == (long)0)
					{
						ReadDataFromDB.execSaveGpsLogTable(this.WorkId, newOrderId, CarId, (int)MsgContext.MsgType, MsgContext.strMsg);
					}
					if (num != (long)0)
					{
						AlarmMsg alarmMsg = new AlarmMsg()
						{
							ClassName = "GpsPicDownData",
							FunctionName = "icar_SendTxtMsg"
						};
						object[] workId = new object[] { "workid-", this.WorkId, ",simNum-", SimNum, ",strMsg-", MsgContext.strMsg };
						alarmMsg.AlarmText = string.Concat(workId);
						alarmMsg.Code = num.ToString();
						this.log.WriteAlarm(alarmMsg);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg()
					{
						ClassName = "GpsPicDownData",
						ErrorText = "下发消息指令时发生错误!"
					};
					this.log.WriteError(errorMsg, exception);
					num = (long)-1;
				}
			}
			return num;
		}

		public long icar_SendRawPackage(string SimNumFirst, object SimNums, int CarId)
		{
			long num = (long)0;
			this.logMsg.FunctionName = "icar_SendRawPackage";
			if (!this.isStartCommon())
			{
				num = (long)-1;
			}
			else
			{
				try
				{
					this.log.WriteLog(this.logMsg);
					int num1 = 0;
					int num2 = 0;
					int num3 = 12289;
                    num = DownData.CarCmdSend.icar_SendRawPackage(num1, num2, SimNumFirst, CmdParam.CmdCode.带时间段的超速设置 & CmdParam.CmdCode.定时监控查看, ref SimNums, CmdParam.CommMode.短信);
					if (num == (long)0)
					{
						ReadDataFromDB.execSaveGpsLogTable(this.WorkId, num1, CarId.ToString(), num3, "LBS批量定位");
					}
					else
					{
						AlarmMsg alarmMsg = new AlarmMsg()
						{
							ClassName = "GpsPicDownData",
							FunctionName = "icar_SendRawPackage"
						};
						object[] workId = new object[] { "workid-", this.WorkId, ",simNum-", SimNums, ",strMsg-LBS调用位置查询" };
						alarmMsg.AlarmText = string.Concat(workId);
						alarmMsg.Code = num.ToString();
						this.log.WriteAlarm(alarmMsg);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg()
					{
						ClassName = "GpsPicDownData",
						ErrorText = "下发消息指令时发生错误!"
					};
					this.log.WriteError(errorMsg, exception);
					num = (long)-1;
				}
			}
			return num;
		}

		public long icar_SendRawPackage(CmdParam.ParamType ParamType, string SimNum, string CarPw, CmdParam.CommMode CommMode, TrafficRawPackage trafficRawPackage, string CarID)
		{
			long num = (long)-1;
			this.logMsg.FunctionName = "icar_SendRawPackage";
			LogMsg logMsg = this.logMsg;
			string[] str = new string[] { "发送：类型-", ParamType.ToString(), ",车辆-", SimNum, ",指令-", trafficRawPackage.OrderCode.ToString(), ",参数-", trafficRawPackage.strText };
			logMsg.Msg = string.Concat(str);
			string msg = this.logMsg.Msg;
			this.log.WriteLog(this.logMsg);
			if (!this.isStartCommon())
			{
				num = (long)-1;
			}
			else
			{
				try
				{
					int num1 = 1;
					int newOrderId = DownData.CarCmdSend.GetNewOrderId();
					string str1 = "";
					string xmlString = trafficRawPackage.ToXmlString(this.CalOrderId(num1, newOrderId), SimNum, "JTBGPS", (int)CommMode, "SendRawPackage", ref str1);
                    num = DownData.CarCmdSend.icar_SendCmdXML(num1, newOrderId, SimNum, "JTBGPS", (int)trafficRawPackage.OrderCode, (int)CommMode, xmlString);
					if (num == (long)0)
					{
						ReadDataFromDB.execSaveGpsLogTable(num1, newOrderId, CarID, (int)trafficRawPackage.OrderCode, trafficRawPackage.strText);
					}
					else
					{
						AlarmMsg alarmMsg = new AlarmMsg()
						{
							ClassName = "GpsPicDownData",
							FunctionName = "icar_SendRawPackage"
						};
						string[] simNum = new string[] { "异常发送：", SimNum, "类型-", ParamType.ToString(), ",车辆-", SimNum, ",指令-", trafficRawPackage.OrderCode.ToString(), ",参数-", trafficRawPackage.strText };
						alarmMsg.AlarmText = string.Concat(simNum);
						alarmMsg.Code = num.ToString();
						this.log.WriteAlarm(alarmMsg);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg()
					{
						ClassName = "GpsPicDownData",
						ErrorText = "下发消息指令时发生错误!"
					};
					this.log.WriteError(errorMsg, exception);
					num = (long)-1;
				}
			}
			return num;
		}

		public long icar_SendTxtMsg(CmdParam.ParamType ParamType, string SimNum, string CarPw, CmdParam.CommMode CommMode, TxtMsg msgContent, int iMsgId, string sMsgType)
		{
			long num = (long)0;
			this.logMsg.FunctionName = "icar_SendTxtMsg";
			this.logMsg.Msg = string.Concat("发送：类型-", ParamType.ToString(), ",车辆-", SimNum);
			string str = string.Concat("消息类型-", msgContent.MsgType.ToString(), string.Format(",{0}-", sMsgType), msgContent.strMsg);
			LogMsg logMsg = this.logMsg;
			logMsg.Msg = string.Concat(logMsg.Msg, ",", str);
			this.log.WriteLog(this.logMsg);
			if (!this.isStartCommon())
			{
				num = (long)-1;
			}
			else
			{
				try
				{
                    if (msgContent.MsgType != CmdParam.MsgType.掉线短信通知)
					{
						int num1 = 1;
						int num2 = iMsgId;
						num = DownData.CarCmdSend.icar_SendTxtMsg(num1, num2, SimNum, msgContent.MsgType, msgContent.strMsg);
						if (num == (long)0)
						{
							ReadDataFromDB.execSaveGpsLogTable(num1, num2, msgContent.CarId, (int)msgContent.MsgType, msgContent.strMsg);
						}
					}
					else
					{
						object[] objArray = new object[] { msgContent.MsgType.ToString(), msgContent.SimNum, msgContent.strMsg };
						object obj = objArray;
						int num3 = 4;
						int newOrderId = DownData.CarCmdSend.GetNewOrderId();
						num = DownData.CarCmdSend.icar_SetCommonCmd(num3, newOrderId, SimNum, (CmdParam.CmdCode)768, ref obj, CmdParam.CommMode.未知方式);
						if (num == (long)0)
						{
							ReadDataFromDB.execSaveGpsLogTable(num3, newOrderId, msgContent.CarId, (int)msgContent.MsgType, msgContent.strMsg);
						}
					}
					if (num != (long)0)
					{
						AlarmMsg alarmMsg = new AlarmMsg()
						{
							ClassName = "GpsPicDownData",
							FunctionName = "icar_SendTxtMsg"
						};
						object[] workId = new object[] { "workid-", this.WorkId, ",simNum-", SimNum, ",strMsg-", msgContent.strMsg };
						alarmMsg.AlarmText = string.Concat(workId);
						alarmMsg.Code = num.ToString();
						this.log.WriteAlarm(alarmMsg);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg()
					{
						ClassName = "GpsPicDownData",
						ErrorText = "下发消息指令时发生错误!"
					};
					this.log.WriteError(errorMsg, exception);
					num = (long)-1;
				}
			}
			return num;
		}

		public long icar_SetCaptureEx(CmdParam.ParamType ParamType, string SimNum, string CarPw, CmdParam.CommMode CommMode, CaptureEx captureEx, string PicTime, string CarID)
		{
			long num = (long)0;
			this.logMsg.FunctionName = "icar_SetCaptureEx";
			this.logMsg.Msg = string.Concat("发送：类型-", ParamType.ToString(), ",车辆-", SimNum);
			object[] picTime = new object[] { "抓拍时间-", PicTime, ",是否多帧-", captureEx.IsMulitFramebool, ",监控次数-", captureEx.Times, ",间隔时间-", (double)captureEx.Interval * 0.1, ",图像质量-", captureEx.Quality, ",图像亮度-", captureEx.Brightness, ",图像对比度-", captureEx.Contrast, ",图像饱和度-", captureEx.Saturation, ",图像色度", captureEx.Chroma, ",停车是否拍照-", captureEx.IsCapWhenStop };
			string str = string.Concat(picTime);
			LogMsg logMsg = this.logMsg;
			logMsg.Msg = string.Concat(logMsg.Msg, ",", str);
			this.log.WriteLog(this.logMsg);
			if (!this.isStartCommon())
			{
				num = (long)-1;
			}
			else
			{
				try
				{
					if (captureEx.ProtocolName == "JTBGPS")
					{
						int newOrderId = DownData.CarCmdSend.GetNewOrderId();
						string str1 = "";
						captureEx.TransformCode = CmdParam.TrafficProtocolCodeExchange2((int)captureEx.OrderCode);
						if (captureEx.Quality == 0)
						{
							captureEx.Quality = 1;
						}
						if (captureEx.CaptureCache == 1)
						{
							captureEx.CaptureCache = -1;
						}
						string xmlString = captureEx.ToXmlString(this.CalOrderId(this.WorkId, newOrderId), SimNum, captureEx.ProtocolName, (int)CommMode, "SetCapture", ref str1);
						num = DownData.CarCmdSend.icar_SendCmdXML(this.WorkId, newOrderId, SimNum, captureEx.ProtocolName, captureEx.TransformCode, (int)CommMode, xmlString);
					}
					else
					{
                        int newOrderId = DownData.CarCmdSend.GetNewOrderId();
						num = DownData.CarCmdSend.icar_SetCaptureEx(this.WorkId, newOrderId, SimNum, captureEx.IsMultiFrame, captureEx.CamerasID, captureEx.CaptureFlag, captureEx.CaptureCache, captureEx.Times, captureEx.Interval, captureEx.Quality, captureEx.Brightness, captureEx.Contrast, captureEx.Saturation, captureEx.Chroma, captureEx.CapWhenStop);
					}
					if (num == (long)0)
					{
						ReadDataFromDB.execSaveGpsLogTable(this.WorkId, DownData.CarCmdSend.OrderId, CarID, 18, "定时拍照");
					}
					else
					{
						AlarmMsg alarmMsg = new AlarmMsg()
						{
							ClassName = "GpsPicDownData",
							FunctionName = "icar_SetCaptureEx"
						};
						object[] workId = new object[] { "workid-", this.WorkId, ",simNum-", SimNum, ",OrderCode-", captureEx.OrderCode };
						alarmMsg.AlarmText = string.Concat(workId);
						alarmMsg.Code = num.ToString();
						this.log.WriteAlarm(alarmMsg);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg()
					{
						ClassName = "GpsPicDownData",
						ErrorText = "下发消息指令时发生错误!"
					};
					this.log.WriteError(errorMsg, exception);
					num = (long)-1;
				}
			}
			return num;
		}

		public long icar_SetCmdXML(string OrderID, string simnum, string strCarType, string CmdCode, string strXML, string CommFlag, string CarID)
		{
			long num = (long)0;
			this.logMsg.FunctionName = "icar_SetCmdXML";
			LogMsg logMsg = this.logMsg;
			string[] strArrays = new string[] { "发送：", strCarType, ",类型-", CmdCode.ToString(), ",车辆-", simnum };
			logMsg.Msg = string.Concat(strArrays);
			if (!this.isStartCommon())
			{
				num = (long)-1;
			}
			else
			{
				try
				{
					this.log.WriteLog(this.logMsg);
					int newOrderId = DownData.CarCmdSend.GetNewOrderId();
					num = DownData.CarCmdSend.icar_SendCmdXML(this.WorkId, Convert.ToInt32(OrderID), simnum, strCarType, Convert.ToInt32(CmdCode), Convert.ToInt32(CommFlag), strXML);
					if (num == (long)0)
					{
						ReadDataFromDB.execSaveGpsLogTable(this.WorkId, newOrderId, CarID, Convert.ToInt32(CmdCode), strXML);
					}
					else
					{
						AlarmMsg alarmMsg = new AlarmMsg()
						{
							ClassName = "GpsPicDownData",
							FunctionName = "icar_SetCmdXML"
						};
						string[] strArrays1 = new string[] { "异常发送：", strCarType, ",类型-", CmdCode.ToString(), ",车辆-", simnum, ",XML文件:", strXML };
						alarmMsg.AlarmText = string.Concat(strArrays1);
						alarmMsg.Code = num.ToString();
						this.log.WriteAlarm(alarmMsg);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg()
					{
						ClassName = "GpsPicDownData",
						ErrorText = "下发XML时发生错误!"
					};
					this.log.WriteError(errorMsg, exception);
					num = (long)-1;
				}
			}
			return num;
		}

		public long iCar_SetPosReport(CmdParam.ParamType ParamType, string SimNum, string CarPw, CmdParam.CommMode CommMode, PosReport posReport, int CarID)
		{
			long num = (long)0;
			this.logMsg.FunctionName = "iCar_SetPosReport";
			this.logMsg.Msg = string.Concat("发送：类型-", ParamType.ToString(), ",车辆-", SimNum);
			if (!this.isStartCommon())
			{
				num = (long)-1;
			}
			else
			{
				try
				{
					this.log.WriteLog(this.logMsg);
					int newOrderId = DownData.CarCmdSend.GetNewOrderId();
					num = DownData.CarCmdSend.icar_SetPosReport(this.WorkId, newOrderId, SimNum, posReport.ReportType, posReport.ReportTiming, posReport.ReportCycle, posReport.IsAutoCalArc, posReport.isCompressed, posReport.ReportWhenStop);
					if (num == (long)0)
					{
						ReadDataFromDB.execSaveGpsLogTable(this.WorkId, newOrderId, CarID.ToString(), (int)posReport.OrderCode, "LBS单次定位");
					}
					else
					{
						AlarmMsg alarmMsg = new AlarmMsg()
						{
							ClassName = "GpsPicDownData",
							FunctionName = "iCar_SetPosReport"
						};
						object[] workId = new object[] { "workid-", this.WorkId, ",simNum-", SimNum, ",strMsg-调用位置查询" };
						alarmMsg.AlarmText = string.Concat(workId);
						alarmMsg.Code = num.ToString();
						this.log.WriteAlarm(alarmMsg);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					ErrorMsg errorMsg = new ErrorMsg()
					{
						ClassName = "GpsPicDownData",
						ErrorText = "下发消息指令时发生错误!"
					};
					this.log.WriteError(errorMsg, exception);
					num = (long)-1;
				}
			}
			return num;
		}

		private bool isStartCommon()
		{
			bool flag = true;
			try
			{
				if (DownData.CarCmdSend == null)
				{
					DownData.CarCmdSend = new CarDownCmd.CarDownCmd(ReadDataFromXml.CommunicationUrl);
				}
				try
				{
					if (!DownData.CarCmdSend.isConnect() && DownData.CarCmdSend.StartCommServer() != 0)
					{
						AlarmMsg alarmMsg = new AlarmMsg()
						{
							ClassName = "GpsPicDownData",
							AlarmText = "启动通讯服务器发生错误!"
						};
						this.log.WriteAlarm(alarmMsg);
						flag = false;
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					DownData.CarCmdSend = new CarDownCmd.CarDownCmd(ReadDataFromXml.CommunicationUrl);
					if (DownData.CarCmdSend.StartCommServer() != 0)
					{
						ErrorMsg errorMsg = new ErrorMsg()
						{
							ClassName = "GpsPicDownData",
							ErrorText = "启动通讯服务器发生错误!"
						};
						this.log.WriteError(errorMsg, exception);
						flag = false;
					}
				}
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				ErrorMsg errorMsg1 = new ErrorMsg()
				{
					ClassName = "GpsPicDownData",
					ErrorText = "初始化通讯服务器发生错误!"
				};
				this.log.WriteError(errorMsg1, exception2);
				flag = false;
			}
			return flag;
		}
	}
}