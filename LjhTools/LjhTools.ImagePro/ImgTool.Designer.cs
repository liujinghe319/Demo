using System.Drawing;

namespace LjhTools.ImagePro
{
    partial class ImgTool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImgTool));
            this.lbxDataFile = new System.Windows.Forms.ListBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.lblData = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblTip = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.txtTail = new System.Windows.Forms.TextBox();
            this.lblTail = new System.Windows.Forms.Label();
            this.cbkMark = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lable12 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTrans = new System.Windows.Forms.TextBox();
            this.txtQuality = new System.Windows.Forms.TextBox();
            this.txtPosition = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCopy = new System.Windows.Forms.Button();
            this.cbxNoZoom = new System.Windows.Forms.CheckBox();
            this.radBmnoSize = new System.Windows.Forms.RadioButton();
            this.radCloudSize = new System.Windows.Forms.RadioButton();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.txtSize1 = new System.Windows.Forms.TextBox();
            this.txtSize2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btnDownload = new System.Windows.Forms.Button();
            this.cboAddIndex = new System.Windows.Forms.CheckBox();
            this.cboAddSize = new System.Windows.Forms.CheckBox();
            this.btnFile = new System.Windows.Forms.Button();
            this.cboImgFormat = new System.Windows.Forms.ComboBox();
            this.btnSaveTo = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnToFolder = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbxDataFile
            // 
            this.lbxDataFile.FormattingEnabled = true;
            this.lbxDataFile.ItemHeight = 12;
            this.lbxDataFile.Location = new System.Drawing.Point(91, 74);
            this.lbxDataFile.Name = "lbxDataFile";
            this.lbxDataFile.Size = new System.Drawing.Size(317, 184);
            this.lbxDataFile.TabIndex = 28;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(595, 25);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 27;
            this.btnSelect.Text = "选择";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtPath
            // 
            this.txtPath.Enabled = false;
            this.txtPath.Location = new System.Drawing.Point(91, 27);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(482, 21);
            this.txtPath.TabIndex = 26;
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Location = new System.Drawing.Point(26, 31);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(53, 12);
            this.lblData.TabIndex = 25;
            this.lblData.Text = "图片路径";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(455, 358);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(109, 28);
            this.btnStart.TabIndex = 34;
            this.btnStart.Text = "开始批处理";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.start_Click);
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.Location = new System.Drawing.Point(89, 374);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(41, 12);
            this.lblTip.TabIndex = 35;
            this.lblTip.Text = "lblTip";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(453, 294);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 36;
            this.label4.Text = "后缀";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(47, 293);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 37;
            this.label5.Text = "尺寸";
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(91, 290);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(317, 21);
            this.txtSize.TabIndex = 38;
            this.txtSize.Text = "72*54|400*300|800*600|218*164|100*75|240*180|80*60|200*150";
            // 
            // txtTail
            // 
            this.txtTail.Location = new System.Drawing.Point(490, 290);
            this.txtTail.Name = "txtTail";
            this.txtTail.Size = new System.Drawing.Size(74, 21);
            this.txtTail.TabIndex = 39;
            this.txtTail.Text = "-small";
            // 
            // lblTail
            // 
            this.lblTail.AutoSize = true;
            this.lblTail.ForeColor = System.Drawing.Color.Red;
            this.lblTail.Location = new System.Drawing.Point(574, 295);
            this.lblTail.Name = "lblTail";
            this.lblTail.Size = new System.Drawing.Size(89, 12);
            this.lblTail.TabIndex = 40;
            this.lblTail.Text = "默认分辨率后缀";
            // 
            // cbkMark
            // 
            this.cbkMark.AutoSize = true;
            this.cbkMark.Location = new System.Drawing.Point(455, 70);
            this.cbkMark.Name = "cbkMark";
            this.cbkMark.Size = new System.Drawing.Size(60, 16);
            this.cbkMark.TabIndex = 41;
            this.cbkMark.Text = "打水印";
            this.cbkMark.UseVisualStyleBackColor = true;
            this.cbkMark.CheckedChanged += new System.EventHandler(this.cbkMark_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 44;
            this.label6.Text = "透明度";
            // 
            // lable12
            // 
            this.lable12.AutoSize = true;
            this.lable12.Location = new System.Drawing.Point(13, 67);
            this.lable12.Name = "lable12";
            this.lable12.Size = new System.Drawing.Size(41, 12);
            this.lable12.TabIndex = 45;
            this.lable12.Text = "图质量";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 107);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 46;
            this.label7.Text = "位置";
            // 
            // txtTrans
            // 
            this.txtTrans.Location = new System.Drawing.Point(70, 23);
            this.txtTrans.Name = "txtTrans";
            this.txtTrans.Size = new System.Drawing.Size(48, 21);
            this.txtTrans.TabIndex = 47;
            this.txtTrans.Text = "3";
            // 
            // txtQuality
            // 
            this.txtQuality.Location = new System.Drawing.Point(69, 64);
            this.txtQuality.Name = "txtQuality";
            this.txtQuality.Size = new System.Drawing.Size(48, 21);
            this.txtQuality.TabIndex = 48;
            this.txtQuality.Text = "70";
            // 
            // txtPosition
            // 
            this.txtPosition.Location = new System.Drawing.Point(68, 104);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(48, 21);
            this.txtPosition.TabIndex = 49;
            this.txtPosition.Text = "5";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(140, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 50;
            this.label8.Text = "*1-10";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(140, 67);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 51;
            this.label9.Text = "*1-100";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(140, 107);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 52;
            this.label10.Text = "*九宫格1-9";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtQuality);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.lable12);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtPosition);
            this.groupBox1.Controls.Add(this.txtTrans);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(455, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 140);
            this.groupBox1.TabIndex = 53;
            this.groupBox1.TabStop = false;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(91, 261);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(0, 12);
            this.lblCount.TabIndex = 54;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(514, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 55;
            this.label1.Text = "(水印图片应小于底图)";
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(456, 467);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(108, 28);
            this.btnCopy.TabIndex = 56;
            this.btnCopy.Text = "图片尺寸分组";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // cbxNoZoom
            // 
            this.cbxNoZoom.AutoSize = true;
            this.cbxNoZoom.Location = new System.Drawing.Point(455, 97);
            this.cbxNoZoom.Name = "cbxNoZoom";
            this.cbxNoZoom.Size = new System.Drawing.Size(180, 16);
            this.cbxNoZoom.TabIndex = 58;
            this.cbxNoZoom.Text = "图片不放大只置于新图片中央";
            this.cbxNoZoom.UseVisualStyleBackColor = true;
            // 
            // radBmnoSize
            // 
            this.radBmnoSize.AutoSize = true;
            this.radBmnoSize.Location = new System.Drawing.Point(91, 327);
            this.radBmnoSize.Name = "radBmnoSize";
            this.radBmnoSize.Size = new System.Drawing.Size(83, 16);
            this.radBmnoSize.TabIndex = 59;
            this.radBmnoSize.TabStop = true;
            this.radBmnoSize.Text = "北迈号尺寸";
            this.radBmnoSize.UseVisualStyleBackColor = true;
            this.radBmnoSize.CheckedChanged += new System.EventHandler(this.radBmnoSize_CheckedChanged);
            // 
            // radCloudSize
            // 
            this.radCloudSize.AutoSize = true;
            this.radCloudSize.Location = new System.Drawing.Point(191, 327);
            this.radCloudSize.Name = "radCloudSize";
            this.radCloudSize.Size = new System.Drawing.Size(83, 16);
            this.radCloudSize.TabIndex = 60;
            this.radCloudSize.TabStop = true;
            this.radCloudSize.Text = "北迈云尺寸";
            this.radCloudSize.UseVisualStyleBackColor = true;
            this.radCloudSize.CheckedChanged += new System.EventHandler(this.radCloudSize_CheckedChanged);
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(92, 421);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(50, 21);
            this.txtWidth.TabIndex = 61;
            this.txtWidth.Text = "300";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(164, 421);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(50, 21);
            this.txtHeight.TabIndex = 62;
            this.txtHeight.Text = "300";
            // 
            // txtSize1
            // 
            this.txtSize1.Location = new System.Drawing.Point(285, 421);
            this.txtSize1.Name = "txtSize1";
            this.txtSize1.Size = new System.Drawing.Size(50, 21);
            this.txtSize1.TabIndex = 63;
            this.txtSize1.Text = "1";
            // 
            // txtSize2
            // 
            this.txtSize2.Location = new System.Drawing.Point(355, 421);
            this.txtSize2.Name = "txtSize2";
            this.txtSize2.Size = new System.Drawing.Size(50, 21);
            this.txtSize2.TabIndex = 64;
            this.txtSize2.Text = "2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 424);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 65;
            this.label2.Text = "分辩率";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(246, 424);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 66;
            this.label11.Text = "比例";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("等线", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(144, 420);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(19, 21);
            this.label12.TabIndex = 67;
            this.label12.Text = "x";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("等线", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(337, 420);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(18, 21);
            this.label13.TabIndex = 68;
            this.label13.Text = "/";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(90, 454);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(161, 12);
            this.label14.TabIndex = 69;
            this.label14.Text = "小于此分辨率或比例为不合格";
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(570, 358);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 28);
            this.btnDownload.TabIndex = 70;
            this.btnDownload.Text = "批量下载图片";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // cboAddIndex
            // 
            this.cboAddIndex.AutoSize = true;
            this.cboAddIndex.Location = new System.Drawing.Point(455, 327);
            this.cboAddIndex.Name = "cboAddIndex";
            this.cboAddIndex.Size = new System.Drawing.Size(72, 16);
            this.cboAddIndex.TabIndex = 73;
            this.cboAddIndex.Text = "添加序号";
            this.cboAddIndex.UseVisualStyleBackColor = true;
            // 
            // cboAddSize
            // 
            this.cboAddSize.AutoSize = true;
            this.cboAddSize.Checked = true;
            this.cboAddSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboAddSize.Location = new System.Drawing.Point(576, 327);
            this.cboAddSize.Name = "cboAddSize";
            this.cboAddSize.Size = new System.Drawing.Size(96, 16);
            this.cboAddSize.TabIndex = 74;
            this.cboAddSize.Text = "添加尺寸后缀";
            this.cboAddSize.UseVisualStyleBackColor = true;
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(576, 467);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(100, 28);
            this.btnFile.TabIndex = 75;
            this.btnFile.Text = "补充空白边缘";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // cboImgFormat
            // 
            this.cboImgFormat.Font = new System.Drawing.Font("隶书", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboImgFormat.FormattingEnabled = true;
            this.cboImgFormat.ItemHeight = 16;
            this.cboImgFormat.Items.AddRange(new object[] {
            "jpg",
            "png"});
            this.cboImgFormat.Location = new System.Drawing.Point(570, 417);
            this.cboImgFormat.Name = "cboImgFormat";
            this.cboImgFormat.Size = new System.Drawing.Size(76, 24);
            this.cboImgFormat.TabIndex = 76;
            // 
            // btnSaveTo
            // 
            this.btnSaveTo.Location = new System.Drawing.Point(455, 416);
            this.btnSaveTo.Name = "btnSaveTo";
            this.btnSaveTo.Size = new System.Drawing.Size(109, 28);
            this.btnSaveTo.TabIndex = 77;
            this.btnSaveTo.Text = "图片另存为";
            this.btnSaveTo.UseVisualStyleBackColor = true;
            this.btnSaveTo.Click += new System.EventHandler(this.btnSaveTo_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(647, 424);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 78;
            this.label3.Text = "格式";
            // 
            // btnToFolder
            // 
            this.btnToFolder.Location = new System.Drawing.Point(324, 467);
            this.btnToFolder.Name = "btnToFolder";
            this.btnToFolder.Size = new System.Drawing.Size(108, 28);
            this.btnToFolder.TabIndex = 79;
            this.btnToFolder.Text = "图片名称分目录";
            this.btnToFolder.UseVisualStyleBackColor = true;
            this.btnToFolder.Click += new System.EventHandler(this.btnToFolder_Click);
            // 
            // ImgTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 518);
            this.Controls.Add(this.btnToFolder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSaveTo);
            this.Controls.Add(this.cboImgFormat);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.cboAddSize);
            this.Controls.Add(this.cboAddIndex);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSize2);
            this.Controls.Add(this.txtSize1);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.txtWidth);
            this.Controls.Add(this.radCloudSize);
            this.Controls.Add(this.radBmnoSize);
            this.Controls.Add(this.cbxNoZoom);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbkMark);
            this.Controls.Add(this.lblTail);
            this.Controls.Add(this.txtTail);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblTip);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lbxDataFile);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.lblData);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImgTool";
            this.Text = "图片批量处理工具";
            this.Load += new System.EventHandler(this.ImgTool_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbxDataFile;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblTip;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.TextBox txtTail;
        private System.Windows.Forms.Label lblTail;
        private System.Windows.Forms.CheckBox cbkMark;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lable12;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTrans;
        private System.Windows.Forms.TextBox txtQuality;
        private System.Windows.Forms.TextBox txtPosition;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.CheckBox cbxNoZoom;
        private System.Windows.Forms.RadioButton radBmnoSize;
        private System.Windows.Forms.RadioButton radCloudSize;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.TextBox txtSize1;
        private System.Windows.Forms.TextBox txtSize2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.CheckBox cboAddIndex;
        private System.Windows.Forms.CheckBox cboAddSize;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.ComboBox cboImgFormat;
        private System.Windows.Forms.Button btnSaveTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnToFolder;
    }
}