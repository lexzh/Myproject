namespace Protocol
{
    using PublicClass;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Library;

    public class JTBDataParse : Parse
    {
        private const string JTB_ACCIDENT_REPORT = "事件报告应答";
        private const string JTB_AccidentDoubtfulPointData_ResponseMessage = "事故疑点数据应答";
        private const string JTB_AlarmSuperviseRequest = "报警督办请求";
        private const string JTB_ApplyforExchangePosInfor_ResponseMessage = "申请交换指定车辆定位信息应答";
        private const string JTB_ASK_REPORT = "提问消息应答";
        private const string JTB_CancelDesignateCarPostInfo_ResponseMessage = "取消交换指定车辆定位信息应答";
        private const string JTB_CAR_DRIVER_MESSAGE = "驾驶员身份信息应答";
        private const string JTB_CAR_REMOTING_PARAMETER_ABOUT_MESSAGE = "采集车辆VIN号车牌号码车牌分类远程参数查看应答";
        private const string JTB_CAR_REMOTING_PARAMETER_MESSAGE = "采集记录仪中的车辆特征系数远程参数查看应答";
        private const string JTB_CompressData = "压缩数据";
        private const string JTB_DISTANCE2DAY_MESSAGE = "2天里程查询应答";
        private const string JTB_DISTANCE360_MESSAGE = "360小时里程查询应答";
        private const string JTB_DRIVER_MESSAGE = "驾驶员身份数据应答";
        private const string JTB_ELETRORY_MESSAGE = "电子运单上报信息应答";
        private const string JTB_EndUp_ExchangeCarPostInfoRequest = "结束车辆定位信息交换请求";
        private const string JTB_ExchangeAlarmInfoMessage = "交换警情信息消息";
        private const string JTB_GatherCarVinAndCarNum_ResponseMessage = "采集车辆VIN号车牌号码车牌分类应答";
        private const string JTB_MESSAGE_CANCEL_REPORT = "信息点播/取消应答";
        private const string JTB_MESSAGE_PLAY_REPORT = "信息点播/取消应答";
        private const string JTB_MUTIL_MEDIA_ACCIDENT_MESSAGE = "多媒体事件信息上传数据应答";
        private const string JTB_PlatformConnectionMessage = "平台连线消息";
        private const string JTB_PlatformOnlinePostRequest = "平台查岗请求";
        private const string JTB_PlatformToPlatform_MessageRequest = "下发平台间报文请求";
        private const string JTB_PreAlarmMessage = "报警预警消息";
        private const string JTB_QueryDuring2DayDrivingOvertime_ResponseMessage = "查询2个日历天内超时驾驶情况应答";
        private const string JTB_REMOTING_MESSAGE = "远程参数查看应答";
        private const string JTB_ReSendCarPostInfoResponseMessage = "补发车辆定位信息应答";
        private const string JTB_SPEED2DAY_MESSAGE = "采集2天行驶速度数据应答";
        private const string JTB_SPEED360_MESSAGE = "采集360小时行驶速度数据应答";
        private const string JTB_StarUp_ExchangeCarPostInfoRequest = "启动车辆定位信息交换请求";
        private const string JTB_STORE_MUTIL_MEDIA_MESSAGE = "存储多媒体数据检索应答";
        private const string JTB_TerminalDataThoroughly = "终端数据透传";
        private ProtocolDrData protocolDrData_0;
        [CompilerGenerated]
        private UpdataIODeviceAttachInfo updataIODeviceAttachInfo_0;

        public JTBDataParse(ProtocolDrData protocolDrData_1, DataRow dataRow_0)
        {
            this.protocolDrData_0 = protocolDrData_1;
            base.InitData(protocolDrData_1.SourceData, dataRow_0);
        }

        public void Get2DayDistanceUpData(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "2天里程查询应答";
            dataRow_0["Describe"] = this.method_2();
        }

        public void Get2DaySpeedUpData(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "采集2天行驶速度数据应答";
            string str = "时间：";
            string str2 = "速度：";
            string str3 = "";
            string str4 = this.protocolDrData_0.ContextData.Substring(2);
            while (str4.Length > 0)
            {
                string str6 = str3;
                str3 = str6 + str + NumHelper.GetBCDDataTime(str4.Substring(0, 10)) + Environment.NewLine + str2;
                for (string str5 = str4.Substring(10, 120); str5.Length > 0; str5 = str5.Substring(2))
                {
                    str3 = str3 + NumHelper.Convert16To10(str5.Substring(0, 2)) + ",";
                }
                str4 = str4.Substring(130);
                str3 = str3.Trim(new char[] { ',' }) + Environment.NewLine;
            }
            dataRow_0["Describe"] = str3;
        }

        public void Get360DistanceUpData(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "360小时里程查询应答";
            double num = int.Parse(this.protocolDrData_0.ContextData.Substring(2, 6)) * 0.1;
            string bCDDataTime = NumHelper.GetBCDDataTime(this.protocolDrData_0.ContextData.Substring(8));
            dataRow_0["Describe"] = "里程：" + num.ToString() + "公里,时间：" + bCDDataTime;
        }

        public void Get360SpeedUpData(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "采集360小时行驶速度数据应答";
            string str = "时间：";
            string str2 = "速度：";
            string str3 = "";
            string str4 = this.protocolDrData_0.ContextData.Substring(2);
            while (str4.Length > 0)
            {
                string str6 = str3;
                str3 = str6 + str + NumHelper.GetBCDDataTime(str4.Substring(0, 10)) + Environment.NewLine + str2;
                for (string str5 = str4.Substring(10, 120); str5.Length > 0; str5 = str5.Substring(2))
                {
                    str3 = str3 + NumHelper.Convert16To10(str5.Substring(0, 2)) + ",";
                }
                str4 = str4.Substring(130);
                str3 = str3.Trim(new char[] { ',' }) + Environment.NewLine;
            }
            dataRow_0["Describe"] = str3;
        }

        public void GetAccidentReportData(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "事件报告应答";
            int num = NumHelper.Convert16To10(this.protocolDrData_0.ContextData);
            dataRow_0["Describe"] = string.Concat(new object[] { "事件ID：", num, ",事件内容：", UpdataInsertDBInfor.GetEventName(num.ToString()) });
        }

        public void GetApplyforExchangePosInfor(DataRow dataRow_0)
        {
            string contextData = this.protocolDrData_0.ContextData;
            dataRow_0["msgType"] = "4902";
            dataRow_0["OrderName"] = "申请交换指定车辆定位信息应答";
            dataRow_0["Describe"] = this.method_9(contextData);
            dataRow_0["simnum"].ToString();
            dataRow_0["OrderType"] = "接收";
        }

        public void GetAskData(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "提问消息应答";
            dataRow_0["Describe"] = "答案ID：" + NumHelper.Convert16To10(this.protocolDrData_0.ContextData);
        }

        public void GetCancelExchangePosInfor(DataRow dataRow_0)
        {
            string contextData = this.protocolDrData_0.ContextData;
            dataRow_0["msgType"] = "4903";
            dataRow_0["OrderName"] = "取消交换指定车辆定位信息应答";
            dataRow_0["Describe"] = this.method_10(contextData);
            dataRow_0["CarNum"] = "";
            dataRow_0["OrderType"] = "接收";
        }

        public void GetCar2DayOverSpeedUpData(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "查询2个日历天内超时驾驶情况应答";
            string str = this.protocolDrData_0.ContextData.Substring(2);
            string str2 = "";
            while (str.Length > 0)
            {
                DataRow row = dataRow_0;
                string str3 = NumHelper.GetStringFromBase16ASCII(str.Substring(0, 0x24));
                string bCDDataTime = NumHelper.GetBCDDataTime(str.Substring(0x24, 10));
                string str5 = NumHelper.GetBCDDataTime(str.Substring(0x2e, 10));
                string str6 = str2;
                str2 = str6 + "驾驶证号:" + str3.Replace("\0", "") + ", 开始时间:" + bCDDataTime + " 结束时间:" + str5 + ";";
                object obj2 = row["Describe"];
                (row = dataRow_0)["Describe"] = string.Concat(new object[] { obj2, "驾驶证号:", str3.Replace("\0", ""), ", 开始时间:", bCDDataTime, " 结束时间:", str5, ";" });
                str = str.Substring(0x38);
            }
            dataRow_0["Describe"] = str2;
        }

        public void GetCarDoubtfulDataUpData(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "事故疑点数据应答";
            string str = " " + Environment.NewLine;
            SortedDictionary<DateTime, string> dictionary = new SortedDictionary<DateTime, string>();
            string str2 = string.Empty;
            string str3 = string.Empty;
            string str4 = this.protocolDrData_0.ContextData.Substring(2);
            string str5 = "";
            while (str4.Length > 0)
            {
                SortedDictionary<DateTime, string> dictionary3;
                DateTime time3;
                DateTime time = Convert.ToDateTime(NumHelper.GetBCDDataTime(str4.Substring(0, 12)));
                dictionary[time] = "";
                for (string str6 = str4.Substring(12, 400); str6.Length > 0; str6 = str6.Substring(40))
                {
                    SortedDictionary<DateTime, string> dictionary2 = dictionary;
                    DateTime time2 = time;
                    string str7 = str6.Substring(0, 40);
                    str2 = "";
                    str3 = "";
                    while (str7.Length > 0)
                    {
                        str2 = str2 + NumHelper.Convert16To10ToString(str7.Substring(0, 2)) + ",";
                        str3 = str3 + (((NumHelper.Convert16To10(str7.Substring(2, 2)) & 0x80) == 0) ? "关," : "开,");
                        str7 = str7.Substring(4);
                    }
                    string str8 = dictionary2[time2];
                    (dictionary2 = dictionary)[time2 = time] = str8 + "速度:" + str2.Trim(new char[] { ',' }) + str + "制动:" + str3.Trim(new char[] { ',' }) + str;
                }
                (dictionary3 = dictionary)[time3 = time] = dictionary3[time3] + str;
                str4 = str4.Substring(0x19c);
            }
            int num = 0;
            foreach (KeyValuePair<DateTime, string> pair in dictionary)
            {
                num++;
                object obj2 = str5;
                str5 = string.Concat(new object[] { obj2, "事故疑点", num, ":\n时间:", pair.Key.ToString(), "\n" });
                str5 = str5 + pair.Value;
            }
            dataRow_0["Describe"] = str5;
        }

        public void GetCarDriverData(DataRow dataRow_0)
        {
            string contextData = this.protocolDrData_0.ContextData;
            try
            {
                int num = NumHelper.ConvertToInt(contextData.Substring(0, 2));
                if ((num != 1) && (num != 2))
                {
                    throw new Exception();
                }
                string str2 = (num == 1) ? "插卡" : "拔卡";
                DateTime time = DateTime.Parse(NumHelper.GetBCDDataTime(contextData.Substring(2, 12)));
                int num2 = 0;
                string str3 = "";
                int length = 0;
                string str4 = "";
                string str5 = "";
                int num4 = 0;
                string str6 = "";
                DateTime time2 = Convert.ToDateTime("1970-01-01");
                string format = "";
                if (num == 1)
                {
                    num2 = NumHelper.ConvertToInt(contextData.Substring(14, 2));
                    if ((num2 > 3) || (num2 < 0))
                    {
                        throw new Exception();
                    }
                    switch (num2)
                    {
                        case 0:
                            str3 = "IC卡读卡成功";
                            break;

                        case 1:
                            str3 = "读卡失败，原因为卡片密钥认证未通过";
                            break;

                        case 2:
                            str3 = "读卡失败，原因为卡片已被锁定";
                            break;

                        case 3:
                            str3 = "读卡失败，原因为卡内信息为空";
                            break;
                    }
                    if (num2 == 0)
                    {
                        length = NumHelper.ConvertToInt(contextData.Substring(0x10, 2)) * 2;
                        str4 = NumHelper.GetStringFromBase16ASCII(contextData.Substring(0x12, length));
                        str5 = NumHelper.GetStringFromBase16ASCII(contextData.Substring(0x12 + length, 40));
                        num4 = NumHelper.Convert16To10(contextData.Substring(0x3a + length, 2)) * 2;
                        str6 = NumHelper.GetStringFromBase16ASCII(contextData.Substring(60 + length, num4));
                        time2 = DateTime.Parse(NumHelper.GetBCDDate(contextData.Substring((60 + length) + num4, 8)));
                        format = "驾驶员姓名：{0},状态：{6},时间：{5},IC卡读取结果：{1},从业资格证：{2},发证机构名称：{3},证件有效期：{4}";
                        format = string.Format(format, new object[] { str4, str3, str5, str6, time2.ToString("yyyy-MM-dd"), time, str2 });
                    }
                    else
                    {
                        format = "驾驶员姓名：{0},状态：{3},时间：{2},IC卡读取结果：{1}";
                        format = string.Format(format, new object[] { str4, str3, time, str2 });
                    }
                    format = format.Replace("\0", "");
                }
                else
                {
                    format = "状态：{0},时间：{1}";
                    format = string.Format(format, str2, time);
                }
                dataRow_0["OrderName"] = "驾驶员身份信息应答";
                dataRow_0["Describe"] = format;
                int num5 = (dataRow_0["CarId"] == DBNull.Value) ? 0 : int.Parse(dataRow_0["CarId"].ToString());
                int num6 = UpdataInsertDBInfor.UpdateDriverInfor(num5, num, time, num2, str4.Replace("\0", ""), str5.Replace("\0", ""), str6, time2);
                string str8 = "";
                if (num == 1)
                {
                    str8 = "驾驶员登陆成功";
                    if (num2 != 0)
                    {
                        str8 = "驾驶员登录失败," + str3;
                    }
                    else if (num6 == 0)
                    {
                        str8 = "驾驶员登录失败,原因为驾驶员身份信息无效";
                    }
                }
                else
                {
                    str8 = "驾驶员取卡成功";
                }
                IODeviceTextMsg msg = new IODeviceTextMsg {
                    InfoID = 1,
                    Message = str8,
                    SimNum = dataRow_0["SimNum"].ToString()
                };
                this.IODeviceAttachInfo.Add(msg);
            }
            catch (Exception)
            {
                int startIndex = 0;
                int num8 = NumHelper.Convert16To10(contextData.Substring(0, 2)) * 2;
                string str9 = NumHelper.GetStringFromBase16ASCII(contextData.Substring(2, num8));
                string str10 = NumHelper.GetStringFromBase16ASCII(contextData.Substring(2 + num8, 40));
                startIndex = (2 + num8) + 40;
                string str11 = NumHelper.GetStringFromBase16ASCII(contextData.Substring(startIndex, 80));
                startIndex += 80;
                int num9 = NumHelper.Convert16To10(contextData.Substring(startIndex, 2)) * 2;
                startIndex += 2;
                string str12 = NumHelper.GetStringFromBase16ASCII(contextData.Substring(startIndex, num9));
                string str13 = "驾驶员姓名：{0},驾驶员身份证编码：{1},从业资格证：{2},发证机构名称：{3}";
                str13 = string.Format(str13, new object[] { str9, str10, str11, str12 }).Replace("\0", "");
                dataRow_0["OrderName"] = "驾驶员身份信息应答";
                dataRow_0["Describe"] = str13;
                int num10 = (dataRow_0["CarId"] == DBNull.Value) ? 0 : int.Parse(dataRow_0["CarId"].ToString());
                if (UpdataInsertDBInfor.UpdateDriverInfor(num10, str10.Replace("\0", ""), str11.Replace("\0", ""), str9.Replace("\0", ""), str12.Replace("\0", "")) == 0)
                {
                    IODeviceTextMsg msg3 = new IODeviceTextMsg {
                        InfoID = 1,
                        Message = "驾驶员身份信息无效",
                        SimNum = dataRow_0["SimNum"].ToString()
                    };
                    this.IODeviceAttachInfo.Add(msg3);
                }
            }
        }

        public void GetCarPostionMessageRequestUpData(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "启动车辆定位信息交换请求";
            dataRow_0["Describe"] = this.method_5(Convert.ToInt16(this.protocolDrData_0.ContextData));
            dataRow_0["OrderType"] = "接收";
        }

        public void GetCarRemotingParameterUpData(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "采集记录仪中的车辆特征系数远程参数查看应答";
            dataRow_0["Describe"] = "特征系数：" + NumHelper.Convert16To10(this.protocolDrData_0.ContextData.Substring(2));
        }

        public void GetCarStopPostionMessageRequestUpData(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "结束车辆定位信息交换请求";
            dataRow_0["Describe"] = this.method_6(Convert.ToInt16(this.protocolDrData_0.ContextData));
            dataRow_0["OrderType"] = "接收";
        }

        public void GetChangeAlarmRequestUpData(DataRow dataRow_0)
        {
            AlarmHandler handler = new AlarmHandler(this.protocolDrData_0.ContextData);
            dataRow_0["OrderName"] = "交换警情信息消息";
            dataRow_0["Describe"] = handler.GetPreAlarmMessage();
            dataRow_0["OrderType"] = "接收";
        }

        public void GetDirectionRequestUpData(DataRow dataRow_0)
        {
            AlarmHandler handler = new AlarmHandler(this.protocolDrData_0.ContextData);
            dataRow_0["msgType"] = "4354";
            dataRow_0["OrderName"] = "报警督办请求";
            dataRow_0["Describe"] = handler.GetAlarmDirectionMessage();
            dataRow_0["OrderType"] = "接收";
        }

        public void GetDownPlatRequestUpData(DataRow dataRow_0)
        {
            string str = NumHelper.Convert16To10ToString(this.protocolDrData_0.ContextData.Substring(0, 2));
            string str2 = NumHelper.GetStringFromBase16ASCII(this.protocolDrData_0.ContextData.Substring(2, 0x18)).Replace("\0", "");
            string str3 = NumHelper.Convert16To10ToString(this.protocolDrData_0.ContextData.Substring(0x1a, 8));
            string s = NumHelper.Convert16To10ToString(this.protocolDrData_0.ContextData.Substring(0x22, 8));
            string str5 = NumHelper.GetStringFromBase16ASCII(this.protocolDrData_0.ContextData.Substring(0x2a, int.Parse(s) * 2)).Replace("\0", "");
            dataRow_0["OrderName"] = "下发平台间报文请求";
            dataRow_0["CarNum"] = "";
            dataRow_0["msgType"] = "4911";
            dataRow_0["Describe"] = "消息ID：" + str3 + ",消息内容：" + str5.Replace(",", "，") + ",查岗对象类型：" + str + ",查岗对象的ID：" + str2;
            dataRow_0["OrderType"] = "接收";
            dataRow_0["OBJECT_TYPE"] = str;
            dataRow_0["OBJECT_ID"] = str2;
        }

        public void GetDriverInfomationUpData(DataRow dataRow_0)
        {
            string str2 = NumHelper.Convert16To10ToString(this.protocolDrData_0.ContextData.Substring(2, 6));
            string str3 = NumHelper.GetStringFromBase16ASCII(this.protocolDrData_0.ContextData.Substring(8, 0x24)).Replace("\0", "");
            dataRow_0["OrderName"] = "驾驶员身份数据应答";
            dataRow_0["Describe"] = "驾驶员编号：" + str2 + ",机动车驾驶证号：" + str3;
        }

        public void GetEletroryRecordReportData(DataRow dataRow_0)
        {
            int length = NumHelper.Convert16To10(this.protocolDrData_0.ContextData.Substring(0, 8)) * 2;
            string str = NumHelper.GetStringFromBase16ASCII(this.protocolDrData_0.ContextData.Substring(8, length));
            dataRow_0["OrderName"] = "电子运单上报信息应答";
            dataRow_0["Describe"] = "电子运单内容：" + str;
        }

        public void GetMessagePlayData(DataRow dataRow_0)
        {
            string str = this.protocolDrData_0.ContextData.Substring(0, 2);
            int num = NumHelper.Convert16To10(this.protocolDrData_0.ContextData.Substring(2));
            string str3 = "消息ID：0x" + str + ",消息内容:" + UpdataInsertDBInfor.GetMenuName(str);
            if (num == 0)
            {
                dataRow_0["OrderName"] = "信息点播/取消应答";
                str3 = str3 + ",消息类型：取消";
            }
            else
            {
                dataRow_0["OrderName"] = "信息点播/取消应答";
                str3 = str3 + ",消息类型：点播";
            }
            dataRow_0["Describe"] = str3;
            UpdataInsertDBInfor.UpdateMenuInfor(((dataRow_0[14] == DBNull.Value) ? "0" : dataRow_0["SimNum"].ToString()).ToString(), "2", NumHelper.Convert16To10ToString(str), num != 0);
        }

        public void GetMutilMediaAccidentData(DataRow dataRow_0)
        {
            string str = this.method_0();
            dataRow_0["OrderName"] = "多媒体事件信息上传数据应答";
            dataRow_0["Describe"] = str;
        }

        public void GetPlatLinkUpData(DataRow dataRow_0)
        {
            string contextData = this.protocolDrData_0.ContextData;
            dataRow_0["msgType"] = "4355";
            dataRow_0["OrderName"] = "平台连线消息";
            dataRow_0["Describe"] = this.method_7(contextData);
            dataRow_0["CarNum"] = "";
            dataRow_0["OrderType"] = "接收";
        }


        /// <summary>
        /// 平台查岗请求处理
        /// </summary>
        /// <param name="dr"></param>
        public void GetPlatRequestUpData(DataRow dr)
        {
            //对应存储过程WebGpsClient_GetOutEquipmentData返回列propertyData除掉前4个字符，定义见Protocol.ProtocolDrData
            string contextData = this.protocolDrData_0.ContextData;
            //查岗对象类型 OBJECT_TYPE, 重庆运管定义1为自动查岗，2为手动查岗
            string sObjectType = NumHelper.Convert16To10ToString(contextData.Substring(0, 2));
            //重庆运管要求:
            //OBJECT_TYPE 为1 时OBJECT_ID 为查岗对象ID, 值为5000000+ 平台接入码( 行政区划码+ 平台唯一编码)
            //当OBJECT_TYPE 为2,OBJECT_ID 为业户经营许可证号;
            //当OBJECT_TYPE为3 时,OBJECT_ID值为5000000+平台接入码(行政区划码+平台唯一编码),例如50000008960
            string sObjectId = NumHelper.GetStringFromBase16ASCII(contextData.Substring(2, 24)).Replace("\0", "");
            //信息ID
            string sIndex = NumHelper.Convert16To10ToString(contextData.Substring(26, 8));
            //内容长度
            string sLen = NumHelper.Convert16To10ToString(contextData.Substring(34, 8));
            //取得内容
            string sContant = NumHelper.GetStringFromBase16ASCII(contextData.Substring(42, int.Parse(sLen) * 2)).Replace("\0", "");
            string userIdByRegionCode = string.Empty;
            string workIdByUserId = string.Empty;
            if (sObjectType.Equals("1"))
            {
                if (!string.IsNullOrEmpty(sContant) && (sContant.IndexOf("请回复当前全部在线业户") != -1))
                {
                    workIdByUserId = "-1";
                    string str10 = this.method_3(sObjectType, sObjectId, sIndex, "当前在线业户列表:=" + AskConfigerParameter.GetAutoRelyInfoString());
                    try
                    {
                        AskConfigerParameter.InsertCommandParameterToDB("8613489997299", 999, 16900, str10);
                        //修改如果自动查岗，则不发送消息到客户端 huzh 2014.1.22
                        workIdByUserId = "-2";
                        dr["WrkId"] = workIdByUserId;
                        return;
                        //修改如果为重庆运管自动查岗则不需要发送到客户端,by huzh 2014.1.8
                        //goto Label_0147;
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("自动回答发生错误：" + exception.Message + exception.StackTrace);
                    }
                }
                userIdByRegionCode = AskConfigerParameter.GetUserIdByRegionCode(sObjectId);
            }
            else if (sObjectType.Equals("2"))
            {
                if (!string.IsNullOrEmpty(sContant) && (sContant.IndexOf("OBJECT_ID:=") != -1))
                {
                    //重庆运管要求，由于营运证长度最长为15位，因此INFO_CONTENT内容格式改为：
                    //OBJECT_ID:=500101000001-3;手工查岗问题及答案:=问题|1,答案1;2,答案2;3,答案3;4,答案4
                    //因此替换查岗对象
                    string[] array = sContant.Split(new char[] { ';' });
                    sObjectId = array[0].Substring(array[0].IndexOf('=')+1);
                }
                userIdByRegionCode = AskConfigerParameter.GetUserIdByYYZCarId(sObjectId);
            }
            else if (sObjectType.Equals("3"))
            {
                workIdByUserId = "0";
            }
        //Label_0147:
            if (string.IsNullOrEmpty(workIdByUserId) && !string.IsNullOrEmpty(userIdByRegionCode))
            {
                workIdByUserId = AskConfigerParameter.GetWorkIdByUserId(userIdByRegionCode);
            }
            if (string.IsNullOrEmpty(workIdByUserId))
            {
                workIdByUserId = "-1";
            }
            dr["WrkId"] = workIdByUserId;
            dr["msgType"] = "4353";
            dr["CarNum"] = "";
            dr["OrderName"] = "平台查岗请求";
            dr["Describe"] = "消息ID：" + sIndex + ",消息内容：" + sContant.Replace(",", "，") + ",查岗对象类型：" + sObjectType + ",查岗对象的ID：" + sObjectId;
            dr["OrderType"] = "接收";
            LogMsg msg = new LogMsg("JTBDataParse", "GetPlatRequestUpData", dr["Describe"].ToString() + " workIdByUserId: " + workIdByUserId + " userIdByRegionCode: " + userIdByRegionCode);
            new LogHelper().WriteLog(msg);
        }

        public void GetRemotingParameterAboutVINUpData(DataRow dataRow_0)
        {
            string str = NumHelper.GetStringFromBase16ASCII(this.protocolDrData_0.ContextData.Substring(2, 0x22)).Replace("\0", "");
            string str2 = NumHelper.GetStringFromBase16ASCII(this.protocolDrData_0.ContextData.Substring(0x24, 0x18)).Replace("\0", "");
            string str3 = NumHelper.GetStringFromBase16ASCII(this.protocolDrData_0.ContextData.Substring(60)).Replace("\0", "");
            dataRow_0["OrderName"] = "采集车辆VIN号车牌号码车牌分类应答";
            dataRow_0["Describe"] = "VIN号码:" + str + ",车牌号码:" + str2 + ",车牌分类：" + str3;
        }

        public void GetRemotingParameterUpData(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "远程参数查看应答";
            dataRow_0["Describe"] = NumHelper.GetBCDDataTime(this.protocolDrData_0.ContextData.Substring(2));
        }

        public void GetRreAlarmRequestUpData(DataRow dataRow_0)
        {
            AlarmHandler handler = new AlarmHandler(this.protocolDrData_0.ContextData);
            dataRow_0["OrderName"] = "报警预警消息";
            dataRow_0["Describe"] = handler.GetPreAlarmMessage();
        }

        public void GetSendPosInfor(DataRow dataRow_0)
        {
            string contextData = this.protocolDrData_0.ContextData;
            dataRow_0["msgType"] = "4901";
            dataRow_0["OrderName"] = "补发车辆定位信息应答";
            dataRow_0["Describe"] = this.method_8(contextData);
            dataRow_0["CarNum"] = "";
            dataRow_0["OrderType"] = "接收";
        }

        public void GetStoreMutilMediaUpData(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "存储多媒体数据检索应答";
            dataRow_0["Describe"] = this.method_1();
        }

        public void GetTransportDataUpData(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "终端数据透传";
            dataRow_0["Describe"] = "透传消息类型：" + this.protocolDrData_0.ContextData.Substring(0, 2) + ",透传数据：" + NumHelper.GetStringFromBase16ASCII(this.protocolDrData_0.ContextData.Substring(2));
        }

        public void GetZipDataUpData(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "压缩数据";
            int num = NumHelper.Convert16To10(this.protocolDrData_0.ContextData.Substring(0, 8));
            if (num == 0)
            {
                dataRow_0["Describe"] = "无数据";
            }
            else
            {
                string str = NumHelper.Convert16To10ToString(this.protocolDrData_0.ContextData.Substring(8, num * 2));
                dataRow_0["Describe"] = str;
            }
        }

        private string method_0()
        {
            NumHelper.Convert16To10ToString(this.protocolDrData_0.ContextData.Substring(0, 8));
            int num = NumHelper.Convert16To10(this.protocolDrData_0.ContextData.Substring(8, 2));
            string str = "多媒体类型：" + ((MenuDefine.MutilMediaType) num).ToString();
            int num2 = NumHelper.Convert16To10(this.protocolDrData_0.ContextData.Substring(10, 2));
            str = str + "，多媒体格式编码：" + ((MenuDefine.MutilMediaFormat) num2).ToString();
            int num3 = NumHelper.Convert16To10(this.protocolDrData_0.ContextData.Substring(12, 2));
            return ((str + "，事件现编码：" + ((MenuDefine.AccidentCode) num3).ToString()) + "，通道ID：" + NumHelper.Convert16To10ToString(this.protocolDrData_0.ContextData.Substring(14)));
        }

        private string method_1()
        {
            string str = string.Empty;
            int num = NumHelper.Convert16To10(this.protocolDrData_0.ContextData.Substring(0, 4));
            if (num == 0)
            {
                return (str + "多媒体总数:0");
            }
            object obj2 = str;
            str = string.Concat(new object[] { obj2, "多媒体总数：", num, Environment.NewLine });
            string str2 = this.protocolDrData_0.ContextData.Substring(4);
            int num2 = str2.Length / 70;
            int startIndex = 0;
            for (int i = 0; i < num2; i++)
            {
                str = str + "多媒体ID：" + NumHelper.Convert16To10ToString(str2.Substring(startIndex, 8));
                startIndex += 8;
                str = str + ",多媒体类型：" + ((MenuDefine.MutilMediaType) NumHelper.Convert16To10(str2.Substring(startIndex, 2))).ToString();
                startIndex += 2;
                str = str + ",通道ID：" + NumHelper.Convert16To10ToString(str2.Substring(startIndex, 2));
                startIndex += 2;
                str = str + ",事件项编码：" + ((MenuDefine.AccidentCode) NumHelper.Convert16To10(str2.Substring(startIndex, 2))).ToString();
                startIndex += 2;
                str = str + ",时间：" + NumHelper.GetBCDDataTime(str2.Substring(startIndex, 0x38).Substring(0x2c, 12)) + Environment.NewLine;
                startIndex += 0x38;
            }
            return str;
        }

        private string method_10(string string_0)
        {
            string str = string.Empty;
            int num = NumHelper.Convert16To10(this.protocolDrData_0.ContextData.Substring(0, 2));
            string str2 = string.Empty;
            switch (num)
            {
                case 0:
                    str2 = "取消申请成功";
                    break;

                case 1:
                    str2 = "之前没有对应申请信息";
                    break;

                case 2:
                    str2 = "其他";
                    break;
            }
            return (str + "取消交换指定车辆定位信息应答结果:" + str2);
        }

        private string method_2()
        {
            int num = int.Parse(this.protocolDrData_0.ContextData.Substring(2, 6));
            string bCDDataTime = NumHelper.GetBCDDataTime(this.protocolDrData_0.ContextData.Substring(8));
            return string.Concat(new object[] { "行驶里程：", num * 0.1, "公里,时间：", bCDDataTime });
        }

        private string method_3(string string_0, string string_1, string string_2, string string_3)
        {
            string str = Convert.ToString(int.Parse(string_0), 0x10).PadLeft(2, '0');
            string str2 = this.method_4(string_1);
            str = str + str2.PadRight(0x18, '0') + Convert.ToString(int.Parse(string_2), 0x10).PadLeft(8, '0');
            string str4 = Convert.ToString(Encoding.GetEncoding("gb2312").GetBytes(string_3).Length, 0x10).PadLeft(8, '0') + this.method_4(string_3);
            return (str + str4);
        }

        private string method_4(string string_0)
        {
            StringBuilder builder = new StringBuilder();
            byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(string_0);
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("X2"));
            }
            return builder.ToString();
        }

        private string method_5(int int_0)
        {
            string str = string.Empty;
            switch (int_0)
            {
                case 0:
                    return "车辆进入指定区域";

                case 1:
                    return "人工指定交换";

                case 2:
                    return "应急状态下车辆定位信息回传";

                case 3:
                    return "其它原因";
            }
            return str;
        }

        private string method_6(int int_0)
        {
            string str = string.Empty;
            switch (int_0)
            {
                case 0:
                    return "车辆离开指定区域";

                case 1:
                    return "人工停止交换";

                case 2:
                    return "紧急监控完成";

                case 3:
                    return "车辆离线";

                case 4:
                    return "其它原因";
            }
            return str;
        }

        private string method_7(string string_0)
        {
            int num = NumHelper.Convert16To10(string_0);
            string str = string.Empty;
            switch (num)
            {
                case 1:
                    return "与上级监管平台主链路断开";

                case 2:
                    return "与上级监管平台主链路正常";

                case 3:
                    return "与上级监管平台从链路断开";

                case 4:
                    return "与上级监管平台从链路正常";
            }
            return str;
        }

        private string method_8(string string_0)
        {
            string str = string.Empty;
            int num = NumHelper.Convert16To10(this.protocolDrData_0.ContextData.Substring(0, 2));
            string str2 = string.Empty;
            switch (num)
            {
                case 0:
                    str2 = "成功，上级平台即刻补发";
                    break;

                case 1:
                    str2 = "成功，上级平台即期补发";
                    break;

                case 2:
                    str2 = "失败，上级平台无对应申请定位数据";
                    break;

                case 3:
                    str2 = "失败，申请内容不准确";
                    break;

                case 4:
                    str2 = "其他原因";
                    break;
            }
            return (str + "补发车辆定位信息应答结果:" + str2);
        }

        private string method_9(string string_0)
        {
            string str = string.Empty;
            int num = NumHelper.Convert16To10(this.protocolDrData_0.ContextData.Substring(0, 2));
            string str2 = string.Empty;
            switch (num)
            {
                case 0:
                    str2 = "申请成功";
                    break;

                case 1:
                    str2 = "上级平台没有该车数据";
                    break;

                case 2:
                    str2 = "申请时间段错误";
                    break;

                case 3:
                    str2 = "其它";
                    break;
            }
            return (str + "申请交换指定车辆定位信息应答结果:" + str2);
        }

        public UpdataIODeviceAttachInfo IODeviceAttachInfo
        {
            [CompilerGenerated]
            get
            {
                return this.updataIODeviceAttachInfo_0;
            }
            [CompilerGenerated]
            set
            {
                this.updataIODeviceAttachInfo_0 = value;
            }
        }
    }
}

