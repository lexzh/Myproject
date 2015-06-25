namespace Bussiness
{
    using System;
    using System.Collections;

    public class Oix
    {
        public byte[] GetObjectArg(string[] string_0)
        {
            if ((string_0 == null) || (string_0.Length <= 0))
            {
                return null;
            }
            int num = int.Parse(string_0[0]);
            int num2 = int.Parse(string_0[1]);
            byte[] array = this.method_0(num, num2);
            array[0] = (byte) num;
            array[1] = (byte) num2;
            ArrayList list = this.method_3(string_0);
            for (int i = 0; i <= (list.Count - 1); i++)
            {
                byte[] buffer2 = this.method_1(list[i] as OixBoxParameter);
                buffer2.CopyTo(array, (int) (2 + (buffer2.Length * i)));
            }
            return array;
        }

        private byte[] method_0(int int_0, int int_1)
        {
            byte[] buffer = null;
            if (((int_0 == 0) && (int_1 == 1)) || (int_0 == 1))
            {
                buffer = new byte[0x19];
            }
            if ((int_0 == 0) && (int_1 == 2))
            {
                buffer = new byte[0x30];
            }
            return buffer;
        }

        private byte[] method_1(OixBoxParameter oixBoxParameter_0)
        {
            byte[] buffer = new byte[0x17];
            byte[] buffer2 = new byte[2];
            buffer[0] = (byte) oixBoxParameter_0.Code;
            buffer2 = this.method_2(oixBoxParameter_0.Captity);
            buffer[1] = buffer2[0];
            buffer[2] = buffer2[1];
            buffer2 = this.method_2(oixBoxParameter_0.Parameter0);
            buffer[3] = buffer2[0];
            buffer[4] = buffer2[1];
            buffer2 = this.method_2(oixBoxParameter_0.Parameter1);
            buffer[5] = buffer2[0];
            buffer[6] = buffer2[1];
            buffer2 = this.method_2(oixBoxParameter_0.Parameter2);
            buffer[7] = buffer2[0];
            buffer[8] = buffer2[1];
            buffer2 = this.method_2(oixBoxParameter_0.Parameter3);
            buffer[9] = buffer2[0];
            buffer[10] = buffer2[1];
            buffer2 = this.method_2(oixBoxParameter_0.Parameter4);
            buffer[11] = buffer2[0];
            buffer[12] = buffer2[1];
            buffer2 = this.method_2(oixBoxParameter_0.Parameter5);
            buffer[13] = buffer2[0];
            buffer[14] = buffer2[1];
            buffer2 = this.method_2(oixBoxParameter_0.Parameter6);
            buffer[15] = buffer2[0];
            buffer[0x10] = buffer2[1];
            buffer2 = this.method_2(oixBoxParameter_0.Parameter7);
            buffer[0x11] = buffer2[0];
            buffer[0x12] = buffer2[1];
            buffer2 = this.method_2(oixBoxParameter_0.Parameter8);
            buffer[0x13] = buffer2[0];
            buffer[20] = buffer2[1];
            buffer2 = this.method_2(oixBoxParameter_0.AlaramOixValue);
            buffer[0x15] = buffer2[0];
            buffer[0x16] = buffer2[1];
            return buffer;
        }

        private byte[] method_2(int int_0)
        {
            return new byte[] { ((byte) (int_0 >> 8)), ((byte) (int_0 & 0xff)) };
        }

        private ArrayList method_3(string[] string_0)
        {
            int num = 1;
            if (string_0.Length == 0x1a)
            {
                num = 2;
            }
            int index = 1;
            ArrayList list = new ArrayList();
            for (int i = 0; i <= (num - 1); i++)
            {
                OixBoxParameter parameter = new OixBoxParameter();
                index++;
                parameter.Code = int.Parse(string_0[index]);
                index++;
                parameter.Captity = int.Parse(string_0[index]);
                index++;
                parameter.Parameter0 = int.Parse(string_0[index]);
                index++;
                parameter.Parameter1 = int.Parse(string_0[index]);
                index++;
                parameter.Parameter2 = int.Parse(string_0[index]);
                index++;
                parameter.Parameter3 = int.Parse(string_0[index]);
                index++;
                parameter.Parameter4 = int.Parse(string_0[index]);
                index++;
                parameter.Parameter5 = int.Parse(string_0[index]);
                index++;
                parameter.Parameter6 = int.Parse(string_0[index]);
                index++;
                parameter.Parameter7 = int.Parse(string_0[index]);
                index++;
                parameter.Parameter8 = int.Parse(string_0[index]);
                index++;
                parameter.AlaramOixValue = int.Parse(string_0[index]);
                list.Add(parameter);
            }
            return list;
        }
    }
}

