namespace Fetion.Practice.ChatHelper
{
    partial class FrmMain
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
            this.btnGetWindowHandler = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtWindowName = new System.Windows.Forms.TextBox();
            this.btnStopListen = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRefreshInterval = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnGetWindowHandler
            // 
            this.btnGetWindowHandler.Location = new System.Drawing.Point(83, 150);
            this.btnGetWindowHandler.Name = "btnGetWindowHandler";
            this.btnGetWindowHandler.Size = new System.Drawing.Size(91, 23);
            this.btnGetWindowHandler.TabIndex = 0;
            this.btnGetWindowHandler.Text = "监听聊天信息";
            this.btnGetWindowHandler.UseVisualStyleBackColor = true;
            this.btnGetWindowHandler.Click += new System.EventHandler(this.btnGetWindowHandler_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "窗口名称";
            // 
            // txtWindowName
            // 
            this.txtWindowName.Location = new System.Drawing.Point(83, 19);
            this.txtWindowName.Name = "txtWindowName";
            this.txtWindowName.Size = new System.Drawing.Size(178, 21);
            this.txtWindowName.TabIndex = 2;
            // 
            // btnStopListen
            // 
            this.btnStopListen.Enabled = false;
            this.btnStopListen.Location = new System.Drawing.Point(186, 150);
            this.btnStopListen.Name = "btnStopListen";
            this.btnStopListen.Size = new System.Drawing.Size(75, 23);
            this.btnStopListen.TabIndex = 5;
            this.btnStopListen.Text = "停止监听";
            this.btnStopListen.UseVisualStyleBackColor = true;
            this.btnStopListen.Click += new System.EventHandler(this.btnStopListen_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "刷新时间间隔(秒)";
            // 
            // txtRefreshInterval
            // 
            this.txtRefreshInterval.Location = new System.Drawing.Point(161, 49);
            this.txtRefreshInterval.Name = "txtRefreshInterval";
            this.txtRefreshInterval.Size = new System.Drawing.Size(100, 21);
            this.txtRefreshInterval.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "监听关键词";
            // 
            // txtKeyword
            // 
            this.txtKeyword.Location = new System.Drawing.Point(83, 82);
            this.txtKeyword.Multiline = true;
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(178, 49);
            this.txtKeyword.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "\";\"隔开";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(472, 184);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(319, 22);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(228, 151);
            this.txtResult.TabIndex = 13;
            this.txtResult.Text = "";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 219);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtKeyword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRefreshInterval);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnStopListen);
            this.Controls.Add(this.txtWindowName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGetWindowHandler);
            this.Name = "FrmMain";
            this.Text = "飞信外挂V0.1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetWindowHandler;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtWindowName;
        private System.Windows.Forms.Button btnStopListen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRefreshInterval;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.RichTextBox txtResult;
    }
}

