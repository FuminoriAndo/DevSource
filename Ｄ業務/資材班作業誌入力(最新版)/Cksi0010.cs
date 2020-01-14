using Microsoft.VisualBasic;
using System;                           //2013.04.13 miura mori
using System.Data;
using System.Diagnostics;
using System.Data.Odbc;
using System.Collections.Generic;

namespace Project1
{
    static class Cksi0010
    {
        //*************************************************************************************
        //
        //
        //
        //   修正履歴
        //
        //   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
        //   03.03.24             NTIS浅田 　Ｓ／Ｏサーバ更新の為、SPACE_SET句の追加
        //*************************************************************************************
               
        //件数用
        public static int G_N;
        //ページ(分子)カウンター用
        public static string G_Page;
        //ページ(分母)カウンター用
        public static int G_Page1;
        //入力年月用ワーク
        public static string G_YMD;
        //数量入力用ワーク
        public static string[] G_Suryo = new string[101];
        //水分引入力用ワーク
        public static string[] G_Suibun = new string[101];
        //dbのワークエリア
        //副資材作業誌トランのエリア
        public static string[,] G_SAGYOSI_AREA = new string[101, 21];
        //副資材品目マスタのエリア
        public static string[] G_HINMOKU_AREA = new string[4];
        //副資材コントロールマスタのエリア
        public static string[] G_CONTROL_AREA = new string[2];
        //業者マスタのエリア
        public static string[] G_GYOSYA_AREA = new string[2];

        //業者初期表示用のエリア
        public static string[] G_Syoki_Gyosya_AREA = new string[3];

        // 副資材班作業誌トラン削除処理
        public static bool Del_Sagyosi(C_ODBC db, string sKey)
        {
            bool functionReturnValue = false;
            IPPAN.G_SQL = "DELETE SIZAI_SAGYOSI_TRN WHERE SAGYOBI = '" + sKey + "'";
            Debug.Print(IPPAN.G_SQL);

            try
            {
                // SQL実行
                // トランザクションは呼出し元で制御する
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

            functionReturnValue = true;
            return functionReturnValue;
        }

        public static short Sagyosi_Touroku(C_ODBC db, string L_YMD)
        {
            short functionReturnValue = 1;
            object L_datetime = null;
            short L_SuibunRyo = 0;
            string L_Seq = null;
            short ix = 0;
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 start.
            string approvalFlg = null;
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 end.

            L_Seq = String.Format("{0:000}", 1);

            for (ix = 0; ix <= ((G_Page1 + 1) * 10) - 1; ix += 1)
            {
                if (IPPAN.C_Allspace(G_SAGYOSI_AREA[ix, 2]) != 0 || IPPAN.C_Allspace(G_SAGYOSI_AREA[ix, 3]) != 0 || IPPAN.C_Allspace(G_SAGYOSI_AREA[ix, 4]) != 0 || IPPAN.C_Allspace(G_SAGYOSI_AREA[ix, 5]) != 0 || IPPAN.C_Allspace(G_SAGYOSI_AREA[ix, 8]) != 0 || IPPAN.C_Allspace(G_SAGYOSI_AREA[ix, 9]) != 0)
                {
                    if (G_SAGYOSI_AREA[ix, 2] == "1" || G_SAGYOSI_AREA[ix, 2] == "2" || G_SAGYOSI_AREA[ix, 2] == "3" || G_SAGYOSI_AREA[ix, 2] == "4")
                    {
                        L_datetime = String.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);
                        if (IPPAN.C_Allspace(G_SAGYOSI_AREA[ix, 8]) != 0 && IPPAN.C_Allspace(G_SAGYOSI_AREA[ix, 9]) != 0)
                        {
                            L_SuibunRyo = Convert.ToInt16(IPPAN.Numeric_Hensyu(Convert.ToString(IPPAN.Marume_RTN(Convert.ToDouble(Convert.ToDouble(G_SAGYOSI_AREA[ix, 8]) * Convert.ToDouble(G_SAGYOSI_AREA[ix, 9]) / 1000), 2, 1) * 10)));
                        }
                        IPPAN.G_SQL = "insert into SIZAI_SAGYOSI_TRN values (";
                        IPPAN.G_SQL = IPPAN.G_SQL + "'" + L_YMD + "'";
                        IPPAN.G_SQL = IPPAN.G_SQL + ",'" + L_Seq + "'";
                        IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(G_SAGYOSI_AREA[ix, 2], 1, 1) + "'";
                        IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(G_SAGYOSI_AREA[ix, 3], 4, 1) + "'";
                        IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(G_SAGYOSI_AREA[ix, 4], 4, 1) + "'";
                        IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(G_SAGYOSI_AREA[ix, 5], 1, 1) + "'";
                        IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(G_SAGYOSI_AREA[ix, 6], 1, 20), 20, 2) + "'";
                        IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(G_SAGYOSI_AREA[ix, 7], 1, 20), 20, 2) + "'";

                        IPPAN.G_SQL = IPPAN.G_SQL + "," + C_COMMON.ChkStrToNum(G_SAGYOSI_AREA[ix, 8]);
                        IPPAN.G_SQL = IPPAN.G_SQL + "," + C_COMMON.ChkStrToNum(G_SAGYOSI_AREA[ix, 9]);
                        IPPAN.G_SQL = IPPAN.G_SQL + "," + L_SuibunRyo;
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 start.
                        IPPAN.G_SQL = IPPAN.G_SQL + ",to_date('" + L_datetime + "', 'YYYY/MM/DD HH24:MI:SS')";
                        approvalFlg = G_SAGYOSI_AREA[ix, 10] = string.IsNullOrEmpty(G_SAGYOSI_AREA[ix, 10]) ? "0" : G_SAGYOSI_AREA[ix, 10];
                        IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(approvalFlg, 1, 1) + "')";
                        //IPPAN.G_SQL = IPPAN.G_SQL + ",to_date('" + L_datetime + "', 'YYYY/MM/DD HH24:MI:SS'))";
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 end.
                        L_Seq = String.Format("{0:000}", Convert.ToDouble(L_Seq) + 1);
                        Debug.Print(IPPAN.G_SQL);

                        try
                        {
                            //SQL実行
                            //トランザクションは呼出し元で制御
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
                    }
                }
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short Sagyosi_Kensaku(string L_YMD)
        {
            //副資材作業誌トランの検索　データがないときは１を返す。
            short functionReturnValue = 1;

            DataTable tbl = null;
            //副資材作業誌トランの検索

            G_N = 0;
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "SELECT SAGYOBI,SEQ,KUBUN,HINMOKUCD,GYOSYACD,";
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 start.
            IPPAN.G_SQL = IPPAN.G_SQL + "MUKESAKI,HINMOKUNM,GYOSYANM,SURYO,SUIBUNRITU,APPROVAL_FLG ";
            //IPPAN.G_SQL = IPPAN.G_SQL + "MUKESAKI,HINMOKUNM,GYOSYANM,SURYO,SUIBUNRITU ";
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 end.
            IPPAN.G_SQL = IPPAN.G_SQL + "FROM SIZAI_SAGYOSI_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE SAGYOBI = '" + L_YMD + "'";
            //申請日で昇順
            IPPAN.G_SQL = IPPAN.G_SQL + " Order By SEQ ASC";

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
                //2013.04.08 miura mori start
                catch (Exception)
                {
                    db.Error();
                }
                //2013.04.08 miura mori end
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
                    G_SAGYOSI_AREA[G_N, j] = tbl.Rows[i][j].ToString();
                }
                if (Convert.ToInt32(Strings.Right(Convert.ToString(G_N), 1)) == 0)
                {
                    G_Page1 = G_Page1 + 1;
                }
                G_N = G_N + 1;
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
        public static short Sagyosi_Kensaku
            (string L_YMD, string ekisanParam, IList<SizaiInOutItem>kensinExcelData)
        {
            //副資材作業誌トランの検索　データがないときは１を返す。
            short functionReturnValue = 1;

            DataTable tbl = null;

            bool isDuplicate = false;

            //副資材作業誌トランの検索
            G_N = 0;
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "SELECT SAGYOBI,SEQ,KUBUN,HINMOKUCD,GYOSYACD,";
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 start.
            IPPAN.G_SQL = IPPAN.G_SQL + "MUKESAKI,HINMOKUNM,GYOSYANM,SURYO,SUIBUNRITU,APPROVAL_FLG ";
            //IPPAN.G_SQL = IPPAN.G_SQL + "MUKESAKI,HINMOKUNM,GYOSYANM,SURYO,SUIBUNRITU ";
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 end.
            IPPAN.G_SQL = IPPAN.G_SQL + "FROM SIZAI_SAGYOSI_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE SAGYOBI = '" + L_YMD + "'";
            //申請日で昇順
            IPPAN.G_SQL = IPPAN.G_SQL + " Order By SEQ ASC";

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
                //2013.04.08 miura mori start
                catch (Exception)
                {
                    db.Error();
                }
                //2013.04.08 miura mori end
            }

            if((tbl.Columns.Count >=1) && (tbl.Rows.Count >=1))
            {
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    isDuplicate = false;

                    foreach (SizaiInOutItem data in kensinExcelData)
                    {
                        if ((data.Kbn.Equals(tbl.Rows[i][2])) && (data.HinmokuCode.Equals(tbl.Rows[i][3])))
                        {
                            isDuplicate = true;
                            break;
                        }
                    }

                    if (!isDuplicate)
                    {
                        for (int j = 0; j < tbl.Columns.Count; j++)
                        {
                            //項目セット
                            G_SAGYOSI_AREA[G_N, j] = tbl.Rows[i][j].ToString();
                        }
                        if (Convert.ToInt32(Strings.Right(Convert.ToString(G_N), 1)) == 0)
                        {
                            G_Page1 = G_Page1 + 1;
                        }

                        G_N = G_N + 1;
                    }
                }
            }

            if (ekisanParam.Equals("1") || ekisanParam.Equals("2"))
            {
                foreach (SizaiInOutItem data in kensinExcelData)
                {
                    if (data.Suryou == "" || data.Suryou == "0")
                    {
                        //何もしない
                    }

                    else
                    {
                        G_SAGYOSI_AREA[G_N, 0] = L_YMD;
                        G_SAGYOSI_AREA[G_N, 1] = string.Empty;
                        G_SAGYOSI_AREA[G_N, 2] = data.Kbn;
                        G_SAGYOSI_AREA[G_N, 3] = data.HinmokuCode;
                        G_SAGYOSI_AREA[G_N, 4] = data.GyosyaCode;
                        G_SAGYOSI_AREA[G_N, 5] = data.Mukesaki;
                        G_SAGYOSI_AREA[G_N, 6] = data.HinmokuName;
                        G_SAGYOSI_AREA[G_N, 7] = data.GyosyaName;
                        G_SAGYOSI_AREA[G_N, 8] = data.Suryou;
                        G_SAGYOSI_AREA[G_N, 9] = data.Suibunhiki;
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 start.
                        G_SAGYOSI_AREA[G_N, 10] = "0"; //未承認
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 end.

                        if (Convert.ToInt32(Strings.Right(Convert.ToString(G_N), 1)) == 0)
                        {
                            G_Page1 = G_Page1 + 1;
                        }

                        G_N = G_N + 1;
                    }
                }
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.

        public static short Control_Kensaku(string L_ID)
        {
            //副資材コントロールマスタの検索　データがないときは１を返す。
            short functionReturnValue = 1;

            DataTable tbl = null;

            //副資材コントロールマスタの検索

            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "SELECT SKYM FROM SIZAI_CONTROL_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE ID = '" + L_ID + "'";

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
                //2013.04.08 miura mori start
                catch (Exception)
                {
                    db.Error();
                }
                //2013.04.08 miura mori end
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
                    G_CONTROL_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short Hinmoku_Kensaku(string L_HMCD)
        {
            //副資材品目マスタの検索　データがないときは１を返す。
            short functionReturnValue = 1;

            //副資材品目マスタの検索
            DataTable tbl = null;

            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "SELECT HINMOKUNM,SUIBUNKBN,MUKESAKI FROM SIZAI_HINMOKU_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKUCD = '" + L_HMCD + "'";

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
                //2013.04.08 miura mori start
                catch (Exception)
                {
                    db.Error();
                }
                //2013.04.08 miura mori end
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
                    G_HINMOKU_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short Syoki_Gyosya_Kensaku(string L_HMCD)
        {
            //資材単価マスタの検索　データがユニークでないときは１を返す。
            short functionReturnValue = 1;

            short W_i = 0;
            short W_j = 0;
            DataTable tbl = null;

            //資材単価マスタの検索

            IPPAN.G_SQL = "";
            IPPAN.G_SQL = IPPAN.G_SQL + "SELECT HINMOKUCD, GYOSYACD ";
            IPPAN.G_SQL = IPPAN.G_SQL + "FROM SIZAI_TANKA_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKUCD = '" + L_HMCD + "'";

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
                //2013.04.08 miura mori start
                catch (Exception)
                {
                    db.Error();
                }
                //2013.04.08 miura mori end
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

            W_i = 0;

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                if (W_i == 1)
                {
                    return functionReturnValue;
                }
                else
                {
                    W_i = 1;
                }

                for (W_j = 1; W_j <= tbl.Columns.Count; W_j++)
                {
                    //項目セット
                    G_Syoki_Gyosya_AREA[W_j] = tbl.Rows[i][W_j - 1].ToString();
                }
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short Gyosya_Kensaku(string L_GSCD)
        {
            //業者マスタの検索　データがないときは１を返す。
            short functionReturnValue = 1;

            DataTable tbl = null;

            //業者マスタの検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "SELECT KOZANM FROM GYOSYA_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE GYOSYACD = '" + L_GSCD + "'";

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
                //2013.04.08 miura mori start
                catch (Exception)
                {
                    db.Error();
                }
                //2013.04.08 miura mori end
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
                    G_GYOSYA_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
        /// <summary>
        /// その他の資材区分で入力してはいけない品目の一覧を取得する
        /// </summary>
        /// <returns>その他の資材区分で入力してはいけない品目の一覧</returns>
        public static IList<string> GetOthersNotInputHinmokuCode()
        {
            IList<string> othersNotInputHinmokuCode = new List<string>();
            DataTable tbl = null;

            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "SELECT HINMOKUCD FROM SIZAI_HINMOKU_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "where (HIMOKU in ('03','04','05','06','07','23') AND SYUBETU='1')　Or (HIMOKU='09' AND UTIWAKE='01' AND TANABAN='03' AND SYUBETU='1')";

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

                catch (Exception)
                {
                    db.Error();
                }
            }

            if (tbl != null)
            {
                if (tbl.Columns.Count < 1)
                {
                    return othersNotInputHinmokuCode;
                }

                //データがない
                if (tbl.Rows.Count == 0)
                {
                    return othersNotInputHinmokuCode;
                }

                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    for (int j = 0; j < tbl.Columns.Count; j++)
                    {
                        //項目セット
                        othersNotInputHinmokuCode.Add(tbl.Rows[i][j].ToString());
                    }
                }
            }

            return othersNotInputHinmokuCode;
        }
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.

// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 start.
        /// <summary>
        /// 指定した品目コード、資材区分の品目データが資材棚卸マスタに存在するかチェックする
        /// </summary>
        /// <param name="hinmokuCode">品目コード</param>
        /// <param name="sizaiCategory">資材区分</param>
        /// <returns>結果</returns>
        public static bool ExistSizaiTanaorosiMst(string hinmokuCode, IList<string> sizaiCategory)
        {
            bool ret = false;
            DataTable tbl = null;
            string sizaiCategoryCondition = string.Empty;

            foreach(var item in sizaiCategory)
            {
                if(sizaiCategoryCondition != string.Empty)
                {
                    sizaiCategoryCondition += ",";
                }

                sizaiCategoryCondition += "'" + item + "'";
            }

            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "SELECT * FROM SIZAI_TANAOROSI_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKUCD='" + hinmokuCode + "' AND SIZAI_KBN IN (" + sizaiCategoryCondition + ")";

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

                catch (Exception)
                {
                    db.Error();
                }
            }

            if (tbl != null)
            {
                //　品目データが存在する
                if (tbl.Rows.Count > 0)
                {
                    ret = true;
                }
            }

            return ret;
        }
// 2017.04.28 yoshitake 副資材棚卸システム再構築 追加対応 end.
    }
}
