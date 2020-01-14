using System;
namespace Project1
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    partial class FRM_CKSI0110M
    {
        #region "Windows フォーム デザイナによって生成されたコード "
        [System.Diagnostics.DebuggerNonUserCode()]
        public FRM_CKSI0110M()
            : base()
        {
            Load += FRM_CKSI0110M_Load;
            //この呼び出しは、Windows フォーム デザイナで必要です。
            InitializeComponent();
        }
        //Form は、コンポーネント一覧に後処理を実行するために dispose をオーバーライドします。
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool Disposing)
        {
            if (Disposing)
            {
                if ((components != null))
                {
                    components.Dispose();
                }
            }
            base.Dispose(Disposing);
        }
        //Windows フォーム デザイナで必要です。
        private System.ComponentModel.IContainer components;
        public System.Windows.Forms.ToolTip ToolTip1;
        public System.Windows.Forms.Button _CMD_BUTTON_2;
        private System.Windows.Forms.TextBox TXT_GYOSYACD;
        public System.Windows.Forms.Label LBL_GYOSYANM;
        public System.Windows.Forms.Label Label7;
        public System.Windows.Forms.Label LBL_TUKI;
        public System.Windows.Forms.Label LBL_NEN;
        public System.Windows.Forms.Label Label2;
        public System.Windows.Forms.Label Label1;
        public System.Windows.Forms.Label _Label13_17;
        public System.Windows.Forms.Label _Label13_16;
        public System.Windows.Forms.GroupBox Frame1;
        public System.Windows.Forms.Button _CMD_BUTTON_1;
        public System.Windows.Forms.Button _CMD_BUTTON_0;
        private System.Windows.Forms.Timer TMR_Timer;
        public System.Windows.Forms.Label Label41;
        public Microsoft.VisualBasic.Compatibility.VB6.ButtonArray CMD_BUTTON;
        public Microsoft.VisualBasic.Compatibility.VB6.LabelArray Label13;
        //メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
        //Windows フォーム デザイナを使って変更できます。
        //コード エディタを使用して、変更しないでください。
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._CMD_BUTTON_2 = new System.Windows.Forms.Button();
            this.Frame1 = new System.Windows.Forms.GroupBox();
            this.TXT_GYOSYACD = new System.Windows.Forms.TextBox();
            this.LBL_GYOSYANM = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.LBL_TUKI = new System.Windows.Forms.Label();
            this.LBL_NEN = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this._Label13_17 = new System.Windows.Forms.Label();
            this._Label13_16 = new System.Windows.Forms.Label();
            this._CMD_BUTTON_1 = new System.Windows.Forms.Button();
            this._CMD_BUTTON_0 = new System.Windows.Forms.Button();
            this.TMR_Timer = new System.Windows.Forms.Timer(this.components);
            this.Label41 = new System.Windows.Forms.Label();
            this.CMD_BUTTON = new Microsoft.VisualBasic.Compatibility.VB6.ButtonArray(this.components);
            this.Label13 = new Microsoft.VisualBasic.Compatibility.VB6.LabelArray(this.components);
            this.Frame1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CMD_BUTTON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Label13)).BeginInit();
            this.SuspendLayout();
            // 
            // _CMD_BUTTON_2
            // 
            this._CMD_BUTTON_2.BackColor = System.Drawing.SystemColors.Control;
            this._CMD_BUTTON_2.Cursor = System.Windows.Forms.Cursors.Default;
            this._CMD_BUTTON_2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CMD_BUTTON.SetIndex(this._CMD_BUTTON_2, ((short)(2)));
            this._CMD_BUTTON_2.Location = new System.Drawing.Point(564, 507);
            this._CMD_BUTTON_2.Name = "_CMD_BUTTON_2";
            this._CMD_BUTTON_2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._CMD_BUTTON_2.Size = new System.Drawing.Size(97, 33);
            this._CMD_BUTTON_2.TabIndex = 13;
            this._CMD_BUTTON_2.Text = "発注№採番";
            this._CMD_BUTTON_2.UseVisualStyleBackColor = false;
            // 
            // Frame1
            // 
            this.Frame1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Frame1.Controls.Add(this.TXT_GYOSYACD);
            this.Frame1.Controls.Add(this.LBL_GYOSYANM);
            this.Frame1.Controls.Add(this.Label7);
            this.Frame1.Controls.Add(this.LBL_TUKI);
            this.Frame1.Controls.Add(this.LBL_NEN);
            this.Frame1.Controls.Add(this.Label2);
            this.Frame1.Controls.Add(this.Label1);
            this.Frame1.Controls.Add(this._Label13_17);
            this.Frame1.Controls.Add(this._Label13_16);
            this.Frame1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Frame1.Location = new System.Drawing.Point(128, 116);
            this.Frame1.Name = "Frame1";
            this.Frame1.Padding = new System.Windows.Forms.Padding(0);
            this.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame1.Size = new System.Drawing.Size(541, 237);
            this.Frame1.TabIndex = 4;
            this.Frame1.TabStop = false;
            // 
            // TXT_GYOSYACD
            // 
            this.TXT_GYOSYACD.AcceptsReturn = true;
            this.TXT_GYOSYACD.BackColor = System.Drawing.SystemColors.Window;
            this.TXT_GYOSYACD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TXT_GYOSYACD.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TXT_GYOSYACD.ForeColor = System.Drawing.SystemColors.WindowText;
            this.TXT_GYOSYACD.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.TXT_GYOSYACD.Location = new System.Drawing.Point(120, 116);
            this.TXT_GYOSYACD.MaxLength = 4;
            this.TXT_GYOSYACD.Name = "TXT_GYOSYACD";
            this.TXT_GYOSYACD.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TXT_GYOSYACD.Size = new System.Drawing.Size(37, 23);
            this.TXT_GYOSYACD.TabIndex = 0;
            this.TXT_GYOSYACD.Leave += new System.EventHandler(this.TXT_GYOSYACD_Leave);
            this.TXT_GYOSYACD.Enter += new System.EventHandler(this.TXT_GYOSYACD_Enter);
            // 
            // LBL_GYOSYANM
            // 
            this.LBL_GYOSYANM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.LBL_GYOSYANM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBL_GYOSYANM.Cursor = System.Windows.Forms.Cursors.Default;
            this.LBL_GYOSYANM.ForeColor = System.Drawing.SystemColors.WindowText;
            this.LBL_GYOSYANM.Location = new System.Drawing.Point(168, 116);
            this.LBL_GYOSYANM.Name = "LBL_GYOSYANM";
            this.LBL_GYOSYANM.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LBL_GYOSYANM.Size = new System.Drawing.Size(325, 25);
            this.LBL_GYOSYANM.TabIndex = 12;
            // 
            // Label7
            // 
            this.Label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label7.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label7.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label7.Location = new System.Drawing.Point(64, 120);
            this.Label7.Name = "Label7";
            this.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label7.Size = new System.Drawing.Size(50, 21);
            this.Label7.TabIndex = 11;
            this.Label7.Text = "業者";
            // 
            // LBL_TUKI
            // 
            this.LBL_TUKI.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.LBL_TUKI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBL_TUKI.Cursor = System.Windows.Forms.Cursors.Default;
            this.LBL_TUKI.ForeColor = System.Drawing.SystemColors.WindowText;
            this.LBL_TUKI.Location = new System.Drawing.Point(196, 64);
            this.LBL_TUKI.Name = "LBL_TUKI";
            this.LBL_TUKI.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LBL_TUKI.Size = new System.Drawing.Size(28, 25);
            this.LBL_TUKI.TabIndex = 10;
            // 
            // LBL_NEN
            // 
            this.LBL_NEN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.LBL_NEN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBL_NEN.Cursor = System.Windows.Forms.Cursors.Default;
            this.LBL_NEN.ForeColor = System.Drawing.SystemColors.WindowText;
            this.LBL_NEN.Location = new System.Drawing.Point(119, 64);
            this.LBL_NEN.Name = "LBL_NEN";
            this.LBL_NEN.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LBL_NEN.Size = new System.Drawing.Size(43, 25);
            this.LBL_NEN.TabIndex = 9;
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label2.Location = new System.Drawing.Point(116, 180);
            this.Label2.Name = "Label2";
            this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label2.Size = new System.Drawing.Size(296, 20);
            this.Label2.TabIndex = 8;
            this.Label2.Text = "注文一覧表を出力します。";
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label1.Location = new System.Drawing.Point(33, 68);
            this.Label1.Name = "Label1";
            this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label1.Size = new System.Drawing.Size(86, 19);
            this.Label1.TabIndex = 7;
            this.Label1.Text = "検収年月";
            // 
            // _Label13_17
            // 
            this._Label13_17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._Label13_17.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label13_17.ForeColor = System.Drawing.SystemColors.WindowText;
            this._Label13_17.Location = new System.Drawing.Point(168, 70);
            this._Label13_17.Name = "_Label13_17";
            this._Label13_17.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label13_17.Size = new System.Drawing.Size(17, 17);
            this._Label13_17.TabIndex = 6;
            this._Label13_17.Text = "年";
            // 
            // _Label13_16
            // 
            this._Label13_16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._Label13_16.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label13_16.ForeColor = System.Drawing.SystemColors.WindowText;
            this._Label13_16.Location = new System.Drawing.Point(232, 70);
            this._Label13_16.Name = "_Label13_16";
            this._Label13_16.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label13_16.Size = new System.Drawing.Size(17, 17);
            this._Label13_16.TabIndex = 5;
            this._Label13_16.Text = "月";
            // 
            // _CMD_BUTTON_1
            // 
            this._CMD_BUTTON_1.BackColor = System.Drawing.SystemColors.Control;
            this._CMD_BUTTON_1.Cursor = System.Windows.Forms.Cursors.Default;
            this._CMD_BUTTON_1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CMD_BUTTON.SetIndex(this._CMD_BUTTON_1, ((short)(1)));
            this._CMD_BUTTON_1.Location = new System.Drawing.Point(680, 507);
            this._CMD_BUTTON_1.Name = "_CMD_BUTTON_1";
            this._CMD_BUTTON_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._CMD_BUTTON_1.Size = new System.Drawing.Size(97, 33);
            this._CMD_BUTTON_1.TabIndex = 1;
            this._CMD_BUTTON_1.Text = "印刷";
            this._CMD_BUTTON_1.UseVisualStyleBackColor = false;
            // 
            // _CMD_BUTTON_0
            // 
            this._CMD_BUTTON_0.BackColor = System.Drawing.SystemColors.Control;
            this._CMD_BUTTON_0.Cursor = System.Windows.Forms.Cursors.Default;
            this._CMD_BUTTON_0.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CMD_BUTTON.SetIndex(this._CMD_BUTTON_0, ((short)(0)));
            this._CMD_BUTTON_0.Location = new System.Drawing.Point(16, 507);
            this._CMD_BUTTON_0.Name = "_CMD_BUTTON_0";
            this._CMD_BUTTON_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._CMD_BUTTON_0.Size = new System.Drawing.Size(97, 33);
            this._CMD_BUTTON_0.TabIndex = 2;
            this._CMD_BUTTON_0.Text = "閉じる";
            this._CMD_BUTTON_0.UseVisualStyleBackColor = false;
            // 
            // TMR_Timer
            // 
            this.TMR_Timer.Enabled = true;
            this.TMR_Timer.Interval = 1000;
            this.TMR_Timer.Tick += new System.EventHandler(this.TMR_TIMER_Tick);
            // 
            // Label41
            // 
            this.Label41.BackColor = System.Drawing.SystemColors.Control;
            this.Label41.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label41.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Label41.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label41.Location = new System.Drawing.Point(-56, 144);
            this.Label41.Name = "Label41";
            this.Label41.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label41.Size = new System.Drawing.Size(17, 21);
            this.Label41.TabIndex = 3;
            this.Label41.Text = "日";
            // 
            // FRM_CKSI0110M
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(794, 548);
            this.ControlBox = false;
            this.Controls.Add(this._CMD_BUTTON_2);
            this.Controls.Add(this.Frame1);
            this.Controls.Add(this._CMD_BUTTON_1);
            this.Controls.Add(this._CMD_BUTTON_0);
            this.Controls.Add(this.Label41);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(116, 91);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FRM_CKSI0110M";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "注文一覧表出力";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Frame1.ResumeLayout(false);
            this.Frame1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CMD_BUTTON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Label13)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

    }
}
