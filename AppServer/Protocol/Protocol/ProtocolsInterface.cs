namespace Protocol
{
    using System;
    using System.Data;

    public class ProtocolsInterface : Parse
    {
        private UpdataIODeviceAttachInfo updataIODeviceAttachInfo_0 = new UpdataIODeviceAttachInfo();

        /// <summary>
        /// 解析数据提供发送到客户端
        /// </summary>
        /// <param name="dr_src">数据源</param>
        /// <param name="dr_out">解析后的数据</param>
        /// <returns></returns>
        public bool DataParse(DataRow dr_src, DataRow dr_out)
        {
            bool flag = true;
            ProtocolDrData data = new ProtocolDrData(dr_src);
            if (data.EquipId == 13)
            {
                DB44DataParse parse = new DB44DataParse(data, dr_out);
                if ("023834".Equals(data.ContextData.Substring(0, 6)))
                {
                    parse.SumPriceNanjin(dr_out);
                    return flag;
                }
                return false;
            }
            if (data.EquipId == 0x15)
            {
                DB44DataParse parse2 = new DB44DataParse(data, dr_out);
                switch (data.ResponseType.ToUpper())
                {
                    case "2083":
                        parse2.AccidentDataPrase(dr_out);
                        return flag;

                    case "20D0":
                        parse2.OverSpeedDataPrase(dr_out);
                        return flag;

                    case "2085":
                        parse2.RoadLineDataPrase(dr_out);
                        return flag;

                    case "20D1":
                        parse2.RunDistanceDataPrase(dr_out);
                        return flag;

                    case "2081":
                        parse2.ParamLookDataPrase1(dr_out);
                        return flag;

                    case "2082":
                        parse2.ParamLookDataPrase2(dr_out);
                        return flag;

                    case "2086":
                        parse2.DriverIdDataPrase(dr_out);
                        return flag;

                    case "2087":
                        parse2.RemotingParamDataPrase1(dr_out);
                        return flag;

                    case "2088":
                        parse2.RemotingParamDataPrase2(dr_out);
                        return flag;

                    case "20FF":
                        parse2.DriverStateDataPrase(dr_out);
                        return flag;
                }
                return false;
            }
            if (data.EquipId != 0x1a)
            {
                return flag;
            }
            JTBDataParse parse3 = new JTBDataParse(data, dr_out) {
                IODeviceAttachInfo = this.updataIODeviceAttachInfo_0
            };
            switch (data.ResponseType.ToUpper())
            {
                case "4081":
                    parse3.GetAccidentReportData(dr_out);
                    return flag;

                case "4082":
                    parse3.GetAskData(dr_out);
                    return flag;

                case "4083":
                    parse3.GetMessagePlayData(dr_out);
                    return flag;

                case "4084":
                    parse3.GetCarDriverData(dr_out);
                    return flag;

                case "4085":
                    parse3.GetEletroryRecordReportData(dr_out);
                    return flag;

                case "4086":
                    parse3.GetMutilMediaAccidentData(dr_out);
                    return flag;

                case "4088":
                    parse3.GetStoreMutilMediaUpData(dr_out);
                    return flag;

                case "408B":
                    parse3.GetDriverInfomationUpData(dr_out);
                    return flag;

                case "408C":
                    parse3.GetRemotingParameterUpData(dr_out);
                    return flag;

                case "408D":
                    parse3.Get360DistanceUpData(dr_out);
                    return flag;

                case "408E":
                    parse3.Get2DayDistanceUpData(dr_out);
                    return flag;

                case "4090":
                    parse3.Get360SpeedUpData(dr_out);
                    return flag;

                case "4091":
                    parse3.Get2DaySpeedUpData(dr_out);
                    return flag;

                case "4092":
                    parse3.GetRemotingParameterAboutVINUpData(dr_out);
                    return flag;

                case "408F":
                    parse3.GetCarRemotingParameterUpData(dr_out);
                    return flag;

                case "4093":
                    parse3.GetCarDoubtfulDataUpData(dr_out);
                    return flag;

                case "4094":
                    parse3.GetCar2DayOverSpeedUpData(dr_out);
                    return flag;

                case "4095":
                    parse3.GetZipDataUpData(dr_out);
                    return flag;

                case "4087":
                    parse3.GetTransportDataUpData(dr_out);
                    return flag;

                //查岗指令
                case "4910":
                    parse3.GetPlatRequestUpData(dr_out);
                    return flag;

                case "4911":
                    parse3.GetDownPlatRequestUpData(dr_out);
                    return flag;

                case "4912":
                    //报警督办
                    parse3.GetDirectionRequestUpData(dr_out);
                    return flag;

                case "4913":
                    parse3.GetRreAlarmRequestUpData(dr_out);
                    return flag;

                case "4914":
                    parse3.GetChangeAlarmRequestUpData(dr_out);
                    return flag;

                case "4915":
                    parse3.GetCarPostionMessageRequestUpData(dr_out);
                    return flag;

                case "4916":
                    parse3.GetCarStopPostionMessageRequestUpData(dr_out);
                    return flag;

                case "4917":
                    parse3.GetPlatLinkUpData(dr_out);
                    return flag;

                case "4901":
                    parse3.GetSendPosInfor(dr_out);
                    return flag;

                case "4902":
                    parse3.GetApplyforExchangePosInfor(dr_out);
                    return flag;

                case "4903":
                    parse3.GetCancelExchangePosInfor(dr_out);
                    return flag;
            }
            return false;
        }

        public UpdataIODeviceAttachInfo IODeviceAttachInfo
        {
            get
            {
                return this.updataIODeviceAttachInfo_0;
            }
            set
            {
                this.updataIODeviceAttachInfo_0 = value;
            }
        }
    }
}

