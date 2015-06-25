﻿namespace Client
{
    using Remoting;
    using ParamLibrary.Application;
    using ParamLibrary.CarEntity;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WinFormsUI.Controls;

    partial class CarAlerm
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

       
        private void InitializeComponent()
        {
            this.tabControl1 = new TabControl();
            this.tpAlerm = new TabPage();
            this.lblFirstLinkmanValue = new Label();
            this.lblCompanyValue = new Label();
            this.lblOwnerSimNumValue = new Label();
            this.lblOwnerNameValue = new Label();
            this.lblSimNumValue = new Label();
            this.lblCarIdValue = new Label();
            this.lblCarNumValue = new Label();
            this.lblAlermTimeValue = new Label();
            this.lblAlermTypeValue = new Label();
            this.lblFirstLinkmanTelValue = new Label();
            this.lblAreaValue = new Label();
            this.lblFirstLinkman = new Label();
            this.lblCompany = new Label();
            this.lblOwnerSimNum = new Label();
            this.lblOwnerName = new Label();
            this.lblSimNum = new Label();
            this.lblCarId = new Label();
            this.lblCarNum = new Label();
            this.lblAlermTime = new Label();
            this.lblAlermType = new Label();
            this.lblFirstLinkmanTel = new Label();
            this.lblArea = new Label();
            this.tpCarInfo = new TabPage();
            this.lblOwnerSexValue = new Label();
            this.lblOtherLinkValue = new Label();
            this.lblAddressValue = new Label();
            this.lblIdentityCardValue = new Label();
            this.lblSecondLinkmanTelValue = new Label();
            this.lblSecondLinkmanValue = new Label();
            this.lblColorValue = new Label();
            this.lblCarBrandValue = new Label();
            this.lblLongitudeValue = new Label();
            this.lblPostCodeValue = new Label();
            this.lblLatitudeValue = new Label();
            this.lblOwnerSex = new Label();
            this.lblOtherLink = new Label();
            this.lblAddress = new Label();
            this.lblIdentityCard = new Label();
            this.lblSecondLinkmanTel = new Label();
            this.lblSecondLinkman = new Label();
            this.lblColor = new Label();
            this.lblCarBrand = new Label();
            this.lblLongitude = new Label();
            this.lblPostCode = new Label();
            this.lblLatitude = new Label();
            this.pnlBtn = new Panel();
            this.btnRealTimeReport = new Button();
            this.btnStopReport = new Button();
            this.btnClose = new Button();
            this.pnlAlerm = new Panel();
            this.tabControl1.SuspendLayout();
            this.tpAlerm.SuspendLayout();
            this.tpCarInfo.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            this.pnlAlerm.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tpAlerm);
            this.tabControl1.Controls.Add(this.tpCarInfo);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(418, 364);
            this.tabControl1.TabIndex = 0;
            this.tpAlerm.Controls.Add(this.lblFirstLinkmanValue);
            this.tpAlerm.Controls.Add(this.lblCompanyValue);
            this.tpAlerm.Controls.Add(this.lblOwnerSimNumValue);
            this.tpAlerm.Controls.Add(this.lblOwnerNameValue);
            this.tpAlerm.Controls.Add(this.lblSimNumValue);
            this.tpAlerm.Controls.Add(this.lblCarIdValue);
            this.tpAlerm.Controls.Add(this.lblCarNumValue);
            this.tpAlerm.Controls.Add(this.lblAlermTimeValue);
            this.tpAlerm.Controls.Add(this.lblAlermTypeValue);
            this.tpAlerm.Controls.Add(this.lblFirstLinkmanTelValue);
            this.tpAlerm.Controls.Add(this.lblAreaValue);
            this.tpAlerm.Controls.Add(this.lblFirstLinkman);
            this.tpAlerm.Controls.Add(this.lblCompany);
            this.tpAlerm.Controls.Add(this.lblOwnerSimNum);
            this.tpAlerm.Controls.Add(this.lblOwnerName);
            this.tpAlerm.Controls.Add(this.lblSimNum);
            this.tpAlerm.Controls.Add(this.lblCarId);
            this.tpAlerm.Controls.Add(this.lblCarNum);
            this.tpAlerm.Controls.Add(this.lblAlermTime);
            this.tpAlerm.Controls.Add(this.lblAlermType);
            this.tpAlerm.Controls.Add(this.lblFirstLinkmanTel);
            this.tpAlerm.Controls.Add(this.lblArea);
            this.tpAlerm.Location = new Point(4, 21);
            this.tpAlerm.Name = "tpAlerm";
            this.tpAlerm.Padding = new Padding(3);
            this.tpAlerm.Size = new Size(410, 339);
            this.tpAlerm.TabIndex = 0;
            this.tpAlerm.Text = "报警信息";
            this.tpAlerm.UseVisualStyleBackColor = true;
            this.lblFirstLinkmanValue.AccessibleRole =  System.Windows.Forms.AccessibleRole.None;
            this.lblFirstLinkmanValue.AutoSize = true;
            this.lblFirstLinkmanValue.Location = new Point(119, 283);
            this.lblFirstLinkmanValue.Name = "lblFirstLinkmanValue";
            this.lblFirstLinkmanValue.Size = new Size(11, 12);
            this.lblFirstLinkmanValue.TabIndex = 21;
            this.lblFirstLinkmanValue.Text = " ";
            this.lblCompanyValue.AutoSize = true;
            this.lblCompanyValue.Location = new Point(119, 254);
            this.lblCompanyValue.Name = "lblCompanyValue";
            this.lblCompanyValue.Size = new Size(11, 12);
            this.lblCompanyValue.TabIndex = 20;
            this.lblCompanyValue.Text = " ";
            this.lblOwnerSimNumValue.AutoSize = true;
            this.lblOwnerSimNumValue.Location = new Point(119, 225);
            this.lblOwnerSimNumValue.Name = "lblOwnerSimNumValue";
            this.lblOwnerSimNumValue.Size = new Size(11, 12);
            this.lblOwnerSimNumValue.TabIndex = 19;
            this.lblOwnerSimNumValue.Text = " ";
            this.lblOwnerNameValue.AutoSize = true;
            this.lblOwnerNameValue.Location = new Point(119, 196);
            this.lblOwnerNameValue.Name = "lblOwnerNameValue";
            this.lblOwnerNameValue.Size = new Size(11, 12);
            this.lblOwnerNameValue.TabIndex = 18;
            this.lblOwnerNameValue.Text = " ";
            this.lblSimNumValue.AutoSize = true;
            this.lblSimNumValue.Location = new Point(119, 167);
            this.lblSimNumValue.Name = "lblSimNumValue";
            this.lblSimNumValue.Size = new Size(11, 12);
            this.lblSimNumValue.TabIndex = 17;
            this.lblSimNumValue.Text = " ";
            this.lblCarIdValue.AccessibleRole =  System.Windows.Forms.AccessibleRole.None;
            this.lblCarIdValue.AutoSize = true;
            this.lblCarIdValue.Location = new Point(119, 138);
            this.lblCarIdValue.Name = "lblCarIdValue";
            this.lblCarIdValue.Size = new Size(11, 12);
            this.lblCarIdValue.TabIndex = 16;
            this.lblCarIdValue.Text = " ";
            this.lblCarNumValue.AutoSize = true;
            this.lblCarNumValue.Location = new Point(119, 109);
            this.lblCarNumValue.Name = "lblCarNumValue";
            this.lblCarNumValue.Size = new Size(11, 12);
            this.lblCarNumValue.TabIndex = 15;
            this.lblCarNumValue.Text = " ";
            this.lblAlermTimeValue.AutoSize = true;
            this.lblAlermTimeValue.Location = new Point(119, 80);
            this.lblAlermTimeValue.Name = "lblAlermTimeValue";
            this.lblAlermTimeValue.Size = new Size(11, 12);
            this.lblAlermTimeValue.TabIndex = 14;
            this.lblAlermTimeValue.Text = " ";
            this.lblAlermTypeValue.AutoSize = true;
            this.lblAlermTypeValue.Location = new Point(119, 22);
            this.lblAlermTypeValue.Name = "lblAlermTypeValue";
            this.lblAlermTypeValue.Size = new Size(11, 12);
            this.lblAlermTypeValue.TabIndex = 13;
            this.lblAlermTypeValue.Text = " ";
            this.lblFirstLinkmanTelValue.AutoSize = true;
            this.lblFirstLinkmanTelValue.Location = new Point(119, 312);
            this.lblFirstLinkmanTelValue.Name = "lblFirstLinkmanTelValue";
            this.lblFirstLinkmanTelValue.Size = new Size(11, 12);
            this.lblFirstLinkmanTelValue.TabIndex = 12;
            this.lblFirstLinkmanTelValue.Text = " ";
            this.lblAreaValue.AutoSize = true;
            this.lblAreaValue.Location = new Point(119, 51);
            this.lblAreaValue.Name = "lblAreaValue";
            this.lblAreaValue.Size = new Size(11, 12);
            this.lblAreaValue.TabIndex = 11;
            this.lblAreaValue.Text = " ";
            this.lblFirstLinkman.AutoSize = true;
            this.lblFirstLinkman.Location = new Point(44, 283);
            this.lblFirstLinkman.Name = "lblFirstLinkman";
            this.lblFirstLinkman.Size = new Size(77, 12);
            this.lblFirstLinkman.TabIndex = 10;
            this.lblFirstLinkman.Text = "第一联系人：";
            this.lblCompany.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lblCompany.AutoSize = true;
            this.lblCompany.Location = new Point(56, 254);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new Size(65, 12);
            this.lblCompany.TabIndex = 9;
            this.lblCompany.Text = "所属单位：";
            this.lblOwnerSimNum.AutoSize = true;
            this.lblOwnerSimNum.Location = new Point(44, 225);
            this.lblOwnerSimNum.Name = "lblOwnerSimNum";
            this.lblOwnerSimNum.Size = new Size(77, 12);
            this.lblOwnerSimNum.TabIndex = 8;
            this.lblOwnerSimNum.Text = "车主手机号：";
            this.lblOwnerName.AutoSize = true;
            this.lblOwnerName.Location = new Point(56, 196);
            this.lblOwnerName.Name = "lblOwnerName";
            this.lblOwnerName.Size = new Size(65, 12);
            this.lblOwnerName.TabIndex = 7;
            this.lblOwnerName.Text = "车主姓名：";
            this.lblSimNum.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lblSimNum.AutoSize = true;
            this.lblSimNum.Location = new Point(32, 167);
            this.lblSimNum.Name = "lblSimNum";
            this.lblSimNum.Size = new Size(89, 12);
            this.lblSimNum.TabIndex = 6;
            this.lblSimNum.Text = "车台手机号码：";
            this.lblCarId.AutoSize = true;
            this.lblCarId.Location = new Point(56, 138);
            this.lblCarId.Name = "lblCarId";
            this.lblCarId.Size = new Size(65, 12);
            this.lblCarId.TabIndex = 5;
            this.lblCarId.Text = "车辆编号：";
            this.lblCarNum.AutoSize = true;
            this.lblCarNum.Location = new Point(56, 109);
            this.lblCarNum.Name = "lblCarNum";
            this.lblCarNum.Size = new Size(65, 12);
            this.lblCarNum.TabIndex = 4;
            this.lblCarNum.Text = "车牌号码：";
            this.lblAlermTime.AllowDrop = true;
            this.lblAlermTime.AutoSize = true;
            this.lblAlermTime.Location = new Point(56, 80);
            this.lblAlermTime.Name = "lblAlermTime";
            this.lblAlermTime.Size = new Size(65, 12);
            this.lblAlermTime.TabIndex = 3;
            this.lblAlermTime.Text = "报警时间：";
            this.lblAlermType.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lblAlermType.AutoSize = true;
            this.lblAlermType.Location = new Point(56, 22);
            this.lblAlermType.Name = "lblAlermType";
            this.lblAlermType.Size = new Size(65, 12);
            this.lblAlermType.TabIndex = 2;
            this.lblAlermType.Text = "报警类型：";
            this.lblFirstLinkmanTel.AllowDrop = true;
            this.lblFirstLinkmanTel.AutoSize = true;
            this.lblFirstLinkmanTel.Location = new Point(32, 312);
            this.lblFirstLinkmanTel.Name = "lblFirstLinkmanTel";
            this.lblFirstLinkmanTel.Size = new Size(89, 12);
            this.lblFirstLinkmanTel.TabIndex = 1;
            this.lblFirstLinkmanTel.Text = "第一联系电话：";
            this.lblArea.AutoSize = true;
            this.lblArea.Location = new Point(32, 51);
            this.lblArea.Name = "lblArea";
            this.lblArea.Size = new Size(89, 12);
            this.lblArea.TabIndex = 0;
            this.lblArea.Text = "车辆所属区域：";
            this.tpCarInfo.Controls.Add(this.lblOwnerSexValue);
            this.tpCarInfo.Controls.Add(this.lblOtherLinkValue);
            this.tpCarInfo.Controls.Add(this.lblAddressValue);
            this.tpCarInfo.Controls.Add(this.lblIdentityCardValue);
            this.tpCarInfo.Controls.Add(this.lblSecondLinkmanTelValue);
            this.tpCarInfo.Controls.Add(this.lblSecondLinkmanValue);
            this.tpCarInfo.Controls.Add(this.lblColorValue);
            this.tpCarInfo.Controls.Add(this.lblCarBrandValue);
            this.tpCarInfo.Controls.Add(this.lblLongitudeValue);
            this.tpCarInfo.Controls.Add(this.lblPostCodeValue);
            this.tpCarInfo.Controls.Add(this.lblLatitudeValue);
            this.tpCarInfo.Controls.Add(this.lblOwnerSex);
            this.tpCarInfo.Controls.Add(this.lblOtherLink);
            this.tpCarInfo.Controls.Add(this.lblAddress);
            this.tpCarInfo.Controls.Add(this.lblIdentityCard);
            this.tpCarInfo.Controls.Add(this.lblSecondLinkmanTel);
            this.tpCarInfo.Controls.Add(this.lblSecondLinkman);
            this.tpCarInfo.Controls.Add(this.lblColor);
            this.tpCarInfo.Controls.Add(this.lblCarBrand);
            this.tpCarInfo.Controls.Add(this.lblLongitude);
            this.tpCarInfo.Controls.Add(this.lblPostCode);
            this.tpCarInfo.Controls.Add(this.lblLatitude);
            this.tpCarInfo.Location = new Point(4, 21);
            this.tpCarInfo.Name = "tpCarInfo";
            this.tpCarInfo.Padding = new Padding(3);
            this.tpCarInfo.Size = new Size(410, 339);
            this.tpCarInfo.TabIndex = 1;
            this.tpCarInfo.Text = "车辆详细信息";
            this.tpCarInfo.UseVisualStyleBackColor = true;
            this.lblOwnerSexValue.AutoSize = true;
            this.lblOwnerSexValue.Location = new Point(121, 283);
            this.lblOwnerSexValue.Name = "lblOwnerSexValue";
            this.lblOwnerSexValue.Size = new Size(11, 12);
            this.lblOwnerSexValue.TabIndex = 32;
            this.lblOwnerSexValue.Text = " ";
            this.lblOtherLinkValue.AllowDrop = true;
            this.lblOtherLinkValue.AutoSize = true;
            this.lblOtherLinkValue.Location = new Point(121, 254);
            this.lblOtherLinkValue.Name = "lblOtherLinkValue";
            this.lblOtherLinkValue.Size = new Size(11, 12);
            this.lblOtherLinkValue.TabIndex = 31;
            this.lblOtherLinkValue.Text = " ";
            this.lblAddressValue.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lblAddressValue.AllowDrop = true;
            this.lblAddressValue.AutoSize = true;
            this.lblAddressValue.Location = new Point(121, 225);
            this.lblAddressValue.Name = "lblAddressValue";
            this.lblAddressValue.Size = new Size(11, 12);
            this.lblAddressValue.TabIndex = 30;
            this.lblAddressValue.Text = " ";
            this.lblIdentityCardValue.AutoSize = true;
            this.lblIdentityCardValue.Location = new Point(121, 196);
            this.lblIdentityCardValue.Name = "lblIdentityCardValue";
            this.lblIdentityCardValue.Size = new Size(11, 12);
            this.lblIdentityCardValue.TabIndex = 29;
            this.lblIdentityCardValue.Text = " ";
            this.lblSecondLinkmanTelValue.AutoSize = true;
            this.lblSecondLinkmanTelValue.Location = new Point(121, 167);
            this.lblSecondLinkmanTelValue.Name = "lblSecondLinkmanTelValue";
            this.lblSecondLinkmanTelValue.Size = new Size(11, 12);
            this.lblSecondLinkmanTelValue.TabIndex = 28;
            this.lblSecondLinkmanTelValue.Text = " ";
            this.lblSecondLinkmanValue.AllowDrop = true;
            this.lblSecondLinkmanValue.AutoSize = true;
            this.lblSecondLinkmanValue.Location = new Point(121, 138);
            this.lblSecondLinkmanValue.Name = "lblSecondLinkmanValue";
            this.lblSecondLinkmanValue.Size = new Size(11, 12);
            this.lblSecondLinkmanValue.TabIndex = 27;
            this.lblSecondLinkmanValue.Text = " ";
            this.lblColorValue.AutoSize = true;
            this.lblColorValue.Location = new Point(121, 109);
            this.lblColorValue.Name = "lblColorValue";
            this.lblColorValue.Size = new Size(11, 12);
            this.lblColorValue.TabIndex = 26;
            this.lblColorValue.Text = " ";
            this.lblCarBrandValue.AutoSize = true;
            this.lblCarBrandValue.Location = new Point(121, 80);
            this.lblCarBrandValue.Name = "lblCarBrandValue";
            this.lblCarBrandValue.Size = new Size(11, 12);
            this.lblCarBrandValue.TabIndex = 25;
            this.lblCarBrandValue.Text = " ";
            this.lblLongitudeValue.AutoSize = true;
            this.lblLongitudeValue.Location = new Point(121, 22);
            this.lblLongitudeValue.Name = "lblLongitudeValue";
            this.lblLongitudeValue.Size = new Size(11, 12);
            this.lblLongitudeValue.TabIndex = 24;
            this.lblLongitudeValue.Text = " ";
            this.lblPostCodeValue.AutoSize = true;
            this.lblPostCodeValue.Location = new Point(121, 312);
            this.lblPostCodeValue.Name = "lblPostCodeValue";
            this.lblPostCodeValue.Size = new Size(11, 12);
            this.lblPostCodeValue.TabIndex = 23;
            this.lblPostCodeValue.Text = " ";
            this.lblLatitudeValue.AutoSize = true;
            this.lblLatitudeValue.Location = new Point(121, 51);
            this.lblLatitudeValue.Name = "lblLatitudeValue";
            this.lblLatitudeValue.Size = new Size(11, 12);
            this.lblLatitudeValue.TabIndex = 22;
            this.lblLatitudeValue.Text = " ";
            this.lblOwnerSex.AutoSize = true;
            this.lblOwnerSex.Location = new Point(56, 283);
            this.lblOwnerSex.Name = "lblOwnerSex";
            this.lblOwnerSex.Size = new Size(65, 12);
            this.lblOwnerSex.TabIndex = 21;
            this.lblOwnerSex.Text = "车主性别：";
            this.lblOtherLink.AutoSize = true;
            this.lblOtherLink.Location = new Point(32, 254);
            this.lblOtherLink.Name = "lblOtherLink";
            this.lblOtherLink.Size = new Size(89, 12);
            this.lblOtherLink.TabIndex = 20;
            this.lblOtherLink.Text = "其他联系方式：";
            this.lblAddress.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new Point(56, 225);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new Size(65, 12);
            this.lblAddress.TabIndex = 19;
            this.lblAddress.Text = "家庭住址：";
            this.lblIdentityCard.AutoSize = true;
            this.lblIdentityCard.Location = new Point(20, 196);
            this.lblIdentityCard.Name = "lblIdentityCard";
            this.lblIdentityCard.Size = new Size(101, 12);
            this.lblIdentityCard.TabIndex = 18;
            this.lblIdentityCard.Text = "车主身份证号码：";
            this.lblSecondLinkmanTel.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lblSecondLinkmanTel.AutoSize = true;
            this.lblSecondLinkmanTel.Location = new Point(32, 167);
            this.lblSecondLinkmanTel.Name = "lblSecondLinkmanTel";
            this.lblSecondLinkmanTel.Size = new Size(89, 12);
            this.lblSecondLinkmanTel.TabIndex = 17;
            this.lblSecondLinkmanTel.Text = "第二联系电话：";
            this.lblSecondLinkman.AutoSize = true;
            this.lblSecondLinkman.Location = new Point(44, 138);
            this.lblSecondLinkman.Name = "lblSecondLinkman";
            this.lblSecondLinkman.Size = new Size(77, 12);
            this.lblSecondLinkman.TabIndex = 16;
            this.lblSecondLinkman.Text = "第二联系人：";
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new Point(56, 109);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new Size(65, 12);
            this.lblColor.TabIndex = 15;
            this.lblColor.Text = "车辆颜色：";
            this.lblCarBrand.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lblCarBrand.AutoSize = true;
            this.lblCarBrand.Location = new Point(56, 80);
            this.lblCarBrand.Name = "lblCarBrand";
            this.lblCarBrand.Size = new Size(65, 12);
            this.lblCarBrand.TabIndex = 14;
            this.lblCarBrand.Text = "车辆品牌：";
            this.lblLongitude.AutoSize = true;
            this.lblLongitude.Location = new Point(80, 22);
            this.lblLongitude.Name = "lblLongitude";
            this.lblLongitude.Size = new Size(41, 12);
            this.lblLongitude.TabIndex = 13;
            this.lblLongitude.Text = "经度：";
            this.lblPostCode.AutoSize = true;
            this.lblPostCode.Location = new Point(56, 312);
            this.lblPostCode.Name = "lblPostCode";
            this.lblPostCode.Size = new Size(65, 12);
            this.lblPostCode.TabIndex = 12;
            this.lblPostCode.Text = "邮政编码：";
            this.lblLatitude.AutoSize = true;
            this.lblLatitude.Location = new Point(80, 51);
            this.lblLatitude.Name = "lblLatitude";
            this.lblLatitude.Size = new Size(41, 12);
            this.lblLatitude.TabIndex = 11;
            this.lblLatitude.Text = "纬度：";
            this.pnlBtn.Controls.Add(this.btnRealTimeReport);
            this.pnlBtn.Controls.Add(this.btnStopReport);
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Dock = DockStyle.Bottom;
            this.pnlBtn.Location = new Point(5, 369);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new Size(418, 28);
            this.pnlBtn.TabIndex = 1;
            this.btnRealTimeReport.AllowDrop = true;
            this.btnRealTimeReport.Location = new Point(258, 3);
            this.btnRealTimeReport.Name = "btnRealTimeReport";
            this.btnRealTimeReport.Size = new Size(82, 23);
            this.btnRealTimeReport.TabIndex = 9;
            this.btnRealTimeReport.Text = "实时监控...";
            this.btnRealTimeReport.UseVisualStyleBackColor = true;
            this.btnRealTimeReport.Click += new EventHandler(this.btnRealTimeReport_Click);
            this.btnStopReport.Location = new Point(169, 3);
            this.btnStopReport.Name = "btnStopReport";
            this.btnStopReport.Size = new Size(80, 23);
            this.btnStopReport.TabIndex = 8;
            this.btnStopReport.Text = "结束报警...";
            this.btnStopReport.UseVisualStyleBackColor = true;
            this.btnStopReport.Click += new EventHandler(this.btnStopReport_Click);
            this.btnClose.Location = new Point(346, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(69, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "关闭(&C)";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.pnlAlerm.Controls.Add(this.tabControl1);
            this.pnlAlerm.Dock = DockStyle.Fill;
            this.pnlAlerm.Location = new Point(5, 5);
            this.pnlAlerm.Name = "pnlAlerm";
            this.pnlAlerm.Size = new Size(418, 364);
            this.pnlAlerm.TabIndex = 2;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.ClientSize = new Size(428, 402);
            base.Controls.Add(this.pnlAlerm);
            base.Controls.Add(this.pnlBtn);
            base.Name = "CarAlerm";
            base.Padding = new Padding(5);
            this.Text = "车辆报警";
            base.Load += new EventHandler(this.itmCarAlerm_Load);
            base.FormClosing += new FormClosingEventHandler(this.CarAlerm_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tpAlerm.ResumeLayout(false);
            this.tpAlerm.PerformLayout();
            this.tpCarInfo.ResumeLayout(false);
            this.tpCarInfo.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
            this.pnlAlerm.ResumeLayout(false);
            base.ResumeLayout(false);
        }

       
        private IContainer components;
        private Label lblAddress;
        private Label lblAddressValue;
        private Label lblAlermTime;
        private Label lblAlermTimeValue;
        private Label lblAlermType;
        private Label lblAlermTypeValue;
        private Label lblArea;
        private Label lblAreaValue;
        private Label lblCarBrand;
        private Label lblCarBrandValue;
        private Label lblCarId;
        private Label lblCarIdValue;
        private Label lblCarNum;
        private Label lblCarNumValue;
        private Label lblColor;
        private Label lblColorValue;
        private Label lblCompany;
        private Label lblCompanyValue;
        private Label lblFirstLinkman;
        private Label lblFirstLinkmanTel;
        private Label lblFirstLinkmanTelValue;
        private Label lblFirstLinkmanValue;
        private Label lblIdentityCard;
        private Label lblIdentityCardValue;
        private Label lblLatitude;
        private Label lblLatitudeValue;
        private Label lblLongitude;
        private Label lblLongitudeValue;
        private Label lblOtherLink;
        private Label lblOtherLinkValue;
        private Label lblOwnerName;
        private Label lblOwnerNameValue;
        private Label lblOwnerSex;
        private Label lblOwnerSexValue;
        private Label lblOwnerSimNum;
        private Label lblOwnerSimNumValue;
        private Label lblPostCode;
        private Label lblPostCodeValue;
        private Label lblSecondLinkman;
        private Label lblSecondLinkmanTel;
        private Label lblSecondLinkmanTelValue;
        private Label lblSecondLinkmanValue;
        private Label lblSimNum;
        private Label lblSimNumValue;
        private Panel pnlAlerm;
        private string sCarID;
        private System.Windows.Forms.TabControl tabControl1;
        private TabPage tpAlerm;
        private TabPage tpCarInfo;
    }
}