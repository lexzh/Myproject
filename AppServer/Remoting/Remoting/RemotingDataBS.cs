namespace Remoting
{
    using ParamLibrary.Bussiness;
    using ParamLibrary.GpsEntity;
    using Library;
    using System;
    using System.Collections;
    using System.Data;
    using Bussiness;
    using PublicClass;

    public class RemotingDataBS : DataBase
    {
        public override void Add(DataTable data)
        {
            if ((data != null) && (data.Rows.Count > 0))
            {
                GpsDataTable table = new GpsDataTable(UpdataStruct.ColNameList);
                try
                {
                    int num = 1;
                    for (int i = 0; i <= (data.Rows.Count - 1); i++)
                    {
                        table.InsertRows(data.Rows[i].ItemArray);
                        if ((num >= base._Size) || (i == (data.Rows.Count - 1)))
                        {
                            lock (base._PosList.SyncRoot)
                            {
                                base._PosList.Add(CompressHelper.CompressToSelf(table));
                            }
                            table.Rows.Clear();
                            num = 0;
                        }
                        num++;
                    }
                }
                catch (Exception exception)
                {
                    ErrorMsg msg = new ErrorMsg("RemotingDataBS", "Add", exception.Message + exception.StackTrace);
                    new LogHelper().WriteError(msg);
                    base._PosList = new ArrayList(100);
                }
            }
        }

        public GpsDataTable ConvertToBSDataTable(DataTable data)
        {
            if ((data == null) || (data.Rows.Count <= 0))
            {
                return null;
            }
            GpsDataTable table = new GpsDataTable(UpdataStruct.ColNameList);
            for (int i = 0; i <= (data.Rows.Count - 1); i++)
            {
                table.InsertRows(data.Rows[i].ItemArray);
            }
            return table;
        }
    }
}

