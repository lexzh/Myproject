namespace Remoting
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Messaging;

    public class ClientIPServerSink : BaseChannelObjectWithProperties, IServerChannelSink, IChannelSinkBase
    {
        private IServerChannelSink _nextSink;

        public ClientIPServerSink(IServerChannelSink next)
        {
            this._nextSink = next;
        }

        public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage message, ITransportHeaders headers, Stream stream)
        {
            object obj1 = headers["__IPAddress"];
            sinkStack.AsyncProcessResponse(message, headers, stream);
        }

        public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, object state, IMessage message, ITransportHeaders headers)
        {
            return null;
        }

        public ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack, IMessage requestMsg, ITransportHeaders requestHeaders, Stream requestStream, out IMessage responseMsg, out ITransportHeaders responseHeaders, out Stream responseStream)
        {
            if (this._nextSink != null)
            {
                object obj1 = requestHeaders["__IPAddress"];
                return this._nextSink.ProcessMessage(sinkStack, requestMsg, requestHeaders, requestStream, out responseMsg, out responseHeaders, out responseStream);
            }
            responseMsg = null;
            responseHeaders = null;
            responseStream = null;
            return ServerProcessing.Complete;
        }

        public IServerChannelSink NextChannelSink
        {
            get
            {
                return this._nextSink;
            }
            set
            {
                this._nextSink = value;
            }
        }
    }
}

