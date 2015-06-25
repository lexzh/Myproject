namespace Protocol
{
    using System;
    using System.Runtime.CompilerServices;

    public class IODeviceTextMsg : IODeviceAttachInfo
    {
        [CompilerGenerated]
        private string string_0;
        [CompilerGenerated]
        private string string_1;

        public string Message
        {
            [CompilerGenerated]
            get
            {
                return this.string_1;
            }
            [CompilerGenerated]
            set
            {
                this.string_1 = value;
            }
        }

        public string SimNum
        {
            [CompilerGenerated]
            get
            {
                return this.string_0;
            }
            [CompilerGenerated]
            set
            {
                this.string_0 = value;
            }
        }
    }
}

