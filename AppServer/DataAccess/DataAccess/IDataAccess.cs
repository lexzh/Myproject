namespace DataAccess
{
    using System;
    using System.Data;
    using System.Data.Common;

    public interface IDataAccess
    {
        void closeSqlConnection();
        int DeleteBySp(string string_0, DbParameter[] dbParameter_0);
        int DeleteBySp(string string_0, DbParameter[] dbParameter_0, int int_0);
        int DeleteBySql(string string_0);
        int DeleteBySql(string string_0, int int_0);
        int getCountBySql(string string_0);
        int getCountBySqlAndWhere(string string_0, string string_1, string string_2);
        int getCountBySqlNotWhere(string string_0);
        int getCountBySqlNotWhere(string string_0, string string_1);
        DataTable getDataByRecordRow(string string_0, int int_0, int int_1);
        DataTable getDataBySP(string string_0, DbParameter[] dbParameter_0);
        DataTable getDataBySql(string string_0);
        DateTime getSystemDate();
        int insertBySp(string string_0, DbParameter[] dbParameter_0);
        int insertBySql(string string_0);
        int updateBySp(string string_0, DbParameter[] dbParameter_0);
        int updateBySql(string string_0);
    }
}

