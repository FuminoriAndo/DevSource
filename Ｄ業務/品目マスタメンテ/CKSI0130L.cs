using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Data.Odbc;

namespace Project1
{
    /// <summary>
    /// 13.07.03    ISV-TRUC    品目の一覧表示からメンテナンス画面。
    /// </summary>
    internal partial class FRM_CKSI0130L : System.Windows.Forms.Form
    {
        public FRM_CKSI0130L()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string L_Search = ",";
            char L_Search1 = ',';
            string L_Command2 = null;
            IPPAN.G_COMMAND = Interaction.Command();

            if (string.IsNullOrEmpty(IPPAN.G_COMMAND))
            {
                //コマンド ライン引数が指定されていません
                IPPAN.Error_Msg("E507", 0, " ");
                System.Environment.Exit(0);
            }
            else
            {
                //コマンド引数１を取得する
                string[] str = IPPAN.G_COMMAND.Split(L_Search1);
                if (str.Length < 2)
                {
                    IPPAN.Error_Msg("E507", 0, " ");
                    System.Environment.Exit(0);
                }
                else
                {
                    // コマンド ライン引数の受け渡し
                    CKSI0130.G_UserId = Strings.Mid(IPPAN.G_COMMAND, 1, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) - 1);
                    L_Command2 = Strings.Mid(IPPAN.G_COMMAND, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) + 1, 30);
                    CKSI0130.G_OfficeId = Strings.Mid(L_Command2, 1, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) - 1);
                    CKSI0130.G_SyokuiCd = Strings.Mid(L_Command2, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) + 1, 2);

                    if (CKSI0130.G_OfficeId.Equals("KOUBAI"))
                    {
                        Application.Run(new FRM_CKSI0130M());
                    }
                    else
                    {
                        Application.Run(new FRM_CKSI0130L());
                    }
                }
            }
            
        }

        [System.Security.Permissions.SecurityPermission(
            System.Security.Permissions.SecurityAction.LinkDemand,
            Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x112;
            const long SC_CLOSE = 0xF060L;

            if (m.Msg == WM_SYSCOMMAND &&
               (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE)
            {
                return;
            }
            base.WndProc(ref m);
        }

        private void FRM_CKSI0130L_Load(object sender, EventArgs e)
        {
           
            //待機する秒数
            const short Waittime = 2;
            //起動時刻を待避
            System.DateTime now_old = DateAndTime.Now;

            //現在の時刻－起動時刻が待機する秒数になるまでループする
            while (!(DateTime.FromOADate(DateAndTime.Now.ToOADate() - Convert.ToDateTime(now_old).ToOADate()) >= DateAndTime.TimeSerial(0, 0, Waittime)))
            {
                System.Windows.Forms.Application.DoEvents();
            }

            IPPAN.G_RET = IPPAN2.TIMER_SET(CKSI0130.G_UserId);
            if (IPPAN.G_RET != 0)
            {
                System.Environment.Exit(0);
            }
           
            // 通常の画面サイズを設定
            this.Width = 979;
            this.Height = 580;
            this.Top = 0;
            this.Left = 0;

            this.GRD_DATA.Width = 948;
            this.GRD_DATA.Height = 479;

            //GridViewのデータをセット
            this.SetGridViewData();           

            //プロジェクト名を設定
            C_COMMON.G_COMMON_MSGTITLE = "CKSI0130";

            this.Show();
        }

        private void SelectGridVewData(string himoku)
        {
            for (short i = 0; i < this.GRD_DATA.RowCount; i++)
            {
                string stHimoku = this.GRD_DATA.Rows[i].Cells["HINMOKUCD"].Value.ToString();
                if (stHimoku == himoku)
                {
                    this.GRD_DATA.Rows[i].Selected = true;
                    this.GRD_DATA.FirstDisplayedScrollingRowIndex = i;
                }
            }
        }
        /// <summary>
        /// GridViewのデータをセット
        /// </summary>
        private void SetGridViewData()
        {
            //GridViewのプロパティをオフにする。
            this.GRD_DATA.AllowUserToAddRows = false;
            this.GRD_DATA.AutoGenerateColumns = false;

            //データを取得する。
            IPPAN.G_RET = CKSI0130.Get_All_Hinmoku_Master();
            if (IPPAN.G_RET == 1)
            {
                this.CMD_SAKUJYO.Enabled = false;
                this.CMD_SHUUSEI.Enabled = false;
            }
            else
            {
                //データは空です。
                if (CKSI0130.dtbHinmoku.Rows.Count < 1)
                {
                    this.CMD_SAKUJYO.Enabled = false;
                    this.CMD_SHUUSEI.Enabled = false;
                }
                this.GRD_DATA.DataSource = CKSI0130.dtbHinmoku;
                this.GRD_DATA.Focus();                
            }
        }

        private void CMD_TOJIRU_Click(object sender, EventArgs e)
        {
            //閉じる
            this.Close();
        }

        private void CMD_TSUIKA_Click(object sender, EventArgs e)
        {
            string himokuCD = string.Empty;
            short buttonIndex = 0;
            //品目マスタメンテ画面（FRM_CKSI0130M）のインスタンスを作成する。
            using (FRM_CKSI0130M frm = new FRM_CKSI0130M())
            {
                frm.ProcKBN = CKSI0130.Mode.INSERT_MODE;
                frm.ShowDialog(this);
                himokuCD = frm.HinmokuCD;
                buttonIndex = frm.ButtonIndex;
            }
            if (buttonIndex != 1)
            {
                //GridViewのデータをセット
                this.SetGridViewData();
                SelectGridVewData(himokuCD);
            }
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
        }

        private void CMD_SHUUSEI_Click(object sender, EventArgs e)
        {

            string himokuCD = string.Empty;
            short buttonIndex = 0;
            if (GRD_DATA.SelectedRows.Count > 0)
            {
                himokuCD = GRD_DATA.SelectedRows[0].Cells["HINMOKUCD"].Value.ToString();
                if (!string.IsNullOrEmpty(himokuCD))
                {
                    //品目マスタメンテ画面（FRM_CKSI0130M）のインスタンスを作成する。    
                    using (FRM_CKSI0130M frm = new FRM_CKSI0130M())
                    {
                        frm.HinmokuCD = himokuCD;
                        frm.ProcKBN = CKSI0130.Mode.UPDATE_MODE;
                        frm.ShowDialog(this);
                        buttonIndex = frm.ButtonIndex;
                    }
                    if (buttonIndex != 1)
                    {
                        //GridViewのデータをセット
                        this.SetGridViewData();
                        SelectGridVewData(himokuCD);
                    }
                }
            }
            else
            {
                //リストから選択してください
                IPPAN.Error_Msg("E602", 0, " ");
            }           
           
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
        }

        private void CMD_SAKUJYO_Click(object sender, EventArgs e)
        {
            DialogResult dlgRlt = DialogResult.None;

            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);

             string himokuCD = string.Empty;
             if (GRD_DATA.SelectedRows.Count>0)
            {
                himokuCD = GRD_DATA.SelectedRows[0].Cells["HINMOKUCD"].Value.ToString();
                if (!string.IsNullOrEmpty(himokuCD))
                {     
                    //入力確定
                    using (C_ODBC db = new C_ODBC())
                    {
                        try
                        {
                            //DB接続
                            db.Connect();
                            //トランザクション開始
                            db.BeginTrans();

                            //確認メッセージ
                            dlgRlt = IPPAN.Error_Msg("I014", 4, " ");
                            // [はい] ボタンを選択した場合
                            if (dlgRlt == DialogResult.Yes)
                            {
                                IPPAN.G_SQL = "Delete FROM SIZAI_TANKA_MST ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKUCD ='" + himokuCD + "'";

                                //SQL実行
                                db.ExecSQL(IPPAN.G_SQL);

                                IPPAN.G_SQL = "Delete FROM SIZAI_HINMOKU_MST ";
                                IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKUCD ='" + himokuCD + "'";

                                //SQL実行
                                db.ExecSQL(IPPAN.G_SQL);
                            }
                            else
                            {
                                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                                return;
                            }
                           
                            //コミット
                            db.Commit();
                        }
                        catch (OdbcException)
                        {
                            db.Error();
                        }
                        catch (Exception)
                        {
                            db.Error();
                        }
                    }
                }
            }
            else
            {
                //リストから選択してください
                IPPAN.Error_Msg("E602", 0, " ");
            }
            //GridViewのデータをセット
            SetGridViewData();
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
        }

        private void TMR_Timer_Tick(object sender, EventArgs e)
        {
            IPPAN.Control_Init(this, "品目マスタ一覧", "CKSI0130", SO.SO_USERNAME, SO.SO_OFFICENAME, "1.00");
        }
    }
}
