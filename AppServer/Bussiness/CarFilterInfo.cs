namespace Bussiness
{
    using System;
    using System.Runtime.CompilerServices;

    public class CarFilterInfo
    {
        private bool bool_0;
        [CompilerGenerated]
        private CarInfo carInfo_0;
        [CompilerGenerated]
        private DateTime dateTime_0;
        [CompilerGenerated]
        private DateTime dateTime_1;
        private int int_0 = -1;
        [CompilerGenerated]
        private string string_0;
        [CompilerGenerated]
        private string string_1;

        public CarInfo CarInfoData
        {
            [CompilerGenerated]
            get
            {
                return this.carInfo_0;
            }
            [CompilerGenerated]
            set
            {
                this.carInfo_0 = value;
            }
        }

        public bool IsPosSearchFlag
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public int OrderId
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }

        public DateTime PicReadTime
        {
            [CompilerGenerated]
            get
            {
                return this.dateTime_1;
            }
            [CompilerGenerated]
            set
            {
                this.dateTime_1 = value;
            }
        }

        public DateTime PosReadTime
        {
            [CompilerGenerated]
            get
            {
                return this.dateTime_0;
            }
            [CompilerGenerated]
            set
            {
                this.dateTime_0 = value;
            }
        }

        public string PosSearchTime
        {
            [CompilerGenerated]
            get
            {
                return this.string_1;
            }
            [CompilerGenerated]
            set
            {
                this.string_1 = value;
            }
        }

        public string SimNum
        {
            [CompilerGenerated]
            get
            {
                return this.string_0;
            }
            [CompilerGenerated]
            set
            {
                this.string_0 = value;
            }
        }
    }
}

