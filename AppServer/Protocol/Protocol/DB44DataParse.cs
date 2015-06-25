namespace Protocol
{
    using System;
    using System.Data;
    using Library;

    public class DB44DataParse : Parse
    {
        protected const string _AccidentDesc = "事故疑点应答";
        protected const string _DriverIdDesc = "驾驶员身份功能响应";
        protected const string _DriverStateDesc = "驾驶员状态变化上报";
        protected const string _OverSpeedDesc = "超时驾驶情况查询应答";
        protected const string _ParamLookDesc1 = "定时监控查看响应";
        protected const string _ParamLookDesc2 = "定距监控查看响应";
        protected const string _RemotingParamDesc1 = "远程参数查看响应";
        protected const string _RemotingParamDesc2 = "远程参数设置响应";
        protected const string _RoadLineDesc = "历史轨迹查询应答";
        protected const string _RunDistanceDesc = "累计里程查询应答";
        private ProtocolDrData protocolDrData_0;

        public DB44DataParse(ProtocolDrData protocolDrData_1, DataRow dataRow_0)
        {
            this.protocolDrData_0 = protocolDrData_1;
            base.InitData(protocolDrData_1.SourceData, dataRow_0);
        }

        public void AccidentDataPrase(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "事故疑点应答";
            dataRow_0["Describe"] = "事故疑点应答";
        }

        public void DriverIdDataPrase(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "驾驶员身份功能响应";
            dataRow_0["Describe"] = this.method_3();
        }

        public void DriverStateDataPrase(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "驾驶员状态变化上报";
            dataRow_0["Describe"] = "驾驶员状态变化上报";
        }

        private string method_0()
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(this.protocolDrData_0.ContextData) || (this.protocolDrData_0.ContextData.Length != 10))
            {
                return str;
            }
            string str2 = this.protocolDrData_0.ContextData.Substring(0, 2);
            string str3 = NumHelper.Convert16To10(this.protocolDrData_0.ContextData.Substring(2)).ToString();
            if ("00".Equals(str2))
            {
                str = "两个日历天的累计里程:";
            }
            else
            {
                str = "360小时的累计里程:";
            }
            return (str + str3);
        }

        private string method_1()
        {
            string str2 = this.protocolDrData_0.ContextData.Substring(60);
            object obj2 = "时间间隔:" + NumHelper.Convert16To10(str2.Substring(0, 2)) + ",";
            return (string.Concat(new object[] { obj2, "发送条数:", NumHelper.Convert16To10(str2.Substring(2, 2)), "," }) + "剩余发送包数:" + NumHelper.Convert16To10(str2.Substring(4)));
        }

        private string method_2()
        {
            string str2 = this.protocolDrData_0.ContextData.Substring(60);
            object obj2 = "距离间隔:" + NumHelper.Convert16To10(str2.Substring(0, 4)) + ",";
            return (string.Concat(new object[] { obj2, "发送条数:", NumHelper.Convert16To10(str2.Substring(4, 2)), "," }) + "剩余发送包数:" + NumHelper.Convert16To10(str2.Substring(6)));
        }

        private string method_3()
        {
            string s = this.protocolDrData_0.ContextData.Substring(0, 8);
            object obj2 = "身份信息:" + int.Parse(s).ToString() + ",";
            return string.Concat(new object[] { obj2, "驾驶时间:", NumHelper.Convert16To10(this.protocolDrData_0.ContextData.Substring(8)), "分钟" });
        }

        private string method_4()
        {
            string str2 = this.protocolDrData_0.ContextData.Substring(60);
            int num = NumHelper.Convert16To10(str2.Substring(0, 2));
            string str3 = str2.Substring(2);
            return RemoteParam.GetParamDesc(num, str3);
        }

        public void OverSpeedDataPrase(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "超时驾驶情况查询应答";
            dataRow_0["Describe"] = "超时驾驶情况查询应答";
        }

        public void ParamLookDataPrase1(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "定时监控查看响应";
            if (!string.IsNullOrEmpty(this.protocolDrData_0.ContextData))
            {
                dataRow_0["Describe"] = this.method_1();
            }
        }

        public void ParamLookDataPrase2(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "定距监控查看响应";
            if (!string.IsNullOrEmpty(this.protocolDrData_0.ContextData))
            {
                dataRow_0["Describe"] = this.method_2();
            }
        }

        public void RemotingParamDataPrase1(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "远程参数查看响应";
            if (!string.IsNullOrEmpty(this.protocolDrData_0.ContextData))
            {
                dataRow_0["Describe"] = this.method_4();
            }
        }

        public void RemotingParamDataPrase2(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "远程参数设置响应";
            dataRow_0["Describe"] = "远程参数设置响应";
        }

        public void RoadLineDataPrase(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "历史轨迹查询应答";
            dataRow_0["Describe"] = "历史轨迹查询应答";
        }

        public void RunDistanceDataPrase(DataRow dataRow_0)
        {
            dataRow_0["OrderName"] = "累计里程查询应答";
            if (!string.IsNullOrEmpty(this.protocolDrData_0.ContextData))
            {
                dataRow_0["Describe"] = this.method_0();
            }
        }

        public void SumPriceNanjin(DataRow dataRow_0)
        {
            if (!"3030".Equals(this.protocolDrData_0.ContextData.Substring(10, 4)))
            {
                dataRow_0["OrderResult"] = "失败";
            }
            dataRow_0["OrderName"] = "停机时间命令应答";
            dataRow_0["Describe"] = "";
        }
    }
}

