using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using DataAccess;
using Library;
using Remoting;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Timers;
using Bussiness;

namespace AppServer
{
    partial class Service : ServiceBase
    {
        private BackgroundWorker bgw = new BackgroundWorker();
        private const int recordTime = 1000;
        private static System.Timers.Timer RecordTimer;
        private const int registerInteralTime = 300000;
        private const string serverName = "业务服务";
        private readonly string startServer = "开启业务服务";
        private readonly string stopServer = "关闭业务服务";

        public Service()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 服务启动后台线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BussinessHelper.AppInitDataAndStarRun();
                new OnlineUserManager().Start();
                new RemotingManager().RegChannel();
                RecordTimer = new System.Timers.Timer(1000.0);
                RecordTimer.Elapsed += new ElapsedEventHandler(Service.onRecordTimerMain);
                RecordTimer.Enabled = true;
            }
            catch (Exception exception3)
            {
                this.WriteLog("bgw_DoWork", exception3.Message);
            }
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (!this.bgw.CancellationPending)
                {
                    this.WriteLog(this.startServer);
                }
            }
            catch (Exception exception)
            {
                this.WriteLog("bgw_RunWorkerCompleted", exception.Message);
            }
        }

        /// <summary>
        /// 获取授权服务器地址
        /// </summary>
        /// <returns></returns>
        private static string GetGlsUrl()
        {
            string glsIp = Const.GlsIp;
            string glsPort = Const.GlsPort;
            return string.Format("http://{0}:{1}/GLS/GLSService.asmx", glsIp, glsPort);
        }

        /// <summary>
        /// 获取备用授权服务器地址
        /// </summary>
        /// <returns></returns>
        private static string GetStandbyGlsUrl()
        {
            string standbyGlsIp = Const.StandbyGlsIp;
            string standbyGlsPort = Const.StandbyGlsPort;
            return string.Format("http://{0}:{1}/GLS/GLSService.asmx", standbyGlsIp, standbyGlsPort);
        }

        private static DateTime getSystemDate()
        {
            SqlDataAccess access = new SqlDataAccess();
            return access.getSystemDate();
        }

        private static string getSystemVersion()
        {
            return ("V" + Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        /// <summary>
        /// 调试方法
        /// </summary>
        public void Main1()
        {
            BussinessHelper.AppInitDataAndStarRun();
            new OnlineUserManager().Start();
            new RemotingManager().RegChannel();
            RecordTimer = new System.Timers.Timer(1000.0);
            RecordTimer.Enabled = true;
            RecordTimer.Elapsed += new ElapsedEventHandler(Service.onRecordTimerMain);
            Thread.Sleep(-1);
        }

        private static void onRecordTimerMain(object sender, ElapsedEventArgs e)
        {
            RecordTimer.Enabled = false;
            int num = 0;
            try
            {
                num = Convert.ToInt32(Const.LogSaveDate);
            }
            catch (Exception exception)
            {
                num = 3;
                LogMsg msg = new LogMsg("业务服务", "onRecordTimerMain", exception.Message);
                new LogHelper().WriteLog(msg);
            }
            new LogHelper().DeleteOldRecord(num);
            RecordTimer.Interval = 86400000.0;
            RecordTimer.Enabled = true;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="pContext"></param>
        private void WriteLog(string pContext)
        {
            LogMsg msg = new LogMsg("业务服务", pContext, "");
            new LogHelper().WriteLog(msg);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="funtionName"></param>
        /// <param name="pContext"></param>
        private void WriteLog(string funtionName, string pContext)
        {
            LogMsg msg = new LogMsg("业务服务", funtionName, pContext);
            new LogHelper().WriteLog(msg);
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            this.bgw.DoWork += new DoWorkEventHandler(this.bgw_DoWork);
            this.bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bgw_RunWorkerCompleted);
            this.bgw.WorkerSupportsCancellation = true;
            this.bgw.RunWorkerAsync();
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            try
            {
                new RemotingManager().UnRegChannel();
                this.WriteLog(this.stopServer);
                RecordTimer.Enabled = false;
            }
            catch (Exception exception)
            {
                this.WriteLog("OnStop", exception.Message);
            }
        }
    }
}
