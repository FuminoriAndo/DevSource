using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Windows.Forms;
using System.Data.Odbc;

namespace Project1
{
    /// <summary>
    /// 注文一覧表出力
    /// </summary>
    public partial class FRM_CKSI0110M : System.Windows.Forms.Form
    {
        private const int PC_BTNIDX_PRINT = 1;
        private const int PC_BTNIDX_ORDER = 2;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FRM_CKSI0110M());
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

            DataTable tbl = null;
            string[] L_MST_AREA = new string[2];

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
                //副資材コントロールマスタが見つかりません。
                IPPAN.Error_Msg("E700", 0, " ");
                return functionReturnValue;
            }

            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                //項目セット
                L_MST_AREA[j] = tbl.Rows[0][j].ToString();
            }

            //検収年月
            CKSI0110_01.G_NENGETU = Strings.Trim(Strings.Mid(L_MST_AREA[1], 1, 6));

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

        public short GET_DATA(string L_GYOSYACD, int L_KBN)
        {
            short functionReturnValue = 1;
            DataTable tbl = null;
            string[] L_AREA = new string[2];

            //副資材検収トランの検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "Select COUNT(KENSYUNO) ";
            IPPAN.G_SQL = IPPAN.G_SQL + "from SIZAI_KENSYU_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE SIMEYM = '" + CKSI0110_01.G_NENGETU + "' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "AND KENSYUFLG = '1' ";
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

            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                //項目セット
                L_AREA[j] = tbl.Rows[0][j].ToString();
            }

            functionReturnValue = Convert.ToInt16(L_AREA[0]);
            return functionReturnValue;
        }

        public short DATA_SYUSEI(string L_GYOSYACD_1, int L_KBN)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;
            //読込データ処理用ワーク
            string[] L_AREA = new string[5];

            //対象データ件数カウンター
            int L_COUNTER = 0;
            //対象データページカウンター
            int L_PAGE = 0;
            //ページ毎件数(明細行)
            int L_PAGE_MAX = 0;
            //最初フラグ
            bool L_SAISYO = false;
            string L_KENSYUNO = string.Empty;
            string L_GYOSYACD = string.Empty;
            string L_JYOKENCD = string.Empty;
            string L_HACHUNO = string.Empty;

            L_COUNTER = 0;
            L_PAGE = 1;
            L_PAGE_MAX = 20;
            L_SAISYO = true;

            L_KENSYUNO = "";
            L_GYOSYACD = "";
            L_JYOKENCD = "";
            L_HACHUNO = "";

            //副資材検収トランの検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "Select KENSYUNO,GYOSYACD,JYOKENCD,HACHUNO ";
            IPPAN.G_SQL = IPPAN.G_SQL + "from SIZAI_KENSYU_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE SIMEYM = '" + CKSI0110_01.G_NENGETU + "' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "AND KENSYUFLG = '1' ";
            if (L_KBN != 0)
            {
                IPPAN.G_SQL = IPPAN.G_SQL + "AND GYOSYACD = '" + L_GYOSYACD_1 + "' ";
            }
            IPPAN.G_SQL = IPPAN.G_SQL + "ORDER BY ";
            if (L_KBN == 0)
            {
                IPPAN.G_SQL = IPPAN.G_SQL + "GYOSYACD ASC, ";
            }
            IPPAN.G_SQL = IPPAN.G_SQL + "JYOKENCD ASC,HACHUNO DESC ";

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
                L_COUNTER = L_COUNTER + 1;
                if (L_COUNTER > L_PAGE_MAX)
                {
                    L_PAGE = L_PAGE + 1;
                    L_COUNTER = 1;
                }

                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    //項目セット
                    L_AREA[j] = tbl.Rows[i][j].ToString();
                }

                //検収№
                L_KENSYUNO = Strings.Mid(L_AREA[0], 1, 12);

                if ((L_SAISYO == true) || (L_GYOSYACD != Strings.Mid(L_AREA[1], 1, 4) || L_JYOKENCD != Strings.Mid(L_AREA[2], 1, 2)))
                {

                    //業者CD
                    L_GYOSYACD = Strings.Mid(L_AREA[1], 1, 4);
                    //条件CD
                    L_JYOKENCD = Strings.Mid(L_AREA[2], 1, 2);
                    //発注№
                    L_HACHUNO = Strings.Mid(L_AREA[3], 1, 12);

                    if (IPPAN.C_Allspace(L_HACHUNO) == 0)
                    {
                        using (C_ODBC db = new C_ODBC())
                        {
                            try
                            {
                                //DB接続
                                db.Connect();
                                //トランザクション開始
                                db.BeginTrans();
                                IPPAN.G_RET = GYOMU.Hakko_Kensaku(db, "SS");
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
                        //発注№発番
                        L_HACHUNO = GYOMU.G_SEQNO;
                    }

                    if (L_SAISYO == true)
                    {
                        L_SAISYO = false;
                    }
                    else
                    {
                        L_PAGE = L_PAGE + 1;
                        L_COUNTER = 1;
                    }

                }

                IPPAN.G_RET = DATA_KOSIN(L_KENSYUNO, L_HACHUNO, L_PAGE);
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private short DATA_KOSIN(string L_KENSYUNO, string L_HACHUNO, int L_PAGE)
        {
            short functionReturnValue = 1;

            IPPAN.G_datetime = String.Format("{0:yyyyMMdd HH:mm:ss}", DateTime.Now);

            //副資材検収トランの更新
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "UPDATE SIZAI_KENSYU_TRN SET ";
            IPPAN.G_SQL = IPPAN.G_SQL + "HACHUNO = '" + L_HACHUNO + "', ";
            IPPAN.G_SQL = IPPAN.G_SQL + "PAGENO = " + L_PAGE + " ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE SIMEYM = '" + CKSI0110_01.G_NENGETU + "' AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "KENSYUNO = '" + L_KENSYUNO + "' ";

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    //DB接続
                    db.Connect();
                    //トランザクション開始
                    db.BeginTrans();
                    //SQL実行
                    db.ExecSQL(IPPAN.G_SQL);
                    //コミット
                    db.Commit();
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

            functionReturnValue = 0;
            return functionReturnValue;
        }

        //業者マスタ読込み処理
        public short GYOSYA_KENSAKU(string L_CD)
        {
            short functionReturnValue = 1;
            DataTable tbl = null;

            string[] L_MST_AREA = new string[2];

            //業者コードをｷｰにﾃﾞｰﾀﾍﾞｰｽ検索
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

        private void CMD_BUTTON_Click(System.Object eventSender, System.EventArgs eventArgs)
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
                case 2:
                    //印刷・発注№採番
                    IPPAN.G_RET = IPPAN.C_Allspace((TXT_GYOSYACD.Text));
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

                        IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);

                        IPPAN.G_RET = DATA_SYUSEI("", 0);

                        IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);

                        if (IPPAN.G_RET != 0)
                        {
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

                        IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);

                        IPPAN.G_RET = DATA_SYUSEI(TXT_GYOSYACD.Text, 1);

                        IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);

                        if (IPPAN.G_RET != 0)
                        {
                            return;
                        }
                    }

                    //発注№採番ボタンのときはリストは出さない
                    if (Index == PC_BTNIDX_ORDER)
                    {
                        C_COMMON.Msg("発注№の採番を完了しました");
                        return;
                    }

                    //抽出条件
                    sSql = "SELECT SIMEYM, HACHUNO, GYOSYANM, HINMOKUNM, ";
                    sSql = sSql + " SURYO, TANI, TANKA, KINGAKU, HINMOKUNM2, DELFLG ";
                    sSql = sSql + " FROM SIZAI_KENSYU_TRN ";
                    sSql = sSql + " WHERE DELFLG=' ' ";
                    sSql = sSql + " AND SIMEYM='" + CKSI0110_01.G_NENGETU + "' ";
                    sSql = sSql + " AND KENSYUFLG='1' ";
                    if (IPPAN.C_Allspace(TXT_GYOSYACD.Text) != 0)
                    {
                        sSql = sSql + " AND GYOSYACD='" + TXT_GYOSYACD.Text + "' ";
                    }
                    sSql = sSql + " ORDER BY HACHUNO, GYOSYANM, HINMOKUNM";

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
                        CrystalDecisions.CrystalReports.Engine.ReportDocument CRP_REP = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        CRP_REP.Load("CKSI0110.rpt");
                        //データセット設定
                        CRP_REP.SetDataSource(tbl);
                        //印刷部数：1部
                        //出力先：プリンター
                        //印刷開始：1部、部単位に印刷しない、全ページ印刷
                        CRP_REP.PrintToPrinter(1, false, 0, 0);
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
            //CMD_BUTTON
            for (short i = 0; i < this.CMD_BUTTON.Count(); i++)
            {
                this.CMD_BUTTON[i].Click += new EventHandler(CMD_BUTTON_Click);
            }
        }

        private void FRM_CKSI0110M_Load(object eventSender, System.EventArgs eventArgs)
        {
            string L_Search = string.Empty;
            string L_Command2 = string.Empty;

            //プロジェクト名を設定
            C_COMMON.G_COMMON_MSGTITLE = "CKSI0110";

            //コントロール配列にイベントを設定
            setEventCtrlAry();

            Show();

            //待機する秒数
            const short Waittime = 1;
            //起動時刻を待避
            DateTime now_old = DateTime.Now;

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
                //コマンド ライン引数が指定されていないとき。
                //コマンド ライン引数が指定されていません
                IPPAN.Error_Msg("E507", 0, " ");
                System.Environment.Exit(0);
            }
            else
            {
                //コマンド ライン引数の受け渡し
                CKSI0110_01.G_USERID = Strings.Mid(IPPAN.G_COMMAND, 1, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) - 1);
                L_Command2 = Strings.Mid(IPPAN.G_COMMAND, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) + 1, 30);
                CKSI0110_01.G_OFFICEID = Strings.Mid(L_Command2, 1, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) - 1);
                CKSI0110_01.G_SYOKUICD = Strings.Mid(L_Command2, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) + 1, 2);
                IPPAN.G_RET = IPPAN2.TIMER_SET(CKSI0110_01.G_USERID);
                if (IPPAN.G_RET != 0)
                {
                    System.Environment.Exit(0);
                }
            }

            //画面初期化
            GAMEN_CLEAR();

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
            //2013.09.03 DSK yoshida start
            //LBL_NEN.Text = Strings.Mid(CKSI0110_01.G_NENGETU, 3, 2);
            LBL_NEN.Text = Strings.Mid(CKSI0110_01.G_NENGETU, 1, 4);
            //2013.09.03 DSK yoshida end
            LBL_TUKI.Text = Strings.Mid(CKSI0110_01.G_NENGETU, 5, 2);

            // 初期フォーカス設定
            TXT_GYOSYACD.Focus();
        }

        private void TMR_TIMER_Tick(object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Control_Init(this, "注文一覧表出力", "CKSI0110", Convert.ToString(SO.SO_USERNAME), Convert.ToString(SO.SO_OFFICENAME), "1.00");
        }

        private void TXT_GYOSYACD_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
            //入力前状態保存
            CKSI0110_01.G_CD = TXT_GYOSYACD.Text;
        }

        private void TXT_GYOSYACD_Leave(object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);

            int num = 0;
            if (int.TryParse(TXT_GYOSYACD.Text, out num))
            {
                TXT_GYOSYACD.Text = String.Format("{0:0000}", num);
            }

            //内容が変更されていて且つ空白じゃないとき
            if (CKSI0110_01.G_CD != TXT_GYOSYACD.Text & IPPAN.C_Allspace(TXT_GYOSYACD.Text) != 0)
            {
                //入力禁止文字チェック
                IPPAN.G_RET = IPPAN.Input_Check(TXT_GYOSYACD.Text);
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
            else if (IPPAN.C_Allspace(TXT_GYOSYACD.Text) == 0)
            {
                //空白のとき
                LBL_GYOSYANM.Text = "";
            }
        }
    }
}
