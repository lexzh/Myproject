namespace Bussiness
{
    using Library;
    using System;
    using System.Data;

    public class UpPic
    {
        private const string _ReportDes = "摄像头信息报文";
        private UpdataCommon updataCommon_0 = new UpdataCommon();

        public void CalPicData(DataRow dataRow_0, CarPartInfo carPartInfo_0, DataTable dataTable_0)
        {
            try
            {
                DataRow row = dataTable_0.NewRow();
                this.updataCommon_0.GetCarPartInfo(row, dataRow_0, carPartInfo_0);
                row["GpsTime"] = Convert.ToString(dataRow_0["GpsTime"]);
                row["OrderID"] = Convert.ToString(dataRow_0["orderId"]);
                row["CarId"] = Convert.ToString(dataRow_0["CarId"]);
                row["CarNum"] = Convert.ToString(dataRow_0["CarNum"]);
                row["SimNum"] = Convert.ToString(dataRow_0["Phone"]);
                row["OrderType"] = "";
                row["OrderName"] = "";
                row["MsgType"] = "";
                row["OrderResult"] = "";
                row["CommFlag"] = "";
                row["Describe"] = carPartInfo_0.GetCarCurrentInfo() + "摄像头信息报文";
                row["Longitude"] = carPartInfo_0.Lon;
                row["Latitude"] = carPartInfo_0.Lat;
                row["isImportWatch"] = -1;
                if (dataRow_0["CameraId"] != DBNull.Value)
                {
                    row["CameraId"] = this.updataCommon_0.ConvertCameraId(Convert.ToInt32(dataRow_0["CameraId"]));
                }
                row["svrTime"] = Convert.ToString(dataRow_0["svrTime"]);
                row["statuName"] = carPartInfo_0.StatusName;
                row["ReceTime"] = Convert.ToDateTime(Convert.ToString(dataRow_0["ReceTime"])).ToString("yyyy-MM-dd HH:mm:ss.fff");
                row["ReadPicTime"] = Convert.ToDateTime(Convert.ToString(dataRow_0["ReadPicTime"])).ToString("yyyy-MM-dd HH:mm:ss.fff");
                row["CarStatus"] = 2;
                row["AlarmType"] = 0;
                row["PicDataType"] = Convert.ToString(dataRow_0["PicDataType"]);
                dataTable_0.Rows.Add(row);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("CarPic", "CalPicData", exception.Message);
                new LogHelper().WriteError(msg);
            }
        }
    }
}

