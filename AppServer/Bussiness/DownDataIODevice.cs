namespace Bussiness
{
    using ParamLibrary.Application;
    using ParamLibrary.CmdParamInfo;
    using ParamLibrary.Entity;
    using System;

    public class DownDataIODevice : SendBase
    {
        public DownDataIODevice(int int_0, bool bool_3, bool bool_4, bool bool_5)
        {
            base.errMsg.ClassName = base.alarmMsg.ClassName = base.logMsg.ClassName = "DownDataIODevice";
            base.WorkId = int_0;
            base.IsSudoOverDue = bool_4;
            base.IsMultiSend = bool_5;
            base.IsAllowNullPassWord = bool_3;
            base.userInfo = null;
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
    }
}

