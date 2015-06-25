namespace Remoting
{
    using ParamLibrary.Bussiness;
    using Library;
    using System;
    using System.Collections;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Http;
    using System.Runtime.Remoting.Channels.Tcp;
    using System.Runtime.Remoting.Services;
    using System.Runtime.Serialization.Formatters;
    using System.Threading;
    using Bussiness;

    public class RemotingManager
    {
        private const string _ChannelName = "ClientServerChannel";
        private const string _ExceptionMessage = "通常每个套接字地址(协议/网络地址/端口)只允许使用一次";
        private const string _ServerObjectUrl = "RemotingServer";
        private const int _timeOut = 180;
        private const int _TryNum = 5;
        private const int _TrySleepTime = 0x4e20;

        private bool IsRemotingPortHadRegist(string exMessage)
        {
            return (!string.IsNullOrEmpty(exMessage) && (exMessage.IndexOf("通常每个套接字地址(协议/网络地址/端口)只允许使用一次") != -1));
        }

        public void RegChannel()
        {
            if (!this.HasRegChannel)
            {
                string str = Const.RemotingServerPort1;
                if (!string.IsNullOrEmpty(str))
                {
                    foreach (string str2 in str.Split(new char[] { ';' }))
                    {
                        this.RegTcpChannel(str2, str2);
                    }
                }
                str = Const.RemotingServerPort2;
                if (!string.IsNullOrEmpty(str))
                {
                    foreach (string str3 in str.Split(new char[] { ';' }))
                    {
                        this.RegHttpChannel(str3, str3);
                    }
                }
                this.RemotingSetConfig();
            }
        }

        private void RegHttpChannel(string port, string name)
        {
            try
            {
                HttpChannel chnl = new HttpChannel(int.Parse(port));
                ChannelServices.RegisterChannel(chnl, false);
            }
            catch (Exception exception)
            {
                ErrorMsg msg = new ErrorMsg("RemotingManager", "RegHttpChannel", exception.Message);
                new LogHelper().WriteError(msg);
            }
        }

        private void RegTcpChannel(string port, string name)
        {
            IDictionary properties = new Hashtable();
            properties["port"] = port;
            properties["name"] = name;
            properties["timeout"] = 180;
            int num = 1;
        Label_0035:
            try
            {
                this.UnRegChannel();
                BinaryServerFormatterSinkProvider serverSinkProvider = new BinaryServerFormatterSinkProvider {
                    TypeFilterLevel = TypeFilterLevel.Full
                };
                TcpChannel chnl = new TcpChannel(properties, null, serverSinkProvider);
                ChannelServices.RegisterChannel(chnl, false);
            }
            catch (Exception exception)
            {
                if (this.IsRemotingPortHadRegist(exception.Message))
                {
                    this.UnRegChannel();
                    Thread.Sleep(0x4e20);
                    num++;
                    if (num <= 5)
                    {
                        goto Label_0035;
                    }
                }
                ErrorMsg msg = new ErrorMsg("RemotingManager", "RegTcpChannel", exception.Message);
                new LogHelper().WriteError(msg);
            }
        }

        private void RemotingSetConfig()
        {
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            RemotingConfiguration.CustomErrorsEnabled(false);
            RemotingConfiguration.ApplicationName = "ClientServerChannel";
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemotingLogin), "RemotingServer", WellKnownObjectMode.SingleCall);
            TrackingServices.RegisterTrackingHandler(new IPTrackingHandler());
        }

        public void UnRegChannel()
        {
            foreach (IChannel channel in ChannelServices.RegisteredChannels)
            {
                ChannelServices.UnregisterChannel(channel);
            }
        }

        private bool HasRegChannel
        {
            get
            {
                foreach (IChannel channel in ChannelServices.RegisteredChannels)
                {
                    if ("ClientServerChannel".Equals(channel.ChannelName))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}

