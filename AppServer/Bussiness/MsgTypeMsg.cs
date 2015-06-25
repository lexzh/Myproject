namespace Bussiness
{
    using System;

    public class MsgTypeMsg
    {
        public const string AdminRegionAlarm = "行政区域报警";
        public const string Command = "命令发送应答";
        public const string Compress = "压缩定位信息报文";
        public const string CuffTime = "掉线通知";
        public const string Flag = "查询标志位响应";
        public const string RealTime = "实时定位信息报文";
        public const string RemoteUp = "远程升级终端回复";
        public const string SelfTest = "车台自检响应";
        public const string Terminal = "终端应答报文";
    }
}

