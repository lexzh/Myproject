namespace DataAccess
{
    using System;
    using System.Data;
    using System.Data.Common;

    public class OracleDataAccess : IDataAccess
    {
        public void closeSqlConnection()
        {
            throw new NotImplementedException();
        }

        public int DeleteBySp(string string_0, DbParameter[] dbParameter_0)
        {
            throw new NotImplementedException();
        }

        public int DeleteBySp(string string_0, DbParameter[] dbParameter_0, int int_0)
        {
            throw new NotImplementedException();
        }

        public int DeleteBySql(string string_0)
        {
            throw new NotImplementedException();
        }

        public int DeleteBySql(string string_0, int int_0)
        {
            throw new NotImplementedException();
        }

        public int getCountBySql(string string_0)
        {
            throw new NotImplementedException();
        }

        public int getCountBySqlAndWhere(string string_0, string string_1, string string_2)
        {
            throw new NotImplementedException();
        }

        public int getCountBySqlNotWhere(string string_0)
        {
            throw new NotImplementedException();
        }

        public int getCountBySqlNotWhere(string string_0, string string_1)
        {
            throw new NotImplementedException();
        }

        public DataTable getDataByRecordRow(string string_0, int int_0, int int_1)
        {
            throw new NotImplementedException();
        }

        public DataTable getDataBySP(string string_0, DbParameter[] dbParameter_0)
        {
            throw new NotImplementedException();
        }

        public DataTable getDataBySql(string string_0)
        {
            throw new NotImplementedException();
        }

        public DateTime getSystemDate()
        {
            throw new NotImplementedException();
        }

        public int insertBySp(string string_0, DbParameter[] dbParameter_0)
        {
            throw new NotImplementedException();
        }

        public int insertBySql(string string_0)
        {
            throw new NotImplementedException();
        }

        public int updateBySp(string string_0, DbParameter[] dbParameter_0)
        {
            throw new NotImplementedException();
        }

        public int updateBySql(string string_0)
        {
            throw new NotImplementedException();
        }
    }
}

