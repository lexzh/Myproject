namespace Protocol
{
    using DataAccess;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class UpdataInsertDBInfor
    {
        public static string GetEventName(string string_0)
        {
            string str = "select MsgName from  GpsJTBMsgParam where id=" + string_0 + " and MsgType=1";
            DataTable table = new SqlDataAccess().getDataBySql(str);
            if (((table != null) && (table.Rows.Count != 0)) && (table.Rows[0][0] != DBNull.Value))
            {
                return table.Rows[0][0].ToString();
            }
            return "未知事件类型";
        }

        public static string GetMenuName(string string_0)
        {
            string str = "select MsgName from GpsJTBMsgParam where id=" + string_0 + " and MsgType=2";
            DataTable table = new SqlDataAccess().getDataBySql(str);
            if (((table != null) && (table.Rows.Count != 0)) && (table.Rows[0][0] != DBNull.Value))
            {
                return table.Rows[0][0].ToString();
            }
            return "未知信息类型";
        }

        public static int UpdateDriverInfor(int int_0, string string_0, string string_1, string string_2, string string_3)
        {
            SqlDataAccess access = new SqlDataAccess();
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@carid", int_0), new SqlParameter("@IdentityCard", string_0), new SqlParameter("@LaborQualificationNo", string_1), new SqlParameter("@DriverName", string_2), new SqlParameter("@releaseName", string_3), new SqlParameter("@ErrMsg", SqlDbType.Int) };
            object obj2 = 1;
            access.updateBySp("WebGpsClient_UpdateDriverInfor", parameterArray, "@ErrMsg", out obj2);
            return Convert.ToInt32(obj2);
        }

        public static int UpdateDriverInfor(int int_0, int int_1, DateTime dateTime_0, int int_2, string string_0, string string_1, string string_2, DateTime dateTime_1)
        {
            SqlDataAccess access = new SqlDataAccess();
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@carid", int_0), new SqlParameter("@Status", int_1), new SqlParameter("@ReportTime", dateTime_0), new SqlParameter("@ReadResult", int_2), new SqlParameter("@DriverName", string_0), new SqlParameter("@LaborQualificationNo", string_1), new SqlParameter("@releaseName", string_2), new SqlParameter("@validity", dateTime_1), new SqlParameter("@ErrMsg", SqlDbType.Int) };
            object obj2 = 1;
            access.updateBySp("WebGpsClient_UpdateDriverInforEx", parameterArray, "@ErrMsg", out obj2);
            return Convert.ToInt32(obj2);
        }

        public static void UpdateMenuInfor(string string_0, string string_1, string string_2, bool bool_0)
        {
            string str = "";
            SqlDataAccess access = new SqlDataAccess();
            str = "delete from GpsJTBMsgHistory where SimNum='" + string_0 + "' and MsgType=" + string_1 + " and MsgID=" + string_2;
            access.DeleteBySql(str);
            if (bool_0)
            {
                str = " insert into GpsJTBMsgHistory(SimNum,MsgType,MsgID,CreateDate) values('" + string_0 + "'," + string_1 + "," + string_2 + ",getdate())";
                access.insertBySql(str);
            }
        }
    }
}

