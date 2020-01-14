using System;

namespace Project1
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    partial class FRM_CKSI1000M
    {
        #region "Windows フォーム デザイナによって生成されたコード "
        [System.Diagnostics.DebuggerNonUserCode()]
        public FRM_CKSI1000M()
            : base()
        {
            Load += FRM_CKSI1000M_Load;
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
        public System.Windows.Forms.ListBox LST_LIST;
        public System.Windows.Forms.Timer TMR_TIMER;
        public System.Windows.Forms.TextBox TXT_HINCD;
        public System.Windows.Forms.Button _CMD_BUTTON_0;
        public System.Windows.Forms.Label _Label3_0;
        public System.Windows.Forms.GroupBox Frame1;
        public System.Windows.Forms.Button _CMD_BUTTON_1;
        public System.Windows.Forms.Button _CMD_BUTTON_2;
        public System.Windows.Forms.Label Label7;
        public System.Windows.Forms.Label Label6;
        public System.Windows.Forms.Label Label5;
        public System.Windows.Forms.Label Label4;
        public System.Windows.Forms.Label Label2;
        public System.Windows.Forms.Label Label1;
        public System.Windows.Forms.Label Label12;
        public System.Windows.Forms.Label Label9;
        public Microsoft.VisualBasic.Compatibility.VB6.ButtonArray CMD_BUTTON;
        public Microsoft.VisualBasic.Compatibility.VB6.LabelArray Label3;
        //メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
        //Windows フォーム デザイナを使って変更できます。
        //コード エディタを使用して、変更しないでください。
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.LST_LIST = new System.Windows.Forms.ListBox();
            this.TMR_TIMER = new System.Windows.Forms.Timer(this.components);
            this.Frame1 = new System.Windows.Forms.GroupBox();
            this.TXT_HINCD = new System.Windows.Forms.TextBox();
            this._CMD_BUTTON_0 = new System.Windows.Forms.Button();
            this._Label3_0 = new System.Windows.Forms.Label();
            this._CMD_BUTTON_1 = new System.Windows.Forms.Button();
            this._CMD_BUTTON_2 = new System.Windows.Forms.Button();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label12 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.CMD_BUTTON = new Microsoft.VisualBasic.Compatibility.VB6.ButtonArray(this.components);
            this.Label3 = new Microsoft.VisualBasic.Compatibility.VB6.LabelArray(this.components);
            this.Frame1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CMD_BUTTON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Label3)).BeginInit();
            this.SuspendLayout();
            // 
            // LST_LIST
            // 
            this.LST_LIST.BackColor = System.Drawing.SystemColors.Window;
            this.LST_LIST.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LST_LIST.Cursor = System.Windows.Forms.Cursors.Default;
            this.LST_LIST.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LST_LIST.ForeColor = System.Drawing.SystemColors.WindowText;
            this.LST_LIST.Location = new System.Drawing.Point(4, 112);
            this.LST_LIST.Name = "LST_LIST";
            this.LST_LIST.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LST_LIST.Size = new System.Drawing.Size(797, 405);
            this.LST_LIST.TabIndex = 2;
            // 
            // TMR_TIMER
            // 
            this.TMR_TIMER.Enabled = true;
            this.TMR_TIMER.Interval = 1000;
            this.TMR_TIMER.Tick += new System.EventHandler(this.TMR_Timer_Tick);
            // 
            // Frame1
            // 
            this.Frame1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Frame1.Controls.Add(this.TXT_HINCD);
            this.Frame1.Controls.Add(this._CMD_BUTTON_0);
            this.Frame1.Controls.Add(this._Label3_0);
            this.Frame1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Frame1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Frame1.Location = new System.Drawing.Point(4, 18);
            this.Frame1.Name = "Frame1";
            this.Frame1.Padding = new System.Windows.Forms.Padding(0);
            this.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame1.Size = new System.Drawing.Size(301, 68);
            this.Frame1.TabIndex = 5;
            this.Frame1.TabStop = false;
            this.Frame1.Text = "検索条件";
            // 
            // TXT_HINCD
            // 
            this.TXT_HINCD.AcceptsReturn = true;
            this.TXT_HINCD.BackColor = System.Drawing.Color.White;
            this.TXT_HINCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TXT_HINCD.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TXT_HINCD.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TXT_HINCD.ForeColor = System.Drawing.Color.Black;
            this.TXT_HINCD.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.TXT_HINCD.Location = new System.Drawing.Point(78, 28);
            this.TXT_HINCD.MaxLength = 4;
            this.TXT_HINCD.Multiline = true;
            this.TXT_HINCD.Name = "TXT_HINCD";
            this.TXT_HINCD.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TXT_HINCD.Size = new System.Drawing.Size(37, 25);
            this.TXT_HINCD.TabIndex = 0;
            this.TXT_HINCD.Leave += new System.EventHandler(this.TXT_HINCD_Leave);
            this.TXT_HINCD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_HINCD_KeyPress);
            this.TXT_HINCD.Enter += new System.EventHandler(this.TXT_HINCD_Enter);
            // 
            // _CMD_BUTTON_0
            // 
            this._CMD_BUTTON_0.BackColor = System.Drawing.SystemColors.Control;
            this._CMD_BUTTON_0.Cursor = System.Windows.Forms.Cursors.Default;
            this._CMD_BUTTON_0.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CMD_BUTTON.SetIndex(this._CMD_BUTTON_0, ((short)(0)));
            this._CMD_BUTTON_0.Location = new System.Drawing.Point(216, 12);
            this._CMD_BUTTON_0.Name = "_CMD_BUTTON_0";
            this._CMD_BUTTON_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._CMD_BUTTON_0.Size = new System.Drawing.Size(77, 49);
            this._CMD_BUTTON_0.TabIndex = 1;
            this._CMD_BUTTON_0.Text = "検索";
            this._CMD_BUTTON_0.UseVisualStyleBackColor = false;
            // 
            // _Label3_0
            // 
            this._Label3_0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._Label3_0.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label3_0.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label3.SetIndex(this._Label3_0, ((short)(0)));
            this._Label3_0.Location = new System.Drawing.Point(17, 34);
            this._Label3_0.Name = "_Label3_0";
            this._Label3_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label3_0.Size = new System.Drawing.Size(65, 21);
            this._Label3_0.TabIndex = 6;
            this._Label3_0.Text = "品目CD";
            // 
            // _CMD_BUTTON_1
            // 
            this._CMD_BUTTON_1.BackColor = System.Drawing.SystemColors.Control;
            this._CMD_BUTTON_1.Cursor = System.Windows.Forms.Cursors.Default;
            this._CMD_BUTTON_1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CMD_BUTTON.SetIndex(this._CMD_BUTTON_1, ((short)(1)));
            this._CMD_BUTTON_1.Location = new System.Drawing.Point(206, 528);
            this._CMD_BUTTON_1.Name = "_CMD_BUTTON_1";
            this._CMD_BUTTON_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._CMD_BUTTON_1.Size = new System.Drawing.Size(100, 33);
            this._CMD_BUTTON_1.TabIndex = 4;
            this._CMD_BUTTON_1.Text = "クリア";
            this._CMD_BUTTON_1.UseVisualStyleBackColor = false;
            // 
            // _CMD_BUTTON_2
            // 
            this._CMD_BUTTON_2.BackColor = System.Drawing.SystemColors.Control;
            this._CMD_BUTTON_2.Cursor = System.Windows.Forms.Cursors.Default;
            this._CMD_BUTTON_2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CMD_BUTTON.SetIndex(this._CMD_BUTTON_2, ((short)(2)));
            this._CMD_BUTTON_2.Location = new System.Drawing.Point(8, 528);
            this._CMD_BUTTON_2.Name = "_CMD_BUTTON_2";
            this._CMD_BUTTON_2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._CMD_BUTTON_2.Size = new System.Drawing.Size(97, 33);
            this._CMD_BUTTON_2.TabIndex = 3;
            this._CMD_BUTTON_2.Text = "閉じる";
            this._CMD_BUTTON_2.UseVisualStyleBackColor = false;
            // 
            // Label7
            // 
            this.Label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label7.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label7.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label7.Location = new System.Drawing.Point(745, 88);
            this.Label7.Name = "Label7";
            this.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label7.Size = new System.Drawing.Size(56, 17);
            this.Label7.TabIndex = 14;
            this.Label7.Text = "直送数";
            // 
            // Label6
            // 
            this.Label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label6.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label6.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label6.Location = new System.Drawing.Point(667, 72);
            this.Label6.Name = "Label6";
            this.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label6.Size = new System.Drawing.Size(59, 33);
            this.Label6.TabIndex = 13;
            this.Label6.Text = "現在 在庫数";
            // 
            // Label5
            // 
            this.Label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label5.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label5.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label5.Location = new System.Drawing.Point(590, 88);
            this.Label5.Name = "Label5";
            this.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label5.Size = new System.Drawing.Size(57, 17);
            this.Label5.TabIndex = 12;
            this.Label5.Text = "返品数";
            // 
            // Label4
            // 
            this.Label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label4.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label4.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label4.Location = new System.Drawing.Point(514, 88);
            this.Label4.Name = "Label4";
            this.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label4.Size = new System.Drawing.Size(58, 17);
            this.Label4.TabIndex = 11;
            this.Label4.Text = "出庫数";
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label2.Location = new System.Drawing.Point(435, 88);
            this.Label2.Name = "Label2";
            this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label2.Size = new System.Drawing.Size(62, 17);
            this.Label2.TabIndex = 10;
            this.Label2.Text = "入庫数";
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label1.Location = new System.Drawing.Point(361, 72);
            this.Label1.Name = "Label1";
            this.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label1.Size = new System.Drawing.Size(58, 33);
            this.Label1.TabIndex = 9;
            this.Label1.Text = "月初 在庫数";
            // 
            // Label12
            // 
            this.Label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label12.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label12.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label12.Location = new System.Drawing.Point(61, 90);
            this.Label12.Name = "Label12";
            this.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label12.Size = new System.Drawing.Size(68, 17);
            this.Label12.TabIndex = 8;
            this.Label12.Text = "品目名";
            // 
            // Label9
            // 
            this.Label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Label9.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label9.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label9.Location = new System.Drawing.Point(4, 90);
            this.Label9.Name = "Label9";
            this.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label9.Size = new System.Drawing.Size(57, 17);
            this.Label9.TabIndex = 7;
            this.Label9.Text = "品目CD";
            // 
            // FRM_CKSI1000M
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(804, 575);
            this.ControlBox = false;
            this.Controls.Add(this.LST_LIST);
            this.Controls.Add(this.Frame1);
            this.Controls.Add(this._CMD_BUTTON_1);
            this.Controls.Add(this._CMD_BUTTON_2);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Label12);
            this.Controls.Add(this.Label9);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(259, 284);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FRM_CKSI1000M";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "在庫問合";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Frame1.ResumeLayout(false);
            this.Frame1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CMD_BUTTON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Label3)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
    }
}
