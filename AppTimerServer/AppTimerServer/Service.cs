using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Bussiness;
using PublicClass;
using System.Threading;
using Library;

namespace TimerServer
{
    partial class Service : ServiceBase
    {
        private static System.Timers.Timer tDeleteLogTimer;

        private static PlatformGatheredAlarm platformGatheredAlarm;

        private static PlatformAlarmRegionAlarm platformAlarmRegionAlarm;

        private static PlatformAlarmPathSegmentAlarm platformAlarmPathSegmentAlarm;

        private static PlatformAlarmPathAlarm platformAlarmPathAlarm;

        private static SeparateAndSticky separateAndSticky;

        private static LCSTimer lCSTimer;

        private static LBSTimer lBSTimer;

        private static PicTimer picTimer;

        private static JTBTerminalDemand jTBTerminalDemand;

        private static JTBOnOffLineNotice jTBOnOffLineNotice;

        private static IORegionTimer iORegionTimer;

        private static InquiresCarCurrentAddress inquiresCarCurrentAddress;

        private static DWLBSPos dWLBSPos;

        private static CuffTimer cuffTimer;

        private static ChkErrorTimer chkErrorTimer;

        private static CarInOutOfRangeOnTime carInOutOfRangeOnTime;

        private static CarBeBackOnTime carBeBackOnTime;

        private static BroadCastTimer broadCastTimer;

        private static AddressResolution addressResolution;

        private static TerminalOffLineMessageRemind terminalOffLineMessageRemind;

        private static SendPZMessage sendPZMessage;

        private static PlatFormrForbidDriveAlarm platFormrForbidDriveAlarm;

        private static GpsCarCurrentPosInfo gpsCarCurrentPosInfo;

        private static PlatFormAlarmThreeLevelRoadAlarm platFormAlarmThreeLevelRoadAlarm;

        private static PlatFormCheckRoadSpeedAndRank platFormCheckRoadSpeedAndRank;

        public Service()
        {
            InitializeComponent();
        }

        private static void GpsPicMain()
        {
            LogMsg logMsg = new LogMsg()
            {
                ClassName = "Service",
                FunctionName = "GpsPicMain",
                Msg = "启动定时服务器 成功"
            };
            LogHelper logHelper = new LogHelper();
            logHelper.WriteLog(logMsg);
            ReadDataFromXml.UpdateParameter();
            logMsg.Msg = "未开启定时拍照功能";
            if (ReadDataFromXml.IsPic)
            {
                logMsg.Msg = "开启定时拍照功能";
                picTimer = new PicTimer();
                picTimer.start();
                logHelper.WriteLog(logMsg, logMsg.Msg);
            }
            logMsg.Msg = "未开启掉线通知功能";
            if (ReadDataFromXml.IsCuff)
            {
                logMsg.Msg = "开启掉线通知功能";
                cuffTimer = new CuffTimer();
                cuffTimer.start();
                logHelper.WriteLog(logMsg, logMsg.Msg);
            }
            logMsg.Msg = "未开启播报信息功能";
            if (ReadDataFromXml.IsBroadCast)
            {
                logMsg.Msg = "开启播报信息功能";
                broadCastTimer = new BroadCastTimer();
                broadCastTimer.start();
                logHelper.WriteLog(logMsg, logMsg.Msg);
            }
            logMsg.Msg = "未开启LBS定位服务";
            if (ReadDataFromXml.IsLBSPos)
            {
                logMsg.Msg = "开启LBS定位服务";
                lBSTimer = new LBSTimer();
                lBSTimer.start();
                logHelper.WriteLog(logMsg, logMsg.Msg);
            }
            logMsg.Msg = "未开启LCS定位服务";
            if (ReadDataFromXml.IsLCSPos)
            {
                logMsg.Msg = "开启LCS定位服务";
                lCSTimer = new LCSTimer();
                lCSTimer.start();
                logHelper.WriteLog(logMsg, logMsg.Msg);
            }
            logMsg.Msg = "未开启订单状态解析";
            if (ReadDataFromXml.IsBillPos)
            {
                logMsg.Msg = "开启订单状态解析";
                addressResolution = new AddressResolution();
                addressResolution.start();
                logHelper.WriteLog(logMsg, logMsg.Msg);
            }
            logMsg.Msg = "未开启出入行政区报警";
            if (ReadDataFromXml.IsAdminRegionAlarm)
            {
                logMsg.Msg = "开启出入行政区报警服务";
                iORegionTimer = new IORegionTimer();
                iORegionTimer.start();
                logHelper.WriteLog(logMsg, logMsg.Msg);
            }
            logMsg.Msg = "未开故障检测功能";
            if (ReadDataFromXml.IsChkError)
            {
                logMsg.Msg = "开启故障检测功能";
                chkErrorTimer = new ChkErrorTimer();
                chkErrorTimer.start();
                logHelper.WriteLog(logMsg, logMsg.Msg);
            }
            logMsg.Msg = "未开启未按时归班报警";
            if (ReadDataFromXml.IsBeBackOnTime)
            {
                logMsg.Msg = "开启未按时归班报警";
                carBeBackOnTime = new CarBeBackOnTime();
                carBeBackOnTime.start();
                logHelper.WriteLog(logMsg);
            }
            logMsg.Msg = "未开启DWLBS手机定位";
            if (ReadDataFromXml.IsDWLBSPos)
            {
                logMsg.Msg = "开启DWLBS手机定位";
                dWLBSPos = new DWLBSPos();
                dWLBSPos.start();
                logHelper.WriteLog(logMsg);
            }
            logMsg.Msg = "未开启未按时进出站报警";
            if (ReadDataFromXml.IsInOutOfRangeOnTime)
            {
                logMsg.Msg = "开启未按时进出站报警";
                carInOutOfRangeOnTime = new CarInOutOfRangeOnTime();
                carInOutOfRangeOnTime.start();
                logHelper.WriteLog(logMsg);
            }
            logMsg.Msg = "未开启平台报警-偏移路线报警";
            if (ReadDataFromXml.IsPathAlarm)
            {
                logMsg.Msg = "开启平台报警-偏移路线报警";
                platformAlarmPathAlarm = new PlatformAlarmPathAlarm();
                platformAlarmPathAlarm.start();
                logHelper.WriteLog(logMsg);
            }
            logMsg.Msg = "未开启平台报警-分路段超速报警";
            if (ReadDataFromXml.IsPathSegmentAlarm)
            {
                logMsg.Msg = "开启平台报警-分路段超速报警";
                platformAlarmPathSegmentAlarm = new PlatformAlarmPathSegmentAlarm();
                platformAlarmPathSegmentAlarm.start();
                logHelper.WriteLog(logMsg);
            }
            logMsg.Msg = "未开启平台报警-区域报警报警";
            if (ReadDataFromXml.IsRegionAlarm)
            {
                logMsg.Msg = "开启平台报警-区域报警报警";
                platformAlarmRegionAlarm = new PlatformAlarmRegionAlarm();
                platformAlarmRegionAlarm.start();
                logHelper.WriteLog(logMsg);
            }
            logMsg.Msg = "未开启交通部上下线通知";
            if (ReadDataFromXml.IsJTBOnOffNotice)
            {
                logMsg.Msg = "开启交通部上下线通知";
                jTBOnOffLineNotice = new JTBOnOffLineNotice();
                jTBOnOffLineNotice.start();
                logHelper.WriteLog(logMsg);
            }
            logMsg.Msg = "未开启定时下发终端点播";
            if (ReadDataFromXml.IsTerminalDemand)
            {
                logMsg.Msg = "开启定时下发终端点播";
                jTBTerminalDemand = new JTBTerminalDemand();
                jTBTerminalDemand.start();
                logHelper.WriteLog(logMsg);
            }
            logMsg.Msg = "未开启聚集报警";
            if (ReadDataFromXml.IsGatheredAlarm)
            {
                logMsg.Msg = "开启聚集报警";
                platformGatheredAlarm = new PlatformGatheredAlarm();
                platformGatheredAlarm.start();
                logHelper.WriteLog(logMsg);
            }
            logMsg.Msg = "未开启发送详细位置信息";
            if (ReadDataFromXml.IsCurrentAddress)
            {
                logMsg.Msg = "开启发送详细位置信息";
                inquiresCarCurrentAddress = new InquiresCarCurrentAddress();
                inquiresCarCurrentAddress.start();
                logHelper.WriteLog(logMsg);
            }
            logMsg.Msg = "未开启脱车粘车报警";
            if (ReadDataFromXml.IsSeparateAndSticky)
            {
                logMsg.Msg = "开启脱车粘车报警";
                separateAndSticky = new SeparateAndSticky();
                separateAndSticky.start();
                logHelper.WriteLog(logMsg);
            }
            logMsg.Msg = "未开启终端未上线短信提醒";
            if (ReadDataFromXml.IsMsgRemind)
            {
                logMsg.Msg = "开启终端未上线短信提醒";
                terminalOffLineMessageRemind = new TerminalOffLineMessageRemind();
                terminalOffLineMessageRemind.start();
                logHelper.WriteLog(logMsg);
            }
            logMsg.Msg = "未开启发送配置短信功能";
            if (ReadDataFromXml.IsSendPZMsg)
            {
                logMsg.Msg = "开启发送配置短信功能";
                sendPZMessage = new SendPZMessage();
                sendPZMessage.start();
                logHelper.WriteLog(logMsg);
            }
            if (ReadDataFromXml.IsForbidDriveAlarm)
            {
                logMsg.Msg = "开启平台禁驾报警功能";
                platFormrForbidDriveAlarm = new PlatFormrForbidDriveAlarm();
                platFormrForbidDriveAlarm.start();
                logHelper.WriteLog(logMsg);
            }
            if (ReadDataFromXml.IsThreeLevelRoadAlarm)
            {
                logMsg.Msg = "开启平台三级路面报警";
                platFormAlarmThreeLevelRoadAlarm = new PlatFormAlarmThreeLevelRoadAlarm();
                platFormAlarmThreeLevelRoadAlarm.start();
                logHelper.WriteLog(logMsg);
            }
            if (ReadDataFromXml.IschkRoadSpeedAndRank)
            {
                logMsg.Msg = "开启分道路等级超速报警";
                platFormCheckRoadSpeedAndRank = new PlatFormCheckRoadSpeedAndRank();
                platFormCheckRoadSpeedAndRank.start();
                logHelper.WriteLog(logMsg);
            }
            try
            {
                tDeleteLogTimer = new System.Timers.Timer((double)1000);
                tDeleteLogTimer.Elapsed += new System.Timers.ElapsedEventHandler(onDeleteOldRecord);
                tDeleteLogTimer.Enabled = true;
            }
            catch (Exception exception)
            {
                logHelper.WriteError(new ErrorMsg("Service", "启动日志删除失败", exception.Message));
            }
        }

        private static void Main1()
        {
            GpsPicMain();
            Thread.Sleep(-1);
        }

        private static void onDeleteOldRecord(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                try
                {
                    int num = 0;
                    if (!int.TryParse(ReadDataFromXml.LogSaveDate, out num))
                    {
                        num = 7;
                    }
                    (new LogHelper()).DeleteOldRecord(num);
                    Variable.sSaveLogDays = num.ToString();
                    Record.execDeleteRecord();
                }
                catch
                {
                }
            }
            finally
            {
                tDeleteLogTimer.Interval = 86400000;
            }
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            GpsPicMain();
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            try
            {
                if (ReadDataFromXml.IsPic && picTimer != null)
                {
                    picTimer.stop();
                }
                if (ReadDataFromXml.IsCuff && cuffTimer != null)
                {
                    cuffTimer.stop();
                }
                if (ReadDataFromXml.IsBroadCast && broadCastTimer != null)
                {
                    broadCastTimer.stop();
                }
                if (ReadDataFromXml.IsLBSPos && lBSTimer != null)
                {
                    lBSTimer.stop();
                }
                if (ReadDataFromXml.IsLCSPos && lCSTimer != null)
                {
                    lCSTimer.stop();
                }
                if (ReadDataFromXml.IsBillPos && addressResolution != null)
                {
                    addressResolution.stop();
                }
                if (ReadDataFromXml.IsAdminRegionAlarm && iORegionTimer != null)
                {
                    iORegionTimer.stop();
                }
                if (ReadDataFromXml.IsBeBackOnTime && carBeBackOnTime != null)
                {
                    carBeBackOnTime.stop();
                }
                if (ReadDataFromXml.IsDWLBSPos && dWLBSPos != null)
                {
                    dWLBSPos.stop();
                }
                if (ReadDataFromXml.IsInOutOfRangeOnTime && carInOutOfRangeOnTime != null)
                {
                    carInOutOfRangeOnTime.stop();
                }
                if (ReadDataFromXml.IsPathAlarm && platformAlarmPathAlarm != null)
                {
                    platformAlarmPathAlarm.stop();
                }
                if (ReadDataFromXml.IsPathSegmentAlarm && platformAlarmPathSegmentAlarm != null)
                {
                    platformAlarmPathSegmentAlarm.stop();
                }
                if (ReadDataFromXml.IsRegionAlarm && platformAlarmRegionAlarm != null)
                {
                    platformAlarmRegionAlarm.stop();
                }
                if (ReadDataFromXml.IsJTBOnOffNotice && jTBOnOffLineNotice != null)
                {
                    jTBOnOffLineNotice.stop();
                }
                if (ReadDataFromXml.IsTerminalDemand && jTBTerminalDemand != null)
                {
                    jTBTerminalDemand.stop();
                }
                if (ReadDataFromXml.IsGatheredAlarm && platformGatheredAlarm != null)
                {
                    platformGatheredAlarm.stop();
                }
                if (ReadDataFromXml.IsCurrentAddress && inquiresCarCurrentAddress != null)
                {
                    inquiresCarCurrentAddress.stop();
                }
                if (ReadDataFromXml.IsSeparateAndSticky && separateAndSticky != null)
                {
                    separateAndSticky.stop();
                }
                if (ReadDataFromXml.IsMsgRemind && terminalOffLineMessageRemind != null)
                {
                    terminalOffLineMessageRemind.stop();
                }
                if (ReadDataFromXml.IsSendPZMsg && sendPZMessage != null)
                {
                    sendPZMessage.stop();
                }
                if (ReadDataFromXml.IsForbidDriveAlarm && platFormrForbidDriveAlarm != null)
                {
                    platFormrForbidDriveAlarm.stop();
                }
                if (gpsCarCurrentPosInfo != null)
                {
                    gpsCarCurrentPosInfo.stop();
                }
                if (ReadDataFromXml.IsThreeLevelRoadAlarm && platFormAlarmThreeLevelRoadAlarm != null)
                {
                    platFormAlarmThreeLevelRoadAlarm.stop();
                }
                if (ReadDataFromXml.IschkRoadSpeedAndRank && platFormAlarmThreeLevelRoadAlarm != null)
                {
                    platFormAlarmThreeLevelRoadAlarm.stop();
                }
                tDeleteLogTimer.Stop();
                LogMsg logMsg = new LogMsg("Service", "OnStop", "成功 关闭");
                (new LogHelper()).WriteLog(logMsg);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                LogMsg logMsg1 = new LogMsg("Service", "OnStop", string.Concat("发生错误,", exception.Message));
                (new LogHelper()).WriteLog(logMsg1);
            }
            FileHelper.StopWrite();
        }
    }
}
