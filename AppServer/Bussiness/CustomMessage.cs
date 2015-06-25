namespace Bussiness
{
    using System;
    using Library;

    public class CustomMessage : AttachParser
    {
        public CustomMessage()
        {
        }

        public CustomMessage(string string_1)
        {
            base.MessageAlarmText = string_1;
        }

        public override string Parse()
        {
            string str = string.Empty;
            string str2 = NumHelper.GetStringFromBase16ASCII(base.MessageAlarmText.Substring(0));
            return (str + "自定义信息：" + str2);
        }
    }
}

