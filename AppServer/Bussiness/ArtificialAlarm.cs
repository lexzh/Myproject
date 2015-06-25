namespace Bussiness
{
    using System;
    using Library;

    public class ArtificialAlarm : AttachParser
    {
        public ArtificialAlarm()
        {
        }

        public ArtificialAlarm(string string_1)
        {
            base.MessageAlarmText = string_1;
        }

        public override string Parse()
        {
            string str = string.Empty;
            string str2 = NumHelper.Convert16To10ToString(base.MessageAlarmText.Substring(0, 4));
            return (str + "人工确认事件报警ID：" + str2);
        }
    }
}

