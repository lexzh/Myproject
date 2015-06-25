namespace Bussiness
{
    using System;
    using Library;

    public class RunSpeedAlarm : AttachParser
    {
        public RunSpeedAlarm()
        {
        }

        public RunSpeedAlarm(string string_1)
        {
            base.MessageAlarmText = string_1;
        }

        public override string Parse()
        {
            string str = string.Empty;
            str = (NumHelper.Convert16To10(base.MessageAlarmText.Substring(0)) / 10).ToString();
            return string.Format("行驶记录仪速度值：{0}km/h", str);
        }
    }
}

