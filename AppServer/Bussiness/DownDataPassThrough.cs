namespace Bussiness
{
    using ParamLibrary.Application;
    using System;
    using System.Text;
    using ParamLibrary.Bussiness;

    public class DownDataPassThrough : SendBase
    {
        private const string _Send = "发送";
        private const string _Split = "|";
        private const string _Wait = "等待";

        public DownDataPassThrough(int int_0, bool bool_3, bool bool_4, bool bool_5, OnlineUserInfo onlineUserInfo_0)
        {
            base.errMsg.ClassName = base.alarmMsg.ClassName = base.logMsg.ClassName = "DownDataPassThrough";
            base.WorkId = int_0;
            base.IsSudoOverDue = bool_4;
            base.IsMultiSend = bool_5;
            base.IsAllowNullPassWord = bool_3;
            base.userInfo = onlineUserInfo_0;
        }

        public int GetNewOrderId()
        {
            return SendBase.CarCmdSend.GetNewOrderId();
        }

        public AppRespone icar_SendIOCommand(AppRequest appRequest_0)
        {
            AppRespone respone = new AppRespone();
            base.logMsg.FunctionName = "icar_SendIOCommand";
            this.method_5(appRequest_0);
            if (this.method_7(appRequest_0, respone))
            {
                int newOrderId = -1;
                long num2 = -1L;
                object obj2 = this.method_12(appRequest_0, respone);
                foreach (CarInfo info in base.carInfoList)
                {
                    newOrderId = SendBase.CarCmdSend.GetNewOrderId();
                    try
                    {
                        base.SaveCmdParm(newOrderId.ToString() + "|" + info.CarId.ToString() + ";");
                        this.method_10(newOrderId, info.CarNum, appRequest_0);
                        num2 = SendBase.CarCmdSend.icar_SendIOCommand(base.WorkId, newOrderId, info.SimNum, CmdParam.IodeviceType.南京通用计价器, ref obj2);
                        if (num2 != 0L)
                        {
                            this.method_6(base.logMsg.FunctionName, info.SimNum, num2, (int) appRequest_0.OrderCode);
                        }
                    }
                    catch (Exception exception)
                    {
                        base.errMsg.ErrorText = "下发消息指令时发生错误!";
                        base.log.WriteError(base.errMsg, exception);
                        this.method_8(respone, base.errMsg.ErrorText);
                    }
                }
            }
            return respone;
        }

        private void method_10(int int_0, string string_1, AppRequest appRequest_0)
        {
            string str = this.method_9(appRequest_0.ParamCont, (int) appRequest_0.OrderCode);
            base.userInfo.DownCommd.AddCarNewLogData(int_0, string_1, "发送", appRequest_0.CmdCodeDes, "等待", "", str);
        }

        private string method_11(string string_1)
        {
            return ("8405" + string_1.Substring(0, 10));
        }

        private object method_12(AppRequest appRequest_0, AppRespone appRespone_0)
        {
            object obj3;
            try
            {
                Packer packer = new Packer();
                return packer.packData(this.method_11(appRequest_0.ParamCont[0]));
            }
            catch (Exception exception)
            {
                base.errMsg.ErrorText = "打包发生错误!";
                base.log.WriteError(base.errMsg, exception);
                this.method_8(appRespone_0, base.errMsg.ErrorText);
                obj3 = appRespone_0;
            }
            return obj3;
        }

        private void method_5(AppRequest appRequest_0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("发送");
            builder.Append(",类型-" + appRequest_0.ParamType.ToString());
            builder.Append(",车辆-" + appRequest_0.CarValues);
            builder.Append(",指令-" + appRequest_0.ToString() + appRequest_0.CmdCodeDes);
            builder.Append(",信息-" + appRequest_0.ParamCont[0]);
            builder.Append(",信息类型-透传数据");
            base.logMsg.Msg = builder.ToString();
            base.log.WriteLog(base.logMsg);
        }

        private void method_6(string string_1, string string_2, long long_0, int int_0)
        {
            base.alarmMsg.FunctionName = string_1;
            base.alarmMsg.AlarmText = string.Concat(new object[] { "workid-", base.WorkId, ",simNum-", string_2, ",OrderCode-", int_0 });
            base.alarmMsg.Code = long_0.ToString();
            base.log.WriteAlarm(base.alarmMsg);
        }

        private bool method_7(AppRequest appRequest_0, AppRespone appRespone_0)
        {
            bool flag = true;
            if (!base.CheckCar(appRequest_0.ParamType, appRequest_0.CarValues, appRequest_0.CarPw))
            {
                flag = false;
            }
            if (flag && !base.isStartCommon())
            {
                flag = false;
            }
            if (!flag)
            {
                this.method_8(appRespone_0, base.ErrorMsg);
            }
            return flag;
        }

        private void method_8(AppRespone appRespone_0, string string_1)
        {
            appRespone_0.ResultCode = -1;
            if (!string.IsNullOrEmpty(string_1))
            {
                appRespone_0.ResultMsg = string_1;
            }
        }

        private string method_9(string[] string_1, int int_0)
        {
            string str = "下发参数：";
            if ((string_1 != null) && (string_1.Length > 0))
            {
                str = "下发参数：";
                for (int i = 0; i <= (string_1.Length - 1); i++)
                {
                    str = str + string_1[i] + ";";
                }
            }
            return str.Trim(new char[] { ';' });
        }
    }
}

