namespace Bussiness
{
    using DataAccess;
    using System;

    public class OnlineUserInfo
    {
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private Bussiness.CarFilter carFilter_0;
        private DateTime dateTime_0;
        private DateTime dateTime_1;
        private DateTime dateTime_2;
        private DateTime dateTime_3;
        private DateTime dateTime_4;
        private DateTime dateTime_5;
        private DownCommand downCommand_0;
        private int int_0;
        private int int_1;
        private int int_2;
        private string string_0;
        private string string_1;
        private string string_2;
        private Bussiness.UserCarId userCarId_0;

        public OnlineUserInfo(int int_3, string string_3, int int_4, int int_5, bool bool_3, bool bool_4, bool bool_5, string string_4)
        {
            DateTime time;
            DateTime time2;
            DateTime time3;
            this.dateTime_5 = DateTime.Now;
            this.int_0 = -1;
            this.string_0 = "";
            this.string_1 = "";
            this.string_2 = string.Empty;
            this.carFilter_0 = new Bussiness.CarFilter();
            this.downCommand_0 = new DownCommand();
            this.userCarId_0 = new Bussiness.UserCarId(string_3);
            this.WorkId = int_3;
            this.UserId = string_3;
            this.ModuleId = int_5;
            this.AllowSelMutil = bool_3;
            this.AllowEmptyPw = bool_4;
            this.SudoOverDue = bool_5;
            this.GroupId = int_4;
            this.RoadTransportID = string_4;
            this.NewAlarmTime = time = new SqlDataAccess().getSystemDate();
            this.NewLogTime = time2 = time;
            this.NewLogIOTime = time3 = time2;
            this.NewOtherDataTime = this.NewPicTime = time3;
        }

        public OnlineUserInfo(int int_3, string string_3, int int_4, int int_5, bool bool_3, bool bool_4, bool bool_5, string string_4, string string_5)
        {
            DateTime time;
            DateTime time2;
            DateTime time3;
            this.dateTime_5 = DateTime.Now;
            this.int_0 = -1;
            this.string_0 = "";
            this.string_1 = "";
            this.string_2 = string.Empty;
            this.carFilter_0 = new Bussiness.CarFilter();
            this.downCommand_0 = new DownCommand();
            this.userCarId_0 = new Bussiness.UserCarId(string_3);
            this.WorkId = int_3;
            this.UserId = string_3;
            this.ModuleId = int_5;
            this.AllowSelMutil = bool_3;
            this.AllowEmptyPw = bool_4;
            this.SudoOverDue = bool_5;
            this.GroupId = int_4;
            this.RoadTransportID = string_4;
            this.AreaCode = string_5;
            this.NewAlarmTime = time = new SqlDataAccess().getSystemDate();
            this.NewLogTime = time2 = time;
            this.NewLogIOTime = time3 = time2;
            this.NewOtherDataTime = this.NewPicTime = time3;
        }

        public string GetRegisterNo(string string_3)
        {
            string format = "select c. RoadTransportID from GpsArea as a inner join GpsUser b on (a.AreaID = b.AreaID and b.UserID='{0}')inner join GpsArea c on CHARINDEX(c.AreaCode,a.AreaCode)=1where   not c.RoadTransportID  is null and  c.RoadTransportID<>''";
            format = string.Format(format, string_3);
            object returnBySql = new SqlDataAccess().GetReturnBySql(format);
            if (returnBySql != null)
            {
                return returnBySql.ToString();
            }
            return "";
        }

        public bool AllowEmptyPw
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
            }
        }

        public bool AllowSelMutil
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

        public string AreaCode
        {
            get
            {
                return this.string_2;
            }
            set
            {
                this.string_2 = value;
            }
        }

        public Bussiness.CarFilter CarFilter
        {
            get
            {
                return this.carFilter_0;
            }
        }

        public DownCommand DownCommd
        {
            get
            {
                return this.downCommand_0;
            }
        }

        public int GroupId
        {
            get
            {
                return this.int_1;
            }
            set
            {
                this.int_1 = value;
            }
        }

        public int ModuleId
        {
            get
            {
                return this.int_2;
            }
            set
            {
                this.int_2 = value;
            }
        }

        public DateTime NewAlarmTime
        {
            get
            {
                return this.dateTime_1;
            }
            set
            {
                this.dateTime_1 = value;
            }
        }

        public DateTime NewLogIOTime
        {
            get
            {
                return this.dateTime_2;
            }
            set
            {
                this.dateTime_2 = value;
            }
        }

        public DateTime NewLogTime
        {
            get
            {
                return this.dateTime_0;
            }
            set
            {
                this.dateTime_0 = value;
            }
        }

        public DateTime NewOtherDataTime
        {
            get
            {
                return this.dateTime_4;
            }
            set
            {
                this.dateTime_4 = value;
            }
        }

        public DateTime NewPicTime
        {
            get
            {
                return this.dateTime_3;
            }
            set
            {
                this.dateTime_3 = value;
            }
        }

        public string RoadTransportID
        {
            get
            {
                return this.string_1;
            }
            set
            {
                this.string_1 = value;
            }
        }

        public bool SudoOverDue
        {
            get
            {
                return this.bool_2;
            }
            set
            {
                this.bool_2 = value;
            }
        }

        public DateTime SynDbUserTime
        {
            get
            {
                return this.dateTime_5;
            }
            set
            {
                this.dateTime_5 = value;
            }
        }

        public Bussiness.UserCarId UserCarId
        {
            get
            {
                return this.userCarId_0;
            }
        }

        public string UserId
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }

        public int WorkId
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
    }
}

