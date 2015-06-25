namespace Bussiness
{
    using System;
    using System.Collections;
    using System.Data;

    public class DownCommand : ReceiveDataBase
    {
        private ArrayList arrayList_CarNewLogData = new ArrayList(30);

        /// <summary>
        /// 添加客户端最新日志 重载
        /// </summary>
        /// <param name="num"></param>
        /// <param name="carNum"></param>
        /// <param name="strType"></param>
        /// <param name="strOrderCode"></param>
        /// <param name="strResult"></param>
        /// <param name="strMode"></param>
        /// <param name="strInfo"></param>
        public void AddCarNewLogData(int num, string carNum, string strType, string strOrderCode, string strResult, string strMode, string strInfo)
        {
            this.AddCarNewLogData(num, carNum, strType, strOrderCode, strResult, strMode, strInfo, null, null, null, null, null, null, null, 0, null, null);
        }

        /// <summary>
        /// 添加客户端最新日志
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="carNum"></param>
        /// <param name="strType"></param>
        /// <param name="strOrderCode"></param>
        /// <param name="strResult"></param>
        /// <param name="strMode"></param>
        /// <param name="strInfo"></param>
        /// <param name="strLon"></param>
        /// <param name="strLat"></param>
        /// <param name="strAcc"></param>
        /// <param name="speed"></param>
        /// <param name="IsFill"></param>
        /// <param name="GpsValid"></param>
        /// <param name="CarId"></param>
        /// <param name="Direct"></param>
        /// <param name="ReceTime"></param>
        /// <param name="GpsTime"></param>
        public void AddCarNewLogData(int OrderID, string carNum, string strType, string strOrderCode, string strResult, string strMode, string strInfo, string strLon, string strLat, string strAcc, string speed, string IsFill, string GpsValid, string CarId, int Direct, string ReceTime, string GpsTime)
        {
            this.initData();
            DataRow row = UpdataStruct.CloneDataTableColumn.NewRow();
            if (string.IsNullOrEmpty(CarId))
            {
                CarInfo dataCarInfoByCarNum = CarDataInfoBuffer.GetDataCarInfoByCarNum(carNum);
                if (dataCarInfoByCarNum != null)
                {
                    row["CarId"] = dataCarInfoByCarNum.CarId;
                    row["SimNum"] = dataCarInfoByCarNum.SimNum;
                }
            }
            else
            {
                row["CarId"] = CarId;
            }
            row["GpsTime"] = GpsTime;
            row["ReceTime"] = ReceTime;
            if (string.IsNullOrEmpty(GpsTime) || string.IsNullOrEmpty(ReceTime))
            {
                row["ReceTime"] = row["GpsTime"] = base.GetDBCurrentDateTime();
            }
            row["OrderID"] = OrderID;
            row["CarNum"] = carNum;
            row["OrderType"] = strType;
            row["OrderName"] = strOrderCode;
            row["msgType"] = -1;
            row["OrderResult"] = strResult;
            row["CommFlag"] = strMode;
            row["Describe"] = strInfo;
            row["Longitude"] = strLon;
            row["Latitude"] = strLat;
            row["AccOn"] = strAcc;
            row["Speed"] = speed;
            row["IsFill"] = IsFill;
            row["GpsValid"] = GpsValid;
            row["Direct"] = Direct;
            this.arrayList_CarNewLogData.Add(row.ItemArray);
        }

        /// <summary>
        /// 添加位置信息
        /// </summary>
        /// <param name="posData"></param>
        public void AddNewLog(object[] posData)
        {
            this.arrayList_CarNewLogData.Add(posData);
        }

        /// <summary>
        /// 初始化日志列表
        /// </summary>
        private void initData()
        {
            if (this.arrayList_CarNewLogData == null)
            {
                this.arrayList_CarNewLogData = new ArrayList(30);
            }
        }

        /// <summary>
        /// 获取最新日志
        /// </summary>
        /// <returns></returns>
        public DataTable ReadCarNewLogData()
        {
            DataTable cloneDataTableColumn = UpdataStruct.CloneDataTableColumn;
            if ((this.arrayList_CarNewLogData != null) && (this.arrayList_CarNewLogData.Count > 0))
            {
                while (this.arrayList_CarNewLogData.Count > 0)
                {
                    cloneDataTableColumn.LoadDataRow(this.arrayList_CarNewLogData[0] as object[], false);
                    this.arrayList_CarNewLogData.RemoveAt(0);
                }
                this.arrayList_CarNewLogData.TrimToSize();
                return cloneDataTableColumn;
            }
            return cloneDataTableColumn;
        }
    }
}

