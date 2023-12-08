namespace 计时累加器
{
    partial class 计时器
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.start = new System.Windows.Forms.Button();
            this.end = new System.Windows.Forms.Button();
            this.MIAO = new System.Windows.Forms.Timer(this.components);
            this.show = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(76, 62);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(75, 23);
            this.start.TabIndex = 0;
            this.start.Text = "计时开始";
            this.start.UseVisualStyleBackColor = true;
            // 
            // end
            // 
            this.end.Location = new System.Drawing.Point(226, 62);
            this.end.Name = "end";
            this.end.Size = new System.Drawing.Size(75, 23);
            this.end.TabIndex = 1;
            this.end.Text = "计时结束";
            this.end.UseVisualStyleBackColor = true;
            // 
            // MIAO
            // 
            this.MIAO.Tick += new System.EventHandler(this.MIAO_Tick);
            // 
            // show
            // 
            this.show.AutoSize = true;
            this.show.Location = new System.Drawing.Point(151, 164);
            this.show.Name = "show";
            this.show.Size = new System.Drawing.Size(63, 14);
            this.show.TabIndex = 2;
            this.show.Text = "计时显示";
            // 
            // 计时器
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 450);
            this.Controls.Add(this.show);
            this.Controls.Add(this.end);
            this.Controls.Add(this.start);
            this.Name = "计时器";
            this.Text = "计时累加器";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button end;
        private System.Windows.Forms.Timer MIAO;
        private System.Windows.Forms.Label show;
    }
}

