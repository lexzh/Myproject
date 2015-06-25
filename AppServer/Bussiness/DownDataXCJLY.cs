namespace Bussiness
{
    using ParamLibrary.Application;
    using ParamLibrary.CmdParamInfo;
    using ParamLibrary.Entity;
    using System;
    using System.Threading;

    public class DownDataXCJLY : SendBase
    {
        public DownDataXCJLY(int int_0, bool bool_3, bool bool_4, bool bool_5, OnlineUserInfo onlineUserInfo_0)
        {
            base.errMsg.ClassName = base.alarmMsg.ClassName = base.logMsg.ClassName = "DownDataXCJLY";
            base.WorkId = int_0;
            base.IsSudoOverDue = bool_4;
            base.IsMultiSend = bool_5;
            base.IsAllowNullPassWord = bool_3;
            base.userInfo = onlineUserInfo_0;
        }

        public Response icar_SetCommonCmd(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, SimpleCmd simpleCmd_0)
        {
            Response response = new Response();
            string str = "";
            if ((simpleCmd_0.CmdParams != null) && (simpleCmd_0.CmdParams.Count > 0))
            {
                foreach (string[] strArray in simpleCmd_0.CmdParams)
                {
                    string str6 = str;
                    str = str6 + "命令码-" + simpleCmd_0.OrderCode.ToString() + ",参数-" + this.method_5(strArray, 0) + ";";
                }
            }
            base.WriteLog(paramType_0.ToString(), string_1 + "," + str);
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
            object obj2 = null;
            int newOrderId = -1;
            foreach (Bussiness.CarInfo info in base.carInfoList)
            {
                try
                {
                    newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                    int num2 = 0;
                    base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                    string orderIDParam = response.OrderIDParam;
                    response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                    if ((simpleCmd_0.CmdParams != null) && (simpleCmd_0.CmdParams.Count > 0))
                    {
                        if (info.ProtocolName == SendBase.m_ProtocolName)
                        {
                            string conntent = "";
                            string str5 = simpleCmd_0.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SimpleCmd", ref conntent);
                            base.AddUpDataLog(newOrderId, info.CarNum, simpleCmd_0.OrderCode.ToString(), conntent);
                            response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, (int) simpleCmd_0.OrderCode, (int) commMode_0, str5);
                        }
                        else
                        {
                            foreach (string[] strArray2 in simpleCmd_0.CmdParams)
                            {
                                base.AddUpDataLog(newOrderId, info.CarNum, simpleCmd_0.OrderCode.ToString(), this.method_5(strArray2, (int) simpleCmd_0.OrderCode));
                                obj2 = this.method_6(strArray2);
                                response.ResultCode = SendBase.CarCmdSend.icar_SetCommonCmd(base.WorkId, newOrderId, info.SimNum, simpleCmd_0.CmdCode, ref obj2, commMode_0);
                                if ((simpleCmd_0.CmdParams.Count - num2) > 1)
                                {
                                    Thread.Sleep(0x7d0);
                                }
                                num2++;
                                if (response.ResultCode != 0L)
                                {
                                    base.WriteError(base.WorkId.ToString(), info.SimNum, simpleCmd_0.OrderCode.ToString());
                                }
                            }
                        }
                    }
                    else
                    {
                        base.AddUpDataLog(newOrderId, info.CarNum, simpleCmd_0.OrderCode.ToString(), "");
                        if (info.ProtocolName == SendBase.m_ProtocolName)
                        {
                            string str2 = string.Empty;
                            string str3 = simpleCmd_0.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SimpleCmd", ref str2);
                            response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, (int) simpleCmd_0.OrderCode, (int) commMode_0, str3);
                        }
                        else
                        {
                            response.ResultCode = SendBase.CarCmdSend.icar_SetCommonCmd(base.WorkId, newOrderId, info.SimNum, simpleCmd_0.CmdCode, ref obj2, commMode_0);
                        }
                        if (response.ResultCode != 0L)
                        {
                            base.WriteError(base.WorkId.ToString(), info.SimNum, simpleCmd_0.OrderCode.ToString());
                        }
                        else if (simpleCmd_0.OrderCode == CmdParam.OrderCode.LBS位置查询)
                        {
                            base.userInfo.CarFilter.UpdatePosSearchFlag(simpleCmd_0.OrderCode.ToString(), newOrderId, info.CarId);
                        }
                    }
                }
                catch (Exception exception)
                {
                    response.ErrorMsg = "下发消息指令时发生错误,错误详细信息：" + exception.Message;
                    base.log.WriteError(base.errMsg, exception);
                }
            }
            return response;
        }

        private string method_5(string[] string_1, int int_0)
        {
            string str = "下发参数：";
            if ((string_1 != null) && (string_1.Length > 0))
            {
                int_0 = 0;
                str = "下发参数：";
                for (int i = 0; i <= (string_1.Length - 1); i++)
                {
                    str = str + string_1[i] + ";";
                }
            }
            return str.Trim(new char[] { ';' });
        }

        private object[] method_6(string[] string_1)
        {
            if ((string_1 == null) || (string_1.Length <= 0))
            {
                return null;
            }
            object[] objArray = new object[string_1.Length];
            for (int i = 0; i <= (string_1.Length - 1); i++)
            {
                objArray[i] = string_1[i];
            }
            return objArray;
        }
    }
}

