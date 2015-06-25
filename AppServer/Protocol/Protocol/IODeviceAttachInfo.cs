namespace Protocol
{
    using System;
    using System.Runtime.CompilerServices;

    public abstract class IODeviceAttachInfo
    {
        [CompilerGenerated]
        private int int_0;

        protected IODeviceAttachInfo()
        {
        }

        public int InfoID
        {
            [CompilerGenerated]
            get
            {
                return this.int_0;
            }
            [CompilerGenerated]
            set
            {
                this.int_0 = value;
            }
        }
    }
}

