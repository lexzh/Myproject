using System.Diagnostics;
namespace Bussiness
{
    using DataAccess;
    using Library;
    using Protocol;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;

    public class UpDataIODeviceData : ReceiveDataBase
    {
        private UpBuffer upBuffer_0 = new UpBuffer();
        private UpBuffer upBuffer_1 = new UpBuffer();
        private UpdataIODeviceHandler updataIODeviceHandler_0;

        public void Delete(Hashtable hashtable_0)
        {
            this.upBuffer_0.Delete(hashtable_0);
        }

        public void FilterPlatRequestUpLog(DataTable dataTable_0, string string_0, string string_1)
        {
            List<DataRow> list = new List<DataRow>();
            foreach (DataRow row in dataTable_0.Rows)
            {
                if (row["msgType"].ToString() == "4353")
                {
                    if ((row["OBJECT_TYPE"].ToString() == "1") && (string_1 != "1"))
                    {
                        list.Add(row);
                    }
                    else if ((row["OBJECT_TYPE"].ToString() == "2") && (row["OBJECT_ID"].ToString() != string_0))
                    {
                        list.Add(row);
                    }
                    else if ((row["OBJECT_TYPE"].ToString() == "3") && (string.IsNullOrEmpty(string_0) || (string_1 == "1")))
                    {
                        list.Add(row);
                    }
                }
                if (row["msgType"].ToString() == "4911")
                {
                    if (((((row["OBJECT_TYPE"].ToString() == "0") || (row["OBJECT_TYPE"].ToString() == "1")) || ((row["OBJECT_TYPE"].ToString() == "4") || (row["OBJECT_TYPE"].ToString() == "4"))) || (((row["OBJECT_TYPE"].ToString() == "7") || (row["OBJECT_TYPE"].ToString() == "8")) || (row["OBJECT_TYPE"].ToString() == "9"))) && (string_1 != "1"))
                    {
                        list.Add(row);
                    }
                    else if ((row["OBJECT_TYPE"].ToString() == "2") && (row["OBJECT_ID"].ToString() != string_0))
                    {
                        list.Add(row);
                    }
                    else if ((row["OBJECT_TYPE"].ToString() == "3") && (string.IsNullOrEmpty(string_0) || (string_1 == "1")))
                    {
                        list.Add(row);
                    }
                    else if (((row["OBJECT_TYPE"].ToString() == "5") && string.IsNullOrEmpty(string_0)) && (string_1 != "1"))
                    {
                        list.Add(row);
                    }
                }
                if ((row["msgType"].ToString() == "4355") && (string_1 != "1"))
                {
                    list.Add(row);
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                dataTable_0.Rows.Remove(list[i]);
            }
        }

        public DataTable GetData(OnlineUserInfo onlineUserInfo_0)
        {
            DataTable datByWorkId = null;
            if (this.IsOpened)
            {
                datByWorkId = this.upBuffer_0.GetDataByWorkId(onlineUserInfo_0.WorkId);
                DateTime newLogIOTime = onlineUserInfo_0.NewLogIOTime;
                DataTable data = this.upBuffer_1.GetData(ref newLogIOTime);
                onlineUserInfo_0.NewLogIOTime = newLogIOTime;
                if ((data != null) && (data.Rows.Count > 0))
                {
                    datByWorkId.Merge(data);
                }
            }
            return datByWorkId;
        }

        private void method_0(Exception exception_0)
        {
            Thread.Sleep(0x1388);
            LogHelper helper = new LogHelper();
            ErrorMsg msg = new ErrorMsg("UpDataIODeviceData", helper.GetCallFunction(), helper.GetExceptionMsg(exception_0));
            helper.WriteError(msg);
        }

        private bool method_1(DateTime dateTime_0)
        {
            bool flag = false;
            TimeSpan span = (TimeSpan) (DateTime.Now - dateTime_0);
            if (span.TotalSeconds > 30.0)
            {
                this.upBuffer_1.Delete();
                flag = true;
            }
            return flag;
        }

        private void method_2(IODeviceAttachInfo iodeviceAttachInfo_0)
        {
            if (iodeviceAttachInfo_0 != null)
            {
                if (this.updataIODeviceHandler_0 == null)
                {
                    this.updataIODeviceHandler_0 = new UpdataIODeviceHandler();
                }
                this.updataIODeviceHandler_0.HandleIODeviceAttachInfo(iodeviceAttachInfo_0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void upOutEquipmentData()
        {
            Trace.Write("appserver - thread upOutEquipmentData, WebGpsClient_GetOutEquipmentData start!");
            SqlDataAccess access = new SqlDataAccess();
            DateTime dbTime = base.GetDbTime(access);
            DateTime now = DateTime.Now;
            DataTable cloneDataTableColumn = UpdataStruct.CloneDataTableColumn;
            DataTable table2 = UpdataStruct.CloneDataTableColumn;
            DataRow row = cloneDataTableColumn.NewRow();
            ProtocolsInterface interface2 = new ProtocolsInterface();
            while (true)
            {
                DataTable table3 = null;
                try
                {
                    SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@PreReadTime", dbTime) };
                    table3 = access.getDataBySP("WebGpsClient_GetOutEquipmentData", parameterArray);
                }
                catch (SqlException exception)
                {
                    this.method_0(exception);
                }
                if ((table3 != null) && (table3.Rows.Count > 0))
                {
                    int workid = -1;
                    foreach (DataRow row2 in table3.Rows)
                    {
                        try
                        {
                            if (interface2.DataParse(row2, row))
                            {
                                if (interface2.IODeviceAttachInfo.Count > 0)
                                {
                                    this.method_2(interface2.IODeviceAttachInfo.Get());
                                }
                                workid = Convert.ToInt32(row["WrkId"]);
                                //修改workid为处理后获取得到的workId，将数据发送到指定的用户
                                //Trace.Write("appserver - UpDataIODeviceData, workid = " + workid.ToString());
                                if (workid != 0 && workid != -2)
                                {
                                    cloneDataTableColumn.Rows.Add(row.ItemArray);
                                    this.upBuffer_0.AddByWorkId(workid, cloneDataTableColumn.Copy());
                                    cloneDataTableColumn.Clear();
                                }
                                else if (workid == -2)
                                {
                                    //如果自动查岗，则不发送消息到客户端 huzh 2014.1.22
                                    continue;
                                }
                                else
                                {
                                    table2.Rows.Add(row.ItemArray);
                                }
                            }
                        }
                        catch (Exception exception2)
                        {
                            this.method_0(exception2);
                        }
                    }
                    try
                    {
                        dbTime = Convert.ToDateTime(table3.Rows[0]["svrTime"]);
                    }
                    catch (Exception exception3)
                    {
                        this.method_0(exception3);
                    }
                    if ((table2 != null) && (table2.Rows.Count > 0))
                    {
                        this.upBuffer_1.Add(dbTime, table2.Copy());
                        table2.Clear();
                    }
                    Thread.Sleep(3000);
                    if (this.method_1(now))
                    {
                        now = DateTime.Now;
                    }
                }
                else
                {
                    Thread.Sleep(5000);
                }
            }
        }

        public void Start()
        {
            if (this.IsOpened)
            {
                Thread thread = new Thread(new ThreadStart(this.upOutEquipmentData)) {
                    IsBackground = true
                };
                thread.Priority = ThreadPriority.AboveNormal;
                thread.Start();
            }
        }

        private bool IsOpened
        {
            get
            {
                return !"-1".Equals(Const.RemotingServerIP2);
            }
        }
    }
}

