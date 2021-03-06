﻿namespace Client
{
    using Properties;
    using PublicClass;
    using Remoting;
    using ParamLibrary.Application;
    using ParamLibrary.CmdParamInfo;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using WinFormsUI.Controls;

    partial class itmSegPath
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(itmSegPath));
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            this.grpSetSubSpeedAlarmParam = new GroupBox();
            this.dgvSubSpeedParam = new DataGridViewEx();
            this.PathName = new DataGridViewTextBoxColumn();
            this.Choose = new DataGridViewCheckBoxColumn();
            this.TopSpeed = new DataGridViewTextBoxColumn();
            this.HoldTime = new DataGridViewTextBoxColumn();
            this.PathID = new DataGridViewTextBoxColumn();
            this.carID = new DataGridViewTextBoxColumn();
            this.IsChoose = new DataGridViewTextBoxColumn();
            this.BeginTime = new DataGridViewTextBoxColumn();
            this.EndTime = new DataGridViewTextBoxColumn();
            this.pnlFilter = new Panel();
            this.toolStrip2 = new ToolStrip();
            this.tsBtnSearchdata = new ToolStripButton();
            this.txtFindRegion = new TextBox();
            this.lblFindRegion = new Label();
            this.comboBoxLines = new ComboBox();
            this.toolStrip1 = new ToolStrip();
            this.tsBtnFilter = new ToolStripButton();
            this.lbLine = new Label();
            this.btmAllSelect = new Button();
            this.btnCopyToSelected = new Button();
            this.pnlWait = new Panel();
            this.pbPicWait = new PictureBox();
            this.lblWaitText = new Label();
            base.grpCar.SuspendLayout();
            base.pnlBtn.SuspendLayout();
            this.grpSetSubSpeedAlarmParam.SuspendLayout();
            ((ISupportInitialize) this.dgvSubSpeedParam).BeginInit();
            this.pnlFilter.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnlWait.SuspendLayout();
            ((ISupportInitialize) this.pbPicWait).BeginInit();
            base.SuspendLayout();
            base.grpCar.Size = new Size(538, 116);
            base.grpCar.TabIndex = 0;
            base.pnlBtn.Controls.Add(this.pnlWait);
            base.pnlBtn.Controls.Add(this.btnCopyToSelected);
            base.pnlBtn.Controls.Add(this.btmAllSelect);
            base.pnlBtn.Dock = DockStyle.Bottom;
            base.pnlBtn.Location = new System.Drawing.Point(5, 404);
            base.pnlBtn.Size = new Size(538, 28);
            base.pnlBtn.TabIndex = 2;
            base.pnlBtn.Controls.SetChildIndex(this.btmAllSelect, 0);
            base.pnlBtn.Controls.SetChildIndex(this.btnCopyToSelected, 0);
            base.pnlBtn.Controls.SetChildIndex(base.btnOK, 0);
            base.pnlBtn.Controls.SetChildIndex(base.btnCancel, 0);
            base.pnlBtn.Controls.SetChildIndex(this.pnlWait, 0);
            base.btnCancel.Location = new System.Drawing.Point(286, 5);
            base.btnCancel.TabIndex = 1;
            base.btnOK.Location = new System.Drawing.Point(201, 5);
            base.btnOK.TabIndex = 0;
            this.grpSetSubSpeedAlarmParam.Controls.Add(this.dgvSubSpeedParam);
            this.grpSetSubSpeedAlarmParam.Controls.Add(this.pnlFilter);
            this.grpSetSubSpeedAlarmParam.Dock = DockStyle.Fill;
            this.grpSetSubSpeedAlarmParam.Location = new System.Drawing.Point(5, 121);
            this.grpSetSubSpeedAlarmParam.Name = "grpSetSubSpeedAlarmParam";
            this.grpSetSubSpeedAlarmParam.Size = new Size(538, 283);
            this.grpSetSubSpeedAlarmParam.TabIndex = 1;
            this.grpSetSubSpeedAlarmParam.TabStop = false;
            this.grpSetSubSpeedAlarmParam.Text = "设置分断超速报警参数";
            this.dgvSubSpeedParam.AllowUserToAddRows = false;
            this.dgvSubSpeedParam.AllowUserToDeleteRows = false;
            this.dgvSubSpeedParam.AllowUserToResizeRows = false;
            this.dgvSubSpeedParam.BackgroundColor = SystemColors.ControlLightLight;
            this.dgvSubSpeedParam.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSubSpeedParam.Columns.AddRange(new DataGridViewColumn[] { this.PathName, this.Choose, this.TopSpeed, this.HoldTime, this.PathID, this.carID, this.IsChoose, this.BeginTime, this.EndTime });
            this.dgvSubSpeedParam.Dock = DockStyle.Fill;
            this.dgvSubSpeedParam.ImeMode =  System.Windows.Forms.ImeMode.Disable;
            this.dgvSubSpeedParam.Location = new System.Drawing.Point(3, 43);
            this.dgvSubSpeedParam.Name = "dgvSubSpeedParam";
            this.dgvSubSpeedParam.NotMultiSelectedColumnName = (List<string>)resources.GetObject("dgvSubSpeedParam.NotMultiSelectedColumnName");
            this.dgvSubSpeedParam.RowHeadersVisible = false;
            this.dgvSubSpeedParam.RowTemplate.Height = 20;
            this.dgvSubSpeedParam.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.dgvSubSpeedParam.Size = new Size(532, 237);
            this.dgvSubSpeedParam.TabIndex = 0;
            this.dgvSubSpeedParam.CellValueChanged += new DataGridViewCellEventHandler(this.dgvSubSpeedParam_CellValueChanged);
            this.dgvSubSpeedParam.CellMouseUp += new DataGridViewCellMouseEventHandler(this.dgvSubSpeedParam_CellMouseUp);
            this.dgvSubSpeedParam.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvSubSpeedParam_CellDoubleClick);
            this.dgvSubSpeedParam.CellClick += new DataGridViewCellEventHandler(this.dgvSubSpeedParam_CellClick);
            this.dgvSubSpeedParam.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgvSubSpeedParam_EditingControlShowing);
            this.PathName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.PathName.DataPropertyName = "PathName";
            style.BackColor = Color.FromArgb(192, 192, 255);
            this.PathName.DefaultCellStyle = style;
            this.PathName.Frozen = true;
            this.PathName.HeaderText = "区域名称";
            this.PathName.MinimumWidth = 78;
            this.PathName.Name = "PathName";
            this.PathName.ReadOnly = true;
            this.PathName.Width = 78;
            this.Choose.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.Choose.DataPropertyName = "Choose";
            this.Choose.FalseValue = "0";
            this.Choose.HeaderText = "选择";
            this.Choose.MinimumWidth = 35;
            this.Choose.Name = "Choose";
            this.Choose.TrueValue = "1";
            this.Choose.Width = 35;
            this.TopSpeed.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.TopSpeed.DataPropertyName = "TopSpeed";
            this.TopSpeed.HeaderText = "最高时速(km/h)";
            this.TopSpeed.MaxInputLength = 3;
            this.TopSpeed.MinimumWidth = 100;
            this.TopSpeed.Name = "TopSpeed";
            this.TopSpeed.ReadOnly = true;
            this.TopSpeed.Resizable = DataGridViewTriState.False;
            this.TopSpeed.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.HoldTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.HoldTime.DataPropertyName = "HoldTime";
            this.HoldTime.HeaderText = "持续时长(秒)";
            this.HoldTime.MaxInputLength = 3;
            this.HoldTime.MinimumWidth = 90;
            this.HoldTime.Name = "HoldTime";
            this.HoldTime.ReadOnly = true;
            this.HoldTime.Resizable = DataGridViewTriState.False;
            this.HoldTime.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.HoldTime.Width = 90;
            this.PathID.DataPropertyName = "PathID";
            this.PathID.HeaderText = "区域ID";
            this.PathID.Name = "PathID";
            this.PathID.Visible = false;
            this.carID.DataPropertyName = "carID";
            this.carID.HeaderText = "车辆ID";
            this.carID.Name = "carID";
            this.carID.Visible = false;
            this.IsChoose.DataPropertyName = "IsChoose";
            this.IsChoose.HeaderText = "选择";
            this.IsChoose.Name = "IsChoose";
            this.IsChoose.Visible = false;
            this.BeginTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.BeginTime.DataPropertyName = "BeginTime";
            this.BeginTime.HeaderText = "起始时间";
            this.BeginTime.MinimumWidth = 78;
            this.BeginTime.Name = "BeginTime";
            this.BeginTime.ReadOnly = true;
            this.BeginTime.Width = 78;
            this.EndTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.EndTime.DataPropertyName = "EndTime";
            this.EndTime.HeaderText = "终止时间";
            this.EndTime.MinimumWidth = 78;
            this.EndTime.Name = "EndTime";
            this.EndTime.ReadOnly = true;
            this.EndTime.Width = 78;
            this.pnlFilter.Controls.Add(this.toolStrip2);
            this.pnlFilter.Controls.Add(this.txtFindRegion);
            this.pnlFilter.Controls.Add(this.lblFindRegion);
            this.pnlFilter.Controls.Add(this.comboBoxLines);
            this.pnlFilter.Controls.Add(this.toolStrip1);
            this.pnlFilter.Controls.Add(this.lbLine);
            this.pnlFilter.Dock = DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(3, 17);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new Size(532, 26);
            this.pnlFilter.TabIndex = 2;
            this.toolStrip2.Dock = DockStyle.None;
            this.toolStrip2.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new ToolStripItem[] { this.tsBtnSearchdata });
            this.toolStrip2.Location = new System.Drawing.Point(481, 1);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new Size(26, 25);
            this.toolStrip2.TabIndex = 11;
            this.toolStrip2.Text = "toolStrip2";
            this.tsBtnSearchdata.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsBtnSearchdata.Image = Resources.find;
            this.tsBtnSearchdata.ImageTransparentColor = Color.Magenta;
            this.tsBtnSearchdata.Name = "tsBtnSearchdata";
            this.tsBtnSearchdata.Size = new Size(23, 22);
            this.tsBtnSearchdata.Text = "区域过滤查找";
            this.tsBtnSearchdata.Click += new EventHandler(this.tsBtnSearchdata_Click);
            this.txtFindRegion.Location = new System.Drawing.Point(378, 4);
            this.txtFindRegion.MaxLength = 100;
            this.txtFindRegion.Name = "txtFindRegion";
            this.txtFindRegion.Size = new Size(100, 21);
            this.txtFindRegion.TabIndex = 10;
            this.txtFindRegion.Tag = "";
            this.txtFindRegion.TextChanged += new EventHandler(this.txtFindRegion_TextChanged);
            this.lblFindRegion.AutoSize = true;
            this.lblFindRegion.Location = new System.Drawing.Point(317, 8);
            this.lblFindRegion.Name = "lblFindRegion";
            this.lblFindRegion.Size = new Size(65, 12);
            this.lblFindRegion.TabIndex = 9;
            this.lblFindRegion.Tag = "999";
            this.lblFindRegion.Text = "查找区域：";
            this.comboBoxLines.DisplayMember = "PathgroupName";
            this.comboBoxLines.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxLines.FormattingEnabled = true;
            this.comboBoxLines.ImeMode =  System.Windows.Forms.ImeMode.NoControl;
            this.comboBoxLines.Location = new System.Drawing.Point(123, 3);
            this.comboBoxLines.Name = "comboBoxLines";
            this.comboBoxLines.Size = new Size(157, 20);
            this.comboBoxLines.TabIndex = 3;
            this.comboBoxLines.Tag = "999";
            this.comboBoxLines.ValueMember = "PathgroupID";
            this.toolStrip1.Dock = DockStyle.None;
            this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.tsBtnFilter });
            this.toolStrip1.Location = new System.Drawing.Point(285, 1);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(26, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.tsBtnFilter.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsBtnFilter.Image = Resources.find;
            this.tsBtnFilter.ImageTransparentColor = Color.Magenta;
            this.tsBtnFilter.Name = "tsBtnFilter";
            this.tsBtnFilter.Size = new Size(23, 22);
            this.tsBtnFilter.Text = "搜索 ";
            this.tsBtnFilter.Click += new EventHandler(this.tsBtnFilter_Click);
            this.lbLine.AutoSize = true;
            this.lbLine.Location = new System.Drawing.Point(23, 7);
            this.lbLine.Name = "lbLine";
            this.lbLine.Size = new Size(89, 12);
            this.lbLine.TabIndex = 1;
            this.lbLine.Tag = "999";
            this.lbLine.Text = "路线分组名称：";
            this.btmAllSelect.Location = new System.Drawing.Point(31, 5);
            this.btmAllSelect.Name = "btmAllSelect";
            this.btmAllSelect.Size = new Size(75, 23);
            this.btmAllSelect.TabIndex = 2;
            this.btmAllSelect.Text = "全选";
            this.btmAllSelect.UseVisualStyleBackColor = true;
            this.btmAllSelect.Click += new EventHandler(this.btmAllSelect_Click);
            this.btnCopyToSelected.Location = new System.Drawing.Point(116, 5);
            this.btnCopyToSelected.Name = "btnCopyToSelected";
            this.btnCopyToSelected.Size = new Size(75, 23);
            this.btnCopyToSelected.TabIndex = 3;
            this.btnCopyToSelected.Text = "应用到所选";
            this.btnCopyToSelected.UseVisualStyleBackColor = true;
            this.btnCopyToSelected.Click += new EventHandler(this.btnCopyToSelected_Click);
            this.pnlWait.Controls.Add(this.pbPicWait);
            this.pnlWait.Controls.Add(this.lblWaitText);
            this.pnlWait.Location = new System.Drawing.Point(362, 3);
            this.pnlWait.Name = "pnlWait";
            this.pnlWait.Size = new Size(129, 22);
            this.pnlWait.TabIndex = 14;
            this.pnlWait.Tag = "9999";
            this.pbPicWait.BackColor = Color.Transparent;
            this.pbPicWait.Image = Resources.loading;
            this.pbPicWait.InitialImage = null;
            this.pbPicWait.Location = new System.Drawing.Point(3, 3);
            this.pbPicWait.Name = "pbPicWait";
            this.pbPicWait.Size = new Size(16, 16);
            this.pbPicWait.TabIndex = 11;
            this.pbPicWait.TabStop = false;
            this.pbPicWait.Tag = "9999";
            this.pbPicWait.Visible = false;
            this.lblWaitText.AutoSize = true;
            this.lblWaitText.Location = new System.Drawing.Point(22, 5);
            this.lblWaitText.Name = "lblWaitText";
            this.lblWaitText.Size = new Size(89, 12);
            this.lblWaitText.TabIndex = 9;
            this.lblWaitText.Text = "正在执行中....";
            this.lblWaitText.Visible = false;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode =  System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            base.ClientSize = new Size(548, 437);
            base.Controls.Add(this.grpSetSubSpeedAlarmParam);
            base.FormBorderStyle =  System.Windows.Forms.FormBorderStyle.Sizable;
            base.MaximizeBox = true;
            base.Name = "itmSegPath";
            this.Text = "SetSubSpeedAlarm";
            base.Load += new EventHandler(this.itmSegPath_Load);
            base.Controls.SetChildIndex(base.pnlBtn, 0);
            base.Controls.SetChildIndex(base.grpCar, 0);
            base.Controls.SetChildIndex(this.grpSetSubSpeedAlarmParam, 0);
            base.grpCar.ResumeLayout(false);
            base.grpCar.PerformLayout();
            base.pnlBtn.ResumeLayout(false);
            this.grpSetSubSpeedAlarmParam.ResumeLayout(false);
            ((ISupportInitialize) this.dgvSubSpeedParam).EndInit();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlWait.ResumeLayout(false);
            this.pnlWait.PerformLayout();
            ((ISupportInitialize) this.pbPicWait).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

       
        private IContainer components;
        private DataGridViewTextBoxColumn BeginTime;
        private Button btmAllSelect;
        private Button btnCopyToSelected;
        private DataGridViewTextBoxColumn carID;
        private DataGridViewCheckBoxColumn Choose;
        private ComboBox comboBoxLines;
        private DataGridViewEx dgvSubSpeedParam;
        private DataGridViewTextBoxEditingControl EditingControl;
        private DataGridViewTextBoxColumn EndTime;
        private GroupBox grpSetSubSpeedAlarmParam;
        private DataGridViewTextBoxColumn HoldTime;
        private DataGridViewTextBoxColumn IsChoose;
        private Label lblFindRegion;
        private Label lbLine;
        private Label lblWaitText;
        private DataGridViewTextBoxColumn PathID;
        private DataGridViewTextBoxColumn PathName;
        private PictureBox pbPicWait;
        private Panel pnlFilter;
        private Panel pnlWait;
        private ToolStrip toolStrip1;
        private ToolStrip toolStrip2;
        private DataGridViewTextBoxColumn TopSpeed;
        private ToolStripButton tsBtnFilter;
        private ToolStripButton tsBtnSearchdata;
        private TextBox txtFindRegion;
    }
}