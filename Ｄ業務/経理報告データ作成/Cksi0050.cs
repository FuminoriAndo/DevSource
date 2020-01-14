using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.Odbc;
using System.IO;

namespace Project1
{
    internal partial class FRM_CKSI0050M : System.Windows.Forms.Form
    {
        //*************************************************************************************
        //
        //
        //   プログラム名　　　経理報告データ作成
        //
        //
        //   修正履歴
        //
        //   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
        //   03.01.15           NTIS浅田　　Ｓ／Ｏサーバ更新の為、VB4.0→VB6.0SP5へのバージョンアップ対応
        //*************************************************************************************
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FRM_CKSI0050M());
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

        public short DATA_TENSO()
        {
            short functionReturnValue = 1;
            string L_FilePath = null;
            string[] L_LIST_AREA = new string[31];
            string L_LIST = null;
            short L_l = 0;
            StreamWriter swFile = null;
            DataTable tbl = null;

            //ホスト伝送テキストｐａｔｈをサーバに変更
            L_FilePath = @"\\CKSOSVR32\FTP\SIZAI";

            try
            {
                //ファイルオープン
                swFile = new StreamWriter(L_FilePath, false, System.Text.Encoding.GetEncoding("shift_jis"));
            }
            catch (Exception ex)
            {
                C_COMMON.Msg(L_FilePath + " を作成できません。" + System.Environment.NewLine + ex.Message);
                return functionReturnValue;
            }

            //副資材経理報告トランの検索

            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "Select ";
            IPPAN.G_SQL = IPPAN.G_SQL + "HIMOKU, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "UTIWAKE, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "TANABAN, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKUCD, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "YM, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "TANI, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SZAIKO, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "EZAIKO, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "NYUKO, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SEF, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SLF, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SCC, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SSONOTA, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SJIGYO, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "S1JI, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "STD, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "S2JI, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SYOBI1, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SYOBI2, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "TANKA, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "UPDYMD ";
            IPPAN.G_SQL = IPPAN.G_SQL + "from SIZAI_HOUKOKU_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKUCD = '    ' ";

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    // SQL実行
                    db.Connect();
                    tbl = db.ExecSQL(IPPAN.G_SQL);
                }
                catch (OdbcException)
                {
                    //ファイルクローズ
                    swFile.Close();
                    swFile = null;
                    db.Error();
                }
                //2013.04.13 miura mori start
                catch (Exception)
                {
                    //ファイルクローズ
                    swFile.Close();
                    swFile = null;
                    db.Error();
                }
                //2013.04.13 miura mori end
            }

            //結果の列数を取得する
            if (tbl.Columns.Count < 1)
            {
                //ファイルクローズ
                swFile.Close();
                swFile = null;
                return functionReturnValue;
            }

            //データがない
            if (tbl.Rows.Count == 0)
            {
                //ファイルクローズ
                swFile.Close();
                swFile = null;
                return functionReturnValue;
            }

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    // 項目をﾜｰｸにｾｯﾄ
                    L_LIST_AREA[j] = tbl.Rows[i][j].ToString();
                }

                L_LIST = "";
                L_LIST = L_LIST + Strings.Mid(L_LIST_AREA[0], 1, 2);
                L_LIST = L_LIST + Strings.Mid(L_LIST_AREA[1], 1, 2);
                L_LIST = L_LIST + Strings.Mid(L_LIST_AREA[2], 1, 2);
                L_LIST = L_LIST + Strings.Mid(L_LIST_AREA[3], 1, 4);
                L_LIST = L_LIST + Strings.Mid(L_LIST_AREA[4], 1, 6);
                L_LIST = L_LIST + Strings.Mid(L_LIST_AREA[5], 1, 1);

                for (L_l = 6; L_l <= 18; L_l++)
                {
                    if (Conversion.Val(Strings.Mid(L_LIST_AREA[L_l], 1, 9)) < 0)
                    {
                        L_LIST = L_LIST + String.Format("{0:000000000}", Convert.ToDecimal(Strings.Mid(L_LIST_AREA[L_l], 1, 9)) * -1);
                        L_LIST = L_LIST + "1";
                    }
                    else
                    {
                        L_LIST = L_LIST + String.Format("{0:000000000}", Convert.ToDecimal(Strings.Mid(L_LIST_AREA[L_l], 1, 9)));
                        L_LIST = L_LIST + Strings.Space(1);
                    }
                }

                L_LIST = L_LIST + String.Format("{0:000000000}", Convert.ToDecimal(Strings.Mid(L_LIST_AREA[19], 1, 10)) * 100);
                //日付の「/」を除外する
                DateTime dt = DateTime.Parse(Strings.Mid(L_LIST_AREA[20], 1, 10));
                L_LIST = L_LIST + dt.ToString("yyyyMMdd");

                //ﾃｷｽﾄへ書込み
                swFile.WriteLine(L_LIST);
            }

            //ファイルクローズ
            swFile.Close();
            swFile = null;

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public short DATA_YOMIKOMI()
        {
            short functionReturnValue = 1;

            //読込データ処理用ワーク
            string[] L_AREA = new string[24];

            bool L_SAISYO = false;

            int L_COUNT = 0;
            decimal L_KSiyoryo = default(decimal);
            decimal L_Gosa = default(decimal);
            decimal L_Gokei = default(decimal);
            int nCnt = 0;
            DataTable tbl = null;

            L_SAISYO = true;
            L_COUNT = 0;
            WORK_SYOKIKA();

            //副資材元帳トランの検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "SELECT ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.MOTOYM, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.HINMOKUCD, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.SZAIKO, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.NYUKO, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.SUIBUN, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.EZAIKO, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.SIYO, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.SEF, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.SLF, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.SCC, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.SSONOTA, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.SJIGYO, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.S1JI, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.STD, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.S2JI, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.SYOBI1, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.SYOBI2, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.HIMOKU, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.UTIWAKE, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.TANABAN, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.HINMOKUNM, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.SYUBETU, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.ICHIKBN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "FROM ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.MOTOYM = '" + CKSI0050_01.G_NENGETU + "' AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.HINMOKUCD = SIZAI_HINMOKU_MST.HINMOKUCD ";
            IPPAN.G_SQL = IPPAN.G_SQL + " and SIZAI_HINMOKU_MST.HOUKOKUKBN = '1' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "ORDER BY ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.HIMOKU, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.UTIWAKE, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_MOTOCHO_TRN.TANABAN, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.SYUBETU ";

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    // SQL実行
                    db.Connect();
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
                //対象データがありません
                IPPAN.Error_Msg("E536", 0, " ");
                return functionReturnValue;
            }

            string L_Hinmokucd = null;
            string L_HINMOKUNM = null;
            string L_MOTOCHO = null;
            string L_KEISAN = null;

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    //項目セット
                    L_AREA[j] = tbl.Rows[i][j].ToString();
                }

                if (L_SAISYO == true)
                {
                    //副資材経理報告トランの削除
                    DATA_SAKUJYO();
                    //最初フラグの解除
                    L_SAISYO = false;
                }

                L_KSiyoryo = Convert.ToDecimal(L_AREA[2]) + Convert.ToDecimal(L_AREA[3]) - Convert.ToDecimal(L_AREA[4]) - Convert.ToDecimal(L_AREA[5]);

                //入庫払い
                if (L_AREA[21] == "1")
                {

                    if (Convert.ToDecimal(L_AREA[6]) != L_KSiyoryo)
                    {
                        L_Hinmokucd = Strings.Mid(Strings.Trim(Strings.Mid(L_AREA[1], 1, 4)) + Strings.Space(4), 1, 4);
                        L_HINMOKUNM = Strings.Mid(Strings.StrConv(Strings.Trim(Strings.Mid(L_AREA[20], 1, 20)) + Strings.Space(20), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP), 1, 20);
                        L_MOTOCHO = String.Format("{0,11}", C_COMMON.FormatToNum(L_AREA[6], "###,###,##0"));
                        L_KEISAN = String.Format("{0,11}", String.Format("{0:###,###,##0}", L_KSiyoryo));

                        //エラーレコードのリストボックスへの表示
                        LST_LIST.Items.Add(Strings.Space(1) + L_Hinmokucd + Strings.Space(1) + L_HINMOKUNM + Strings.Space(1) + L_MOTOCHO + Strings.Space(1) + L_KEISAN);
                    }
                }
                //使用高払い
                if (CKSI0050_01.G_SYUBETU == "2")
                {
                    if (Convert.ToDecimal(L_AREA[6]) != L_KSiyoryo)
                    {
                        L_Hinmokucd = Strings.Mid(Strings.Trim(Strings.Mid(L_AREA[1], 1, 4)) + Strings.Space(4), 1, 4);
                        L_HINMOKUNM = Strings.Mid(Strings.StrConv(Strings.Trim(Strings.Mid(L_AREA[20], 1, 20)) + Strings.Space(20), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP), 1, 20);
                        L_MOTOCHO = String.Format("{0,11}", C_COMMON.FormatToNum(L_AREA[6], "###,###,##0"));
                        L_KEISAN = String.Format("{0,11}", String.Format("{0:###,###,##0}", L_KSiyoryo));

                        //エラーレコードのリストボックスへの表示
                        LST_LIST.Items.Add(Strings.Space(1) + L_Hinmokucd + Strings.Space(1) + L_HINMOKUNM + Strings.Space(1) + L_MOTOCHO + Strings.Space(1) + L_KEISAN);
                    }

                    //副資材経理報告トランの作成
                    DATA_SAKUSEI(2);
                    WORK_SYOKIKA();
                    L_COUNT = 0;
                }
                //入庫払い
                else if ((CKSI0050_01.G_SYUBETU == "1") && (CKSI0050_01.G_HIMOKU != L_AREA[17] || CKSI0050_01.G_UTIWAKE != L_AREA[18] || CKSI0050_01.G_TANABAN != L_AREA[19] || CKSI0050_01.G_SYUBETU != L_AREA[21]))
                {
                    //副資材経理報告トランの作成
                    DATA_SAKUSEI(1);
                    WORK_SYOKIKA();
                    L_COUNT = 0;
                }

                //読込データの移送
                CKSI0050_01.G_MOTOYM = L_AREA[0];
                CKSI0050_01.G_HINMOKUCD[L_COUNT] = L_AREA[1];
                CKSI0050_01.G_SZAIKO = CKSI0050_01.G_SZAIKO + Convert.ToDecimal(L_AREA[2]);
                CKSI0050_01.G_NYUKO = CKSI0050_01.G_NYUKO + Convert.ToDecimal(L_AREA[3]) - Convert.ToDecimal(L_AREA[4]);

                CKSI0050_01.G_EZAIKO = CKSI0050_01.G_EZAIKO + Convert.ToDecimal(L_AREA[5]);
                //-----------------------------------------------------
                if (L_AREA[22] == " ")
                {
                    L_AREA[22] = "0";
                }

                if (Convert.ToDecimal(L_AREA[6]) == 0)
                {
                    if (L_KSiyoryo != 0)
                    {
                        CKSI0050_01.G_SIYORYO[Convert.ToInt16(L_AREA[22])] = CKSI0050_01.G_SIYORYO[Convert.ToInt16(L_AREA[22])] + L_KSiyoryo;
                    }
                }
                else
                {
                    L_Gokei = 0;
                    for (nCnt = 7; nCnt <= 16; nCnt++)
                    {
                        if (Conversion.Val(L_AREA[nCnt]) > 0)
                        {
                            L_Gokei = L_Gokei + Convert.ToDecimal(L_AREA[nCnt]);
                        }
                    }

                    if (Conversion.Val(L_AREA[7]) > 0)
                    {
                        //出庫量（ＥＦ）
                        CKSI0050_01.G_SIYO[1] = Conversion.Fix(L_KSiyoryo * Convert.ToDecimal(L_AREA[7]) / L_Gokei);
                    }
                    else
                    {
                        CKSI0050_01.G_SIYO[1] = 0;
                    }

                    if (Conversion.Val(L_AREA[8]) > 0)
                    {
                        //出庫量（ＬＦ）
                        CKSI0050_01.G_SIYO[2] = Conversion.Fix(L_KSiyoryo * Convert.ToDecimal(L_AREA[8]) / L_Gokei);
                    }
                    else
                    {
                        CKSI0050_01.G_SIYO[2] = 0;
                    }

                    if (Conversion.Val(L_AREA[9]) > 0)
                    {
                        //出庫量（ＣＣ）
                        CKSI0050_01.G_SIYO[3] = Conversion.Fix(L_KSiyoryo * Convert.ToDecimal(L_AREA[9]) / L_Gokei);
                    }
                    else
                    {
                        CKSI0050_01.G_SIYO[3] = 0;
                    }

                    if (Conversion.Val(L_AREA[10]) > 0)
                    {
                        //出庫量（その他）
                        CKSI0050_01.G_SIYO[4] = Conversion.Fix(L_KSiyoryo * Convert.ToDecimal(L_AREA[10]) / L_Gokei);
                    }
                    else
                    {
                        CKSI0050_01.G_SIYO[4] = 0;
                    }

                    if (Conversion.Val(L_AREA[11]) > 0)
                    {
                        //出庫量（事業開発）
                        CKSI0050_01.G_SIYO[5] = Conversion.Fix(L_KSiyoryo * Convert.ToDecimal(L_AREA[11]) / L_Gokei);
                    }
                    else
                    {
                        CKSI0050_01.G_SIYO[5] = 0;
                    }

                    if (Conversion.Val(L_AREA[12]) > 0)
                    {
                        //出庫量（１次切断）
                        CKSI0050_01.G_SIYO[6] = Conversion.Fix(L_KSiyoryo * Convert.ToDecimal(L_AREA[12]) / L_Gokei);
                    }
                    else
                    {
                        CKSI0050_01.G_SIYO[6] = 0;
                    }

                    if (Conversion.Val(L_AREA[13]) > 0)
                    {
                        //出庫量（ＴＤ）
                        CKSI0050_01.G_SIYO[7] = Conversion.Fix(L_KSiyoryo * Convert.ToDecimal(L_AREA[13]) / L_Gokei);
                    }
                    else
                    {
                        CKSI0050_01.G_SIYO[7] = 0;
                    }

                    if (Conversion.Val(L_AREA[14]) > 0)
                    {
                        //出庫量（２次切断）
                        CKSI0050_01.G_SIYO[8] = Conversion.Fix(L_KSiyoryo * Convert.ToDecimal(L_AREA[14]) / L_Gokei);
                    }
                    else
                    {
                        CKSI0050_01.G_SIYO[8] = 0;
                    }

                    if (Conversion.Val(L_AREA[15]) > 0)
                    {
                        //出庫量（予備１）
                        CKSI0050_01.G_SIYO[9] = Conversion.Fix(L_KSiyoryo * Convert.ToDecimal(L_AREA[15]) / L_Gokei);
                    }
                    else
                    {
                        CKSI0050_01.G_SIYO[9] = 0;
                    }

                    if (Conversion.Val(L_AREA[16]) > 0)
                    {
                        //出庫量（予備２）
                        CKSI0050_01.G_SIYO[0] = Conversion.Fix(L_KSiyoryo * Convert.ToDecimal(L_AREA[16]) / L_Gokei);
                    }
                    else
                    {
                        CKSI0050_01.G_SIYO[0] = 0;
                    }

                    CKSI0050_01.G_SIYORYO[1] = CKSI0050_01.G_SIYORYO[1] + CKSI0050_01.G_SIYO[1];
                    CKSI0050_01.G_SIYORYO[2] = CKSI0050_01.G_SIYORYO[2] + CKSI0050_01.G_SIYO[2];
                    CKSI0050_01.G_SIYORYO[3] = CKSI0050_01.G_SIYORYO[3] + CKSI0050_01.G_SIYO[3];
                    CKSI0050_01.G_SIYORYO[4] = CKSI0050_01.G_SIYORYO[4] + CKSI0050_01.G_SIYO[4];
                    CKSI0050_01.G_SIYORYO[5] = CKSI0050_01.G_SIYORYO[5] + CKSI0050_01.G_SIYO[5];
                    CKSI0050_01.G_SIYORYO[6] = CKSI0050_01.G_SIYORYO[6] + CKSI0050_01.G_SIYO[6];
                    CKSI0050_01.G_SIYORYO[7] = CKSI0050_01.G_SIYORYO[7] + CKSI0050_01.G_SIYO[7];
                    CKSI0050_01.G_SIYORYO[8] = CKSI0050_01.G_SIYORYO[8] + CKSI0050_01.G_SIYO[8];
                    CKSI0050_01.G_SIYORYO[9] = CKSI0050_01.G_SIYORYO[9] + CKSI0050_01.G_SIYO[9];
                    CKSI0050_01.G_SIYORYO[0] = CKSI0050_01.G_SIYORYO[0] + CKSI0050_01.G_SIYO[0];
                    L_Gosa = L_KSiyoryo - (CKSI0050_01.G_SIYO[1] + CKSI0050_01.G_SIYO[2] + CKSI0050_01.G_SIYO[3] + CKSI0050_01.G_SIYO[4] + CKSI0050_01.G_SIYO[5] + CKSI0050_01.G_SIYO[6] + CKSI0050_01.G_SIYO[7] + CKSI0050_01.G_SIYO[8] + CKSI0050_01.G_SIYO[9] + CKSI0050_01.G_SIYO[0]);
                    if (L_Gosa != 0)
                    {
                        CKSI0050_01.G_SIYORYO[Convert.ToInt16(L_AREA[22])] = CKSI0050_01.G_SIYORYO[Convert.ToInt16(L_AREA[22])] + L_Gosa;
                    }
                }
                //-----------------------------------------------------
                CKSI0050_01.G_HIMOKU = L_AREA[17];
                CKSI0050_01.G_UTIWAKE = L_AREA[18];
                CKSI0050_01.G_TANABAN = L_AREA[19];
                CKSI0050_01.G_HINMOKUNM[L_COUNT] = L_AREA[20];
                CKSI0050_01.G_SYUBETU = L_AREA[21];
                CKSI0050_01.G_ICHIKBN = L_AREA[22];
                L_COUNT = L_COUNT + 1;
            }

            //使用高払い
            if (CKSI0050_01.G_SYUBETU == "2")
            {
                if (Convert.ToDecimal(L_AREA[6]) != L_KSiyoryo)
                {
                    L_Hinmokucd = Strings.Mid(Strings.Trim(Strings.Mid(L_AREA[1], 1, 4)) + Strings.Space(4), 1, 4);
                    L_HINMOKUNM = Strings.Mid(Strings.StrConv(Strings.Trim(Strings.Mid(L_AREA[20], 1, 20)) + Strings.Space(20), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP), 1, 20);
                    L_MOTOCHO = String.Format("{0,11}", C_COMMON.FormatToNum(L_AREA[6], "###,###,##0"));
                    L_KEISAN = String.Format("{0,11}", String.Format("{0:###,###,##0}", L_KSiyoryo));

                    //エラーレコードのリストボックスへの表示
                    LST_LIST.Items.Add(Strings.Space(1) + L_Hinmokucd + Strings.Space(1) + L_HINMOKUNM + Strings.Space(1) + L_MOTOCHO + Strings.Space(1) + L_KEISAN);
                }

                //副資材経理報告トランの作成
                DATA_SAKUSEI(2);
            }
            //入庫払い
            else if (CKSI0050_01.G_SYUBETU == "1")
            {

                if (Convert.ToDecimal(L_AREA[6]) != L_KSiyoryo)
                {
                    L_Hinmokucd = Strings.Mid(Strings.Trim(Strings.Mid(L_AREA[1], 1, 4)) + Strings.Space(4), 1, 4);
                    L_HINMOKUNM = Strings.Mid(Strings.StrConv(Strings.Trim(Strings.Mid(L_AREA[20], 1, 20)) + Strings.Space(20), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP), 1, 20);
                    L_MOTOCHO = String.Format("{0,11}", C_COMMON.FormatToNum(L_AREA[6], "###,###,##0"));
                    L_KEISAN = String.Format("{0,11}", String.Format("{0:###,###,##0}", L_KSiyoryo));

                    //エラーレコードのリストボックスへの表示
                    LST_LIST.Items.Add(Strings.Space(1) + L_Hinmokucd + Strings.Space(1) + L_HINMOKUNM + Strings.Space(1) + L_MOTOCHO + Strings.Space(1) + L_KEISAN);
                }

                //副資材経理報告トランの作成
                DATA_SAKUSEI(1);
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public void DATA_SAKUJYO()
        {
            //副資材経理報告トランの削除

            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "DELETE SIZAI_HOUKOKU_TRN ";

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
                    //トランザクション終了
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

        public short GET_NENGETU()
        {
            short functionReturnValue = 1;

            string[] L_MST_AREA = new string[3];
            DataTable tbl = null;

            //日付管理マスタの検索

            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "Select NEN1,TUKI1 from HIZUKE_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE PKEY = 'DATE' ";

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    // SQL実行
                    db.Connect();
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
                //日付管理マスタが見つかりません。
                IPPAN.Error_Msg("E119", 0, " ");
                return functionReturnValue;
            }

            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                //項目セット
                L_MST_AREA[j] = "";
                L_MST_AREA[j] = tbl.Rows[0][j].ToString();
            }

            //検収年月
            CKSI0050_01.G_NENGETU = Strings.Trim(Strings.Mid(L_MST_AREA[0], 1, 4)) + Strings.Trim(Strings.Mid(L_MST_AREA[1], 1, 2));

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public void GAMEN_CLEAR()
        {
            //画面の初期化
            TXT_NEN.Text = "";
            TXT_TUKI.Text = "";
            LST_LIST.Items.Clear();
        }

        public void LIST_HYOJI(string L_Hinmokucd, string L_HINMOKUNM)
        {
            string L_MOTOCHO = null;
            string L_KEISAN = null;

            L_Hinmokucd = Strings.Mid(Strings.Trim(Strings.Mid(L_Hinmokucd, 1, 4)) + Strings.Space(4), 1, 4);
            L_HINMOKUNM = Strings.Mid(Strings.StrConv(Strings.Trim(Strings.Mid(L_HINMOKUNM, 1, 20)) + Strings.Space(20), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP), 1, 20);
            L_MOTOCHO = String.Format("{0,11}", String.Format("{0:###,###,##0}", CKSI0050_01.G_SIYO));
            L_KEISAN = String.Format("{0,11}", String.Format("{0:###,###,##0}", CKSI0050_01.G_SZAIKO + CKSI0050_01.G_NYUKO - CKSI0050_01.G_SUIBUN - CKSI0050_01.G_EZAIKO));

            //エラーレコードのリストボックスへの表示
            LST_LIST.Items.Add(Strings.Space(1) + L_Hinmokucd + Strings.Space(1) + L_HINMOKUNM + Strings.Space(1) + L_MOTOCHO + Strings.Space(1) + L_KEISAN);
        }

        public void DATA_SAKUSEI(short L_KBN)
        {
            short L_I = 0;
            string[] L_AREA = new string[2];
            string L_TANI = null;
            decimal[] L_SIYORYO = new decimal[10];
            short[] L_ICHIFLG = new short[10];
            DataTable tbl = null;

            //単位の取込
            IPPAN.G_SQL = "";
            switch (L_KBN)
            {
                case 2:
                    IPPAN.G_SQL = "Select TANI from SIZAI_HINMOKU_MST ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKUCD = '" + CKSI0050_01.G_HINMOKUCD[0] + "' ";
                    break;
                case 1:
                    IPPAN.G_SQL = "Select ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "TANI_MST.TANINM ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "from ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKU_MST,TANI_MST ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "WHERE ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKU_MST.HIMOKUCD = '" + CKSI0050_01.G_HIMOKU + "' AND ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKU_MST.TANACD = '" + CKSI0050_01.G_TANABAN + "' AND ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKU_MST.TANICD = TANI_MST.TANICD";
                    break;
            }
            Debug.Print(IPPAN.G_SQL);

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    // SQL実行
                    db.Connect();
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
                return;
            }

            //データがない
            if (tbl.Rows.Count == 0)
            {
                //単位マスタが見つかりません。
                IPPAN.Error_Msg("E106", 0, " ");
                return;
            }

            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                //項目セット
                L_AREA[j] = "";
                L_AREA[j] = tbl.Rows[0][j].ToString();
            }

            //検収年月
            L_TANI = Strings.Mid(Strings.Mid(L_AREA[0], 1, 1) + Strings.StrConv(Strings.Space(1), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP), 1, 1);

            //ワーク初期化
            for (L_I = 0; L_I <= 9; L_I++)
            {
                L_SIYORYO[L_I] = 0;
                L_ICHIFLG[L_I] = 0;
            }

            IPPAN.G_datetime = String.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);

            //副資材経理報告トランの作成

            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "INSERT INTO SIZAI_HOUKOKU_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "( ";
            IPPAN.G_SQL = IPPAN.G_SQL + "HIMOKU, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "UTIWAKE, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "TANABAN, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKUCD, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "YM, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "TANI, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SZAIKO, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "EZAIKO, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "NYUKO, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SEF, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SLF, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SCC, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SSONOTA, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SJIGYO, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "S1JI, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "STD, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "S2JI, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SYOBI1, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SYOBI2, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "TANKA, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "UPDYMD ";
            IPPAN.G_SQL = IPPAN.G_SQL + ") ";
            IPPAN.G_SQL = IPPAN.G_SQL + "VALUES ";
            IPPAN.G_SQL = IPPAN.G_SQL + "( ";

            switch (L_KBN)
            {
                case 1:
                    //入庫払い
                    IPPAN.G_SQL = IPPAN.G_SQL + "'" + CKSI0050_01.G_HIMOKU + "', ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "'" + CKSI0050_01.G_UTIWAKE + "', ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "'" + CKSI0050_01.G_TANABAN + "', ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "'" + Strings.Space(4) + "', ";
                    break;
                case 2:
                    //使用高払い
                    IPPAN.G_SQL = IPPAN.G_SQL + "'" + CKSI0050_01.G_HIMOKU + "', ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "'" + CKSI0050_01.G_UTIWAKE + "', ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "'" + CKSI0050_01.G_TANABAN + "', ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "'" + CKSI0050_01.G_HINMOKUCD[0] + "', ";
                    break;
            }

            //年月
            IPPAN.G_SQL = IPPAN.G_SQL + "'" + CKSI0050_01.G_NENGETU + "', ";
            //単位
            IPPAN.G_SQL = IPPAN.G_SQL + "'" + L_TANI + "', ";
            //月初在庫
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0050_01.G_SZAIKO + ", ";
            //月末在庫
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0050_01.G_EZAIKO + ", ";
            //入庫量
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0050_01.G_NYUKO + ", ";
            //出庫量（ＥＦ）
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0050_01.G_SIYORYO[1] + ", ";
            //出庫量（ＬＦ）
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0050_01.G_SIYORYO[2] + ", ";
            //出庫量（ＣＣ）
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0050_01.G_SIYORYO[3] + ", ";
            //出庫量（その他）
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0050_01.G_SIYORYO[4] + ", ";
            //出庫量（事業開発）
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0050_01.G_SIYORYO[5] + ", ";
            //出庫量（１次切断）
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0050_01.G_SIYORYO[6] + ", ";
            //出庫量（ＴＤ）
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0050_01.G_SIYORYO[7] + ", ";
            //出庫量（２次切断）
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0050_01.G_SIYORYO[8] + ", ";
            //出庫量（予備１）
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0050_01.G_SIYORYO[9] + ", ";
            //出庫量（予備２）
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0050_01.G_SIYORYO[0] + ", ";
            //単価
            IPPAN.G_SQL = IPPAN.G_SQL + "0" + ", ";
            //更新日付
            IPPAN.G_SQL = IPPAN.G_SQL + "to_date('" + IPPAN.G_datetime + "', 'YYYY/MM/DD HH24:MI:SS') ";
            IPPAN.G_SQL = IPPAN.G_SQL + ") ";
            
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
                    //トランザクション終了
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

        public void WORK_SYOKIKA()
        {
            short L_I = 0;

            //ワーク初期化
            CKSI0050_01.G_MOTOYM = "";
            CKSI0050_01.G_SZAIKO = 0;
            CKSI0050_01.G_NYUKO = 0;
            CKSI0050_01.G_SUIBUN = 0;
            CKSI0050_01.G_EZAIKO = 0;
            for (L_I = 0; L_I <= 9; L_I++)
            {
                CKSI0050_01.G_SIYO[L_I] = 0;
            }

            for (L_I = 0; L_I <= 9; L_I++)
            {
                CKSI0050_01.G_SIYORYO[L_I] = 0;
            }
            CKSI0050_01.G_HIMOKU = "";
            CKSI0050_01.G_UTIWAKE = "";
            CKSI0050_01.G_TANABAN = "";
            CKSI0050_01.G_SYUBETU = "";
            CKSI0050_01.G_ICHIKBN = "";
            for (L_I = 0; L_I <= 50; L_I++)
            {
                CKSI0050_01.G_HINMOKUCD[L_I] = "";
                CKSI0050_01.G_HINMOKUNM[L_I] = "";
            }
        }

        public short GAMEN_CHECK()
        {
            short functionReturnValue = 1;

            if (IPPAN.C_Allspace(TXT_NEN.Text) == 0)
            {
                TXT_NEN.Focus();
                TXT_NEN_Enter(TXT_NEN, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            //入力禁止文字チェック
            else if (IPPAN.Input_Check(TXT_NEN.Text) != 0)
            {
                TXT_NEN.Focus();
                TXT_NEN_Enter(TXT_NEN, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }
            else if (IPPAN.Numeric_Check(TXT_NEN.Text) != 0)
            {
                TXT_NEN.Focus();
                TXT_NEN_Enter(TXT_NEN, new System.EventArgs());
                //日付入力エラー
                IPPAN.Error_Msg("E201", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.C_Allspace(TXT_TUKI.Text) == 0)
            {
                TXT_TUKI.Focus();
                TXT_TUKI_Enter(TXT_TUKI, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            //入力禁止文字チェック
            else if (IPPAN.Input_Check(TXT_TUKI.Text) != 0)
            {
                TXT_TUKI.Focus();
                TXT_TUKI_Enter(TXT_TUKI, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }
            else if ((IPPAN.Numeric_Check(TXT_TUKI.Text) != 0) || (Convert.ToInt16(TXT_TUKI.Text) < 1 || Convert.ToInt16(TXT_TUKI.Text) > 12))
            {
                TXT_TUKI.Focus();
                TXT_TUKI_Enter(TXT_TUKI, new System.EventArgs());
                //日付入力エラー
                IPPAN.Error_Msg("E201", 0, " ");
                return functionReturnValue;
            }
            CKSI0050_01.G_NENGETU = "";
            CKSI0050_01.G_NENGETU = TXT_NEN.Text + TXT_TUKI.Text + "01";
            //13.08.22 DSK yoshida start
            //if (IPPAN.Date_Henkan(ref CKSI0050_01.G_NENGETU) != 0)
            //{
            //    TXT_NEN.Focus();
            //    TXT_NEN_Enter(TXT_NEN, new System.EventArgs());
            //    //日付入力エラー
            //    IPPAN.Error_Msg("E201", 0, " ");
            //    return functionReturnValue;
            //}
            //else if (IPPAN.Date_Check2(CKSI0050_01.G_NENGETU) != 0)
            if (IPPAN.Date_Check2(CKSI0050_01.G_NENGETU) != 0)
            //13.08.22 DSK yoshida end
            {
                TXT_NEN.Focus();
                TXT_NEN_Enter(TXT_NEN, new System.EventArgs());
                //日付入力エラー
                IPPAN.Error_Msg("E201", 0, " ");
                return functionReturnValue;
            }
            CKSI0050_01.G_NENGETU = Strings.Mid(CKSI0050_01.G_NENGETU, 1, 6);

            functionReturnValue = 0;
            return functionReturnValue;

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
                    //砂時計ポインタ
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
                    //データを作成します。よろしいですか？
                    dlgRlt = IPPAN.Error_Msg("I701", 4, " ");
                    if (dlgRlt != DialogResult.Yes)
                    {
                        //標準ポインタ
                        IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                        return;
                    }

                    CMD_Button[0].Enabled = false;
                    CMD_Button[1].Enabled = false;

                    //画面の入力内容チェック
                    //砂時計ポインタ
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
                    IPPAN.G_RET = GAMEN_CHECK();
                    if (IPPAN.G_RET != 0)
                    {
                        //標準ポインタ
                        IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                        CMD_Button[0].Enabled = true;
                        CMD_Button[1].Enabled = true;
                        return;
                    }

                    //-------資材元帳作成
                    LBL_MSG.Text = "資材元帳データを作成しています．．．";
                    System.Windows.Forms.Application.DoEvents();

                    using (C_ODBC db = new C_ODBC())
                    {
                        try
                        {
                            //DB接続
                            db.Connect();
                            //トランザクション開始
                            db.BeginTrans();

                            if (Motocho_Kosin.Motocho_Sakujo(db) == 1)
                            {
                                //更新に失敗しました
                                IPPAN.Error_Msg("I503", 0, " ");
                                //SQLデータベース切断
                                db.Disconnect();
                                System.Environment.Exit(0);
                            }

                            //SQLトランザクション終了
                            db.Commit();
                            //トランザクション開始
                            db.BeginTrans();

                            if (Motocho_Kosin.Motocho_Sakusei(db) == 1)
                            {
                                //更新に失敗しました
                                IPPAN.Error_Msg("I503", 0, " ");
                                //SQLデータベース切断
                                db.Disconnect();
                                System.Environment.Exit(0);
                            }

                            //SQLトランザクション終了
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

                    //副資材トランの検索及び
                    //副資材経理報告トランの作成
                    LBL_MSG.Text = "経理報告データを作成しています．．．";
                    System.Windows.Forms.Application.DoEvents();
                    IPPAN.G_RET = DATA_YOMIKOMI();

                    //副資材経理報告トランからホスト伝送用テキストの作成
                    if (IPPAN.G_RET != 0)
                    {
                    }
                    else
                    {
                        LBL_MSG.Text = "ホスト伝送テキストを作成しています．．．";
                        System.Windows.Forms.Application.DoEvents();
                        IPPAN.G_RET = DATA_TENSO();
                    }
// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
                    if (CKSI0050_01.G_START_SIZAI_TANAOROSI_SYSTEM.Equals("1"))
                    {
                        this.Close();
                    }
                    else
                    {
                        CMD_Button[0].Enabled = true;
                        CMD_Button[1].Enabled = false;
                        CMD_Button[0].Focus();
                        IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                        LBL_MSG.BackColor = System.Drawing.ColorTranslator.FromOle(Information.QBColor(9));
                        LBL_MSG.ForeColor = System.Drawing.ColorTranslator.FromOle(Information.QBColor(15));
                        LBL_MSG.Text = "処理は終了しました。閉じるボタンを押して下さい。";
                    }
                    //CMD_Button[0].Enabled = true;
                    //CMD_Button[1].Enabled = false;
                    //CMD_Button[0].Focus();
                    //IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    //LBL_MSG.BackColor = System.Drawing.ColorTranslator.FromOle(Information.QBColor(9));
                    //LBL_MSG.ForeColor = System.Drawing.ColorTranslator.FromOle(Information.QBColor(15));
                    //LBL_MSG.Text = "処理は終了しました。閉じるボタンを押して下さい。";
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.
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

        private void FRM_CKSI0050M_Load(System.Object eventSender, System.EventArgs eventArgs)
        {
            string L_Search = null;
            string L_Command2 = null;

            //プロジェクト名を設定
            C_COMMON.G_COMMON_MSGTITLE = "CKSI0050";

            //コントロール配列にイベントを設定
            setEventCtrlAry();

            Show();

            //初期フォーカス設定
            TXT_NEN.Focus();

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

            // コマンド ライン引数が指定されていないとき。
            if (string.IsNullOrEmpty(IPPAN.G_COMMAND))
            {
                //コマンド ライン引数が指定されていません
                IPPAN.Error_Msg("E507", 0, " ");
                System.Environment.Exit(0);
            }
            else
            {
                // コマンド ライン引数の受け渡し
// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
                string[] args = IPPAN.G_COMMAND.Split(',');
                CKSI0050_01.G_USERID = args[0];
                CKSI0050_01.G_OFFICEID = args[1];
                CKSI0050_01.G_SYOKUICD = args[2];
                if (args.Length >= 4)
                {
                    CKSI0050_01.G_START_SIZAI_TANAOROSI_SYSTEM = args[3];
                    if (args.Length > 4)
                    {
                        CKSI0050_01.G_YEARMONTH = args[4];
                    }
                }
                //CKSI0050_01.G_USERID = Strings.Mid(IPPAN.G_COMMAND, 1, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) - 1);
                //L_Command2 = Strings.Mid(IPPAN.G_COMMAND, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) + 1, 30);
                //CKSI0050_01.G_OFFICEID = Strings.Mid(L_Command2, 1, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) - 1);
                //CKSI0050_01.G_SYOKUICD = Strings.Mid(L_Command2, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) + 1, 2);
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.

                //ユーザーＩＤが副資材ユーザ以外かを見る
                IPPAN.G_RET = FS_USER_CHECK.User_Check(CKSI0050_01.G_USERID);
                if (IPPAN.G_RET != 0)
                {
                    System.Environment.Exit(0);
                }
                else
                {
                    IPPAN.G_RET = IPPAN2.TIMER_SET(CKSI0050_01.G_USERID);
                    if (IPPAN.G_RET != 0)
                    {
                        System.Environment.Exit(0);
                    }
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
            //2013.08.20 DSK yoshida start
            //TXT_NEN.Text = Strings.Mid(CKSI0050_01.G_NENGETU, 3, 2);
            TXT_NEN.Text = Strings.Mid(CKSI0050_01.G_NENGETU, 1, 4);
            //2013.08.20 DSK yoshida end
            TXT_TUKI.Text = Strings.Mid(CKSI0050_01.G_NENGETU, 5, 2);
// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
            if (CKSI0050_01.G_START_SIZAI_TANAOROSI_SYSTEM.Equals("1"))
            {
                if (!String.IsNullOrEmpty(CKSI0050_01.G_YEARMONTH))
                {
                    TXT_NEN.Text = CKSI0050_01.G_YEARMONTH.Substring(0, 4);
                    TXT_TUKI.Text = CKSI0050_01.G_YEARMONTH.Substring(4, 2);
                    this._CMD_Button_0.Enabled = false;
                    TXT_NEN.Enabled = false;
                    TXT_TUKI.Enabled = false;
                }
            }
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.
        }

        private void TMR_TIMER_Tick(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Control_Init(this, "経理報告データ作成", "CKSI0050", SO.SO_USERNAME, SO.SO_OFFICENAME, "1.00");
        }

        private void TXT_NEN_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            ////13.08.08 DSK yoshida start
            //if (TXT_NEN.Visible == true && Strings.Len(TXT_NEN.Text) >= 2)
            if (TXT_NEN.Visible == true && Strings.Len(TXT_NEN.Text) >= 4)
            ////13.08.08 DSK yoshida start
            {
                TXT_TUKI.Focus();
            }
        }

        private void TXT_NEN_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void TXT_NEN_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);

            TXT_NEN.Text = IPPAN2.Numeric_Hensyu3(TXT_NEN.Text);
            //13.08.08 DSK yoshida start
            //TXT_NEN.Text = C_COMMON.FormatToNum(TXT_NEN.Text, "00");
            TXT_NEN.Text = C_COMMON.DateYYChanged(TXT_NEN.Text);
            //13.08.08 DSK yoshida end
        }

        private void TXT_TUKI_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void TXT_TUKI_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);

            TXT_TUKI.Text = IPPAN2.Numeric_Hensyu3(TXT_TUKI.Text);
            TXT_TUKI.Text = C_COMMON.FormatToNum(TXT_TUKI.Text, "00");

            if (IPPAN.C_Allspace(TXT_TUKI.Text) != 0)
            {
                if ((TXT_TUKI.Visible == true) && (Convert.ToInt16(TXT_TUKI.Text) < 1 || Convert.ToInt16(TXT_TUKI.Text) > 12))
                {
                    TXT_TUKI.Focus();
                    TXT_TUKI_Enter(TXT_TUKI, new System.EventArgs());
                    //日付入力エラー
                    IPPAN.Error_Msg("E201", 0, " ");
                    return;
                }
            }
        }
    }
}
