namespace Remoting
{
    using System;
    using System.Collections;
    using System.Runtime.Remoting.Channels;

    public class ClientIPServerSinkProvider : IServerChannelSinkProvider
    {
        private IServerChannelSinkProvider _nextProvider;

        public ClientIPServerSinkProvider()
        {
        }

        public ClientIPServerSinkProvider(IDictionary properties, ICollection providerData)
        {
        }

        public IServerChannelSink CreateSink(IChannelReceiver channel)
        {
            IServerChannelSink next = null;
            if (this._nextProvider != null)
            {
                next = this._nextProvider.CreateSink(channel);
            }
            return new ClientIPServerSink(next);
        }

        public void GetChannelData(IChannelDataStore channelData)
        {
        }

        public IServerChannelSinkProvider Next
        {
            get
            {
                return this._nextProvider;
            }
            set
            {
                this._nextProvider = value;
            }
        }
    }
}

