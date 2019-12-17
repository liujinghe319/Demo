namespace LjhTools.AutoCarCatch
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAutoHome = new System.Windows.Forms.Button();
            this.btnYiChe = new System.Windows.Forms.Button();
            this.btnQuanNa = new System.Windows.Forms.Button();
            this.btnKaChe = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAutoHome
            // 
            this.btnAutoHome.Location = new System.Drawing.Point(34, 46);
            this.btnAutoHome.Name = "btnAutoHome";
            this.btnAutoHome.Size = new System.Drawing.Size(132, 23);
            this.btnAutoHome.TabIndex = 0;
            this.btnAutoHome.Text = "汽车之家+太平洋";
            this.btnAutoHome.UseVisualStyleBackColor = true;
            this.btnAutoHome.Click += new System.EventHandler(this.btnAutoHome_Click);
            // 
            // btnYiChe
            // 
            this.btnYiChe.Location = new System.Drawing.Point(201, 46);
            this.btnYiChe.Name = "btnYiChe";
            this.btnYiChe.Size = new System.Drawing.Size(136, 23);
            this.btnYiChe.TabIndex = 1;
            this.btnYiChe.Text = "易车网";
            this.btnYiChe.UseVisualStyleBackColor = true;
            this.btnYiChe.Click += new System.EventHandler(this.btnYiChe_Click);
            // 
            // btnQuanNa
            // 
            this.btnQuanNa.Location = new System.Drawing.Point(34, 105);
            this.btnQuanNa.Name = "btnQuanNa";
            this.btnQuanNa.Size = new System.Drawing.Size(132, 23);
            this.btnQuanNa.TabIndex = 2;
            this.btnQuanNa.Text = "全纳网";
            this.btnQuanNa.UseVisualStyleBackColor = true;
            this.btnQuanNa.Click += new System.EventHandler(this.btnQuanNa_Click);
            // 
            // btnKaChe
            // 
            this.btnKaChe.Location = new System.Drawing.Point(201, 105);
            this.btnKaChe.Name = "btnKaChe";
            this.btnKaChe.Size = new System.Drawing.Size(136, 23);
            this.btnKaChe.TabIndex = 3;
            this.btnKaChe.Text = "卡车之家";
            this.btnKaChe.UseVisualStyleBackColor = true;
            this.btnKaChe.Click += new System.EventHandler(this.btnKaChe_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 241);
            this.Controls.Add(this.btnKaChe);
            this.Controls.Add(this.btnQuanNa);
            this.Controls.Add(this.btnYiChe);
            this.Controls.Add(this.btnAutoHome);
            this.Name = "Form1";
            this.Text = "车型配置表采集";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAutoHome;
        private System.Windows.Forms.Button btnYiChe;
        private System.Windows.Forms.Button btnQuanNa;
        private System.Windows.Forms.Button btnKaChe;
    }
}

