namespace Remoting
{
    using ParamLibrary.Bussiness;
    using ParamLibrary.Entity;
    using Contract;
    //using GAS;
    using Library;
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting.Lifetime;
    using System.Runtime.Remoting.Messaging;
    using Bussiness;

    public sealed class RemotingLogin : MarshalByRefObject, IRemotingLogin
    {
        private readonly string _ADCUrl = Const.ADCUrl;
        private const bool isOldInterfaceFlag = true;

        private bool ADCCheck(int workId, string ecCode, string userId, string pwd, ref string ErrorMessage)
        {
            if (!string.IsNullOrEmpty(this._ADCUrl) && (workId == -1))
            {
                string str = new ADCUserChecker(this._ADCUrl, ecCode, userId, pwd).ADCCheckUser("");
                if (!string.IsNullOrEmpty(str))
                {
                    ErrorMessage = str;
                    return false;
                }
            }
            return true;
        }

        private int ChangeModuleIdFromConfiger(int moduleId)
        {
            if (!string.IsNullOrEmpty(Const.ModuleId))
            {
                int num = Convert.ToInt32(Const.ModuleId);
                if (moduleId < num)
                {
                    moduleId = num;
                }
            }
            return moduleId;
        }

        private bool CheckOnline(ref string ErrorMessage)
        {
            OnlineUserManager manager = new OnlineUserManager();
            if (manager.Count > 50000)
            {
                ErrorMessage = "已经到达最大客户端登入数！";
                return false;
            }
            return true;
        }

        private RemotingServer CreateRemotingServer(UserInfoEntity userInfoEntity, string userId, int moduleId)
        {
            OnlineUserManager manager = new OnlineUserManager();
            RemotingServer remotingObj = new RemotingServer(userInfoEntity, userId, moduleId);
            manager.Add(userInfoEntity.WorkId, remotingObj);
            return remotingObj;
        }

        private Response ExecLogin(string ecCode, string userId, string userPw, string clientIp, int moduleId, int workId, out IRemotingServer remotingServerObject, bool isOldInterfaceFlag)
        {
            remotingServerObject = null;
            Response response = new Response();
            string format = "{0}|{1}|{2}";
            try
            {
                new LogHelper().WriteLog(new LogMsg("RemotingLogin", "ExecLogin", userId + "," + userPw + "," + clientIp));
                string errorMessage = string.Empty;
                if (!this.CheckOnline(ref errorMessage))
                {
                    ErrorMsg msg = new ErrorMsg("RemotingLogin", "ExecLogin", errorMessage);
                    new LogHelper().WriteError(msg);
                    response.ErrorMsg = errorMessage;
                    return response;
                }
                if (!this.ADCCheck(workId, ecCode, userId, userPw, ref errorMessage))
                {
                    ErrorMsg msg2 = new ErrorMsg("RemotingLogin", "ExecLogin", errorMessage);
                    new LogHelper().WriteError(msg2);
                    response.ErrorMsg = errorMessage;
                    return response;
                }
                if (((workId == -1) || !this.GetExistRemotingServerObject(workId, out remotingServerObject)) || !this.IsDbExistUser(workId))
                {
                    new LogHelper().WriteLog(new LogMsg("RemotingLogin", "ExecLogin", "开始新的重登，" + workId.ToString()));
                    moduleId = this.ChangeModuleIdFromConfiger(moduleId);
                    UserInfoEntity userInfoEntity = new UserInfoEntity();
                    if (!this.GetNewUser(userId, userPw, clientIp, moduleId, OnlineUserManager.MaxWorkId + 1, isOldInterfaceFlag, ref userInfoEntity, out errorMessage))
                    {
                        ErrorMsg msg3 = new ErrorMsg("RemotingLogin", "ExecLogin", errorMessage);
                        new LogHelper().WriteError(msg3);
                        response.ErrorMsg = errorMessage;
                        return response;
                    }
                    response.ErrorMsg = errorMessage;
                    remotingServerObject = this.CreateRemotingServer(userInfoEntity, userId, moduleId);
                    response.ResultCode = 0L;
                    response.SvcContext = string.Format(format, userInfoEntity.WorkId, userInfoEntity.UserName, SecurityHelper.Key);
                    CarDataInfoBuffer.UpdateLoginUserCarInfo(userId);
                    new LogHelper().WriteLog(new LogMsg("RemotingLogin", "ExecLogin", "结束新的重登，" + userInfoEntity.WorkId.ToString()));
                    return response;
                }
                new LogHelper().WriteLog(new LogMsg("RemotingLogin", "ExecLogin", "开始原有还存在对象登入，" + workId.ToString()));
                response.ResultCode = 0L;
                string userName = this.GetUserName(userId);
                response.SvcContext = string.Format(format, workId.ToString(), userName, SecurityHelper.Key);
                new LogHelper().WriteLog(new LogMsg("RemotingLogin", "ExecLogin", "结束原有还存在对象登入，" + workId.ToString()));
            }
            catch (Exception exception)
            {
                ErrorMsg msg4 = new ErrorMsg("RemotingLogin", "ExecLogin", exception.Message + exception.StackTrace);
                new LogHelper().WriteError(msg4);
                response.ErrorMsg = exception.Message;
            }
            return response;
        }

        private bool GetExistRemotingServerObject(int workId, out IRemotingServer serverObj)
        {
            bool flag = true;
            OnlineUserManager manager = new OnlineUserManager();
            serverObj = (RemotingServer) manager.GetExistRemotingServer(workId);
            if (serverObj == null)
            {
                manager.Remove(workId);
                flag = false;
            }
            return flag;
        }

        public int GetNewIdForSSOLogin(string ModuleId)
        {
            SSOLogin login = new SSOLogin();
            return login.GetNewId(ModuleId);
        }

        private bool GetNewUser(string userId, string userPw, string sIp, int moduleId, int PreWorkId, bool IsChecked, ref UserInfoEntity userInfoEntity, out string errorMessage)
        {
            errorMessage = string.Empty;
            UserLoginInterface interface2 = new UserLoginInterface(userId, userPw, sIp, moduleId.ToString(), PreWorkId);
            bool flag = true;
            if (!IsChecked)
            {
                flag = interface2.CheckUserPasswordOutDays(out errorMessage);
            }
            if (!flag)
            {
                return false;
            }
            interface2.PreWorkId = PreWorkId;
            return interface2.Login(ref userInfoEntity, ref errorMessage);
        }

        public Response getUpdateClientList(string currentUsedVersion)
        {
            string configClientVersion = FileHelper.ReadXmlEveryOne("Version");
            string remotingUpdateFileUrl = FileHelper.ReadXmlEveryOne("CsFileUpdateUrl");
            ClientUpdateVersion version = new ClientUpdateVersion(currentUsedVersion, configClientVersion, remotingUpdateFileUrl);
            return version.GetUpdateClientList();
        }

        private string GetUserName(string userId)
        {
            LoginUserInfo info = new LoginUserInfo(userId);
            return info.GetUserName();
        }

        public override object InitializeLifetimeService()
        {
            ILease lease = (ILease) base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromSeconds(30.0);
            }
            return lease;
        }

        private bool IsDbExistUser(int workId)
        {
            LoginOnlieUserInfo info = new LoginOnlieUserInfo(workId);
            return info.IsExistUser();
        }

        public Response Login(string userId, string userPw, string sIp, int moduleId, int workId, out IRemotingServer remotingObj)
        {
            return this.Login("", userId, userPw, sIp, moduleId, workId, out remotingObj);
        }

        public Response Login(string ecCode, string userId, string userPw, string sIp, int moduleId, int workId, out IRemotingServer remotingObj)
        {
            remotingObj = null;
            Response response = this.ExecLogin(ecCode, userId, userPw, sIp, moduleId, workId, out remotingObj, true);
            if (Const.RemotingServerIP1 != null)
            {
                CallContext.SetData("ClientIp", Const.RemotingServerIP1.Trim());
            }
            return response;
        }

        public Response Login(string ecCode, string userId, string userPw, string sIp, int moduleId, int workId, string Ip, out IRemotingServer remotingObj)
        {
            remotingObj = null;
            Response response = this.ExecLogin(ecCode, userId, userPw, sIp, moduleId, workId, out remotingObj, true);
            CallContext.SetData("ClientIp", Ip);
            return response;
        }

        public Response Login(string ecCode, string userId, string userPw, string sIp, int moduleId, int workId, string Ip, string Port, out IRemotingServer remotingObj)
        {
            remotingObj = null;
            string data = Ip + ":" + Port;
            bool flag = true;
            if (!string.IsNullOrEmpty(Const.IsNeedPWDUpdate) && "0".Equals(Const.IsNeedPWDUpdate))
            {
                flag = false;
            }
            Response response = this.ExecLogin(ecCode, userId, userPw, sIp, moduleId, workId, out remotingObj, !flag);
            CallContext.SetData("ClientIp", data);
            return response;
        }

        public Response LoginSendMsg(string ecCode, string userId, string pwd, string verifyCode)
        {
            ADCUserChecker checker = new ADCUserChecker(this._ADCUrl, ecCode, userId, pwd);
            return checker.ADCCheckUserAndSendMSG(verifyCode);
        }

        public Response ModifyUserPassword(string userId, string userOldPassword, string userNewPassword)
        {
            Response response = new Response();
            try
            {
                string str = new LoginUserInfo(userId, userOldPassword).ChangePassword(userNewPassword);
                if (string.IsNullOrEmpty(str))
                {
                    response.ResultCode = 0L;
                    return response;
                }
                response.ErrorMsg = str;
            }
            catch (Exception exception)
            {
                response.ErrorMsg = exception.Message;
            }
            return response;
        }

        public Response CheckSendMsgLogin(string ecCode, string userId, string userPw, string sIp, int moduleId, int workId, string Ip, string Port, out IRemotingServer remotingObj, string verfNum, string verfCode)
        {
            throw new NotImplementedException();
        }

        public Response SendMsgLogin(string ecCode, string userId, string userPw, string sIp, int moduleId, int workId, string Ip, string Port, out IRemotingServer remotingObj)
        {
            throw new NotImplementedException();
        }
    }
}

