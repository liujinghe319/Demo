namespace LjhTools.AutoCarCatch
{
    partial class CatchSoHuMtn
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
            this.btnMtn = new System.Windows.Forms.Button();
            this.lblTip = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnMtn
            // 
            this.btnMtn.Location = new System.Drawing.Point(32, 24);
            this.btnMtn.Name = "btnMtn";
            this.btnMtn.Size = new System.Drawing.Size(121, 23);
            this.btnMtn.TabIndex = 1;
            this.btnMtn.Text = "采集保养信息";
            this.btnMtn.UseVisualStyleBackColor = true;
            this.btnMtn.Click += new System.EventHandler(this.btnMtn_Click);
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.Location = new System.Drawing.Point(34, 84);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(41, 12);
            this.lblTip.TabIndex = 2;
            this.lblTip.Text = "lblTip";
            // 
            // CatchSoHuMtn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 329);
            this.Controls.Add(this.lblTip);
            this.Controls.Add(this.btnMtn);
            this.Name = "CatchSoHuMtn";
            this.Text = "汽车之家保养信息";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMtn;
        private System.Windows.Forms.Label lblTip;
    }
}