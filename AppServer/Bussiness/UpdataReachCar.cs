namespace Bussiness
{
    using DataAccess;
    using Library;
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class UpdataReachCar
    {
        public DataTable GetUpdata(string string_0)
        {
            DataTable cloneDataTableColumn = null;
            try
            {
                SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@UserId", string_0) };
                DataTable table2 = new SqlDataAccess().getDataBySP("WebGpsClient_GetReachCarInf", parameterArray);
                cloneDataTableColumn = UpdataStruct.CloneDataTableColumn;
                if ((table2 == null) && (table2.Rows.Count <= 0))
                {
                    return cloneDataTableColumn;
                }
                foreach (DataRow row in table2.Rows)
                {
                    DataRow row2 = cloneDataTableColumn.NewRow();
                    row2["GpsTime"] = Convert.ToString(row["gpstime"]);
                    row2["ReceTime"] = Convert.ToString(row["ReceTime"]);
                    row2["OrderID"] = Convert.ToString(row["orderId"]);
                    row2["CarNum"] = "";
                    row2["OrderType"] = "信息";
                    row2["orderName"] = "提示信息";
                    row2["msgType"] = -1;
                    row2["OrderResult"] = "";
                    row2["CommFlag"] = "";
                    row2["Describe"] = Convert.ToString(row["desc1"]);
                    row2["Longitude"] = "";
                    row2["Latitude"] = "";
                    row2["isImportWatch"] = -1;
                    row2["CarId"] = "";
                    cloneDataTableColumn.Rows.Add(row2);
                }
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("UpdataReachCar", "GetUpdata", exception.Message);
                new LogHelper().WriteError(msg);
            }
            return cloneDataTableColumn;
        }
    }
}

