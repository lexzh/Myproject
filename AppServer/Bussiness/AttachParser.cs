namespace Bussiness
{
    using System;

    public abstract class AttachParser
    {
        private string string_0;

        protected AttachParser()
        {
        }

        public virtual string Parse()
        {
            return string.Empty;
        }

        public string MessageAlarmText
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }
    }
}

