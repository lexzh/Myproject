namespace DataAccess
{
    using System;

    public static class DataType
    {
        public static int Oracle;
        public static int SqlServer;
        public static int XML;

        static DataType()
        {
            old_acctor_mc();
        }

        private static void old_acctor_mc()
        {
            SqlServer = 1;
            Oracle = 2;
            XML = 3;
        }
    }
}

