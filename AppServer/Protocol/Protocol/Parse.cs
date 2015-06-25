namespace Protocol
{
    using System;
    using System.Data;

    public class Parse
    {
        protected void InitData(DataRow dataRow_0, DataRow dataRow_1)
        {
            dataRow_1["OrderType"] = "接收";
            dataRow_1["msgType"] = 0x45;
            dataRow_1["OrderResult"] = "成功";
            dataRow_1["CommFlag"] = "GPRS/CDMA";
            dataRow_1["GpsTime"] = Convert.ToString(dataRow_0["GpsTime"]);
            dataRow_1["ReceTime"] = Convert.ToString(dataRow_0["instime"]);
            dataRow_1["CarNum"] = Convert.ToString(dataRow_0["carNum"]);
            dataRow_1["CarId"] = Convert.ToInt32(dataRow_0["CarId"]);
            dataRow_1["WrkId"] = Convert.ToString(dataRow_0["WorkId"]);
            dataRow_1["SimNum"] = Convert.ToString(dataRow_0["SimNum"]);
            dataRow_1["OrderID"] = Convert.ToString(dataRow_0["orderId"]);
        }
    }
}

