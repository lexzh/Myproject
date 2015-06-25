namespace Bussiness
{
    using System;
    using System.Collections;

    public class RemotingParam
    {
        private static Hashtable _RomotingList;

        static RemotingParam()
        {
            old_acctor_mc();
        }

        public static string GetRespOrderNameName(int int_0)
        {
            if (_RomotingList.Count <= 0)
            {
                _RomotingList.Add(1, "车台远程升级信息");
                _RomotingList.Add(2, "车台软件版本信息");
                _RomotingList.Add(3, "当前温度");
                _RomotingList.Add(0xff, "自定义信息");
                _RomotingList.Add(0x100, "下载服务器端数据");
                _RomotingList.Add(0x1da, "驾培信息查询应答");
            }
            return (string) _RomotingList[int_0];
        }

        private static void old_acctor_mc()
        {
            _RomotingList = new Hashtable(0x11);
        }
    }
}

