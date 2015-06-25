namespace Bussiness
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class UpdataStruct
    {
        private static Dictionary<string, int> _list;
        private static DataTable m_GetDTColumn;


        /// <summary>
        /// 更新定位数据结构，对应存储过程WebGpsClient_GetCurrentPosData获取的列
        /// 将需要的值传递到客户端接口
        /// </summary>
        static UpdataStruct()
        {
            m_GetDTColumn = null;
            _list = new Dictionary<string, int>(0x4f);
            if (m_GetDTColumn == null)
            {
                m_GetDTColumn = new DataTable("GpsRealTimePos");
                m_GetDTColumn.Columns.Add(new DataColumn("GpsTime"));
                m_GetDTColumn.Columns.Add(new DataColumn("OrderID"));
                m_GetDTColumn.Columns.Add(new DataColumn("CarNum"));
                m_GetDTColumn.Columns.Add(new DataColumn("OrderType"));
                m_GetDTColumn.Columns.Add(new DataColumn("MsgType"));
                m_GetDTColumn.Columns.Add(new DataColumn("OrderName"));
                m_GetDTColumn.Columns.Add(new DataColumn("OrderResult"));
                m_GetDTColumn.Columns.Add(new DataColumn("CommFlag"));
                m_GetDTColumn.Columns.Add(new DataColumn("Describe"));
                m_GetDTColumn.Columns.Add(new DataColumn("Longitude"));
                m_GetDTColumn.Columns.Add(new DataColumn("Latitude"));
                m_GetDTColumn.Columns.Add(new DataColumn("IsShowTrace"));
                m_GetDTColumn.Columns.Add(new DataColumn("IsShowAlarm"));
                m_GetDTColumn.Columns.Add(new DataColumn("IsImportWatch"));
                m_GetDTColumn.Columns.Add(new DataColumn("CarId"));
                m_GetDTColumn.Columns.Add(new DataColumn("WrkId"));
                m_GetDTColumn.Columns.Add(new DataColumn("CameraId"));
                m_GetDTColumn.Columns.Add(new DataColumn("AccOn"));
                m_GetDTColumn.Columns.Add(new DataColumn("AlarmType"));
                m_GetDTColumn.Columns.Add(new DataColumn("IsFill"));
                m_GetDTColumn.Columns.Add(new DataColumn("GpsValid"));
                m_GetDTColumn.Columns.Add(new DataColumn("Speed"));
                m_GetDTColumn.Columns.Add(new DataColumn("CarStatus"));
                m_GetDTColumn.Columns.Add(new DataColumn("SimNum"));
                m_GetDTColumn.Columns.Add(new DataColumn("SvrTime"));
                m_GetDTColumn.Columns.Add(new DataColumn("RespCode"));
                m_GetDTColumn.Columns.Add(new DataColumn("StatuDesc"));
                m_GetDTColumn.Columns.Add(new DataColumn("StatuName"));
                m_GetDTColumn.Columns.Add(new DataColumn("ReadPicTime"));
                m_GetDTColumn.Columns.Add(new DataColumn("ReceTime"));
                m_GetDTColumn.Columns.Add(new DataColumn("Distance"));
                foreach (DataColumn column in m_GetDTColumn.Columns)
                {
                    column.DefaultValue = "";
                }
                m_GetDTColumn.Columns.Add(new DataColumn("Direct", typeof(int)));
                m_GetDTColumn.Columns["Direct"].DefaultValue = 0;
                m_GetDTColumn.Columns.Add(new DataColumn("Status"));
                m_GetDTColumn.Columns["Status"].DefaultValue = "";
                m_GetDTColumn.Columns.Add(new DataColumn("StatusEx", typeof(long)));
                m_GetDTColumn.Columns["Status"].DefaultValue = 0;
                m_GetDTColumn.Columns.Add(new DataColumn("PicDataType"));
                m_GetDTColumn.Columns["PicDataType"].DefaultValue = 0;
                m_GetDTColumn.Columns.Add(new DataColumn("OBJECT_TYPE"));
                m_GetDTColumn.Columns["OBJECT_TYPE"].DefaultValue = 0;
                m_GetDTColumn.Columns.Add(new DataColumn("OBJECT_ID"));
                m_GetDTColumn.Columns["OBJECT_ID"].DefaultValue = "";
                //AddMsgTxt 添加附加消息用户客户端解析, 对应UpdataCommon.SetUpdataPosData  huzh 2014.1.6
                m_GetDTColumn.Columns.Add(new DataColumn("AddMsgTxt"));
                m_GetDTColumn.Columns["AddMsgTxt"].DefaultValue = "";
                for (int i = 0; i < m_GetDTColumn.Columns.Count; i++)
                {
                    _list.Add(m_GetDTColumn.Columns[i].ColumnName, i);
                }
            }
        }

        public static DataTable CloneDataTableColumn
        {
            get
            {
                return m_GetDTColumn.Clone();
            }
        }

        public static Dictionary<string, int> ColNameList
        {
            get
            {
                return _list;
            }
        }
    }
}

