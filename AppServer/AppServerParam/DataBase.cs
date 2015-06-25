namespace AppServerParam
{
    using System;

    public class DataBase : ICheck
    {
        public bool Check(object object_0)
        {
            if (!(object_0 is DataBaseParams))
            {
                throw new Exception("数据为空!");
            }
            DataBaseParams @params = object_0 as DataBaseParams;
            return this.method_0(@params);
        }

        private bool method_0(DataBaseParams dataBaseParams_0)
        {
            this.method_1(dataBaseParams_0.DataBaseIp);
            if (string.IsNullOrEmpty(dataBaseParams_0.DataBaseName))
            {
                throw new Exception("数据库名称不能空!");
            }
            if (string.IsNullOrEmpty(dataBaseParams_0.DataBaseUser))
            {
                throw new Exception("数据库用户不能空!");
            }
            if (string.IsNullOrEmpty(dataBaseParams_0.DataBasePassword))
            {
                throw new Exception("数据库秘密不能空!");
            }
            if (!this.method_2(dataBaseParams_0))
            {
                throw new Exception("数据库连接失败!");
            }
            return true;
        }

        private void method_1(string string_0)
        {
            if (string.IsNullOrEmpty(string_0))
            {
                throw new Exception("数据库IP不能空!");
            }
        }

        private bool method_2(DataBaseParams dataBaseParams_0)
        {
            DataBaseConnect connect = new DataBaseConnect();
            return connect.IsConnected(dataBaseParams_0);
        }
    }
}

