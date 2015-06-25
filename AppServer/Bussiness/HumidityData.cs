namespace Bussiness
{
    using System;
    using Library;

    public class HumidityData : AttachParser
    {
        public HumidityData()
        {
        }

        public HumidityData(string string_1)
        {
            base.MessageAlarmText = string_1;
        }

        public override string Parse()
        {
            int num = NumHelper.Convert16To10(base.MessageAlarmText.Substring(0, 4));
            return string.Format("当前湿度：{0}%RH", num * 0.01);
        }
    }
}

