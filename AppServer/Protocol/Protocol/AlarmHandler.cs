namespace Protocol
{
    using System;
    using Library;

    public class AlarmHandler
    {
        private string string_0;

        public AlarmHandler()
        {
        }

        public AlarmHandler(string context)
        {
            this.string_0 = context;
        }

        public string GetAlarmDirectionMessage()
        {
            if (string.IsNullOrEmpty(this.SourceData))
            {
                return string.Empty;
            }
            string str = string.Empty;
            str = "报警督办ID：" + NumHelper.Convert16To10(this.SourceData.Substring(0x16, 8)) + ",";
            int num = Convert.ToInt16(this.SourceData.Substring(0, 2));
            str = str + "报警信息来源：" + this.method_1(num) + ",";
            int num2 = NumHelper.Convert16To10(this.SourceData.Substring(2, 4));
            str = ((str + "报警类型：" + this.method_2(num2) + ",") + "报警时间：" + NumHelper.ConvertStringToDatetime(this.SourceData.Substring(6, 0x10)) + ",") + "报警截止时间：" + NumHelper.ConvertStringToDatetime(this.SourceData.Substring(30, 0x10)) + ",";
            int num3 = Convert.ToInt16(this.SourceData.Substring(0x2e, 2));
            return ((((str + "督办级别：" + this.method_0(num3) + ",") + "督办人：" + NumHelper.GetStringFromBase16ASCII(this.SourceData.Substring(0x30, 0x20)).Replace("\0", "") + ",") + "督办联系电话：" + NumHelper.GetStringFromBase16ASCII(this.SourceData.Substring(80, 40)).Replace("\0", "") + ",") + "督办人联系电子邮件：" + NumHelper.GetStringFromBase16ASCII(this.SourceData.Substring(120)).Replace("\0", "") + ",");
        }

        public string GetPreAlarmMessage()
        {
            if (string.IsNullOrEmpty(this.SourceData))
            {
                return string.Empty;
            }
            string str = string.Empty;
            int num = Convert.ToInt16(this.SourceData.Substring(0, 2));
            str = "报警信息来源：" + this.method_1(num) + ",";
            int num2 = NumHelper.Convert16To10(this.SourceData.Substring(2, 4));
            return (((str + "报警类型：" + this.method_2(num2) + ",") + "报警时间：" + NumHelper.ConvertStringToDatetime(this.SourceData.Substring(6, 0x10)) + ",") + "警情信息：" + NumHelper.GetStringFromBase16ASCII(this.SourceData.Substring(30)).Replace("\0", "") + ",");
        }

        private string method_0(int int_0)
        {
            if (int_0 == 0)
            {
                return "紧急";
            }
            return "一般";
        }

        private string method_1(int int_0)
        {
            string str = string.Empty;
            int num = int_0;
            switch (num)
            {
                case 1:
                    return "车载终端";

                case 2:
                    return "企业监控平台";

                case 3:
                    return "政府监管平台";
            }
            if (num == 9)
            {
                str = "其它";
            }
            return str;
        }

        private string method_2(int int_0)
        {
            string str = string.Empty;
            int num = int_0;
            switch (num)
            {
                case 1:
                    return "超速报警";

                case 2:
                    return "疲劳驾驶告警";

                case 3:
                    return "紧急告警";

                case 4:
                    return "进入指定区域告警";

                case 5:
                    return "离开指定区域告警";

                case 6:
                    return "路段堵塞告警";

                case 7:
                    return "危险路段告警";

                case 8:
                    return "越界告警";

                case 9:
                    return "盗警";

                case 10:
                    return "劫警";

                case 11:
                    return "偏离路线报警";

                case 12:
                    return "车辆移动告警";

                case 13:
                    return "超时驾驶告警";
            }
            if (num == 0xff)
            {
                str = "其它报警";
            }
            return str;
        }

        public string SourceData
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }
    }
}

