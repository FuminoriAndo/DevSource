namespace ExcelCreatorBase
{
    partial class BaseWindow
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseWindow));
            this.progressControl1 = new UserControls.Control.ProgressControl();
            this.SuspendLayout();
            // 
            // progressControl1
            // 
            this.progressControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressControl1.Argment = null;
            this.progressControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.progressControl1.Location = new System.Drawing.Point(47, 74);
            this.progressControl1.Name = "progressControl1";
            this.progressControl1.Size = new System.Drawing.Size(503, 121);
            this.progressControl1.TabIndex = 0;
            this.progressControl1.RunWorkerCompleted += new UserControls.Control.ProgressControl.RunWorkerCompletedHandler(this.BackgroundWorker1_RunWorkerCompleted);
            this.progressControl1.DoWork += new UserControls.Control.ProgressControl.DoWorkHandler(this.BackgroundWorker1_DoWork);
            // 
            // BaseWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(606, 288);
            this.Controls.Add(this.progressControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BaseWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ベース画面";
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        protected UserControls.Control.ProgressControl progressControl1;

    }
}