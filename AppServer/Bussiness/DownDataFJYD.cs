namespace Bussiness
{
    using ParamLibrary.Application;
    using ParamLibrary.CmdParamInfo;
    using ParamLibrary.Entity;
    using System;
    using System.Collections;
    using System.Data;
    using System.Text;
    using System.Threading;

    public class DownDataFJYD : SendBase
    {
        public DownDataFJYD(int int_0, bool bool_3, bool bool_4, bool bool_5, OnlineUserInfo onlineUserInfo_0)
        {
            base.errMsg.ClassName = base.alarmMsg.ClassName = base.logMsg.ClassName = "DownDataFJYD";
            base.WorkId = int_0;
            base.IsSudoOverDue = bool_4;
            base.IsMultiSend = bool_5;
            base.IsAllowNullPassWord = bool_3;
            base.userInfo = onlineUserInfo_0;
        }

        public Response Car_CommandParameterInsterToDB(Car myCar, CmdParam.ParamType paramType, string CarValues, string CarPw, SimpleCmd cmdParameter, string cmdContent, string desc)
        {
            Response response = new Response();
            string str = cmdParameter.OrderCode.ToString();
            if (!base.CheckCar(paramType, CarValues, CarPw))
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
                int num = -1;
                int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                int num3 = SendBase.CarCmdSend.CalOrderId(base.WorkId, newOrderId);
                try
                {
                    num = myCar.InsertCommandParameterToDB(info.SimNum, num3, (int) cmdParameter.CmdCode, cmdContent);
                }
                catch (Exception exception)
                {
                    base.userInfo.DownCommd.AddCarNewLogData(newOrderId, (cmdParameter.OrderCode == CmdParam.OrderCode.平台查岗应答) ? "" : info.CarNum, "发送", cmdParameter.OrderCode.ToString(), "失败", "", "错误信息：" + exception.Message);
                    continue;
                }
                if ((num > 0) && (cmdParameter.OrderCode != CmdParam.OrderCode.主动上报报警处理结果信息))
                {
                    base.userInfo.DownCommd.AddCarNewLogData(newOrderId, (cmdParameter.OrderCode == CmdParam.OrderCode.平台查岗应答) ? "" : info.CarNum, "发送", str, "成功", "", desc);
                    //将状态从等待修改为成功 huzh，2014.1.24
                }
                response.ResultCode = 0L;
            }
            return response;
        }

        public Response icar_SelMultiPathAlarm(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, PathAlarmList pathAlarmList_0)
        {
            Response response = new Response();
            base.WriteLog(paramType_0.ToString(), string_1);
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
            Car car = new Car();
            int num = -1;
            int newOrderId = -1;
            DataTable table = null;
            object obj2 = null;
            PathAlarm alarm = null;
            object[] objArray = new object[4];
            foreach (Bussiness.CarInfo info in base.carInfoList)
            {
                try
                {
                    int num3 = -1;
                    newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                    base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                    string orderIDParam = response.OrderIDParam;
                    response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                    for (int i = 0; i < pathAlarmList_0.Count; i++)
                    {
                        alarm = (PathAlarm) pathAlarmList_0[i];
                        table = car.GetNewPathId(info.CarId, alarm.PathName, num3);
                        if ((table == null) || (table.Rows.Count <= 0))
                        {
                            goto Label_037E;
                        }
                        num = int.Parse(table.Rows[0][0].ToString());
                        alarm.ID = num;
                        if (-1 == num)
                        {
                            goto Label_033E;
                        }
                        num3 = int.Parse(table.Rows[0][1].ToString());
                        objArray[0] = num.ToString();
                        objArray[1] = alarm.PointCount.ToString();
                        objArray[2] = this.method_7(alarm.Points);
                        objArray[3] = alarm.PathDif;
                        obj2 = objArray;
                        base.AddUpDataLog(newOrderId, info.CarNum, RespCodeParam.GetRespName(((int) pathAlarmList_0.OrderCode) + 0x80), "");
                        car.InsertPathIdsIntoPathParam(info.CarId, base.WorkId.ToString(), newOrderId.ToString(), alarm.ID.ToString(), num.ToString());
                        if (i == (pathAlarmList_0.Count - 1))
                        {
                            object pvRegions = pathAlarmList_0.pvRegions;
                            car.InsertPathIntoGisCar(info.CarId, base.WorkId, newOrderId, pathAlarmList_0.AlarmPathDot);
                        }
                        response.ResultCode = SendBase.CarCmdSend.icar_SetCommonCmd(base.WorkId, newOrderId, info.SimNum, (CmdParam.CmdCode) pathAlarmList_0.OrderCode, ref obj2, commMode_0);
                        if ((pathAlarmList_0.Count - i) > 1)
                        {
                            Thread.Sleep(300);
                        }
                        if (response.ResultCode != 0L)
                        {
                            base.WriteError(base.WorkId.ToString(), info.SimNum, pathAlarmList_0.OrderCode.ToString());
                        }
                        car.UpdatePathAlarm(info.CarId, alarm.PathName, 1, 0, 0, alarm.ID, "", "", alarm.PathFlag, alarm.DriEnough, alarm.DriNoEnough);
                    }
                    continue;
                Label_033E:
                    response.ErrorMsg = base.errMsg.ErrorText = string.Format("\"{0}\"偏移路线ID已满，请删除部分偏移路线!", info.CarNum);
                    base.log.WriteError(base.errMsg);
                    return response;
                Label_037E:
                    response.ErrorMsg = base.errMsg.ErrorText = string.Format("\"{0}\"获取偏移路线ID失败!", info.CarNum);
                    base.log.WriteError(base.errMsg);
                    return response;
                }
                catch (Exception exception)
                {
                    response.ErrorMsg = base.ErrorMsg = base.errMsg.ErrorText = "下发消息指令时发生错误!";
                    base.log.WriteError(base.errMsg, exception);
                }
            }
            return response;
        }

        public Response icar_SendRawPackage(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, SimpleCmd simpleCmd_0)
        {
            Response response = new Response();
            string str = "";
            foreach (string[] strArray in simpleCmd_0.CmdParams)
            {
                string str8 = str;
                str = str8 + "命令码-" + simpleCmd_0.OrderCode.ToString() + ",参数-" + this.method_5(strArray, 0) + ";";
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
            if ((simpleCmd_0.CmdParams != null) && (simpleCmd_0.CmdParams.Count > 0))
            {
                int newOrderId = -1;
                object objectArg = null;
                byte[] bytes = null;
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        foreach (string[] strArray2 in simpleCmd_0.CmdParams)
                        {
                            newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                            base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                            string orderIDParam = response.OrderIDParam;
                            response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                            base.AddUpDataLog(newOrderId, info.CarNum, simpleCmd_0.OrderCode.ToString(), this.method_5(strArray2, (int) simpleCmd_0.OrderCode));
                            if (simpleCmd_0.OrderCode == CmdParam.OrderCode.回拔坐席电话指令)
                            {
                                bytes = Encoding.GetEncoding("gb2312").GetBytes(strArray2[0]);
                                byte[] array = new byte[15];
                                bytes.CopyTo(array, 0);
                                objectArg = array;
                            }
                            if (simpleCmd_0.OrderCode == CmdParam.OrderCode.设置油耗仪参数)
                            {
                                objectArg = new Oix().GetObjectArg(strArray2);
                            }
                            if (simpleCmd_0.OrderCode == CmdParam.OrderCode.设置出入口分段超速报警)
                            {
                                Car car = new Car();
                                string[] strArray3 = simpleCmd_0.CmdParams[0] as string[];
                                Hashtable hashtable = new Hashtable();
                                for (int i = 0; i < (strArray3.Length / 8); i++)
                                {
                                    if (!hashtable.Contains(strArray3[i * 8]))
                                    {
                                        car.Car_AddPassWayPathIdToTmp(info.CarId, base.WorkId, newOrderId, strArray3[i * 8], strArray3[(i * 8) + 1], (Convert.ToDouble(strArray3[(i * 8) + 6]) > 0.0) ? ((int) Convert.ToDouble(strArray3[(i * 8) + 6])) : ((int) Convert.ToDouble(strArray3[(i * 8) + 7])));
                                        hashtable[strArray3[i * 8]] = null;
                                    }
                                }
                                objectArg = simpleCmd_0.pvArg;
                            }
                            if (info.ProtocolName == SendBase.m_ProtocolName)
                            {
                                if (simpleCmd_0.OrderCode == CmdParam.OrderCode.设置关键区域)
                                {
                                    TrafficRawPackage package = new TrafficRawPackage {
                                        OrderCode = CmdParam.OrderCode.命令透传,
                                        SubOrderCode = simpleCmd_0.OrderCode
                                    };
                                    Car car2 = new Car();
                                    StringBuilder builder = new StringBuilder();
                                    builder.Append((strArray2.Length / 8).ToString("X4"));
                                    for (int j = 0; j < strArray2.Length; j += 8)
                                    {
                                        builder.Append(Convert.ToInt32(strArray2[j]).ToString("X8"));
                                        builder.Append(Convert.ToByte(strArray2[j + 1]).ToString("X2"));
                                        builder.Append(((int) (Convert.ToDouble(strArray2[j + 2]) * 1000000.0)).ToString("X8"));
                                        builder.Append(((int) (Convert.ToDouble(strArray2[j + 3]) * 1000000.0)).ToString("X8"));
                                        if (string.IsNullOrEmpty(strArray2[j + 5]))
                                        {
                                            builder.Append(Convert.ToInt32(strArray2[j + 4]).ToString("X8"));
                                            builder.Append("00000000");
                                        }
                                        else
                                        {
                                            builder.Append(((int) (Convert.ToDouble(strArray2[j + 4]) * 1000000.0)).ToString("X8"));
                                            builder.Append(((int) (Convert.ToDouble(strArray2[j + 5]) * 1000000.0)).ToString("X8"));
                                        }
                                        builder.Append(Convert.ToByte(strArray2[j + 6]).ToString("X2"));
                                        builder.Append(Convert.ToInt32(strArray2[j + 7]).ToString("X8"));
                                        if (car2.SetCriticalRegionToTmp(info.CarId, base.WorkId, newOrderId, strArray2[j], strArray2[j + 6], strArray2[j + 7]) != 0)
                                        {
                                            base.WriteError(base.WorkId.ToString(), info.SimNum, simpleCmd_0.OrderCode.ToString() + "-参数保存失败-" + strArray2[j] + "," + strArray2[j + 6] + "," + strArray2[j + 7]);
                                        }
                                    }
                                    package.strText = builder.ToString();
                                    string conntent = "";
                                    string str3 = package.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SendRawPackage", ref conntent);
                                    response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, (int) package.OrderCode, (int) commMode_0, str3);
                                }
                                else if (simpleCmd_0.OrderCode == CmdParam.OrderCode.设置出入口分段超速报警)
                                {
                                    TrafficRawPackage package2 = new TrafficRawPackage {
                                        OrderCode = CmdParam.OrderCode.命令透传,
                                        SubOrderCode = simpleCmd_0.OrderCode
                                    };
                                    StringBuilder builder2 = new StringBuilder();
                                    byte[] pvArg = (byte[]) simpleCmd_0.pvArg;
                                    if (pvArg != null)
                                    {
                                        for (int k = 0; k < pvArg.Length; k++)
                                        {
                                            if (((k + 1) % 0x16) == 1)
                                            {
                                                byte[] buffer4 = new byte[4];
                                                for (int m = 0; m < 4; m++)
                                                {
                                                    buffer4[m] = pvArg[k + m];
                                                }
                                                k += 3;
                                                builder2.Append(BitConverter.ToInt32(buffer4, 0).ToString("X8"));
                                            }
                                            else
                                            {
                                                builder2.Append(pvArg[k].ToString("X2"));
                                            }
                                        }
                                    }
                                    package2.strText = builder2.ToString();
                                    string str4 = "";
                                    string str5 = package2.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SendRawPackage", ref str4);
                                    response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, (int) package2.OrderCode, (int) commMode_0, str5);
                                }
                                else
                                {
                                    string str6 = "";
                                    string str7 = simpleCmd_0.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SimpleCmd", ref str6);
                                    response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, (int) simpleCmd_0.OrderCode, (int) commMode_0, str7);
                                }
                            }
                            else
                            {
                                response.ResultCode = SendBase.CarCmdSend.icar_SendRawPackage(base.WorkId, newOrderId, info.SimNum, simpleCmd_0.CmdCode, ref objectArg, commMode_0);
                            }
                            if (response.ResultCode != 0L)
                            {
                                base.WriteError(base.WorkId.ToString(), info.SimNum, simpleCmd_0.OrderCode.ToString());
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
            response.ResultCode = -1L;
            response.ErrorMsg = "下发参数为空!";
            return response;
        }

        public Response icar_SendRawPackage(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, TrafficRawPackage trafficRawPackage_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SendRawPackage";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1 + ",指令-" + trafficRawPackage_0.OrderCode.ToString() + ",参数-" + trafficRawPackage_0.strText;
            string msg = base.logMsg.Msg;
            base.log.WriteLog(base.logMsg);
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
            new Car();
            foreach (Bussiness.CarInfo info in base.carInfoList)
            {
                try
                {
                    int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                    if (info.ProtocolName == SendBase.m_ProtocolName)
                    {
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", trafficRawPackage_0.SubOrderCode.ToString(), "等待", "", msg);
                        string conntent = "";
                        string str3 = trafficRawPackage_0.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SendRawPackage", ref conntent);
                        response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, (int) trafficRawPackage_0.OrderCode, (int) commMode_0, str3);
                        if ((response.ResultCode == 0L) && (trafficRawPackage_0.SubOrderCode == CmdParam.OrderCode.获得当前车台温度))
                        {
                            base.userInfo.CarFilter.UpdatePosSearchFlag(trafficRawPackage_0.SubOrderCode.ToString(), newOrderId, info.CarId);
                        }
                    }
                    else if (trafficRawPackage_0.SubOrderCode == CmdParam.OrderCode.设置禁驾报警)
                    {
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", trafficRawPackage_0.SubOrderCode.ToString(), "等待", "", msg);
                        object pvArg = trafficRawPackage_0.pvArg;
                        response.ResultCode = SendBase.CarCmdSend.icar_SendRawPackage(base.WorkId, newOrderId, info.SimNum, trafficRawPackage_0.CmdCode, ref pvArg, commMode_0);
                    }
                    else if (trafficRawPackage_0.SubOrderCode == CmdParam.OrderCode.获得当前车台温度)
                    {
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", trafficRawPackage_0.SubOrderCode.ToString(), "等待", "", msg);
                        object obj3 = 0;
                        response.ResultCode = SendBase.CarCmdSend.icar_SimpleCmd(base.WorkId, newOrderId, info.SimNum, trafficRawPackage_0.CmdCode, ref obj3);
                    }
                    else if (trafficRawPackage_0.SubOrderCode == CmdParam.OrderCode.设置温度报警)
                    {
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", trafficRawPackage_0.SubOrderCode.ToString(), "等待", "", msg);
                        object obj4 = trafficRawPackage_0.pvArg;
                        response.ResultCode = SendBase.CarCmdSend.icar_SimpleCmd(base.WorkId, newOrderId, info.SimNum, trafficRawPackage_0.CmdCode, ref obj4);
                    }
                    else
                    {
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", trafficRawPackage_0.SubOrderCode.ToString(), "失败", "", "错误描述：该车辆终端不支持此操作。");
                        response.ErrorMsg = "终端不支持该协议";
                    }
                }
                catch (Exception exception)
                {
                    response.ErrorMsg = exception.Message;
                    return response;
                }
            }
            return response;
        }

        public Response icar_SetCommonCmd(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, SimpleCmd simpleCmd_0)
        {
            Response response = new Response();
            string str = "";
            foreach (string[] strArray in simpleCmd_0.CmdParams)
            {
                string str2 = str;
                str = str2 + "命令码-" + simpleCmd_0.OrderCode.ToString() + ",参数-" + this.method_5(strArray, 0) + ";";
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
            if ((simpleCmd_0.CmdParams != null) && (simpleCmd_0.CmdParams.Count > 0))
            {
                object obj2 = null;
                int newOrderId = -1;
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    try
                    {
                        int num2 = 0;
                        newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        foreach (string[] strArray2 in simpleCmd_0.CmdParams)
                        {
                            base.AddUpDataLog(newOrderId, info.CarNum, simpleCmd_0.OrderCode.ToString(), this.method_5(strArray2, (int) simpleCmd_0.OrderCode));
                            obj2 = this.method_6(strArray2);
                            switch (simpleCmd_0.CmdCode)
                            {
                                case CmdParam.CmdCode.取消区域报警:
                                    new Car().InsertRegionIdsIntoGisCar(info.CarId, base.WorkId.ToString(), newOrderId.ToString(), strArray2[0]);
                                    break;

                                case CmdParam.CmdCode.取消路线报警:
                                    new Car().InsertPathIdsIntoGisCar(info.CarId, base.WorkId.ToString(), newOrderId.ToString(), strArray2[0]);
                                    break;
                            }
                            response.ResultCode = SendBase.CarCmdSend.icar_SetCommonCmd(base.WorkId, newOrderId, info.SimNum, simpleCmd_0.CmdCode, ref obj2, commMode_0);
                            if ((simpleCmd_0.CmdParams.Count - num2) > 1)
                            {
                                Thread.Sleep(500);
                            }
                            num2++;
                            if (response.ResultCode != 0L)
                            {
                                base.WriteError(base.WorkId.ToString(), info.SimNum, simpleCmd_0.OrderCode.ToString());
                            }
                            else if (simpleCmd_0.OrderCode == CmdParam.OrderCode.实时点名查询)
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
            response.ResultCode = -1L;
            response.ErrorMsg = "下发参数为空!";
            return response;
        }

        public Response icar_SetCommonCmdTraffic(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, TrafficSimpleCmd trafficSimpleCmd_0)
        {
            Response response = new Response();
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
            int newOrderId = -1;
            foreach (Bussiness.CarInfo info in base.carInfoList)
            {
                try
                {
                    newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                    if (!string.IsNullOrEmpty(trafficSimpleCmd_0.m_Params))
                    {
                        base.m_Params1 = trafficSimpleCmd_0.m_Params;
                    }
                    if (!string.IsNullOrEmpty(trafficSimpleCmd_0.m_ParamsReport))
                    {
                        base.m_Params2 = trafficSimpleCmd_0.m_ParamsReport;
                    }
                    base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                    string orderIDParam = response.OrderIDParam;
                    response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                    if (info.ProtocolName == SendBase.m_ProtocolName)
                    {
                        string conntent = "";
                        string str2 = trafficSimpleCmd_0.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SimpleCmd", ref conntent);
                        base.WriteLog(paramType_0.ToString(), string_1 + "," + conntent);
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", trafficSimpleCmd_0.OrderCode.ToString(), "等待", "", conntent);
                        response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, (int) trafficSimpleCmd_0.OrderCode, (int) commMode_0, str2);
                    }
                    else
                    {
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", trafficSimpleCmd_0.OrderCode.ToString(), "失败", "", "终端不支持该命令");
                        response.ErrorMsg = "终端不支持该命令";
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

        public Response icar_SetPhoneNumText(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, TrafficPhoneNumText trafficPhoneNumText_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetPhoneNumText";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1 + ",指令-" + trafficPhoneNumText_0.OrderCode.ToString();
            string msg = base.logMsg.Msg;
            base.log.WriteLog(base.logMsg);
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
            new Car();
            foreach (Bussiness.CarInfo info in base.carInfoList)
            {
                try
                {
                    int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                    if (info.ProtocolName == SendBase.m_ProtocolName)
                    {
                        base.m_Params1 = trafficPhoneNumText_0.m_Params;
                        base.m_Params2 = trafficPhoneNumText_0.m_ParamsReport;
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", trafficPhoneNumText_0.OrderCode.ToString(), "等待", "", msg);
                        string conntent = "";
                        string str3 = trafficPhoneNumText_0.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "icar_SetCommonCmd", ref conntent);
                        response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, (int) trafficPhoneNumText_0.OrderCode, (int) commMode_0, str3);
                    }
                    else
                    {
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", trafficPhoneNumText_0.OrderCode.ToString(), "失败", "", "错误描述：该车辆终端不支持此操作。");
                        response.ErrorMsg = "终端不支持该协议";
                    }
                }
                catch (Exception exception)
                {
                    response.ErrorMsg = exception.Message;
                    return response;
                }
            }
            return response;
        }

        public Response icar_SetPlatformAlarmCmd(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, TrafficSimpleCmd trafficSimpleCmd_0)
        {
            Response response = new Response();
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
            int newOrderId = -1;
            foreach (Bussiness.CarInfo info in base.carInfoList)
            {
                try
                {
                    newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                    base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                    string orderIDParam = response.OrderIDParam;
                    response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                    string str = "平台报警";
                    base.WriteLog(paramType_0.ToString(), string_1 + "," + str);
                    base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", trafficSimpleCmd_0.OrderCode.ToString(), "等待", "GPRS/CDMA", str);
                    if (trafficSimpleCmd_0.OrderCode == CmdParam.OrderCode.设置偏移路线报警)
                    {
                        if (new Car().SetCarPathAlarm_Platform(info.CarId, trafficSimpleCmd_0.CommonArgs as DataTable) != 0)
                        {
                            response.ResultCode = -1L;
                            response.ErrorMsg = "参数保存失败";
                        }
                        else
                        {
                            response.ResultCode = 0L;
                        }
                    }
                    else if (trafficSimpleCmd_0.OrderCode == CmdParam.OrderCode.设置区域报警)
                    {
                        response.ResultCode = new Car().SetCarRegionAlarm_Platform(info.CarId, trafficSimpleCmd_0.CommonArgs as DataTable);
                    }
                    else if (trafficSimpleCmd_0.OrderCode == CmdParam.OrderCode.设置分路段超速报警)
                    {
                        response.ResultCode = new Car().SetCarPathSegmentAlarm_Platform(info.CarId, trafficSimpleCmd_0.CommonArgs);
                    }
                    if (response.ResultCode == 0L)
                    {
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", trafficSimpleCmd_0.OrderCode.ToString(), "成功", "GPRS/CDMA", str);
                    }
                    else
                    {
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", trafficSimpleCmd_0.OrderCode.ToString(), "失败", "GPRS/CDMA", str);
                    }
                }
                catch (Exception exception)
                {
                    response.ErrorMsg = "下发消息指令时发生错误,错误详细信息：" + exception.Message;
                    base.log.WriteError(base.errMsg, exception);
                    base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", trafficSimpleCmd_0.OrderCode.ToString(), "失败", "GPRS/CDMA", "");
                }
            }
            return response;
        }

        public Response icar_SetPosReportConditions(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, TrafficPosReport trafficPosReport_0)
        {
            Response response = new Response();
            base.logMsg.FunctionName = "icar_SetPosReportConditions";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1 + ",指令-" + trafficPosReport_0.OrderCode.ToString();
            string conntent = "";
            base.log.WriteLog(base.logMsg);
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
                    if (info.ProtocolName == SendBase.m_ProtocolName)
                    {
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        string str2 = trafficPosReport_0.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SetPosReport", ref conntent);
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", trafficPosReport_0.OrderCode.ToString(), "等待", "", conntent);
                        response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, (int) trafficPosReport_0.OrderCode, (int) commMode_0, str2);
                    }
                    else
                    {
                        response.ErrorMsg = "终端不支持该协议";
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", trafficPosReport_0.OrderCode.ToString(), "失败", "", "错误描述：该车辆终端不支持此操作。");
                    }
                }
                catch (Exception exception)
                {
                    response.ErrorMsg = exception.Message;
                    return response;
                }
            }
            return response;
        }

        public Response icar_SetRegionAlarm(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, RegionAlarmList regionAlarmList_0)
        {
            Response response = new Response();
            base.WriteLog(paramType_0.ToString(), string_1);
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
            Car car = new Car();
            object obj2 = null;
            RegionAlarm alarm = null;
            int newOrderId = -1;
            try
            {
                foreach (Bussiness.CarInfo info in base.carInfoList)
                {
                    newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                    obj2 = this.method_8(info.CarId, regionAlarmList_0, car);
                    base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                    string orderIDParam = response.OrderIDParam;
                    response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                    base.AddUpDataLog(newOrderId, info.CarNum, CmdParam.CmdCode.区域报警设置.ToString(), "");
                    car.InsertRegionIntoGisCar(info.CarId, base.WorkId, newOrderId, regionAlarmList_0.AlarmRegionDot, regionAlarmList_0.RegionFeature.ToString());
                    for (int i = 0; i < regionAlarmList_0.Count; i++)
                    {
                        alarm = (RegionAlarm) regionAlarmList_0[i];
                        car.UpdateRegionParam(base.WorkId, newOrderId, int.Parse(info.CarId), alarm.RegionID, alarm.param, alarm.toEndTime, alarm.toBackTime, alarm.RegionType & 15, alarm.BeginTime, alarm.EndTime, regionAlarmList_0.RegionFeature, alarm.AlarmCondition.ToString(), alarm.PlanUpTime, alarm.PlanDownTime, alarm.newRegionId, alarm.AlarmFlag, alarm.MaxSpeed, alarm.HodeTime);
                    }
                    response.ResultCode = SendBase.CarCmdSend.icar_SetCommonCmd(base.WorkId, newOrderId, info.SimNum, CmdParam.CmdCode.区域报警设置, ref obj2, commMode_0);
                    if (response.ResultCode != 0L)
                    {
                        base.WriteError(base.WorkId.ToString(), info.SimNum, regionAlarmList_0.OrderCode.ToString());
                    }
                }
                response.ResultCode = 0L;
            }
            catch (Exception exception)
            {
                response.ErrorMsg = "下发消息指令时发生错误,错误详细信息：" + exception.Message;
                base.log.WriteError(base.errMsg, exception);
            }
            return response;
        }

        public Response icar_SetTextMsg(CmdParam.ParamType paramType_0, string string_1, string string_2, CmdParam.CommMode commMode_0, TxtMsg txtMsg_0, string string_3, string string_4)
        {
            Response response = new Response();
            if (string.IsNullOrEmpty(string_1))
            {
                DataTable table = base.GetCarInfoByArea(txtMsg_0.LLon, txtMsg_0.LLat, txtMsg_0.RLon, txtMsg_0.RLat, string_3, string_4);
                if ((table != null) && (table.Rows.Count > 0))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        string_1 = string_1 + row["telephone"].ToString() + ",";
                    }
                    string_1 = string_1.Trim(new char[] { ',' });
                }
                if (string.IsNullOrEmpty(string_1))
                {
                    response.ErrorMsg = "没有存在下发的车辆！";
                    return response;
                }
            }
            base.logMsg.FunctionName = "icar_SetTextMsg";
            base.logMsg.Msg = "发送：类型-" + paramType_0.ToString() + ",车辆-" + string_1 + ",指令-" + txtMsg_0.OrderCode.ToString();
            string msg = base.logMsg.Msg;
            base.log.WriteLog(base.logMsg);
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
            new Car();
            foreach (Bussiness.CarInfo info in base.carInfoList)
            {
                try
                {
                    int newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                    if (info.ProtocolName == SendBase.m_ProtocolName)
                    {
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", txtMsg_0.OrderCode.ToString(), "等待", "", msg);
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        string conntent = "";
                        string str3 = txtMsg_0.ToXmlString(base.CalOrderId(base.WorkId, newOrderId), info.SimNum, SendBase.m_ProtocolName, (int) commMode_0, "SetTextMsg", ref conntent);
                        response.ResultCode = SendBase.CarCmdSend.icar_SendCmdXML(base.WorkId, newOrderId, info.SimNum, SendBase.m_ProtocolName, (int) txtMsg_0.OrderCode, (int) commMode_0, str3);
                    }
                    else if ((txtMsg_0.OrderCode == CmdParam.OrderCode.电召指令) && (txtMsg_0.MsgType == CmdParam.MsgType.电召信息))
                    {
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", txtMsg_0.OrderCode.ToString(), "等待", "", msg);
                        string str4 = base.WorkId.ToString() + SendBase.CarCmdSend.GetNewOrderId().ToString();
                        response.SvcContext = str4;
                        string str6 = response.OrderIDParam;
                        response.OrderIDParam = str6 + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        object obj2 = this.method_9(info.SimNum, txtMsg_0.TelNumber, txtMsg_0.strMsg, str4, txtMsg_0.Way, txtMsg_0.LLon + "," + txtMsg_0.LLat + "," + txtMsg_0.RLon + "," + txtMsg_0.RLat);
                        response.ResultCode = SendBase.CarCmdSend.icar_SetCommonCmd(base.WorkId, newOrderId, info.SimNum, CmdParam.CmdCode.电召指令, ref obj2, commMode_0);
                    }
                    else if (txtMsg_0.OrderCode == CmdParam.OrderCode.拨打电话号码)
                    {
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", txtMsg_0.OrderCode.ToString(), "等待", "", msg);
                        string str7 = response.OrderIDParam;
                        response.OrderIDParam = str7 + newOrderId.ToString() + "|" + info.CarId.ToString() + ";";
                        SimpleCmd cmd = new SimpleCmd {
                            OrderCode = CmdParam.OrderCode.抢答确认指令
                        };
                        new ArrayList();
                        string[] strArray = new string[] { txtMsg_0.sPhone, txtMsg_0.TelNumber, txtMsg_0.strMsg, txtMsg_0.Orderid, txtMsg_0.sCarName };
                        object obj3 = this.method_6(strArray);
                        response.ResultCode = SendBase.CarCmdSend.icar_SetCommonCmd(base.WorkId, newOrderId, info.SimNum, cmd.CmdCode, ref obj3, commMode_0);
                    }
                    else
                    {
                        base.userInfo.DownCommd.AddCarNewLogData(newOrderId, info.CarNum, "发送", txtMsg_0.OrderCode.ToString(), "失败", "", "错误描述：该车辆终端不支持此操作。");
                        response.ErrorMsg = "终端不支持该协议";
                    }
                }
                catch (Exception exception)
                {
                    response.ErrorMsg = exception.Message;
                    return response;
                }
            }
            return response;
        }

        public Response icar_SmallArea(string string_1, string string_2, string string_3, string string_4, string string_5, string string_6, ArrayList arrayList_0, TxtMsg txtMsg_0, CmdParam.CommMode commMode_0)
        {
            Response response = new Response();
            if ((arrayList_0 != null) && (arrayList_0.Count > 0))
            {
                string strErrorMsg = string.Empty;
                if (txtMsg_0.CheckData(out strErrorMsg) != 0)
                {
                    response.ErrorMsg = strErrorMsg;
                    return response;
                }
                base.errMsg.FunctionName = base.logMsg.FunctionName = "icar_SmallArea";
                string str2 = string.Empty;
                ArrayList list = new ArrayList(100);
                foreach (string str3 in arrayList_0)
                {
                    if (!base.CheckCar(CmdParam.ParamType.SimNum, str3, ""))
                    {
                        base.logMsg.Msg = "simNum:" + str3 + "，错误信息:" + base.ErrorMsg;
                        base.log.WriteLog(base.logMsg);
                    }
                    else if ((base.carInfoList != null) && (base.carInfoList.Count > 0))
                    {
                        Bussiness.CarInfo info = base.carInfoList[0] as Bussiness.CarInfo;
                        str2 = "," + info.SimNum;
                        list.Add(info);
                    }
                }
                if (str2.Length <= 0)
                {
                    response.ErrorMsg = "没有符合的车辆!";
                    return response;
                }
                string str4 = "信息-" + txtMsg_0.strMsg + ",信息类型-" + txtMsg_0.MsgType.ToString();
                base.logMsg.Msg = "发送：类型-" + CmdParam.ParamType.SimNum.ToString() + ",车辆-" + str2.Substring(1) + ",指令-" + txtMsg_0.OrderCode.ToString();
                base.logMsg.Msg = base.logMsg.Msg + "," + str4;
                base.log.WriteLog(base.logMsg);
                if (!base.isStartCommon())
                {
                    response.ErrorMsg = base.ErrorMsg;
                    return response;
                }
                int newOrderId = -1;
                object obj2 = null;
                string str5 = base.WorkId.ToString() + SendBase.CarCmdSend.GetNewOrderId().ToString();
                response.SvcContext = str5;
                foreach (Bussiness.CarInfo info2 in list)
                {
                    try
                    {
                        newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info2.CarId.ToString() + ";");
                        string orderIDParam = response.OrderIDParam;
                        response.OrderIDParam = orderIDParam + newOrderId.ToString() + "|" + info2.CarId.ToString() + ";";
                        obj2 = this.method_9(info2.SimNum, string_6, txtMsg_0.strMsg, str5, string_5, string_1 + "," + string_2 + "," + string_3 + "," + string_4);
                        response.ResultCode = SendBase.CarCmdSend.icar_SetCommonCmd(base.WorkId, newOrderId, info2.SimNum, CmdParam.CmdCode.电召指令, ref obj2, commMode_0);
                        if (response.ResultCode != 0L)
                        {
                            base.alarmMsg.FunctionName = "icar_SmallArea_FJYD";
                            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", info2.SimNum, ",OrderCode-", CmdParam.OrderCode.电召指令.ToString() });
                            base.alarmMsg.Code = response.ResultCode.ToString();
                            base.log.WriteAlarm(base.alarmMsg);
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
            response.ErrorMsg = "没有存在下发的车辆！";
            return response;
        }

        public Response icar_SmallArea(string string_1, string string_2, string string_3, string string_4, string string_5, string string_6, TxtMsg txtMsg_0, string string_7, string string_8, CmdParam.CommMode commMode_0)
        {
            DataTable table = base.GetCarInfoByArea(string_1, string_2, string_3, string_4, string_7, string_8);
            ArrayList list = new ArrayList(100);
            if ((table != null) && (table.Rows.Count > 0))
            {
                foreach (DataRow row in table.Rows)
                {
                    list.Add(row["telephone"] as string);
                }
            }
            return this.icar_SmallArea(string_1, string_2, string_3, string_4, string_5, string_6, list, txtMsg_0, commMode_0);
        }

        private string method_5(string[] string_1, int int_0)
        {
            string str = "下发参数：";
            int_0 = 0;
            str = "下发参数：";
            for (int i = 0; i <= (string_1.Length - 1); i++)
            {
                str = str + string_1[i] + ";";
            }
            return str.Trim(new char[] { ';' });
        }

        private object[] method_6(string[] string_1)
        {
            object[] objArray = new object[string_1.Length];
            for (int i = 0; i <= (string_1.Length - 1); i++)
            {
                objArray[i] = string_1[i];
            }
            return objArray;
        }

        private string method_7(ArrayList arrayList_0)
        {
            string str = string.Empty;
            if ((arrayList_0 == null) || (arrayList_0.Count <= 0))
            {
                return str;
            }
            foreach (Point point in arrayList_0)
            {
                string str2 = str;
                str = str2 + "," + point.Longitude.ToString() + "," + point.Latitude.ToString();
            }
            return str.Trim(new char[] { ',' });
        }

        private object method_8(string string_1, RegionAlarmList regionAlarmList_0, Car car_0)
        {
            int index = 1;
            string[] strArray = null;
            RegionAlarm alarm = null;
            DataTable table = null;
            string str = "0";
            int key = -1;
            object[] objArray = new object[(regionAlarmList_0.Count * 4) + 1];
            objArray[0] = regionAlarmList_0.Count.ToString();
            Hashtable hashtable = new Hashtable(0x40);
            for (int i = 0; i < regionAlarmList_0.Count; i++)
            {
                alarm = (RegionAlarm) regionAlarmList_0[i];
                try
                {
                    int num4 = 0;
                    goto Label_00A7;
                Label_0065:
                    num4++;
                    if ((num4 < 5) && hashtable.ContainsKey(key))
                    {
                        goto Label_00A7;
                    }
                    goto Label_00C4;
                Label_0082:
                    str = table.Rows[0][0].ToString();
                    key = Convert.ToInt32(str);
                    goto Label_0065;
                Label_00A7:
                    table = car_0.GetNewRegionId(string_1, alarm.PathName, key);
                    if (base.IsNullDataTable(table))
                    {
                        goto Label_0065;
                    }
                    goto Label_0082;
                Label_00C4:
                    hashtable.Add(key, key);
                }
                catch
                {
                }
                alarm.newRegionId = (alarm.newRegionId == 0) ? 0 : int.Parse(str);
                strArray = alarm.AlarmRegionDot.Split(new char[] { '\\' });
                if (strArray.Length == 4)
                {
                    objArray[index + 1] = "1";
                    objArray[index + 2] = strArray[1] + "," + strArray[2] + "," + strArray[3];
                    objArray[index + 3] = strArray[0];
                }
                else if (strArray.Length == 5)
                {
                    objArray[index + 1] = "2";
                    objArray[index + 2] = strArray[1] + "," + strArray[2] + "," + strArray[3] + "," + strArray[4];
                    objArray[index + 3] = strArray[0];
                }
                else if (strArray.Length > 5)
                {
                    string str2 = string.Empty;
                    for (int j = 1; j <= (strArray.Length - 2); j += 2)
                    {
                        string str3 = str2;
                        str2 = str3 + strArray[j] + "," + strArray[j + 1] + ",";
                    }
                    objArray[index + 1] = "3";
                    objArray[index + 2] = str2.Substring(0, str2.Length - 1);
                    objArray[index + 3] = strArray[0];
                }
                if (alarm.newRegionId == 0)
                {
                    objArray[index] = "0";
                }
                else
                {
                    objArray[index] = str;
                }
                index += 4;
                object alarmRegionDot = regionAlarmList_0.AlarmRegionDot;
                regionAlarmList_0.AlarmRegionDot = string.Concat(new object[] { alarmRegionDot, alarm.newRegionId, @"\", alarm.AlarmRegionDot, "*" });
            }
            return objArray;
        }

        private object[] method_9(string string_1, string string_2, string string_3, string string_4, string string_5, string string_6)
        {
            return new object[] { string_1, string_2, string_3, string_4, string_5, string_6 };
        }
    }
}

