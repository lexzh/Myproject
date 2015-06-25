namespace AppServerParam
{
    using System;
    using System.Runtime.CompilerServices;

    public class DataBaseParams
    {
        [CompilerGenerated]
        private string string_0;
        [CompilerGenerated]
        private string string_1;
        [CompilerGenerated]
        private string string_2;
        [CompilerGenerated]
        private string string_3;

        public string DataBaseIp
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

        public string DataBaseName
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

        public string DataBasePassword
        {
            [CompilerGenerated]
            get
            {
                return this.string_3;
            }
            [CompilerGenerated]
            set
            {
                this.string_3 = value;
            }
        }

        public string DataBaseStringParam
        {
            get
            {
                string str = "";
                return ((((str + "server=" + this.DataBaseIp + ";") + "uid=" + this.DataBaseUser + ";") + "pwd=" + this.DataBasePassword + ";") + "database=" + this.DataBaseName + ";Connect Timeout=120;");
            }
        }

        public string DataBaseUser
        {
            [CompilerGenerated]
            get
            {
                return this.string_2;
            }
            [CompilerGenerated]
            set
            {
                this.string_2 = value;
            }
        }
    }
}

