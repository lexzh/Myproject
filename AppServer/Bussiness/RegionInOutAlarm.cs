namespace Bussiness
{
    using System;
    using Library;

    public class RegionInOutAlarm : AttachParser
    {
        public RegionInOutAlarm()
        {
        }

        public RegionInOutAlarm(string string_1)
        {
            base.MessageAlarmText = string_1;
        }

        private string method_0(string string_1)
        {
            string str = string.Empty;
            switch (string_1)
            {
                case "01":
                    return "圆形区域报警";

                case "02":
                    return "矩形区域报警";

                case "03":
                    return "多边形区域报警";

                case "04":
                    return "路线报警";

                case "FD":
                    return "建筑工地";

                case "FE":
                    return "卸料场";

                case "FF":
                    return "关键区域报警";
            }
            return str;
        }

        private string method_1(string string_1)
        {
            if ("00".Equals(string_1))
            {
                return "进";
            }
            return "出";
        }

        public override string Parse()
        {
            string str2 = base.MessageAlarmText.Substring(0, 2);
            string str3 = NumHelper.Convert16To10ToString(base.MessageAlarmText.Substring(2, 8));
            string str4 = base.MessageAlarmText.Substring(10);
            return ((("类型：" + this.method_0(str2)) + ",ID：" + str3) + ",方向：" + this.method_1(str4));
        }
    }
}

