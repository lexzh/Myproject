namespace Bussiness
{
    using System;
    using System.Collections;

    public class ReportTypeParam
    {
        private static Hashtable _CurrentMessageTypeList;
        private static Hashtable _MessageTypeList;

        static ReportTypeParam()
        {
            _CurrentMessageTypeList = new Hashtable(17);
            _MessageTypeList = new Hashtable(17);
        }

        public static string GetCurrentMessType(int int_0)
        {
            if (_CurrentMessageTypeList.Count <= 0)
            {
                _CurrentMessageTypeList.Add(0x28d, "定时压缩报文");
                _CurrentMessageTypeList.Add(0x28e, "定距压缩报文");
                _CurrentMessageTypeList.Add(0x28f, "定次压缩报文");
                _CurrentMessageTypeList.Add(0x1bd, "定次报文");
                _CurrentMessageTypeList.Add(0x10000, "定次报文");
                _CurrentMessageTypeList.Add(0x28b, "定次报文");
                _CurrentMessageTypeList.Add(650, "定时报文");
                _CurrentMessageTypeList.Add(0x981, "摄像头信息报文");
                _CurrentMessageTypeList.Add(0x281, "定时报文");
                _CurrentMessageTypeList.Add(0x282, "定距报文");
                _CurrentMessageTypeList.Add(0x283, "定次报文");
                _CurrentMessageTypeList.Add(0x287, "定时报文");
                _CurrentMessageTypeList.Add(0x289, "定次报文");
                _CurrentMessageTypeList.Add(0x482, "报警报文");
                _CurrentMessageTypeList.Add(0x4900, "跨区域位置报文");
                _MessageTypeList.Add(0x292, "基站定位报文");
            }
            if (!_CurrentMessageTypeList.ContainsKey(int_0))
            {
                if (int_0 > 0xffff)
                {
                    return "外设报文";
                }
                return "未知报文";
            }
            return (string) _CurrentMessageTypeList[int_0];
        }

        public static string GetMessType(int type)
        {
            if (_MessageTypeList.Count <= 0)
            {
                _MessageTypeList.Add(0x28d, "定时压缩报文");
                _MessageTypeList.Add(0x28e, "定距压缩报文");
                _MessageTypeList.Add(0x28f, "定次压缩报文");
                _MessageTypeList.Add(0x1bd, "设置车台总里程报文");
                _MessageTypeList.Add(0x10000, "公交报文");
                _MessageTypeList.Add(0x28b, "黑匣子报文");
                _MessageTypeList.Add(650, "出租车监控报文");
                _MessageTypeList.Add(0x981, "定时报文");
                _MessageTypeList.Add(0x281, "定时报文");
                _MessageTypeList.Add(0x282, "定距报文");
                _MessageTypeList.Add(0x283, "定次报文");
                _MessageTypeList.Add(0x287, "轮询报文");
                _MessageTypeList.Add(0x289, "签到报文");
                _MessageTypeList.Add(0x482, "定次报文");
                _CurrentMessageTypeList.Add(0x4900, "跨区域位置报文");
                _MessageTypeList.Add(0x292, "基站定位报文");
            }
            if (!_MessageTypeList.ContainsKey(type))
            {
                if (type > 0xffff)
                {
                    return "外设报文";
                }
                return "未知报文";
            }
            return (string) _MessageTypeList[type];
        }

    }
}

