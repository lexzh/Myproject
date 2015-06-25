namespace Remoting
{
    using Library;
    using System;
    using System.Collections;
    using System.Data;

    public class DataBase
    {
        protected ArrayList _PosList = new ArrayList(500);
        protected int _Size = 500;

        public virtual void Add(DataTable data)
        {
        }

        public byte[] Get()
        {
            byte[] buffer = null;
            try
            {
                if (!this.IsExistsBufferData)
                {
                    return buffer;
                }
                lock (this._PosList.SyncRoot)
                {
                    buffer = this._PosList[0] as byte[];
                    this._PosList.RemoveAt(0);
                }
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("DataBase", "Get", exception.Message + exception.StackTrace);
                new LogHelper().WriteError(msg);
                this._PosList = new ArrayList(100);
            }
            return buffer;
        }

        public bool IsExistsBufferData
        {
            get
            {
                return ((this._PosList != null) && (this._PosList.Count > 0));
            }
        }
    }
}

