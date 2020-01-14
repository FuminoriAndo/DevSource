using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.Odbc;

namespace Project1
{
    internal partial class FRM_CKSI0120M : System.Windows.Forms.Form
    {
        //*************************************************************************************
        //
        //
        //   プログラム名　　　正規注文書発行
        //
        //
        //   修正履歴
        //
        //   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
        //   03.01.15           NTIS浅田　　Ｓ／Ｏサーバ更新の為、VB4.0→VB6.0SP5へのバージョンアップ対応
        //   13.08.06           HIT綱本      西暦4桁対応
        //*************************************************************************************
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FRM_CKSI0120M());
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

        public short GET_NENGETU()
        {
            short functionReturnValue = 1;

            string[] L_MST_AREA = new string[2];
            DataTable tbl = null;

            //副資材コントロールマスタの検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "Select ID,KBYM from SIZAI_CONTROL_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE ID = 'CKSI' ";

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    //DB接続
                    db.Connect();
                    //SQL実行
                    tbl = db.ExecSQL(IPPAN.G_SQL);
                }
                catch (Exception)
                {
                    db.Error();
                }
            }

            //結果の列数を取得する
            if (tbl.Columns.Count < 1)
            {
                return functionReturnValue;
            }

            //ﾃﾞｰﾀがない
            if (tbl.Rows.Count == 0)
            {
                return functionReturnValue;
            }

            //行の各列のﾃﾞｰﾀの値を取得する
            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                //項目をﾜｰｸにｾｯﾄ
                L_MST_AREA[j] = tbl.Rows[0][j].ToString();
            }

            //検収年月
            CKSI0120_01.G_NENGETU = Strings.Trim(Strings.Mid(L_MST_AREA[1], 1, 6));

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private void GAMEN_CLEAR()
        {
            LBL_NEN.Text = "";
            LBL_TUKI.Text = "";

            TXT_GYOSYACD.Text = "";
            LBL_GYOSYANM.Text = "";
        }

        public short GET_DATA(string L_GYOSYACD, short L_KBN)
        {
            short functionReturnValue = 1;

            string[] L_AREA = new string[2];
            DataTable tbl = null;

            //副資材検収トランの検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "Select COUNT(KENSYUNO) ";
            IPPAN.G_SQL = IPPAN.G_SQL + "from SIZAI_KENSYU_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE SIMEYM = '" + CKSI0120_01.G_NENGETU + "' ";
            if (L_KBN != 0)
            {
                IPPAN.G_SQL = IPPAN.G_SQL + "AND GYOSYACD = '" + L_GYOSYACD + "' ";
            }

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    //DB接続
                    db.Connect();
                    //SQL実行
                    tbl = db.ExecSQL(IPPAN.G_SQL);
                }
                catch (Exception)
                {
                    db.Error();
                }
            }

            //結果の列数を取得する
            if (tbl.Columns.Count < 1)
            {
                return functionReturnValue;
            }

            //行の各列のﾃﾞｰﾀの値を取得する
            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                //項目をﾜｰｸにｾｯﾄ
                L_AREA[j] = tbl.Rows[0][j].ToString();
            }

            functionReturnValue = Convert.ToInt16(L_AREA[0]);
            return functionReturnValue;
        }

        // 業者マスタ読込み処理
        public short GYOSYA_KENSAKU(string L_CD)
        {
            short functionReturnValue = 1;

            string[] L_MST_AREA = new string[2];
            DataTable tbl = null;

            // 業者コードをｷｰにﾃﾞｰﾀﾍﾞｰｽ検索
            IPPAN.G_SQL = "Select GYOSYACD,KOZANM from GYOSYA_MST WHERE GYOSYACD = '" + L_CD + "'";

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    //DB接続
                    db.Connect();
                    //SQL実行
                    tbl = db.ExecSQL(IPPAN.G_SQL);
                }
                catch (Exception)
                {
                    db.Error();
                }
            }

            //結果の列数を取得する
            if (tbl.Columns.Count < 1)
            {
                return functionReturnValue;
            }

            //ﾃﾞｰﾀがない
            if (tbl.Rows.Count == 0)
            {
                return functionReturnValue;
            }

            //行の各列のﾃﾞｰﾀの値を取得する
            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                //項目をﾜｰｸにｾｯﾄ
                L_MST_AREA[j] = tbl.Rows[0][j].ToString();
            }

            LBL_GYOSYANM.Text = Strings.Mid(L_MST_AREA[1], 1, 20);

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private void CMD_Button_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = CMD_BUTTON.GetIndex((Button)eventSender);
            string sSql = "";
            DataTable tbl = null;

            switch (Index)
            {
                case 0:
                    //閉じる
                    this.Close();
                    break;
                case 1:
                    //印刷
                    IPPAN.G_RET = IPPAN.C_Allspace(TXT_GYOSYACD.Text);
                    if (IPPAN.G_RET == 0)
                    {
                        //対象データ存在チェック
                        IPPAN.G_RET = GET_DATA("", 0);
                        if (IPPAN.G_RET == 0)
                        {
                            //対象データがありません
                            IPPAN.Error_Msg("E536", 0, " ");
                            return;
                        }
                    }
                    else
                    {
                        //入力禁止文字チェック
                        if (IPPAN.Input_Check(TXT_GYOSYACD.Text) != 0)
                        {
                            LBL_GYOSYANM.Text = "";
                            TXT_GYOSYACD.Focus();
                            TXT_GYOSYACD_Enter(TXT_GYOSYACD, new System.EventArgs());
                            //入力エラー
                            IPPAN.Error_Msg("E202", 0, " ");
                            return;
                        }
                        //業者名存在チェック
                        else if (GYOSYA_KENSAKU(TXT_GYOSYACD.Text) != 0)
                        {
                            LBL_GYOSYANM.Text = "";
                            TXT_GYOSYACD.Focus();
                            TXT_GYOSYACD_Enter(TXT_GYOSYACD, new System.EventArgs());
                            //業者マスタが見つかりません
                            IPPAN.Error_Msg("E102", 0, " ");
                            return;
                        }
                        //対象データ存在チェック
                        IPPAN.G_RET = GET_DATA(TXT_GYOSYACD.Text, 1);
                        if (IPPAN.G_RET == 0)
                        {
                            //対象データがありません
                            IPPAN.Error_Msg("E536", 0, " ");
                            return;
                        }
                    }

                    //注文書の発行
                    //CKSI0120.rptの印刷
                    //抽出条件
                    sSql = "SELECT SIMEYM, HACHUNO, GYOSYANM, PAGENO, HINMOKUNM2, KENSYUNO, ";
                    sSql = sSql + "JYOKENNM, KINGAKU, TANKA, TANI, SURYO, SYOHIZEI, GKINGAKU, DELFLG ";
                    sSql = sSql + "FROM SIZAI_KENSYU_TRN ";
                    sSql = sSql + "WHERE DELFLG=' ' ";
                    sSql = sSql + "AND SIMEYM='" + CKSI0120_01.G_NENGETU + "' ";
                    sSql = sSql + "AND KENSYUFLG='1' ";
                    if (IPPAN.C_Allspace(TXT_GYOSYACD.Text) != 0)
                    {
                        sSql = sSql + " AND GYOSYACD='" + TXT_GYOSYACD.Text + "' ";
                    }
                    sSql = sSql + "ORDER BY HACHUNO, PAGENO, HINMOKUNM2, KENSYUNO";

                    using (C_ODBC db = new C_ODBC())
                    {
                        try
                        {
                            //DB接続
                            db.Connect();
                            //SQL実行
                            tbl = db.ExecSQL(sSql);
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

                    try
                    {
                        CrystalDecisions.CrystalReports.Engine.ReportDocument CRP_REP_0120 = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        //レポートファイル指定
                        CRP_REP_0120.Load("CKSI0120.rpt");
                        //データセット設定
                        CRP_REP_0120.SetDataSource(tbl);
                        //印刷部数：1部
                        //出力先：プリンター
                        //印刷開始：1部、部単位に印刷しない、全ページ印刷
                        CRP_REP_0120.PrintToPrinter(1, false, 0, 0);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    tbl.Clear();
                    tbl.Dispose();
                    tbl = null;

                    //注文請書の発行
                    //CKSI0125.rptの印刷
                    //抽出条件
                    sSql = "SELECT SIMEYM, HACHUNO, PAGENO, HINMOKUNM2, KENSYUNO, ";
                    sSql = sSql + "JYOKENNM, KINGAKU, TANI, TANKA, SURYO, SYOHIZEI, GKINGAKU, DELFLG ";
                    sSql = sSql + "FROM SIZAI_KENSYU_TRN ";
                    sSql = sSql + "WHERE DELFLG=' ' ";
                    sSql = sSql + "AND SIMEYM='" + CKSI0120_01.G_NENGETU + "' ";
                    sSql = sSql + "AND KENSYUFLG='1' ";
                    if (IPPAN.C_Allspace(TXT_GYOSYACD.Text) != 0)
                    {
                        sSql = sSql + " AND GYOSYACD='" + TXT_GYOSYACD.Text + "' ";
                    }
                    sSql = sSql + "ORDER BY HACHUNO, PAGENO, HINMOKUNM2, KENSYUNO ";

                    using (C_ODBC db = new C_ODBC())
                    {
                        try
                        {
                            //DB接続
                            db.Connect();
                            //SQL実行
                            tbl = db.ExecSQL(sSql);
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

                    try
                    {
                        CrystalDecisions.CrystalReports.Engine.ReportDocument CRP_REP_0125 = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        //レポートファイル指定
                        CRP_REP_0125.Load("CKSI0125.rpt");
                        //データセット設定
                        CRP_REP_0125.SetDataSource(tbl);
                        //印刷部数：1部
                        //出力先：プリンター
                        //印刷開始：1部、部単位に印刷しない、全ページ印刷
                        CRP_REP_0125.PrintToPrinter(1, false, 0, 0);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    break;
            }
        }

        private void setEventCtrlAry()
        {
            //コントロール配列にイベントを設定
            //CMD_Button
            for (short i = 0; i < this.CMD_BUTTON.Count(); i++)
            {
                this.CMD_BUTTON[i].Click += new EventHandler(CMD_Button_Click);
            }
        }

        private void FRM_CKSI0120M_Load(System.Object eventSender, System.EventArgs eventArgs)
        {
            string L_Search = null;
            string L_Command2 = null;

            //プロジェクト名を設定
            C_COMMON.G_COMMON_MSGTITLE = "CKSI0120";

            //コントロール配列にイベントを設定
            setEventCtrlAry();

            Show();

            //待機する秒数
            const short Waittime = 1;
            //起動時刻を待避
            System.DateTime now_old = DateTime.Now;

            //現在の時刻－起動時刻が待機する秒数になるまでループする
            while (!(DateTime.FromOADate(DateAndTime.Now.ToOADate() - Convert.ToDateTime(now_old).ToOADate()) >= DateAndTime.TimeSerial(0, 0, Waittime)))
            {
                System.Windows.Forms.Application.DoEvents();
            }

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
                CKSI0120_01.G_USERID = Strings.Mid(IPPAN.G_COMMAND, 1, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) - 1);
                L_Command2 = Strings.Mid(IPPAN.G_COMMAND, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) + 1, 30);
                CKSI0120_01.G_OFFICEID = Strings.Mid(L_Command2, 1, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) - 1);
                CKSI0120_01.G_SYOKUICD = Strings.Mid(L_Command2, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) + 1, 2);
                IPPAN.G_RET = IPPAN2.TIMER_SET(CKSI0120_01.G_USERID);
                if (IPPAN.G_RET != 0)
                {
                    System.Environment.Exit(0);
                }
            }

            //画面初期化
            GAMEN_CLEAR();

            this.TXT_GYOSYACD.Focus();

            //砂時計ポインタ
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
            //検収年月取得
            IPPAN.G_RET = GET_NENGETU();
            //標準ポインタ
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
            if (IPPAN.G_RET != 0)
            {
                return;
            }
            //13.08.06 tsunamoto 西暦4桁対応
            //LBL_NEN.Text = Strings.Mid(CKSI0120_01.G_NENGETU, 3, 2);
            LBL_NEN.Text = Strings.Mid(CKSI0120_01.G_NENGETU, 1, 4);
            //13.08.06 tsunamoto 西暦4桁対応　END
            LBL_TUKI.Text = Strings.Mid(CKSI0120_01.G_NENGETU, 5, 2);
        }

        private void TMR_TIMER_Tick(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Control_Init(this, "正規注文書発行", "CKSI0120", SO.SO_USERNAME, SO.SO_OFFICENAME, "1.00");
        }

        private void TXT_GYOSYACD_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
            //入力前状態保存
            CKSI0120_01.G_CD = TXT_GYOSYACD.Text;
        }

        private void TXT_GYOSYACD_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);

            TXT_GYOSYACD.Text = C_COMMON.FormatToNum(TXT_GYOSYACD.Text, "0000");

            //内容が変更されていて且つ空白じゃないとき
            if (CKSI0120_01.G_CD != TXT_GYOSYACD.Text && IPPAN.C_Allspace(TXT_GYOSYACD.Text) != 0)
            {

                IPPAN.G_RET = IPPAN.Input_Check(TXT_GYOSYACD.Text);
                //入力禁止文字チェック
                if (IPPAN.G_RET != 0)
                {
                    LBL_GYOSYANM.Text = "";
                    TXT_GYOSYACD.Focus();
                    TXT_GYOSYACD_Enter(TXT_GYOSYACD, new System.EventArgs());
                    //入力エラー
                    IPPAN.Error_Msg("E202", 0, " ");
                    return;
                }

                //業者名取得
                IPPAN.G_RET = GYOSYA_KENSAKU(TXT_GYOSYACD.Text);
                if (IPPAN.G_RET != 0)
                {
                    LBL_GYOSYANM.Text = "";
                    TXT_GYOSYACD.Focus();
                    TXT_GYOSYACD_Enter(TXT_GYOSYACD, new System.EventArgs());
                    //業者マスタが見つかりません
                    IPPAN.Error_Msg("E102", 0, " ");
                    return;
                }
            }
            //空白のとき
            else if (IPPAN.C_Allspace(TXT_GYOSYACD.Text) == 0)
            {
                LBL_GYOSYANM.Text = "";
            }
        }

    }
}
