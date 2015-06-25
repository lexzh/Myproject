namespace Bussiness
{
    using Library;
    using System;
    using System.Data;

    public class UpAlarm
    {
        private UpdataCommon updataCommon_0 = new UpdataCommon();

        public void CalAlarmData(DataRow dataRow_0, DataRow dataRow_1, CarPartInfo carPartInfo_0, DataTable dataTable_0)
        {
            try
            {
                int num = Convert.ToInt32(dataRow_0["reserved"]);
                string str = Convert.ToString(dataRow_0["phone"]);
                this.updataCommon_0.GetCarPartInfo(dataRow_1, dataRow_0, carPartInfo_0);
                int num2 = this.updataCommon_0.SetCarAlarmStatus(dataRow_1, dataRow_0, str);
                this.updataCommon_0.SetUpdataPosData(dataRow_1, dataRow_0, "", this.updataCommon_0.GetAddMsgText(dataRow_0, num), carPartInfo_0);
                dataRow_1["IsImportWatch"] = Convert.ToInt32(dataRow_0["isImportWatch"]);
                dataRow_1["statuDesc"] = this.updataCommon_0.GetStatuDesc(str, Convert.ToString(dataRow_0["recetime"]), carPartInfo_0.GpsTime, carPartInfo_0.StatusName);
                dataRow_1["OrderType"] = new CarAlarmType().GetAlarmTypeName(num2);
                dataTable_0.Rows.Add(dataRow_1.ItemArray);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("UpAlarm", "CalAlarmData", exception.Message + exception.StackTrace);
                new LogHelper().WriteError(msg);
            }
        }
    }
}

