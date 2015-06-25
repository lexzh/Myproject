namespace Bussiness
{
    using DataAccess;
    using System;
    using System.Data;

    public class CommonOverSpeedPathID : AttachParser
    {
        private string string_1;

        public CommonOverSpeedPathID()
        {
        }

        public CommonOverSpeedPathID(string string_2, string string_3)
        {
            base.MessageAlarmText = string_2;
            this.string_1 = string_3;
        }

        private string method_0(string string_2, string string_3)
        {
            string format = "select a.pathName from GpsPathType a left join GpsCarPathParam b on a.pathid = b.pathid where b.newpathid = {0} and b.carid = {1}";
            format = string.Format(format, string_3, string_2);
            DataTable table = new SqlDataAccess().getDataBySql(format);
            if ((table != null) && (table.Rows.Count > 0))
            {
                return table.Rows[0]["pathName"].ToString();
            }
            return string.Empty;
        }

        public override string Parse()
        {
            return string.Format("超速路线ID：{0},超速路线名称：{1}", base.MessageAlarmText, this.method_0(this.string_1, base.MessageAlarmText));
        }
    }
}

