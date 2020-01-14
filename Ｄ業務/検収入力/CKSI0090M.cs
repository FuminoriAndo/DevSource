using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.Odbc;

namespace Project1
{
    internal partial class FRM_CKSI0090M : System.Windows.Forms.Form
    {
        //**************************************************************'
        //                                                              '
        //   プログラム名　　　検収入力－検収一覧画面処理               '
        //                                                              '
        //   更新履歴
        //   更新日付    Rev     修正者      内容
        //   03.01.15           NTIS浅田　　Ｓ／Ｏサーバ更新の為、VB4.0→VB6.0SP5へのバージョンアップ対応
        //**************************************************************'

        string M_Gyosyacd;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FRM_CKSI0090M());
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

        private void CMD_Button_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = CMD_Button.GetIndex((Button)eventSender);

            switch (Index)
            {
                case 0:
                    //検索
                    Lst_List.Items.Clear();
                    if (IPPAN.C_Allspace(Txt_Gyosyacd.Text) == 0)
                    {
                        Txt_Gyosyacd.Focus();
                        Txt_Gyosyacd_Enter(Txt_Gyosyacd, new System.EventArgs());
                        //必須項目入力エラー
                        IPPAN.Error_Msg("E200", 0, " ");
                        return;
                    }
                    if (IPPAN.Input_Check(Txt_Gyosyacd.Text) == 1)
                    {
                        Txt_Gyosyacd.Focus();
                        Txt_Gyosyacd_Enter(Txt_Gyosyacd, new System.EventArgs());
                        //入力エラー
                        IPPAN.Error_Msg("E202", 0, " ");
                        return;
                    }

                    if (CKSI0090.Gyosya_Kensaku(Txt_Gyosyacd.Text) == 1)
                    {
                        Txt_Gyosyacd.Focus();
                        Txt_Gyosyacd_Enter(Txt_Gyosyacd, new System.EventArgs());
                        //業者マスタが見つかりません
                        IPPAN.Error_Msg("E503", 0, " ");
                        return;
                    }
                    if (Kensyu_Kensaku() == 1)
                    {
                        Txt_Gyosyacd.Focus();
                        Txt_Gyosyacd_Enter(Txt_Gyosyacd, new System.EventArgs());
                        //検索条件に該当するデータはありません
                        IPPAN.Error_Msg("E609", 0, " ");
                        return;
                    }
                    CMD_Button[2].Enabled = true;
                    Lst_List.Enabled = true;
                    Lst_List.SelectedIndex = 0;
                    Lst_List.Focus();
                    break;
                case 1:
                    //閉じる
                    this.Close();
                    break;
                case 2:
                    //入力確定
                    if (Opt_Shori[1].Checked == true)
                    {
                        CKSI0090.G_Current_Data = 0;
                    }
                    else
                    {
                        CKSI0090.G_Current_Data = Lst_List.SelectedIndex;
                    }

                    TMR_Timer.Enabled = false;

                    //モーダル表示
                    FRM_CKSI0090S01 fm = new FRM_CKSI0090S01();
                    fm.ShowDialog(this);
                    fm.Dispose();
                    fm = null;

                    TMR_Timer.Enabled = true;

                    if (Opt_Shori[0].Checked == true || Opt_Shori[2].Checked == true)
                    {
                        Lst_List.Items.Clear();
                        if (Kensyu_Kensaku() == 1)
                        {
                            Txt_Gyosyacd.Focus();
                            CMD_Button[0].Enabled = true;
                            CMD_Button[2].Enabled = false;
                            Lst_List.Enabled = false;
                        }
                        else
                        {
                            Lst_List.Enabled = true;
                            Lst_List.SelectedIndex = 0;
                            Lst_List.Focus();
                        }
                    }
                    break;
            }
        }

        private void setEventCtrlAry()
        {
            //コントロール配列にイベントを設定
            //CMD_Button
            for (short i = 0; i < this.CMD_Button.Count(); i++)
            {
                this.CMD_Button[i].Click += new EventHandler(CMD_Button_Click);
            }
            //Opt_Shori
            for (short i = 0; i < this.Opt_Shori.Count(); i++)
            {
                this.Opt_Shori[i].CheckedChanged += new EventHandler(Opt_Shori_CheckedChanged);
            }
        }

        private void FRM_CKSI0090M_Load(System.Object eventSender, System.EventArgs eventArgs)
        {
            string L_Search = null;
            string L_Command2 = null;

            //プロジェクト名を設定
            C_COMMON.G_COMMON_MSGTITLE = "CKSI0090";

            //コントロール配列にイベントを設定
            setEventCtrlAry();

            //待機する秒数
            const short Waittime = 2;
            //起動時刻を待避
            System.DateTime now_old = DateTime.Now;

            //現在の時刻－起動時刻が待機する秒数になるまでループする
            while (!(DateTime.FromOADate(DateAndTime.Now.ToOADate() - Convert.ToDateTime(now_old).ToOADate()) >= DateAndTime.TimeSerial(0, 0, Waittime)))
            {
                System.Windows.Forms.Application.DoEvents();
            }

            Show();

            //引数のチェック
            IPPAN.G_COMMAND = Interaction.Command();

            L_Search = ",";

            if (string.IsNullOrEmpty(IPPAN.G_COMMAND))
            {
                // コマンド ライン引数が指定されていないとき。
                //コマンド ライン引数が指定されていません
                IPPAN.Error_Msg("E507", 0, " ");
                System.Environment.Exit(0);
            }
            else
            {
                // コマンド ライン引数の受け渡し
                CKSI0090.G_UserId = Strings.Mid(IPPAN.G_COMMAND, 1, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) - 1);
                L_Command2 = Strings.Mid(IPPAN.G_COMMAND, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) + 1, 30);
                CKSI0090.G_OfficeId = Strings.Mid(L_Command2, 1, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) - 1);
                CKSI0090.G_Syokuicd = Strings.Mid(L_Command2, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) + 1, 2);
                IPPAN.G_RET = IPPAN2.TIMER_SET(CKSI0090.G_UserId);
                if (IPPAN.G_RET != 0)
                {
                    System.Environment.Exit(0);
                }
            }

            if (CKSI0090.Control_Kensaku() == 1)
            {
                //副資材コントロールマスタが見つかりません
                IPPAN.Error_Msg("E700", 0, " ");
                System.Environment.Exit(0);
            }
            CMD_Button[2].Enabled = false;
        }

        private void Opt_Shori_CheckedChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            RadioButton rdBtn = (RadioButton)eventSender;

            if (rdBtn.Checked)
            {
                short Index = Opt_Shori.GetIndex(rdBtn);
                if (Opt_Shori[0].Checked == true || Opt_Shori[2].Checked == true)
                {
                    Txt_Gyosyacd.Enabled = true;
                    CMD_Button[0].Enabled = true;
                    CMD_Button[2].Enabled = false;
                }
                else
                {
                    Txt_Gyosyacd.Enabled = false;
                    Txt_Gyosyacd.Text = "";
                    Lbl_Gyosyanm.Text = "";
                    CMD_Button[0].Enabled = false;
                    CMD_Button[2].Enabled = true;
                }
                Lst_List.Items.Clear();
                Lst_List.Enabled = false;
            }
        }

        private void TMR_TIMER_Tick(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Control_Init(this, "検収一覧", "CKSI0090M", SO.SO_USERNAME, SO.SO_OFFICENAME, "2.00");
        }

        private void Txt_Gyosyacd_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            M_Gyosyacd = Strings.Trim(Txt_Gyosyacd.Text);
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void Txt_Gyosyacd_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (Strings.Trim(Txt_Gyosyacd.Text) == M_Gyosyacd)
            {
                IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
                return;
            }
            if (IPPAN.C_Allspace(Txt_Gyosyacd.Text) == 0)
            {
                Lbl_Gyosyanm.Text = "";
                Txt_Gyosyacd.Focus();
                Txt_Gyosyacd_Enter(Txt_Gyosyacd, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return;
            }
            if (IPPAN.Input_Check(Txt_Gyosyacd.Text) == 1)
            {
                Lbl_Gyosyanm.Text = "";
                Txt_Gyosyacd.Focus();
                Txt_Gyosyacd_Enter(Txt_Gyosyacd, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return;
            }

            if (CKSI0090.Gyosya_Kensaku(Txt_Gyosyacd.Text) == 1)
            {
                Lbl_Gyosyanm.Text = "";
                Txt_Gyosyacd.Focus();
                Txt_Gyosyacd_Enter(Txt_Gyosyacd, new System.EventArgs());
                //業者マスタが見つかりません
                IPPAN.Error_Msg("E503", 0, " ");
                return;
            }
            Lbl_Gyosyanm.Text = CKSI0090.G_Gyosyanm;
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private short Kensyu_Kensaku()
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //検収トランの検索
            IPPAN.G_SQL = "SELECT * FROM SIZAI_KENSYU_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE GYOSYACD = '" + Txt_Gyosyacd.Text + "' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "and SIMEYM = '" + CKSI0090.G_KBYM + "' ";
            if (Opt_Shori[0].Checked == true)
            {
                IPPAN.G_SQL = IPPAN.G_SQL + "and KENSYUFLG = ' ' ";
            }
            else
            {
                IPPAN.G_SQL = IPPAN.G_SQL + "and KENSYUFLG = '1' ";
            }
            IPPAN.G_SQL = IPPAN.G_SQL + "and DELFLG = ' ' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "order by KUBUN, HIMOKU, UTIWAKE, TANABAN ";
            Debug.Print(IPPAN.G_SQL);

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    //DB接続
                    db.Connect();
                    //SQL実行
                    tbl = db.ExecSQL(IPPAN.G_SQL);
                }
                catch (OdbcException)
                {
                    db.Error();
                }
                //2013.04.13 miura mori start
                catch (Exception)
                {
                    db.Error();
                }
                //2013.04.13 miura mori end
            }

            //結果の列数を取得する
            if (tbl.Columns.Count < 1)
            {
                return functionReturnValue;
            }

            //データがない
            if (tbl.Rows.Count == 0)
            {
                return functionReturnValue;
            }

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    //項目セット
                    CKSI0090.G_Kensyu_Area[i, j] = tbl.Rows[i][j].ToString();
                }

                Lst_List.Items.Add((Strings.Mid(CKSI0090.G_Kensyu_Area[i, 28], 1, 15) + "　" + Strings.Mid(CKSI0090.G_Kensyu_Area[i, 16], 1, 2) + "　" + IPPAN.Money_Hensyu(Strings.Trim(CKSI0090.G_Kensyu_Area[i, 17]), "##,###,##0") + "　" + IPPAN.Money_Hensyu(Strings.Trim(CKSI0090.G_Kensyu_Area[i, 18]), "##,###,##0.000") + "　" + IPPAN.Money_Hensyu(Strings.Trim(CKSI0090.G_Kensyu_Area[i, 19]), "#,###,###,##0") + "　" + Strings.Mid(CKSI0090.G_Kensyu_Area[i, 8], 1, 2) + "　" + Strings.Mid(CKSI0090.G_Kensyu_Area[i, 10], 1, 2) + "　" + Strings.Mid(CKSI0090.G_Kensyu_Area[i, 12], 1, 2)));
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }
    }
}
