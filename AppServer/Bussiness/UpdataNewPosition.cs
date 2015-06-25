using System.Diagnostics;
namespace Bussiness
{
    using DataAccess;
    using Library;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;

    public class UpdataNewPosition : ReceiveDataBase
    {
        private UpdataCommon updataCommon_0 = new UpdataCommon();

        private void method_0()
        {
            Trace.Write("appserver - Thread upNewPosition, WebGpsClient_GetCurrentPosData start!");
            DataRow row = UpdataStruct.CloneDataTableColumn.NewRow();
            SqlDataAccess access = new SqlDataAccess();
            DateTime dbTime = base.GetDbTime(access);
        Label_0019:
            try
            {
                SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@ReadTime", dbTime) };
                DataTable table = access.getDataBySP("WebGpsClient_GetCurrentPosData", parameterArray);
                if (table != null)
                {
                    goto Label_0056;
                }
            Label_004A:
                Thread.Sleep(0x7d0);
                goto Label_0019;
            Label_0056:
                if (table.Rows.Count <= 0)
                {
                    goto Label_004A;
                }
                dbTime = Convert.ToDateTime(table.Rows[0]["svrTime"]);
                string str = string.Empty;
                string str2 = string.Empty;
                CarInfo dataCarInfoBySimNum = null;
                CarPartInfo info2 = new CarPartInfo();
                foreach (DataRow row2 in table.Rows)
                {
                    this.method_1(row2, row, info2);
                    str = Convert.ToString(row2["phone"]);
                    str2 = Convert.ToString(row2["carNum"]);
                    dataCarInfoBySimNum = CarDataInfoBuffer.GetDataCarInfoBySimNum(str);
                    if (((dataCarInfoBySimNum != null) && !string.IsNullOrEmpty(str2)) && !str2.Equals(dataCarInfoBySimNum.CarNum))
                    {
                        CarDataInfoBuffer.GetDataCarInfoByCarNum(str2);
                    }
                    if (dataCarInfoBySimNum != null)
                    {
                        dataCarInfoBySimNum.CarPosData = row.ItemArray;
                        dataCarInfoBySimNum.IsNewPosTime = dbTime;
                    }
                }
                Thread.Sleep(20);
                goto Label_0019;
            }
            catch (Exception exception)
            {
                Thread.Sleep(0xbb8);
                LogHelper helper = new LogHelper();
                ErrorMsg msg = new ErrorMsg("UpdataNewPosition", helper.GetCallFunction(), helper.GetExceptionMsg(exception));
                helper.WriteError(msg);
                goto Label_0019;
            }
        }

        private void method_1(DataRow dataRow_0, DataRow dataRow_1, CarPartInfo carPartInfo_0)
        {
            int drInt = base.GetDrInt(dataRow_0, "reserved");
            base.GetDrStr(dataRow_0, "phone");
            this.updataCommon_0.GetCarPartInfo(dataRow_1, dataRow_0, carPartInfo_0);
            this.updataCommon_0.SetUpdataPosData(dataRow_1, dataRow_0, "", this.updataCommon_0.GetAddMsgText(dataRow_0, drInt), carPartInfo_0);
            dataRow_1["CarStatus"] = 2;
            dataRow_1["AlarmType"] = 0;
        }

        public void Start()
        {
            Thread thread = new Thread(new ThreadStart(this.method_0)) {
                IsBackground = true
            };
            thread.Priority = ThreadPriority.AboveNormal;
            thread.Start();
        }
    }
}

