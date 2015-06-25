namespace AppServerParam
{
    using System;

    public class LogSaveDay : ICheck
    {
        private const int MAX_DAY_SIZE = 30;
        private const int MIN_DAY_SIZE = 0;

        public bool Check(object object_0)
        {
            string str = object_0 as string;
            if (!AppServerParam.Number.IsNumber(str))
            {
                throw new Exception("不是数字！");
            }
            int num = Convert.ToInt32(str);
            if ((num <= 0) || (num > 30))
            {
                throw new Exception("日志保存天数范围应该在0-30!");
            }
            return true;
        }
    }
}

