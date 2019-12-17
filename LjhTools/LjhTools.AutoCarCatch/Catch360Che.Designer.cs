namespace LjhTools.AutoCarCatch
{
    partial class Catch360Che
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
            this.btnCatchConfig = new System.Windows.Forms.Button();
            this.txtLinks = new System.Windows.Forms.TextBox();
            this.lblTip = new System.Windows.Forms.Label();
            this.btnAllBrands = new System.Windows.Forms.Button();
            this.btnConfig = new System.Windows.Forms.Button();
            this.btnCarDetail = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCatchConfig
            // 
            this.btnCatchConfig.Location = new System.Drawing.Point(36, 23);
            this.btnCatchConfig.Name = "btnCatchConfig";
            this.btnCatchConfig.Size = new System.Drawing.Size(114, 23);
            this.btnCatchConfig.TabIndex = 0;
            this.btnCatchConfig.Text = "采集对比页面配置";
            this.btnCatchConfig.UseVisualStyleBackColor = true;
            this.btnCatchConfig.Click += new System.EventHandler(this.btnCatchConfig_Click);
            // 
            // txtLinks
            // 
            this.txtLinks.Location = new System.Drawing.Point(36, 70);
            this.txtLinks.MaxLength = 3276700;
            this.txtLinks.Multiline = true;
            this.txtLinks.Name = "txtLinks";
            this.txtLinks.Size = new System.Drawing.Size(634, 291);
            this.txtLinks.TabIndex = 1;
            this.txtLinks.Text = "https://product.360che.com/m192/48186_param.html";
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.Location = new System.Drawing.Point(38, 379);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(41, 12);
            this.lblTip.TabIndex = 2;
            this.lblTip.Text = "lblTip";
            // 
            // btnAllBrands
            // 
            this.btnAllBrands.Location = new System.Drawing.Point(164, 23);
            this.btnAllBrands.Name = "btnAllBrands";
            this.btnAllBrands.Size = new System.Drawing.Size(118, 23);
            this.btnAllBrands.TabIndex = 3;
            this.btnAllBrands.Text = "采集品牌大全首页";
            this.btnAllBrands.UseVisualStyleBackColor = true;
            this.btnAllBrands.Click += new System.EventHandler(this.btnAllBrands_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.Location = new System.Drawing.Point(297, 23);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(114, 23);
            this.btnConfig.TabIndex = 4;
            this.btnConfig.Text = "采集车系页链接";
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // btnCarDetail
            // 
            this.btnCarDetail.Location = new System.Drawing.Point(427, 23);
            this.btnCarDetail.Name = "btnCarDetail";
            this.btnCarDetail.Size = new System.Drawing.Size(114, 23);
            this.btnCarDetail.TabIndex = 5;
            this.btnCarDetail.Text = "采集配置页链接";
            this.btnCarDetail.UseVisualStyleBackColor = true;
            this.btnCarDetail.Click += new System.EventHandler(this.btnCarDetail_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(556, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "采集配置页详情";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Catch360Che
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 405);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCarDetail);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.btnAllBrands);
            this.Controls.Add(this.lblTip);
            this.Controls.Add(this.txtLinks);
            this.Controls.Add(this.btnCatchConfig);
            this.Name = "Catch360Che";
            this.Text = "Catch360Che";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCatchConfig;
        private System.Windows.Forms.TextBox txtLinks;
        private System.Windows.Forms.Label lblTip;
        private System.Windows.Forms.Button btnAllBrands;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.Button btnCarDetail;
        private System.Windows.Forms.Button button1;
    }
}