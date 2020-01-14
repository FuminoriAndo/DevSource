using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Drawing.Printing;

namespace Project1
{
    internal partial class FRM_CKSI0140M : System.Windows.Forms.Form
    {
        //**************************************************************'
        //                                                              '
        //   プログラム名　　　購買要求申請画面処理                     '
        //                                                              '
        //   更新履歴
        //   更新日付    Rev     修正者      内容
        //   03.01.15           NTIS浅田　　Ｓ／Ｏサーバ更新の為、VB4.0→VB6.0SP5へのバージョンアップ対応
        //**************************************************************'
        private C_PRINT printDoc = null;
        /// <summary>
        /// プリンタ名：NEC PC-PR750/150R(168dpi)
        /// </summary>
        private const string PC_PRINT_NM = "NEC PC-PR750/150R(168dpi)";
        /// <summary>
        /// フォント名：ＭＳ 明朝
        /// </summary>
        private const string PC_PRINT_FONT_NM = "ＭＳ 明朝";
        /// <summary>
        /// フォントサイズ：12
        /// </summary>
        private const int PC_PRINT_FONT_SIZE = 12;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FRM_CKSI0140M());
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
            DialogResult dlgRlt = DialogResult.None;

            switch (Index)
            {
                case 0:
                    //閉じる
                    this.Close();
                    break;

                case 1:
                    //実行
                    dlgRlt = IPPAN.Error_Msg("I701", 4, " ");
                    //データを作成します。よろしいですか？
                    if (dlgRlt != DialogResult.Yes)
                    {
                        return;
                    }

                    CMD_Button[1].Enabled = false;
                    CMD_Button[0].Enabled = false;

                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);

                    LBL_MSG.Text = "副資材仕訳データを作成しています．．．";
                    System.Windows.Forms.Application.DoEvents();
                    if (Siwake_Kosin.Siwake_Sakusei(CKSI0140.G_KBYM) == 1)
                    {
                        //更新に失敗しました
                        IPPAN.Error_Msg("I503", 0, " ");
                        this.Close();
                        return;
                    }

                    LBL_MSG.Text = "コントロールマスタを更新しています．．．";
                    System.Windows.Forms.Application.DoEvents();

                    using (C_ODBC db = new C_ODBC())
                    {
                        try
                        {
                            //DB接続
                            db.Connect();
                            //トランザクション開始
                            db.BeginTrans();

                            if (Control_Kosin(db) == 1)
                            {
                                //更新に失敗しました
                                IPPAN.Error_Msg("I503", 0, " ");
                                //ロールバック
                                db.Rollback();
                                this.Close();
                                return;
                            }

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

                    CKKR0010.G_KENSU = Kensu_Kakunin("", CKKR0010.C_FLG0);
                    if (Convert.ToInt32(CKKR0010.G_KENSU) == 0)
                    {
                        C_COMMON.Msg("対象データはありません");
                    }
                    else
                    {
                        LBL_MSG.Text = "副資材仕訳確認リストを印刷しています．．．";
                        System.Windows.Forms.Application.DoEvents();
                        //印刷処理
                        IPPAN.G_RET = LIST_Insatu();
                    }

                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);

                    Txt_Listno.Text = Strings.Mid(GYOMU.G_SEQNO, 5, 8);
                    LBL_MSG.Text = "処理は終了しました。閉じるボタンを押して下さい。";
                    CMD_Button[0].Enabled = true;
                    CMD_Button[2].Enabled = true;
                    break;

                case 2:
                    //再印刷
                    if (IPPAN.C_Allspace(Txt_Listno.Text) == 0)
                    {
                        Txt_Listno.Focus();
                        Txt_Listno_Enter(Txt_Listno, new System.EventArgs());
                        //必須項目入力エラー
                        IPPAN.Error_Msg("E200", 0, " ");
                        return;
                    }
                    if (IPPAN.Input_Check(Txt_Listno.Text) == 1)
                    {
                        Txt_Listno.Focus();
                        Txt_Listno_Enter(Txt_Listno, new System.EventArgs());
                        //入力エラー
                        IPPAN.Error_Msg("E202", 0, " ");
                        return;
                    }

                    IPPAN.G_RET = Siwake_Check(Txt_Listno.Text);

                    CKKR0010.G_KENSU = Kensu_Kakunin(CKKR0010.G_LISTNO, CKKR0010.C_FLG1);
                    if (Convert.ToInt32(CKKR0010.G_KENSU) == 0)
                    {
                        C_COMMON.Msg("対象データはありません");
                        return;
                    }

                    //確認メッセージ
                    dlgRlt = IPPAN.Error_Msg("I008", 4, "");
                    if (dlgRlt != DialogResult.Yes)
                    {
                        return;
                    }

                    //はいのときはそのまま印刷
                    CMD_Button[0].Enabled = false;
                    CMD_Button[2].Enabled = false;

                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);

                    LBL_MSG.Text = "副資材仕訳確認リストを印刷しています．．．";

                    System.Windows.Forms.Application.DoEvents();

                    //印刷処理
                    IPPAN.G_RET = LIST_Insatu();

                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);

                    LBL_MSG.Text = "処理は終了しました。閉じるボタンを押して下さい。";

                    CMD_Button[0].Enabled = true;
                    CMD_Button[2].Enabled = true;
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
        }

        private void FRM_CKSI0140M_Load(System.Object eventSender, System.EventArgs eventArgs)
        {
            string L_Search = null;
            string L_Command2 = null;

            //プロジェクト名を設定
            C_COMMON.G_COMMON_MSGTITLE = "CKSI0140";

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
                CKSI0140.G_UserId = Strings.Mid(IPPAN.G_COMMAND, 1, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) - 1);
                L_Command2 = Strings.Mid(IPPAN.G_COMMAND, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) + 1, 30);
                CKSI0140.G_OfficeId = Strings.Mid(L_Command2, 1, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) - 1);
                CKSI0140.G_Syokuicd = Strings.Mid(L_Command2, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) + 1, 2);
                IPPAN.G_RET = IPPAN2.TIMER_SET(CKSI0140.G_UserId);
                if (IPPAN.G_RET != 0)
                {
                    System.Environment.Exit(0);
                }
            }
            if (CKSI0140.Control_Kensaku() == 1)
            {
                //副資材コントロールマスタが見つかりません
                IPPAN.Error_Msg("E700", 0, " ");
                System.Environment.Exit(0);
            }

            LBL_Kensyuyy.Text = Strings.Mid(CKSI0140.G_KBYM, 3, 2);
            LBL_Kensyumm.Text = Strings.Mid(CKSI0140.G_KBYM, 5, 2);

            if (CKSI0140.G_SIWAKEFLG == "1")
            {
                CMD_Button[1].Enabled = false;
                CMD_Button[2].Enabled = true;
            }
            else
            {
                CMD_Button[1].Enabled = true;
                CMD_Button[2].Enabled = false;
                Txt_Listno.Enabled = false;
            }
        }

        private void TMR_TIMER_Tick(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Control_Init(this, "副資材仕訳データ作成", "CKSI0140M", SO.SO_USERNAME, SO.SO_OFFICENAME, "1.00");
        }

        private short Control_Kosin(C_ODBC db)
        {
            short functionReturnValue = 1;
            string L_datetime = null;

            L_datetime = String.Format("{0:yyyyMMdd HH:mm:ss}", DateTime.Now);

            //仕訳作成済みＦＬＧを”１”にする
            IPPAN.G_SQL = "update SIZAI_CONTROL_MST set ";
            IPPAN.G_SQL = IPPAN.G_SQL + "UPDYMD = to_date('" + L_datetime + "', 'YYYY/MM/DD HH24:MI:SS')";
            IPPAN.G_SQL = IPPAN.G_SQL + ",SIWAKEFLG = '1' ";
            Debug.Print(IPPAN.G_SQL);

            try
            {
                //トランザクションは呼出し元で制御する
                //SQL実行
                db.ExecSQL(IPPAN.G_SQL);
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

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private void Txt_Listno_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void Txt_Listno_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private string Kensu_Kakunin(string WH, short flg)
        {
            string functionReturnValue = "0";
            string L_GM1 = "0";
            DataTable tbl = null;
// 2015.02.02 yoshitake OBIC7仕訳連携対応(step3) start.
            IPPAN.G_SQL = "Select Count(*) from SIWAKE_SINSEI2_KAIKEI_TRN ";
            //IPPAN.G_SQL = "Select Count(*) from SIWAKE_SINSEI2_TRN ";
// 2015.02.02 yoshitake OBIC7仕訳連携対応(step3) end.

            switch (flg)
            {
                case 0:
                    //検索対象件数チェック
                    IPPAN.G_SQL = IPPAN.G_SQL + "WHERE substr(SINNO,1,2) = 'SD' ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "and LISTNO = '            '";
                    break;
                case 1:
                    //再印刷件数チェック
                    IPPAN.G_SQL = IPPAN.G_SQL + "WHERE LISTNO = '" + WH + "'";
                    break;
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

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    //項目セット
                    GYOMU.G_TRAN_AREA[j] = tbl.Rows[i][j].ToString();
                }

                //件数
                L_GM1 = GYOMU.G_TRAN_AREA[0];
            }

            //正常終了
            functionReturnValue = L_GM1;
            return functionReturnValue;
        }

        public short LIST_Insatu()
        {
            short functionReturnValue = 1;

            //画面上のリスト№をチェックする
            switch (ListNo_Check())
            {
                case 0:
                    //リスト№あり(再印刷)
                    //再印刷
                    IPPAN.G_RET = Sai_Insatu();
                    if (IPPAN.G_RET != 0)
                    {
                        Txt_Listno.Focus();
                        return functionReturnValue;
                    }
                    break;

                case 1:
                    //リスト№なし(通常印刷)
                    //G_SEQNOに発番される
                    //自動発番
                    using (C_ODBC db = new C_ODBC())
                    {
                        try
                        {
                            //DB接続
                            db.Connect();
                            //トランザクション開始
                            db.BeginTrans();
                            IPPAN.G_RET = GYOMU.Hakko_Kensaku(db, "II");
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

                    //通常印刷
                    IPPAN.G_RET = Tuuzyo_Insatu("");
                    if (IPPAN.G_RET != 0)
                    {
                        return functionReturnValue;
                    }

                    using (C_ODBC db = new C_ODBC())
                    {
                        try
                        {
                            //DB接続
                            db.Connect();
                            //トランザクション開始
                            db.BeginTrans();
                            //仕訳トランの更新
                            IPPAN.G_RET = Siwake_Kousin(db, GYOMU.G_SEQNO);
                            if (IPPAN.G_RET != 0)
                            {
                                //ロールバック処理
                                db.Rollback();
                                return functionReturnValue;
                            }
                            else
                            {
                                //コミット処理
                                db.Commit();
                            }
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
                    break;

                default:
                    //エラー
                    return functionReturnValue;
            }
            functionReturnValue = 0;
            return functionReturnValue;
        }

        /// <summary>
        /// 印刷初期化処理
        /// </summary>
        /// <param name="WH">検索条件：LISTNO</param>
        /// <param name="flg">0：再印刷 1：通常印刷</param>
        /// <returns>true:正常 false:異常</returns>
        private Boolean PRINT_Init(string WH, short flg)
        {
            try
            {
                //印刷処理設定開始
                if (this.printDoc != null)
                {
                    this.printDoc.Dispose();
                    this.printDoc = null;
                }
                this.printDoc = new C_PRINT();

                //印刷クラスオブジェクト取得
                C_PRINT pdCls = this.printDoc;

                //初期化
                pdCls.Init();
                pdCls.setPrinter(PC_PRINT_NM);
                pdCls.setFont(PC_PRINT_FONT_NM, PC_PRINT_FONT_SIZE);
                pdCls.DocumentName = "CKSI0140";
                pdCls.setPrintEvent(PRINT_PrintPage);

                //プリンタ変数初期化
                //印刷タイプ取得
                CKKR0010.G_PRINT_TYPE = flg;
                //印刷成否判定
                CKKR0010.G_PRINT_CHECK = false;
                //最初フラグ
                CKKR0010.G_SAISYO = true;
                //最終頁印刷フラグ
                CKKR0010.G_PRINT_END = false;
                
                //基準X
                CKKR0010.G_X = 0f;
                //現在行数：最大印字行数
                CKKR0010.G_NW_LINE = 63;
                CKKR0010.G_GT_LINE = 63;
                //ページカウンタ
                CKKR0010.G_PAGE = 0;
                //総合計
                CKKR0010.G_GOKEI = 0;
                //小計
                CKKR0010.G_GINKEI = 0;
                //改頁銀行
                CKKR0010.G_NW_GINKO = Strings.Space(2);
                CKKR0010.G_SV_GINKO = Strings.Space(2);
                //比較領域の初期化
                CKKR0010.G_SV_HAKONO = Strings.Space(12);
                //比較領域の初期化
                CKKR0010.G_NW_HAKONO = Strings.Space(12);

                //印字に必要な項目を取得する
                IPPAN.G_SQL = "Select ";
                //                0       ,1       ,2          ,3         ,4        ,
                IPPAN.G_SQL = IPPAN.G_SQL + "S.LISTNO,S.KEIYMD,S.KAIDENNO1,S.SYOHYONO,S.DATAKBN,";
                //                5            ,6         ,7          ,8            ,9         ,
                IPPAN.G_SQL = IPPAN.G_SQL + "S.KRKAMOKUCD1,S.KRNAIYO1,S.KRKAMONM1,S.KSKAMOKUCD1,S.KSNAIYO1,";
                //                10         ,11     ,12       ,13     ,14     ,
                IPPAN.G_SQL = IPPAN.G_SQL + "S.KSKAMONM1,S.KRKIN,S.TEKIYO1,S.HINNM,S.SURYO,";
                //                15    ,16     ,17        ,18       ,19       ,
                IPPAN.G_SQL = IPPAN.G_SQL + "S.TANI,S.TANKA,S.GYOSYANM,S.GINKONM,S.SITENNM,";
                //                20      ,21    ,22     ,23     ,24     ,
                IPPAN.G_SQL = IPPAN.G_SQL + "S.KOZANO,S.SITE,S.TEGNO,S.SINNO,S.SEINO,";
                //                25     ,26       ,27       ,28       ,29           ,
                IPPAN.G_SQL = IPPAN.G_SQL + "S.SEINO,S.ZEIKBN1,S.TEKIYO2,S.ZEIKBN2,S.KRKAMOKUCD2,";
                //                30        ,31         ,32           ,33        ,34         ,
                IPPAN.G_SQL = IPPAN.G_SQL + "S.KRNAIYO2,S.KRKAMONM2,S.KSKAMOKUCD2,S.KSNAIYO2,S.KSKAMONM2,";
                //                35      ,36     ,37     ,38     ,39     ,
                IPPAN.G_SQL = IPPAN.G_SQL + "S.ZEIKIN,S.KARI1,S.KASI1,S.KARI2,S.KASI2,";
                //                40         ,41        ,42     ,43        ,44       ,
                IPPAN.G_SQL = IPPAN.G_SQL + "S.KAIDENNO2,S.GOKEIKIN,S.KARI3,J.JYOKENNM,S.JOKENCD,";
                //                45     ,46       ,47       ,48       ,49       ,
                IPPAN.G_SQL = IPPAN.G_SQL + "S.KASI3,S.KARICD1,S.KASICD1,S.KARICD2,S.KASICD2,";
                //                50        ,51        ,52       ,53      ,54       ,
                IPPAN.G_SQL = IPPAN.G_SQL + "S.KRKIJITU,S.KSKIJITU,G.SYUBETU,G.KOZANM,G.GINKONM,";
                //                55       ,56      ,57        ,58          ,59
                IPPAN.G_SQL = IPPAN.G_SQL + "G.SITENNM,G.KOZANO,S.YOKINKBN,S.ZEIRITUKBN,S.KRACKBN1,";
                //                60        ,61        ,62       ,63      ,64      ,65
                IPPAN.G_SQL = IPPAN.G_SQL + "S.KSACKBN1,S.GYOSYACD,S.YOSANNO,S.YBFLG1,S.YBFLG2,S.YBFLG3,";
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
                //                66          ,67       ,68      ,69      ,70      ,71
                IPPAN.G_SQL = IPPAN.G_SQL + "S.KRGYOSYACD,G2.KOZANM,S.SYONM5,S.SYONM6,S.SYONM7,ZEIRITU_HOJO_KBN";
                ////                66          ,67       ,68      ,69      ,70
                //IPPAN.G_SQL = IPPAN.G_SQL + "S.KRGYOSYACD,G2.KOZANM,S.SYONM5,S.SYONM6,S.SYONM7";
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end
// 2015.02.02 yoshitake OBIC7仕訳連携対応(step3) start.
                IPPAN.G_SQL += " from SIWAKE_SINSEI2_KAIKEI_TRN S LEFT JOIN JYOKEN_MST J ON S.JOKENCD = J.JYOKENCD ";
                //IPPAN.G_SQL = IPPAN.G_SQL + " from SIWAKE_SINSEI2_TRN S LEFT JOIN JYOKEN_MST J ON S.JOKENCD = J.JYOKENCD ";
// 2015.02.02 yoshitake OBIC7仕訳連携対応(step3) end.
                IPPAN.G_SQL = IPPAN.G_SQL + " LEFT JOIN GYOSYA_MST G ON S.GYOSYACD = G.GYOSYACD ";
                IPPAN.G_SQL = IPPAN.G_SQL + " LEFT JOIN GYOSYA_MST G2 ON S.GYOSYACD = G2.GYOSYACD ";

                switch (flg)
                {
                    case 0:
                        //リスト№あり(再印刷)
                        IPPAN.G_SQL = IPPAN.G_SQL + "WHERE LISTNO = '" + WH + "'";
                        break;
                    case 1:
                        //リスト№なし(通常印刷)
                        IPPAN.G_SQL = IPPAN.G_SQL + "WHERE SINNO like 'SD__________' ";
                        IPPAN.G_SQL = IPPAN.G_SQL + "AND S.LISTNO = '            ' ";
                        break;
                }

                IPPAN.G_SQL = IPPAN.G_SQL + " ORDER BY S.KEIYMD ASC,S.KAIDENNO1 ASC";
                Debug.Print(IPPAN.G_SQL);

                using (C_ODBC db = new C_ODBC())
                {
                    try
                    {
                        //DB接続
                        db.Connect();
                        //SQL実行
                        pdCls.Records = db.ExecSQL(IPPAN.G_SQL);
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

                //印刷情報の取得
                IPPAN.G_RET = CKKR0010.GetPrintRecord(pdCls.Records, 0);
                //該当データなし
                if (IPPAN.G_RET == 1)
                {
                    return false;
                }

                //GetPrintRecordにより1件目が設定されているので1から開始する
                pdCls.Count = 1;
            }
            catch (Exception)
            {
                C_COMMON.Msg("印刷初期設定に失敗しました。");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 印刷処理
        /// </summary>
        /// <param name="sender">イベントのソース</param>
        /// <param name="e">イベント データを格納している PrintPageEventArgs</param>
        public void PRINT_PrintPage(object sender, PrintPageEventArgs e)
        {
            //在庫棚卸調査表印刷
            try
            {
                //印刷クラスオブジェクト取得
                C_PRINT pdCls = this.printDoc;

                //検索レコード取得
                DataTable tbl = pdCls.Records;

                //読込カウンタ取得
                int nCnt = pdCls.Count;
                bool bPrintLoop = true;

                //データなし
                if (tbl == null)
                {
                    return;
                }
                else
                {
                    //見出し印字
                    CKKR0010.MIDASI_INSATU(pdCls, e);

                    //最終頁印刷判定
                    if (CKKR0010.G_PRINT_END == true)
                    {
                        //総合計印刷
                        CKKR0010.GOKEI_INSATU(pdCls, e);
                        //印刷正常終了
                        CKKR0010.G_PRINT_CHECK = true;
                        //印刷終了
                        e.HasMorePages = false;
                        return;
                    }

                    if (CKKR0010.G_SAISYO != true)
                    {
                        //明細データ移送
                        CKKR0010.SET_PR_ITEM(CKKR0010.G_PRINT_TYPE, true);

                        //印刷処理
                        CKKR0010.PR_START(pdCls, e);

                        //明細14行印字
                        CKKR0010.G_NW_LINE = CKKR0010.G_NW_LINE + 14;

                        //印刷情報取得
                        IPPAN.G_RET = CKKR0010.GetPrintRecord(tbl, nCnt);
                        //ページカウント
                        nCnt++;

                        //該当データなし
                        if (IPPAN.G_RET == 1)
                        {
                            bPrintLoop = false;
                        }
                    }
                }

                //対象のレコードがなくなるまで繰り返す。
                while (bPrintLoop)
                {
                    //明細データ移送
                    //銀行別改頁 START
                    CKKR0010.SET_PR_ITEM(CKKR0010.G_PRINT_TYPE, false);

                    //貸方内容１
                    CKKR0010.G_NW_GINKO = Strings.Mid(GYOMU.G_TRAN_AREA[9], 1, 2);

                    if ((CKKR0010.G_GINKOCHK == true) && (CKKR0010.G_NW_GINKO != CKKR0010.G_SV_GINKO))
                    {
                        //改ページ(最初はしない)
                        if (CKKR0010.G_SAISYO != true)
                        {
                            //小計
                            CKKR0010.G_GINKEI = CKKR0010.G_GINKEI - CKKR0010.G_KIN3;
                            //小計印刷
                            CKKR0010.SYOKEI_INSATU(pdCls, e);
                            //小計
                            CKKR0010.G_GINKEI = CKKR0010.G_KIN3;
                        }

                        //改頁条件保存
                        CKKR0010.G_SV_GINKO = CKKR0010.G_NW_GINKO;

                        //改ページ(最初はしない)
                        if (CKKR0010.G_SAISYO != true)
                        {
                            //継続して印刷(次ページ印刷)
                            e.HasMorePages = true;
                            //カウンタ再設定
                            pdCls.Count = nCnt;
                            return;
                        }
                    }

                    //行カウンタが最大印字行数を超えたとき
                    if (CKKR0010.G_NW_LINE >= CKKR0010.G_GT_LINE)
                    {
                        //継続して印刷(次ページ印刷)
                        e.HasMorePages = true;
                        //カウンタ再設定
                        pdCls.Count = nCnt;
                        return;
                    }

                    //印刷処理
                    CKKR0010.PR_START(pdCls, e);

                    //明細14行印字
                    CKKR0010.G_NW_LINE = CKKR0010.G_NW_LINE + 14;
                    if (CKKR0010.G_SAISYO == true)
                    {
                        CKKR0010.G_SAISYO = false;
                    }

                    //印刷情報取得
                    IPPAN.G_RET = CKKR0010.GetPrintRecord(tbl, nCnt);
                    //ページカウント
                    nCnt++;

                    //該当データなし
                    if (IPPAN.G_RET == 1)
                    {
                        bPrintLoop = false;
                        break;
                    }
                }

                //小計印刷
                if ((CKKR0010.G_GINKOCHK == true))
                {
                    //小計印刷
                    CKKR0010.SYOKEI_INSATU(pdCls, e);
                }

                //最終頁印刷
                CKKR0010.G_PRINT_END = true;

                //必ず改ページ
                e.HasMorePages = true;
            }
            catch (Exception)
            {
                //印刷終了
                e.HasMorePages = false;
                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                C_COMMON.Msg("印刷時にエラーが発生しました。印刷を中断します。");
                return;
            }
        }

        //
        //   印刷処理（再印刷）
        //
        private short Sai_Insatu()
        {
            short functionReturnValue = 1;

            //ページカウンタ初期化
            CKKR0010.G_PAGE = 0;

            //再印刷ルーチン開始
            //印刷初期設定
            if (this.PRINT_Init(CKKR0010.G_LISTNO, CKKR0010.C_SAI))
            {
                System.Threading.Thread t = new System.Threading.Thread(
                            new System.Threading.ThreadStart(this.printDoc.Print));
                //印刷開始
                t.Start();
                //終了まで待つ
                t.Join();
            }
            if (CKKR0010.G_PRINT_CHECK != true)
            {
                return functionReturnValue;
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        //
        //   印刷処理（通常印刷）
        //
        private short Tuuzyo_Insatu(string WH)
        {
            short functionReturnValue = 1;

            //ページカウンタ初期化
            CKKR0010.G_PAGE = 0;

            //印刷初期設定
            if (this.PRINT_Init(WH, CKKR0010.C_TUZYO))
            {
                System.Threading.Thread t = new System.Threading.Thread(
                            new System.Threading.ThreadStart(this.printDoc.Print));
                //印刷開始
                t.Start();
                //終了まで待つ
                t.Join();
            }
            if (CKKR0010.G_PRINT_CHECK != true)
            {
                return functionReturnValue;
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        //
        //   リスト№チェック
        //
        //       戻り値　0 -> 正常 , 1 -> 未入力状態 , 255 -> エラー
        //
        //
        public short ListNo_Check()
        {
            short functionReturnValue = 255;

            //未入力チェック
            //空白のとき
            if (IPPAN.C_Allspace(Strings.Trim(Txt_Listno.Text)) == 0)
            {
                functionReturnValue = 1;
                return functionReturnValue;
            }
            //数字チェック
            if (IPPAN.Numeric_Check(Strings.Trim(Txt_Listno.Text)) != 0)
            {
                //入力エラー
                IPPAN.Error_Msg("E202", 0, "");
                Txt_Listno.Focus();
                return functionReturnValue;
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public short Siwake_Kousin(C_ODBC db, string SEQNO)
        {
            short functionReturnValue = 1;
            string L_YMD = null;

            L_YMD = String.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);
// 2015.02.02 yoshitake OBIC7仕訳連携対応(step3) start.
            IPPAN.G_SQL = "UPDATE SIWAKE_SINSEI2_KAIKEI_TRN SET ";
            //IPPAN.G_SQL = "UPDATE SIWAKE_SINSEI2_TRN SET ";
// 2015.02.02 yoshitake OBIC7仕訳連携対応(step3) end.
            IPPAN.G_SQL = IPPAN.G_SQL + "LISTNO = '" + SEQNO + "',";
            IPPAN.G_SQL = IPPAN.G_SQL + "UPDYMD = to_date('" + L_YMD + "', 'YYYY/MM/DD HH24:MI:SS') ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE SINNO like 'SD__________' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "AND LISTNO = '            '";

            try
            {
                //トランザクションは呼出し元で制御する
                //SQL実行
                db.ExecSQL(IPPAN.G_SQL);
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

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public short Siwake_Check(string LNO)
        {
            short functionReturnValue = 1;
            string L_LISTNO = null;
            string L_YMD = null;

            L_YMD = Strings.Left(LNO, 4) + "01";
            IPPAN.G_RET = IPPAN.Date_Henkan(ref L_YMD);
            L_LISTNO = "II" + Strings.Left(L_YMD, 6) + Strings.Mid(LNO, 5, 4);
            CKKR0010.G_LISTNO = L_LISTNO;

            functionReturnValue = 0;
            return functionReturnValue;
        }
    }
}
