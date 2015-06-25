namespace Remoting
{
    using System;
    using System.Collections;
    using System.Data;
    using PublicClass;
    using Library;

    public class RemotingDataCS : DataBase
    {
        public override void Add(DataTable data)
        {
            try
            {
                if ((data != null) && (data.Rows.Count <= base._Size))
                {
                    lock (base._PosList.SyncRoot)
                    {
                        base._PosList.Add(CompressHelper.Compress(data));
                        return;
                    }
                }
                this.DataDisBach(data);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingDataCS", "Add", exception.Message + exception.StackTrace);
                new LogHelper().WriteError(msg);
                base._PosList = new ArrayList(100);
            }
        }

        private void DataDisBach(DataTable data)
        {
            if ((data != null) && (data.Rows.Count > 0))
            {
                DataTable table = data.Clone();
                int num = 1;
                for (int i = 0; i <= (data.Rows.Count - 1); i++)
                {
                    table.Rows.Add(data.Rows[i].ItemArray);
                    if ((num >= base._Size) || (i == (data.Rows.Count - 1)))
                    {
                        lock (base._PosList.SyncRoot)
                        {
                            base._PosList.Add(CompressHelper.Compress(table.Copy()));
                        }
                        table.Clear();
                        num = 0;
                    }
                    num++;
                }
            }
        }
    }
}

