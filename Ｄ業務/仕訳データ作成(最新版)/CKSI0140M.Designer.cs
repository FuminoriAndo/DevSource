using System;

namespace Project1
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    partial class FRM_CKSI0140M
    {
        #region "Windows フォーム デザイナによって生成されたコード "
        [System.Diagnostics.DebuggerNonUserCode()]
        public FRM_CKSI0140M()
            : base()
        {
            Load += FRM_CKSI0140M_Load;
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
        public System.Windows.Forms.Button _CMD_Button_2;
        public System.Windows.Forms.TextBox Txt_Listno;
        public System.Windows.Forms.Label Label4;
        public System.Windows.Forms.Label Label3;
        public System.Windows.Forms.GroupBox Frame2;
        public System.Windows.Forms.Label LBL_MSG;
        public System.Windows.Forms.Label LBL_Kensyumm;
        public System.Windows.Forms.Label LBL_Kensyuyy;
        public System.Windows.Forms.Label Label2;
        public System.Windows.Forms.Label Label1;
        public System.Windows.Forms.Label _Label13_17;
        public System.Windows.Forms.Label _Label13_16;
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
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Frame2 = new System.Windows.Forms.GroupBox();
            this._CMD_Button_2 = new System.Windows.Forms.Button();
            this.Txt_Listno = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Frame1 = new System.Windows.Forms.GroupBox();
            this.LBL_MSG = new System.Windows.Forms.Label();
            this.LBL_Kensyumm = new System.Windows.Forms.Label();
            this.LBL_Kensyuyy = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this._Label13_17 = new System.Windows.Forms.Label();
            this._Label13_16 = new System.Windows.Forms.Label();
            this._CMD_Button_1 = new System.Windows.Forms.Button();
            this._CMD_Button_0 = new System.Windows.Forms.Button();
            this.TMR_Timer = new System.Windows.Forms.Timer(this.components);
            this.Label41 = new System.Windows.Forms.Label();
            this.CMD_Button = new Microsoft.VisualBasic.Compatibility.VB6.ButtonArray(this.components);
            this.Label13 = new Microsoft.VisualBasic.Compatibility.VB6.LabelArray(this.components);
            this.Frame2.SuspendLayout();
            this.Frame1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CMD_Button)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Label13)).BeginInit();
            this.SuspendLayout();
            // 
            // Frame2
            // 
            this.Frame2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Frame2.Controls.Add(this._CMD_Button_2);
            this.Frame2.Controls.Add(this.Txt_Listno);
            this.Frame2.Controls.Add(this.Label4);
            this.Frame2.Controls.Add(this.Label3);
            this.Frame2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Frame2.Location = new System.Drawing.Point(450, 396);
            this.Frame2.Name = "Frame2";
            this.Frame2.Padding = new System.Windows.Forms.Padding(0);
            this.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame2.Size = new System.Drawing.Size(292, 67);
            this.Frame2.TabIndex = 11;
            this.Frame2.TabStop = false;
            // 
            // _CMD_Button_2
            // 
            this._CMD_Button_2.BackColor = System.Drawing.SystemColors.Control;
            this._CMD_Button_2.Cursor = System.Windows.Forms.Cursors.Default;
            this._CMD_Button_2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CMD_Button.SetIndex(this._CMD_Button_2, ((short)(2)));
            this._CMD_Button_2.Location = new System.Drawing.Point(183, 21);
            this._CMD_Button_2.Name = "_CMD_Button_2";
            this._CMD_Button_2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._CMD_Button_2.Size = new System.Drawing.Size(97, 33);
            this._CMD_Button_2.TabIndex = 13;
            this._CMD_Button_2.Text = "再印刷";
            this._CMD_Button_2.UseVisualStyleBackColor = false;
            // 
            // Txt_Listno
            // 
            this.Txt_Listno.AcceptsReturn = true;
            this.Txt_Listno.BackColor = System.Drawing.Color.White;
            this.Txt_Listno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Txt_Listno.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Txt_Listno.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Txt_Listno.Location = new System.Drawing.Point(97, 24);
            this.Txt_Listno.MaxLength = 0;
            this.Txt_Listno.Name = "Txt_Listno";
            this.Txt_Listno.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Txt_Listno.Size = new System.Drawing.Size(70, 23);
            this.Txt_Listno.TabIndex = 12;
            this.Txt_Listno.Leave += new System.EventHandler(this.Txt_Listno_Leave);
            this.Txt_Listno.Enter += new System.EventHandler(this.Txt_Listno_Enter);
            // 
            // Label4
            // 
            this.Label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label4.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label4.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label4.Location = new System.Drawing.Point(9, 27);
            this.Label4.Name = "Label4";
            this.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label4.Size = new System.Drawing.Size(63, 16);
            this.Label4.TabIndex = 15;
            this.Label4.Text = "リスト№";
            // 
            // Label3
            // 
            this.Label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Label3.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label3.Location = new System.Drawing.Point(72, 24);
            this.Label3.Name = "Label3";
            this.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label3.Size = new System.Drawing.Size(26, 23);
            this.Label3.TabIndex = 14;
            this.Label3.Text = "II";
            // 
            // Frame1
            // 
            this.Frame1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Frame1.Controls.Add(this.LBL_MSG);
            this.Frame1.Controls.Add(this.LBL_Kensyumm);
            this.Frame1.Controls.Add(this.LBL_Kensyuyy);
            this.Frame1.Controls.Add(this.Label2);
            this.Frame1.Controls.Add(this.Label1);
            this.Frame1.Controls.Add(this._Label13_17);
            this.Frame1.Controls.Add(this._Label13_16);
            this.Frame1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Frame1.Location = new System.Drawing.Point(40, 116);
            this.Frame1.Name = "Frame1";
            this.Frame1.Padding = new System.Windows.Forms.Padding(0);
            this.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame1.Size = new System.Drawing.Size(708, 276);
            this.Frame1.TabIndex = 3;
            this.Frame1.TabStop = false;
            // 
            // LBL_MSG
            // 
            this.LBL_MSG.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.LBL_MSG.Cursor = System.Windows.Forms.Cursors.Default;
            this.LBL_MSG.Font = new System.Drawing.Font("ＭＳ ゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LBL_MSG.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LBL_MSG.Location = new System.Drawing.Point(8, 186);
            this.LBL_MSG.Name = "LBL_MSG";
            this.LBL_MSG.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LBL_MSG.Size = new System.Drawing.Size(692, 31);
            this.LBL_MSG.TabIndex = 10;
            this.LBL_MSG.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // LBL_Kensyumm
            // 
            this.LBL_Kensyumm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.LBL_Kensyumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBL_Kensyumm.Cursor = System.Windows.Forms.Cursors.Default;
            this.LBL_Kensyumm.ForeColor = System.Drawing.SystemColors.WindowText;
            this.LBL_Kensyumm.Location = new System.Drawing.Point(382, 60);
            this.LBL_Kensyumm.Name = "LBL_Kensyumm";
            this.LBL_Kensyumm.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LBL_Kensyumm.Size = new System.Drawing.Size(28, 25);
            this.LBL_Kensyumm.TabIndex = 9;
            // 
            // LBL_Kensyuyy
            // 
            this.LBL_Kensyuyy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.LBL_Kensyuyy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBL_Kensyuyy.Cursor = System.Windows.Forms.Cursors.Default;
            this.LBL_Kensyuyy.ForeColor = System.Drawing.SystemColors.WindowText;
            this.LBL_Kensyuyy.Location = new System.Drawing.Point(318, 60);
            this.LBL_Kensyuyy.Name = "LBL_Kensyuyy";
            this.LBL_Kensyuyy.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LBL_Kensyuyy.Size = new System.Drawing.Size(28, 25);
            this.LBL_Kensyuyy.TabIndex = 8;
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label2.Location = new System.Drawing.Point(150, 123);
            this.Label2.Name = "Label2";
            this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label2.Size = new System.Drawing.Size(426, 16);
            this.Label2.TabIndex = 7;
            this.Label2.Text = "副資材仕訳データを作成し、確認リストを印刷します。";
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label1.Location = new System.Drawing.Point(234, 66);
            this.Label1.Name = "Label1";
            this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label1.Size = new System.Drawing.Size(78, 16);
            this.Label1.TabIndex = 6;
            this.Label1.Text = "経理年月";
            // 
            // _Label13_17
            // 
            this._Label13_17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._Label13_17.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label13_17.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label13.SetIndex(this._Label13_17, ((short)(17)));
            this._Label13_17.Location = new System.Drawing.Point(354, 66);
            this._Label13_17.Name = "_Label13_17";
            this._Label13_17.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label13_17.Size = new System.Drawing.Size(17, 17);
            this._Label13_17.TabIndex = 5;
            this._Label13_17.Text = "年";
            // 
            // _Label13_16
            // 
            this._Label13_16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._Label13_16.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label13_16.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label13.SetIndex(this._Label13_16, ((short)(16)));
            this._Label13_16.Location = new System.Drawing.Point(418, 66);
            this._Label13_16.Name = "_Label13_16";
            this._Label13_16.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label13_16.Size = new System.Drawing.Size(17, 17);
            this._Label13_16.TabIndex = 4;
            this._Label13_16.Text = "月";
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
            this._CMD_Button_1.TabIndex = 1;
            this._CMD_Button_1.Text = "実行";
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
            this._CMD_Button_0.TabIndex = 0;
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
            this.Label41.TabIndex = 2;
            this.Label41.Text = "日";
            // 
            // FRM_CKSI0140M
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(794, 548);
            this.ControlBox = false;
            this.Controls.Add(this.Frame2);
            this.Controls.Add(this.Frame1);
            this.Controls.Add(this._CMD_Button_1);
            this.Controls.Add(this._CMD_Button_0);
            this.Controls.Add(this.Label41);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(80, 142);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FRM_CKSI0140M";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "副資材仕訳データ作成";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Frame2.ResumeLayout(false);
            this.Frame2.PerformLayout();
            this.Frame1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CMD_Button)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Label13)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
    }
}
