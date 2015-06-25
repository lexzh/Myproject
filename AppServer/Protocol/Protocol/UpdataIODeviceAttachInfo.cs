namespace Protocol
{
    using System;
    using System.Collections.Generic;

    public class UpdataIODeviceAttachInfo
    {
        private Queue<IODeviceAttachInfo> queue_0 = new Queue<IODeviceAttachInfo>();

        public void Add(IODeviceAttachInfo iodeviceAttachInfo_0)
        {
            this.queue_0.Enqueue(iodeviceAttachInfo_0);
        }

        public IODeviceAttachInfo Get()
        {
            IODeviceAttachInfo info = null;
            if (this.queue_0.Count > 0)
            {
                info = this.queue_0.Dequeue();
            }
            return info;
        }

        public int Count
        {
            get
            {
                return this.queue_0.Count;
            }
        }
    }
}

