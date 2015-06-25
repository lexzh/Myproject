using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Xml;

namespace AppServerParam
{
    public partial class SetParam : Form
    {
        private static string sFileName = "\\param.xml";

        private int LBSPosMaxNum;

        private int LBSPosSleepTime;

        public SetParam()
        {
            InitializeComponent();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnMsgOk_Click(object sender, EventArgs e)
        {
            try
            {
                string text = this.txtSendMsg.Text;
                if (text.Length <= 150)
                {
                    string str = string.Concat("HKEY_LOCAL_MACHINE", "\\", "SOFTWARE\\StarNet\\GpsWebServices\\1.0");
                    Registry.SetValue(str, "AlarmSndMsgtext", Encoding.Default.GetBytes(text));
                    MessageBox.Show("短信内容已保存！");
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

        private void btnRestarService_Click(object sender, EventArgs e)
        {
            ServiceController servicObject = this.GetServicObject();
            try
            {
                this.ReStarService(servicObject);
                MessageBox.Show("重服务成功！");
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(string.Concat("重启服务发生错误：", exception.Message));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.Save())
            {
                MessageBox.Show("参数配置成功,请重新启动服务。", "参数配置", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        private void btnStarService_Click(object sender, EventArgs e)
        {
            ServiceController servicObject = this.GetServicObject();
            try
            {
                this.StarService(servicObject);
                MessageBox.Show("启动服务成功!");
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(string.Concat("启动服务发生错误：", exception.Message));
            }
        }

        private void btnStopService_Click(object sender, EventArgs e)
        {
            ServiceController servicObject = this.GetServicObject();
            try
            {
                this.StopService(servicObject);
                MessageBox.Show("停止服务成功!");
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(string.Concat("停止服务发生错误：", exception.Message));
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (this.Save())
            {
                MessageBox.Show("参数配置成功。", "参数配置", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            string str = "";
            string dbParameter = this.getDbParameter(out str);
            if (dbParameter.Trim().Length == 0)
            {
                MessageBox.Show(str, "数据库连接测试", MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }
            string str1 = this.execTestConnect(dbParameter);
            if (str1.Length == 0)
            {
                MessageBox.Show("连接成功！", "数据库连接测试", MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }
            MessageBox.Show(str1, "数据库连接测试", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.fbdPath.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtLogPath.Text = this.fbdPath.SelectedPath;
            }
        }

        private void cbIsBillPos_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlLocation.Enabled = this.cbIsBillPos.Checked;
        }

        public static bool CheckIpAddress(string strIpAddress)
        {
            int num = 0;
            if (strIpAddress == null || strIpAddress.Length <= 0)
            {
                return false;
            }
            string[] strArrays = strIpAddress.Split(new char[] { '.' });
            if ((int)strArrays.Length < 4)
            {
                return false;
            }
            for (int i = 0; i < (int)strArrays.Length; i++)
            {
                if (!int.TryParse(strArrays[i], out num))
                {
                    return false;
                }
                if (num > 255 || num < 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void chkAdminRegionAlarm_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlAdminRegionAlarm.Enabled = this.chkAdminRegionAlarm.Checked;
        }

        private void chkBroadCast_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlBroadCast.Enabled = this.chkBroadCast.Checked;
        }

        private void chkCuff_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlCuff.Enabled = this.chkCuff.Checked;
        }

        private void chkIsSendMsg_CheckedChanged(object sender, EventArgs e)
        {
            this.gbMsg.Enabled = this.chkIsSendMsg.Checked;
        }

        private void chkLBSPos_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlLBS.Enabled = this.chkLBSPos.Checked;
        }

        private void chkPic_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlPic.Enabled = this.chkPic.Checked;
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
            string str = this.txtDataSource.Text.Trim();
            string str1 = this.txtInitialCatalog.Text.Trim();
            string str2 = this.txtUserId.Text.Trim();
            string str3 = this.txtPassword.Text.Trim();
            if (str.Length == 0)
            {
                sError = "服务器IP不能为空！";
                this.txtDataSource.Focus();
                return "";
            }
            if (!SetParam.CheckIpAddress(str))
            {
                sError = "ip地址不正确！";
                return "";
            }
            if (str1.Length == 0)
            {
                sError = "数据库名不能为空！";
                this.txtInitialCatalog.Focus();
                return "";
            }
            if (str2.Length == 0)
            {
                sError = "用户名不能为空！";
                this.txtUserId.Focus();
                return "";
            }
            if (str3.Length == 0)
            {
                sError = "密码不能为空！";
                this.txtPassword.Focus();
                return "";
            }
            sError = "";
            string str4 = string.Concat("", "server=", str, ";");
            str4 = string.Concat(str4, "Persist Security Info=True;");
            str4 = string.Concat(str4, "uid=", str2, ";");
            str4 = string.Concat(str4, "pwd=", str3, ";");
            return string.Concat(str4, "database=", str1, ";");
        }

        private ServiceController GetServicObject()
        {
            return new ServiceController("GpsAppTimerServer");
        }

        private void numLBSTpye_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.numLBSTpye.Text.Equals(""))
                {
                    if (int.Parse(this.numLBSTpye.Value.ToString()) != 100)
                    {
                        this.lbMaxCount.Enabled = false;
                        this.lbMaxCountContent.Enabled = false;
                        this.numLBSPosMaxNum.Enabled = false;
                        this.numLBSPosInterval.Minimum = new decimal(15);
                        this.numLBSPosSleepTime.Minimum = new decimal(5);
                    }
                    else
                    {
                        this.lbMaxCount.Enabled = true;
                        this.lbMaxCountContent.Enabled = true;
                        this.numLBSPosMaxNum.Enabled = true;
                        this.numLBSPosInterval.Minimum = new decimal(1);
                        this.numLBSPosSleepTime.Minimum = new decimal(0);
                    }
                }
            }
            catch
            {
            }
        }

        private void ReStarService(ServiceController sc)
        {
            this.StopService(sc);
            this.StarService(sc);
        }

        private bool Save()
        {
            bool flag;
            try
            {
                string str = this.txtLogPath.Text.Trim();
                string str1 = this.txtServerIp.Text.Trim();
                string str2 = this.numLogSaveDate.Value.ToString();
                string str3 = this.tbMapUrl.Text.Trim();
                string str4 = this.numPicTimeDiff.Value.ToString();
                string str5 = this.numCuffDiff.Value.ToString();
                string str6 = this.numCuffTime.Value.ToString();
                string str7 = this.dtpCuffBgnTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string str8 = this.numBroadCastTimeDiff.Value.ToString();
                string str9 = this.numBroadCastMsgTime.Value.ToString();
                string str10 = this.numGetLBSPosDataInterval.Value.ToString();
                string str11 = this.numLBSPosInterval.Value.ToString();
                string str12 = this.numLBSPosMaxNum.Value.ToString();
                string str13 = this.numLBSPosSleepTime.Value.ToString();
                string str14 = this.numLBSTpye.Value.ToString();
                string shortTimeString = this.datepickLBSStart.Value.ToShortTimeString();
                string shortTimeString1 = this.datepickLBSEnd.Value.ToShortTimeString();
                string str15 = this.numGetCurrentPosInterval.Value.ToString();
                if (str1.Trim().Length == 0)
                {
                    MessageBox.Show("服务器IP不能为空");
                    flag = false;
                }
                else if (SetParam.CheckIpAddress(str1))
                {
                    string str16 = "";
                    string dbParameter = this.getDbParameter(out str16);
                    if (dbParameter.Trim().Length == 0)
                    {
                        MessageBox.Show(string.Concat("数据库配置错误：", str16));
                        flag = false;
                    }
                    else if (this.execTestConnect(dbParameter).Length > 0)
                    {
                        MessageBox.Show("该配置无法正常连接数据库，不保存该配置！", "数据库参数配置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtDataSource.Focus();
                        flag = false;
                    }
                    else if (this.chkPort(str2))
                    {
                        if (this.chkLBSPos.Checked)
                        {
                            if (string.IsNullOrEmpty(this.numGetLBSPosDataInterval.Text))
                            {
                                MessageBox.Show("请输入读取数据间隔！");
                                flag = false;
                                return flag;
                            }
                            else if (string.IsNullOrEmpty(this.numLBSPosInterval.Text))
                            {
                                MessageBox.Show("请输入批量定位间隔！");
                                flag = false;
                                return flag;
                            }
                            else if (string.IsNullOrEmpty(this.numLBSPosMaxNum.Text))
                            {
                                MessageBox.Show("请输入批量定位上限！");
                                flag = false;
                                return flag;
                            }
                            else if (string.IsNullOrEmpty(this.numLBSPosSleepTime.Text))
                            {
                                MessageBox.Show("请输入批量定位休眠时间！");
                                flag = false;
                                return flag;
                            }
                            else if (string.IsNullOrEmpty(this.numLBSTpye.Text))
                            {
                                MessageBox.Show("请输入要定位的LBS终端的类型！");
                                flag = false;
                                return flag;
                            }
                            else if (DateTime.Parse(shortTimeString) > DateTime.Parse(shortTimeString1))
                            {
                                MessageBox.Show("开始时间不能大于结束时间！");
                                flag = false;
                                return flag;
                            }
                        }
                        if (this.cbIsBillPos.Checked)
                        {
                            if (string.IsNullOrEmpty(this.sltdays.Text))
                            {
                                MessageBox.Show("请输入读取监控订单的天数！");
                                flag = false;
                                return flag;
                            }
                            else if (string.IsNullOrEmpty(this.sltBillTime.Text))
                            {
                                MessageBox.Show("请输入订单读取间隔！");
                                flag = false;
                                return flag;
                            }
                            else if (string.IsNullOrEmpty(this.sltBillStatus.Text))
                            {
                                MessageBox.Show("请输入订单状态检测时间！");
                                flag = false;
                                return flag;
                            }
                        }
                        if (this.chkCuff.Checked)
                        {
                            if (!string.IsNullOrEmpty(this.numCuffDiff.Text))
                            {
                                bool @checked = this.chkIsSendMsg.Checked;
                            }
                            else
                            {
                                MessageBox.Show("请输入掉线报警的时间间隔!");
                                flag = false;
                                return flag;
                            }
                        }
                        if (!this.chkPic.Checked || !string.IsNullOrEmpty(this.numPicTimeDiff.Text))
                        {
                            if (this.chkBroadCast.Checked)
                            {
                                if (string.IsNullOrEmpty(this.numBroadCastTimeDiff.Text))
                                {
                                    MessageBox.Show("读取播报信息时间间隔!");
                                    flag = false;
                                    return flag;
                                }
                                else if (string.IsNullOrEmpty(this.numBroadCastMsgTime.Text))
                                {
                                    MessageBox.Show("同车播报信息时间间隔!");
                                    flag = false;
                                    return flag;
                                }
                                else if (string.IsNullOrEmpty(this.numCuffTime.Text))
                                {
                                    MessageBox.Show("请输入播报信息的掉线时间!");
                                    flag = false;
                                    return flag;
                                }
                            }
                            if (!this.chkAdminRegionAlarm.Checked || !string.IsNullOrEmpty(this.numGetCurrentPosInterval.Text))
                            {
                                if (this.cbIsBillPos.Checked || this.chkAdminRegionAlarm.Checked)
                                {
                                    if (this.tbMapUrl.Text.Trim().Length != 0)
                                    {
                                        string text = this.tbMapUrl.Text;
                                        if (text.IndexOf(':') >= 0)
                                        {
                                            string[] strArrays = text.Split(new char[] { ':' });
                                            if (!SetParam.CheckIpAddress(strArrays[0]))
                                            {
                                                MessageBox.Show("地图ip地址不正确！");
                                                flag = false;
                                                return flag;
                                            }
                                            else if (!this.chkPort(strArrays[1]))
                                            {
                                                MessageBox.Show("地图端口格式不正确！");
                                                flag = false;
                                                return flag;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("地图地址不正确！");
                                            flag = false;
                                            return flag;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("地图服务器IP不能为空");
                                        this.tbMapUrl.Focus();
                                        flag = false;
                                        return flag;
                                    }
                                }
                                this.setConfig("ConnectionString", dbParameter);
                                this.setConfig("serverIP", str1);
                                this.setConfig("MapUrl", str3);
                                this.setConfig("logPath", str);
                                this.setConfig("LogSaveDate", str2);
                                this.setConfig("IsPic", this.chkPic.Checked.ToString());
                                this.setConfig("PicTimeDiff", str4);
                                this.setConfig("IsCuff", this.chkCuff.Checked.ToString());
                                this.setConfig("CuffDiff", str5);
                                this.setConfig("CuffTime", str6);
                                this.setConfig("CuffBeginTime", str7);
                                this.setConfig("IsSendMsg", this.chkIsSendMsg.Checked.ToString());
                                this.setConfig("IsBroadCast", this.chkBroadCast.Checked.ToString());
                                this.setConfig("BroadCastDiff", str8);
                                this.setConfig("BroadCastMsgTime", str9);
                                this.setConfig("IsLBSPos", this.chkLBSPos.Checked.ToString());
                                this.setConfig("GetLBSPosDataInterval", str10);
                                this.setConfig("LBSPosInterval", str11);
                                this.setConfig("LBSPosMaxNum", str12);
                                this.setConfig("LBSPosSleepTime", str13);
                                this.setConfig("LBSType", str14);
                                this.setConfig("LBSStartTime", shortTimeString);
                                this.setConfig("LBSEndTime", shortTimeString1);
                                this.setConfig("IsBillPos", this.cbIsBillPos.Checked.ToString());
                                this.setConfig("Days", this.sltdays.Value.ToString());
                                this.setConfig("BillTime", this.sltBillTime.Value.ToString());
                                this.setConfig("BillStatus", this.sltBillStatus.Value.ToString());
                                this.setConfig("IsAdminRegionAlarm", this.chkAdminRegionAlarm.Checked.ToString());
                                this.setConfig("GetCurrentPosInterval", str15);
                                this.setConfig("AppIp", this.txtAppIp.Text);
                                this.setConfig("AppPort", this.txtAppPort.Text);
                                this.setConfig("AppUser", this.txtUser.Text);
                                this.setConfig("AppPwd", this.txtPwd.Text);
                                this.setConfig("IsChkErr", this.chkError.Checked.ToString());
                                this.setConfig("ChkInterval", this.numChkInterval.Value.ToString());
                                this.setConfig("DelayTime", this.numDelay.Value.ToString());
                                this.setConfig("Linkman", this.txtLinkman.Text);
                                flag = true;
                            }
                            else
                            {
                                MessageBox.Show("请输入读取车辆最新位置报文的时间间隔！");
                                flag = false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("请输入定时拍照的时间间隔!");
                            flag = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("日志保存天数必须为1～65535的整数！");
                        this.numLogSaveDate.Focus();
                        flag = false;
                    }
                }
                else
                {
                    MessageBox.Show("服务器IP格式不正确！");
                    this.txtServerIp.Focus();
                    flag = false;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return false;
            }
            return flag;
        }

        private void setConfig(string appKey, string AppValue)
        {
            string str = string.Concat(Application.StartupPath, SetParam.sFileName);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(str);
            XmlNode xmlNodes = xmlDocument.SelectSingleNode("//param");
            XmlElement appValue = (XmlElement)xmlNodes.SelectSingleNode(string.Concat("//", appKey));
            if (appValue == null)
            {
                XmlElement xmlElement = xmlDocument.CreateElement(appKey);
                xmlElement.InnerText = AppValue;
                xmlNodes.AppendChild(xmlElement);
            }
            else
            {
                appValue.InnerText = AppValue;
            }
            xmlDocument.Save(str);
        }

        private void setConfigKey(string AppKey, string AppValue)
        {
            string str = string.Concat(Application.StartupPath, SetParam.sFileName);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(str);
            XmlNode xmlNodes = xmlDocument.SelectSingleNode("//param");
            XmlElement xmlElement = (XmlElement)xmlNodes.SelectSingleNode(string.Concat("//add[@key='", AppKey, "']"));
            if (xmlElement == null)
            {
                XmlElement xmlElement1 = xmlDocument.CreateElement("add");
                xmlElement1.SetAttribute("key", AppKey);
                xmlElement1.SetAttribute("value", AppValue);
                xmlNodes.AppendChild(xmlElement1);
            }
            else
            {
                xmlElement.SetAttribute("value", AppValue);
            }
            xmlDocument.Save(str);
        }

        private void SetParam_Load(object sender, EventArgs e)
        {
            string str = string.Concat(Application.StartupPath, SetParam.sFileName);
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(str);
                XmlNode xmlNodes = xmlDocument.SelectSingleNode("//param");
                this.txtServerIp.Text = this.getConfig(xmlNodes, "serverIP");
                this.tbMapUrl.Text = this.getConfig(xmlNodes, "MapUrl");
                bool flag = true;
                bool.TryParse(this.getConfig(xmlNodes, "IsPic"), out flag);
                this.chkPic.Checked = flag;
                this.numPicTimeDiff.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "PicTimeDiff"));
                bool flag1 = true;
                bool.TryParse(this.getConfig(xmlNodes, "IsCuff"), out flag1);
                this.chkCuff.Checked = flag1;
                this.numCuffDiff.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "CuffDiff"));
                this.numCuffTime.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "CuffTime"));
                this.dtpCuffBgnTime.Value = Convert.ToDateTime(this.getConfig(xmlNodes, "CuffBeginTime"));
                bool flag2 = true;
                bool.TryParse(this.getConfig(xmlNodes, "IsSendMsg"), out flag2);
                this.chkIsSendMsg.Checked = flag2;
                try
                {
                    byte[] value = (byte[])Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\StarNet\\GpsWebServices\\1.0", "AlarmSndMsgtext", "");
                    this.txtSendMsg.Text = Encoding.Default.GetString(value);
                }
                catch
                {
                    this.txtSendMsg.Text = "";
                }
                bool flag3 = true;
                bool.TryParse(this.getConfig(xmlNodes, "IsBroadCast"), out flag3);
                this.chkBroadCast.Checked = flag3;
                this.numBroadCastTimeDiff.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "BroadCastDiff"));
                this.numBroadCastMsgTime.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "BroadCastMsgTime"));
                bool flag4 = true;
                bool.TryParse(this.getConfig(xmlNodes, "IsLBSPos"), out flag4);
                this.chkLBSPos.Checked = flag4;
                this.numGetLBSPosDataInterval.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "GetLBSPosDataInterval"));
                this.numLBSPosInterval.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "LBSPosInterval"));
                this.numLBSPosMaxNum.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "LBSPosMaxNum"));
                decimal num = this.numLBSPosMaxNum.Value;
                this.LBSPosMaxNum = int.Parse(num.ToString());
                this.numLBSPosSleepTime.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "LBSPosSleepTime"));
                decimal value1 = this.numLBSPosSleepTime.Value;
                this.LBSPosSleepTime = int.Parse(value1.ToString());
                this.numLBSTpye.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "LBSType"));
                this.datepickLBSStart.Value = DateTime.Parse(this.getConfig(xmlNodes, "LBSStartTime").ToString());
                this.datepickLBSEnd.Value = DateTime.Parse(this.getConfig(xmlNodes, "LBSEndTime").ToString());
                bool flag5 = true;
                bool.TryParse(this.getConfig(xmlNodes, "IsBillPos"), out flag5);
                this.cbIsBillPos.Checked = flag5;
                this.sltdays.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "Days"));
                this.sltBillTime.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "BillTime"));
                this.sltBillStatus.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "BillStatus"));
                bool flag6 = true;
                bool.TryParse(this.getConfig(xmlNodes, "IsAdminRegionAlarm"), out flag6);
                this.chkAdminRegionAlarm.Checked = flag6;
                this.numGetCurrentPosInterval.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "GetCurrentPosInterval"));
                bool flag7 = true;
                bool.TryParse(this.getConfig(xmlNodes, "IsChkErr"), out flag7);
                this.chkError.Checked = flag7;
                this.txtAppIp.Text = this.getConfig(xmlNodes, "AppIp");
                this.txtAppPort.Text = this.getConfig(xmlNodes, "AppPort");
                this.txtUser.Text = this.getConfig(xmlNodes, "AppUser");
                this.txtPwd.Text = this.getConfig(xmlNodes, "AppPwd");
                this.numChkInterval.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "ChkInterval"));
                this.numDelay.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "DelayTime"));
                this.txtLinkman.Text = this.getConfig(xmlNodes, "Linkman");
                this.txtLogPath.Text = this.getConfig(xmlNodes, "logPath");
                this.numLogSaveDate.Value = Convert.ToDecimal(this.getConfig(xmlNodes, "LogSaveDate"));
                string config = this.getConfig(xmlNodes, "ConnectionString");
                if (!string.IsNullOrEmpty(config))
                {
                    string[] strArrays = config.Split(new char[] { ';' });
                    for (int i = 0; i < (int)strArrays.Length; i++)
                    {
                        string[] strArrays1 = strArrays[i].Trim().Split(new char[] { '=' });
                        string lower = strArrays1[0].ToLower();
                        string str1 = lower;
                        if (lower != null)
                        {
                            if (str1 == "server")
                            {
                                this.txtDataSource.Text = strArrays1[1];
                            }
                            else if (str1 == "database")
                            {
                                this.txtInitialCatalog.Text = strArrays1[1];
                            }
                            else if (str1 == "uid")
                            {
                                this.txtUserId.Text = strArrays1[1];
                            }
                            else if (str1 == "pwd")
                            {
                                this.txtPassword.Text = strArrays1[1];
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void StarService(ServiceController sc)
        {
            if (sc.Status == ServiceControllerStatus.Paused || sc.Status == ServiceControllerStatus.Stopped)
            {
                sc.Start();
            }
        }

        private void StopService(ServiceController sc)
        {
            if (sc.Status == ServiceControllerStatus.Paused || sc.Status == ServiceControllerStatus.Running)
            {
                sc.Stop();
            }
        }
	
    }
}
