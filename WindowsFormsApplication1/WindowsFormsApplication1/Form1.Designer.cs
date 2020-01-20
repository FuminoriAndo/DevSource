namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.NewComandExcel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NewComandExcel
            // 
            this.NewComandExcel.Location = new System.Drawing.Point(758, 110);
            this.NewComandExcel.Name = "NewComandExcel";
            this.NewComandExcel.Size = new System.Drawing.Size(75, 23);
            this.NewComandExcel.TabIndex = 0;
            this.NewComandExcel.Text = "新規";
            this.NewComandExcel.UseVisualStyleBackColor = true;
            this.NewComandExcel.Click += new System.EventHandler(this.NewComandExcel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 261);
            this.Controls.Add(this.NewComandExcel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button NewComandExcel;
    }
}

