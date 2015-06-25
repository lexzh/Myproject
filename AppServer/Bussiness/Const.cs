namespace Bussiness
{
    using Library;
    using System;

    public class Const
    {
        public const int AccStatus = 0x4000;
        public static string ADCUrl;
        public const string COMM_GPRS_OR_CDMA = "GPRS/CDMA";
        public const string COMM_SMS = "短信方式";
        public static string communicationUrl;
        public const int CON_ALARM_TYPE_ALL_ALARM = 0;
        public const int CON_ALARM_TYPE_CLOSE = 3;
        public const int CON_ALARM_TYPE_ONCE_ALARM = 1;
        public const int CON_ALARM_TYPE_RED_COLOR = 2;
        public const int CON_CALL_CONFIRM = 0;
        public const int CON_CALL_OVERTIME = 3;
        public const int CON_CALL_REFUSE = 1;
        public static string CorpName;
        public static string CustomInfo;
        public const int DUTY_OFF = 0;
        public const int DUTY_ON = 1;
        public const int FLAG_GPRS_CDMA = 2;
        public const int FLAG_SMS = 1;
        public static string GlsIp;
        public static string GlsPort;
        public const int GPS_ERR_COMM = 0xf041;
        public static string LogSaveDate;
        public const long MAX_ORDERID = 0x2710L;
        public static string ModuleId;
        public const int OLE_ERROR_MASK = 0x4000;
        public static string RemotingServerIP1;
        public static string RemotingServerIP2;
        public static string RemotingServerPort1;
        public static string RemotingServerPort2;
        public const int RESP_CALL_PHONE = 0x387;
        public const int RESP_CAR_SYSTEM = 0xffff;
        public const int RESP_SET_CUSTOM_ALARMER = 0x1bc;
        public const int RESP_TEXT_TEMPERATURE = 3;
        public static string sEMPId;
        public static string sProductID;
        public static string sSystemId;
        public static string StandbyGlsIp;
        public static string StandbyGlsPort;
        public static string Title;
        public static string Version;
        public static string sMapAddr;
        public static string sMapName;

        static Const()
        {
            communicationUrl = FileHelper.ReadParamFromXml("communicationUrl");
            RemotingServerPort1 = FileHelper.ReadParamFromXml("Port1");
            RemotingServerIP1 = FileHelper.ReadParamFromXml("IP1");
            RemotingServerPort2 = FileHelper.ReadParamFromXml("Port2");
            RemotingServerIP2 = FileHelper.ReadParamFromXml("IP2");
            LogSaveDate = FileHelper.ReadParamFromXml("LogSaveDate");
            GlsIp = FileHelper.ReadParamFromXml("GlsIp");
            GlsPort = FileHelper.ReadParamFromXml("GlsPort");
            StandbyGlsIp = FileHelper.ReadParamFromXml("StandbyGlsIp");
            StandbyGlsPort = FileHelper.ReadParamFromXml("StandbyGlsPort");
            CorpName = FileHelper.ReadParamFromXml("CorpName");
            Title = FileHelper.ReadParamFromXml("Title");
            Version = FileHelper.ReadParamFromXml("Version");
            CustomInfo = FileHelper.ReadParamFromXml("CustomInfo");
            //ADCUrl = FileHelper.ReadParamFromXml("ADCUrl");
            ModuleId = FileHelper.ReadParamFromXml("ModuleId");
            sSystemId = "P20100115165302";
            sEMPId = "P20121121092847";
            sProductID = "yunyingshangchanpinmingc";
            sMapAddr = FileHelper.ReadParamFromXml("MapAddress");
            sMapName = FileHelper.ReadParamFromXml("MapName");
        }

        public static string AlarmCodeList
        {
            get
            {
                string str = string.Empty;
                try
                {
                    str = FileHelper.ReadParamFromXml("AlarmCode");
                }
                catch
                {
                }
                return str;
            }
        }

        public static string IsNeedPWDUpdate
        {
            get
            {
                string str = string.Empty;
                try
                {
                    str = FileHelper.ReadParamFromXml("IsNeedPWDUpdate");
                }
                catch
                {
                }
                if (string.IsNullOrEmpty(str))
                {
                    str = "0";
                }
                return str;
            }
        }

        public static string UpdateFilePath
        {
            get
            {
                return FileHelper.ReadParamFromXml("UpdateFilePath");
            }
        }

        public static string UpdateFileVersion
        {
            get
            {
                return FileHelper.ReadParamFromXml("UpdateFileVersion");
            }
        }
    }
}

