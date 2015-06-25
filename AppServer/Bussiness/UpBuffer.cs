using System.Diagnostics;
namespace Bussiness
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;

    public class UpBuffer
    {
        private Hashtable hashtable_0;
        private List<int> list_0;
        private readonly ReaderWriterLock readerWriterLock_0;

        public UpBuffer()
        {
            this.list_0 = new List<int>(30);
            this.readerWriterLock_0 = new ReaderWriterLock();
            this.hashtable_0 = Hashtable.Synchronized(new Hashtable(0xa3));
        }

        public UpBuffer(string string_0)
        {
            this.list_0 = new List<int>(30);
            this.readerWriterLock_0 = new ReaderWriterLock();
            this.hashtable_0 = Hashtable.Synchronized(new Hashtable(0xa3));
            this.AlarmCodeList = string_0;
        }

        /// <summary>
        /// 根据时间添加缓存数据
        /// </summary>
        /// <param name="time">数据接收时间</param>
        /// <param name="dt">缓存数据</param>
        public void Add(DateTime time, DataTable dt)
        {
            try
            {
                this.readerWriterLock_0.AcquireWriterLock(0x7530);
                try
                {
                    this.hashtable_0.Add(time, dt);
                }
                finally
                {
                    this.readerWriterLock_0.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
            }
        }

        /// <summary>
        /// 根据用户登录ID添加缓存数据
        /// </summary>
        /// <param name="workid">登录ID</param>
        /// <param name="dt">缓存数据</param>
        public void AddByWorkId(int workid, DataTable dt)
        {
            try
            {
                this.readerWriterLock_0.AcquireWriterLock(30000);
                //Trace.Write("appserver - UpBuffer, AddByWorkId  workid = " + workid.ToString());
                try
                {
                    if (this.hashtable_0.ContainsKey(workid))
                    {
                        (this.hashtable_0[workid] as ArrayList).Add(dt);
                    }
                    else
                    {
                        ArrayList list2 = ArrayList.Synchronized(new ArrayList());
                        list2.Add(dt);
                        this.hashtable_0.Add(workid, list2);
                    }
                }
                finally
                {
                    this.readerWriterLock_0.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
            }
        }

        public void Clear()
        {
            if ((this.hashtable_0 != null) && (this.hashtable_0.Count > 0))
            {
                this.hashtable_0.Clear();
            }
        }

        public void Delete()
        {
            if (this.Count > 0)
            {
                try
                {
                    this.readerWriterLock_0.AcquireWriterLock(0x7530);
                    try
                    {
                        ArrayList list = new ArrayList();
                        foreach (DateTime time in this.hashtable_0.Keys)
                        {
                            TimeSpan span = (TimeSpan) (DateTime.Now - time);
                            if (span.TotalMinutes >= 3.0)
                            {
                                list.Add(time);
                            }
                        }
                        for (int i = 0; i <= (list.Count - 1); i++)
                        {
                            this.hashtable_0.Remove(list[i]);
                        }
                    }
                    finally
                    {
                        this.readerWriterLock_0.ReleaseWriterLock();
                    }
                }
                catch (ApplicationException)
                {
                }
            }
        }

        public void Delete(Hashtable hashtable_1)
        {
            if ((hashtable_1 == null) || (hashtable_1.Count <= 0))
            {
                this.Clear();
            }
            try
            {
                this.readerWriterLock_0.AcquireWriterLock(0x7530);
                try
                {
                    ArrayList list = new ArrayList();
                    foreach (int num in this.hashtable_0.Keys)
                    {
                        if (!hashtable_1.ContainsKey(num))
                        {
                            list.Add(num);
                        }
                    }
                    for (int i = 0; i <= (list.Count - 1); i++)
                    {
                        this.hashtable_0.Remove(list[i]);
                    }
                    list.Clear();
                }
                finally
                {
                    this.readerWriterLock_0.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
            }
        }

        public DataTable GetAlarmData(OnlineUserInfo onlineUserInfo_0)
        {
            DataTable cloneDataTableColumn = UpdataStruct.CloneDataTableColumn;
            DateTime newAlarmTime = onlineUserInfo_0.NewAlarmTime;
            int num = 0;
            DataTable table2 = null;
            try
            {
                this.readerWriterLock_0.AcquireReaderLock(0x1388);
                try
                {
                    foreach (DateTime time2 in this.hashtable_0.Keys)
                    {
                        if (newAlarmTime.CompareTo(time2) < 0)
                        {
                            if (onlineUserInfo_0.NewAlarmTime.CompareTo(time2) == -1)
                            {
                                onlineUserInfo_0.NewAlarmTime = time2;
                            }
                            table2 = this.hashtable_0[time2] as DataTable;
                            foreach (DataRow row in table2.Rows)
                            {
                                if (row["carId"] != DBNull.Value)
                                {
                                    num = Convert.ToInt32(row["carId"]);
                                    if (onlineUserInfo_0.UserCarId.IsExistCarID(num))
                                    {
                                        cloneDataTableColumn.Rows.Add(row.ItemArray);
                                    }
                                }
                            }
                        }
                    }
                    return cloneDataTableColumn;
                }
                finally
                {
                    this.readerWriterLock_0.ReleaseReaderLock();
                }
            }
            catch (ApplicationException)
            {
            }
            return cloneDataTableColumn;
        }

        public DataTable GetData(ref DateTime dateTime_0)
        {
            DataTable table = new DataTable("data");
            DateTime time = dateTime_0;
            try
            {
                //Trace.Write("appserver - UpBuffer, GetData: " + dateTime_0.ToString());
                this.readerWriterLock_0.AcquireReaderLock(0x1388);
                try
                {
                    foreach (DateTime time2 in this.hashtable_0.Keys)
                    {
                        if (dateTime_0.CompareTo(time2) < 0)
                        {
                            if (time.CompareTo(time2) == -1)
                            {
                                time = time2;
                            }
                            table.Merge(this.hashtable_0[time2] as DataTable);
                        }
                    }
                }
                finally
                {
                    this.readerWriterLock_0.ReleaseReaderLock();
                }
            }
            catch (ApplicationException)
            {
            }
            dateTime_0 = time;
            return table;
        }

        /// <summary>
        /// 根据客户端连接ID获取数据
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        public DataTable GetDataByWorkId(int workId)
        {
            DataTable table = new DataTable("data");
            ArrayList list = this.hashtable_0[workId] as ArrayList;
            //Trace.Write("appserver - UpBuffer, GetDatByWorkId: " + workId.ToString());
            if (list != null)
            {
                try
                {
                    this.readerWriterLock_0.AcquireReaderLock(0x1388);
                    try
                    {
                        for (int i = 0; i <= (list.Count - 1); i++)
                        {
                            table.Merge(list[i] as DataTable);
                        }
                        this.hashtable_0.Remove(workId);
                    }
                    finally
                    {
                        this.readerWriterLock_0.ReleaseReaderLock();
                    }
                }
                catch (ApplicationException)
                {
                }
            }
            return table;
        }

        public DataTable GetPictureData(OnlineUserInfo onlineUserInfo_0)
        {
            DataTable cloneDataTableColumn = UpdataStruct.CloneDataTableColumn;
            DateTime newPicTime = onlineUserInfo_0.NewPicTime;
            DataTable table2 = null;
            try
            {
                this.readerWriterLock_0.AcquireReaderLock(0x1388);
                try
                {
                    foreach (DateTime time2 in this.hashtable_0.Keys)
                    {
                        if (newPicTime.CompareTo(time2) < 0)
                        {
                            if (onlineUserInfo_0.NewPicTime.CompareTo(time2) == -1)
                            {
                                onlineUserInfo_0.NewPicTime = time2;
                            }
                            table2 = this.hashtable_0[time2] as DataTable;
                            foreach (DataRow row in table2.Rows)
                            {
                                if ((((row["carId"] != DBNull.Value) && onlineUserInfo_0.UserCarId.IsExistCarID(Convert.ToInt32(row["carId"]))) && ((row["SimNum"] != DBNull.Value) && (row["status"] != DBNull.Value))) && (this.method_0(long.Parse(row["status"].ToString())) || onlineUserInfo_0.CarFilter.CarFilterList.ContainsKey(row["SimNum"] as string)))
                                {
                                    cloneDataTableColumn.Rows.Add(row.ItemArray);
                                }
                            }
                        }
                    }
                    return cloneDataTableColumn;
                }
                finally
                {
                    this.readerWriterLock_0.ReleaseReaderLock();
                }
            }
            catch (ApplicationException)
            {
            }
            return cloneDataTableColumn;
        }

        private bool method_0(long long_0)
        {
            if ((this.list_0 != null) && (this.list_0.Count > 0))
            {
                foreach (int num in this.list_0)
                {
                    if ((long_0 & num) != 0L)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void RemoveAt(int int_0)
        {
            if ((this.hashtable_0 != null) && this.hashtable_0.ContainsKey(int_0))
            {
                this.hashtable_0.Remove(int_0);
            }
        }

        public string AlarmCodeList
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    string[] strArray = value.Split(new char[] { ';' });
                    if (strArray.Length > 0)
                    {
                        for (int i = 0; i <= (strArray.Length - 1); i++)
                        {
                            try
                            {
                                int item = int.Parse(strArray[i]);
                                this.list_0.Add(item);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }

        public int Count
        {
            get
            {
                return this.hashtable_0.Count;
            }
        }
    }
}

