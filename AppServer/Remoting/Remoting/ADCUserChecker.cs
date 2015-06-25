using Remoting.ADCCheckUser;
namespace Remoting
{
    using ParamLibrary.Entity;
    using System;

    public sealed class ADCUserChecker
    {
        private string AdcUrl;
        private IGPSM2MDataServer ADCUserServer;
        public string ecCode;
        public string userId;
        public string userPassword;
        private const int webserviceAccessTimeout = 0x2710;

        public ADCUserChecker(string adcUrl)
        {
            this.AdcUrl = string.Empty;
            this.ecCode = string.Empty;
            this.userId = string.Empty;
            this.userPassword = string.Empty;
            this.AdcUrl = adcUrl;
            this.ADCUserServer = new IGPSM2MDataServer();
            this.ADCUserServer.Url = this.AdcUrl;
            this.ADCUserServer.Timeout = 0x2710;
        }

        public ADCUserChecker(string adcUrl, string ecCode, string userId, string userPassword) : this(adcUrl)
        {
            this.ecCode = ecCode;
            this.userId = userId;
            this.userPassword = userPassword;
        }

        public string ADCCheckUser()
        {
            string str = string.Empty;
            GpsResponse response = this.ADCUserServer.LoginVerify(this.ecCode, this.userId, this.userPassword);
            if (response.ResultCode != 0)
            {
                str = response.ResultCode + ":" + response.ResultMsg;
            }
            return str;
        }

        public string ADCCheckUser(string old)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            str2 = this.ADCUserServer.CheckUser(this.ecCode, this.userId, this.userPassword, false);
            if ("0".Equals(str2))
            {
                return str;
            }
            switch (str2)
            {
                case "701":
                    return (str2 + "-集团客户不存在");

                case "702":
                    return (str2 + "-用户不存在");

                case "703":
                    return (str2 + "-密码不符");

                case "704":
                    return (str2 + "-集团客户未审核或已注销或已冻结");

                case "705":
                    return (str2 + "-用户已注销或已冻结");

                case "-1":
                    return (str2 + "-平台用户不存在(用户不存在或密码错误或用户已停用)");

                case "-2":
                    return (str2 + "-未定义错误的异常信息");

                case "-5":
                    return (str2 + "－administrator为内置帐户不允许登录");
            }
            return (str2 + "-ADC鉴权服务器连接失败");
        }

        public Response ADCCheckUserAndSendMSG(string verifyCode)
        {
            GpsResponse response = this.ADCUserServer.ADCCheckUser(this.ecCode, this.userId, this.userPassword, verifyCode);
            return new Response { ResultCode = response.ResultCode, ErrorMsg = response.ResultMsg };
        }
    }
}

