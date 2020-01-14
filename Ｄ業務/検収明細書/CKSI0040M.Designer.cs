using System;
namespace Project1
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    partial class FRM_CKSI0040M
    {
        #region "Windows フォーム デザイナによって生成されたコード "
        [System.Diagnostics.DebuggerNonUserCode()]
        public FRM_CKSI0040M()
            : base()
        {
            Load += FRM_CKSI0040M_Load;
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
        public System.Windows.Forms.TextBox TXT_NEN;
        public System.Windows.Forms.TextBox TXT_TUKI;
        public System.Windows.Forms.TextBox TXT_GYOSYACD;
        public System.Windows.Forms.Label LBL_GYOSYANM;
        public System.Windows.Forms.Label Label2;
        public System.Windows.Forms.Label Label1;
        public System.Windows.Forms.Label _Label13_17;
        public System.Windows.Forms.Label _Label13_16;
        public System.Windows.Forms.Label Label7;
        public System.Windows.Forms.GroupBox Frame1;
        public System.Windows.Forms.Button _CMD_Button_1;
        public System.Windows.Forms.Button _CMD_Button_0;
        public System.Windows.Forms.Timer TMR_Timer;
        public System.Windows.Forms.Label Label41;
        public Microsoft.VisualBasic.Compatibility.VB6.ButtonArray CMD_Button;
        public Microsoft.VisualBasic.Compatibility.VB6.LabelArray Label13;
        //メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
        //Windows フォーム デザイナを使って変更できます。
        //コード エディタを使用して、変更しないでください。
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRM_CKSI0040M));
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Frame1 = new System.Windows.Forms.GroupBox();
            this.TXT_NEN = new System.Windows.Forms.TextBox();
            this.TXT_TUKI = new System.Windows.Forms.TextBox();
            this.TXT_GYOSYACD = new System.Windows.Forms.TextBox();
            this.LBL_GYOSYANM = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this._Label13_17 = new System.Windows.Forms.Label();
            this._Label13_16 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this._CMD_Button_1 = new System.Windows.Forms.Button();
            this._CMD_Button_0 = new System.Windows.Forms.Button();
            this.TMR_Timer = new System.Windows.Forms.Timer(this.components);
            this.Label41 = new System.Windows.Forms.Label();
            this.CMD_Button = new Microsoft.VisualBasic.Compatibility.VB6.ButtonArray(this.components);
            this.Label13 = new Microsoft.VisualBasic.Compatibility.VB6.LabelArray(this.components);
            this.grpFaxSend = new System.Windows.Forms.GroupBox();
            this.rdoFaxNo = new System.Windows.Forms.RadioButton();
            this.rdoFaxYes = new System.Windows.Forms.RadioButton();
            this.LblFax = new System.Windows.Forms.Label();
            this.axMcRemote1 = new AxMCREMOTELib.AxMcRemote();
            this.Frame1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CMD_Button)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Label13)).BeginInit();
            this.grpFaxSend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMcRemote1)).BeginInit();
            this.SuspendLayout();
            // 
            // Frame1
            // 
            this.Frame1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Frame1.Controls.Add(this.grpFaxSend);
            this.Frame1.Controls.Add(this.TXT_NEN);
            this.Frame1.Controls.Add(this.TXT_TUKI);
            this.Frame1.Controls.Add(this.TXT_GYOSYACD);
            this.Frame1.Controls.Add(this.LBL_GYOSYANM);
            this.Frame1.Controls.Add(this.Label2);
            this.Frame1.Controls.Add(this.Label1);
            this.Frame1.Controls.Add(this._Label13_17);
            this.Frame1.Controls.Add(this._Label13_16);
            this.Frame1.Controls.Add(this.Label7);
            this.Frame1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Frame1.Location = new System.Drawing.Point(128, 116);
            this.Frame1.Name = "Frame1";
            this.Frame1.Padding = new System.Windows.Forms.Padding(0);
            this.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame1.Size = new System.Drawing.Size(541, 263);
            this.Frame1.TabIndex = 6;
            this.Frame1.TabStop = false;
            // 
            // TXT_NEN
            // 
            this.TXT_NEN.AcceptsReturn = true;
            this.TXT_NEN.BackColor = System.Drawing.SystemColors.Window;
            this.TXT_NEN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TXT_NEN.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TXT_NEN.ForeColor = System.Drawing.SystemColors.WindowText;
            this.TXT_NEN.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.TXT_NEN.Location = new System.Drawing.Point(126, 45);
            this.TXT_NEN.MaxLength = 4;
            this.TXT_NEN.Name = "TXT_NEN";
            this.TXT_NEN.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TXT_NEN.Size = new System.Drawing.Size(41, 23);
            this.TXT_NEN.TabIndex = 0;
            this.TXT_NEN.TextChanged += new System.EventHandler(this.TXT_NEN_TextChanged);
            this.TXT_NEN.Leave += new System.EventHandler(this.TXT_NEN_Leave);
            this.TXT_NEN.Enter += new System.EventHandler(this.TXT_NEN_Enter);
            // 
            // TXT_TUKI
            // 
            this.TXT_TUKI.AcceptsReturn = true;
            this.TXT_TUKI.BackColor = System.Drawing.SystemColors.Window;
            this.TXT_TUKI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TXT_TUKI.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TXT_TUKI.ForeColor = System.Drawing.SystemColors.WindowText;
            this.TXT_TUKI.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.TXT_TUKI.Location = new System.Drawing.Point(202, 45);
            this.TXT_TUKI.MaxLength = 2;
            this.TXT_TUKI.Name = "TXT_TUKI";
            this.TXT_TUKI.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TXT_TUKI.Size = new System.Drawing.Size(28, 23);
            this.TXT_TUKI.TabIndex = 1;
            this.TXT_TUKI.Leave += new System.EventHandler(this.TXT_TUKI_Leave);
            this.TXT_TUKI.Enter += new System.EventHandler(this.TXT_TUKI_Enter);
            // 
            // TXT_GYOSYACD
            // 
            this.TXT_GYOSYACD.AcceptsReturn = true;
            this.TXT_GYOSYACD.BackColor = System.Drawing.SystemColors.Window;
            this.TXT_GYOSYACD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TXT_GYOSYACD.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TXT_GYOSYACD.ForeColor = System.Drawing.SystemColors.WindowText;
            this.TXT_GYOSYACD.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.TXT_GYOSYACD.Location = new System.Drawing.Point(126, 108);
            this.TXT_GYOSYACD.MaxLength = 4;
            this.TXT_GYOSYACD.Name = "TXT_GYOSYACD";
            this.TXT_GYOSYACD.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TXT_GYOSYACD.Size = new System.Drawing.Size(37, 23);
            this.TXT_GYOSYACD.TabIndex = 2;
            this.TXT_GYOSYACD.Leave += new System.EventHandler(this.TXT_GYOSYACD_Leave);
            this.TXT_GYOSYACD.Enter += new System.EventHandler(this.TXT_GYOSYACD_Enter);
            // 
            // LBL_GYOSYANM
            // 
            this.LBL_GYOSYANM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.LBL_GYOSYANM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBL_GYOSYANM.Cursor = System.Windows.Forms.Cursors.Default;
            this.LBL_GYOSYANM.ForeColor = System.Drawing.SystemColors.WindowText;
            this.LBL_GYOSYANM.Location = new System.Drawing.Point(172, 108);
            this.LBL_GYOSYANM.Name = "LBL_GYOSYANM";
            this.LBL_GYOSYANM.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LBL_GYOSYANM.Size = new System.Drawing.Size(330, 23);
            this.LBL_GYOSYANM.TabIndex = 12;
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label2.Location = new System.Drawing.Point(124, 176);
            this.Label2.Name = "Label2";
            this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label2.Size = new System.Drawing.Size(220, 16);
            this.Label2.TabIndex = 11;
            this.Label2.Text = "検収明細書を作成します。";
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label1.Location = new System.Drawing.Point(40, 48);
            this.Label1.Name = "Label1";
            this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label1.Size = new System.Drawing.Size(80, 16);
            this.Label1.TabIndex = 10;
            this.Label1.Text = "検収年月";
            // 
            // _Label13_17
            // 
            this._Label13_17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._Label13_17.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label13_17.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label13.SetIndex(this._Label13_17, ((short)(17)));
            this._Label13_17.Location = new System.Drawing.Point(173, 50);
            this._Label13_17.Name = "_Label13_17";
            this._Label13_17.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label13_17.Size = new System.Drawing.Size(17, 17);
            this._Label13_17.TabIndex = 9;
            this._Label13_17.Text = "年";
            // 
            // _Label13_16
            // 
            this._Label13_16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._Label13_16.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label13_16.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label13.SetIndex(this._Label13_16, ((short)(16)));
            this._Label13_16.Location = new System.Drawing.Point(237, 50);
            this._Label13_16.Name = "_Label13_16";
            this._Label13_16.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label13_16.Size = new System.Drawing.Size(17, 17);
            this._Label13_16.TabIndex = 8;
            this._Label13_16.Text = "月";
            // 
            // Label7
            // 
            this.Label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label7.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label7.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label7.Location = new System.Drawing.Point(68, 112);
            this.Label7.Name = "Label7";
            this.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label7.Size = new System.Drawing.Size(44, 16);
            this.Label7.TabIndex = 7;
            this.Label7.Text = "業者";
            // 
            // _CMD_Button_1
            // 
            this._CMD_Button_1.BackColor = System.Drawing.SystemColors.Control;
            this._CMD_Button_1.Cursor = System.Windows.Forms.Cursors.Default;
            this._CMD_Button_1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CMD_Button.SetIndex(this._CMD_Button_1, ((short)(1)));
            this._CMD_Button_1.Location = new System.Drawing.Point(680, 508);
            this._CMD_Button_1.Name = "_CMD_Button_1";
            this._CMD_Button_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._CMD_Button_1.Size = new System.Drawing.Size(97, 33);
            this._CMD_Button_1.TabIndex = 3;
            this._CMD_Button_1.Text = "印刷";
            this._CMD_Button_1.UseVisualStyleBackColor = false;
            // 
            // _CMD_Button_0
            // 
            this._CMD_Button_0.BackColor = System.Drawing.SystemColors.Control;
            this._CMD_Button_0.Cursor = System.Windows.Forms.Cursors.Default;
            this._CMD_Button_0.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CMD_Button.SetIndex(this._CMD_Button_0, ((short)(0)));
            this._CMD_Button_0.Location = new System.Drawing.Point(16, 508);
            this._CMD_Button_0.Name = "_CMD_Button_0";
            this._CMD_Button_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._CMD_Button_0.Size = new System.Drawing.Size(97, 33);
            this._CMD_Button_0.TabIndex = 4;
            this._CMD_Button_0.Text = "閉じる";
            this._CMD_Button_0.UseVisualStyleBackColor = false;
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
            this.Label41.TabIndex = 5;
            this.Label41.Text = "日";
            // 
            // grpFaxSend
            // 
            this.grpFaxSend.Controls.Add(this.LblFax);
            this.grpFaxSend.Controls.Add(this.rdoFaxYes);
            this.grpFaxSend.Controls.Add(this.rdoFaxNo);
            this.grpFaxSend.Location = new System.Drawing.Point(43, 207);
            this.grpFaxSend.Name = "grpFaxSend";
            this.grpFaxSend.Size = new System.Drawing.Size(459, 44);
            this.grpFaxSend.TabIndex = 13;
            this.grpFaxSend.TabStop = false;
            // 
            // rdoFaxNo
            // 
            this.rdoFaxNo.AutoSize = true;
            this.rdoFaxNo.Checked = true;
            this.rdoFaxNo.Location = new System.Drawing.Point(100, 17);
            this.rdoFaxNo.Name = "rdoFaxNo";
            this.rdoFaxNo.Size = new System.Drawing.Size(74, 20);
            this.rdoFaxNo.TabIndex = 0;
            this.rdoFaxNo.TabStop = true;
            this.rdoFaxNo.Text = "しない";
            this.rdoFaxNo.UseVisualStyleBackColor = true;
            // 
            // rdoFaxYes
            // 
            this.rdoFaxYes.AutoSize = true;
            this.rdoFaxYes.Location = new System.Drawing.Point(190, 17);
            this.rdoFaxYes.Name = "rdoFaxYes";
            this.rdoFaxYes.Size = new System.Drawing.Size(58, 20);
            this.rdoFaxYes.TabIndex = 1;
            this.rdoFaxYes.TabStop = true;
            this.rdoFaxYes.Text = "する";
            this.rdoFaxYes.UseVisualStyleBackColor = true;
            // 
            // LblFax
            // 
            this.LblFax.AutoSize = true;
            this.LblFax.Location = new System.Drawing.Point(13, 19);
            this.LblFax.Name = "LblFax";
            this.LblFax.Size = new System.Drawing.Size(64, 16);
            this.LblFax.TabIndex = 2;
            this.LblFax.Text = "FAX送信";
            // 
            // axMcRemote1
            // 
            this.axMcRemote1.Enabled = true;
            this.axMcRemote1.Location = new System.Drawing.Point(13, 420);
            this.axMcRemote1.Name = "axMcRemote1";
            this.axMcRemote1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMcRemote1.OcxState")));
            this.axMcRemote1.Size = new System.Drawing.Size(32, 32);
            this.axMcRemote1.TabIndex = 7;
            // 
            // FRM_CKSI0040M
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(794, 548);
            this.ControlBox = false;
            this.Controls.Add(this.axMcRemote1);
            this.Controls.Add(this.Frame1);
            this.Controls.Add(this._CMD_Button_1);
            this.Controls.Add(this._CMD_Button_0);
            this.Controls.Add(this.Label41);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(72, 58);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FRM_CKSI0040M";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "検収明細書作成";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Frame1.ResumeLayout(false);
            this.Frame1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CMD_Button)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Label13)).EndInit();
            this.grpFaxSend.ResumeLayout(false);
            this.grpFaxSend.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMcRemote1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.GroupBox grpFaxSend;
        private System.Windows.Forms.Label LblFax;
        private System.Windows.Forms.RadioButton rdoFaxYes;
        private System.Windows.Forms.RadioButton rdoFaxNo;
        private AxMCREMOTELib.AxMcRemote axMcRemote1;

    }
}
