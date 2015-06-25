namespace Bussiness
{
    using Library;
    using System;
    using System.Collections;
    using System.Data;

    public class RespCodeParam
    {
        private static Hashtable _RespNameList;

        static RespCodeParam()
        {
            old_acctor_mc();
        }

        public static string GetRespName(int int_0)
        {
            string str = string.Empty;
            if (_RespNameList.Count <= 0)
            {
                InitParam();
            }
            if ((_RespNameList.Count > 0) && _RespNameList.ContainsKey(int_0))
            {
                str = _RespNameList[int_0] as string;
            }
            return str;
        }

        private static void InitParam()
        {
            DataSet set = FileHelper.ReadDataXml("RespCode.xml");
            if ((set != null) && (set.Tables.Count > 0))
            {
                int key = 0;
                string str = string.Empty;
                foreach (DataRow row in set.Tables[0].Rows)
                {
                    key = Convert.ToInt32(row["name"].ToString(), 0x10);
                    str = Convert.ToString(row["value"]);
                    if (!_RespNameList.ContainsKey(key))
                    {
                        _RespNameList.Add(key, str);
                    }
                }
            }
        }

        private static void old_acctor_mc()
        {
            _RespNameList = new Hashtable(0xd1);
        }
    }
}

