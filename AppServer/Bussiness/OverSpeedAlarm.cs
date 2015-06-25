namespace Bussiness
{
    using PublicClass;
    using System;
    using Library;

    public class OverSpeedAlarm : AttachParser
    {
        public OverSpeedAlarm()
        {
        }

        public OverSpeedAlarm(string string_1)
        {
            base.MessageAlarmText = string_1;
        }

        private string method_0(string string_1)
        {
            string str = string.Empty;
            string str2 = string_1;
            if (str2 == null)
            {
                return str;
            }
            if (str2 == "00")
            {
                return "无特定位置超速报警";
            }
            if (str2 == "01")
            {
                return "圆形区域超速报警";
            }
            if (str2 == "02")
            {
                return "矩形区域超速报警";
            }
            if (!(str2 == "03"))
            {
                if (str2 == "04")
                {
                    str = "路段超速报警";
                }
                return str;
            }
            return "多边形区域超速报警";
        }

        public override string Parse()
        {
            string str = string.Empty;
            string str2 = base.MessageAlarmText.Substring(0, 2);
            string str3 = NumHelper.Convert16To10ToString(base.MessageAlarmText.Substring(2));
            str = "类型：" + this.method_0(str2);
            if (!str2.Equals("00"))
            {
                str = str + ",ID：" + str3;
            }
            return str;
        }
    }
}

