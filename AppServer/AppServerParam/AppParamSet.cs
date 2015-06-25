namespace AppServerParam
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml;

    public partial class GpsAppParamSet : Form
    {

        public GpsAppParamSet()
        {
            this.InitializeComponent();
        }

        public GpsAppParamSet(string[] string_0)
        {
            if (string_0.Length > 0)
            {
                base.Location = new Point(0, 0);
                SetParent((int) base.Handle, int.Parse(string_0[0]));
            }
            this.InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(this.method_1);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.method_0);
            worker.RunWorkerAsync();
            this.method_20(false);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(this.method_17);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.method_16);
            worker.RunWorkerAsync("启动服务");
            this.method_20(false);
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_0.ShowDialog() == DialogResult.OK)
            {
                this.txtPath.Text = this.openFileDialog_0.FileName;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(this.method_17);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.method_16);
            worker.RunWorkerAsync("停止服务");
            this.method_20(false);
        }

        private void btnStopAndRun_Click(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(this.method_17);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.method_16);
            worker.RunWorkerAsync("重启服务");
            this.method_20(false);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(this.method_12);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.method_11);
            worker.RunWorkerAsync();
            this.method_20(false);
            this.lblShowInfo.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog_0.ShowDialog() == DialogResult.OK)
            {
                this.txtLogPath.Text = this.folderBrowserDialog_0.SelectedPath;
            }
        }

        private void GpsAppParamSet_Load(object sender, EventArgs e)
        {
            string str = this.getXmlPath();
            XMLFile file = new XMLFile(str);
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(str);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }
            XmlNode node = document.SelectSingleNode("//param");
            this.txtServerIp.Text = file.GetConfig(node, "communicationUrl");
            this.txtPath.Text = file.GetConfig(node, "UpdateFilePath");
            this.txtFileVersion.Text = file.GetConfig(node, "UpdateFileVersion");
            this.txtLogPath.Text = file.GetConfig(node, "logPath");
            this.txtLogSaveDate.Text = file.GetConfig(node, "LogSaveDate");
            this.txtGlsIp.Text = file.GetConfig(node, "GlsIp");
            this.txtGlsPort.Text = file.GetConfig(node, "GlsPort");
            this.txtPort1.Text = file.GetConfig(node, "Port1");
            this.txtPort2.Text = file.GetConfig(node, "Port2");
            this.txtCorpName.Text = file.GetConfig(node, "CorpName");
            this.txtTitle.Text = file.GetConfig(node, "Title");
            //this.txtWebserviceAddress.Text = file.GetConfig(node, "ADCUrl");
            this.txtVer.Text = file.GetConfig(node, "Version");
            this.txtCsUrl.Text = file.GetConfig(node, "CsFileUpdateUrl");
            this.txtCustomInfo.Text = file.GetConfig(node, "CustomInfo");
            this.txtStandbyGlsIp.Text = file.GetConfig(node, "StandbyGlsIp");
            this.txtStandbyGlsPort.Text = file.GetConfig(node, "StandbyGlsPort");
            this.textMapAddr.Text = file.GetConfig(node, "MapAddress");
            this.textMapName.Text = file.GetConfig(node, "MapName");
            string config = file.GetConfig(node, "ConnectionString");
            if (!string.IsNullOrEmpty(config))
            {
                foreach (string str3 in config.Split(new char[] { ';' }))
                {
                    string[] strArray2 = str3.Trim().Split(new char[] { '=' });
                    string str4 = strArray2[0].ToLower();
                    if (str4 != null)
                    {
                        if (str4 == "server")
                        {
                            this.txtDataSource.Text = strArray2[1];
                        }
                        else if (str4 == "database")
                        {
                            this.txtInitialCatalog.Text = strArray2[1];
                        }
                        else if (!(str4 == "uid"))
                        {
                            if (str4 == "pwd")
                            {
                                this.txtPassword.Text = strArray2[1];
                            }
                        }
                        else
                        {
                            this.txtUserId.Text = strArray2[1];
                        }
                    }
                }
            }
        }

 private void method_0(object sender, RunWorkerCompletedEventArgs e)
        {
            string str = e.Result.ToString();
            if (string.IsNullOrEmpty(str))
            {
                MessageBox.Show("参数配置成功！请重新启动服务。", "参数配置", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            else
            {
                MessageBox.Show(str);
            }
            this.method_20(true);
        }

        private void method_1(object sender, DoWorkEventArgs e)
        {
            string str = "";
            if (!this.method_4(out str))
            {
                e.Result = str;
            }
            else if (!this.method_14(out str))
            {
                e.Result = str;
            }
            else if (!this.method_3(out str))
            {
                e.Result = str;
            }
            else if (!this.method_6(out str))
            {
                e.Result = str;
            }
            else if (string.IsNullOrEmpty(this.txtLogPath.Text.Trim()))
            {
                e.Result = "日志路径为空！";
            }
            else if (!this.method_5(out str))
            {
                e.Result = str;
            }
            else if (string.IsNullOrEmpty(this.txtCorpName.Text.Trim()))
            {
                e.Result = "公司名称不能为空！";
            }
            else if (string.IsNullOrEmpty(this.txtTitle.Text.Trim()))
            {
                e.Result = "软件名称不能为空！";
            }
            else
            {
                try
                {
                    this.method_2();
                }
                catch (Exception exception)
                {
                    e.Result = exception.Message;
                    return;
                }
                e.Result = "参数配置成功！请重新启动服务。";
            }
        }

        private string getXmlPath()
        {
            return (Application.StartupPath + @"\param.xml");
        }

        private void method_11(object sender, RunWorkerCompletedEventArgs e)
        {
            string str = e.Result.ToString();
            if (string.IsNullOrEmpty(str))
            {
                MessageBox.Show("连接成功！", "数据库连接测试", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            else
            {
                MessageBox.Show(str, "数据库连接测试", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            this.method_20(true);
            this.lblShowInfo.Visible = false;
        }

        private void method_12(object sender, DoWorkEventArgs e)
        {
            e.Result = this.method_13();
        }

        private string method_13()
        {
            try
            {
                ICheck check = new DataBase();
                return (check.Check(this.method_15()) ? "" : "与数据库连接失败");
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        private bool method_14(out string string_0)
        {
            string_0 = "";
            try
            {
                ICheck check = new DataBase();
                return check.Check(this.method_15());
            }
            catch (Exception exception)
            {
                string_0 = exception.Message;
                return false;
            }
        }

        private DataBaseParams method_15()
        {
            return new DataBaseParams { DataBaseIp = this.txtDataSource.Text.Trim(), DataBaseName = this.txtInitialCatalog.Text.Trim(), DataBaseUser = this.txtUserId.Text.Trim(), DataBasePassword = this.txtPassword.Text.Trim() };
        }

        private void method_16(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show(e.Result.ToString());
            this.method_20(true);
        }

        private void method_17(object sender, DoWorkEventArgs e)
        {
            ServerManager manager = this.method_19();
            string str = "";
            try
            {
                if (!this.method_18(manager, out str))
                {
                    e.Result = str;
                }
                else if (e.Argument.ToString().Equals("启动服务"))
                {
                    manager.StarService();
                    e.Result = "启动服务成功!";
                }
                else if (e.Argument.ToString().Equals("停止服务"))
                {
                    manager.StopService();
                    e.Result = "停止服务成功!";
                }
                else if (e.Argument.ToString().Equals("重启服务"))
                {
                    manager.ReStarService();
                    e.Result = "重启服务成功！";
                }
            }
            catch (Exception exception)
            {
                e.Result = e.Argument.ToString() + "发生错误：" + exception.Message;
            }
        }

        private bool method_18(ServerManager serverManager_0, out string string_0)
        {
            string_0 = "";
            if (!serverManager_0.IsExistService)
            {
                string_0 = "服务不存在,请检查!";
                return false;
            }
            return true;
        }

        private ServerManager method_19()
        {
            return new ServerManager("GpsAppServer");
        }

        private void method_2()
        {
            XMLFile file = new XMLFile(this.getXmlPath());
            file.SetConfig("ConnectionString", this.method_15().DataBaseStringParam);
            file.SetConfig("UpdateFilePath", this.txtPath.Text.Trim());
            file.SetConfig("UpdateFileVersion", this.txtFileVersion.Text.Trim());
            file.SetConfig("communicationUrl", this.txtServerIp.Text.Trim());
            file.SetConfig("logPath", this.txtLogPath.Text.Trim());
            file.SetConfig("LogSaveDate", this.txtLogSaveDate.Text.Trim());
            file.SetConfig("GlsIp", this.txtGlsIp.Text.Trim());
            file.SetConfig("GlsPort", this.txtGlsPort.Text.Trim());
            file.SetConfig("StandbyGlsIp", this.txtStandbyGlsIp.Text.Trim());
            file.SetConfig("StandbyGlsPort", this.txtStandbyGlsPort.Text.Trim());
            file.SetConfig("Port1", this.txtPort1.Text.Trim());
            file.SetConfig("Port2", this.txtPort2.Text.Trim());
            file.SetConfig("CorpName", this.txtCorpName.Text.Trim());
            file.SetConfig("Title", this.txtTitle.Text.Trim());
            file.SetConfig("Version", this.txtVer.Text.Trim());
            file.SetConfig("CsFileUpdateUrl", this.txtCsUrl.Text.Trim());
            //file.SetConfig("ADCUrl", this.txtWebserviceAddress.Text.Trim());
            file.SetConfig("CustomInfo", this.txtCustomInfo.Text.Trim());
            file.SetConfig("MapAddress", this.textMapAddr.Text.Trim());
            string sMapName = this.textMapName.Text.Trim();
            if (!string.IsNullOrEmpty(sMapName))
            {
                sMapName = "maps";
            }
            file.SetConfig("MapName", sMapName);
        }

        private void method_20(bool bool_0)
        {
            this.tabControl1.Enabled = bool_0;
            base.ControlBox = bool_0;
            this.pnlOK.Enabled = bool_0;
        }

        private bool method_3(out string string_0)
        {
            string_0 = "";
            return true;
        }

        private bool method_4(out string string_0)
        {
            bool flag;
            if (string.IsNullOrEmpty(this.txtPort1.Text.Trim()) && string.IsNullOrEmpty(this.txtPort2.Text.Trim()))
            {
                string_0 = "服务端端口不能全为空！";
                this.txtPort1.Focus();
                return false;
            }
            if (this.txtPort1.Text.Trim().Equals(this.txtPort2.Text.Trim()))
            {
                string_0 = "服务端两端口不能一样！";
                this.txtPort1.Focus();
                return false;
            }
            Port port = new Port();
            try
            {
                if (!string.IsNullOrEmpty(this.txtPort1.Text.Trim()))
                {
                    port.Check(this.txtPort1.Text.Trim());
                }
            }
            catch (Exception exception)
            {
                string_0 = "服务端端口," + exception.Message;
                return false;
            }
            try
            {
                if (!string.IsNullOrEmpty(this.txtPort2.Text.Trim()))
                {
                    port.Check(this.txtPort2.Text.Trim());
                }
                string_0 = "";
                return true;
            }
            catch (Exception exception2)
            {
                string_0 = "服务端端口," + exception2.Message;
                flag = false;
            }
            return flag;
        }

        private bool method_5(out string string_0)
        {
            bool flag = false;
            string_0 = "";
            string str = this.txtLogSaveDate.Text.Trim();
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    str = "5";
                }
                flag = true;
                ICheck check = new LogSaveDay();
                check.Check(str);
            }
            catch (Exception exception)
            {
                string_0 = "日志保存天数," + exception.Message;
                flag = false;
            }
            return flag;
        }

        private bool method_6(out string string_0)
        {
            bool flag;
            string_0 = "";
            if (string.IsNullOrEmpty(this.txtGlsIp.Text.Trim()))
            {
                string_0 = "注册服务IP不能为空！";
                return false;
            }
            if (string.IsNullOrEmpty(this.txtGlsPort.Text.Trim()))
            {
                string_0 = "注册服务端口不能为空！";
                return false;
            }
            new IP();
            Port port = new Port();
            try
            {
                port.Check(this.txtGlsPort.Text.Trim());
                if (!string.IsNullOrEmpty(this.txtStandbyGlsPort.Text.Trim()))
                {
                    port.Check(this.txtStandbyGlsPort.Text.Trim());
                }
                return true;
            }
            catch (Exception exception)
            {
                string_0 = "注册服务端口" + exception.Message;
                flag = false;
            }
            return flag;
        }

        private void method_7(string string_0, string string_1)
        {
            MessageBox.Show(string_0, string_1, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void method_8()
        {
            this.method_7("连接成功！", "数据库连接测试");
        }

        private void method_9()
        {
            this.method_7("数据库失败！", "数据库连接测试");
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(int int_0, int int_1);
    }
}

