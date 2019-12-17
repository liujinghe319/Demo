
namespace LjhTools.AutoCarCatch
{
    partial class CatchQuanna
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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnCatch1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCatch2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(71, 230);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 12);
            this.label4.TabIndex = 43;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(71, 200);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(275, 12);
            this.label3.TabIndex = 42;
            this.label3.Text = "提示：文本框可输入一个或多个url（车信息页面）";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(80, 230);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 12);
            this.label2.TabIndex = 41;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(63, 15);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(519, 164);
            this.textBox1.TabIndex = 40;
            this.textBox1.Text = "http://www.china2car.com/268/14218/db/";
            // 
            // btnCatch1
            // 
            this.btnCatch1.Location = new System.Drawing.Point(374, 196);
            this.btnCatch1.Name = "btnCatch1";
            this.btnCatch1.Size = new System.Drawing.Size(96, 23);
            this.btnCatch1.TabIndex = 39;
            this.btnCatch1.Text = "全纳网采集";
            this.btnCatch1.UseVisualStyleBackColor = true;
            this.btnCatch1.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 38;
            this.label1.Text = "网址";
            // 
            // btnCatch2
            // 
            this.btnCatch2.Location = new System.Drawing.Point(486, 196);
            this.btnCatch2.Name = "btnCatch2";
            this.btnCatch2.Size = new System.Drawing.Size(96, 23);
            this.btnCatch2.TabIndex = 44;
            this.btnCatch2.Text = "第一车网采集";
            this.btnCatch2.UseVisualStyleBackColor = true;
            this.btnCatch2.Click += new System.EventHandler(this.btnCatch2_Click);
            // 
            // CatchQuanna2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 263);
            this.Controls.Add(this.btnCatch2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnCatch1);
            this.Controls.Add(this.label1);
            this.Name = "CatchQuanna2";
            this.Text = "CatchQuanna2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CatchQuanna2_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnCatch1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCatch2;
    }
}