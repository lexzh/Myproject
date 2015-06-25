namespace AppServerParam
{
    using System;

    public class Port : ICheck
    {
        private const int MAX_PORT_SIZE = 65535;
        private const int MIN_PORT_SIZE = 0;

        public bool Check(object object_0)
        {
            string str = object_0 as string;
            if (!AppServerParam.Number.IsNumber(str))
            {
                throw new Exception("不是数字！");
            }
            int num = Convert.ToInt32(str);
            if ((num <= 0) || (num > 65535))
            {
                throw new Exception("端口范围应该在0-65535!");
            }
            return true;
        }
    }
}

