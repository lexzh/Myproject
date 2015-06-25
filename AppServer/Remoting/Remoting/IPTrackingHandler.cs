namespace Remoting
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Messaging;
    using System.Runtime.Remoting.Services;
    using System.Security.Permissions;

    public class IPTrackingHandler : ITrackingHandler
    {
        [SecurityPermission(SecurityAction.LinkDemand, Flags=SecurityPermissionFlag.Infrastructure)]
        public void DisconnectedObject(object obj)
        {
        }

        private string[] GetClientAddressFromUri(string uri, string clientIp, string clientPort)
        {
            int length = uri.IndexOf("//") + 2;
            string str = uri.Substring(0, length) + clientIp;
            if (string.IsNullOrEmpty(clientPort))
            {
                length = uri.IndexOf(":", length);
                str = str + uri.Substring(length, uri.Length - length);
            }
            else
            {
                str = str + ":" + clientPort;
            }
            return new string[] { str };
        }

        public void MarshaledObject(object obj, ObjRef or)
        {
            object data = CallContext.GetData("ClientIp");
            if ((data != null) && (or.ChannelInfo != null))
            {
                string ip = "";
                string port = "";
                this.SplitClientAddress(data.ToString(), out ip, out port);
                for (int i = or.ChannelInfo.ChannelData.GetLowerBound(0); i <= or.ChannelInfo.ChannelData.GetUpperBound(0); i++)
                {
                    if (or.ChannelInfo.ChannelData[i] is ChannelDataStore)
                    {
                        foreach (string str3 in ((ChannelDataStore) or.ChannelInfo.ChannelData[i]).ChannelUris)
                        {
                            ChannelDataStore store = new ChannelDataStore(this.GetClientAddressFromUri(str3, ip, port));
                            or.ChannelInfo.ChannelData[i] = store;
                        }
                    }
                }
            }
        }

        private void SplitClientAddress(string srcData, out string ip, out string port)
        {
            ip = "";
            port = "";
            if (!string.IsNullOrEmpty(srcData))
            {
                int index = srcData.IndexOf(":");
                if (index <= -1)
                {
                    ip = srcData;
                    port = "";
                }
                else
                {
                    ip = srcData.Substring(0, index);
                    port = srcData.Substring(index + 1);
                }
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags=SecurityPermissionFlag.Infrastructure)]
        public void UnmarshaledObject(object obj, ObjRef objRef)
        {
        }
    }
}

