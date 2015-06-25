namespace Bussiness
{
    using System;
    using Library;

    public class RunTimeAlarm : AttachParser
    {
        public RunTimeAlarm()
        {
        }

        public RunTimeAlarm(string string_1)
        {
            base.MessageAlarmText = string_1;
        }

        private string method_0(string string_1)
        {
            if ("00".Equals(string_1))
            {
                return "终端行驶时间不足";
            }
            return "终端行驶时间过长";
        }

        public override string Parse()
        {
            string str2 = NumHelper.Convert16To10ToString(base.MessageAlarmText.Substring(0, 8));
            string str3 = NumHelper.Convert16To10ToString(base.MessageAlarmText.Substring(8, 4));
            string str4 = base.MessageAlarmText.Substring(12);
            return ((("路段ID：" + str2) + ",行驶时间：" + str3) + ",结果：" + this.method_0(str4));
        }
    }
}

