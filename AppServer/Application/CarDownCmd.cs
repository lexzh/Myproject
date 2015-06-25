namespace CarDownCmd
{
    using GPSCarCommServerLib;
    using System;
    using ParamLibrary.Application;

    public class CarDownCmd
    {
        private CarFunc carFunc;
        private Guid guid;
        //protected LogHelper log = new LogHelper();
        //protected LogMsg logMsg = new LogMsg();
        private static int m_OrderId;
        private const int MAX_ORDERID = 0x10000;

        public CarDownCmd(string communicationUrl)
        {
            try
            {
                this.guid = new Guid("7B63B735-7FFD-4A0A-82EB-B3EEE8A33B46");
                object obj2 = Activator.CreateInstance(Type.GetTypeFromCLSID(this.guid, communicationUrl, true));
                this.carFunc = (CarFunc) obj2;
            }
            catch (Exception exception)
            {
                throw new Exception("初始化通讯服务器结果：" + exception.Message);
            }
        }

        public int CalOrderId(int nHigh, int nLow)
        {
            return (((nHigh & 0xffff) << 16) | (nLow & 0xffff));
        }

        public int GetNewOrderId()
        {
            int num = ++m_OrderId;
            if (num > 0x10000)
            {
                num = m_OrderId = 1;
            }
            return num;
        }

        public int OrderId
        {
            get
            {
                return m_OrderId;
            }
        }
 
        public long icar_RemoteDial(int nHigh, int nLow, string carId, string strPhone, string strMsg)
        {
            return (long) this.carFunc.icar_RemoteDial(this.CalOrderId(nHigh, nLow), carId, strPhone, strMsg);
        }

        public long icar_RemoteUpdate(int int_0, int int_1, string string_1, string string_2, ref object object_0)
        {
            return (long) this.carFunc.icar_RemoteUpdate(this.CalOrderId(int_0, int_1), string_1, string_2, ref object_0);
        }

        public long icar_SelMultiPathAlarm(int int_0, int int_1, string string_1, ref object object_0)
        {
            return (long) this.carFunc.icar_SelMultiPathAlarm(this.CalOrderId(int_0, int_1), string_1, ref object_0);
        }

        public long icar_SendCmdXML(int nHigh, int nLow, string strCarId, string strCarType, int CmdCode, int CommFlg, string strXml)
        {
            return (long) this.carFunc.icar_SetCmdXML(this.CalOrderId(nHigh, nLow), strCarId, strCarType, CmdCode, strXml, CommFlg);
        }

        public long icar_SendIOCommand(int wrkid, int OrderID, string string_1, CmdParam.IodeviceType iodeviceType_0, ref object object_0)
        {
            return (long) this.carFunc.icar_SendIOCommand(this.CalOrderId(wrkid, OrderID), string_1, (int) iodeviceType_0, ref object_0);
        }

        public long icar_SendRawPackage(int nHigh, int nLow, string strCarId, CmdParam.CmdCode cmdCode, ref object pvArg, CmdParam.CommMode commMode)
        {
            return (long)this.carFunc.icar_SendRawPackage(this.CalOrderId(nHigh, nLow), strCarId, (int)cmdCode, ref pvArg, (int)commMode);
        }

        public long icar_SendTxtMsg(int int_0, int int_1, string string_1, CmdParam.MsgType msgType_0, string string_2)
        {
            return (long) this.carFunc.icar_SendTxtMsg(this.CalOrderId(int_0, int_1), string_1, (byte) msgType_0, string_2);
        }

        public long icar_SetAlarmFlag(int int_0, int int_1, string string_1, int int_2, int int_3)
        {
            return (long) this.carFunc.icar_SetAlarmFlag(this.CalOrderId(int_0, int_1), string_1, int_2, int_3);
        }

        public long icar_SetBlackBox(int int_0, int int_1, string string_1, CmdParam.ReportType reportType_0, int int_2, byte byte_0, int int_3)
        {
            return (long) this.carFunc.icar_SetBlackBox(this.CalOrderId(int_0, int_1), string_1, (byte) reportType_0, int_2, byte_0, int_3);
        }

        public long icar_SetCallLimit(int int_0, int int_1, string string_1, int int_2, int int_3, ref object object_0, ref object object_1)
        {
            return (long) this.carFunc.icar_SetCallLimit(this.CalOrderId(int_0, int_1), string_1, int_2, int_3, ref object_0, ref object_1);
        }

        public long icar_SetCapture(int int_0, int int_1, string string_1, byte byte_0, byte byte_1, int int_2, int int_3, int int_4, int int_5, byte byte_2, byte byte_3, byte byte_4, byte byte_5, byte byte_6)
        {
            return (long) this.carFunc.icar_SetCapture(this.CalOrderId(int_0, int_1), string_1, byte_0, byte_1, int_2, int_3, int_4, int_5, byte_2, byte_3, byte_4, byte_5, byte_6);
        }

        public long icar_SetCaptureEx(int int_0, int int_1, string string_1, byte byte_0, byte byte_1, int int_2, int int_3, int int_4, int int_5, byte byte_2, byte byte_3, byte byte_4, byte byte_5, byte byte_6, int int_6)
        {
            return (long) this.carFunc.icar_SetCaptureEx(this.CalOrderId(int_0, int_1), string_1, byte_0, byte_1, int_2, int_3, int_4, int_5, byte_2, byte_3, byte_4, byte_5, byte_6, int_6);
        }

        public long icar_SetCaptureExWithTime(int int_0, int int_1, string string_1, byte byte_0, byte byte_1, int int_2, int int_3, int int_4, int int_5, byte byte_2, byte byte_3, byte byte_4, byte byte_5, byte byte_6, int int_6, string string_2, string string_3)
        {
            return (long) this.carFunc.icar_SetCaptureExWithTime(this.CalOrderId(int_0, int_1), string_1, byte_0, byte_1, int_2, int_3, int_4, int_5, byte_2, byte_3, byte_4, byte_5, byte_6, int_6, string_2, string_3);
        }

        public long icar_SetCarInfo(string string_1, string string_2, CmdParam.CommModule commModule_0, string string_3, string string_4, string string_5)
        {
            return (long) this.carFunc.icar_SetCarInfo(string_1, string_2, (int) commModule_0, string_3, string_4, string_5);
        }

        public long icar_SetCommArg(int int_0, int int_1, string string_1, CmdParam.CommMode commMode_0, string string_2, string string_3, string string_4, string string_5, int int_2, string string_6, int int_3, CmdParam.IsUseProxy isUseProxy_0, string string_7, int int_4)
        {
            return (long) this.carFunc.icar_SetCommArg(this.CalOrderId(int_0, int_1), string_1, (byte) commMode_0, string_2, string_3, string_4, string_5, int_2, string_6, int_3, (byte) isUseProxy_0, string_7, int_4);
        }

        public long icar_SetCommMode(int int_0, int int_1, string string_1, CmdParam.CommMode commMode_0)
        {
            return (long) this.carFunc.icar_SetCommMode(this.CalOrderId(int_0, int_1), string_1, (byte) commMode_0);
        }

        public long icar_SetCommonCmd(int int_0, int int_1, string string_1, CmdParam.CmdCode cmdCode_0, ref object object_0, CmdParam.CommMode commMode_0)
        {
            return (long) this.carFunc.icar_SetCommonCmd(this.CalOrderId(int_0, int_1), string_1, (int) cmdCode_0, ref object_0, (int) commMode_0);
        }

        public long icar_SetCustomAlarmer(int int_0, int int_1, string string_1, uint uint_0, uint uint_1, uint uint_2)
        {
            return (long) this.carFunc.icar_SetCustomAlarmer(this.CalOrderId(int_0, int_1), string_1, uint_0, uint_1, uint_2);
        }

        public long icar_SetMinSMSReportInterval(int int_0, int int_1, string string_1, int int_2, int int_3)
        {
            return (long) this.carFunc.icar_SetMinSMSReportInterval(this.CalOrderId(int_0, int_1), string_1, int_2, int_3);
        }

        public long icar_SetMultiPathInfo(int int_0, int int_1, string string_1, double double_0, double double_1, double double_2, double double_3, ref object object_0)
        {
            return (long) this.carFunc.icar_SetMultiPathInfo(this.CalOrderId(int_0, int_1), string_1, double_0, double_1, double_2, double_3, ref object_0);
        }

        public long icar_SetMultiRegionAlarm(int int_0, int int_1, string string_1, ref object object_0)
        {
            return (long) this.carFunc.icar_SetMultiRegionAlarm(this.CalOrderId(int_0, int_1), string_1, ref object_0);
        }

        public long icar_SetMultiSpeedAlarm(int int_0, int int_1, string string_1, ref object object_0)
        {
            return (long) this.carFunc.icar_SetMultiSpeedAlarm(this.CalOrderId(int_0, int_1), string_1, ref object_0);
        }

        public long icar_SetPathAlarm(int int_0, int int_1, string string_1, int int_2, ref object object_0)
        {
            return (long) this.carFunc.icar_SetPathAlarm(this.CalOrderId(int_0, int_1), string_1, int_2, ref object_0);
        }

        public long icar_SetPhone(int int_0, int int_1, string string_1, CmdParam.PhoneType phoneType_0, string string_2)
        {
            return (long) this.carFunc.icar_SetPhone(this.CalOrderId(int_0, int_1), string_1, (byte) phoneType_0, string_2);
        }
        
        public long icar_SetPosReport(int WorkId, int newOrderId, string SimNum, CmdParam.ReportType reportType, int ReportTiming, int ReportCycle, byte isAutoCalArc, CmdParam.IsCompressed isCompressed, CmdParam.ReportWhenStop reportWhenStop)
        {
            return (long) this.carFunc.icar_SetPosReport(this.CalOrderId(WorkId, newOrderId), SimNum, (byte) reportType, ReportTiming, ReportCycle, isAutoCalArc, (byte) isCompressed, (byte) reportWhenStop);
        }

        public long icar_SetRegionAlarm(int int_0, int int_1, string string_1, int int_2, CmdParam.RegionType regionType_0, ref object object_0)
        {
            return (long) this.carFunc.icar_SetRegionAlarm(this.CalOrderId(int_0, int_1), string_1, int_2, (byte) regionType_0, ref object_0);
        }

        public long icar_SetSpeedAlarm(int int_0, int int_1, string string_1, int int_2, int int_3)
        {
            return (long) this.carFunc.icar_SetSpeedAlarm(this.CalOrderId(int_0, int_1), string_1, int_2, int_3);
        }

        public long icar_SetTransportReport(int int_0, int int_1, string string_1, byte byte_0, int int_2, int int_3, int int_4)
        {
            return (long) this.carFunc.icar_SetTransportReport(this.CalOrderId(int_0, int_1), string_1, byte_0, int_2, int_3, int_4);
        }

        public long icar_SimpleCmd(int int_0, int int_1, string string_1, CmdParam.CmdCode cmdCode_0, ref object object_0)
        {
            return (long) this.carFunc.icar_SimpleCmd(this.CalOrderId(int_0, int_1), string_1, (int) cmdCode_0, ref object_0);
        }

        public long icar_SimpleCmdEx(int int_0, int int_1, string string_1, CmdParam.CmdCode cmdCode_0, ref object object_0, CmdParam.CommMode commMode_0)
        {
            return (long) this.carFunc.icar_SimpleCmdEx(this.CalOrderId(int_0, int_1), string_1, (int) cmdCode_0, ref object_0, (int) commMode_0);
        }

        public long icar_StopCapture(int int_0, int int_1, string string_1, byte byte_0, int int_2, int int_3)
        {
            return (long) this.carFunc.icar_StopCapture(this.CalOrderId(int_0, int_1), string_1, byte_0, int_2, int_3);
        }

        public bool isConnect()
        {
            bool flag = true;
            if (this.carFunc.Test() == -1)
            {
                flag = false;
            }
            return flag;
        }

        public int StartCommServer()
        {
            return this.carFunc.StartCommServer();
        }

        public int StopCommServer()
        {
            return this.carFunc.StopCommServer();
        }
    }
}

