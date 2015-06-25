namespace Bussiness
{
    using System;
    using Library;

    public class CarNotBeBackOnTime : AttachParser
    {
        public CarNotBeBackOnTime()
        {
        }

        public CarNotBeBackOnTime(string string_1)
        {
            base.MessageAlarmText = string_1;
        }

        private string method_0(int int_0)
        {
            string str = string.Empty;
            if (int_0 == 1)
            {
                return "未按时到站";
            }
            if (int_0 == 2)
            {
                return "未按时出站";
            }
            if (int_0 == 4)
            {
                str = "未按时归班";
            }
            return str;
        }

        public override string Parse()
        {
            int num = NumHelper.Convert16To10(base.MessageAlarmText.Substring(0, 4));
            return this.method_0(num);
        }
    }
}

