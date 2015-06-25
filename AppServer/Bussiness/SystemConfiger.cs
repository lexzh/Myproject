namespace Bussiness
{
    using DataAccess;
    using System;

    public sealed class SystemConfiger
    {
        private const string configerPasswordOutDays = "Select a.PwdExpiredDays from GpsSysConfig a";

        public static int GetConfigPasswordOutDays()
        {
            int num = 30;
            try
            {
                object returnBySql = new SqlDataAccess().GetReturnBySql("Select a.PwdExpiredDays from GpsSysConfig a");
                if (returnBySql != null)
                {
                    num = Convert.ToInt32(returnBySql);
                }
            }
            catch
            {
            }
            return num;
        }
    }
}

