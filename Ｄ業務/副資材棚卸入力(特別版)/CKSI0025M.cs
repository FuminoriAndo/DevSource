using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Diagnostics;

namespace Project1
{
    internal partial class FRM_CKSI0025M : System.Windows.Forms.Form
    {
        //*************************************************************************************
        //
        //
        //   プログラム名　　　副資材棚卸入力
        //
        //
        //   修正履歴
        //
        //   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
        //   03.01.15             NTIS浅田　　Ｓ／Ｏサーバ更新の為、VB4.0→VB6.0SP5へのバージョンアップ対応
        //   13.07.02             ISV-TRUC    棚卸年月（年）の入力内容が、２桁の数値文字列だった場合は、先頭に "20"を付加する。
        //   13.07.02             ISV-TRUC    ”向先”を判断して、品目ごとの入力位置に位置付けたい。
        //   13.08.19             HIT 綱本     向先へのフォーカス移動の初期化
        //   13.08.20           ISV-TRUC    向先へのフォーカス変更不具合対応
        //   13.08.21           HIT 綱本    向先へのフォーカス変更不具合対応
        //*************************************************************************************
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FRM_CKSI0025M());
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

        private void GAMEN_CLEAR()
        {
            short L_I = 0;

            LBL_NEN.Text = "";
            LBL_TUKI.Text = "";

            for (L_I = 0; L_I <= 9; L_I++)
            {
                TXT_HINMOKUCD[L_I].Text = "";
                LBL_HINMOKUNM[L_I].Text = "";
                TXT_GYOSYACD[L_I].Text = "";
                TXT_SFLG[L_I].Text = "";
                TXT_ZSOUKO[L_I].Text = "";
                TXT_ZEF[L_I].Text = "";
                TXT_ZLF[L_I].Text = "";
                TXT_ZCC[L_I].Text = "";
                TXT_ZSONOTA[L_I].Text = "";
                TXT_ZMETER[L_I].Text = "";
            }

            for (L_I = 0; L_I <= CKSI0025_01.G_WRK_MAX; L_I++)
            {
                CKSI0025_01.TANAOROSI[L_I].HINMOKUCD = "";
                CKSI0025_01.TANAOROSI[L_I].GYOSYACD = "";
                CKSI0025_01.TANAOROSI[L_I].HINMOKUNM = "";
                CKSI0025_01.TANAOROSI[L_I].GYOSYANM = "";
                CKSI0025_01.TANAOROSI[L_I].ZSOUKO = "";
                CKSI0025_01.TANAOROSI[L_I].ZEF = "";
                CKSI0025_01.TANAOROSI[L_I].ZLF = "";
                CKSI0025_01.TANAOROSI[L_I].ZCC = "";
                CKSI0025_01.TANAOROSI[L_I].ZSONOTA = "";
                CKSI0025_01.TANAOROSI[L_I].ZMETER = "";
            }

        }

        public short GAMEN_CHECK(short L_KBN)
        {
            short functionReturnValue = 1;

            short L_I = 0;
            short L_J = 0;

            for (L_I = 0; L_I <= 9; L_I++)
            {
                if ((IPPAN.C_Allspace(TXT_HINMOKUCD[L_I].Text) == 0 && IPPAN.C_Allspace(TXT_GYOSYACD[L_I].Text) == 0 && IPPAN.C_Allspace(TXT_SFLG[L_I].Text) == 0 && IPPAN.C_Allspace(TXT_ZSOUKO[L_I].Text) == 0 && IPPAN.C_Allspace(TXT_ZEF[L_I].Text) == 0 && IPPAN.C_Allspace(TXT_ZLF[L_I].Text) == 0 && IPPAN.C_Allspace(TXT_ZCC[L_I].Text) == 0 && IPPAN.C_Allspace(TXT_ZSONOTA[L_I].Text) == 0 && IPPAN.C_Allspace(TXT_ZMETER[L_I].Text) == 0) || (TXT_SFLG[L_I].Text == "9"))
                {
                    continue;
                }

                IPPAN.G_RET = IPPAN.C_Allspace(TXT_HINMOKUCD[L_I].Text);
                if (IPPAN.G_RET == 0)
                {
                    if (L_KBN == 1)
                    {
                        LBL_HINMOKUNM[L_I].Text = "";
                        TXT_HINMOKUCD[L_I].Focus();
                        TXT_HINMOKUCD_Enter(TXT_HINMOKUCD[L_I], new System.EventArgs());
                        //必須項目入力エラー
                        IPPAN.Error_Msg("E200", 0, " ");
                    }
                    return functionReturnValue;
                }

                IPPAN.G_RET = IPPAN.Input_Check(TXT_HINMOKUCD[L_I].Text);
                if (IPPAN.G_RET != 0)
                {
                    if (L_KBN == 1)
                    {
                        LBL_HINMOKUNM[L_I].Text = "";
                        TXT_HINMOKUCD[L_I].Focus();
                        TXT_HINMOKUCD_Enter(TXT_HINMOKUCD[L_I], new System.EventArgs());
                        //入力エラー
                        IPPAN.Error_Msg("E202", 0, " ");
                    }
                    return functionReturnValue;
                }

                IPPAN.G_RET = HINMOKU_KENSAKU(TXT_HINMOKUCD[L_I].Text, L_I);
                if (IPPAN.G_RET != 0)
                {
                    if (L_KBN == 1)
                    {
                        LBL_HINMOKUNM[L_I].Text = "";
                        TXT_HINMOKUCD[L_I].Focus();
                        TXT_HINMOKUCD_Enter(TXT_HINMOKUCD[L_I], new System.EventArgs());
                        //副資材品目マスタが見つかりません。
                        IPPAN.Error_Msg("E701", 0, " ");
                    }
                    return functionReturnValue;
                }

                if (CKSI0025_01.G_SYUBETU == "1")
                {
                    TXT_GYOSYACD[L_I].Text = Strings.Space(4);
                    TXT_GYOSYACD[L_I].Enabled = false;
                    CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + L_I].GYOSYACD = Strings.Space(4);
                    CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + L_I].GYOSYANM = IPPAN.Space_Set("", 20, 2);
                }
                else
                {
                    IPPAN.G_RET = IPPAN.C_Allspace(TXT_GYOSYACD[L_I].Text);
                    if (IPPAN.G_RET == 0)
                    {
                        if (L_KBN == 1)
                        {
                            TXT_GYOSYACD[L_I].Focus();
                            TXT_GYOSYACD_Enter(TXT_GYOSYACD[L_I], new System.EventArgs());
                            //必須項目入力エラー
                            IPPAN.Error_Msg("E200", 0, " ");
                        }
                        return functionReturnValue;
                    }

                    IPPAN.G_RET = IPPAN.Input_Check(TXT_GYOSYACD[L_I].Text);
                    if (IPPAN.G_RET != 0)
                    {
                        if (L_KBN == 1)
                        {
                            TXT_GYOSYACD[L_I].Focus();
                            TXT_GYOSYACD_Enter(TXT_GYOSYACD[L_I], new System.EventArgs());
                            //入力エラー
                            IPPAN.Error_Msg("E202", 0, " ");
                        }
                        return functionReturnValue;
                    }

                    IPPAN.G_RET = GYOSYA_KENSAKU(TXT_GYOSYACD[L_I].Text);
                    if (IPPAN.G_RET != 0)
                    {
                        if (L_KBN == 1)
                        {
                            TXT_GYOSYACD[L_I].Focus();
                            TXT_GYOSYACD_Enter(TXT_GYOSYACD[L_I], new System.EventArgs());
                            //業者マスタが見つかりません
                            IPPAN.Error_Msg("E102", 0, " ");
                        }
                        return functionReturnValue;
                    }
                }

                if (IPPAN.C_Allspace(TXT_ZSOUKO[L_I].Text) == 0 && IPPAN.C_Allspace(TXT_ZEF[L_I].Text) == 0 && IPPAN.C_Allspace(TXT_ZLF[L_I].Text) == 0 && IPPAN.C_Allspace(TXT_ZCC[L_I].Text) == 0 && IPPAN.C_Allspace(TXT_ZSONOTA[L_I].Text) == 0 && IPPAN.C_Allspace(TXT_ZMETER[L_I].Text) == 0)
                {
                    TXT_ZSOUKO[L_I].Focus();
                    TXT_ZSOUKO_Enter(TXT_ZSOUKO[L_I], new System.EventArgs());
                    C_COMMON.Msg("向先への数量が入力されていません");
                    return functionReturnValue;
                }

                if (Strings.Trim(TXT_ZSOUKO[L_I].Text) == "0" && Strings.Trim(TXT_ZEF[L_I].Text) == "0" && Strings.Trim(TXT_ZLF[L_I].Text) == "0" && Strings.Trim(TXT_ZCC[L_I].Text) == "0" && Strings.Trim(TXT_ZSONOTA[L_I].Text) == "0" && Strings.Trim(TXT_ZMETER[L_I].Text) == "0")
                {
                    TXT_ZSOUKO[L_I].Focus();
                    TXT_ZSOUKO_Enter(TXT_ZSOUKO[L_I], new System.EventArgs());
                    C_COMMON.Msg("向先への数量が入力されていません");
                    return functionReturnValue;
                }

                for (L_J = 0; L_J <= CKSI0025_01.G_WRK_MAX; L_J++)
                {
                    if (string.IsNullOrEmpty(CKSI0025_01.TANAOROSI[L_J].HINMOKUCD))
                    {
                        break;
                    }
                    if (L_J != (Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + L_I)
                    {
                        if (TXT_HINMOKUCD[L_I].Text == CKSI0025_01.TANAOROSI[L_J].HINMOKUCD && TXT_GYOSYACD[L_I].Text == CKSI0025_01.TANAOROSI[L_J].GYOSYACD && CKSI0025_01.TANAOROSI[L_J].SFLG != "9")
                        {
                            if (L_KBN == 1)
                            {
                                TXT_HINMOKUCD[L_I].Focus();
                                TXT_HINMOKUCD_Enter(TXT_HINMOKUCD[L_I], new System.EventArgs());
                                C_COMMON.Msg("入力内容が重複しています");
                            }
                            return functionReturnValue;
                        }
                    }
                }
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public short GET_COUNT(ref short L_COUNT)
        {
            short functionReturnValue = 1;

            short L_I = 0;

            for (L_I = 0; L_I <= CKSI0025_01.G_WRK_MAX; L_I++)
            {
                if (string.IsNullOrEmpty(CKSI0025_01.TANAOROSI[L_I].HINMOKUCD))
                {
                    break;
                }
                if (TXT_K_HINCD.Text == CKSI0025_01.TANAOROSI[L_I].HINMOKUCD)
                {
                    L_COUNT = L_I;
                    functionReturnValue = 0;
                    return functionReturnValue;
                }
            }
            return functionReturnValue;
        }

        public void GAMEN_SET(int L_PAGE, int L_KBN)
        {
            short L_I = 0;

            for (L_I = 0; L_I <= 9; L_I++)
            {
                TXT_HINMOKUCD[L_I].Text = "";
                LBL_HINMOKUNM[L_I].Text = "";
                TXT_GYOSYACD[L_I].Text = "";
                TXT_SFLG[L_I].Text = "";
                TXT_ZSOUKO[L_I].Text = "";
                TXT_ZEF[L_I].Text = "";
                TXT_ZLF[L_I].Text = "";
                TXT_ZCC[L_I].Text = "";
                TXT_ZSONOTA[L_I].Text = "";
                TXT_ZMETER[L_I].Text = "";
            }

            for (L_I = 0; L_I <= 9; L_I++)
            {
                CKSI0025_01.ST_OLD_KEY[L_I].HINMOKUCD = "";
                CKSI0025_01.ST_OLD_KEY[L_I].GYOSYACD = "";
            }

            for (L_I = 0; L_I <= 9; L_I++)
            {
                if (string.IsNullOrEmpty(CKSI0025_01.TANAOROSI[L_PAGE + L_I].HINMOKUCD))
                {
                    TXT_GYOSYACD[L_I].Enabled = true;
                    if (L_I == 0)
                    {
                        TXT_HINMOKUCD[L_I].Focus();
                        TXT_HINMOKUCD_Enter(TXT_HINMOKUCD[(L_I)], new System.EventArgs());
                    }
                }
                else
                {
                    TXT_HINMOKUCD[L_I].Text = CKSI0025_01.TANAOROSI[L_PAGE + L_I].HINMOKUCD;
                    LBL_HINMOKUNM[L_I].Text = Strings.Mid(CKSI0025_01.TANAOROSI[L_PAGE + L_I].HINMOKUNM, 1, 15);
                    CKSI0025_01.G_HINMOKUNM[L_I] = Strings.Mid(CKSI0025_01.TANAOROSI[L_PAGE + L_I].HINMOKUNM, 1, 20);
                    if (IPPAN.C_Allspace(CKSI0025_01.TANAOROSI[L_PAGE + L_I].GYOSYACD) == 0)
                    {
                        TXT_GYOSYACD[L_I].Text = Strings.Space(4);
                        TXT_GYOSYACD[L_I].Enabled = false;
                    }
                    else
                    {
                        TXT_GYOSYACD[L_I].Enabled = true;
                        TXT_GYOSYACD[L_I].Text = CKSI0025_01.TANAOROSI[L_PAGE + L_I].GYOSYACD;
                    }
                    CKSI0025_01.ST_OLD_KEY[L_I].HINMOKUCD = CKSI0025_01.TANAOROSI[L_PAGE + L_I].HINMOKUCD;
                    CKSI0025_01.ST_OLD_KEY[L_I].GYOSYACD = CKSI0025_01.TANAOROSI[L_PAGE + L_I].GYOSYACD;
                    if (L_KBN == 0)
                    {
                        TXT_SFLG[L_I].Text = "";
                    }
                    else
                    {
                        TXT_SFLG[L_I].Text = CKSI0025_01.TANAOROSI[L_PAGE + L_I].SFLG;
                    }
                    TXT_ZSOUKO[L_I].Text = String.Format("{0,7}", C_COMMON.FormatToNum(CKSI0025_01.TANAOROSI[L_PAGE + L_I].ZSOUKO, "###,##0"));
                    TXT_ZEF[L_I].Text = String.Format("{0,7}", C_COMMON.FormatToNum(CKSI0025_01.TANAOROSI[L_PAGE + L_I].ZEF, "###,##0"));
                    TXT_ZLF[L_I].Text = String.Format("{0,7}", C_COMMON.FormatToNum(CKSI0025_01.TANAOROSI[L_PAGE + L_I].ZLF, "###,##0"));
                    TXT_ZCC[L_I].Text = String.Format("{0,7}", C_COMMON.FormatToNum(CKSI0025_01.TANAOROSI[L_PAGE + L_I].ZCC, "###,##0"));
                    TXT_ZSONOTA[L_I].Text = String.Format("{0,7}", C_COMMON.FormatToNum(CKSI0025_01.TANAOROSI[L_PAGE + L_I].ZSONOTA, "###,##0"));
                    TXT_ZMETER[L_I].Text = String.Format("{0,7}", C_COMMON.FormatToNum(CKSI0025_01.TANAOROSI[L_PAGE + L_I].ZMETER, "###,##0"));
                }
            }
        }

        private string SetZeroString(string str)
        {
            string sRet = "0";

            //文字列が空の場合は「0」を返す
            if (!string.IsNullOrEmpty(str))
            {
                sRet = str;
            }

            return sRet;
        }

        private short WORK_KOUSIN(C_ODBC db, int L_PAGE)
        {
            short functionReturnValue = 1;

            short L_I = 0;

            for (L_I = 0; L_I <= 9; L_I++)
            {
                CKSI0025_01.TANAOROSI[L_PAGE + L_I].SFLG = TXT_SFLG[L_I].Text;
                CKSI0025_01.TANAOROSI[L_PAGE + L_I].ZSOUKO = SetZeroString(IPPAN2.Numeric_Hensyu3(TXT_ZSOUKO[L_I].Text));
                CKSI0025_01.TANAOROSI[L_PAGE + L_I].ZEF = SetZeroString(IPPAN2.Numeric_Hensyu3(TXT_ZEF[L_I].Text));
                CKSI0025_01.TANAOROSI[L_PAGE + L_I].ZLF = SetZeroString(IPPAN2.Numeric_Hensyu3(TXT_ZLF[L_I].Text));
                CKSI0025_01.TANAOROSI[L_PAGE + L_I].ZCC = SetZeroString(IPPAN2.Numeric_Hensyu3(TXT_ZCC[L_I].Text));
                CKSI0025_01.TANAOROSI[L_PAGE + L_I].ZSONOTA = SetZeroString(IPPAN2.Numeric_Hensyu3(TXT_ZSONOTA[L_I].Text));
                CKSI0025_01.TANAOROSI[L_PAGE + L_I].ZMETER = SetZeroString(IPPAN2.Numeric_Hensyu3(TXT_ZMETER[L_I].Text));
            }

            IPPAN.G_RET = KOSIN_MAIN(db, L_PAGE);
            if (IPPAN.G_RET != 0)
            {
                return functionReturnValue;
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private short KOSIN_MAIN(C_ODBC db, int L_PAGE)
        {
            short functionReturnValue = 1;

            short L_I = 0;
            short L_RET = 0;

            for (L_I = 0; L_I <= 9; L_I++)
            {
                IPPAN.G_RET = DATA_SAKUJYO(db, L_I);
            }

            for (L_I = 0; L_I <= 9; L_I++)
            {
                if (CKSI0025_01.TANAOROSI[L_PAGE + L_I].SFLG == "9")
                {
                    L_RET = SONZAI_KAKUNIN(L_PAGE + L_I);
                    IPPAN.G_RET = DATA_KOUSIN(db, 9, L_PAGE + L_I);
                }
                else
                {
                    if (IPPAN.C_Allspace(CKSI0025_01.TANAOROSI[L_PAGE + L_I].HINMOKUCD) != 0)
                    {
                        L_RET = SONZAI_KAKUNIN(L_PAGE + L_I);
                        if (L_RET == 0)
                        {
                            //更新判定のため、DATA_SAKUJYOにて削除済のレコードか判定する
                            if (IS_DELETED_RECORD(CKSI0025_01.TANAOROSI[L_PAGE + L_I].HINMOKUCD, CKSI0025_01.TANAOROSI[L_PAGE + L_I].GYOSYACD))
                            {
                                //削除済なので登録処理に変更する
                                L_RET = 1;
                            }
                        }
                        IPPAN.G_RET = DATA_KOUSIN(db, L_RET, L_PAGE + L_I);
                    }
                }
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        //DATA_SAKUJYOにより削除済のレコードであるか判定
        private bool IS_DELETED_RECORD(string sHinCd, string sGyoCd)
        {
            //true：削除済 false：対象外
            for (int i = 0; i <= 9; i++)
            {
                if ((string.Compare(CKSI0025_01.ST_OLD_KEY[i].HINMOKUCD, sHinCd) == 0)
                 && (string.Compare(CKSI0025_01.ST_OLD_KEY[i].GYOSYACD, sGyoCd) == 0))
                {
                    //削除済
                    return true;
                }
            }
            //対象外
            return false;
        }

        private short SONZAI_KAKUNIN(int L_COUNT)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //副資材棚卸トランの検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "Select COUNT(UPDYMD) from SIZAI_TANAOROSI_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE TANAYM = '" + CKSI0025_01.G_NENGETU + "' AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKUCD = '" + CKSI0025_01.TANAOROSI[L_COUNT].HINMOKUCD + "' AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "GYOSYACD = '" + CKSI0025_01.TANAOROSI[L_COUNT].GYOSYACD + "' ";

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
                CKSI0025_01.G_AREA[j] = "";
                CKSI0025_01.G_AREA[j] = tbl.Rows[0][j].ToString();
            }

            if (Convert.ToInt32(CKSI0025_01.G_AREA[0]) == 0)
            {
                return functionReturnValue;
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private short DATA_KOUSIN(C_ODBC db, int L_KBN, int L_COUNT)
        {
            short functionReturnValue = 1;

            IPPAN.G_datetime = String.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);

            //副資材棚卸トランの更新
            switch (L_KBN)
            {
                case 0:
                    //更新
                    IPPAN.G_SQL = "";
                    IPPAN.G_SQL = "UPDATE SIZAI_TANAOROSI_TRN SET ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKUNM = '" + IPPAN.Space_Set(Strings.Trim(Strings.Mid(CKSI0025_01.TANAOROSI[L_COUNT].HINMOKUNM, 1, 20)), 20, 2) + "', ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "GYOSYANM = '" + IPPAN.Space_Set(Strings.Trim(Strings.Mid(CKSI0025_01.TANAOROSI[L_COUNT].GYOSYANM, 1, 20)), 20, 2) + "', ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "ZSOUKO = " + CKSI0025_01.TANAOROSI[L_COUNT].ZSOUKO + ", ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "ZEF = " + CKSI0025_01.TANAOROSI[L_COUNT].ZEF + ", ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "ZLF = " + CKSI0025_01.TANAOROSI[L_COUNT].ZLF + ", ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "ZCC = " + CKSI0025_01.TANAOROSI[L_COUNT].ZCC + ", ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "ZSONOTA = " + CKSI0025_01.TANAOROSI[L_COUNT].ZSONOTA + ", ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "ZMETER = " + CKSI0025_01.TANAOROSI[L_COUNT].ZMETER + ", ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "UPDYMD = to_date('" + IPPAN.G_datetime + "', 'YYYY/MM/DD HH24:MI:SS') ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "WHERE TANAYM = '" + CKSI0025_01.G_NENGETU + "' AND ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKUCD = '" + CKSI0025_01.TANAOROSI[L_COUNT].HINMOKUCD + "' AND ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "GYOSYACD = '" + CKSI0025_01.TANAOROSI[L_COUNT].GYOSYACD + "' ";
                    break;
                case 1:
                    //新規
                    IPPAN.G_SQL = "";
                    IPPAN.G_SQL = "INSERT INTO SIZAI_TANAOROSI_TRN ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "( TANAYM,HINMOKUCD,GYOSYACD, ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKUNM,GYOSYANM,ZSOUKO, ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "ZEF,ZLF,ZCC,ZSONOTA,ZMETER, ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "ZYOBI1,ZYOBI2,UPDYMD ) ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "VALUES ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "('" + CKSI0025_01.G_NENGETU + "', ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "'" + CKSI0025_01.TANAOROSI[L_COUNT].HINMOKUCD + "', ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "'" + CKSI0025_01.TANAOROSI[L_COUNT].GYOSYACD + "', ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "'" + IPPAN.Space_Set(Strings.Trim(Strings.Mid(CKSI0025_01.TANAOROSI[L_COUNT].HINMOKUNM, 1, 20)), 20, 2) + "', ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "'" + IPPAN.Space_Set(Strings.Trim(Strings.Mid(CKSI0025_01.TANAOROSI[L_COUNT].GYOSYANM, 1, 20)), 20, 2) + "', ";
                    IPPAN.G_SQL = IPPAN.G_SQL + CKSI0025_01.TANAOROSI[L_COUNT].ZSOUKO + ", ";
                    IPPAN.G_SQL = IPPAN.G_SQL + CKSI0025_01.TANAOROSI[L_COUNT].ZEF + ", ";
                    IPPAN.G_SQL = IPPAN.G_SQL + CKSI0025_01.TANAOROSI[L_COUNT].ZLF + ", ";
                    IPPAN.G_SQL = IPPAN.G_SQL + CKSI0025_01.TANAOROSI[L_COUNT].ZCC + ", ";
                    IPPAN.G_SQL = IPPAN.G_SQL + CKSI0025_01.TANAOROSI[L_COUNT].ZSONOTA + ", ";
                    IPPAN.G_SQL = IPPAN.G_SQL + CKSI0025_01.TANAOROSI[L_COUNT].ZMETER + ", ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "0, ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "0, ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "to_date('" + IPPAN.G_datetime + "', 'YYYY/MM/DD HH24:MI:SS')) ";
                    break;
                case 9:
                    //削除
                    IPPAN.G_SQL = "";
                    IPPAN.G_SQL = "DELETE SIZAI_TANAOROSI_TRN ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "WHERE TANAYM = '" + CKSI0025_01.G_NENGETU + "' AND ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKUCD = '" + CKSI0025_01.TANAOROSI[L_COUNT].HINMOKUCD + "' AND ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "GYOSYACD = '" + CKSI0025_01.TANAOROSI[L_COUNT].GYOSYACD + "' ";
                    break;
            }

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

        private short DATA_SAKUJYO(C_ODBC db, int L_COUNT)
        {
            short functionReturnValue = 1;

            IPPAN.G_datetime = String.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);

            //副資材棚卸トランの削除
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "DELETE SIZAI_TANAOROSI_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE TANAYM = '" + CKSI0025_01.G_NENGETU + "' AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKUCD = '" + CKSI0025_01.ST_OLD_KEY[L_COUNT].HINMOKUCD + "' AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "GYOSYACD = '" + CKSI0025_01.ST_OLD_KEY[L_COUNT].GYOSYACD + "' ";

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

        public short GET_NENGETU()
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //副資材コントロールマスタの検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "Select NEN1,TUKI1 from HIZUKE_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE PKEY = 'DATE' ";

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
                CKSI0025_01.G_AREA[j] = "";
                CKSI0025_01.G_AREA[j] = tbl.Rows[0][j].ToString();
            }

            CKSI0025_01.G_NENGETU = Strings.Trim(Strings.Mid(CKSI0025_01.G_AREA[0], 1, 4)) + Strings.Trim(Strings.Mid(CKSI0025_01.G_AREA[1], 1, 2));

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public short GET_DATA(string L_TANAYM)
        {
            short functionReturnValue = 1;
            int L_I = 0;
            DataTable tbl = null;

            //副資材棚卸トランの検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "Select TANAYM,HINMOKUCD,GYOSYACD,";
            IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKUNM,GYOSYANM,ZSOUKO,";
            IPPAN.G_SQL = IPPAN.G_SQL + "ZEF,ZLF,ZCC,ZSONOTA,ZMETER ";
            IPPAN.G_SQL = IPPAN.G_SQL + "from SIZAI_TANAOROSI_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE TANAYM = '" + L_TANAYM + "' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "ORDER BY HINMOKUCD,GYOSYACD ";

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

            L_I = 0;

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    //項目セット
                    CKSI0025_01.G_AREA[j] = "";
                    CKSI0025_01.G_AREA[j] = tbl.Rows[i][j].ToString();
                }

                CKSI0025_01.TANAOROSI[L_I].HINMOKUCD = CKSI0025_01.G_AREA[1];
                CKSI0025_01.TANAOROSI[L_I].GYOSYACD = CKSI0025_01.G_AREA[2];
                CKSI0025_01.TANAOROSI[L_I].HINMOKUNM = CKSI0025_01.G_AREA[3];
                CKSI0025_01.TANAOROSI[L_I].GYOSYANM = CKSI0025_01.G_AREA[4];
                CKSI0025_01.TANAOROSI[L_I].ZSOUKO = CKSI0025_01.G_AREA[5];
                CKSI0025_01.TANAOROSI[L_I].ZEF = CKSI0025_01.G_AREA[6];
                CKSI0025_01.TANAOROSI[L_I].ZLF = CKSI0025_01.G_AREA[7];
                CKSI0025_01.TANAOROSI[L_I].ZCC = CKSI0025_01.G_AREA[8];
                CKSI0025_01.TANAOROSI[L_I].ZSONOTA = CKSI0025_01.G_AREA[9];
                CKSI0025_01.TANAOROSI[L_I].ZMETER = CKSI0025_01.G_AREA[10];

                L_I = L_I + 1;

                if (CKSI0025_01.G_WRK_MAX < L_I)
                {
                    break;
                }
            }

            if (L_I == 0)
            {
                LBL_PAGE.Text = "001/001";
            }
            else
            {
                LBL_PAGE.Text = "001/" + String.Format("{0:000}", Convert.ToInt16(Strings.Mid(String.Format("{0:0000}", L_I - 1), 1, 3)) + 1);
            }

            functionReturnValue = 0;
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

            // 行の各列のﾃﾞｰﾀの値を取得する
            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                // 項目をﾜｰｸにｾｯﾄ
                L_MST_AREA[j] = tbl.Rows[0][j].ToString();
            }

            CKSI0025_01.G_GYOSYANM = Strings.Mid(L_MST_AREA[1], 1, 20);

            functionReturnValue = 0;
            return functionReturnValue;
        }

        // 品目マスタ読込み処理
        public short HINMOKU_KENSAKU(string L_CD, short L_NO)
        {
            short functionReturnValue = 1;

            //   13.07.02             ISV-TRUC    品目ＣＤに入力により、向先項目にフォーカスをセットする。
            // string[] L_MST_AREA = new string[3];
            string[] L_MST_AREA = new string[4];
             //   13.07.02             ISV-TRUC end

            DataTable tbl = null;

            //   13.07.02             ISV-TRUC    品目ＣＤに入力により、向先項目にフォーカスをセットする。
            // 品目コードをｷｰにﾃﾞｰﾀﾍﾞｰｽ検索
            //IPPAN.G_SQL = "Select HINMOKUCD,HINMOKUNM,SYUBETU from SIZAI_HINMOKU_MST ";
            IPPAN.G_SQL = "Select HINMOKUCD,HINMOKUNM,SYUBETU,MUKESAKI from SIZAI_HINMOKU_MST ";
             //   13.07.02             ISV-TRUC end
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKUCD = '" + L_CD + "'";

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

            // 行の各列のﾃﾞｰﾀの値を取得する
            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                // 項目をﾜｰｸにｾｯﾄ
                L_MST_AREA[j] = tbl.Rows[0][j].ToString();
            }

            if (L_NO == 99)
            {
                LBL_K_HINNM.Text = Strings.Mid(L_MST_AREA[1], 1, 15);
            }
            else
            {
                LBL_HINMOKUNM[L_NO].Text = Strings.Mid(L_MST_AREA[1], 1, 15);
                CKSI0025_01.G_HINMOKUNM[L_NO] = Strings.Mid(L_MST_AREA[1], 1, 20);
            }

            CKSI0025_01.G_SYUBETU = Strings.Mid(L_MST_AREA[2], 1, 1);

            //   13.07.02             ISV-TRUC    品目ＣＤに入力により、向先項目にフォーカスをセットする。
            CKSI0025_01.G_MUKESAKI = L_MST_AREA[3];
             //   13.07.02             ISV-TRUC end

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private void CMD_Button_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = CMD_BUTTON.GetIndex((Button)eventSender);
            DialogResult dlgRlt = DialogResult.None;

            CMD_BUTTON[0].Enabled = false;
            CMD_BUTTON[1].Enabled = false;
            CMD_BUTTON[2].Enabled = false;

            switch (Index)
            {
                case 0:
                    //確定閉じる
                    //砂時計ポインタ
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
                    if (GAMEN_CHECK(0) != 0)
                    {
                        //標準ポインタ
                        IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                        dlgRlt = C_COMMON.Msg("エラーがありますが終了しますか", MessageBoxButtons.YesNo);
                        if (dlgRlt == DialogResult.No)
                        {
                            //いいえ
                            //砂時計ポインタ
                            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
                            IPPAN.G_RET = GAMEN_CHECK(1);
                            //標準ポインタ
                            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                            if (IPPAN.G_RET != 0)
                            {
                                CMD_BUTTON[0].Enabled = true;
                                if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) != "001")
                                {
                                    CMD_BUTTON[1].Enabled = true;
                                }
                                if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) != "200")
                                {
                                    CMD_BUTTON[2].Enabled = true;
                                }
                                return;
                            }
                            //砂時計ポインタ
                            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
                            if (CKSI0025_01.G_Flg == 1)
                            {
                                using (C_ODBC db = new C_ODBC())
                                {
                                    try
                                    {
                                        //DB接続
                                        db.Connect();
                                        //トランザクション開始
                                        db.BeginTrans();
                                        IPPAN.G_RET = WORK_KOUSIN(db, (Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10);
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
                            }
                        }
                    }
                    else
                    {
                        //砂時計ポインタ
                        IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
                        if (CKSI0025_01.G_Flg == 1)
                        {
                            using (C_ODBC db = new C_ODBC())
                            {
                                try
                                {
                                    //DB接続
                                    db.Connect();
                                    //トランザクション開始
                                    db.BeginTrans();
                                    IPPAN.G_RET = WORK_KOUSIN(db, (Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10);
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
                        }
                    }
                    //標準ポインタ
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    this.Close();
//2017.01.14 yositake 副資材棚卸システム再構築 start
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    processStartInfo.FileName = "C:\\EXE\\資材システム\\CKSI0050.exe";

                    // コマンドライン引数を指定
                    // 社員コード
                    processStartInfo.Arguments += CKSI0025_01.G_USERID;
                    // 所属コード 
                    processStartInfo.Arguments += "," + CKSI0025_01.G_OFFICEID;
                    // 職位コード
                    processStartInfo.Arguments += "," + CKSI0025_01.G_SYOKUICD;
                    // 呼出し元が副資材棚卸システム
                    processStartInfo.Arguments += ",1";

                    // 起動
                    Process p = Process.Start(processStartInfo);
                    this.Close();
//2017.01.14 yositake 副資材棚卸システム再構築 end
                    break;

                case 1:
                    //確定前へ
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
                    //砂時計ポインタ
                    IPPAN.G_RET = GAMEN_CHECK(1);
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    //標準ポインタ
                    if (IPPAN.G_RET != 0)
                    {
                        if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) != "001")
                        {
                            CMD_BUTTON[1].Enabled = true;
                        }
                        CMD_BUTTON[0].Enabled = true;
                        CMD_BUTTON[2].Enabled = true;
                        return;
                    }
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
                    //砂時計ポインタ
                    if (CKSI0025_01.G_Flg == 1)
                    {
                        using (C_ODBC db = new C_ODBC())
                        {
                            try
                            {
                                //DB接続
                                db.Connect();
                                //トランザクション開始
                                db.BeginTrans();
                                IPPAN.G_RET = WORK_KOUSIN(db, (Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10);
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

                        CKSI0025_01.G_Flg = 0;
                    }

                    GAMEN_SET((Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 2) * 10, 1);
                    //標準ポインタ
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    LBL_PAGE.Text = String.Format("{0:000}", Convert.ToInt16(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) + "/" + Strings.Mid(LBL_PAGE.Text, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) + 1, 3);
                    if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) != "001")
                    {
                        CMD_BUTTON[1].Enabled = true;
                    }
                    CMD_BUTTON[0].Enabled = true;
                    CMD_BUTTON[2].Enabled = true;
                    TXT_HINMOKUCD[0].Focus();
                    break;

                case 2:
                    //確定次へ
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
                    //砂時計ポインタ
                    IPPAN.G_RET = GAMEN_CHECK(1);
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    //標準ポインタ
                    if (IPPAN.G_RET != 0)
                    {
                        if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) != "200")
                        {
                            CMD_BUTTON[2].Enabled = true;
                        }
                        CMD_BUTTON[0].Enabled = true;
                        CMD_BUTTON[1].Enabled = true;
                        return;
                    }
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
                    //砂時計ポインタ
                    if (CKSI0025_01.G_Flg == 1)
                    {
                        using (C_ODBC db = new C_ODBC())
                        {
                            try
                            {
                                //DB接続
                                db.Connect();
                                //トランザクション開始
                                db.BeginTrans();
                                IPPAN.G_RET = WORK_KOUSIN(db, (Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10);
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

                        CKSI0025_01.G_Flg = 0;
                    }
                    GAMEN_SET(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) * 10, 1);
                    
                    //標準ポインタ
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    
                    if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) == Strings.Mid(LBL_PAGE.Text, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) + 1, 3))
                    {
                        LBL_PAGE.Text = String.Format("{0:000}", Convert.ToInt16(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) + 1) + "/" + String.Format("{0:000}", Convert.ToInt16(Strings.Mid(LBL_PAGE.Text, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) + 1, 3)) + 1);
                    }
                    else
                    {
                        LBL_PAGE.Text = String.Format("{0:000}", Convert.ToInt16(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) + 1) + "/" + Strings.Mid(LBL_PAGE.Text, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) + 1, 3);
                    }
                    if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) != "200")
                    {
                        CMD_BUTTON[2].Enabled = true;
                    }
                    CMD_BUTTON[0].Enabled = true;
                    CMD_BUTTON[1].Enabled = true;
                    TXT_HINMOKUCD[0].Focus();
                    break;
            }
        }

        private void CMD_KENSAKU_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            short L_COUNT = 0;
            short L_I = 0;

            CMD_BUTTON[0].Enabled = false;
            CMD_BUTTON[1].Enabled = false;
            CMD_BUTTON[2].Enabled = false;

            //砂時計ポインタ
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
            IPPAN.G_RET = GAMEN_CHECK(1);
            //標準ポインタ
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
            if (IPPAN.G_RET != 0)
            {
                CMD_BUTTON[0].Enabled = true;
                if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) != "001")
                {
                    CMD_BUTTON[1].Enabled = true;
                }
                if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) != "200")
                {
                    CMD_BUTTON[2].Enabled = true;
                }
                return;
            }

            if (IPPAN.C_Allspace(TXT_K_HINCD.Text) == 0)
            {
                TXT_K_HINCD.Focus();
                TXT_K_HINCD_Enter(TXT_K_HINCD, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                CMD_BUTTON[0].Enabled = true;
                if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) != "001")
                {
                    CMD_BUTTON[1].Enabled = true;
                }
                if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) != "200")
                {
                    CMD_BUTTON[2].Enabled = true;
                }
                return;
            }

            if (IPPAN.Input_Check(TXT_K_HINCD.Text) == 1)
            {
                TXT_K_HINCD.Focus();
                TXT_K_HINCD_Enter(TXT_K_HINCD, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                CMD_BUTTON[0].Enabled = true;
                if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) != "001")
                {
                    CMD_BUTTON[1].Enabled = true;
                }
                if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) != "200")
                {
                    CMD_BUTTON[2].Enabled = true;
                }
                return;
            }

            IPPAN.G_RET = GET_COUNT(ref L_COUNT);
            if (IPPAN.G_RET != 0)
            {
                TXT_K_HINCD.Focus();
                TXT_K_HINCD_Enter(TXT_K_HINCD, new System.EventArgs());
                C_COMMON.Msg("入力データがありません");
                CMD_BUTTON[0].Enabled = true;
                if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) != "001")
                {
                    CMD_BUTTON[1].Enabled = true;
                }
                if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) != "200")
                {
                    CMD_BUTTON[2].Enabled = true;
                }
                return;
            }

            //砂時計ポインタ
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    //DB接続
                    db.Connect();
                    //トランザクション開始
                    db.BeginTrans();
                    IPPAN.G_RET = WORK_KOUSIN(db, (Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10);
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

            //標準ポインタ
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);

            LBL_PAGE.Text = String.Format("{0:000}", Convert.ToInt16(Strings.Mid(String.Format("{0:0000}", L_COUNT), 1, 3)) + 1) + "/" + Strings.Mid(LBL_PAGE.Text, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) + 1, 3);

            GAMEN_SET((Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10, 1);

            for (L_I = 0; L_I <= 9; L_I++)
            {
                if (TXT_HINMOKUCD[L_I].Text == TXT_K_HINCD.Text)
                {
                    TXT_HINMOKUCD[L_I].Focus();
                    break;
                }
            }

            CMD_BUTTON[0].Enabled = true;
            if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) != "001")
            {
                CMD_BUTTON[1].Enabled = true;
            }
            if (Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1) != "200")
            {
                CMD_BUTTON[2].Enabled = true;
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
            //TXT_GYOSYACD
            for (short i = 0; i < this.TXT_GYOSYACD.Count(); i++)
            {
                this.TXT_GYOSYACD[i].Enter += new EventHandler(TXT_GYOSYACD_Enter);
                this.TXT_GYOSYACD[i].Leave += new EventHandler(TXT_GYOSYACD_Leave);
            }
            //TXT_HINMOKUCD
            for (short i = 0; i < this.TXT_HINMOKUCD.Count(); i++)
            {
                this.TXT_HINMOKUCD[i].Enter += new EventHandler(TXT_HINMOKUCD_Enter);
                this.TXT_HINMOKUCD[i].Leave += new EventHandler(TXT_HINMOKUCD_Leave);
            }
            //TXT_SFLG
            for (short i = 0; i < this.TXT_SFLG.Count(); i++)
            {
                this.TXT_SFLG[i].Enter += new EventHandler(TXT_SFLG_Enter);
                this.TXT_SFLG[i].Leave += new EventHandler(TXT_SFLG_Leave);
            }
            //TXT_ZCC
            for (short i = 0; i < this.TXT_ZCC.Count(); i++)
            {
                this.TXT_ZCC[i].Enter += new EventHandler(TXT_ZCC_Enter);
                this.TXT_ZCC[i].Leave += new EventHandler(TXT_ZCC_Leave);
            }
            //TXT_ZEF
            for (short i = 0; i < this.TXT_ZEF.Count(); i++)
            {
                this.TXT_ZEF[i].Enter += new EventHandler(TXT_ZEF_Enter);
                this.TXT_ZEF[i].Leave += new EventHandler(TXT_ZEF_Leave);
            }
            //TXT_ZLF
            for (short i = 0; i < this.TXT_ZLF.Count(); i++)
            {
                this.TXT_ZLF[i].Enter += new EventHandler(TXT_ZLF_Enter);
                this.TXT_ZLF[i].Leave += new EventHandler(TXT_ZLF_Leave);
            }
            //TXT_ZMETER
            for (short i = 0; i < this.TXT_ZMETER.Count(); i++)
            {
                this.TXT_ZMETER[i].Enter += new EventHandler(TXT_ZMETER_Enter);
                this.TXT_ZMETER[i].Leave += new EventHandler(TXT_ZMETER_Leave);
            }
            //TXT_ZSONOTA
            for (short i = 0; i < this.TXT_ZSONOTA.Count(); i++)
            {
                this.TXT_ZSONOTA[i].Enter += new EventHandler(TXT_ZSONOTA_Enter);
                this.TXT_ZSONOTA[i].Leave += new EventHandler(TXT_ZSONOTA_Leave);
            }
            //TXT_ZSOUKO
            for (short i = 0; i < this.TXT_ZSOUKO.Count(); i++)
            {
                this.TXT_ZSOUKO[i].Enter += new EventHandler(TXT_ZSOUKO_Enter);
                this.TXT_ZSOUKO[i].Leave += new EventHandler(TXT_ZSOUKO_Leave);
            }
        }

        private void FRM_CKSI0025M_Load(System.Object eventSender, System.EventArgs eventArgs)
        {
            string L_Search = null;
            string L_Command2 = null;

            //プロジェクト名を設定
            C_COMMON.G_COMMON_MSGTITLE = "CKSI0025";

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
                CKSI0025_01.G_USERID = Strings.Mid(IPPAN.G_COMMAND, 1, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) - 1);
                L_Command2 = Strings.Mid(IPPAN.G_COMMAND, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) + 1, 30);
                CKSI0025_01.G_OFFICEID = Strings.Mid(L_Command2, 1, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) - 1);
                CKSI0025_01.G_SYOKUICD = Strings.Mid(L_Command2, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) + 1, 2);
            }

            //ユーザーＩＤが副資材ユーザ以外かを見る
            IPPAN.G_RET = FS_USER_CHECK.User_Check(CKSI0025_01.G_USERID);
            if (IPPAN.G_RET != 0)
            {
                System.Environment.Exit(0);
            }
            else
            {
                IPPAN.G_RET = IPPAN2.TIMER_SET(CKSI0025_01.G_USERID);
                if (IPPAN.G_RET != 0)
                {
                    System.Environment.Exit(0);
                }
            }

            GAMEN_CLEAR();

            //砂時計ポインタ
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
            IPPAN.G_RET = GET_NENGETU();
            //標準ポインタ
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
            if (IPPAN.G_RET != 0)
            {
                return;
            }

            //  13.07.02    ISV-TRUC    4桁数の年度を表示
            //LBL_NEN.Text = Strings.Mid(CKSI0020_01.G_NENGETU, 3, 2);
            LBL_NEN.Text = Strings.Mid(CKSI0025_01.G_NENGETU, 1, 4);
             //   13.07.02             ISV-TRUC end

            LBL_TUKI.Text = Strings.Mid(CKSI0025_01.G_NENGETU, 5, 2);

            //砂時計ポインタ
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
            IPPAN.G_RET = GET_DATA(CKSI0025_01.G_NENGETU);
            //標準ポインタ
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
            if (IPPAN.G_RET != 0)
            {
                return;
            }

            //砂時計ポインタ
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
            GAMEN_SET(0, 0);
            //標準ポインタ
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);

            CMD_BUTTON[1].Enabled = false;

            CKSI0025_01.G_Flg = 0;
        }

        private void TMR_TIMER_Tick(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Control_Init(this, "副資材棚卸入力", "CKSI0025", SO.SO_USERNAME, SO.SO_OFFICENAME, "1.00");
        }

        private void TXT_GYOSYACD_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_GYOSYACD.GetIndex((TextBox)eventSender);

            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);

            CKSI0025_01.G_CD = TXT_GYOSYACD[Index].Text;

            //無効の状態でフォーカスを得た場合はフォーカスを解除し、次の列へ移動する
            if (TXT_GYOSYACD[Index].Enabled == false)
            {
                TXT_GYOSYACD_Leave(TXT_GYOSYACD[Index], new System.EventArgs());
                TXT_SFLG[Index].Focus();
            }
        }

        private void TXT_GYOSYACD_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_GYOSYACD.GetIndex((TextBox)eventSender);

            short L_I = 0;

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);

            if (CKSI0025_01.G_CD != TXT_GYOSYACD[Index].Text && TXT_SFLG[Index].Text != "9" && TXT_GYOSYACD[Index].Enabled == true)
            {

                if (IPPAN.C_Allspace(TXT_HINMOKUCD[Index].Text) == 0 && IPPAN.C_Allspace(TXT_GYOSYACD[Index].Text) == 0 && IPPAN.C_Allspace(TXT_SFLG[Index].Text) == 0 && IPPAN.C_Allspace(TXT_ZSOUKO[Index].Text) == 0 && IPPAN.C_Allspace(TXT_ZEF[Index].Text) == 0 && IPPAN.C_Allspace(TXT_ZLF[Index].Text) == 0 && IPPAN.C_Allspace(TXT_ZCC[Index].Text) == 0 && IPPAN.C_Allspace(TXT_ZSONOTA[Index].Text) == 0 && IPPAN.C_Allspace(TXT_ZMETER[Index].Text) == 0)
                {
                    TXT_HINMOKUCD[Index].Text = "";
                    LBL_HINMOKUNM[Index].Text = "";
                    TXT_GYOSYACD[Index].Text = "";
                    TXT_SFLG[Index].Text = "";
                    TXT_ZSOUKO[Index].Text = "";
                    TXT_ZEF[Index].Text = "";
                    TXT_ZLF[Index].Text = "";
                    TXT_ZCC[Index].Text = "";
                    TXT_ZSONOTA[Index].Text = "";
                    TXT_ZMETER[Index].Text = "";
                    return;
                }
                else if (IPPAN.C_Allspace(TXT_GYOSYACD[Index].Text) == 0)
                {
                    TXT_GYOSYACD[Index].Text = "";
                    TXT_GYOSYACD[Index].Focus();
                    TXT_GYOSYACD_Enter(TXT_GYOSYACD[Index], new System.EventArgs());
                    //必須項目入力エラー
                    IPPAN.Error_Msg("E200", 0, " ");
                    CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index].GYOSYACD = Strings.Space(4);
                    CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index].GYOSYANM = IPPAN.Space_Set("", 20, 2);
                    return;
                }

                IPPAN.G_RET = IPPAN.Input_Check(TXT_GYOSYACD[Index].Text);
                if (IPPAN.G_RET != 0)
                {
                    TXT_GYOSYACD[Index].Focus();
                    TXT_GYOSYACD_Enter(TXT_GYOSYACD[Index], new System.EventArgs());
                    //入力エラー
                    IPPAN.Error_Msg("E202", 0, " ");
                    return;
                }

                //砂時計ポインタ
                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
                IPPAN.G_RET = GYOSYA_KENSAKU(TXT_GYOSYACD[Index].Text);
                //標準ポインタ
                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                if (IPPAN.G_RET != 0)
                {
                    TXT_GYOSYACD[Index].Focus();
                    TXT_GYOSYACD_Enter(TXT_GYOSYACD[Index], new System.EventArgs());
                    //業者マスタが見つかりません
                    IPPAN.Error_Msg("E102", 0, " ");
                    return;
                }

                for (L_I = 0; L_I <= CKSI0025_01.G_WRK_MAX; L_I++)
                {
                    if (string.IsNullOrEmpty(CKSI0025_01.TANAOROSI[L_I].HINMOKUCD))
                    {
                        break;
                    }
                    if (L_I != (Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index)
                    {
                        if (TXT_HINMOKUCD[Index].Text == CKSI0025_01.TANAOROSI[L_I].HINMOKUCD && TXT_GYOSYACD[Index].Text == CKSI0025_01.TANAOROSI[L_I].GYOSYACD)
                        {
                            TXT_GYOSYACD[Index].Focus();
                            TXT_GYOSYACD_Enter(TXT_GYOSYACD[Index], new System.EventArgs());
                            C_COMMON.Msg("入力内容が重複しています");
                            return;
                        }
                    }
                }
                CKSI0025_01.G_Flg = 1;
            }

            CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index].GYOSYACD = TXT_GYOSYACD[Index].Text;
            CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index].GYOSYANM = IPPAN.Space_Set(Strings.Trim(Strings.Mid(CKSI0025_01.G_GYOSYANM, 1, 20)), 20, 2);
        }

        private void TXT_HINMOKUCD_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_HINMOKUCD.GetIndex((TextBox)eventSender);

            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);

            TXT_GYOSYACD[Index].Enabled = true;

            CKSI0025_01.G_CD = TXT_HINMOKUCD[Index].Text;
        }

        private void TXT_HINMOKUCD_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            //13.08.19 tsunamoto 向先フォーカス初期化 start
            CKSI0025_01.G_MUKESAKI = "";
            //13.08.19 tsunamoto 向先フォーカス初期化 end
            short Index = TXT_HINMOKUCD.GetIndex((TextBox)eventSender);

            short L_I = 0;

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);

            // 13.08.21 tsunamoto del start
            //  13.08.20    ISV-TRUC         
            //if (this.ActiveControl != null && (this.ActiveControl.Name.Contains("TXT_ZMETER") || this.ActiveControl.Name.Contains("CMD_KENSAKU")))
            //{
            //    return;
            //}
            //  End 13.08.20    ISV-TRUC
            // 13.08.21 tsunamoto end

            if (CKSI0025_01.G_CD != TXT_HINMOKUCD[Index].Text && TXT_SFLG[Index].Text != "9")
            {
                if (IPPAN.C_Allspace(TXT_HINMOKUCD[Index].Text) == 0 && IPPAN.C_Allspace(TXT_GYOSYACD[Index].Text) == 0 && IPPAN.C_Allspace(TXT_SFLG[Index].Text) == 0 && IPPAN.C_Allspace(TXT_ZSOUKO[Index].Text) == 0 && IPPAN.C_Allspace(TXT_ZEF[Index].Text) == 0 && IPPAN.C_Allspace(TXT_ZLF[Index].Text) == 0 && IPPAN.C_Allspace(TXT_ZCC[Index].Text) == 0 && IPPAN.C_Allspace(TXT_ZSONOTA[Index].Text) == 0 && IPPAN.C_Allspace(TXT_ZMETER[Index].Text) == 0)
                {
                    TXT_HINMOKUCD[Index].Text = "";
                    LBL_HINMOKUNM[Index].Text = "";
                    TXT_GYOSYACD[Index].Text = "";
                    TXT_SFLG[Index].Text = "";
                    TXT_ZSOUKO[Index].Text = "";
                    TXT_ZEF[Index].Text = "";
                    TXT_ZLF[Index].Text = "";
                    TXT_ZCC[Index].Text = "";
                    TXT_ZSONOTA[Index].Text = "";
                    TXT_ZMETER[Index].Text = "";
                    TXT_GYOSYACD[Index].Enabled = true;
                    CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index].HINMOKUCD = TXT_HINMOKUCD[Index].Text;
                    CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index].HINMOKUNM = IPPAN.Space_Set(CKSI0025_01.G_HINMOKUNM[Index], 20, 2);
                    return;
                }
                else if (IPPAN.C_Allspace(TXT_HINMOKUCD[Index].Text) == 0)
                {
                    LBL_HINMOKUNM[Index].Text = "";
                    TXT_HINMOKUCD[Index].Focus();
                    TXT_HINMOKUCD_Enter(TXT_HINMOKUCD[Index], new System.EventArgs());
                    //必須項目入力エラー
                    IPPAN.Error_Msg("E200", 0, " ");
                    return;
                }

                IPPAN.G_RET = IPPAN.Input_Check(TXT_HINMOKUCD[Index].Text);
                if (IPPAN.G_RET != 0)
                {
                    LBL_HINMOKUNM[Index].Text = "";
                    TXT_HINMOKUCD[Index].Focus();
                    TXT_HINMOKUCD_Enter(TXT_HINMOKUCD[Index], new System.EventArgs());
                    //入力エラー
                    IPPAN.Error_Msg("E202", 0, " ");
                    return;
                }

                //砂時計ポインタ
                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
                IPPAN.G_RET = HINMOKU_KENSAKU(TXT_HINMOKUCD[Index].Text, Index);
                //標準ポインタ
                IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                if (IPPAN.G_RET != 0)
                {
                    LBL_HINMOKUNM[Index].Text = "";
                    TXT_HINMOKUCD[Index].Focus();
                    TXT_HINMOKUCD_Enter(TXT_HINMOKUCD[Index], new System.EventArgs());
                    //副資材品目マスタが見つかりません。
                    IPPAN.Error_Msg("E701", 0, " ");
                    return;
                }

                if (CKSI0025_01.G_SYUBETU == "1")
                {
                    TXT_GYOSYACD[Index].Text = Strings.Space(4);
                    TXT_GYOSYACD[Index].Enabled = false;
                    CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index].GYOSYACD = Strings.Space(4);
                    CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index].GYOSYANM = Strings.StrConv(Strings.Space(20), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
                }
                else
                {
                    TXT_GYOSYACD[Index].Text = "";
                }

                for (L_I = 0; L_I <= CKSI0025_01.G_WRK_MAX; L_I++)
                {
                    if (string.IsNullOrEmpty(CKSI0025_01.TANAOROSI[L_I].HINMOKUCD))
                    {
                        break;
                    }
                    if (L_I != (Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index)
                    {
                        if (TXT_HINMOKUCD[Index].Text == CKSI0025_01.TANAOROSI[L_I].HINMOKUCD && TXT_GYOSYACD[Index].Text == CKSI0025_01.TANAOROSI[L_I].GYOSYACD)
                        {
                            TXT_HINMOKUCD[Index].Focus();
                            TXT_HINMOKUCD_Enter(TXT_HINMOKUCD[Index], new System.EventArgs());
                            C_COMMON.Msg("入力内容が重複しています");
                            return;
                        }
                    }
                }
                CKSI0025_01.G_Flg = 1;
            }
            else
            {
                //品目コードが変更されなかったときでも品目マスタをチェックする。
                if (TXT_SFLG[Index].Text != "9" && IPPAN.C_Allspace(TXT_HINMOKUCD[Index].Text) != 0)
                {
                    IPPAN.G_RET = IPPAN.Input_Check(TXT_HINMOKUCD[Index].Text);
                    if (IPPAN.G_RET != 0)
                    {
                        LBL_HINMOKUNM[Index].Text = "";
                        TXT_HINMOKUCD[Index].Focus();
                        TXT_HINMOKUCD_Enter(TXT_HINMOKUCD[Index], new System.EventArgs());
                        //入力エラー
                        IPPAN.Error_Msg("E202", 0, " ");
                        return;
                    }

                    //砂時計ポインタ
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
                    IPPAN.G_RET = HINMOKU_KENSAKU(TXT_HINMOKUCD[Index].Text, Index);
                    //標準ポインタ
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    if (IPPAN.G_RET != 0)
                    {
                        LBL_HINMOKUNM[Index].Text = "";
                        TXT_HINMOKUCD[Index].Focus();
                        TXT_HINMOKUCD_Enter(TXT_HINMOKUCD[Index], new System.EventArgs());
                        //副資材品目マスタが見つかりません。
                        IPPAN.Error_Msg("E701", 0, " ");
                        return;
                    }

                    if (CKSI0025_01.G_SYUBETU == "1")
                    {
                        TXT_GYOSYACD[Index].Text = Strings.Space(4);
                        TXT_GYOSYACD[Index].Enabled = false;
                        CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index].GYOSYACD = Strings.Space(4);
                        CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index].GYOSYANM = Strings.StrConv(Strings.Space(20), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP);
                    }
                }
            }
            
            // 13.08.21 tsunamoto start
            CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index].HINMOKUCD = TXT_HINMOKUCD[Index].Text;
            CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index].HINMOKUNM = IPPAN.Space_Set(CKSI0025_01.G_HINMOKUNM[Index], 20, 2);

            if (this.ActiveControl != null && (this.ActiveControl.Name.Contains("TXT_ZMETER") || this.ActiveControl.Name.Contains("CMD_KENSAKU")))
            {
                return;
            }
            //13.08.21 tsunamoto end
            
            //13.08.20    ISV-TRUC       品目ＣＤに入力により、向先項目にフォーカスをセットする。 
            if (IPPAN.C_Allspace(TXT_HINMOKUCD[Index].Text) != 0 && TXT_SFLG[Index].Text != "9")
            {
                string mukesaki = CKSI0025_01.G_MUKESAKI;
                switch (mukesaki)
                {

                    case "1":
                        TXT_ZEF[Index].Focus();
                        break;

                    case "2":
                        TXT_ZLF[Index].Focus();
                        break;

                    case "3":
                        TXT_ZCC[Index].Focus();
                        break;

                    case "4":
                        TXT_ZSONOTA[Index].Focus();
                        break;

                    default:
                        TXT_SFLG[Index].Focus();
                        break;
                }
            }
            //End  13.08.20  ISV-TRUC

            //13.08.21 tsunamoto del start
            //CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index].HINMOKUCD = TXT_HINMOKUCD[Index].Text;
            //CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index].HINMOKUNM = IPPAN.Space_Set(CKSI0025_01.G_HINMOKUNM[Index], 20, 2);
            //13.08.21 tsunamoto end
        }

        private void TXT_K_HINCD_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);

            CKSI0025_01.G_CD = TXT_K_HINCD.Text;
        }

        private void TXT_K_HINCD_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            if (CKSI0025_01.G_CD != TXT_K_HINCD.Text)
            {
                IPPAN.G_RET = IPPAN.Input_Check(TXT_K_HINCD.Text);
                if (IPPAN.G_RET != 0)
                {
                    LBL_K_HINNM.Text = "";
                    TXT_K_HINCD.Focus();
                    TXT_K_HINCD_Enter(TXT_K_HINCD, new System.EventArgs());
                    //入力エラー
                    IPPAN.Error_Msg("E202", 0, " ");
                    return;
                }

                IPPAN.G_RET = HINMOKU_KENSAKU(TXT_K_HINCD.Text, 99);
                if (IPPAN.G_RET != 0)
                {
                    LBL_K_HINNM.Text = "";
                    TXT_K_HINCD.Focus();
                    TXT_K_HINCD_Enter(TXT_K_HINCD, new System.EventArgs());
                    //副資材品目マスタが見つかりません。
                    IPPAN.Error_Msg("E701", 0, " ");
                    return;
                }
            }

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_SFLG_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_SFLG.GetIndex((TextBox)eventSender);

            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);

            CKSI0025_01.G_CD = TXT_SFLG[Index].Text;
        }

        private void TXT_SFLG_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_SFLG.GetIndex((TextBox)eventSender);

            if (CKSI0025_01.G_CD != TXT_SFLG[Index].Text)
            {

                IPPAN.G_RET = IPPAN.Input_Check(TXT_SFLG[Index].Text);
                if (IPPAN.G_RET != 0)
                {
                    TXT_SFLG[Index].Focus();
                    TXT_SFLG_Enter(TXT_SFLG[Index], new System.EventArgs());
                    //入力エラー
                    IPPAN.Error_Msg("E202", 0, " ");
                    return;
                }

                IPPAN.G_RET = IPPAN.Numeric_Check2(TXT_SFLG[Index].Text);
                if (IPPAN.G_RET == 1)
                {
                    TXT_SFLG[Index].Focus();
                    TXT_SFLG_Enter(TXT_SFLG[Index], new System.EventArgs());
                    //入力エラー
                    IPPAN.Error_Msg("E202", 0, " ");
                    return;
                }

                if (TXT_SFLG[Index].Text == "9" || string.IsNullOrEmpty(TXT_SFLG[Index].Text) || TXT_SFLG[Index].Text == " ")
                {
                }
                else
                {
                    TXT_SFLG[Index].Focus();
                    TXT_SFLG_Enter(TXT_SFLG[Index], new System.EventArgs());
                    //入力エラー
                    IPPAN.Error_Msg("E202", 0, " ");
                    return;
                }
                CKSI0025_01.G_Flg = 1;
            }
            CKSI0025_01.TANAOROSI[(Convert.ToInt32(Strings.Mid(LBL_PAGE.Text, 1, Strings.InStr(LBL_PAGE.Text, "/", CompareMethod.Binary) - 1)) - 1) * 10 + Index].SFLG = TXT_SFLG[Index].Text;

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_ZCC_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_ZCC.GetIndex((TextBox)eventSender);

            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);

            TXT_ZCC[Index].Text = IPPAN2.Numeric_Hensyu3(TXT_ZCC[Index].Text);

            TXT_ZCC[Index].MaxLength = 6;

            CKSI0025_01.G_CD = TXT_ZCC[Index].Text;
        }

        private void TXT_ZCC_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_ZCC.GetIndex((TextBox)eventSender);

            if (CKSI0025_01.G_CD != TXT_ZCC[Index].Text)
            {
                CKSI0025_01.G_Flg = 1;
            }

            TXT_ZCC[Index].MaxLength = 7;

            TXT_ZCC[Index].Text = IPPAN2.Numeric_Hensyu3(TXT_ZCC[Index].Text);
            TXT_ZCC[Index].Text = C_COMMON.FormatToNum(TXT_ZCC[Index].Text, "###,##0");
            TXT_ZCC[Index].Text = String.Format("{0,7}", TXT_ZCC[Index].Text);

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_ZEF_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_ZEF.GetIndex((TextBox)eventSender);

            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);

            TXT_ZEF[Index].Text = IPPAN2.Numeric_Hensyu3(TXT_ZEF[Index].Text);

            TXT_ZEF[Index].MaxLength = 6;

            CKSI0025_01.G_CD = TXT_ZEF[Index].Text;
        }

        private void TXT_ZEF_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_ZEF.GetIndex((TextBox)eventSender);

            if (CKSI0025_01.G_CD != TXT_ZEF[Index].Text)
            {
                CKSI0025_01.G_Flg = 1;
            }

            TXT_ZEF[Index].MaxLength = 7;

            TXT_ZEF[Index].Text = IPPAN2.Numeric_Hensyu3(TXT_ZEF[Index].Text);
            TXT_ZEF[Index].Text = C_COMMON.FormatToNum(TXT_ZEF[Index].Text, "###,##0");
            TXT_ZEF[Index].Text = String.Format("{0,7}", TXT_ZEF[Index].Text);

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_ZLF_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_ZLF.GetIndex((TextBox)eventSender);

            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);

            TXT_ZLF[Index].Text = IPPAN2.Numeric_Hensyu3(TXT_ZLF[Index].Text);

            TXT_ZLF[Index].MaxLength = 6;

            CKSI0025_01.G_CD = TXT_ZLF[Index].Text;
        }

        private void TXT_ZLF_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_ZLF.GetIndex((TextBox)eventSender);

            if (CKSI0025_01.G_CD != TXT_ZLF[Index].Text)
            {
                CKSI0025_01.G_Flg = 1;
            }

            TXT_ZLF[Index].MaxLength = 7;

            TXT_ZLF[Index].Text = IPPAN2.Numeric_Hensyu3(TXT_ZLF[Index].Text);
            TXT_ZLF[Index].Text = C_COMMON.FormatToNum(TXT_ZLF[Index].Text, "###,##0");
            TXT_ZLF[Index].Text = String.Format("{0,7}", TXT_ZLF[Index].Text);

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_ZMETER_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_ZMETER.GetIndex((TextBox)eventSender);

            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);

            TXT_ZMETER[Index].Text = IPPAN2.Numeric_Hensyu3(TXT_ZMETER[Index].Text);

            TXT_ZMETER[Index].MaxLength = 6;

            CKSI0025_01.G_CD = TXT_ZMETER[Index].Text;
        }

        private void TXT_ZMETER_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_ZMETER.GetIndex((TextBox)eventSender);

            if (CKSI0025_01.G_CD != TXT_ZMETER[Index].Text)
            {
                CKSI0025_01.G_Flg = 1;
            }

            TXT_ZMETER[Index].MaxLength = 7;

            TXT_ZMETER[Index].Text = IPPAN2.Numeric_Hensyu3(TXT_ZMETER[Index].Text);
            TXT_ZMETER[Index].Text = C_COMMON.FormatToNum(TXT_ZMETER[Index].Text, "###,##0");
            TXT_ZMETER[Index].Text = String.Format("{0,7}", TXT_ZMETER[Index].Text);

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_ZSONOTA_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_ZSONOTA.GetIndex((TextBox)eventSender);

            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);

            TXT_ZSONOTA[Index].Text = IPPAN2.Numeric_Hensyu3(TXT_ZSONOTA[Index].Text);

            TXT_ZSONOTA[Index].MaxLength = 6;

            CKSI0025_01.G_CD = TXT_ZSONOTA[Index].Text;
        }

        private void TXT_ZSONOTA_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_ZSONOTA.GetIndex((TextBox)eventSender);

            if (CKSI0025_01.G_CD != TXT_ZSONOTA[Index].Text)
            {
                CKSI0025_01.G_Flg = 1;
            }

            TXT_ZSONOTA[Index].MaxLength = 7;

            TXT_ZSONOTA[Index].Text = IPPAN2.Numeric_Hensyu3(TXT_ZSONOTA[Index].Text);
            TXT_ZSONOTA[Index].Text = C_COMMON.FormatToNum(TXT_ZSONOTA[Index].Text, "###,##0");
            TXT_ZSONOTA[Index].Text = String.Format("{0,7}", TXT_ZSONOTA[Index].Text);

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }

        private void TXT_ZSOUKO_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_ZSOUKO.GetIndex((TextBox)eventSender);

            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);

            TXT_ZSOUKO[Index].Text = IPPAN2.Numeric_Hensyu3(TXT_ZSOUKO[Index].Text);

            TXT_ZSOUKO[Index].MaxLength = 6;

            CKSI0025_01.G_CD = TXT_ZSOUKO[Index].Text;
        }

        private void TXT_ZSOUKO_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = TXT_ZSOUKO.GetIndex((TextBox)eventSender);

            if (CKSI0025_01.G_CD != TXT_ZSOUKO[Index].Text)
            {
                CKSI0025_01.G_Flg = 1;
            }

            TXT_ZSOUKO[Index].MaxLength = 7;

            TXT_ZSOUKO[Index].Text = IPPAN2.Numeric_Hensyu3(TXT_ZSOUKO[Index].Text);
            TXT_ZSOUKO[Index].Text = C_COMMON.FormatToNum(TXT_ZSOUKO[Index].Text, "###,##0");
            TXT_ZSOUKO[Index].Text = String.Format("{0,7}", TXT_ZSOUKO[Index].Text);

            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);
        }
    }
}
