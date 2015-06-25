namespace AppServerParam
{
    using System;
    using System.Linq;

    public class IP : ICheck
    {
        private const string IP_ERROR_MSG = "IP格式不对";
        private const int MAX_IP_SIZE = 255;
        private const int MIX_IP_SIZE = 0;

        public bool Check(object object_0)
        {
            string source = object_0 as string;
            if (source.Contains<char>('\\'))
            {
                source = source.Split(new char[] { '\\' })[0];
            }
            string[] strArray = source.Split(new char[] { '.' });
            if (strArray.Length != 4)
            {
                throw new Exception("IP格式不对");
            }
            int num = 0;
            for (int i = 0; i <= (strArray.Length - 1); i++)
            {
                if (!AppServerParam.Number.IsNumber(strArray[i]))
                {
                    throw new Exception("IP格式不对");
                }
                num = Convert.ToInt32(strArray[i]);
                if (!this.method_0(num))
                {
                    throw new Exception("IP格式不对");
                }
            }
            return true;
        }

        private bool method_0(int int_0)
        {
            return ((int_0 >= 0) && (int_0 <= 255));
        }
    }
}

