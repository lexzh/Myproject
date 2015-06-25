namespace Bussiness
{
    using DataAccess;
    using System;
    using System.Data;

    public class CarOil : ReceiveDataBase
    {
        public DataTable GetOilBoxInfo(string string_0)
        {
            string str = "  SELECT a.OilBoxTypeId, c.Point, c.Percentage, c.Vol, c.AD,  b.name, b.EmptyAD, b.FullAD, b.OilBoxVol, b.PowerType FROM GisCar a , GpsOilBoxRefPoint c left join GpsOilBoxType b on b.id = c.TypeID WHERE a.OilBoxTypeId = b.id  and a.CarId = " + string_0 + " ORDER BY Point ";
            SqlDataAccess access = new SqlDataAccess();
            return access.getDataBySql(str);
        }

        public int GetOilBoxVol(string string_0)
        {
            string str = "select OilBoxVol from GpsOilBoxType where id in";
            str = str + "(select OilBoxTypeId from giscar where carid = " + string_0 + ")";
            DataTable table = new SqlDataAccess().getDataBySql(str);
            if (table.Rows.Count > 0)
            {
                return int.Parse(table.Rows[0]["OilBoxVol"].ToString());
            }
            return 0;
        }
    }
}

