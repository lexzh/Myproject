namespace AppServerParam
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml;

    partial class GpsAppParamSet
    {
        private IContainer icontainer_0 = null;

        protected override void Dispose(bool bool_0)
        {
            if (bool_0 && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(bool_0);
        }

       
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GpsAppParamSet));
            this.openFileDialog_0 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog_0 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnOk = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStopAndRun = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtStandbyGlsPort = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.txtStandbyGlsIp = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtGlsPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGlsIp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtLogPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtServerIp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblShowInfo = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.lblUserID = new System.Windows.Forms.Label();
            this.txtInitialCatalog = new System.Windows.Forms.TextBox();
            this.lblInitialCatalog = new System.Windows.Forms.Label();
            this.txtDataSource = new System.Windows.Forms.TextBox();
            this.lblDataSource = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPort2 = new System.Windows.Forms.TextBox();
            this.txtPort1 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtCustomInfo = new System.Windows.Forms.TextBox();
            this.lblCustomInfo = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtCsUrl = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.txtVer = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCorpName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.txtLogSaveDate = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFileVersion = new System.Windows.Forms.TextBox();
            this.lblFileVersion = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.pnlOK = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.textMapAddr = new System.Windows.Forms.TextBox();
            this.textMapName = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.pnlOK.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(363, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(87, 33);
            this.btnOk.TabIndex = 16;
            this.btnOk.Text = "保存";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ForeColor = System.Drawing.Color.Red;
            this.label19.Location = new System.Drawing.Point(37, 566);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(0, 12);
            this.label19.TabIndex = 17;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(9, 8);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(87, 28);
            this.btnRun.TabIndex = 19;
            this.btnRun.Text = "启动服务";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(102, 8);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(87, 28);
            this.btnStop.TabIndex = 20;
            this.btnStop.Text = "停止服务";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStopAndRun
            // 
            this.btnStopAndRun.Location = new System.Drawing.Point(206, 8);
            this.btnStopAndRun.Name = "btnStopAndRun";
            this.btnStopAndRun.Size = new System.Drawing.Size(87, 28);
            this.btnStopAndRun.TabIndex = 21;
            this.btnStopAndRun.Text = "重启服务";
            this.btnStopAndRun.UseVisualStyleBackColor = true;
            this.btnStopAndRun.Click += new System.EventHandler(this.btnStopAndRun_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(464, 441);
            this.tabControl1.TabIndex = 22;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtStandbyGlsPort);
            this.tabPage1.Controls.Add(this.label23);
            this.tabPage1.Controls.Add(this.txtStandbyGlsIp);
            this.tabPage1.Controls.Add(this.label24);
            this.tabPage1.Controls.Add(this.label15);
            this.tabPage1.Controls.Add(this.txtGlsPort);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.txtGlsIp);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.txtLogPath);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtServerIp);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(456, 415);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtStandbyGlsPort
            // 
            this.txtStandbyGlsPort.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtStandbyGlsPort.Location = new System.Drawing.Point(265, 302);
            this.txtStandbyGlsPort.Name = "txtStandbyGlsPort";
            this.txtStandbyGlsPort.Size = new System.Drawing.Size(47, 21);
            this.txtStandbyGlsPort.TabIndex = 41;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label23.Location = new System.Drawing.Point(225, 306);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(41, 12);
            this.label23.TabIndex = 43;
            this.label23.Text = "端口：";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStandbyGlsIp
            // 
            this.txtStandbyGlsIp.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtStandbyGlsIp.Location = new System.Drawing.Point(101, 302);
            this.txtStandbyGlsIp.Name = "txtStandbyGlsIp";
            this.txtStandbyGlsIp.Size = new System.Drawing.Size(108, 21);
            this.txtStandbyGlsIp.TabIndex = 40;
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label24.Location = new System.Drawing.Point(-5, 302);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(108, 21);
            this.label24.TabIndex = 42;
            this.label24.Text = "备份注册中心IP：";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(23, 374);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(281, 12);
            this.label15.TabIndex = 39;
            this.label15.Text = "(基本信息是必填内容，如果没有正确配置无法运行)";
            // 
            // txtGlsPort
            // 
            this.txtGlsPort.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGlsPort.Location = new System.Drawing.Point(265, 272);
            this.txtGlsPort.Name = "txtGlsPort";
            this.txtGlsPort.Size = new System.Drawing.Size(47, 21);
            this.txtGlsPort.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(225, 276);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 23;
            this.label4.Text = "端口：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtGlsIp
            // 
            this.txtGlsIp.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGlsIp.Location = new System.Drawing.Point(101, 272);
            this.txtGlsIp.Name = "txtGlsIp";
            this.txtGlsIp.Size = new System.Drawing.Size(108, 21);
            this.txtGlsIp.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(7, 272);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 21);
            this.label3.TabIndex = 22;
            this.label3.Text = "注册中心IP：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 8F);
            this.button1.Location = new System.Drawing.Point(352, 333);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 23);
            this.button1.TabIndex = 18;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtLogPath
            // 
            this.txtLogPath.Enabled = false;
            this.txtLogPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLogPath.Location = new System.Drawing.Point(101, 334);
            this.txtLogPath.Name = "txtLogPath";
            this.txtLogPath.Size = new System.Drawing.Size(248, 21);
            this.txtLogPath.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(7, 334);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 21);
            this.label1.TabIndex = 19;
            this.label1.Text = "日志路径：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtServerIp
            // 
            this.txtServerIp.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtServerIp.Location = new System.Drawing.Point(101, 244);
            this.txtServerIp.Name = "txtServerIp";
            this.txtServerIp.Size = new System.Drawing.Size(108, 21);
            this.txtServerIp.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(7, 244);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "通讯服务IP：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblShowInfo);
            this.groupBox1.Controls.Add(this.btnTest);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.lblPassword);
            this.groupBox1.Controls.Add(this.txtUserId);
            this.groupBox1.Controls.Add(this.lblUserID);
            this.groupBox1.Controls.Add(this.txtInitialCatalog);
            this.groupBox1.Controls.Add(this.lblInitialCatalog);
            this.groupBox1.Controls.Add(this.txtDataSource);
            this.groupBox1.Controls.Add(this.lblDataSource);
            this.groupBox1.Location = new System.Drawing.Point(15, 104);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(403, 130);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据库配置";
            // 
            // lblShowInfo
            // 
            this.lblShowInfo.AutoSize = true;
            this.lblShowInfo.ForeColor = System.Drawing.Color.Red;
            this.lblShowInfo.Location = new System.Drawing.Point(125, 95);
            this.lblShowInfo.Name = "lblShowInfo";
            this.lblShowInfo.Size = new System.Drawing.Size(125, 12);
            this.lblShowInfo.TabIndex = 20;
            this.lblShowInfo.Text = "正在连接中,请稍候...";
            this.lblShowInfo.Visible = false;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(256, 90);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(87, 23);
            this.btnTest.TabIndex = 19;
            this.btnTest.Text = "数据库测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPassword.Location = new System.Drawing.Point(256, 52);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(91, 21);
            this.txtPassword.TabIndex = 3;
            // 
            // lblPassword
            // 
            this.lblPassword.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPassword.Location = new System.Drawing.Point(212, 53);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(48, 21);
            this.lblPassword.TabIndex = 15;
            this.lblPassword.Text = "密码：";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtUserId
            // 
            this.txtUserId.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserId.Location = new System.Drawing.Point(256, 27);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(91, 21);
            this.txtUserId.TabIndex = 1;
            // 
            // lblUserID
            // 
            this.lblUserID.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblUserID.Location = new System.Drawing.Point(212, 27);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(49, 21);
            this.lblUserID.TabIndex = 16;
            this.lblUserID.Text = "账号：";
            this.lblUserID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtInitialCatalog
            // 
            this.txtInitialCatalog.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtInitialCatalog.Location = new System.Drawing.Point(95, 52);
            this.txtInitialCatalog.Name = "txtInitialCatalog";
            this.txtInitialCatalog.Size = new System.Drawing.Size(99, 21);
            this.txtInitialCatalog.TabIndex = 2;
            // 
            // lblInitialCatalog
            // 
            this.lblInitialCatalog.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInitialCatalog.Location = new System.Drawing.Point(18, 52);
            this.lblInitialCatalog.Name = "lblInitialCatalog";
            this.lblInitialCatalog.Size = new System.Drawing.Size(79, 21);
            this.lblInitialCatalog.TabIndex = 17;
            this.lblInitialCatalog.Text = "数据库名：";
            this.lblInitialCatalog.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDataSource
            // 
            this.txtDataSource.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDataSource.Location = new System.Drawing.Point(95, 25);
            this.txtDataSource.Name = "txtDataSource";
            this.txtDataSource.Size = new System.Drawing.Size(99, 21);
            this.txtDataSource.TabIndex = 0;
            this.txtDataSource.Text = "127.0.0.1";
            // 
            // lblDataSource
            // 
            this.lblDataSource.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDataSource.Location = new System.Drawing.Point(18, 25);
            this.lblDataSource.Name = "lblDataSource";
            this.lblDataSource.Size = new System.Drawing.Size(79, 21);
            this.lblDataSource.TabIndex = 18;
            this.lblDataSource.Text = "服务器IP：";
            this.lblDataSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label22);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.txtPort2);
            this.groupBox6.Controls.Add(this.txtPort1);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Location = new System.Drawing.Point(15, 16);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(403, 82);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "服务端IP和端口配置(端口1使用TCP协议，端口2使用HTTP协议)";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.ForeColor = System.Drawing.Color.Red;
            this.label22.Location = new System.Drawing.Point(208, 51);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(113, 12);
            this.label22.TabIndex = 18;
            this.label22.Text = "(两端口号不能一样)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(208, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(113, 12);
            this.label9.TabIndex = 18;
            this.label9.Text = "(必须填写其中一个)";
            // 
            // txtPort2
            // 
            this.txtPort2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPort2.Location = new System.Drawing.Point(96, 47);
            this.txtPort2.Name = "txtPort2";
            this.txtPort2.Size = new System.Drawing.Size(94, 21);
            this.txtPort2.TabIndex = 0;
            // 
            // txtPort1
            // 
            this.txtPort1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPort1.Location = new System.Drawing.Point(96, 18);
            this.txtPort1.Name = "txtPort1";
            this.txtPort1.Size = new System.Drawing.Size(94, 21);
            this.txtPort1.TabIndex = 1;
            // 
            // label17
            // 
            this.label17.AllowDrop = true;
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(48, 51);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(47, 12);
            this.label17.TabIndex = 16;
            this.label17.Text = "端口2：";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AllowDrop = true;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(48, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "端口1：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textMapName);
            this.tabPage2.Controls.Add(this.textMapAddr);
            this.tabPage2.Controls.Add(this.label18);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.txtCustomInfo);
            this.tabPage2.Controls.Add(this.lblCustomInfo);
            this.tabPage2.Controls.Add(this.label16);
            this.tabPage2.Controls.Add(this.txtCsUrl);
            this.tabPage2.Controls.Add(this.label20);
            this.tabPage2.Controls.Add(this.label21);
            this.tabPage2.Controls.Add(this.txtVer);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.label14);
            this.tabPage2.Controls.Add(this.txtTitle);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.txtCorpName);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.btnSelectFile);
            this.tabPage2.Controls.Add(this.txtLogSaveDate);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txtFileVersion);
            this.tabPage2.Controls.Add(this.lblFileVersion);
            this.tabPage2.Controls.Add(this.txtPath);
            this.tabPage2.Controls.Add(this.lblPath);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(456, 415);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "可选信息";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtCustomInfo
            // 
            this.txtCustomInfo.Location = new System.Drawing.Point(108, 324);
            this.txtCustomInfo.Multiline = true;
            this.txtCustomInfo.Name = "txtCustomInfo";
            this.txtCustomInfo.Size = new System.Drawing.Size(289, 55);
            this.txtCustomInfo.TabIndex = 40;
            // 
            // lblCustomInfo
            // 
            this.lblCustomInfo.AutoSize = true;
            this.lblCustomInfo.Location = new System.Drawing.Point(30, 325);
            this.lblCustomInfo.Name = "lblCustomInfo";
            this.lblCustomInfo.Size = new System.Drawing.Size(77, 12);
            this.lblCustomInfo.TabIndex = 39;
            this.lblCustomInfo.Text = "自定义信息：";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(41, 393);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(185, 12);
            this.label16.TabIndex = 38;
            this.label16.Text = "(公司名和软件名称为必须填写项)";
            // 
            // txtCsUrl
            // 
            this.txtCsUrl.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCsUrl.Location = new System.Drawing.Point(108, 229);
            this.txtCsUrl.Name = "txtCsUrl";
            this.txtCsUrl.Size = new System.Drawing.Size(289, 21);
            this.txtCsUrl.TabIndex = 34;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label20.Location = new System.Drawing.Point(6, 232);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(101, 12);
            this.label20.TabIndex = 35;
            this.label20.Text = "客户端升级地址：";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.Color.Red;
            this.label21.Location = new System.Drawing.Point(41, 205);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(329, 12);
            this.label21.TabIndex = 33;
            this.label21.Text = "(版本格式：V2.1.0，如果版本配置错误，会导致升级失败。)";
            // 
            // txtVer
            // 
            this.txtVer.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtVer.Location = new System.Drawing.Point(108, 172);
            this.txtVer.Name = "txtVer";
            this.txtVer.Size = new System.Drawing.Size(101, 21);
            this.txtVer.TabIndex = 31;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(6, 176);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(101, 12);
            this.label12.TabIndex = 32;
            this.label12.Text = "客户端软件版本：";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(266, 144);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(143, 12);
            this.label14.TabIndex = 30;
            this.label14.Text = "（*客户端显示产品名称）";
            // 
            // txtTitle
            // 
            this.txtTitle.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTitle.Location = new System.Drawing.Point(108, 142);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(151, 21);
            this.txtTitle.TabIndex = 28;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(-79, 148);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 29;
            this.label11.Text = "产品标题：";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(265, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(155, 12);
            this.label8.TabIndex = 27;
            this.label8.Text = "（*客户端显示的公司名称）";
            // 
            // txtCorpName
            // 
            this.txtCorpName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCorpName.Location = new System.Drawing.Point(108, 111);
            this.txtCorpName.Name = "txtCorpName";
            this.txtCorpName.Size = new System.Drawing.Size(151, 21);
            this.txtCorpName.TabIndex = 25;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(42, 146);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 26;
            this.label7.Text = "软件名称：";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(42, 113);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 26;
            this.label10.Text = "公司名称：";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Font = new System.Drawing.Font("宋体", 8F);
            this.btnSelectFile.Location = new System.Drawing.Point(359, 23);
            this.btnSelectFile.Margin = new System.Windows.Forms.Padding(0);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(32, 23);
            this.btnSelectFile.TabIndex = 24;
            this.btnSelectFile.Text = "...";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // txtLogSaveDate
            // 
            this.txtLogSaveDate.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLogSaveDate.Location = new System.Drawing.Point(108, 81);
            this.txtLogSaveDate.MaxLength = 2;
            this.txtLogSaveDate.Name = "txtLogSaveDate";
            this.txtLogSaveDate.Size = new System.Drawing.Size(36, 21);
            this.txtLogSaveDate.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(18, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 22;
            this.label6.Text = "日志保存天数：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFileVersion
            // 
            this.txtFileVersion.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFileVersion.Location = new System.Drawing.Point(108, 52);
            this.txtFileVersion.Name = "txtFileVersion";
            this.txtFileVersion.Size = new System.Drawing.Size(82, 21);
            this.txtFileVersion.TabIndex = 18;
            // 
            // lblFileVersion
            // 
            this.lblFileVersion.AutoSize = true;
            this.lblFileVersion.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFileVersion.Location = new System.Drawing.Point(18, 52);
            this.lblFileVersion.Name = "lblFileVersion";
            this.lblFileVersion.Size = new System.Drawing.Size(89, 12);
            this.lblFileVersion.TabIndex = 20;
            this.lblFileVersion.Text = "升级文件版本：";
            this.lblFileVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPath.Location = new System.Drawing.Point(108, 23);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(248, 21);
            this.txtPath.TabIndex = 17;
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPath.Location = new System.Drawing.Point(18, 23);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(89, 12);
            this.lblPath.TabIndex = 19;
            this.lblPath.Text = "车台升级文件：";
            this.lblPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlOK
            // 
            this.pnlOK.Controls.Add(this.btnOk);
            this.pnlOK.Controls.Add(this.btnRun);
            this.pnlOK.Controls.Add(this.btnStopAndRun);
            this.pnlOK.Controls.Add(this.btnStop);
            this.pnlOK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlOK.Location = new System.Drawing.Point(10, 450);
            this.pnlOK.Name = "pnlOK";
            this.pnlOK.Size = new System.Drawing.Size(449, 39);
            this.pnlOK.TabIndex = 23;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 267);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(101, 12);
            this.label13.TabIndex = 41;
            this.label13.Text = "地图服务器地址：";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(42, 296);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(65, 12);
            this.label18.TabIndex = 42;
            this.label18.Text = "地图名称：";
            // 
            // textMapAddr
            // 
            this.textMapAddr.Location = new System.Drawing.Point(108, 261);
            this.textMapAddr.Name = "textMapAddr";
            this.textMapAddr.Size = new System.Drawing.Size(289, 21);
            this.textMapAddr.TabIndex = 43;
            // 
            // textMapName
            // 
            this.textMapName.Location = new System.Drawing.Point(108, 292);
            this.textMapName.Name = "textMapName";
            this.textMapName.Size = new System.Drawing.Size(289, 21);
            this.textMapName.TabIndex = 44;
            // 
            // GpsAppParamSet
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(469, 499);
            this.Controls.Add(this.pnlOK);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label19);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GpsAppParamSet";
            this.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "业务服务参数配置";
            this.Load += new System.EventHandler(this.GpsAppParamSet_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.pnlOK.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       
        private Button btnOk;
        private Button btnRun;
        private Button btnSelectFile;
        private Button btnStop;
        private Button btnStopAndRun;
        private Button btnTest;
        private Button button1;
        private FolderBrowserDialog folderBrowserDialog_0;
        private GroupBox groupBox1;
        private GroupBox groupBox6;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label19;
        private Label label2;
        private Label label20;
        private Label label21;
        private Label label22;
        private Label label23;
        private Label label24;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label lblCustomInfo;
        private Label lblDataSource;
        private Label lblFileVersion;
        private Label lblInitialCatalog;
        private Label lblPassword;
        private Label lblPath;
        private Label lblShowInfo;
        private Label lblUserID;
        private OpenFileDialog openFileDialog_0;
        private Panel pnlOK;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox txtCorpName;
        private TextBox txtCsUrl;
        private TextBox txtCustomInfo;
        private TextBox txtDataSource;
        private TextBox txtFileVersion;
        private TextBox txtGlsIp;
        private TextBox txtGlsPort;
        private TextBox txtInitialCatalog;
        private TextBox txtLogPath;
        private TextBox txtLogSaveDate;
        private TextBox txtPassword;
        private TextBox txtPath;
        private TextBox txtPort1;
        private TextBox txtPort2;
        private TextBox txtServerIp;
        private TextBox txtStandbyGlsIp;
        private TextBox txtStandbyGlsPort;
        private TextBox txtTitle;
        private TextBox txtUserId;
        private TextBox textMapName;
        private TextBox textMapAddr;
        private Label label18;
        private Label label13;
        private TextBox txtVer;
    }
}