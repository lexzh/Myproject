namespace Bussiness
{
    using DataAccess;
    using System;
    using System.Data;

    public class SSOLogin
    {
        public int GetNewId(string string_0)
        {
            string str = "select NewID from GpsModule where id=" + string_0;
            DataTable table = new SqlDataAccess().getDataBySql(str);
            if ((table != null) && (table.Rows.Count > 0))
            {
                return int.Parse(table.Rows[0][0].ToString());
            }
            return -1;
        }
    }
}

