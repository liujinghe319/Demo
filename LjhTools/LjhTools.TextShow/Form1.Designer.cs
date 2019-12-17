namespace LjhTools.TextShow
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.cbxTable = new System.Windows.Forms.CheckBox();
            this.lblSplit = new System.Windows.Forms.Label();
            this.txtLine = new System.Windows.Forms.TextBox();
            this.btnSplit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtALL = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cbxWrap = new System.Windows.Forms.CheckBox();
            this.cbxRepeat = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnGroup = new System.Windows.Forms.Button();
            this.lblTip1 = new System.Windows.Forms.Label();
            this.btmSearch = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblTip = new System.Windows.Forms.Label();
            this.radExcel = new System.Windows.Forms.RadioButton();
            this.radTxt = new System.Windows.Forms.RadioButton();
            this.cbxRemove1 = new System.Windows.Forms.CheckBox();
            this.txtProcess = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lbxDataFile = new System.Windows.Forms.ListBox();
            this.cbxFirst = new System.Windows.Forms.CheckBox();
            this.btnMerge = new System.Windows.Forms.Button();
            this.txtNum = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnFolder = new System.Windows.Forms.Button();
            this.btnFindFile = new System.Windows.Forms.Button();
            this.btnToUpper = new System.Windows.Forms.Button();
            this.btnToLower = new System.Windows.Forms.Button();
            this.btnCopyPath = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.btnSelect4 = new System.Windows.Forms.Button();
            this.txtFileSource = new System.Windows.Forms.TextBox();
            this.txtContent3 = new System.Windows.Forms.TextBox();
            this.btnDelete3 = new System.Windows.Forms.Button();
            this.btnMove3 = new System.Windows.Forms.Button();
            this.txtProcess3 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCopy3 = new System.Windows.Forms.Button();
            this.txtNum3 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSelect3 = new System.Windows.Forms.Button();
            this.txtFile3 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFile
            // 
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.Location = new System.Drawing.Point(66, 11);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(586, 21);
            this.txtFile.TabIndex = 1;
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Location = new System.Drawing.Point(665, 11);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(68, 21);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "选择文本";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtContent
            // 
            this.txtContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContent.BackColor = System.Drawing.Color.White;
            this.txtContent.Location = new System.Drawing.Point(9, 44);
            this.txtContent.MaxLength = 3276700;
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtContent.Size = new System.Drawing.Size(882, 339);
            this.txtContent.TabIndex = 3;
            this.txtContent.WordWrap = false;
            // 
            // ofd
            // 
            this.ofd.FileName = "openFileDialog1";
            this.ofd.Filter = "文本文件(*.txt)|*.txt";
            // 
            // cbxTable
            // 
            this.cbxTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxTable.AutoSize = true;
            this.cbxTable.Location = new System.Drawing.Point(14, 396);
            this.cbxTable.Name = "cbxTable";
            this.cbxTable.Size = new System.Drawing.Size(84, 16);
            this.cbxTable.TabIndex = 4;
            this.cbxTable.Text = "显示制表符";
            this.cbxTable.UseVisualStyleBackColor = true;
            this.cbxTable.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // lblSplit
            // 
            this.lblSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSplit.AutoSize = true;
            this.lblSplit.Location = new System.Drawing.Point(621, 398);
            this.lblSplit.Name = "lblSplit";
            this.lblSplit.Size = new System.Drawing.Size(53, 12);
            this.lblSplit.TabIndex = 5;
            this.lblSplit.Text = "文件行数";
            // 
            // txtLine
            // 
            this.txtLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLine.Location = new System.Drawing.Point(681, 393);
            this.txtLine.Name = "txtLine";
            this.txtLine.Size = new System.Drawing.Size(40, 21);
            this.txtLine.TabIndex = 6;
            this.txtLine.Text = "1000";
            // 
            // btnSplit
            // 
            this.btnSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSplit.Location = new System.Drawing.Point(728, 393);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(46, 21);
            this.btnSplit.TabIndex = 7;
            this.btnSplit.Text = "拆分";
            this.btnSplit.UseVisualStyleBackColor = true;
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(194, 398);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "行数";
            // 
            // txtALL
            // 
            this.txtALL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtALL.Location = new System.Drawing.Point(228, 394);
            this.txtALL.Name = "txtALL";
            this.txtALL.Size = new System.Drawing.Size(37, 21);
            this.txtALL.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择文本";
            // 
            // cbxWrap
            // 
            this.cbxWrap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxWrap.AutoSize = true;
            this.cbxWrap.Location = new System.Drawing.Point(102, 396);
            this.cbxWrap.Name = "cbxWrap";
            this.cbxWrap.Size = new System.Drawing.Size(84, 16);
            this.cbxWrap.TabIndex = 12;
            this.cbxWrap.Text = "显示换行符";
            this.cbxWrap.UseVisualStyleBackColor = true;
            this.cbxWrap.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // cbxRepeat
            // 
            this.cbxRepeat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxRepeat.AutoSize = true;
            this.cbxRepeat.Location = new System.Drawing.Point(506, 397);
            this.cbxRepeat.Name = "cbxRepeat";
            this.cbxRepeat.Size = new System.Drawing.Size(108, 16);
            this.cbxRepeat.TabIndex = 13;
            this.cbxRepeat.Text = "去重复行、空行";
            this.cbxRepeat.UseVisualStyleBackColor = true;
            this.cbxRepeat.CheckedChanged += new System.EventHandler(this.cbxRepeat_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(-5, -1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(911, 453);
            this.tabControl1.TabIndex = 14;
            this.tabControl1.TabIndexChanged += new System.EventHandler(this.tabControl1_TabIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.tabPage1.Controls.Add(this.btnGroup);
            this.tabPage1.Controls.Add(this.lblTip1);
            this.tabPage1.Controls.Add(this.btmSearch);
            this.tabPage1.Controls.Add(this.txtContent);
            this.tabPage1.Controls.Add(this.btnSelect);
            this.tabPage1.Controls.Add(this.btnSplit);
            this.tabPage1.Controls.Add(this.txtFile);
            this.tabPage1.Controls.Add(this.cbxRepeat);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtLine);
            this.tabPage1.Controls.Add(this.cbxTable);
            this.tabPage1.Controls.Add(this.lblSplit);
            this.tabPage1.Controls.Add(this.txtALL);
            this.tabPage1.Controls.Add(this.cbxWrap);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(903, 427);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "文本查看";
            // 
            // btnGroup
            // 
            this.btnGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroup.Location = new System.Drawing.Point(775, 393);
            this.btnGroup.Name = "btnGroup";
            this.btnGroup.Size = new System.Drawing.Size(46, 21);
            this.btnGroup.TabIndex = 16;
            this.btnGroup.Text = "分组";
            this.btnGroup.UseVisualStyleBackColor = true;
            this.btnGroup.Click += new System.EventHandler(this.btnGroup_Click);
            // 
            // lblTip1
            // 
            this.lblTip1.AutoSize = true;
            this.lblTip1.Location = new System.Drawing.Point(283, 401);
            this.lblTip1.Name = "lblTip1";
            this.lblTip1.Size = new System.Drawing.Size(41, 12);
            this.lblTip1.TabIndex = 15;
            this.lblTip1.Text = "lblTip";
            this.lblTip1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // btmSearch
            // 
            this.btmSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btmSearch.Location = new System.Drawing.Point(821, 393);
            this.btmSearch.Name = "btmSearch";
            this.btmSearch.Size = new System.Drawing.Size(70, 21);
            this.btmSearch.TabIndex = 14;
            this.btmSearch.Text = "搜索文件";
            this.btmSearch.UseVisualStyleBackColor = true;
            this.btmSearch.Click += new System.EventHandler(this.btmSearch_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.tabPage2.Controls.Add(this.lblTip);
            this.tabPage2.Controls.Add(this.radExcel);
            this.tabPage2.Controls.Add(this.radTxt);
            this.tabPage2.Controls.Add(this.cbxRemove1);
            this.tabPage2.Controls.Add(this.txtProcess);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.lbxDataFile);
            this.tabPage2.Controls.Add(this.cbxFirst);
            this.tabPage2.Controls.Add(this.btnMerge);
            this.tabPage2.Controls.Add(this.txtNum);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.btnSelectFolder);
            this.tabPage2.Controls.Add(this.txtPath);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(903, 427);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "文本合并";
            // 
            // lblTip
            // 
            this.lblTip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTip.AutoSize = true;
            this.lblTip.Location = new System.Drawing.Point(330, 393);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(0, 12);
            this.lblTip.TabIndex = 15;
            // 
            // radExcel
            // 
            this.radExcel.AutoSize = true;
            this.radExcel.Location = new System.Drawing.Point(25, 164);
            this.radExcel.Name = "radExcel";
            this.radExcel.Size = new System.Drawing.Size(47, 16);
            this.radExcel.TabIndex = 14;
            this.radExcel.Text = "表格";
            this.radExcel.UseVisualStyleBackColor = true;
            this.radExcel.CheckedChanged += new System.EventHandler(this.radExcel_CheckedChanged);
            // 
            // radTxt
            // 
            this.radTxt.AutoSize = true;
            this.radTxt.Checked = true;
            this.radTxt.Location = new System.Drawing.Point(25, 106);
            this.radTxt.Name = "radTxt";
            this.radTxt.Size = new System.Drawing.Size(47, 16);
            this.radTxt.TabIndex = 13;
            this.radTxt.TabStop = true;
            this.radTxt.Text = "文本";
            this.radTxt.UseVisualStyleBackColor = true;
            this.radTxt.CheckedChanged += new System.EventHandler(this.radTxt_CheckedChanged);
            // 
            // cbxRemove1
            // 
            this.cbxRemove1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxRemove1.AutoSize = true;
            this.cbxRemove1.Location = new System.Drawing.Point(635, 391);
            this.cbxRemove1.Name = "cbxRemove1";
            this.cbxRemove1.Size = new System.Drawing.Size(60, 16);
            this.cbxRemove1.TabIndex = 12;
            this.cbxRemove1.Text = "移除\\n";
            this.cbxRemove1.UseVisualStyleBackColor = true;
            // 
            // txtProcess
            // 
            this.txtProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtProcess.Location = new System.Drawing.Point(257, 388);
            this.txtProcess.Name = "txtProcess";
            this.txtProcess.ReadOnly = true;
            this.txtProcess.Size = new System.Drawing.Size(57, 21);
            this.txtProcess.TabIndex = 11;
            this.txtProcess.Text = "0";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(198, 390);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "完成数目";
            // 
            // lbxDataFile
            // 
            this.lbxDataFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbxDataFile.FormattingEnabled = true;
            this.lbxDataFile.ItemHeight = 12;
            this.lbxDataFile.Location = new System.Drawing.Point(83, 42);
            this.lbxDataFile.Name = "lbxDataFile";
            this.lbxDataFile.Size = new System.Drawing.Size(797, 328);
            this.lbxDataFile.TabIndex = 8;
            // 
            // cbxFirst
            // 
            this.cbxFirst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxFirst.AutoSize = true;
            this.cbxFirst.Location = new System.Drawing.Point(714, 391);
            this.cbxFirst.Name = "cbxFirst";
            this.cbxFirst.Size = new System.Drawing.Size(72, 16);
            this.cbxFirst.TabIndex = 7;
            this.cbxFirst.Text = "移除首行";
            this.cbxFirst.UseVisualStyleBackColor = true;
            // 
            // btnMerge
            // 
            this.btnMerge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMerge.Location = new System.Drawing.Point(805, 386);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(75, 23);
            this.btnMerge.TabIndex = 6;
            this.btnMerge.Text = "合并";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // txtNum
            // 
            this.txtNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtNum.Location = new System.Drawing.Point(96, 387);
            this.txtNum.Name = "txtNum";
            this.txtNum.ReadOnly = true;
            this.txtNum.Size = new System.Drawing.Size(68, 21);
            this.txtNum.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 390);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "文件数目";
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectFolder.Location = new System.Drawing.Point(818, 13);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(62, 23);
            this.btnSelectFolder.TabIndex = 2;
            this.btnSelectFolder.Text = "选择";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(83, 14);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(716, 21);
            this.txtPath.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "数据路径";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.tabPage3.Controls.Add(this.btnFolder);
            this.tabPage3.Controls.Add(this.btnFindFile);
            this.tabPage3.Controls.Add(this.btnToUpper);
            this.tabPage3.Controls.Add(this.btnToLower);
            this.tabPage3.Controls.Add(this.btnCopyPath);
            this.tabPage3.Controls.Add(this.btnRename);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.btnSelect4);
            this.tabPage3.Controls.Add(this.txtFileSource);
            this.tabPage3.Controls.Add(this.txtContent3);
            this.tabPage3.Controls.Add(this.btnDelete3);
            this.tabPage3.Controls.Add(this.btnMove3);
            this.tabPage3.Controls.Add(this.txtProcess3);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.btnCopy3);
            this.tabPage3.Controls.Add(this.txtNum3);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.btnSelect3);
            this.tabPage3.Controls.Add(this.txtFile3);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(903, 427);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "文件操作";
            // 
            // btnFolder
            // 
            this.btnFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFolder.Location = new System.Drawing.Point(596, 395);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(75, 23);
            this.btnFolder.TabIndex = 29;
            this.btnFolder.Text = "文件分目录";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // btnFindFile
            // 
            this.btnFindFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindFile.Location = new System.Drawing.Point(288, 395);
            this.btnFindFile.Name = "btnFindFile";
            this.btnFindFile.Size = new System.Drawing.Size(70, 23);
            this.btnFindFile.TabIndex = 28;
            this.btnFindFile.Text = "搜索文件";
            this.btnFindFile.UseVisualStyleBackColor = true;
            this.btnFindFile.Click += new System.EventHandler(this.btnFindFile_Click);
            // 
            // btnToUpper
            // 
            this.btnToUpper.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToUpper.Location = new System.Drawing.Point(445, 395);
            this.btnToUpper.Name = "btnToUpper";
            this.btnToUpper.Size = new System.Drawing.Size(75, 23);
            this.btnToUpper.TabIndex = 27;
            this.btnToUpper.Text = "小写转大写";
            this.btnToUpper.UseVisualStyleBackColor = true;
            this.btnToUpper.Click += new System.EventHandler(this.btnToUpper_Click);
            // 
            // btnToLower
            // 
            this.btnToLower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToLower.Location = new System.Drawing.Point(364, 395);
            this.btnToLower.Name = "btnToLower";
            this.btnToLower.Size = new System.Drawing.Size(75, 23);
            this.btnToLower.TabIndex = 26;
            this.btnToLower.Text = "大写转小写";
            this.btnToLower.UseVisualStyleBackColor = true;
            this.btnToLower.Click += new System.EventHandler(this.btnToLower_Click);
            // 
            // btnCopyPath
            // 
            this.btnCopyPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyPath.Location = new System.Drawing.Point(526, 395);
            this.btnCopyPath.Name = "btnCopyPath";
            this.btnCopyPath.Size = new System.Drawing.Size(64, 23);
            this.btnCopyPath.TabIndex = 25;
            this.btnCopyPath.Text = "文件目录";
            this.btnCopyPath.UseVisualStyleBackColor = true;
            this.btnCopyPath.Click += new System.EventHandler(this.btnCopyPath_Click);
            // 
            // btnRename
            // 
            this.btnRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRename.Location = new System.Drawing.Point(830, 395);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(45, 23);
            this.btnRename.TabIndex = 24;
            this.btnRename.Text = "改名";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(27, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 23;
            this.label9.Text = "搜索目录";
            // 
            // btnSelect4
            // 
            this.btnSelect4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect4.Location = new System.Drawing.Point(813, 54);
            this.btnSelect4.Name = "btnSelect4";
            this.btnSelect4.Size = new System.Drawing.Size(62, 23);
            this.btnSelect4.TabIndex = 22;
            this.btnSelect4.Text = "选择";
            this.btnSelect4.UseVisualStyleBackColor = true;
            this.btnSelect4.Click += new System.EventHandler(this.btnSelect4_Click);
            // 
            // txtFileSource
            // 
            this.txtFileSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileSource.Location = new System.Drawing.Point(87, 55);
            this.txtFileSource.Name = "txtFileSource";
            this.txtFileSource.Size = new System.Drawing.Size(718, 21);
            this.txtFileSource.TabIndex = 21;
            // 
            // txtContent3
            // 
            this.txtContent3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContent3.BackColor = System.Drawing.Color.White;
            this.txtContent3.Location = new System.Drawing.Point(87, 92);
            this.txtContent3.MaxLength = 3276700;
            this.txtContent3.Multiline = true;
            this.txtContent3.Name = "txtContent3";
            this.txtContent3.ReadOnly = true;
            this.txtContent3.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtContent3.Size = new System.Drawing.Size(788, 287);
            this.txtContent3.TabIndex = 20;
            this.txtContent3.WordWrap = false;
            // 
            // btnDelete3
            // 
            this.btnDelete3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete3.Location = new System.Drawing.Point(779, 395);
            this.btnDelete3.Name = "btnDelete3";
            this.btnDelete3.Size = new System.Drawing.Size(45, 23);
            this.btnDelete3.TabIndex = 19;
            this.btnDelete3.Text = "删除";
            this.btnDelete3.UseVisualStyleBackColor = true;
            this.btnDelete3.Click += new System.EventHandler(this.btnDelete3_Click);
            // 
            // btnMove3
            // 
            this.btnMove3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMove3.Location = new System.Drawing.Point(728, 395);
            this.btnMove3.Name = "btnMove3";
            this.btnMove3.Size = new System.Drawing.Size(45, 23);
            this.btnMove3.TabIndex = 18;
            this.btnMove3.Text = "移动";
            this.btnMove3.UseVisualStyleBackColor = true;
            this.btnMove3.Click += new System.EventHandler(this.btnMove3_Click);
            // 
            // txtProcess3
            // 
            this.txtProcess3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtProcess3.Location = new System.Drawing.Point(218, 397);
            this.txtProcess3.Name = "txtProcess3";
            this.txtProcess3.ReadOnly = true;
            this.txtProcess3.Size = new System.Drawing.Size(57, 21);
            this.txtProcess3.TabIndex = 17;
            this.txtProcess3.Text = "0";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(156, 399);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "完成数目";
            // 
            // btnCopy3
            // 
            this.btnCopy3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy3.Location = new System.Drawing.Point(677, 395);
            this.btnCopy3.Name = "btnCopy3";
            this.btnCopy3.Size = new System.Drawing.Size(45, 23);
            this.btnCopy3.TabIndex = 14;
            this.btnCopy3.Text = "复制";
            this.btnCopy3.UseVisualStyleBackColor = true;
            this.btnCopy3.Click += new System.EventHandler(this.btnCopy3_Click);
            // 
            // txtNum3
            // 
            this.txtNum3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtNum3.Location = new System.Drawing.Point(87, 396);
            this.txtNum3.Name = "txtNum3";
            this.txtNum3.ReadOnly = true;
            this.txtNum3.Size = new System.Drawing.Size(50, 21);
            this.txtNum3.TabIndex = 13;
            this.txtNum3.Text = "0";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(27, 399);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 12;
            this.label8.Text = "文件数目";
            // 
            // btnSelect3
            // 
            this.btnSelect3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect3.Location = new System.Drawing.Point(813, 22);
            this.btnSelect3.Name = "btnSelect3";
            this.btnSelect3.Size = new System.Drawing.Size(62, 23);
            this.btnSelect3.TabIndex = 5;
            this.btnSelect3.Text = "选择";
            this.btnSelect3.UseVisualStyleBackColor = true;
            this.btnSelect3.Click += new System.EventHandler(this.btnSelect3_Click);
            // 
            // txtFile3
            // 
            this.txtFile3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile3.Location = new System.Drawing.Point(87, 23);
            this.txtFile3.Name = "txtFile3";
            this.txtFile3.Size = new System.Drawing.Size(718, 21);
            this.txtFile3.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "文件名称";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 447);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Text 文本查看器";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.CheckBox cbxTable;
        private System.Windows.Forms.Label lblSplit;
        private System.Windows.Forms.TextBox txtLine;
        private System.Windows.Forms.Button btnSplit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtALL;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbxWrap;
        private System.Windows.Forms.CheckBox cbxRepeat;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox cbxFirst;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.TextBox txtNum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lbxDataFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtProcess;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnDelete3;
        private System.Windows.Forms.Button btnMove3;
        private System.Windows.Forms.TextBox txtProcess3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnCopy3;
        private System.Windows.Forms.TextBox txtNum3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnSelect3;
        private System.Windows.Forms.TextBox txtFile3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtContent3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnSelect4;
        private System.Windows.Forms.TextBox txtFileSource;
        private System.Windows.Forms.CheckBox cbxRemove1;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.RadioButton radExcel;
        private System.Windows.Forms.RadioButton radTxt;
        private System.Windows.Forms.Label lblTip;
        private System.Windows.Forms.Button btnCopyPath;
        private System.Windows.Forms.Button btnToUpper;
        private System.Windows.Forms.Button btnToLower;
        private System.Windows.Forms.Button btmSearch;
        private System.Windows.Forms.Label lblTip1;
        private System.Windows.Forms.Button btnGroup;
        private System.Windows.Forms.Button btnFindFile;
        private System.Windows.Forms.Button btnFolder;
    }
}

