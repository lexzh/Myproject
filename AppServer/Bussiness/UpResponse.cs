namespace Bussiness
{
    using Library;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    public class UpResponse : ReceiveDataBase
    {
        private UpdataCommon updataCommon_0 = new UpdataCommon();

        public int CalNewLog(DataRow dataRow_0, DataTable dataTable_0)
        {
            int num = -1;
            try
            {
                switch (base.GetDrInt(dataRow_0, "MsgType"))
                {
                    case 0x41:
                        this.method_2(dataRow_0, dataTable_0);
                        break;

                    case 0x42:
                        this.method_7(dataRow_0, dataTable_0);
                        break;

                    case 0x43:
                        this.method_6(dataRow_0, dataTable_0);
                        break;

                    case 0x44:
                        this.method_9(dataRow_0, dataTable_0);
                        break;

                    case 0x45:
                        this.method_5(dataRow_0, dataTable_0);
                        break;

                    case 70:
                        this.method_8(dataRow_0, dataTable_0);
                        break;

                    case 240:
                        this.method_4(dataRow_0, dataTable_0);
                        break;

                    case 0xaa01:
                        this.method_0(dataRow_0, dataTable_0);
                        break;

                    case 0xaa02:
                        this.method_1(dataRow_0, dataTable_0);
                        break;
                }
                num = 0;
            }
            catch (Exception exception)
            {
                //LogHelper helper = new LogHelper();
                //ErrorMsg msg = new ErrorMsg("CommandResponse", helper.GetCallFunction(), helper.GetExceptionMsg(exception));
                //helper.WriteError(msg);
            }
            return num;
        }

        private void method_0(DataRow dataRow_0, DataTable dataTable_0)
        {
            int drInt = base.GetDrInt(dataRow_0, "reserved");
            CarPartInfo info = new CarPartInfo();
            DataRow row = dataTable_0.NewRow();
            this.updataCommon_0.GetCarPartInfo(row, dataRow_0, info);
            this.updataCommon_0.SetUpdataPosData(row, dataRow_0, null, this.updataCommon_0.GetAddMsgText(dataRow_0, drInt), info);
            row["OrderName"] = "掉线通知";
            row["msgType"] = 0xaa01;
            row["OrderType"] = "信息";
            row["isImportWatch"] = -1;
            row["CarStatus"] = 2;
            row["AlarmType"] = 1;
            row["speed"] = base.GetDrStr(dataRow_0, "speed");
            row["AccOn"] = ((base.GetDrInt(dataRow_0, "statu") & 0x4000) == 0) ? 0 : 1;
            row["ReceTime"] = base.GetDrStr(dataRow_0, "instime");
            dataTable_0.Rows.Add(row);
        }

        private void method_1(DataRow dataRow_0, DataTable dataTable_0)
        {
            int drInt = base.GetDrInt(dataRow_0, "reserved");
            CarPartInfo info = new CarPartInfo();
            DataRow row = dataTable_0.NewRow();
            this.updataCommon_0.GetCarPartInfo(row, dataRow_0, info);
            this.updataCommon_0.SetUpdataPosData(row, dataRow_0, null, this.updataCommon_0.GetAddMsgText(dataRow_0, drInt), info);
            row["OrderName"] = "行政区域报警";
            row["msgType"] = 0xaa02;
            row["OrderType"] = "信息";
            row["isImportWatch"] = -1;
            row["CarStatus"] = 2;
            row["AlarmType"] = 1;
            row["speed"] = base.GetDrStr(dataRow_0, "speed");
            row["AccOn"] = ((base.GetDrInt(dataRow_0, "statu") & 0x4000) == 0) ? 0 : 1;
            row["ReceTime"] = base.GetDrStr(dataRow_0, "instime");
            dataTable_0.Rows.Add(row);
        }

        private string method_10(int int_0, int int_1, out string string_0)
        {
            string str = "失败";
            string_0 = string.Empty;
            if (int_1 == 0)
            {
                str = "成功";
            }
            if (int_0 != 0x387)
            {
                if (int_0 == 0xffff)
                {
                    string_0 = ErrorDesc.GetDetailErrorMessage(int_1);
                    return str;
                }
                string_0 = ErrorDesc.GetErrorMessage(int_1);
                return str;
            }
            switch (int_1)
            {
                case 0:
                    string_0 = "车主确认电召";
                    return str;

                case 1:
                    string_0 = "车主拒绝电召";
                    return str;

                case 3:
                    string_0 = "车主超时无应答";
                    return str;
            }
            string_0 = "未知信息";
            return str;
        }

        private void method_2(DataRow dataRow_0, DataTable dataTable_0)
        {
            int drInt = base.GetDrInt(dataRow_0, "reserved");
            int num2 = base.GetDrInt(dataRow_0, "isImportWatch");
            string drStr = base.GetDrStr(dataRow_0, "dutyStr");
            Convert.ToString(dataRow_0["phone"]);
            string str2 = string.Empty;
            if ((drStr != null) && (drStr.Length > 0))
            {
                bool flag = false;
                str2 = this.method_3(drStr, out flag);
            }
            DataRow row = dataTable_0.NewRow();
            CarPartInfo info = new CarPartInfo();
            this.updataCommon_0.GetCarPartInfo(row, dataRow_0, info);
            row["isImportWatch"] = num2;
            this.updataCommon_0.SetUpdataPosData(row, dataRow_0, str2, this.updataCommon_0.GetAddMsgText(dataRow_0, drInt), info);
            row["CarStatus"] = 2;
            row["AlarmType"] = 0;
            row["ReceTime"] = base.GetDrStr(dataRow_0, "instime");
            dataTable_0.Rows.Add(row);
        }

        private string method_3(string string_0, out bool bool_0)
        {
            string str = "";
            bool_0 = false;
            string[] strArray = string_0.Split(new char[] { '\\' });
            if (strArray.Length == 2)
            {
                str = "员工:" + strArray[0] + "->";
                try
                {
                    switch (Convert.ToInt32(strArray[1]))
                    {
                        case 0:
                            str = str + "签退";
                            bool_0 = true;
                            return str;

                        case 1:
                            return (str + "签到");
                    }
                    return (str + "未知状态");
                }
                catch
                {
                    str = str + "未知状态";
                }
            }
            return str;
        }

        private void method_4(DataRow dataRow_0, DataTable dataTable_0)
        {
            int drInt = base.GetDrInt(dataRow_0, "orderResult");
            int num2 = base.GetDrInt(dataRow_0, "commFlag");
            string str = "";
            string str2 = "失败";
            if (drInt == 0)
            {
                str2 = "成功";
            }
            else
            {
                switch (num2)
                {
                    case 1:
                    case 0:
                    {
                        int num3 = 0x4000 | drInt;
                        if (num3 == 0xf041)
                        {
                            str2 = "无网络";
                        }
                        this.method_10(0, num3, out str);
                        str = "错误描述：" + str;
                        break;
                    }
                }
            }
            DataRow row = dataTable_0.NewRow();
            row["GpsTime"] = base.GetDrStr(dataRow_0, "GpsTime");
            row["ReceTime"] = base.GetDrStr(dataRow_0, "instime");
            row["OrderID"] = base.GetDrStr(dataRow_0, "orderId");
            row["CarNum"] = base.GetDrStr(dataRow_0, "carNum");
            row["CarId"] = base.GetDrStr(dataRow_0, "CarId");
            row["OrderType"] = "发送";
            row["OrderName"] = "命令发送应答";
            row["msgType"] = 240;
            row["OrderResult"] = str2;
            row["CommFlag"] = base.GetCommFlagName(num2);
            row["Describe"] = str;
            row["speed"] = base.GetDrStr(dataRow_0, "speed");
            row["AccOn"] = ((base.GetDrInt(dataRow_0, "statu") & 0x4000) == 0) ? 0 : 1;
            dataTable_0.Rows.Add(row);
        }

        private void method_5(DataRow dataRow_0, DataTable dataTable_0)
        {
            int drInt = base.GetDrInt(dataRow_0, "RespValue");
            int num2 = base.GetDrInt(dataRow_0, "cmdResult");
            string str = string.Empty;
            int num3 = base.GetDrInt(dataRow_0, "commFlag");
            DataRow row = dataTable_0.NewRow();
            row["GpsTime"] = base.GetDrStr(dataRow_0, "GpsTime");
            row["ReceTime"] = base.GetDrStr(dataRow_0, "instime");
            row["OrderID"] = base.GetDrStr(dataRow_0, "orderId");
            row["CarNum"] = base.GetDrStr(dataRow_0, "carNum");
            row["CarId"] = base.GetDrStr(dataRow_0, "CarId");
            row["OrderType"] = "接收";
            row["OrderName"] = RespCodeParam.GetRespName(drInt);
            row["msgType"] = 0x45;
            row["OrderResult"] = this.method_10(drInt, num2, out str);
            row["CommFlag"] = base.GetCommFlagName(num3);
            row["Describe"] = str;
            row["RespCode"] = drInt;
            row["speed"] = base.GetDrStr(dataRow_0, "speed");
            row["AccOn"] = ((base.GetDrInt(dataRow_0, "statu") & 0x4000) == 0) ? 0 : 1;
            dataTable_0.Rows.Add(row);
            if (base.IsCancelAlarm(drInt) && (num2 == 0))
            {
                this.updataCommon_0.UpdateAlarmFlag(false, Convert.ToString(row["CarId"]), null);
            }
            if ((drInt == 0x1bc) || (drInt == 0x184))
            {
                DataRow row2 = dataTable_0.NewRow();
                try
                {
                    row2["GpsTime"] = base.GetDrStr(dataRow_0, "GpsTime");
                    row2["ReceTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    row2["OrderID"] = 0;
                    row2["CarNum"] = base.GetDrStr(dataRow_0, "carNum");
                    row2["CarId"] = base.GetDrStr(dataRow_0, "CarId");
                    row2["OrderType"] = "信息";
                    row2["OrderName"] = "提示信息";
                    row2["msgType"] = -1;
                    row2["CommFlag"] = "";
                    new CarAlarmType().UpdateAlarmType(base.GetDrStr(dataRow_0, "phone"));
                    row2["OrderResult"] = "成功";
                    row2["Describe"] = "下载数据成功";
                }
                catch (Exception exception)
                {
                    row2["OrderResult"] = "失败";
                    row2["Describe"] = "下载数据失败";
                    LogHelper helper = new LogHelper();
                    ErrorMsg msg = new ErrorMsg("UpdataNewLogData", helper.GetCallFunction(), helper.GetExceptionMsg(exception));
                    helper.WriteError(msg);
                }
                dataTable_0.Rows.Add(row2);
            }
        }

        private void method_6(DataRow dataRow_0, DataTable dataTable_0)
        {
            SelfTestMsg msg = new SelfTestMsg {
                SCREENstatus = base.GetDrInt(dataRow_0, "LCDstatu"),
                RAMstatus = base.GetDrInt(dataRow_0, "RAMstatu"),
                GPSstatus = base.GetDrInt(dataRow_0, "GPSstatu"),
                GSMstatus = base.GetDrInt(dataRow_0, "GSMstatu"),
                StarStatus = base.GetDrInt(dataRow_0, "StarStatu"),
                NetworkStatus = base.GetDrInt(dataRow_0, "NetWorkStatu"),
                PhoneStatus = base.GetDrInt(dataRow_0, "PhoneStatu"),
                Context = base.GetDrStr(dataRow_0, "configStr")
            };
            int drInt = base.GetDrInt(dataRow_0, "commFlag");
            DataRow row = dataTable_0.NewRow();
            row["GpsTime"] = base.GetDrStr(dataRow_0, "GpsTime");
            row["ReceTime"] = base.GetDrStr(dataRow_0, "instime");
            row["OrderID"] = base.GetDrStr(dataRow_0, "orderId");
            row["CarNum"] = base.GetDrStr(dataRow_0, "carNum");
            row["CarId"] = base.GetDrStr(dataRow_0, "CarId");
            row["OrderType"] = "接收";
            row["OrderName"] = msg.SelfTypeName;
            row["msgType"] = 0x43;
            row["OrderResult"] = "成功";
            row["CommFlag"] = base.GetCommFlagName(drInt);
            row["Describe"] = msg.GetCheckTestMsg();
            row["speed"] = base.GetDrStr(dataRow_0, "speed");
            row["AccOn"] = ((base.GetDrInt(dataRow_0, "statu") & 0x4000) == 0) ? 0 : 1;
            dataTable_0.Rows.Add(row);
        }

        private void method_7(DataRow dataRow_0, DataTable dataTable_0)
        {
            int drInt = base.GetDrInt(dataRow_0, "commFlag");
            CarPartInfo info = new CarPartInfo();
            DataRow row = dataTable_0.NewRow();
            string statusNameByCarStatu = AlamStatus.GetStatusNameByCarStatu((long) base.GetDrInt(dataRow_0, "statu"));
            if (base.isPosStatus(base.GetDrInt(dataRow_0, "statu")))
            {
                info.StarNum = base.GetDrStr(dataRow_0, "starNum");
                row["GpsValid"] = 1;
            }
            else
            {
                row["GpsValid"] = 0;
                info.StarNum = "0";
            }
            info.AccStatus = base.GetACCStatus(base.GetDrInt(dataRow_0, "zip_carstatu"));
            string drStr = base.GetDrStr(dataRow_0, "zip_speed");
            info.Speed = drStr.Substring(0, drStr.IndexOf('.') + 3);
            info.TransportStatu = base.GetTransportStatus(base.GetDrInt(dataRow_0, "zip_TransportStatus"));
            info.StatusName = statusNameByCarStatu;
            info.GpsTime = base.GetDrStr(dataRow_0, "gpstime");
            info.DistanceDiff = base.GetDrStr(dataRow_0, "zip_DistanceDiff");
            string str3 = base.GetDrStr(dataRow_0, "latitude1");
            string str4 = base.GetDrStr(dataRow_0, "longitude1");
            info.Lat = str3.Substring(0, str3.IndexOf('.') + 7);
            info.Lon = str4.Substring(0, str4.IndexOf('.') + 7);
            row["GpsTime"] = info.GpsTime;
            row["receTime"] = base.GetDrStr(dataRow_0, "instime");
            row["OrderID"] = base.GetDrStr(dataRow_0, "orderId");
            row["CarNum"] = base.GetDrStr(dataRow_0, "carNum");
            row["CarId"] = base.GetDrStr(dataRow_0, "carId");
            row["SimNum"] = base.GetDrStr(dataRow_0, "phone");
            row["OrderType"] = "接收";
            row["OrderName"] = "压缩定位信息报文";
            row["msgType"] = 0x42;
            row["OrderResult"] = "成功";
            row["CommFlag"] = base.GetCommFlagName(drInt);
            row["Describe"] = info.GetCarCurrentInfo() + base.GetCommFlagName(drInt);
            row["Longitude"] = info.Lon;
            row["Latitude"] = info.Lat;
            row["IsImportWatch"] = base.GetDrInt(dataRow_0, "isImportWatch");
            row["IsFill"] = (base.GetDrInt(dataRow_0, "zip_TransportStatus") == 3) ? 1 : 0;
            row["AccOn"] = ((base.GetDrInt(dataRow_0, "zip_carstatu") & 0x4000) == 0) ? 0 : 1;
            row["statuName"] = info.StatusName;
            row["Distance"] = info.DistanceDiff;
            row["Direct"] = base.GetDrInt(dataRow_0, "zip_direct");
            row["speed"] = drStr;
            dataTable_0.Rows.Add(row);
        }

        private void method_8(DataRow dataRow_0, DataTable dataTable_0)
        {
            int drInt = base.GetDrInt(dataRow_0, "commFlag");
            DataRow row = dataTable_0.NewRow();
            row["GpsTime"] = base.GetDrStr(dataRow_0, "GpsTime");
            row["receTime"] = base.GetDrStr(dataRow_0, "instime");
            row["OrderID"] = base.GetDrStr(dataRow_0, "orderId");
            row["CarNum"] = base.GetDrStr(dataRow_0, "carNum");
            row["CarId"] = base.GetDrStr(dataRow_0, "carId");
            row["OrderType"] = "接收";
            row["OrderName"] = "查询标志位响应";
            row["msgType"] = 70;
            row["OrderResult"] = "成功";
            row["CommFlag"] = base.GetCommFlagName(drInt);
            row["Describe"] = base.GetDrStr(dataRow_0, "content");
            row["speed"] = base.GetDrStr(dataRow_0, "speed");
            row["AccOn"] = ((base.GetDrInt(dataRow_0, "statu") & 0x4000) == 0) ? 0 : 1;
            dataTable_0.Rows.Add(row);
        }

        private void method_9(DataRow dataRow_0, DataTable dataTable_0)
        {
            string drStr = base.GetDrStr(dataRow_0, "content");
            int drInt = base.GetDrInt(dataRow_0, "text_msgType");
            int num2 = base.GetDrInt(dataRow_0, "commFlag");
            if (3 == drInt)
            {
                drStr = drStr + "；单位：0.01度";
            }
            DataRow row = dataTable_0.NewRow();
            row["GpsTime"] = base.GetDrStr(dataRow_0, "GpsTime");
            row["receTime"] = base.GetDrStr(dataRow_0, "instime");
            row["OrderID"] = base.GetDrStr(dataRow_0, "orderId");
            row["CarNum"] = base.GetDrStr(dataRow_0, "carNum");
            row["CarId"] = base.GetDrStr(dataRow_0, "carId");
            row["OrderType"] = "接收";
            row["OrderName"] = RemotingParam.GetRespOrderNameName(drInt);
            row["msgType"] = 0x44;
            row["OrderResult"] = "成功";
            row["CommFlag"] = base.GetCommFlagName(num2);
            row["Describe"] = drStr;
            row["speed"] = base.GetDrStr(dataRow_0, "speed");
            row["AccOn"] = ((base.GetDrInt(dataRow_0, "statu") & 0x4000) == 0) ? 0 : 1;
            dataTable_0.Rows.Add(row);
        }
    }
}

