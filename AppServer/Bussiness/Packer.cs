namespace Bussiness
{
    using System;
    using System.Text;

    public class Packer
    {
        private string method_0(string string_0)
        {
            return this.xOr(string_0);
        }

        private byte[] method_1(byte[] byte_0)
        {
            byte[] array = new byte[byte_0.Length + 2];
            array[0] = 2;
            byte_0.CopyTo(array, 1);
            array[array.Length - 1] = 3;
            return array;
        }

        public byte[] packData(string string_0)
        {
            byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(string_0);
            byte[] buffer2 = Encoding.ASCII.GetBytes(this.method_0(string_0));
            byte[] array = new byte[bytes.Length + buffer2.Length];
            bytes.CopyTo(array, 0);
            buffer2.CopyTo(array, bytes.Length);
            return this.method_1(array);
        }

        protected string xOr(string string_0)
        {
            string str = "0x";
            string str2 = str + string_0.Substring(0, 2);
            string str3 = str + string_0.Substring(2, 2);
            string_0 = string_0.Substring(4);
            int num = Convert.ToInt16(str2, 0x10) ^ Convert.ToInt16(str3, 0x10);
            for (int i = 0; i < string_0.Length; i += 2)
            {
                str3 = str + string_0.Substring(i, 2);
                num ^= Convert.ToInt16(str3, 0x10);
            }
            return Convert.ToString(num, 0x10).ToUpper();
        }
    }
}

