namespace Bussiness
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct UserInfoEntity
    {
        public string UserName;
        public string AreaCode;
        public int WorkId;
        public int GroupId;
        public bool AllowSelMutil;
        public bool AllowEmptyPw;
        public bool SudoOverDue;
        public string RoadTransportID;
    }
}

