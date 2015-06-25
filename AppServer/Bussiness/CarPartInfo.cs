namespace Bussiness
{
    using System;
    using System.Runtime.CompilerServices;

    public class CarPartInfo
    {
        public string GetCarCurrentInfo()
        {
            //去掉星数显示，由于部分设备不会上传星数， huzh 2014.1.21
            string format = "ACC[{0}]；车速[{1}km/h]；运营状态[{2}]；卫星时间[{3}]；总里程[{4}公里]；经纬度[{6}, {7}]；车辆状态[{8}]；";//星数[{5}]；
            return string.Format(format, new object[] { this.AccStatus, this.Speed, this.TransportStatu, this.GpsTime, this.DistanceDiff, this.StarNum, this.Lon, this.Lat, this.StatusName });
        }

        public string AccOn
        {
            get;
            set;
        }

        public string AccStatus
        {
            get;
            set;
        }

        public int Direct
        {
            get;
            set;
        }

        public string DistanceDiff
        {
            get;
            set;
        }

        public string GpsTime
        {
            get;
            set;
        }

        public string GpsValid
        {
            get;
            set;
        }

        public string IsFill
        {
            get;
            set;
        }

        public string Lat
        {
            get;
            set;
        }

        public string Lon
        {
            get;
            set;
        }

        public string ReceTime
        {
            get;
            set;
        }

        public string Speed
        {
            get;
            set;
        }

        public string StarNum
        {
            get;
            set;
        }

        public string StatusName
        {
            get;
            set;
        }

        public string TransportStatu
        {
            get;
            set;
        }
    }
}

