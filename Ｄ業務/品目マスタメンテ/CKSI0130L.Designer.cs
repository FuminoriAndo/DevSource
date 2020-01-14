namespace Project1
{
    partial class FRM_CKSI0130L
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.GRD_DATA = new System.Windows.Forms.DataGridView();
            this.HINMOKUCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HINMOKUNM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SYUBETU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HIMOKU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UTIWAKE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TANABAN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TANI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SUIBUNKBN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KENSYUKBN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HOUKOKUKBN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KOUSAKI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SHUKKOICHI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TANKASETTE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CMD_SAKUJYO = new System.Windows.Forms.Button();
            this.CMD_SHUUSEI = new System.Windows.Forms.Button();
            this.CMD_TOJIRU = new System.Windows.Forms.Button();
            this.CMD_TSUIKA = new System.Windows.Forms.Button();
            this.TMR_Timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.GRD_DATA)).BeginInit();
            this.SuspendLayout();
            // 
            // GRD_DATA
            // 
            this.GRD_DATA.BackgroundColor = System.Drawing.SystemColors.Window;
            this.GRD_DATA.ColumnHeadersHeight = 56;
            this.GRD_DATA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.GRD_DATA.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.HINMOKUCD,
            this.HINMOKUNM,
            this.SYUBETU,
            this.HIMOKU,
            this.UTIWAKE,
            this.TANABAN,
            this.TANI,
            this.SUIBUNKBN,
            this.KENSYUKBN,
            this.HOUKOKUKBN,
            this.KOUSAKI,
            this.SHUKKOICHI,
            this.TANKASETTE});
            this.GRD_DATA.Location = new System.Drawing.Point(13, 12);
            this.GRD_DATA.MultiSelect = false;
            this.GRD_DATA.Name = "GRD_DATA";
            this.GRD_DATA.ReadOnly = true;
            this.GRD_DATA.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.GRD_DATA.RowTemplate.Height = 21;
            this.GRD_DATA.RowTemplate.ReadOnly = true;
            this.GRD_DATA.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.GRD_DATA.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GRD_DATA.Size = new System.Drawing.Size(945, 480);
            this.GRD_DATA.TabIndex = 0;
            // 
            // HINMOKUCD
            // 
            this.HINMOKUCD.DataPropertyName = "HINMOKUCD";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HINMOKUCD.DefaultCellStyle = dataGridViewCellStyle1;
            this.HINMOKUCD.HeaderText = "品目CD";
            this.HINMOKUCD.Name = "HINMOKUCD";
            this.HINMOKUCD.ReadOnly = true;
            this.HINMOKUCD.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.HINMOKUCD.Width = 50;
            // 
            // HINMOKUNM
            // 
            this.HINMOKUNM.DataPropertyName = "HINMOKUNM";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F);
            this.HINMOKUNM.DefaultCellStyle = dataGridViewCellStyle2;
            this.HINMOKUNM.HeaderText = "品目名";
            this.HINMOKUNM.Name = "HINMOKUNM";
            this.HINMOKUNM.ReadOnly = true;
            this.HINMOKUNM.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.HINMOKUNM.Width = 200;
            // 
            // SYUBETU
            // 
            this.SYUBETU.DataPropertyName = "SYUBETU";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F);
            this.SYUBETU.DefaultCellStyle = dataGridViewCellStyle3;
            this.SYUBETU.HeaderText = "種別 ";
            this.SYUBETU.Name = "SYUBETU";
            this.SYUBETU.ReadOnly = true;
            this.SYUBETU.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SYUBETU.Width = 40;
            // 
            // HIMOKU
            // 
            this.HIMOKU.DataPropertyName = "HIMOKU";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F);
            this.HIMOKU.DefaultCellStyle = dataGridViewCellStyle4;
            this.HIMOKU.HeaderText = "費目";
            this.HIMOKU.Name = "HIMOKU";
            this.HIMOKU.ReadOnly = true;
            this.HIMOKU.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.HIMOKU.Width = 40;
            // 
            // UTIWAKE
            // 
            this.UTIWAKE.DataPropertyName = "UTIWAKE";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F);
            this.UTIWAKE.DefaultCellStyle = dataGridViewCellStyle5;
            this.UTIWAKE.HeaderText = "内訳";
            this.UTIWAKE.Name = "UTIWAKE";
            this.UTIWAKE.ReadOnly = true;
            this.UTIWAKE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.UTIWAKE.Width = 40;
            // 
            // TANABAN
            // 
            this.TANABAN.DataPropertyName = "TANABAN";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F);
            this.TANABAN.DefaultCellStyle = dataGridViewCellStyle6;
            this.TANABAN.HeaderText = "棚番";
            this.TANABAN.Name = "TANABAN";
            this.TANABAN.ReadOnly = true;
            this.TANABAN.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TANABAN.Width = 40;
            // 
            // TANI
            // 
            this.TANI.DataPropertyName = "TANI";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F);
            this.TANI.DefaultCellStyle = dataGridViewCellStyle7;
            this.TANI.HeaderText = "単位";
            this.TANI.Name = "TANI";
            this.TANI.ReadOnly = true;
            this.TANI.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TANI.Width = 45;
            // 
            // SUIBUNKBN
            // 
            this.SUIBUNKBN.DataPropertyName = "SUIBUNKBN";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F);
            this.SUIBUNKBN.DefaultCellStyle = dataGridViewCellStyle8;
            this.SUIBUNKBN.HeaderText = "水分引き";
            this.SUIBUNKBN.Name = "SUIBUNKBN";
            this.SUIBUNKBN.ReadOnly = true;
            this.SUIBUNKBN.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SUIBUNKBN.Width = 50;
            // 
            // KENSYUKBN
            // 
            this.KENSYUKBN.DataPropertyName = "KENSYUKBN";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F);
            this.KENSYUKBN.DefaultCellStyle = dataGridViewCellStyle9;
            this.KENSYUKBN.HeaderText = "検収明細出力";
            this.KENSYUKBN.Name = "KENSYUKBN";
            this.KENSYUKBN.ReadOnly = true;
            this.KENSYUKBN.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.KENSYUKBN.Width = 82;
            // 
            // HOUKOKUKBN
            // 
            this.HOUKOKUKBN.DataPropertyName = "HOUKOKUKBN";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F);
            this.HOUKOKUKBN.DefaultCellStyle = dataGridViewCellStyle10;
            this.HOUKOKUKBN.HeaderText = "経理報告";
            this.HOUKOKUKBN.Name = "HOUKOKUKBN";
            this.HOUKOKUKBN.ReadOnly = true;
            this.HOUKOKUKBN.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.HOUKOKUKBN.Width = 50;
            // 
            // KOUSAKI
            // 
            this.KOUSAKI.DataPropertyName = "KOUSAKI";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F);
            this.KOUSAKI.DefaultCellStyle = dataGridViewCellStyle11;
            this.KOUSAKI.HeaderText = "向先";
            this.KOUSAKI.Name = "KOUSAKI";
            this.KOUSAKI.ReadOnly = true;
            this.KOUSAKI.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // SHUKKOICHI
            // 
            this.SHUKKOICHI.DataPropertyName = "SHUKKOICHI";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F);
            this.SHUKKOICHI.DefaultCellStyle = dataGridViewCellStyle12;
            this.SHUKKOICHI.HeaderText = "出庫位置";
            this.SHUKKOICHI.Name = "SHUKKOICHI";
            this.SHUKKOICHI.ReadOnly = true;
            this.SHUKKOICHI.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // TANKASETTE
            // 
            this.TANKASETTE.DataPropertyName = "TANKASETTE";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F);
            this.TANKASETTE.DefaultCellStyle = dataGridViewCellStyle13;
            this.TANKASETTE.HeaderText = "単価設定";
            this.TANKASETTE.Name = "TANKASETTE";
            this.TANKASETTE.ReadOnly = true;
            this.TANKASETTE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TANKASETTE.Width = 50;
            // 
            // CMD_SAKUJYO
            // 
            this.CMD_SAKUJYO.BackColor = System.Drawing.SystemColors.Control;
            this.CMD_SAKUJYO.Cursor = System.Windows.Forms.Cursors.Default;
            this.CMD_SAKUJYO.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CMD_SAKUJYO.Location = new System.Drawing.Point(327, 501);
            this.CMD_SAKUJYO.Name = "CMD_SAKUJYO";
            this.CMD_SAKUJYO.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CMD_SAKUJYO.Size = new System.Drawing.Size(97, 37);
            this.CMD_SAKUJYO.TabIndex = 52;
            this.CMD_SAKUJYO.Text = "削 除";
            this.CMD_SAKUJYO.UseVisualStyleBackColor = false;
            this.CMD_SAKUJYO.Click += new System.EventHandler(this.CMD_SAKUJYO_Click);
            // 
            // CMD_SHUUSEI
            // 
            this.CMD_SHUUSEI.BackColor = System.Drawing.SystemColors.Control;
            this.CMD_SHUUSEI.Cursor = System.Windows.Forms.Cursors.Default;
            this.CMD_SHUUSEI.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CMD_SHUUSEI.Location = new System.Drawing.Point(222, 501);
            this.CMD_SHUUSEI.Name = "CMD_SHUUSEI";
            this.CMD_SHUUSEI.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CMD_SHUUSEI.Size = new System.Drawing.Size(97, 37);
            this.CMD_SHUUSEI.TabIndex = 51;
            this.CMD_SHUUSEI.Text = "修 正";
            this.CMD_SHUUSEI.UseVisualStyleBackColor = false;
            this.CMD_SHUUSEI.Click += new System.EventHandler(this.CMD_SHUUSEI_Click);
            // 
            // CMD_TOJIRU
            // 
            this.CMD_TOJIRU.BackColor = System.Drawing.SystemColors.Control;
            this.CMD_TOJIRU.Cursor = System.Windows.Forms.Cursors.Default;
            this.CMD_TOJIRU.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CMD_TOJIRU.Location = new System.Drawing.Point(12, 501);
            this.CMD_TOJIRU.Name = "CMD_TOJIRU";
            this.CMD_TOJIRU.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CMD_TOJIRU.Size = new System.Drawing.Size(97, 37);
            this.CMD_TOJIRU.TabIndex = 50;
            this.CMD_TOJIRU.Text = "閉じる";
            this.CMD_TOJIRU.UseVisualStyleBackColor = false;
            this.CMD_TOJIRU.Click += new System.EventHandler(this.CMD_TOJIRU_Click);
            // 
            // CMD_TSUIKA
            // 
            this.CMD_TSUIKA.BackColor = System.Drawing.SystemColors.Control;
            this.CMD_TSUIKA.Cursor = System.Windows.Forms.Cursors.Default;
            this.CMD_TSUIKA.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CMD_TSUIKA.Location = new System.Drawing.Point(117, 501);
            this.CMD_TSUIKA.Name = "CMD_TSUIKA";
            this.CMD_TSUIKA.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CMD_TSUIKA.Size = new System.Drawing.Size(97, 37);
            this.CMD_TSUIKA.TabIndex = 53;
            this.CMD_TSUIKA.Text = "追 加";
            this.CMD_TSUIKA.UseVisualStyleBackColor = false;
            this.CMD_TSUIKA.Click += new System.EventHandler(this.CMD_TSUIKA_Click);
            // 
            // TMR_Timer
            // 
            this.TMR_Timer.Enabled = true;
            this.TMR_Timer.Interval = 1000;
            this.TMR_Timer.Tick += new System.EventHandler(this.TMR_Timer_Tick);
            // 
            // FRM_CKSI0130L
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(969, 548);
            this.ControlBox = false;
            this.Controls.Add(this.CMD_TSUIKA);
            this.Controls.Add(this.CMD_SAKUJYO);
            this.Controls.Add(this.CMD_SHUUSEI);
            this.Controls.Add(this.CMD_TOJIRU);
            this.Controls.Add(this.GRD_DATA);
            this.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FRM_CKSI0130L";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "品目マスタ一覧";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FRM_CKSI0130L_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GRD_DATA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip ToolTip1;
        private System.Windows.Forms.DataGridView GRD_DATA;
        internal System.Windows.Forms.Button CMD_SAKUJYO;
        internal System.Windows.Forms.Button CMD_SHUUSEI;
        internal System.Windows.Forms.Button CMD_TOJIRU;
        public System.Windows.Forms.Button CMD_TSUIKA;
        public System.Windows.Forms.Timer TMR_Timer;
        private System.Windows.Forms.DataGridViewTextBoxColumn HINMOKUCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn HINMOKUNM;
        private System.Windows.Forms.DataGridViewTextBoxColumn SYUBETU;
        private System.Windows.Forms.DataGridViewTextBoxColumn HIMOKU;
        private System.Windows.Forms.DataGridViewTextBoxColumn UTIWAKE;
        private System.Windows.Forms.DataGridViewTextBoxColumn TANABAN;
        private System.Windows.Forms.DataGridViewTextBoxColumn TANI;
        private System.Windows.Forms.DataGridViewTextBoxColumn SUIBUNKBN;
        private System.Windows.Forms.DataGridViewTextBoxColumn KENSYUKBN;
        private System.Windows.Forms.DataGridViewTextBoxColumn HOUKOKUKBN;
        private System.Windows.Forms.DataGridViewTextBoxColumn KOUSAKI;
        private System.Windows.Forms.DataGridViewTextBoxColumn SHUKKOICHI;
        private System.Windows.Forms.DataGridViewTextBoxColumn TANKASETTE;
    }
}