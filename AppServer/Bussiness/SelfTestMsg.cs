namespace Bussiness
{
    using System;
    using System.Runtime.CompilerServices;

    public class SelfTestMsg
    {
        private int int_0 = -1;
        [CompilerGenerated]
        private int int_1;
        [CompilerGenerated]
        private int int_2;
        [CompilerGenerated]
        private int int_3;
        [CompilerGenerated]
        private int int_4;
        [CompilerGenerated]
        private int int_5;
        [CompilerGenerated]
        private int int_6;
        [CompilerGenerated]
        private int int_7;
        private string string_0 = string.Empty;

        public string GetCheckTestMsg()
        {
            if (this.SelfType == -1)
            {
                return ("显示屏：" + this.method_0(this.SCREENstatus) + "，网络信号：" + this.method_0(this.NetworkStatus) + "，卫星状态：" + this.method_0(this.StarStatus) + "，RAM状态：" + this.method_0(this.RAMstatus) + "，\r\nGPS状态：" + this.method_0(this.GPSstatus) + "，GSM状态：" + this.method_0(this.GSMstatus) + "，手柄状态：" + this.method_0(this.PhoneStatus) + "，\r\n" + this.Context);
            }
            return this.Context;
        }

        private string method_0(int int_8)
        {
            string str = "正常";
            if (int_8 == 1)
            {
                return str;
            }
            return "不正常";
        }

        public string Context
        {
            get
            {
                return this.string_0;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && (value.IndexOf(',') != -1))
                {
                    string s = value.Substring(0, value.IndexOf(','));
                    int result = -1;
                    if (int.TryParse(s, out result))
                    {
                        this.SelfType = result;
                    }
                }
                if (this.SelfType != -1)
                {
                    this.string_0 = value.Substring(value.IndexOf(',') + 1);
                }
                else
                {
                    this.string_0 = value;
                }
            }
        }

        public int GPSstatus
        {
            [CompilerGenerated]
            get
            {
                return this.int_3;
            }
            [CompilerGenerated]
            set
            {
                this.int_3 = value;
            }
        }

        public int GSMstatus
        {
            [CompilerGenerated]
            get
            {
                return this.int_4;
            }
            [CompilerGenerated]
            set
            {
                this.int_4 = value;
            }
        }

        public int NetworkStatus
        {
            [CompilerGenerated]
            get
            {
                return this.int_5;
            }
            [CompilerGenerated]
            set
            {
                this.int_5 = value;
            }
        }

        public int PhoneStatus
        {
            [CompilerGenerated]
            get
            {
                return this.int_7;
            }
            [CompilerGenerated]
            set
            {
                this.int_7 = value;
            }
        }

        public int RAMstatus
        {
            [CompilerGenerated]
            get
            {
                return this.int_2;
            }
            [CompilerGenerated]
            set
            {
                this.int_2 = value;
            }
        }

        public int SCREENstatus
        {
            [CompilerGenerated]
            get
            {
                return this.int_1;
            }
            [CompilerGenerated]
            set
            {
                this.int_1 = value;
            }
        }

        public int SelfType
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

        public string SelfTypeName
        {
            get
            {
                switch (this.SelfType)
                {
                    case 1:
                        return "终端自检数据";

                    case 2:
                        return "终端状态数据";

                    case 3:
                        return "税控数据";

                    case 4:
                        return "终端关机上传数据";

                    case 5:
                        return "油耗资料";

                    case 6:
                        return "行驶记录管理";

                    case 7:
                        return "图像名称查询";
                }
                return "车台自检响应";
            }
        }

        public int StarStatus
        {
            [CompilerGenerated]
            get
            {
                return this.int_6;
            }
            [CompilerGenerated]
            set
            {
                this.int_6 = value;
            }
        }
    }
}

