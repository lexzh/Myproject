namespace Bussiness
{
    using System;
    using Library;

    public class SiteMap : AttachParser
    {
        public SiteMap()
        {
        }

        public SiteMap(string string_1)
        {
            base.MessageAlarmText = string_1;
        }

        public override string Parse()
        {
            int num = NumHelper.Convert16To10(base.MessageAlarmText.Substring(0, 2));
            int num2 = NumHelper.Convert16To10(base.MessageAlarmText.Substring(2, 6));
            return (string.Format("位置区码：{0}", num) + string.Format(",移动基站：{0}", num2));
        }
    }
}

