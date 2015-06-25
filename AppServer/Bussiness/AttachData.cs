namespace Bussiness
{
    using PublicClass;
    using System;
    using Library;

    public class AttachData
    {
        private string string_0;
        private string string_1;

        public AttachData()
        {
        }

        public AttachData(string string_2)
        {
            this.string_0 = string_2;
        }

        public AttachData(string string_2, string string_3) : this(string_3)
        {
            this.string_1 = string_2;
        }

        private string AddMsgText(string flag, string value)
        {
            string str = string.Empty;
            switch (flag)
            {
                case "0":
                    return string.Format("[下车：{0}],", float.Parse(value));

                case "1":
                    return string.Format("[当前油量：{0}升],", float.Parse(value) / 100f);

                case "2":
                {
                    string str2 = "";
                    if ((Convert.ToUInt32(value) & 0x80000000) == 0)
                    {
                        return string.Format("[当前温度：{0}度],", ((double) float.Parse(value)) / 100.0);
                    }
                    uint num = Convert.ToUInt32(value) & 0xffff;
                    str2 = (((double) Convert.ToInt16(num.ToString("X2").PadLeft(8, '0').Substring(4), 0x10)) / 100.0).ToString();
                    return string.Format("[当前温度：{0}度],", str2);
                }
                case "9":
                    return string.Format("[上车：{0}],", float.Parse(value));

                case "A":
                    return string.Format("[驾驶证号：{0}],", value.Replace("\0", ""));

                case "B":
                    return string.Format("[工程车工作时间：{0}],", value);

                case "C":
                {
                    AttachParser parser = new CommonOverSpeedPathID(value, this.string_1) {
                        MessageAlarmText = value
                    };
                    return string.Format("[{0}],", parser.Parse());
                }
                case "01":
                {
                    double num2 = NumHelper.Convert16To10(value.Substring(0, 8)) * 0.01;
                    return string.Format("[油量：{0}],", num2.ToString());
                }
                case "0D":
                {
                    //多边形ID
                    AttachParser parser2 = new OverSpeedAlarm {
                        MessageAlarmText = value
                    };
                    return string.Format("[{0}],", parser2.Parse());
                }
                case "0A":
                {
                    AttachParser parser3 = new OverSpeedPathID {
                        MessageAlarmText = value
                    };
                    return string.Format("[{0}],", parser3.Parse());
                }
                case "0E":
                {
                    AttachParser parser4 = new RegionInOutAlarm {
                        MessageAlarmText = value
                    };
                    return string.Format("[{0}],", parser4.Parse());
                }
                case "0F":
                {
                    AttachParser parser5 = new RunTimeAlarm {
                        MessageAlarmText = value
                    };
                    return string.Format("[{0}],", parser5.Parse());
                }
                case "10":
                {
                    AttachParser parser6 = new RunSpeedAlarm {
                        MessageAlarmText = value
                    };
                    return string.Format("[{0}],", parser6.Parse());
                }
                case "12":
                {
                    AttachParser parser7 = new ArtificialAlarm {
                        MessageAlarmText = value
                    };
                    return string.Format("[{0}],", parser7.Parse());
                }
                case "13":
                {
                    AttachParser parser8 = new CustomMessage {
                        MessageAlarmText = value
                    };
                    return string.Format("[{0}],", parser8.Parse());
                }
                case "15":
                {
                    AttachParser parser9 = new CarNotBeBackOnTime {
                        MessageAlarmText = value
                    };
                    return string.Format("[{0}],", parser9.Parse());
                }
                case "19":
                {
                    AttachParser parser10 = new HumidityData {
                        MessageAlarmText = value
                    };
                    return string.Format("[{0}],", parser10.Parse());
                }
                case "20":
                {
                    AttachParser parser11 = new SiteMap {
                        MessageAlarmText = value
                    };
                    return string.Format("[{0}],", parser11.Parse());
                }
                case "E6":
                    return ("超速限值:" + Convert.ToInt32(value, 0x10).ToString() + "km/h");
            }
            return str;
        }

        public string ParseAttachData()
        {
            string[] strArray = this.AttachSourceData.Split(new char[] { '/' });
            string str = string.Empty;
            string str2 = string.Empty;
            string str3 = string.Empty;
            for (int i = 0; i <= (strArray.Length - 1); i++)
            {
                try
                {
                    int startIndex = 1;
                    str = strArray[i].Substring(0, 1);
                    if (str.ToUpper().Equals("M"))
                    {
                        str = strArray[i].Substring(1, 2);
                        startIndex = 3;
                    }
                    str2 = strArray[i].Substring(startIndex);
                    str3 = str3 + this.AddMsgText(str, str2);
                }
                catch
                {
                }
            }
            return str3;
        }

        public string AttachSourceData
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
    }
}

