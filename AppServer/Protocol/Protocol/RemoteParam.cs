namespace Protocol
{
    using System;
    using System.Collections.Generic;
    using Library;

    public class RemoteParam
    {
        private static SortedList<int, string> _RemoteList;

        static RemoteParam()
        {
            old_acctor_mc();
        }

        private static string FilterZeroData(string string_0)
        {
            int startIndex = 0;
            string str = string.Empty;
            for (int i = 0; i <= (string_0.Length - 1); i++)
            {
                str = string_0.Substring(i, 1);
                if (!"0".Equals(str))
                {
                    startIndex = i;
                    break;
                }
            }
            return string_0.Substring(startIndex);
        }

        private static string GetContextDesc(int int_0, string string_0)
        {
            string gpsOff = string.Empty;
            int num = int_0;
            switch (num)
            {
                case 1:
                case 2:
                case 3:
                case 5:
                case 6:
                case 7:
                case 0x35:
                    return NumHelper.GetStringFromBase16ASCII(string_0);

                case 4:
                    return NumHelper.ConvertToInt(string_0).ToString();

                case 8:
                case 9:
                    return string_0;

                case 10:
                case 11:
                    return GetIpAndPort(string_0);

                case 12:
                case 13:
                case 14:
                    return NumHelper.GetStringFromBase16ASCII(FilterZeroData(string_0));

                case 50:
                    return string_0;

                case 0x33:
                    return GetSpeedType(string_0);

                case 0x34:
                    return (GetSpeedType(string_0) + "k/m");
            }
            if (num == 240)
            {
                gpsOff = GetGpsOff(string_0);
            }
            return gpsOff;
        }

        private static string GetGpsOff(string string_0)
        {
            string str = "不上传";
            if (NumHelper.ConvertToInt(string_0) == 1)
            {
                str = "上传";
            }
            return str;
        }

        private static string GetIpAndPort(string string_0)
        {
            return (("IP:" + NumHelper.GetIPFrom16to10(string_0.Substring(0, 8)) + ",") + "Port:" + NumHelper.Convert16To10(string_0.Substring(8)).ToString());
        }

        public static string GetParamDesc(int int_0, string string_0)
        {
            string str = string.Empty;
            if ((_RemoteList != null) && _RemoteList.ContainsKey(int_0))
            {
                str = ("参数类型:" + _RemoteList[int_0] + ",") + "参数内容:" + GetContextDesc(int_0, string_0);
            }
            return str;
        }

        private static string GetSpeedType(string string_0)
        {
            string str = "传感器速度";
            if (NumHelper.ConvertToInt(string_0) == 1)
            {
                str = "gps速度";
            }
            return str;
        }

        private static void old_acctor_mc()
        {
            _RemoteList = new SortedList<int, string>(100);
            _RemoteList.Add(1, "车辆识别代号");
            _RemoteList.Add(2, "车牌号码");
            _RemoteList.Add(3, "车牌分类");
            _RemoteList.Add(4, "驾驶员代码");
            _RemoteList.Add(5, "驾驶证证号");
            _RemoteList.Add(6, "MDT 主机ID");
            _RemoteList.Add(7, "固件版本号");
            _RemoteList.Add(8, "初次安装日期");
            _RemoteList.Add(9, "实时时钟");
            _RemoteList.Add(10, "运营管理中心主IP 地址、端口");
            _RemoteList.Add(11, "运营管理中心备用IP地址、端口");
            _RemoteList.Add(12, "短消息服务中心号码");
            _RemoteList.Add(13, "运营管理中心短消息服务号码一");
            _RemoteList.Add(14, "运营管理中心短消息服务号码二");
            _RemoteList.Add(50, "车辆特征系数");
            _RemoteList.Add(0x33, "速度类型设置");
            _RemoteList.Add(0x34, "超速限速值");
            _RemoteList.Add(0x35, "监听号码");
            _RemoteList.Add(0x36, "是否上传GPS");
            _RemoteList.Add(240, "是否上传GPS");
        }
    }
}

