namespace DataAccess
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;

    public class SqlDataAccess
    {
        private static int _CommandTimeOut;

        static SqlDataAccess()
        {
            old_acctor_mc();
        }

        public int DeleteBySp(string string_0, DbParameter[] dbParameter_0)
        {
            using (SqlConnection connection = this.method_0())
            {
                connection.Open();
                return this.method_4(string_0, connection, dbParameter_0, _CommandTimeOut);
            }
        }

        public int DeleteBySql(string string_0)
        {
            using (SqlConnection connection = this.method_0())
            {
                connection.Open();
                return this.method_3(string_0, connection, _CommandTimeOut);
            }
        }

        public DataTable getDataByRecordRow(string string_0, int int_0, int int_1)
        {
            DataSet dataSet = new DataSet();
            using (SqlConnection connection = this.method_0())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(string_0, connection))
                {
                    if (int_0 < 0)
                    {
                        int_0 = 0;
                    }
                    connection.Open();
                    adapter.Fill(dataSet, int_0, int_1, "table0");
                }
            }
            return dataSet.Tables[0].Copy();
        }

        public DataTable getDataBySP(string string_0, DbParameter[] dbParameter_0)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = this.method_0())
            {
                using (SqlCommand command = new SqlCommand(string_0, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    this.method_1(command, dbParameter_0);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                    return table;
                }
            }
        }

        public DataTable getDataBySql(string string_0)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = this.method_0())
            {
                using (SqlCommand command = new SqlCommand(string_0, connection))
                {
                    connection.Open();
                    command.CommandTimeout = _CommandTimeOut;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                    return table;
                }
            }
        }

        public DataTable getDataBySqlParam(string string_0, DbParameter[] dbParameter_0)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = this.method_0())
            {
                using (SqlCommand command = new SqlCommand(string_0, connection))
                {
                    this.method_1(command, dbParameter_0);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                    return table;
                }
            }
        }

        public DataSet getDataSetBySP(string string_0, DbParameter[] dbParameter_0)
        {
            DataSet dataSet = new DataSet();
            using (SqlConnection connection = this.method_0())
            {
                using (SqlCommand command = new SqlCommand(string_0, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    this.method_1(command, dbParameter_0);
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataSet);
                    }
                    return dataSet;
                }
            }
        }

        public object GetReturnBySql(string string_0)
        {
            using (SqlConnection connection = this.method_0())
            {
                connection.Open();
                return this.method_2(string_0, connection);
            }
        }

        public DateTime getSystemDate()
        {
            DateTime now = DateTime.Now;
            using (SqlConnection connection = this.method_0())
            {
                connection.Open();
                object obj2 = this.method_2("select convert(varchar(19),getdate(),25) as sysDate", connection);
                if (obj2 != null)
                {
                    now = Convert.ToDateTime(obj2);
                }
            }
            return now;
        }

        public int insertBySp(string string_0, DbParameter[] dbParameter_0)
        {
            using (SqlConnection connection = this.method_0())
            {
                connection.Open();
                return this.method_4(string_0, connection, dbParameter_0, _CommandTimeOut);
            }
        }

        public int insertBySql(string string_0)
        {
            using (SqlConnection connection = this.method_0())
            {
                connection.Open();
                return this.method_3(string_0, connection, _CommandTimeOut);
            }
        }

        private SqlConnection method_0()
        {
            if (_CommandTimeOut == -1)
            {
                _CommandTimeOut = DataConst.SqlTimeOut;
            }
            return new SqlConnection(DataConst.ConnectionString);
        }

        private void method_1(SqlCommand sqlCommand_0, DbParameter[] dbParameter_0)
        {
            if (dbParameter_0 != null)
            {
                foreach (DbParameter parameter in dbParameter_0)
                {
                    sqlCommand_0.Parameters.Add(parameter);
                }
                sqlCommand_0.CommandTimeout = _CommandTimeOut;
            }
        }

        private object method_2(string string_0, SqlConnection sqlConnection_0)
        {
            using (SqlCommand command = new SqlCommand(string_0, sqlConnection_0))
            {
                return command.ExecuteScalar();
            }
        }

        private int method_3(string string_0, SqlConnection sqlConnection_0, int int_0)
        {
            using (SqlCommand command = new SqlCommand(string_0, sqlConnection_0))
            {
                command.CommandTimeout = int_0;
                return command.ExecuteNonQuery();
            }
        }

        private int method_4(string string_0, SqlConnection sqlConnection_0, DbParameter[] dbParameter_0, int int_0)
        {
            using (SqlCommand command = new SqlCommand(string_0, sqlConnection_0))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = int_0;
                this.method_1(command, dbParameter_0);
                return command.ExecuteNonQuery();
            }
        }

        private int method_5(string string_0, SqlConnection sqlConnection_0, DbParameter[] dbParameter_0, int int_0, string string_1, out object object_0)
        {
            int num = 0;
            using (SqlCommand command = new SqlCommand(string_0, sqlConnection_0))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = int_0;
                this.method_1(command, dbParameter_0);
                if (command.Parameters[string_1] != null)
                {
                    command.Parameters[string_1].Direction = ParameterDirection.Output;
                }
                num = command.ExecuteNonQuery();
                if (command.Parameters[string_1] != null)
                {
                    object_0 = command.Parameters[string_1].Value.ToString();
                    return num;
                }
                object_0 = null;
            }
            return num;
        }

        private static void old_acctor_mc()
        {
            _CommandTimeOut = -1;
        }

        public int updateBySp(string string_0, DbParameter[] dbParameter_0)
        {
            using (SqlConnection connection = this.method_0())
            {
                connection.Open();
                return this.method_4(string_0, connection, dbParameter_0, _CommandTimeOut);
            }
        }

        public int updateBySp(string string_0, DbParameter[] dbParameter_0, string string_1, out object object_0)
        {
            using (SqlConnection connection = this.method_0())
            {
                connection.Open();
                return this.method_5(string_0, connection, dbParameter_0, _CommandTimeOut, string_1, out object_0);
            }
        }

        public int updateBySql(string string_0)
        {
            using (SqlConnection connection = this.method_0())
            {
                connection.Open();
                return this.method_3(string_0, connection, _CommandTimeOut);
            }
        }
    }
}

