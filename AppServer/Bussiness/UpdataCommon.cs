namespace Bussiness
{
    using System;
    using System.Data;

    public class UpdataCommon : ReceiveDataBase
    {
        private CarAlarmType carAlarmType_0 = new CarAlarmType();

        public int ConvertCameraId(int int_0)
        {
            int num = 1;
            int num2 = int_0;
            if (num2 <= 0x10)
            {
                switch (num2)
                {
                    case 0:
                        return 0;

                    case 1:
                        return 1;

                    case 2:
                        return 2;

                    case 3:
                    case 5:
                    case 6:
                    case 7:
                        return num;

                    case 4:
                        return 3;

                    case 8:
                        return 4;
                }
                if (num2 == 0x10)
                {
                    num = 5;
                }
                return num;
            }
            switch (num2)
            {
                case 0x20:
                    return 6;

                case 0x40:
                    return 7;

                case 0x80:
                    num = 8;
                    break;
            }
            return num;
        }

        public string GetAddMsgText(DataRow dr, int type)
        {
            string str = string.Empty;
            string drStr = base.GetDrStr(dr, "AddMsgTxt");
            if (!string.IsNullOrEmpty(drStr))
            {
                str = new AttachData(base.GetDrStr(dr, "carId"), drStr).ParseAttachData();
            }
            return (str + ReportTypeParam.GetMessType(type));
        }

        public void GetCarPartInfo(DataRow dataRow_0, DataRow dataRow_1, CarPartInfo info)
        {
            string drStr = base.GetDrStr(dataRow_1, "phone");
            int drInt = base.GetDrInt(dataRow_1, "statu");
            long num2 = 0L;
            if (dataRow_1["carStatuEx"] != DBNull.Value)
            {
                num2 = Convert.ToInt64(dataRow_1["carStatuEx"]);
            }
            dataRow_0["Status"] = drInt;
            dataRow_0["StatusEx"] = num2;
            if (base.isPosStatus(drInt))
            {
                info.StarNum = base.GetDrStr(dataRow_1, "starNum");
                dataRow_0["GpsValid"] = 1;
            }
            else
            {
                dataRow_0["GpsValid"] = 0;
                info.StarNum = "0";
            }
            info.AccStatus = base.GetACCStatus(drInt);
            info.TransportStatu = base.GetTransportStatus(base.GetDrInt(dataRow_1, "TransportStatus"));
            info.StatusName = AlamStatus.GetStatusNameByCarStatu((long) drInt) + AlamStatus.GetStatusNameByCarStatuExt(num2) + this.carAlarmType_0.GetCustAlarmName(drStr, drInt);
            info.GpsTime = base.GetDrStr(dataRow_1, "gpstime");
            info.DistanceDiff = base.GetDrStr(dataRow_1, "DistanceDiff");
            if (info.DistanceDiff.Length <= 3)
            {
                info.DistanceDiff = "0." + info.DistanceDiff;
            }
            else
            {
                info.DistanceDiff = info.DistanceDiff.Insert(info.DistanceDiff.Length - 3, ".");
                info.DistanceDiff = info.DistanceDiff.Substring(0, info.DistanceDiff.Length - 1);
            }
            info.Speed = base.GetDrStr(dataRow_1, "speed");
            info.Speed = info.Speed.Substring(0, info.Speed.IndexOf('.') + 3);
            info.Lat = base.GetDrStr(dataRow_1, "latitude");
            info.Lon = base.GetDrStr(dataRow_1, "longitude");
            info.Lat = info.Lat.Substring(0, info.Lat.IndexOf('.') + 7);
            info.Lon = info.Lon.Substring(0, info.Lon.IndexOf('.') + 7);
        }

        public string GetStatuDesc(string string_0, string string_1, string string_2, string string_3)
        {
            return string.Format("车载电话：{0}接收时间：{1}卫星时间：{2}车辆状态：{3}", new object[] { string_0, string_1, string_2, string_3 });
        }

        public int SetCarAlarmStatus(DataRow dataRow_0, DataRow dataRow_1, string string_0)
        {
            int num = AlamStatus.IsAlarm(string_0, base.GetDrInt(dataRow_1, "statu"), Convert.ToInt64(dataRow_1["carStatuEx"]));
            if (num == 1)
            {
                dataRow_0["CarStatus"] = 1;
                dataRow_0["AlarmType"] = 1;
                return num;
            }
            dataRow_0["CarStatus"] = 2;
            dataRow_0["AlarmType"] = 2;
            return num;
        }


        /// <summary>
        /// 设置位置更新数据
        /// </summary>
        /// <param name="dataRow_dt">更新 位置数据</param>
        /// <param name="dataRow_src">数据源</param>
        /// <param name="dutyStr">签到数据上传</param>
        /// <param name="szAddMsgText">附加消息</param>
        /// <param name="info">位置信息</param>
        public void SetUpdataPosData(DataRow dataRow_dt, DataRow dataRow_src, string dutyStr, string szAddMsgText, CarPartInfo info)
        {
            int drInt = base.GetDrInt(dataRow_src, "statu");
            dataRow_dt["GpsTime"] = base.GetDrStr(dataRow_src, "GpsTime");
            dataRow_dt["ReceTime"] = base.GetDrStr(dataRow_src, "receTime");
            dataRow_dt["OrderID"] = base.GetDrStr(dataRow_src, "orderId");
            dataRow_dt["CarNum"] = base.GetDrStr(dataRow_src, "carNum");
            dataRow_dt["CarId"] = base.GetDrStr(dataRow_src, "CarId");
            dataRow_dt["OrderType"] = "接收";
            dataRow_dt["orderName"] = base.GetOrderName(base.GetDrInt(dataRow_src, "msgType"));
            dataRow_dt["msgType"] = 0x41;
            dataRow_dt["OrderResult"] = "成功";
            dataRow_dt["CommFlag"] = base.GetCommFlagName(base.GetDrInt(dataRow_src, "commFlag"));
            dataRow_dt["Describe"] = dutyStr + info.GetCarCurrentInfo() + szAddMsgText;
            dataRow_dt["Longitude"] = info.Lon;
            dataRow_dt["Latitude"] = info.Lat;
            dataRow_dt["Speed"] = info.Speed;
            dataRow_dt["IsFill"] = (base.GetDrInt(dataRow_src, "TransportStatus") == 3) ? 1 : 0;
            dataRow_dt["AccOn"] = ((drInt & 0x4000) == 0) ? 0 : 1;
            dataRow_dt["statuName"] = info.StatusName;
            dataRow_dt["SimNum"] = base.GetDrStr(dataRow_src, "phone");
            dataRow_dt["Distance"] = base.GetDrStr(dataRow_src, "distanceDiff");
            dataRow_dt["Direct"] = base.GetDrInt(dataRow_src, "direct");
            //添加AddMsgTxt用于客户端解析数据，依赖UpdataStruct.m_GetDTColumn  huzh 2014.1.6
            dataRow_dt["AddMsgTxt"] = base.GetDrStr(dataRow_src, "AddMsgTxt");
        }

        public void UpdateAlarmFlag(bool bool_0, string string_0, object[] object_0)
        {
            CarInfo dataCarInfoByCarId = CarDataInfoBuffer.GetDataCarInfoByCarId(string_0);
            dataCarInfoByCarId.isAlarm = bool_0;
            dataCarInfoByCarId.CarAlarmData = object_0;
        }
    }
}

