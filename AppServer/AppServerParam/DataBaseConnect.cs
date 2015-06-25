namespace AppServerParam
{
    using System;
    using System.Data.SqlClient;

    public class DataBaseConnect
    {
        public bool IsConnected(DataBaseParams dataBaseParams_0)
        {
            bool flag = true;
            try
            {
                SqlConnection connection = new SqlConnection(dataBaseParams_0.DataBaseStringParam);
                connection.Open();
                connection.Close();
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
    }
}

