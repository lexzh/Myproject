namespace DataAccess
{
    using Library;
    using System;

    internal static class DataConst
    {
        private static FileHelper fileHelper1;
        private static string m_ConnectionString;

        static DataConst()
        {
            old_acctor_mc();
        }

        private static void old_acctor_mc()
        {
            fileHelper1 = new FileHelper();
            m_ConnectionString = FileHelper.ReadParamFromXml("ConnectionString");
        }

        public static string ConnectionString
        {
            get
            {
                return m_ConnectionString;
            }
        }

        public static int SqlTimeOut
        {
            get
            {
                int num = 300;
                try
                {
                    num = Convert.ToInt32(FileHelper.ReadParamFromXml("SqlTimeOut"));
                }
                catch
                {
                }
                return num;
            }
        }
    }
}

