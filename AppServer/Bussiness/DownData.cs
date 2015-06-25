namespace Bussiness
{
    using ParamLibrary.Application;
    using ParamLibrary.CmdParamInfo;
    using ParamLibrary.Entity;
    using DataAccess;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using ParamLibrary.Bussiness;

    public class DownData : SendBase
    {
        public DownData(int int_0, bool bool_3, bool bool_4, bool bool_5, OnlineUserInfo onlineUserInfo_0)
        {
            base.errMsg.ClassName = base.alarmMsg.ClassName = base.logMsg.ClassName = "DownData";
            base.WorkId = int_0;
            base.IsSudoOverDue = bool_4;
            base.IsMultiSend = bool_5;
            base.IsAllowNullPassWord = bool_3;
            base.userInfo = onlineUserInfo_0;
        }

        public Response icar_RemoteDial(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, RemoteDial remoteDial_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_RemoteDial";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1 + ",指令-" + remoteDial_0.OrderCode.ToString();
            string str = "电话号码-" + remoteDial_0.strPhone + ",消息-" + remoteDial_0.strMsg;
            base.logMsg.Msg = base.logMsg.Msg + "," + str;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            string strErrorMsg = string.Empty;
            if (remoteDial_0.CheckData(out strErrorMsg) != 0)
            {
                response.ErrorMsg = strErrorMsg;
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", remoteDial_0.OrderCode.ToString(), "等待", "", str);
                        response.ResultCode = SendBase.CarCmdSend.icar_RemoteDial(base.WorkId, newOrderId, info.SimNum, remoteDial_0.Phone.ToString(), remoteDial_0.strMsg);
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_RemoteDial";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", remoteDial_0.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_RemoteUpdate(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0)
        {
            byte[] buffer;
            Response response = new Response();
            base.logMsg.FunctionName = "icar_RemoteUpdate";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1;
            string str = string.Empty;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            string updateFileVersion = Const.UpdateFileVersion;
            string updateFilePath = Const.UpdateFilePath;
            FileStream input = null;
            BinaryReader reader = null;
            try
            {
                if (!string.IsNullOrEmpty(updateFilePath) && !string.IsNullOrEmpty(updateFilePath))
                {
                    if (!File.Exists(updateFilePath))
                    {
                        base.ErrorMsg = "不存在的升级文件！";
                        response.ErrorMsg = base.errMsg.ErrorText = "不存在的升级文件:FilePath-" + updateFilePath + ",FileVersion-" + updateFileVersion;
                        base.log.WriteError(base.errMsg);
                        return response;
                    }
                    input = new FileStream(updateFilePath, FileMode.Open);
                    reader = new BinaryReader(input);
                    buffer = new byte[input.Length];
                    reader.Read(buffer, 0, buffer.Length);
                    base.logMsg.Msg = string.Concat(new object[] { "升级文件大小：", buffer.Length, ",文件路径-", updateFilePath, ",文件版本-", updateFileVersion });
                    base.log.WriteLog(base.logMsg);
                }
                else
                {
                    base.ErrorMsg = "升级文件信息错误！";
                    response.ErrorMsg = base.errMsg.ErrorText = "升级文件信息错误:FilePath-" + updateFilePath + ",FileVersion-" + updateFileVersion;
                    base.log.WriteError(base.errMsg);
                    return response;
                }
            }
            catch (Exception exception)
            {
                base.ErrorMsg = "读取升级文件信息错误！";
                response.ErrorMsg = base.errMsg.ErrorText = "读取升级文件信息错误:FilePath-" + updateFilePath + ",FileVersion-" + updateFileVersion;
                base.log.WriteError(base.errMsg, exception);
                return response;
            }
            finally
            {
                if (input != null)
                {
                    input.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            object obj2 = buffer;
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", "远程升级车台软件", "等待", "", str);
                        response.ResultCode = SendBase.CarCmdSend.icar_RemoteUpdate(base.WorkId, newOrderId, info.SimNum, updateFileVersion, ref obj2);
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_RemoteUpdate";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception2)
                    {
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        base.log.WriteError(base.errMsg, exception2);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_SelMultiPathAlarm(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, PathAlarmList pathAlarmList_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SelMultiPathAlarm";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        Car car = new Car();
                        int num = -1;
                        for (int i = 0; i < pathAlarmList_0.Count; i++)
                        {
                            PathAlarm alarm = (PathAlarm) pathAlarmList_0[i];
                            DataTable table = car.GetNewPathId(info.CarId, alarm.PathName, num);
                            if (table == null)
                            {
                                goto Label_035B;
                            }
                            int num3 = int.Parse(table.Rows[0][0].ToString());
                            if (-1 == num3)
                            {
                                goto Label_039A;
                            }
                            num = int.Parse(table.Rows[0][1].ToString());
                            alarm.ID = num3;
                        }
                        object pvRegions = pathAlarmList_0.pvRegions;
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", pathAlarmList_0.OrderCode.ToString(), "等待", "", "");
                        car.InsertPathIntoGisCar(info.CarId, base.WorkId, newOrderId, pathAlarmList_0.AlarmPathDot);
                        response.ResultCode = SendBase.CarCmdSend.icar_SelMultiPathAlarm(base.WorkId, newOrderId, info.SimNum, ref pvRegions);
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SelMultiPathAlarm";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", pathAlarmList_0.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        for (int j = 0; j < pathAlarmList_0.Count; j++)
                        {
                            PathAlarm alarm2 = (PathAlarm) pathAlarmList_0[j];
                            car.UpdatePathAlarm(info.CarId, alarm2.PathName, 1, 0, 0, alarm2.ID, "", "", 0, 0, 0);
                        }
                        response.ResultCode = 0L;
                        continue;
                    Label_035B:
                        response.ErrorMsg = base.errMsg.ErrorText = string.Format("\"{0}\"获取偏移路线ID失败!", info.CarNum);
                        base.log.WriteError(base.errMsg);
                        return response;
                    Label_039A:
                        response.ErrorMsg = base.errMsg.ErrorText = string.Format("\"{0}\"偏移路线ID已满，请删除部分偏移路线!", info.CarNum);
                        base.log.WriteError(base.errMsg);
                        return response;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public AppRespone icar_SendRawPackage(AppRequest appRequest_0, object object_0)
        {
            AppRespone respone = new AppRespone();
            base.logMsg.FunctionName = "icar_SendRawPackage";
            base.logMsg.Msg = "发送：类型-" + appRequest_0.ParamType.ToString() + ",车辆-" + appRequest_0.CarValues;
            string str = string.Empty;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(appRequest_0.ParamType, appRequest_0.CarValues, appRequest_0.CarPw))
            {
                respone.ResultMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
            }
            if (!base.isStartCommon())
            {
                respone.ResultMsg = base.ErrorMsg;
                return respone;
            }
            long num = -1L;
            foreach (Bussiness.CarInfo info in base.carInfoList)
            {
                try
                {
                    int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                    base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                    base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", appRequest_0.OrderCode.ToString(), "等待", "", str);
                    if (info.ProtocolName == SendBase.m_ProtocolName)
                    {
                        TrafficRawPackage package = new TrafficRawPackage {
                            OrderCode = CmdParam.OrderCode.命令透传,
                            SubOrderCode = appRequest_0.OrderCode
                        };
                        StringBuilder builder = new StringBuilder();
                        byte[] buffer = (byte[]) object_0;
                        if (buffer != null)
                        {
                            for (int i = 0; i < buffer.Length; i++)
                            {
                                builder.Append(buffer[i].ToString("X2"));
                            }
                        }
                        package.strText = builder.ToString();
                        string conntent = "";
                        string str3 = package.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, -1, "SendRawPackage", ref conntent);
                        num = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, (int) package.OrderCode, -1, str3);
                    }
                    else
                    {
                        num = SendBase.CarCmdSend.icar_SendRawPackage(base.WorkId, newOrderId, info.SimNum, appRequest_0.CmdCode, ref object_0, appRequest_0.CommMode);
                    }
                    if (num != 0L)
                    {
                        base.alarmMsg.FunctionName = "icar_SendRawPackage";
                        base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", appRequest_0.OrderCode });
                        base.alarmMsg.Code = respone.ResultCode.ToString();
                        base.log.WriteAlarm(base.alarmMsg);
                    }
                }
                catch (Exception exception)
                {
                    respone.ResultMsg = base.ErrorMsg = base.errMsg.ErrorText = "下发消息指令时发生错误!";
                    respone.ResultCode = -1;
                    base.log.WriteError(base.errMsg, exception);
                }
            }
            return respone;
        }

        public Response icar_SendTxtMsg(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, TxtMsg txtMsg_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SendTxtMsg";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1 + ",指令-" + txtMsg_0.OrderCode.ToString();
            string str = "信息-" + txtMsg_0.strMsg + ",信息类型-" + txtMsg_0.MsgType.ToString();
            base.logMsg.Msg = base.logMsg.Msg + "," + str;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            string strErrorMsg = string.Empty;
            if (txtMsg_0.CheckData(out strErrorMsg) != 0)
            {
                response.ErrorMsg = strErrorMsg;
                return response;
            }
            if (base.isStartCommon())
            {
                string str3 = null;
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        switch (((int) txtMsg_0.MsgType))
                        {
                            case 240:
                            case 0xf1:
                            case 0xf2:
                            case 0xf3:
                                str3 = txtMsg_0.MsgType.ToString();
                                break;
                        }
                        base.SaveCommandParameterToGpsLogTable(newOrderId.ToString() + "|" + info.CarId.ToString() + ";", str3);
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", txtMsg_0.OrderCode.ToString(), "等待", "", str);
                        if (info.ProtocolName == SendBase.m_ProtocolName)
                        {
                            txtMsg_0.TransformCode = CmdParam.TrafficProtocolCodeExchange((int) txtMsg_0.MsgType);
                            string conntent = "";
                            string str5 = txtMsg_0.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SimpleCmd", ref conntent);
                            response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, txtMsg_0.TransformCode, (int) commMode_0, str5);
                        }
                        else
                        {
                            response.ResultCode = SendBase.CarCmdSend.icar_SendTxtMsg(base.WorkId, newOrderId, info.SimNum, txtMsg_0.MsgType, txtMsg_0.strMsg);
                        }
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SendTxtMsg";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", txtMsg_0.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_SetAlarmFlag(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, AlarmEntity alarmEntity_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetAlarmFlag";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1;
            string str = string.Concat(new object[] { "报警器开关标志位-", alarmEntity_0.CarAlarmSwitch.ToString(), ",报警器类型标志位-", alarmEntity_0.CarAlarmFlag.ToString(), ",扩展报警器开关标志位:", alarmEntity_0.CarAlarmSwitchEx, ",报警器类型标志位", alarmEntity_0.CarShowAlarmEx.ToString() });
            base.logMsg.Msg = base.logMsg.Msg + "," + str;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", alarmEntity_0.OrderCode.ToString(), "等待", "", str);
                        new Car().InsertAlamFlagIntoGisCar(info.CarId, base.WorkId.ToString(), newOrderId, (int) alarmEntity_0.CarAlarmSwitch, (int) alarmEntity_0.CarAlarmFlag, (int) alarmEntity_0.CarShowAlarm, (long) alarmEntity_0.AlarmFlagType, alarmEntity_0.AlarmFlagEx, alarmEntity_0.CarAlarmSwitchEx, alarmEntity_0.CarShowAlarmEx);
                        if (info.ProtocolName == SendBase.m_ProtocolName)
                        {
                            alarmEntity_0.TransformCode = (int) alarmEntity_0.AlarmFlagType;
                            string conntent = "";
                            string str3 = alarmEntity_0.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SetCommArg", ref conntent);
                            response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, alarmEntity_0.TransformCode, (int) commMode_0, str3);
                        }
                        else
                        {
                            response.ResultCode = SendBase.CarCmdSend.icar_SetAlarmFlag(base.WorkId, newOrderId, info.SimNum, (int) alarmEntity_0.CarAlarmSwitch, (int) alarmEntity_0.CarAlarmFlag);
                        }
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SetAlarmFlag";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", alarmEntity_0.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_SetBlackBox(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, BlackBox blackBox_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetBlackBox";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1 + ",指令-" + blackBox_0.OrderCode.ToString();
            string str = string.Concat(new object[] { "拐点补偿-", blackBox_0.IsAutoCalArc, ",自动上传数据-", blackBox_0.Flag, ",间隔-", blackBox_0.ReportCycle, ",汇报方式-", blackBox_0.ReportType });
            base.logMsg.Msg = base.logMsg.Msg + "," + str;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            string strErrorMsg = string.Empty;
            if (blackBox_0.CheckData(out strErrorMsg) != 0)
            {
                response.ErrorMsg = strErrorMsg;
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", blackBox_0.OrderCode.ToString(), "等待", "", str);
                        response.ResultCode = SendBase.CarCmdSend.icar_SetBlackBox(base.WorkId, newOrderId, info.SimNum, blackBox_0.ReportType, blackBox_0.ReportCycle, blackBox_0.IsAutoCalArc, blackBox_0.Flag);
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SetPosReport";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", blackBox_0.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_SetCallLimit(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, CallLimit callLimit_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetCallLimit";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1 + ",指令-" + callLimit_0.OrderCode.ToString();
            string str = "发送车台呼叫限制";
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            string strErrorMsg = string.Empty;
            if (callLimit_0.CheckData(out strErrorMsg) != 0)
            {
                response.ErrorMsg = strErrorMsg;
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        object callInPhone = callLimit_0.CallInPhone;
                        object callOutPhone = callLimit_0.CallOutPhone;
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", callLimit_0.OrderCode.ToString(), "等待", "", str);
                        SqlDataAccess access = new SqlDataAccess();
                        string str3 = string.Format(" insert into GisCarInfoTable_Tmp(carID, wrkID, orderID, carControlType, carControlMask, callInLst, callOutLst) values({0}, {1}, {2}, {3}, {4}, '{5}','{6}')", new object[] { info.CarId, base.WorkId, newOrderId, callLimit_0.FlagValue, callLimit_0.FlagMask, callLimit_0.CallInPhoneString, callLimit_0.CallOutPhoneString });
                        int num2 = access.insertBySql(str3);
                        if (num2 != 1)
                        {
                            base.alarmMsg.FunctionName = "icar_SetCallLimit";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", callLimit_0.OrderCode });
                            base.alarmMsg.Code = num2.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        long num3 = SendBase.CarCmdSend.icar_SetCallLimit(base.WorkId, newOrderId, info.SimNum, callLimit_0.FlagValue, callLimit_0.FlagMask, ref callInPhone, ref callOutPhone);
                        if (num3 != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SetCallLimit";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", callLimit_0.OrderCode });
                            base.alarmMsg.Code = num3.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_SetCaptureEx(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, CaptureEx captureEx_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetCaptureEx";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1;
            string str = string.Concat(new object[] { 
                "是否多帧-", captureEx_0.IsMulitFramebool, ",拍摄类型:", captureEx_0.Type, ",监控次数-", captureEx_0.Times, ",间隔时间-", captureEx_0.Interval * 0.1, ",图像质量-", captureEx_0.Quality, ",图像亮度-", captureEx_0.Brightness, ",图像对比度-", captureEx_0.Contrast, ",图像饱和度-", captureEx_0.Saturation, 
                ",图像色度", captureEx_0.Chroma, ",停车是否拍照-", captureEx_0.IsCapWhenStop, ",图像分辨率-", captureEx_0.PSize
             });
            if (!string.IsNullOrEmpty(captureEx_0.BeginTime) && !string.IsNullOrEmpty(captureEx_0.EndTime))
            {
                string str5 = str;
                str = str5 + ",时间段拍照启始时间-" + captureEx_0.BeginTime + ",时间段拍照结束时间-" + captureEx_0.EndTime;
            }
            base.logMsg.Msg = base.logMsg.Msg + "," + str;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            string strErrorMsg = string.Empty;
            if (captureEx_0.CheckData(out strErrorMsg) != 0)
            {
                response.ErrorMsg = strErrorMsg;
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", captureEx_0.OrderCode.ToString(), "等待", "", str);
                        if ((info.ProtocolName == SendBase.m_ProtocolName) && (captureEx_0.protocolType == CarProtocolType.交通厅))
                        {
                            string conntent = "";
                            captureEx_0.TransformCode = CmdParam.TrafficProtocolCodeExchange2((int) captureEx_0.OrderCode);
                            if (captureEx_0.Quality == 0)
                            {
                                captureEx_0.Quality = 1;
                            }
                            if (captureEx_0.CaptureCache == 1)
                            {
                                captureEx_0.CaptureCache = -1;
                            }
                            string str4 = captureEx_0.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SetCapture", ref conntent);
                            response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, captureEx_0.TransformCode, (int) commMode_0, str4);
                        }
                        else
                        {
                            captureEx_0.Quality = byte.Parse(((10 - captureEx_0.Quality) / 2).ToString());
                            captureEx_0.CaptureCache = 0;
                            if (!string.IsNullOrEmpty(captureEx_0.BeginTime) && !string.IsNullOrEmpty(captureEx_0.EndTime))
                            {
                                response.ResultCode = SendBase.CarCmdSend.icar_SetCaptureExWithTime(base.WorkId, newOrderId, info.SimNum, captureEx_0.IsMultiFrame, captureEx_0.CamerasID, captureEx_0.CaptureFlag, captureEx_0.CaptureCache, captureEx_0.Times, captureEx_0.Interval, captureEx_0.Quality, captureEx_0.Brightness, captureEx_0.Contrast, captureEx_0.Saturation, captureEx_0.Chroma, captureEx_0.CapWhenStop, captureEx_0.BeginTime, captureEx_0.EndTime);
                            }
                            else
                            {
                                response.ResultCode = SendBase.CarCmdSend.icar_SetCaptureEx(base.WorkId, newOrderId, info.SimNum, captureEx_0.IsMultiFrame, captureEx_0.CamerasID, captureEx_0.CaptureFlag, captureEx_0.CaptureCache, captureEx_0.Times, captureEx_0.Interval, captureEx_0.Quality, captureEx_0.Brightness, captureEx_0.Contrast, captureEx_0.Saturation, captureEx_0.Chroma, captureEx_0.CapWhenStop);
                            }
                        }
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SetCaptureEx";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", captureEx_0.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        else
                        {
                            new Car().InsertIntoCaptureParam(info.CarId, captureEx_0);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_SetCommArg(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, CommArgs commArgs_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetCommArg";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1;
            string str = string.Concat(new object[] { 
                "通讯方式-", commArgs_0.CommMode.ToString(), ",TCPIP-", commArgs_0.strTCPIP, ",TCP端口-", commArgs_0.TCPPort, ",UDPIP-", commArgs_0.strUDPIP, "，UDP端口-", commArgs_0.UDPPort, ",拨号用户名-", commArgs_0.strUser, ",是否使用代理-", commArgs_0.IsUseProxy.ToString(), ",代理IP-", commArgs_0.strProxyIP, 
                ",代理端口-", commArgs_0.TCPPort, ",服务器类型-", commArgs_0.ServerType
             });
            base.logMsg.Msg = base.logMsg.Msg + "," + str;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            string strErrorMsg = string.Empty;
            if (commArgs_0.CheckData(out strErrorMsg) != 0)
            {
                response.ErrorMsg = strErrorMsg;
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", commArgs_0.OrderCode.ToString(), "等待", "", str);
                        new Car().InsertComArsgIntoGisCar(info.CarId, base.WorkId.ToString(), newOrderId, (int) commArgs_0.CommMode, commArgs_0.strAPNAddr, commArgs_0.strUser, commArgs_0.strPassword, commArgs_0.strTCPIP, commArgs_0.TCPPort.ToString(), commArgs_0.strUDPIP, commArgs_0.UDPPort.ToString(), (int) commArgs_0.IsUseProxy, commArgs_0.strProxyIP, commArgs_0.ProxyPort.ToString(), commArgs_0.ServerType.ToString());
                        if (info.ProtocolName == SendBase.m_ProtocolName)
                        {
                            commArgs_0.TransformCode = CmdParam.TrafficProtocolCodeExchange((int) commArgs_0.OrderCode);
                            string conntent = "";
                            string str4 = commArgs_0.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SetCommArg", ref conntent);
                            response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, commArgs_0.TransformCode, (int) commMode_0, str4);
                        }
                        else
                        {
                            response.ResultCode = SendBase.CarCmdSend.icar_SetCommArg(base.WorkId, newOrderId, info.SimNum, commArgs_0.CommMode, commArgs_0.strAPNAddr, commArgs_0.strUser, commArgs_0.strPassword, commArgs_0.strTCPIP, commArgs_0.TCPPort, commArgs_0.strUDPIP, commArgs_0.UDPPort, commArgs_0.IsUseProxy, commArgs_0.strProxyIP, commArgs_0.ProxyPort);
                        }
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SetCommArg";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", commArgs_0.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_SetCustomAlarmer(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, AlarmEntity alarmEntity_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetCustomAlarmer";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1 + ",指令-" + alarmEntity_0.OrderCode.ToString();
            string str = "配置自定义报警器";
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            string strErrorMsg = string.Empty;
            if (alarmEntity_0.CheckData(out strErrorMsg) != 0)
            {
                response.ErrorMsg = strErrorMsg;
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", alarmEntity_0.OrderCode.ToString(), "等待", "", str);
                        new Car().InsertCustAlarmIntoGisCar(info.CarId, base.WorkId.ToString(), newOrderId, alarmEntity_0.CarAlarmSwitch, alarmEntity_0.CarAlarmFlag, alarmEntity_0.CarShowAlarm, alarmEntity_0.Level, alarmEntity_0.CustName);
                        response.ResultCode = SendBase.CarCmdSend.icar_SetCustomAlarmer(base.WorkId, newOrderId, info.SimNum, (uint) alarmEntity_0.CarAlarmSwitch, (uint) alarmEntity_0.CarAlarmFlag, (uint) alarmEntity_0.Level);
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SetCustomAlarmer";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", alarmEntity_0.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_SetLastDotQuery(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, CmdParam.OrderCode orderCode_0)
        {
            string format = "车辆{0}末次位置查询未找到该车辆轨迹";
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetLastDotQuery";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1 + ",指令-" + orderCode_0.ToString();
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        DataRow lastDotData = new Car().GetLastDotData(info.SimNum);
                        if (lastDotData == null)
                        {
                            base.alarmMsg.FunctionName = "icar_SetLastDotQuery";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", orderCode_0.ToString() });
                            base.alarmMsg.Code = "-1";
                            base.log.WriteAlarm(base.alarmMsg);
                            base.userInfo.DownCommd.AddCarNewLogData(0, info.CarNum, "信息", "提示信息", "", "", string.Format(format, info.CarNum));
                        }
                        else
                        {
                            CmdParam.CommMode mode = CmdParam.CommMode.短信;
                            CarPartInfo info2 = this.method_5(lastDotData, out mode);
                            base.userInfo.DownCommd.AddCarNewLogData(0, info.CarNum, "接收", orderCode_0.ToString(), "成功", mode.ToString(), info2.GetCarCurrentInfo(), info2.Lon, info2.Lat, info2.AccOn, info2.Speed, info2.IsFill, info2.GpsValid, info.CarId, info2.Direct, info2.ReceTime, info2.GpsTime);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception)
                    {
                        response.ErrorMsg = base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_SetMinSMSReportInterval(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, MinSMSReportInterval minSMSReportInterval_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetMinSMSReportInterval";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1;
            string str = string.Concat(new object[] { "短信模式-", minSMSReportInterval_0.SMSMode, ",混合模式-", minSMSReportInterval_0.MixedMode });
            base.logMsg.Msg = base.logMsg.Msg + "," + str;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            string strErrorMsg = string.Empty;
            if (minSMSReportInterval_0.CheckData(out strErrorMsg) != 0)
            {
                response.ErrorMsg = strErrorMsg;
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", minSMSReportInterval_0.OrderCode.ToString(), "等待", "", str);
                        response.ResultCode = SendBase.CarCmdSend.icar_SetMinSMSReportInterval(base.WorkId, newOrderId, info.SimNum, minSMSReportInterval_0.SMSMode, minSMSReportInterval_0.MixedMode);
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SetMinSMSReportInterval";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", minSMSReportInterval_0.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_SetMultiSegSpeedAlarm(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, PathAlarmList pathAlarmList)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetMultiSegSpeedAlarm";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        Car car = new Car();
                        int num = -1;
                        for (int i = 0; i < pathAlarmList.Count; i++)
                        {
                            PathAlarm alarm = (PathAlarm) pathAlarmList[i];
                            DataTable table = car.GetNewPathId(info.CarId, alarm.PathName, num);
                            if (table == null)
                            {
                                response.ErrorMsg = base.errMsg.ErrorText = string.Format("\"{0}\"获取偏移路线ID失败!", info.CarNum);
                                base.log.WriteError(base.errMsg);
                                return response;
                            
                            }
                            int num3 = int.Parse(table.Rows[0][0].ToString());
                            if (-1 == num3)
                            {
                                response.ErrorMsg = base.errMsg.ErrorText = string.Format("\"{0}\"偏移路线ID已满，请删除部分偏移路线!", info.CarNum);
                                base.log.WriteError(base.errMsg);
                                return response;
                            }
                            num = int.Parse(table.Rows[0][1].ToString());
                            alarm.ID = num3;
                        }
                        object pvRegions = pathAlarmList.pvRegions;
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        if ((info.ProtocolName != SendBase.m_ProtocolName) && (pathAlarmList.protocolType == CarProtocolType.交通厅))
                        {
                            base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "接收", pathAlarmList.OrderCode.ToString(), "失败", "", "终端不支持该协议!");
                            base.errMsg.ErrorText = "下发消息指令时发生错误!";
                            response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                            return response;
                        }
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", pathAlarmList.OrderCode.ToString(), "等待", "", "");
                        car.InsertPathIntoGisCar(info.CarId, base.WorkId, newOrderId, pathAlarmList.AlarmPathDot);
                        if (pathAlarmList.OrderCode == CmdParam.OrderCode.设置分路段超速报警)
                        {
                            for (int j = 0; j < pathAlarmList.Count; j++)
                            {
                                PathAlarm alarm2 = (PathAlarm) pathAlarmList[j];
                                if (((alarm2.PathSegmentAlarmList != null) && (alarm2.PathSegmentAlarmList.Count != 0)) || !(info.ProtocolName == SendBase.m_ProtocolName))
                                {
                                    car.UpdateTrafficPathAlarm_tmp(base.WorkId, newOrderId, info.CarId, alarm2.PathName, 1, alarm2.Speed, alarm2.Time, alarm2.ID, (alarm2.BeginTime == null) ? "" : alarm2.BeginTime.ToString(), (alarm2.EndTime == null) ? "" : alarm2.EndTime.ToString(), alarm2.PathFlag, alarm2.DriEnough, alarm2.DriNoEnough);
                                }
                            }
                        }
                        if ((info.ProtocolName == SendBase.m_ProtocolName) && (pathAlarmList.protocolType == CarProtocolType.交通厅))
                        {
                            for (int k = 0; k < pathAlarmList.Count; k++)
                            {
                                PathAlarm alarm3 = (PathAlarm) pathAlarmList[k];
                                string conntent = "";
                                alarm3.OrderCode = pathAlarmList.OrderCode;
                                string str2 = alarm3.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SetPathAlarm", ref conntent);
                                response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, (int) alarm3.OrderCode, (int) commMode_0, str2);
                            }
                        }
                        else
                        {
                            if ((info.ProtocolName != SendBase.m_ProtocolName) && (pathAlarmList.protocolType == CarProtocolType.交通厅))
                            {
                                base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "接收", pathAlarmList.OrderCode.ToString(), "失败", "", "终端不支持该协议!");
                                response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                                response.ResultCode = 0L;
                                return response;
                            }
                            response.ResultCode = SendBase.CarCmdSend.icar_SimpleCmd(base.WorkId, newOrderId, info.SimNum, CmdParam.CmdCode.设置分段超速报警, ref pvRegions);
                        }
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SetMultiSegSpeedAlarm";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", pathAlarmList.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        int num7 = 0;
                        while (true)
                        {
                            if (num7 >= pathAlarmList.Count)
                            {
                                break;
                            }
                            PathAlarm alarm4 = (PathAlarm) pathAlarmList[num7];
                            if ((pathAlarmList.OrderCode == CmdParam.OrderCode.设置分路段超速报警) && (pathAlarmList.protocolType == CarProtocolType.交通厅))
                            {
                                try
                                {
                                    if ((info.ProtocolName == SendBase.m_ProtocolName) && (pathAlarmList.protocolType == CarProtocolType.交通厅))
                                    {
                                        for (int m = 0; m < alarm4.PathSegmentAlarmList.Count; m++)
                                        {
                                            car.InsertTrafficSegmentParam(info.CarId, alarm4.ParentID, alarm4.PathSegmentAlarmList[m].PathSegmentID, alarm4.PathSegmentAlarmList[m].TopSpeed, alarm4.PathSegmentAlarmList[m].HoldTime, alarm4.PathSegmentAlarmList[m].DriEnough, alarm4.PathSegmentAlarmList[m].DriNoEnough, alarm4.PathSegmentAlarmList[m].Flag, alarm4.PathSegmentAlarmList[m].PathWidth);
                                        }
                                    }
                                }
                                catch (Exception exception)
                                {
                                    base.log.WriteError(base.errMsg, exception);
                                }
                            }
                            num7++;
                        }
                        response.ResultCode = 0L;
                        continue;
                    }
                    catch (Exception exception2)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception2);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_SetPhone(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, SetPhone phone)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetPhone";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1;
            string str = "电话类型-" + phone.PhoneType.ToString() + ",电话号码-" + phone.strPhone;
            base.logMsg.Msg = base.logMsg.Msg + "," + str;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            string strErrorMsg = string.Empty;
            if (phone.CheckData(out strErrorMsg) != 0)
            {
                response.ErrorMsg = strErrorMsg;
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        base.m_OrderCode = "255";
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCommandParameterToGpsLogTable(newOrderId.ToString() + "|" + info.CarId.ToString() + ";", Convert.ToInt32(phone.OrderCode).ToString());
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", phone.OrderCode.ToString(), "等待", "", str);
                        new Car().InsertPhonesIntoGisCar(phone.PhoneType, info.CarId, base.WorkId.ToString(), newOrderId.ToString(), phone.strPhone);
                        if (info.ProtocolName == SendBase.m_ProtocolName)
                        {
                            phone.TransformCode = CmdParam.TrafficProtocolCodeExchange((int) phone.PhoneType);
                            string conntent = "";
                            string str4 = phone.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SetPhone", ref conntent);
                            response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, phone.TransformCode, (int) commMode_0, str4);
                        }
                        else
                        {
                            response.ResultCode = SendBase.CarCmdSend.icar_SetPhone(base.WorkId, newOrderId, info.SimNum, phone.PhoneType, phone.strPhone);
                        }
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SetPhone";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", phone.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_SetPosReport(CmdParam.ParamType paramType, string CarNum, string pwd, CmdParam.CommMode commMode, PosReport posReport)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetPosReport";
            base.logMsg.Msg = "发送：类型-" + paramType.ToString() + ",车辆-" + CarNum + ",指令-" + posReport.OrderCode.ToString();
            string str = string.Concat(new object[] { "拐点补偿-", posReport.IsAutoCalArc, ",是否压缩-", posReport.isCompressed.ToString(), ",压缩上传时间-", posReport.CompressionUpTime, ",间隔-", posReport.LowReportCycle, ",持续-", posReport.ReportTiming, ",汇报类型-", posReport.ReportType, ",停车是否汇报-", posReport.ReportWhenStop });
            base.logMsg.Msg = base.logMsg.Msg + "," + str;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType, CarNum, pwd))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            string strErrorMsg = string.Empty;
            if (posReport.CheckData(out strErrorMsg) != 0)
            {
                base.ErrorMsg = strErrorMsg;
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        if ((posReport.isCompressed == CmdParam.IsCompressed.压缩传送) && (posReport.ReportType == CmdParam.ReportType.定次汇报))
                        {
                            posReport.ReportTiming *= 10;
                        }
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", posReport.OrderCode.ToString(), "等待", "", str);
                        if ((info.ProtocolName == SendBase.m_ProtocolName) && (posReport.protocolType == CarProtocolType.交通厅))
                        {
                            posReport.TransformCode = CmdParam.TrafficProtocolCodeExchange2((int) posReport.ReportType);
                            string conntent = "";
                            string str4 = posReport.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode, "SetPosReport", ref conntent);
                            response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, posReport.TransformCode, (int) commMode, str4);
                        }
                        else
                        {
                            response.ResultCode = SendBase.CarCmdSend.icar_SetPosReport(base.WorkId, newOrderId, info.SimNum, posReport.ReportType, posReport.ReportTiming, posReport.ReportCycle, posReport.IsAutoCalArc, posReport.isCompressed, posReport.ReportWhenStop);
                        }
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SetPosReport";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", posReport.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        else
                        {
                            base.userInfo.CarFilter.UpdatePosSearchFlag(posReport.OrderCode.ToString(), newOrderId, info.CarId);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        /// <summary>
        /// 设置区域报警
        /// </summary>
        /// <param name="ParamType"></param>
        /// <param name="CarValues"></param>
        /// <param name="CarPw"></param>
        /// <param name="CommMode"></param>
        /// <param name="regionAlarmList"></param>
        /// <returns></returns>
        public Response icar_SetRegionAlarm(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, RegionAlarmList regionAlarmList)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetRegionAlarm";
            base.logMsg.Msg = "发送：类型-" + ParamType.ToString() + ",车辆-" + CarValues;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(ParamType, CarValues, CarPw))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            if (base.isStartCommon())
            {
                Car car = new Car();
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        RegionAlarm alarm;
                        int num = -1;
                        for (int i = 0; i < regionAlarmList.Count; i++)
                        {
                            alarm = (RegionAlarm) regionAlarmList[i];
                            ///////////////////////////////////////////
                            //进行反纠偏处理
                            string LatLngString = alarm.AlarmRegionDot;
                            string[] latlng = LatLngString.Trim(new char[]{'\\'}).Split('\\');
                            LatLngString = latlng[0].ToString() + "\\";
                            for (int j = 0; j < (latlng.Length-1) / 2; j++)
                            {
                                double lng = Convert.ToDouble(latlng[j * 2 + 1]);
                                double lat = Convert.ToDouble(latlng[j * 2 + 2]);
                                GoogleOffset.getMars(ref lng, ref lat);
                                LatLngString = LatLngString + lng.ToString() + "\\" + lat.ToString() + "\\";
                            }
                            if ((latlng.Length/2)*2 == latlng.Length)
                            {
                                LatLngString = LatLngString + latlng[latlng.Length - 1];
                            }
                            alarm.AlarmRegionDot = LatLngString.Trim(new char[]{'\\'});
                            ///////////////////////////////////////////
                            DataTable table = car.GetNewRegionId(info.CarId, alarm.PathName, num);
                            if ((table == null) || (table.Rows.Count == 0))
                            {
                                base.errMsg.ErrorText = string.Format("\"{0}\"获取报警区域ID失败!", info.CarNum);
                                response.ErrorMsg = base.errMsg.ErrorText;
                                base.log.WriteError(base.errMsg);
                                return response;
                            }
                            int num3 = int.Parse(table.Rows[0][0].ToString());
                            alarm.newRegionId = num3;
                            num = num3;
                        }
                        object pvRegions = null;
                        if (regionAlarmList.protocolType == CarProtocolType.非交通厅)
                        {
                            pvRegions = regionAlarmList.pvRegions;
                        }
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        if ((info.ProtocolName != SendBase.m_ProtocolName) && (regionAlarmList.protocolType == CarProtocolType.交通厅))
                        {
                            base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", regionAlarmList.OrderCode.ToString(), "失败", "", "终端不支持该协议！");
                            response.ResultCode = 0L;
                            return response;
                        }
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", regionAlarmList.OrderCode.ToString(), "等待", "", "");
                        car.InsertRegionIntoGisCar(info.CarId, base.WorkId, newOrderId, regionAlarmList.AlarmRegionDot, regionAlarmList.RegionFeature.ToString());
                        RegionAlarmList list = new RegionAlarmList();
                        RegionAlarmList list2 = new RegionAlarmList();
                        RegionAlarmList list3 = new RegionAlarmList();
                        for (int j = 0; j < regionAlarmList.Count; j++)
                        {
                            alarm = (RegionAlarm) regionAlarmList[j];
                            switch (alarm.GetSharpe())
                            {
                                case 1:
                                    list.Add(alarm);
                                    break;

                                case 2:
                                    list2.Add(alarm);
                                    break;

                                case 3:
                                    list3.Add(alarm);
                                    break;
                            }
                            car.UpdateRegionParam(base.WorkId, newOrderId, int.Parse(info.CarId), alarm.RegionID, alarm.param, alarm.toEndTime, alarm.toBackTime, alarm.RegionType & 15, alarm.BeginTime, alarm.EndTime, regionAlarmList.RegionFeature, alarm.AlarmCondition.ToString(), alarm.PlanUpTime, alarm.PlanDownTime, alarm.newRegionId, alarm.AlarmFlag, alarm.MaxSpeed, alarm.HodeTime);
                        }
                        if (regionAlarmList.RegionFeature == 1)
                        {
                            if ((info.ProtocolName == SendBase.m_ProtocolName) && (regionAlarmList.protocolType == CarProtocolType.交通厅))
                            {
                                if (list.Count > 0)
                                {
                                    string content = "";
                                    list.OrderCode = regionAlarmList.OrderCode;
                                    string str2 = list.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) CommMode, "SetRegionAlarm", ref content);
                                    if (!string.IsNullOrEmpty(content))
                                    {
                                        response.ErrorMsg = content + "经纬度数据有问题";
                                        response.ResultCode = -1L;
                                        return response;
                                    }
                                    response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, (int) list.OrderCode, (int) CommMode, str2);
                                }
                                if (list2.Count > 0)
                                {
                                    string str3 = "";
                                    list2.OrderCode = regionAlarmList.OrderCode;
                                    string str4 = list2.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) CommMode, "SetRegionAlarm", ref str3);
                                    if (!string.IsNullOrEmpty(str3))
                                    {
                                        response.ErrorMsg = str3 + "经纬度数据有问题";
                                        response.ResultCode = -1L;
                                        return response;
                                    }
                                    response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, (int) list2.OrderCode, (int) CommMode, str4);
                                }
                                if (list3.Count > 0)
                                {
                                    string str5 = "";
                                    list3.OrderCode = regionAlarmList.OrderCode;
                                    string str6 = list3.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) CommMode, "SetRegionAlarm", ref str5);
                                    if (!string.IsNullOrEmpty(str5))
                                    {
                                        response.ErrorMsg = str5 + "经纬度数据有问题";
                                        response.ResultCode = -1L;
                                        return response;
                                    }
                                    response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, (int) list3.OrderCode, (int) CommMode, str6);
                                }
                            }
                            else
                            {
                                response.ResultCode = SendBase.CarCmdSend.icar_SimpleCmd(base.WorkId, newOrderId, info.SimNum, regionAlarmList.CmdCode, ref pvRegions);
                            }
                        }
                        else
                        {
                            response.ResultCode = SendBase.CarCmdSend.icar_SetMultiRegionAlarm(base.WorkId, newOrderId, info.SimNum, ref pvRegions);
                        }
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SetRegionAlarm";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", regionAlarmList.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        response.ResultCode = 0L;
                        continue;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_SetSpeedAlarm(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, SpeedAlarm speedAlarm_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetSpeedAlarm";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1;
            string str = string.Concat(new object[] { "最高速度-", speedAlarm_0.MaxSpeed, ",持续时间-", speedAlarm_0.RealHoldTime, ",手柄提示间隔-", speedAlarm_0.HoldTime });
            base.logMsg.Msg = base.logMsg.Msg + "," + str;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            string strErrorMsg = string.Empty;
            if (speedAlarm_0.CheckData(out strErrorMsg) != 0)
            {
                response.ErrorMsg = strErrorMsg;
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", speedAlarm_0.OrderCode.ToString(), "等待", "", str);
                        if (info.ProtocolName == SendBase.m_ProtocolName)
                        {
                            string conntent = "";
                            speedAlarm_0.TransformCode = CmdParam.TrafficProtocolCodeExchange2((int) speedAlarm_0.OrderCode);
                            string str4 = speedAlarm_0.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SetTransportReport", ref conntent);
                            response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, speedAlarm_0.TransformCode, (int) commMode_0, str4);
                        }
                        else
                        {
                            response.ResultCode = SendBase.CarCmdSend.icar_SetSpeedAlarm(base.WorkId, newOrderId, info.SimNum, speedAlarm_0.MaxSpeed, speedAlarm_0.HoldTime);
                        }
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SetSpeedAlarm";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", speedAlarm_0.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_SetTransportReport(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, TransportReport transportReport_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetTransportReport";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1 + ",指令-" + transportReport_0.OrderCode.ToString();
            string paramDisc = transportReport_0.GetParamDisc();
            base.logMsg.Msg = base.logMsg.Msg + "," + paramDisc;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            string strErrorMsg = string.Empty;
            if (transportReport_0.CheckData(out strErrorMsg) != 0)
            {
                response.ErrorMsg = strErrorMsg;
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", transportReport_0.OrderCode.ToString(), "等待", "", paramDisc);
                        response.ResultCode = SendBase.CarCmdSend.icar_SetTransportReport(base.WorkId, newOrderId, info.SimNum, transportReport_0.ReportFlag, transportReport_0.nStatuFree, transportReport_0.nStatuBusy, transportReport_0.nStatuTask);
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SetPosReport";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", transportReport_0.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_SimpleCmd(CmdParam.ParamType ParamType, string CarValues, string CarPw, CmdParam.CommMode CommMode, SimpleCmd simpleCmd)
        {
            Response response = new Response();
            string paramDisc = simpleCmd.GetParamDisc();
            base.WriteLog(ParamType.ToString(), CarValues + paramDisc);
            if (!base.CheckCar(ParamType, CarValues, CarPw))
            {
                response.ErrorMsg = base.ErrorMsg;
                return response;
            }
            string strErrorMsg = string.Empty;
            if (simpleCmd.CheckData(out strErrorMsg) != 0)
            {
                response.ErrorMsg = strErrorMsg;
                return response;
            }
            if (!base.isStartCommon())
            {
                response.ErrorMsg = base.ErrorMsg;
                return response;
            }
            object pvArg = null;
            int newOrderId = -1;
            foreach (Bussiness.CarInfo info in base.carInfoList)
            {
                try
                {
                    if (simpleCmd.OrderCode == CmdParam.OrderCode.设置车台参数)
                    {
                        response.ResultCode = new Car().UpdateCarconfigOnDuty(info.CarId, simpleCmd.OnDuty, simpleCmd.CloseGSM, simpleCmd.CloseDail);
                    }
                    newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                    base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                    string orderIDParam = response.OrderIDParam;
                    response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                    base.AddUpDataLog(newOrderId, info.CarNum, simpleCmd.OrderCode.ToString(), paramDisc);
                    switch (simpleCmd.CmdCode)
                    {
                        case CmdParam.CmdCode.取消区域报警:
                            new Car().InsertRegionIdsIntoGisCar(info.CarId, base.WorkId.ToString(), newOrderId.ToString(), simpleCmd.RegionIds);
                            break;

                        case CmdParam.CmdCode.取消路线报警:
                            new Car().InsertPathIdsIntoGisCar(info.CarId, base.WorkId.ToString(), newOrderId.ToString(), simpleCmd.RegionIds.Trim(new char[] { '\\' }).Equals("0") ? "" : simpleCmd.RegionIds);
                            break;

                        case CmdParam.CmdCode.配置串口参数:
                            if (response.ResultCode == 0L)
                            {
                                new Car().InsertGpsCarDeviceParam(info.CarId, base.WorkId, newOrderId, simpleCmd.Com1Device, simpleCmd.Com2Device);
                            }
                            break;
                    }
                    if (info.ProtocolName == SendBase.m_ProtocolName)
                    {
                        int orderID = base.CalOrderId(base.WorkId, newOrderId);
                        string conntent = string.Empty;
                        simpleCmd.TransformCode = CmdParam.TrafficProtocolCodeExchange2((int) simpleCmd.OrderCode);
                        string str4 = string.Empty;
                        if (simpleCmd.CmdCode == CmdParam.CmdCode.取消区域报警)
                        {
                            try
                            {
                                string[] strArray = simpleCmd.RegionIds.Split(@"\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                string[] strArray2 = simpleCmd.RegionTypes.Split(@"\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                if (strArray2.Length <= 0)
                                {
                                    throw new Exception();
                                }
                                List<string> list = new List<string>();
                                List<string> list2 = new List<string>();
                                for (int i = 1; i <= 3; i++)
                                {
                                    for (int j = 0; j < strArray2.Length; j++)
                                    {
                                        if (strArray2[j].Equals(i.ToString()))
                                        {
                                            list.Add(strArray[j]);
                                            list2.Add(i.ToString());
                                        }
                                    }
                                    if (list.Count != 0)
                                    {
                                        simpleCmd.RegionIds = string.Join(@"\", list.ToArray());
                                        simpleCmd.RegionTypes = string.Join(@"\", list2.ToArray());
                                        str4 = simpleCmd.ToXmlString(orderID, info.SimNum, SendBase.m_ProtocolName, (int) CommMode, "SimpleCmd", ref conntent);
                                        list.Clear();
                                        list2.Clear();
                                        response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, simpleCmd.TransformCode, (int) CommMode, str4);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", simpleCmd.OrderCode.ToString(), "失败", "", "终端不支持该命令");
                                response.ErrorMsg = "终端不支持该命令";
                            }
                        }
                        else if ((simpleCmd.OrderCode != CmdParam.OrderCode.取消出入口分段超速报警) && (simpleCmd.OrderCode != CmdParam.OrderCode.取消所有关键区域))
                        {
                            if ((simpleCmd.OrderCode == CmdParam.OrderCode.取消偏移路线报警) && simpleCmd.RegionIds.Trim(new char[] { '\\' }).Equals("0"))
                            {
                                simpleCmd.RegionIds = "";
                            }
                            str4 = simpleCmd.ToXmlString(orderID, info.SimNum, SendBase.m_ProtocolName, (int) CommMode, "SimpleCmd", ref conntent);
                            response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, simpleCmd.TransformCode, (int) CommMode, str4);
                        }
                        else
                        {
                            TrafficRawPackage package = new TrafficRawPackage {
                                OrderCode = CmdParam.OrderCode.命令透传,
                                SubOrderCode = simpleCmd.OrderCode,
                                strText = ""
                            };
                            str4 = package.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, -1, "SendRawPackage", ref conntent);
                            response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, (int) package.OrderCode, -1, str4);
                        }
                    }
                    else
                    {
                        pvArg = simpleCmd.pvArg;
                        response.ResultCode = SendBase.CarCmdSend.icar_SimpleCmd(base.WorkId, newOrderId, info.SimNum, simpleCmd.CmdCode, ref pvArg);
                    }
                    if (response.ResultCode != 0L)
                    {
                        base.WriteError(base.WorkId.ToString(), info.SimNum, simpleCmd.OrderCode.ToString());
                    }
                    response.ResultCode = 0L;
                }
                catch (Exception exception)
                {
                    response.ErrorMsg = "下发消息指令时发生错误,错误详细信息：" + exception.Message;
                }
            }
            return response;
        }

        public Response icar_SimpleCmdEx(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, SimpleCmdEx simpleCmdEx_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SimpleCmdEx";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1 + "，通讯方式-" + simpleCmdEx_0.CommFlag.ToString();
            string paramDisc = simpleCmdEx_0.GetParamDisc();
            base.logMsg.Msg = base.logMsg.Msg + ',' + paramDisc;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            string strErrorMsg = string.Empty;
            if (simpleCmdEx_0.CheckData(out strErrorMsg) != 0)
            {
                response.ErrorMsg = strErrorMsg;
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        object pvArg = simpleCmdEx_0.pvArg;
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", simpleCmdEx_0.OrderCode.ToString(), "等待", "", paramDisc);
                        if (info.ProtocolName == SendBase.m_ProtocolName)
                        {
                            simpleCmdEx_0.TransformCode = CmdParam.TrafficProtocolCodeExchange((int) simpleCmdEx_0.OrderCode);
                            string conntent = "";
                            string str4 = simpleCmdEx_0.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SimpleCmd", ref conntent);
                            response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, simpleCmdEx_0.TransformCode, (int) commMode_0, str4);
                        }
                        else
                        {
                            response.ResultCode = SendBase.CarCmdSend.icar_SimpleCmdEx(base.WorkId, newOrderId, info.SimNum, simpleCmdEx_0.CmdCode, ref pvArg, simpleCmdEx_0.CommFlag);
                        }
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SimpleCmdEx";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", simpleCmdEx_0.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        public Response icar_SmallArea(string string_1, string string_2, string string_3, TxtMsg txtMsg_0, string string_4, string string_5)
        {
            Response response = new Response();
            DataTable table = base.GetCarInfoByArea(string_1, string_2, string_3, string_4, string_5);
            if ((table != null) && (table.Rows.Count != 0))
            {
                string str = "信息-" + txtMsg_0.strMsg + ",信息类型-" + txtMsg_0.MsgType.ToString();
                string strErrorMsg = string.Empty;
                if (txtMsg_0.CheckData(out strErrorMsg) != 0)
                {
                    response.ErrorMsg = strErrorMsg;
                    return response;
                }
                base.errMsg.FunctionName = base.logMsg.FunctionName = "icar_SmallArea";
                string str3 = string.Empty;
                ArrayList list = new ArrayList(100);
                foreach (DataRow row in table.Rows)
                {
                    if (!base.CheckCar(CmdParam.ParamType.SimNum, row["telephone"].ToString(), ""))
                    {
                        base.logMsg.Msg = "simNum:" + row["telephone"].ToString() + "，错误信息:" + base.ErrorMsg;
                        base.log.WriteLog(base.logMsg);
                    }
                    else if ((base.carInfoList != null) && (base.carInfoList.Count > 0))
                    {
                        Bussiness.CarInfo info = base.carInfoList[0] as Bussiness.CarInfo;
                        str3 = "," + info.SimNum;
                        list.Add(info);
                    }
                }
                if (str3.Length <= 0)
                {
                    response.ErrorMsg = "没有符合的车辆!";
                    return response;
                }
                base.logMsg.Msg = "发送：类型-" + CmdParam.ParamType.SimNum.ToString() + ",车辆-" + str3.Substring(1) + ",指令-" + txtMsg_0.OrderCode.ToString();
                base.logMsg.Msg = base.logMsg.Msg + "," + str;
                base.log.WriteLog(base.logMsg);
                if (base.isStartCommon())
                {
                    foreach (Bussiness.CarInfo info2 in list)
                    {
                        try
                        {
                            int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                            base.SaveCmdParm(newOrderId.ToString() + "|" + info2.CarId.ToString() + ";");
                            string orderIDParam = response.OrderIDParam;
                            response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info2.CarId.ToString() + ";";
                            response.ResultCode = SendBase.CarCmdSend.icar_RemoteDial(base.WorkId, newOrderId, info2.SimNum, "6", "电召");
                            if (response.ResultCode != 0L)
                            {
                                base.alarmMsg.FunctionName = "icar_SmallArea";
                                base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info2.SimNum, ",OrderCode-", txtMsg_0.OrderCode });
                                base.alarmMsg.Code = response.ResultCode.ToString();
                                base.log.WriteAlarm(base.alarmMsg);
                            }
                            response.ResultCode = 0L;
                        }
                        catch (Exception exception)
                        {
                            base.errMsg.ErrorText = "下发消息指令时发生错误!";
                            response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                            base.log.WriteError(base.errMsg, exception);
                        }
                    }
                    return response;
                }
                response.ErrorMsg = base.ErrorMsg;
                return response;
            }
            response.ErrorMsg = "该范围内未找到车辆！";
            return response;
        }

        public Response icar_StopAlarmDeal(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, TrafficALarmHandle trafficALarmHandle_0, object object_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_StopAlarmDeal";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1 + ",指令-报警解除(交通部)";
            base.log.WriteLog(base.logMsg);
            response.ResultCode = 0L;
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.ErrorMsg;
                return response;
            }
            if (!base.isStartCommon())
            {
                response.ErrorMsg = base.ErrorMsg;
                return response;
            }
            foreach (Bussiness.CarInfo info in base.carInfoList)
            {
                try
                {
                    int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                    trafficALarmHandle_0.OrderID = newOrderId;
                    trafficALarmHandle_0.WorkID = base.WorkId;
                    trafficALarmHandle_0.CarId = info.CarId;
                    if (!string.IsNullOrEmpty(trafficALarmHandle_0.GpsTime))
                    {
                        Alarm.InsertAlarmResult(trafficALarmHandle_0);
                    }
                    if ((trafficALarmHandle_0.iProcMode == 1) && (info.ProtocolName != SendBase.m_ProtocolName))
                    {
                        SimpleCmd cmd = object_0 as SimpleCmd;
                        string paramDisc = cmd.GetParamDisc();
                        object pvArg = cmd.pvArg;
                        base.AddUpDataLog(newOrderId, info.CarNum, cmd.OrderCode.ToString(), paramDisc);
                        response.ResultCode = SendBase.CarCmdSend.icar_SimpleCmd(base.WorkId, newOrderId, info.SimNum, cmd.CmdCode, ref pvArg);
                    }
                    else
                    {
                        string conntent = "";
                        CmdParamBase base2 = object_0 as CmdParamBase;
                        if (base2 == null)
                        {
                            return response;
                        }
                        if (trafficALarmHandle_0.iProcMode == 4)
                        {
                            base2.TransformCode = CmdParam.TrafficProtocolCodeExchange((int) ((TxtMsg) base2).MsgType);
                        }
                        else
                        {
                            base2.TransformCode = CmdParam.TrafficProtocolCodeExchange2((int) base2.OrderCode);
                        }
                        string str3 = base2.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SimpleCmd", ref conntent);
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", base2.OrderCode.ToString(), "等待", "", this.method_6(trafficALarmHandle_0.iProcMode, info.SimNum.ToString()));
                        response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, base2.TransformCode, (int) commMode_0, str3);
                    }
                }
                catch (Exception exception)
                {
                    response.ErrorMsg = exception.Message;
                    response.ResultCode = -1L;
                    return response;
                }
            }
            return response;
        }

        public Response icar_StopCapture(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, StopCapture stopCapture_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetCaptureEx";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1;
            string str = string.Empty;
            base.log.WriteLog(base.logMsg);
            if (!base.CheckCar(paramType_0, string_1, string_2))
            {
                response.ErrorMsg = base.alarmMsg.AlarmText = base.ErrorMsg;
                base.log.WriteAlarm(base.alarmMsg);
                return response;
            }
            string strErrorMsg = string.Empty;
            if (stopCapture_0.CheckData(out strErrorMsg) != 0)
            {
                response.ErrorMsg = strErrorMsg;
                return response;
            }
            if (base.isStartCommon())
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", stopCapture_0.OrderCode.ToString(), "等待", "", str);
                        if (info.ProtocolName == SendBase.m_ProtocolName)
                        {
                            string conntent = "";
                            stopCapture_0.TransformCode = CmdParam.TrafficProtocolCodeExchange2((int) stopCapture_0.OrderCode);
                            string str4 = stopCapture_0.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SimpleCmd", ref conntent);
                            response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, stopCapture_0.TransformCode, (int) commMode_0, str4);
                        }
                        else
                        {
                            response.ResultCode = SendBase.CarCmdSend.icar_StopCapture(base.WorkId, newOrderId, info.SimNum, stopCapture_0.CamerasID, stopCapture_0.Flag1, stopCapture_0.Flag2);
                        }
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_StopCapture";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info.SimNum, ",OrderCode-", stopCapture_0.OrderCode });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
                        }
                        else
                        {
                            new Car().UpdateGisCarCommandTime(info.CarId);
                        }
                        response.ResultCode = 0L;
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText;
                        base.log.WriteError(base.errMsg, exception);
                    }
                }
                return response;
            }
            response.ErrorMsg = base.ErrorMsg;
            return response;
        }

        private CarPartInfo method_5(DataRow dataRow_0, out CmdParam.CommMode commMode_0)
        {
            CarPartInfo info = new CarPartInfo();
            ReceiveDataBase base2 = new ReceiveDataBase();
            int num = int.Parse(dataRow_0["carstatu"].ToString());
            info.AccStatus = base2.GetACCStatus(num);
            long num2 = 0L;
            if (dataRow_0.Table.Columns.Contains("CarStatuEx"))
            {
                num2 = long.Parse(dataRow_0["carStatuEx"].ToString());
            }
            CarAlarmType type = new CarAlarmType();
            string str = AlamStatus.GetStatusNameByCarStatu((long) num) + AlamStatus.GetStatusNameByCarStatuExt(num2) + type.GetCustAlarmName(dataRow_0["telephone"].ToString(), num);
            string str2 = "0";
            if (base2.isPosStatus(num))
            {
                str2 = dataRow_0["starNum"].ToString();
            }
            int num3 = int.Parse(dataRow_0["commflag"].ToString());
            commMode_0 = (CmdParam.CommMode) num3;
            string str3 = dataRow_0["speed"].ToString();
            info.Speed = str3.Substring(0, str3.IndexOf('.') + 3);
            info.TransportStatu = base2.GetTransportStatus(int.Parse(dataRow_0["TransportStatus"].ToString()));
            info.StatusName = str;
            info.GpsTime = Convert.ToDateTime(dataRow_0["gpstime"]).ToString("yyyy-MM-dd HH:mm:ss");
            info.ReceTime = Convert.ToDateTime(dataRow_0["ReceTime"]).ToString("yyyy-MM-dd HH:mm:ss");
            info.DistanceDiff = dataRow_0["DistanceDiff"].ToString();
            info.StarNum = str2;
            if (info.DistanceDiff.Length <= 3)
            {
                info.DistanceDiff = "0." + info.DistanceDiff;
            }
            else
            {
                info.DistanceDiff = info.DistanceDiff.Insert(info.DistanceDiff.Length - 3, ".");
                info.DistanceDiff = info.DistanceDiff.Substring(0, info.DistanceDiff.Length - 1);
            }
            info.Lat = dataRow_0["latitude"].ToString();
            info.Lon = dataRow_0["longitude"].ToString();
            info.Lat = info.Lat.Substring(0, info.Lat.IndexOf('.') + 7);
            info.Lon = info.Lon.Substring(0, info.Lon.IndexOf('.') + 7);
            string aCCStatus = base2.GetACCStatus(base2.GetDrInt(dataRow_0, "carstatu"));
            if ("关".Equals(aCCStatus))
            {
                info.AccOn = "0";
            }
            else
            {
                info.AccOn = "1";
            }
            if (base2.GetDrInt(dataRow_0, "TransportStatus") == 3)
            {
                info.IsFill = "1";
            }
            else
            {
                info.IsFill = "0";
            }
            if (base2.isPosStatus(base2.GetDrInt(dataRow_0, "carstatu")))
            {
                info.GpsValid = "1";
            }
            else
            {
                info.GpsValid = "0";
            }
            info.Direct = Convert.ToInt32(dataRow_0["Direct"]);
            return info;
        }

        private string method_6(int int_0, string string_1)
        {
            SendBase.CarCmdSend.GetNewOrderId();
            string str = "";
            if (int_0 == 1)
            {
                str = "报警解除";
            }
            else if (int_0 == 2)
            {
                str = "车辆监听";
            }
            else if (int_0 == 3)
            {
                str = "拍照";
            }
            else if (int_0 == 4)
            {
                str = "下发消息";
            }
            else if (int_0 == 5)
            {
                str = "不做处理";
            }
            else if (int_0 == 6)
            {
                str = "将来处理";
            }
            return ("处理方式:" + str);
        }
    }
}

