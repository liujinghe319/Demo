namespace LjhTools.AutoCarCatch
{
    partial class AutoHomeDataFrm
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
            this.btnCatch = new System.Windows.Forms.Button();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.lblUrl = new System.Windows.Forms.Label();
            this.txtKeyWord = new System.Windows.Forms.TextBox();
            this.radAutoHome = new System.Windows.Forms.RadioButton();
            this.radPacific = new System.Windows.Forms.RadioButton();
            this.btnClear = new System.Windows.Forms.Button();
            this.radPhoneAutoHome = new System.Windows.Forms.RadioButton();
            this.lblTip = new System.Windows.Forms.Label();
            this.btn_GetCarSeries = new System.Windows.Forms.Button();
            this.btnAllCar = new System.Windows.Forms.Button();
            this.btnAutoCar = new System.Windows.Forms.Button();
            this.btnMtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCatch
            // 
            this.btnCatch.Location = new System.Drawing.Point(567, 51);
            this.btnCatch.Name = "btnCatch";
            this.btnCatch.Size = new System.Drawing.Size(75, 23);
            this.btnCatch.TabIndex = 0;
            this.btnCatch.Text = "抓取";
            this.btnCatch.UseVisualStyleBackColor = true;
            this.btnCatch.Click += new System.EventHandler(this.btnCatch_Click);
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(64, 53);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(495, 21);
            this.txtUrl.TabIndex = 1;
            this.txtUrl.Text = "https://car.m.autohome.com.cn/ashx/car/GetModelConfigNew2.ashx?seriesId=146&data=" +
    "20170919";
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(29, 58);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(23, 12);
            this.lblUrl.TabIndex = 2;
            this.lblUrl.Text = "URL";
            // 
            // txtKeyWord
            // 
            this.txtKeyWord.Location = new System.Drawing.Point(64, 92);
            this.txtKeyWord.Multiline = true;
            this.txtKeyWord.Name = "txtKeyWord";
            this.txtKeyWord.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtKeyWord.Size = new System.Drawing.Size(578, 386);
            this.txtKeyWord.TabIndex = 3;
            // 
            // radAutoHome
            // 
            this.radAutoHome.AutoSize = true;
            this.radAutoHome.Location = new System.Drawing.Point(64, 22);
            this.radAutoHome.Name = "radAutoHome";
            this.radAutoHome.Size = new System.Drawing.Size(71, 16);
            this.radAutoHome.TabIndex = 4;
            this.radAutoHome.Text = "汽车之家";
            this.radAutoHome.UseVisualStyleBackColor = true;
            // 
            // radPacific
            // 
            this.radPacific.AutoSize = true;
            this.radPacific.Location = new System.Drawing.Point(277, 22);
            this.radPacific.Name = "radPacific";
            this.radPacific.Size = new System.Drawing.Size(95, 16);
            this.radPacific.TabIndex = 5;
            this.radPacific.Text = "太平洋汽车网";
            this.radPacific.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(567, 19);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "清屏";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // radPhoneAutoHome
            // 
            this.radPhoneAutoHome.AutoSize = true;
            this.radPhoneAutoHome.Checked = true;
            this.radPhoneAutoHome.Location = new System.Drawing.Point(154, 22);
            this.radPhoneAutoHome.Name = "radPhoneAutoHome";
            this.radPhoneAutoHome.Size = new System.Drawing.Size(107, 16);
            this.radPhoneAutoHome.TabIndex = 7;
            this.radPhoneAutoHome.TabStop = true;
            this.radPhoneAutoHome.Text = "汽车之家手机版";
            this.radPhoneAutoHome.UseVisualStyleBackColor = true;
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.Location = new System.Drawing.Point(62, 501);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(41, 12);
            this.lblTip.TabIndex = 8;
            this.lblTip.Text = "label1";
            // 
            // btn_GetCarSeries
            // 
            this.btn_GetCarSeries.Location = new System.Drawing.Point(567, 496);
            this.btn_GetCarSeries.Name = "btn_GetCarSeries";
            this.btn_GetCarSeries.Size = new System.Drawing.Size(75, 23);
            this.btn_GetCarSeries.TabIndex = 9;
            this.btn_GetCarSeries.Text = "抓取车系ID";
            this.btn_GetCarSeries.UseVisualStyleBackColor = true;
            this.btn_GetCarSeries.Click += new System.EventHandler(this.btn_GetCarSeries_Click);
            // 
            // btnAllCar
            // 
            this.btnAllCar.Location = new System.Drawing.Point(64, 538);
            this.btnAllCar.Name = "btnAllCar";
            this.btnAllCar.Size = new System.Drawing.Size(109, 23);
            this.btnAllCar.TabIndex = 10;
            this.btnAllCar.Text = "抓取主机厂车系";
            this.btnAllCar.UseVisualStyleBackColor = true;
            this.btnAllCar.Click += new System.EventHandler(this.btnAllCar_Click);
            // 
            // btnAutoCar
            // 
            this.btnAutoCar.Location = new System.Drawing.Point(210, 538);
            this.btnAutoCar.Name = "btnAutoCar";
            this.btnAutoCar.Size = new System.Drawing.Size(109, 23);
            this.btnAutoCar.TabIndex = 11;
            this.btnAutoCar.Text = "抓取年款数据";
            this.btnAutoCar.UseVisualStyleBackColor = true;
            this.btnAutoCar.Click += new System.EventHandler(this.btnAutoCar_Click);
            // 
            // btnMtn
            // 
            this.btnMtn.Location = new System.Drawing.Point(354, 538);
            this.btnMtn.Name = "btnMtn";
            this.btnMtn.Size = new System.Drawing.Size(109, 23);
            this.btnMtn.TabIndex = 12;
            this.btnMtn.Text = "抓取年款保养数据";
            this.btnMtn.UseVisualStyleBackColor = true;
            this.btnMtn.Click += new System.EventHandler(this.btnMtn_Click);
            // 
            // AutoHomeDataFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 593);
            this.Controls.Add(this.btnMtn);
            this.Controls.Add(this.btnAutoCar);
            this.Controls.Add(this.btnAllCar);
            this.Controls.Add(this.btn_GetCarSeries);
            this.Controls.Add(this.lblTip);
            this.Controls.Add(this.radPhoneAutoHome);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.radPacific);
            this.Controls.Add(this.radAutoHome);
            this.Controls.Add(this.txtKeyWord);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.btnCatch);
            this.Name = "AutoHomeDataFrm";
            this.Text = "汽车配置表";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCatch;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.TextBox txtKeyWord;
        private System.Windows.Forms.RadioButton radAutoHome;
        private System.Windows.Forms.RadioButton radPacific;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.RadioButton radPhoneAutoHome;
        private System.Windows.Forms.Label lblTip;
        private System.Windows.Forms.Button btn_GetCarSeries;
        private System.Windows.Forms.Button btnAllCar;
        private System.Windows.Forms.Button btnAutoCar;
        private System.Windows.Forms.Button btnMtn;
    }
}