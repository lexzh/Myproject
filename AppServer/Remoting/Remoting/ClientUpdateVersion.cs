namespace Remoting
{
    using ParamLibrary.Entity;
    using Library;
    using System;

    public sealed class ClientUpdateVersion
    {
        private string configClientVersion;
        private string currentClientVersion;
        private string reomtingUpdateFileUrl;

        public ClientUpdateVersion()
        {
            this.currentClientVersion = string.Empty;
            this.configClientVersion = string.Empty;
            this.reomtingUpdateFileUrl = string.Empty;
        }

        public ClientUpdateVersion(string curretnClientVersion, string configClientVersion, string remotingUpdateFileUrl)
        {
            this.currentClientVersion = string.Empty;
            this.configClientVersion = string.Empty;
            this.reomtingUpdateFileUrl = string.Empty;
            this.CurrentClientVersion = curretnClientVersion;
            this.ConfigClientVersion = configClientVersion;
            this.reomtingUpdateFileUrl = remotingUpdateFileUrl;
        }

        public Response GetUpdateClientList()
        {
            Response response = new Response();
            try
            {
                if (string.IsNullOrEmpty(this.reomtingUpdateFileUrl))
                {
                    return response;
                }
                if (string.IsNullOrEmpty(this.ConfigClientVersion))
                {
                    return response;
                }
                if (!this.IsCorrestVerersion())
                {
                    response.ErrorMsg = "配置的版本号格式不正确！";
                    return response;
                }
                string str = this.ConfigClientVersion.Substring(1);
                string[] strArray = str.Split(new char[] { '.' });
                if (strArray.Length == 2)
                {
                    str = str + ".0.0";
                }
                if (strArray.Length == 3)
                {
                    str = str + ".0";
                }
                if (str.CompareTo(this.CurrentClientVersion) > 0)
                {
                    response.ResultCode = 0L;
                    response.SvcContext = this.ReomtingUpdateFileUrl + "|true";
                }
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingLogin", "getUpdateClientList", exception.Message);
                new LogHelper().WriteError(msg);
            }
            return response;
        }

        private bool IsCorrestVerersion()
        {
            if (string.IsNullOrEmpty(this.ConfigClientVersion))
            {
                return false;
            }
            if ((this.ConfigClientVersion.IndexOf('V') == -1) && (this.ConfigClientVersion.IndexOf('v') == -1))
            {
                return false;
            }
            string s = this.ConfigClientVersion.Substring(1).Replace(".", "");
            int result = -1;
            return int.TryParse(s, out result);
        }

        public string ConfigClientVersion
        {
            get
            {
                return this.configClientVersion;
            }
            set
            {
                this.configClientVersion = value;
            }
        }

        public string CurrentClientVersion
        {
            get
            {
                return this.currentClientVersion;
            }
            set
            {
                this.currentClientVersion = value;
            }
        }

        public string ReomtingUpdateFileUrl
        {
            get
            {
                return this.reomtingUpdateFileUrl;
            }
            set
            {
                this.reomtingUpdateFileUrl = value;
            }
        }
    }
}

