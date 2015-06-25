namespace Bussiness
{
    using System;
    using System.Data;

    public class GpsUpData
    {
        private DataRow dataRow_0;
        private string string_0 = string.Empty;
        private string string_1 = string.Empty;
        private string string_2 = string.Empty;
        private string string_3 = string.Empty;
        private string string_4 = string.Empty;
        private string string_5 = string.Empty;

        public GpsUpData(DataRow dataRow_1)
        {
            this.dataRow_0 = dataRow_1;
        }

        public string CarID
        {
            get
            {
                if (string.IsNullOrEmpty(this.string_4))
                {
                    this.string_4 = Convert.ToString(this.dataRow_0["carid"]);
                }
                return this.string_4;
            }
        }

        public string CarNum
        {
            get
            {
                if (string.IsNullOrEmpty(this.string_2))
                {
                    this.string_2 = Convert.ToString(this.dataRow_0["carnum"]);
                }
                return this.string_2;
            }
        }

        public string GpsTime
        {
            get
            {
                if (string.IsNullOrEmpty(this.string_0))
                {
                    this.string_0 = Convert.ToString(this.dataRow_0["gpstime"]);
                }
                return this.string_0;
            }
        }

        public string OrderID
        {
            get
            {
                if (string.IsNullOrEmpty(this.string_5))
                {
                    this.string_5 = Convert.ToString(this.dataRow_0["orderid"]);
                }
                return this.string_5;
            }
        }

        public string ReceTime
        {
            get
            {
                if (string.IsNullOrEmpty(this.string_1))
                {
                    this.string_1 = Convert.ToString(this.dataRow_0["recetime"]);
                }
                return this.string_1;
            }
        }

        public string SimNum
        {
            get
            {
                if (string.IsNullOrEmpty(this.string_3))
                {
                    this.string_3 = Convert.ToString(this.dataRow_0["phone"]);
                }
                return this.string_3;
            }
        }
    }
}

