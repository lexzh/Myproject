namespace AppServerParam
{
    using System;
    using System.IO;

    public class File
    {
        private string string_0 = string.Empty;

        public File(string string_1)
        {
            this.FileName = string_1;
        }

        public bool ISExistFile()
        {
            return System.IO.File.Exists(this.FileName);
        }

        public string FileName
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

