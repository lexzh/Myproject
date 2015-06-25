namespace Remoting
{
    using System;

    public class RemotingState
    {
        private string _DataBaseInfo = "";
        private bool _IsConnected = true;
        public const string ConnectedMsg = "数据库已经连接上！";
        public const string DBErrorMsg = "数据库连接出错，详细信息：";
        private bool isOverTime;

        public string DataBaseInfo
        {
            get
            {
                return this._DataBaseInfo;
            }
            set
            {
                this._DataBaseInfo = value;
            }
        }

        public bool IsConnected
        {
            get
            {
                return this._IsConnected;
            }
            set
            {
                this._IsConnected = value;
            }
        }

        public bool IsOutTime
        {
            get
            {
                return this.isOverTime;
            }
            set
            {
                this.isOverTime = value;
            }
        }
    }
}

