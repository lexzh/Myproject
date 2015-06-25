namespace Contract
{
    using ParamLibrary.Entity;
    using System;
    using System.Runtime.InteropServices;

    public interface IRemotingLogin
    {
        Response CheckSendMsgLogin(string ecCode, string userId, string userPw, string sIp, int moduleId, int workId, string Ip, string Port, out IRemotingServer remotingObj, string verfNum, string verfCode);
        int GetNewIdForSSOLogin(string ModuleId);
        Response getUpdateClientList(string sCurrentVersion);
        Response Login(string userId, string userPw, string sIp, int moduleId, int workId, out IRemotingServer remotingObj);
        Response Login(string ecCode, string userId, string userPw, string sIp, int moduleId, int workId, out IRemotingServer remotingObj);
        Response Login(string ecCode, string userId, string userPw, string sIp, int moduleId, int workId, string Ip, out IRemotingServer remotingObj);
        Response Login(string ecCode, string userId, string userPw, string sIp, int moduleId, int workId, string Ip, string Port, out IRemotingServer remotingObj);
        Response LoginSendMsg(string ecCode, string userId, string pwd, string verifyCode);
        Response ModifyUserPassword(string userId, string userOldPassword, string userNewPassword);
        Response SendMsgLogin(string ecCode, string userId, string userPw, string sIp, int moduleId, int workId, string Ip, string Port, out IRemotingServer remotingObj);
    }
}

