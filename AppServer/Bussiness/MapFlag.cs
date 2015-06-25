namespace Bussiness
{
    using DataAccess;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class MapFlag
    {
        public int AddFlagMap(float lng, float lat, string flagName, string areaCode, int flagTypeCode)
        {
            string str = "WebGpsClient_AddMapFlag";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@lon", SqlDbType.Float), new SqlParameter("@lat", SqlDbType.Float), new SqlParameter("@flagName", SqlDbType.VarChar), new SqlParameter("@areaCode", SqlDbType.VarChar), new SqlParameter("@flagTypeCode", SqlDbType.Int) };
            parameterArray[0].Value = lng;
            parameterArray[1].Value = lat;
            parameterArray[2].Value = flagName;
            parameterArray[3].Value = areaCode;
            parameterArray[4].Value = flagTypeCode;
            return new SqlDataAccess().updateBySp(str, parameterArray);
        }

        public bool DeleteFlagMap(string string_0)
        {
            bool flag = false;
            string str = "delete from GpsMapFlag where flagName = '" + string_0 + "'";
            if (new SqlDataAccess().DeleteBySql(str) > 0)
            {
                flag = true;
            }
            return flag;
        }

        public DataTable FlagMapType(string userId)
        {
            if ((userId != null) && (userId.Length > 0))
            {
                string str = "select * from gpsMapFlagType a where a.id in ";
                str = str + "(select otherKey from GpsOthersAuth where OtherType = 1 and auth&2=2 and userId = '" + userId + "')";
                return new SqlDataAccess().getDataBySql(str);
            }
            return new DataTable();
        }

        public DataTable showFlagMap(string string_0)
        {
            string str = "select POIAuth from GpsSysConfig";
            string str2 = "WebGpsClient_GetAreaMapFlag_mul";
            SqlDataAccess access = new SqlDataAccess();
            DataTable table = access.getDataBySql(str);
            int num = 0;
            if ((table == null) || (table.Rows.Count <= 0))
            {
                return null;
            }
            if (table.Rows[0]["poiAuth"] == DBNull.Value)
            {
                return null;
            }
            num = int.Parse(table.Rows[0]["poiAuth"].ToString());
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@POIAuth", SqlDbType.Int), new SqlParameter("@userID", SqlDbType.VarChar) };
            parameterArray[0].Value = num;
            parameterArray[1].Value = string_0;
            return access.getDataBySP(str2, parameterArray);
        }
    }
}

