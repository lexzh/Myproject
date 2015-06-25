namespace Protocol
{
    using System;
    using System.Data;

    public class ProtocolDrData
    {
        protected string contextData;
        protected int equipmentId;
        protected string responseType;
        protected DataRow sourceDatarowData;

        public ProtocolDrData(DataRow dataRow_0)
        {
            this.SourceData = dataRow_0;
        }

        private string getResponseType(string string_0)
        {
            if (!string.IsNullOrEmpty(string_0) && (string_0.Length >= 4))
            {
                return string_0.Substring(0, 4);
            }
            return "";
        }

        public string ContextData
        {
            get
            {
                return this.contextData;
            }
            set
            {
                this.contextData = value;
            }
        }

        public int EquipId
        {
            get
            {
                return this.equipmentId;
            }
            set
            {
                this.equipmentId = value;
            }
        }

        public string ResponseType
        {
            get
            {
                return this.responseType;
            }
            set
            {
                this.responseType = value;
            }
        }

        public DataRow SourceData
        {
            get
            {
                return this.sourceDatarowData;
            }
            set
            {
                this.sourceDatarowData = value;
                this.EquipId = Convert.ToInt32(value["equipId"]);
                string str = Convert.ToString(value["PropertyData"]);
                if (this.EquipId != 13)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        this.ContextData = str.Substring(4);
                        this.ResponseType = this.getResponseType(str);
                    }
                }
                else
                {
                    this.ContextData = str;
                }
            }
        }
    }
}

