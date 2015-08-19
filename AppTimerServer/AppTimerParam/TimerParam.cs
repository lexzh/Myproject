using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Microsoft.Win32;
using System.Data.SqlClient;

namespace TimerServerParam
{
    public partial class AppTimerParam : Form
    {
        private static string sFileName = "param.xml";

        private string oldLogPaht = "";

        private int LBSPosMaxNum;

        private int LBSPosSleepTime;

        public AppTimerParam(string[] args)
        {
            if ((int)args.Length > 0)
            {
                base.Location = new Point(0, 0);
                SetParent((int)base.Handle, int.Parse(args[0]));
            }
            InitializeComponent();
        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = execTestConnect(e.Argument.ToString());
        }

        private void bgw_DoWorkEx(object sender, DoWorkEventArgs e)
        {
            e.Result = "";
            try
            {
                string str = txtLogPath.Text.Trim();
                string str1 = txtServerIp.Text.Trim();
                string str2 = numLogSaveDate.Value.ToString();
                string str3 = tbMapUrl.Text.Trim();
                string str4 = numPicTimeDiff.Value.ToString();
                string str5 = numCuffDiff.Value.ToString();
                string str6 = numCuffTime.Value.ToString();
                string str7 = dtpCuffBgnTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string str8 = numBroadCastTimeDiff.Value.ToString();
                string str9 = numBroadCastMsgTime.Value.ToString();
                string str10 = numGetLBSPosDataInterval.Value.ToString();
                string str11 = numLBSPosInterval.Value.ToString();
                string str12 = numLBSPosMaxNum.Value.ToString();
                string str13 = numLBSPosSleepTime.Value.ToString();
                string str14 = numLBSTpye.Value.ToString();
                string shortTimeString = datepickLBSStart.Value.ToShortTimeString();
                string shortTimeString1 = datepickLBSEnd.Value.ToShortTimeString();
                string str15 = numGetCurrentPosInterval.Value.ToString();
                string str16 = numLCSTime.Value.ToString();
                string str17 = (tbLCSPosTime.Text.Equals("") ? "15" : tbLCSPosTime.Text);
                string str18 = numLCSCount.Value.ToString();
                if (str1.Trim().Length == 0)
                {
                    e.Result = "通讯服务器IP不能为空";
                }
                else if (CheckIpAddress(str1))
                {
                    string str19 = "";
                    string dbParameter = getDbParameter(out str19);
                    if (dbParameter.Trim().Length == 0)
                    {
                        e.Result = string.Concat("数据库配置错误：", str19);
                    }
                    else if (execTestConnect(dbParameter).Length <= 0)
                    {
                        if (str.Equals(""))
                        {
                            txtLogPath.Text = oldLogPaht;
                        }
                        if (chkPort(str2))
                        {
                            if (chkLBSPos.Checked)
                            {
                                if (string.IsNullOrEmpty(numGetLBSPosDataInterval.Text))
                                {
                                    e.Result = "请输入读取数据间隔！";
                                    return;
                                }
                                else if (string.IsNullOrEmpty(numLBSPosInterval.Text))
                                {
                                    e.Result = "请输入批量定位间隔！";
                                    return;
                                }
                                else if (string.IsNullOrEmpty(numLBSPosMaxNum.Text))
                                {
                                    e.Result = "请输入批量定位上限！";
                                    return;
                                }
                                else if (string.IsNullOrEmpty(numLBSPosSleepTime.Text))
                                {
                                    e.Result = "请输入批量定位休眠时间！";
                                    return;
                                }
                                else if (string.IsNullOrEmpty(numLBSTpye.Text))
                                {
                                    e.Result = "请输入要定位的LBS终端的类型！";
                                    return;
                                }
                                else if (DateTime.Parse(shortTimeString) > DateTime.Parse(shortTimeString1))
                                {
                                    e.Result = "开始时间不能大于结束时间！";
                                    return;
                                }
                            }
                            if (cbIsBillPos.Checked)
                            {
                                if (string.IsNullOrEmpty(sltdays.Text))
                                {
                                    e.Result = "请输入读取监控订单的天数！";
                                    return;
                                }
                                else if (string.IsNullOrEmpty(sltBillTime.Text))
                                {
                                    e.Result = "请输入订单读取间隔！";
                                    return;
                                }
                            }
                            if (chkCuff.Checked)
                            {
                                if (string.IsNullOrEmpty(numCuffDiff.Text))
                                {
                                    e.Result = "请输入掉线报警的时间间隔!";
                                    return;
                                }
                                else if (chkIsSendMsg.Checked)
                                {
                                    MsgConfig();
                                }
                            }
                            if (!chkPic.Checked || !string.IsNullOrEmpty(numPicTimeDiff.Text))
                            {
                                if (chkBroadCast.Checked)
                                {
                                    if (string.IsNullOrEmpty(numBroadCastTimeDiff.Text))
                                    {
                                        e.Result = "读取播报信息时间间隔!";
                                        return;
                                    }
                                    else if (string.IsNullOrEmpty(numBroadCastMsgTime.Text))
                                    {
                                        e.Result = "同车播报信息时间间隔!";
                                        return;
                                    }
                                    else if (string.IsNullOrEmpty(numCuffTime.Text))
                                    {
                                        e.Result = "请输入播报信息的掉线时间!";
                                        return;
                                    }
                                }
                                if (chkAdminRegionAlarm.Checked && string.IsNullOrEmpty(numGetCurrentPosInterval.Text))
                                {
                                    e.Result = "请输入读取车辆最新位置报文的时间间隔！";
                                }
                                else if (!chkLCSPos.Checked || !string.IsNullOrEmpty(numLCSTime.Text))
                                {
                                    if (cbIsBillPos.Checked || chkAdminRegionAlarm.Checked || chkPathAlarm.Checked || chkPathSegmentAlarm.Checked)
                                    {
                                        if (tbMapUrl.Text.Trim().Length != 0)
                                        {
                                            string text = tbMapUrl.Text;
                                            if (text.IndexOf(':') >= 0)
                                            {
                                                string[] strArrays = text.Split(new char[] { ':' });
                                                if (!CheckIpAddress(strArrays[0]))
                                                {
                                                    e.Result = "地图ip地址不正确！";
                                                    return;
                                                }
                                                else if (!chkPort(strArrays[1]))
                                                {
                                                    e.Result = "地图端口格式不正确！";
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                e.Result = "地图地址不正确！";
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            e.Result = "地图服务器IP不能为空";
                                            return;
                                        }
                                    }
                                    setConfig("ConnectionString", dbParameter);
                                    setConfig("serverIP", str1);
                                    setConfig("MapUrl", str3);
                                    setConfig("logPath", str);
                                    setConfig("LogSaveDate", str2);
                                    bool @checked = chkPic.Checked;
                                    setConfig("IsPic", @checked.ToString());
                                    setConfig("PicTimeDiff", str4);
                                    setConfig("IsCuff", chkCuff.Checked.ToString());
                                    setConfig("CuffDiff", str5);
                                    setConfig("CuffTime", str6);
                                    setConfig("CuffBeginTime", str7);
                                    setConfig("IsSendMsg", chkIsSendMsg.Checked.ToString());
                                    setConfig("IsBroadCast", chkBroadCast.Checked.ToString());
                                    setConfig("BroadCastDiff", str8);
                                    setConfig("BroadCastMsgTime", str9);
                                    setConfig("IsLBSPos", chkLBSPos.Checked.ToString());
                                    setConfig("GetLBSPosDataInterval", str10);
                                    setConfig("LBSPosInterval", str11);
                                    setConfig("LBSPosMaxNum", str12);
                                    setConfig("LBSPosSleepTime", str13);
                                    setConfig("LBSType", str14);
                                    setConfig("LBSStartTime", shortTimeString);
                                    setConfig("LBSEndTime", shortTimeString1);
                                    setConfig("IsBillPos", cbIsBillPos.Checked.ToString());
                                    setConfig("Days", sltdays.Value.ToString());
                                    setConfig("BillTime", sltBillTime.Value.ToString());
                                    setConfig("IsAdminRegionAlarm", chkAdminRegionAlarm.Checked.ToString());
                                    setConfig("GetCurrentPosInterval", str15);
                                    setConfig("IsLCSPos", chkLCSPos.Checked.ToString());
                                    setConfig("LCSTime", str16);
                                    setConfig("LCSPosTime", str17);
                                    setConfig("LCSCount", str18);
                                    setConfig("IsBeBackOnTime", chkBeBackOnTime.Checked.ToString());
                                    setConfig("GetBeBackConfig", numBeBackConfig.Value.ToString());
                                    setConfig("GetBeBackPos", numGetBeBackPos.Value.ToString());
                                    setConfig("IsDWLBSPos", chkDWLBSPos.Checked.ToString());
                                    decimal value = numDWLBSTime.Value;
                                    setConfig("DWLBSTime", value.ToString());
                                    @checked = chkInOutOnTime.Checked;
                                    setConfig("IsInOutOfRangeOnTime", @checked.ToString());
                                    value = numInOutConfig.Value;
                                    setConfig("GetInOutConfig", value.ToString());
                                    value = numGetInOutPos.Value;
                                    setConfig("GetInOutPos", value.ToString());
                                    @checked = chkPathAlarm.Checked;
                                    setConfig("IsPathAlarm", @checked.ToString());
                                    @checked = chkPathSegmentAlarm.Checked;
                                    setConfig("IsPathSegmentAlarm", @checked.ToString());
                                    @checked = chkRegionAlarm.Checked;
                                    setConfig("IsRegionAlarm", @checked.ToString());
                                    value = numPathAlarmInterval.Value;
                                    setConfig("PathAlarmInterval", value.ToString());
                                    @checked = IsOnlyFillCheck.Checked;
                                    setConfig("IsOnlyFillCheck", @checked.ToString());
                                    @checked = IsContinuousAlarm.Checked;
                                    setConfig("IsContinuousAlarm", @checked.ToString());
                                    @checked = chkDownMsg.Checked;
                                    setConfig("chkDownMsg", @checked.ToString());
                                    @checked = chkJTBOnOffNotice.Checked;
                                    setConfig("IsJTBOnOffNotice", @checked.ToString());
                                    value = numJTBOnOffInterval.Value;
                                    setConfig("JTBOnOffInterval", value.ToString());
                                    value = numJTBOffLineTime.Value;
                                    setConfig("JTBOffLineTime", value.ToString());
                                    @checked = chkTerminalDemand.Checked;
                                    setConfig("IsTerminalDemand", @checked.ToString());
                                    value = numDownInterval.Value;
                                    setConfig("DownInterval", value.ToString());
                                    @checked = chkSeparateAndSticky.Checked;
                                    setConfig("IsSeparateAndSticky", @checked.ToString());
                                    value = numSeparateAndSticky.Value;
                                    setConfig("SeparateAndSticky", value.ToString());
                                    value = numAlarmInterval.Value;
                                    setConfig("AlarmInterval", value.ToString());
                                    @checked = cbGatheredAlarm.Checked;
                                    setConfig("IsGatheredAlarm", @checked.ToString());
                                    value = numGatheredAlarmInterval.Value;
                                    setConfig("GatheredAlarmInterval", value.ToString());
                                    value = numEffectiveTime.Value;
                                    setConfig("EffectiveTime", value.ToString());
                                    @checked = cbCurrentAddress.Checked;
                                    setConfig("IsCurrentAddress", @checked.ToString());
                                    value = numCurrentAddressInterval.Value;
                                    setConfig("CurrentAddressInterval", value.ToString());
                                    @checked = cbMsgRemind.Checked;
                                    setConfig("IsMsgRemind", @checked.ToString());
                                    setConfig("WarnMsg1", tbWarnMsg1.Text);
                                    setConfig("WarnMsg2", tbWarnMsg2.Text);
                                    setConfig("WarnMsg3", tbWarnMsg3.Text);
                                    value = numAppointedTime.Value;
                                    setConfig("AppointedTime", value.ToString());
                                    value = numTerminalTypeID.Value;
                                    setConfig("TerminalTypeID", value.ToString());
                                    @checked = chkPZMsg.Checked;
                                    setConfig("IsSendPZMsg", @checked.ToString());
                                    value = numPZType.Value;
                                    setConfig("numPZType", value.ToString());
                                    setConfig("tbPZMsg", tbPZMsg.Text);
                                    value = numPZInterval.Value;
                                    setConfig("numPZInterval", value.ToString());
                                    @checked = chkForbidDriveAlarm.Checked;
                                    setConfig("IsForbidDriveAlarm", @checked.ToString());
                                    value = numForbidDriveInterval.Value;
                                    setConfig("numForbidDriveInterval", value.ToString());
                                    @checked = chkIsSendDriverAlarm.Checked;
                                    setConfig("IsSendDriverAlarm", @checked.ToString());
                                    setConfig("SendDriverAlarmMsg", txtAlarmDriverMsg.Text.Trim());
                                    @checked = chkThreeLevelRoadAlarm.Checked;
                                    setConfig("IsThreeLevelRoadAlarm", @checked.ToString());
                                    value = numThreeLevelRoad.Value;
                                    setConfig("numThreeLevelRoadInterval", value.ToString());
                                    @checked = chkRoadSpeedAndRank.Checked;
                                    setConfig("chkRoadSpeedAndRank", @checked.ToString());
                                    value = RoadSpeedAndRankInterval.Value;
                                    setConfig("RoadSpeedAndRankInterval", value.ToString());
                                    @checked = chkIsSend.Checked;
                                    setConfig("IsSend", @checked.ToString());
                                    setConfig("SendMsg", txtMsg.Text.Trim());
                                }
                                else
                                {
                                    e.Result = "请输入手机被动监控定位检测间隔！";
                                }
                            }
                            else
                            {
                                e.Result = "请输入定时拍照的时间间隔!";
                            }
                        }
                        else
                        {
                            e.Result = "日志保存天数必须为1～30的整数！";
                        }
                    }
                    else
                    {
                        e.Result = "该配置无法正常连接数据库，不保存该配置！";
                    }
                }
                else
                {
                    e.Result = "通讯服务器IP格式不正确！";
                }
            }
            catch (Exception exception)
            {
                e.Result = exception.Message;
            }
        }

        private void bgw_DoWorkRun(object sender, DoWorkEventArgs e)
        {
            ServerManager serverManager = GetServerManager();
            string str = "";
            try
            {
                if (!IsExistService(serverManager, out str))
                {
                    e.Result = str;
                }
                else if (e.Argument.ToString().Equals("启动服务"))
                {
                    serverManager.StarService();
                    e.Result = "启动服务成功!";
                }
                else if (e.Argument.ToString().Equals("停止服务"))
                {
                    serverManager.StopService();
                    e.Result = "停止服务成功!";
                }
                else if (e.Argument.ToString().Equals("重启服务"))
                {
                    serverManager.ReStarService();
                    e.Result = "重启服务成功！";
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                e.Result = string.Concat(e.Argument.ToString(), "发生错误：", exception.Message);
            }
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string str = e.Result.ToString();
            if (str.Length != 0)
            {
                MessageBox.Show(str, "数据库连接测试", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("连接成功！", "数据库连接测试", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            SetControlEnable(true);
            lblShowInfo.Visible = false;
        }

        private void bgw_RunWorkerCompletedEx(object sender, RunWorkerCompletedEventArgs e)
        {
            string str = e.Result.ToString();
            if (str.Length != 0)
            {
                MessageBox.Show(str, "参数配置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("参数配置成功,请重新启动服务。", "参数配置", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            SetControlEnable(true);
        }

        private void bgw_RunWorkerCompletedRun(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show(e.Result.ToString());
            SetControlEnable(true);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnRestarService_Click(object sender, EventArgs e)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(bgw_DoWorkRun);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompletedRun);
            backgroundWorker.RunWorkerAsync("重启服务");
            SetControlEnable(false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnStarService_Click(object sender, EventArgs e)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(bgw_DoWorkRun);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompletedRun);
            backgroundWorker.RunWorkerAsync("启动服务");
            SetControlEnable(false);
        }

        private void btnStopService_Click(object sender, EventArgs e)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(bgw_DoWorkRun);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompletedRun);
            backgroundWorker.RunWorkerAsync("停止服务");
            SetControlEnable(false);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            string str = "";
            string dbParameter = getDbParameter(out str);
            if (dbParameter.Trim().Length == 0)
            {
                MessageBox.Show(str, "数据库连接测试", MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(bgw_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            backgroundWorker.RunWorkerAsync(dbParameter);
            SetControlEnable(false);
            lblShowInfo.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (fbdPath.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtLogPath.Text = fbdPath.SelectedPath;
            }
        }

        private void cbBeBackOnTime_CheckedChanged(object sender, EventArgs e)
        {
            pnlBeBackOnTime.Enabled = chkBeBackOnTime.Checked;
        }

        private void cbCurrentAddress_CheckedChanged(object sender, EventArgs e)
        {
            pnlCurrentAddress.Enabled = cbCurrentAddress.Checked;
        }

        private void cbGatheredAlarm_CheckedChanged(object sender, EventArgs e)
        {
            pnlGatheredAlarm.Enabled = cbGatheredAlarm.Checked;
        }

        private void cbIsBillPos_CheckedChanged(object sender, EventArgs e)
        {
            pnlLocation.Enabled = cbIsBillPos.Checked;
        }

        private void cbMsgRemind_CheckedChanged(object sender, EventArgs e)
        {
            pnlMsgRemind.Enabled = cbMsgRemind.Checked;
        }

        public static bool CheckIpAddress(string strIpAddress)
        {
            return true;
        }

        private void chkAdminRegionAlarm_CheckedChanged(object sender, EventArgs e)
        {
            pnlAdminRegionAlarm.Enabled = chkAdminRegionAlarm.Checked;
        }

        private void chkBroadCast_CheckedChanged(object sender, EventArgs e)
        {
            pnlBroadCast.Enabled = chkBroadCast.Checked;
        }

        private void chkCuff_CheckedChanged(object sender, EventArgs e)
        {
            pnlCuff.Enabled = chkCuff.Checked;
        }

        private void chkDWLBSPos_CheckedChanged(object sender, EventArgs e)
        {
            pnlDWLBSPos.Enabled = chkDWLBSPos.Checked;
        }

        private void chkForbidDriveAlarm_CheckedChanged(object sender, EventArgs e)
        {
            pnlForbidDrive.Enabled = chkForbidDriveAlarm.Checked;
        }

        private void chkInOutOnTime_CheckedChanged(object sender, EventArgs e)
        {
            pnlInOutOnTime.Enabled = chkInOutOnTime.Checked;
        }

        private void chkIsSend_CheckedChanged(object sender, EventArgs e)
        {
            gbMessage.Enabled = chkIsSend.Checked;
        }

        private void chkIsSendDriverAlarm_CheckedChanged(object sender, EventArgs e)
        {
            gbAlarmMessage.Enabled = chkIsSendDriverAlarm.Checked;
        }

        private void chkIsSendMsg_CheckedChanged(object sender, EventArgs e)
        {
            gbMsg.Enabled = chkIsSendMsg.Checked;
        }

        private void chkJTBOnOffNotice_CheckedChanged(object sender, EventArgs e)
        {
            pnlJTBOnOffNotice.Enabled = chkJTBOnOffNotice.Checked;
        }

        private void chkLBSPos_CheckedChanged(object sender, EventArgs e)
        {
            pnlLBS.Enabled = chkLBSPos.Checked;
        }

        private void chkLCSPos_CheckedChanged(object sender, EventArgs e)
        {
            pnlMobile.Enabled = chkLCSPos.Checked;
        }

        private void chkPathAlarm_CheckedChanged(object sender, EventArgs e)
        {
            pnlPlatformAlarm.Enabled = (chkPathAlarm.Checked || chkPathSegmentAlarm.Checked ? true : chkRegionAlarm.Checked);
        }

        private void chkPic_CheckedChanged(object sender, EventArgs e)
        {
            pnlPic.Enabled = chkPic.Checked;
        }

        private bool chkPort(string sPort)
        {
            int num;
            if (!int.TryParse(sPort, out num))
            {
                return false;
            }
            if (num > 0 && num <= 65535)
            {
                return true;
            }
            return false;
        }

        private void chkPZMsg_CheckedChanged(object sender, EventArgs e)
        {
            pnlPZMsg.Enabled = chkPZMsg.Checked;
        }

        private void chkRoadSpeedAndRank_CheckedChanged(object sender, EventArgs e)
        {
            pnlRoadSpeedAndRank.Enabled = chkRoadSpeedAndRank.Checked;
        }

        private void chkSeparateAndSticky_CheckedChanged(object sender, EventArgs e)
        {
            pnlSeparateAndSticky.Enabled = chkSeparateAndSticky.Checked;
        }

        private void chkTerminalDemand_CheckedChanged(object sender, EventArgs e)
        {
            pnlTerminalDemand.Enabled = chkTerminalDemand.Checked;
        }

        private void chkThreeLevelRoadAlarm_CheckedChanged(object sender, EventArgs e)
        {
            pnlThreeLevelRoad.Enabled = chkThreeLevelRoadAlarm.Checked;
        }

        private string execTestConnect(string sDbParameter)
        {
            string message;
            SqlConnection sqlConnection = new SqlConnection()
            {
                ConnectionString = sDbParameter
            };
            try
            {
                sqlConnection.Open();
                sqlConnection.Close();
                message = "";
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }
            return message;
        }

        private string getConfig(XmlNode xNode, string appKey)
        {
            XmlElement xmlElement = (XmlElement)xNode.SelectSingleNode(string.Format("//{0}", appKey));
            if (xmlElement == null)
            {
                return "";
            }
            return xmlElement.InnerText;
        }

        private string getDbParameter(out string sError)
        {
            string str = txtDataSource.Text.Trim();
            string str1 = txtInitialCatalog.Text.Trim();
            string str2 = txtUserId.Text.Trim();
            string str3 = txtPassword.Text.Trim();
            if (str.Length == 0)
            {
                sError = "服务器IP不能为空！";
                txtDataSource.Focus();
                return "";
            }
            if (!CheckIpAddress(str))
            {
                sError = "ip地址不正确！";
                return "";
            }
            if (str1.Length == 0)
            {
                sError = "数据库名不能为空！";
                txtInitialCatalog.Focus();
                return "";
            }
            if (str2.Length == 0)
            {
                sError = "用户名不能为空！";
                txtUserId.Focus();
                return "";
            }
            if (str3.Length == 0)
            {
                sError = "密码不能为空！";
                txtPassword.Focus();
                return "";
            }
            sError = "";
            string str4 = string.Concat("", "server=", str, ";");
            str4 = string.Concat(str4, "Persist Security Info=True;");
            str4 = string.Concat(str4, "uid=", str2, ";");
            str4 = string.Concat(str4, "pwd=", str3, ";");
            return string.Concat(str4, "database=", str1, ";");
        }

        private string GetPathFile()
        {
            return string.Concat(Application.StartupPath, "\\", sFileName);
        }

        private ServerManager GetServerManager()
        {
            return new ServerManager("GpsAppTimerServer");
        }

        private bool IsExistService(ServerManager serverManger, out string ErrorMsg)
        {
            ErrorMsg = "";
            if (serverManger.IsExistService)
            {
                return true;
            }
            ErrorMsg = "服务不存在,请检查!";
            return false;
        }

        private bool IsMulPos()
        {
            bool flag;
            try
            {
                string pathFile = GetPathFile();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(pathFile);
                XmlNode xmlNodes = xmlDocument.SelectSingleNode("//param");
                bool flag1 = false;
                string config = getConfig(xmlNodes, "LBSMuliPosTypes");
                if (!string.IsNullOrEmpty(config))
                {
                    string[] strArrays = config.Split(new char[] { ',' });
                    int num = 0;
                    while (num < (int)strArrays.Length)
                    {
                        string str = strArrays[num];
                        if (int.Parse(numLBSTpye.Value.ToString()) != int.Parse(str))
                        {
                            num++;
                        }
                        else
                        {
                            flag1 = true;
                            break;
                        }
                    }
                }
                flag = flag1;
            }
            catch (Exception exception)
            {
                flag = false;
            }
            return flag;
        }

        private void MsgConfig()
        {
            try
            {
                string text = txtSendMsg.Text;
                if (text.Length <= 150)
                {
                    string str = string.Concat("HKEY_LOCAL_MACHINE", "\\", "SOFTWARE\\StarNet\\GpsWebServices\\1.0");
                    Registry.SetValue(str, "AlarmSndMsgtext", text);
                }
                else
                {
                    MessageBox.Show("短信内容已超过150个字，请修改！");
                }
            }
            catch
            {
            }
        }

        private void numLBSTpye_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!numLBSTpye.Text.Equals(""))
                {
                    if (!IsMulPos())
                    {
                        lbMaxCount.Enabled = false;
                        lbMaxCountContent.Enabled = false;
                        numLBSPosMaxNum.Enabled = false;
                        numLBSPosInterval.Minimum = new decimal(10);
                    }
                    else
                    {
                        lbMaxCount.Enabled = true;
                        lbMaxCountContent.Enabled = true;
                        numLBSPosMaxNum.Enabled = true;
                        numLBSPosInterval.Minimum = new decimal(1);
                    }
                }
            }
            catch
            {
            }
        }

        private void numLogSaveDate_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(numLogSaveDate.Text))
            {
                numLogSaveDate.Value = new decimal(7);
            }
        }

        private void Save()
        {
            try
            {
                BackgroundWorker backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += new DoWorkEventHandler(bgw_DoWorkEx);
                backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompletedEx);
                backgroundWorker.RunWorkerAsync();
                SetControlEnable(false);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void setConfig(string appKey, string AppValue)
        {
            string pathFile = GetPathFile();
            XmlDocument xmlDocument = new XmlDocument();
            if (File.Exists(pathFile))
            {
                xmlDocument.Load(pathFile);
            }
            else
            {
                XmlNode xmlNodes = xmlDocument.CreateElement("param");
                XmlNode xmlNodes1 = xmlDocument.CreateElement("LBSMuliPosTypes");
                xmlNodes1.InnerText = "100";
                xmlNodes.AppendChild(xmlNodes1);
                XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmlDocument.AppendChild(xmlDeclaration);
                xmlDocument.AppendChild(xmlNodes);
            }
            XmlNode xmlNodes2 = xmlDocument.SelectSingleNode("//param");
            if (xmlNodes2 == null)
            {
                xmlDocument.AppendChild(xmlDocument.CreateElement("param"));
                xmlNodes2 = xmlDocument.SelectSingleNode("//param");
            }
            XmlElement appValue = (XmlElement)xmlNodes2.SelectSingleNode(string.Concat("//", appKey));
            if (appValue == null)
            {
                XmlElement xmlElement = xmlDocument.CreateElement(appKey);
                xmlElement.InnerText = AppValue;
                xmlNodes2.AppendChild(xmlElement);
            }
            else
            {
                appValue.InnerText = AppValue;
            }
            xmlDocument.Save(pathFile);
        }

        private void setConfigKey(string AppKey, string AppValue)
        {
            string str = string.Concat(Application.StartupPath, sFileName);
            XmlDocument xmlDocument = new XmlDocument();
            if (File.Exists(str))
            {
                xmlDocument.Load(str);
            }
            else
            {
                XmlNode xmlNodes = xmlDocument.CreateElement("param");
                XmlNode xmlNodes1 = xmlDocument.CreateElement("LBSMuliPosTypes");
                xmlNodes1.InnerText = "100";
                xmlNodes.AppendChild(xmlNodes1);
                XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmlDocument.AppendChild(xmlDeclaration);
                xmlDocument.AppendChild(xmlNodes);
            }
            XmlNode xmlNodes2 = xmlDocument.SelectSingleNode("//param");
            if (xmlNodes2 == null)
            {
                xmlDocument.AppendChild(xmlDocument.CreateElement("param"));
                xmlNodes2 = xmlDocument.SelectSingleNode("//param");
            }
            XmlElement xmlElement = (XmlElement)xmlNodes2.SelectSingleNode(string.Concat("//add[@key='", AppKey, "']"));
            if (xmlElement == null)
            {
                XmlElement xmlElement1 = xmlDocument.CreateElement("add");
                xmlElement1.SetAttribute("key", AppKey);
                xmlElement1.SetAttribute("value", AppValue);
                xmlNodes2.AppendChild(xmlElement1);
            }
            else
            {
                xmlElement.SetAttribute("value", AppValue);
            }
            xmlDocument.Save(str);
        }

        private void SetControlEnable(bool isuse)
        {
            tabControl1.Enabled = isuse;
            pnlOk.Enabled = isuse;
            base.ControlBox = isuse;
        }

        private void SetParam_Load(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tpJTBfunction);
            string pathFile = GetPathFile();
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(pathFile);
                XmlNode xmlNodes = xmlDocument.SelectSingleNode("//param");
                txtServerIp.Text = getConfig(xmlNodes, "serverIP");
                tbMapUrl.Text = getConfig(xmlNodes, "MapUrl");
                string config = getConfig(xmlNodes, "ConnectionString");
                if (!string.IsNullOrEmpty(config))
                {
                    string[] strArrays = config.Split(new char[] { ';' });
                    for (int i = 0; i < (int)strArrays.Length; i++)
                    {
                        string[] strArrays1 = strArrays[i].Trim().Split(new char[] { '=' });
                        string lower = strArrays1[0].ToLower();
                        string str = lower;
                        if (lower != null)
                        {
                            if (str == "server")
                            {
                                txtDataSource.Text = strArrays1[1];
                            }
                            else if (str == "database")
                            {
                                txtInitialCatalog.Text = strArrays1[1];
                            }
                            else if (str == "uid")
                            {
                                txtUserId.Text = strArrays1[1];
                            }
                            else if (str == "pwd")
                            {
                                txtPassword.Text = strArrays1[1];
                            }
                        }
                    }
                    txtLogPath.Text = getConfig(xmlNodes, "logPath");
                    oldLogPaht = getConfig(xmlNodes, "logPath");
                    numLogSaveDate.Value = Convert.ToDecimal(getConfig(xmlNodes, "LogSaveDate"));
                    bool flag = true;
                    bool.TryParse(getConfig(xmlNodes, "IsPic"), out flag);
                    chkPic.Checked = flag;
                    numPicTimeDiff.Value = Convert.ToDecimal(getConfig(xmlNodes, "PicTimeDiff"));
                    bool flag1 = true;
                    bool.TryParse(getConfig(xmlNodes, "IsCuff"), out flag1);
                    chkCuff.Checked = flag1;
                    numCuffDiff.Value = Convert.ToDecimal(getConfig(xmlNodes, "CuffDiff"));
                    numCuffTime.Value = Convert.ToDecimal(getConfig(xmlNodes, "CuffTime"));
                    dtpCuffBgnTime.Value = Convert.ToDateTime(getConfig(xmlNodes, "CuffBeginTime"));
                    bool flag2 = true;
                    bool.TryParse(getConfig(xmlNodes, "IsSendMsg"), out flag2);
                    chkIsSendMsg.Checked = flag2;
                    try
                    {
                        txtSendMsg.Text = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\StarNet\\GpsWebServices\\1.0", "AlarmSndMsgtext", "");
                    }
                    catch
                    {
                        txtSendMsg.Text = "";
                    }
                    bool flag3 = true;
                    bool.TryParse(getConfig(xmlNodes, "IsBroadCast"), out flag3);
                    chkBroadCast.Checked = flag3;
                    numBroadCastTimeDiff.Value = Convert.ToDecimal(getConfig(xmlNodes, "BroadCastDiff"));
                    numBroadCastMsgTime.Value = Convert.ToDecimal(getConfig(xmlNodes, "BroadCastMsgTime"));
                    bool flag4 = true;
                    bool.TryParse(getConfig(xmlNodes, "IsLBSPos"), out flag4);
                    chkLBSPos.Checked = flag4;
                    numGetLBSPosDataInterval.Value = Convert.ToDecimal(getConfig(xmlNodes, "GetLBSPosDataInterval"));
                    numLBSPosInterval.Value = Convert.ToDecimal(getConfig(xmlNodes, "LBSPosInterval"));
                    numLBSPosMaxNum.Value = Convert.ToDecimal(getConfig(xmlNodes, "LBSPosMaxNum"));
                    decimal value = numLBSPosMaxNum.Value;
                    LBSPosMaxNum = int.Parse(value.ToString());
                    numLBSPosSleepTime.Value = Convert.ToDecimal(getConfig(xmlNodes, "LBSPosSleepTime"));
                    decimal num = numLBSPosSleepTime.Value;
                    LBSPosSleepTime = int.Parse(num.ToString());
                    numLBSTpye.Value = Convert.ToDecimal(getConfig(xmlNodes, "LBSType"));
                    datepickLBSStart.Value = DateTime.Parse(getConfig(xmlNodes, "LBSStartTime").ToString());
                    datepickLBSEnd.Value = DateTime.Parse(getConfig(xmlNodes, "LBSEndTime").ToString());
                    if (!IsMulPos())
                    {
                        lbMaxCount.Enabled = false;
                        lbMaxCountContent.Enabled = false;
                        numLBSPosMaxNum.Enabled = false;
                        numLBSPosInterval.Minimum = new decimal(10);
                    }
                    else
                    {
                        lbMaxCount.Enabled = true;
                        lbMaxCountContent.Enabled = true;
                        numLBSPosMaxNum.Enabled = true;
                        numLBSPosInterval.Minimum = new decimal(1);
                    }
                    bool flag5 = true;
                    bool.TryParse(getConfig(xmlNodes, "IsBillPos"), out flag5);
                    cbIsBillPos.Checked = flag5;
                    sltdays.Value = Convert.ToDecimal(getConfig(xmlNodes, "Days"));
                    sltBillTime.Value = Convert.ToDecimal(getConfig(xmlNodes, "BillTime"));
                    bool flag6 = true;
                    bool.TryParse(getConfig(xmlNodes, "IsAdminRegionAlarm"), out flag6);
                    chkAdminRegionAlarm.Checked = flag6;
                    numGetCurrentPosInterval.Value = Convert.ToDecimal(getConfig(xmlNodes, "GetCurrentPosInterval"));
                    bool flag7 = true;
                    bool.TryParse(getConfig(xmlNodes, "IsLCSPos"), out flag7);
                    chkLCSPos.Checked = flag7;
                    numLCSTime.Value = Convert.ToDecimal(getConfig(xmlNodes, "LCSTime"));
                    numLCSCount.Value = Convert.ToDecimal(getConfig(xmlNodes, "LCSCount"));
                    tbLCSPosTime.Text = getConfig(xmlNodes, "LCSPosTime");
                    bool flag8 = true;
                    bool.TryParse(getConfig(xmlNodes, "IsBeBackOnTime"), out flag8);
                    chkBeBackOnTime.Checked = flag8;
                    numBeBackConfig.Value = Convert.ToDecimal(getConfig(xmlNodes, "GetBeBackConfig"));
                    numGetBeBackPos.Value = Convert.ToDecimal(getConfig(xmlNodes, "GetBeBackPos"));
                    bool flag9 = true;
                    bool.TryParse(getConfig(xmlNodes, "IsDWLBSPos"), out flag9);
                    chkDWLBSPos.Checked = flag9;
                    numDWLBSTime.Value = Convert.ToDecimal(getConfig(xmlNodes, "DWLBSTime"));
                    bool flag10 = true;
                    bool.TryParse(getConfig(xmlNodes, "IsInOutOfRangeOnTime"), out flag10);
                    chkInOutOnTime.Checked = flag10;
                    numInOutConfig.Value = Convert.ToDecimal(getConfig(xmlNodes, "GetInOutConfig"));
                    numGetInOutPos.Value = Convert.ToDecimal(getConfig(xmlNodes, "GetInOutPos"));
                    bool flag11 = true;
                    bool flag12 = true;
                    bool flag13 = true;
                    bool.TryParse(getConfig(xmlNodes, "IsPathAlarm"), out flag11);
                    chkPathAlarm.Checked = flag11;
                    bool.TryParse(getConfig(xmlNodes, "IsPathSegmentAlarm"), out flag12);
                    chkPathSegmentAlarm.Checked = flag12;
                    bool.TryParse(getConfig(xmlNodes, "IsRegionAlarm"), out flag13);
                    chkRegionAlarm.Checked = flag13;
                    numPathAlarmInterval.Value = Convert.ToDecimal(getConfig(xmlNodes, "PathAlarmInterval"));
                    bool flag14 = true;
                    bool.TryParse(getConfig(xmlNodes, "IsOnlyFillCheck"), out flag14);
                    IsOnlyFillCheck.Checked = flag14;
                    bool flag15 = false;
                    bool.TryParse(getConfig(xmlNodes, "IsContinuousAlarm"), out flag15);
                    IsContinuousAlarm.Checked = flag15;
                    bool flag16 = false;
                    bool.TryParse(getConfig(xmlNodes, "chkDownMsg"), out flag16);
                    chkDownMsg.Checked = flag16;
                    bool flag17 = true;
                    bool.TryParse(getConfig(xmlNodes, "IsJTBOnOffNotice"), out flag17);
                    chkJTBOnOffNotice.Checked = flag17;
                    numJTBOnOffInterval.Value = Convert.ToDecimal(getConfig(xmlNodes, "JTBOnOffInterval"));
                    numJTBOffLineTime.Value = Convert.ToDecimal(getConfig(xmlNodes, "JTBOffLineTime"));
                    bool flag18 = true;
                    bool.TryParse(getConfig(xmlNodes, "IsTerminalDemand"), out flag18);
                    chkTerminalDemand.Checked = flag18;
                    numDownInterval.Value = Convert.ToDecimal(getConfig(xmlNodes, "DownInterval"));
                    bool flag19 = false;
                    bool.TryParse(getConfig(xmlNodes, "IsSeparateAndSticky"), out flag19);
                    chkSeparateAndSticky.Checked = flag19;
                    numSeparateAndSticky.Value = Convert.ToDecimal(getConfig(xmlNodes, "SeparateAndSticky"));
                    numAlarmInterval.Value = Convert.ToDecimal(getConfig(xmlNodes, "AlarmInterval"));
                    bool flag20 = false;
                    bool.TryParse(getConfig(xmlNodes, "IsGatheredAlarm"), out flag20);
                    cbGatheredAlarm.Checked = flag20;
                    numGatheredAlarmInterval.Value = Convert.ToDecimal(getConfig(xmlNodes, "GatheredAlarmInterval"));
                    numEffectiveTime.Value = Convert.ToDecimal(getConfig(xmlNodes, "EffectiveTime"));
                    bool flag21 = false;
                    bool.TryParse(getConfig(xmlNodes, "IsCurrentAddress"), out flag21);
                    cbCurrentAddress.Checked = flag21;
                    numCurrentAddressInterval.Value = Convert.ToDecimal(getConfig(xmlNodes, "CurrentAddressInterval"));
                    bool flag22 = false;
                    bool.TryParse(getConfig(xmlNodes, "IsMsgRemind"), out flag22);
                    cbMsgRemind.Checked = flag22;
                    tbWarnMsg1.Text = getConfig(xmlNodes, "WarnMsg1");
                    tbWarnMsg2.Text = getConfig(xmlNodes, "WarnMsg2");
                    tbWarnMsg3.Text = getConfig(xmlNodes, "WarnMsg3");
                    numAppointedTime.Value = Convert.ToDecimal(getConfig(xmlNodes, "AppointedTime"));
                    numTerminalTypeID.Value = Convert.ToDecimal(getConfig(xmlNodes, "TerminalTypeID"));
                    bool flag23 = false;
                    bool.TryParse(getConfig(xmlNodes, "IsSendPZMsg"), out flag23);
                    chkPZMsg.Checked = flag23;
                    numPZType.Value = Convert.ToDecimal(getConfig(xmlNodes, "numPZType"));
                    tbPZMsg.Text = getConfig(xmlNodes, "tbPZMsg");
                    numPZInterval.Value = Convert.ToDecimal(getConfig(xmlNodes, "numPZInterval"));
                    bool flag24 = false;
                    bool.TryParse(getConfig(xmlNodes, "IsForbidDriveAlarm"), out flag24);
                    chkForbidDriveAlarm.Checked = flag24;
                    numForbidDriveInterval.Value = decimal.Parse(getConfig(xmlNodes, "numForbidDriveInterval"));
                    bool flag25 = false;
                    bool.TryParse(getConfig(xmlNodes, "IsSendDriverAlarm"), out flag25);
                    chkIsSendDriverAlarm.Checked = flag25;
                    txtAlarmDriverMsg.Text = getConfig(xmlNodes, "SendDriverAlarmMsg");
                    bool flag26 = false;
                    bool.TryParse(getConfig(xmlNodes, "IsThreeLevelRoadAlarm"), out flag26);
                    chkThreeLevelRoadAlarm.Checked = flag26;
                    numThreeLevelRoad.Value = decimal.Parse(getConfig(xmlNodes, "numThreeLevelRoadInterval"));
                    bool flag27 = false;
                    bool.TryParse(getConfig(xmlNodes, "chkRoadSpeedAndRank"), out flag27);
                    chkRoadSpeedAndRank.Checked = flag27;
                    RoadSpeedAndRankInterval.Value = decimal.Parse(getConfig(xmlNodes, "RoadSpeedAndRankInterval"));
                    bool flag28 = false;
                    bool.TryParse(getConfig(xmlNodes, "IsSend"), out flag28);
                    chkIsSend.Checked = flag28;
                    txtMsg.Text = getConfig(xmlNodes, "SendMsg");
                }
            }
            catch (Exception exception)
            {
                string message = exception.Message;
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.None, ExactSpelling = false)]
        private static extern IntPtr SetParent(int hWndChild, int hWndNewParent);

        private void txtLogPath_Leave(object sender, EventArgs e)
        {
            if (txtLogPath.Text.EndsWith(""))
            {
                txtLogPath.Text = oldLogPaht;
            }
        }
	
    }
}
