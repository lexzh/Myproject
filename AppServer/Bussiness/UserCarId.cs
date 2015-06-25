namespace Bussiness
{
    using DataAccess;
    using Library;
    using System;
    using System.Collections;
    using System.Data;

    public class UserCarId
    {
        private Hashtable hashtable_0 = new Hashtable(500);
        private const string sqlUserCarList = "select distinct a.CarId as carid from gpsUserCar a where a.UserId = '{0}'";

        public UserCarId(string string_0)
        {
            this.method_0(string_0);
        }

        public bool IsExistCarID(int int_0)
        {
            return this.hashtable_0.ContainsKey(int_0);
        }

        private void method_0(string string_0)
        {
            string str = string.Format("select distinct a.CarId as carid from gpsUserCar a where a.UserId = '{0}'", string_0);
            try
            {
                DataTable table = new SqlDataAccess().getDataBySql(str);
                if ((table != null) && (table.Rows.Count > 0))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        this.hashtable_0.Add(row["CarId"], row["CarId"]);
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("UserCarId", "CreateUserCarInfoToList", exception.Message + exception.StackTrace);
                new LogHelper().WriteError(msg);
            }
        }

        public void queryMembers()
        {
            base.GetType();
        }

        public ICollection Keys
        {
            get
            {
                return this.hashtable_0.Keys;
            }
        }
    }
}

