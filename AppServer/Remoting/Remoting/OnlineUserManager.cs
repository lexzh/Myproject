namespace Remoting
{
    using ParamLibrary.Bussiness;
    using Library;
    using System;
    using System.Collections;
    using System.Threading;
    using Bussiness;

    /// <summary>
    /// 用户管理类
    /// </summary>
    public class OnlineUserManager
    {
        private static int _MaxWorkId = 0;
        private static Hashtable loginUserOnlineList = Hashtable.Synchronized(new Hashtable(0x14b));

        public void Add(int workId, object remotingObj)
        {
            if ((loginUserOnlineList != null) && !loginUserOnlineList.ContainsKey(workId))
            {
                loginUserOnlineList.Add(workId, remotingObj);
            }
        }

        /// <summary>
        /// 内存清理线程
        /// </summary>
        private void ClearBufferData()
        {
            while (true)
            {
                try
                {
                    this.ClearHadOutTimeRomtingServerObject();
                    this.ClearHadOutTimeResponseUpdateData();
                    MemeroyHelper.CollectMemerory();
                    new LogHelper().WriteLog(new LogMsg("心跳日志", "当前内存/虚拟内存/线程数/用户数：", MemeroyHelper.AppMemerorySize.ToString() + "/" + MemeroyHelper.AppVirtualMemerorySize.ToString() + "/" + MemeroyHelper.ThreadCount.ToString() + "/" + this.Count.ToString()));
                    Thread.Sleep(0xea60);
                }
                catch (Exception exception)
                {
                    ErrorMsg msg = new ErrorMsg("OnlineUserManager", "ClearBufferData", exception.Message + exception.StackTrace);
                    new LogHelper().WriteError(msg);
                    Thread.Sleep(0x1d4c0);
                }
            } 
        }

        private void ClearHadOutTimeResponseUpdateData()
        {
            BussinessHelper.upResponse.Delete(loginUserOnlineList);
            BussinessHelper.upOutEquipmentData.Delete(loginUserOnlineList);
        }

        /// <summary>
        /// 清除超时用户
        /// </summary>
        private void ClearHadOutTimeRomtingServerObject()
        {
            if ((loginUserOnlineList != null) && (loginUserOnlineList.Count > 0))
            {
                object[] array = new object[loginUserOnlineList.Count];
                loginUserOnlineList.Keys.CopyTo(array, 0);
                RemotingServer server = null;
                foreach (int num in array)
                {
                    server = loginUserOnlineList[num] as RemotingServer;
                    if ((server == null) || (server._OnlineUserInfo == null))
                    {
                        server = null;
                        loginUserOnlineList.Remove(num);
                    }
                    else
                    {
                        if (!server._RemotingState.IsOutTime)
                        {
                            TimeSpan span = (TimeSpan) (DateTime.Now - server._OnlineUserInfo.SynDbUserTime);
                            if (span.Minutes < 5)
                            {
                                continue;
                            }
                        }
                        //清除超时或者同步时间大于5分钟的用户
                        server = null;
                        loginUserOnlineList.Remove(num);
                    }
                }
            }
        }

        public object GetExistRemotingServer(int workId)
        {
            if (loginUserOnlineList == null)
            {
                return null;
            }
            return loginUserOnlineList[workId];
        }

        /// <summary>
        /// 根据ID移除用户
        /// </summary>
        /// <param name="workId"></param>
        public void Remove(int workId)
        {
            if ((loginUserOnlineList != null) && loginUserOnlineList.ContainsKey(workId))
            {
                loginUserOnlineList.Remove(workId);
            }
        }

        public void Start()
        {
            Thread thread2 = new Thread(new ThreadStart(this.ClearBufferData)) {
                IsBackground = true
            };
            thread2.Start();
        }

        public int Count
        {
            get
            {
                return loginUserOnlineList.Count;
            }
        }

        public static int MaxWorkId
        {
            get
            {
                return _MaxWorkId;
            }
            set
            {
                _MaxWorkId = value;
            }
        }
    }
}

