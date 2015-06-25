namespace Bussiness
{
    using DataAccess;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class Area
    {
        private const string areaAllInfomation = "WebGpsClient_GetAreaInfoAll";
        private const string areaAllInfomationEx = "WebGpsClient_GetAreaInfoAll";
        private string string_0 = string.Empty;
        private const string subAreaAllInfomation = "WebGpsClient_GetTreeInfo";
        private const string userAreaAllInfomation = "WebGpsClient_GetUserAreaInfo";

        public Area(string string_1)
        {
            this.string_0 = string_1;
        }

        public DataSet GetAllTreeInfo(string string_1)
        {
            DataSet set;
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@workId ", string_1), new SqlParameter("@UserId", this.UserId) };
            try
            {
                set = new SqlDataAccess().getDataSetBySP("WebGpsClient_GetTreeInfo", parameterArray);
            }
            catch (Exception exception)
            {
                throw new Exception("获取所有树信息:" + exception.Message);
            }
            return set;
        }

        public DataTable GetAreaInfoAll()
        {
            DataTable table;
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@UserId", this.UserId) };
            try
            {
                table = new SqlDataAccess().getDataBySP("WebGpsClient_GetAreaInfoAll", parameterArray);
            }
            catch (Exception exception)
            {
                throw new Exception("获取所有子区域的区域信息:" + exception.Message);
            }
            return table;
        }

        public DataTable GetAreaInfoAllEx()
        {
            DataTable table;
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@UserId", this.UserId) };
            try
            {
                table = new SqlDataAccess().getDataBySP("WebGpsClient_GetAreaInfoAll", parameterArray);
            }
            catch (Exception exception)
            {
                throw new Exception("获取所有子区域的区域信息:" + exception.Message);
            }
            return table;
        }

        public DataTable GetUserAreaInfo()
        {
            DataTable table;
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@AreaCode ", ""), new SqlParameter("@UserId", this.UserId) };
            try
            {
                table = new SqlDataAccess().getDataBySP("WebGpsClient_GetUserAreaInfo", parameterArray);
            }
            catch (Exception exception)
            {
                throw new Exception("获取区域的所有区域信息:" + exception.Message);
            }
            return table;
        }

        public string UserId
        {
            get
            {
                return this.string_0;
            }
        }
    }
}

