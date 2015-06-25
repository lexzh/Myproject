namespace Bussiness
{
    using System;
    using Library;

    public class OverSpeedPathID : AttachParser
    {
        public OverSpeedPathID()
        {
        }

        public OverSpeedPathID(string string_1)
        {
            base.MessageAlarmText = string_1;
        }

        public override string Parse()
        {
            string str = string.Empty;
            string str2 = NumHelper.Convert16To10ToString(base.MessageAlarmText.Substring(0, 8));
            if (str2 == "-1")
            {
                return "";
            }
            return (str + "分段超速报警路线ID：" + str2);
        }
    }
}

