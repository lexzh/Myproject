namespace Bussiness
{
    using ParamLibrary.Application;
    using DataAccess;
    using Library;
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using CarDownCmd;

    public class SendBase
    {
        protected const string _ExceptMsg = "下发消息指令时发生错误,错误详细信息：";
        private const string _NoteMsgSend = "发送";
        private const string _NoteMsgWait = "等待";
        protected AlarmMsg alarmMsg = new AlarmMsg();
        [CompilerGenerated]
        private bool bool_0;
        [CompilerGenerated]
        private bool bool_1;
        [CompilerGenerated]
        private bool bool_2;
        protected static CarDownCmd CarCmdSend;
        protected ArrayList carInfoList = new ArrayList(10);
        protected Library.ErrorMsg errMsg = new Library.ErrorMsg();
        protected LogHelper log = new LogHelper();
        protected LogMsg logMsg = new LogMsg();
        public string m_OrderCode = string.Empty;
        public string m_Params1 = string.Empty;
        public string m_Params2 = string.Empty;
        public static string m_ProtocolName;
        public string m_UserId = string.Empty;
        protected int m_WorkId;
        protected const int MAX_SEND_SIZE = 500;
        [CompilerGenerated]
        private string string_0;
        protected OnlineUserInfo userInfo;

        static SendBase()
        {
            m_ProtocolName = "JTBGPS";
        }

        protected void AddUpDataLog(int int_0, string string_1, string string_2, string string_3)
        {
            this.userInfo.DownCommd.AddCarNewLogData(int_0, string_1, "发送", string_2, "等待", "", string_3);
        }

        public int CalOrderId(int int_0, int int_1)
        {
            return (((int_0 & 0xffff) << 0x10) | (int_1 & 0xffff));
        }

        protected bool CheckCar(CmdParam.ParamType paramType_0, string string_1, string string_2)
        {
            if ((this.carInfoList != null) && (this.carInfoList.Count > 0))
            {
                this.carInfoList.Clear();
                this.carInfoList.TrimToSize();
            }
            if (string.IsNullOrEmpty(string_1))
            {
                this.ErrorMsg = "请输入查询内容！";
                return false;
            }
            string[] strArray = string_1.Split(new char[] { ',' });
            if ((strArray.Length > 1) && !this.IsMultiSend)
            {
                this.ErrorMsg = "不允许多车发送！";
                return false;
            }
            if (strArray.Length > 500)
            {
                this.ErrorMsg = "下发指令的车辆数不能超过500！";
                return false;
            }
            foreach (string str in strArray)
            {
                CarInfo info = this.method_3(paramType_0, str);
                if (!this.method_0(info))
                {
                    return false;
                }
                if ((this.IsAllowNullPassWord && (strArray.Length == 1)) && !this.method_2(info.SimNum, string_2))
                {
                    this.ErrorMsg = "车辆密码不对！";
                    return false;
                }
                this.carInfoList.Add(info);
            }
            return true;
        }

        public DataTable GetCarInfoByArea(string string_1, string string_2, string string_3, string string_4, string string_5)
        {
            double num = double.Parse(string_3) / 99898.7;
            double num2 = double.Parse(string_3) / 111194.8;
            double num3 = double.Parse(string_1) - num;
            double num4 = double.Parse(string_1) + num;
            double num5 = double.Parse(string_2) - num2;
            double num6 = double.Parse(string_2) + num2;
            return this.GetCarInfoByArea(num3.ToString(), num5.ToString(), num4.ToString(), num6.ToString(), string_4, string_5);
        }

        public DataTable GetCarInfoByArea(string string_1, string string_2, string string_3, string string_4, string string_5, string string_6)
        {
            string str = "WebGpsClient_GetCarsInSelArea";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@MinX", string_1), new SqlParameter("@MinY", string_2), new SqlParameter("@MaxX", string_3), new SqlParameter("@MaxY", string_4), new SqlParameter("@AreaCode", string_5), new SqlParameter("@userID", string_6) };
            DataTable table = null;
            try
            {
                table = new SqlDataAccess().getDataBySP(str, parameterArray);
            }
            catch (Exception exception)
            {
                this.log.WriteError(this.errMsg, exception);
            }
            return table;
        }

        protected string GetExceptionMsg(Exception exception_0)
        {
            return (exception_0.Message + exception_0.StackTrace);
        }

        protected bool IsNullDataTable(DataTable dataTable_0)
        {
            if ((dataTable_0 != null) && (dataTable_0.Rows.Count > 0))
            {
                return false;
            }
            return true;
        }

        protected bool isStartCommon()
        {
            bool flag = true;
            try
            {
                if (CarCmdSend == null)
                {
                    CarCmdSend = new CarDownCmd(Const.communicationUrl);
                }
                try
                {
                    if (!CarCmdSend.isConnect())
                    {
                        flag = false;
                        this.ErrorMsg = "TEST通讯服务器失败！";
                        this.log.WriteAlarm(this.alarmMsg);
                    }
                }
                catch (Exception exception)
                {
                    this.log.WriteError(this.errMsg, exception);
                    CarCmdSend = new CarDownCmd(Const.communicationUrl);
                    if (CarCmdSend.StartCommServer() != 0)
                    {
                        this.errMsg.ErrorText = "启动通讯服务器发生错误!";
                        this.ErrorMsg = this.errMsg.ErrorText;
                        this.log.WriteError(this.errMsg, exception);
                        flag = false;
                    }
                }
            }
            catch (Exception exception2)
            {
                this.errMsg.ErrorText = "初始化通讯服务器发生错误！";
                this.ErrorMsg = "下发发生错误,请重新再试！";
                this.log.WriteError(this.errMsg, exception2);
                flag = false;
            }
            return flag;
        }

        private bool method_0(CarInfo carInfo_0)
        {
            if ((carInfo_0 != null) && this.method_1(carInfo_0.SimNum))
            {
                if (!this.IsSudoOverDue && (carInfo_0.overTime < 0))
                {
                    this.ErrorMsg = "车牌号为：" + carInfo_0.CarNum + "该车辆服务已到期!";
                    return false;
                }
                return true;
            }
            this.ErrorMsg = "查询内容中存在不正确的车辆信息!";
            return false;
        }

        private bool method_1(string string_1)
        {
            bool flag = true;
            if (CarDataInfoBuffer.GetDataCarInfoBySimNum(string_1) == null)
            {
                flag = false;
            }
            return flag;
        }

        private bool method_2(string string_1, string string_2)
        {
            bool flag = true;
            CarInfo dataCarInfoBySimNum = CarDataInfoBuffer.GetDataCarInfoBySimNum(string_1);
            if (((dataCarInfoBySimNum != null) && !string.IsNullOrEmpty(dataCarInfoBySimNum.Password)) && !dataCarInfoBySimNum.Password.Equals(string_2))
            {
                flag = false;
            }
            return flag;
        }

        private CarInfo method_3(CmdParam.ParamType paramType, string str)
        {
            //修改类型判断
            switch (paramType)
            {
                case CmdParam.ParamType.CarNum:
                    return CarDataInfoBuffer.GetDataCarInfoByCarNum(str);

                case CmdParam.ParamType.CarId:
                    return CarDataInfoBuffer.GetDataCarInfoByCarId(str);

                case CmdParam.ParamType.SimNum:
                    return CarDataInfoBuffer.GetDataCarInfoBySimNum(str);
            }
            return null;
        }

        private string method_4()
        {
            string name = string.Empty;
            StackFrame frame = new StackTrace().GetFrame(2);
            if (frame != null)
            {
                name = frame.GetMethod().Name;
            }
            return name;
        }


        protected void SaveCmdParm(string newOrderId)
        {
            this.SaveCommandParameterToGpsLogTable(newOrderId, null);
        }

        protected void SaveCommandParameterToGpsLogTable(string newOrderId, string string_2)
        {
            if ((this.m_Params1 != null) && (this.m_Params1.Length != 0))
            {
                Car car = new Car();
                if (string.IsNullOrEmpty(string_2))
                {
                    car.SaveCarSetParam(this.WorkId, newOrderId, this.m_OrderCode, this.m_Params1);
                }
                else
                {
                    car.SaveCarSetParam(this.WorkId, newOrderId, string_2, this.m_Params1);
                }
                car.SaveCarCmdParam(this.WorkId, newOrderId, this.m_UserId, ((CmdParam.OrderCode) Convert.ToInt32(this.m_OrderCode)).ToString(), this.m_OrderCode, string_2, this.m_Params2);
            }
        }

        protected void WriteError(string string_1, string string_2, string string_3)
        {
            this.alarmMsg.FunctionName = this.method_4();
            this.alarmMsg.AlarmText = "workid-" + string_1 + ",simNum-" + string_2 + ",OrderCode-" + string_3;
            this.log.WriteAlarm(this.alarmMsg);
        }

        protected void WriteLog(string string_1, string string_2)
        {
            this.logMsg.FunctionName = this.method_4();
            this.logMsg.Msg = "发送：类型-" + string_1 + ",车辆-" + string_2;
            this.log.WriteLog(this.logMsg);
        }

        public string ErrorMsg
        {
            [CompilerGenerated]
            get
            {
                return this.string_0;
            }
            [CompilerGenerated]
            set
            {
                this.string_0 = value;
            }
        }

        protected bool IsAllowNullPassWord
        {
            [CompilerGenerated]
            get
            {
                return this.bool_2;
            }
            [CompilerGenerated]
            set
            {
                this.bool_2 = value;
            }
        }

        protected bool IsMultiSend
        {
            [CompilerGenerated]
            get
            {
                return this.bool_1;
            }
            [CompilerGenerated]
            set
            {
                this.bool_1 = value;
            }
        }

        protected bool IsSudoOverDue
        {
            [CompilerGenerated]
            get
            {
                return this.bool_0;
            }
            [CompilerGenerated]
            set
            {
                this.bool_0 = value;
            }
        }

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
    }
}

