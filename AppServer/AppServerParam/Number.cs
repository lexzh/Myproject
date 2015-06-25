namespace AppServerParam
{
    using System;

    public class Number
    {
        public static bool IsNumber(string string_0)
        {
            int num;
            return int.TryParse(string_0, out num);
        }
    }
}

