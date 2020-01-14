using Microsoft.VisualBasic;
using System;
using System.Data;

namespace Project1
{
    static class CKSI0130
    {
        //*************************************************************************************
        //
        //
        //   プログラム名　　　副資材品目マスタメンテ
        //
        //
        //   修正履歴
        //
        //   作成年月日  Ｒｅｖ  　担当者　　 　修正内容
        //　　13.07.03    　　　　　ISV-TRUC    新画面対応
        //    13.07.19              HIT綱本     SQL文の変更
        //*************************************************************************************

        //dbのワークエリア
        public static string[] G_WORK_AREA = new string[21];
        public static string[] G_HINMOKU_AREA = new string[2];
        public static string[] G_KEIRI_AREA = new string[2];
        public static string[] G_ICHI_AREA = new string[2];
        public static string[,] G_TANKA_AREA = new string[11, 6];
        public static string[] G_GYOSYA_AREA = new string[2];
        public static string[] G_JYOKEN_AREA = new string[2];

        //  13.07.03    ISV-TRUC    品目マスタのデータテーブル
        public static DataTable dtbHinmoku = new DataTable();

        //  13.07.03    ISV-TRUC    資材品目メンテ画面　列挙型
        public enum Mode { INSERT_MODE = 1, UPDATE_MODE, DELETE_MODE };
        //  13.07.03    ISV-TRUC end
       

        //コマンドライン情報格納エリア
        public static string G_UserId;
        public static string G_OfficeId;
        public static string G_SyokuiCd;
        //副資材品目マスタのワークエリア
        public static string G_FS_HINMOKUCD;
        public static string G_FS_HINMOKUNM;
        public static string G_FS_HIMOKU;
        public static string G_FS_UTIWAKE;
        public static string G_FS_TANABAN;
        public static string G_FS_TANI;
        public static string G_FS_SYUBETU;
        public static string G_FS_SUIBUNKBN;
        public static string G_FS_KENSYUKBN;
        public static string G_FS_HOUKOKUKBN;
        public static string G_FS_ICHIKBN;
        public static string G_FS_MUKESAKI;
        public static string G_FS_UPDYMD;
        //科目マスタのワークエリア
        public static string G_Kamokunm;
        //費目マスタのワークエリア
        public static string G_Himokunm;
        //内訳マスタのワークエリア
        public static string G_Utiwakenm;
        //日付チェックのワークエリア
        public static string G_CHECK;
        //単価のワークエリア
        public static string[] G_Tanka = new string[11];
        //件数用
        public static short G_N;
        //仕訳確認画面リターンコード
        public static short CKSI0130S01_RET;

        public struct HINMOKU_WORK
        {
            //副資材品目マスタdb用ワークエリア
            //品目コード
            public string HINMOKUCD;
            //品目名
            public string HINMOKUNM;
            //費目
            public string HIMOKU;
            //内訳
            public string UTIWAKE;
            //棚番
            public string TANABAN;
            //単位
            public string TANI;
            //種別
            public string SYUBETU;
            //水分引き区分
            public string SUIBUNKBN;
            //検収明細出力区分
            public string KENSYUKBN;
            //経理報告区分
            public string HOUKOKUKBN;
            //出庫位置区分
            public string ICHIKBN;
            //向先区分
            public string MUKESAKI;
        }
        public static HINMOKU_WORK HINMOKU;

        public static short Jyoken_Kensaku(string L_Jyokencd)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //条件マスタの検索
            IPPAN.G_SQL = "SELECT JYOKENNM FROM JYOKEN_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE JYOKENCD = '" + L_Jyokencd + "'";

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

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    //項目セット
                    G_JYOKEN_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        //業者マスタの検索　データがないときは１を返す。
        public static short Gyosya_Kensaku(string L_Gyosyacd)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //業者マスタの検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "SELECT KOZANM FROM GYOSYA_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE GYOSYACD = '" + L_Gyosyacd + "'";

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

        //副資材単価マスタの検索　データがないときは１を返す。
        public static short Fs_Tanka_Kensaku(string L_Hincd)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            G_N = 0;
            //副資材単価マスタの検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "SELECT HINMOKUCD,GYOSYACD,";
            IPPAN.G_SQL = IPPAN.G_SQL + "TANKA,JYOKENCD,UPDYMD ";
            IPPAN.G_SQL = IPPAN.G_SQL + "FROM SIZAI_TANKA_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKUCD = '" + L_Hincd + "'";

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

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    //項目セット
                    G_TANKA_AREA[G_N, j] = tbl.Rows[i][j].ToString();
                }
                G_N++;
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        //副資材品目マスタの検索 データがないときは１を返す。
        public static short Syuko_Kensaku(string L_Hincd, string L_Himoku, string L_Utiwake, string L_Tanaban)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //副資材品目マスタ検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "Select ICHIKBN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "From SIZAI_HINMOKU_MST Where HINMOKUCD <> '" + L_Hincd + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + " and HIMOKU = '" + L_Himoku + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + " and UTIWAKE = '" + L_Utiwake + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + " and TANABAN = '" + L_Tanaban + "'";

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

            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                //項目セット
                G_ICHI_AREA[j] = tbl.Rows[0][j].ToString();
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        //副資材品目マスタの検索 データがないときは１を返す。
        public static short Keiri_Houkoku_Kensaku(string L_Hincd, string L_Himoku, string L_Utiwake, string L_Tanaban)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //副資材品目マスタ検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "Select HOUKOKUKBN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "From SIZAI_HINMOKU_MST Where HINMOKUCD <> '" + L_Hincd + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + " and HIMOKU = '" + L_Himoku + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + " and UTIWAKE = '" + L_Utiwake + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + " and TANABAN = '" + L_Tanaban + "'";

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
                functionReturnValue = 2;
                return functionReturnValue;
            }

            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                //項目セット
                G_KEIRI_AREA[j] = tbl.Rows[0][j].ToString();
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short SIZAI_HINMOKU_CHECK(C_ODBC db, Boolean bSakujoChk, Boolean bSinkiChk, string sHinCd)
        {
            short functionReturnValue = 1;
            string L_SQL = null;

            DataTable tbl = null;

            L_SQL = "SELECT UPDYMD FROM SIZAI_HINMOKU_MST ";

            if (bSakujoChk == true)
            {
                L_SQL = L_SQL + "WHERE HINMOKUCD ='" + sHinCd + "'";
            }
            else
            {
                L_SQL = L_SQL + "WHERE HINMOKUCD ='" + HINMOKU.HINMOKUCD + "'";
            }

            try
            {
                //トランザクションは呼出し元で制御
                //排他ロック実施
                db.TableLockEX("SIZAI_HINMOKU_MST");
                //SQL実行
                tbl = db.ExecSQL(L_SQL);
            }
            catch (Exception)
            {
                db.Error();
            }

            //結果の列数を取得する
            if (tbl.Columns.Count < 1)
            {
                return functionReturnValue;
            }

            //ﾃﾞｰﾀがない
            if (tbl.Rows.Count == 0)
            {
                if (bSinkiChk == true)
                {
                    functionReturnValue = 0;
                }
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

            G_CHECK = G_HINMOKU_AREA[0];

            //追加でデータあり
            if (bSinkiChk == true)
            {
                functionReturnValue = 2;
                return functionReturnValue;
            }

            //更新日時が不一致
            if (G_CHECK != G_FS_UPDYMD)
            {
                return functionReturnValue;
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short Utiwake_Kensaku(string L_Himoku, string L_Utiwake)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //製造費・費目内訳マスタの検索
            IPPAN.G_SQL = "SELECT HIMOUTINM FROM SEIZO_HIMOKU_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HIMOKUCD = '" + L_Himoku + "' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "and UTIWAKECD = '" + L_Utiwake + "'";

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

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    //項目セット
                    G_WORK_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_Utiwakenm = Strings.Mid(G_WORK_AREA[0], 1, 10);

            functionReturnValue = 0;
            return functionReturnValue;
        }
        public static short Kamoku_Kensaku(string L_Kamoku, string L_Bumon)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;
          
            //科目マスタの検索
// 2015.01.21 yoshitake OBIC7仕訳連携対応(step3) start.
            IPPAN.G_SQL = "SELECT MEISAINM FROM KAIKEI_KAMOKU_MST ";
// 2015.01.21 yoshitake OBIC7仕訳連携対応(step3) end.
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE KAMOKUCD = '" + L_Kamoku + "' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "and MEISAICD = '" + L_Bumon + "'";

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

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    //項目セット
                    G_WORK_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_Kamokunm = Strings.Mid(G_WORK_AREA[0], 1, 10);

            functionReturnValue = 0;
            return functionReturnValue;
        }

// 2015.02.25 nishi OBIC7仕訳連携 Step3対応 start.
        //public static short Himoku_Kensaku(string L_Himoku)
        //{
        //    short functionReturnValue = 1;

        //    DataTable tbl = null;
            
        //    //費目マスタの検索
        //    IPPAN.G_SQL = "SELECT HIMOKUNM FROM HIMOKU_MST ";
        //    IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HIMOKUCD = '" + L_Himoku + "'";

        //    using (C_ODBC db = new C_ODBC())
        //    {
        //        try
        //        {
        //            //DB接続
        //            db.Connect();
        //            //SQL実行
        //            tbl = db.ExecSQL(IPPAN.G_SQL);
        //        }
        //        catch (Exception)
        //        {
        //            db.Error();
        //        }
        //    }

        //    //結果の列数を取得する
        //    if (tbl.Columns.Count < 1)
        //    {
        //        return functionReturnValue;
        //    }

        //    //ﾃﾞｰﾀがない
        //    if (tbl.Rows.Count == 0)
        //    {
        //        return functionReturnValue;
        //    }

        //    for (int i = 0; i < tbl.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < tbl.Columns.Count; j++)
        //        {
        //            //項目セット
        //            G_WORK_AREA[j] = tbl.Rows[i][j].ToString();
        //        }
        //    }

        //    G_Himokunm = Strings.Mid(G_WORK_AREA[0], 1, 10);

        //    functionReturnValue = 0;
        //    return functionReturnValue;
        //}
// 2015.02.25 nishi OBIC7仕訳連携 Step3対応 end.

        //副資材品目マスタの検索 データがないときは１を返す。
        public static short Fs_Hinmoku_Kensaku(string L_Hincd)
        {
            short functionReturnValue = 1;

            G_FS_HINMOKUCD = "";
            G_FS_HINMOKUNM = "";
            G_FS_HIMOKU = "";
            G_FS_UTIWAKE = "";
            G_FS_TANABAN = "";
            G_FS_TANI = "";
            G_FS_SYUBETU = "";
            G_FS_SUIBUNKBN = "";
            G_FS_KENSYUKBN = "";
            G_FS_HOUKOKUKBN = "";
            G_FS_ICHIKBN = "";
            G_FS_MUKESAKI = "";
            G_FS_UPDYMD = "";

            DataTable tbl = null;

            //副資材品目マスタ検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "Select HINMOKUCD,HINMOKUNM,HIMOKU,UTIWAKE,TANABAN,TANI,";
            IPPAN.G_SQL = IPPAN.G_SQL + "SYUBETU,SUIBUNKBN,KENSYUKBN,HOUKOKUKBN,ICHIKBN,MUKESAKI,UPDYMD ";
            IPPAN.G_SQL = IPPAN.G_SQL + "From SIZAI_HINMOKU_MST Where HINMOKUCD = '" + L_Hincd + "'";

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

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    //項目セット
                    G_WORK_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_FS_HINMOKUCD = G_WORK_AREA[0];
            G_FS_HINMOKUNM = G_WORK_AREA[1];
            G_FS_HIMOKU = G_WORK_AREA[2];
            G_FS_UTIWAKE = G_WORK_AREA[3];
            G_FS_TANABAN = G_WORK_AREA[4];
            G_FS_TANI = G_WORK_AREA[5];
            G_FS_SYUBETU = G_WORK_AREA[6];
            G_FS_SUIBUNKBN = G_WORK_AREA[7];
            G_FS_KENSYUKBN = G_WORK_AREA[8];
            G_FS_HOUKOKUKBN = G_WORK_AREA[9];
            G_FS_ICHIKBN = G_WORK_AREA[10];
            G_FS_MUKESAKI = G_WORK_AREA[11];
            G_FS_UPDYMD = G_WORK_AREA[12];

            functionReturnValue = 0;
            return functionReturnValue;
        }

        /// <summary>
        /// 13.07.03    ISV-TRUC    資材品目マスタを得る。
        /// Get All Hinmoku Master
        /// </summary>
        /// <returns>
        /// 0 : Success
        /// 1 : Fail
        /// </returns>
        public static short Get_All_Hinmoku_Master()
        {
            short functionReturnValue = 1;

            //副資材品目マスタ検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = IPPAN.G_SQL + "Select";
            IPPAN.G_SQL = IPPAN.G_SQL + " h.HINMOKUCD";
            IPPAN.G_SQL = IPPAN.G_SQL + ",trim(trim('　' from h.HINMOKUNM)) as HINMOKUNM"; //13.07.19 tsunamoto trim追加
            IPPAN.G_SQL = IPPAN.G_SQL + ",h.HIMOKU";
            IPPAN.G_SQL = IPPAN.G_SQL + ",h.UTIWAKE";
            IPPAN.G_SQL = IPPAN.G_SQL + ",h.TANABAN";
            IPPAN.G_SQL = IPPAN.G_SQL + ",h.TANI";
            IPPAN.G_SQL = IPPAN.G_SQL + ",h.SYUBETU";
            IPPAN.G_SQL = IPPAN.G_SQL + ",h.SUIBUNKBN";
            IPPAN.G_SQL = IPPAN.G_SQL + ",h.KENSYUKBN";
            IPPAN.G_SQL = IPPAN.G_SQL + ",h.HOUKOKUKBN";
            IPPAN.G_SQL = IPPAN.G_SQL + ",h.ICHIKBN";
            IPPAN.G_SQL = IPPAN.G_SQL + ",h.MUKESAKI";
            IPPAN.G_SQL = IPPAN.G_SQL + ",Case h.MUKESAKI"; 
            IPPAN.G_SQL = IPPAN.G_SQL + "   when '1' then 'EF'"; 
            IPPAN.G_SQL = IPPAN.G_SQL + "   when '2' then 'LF'";
            IPPAN.G_SQL = IPPAN.G_SQL + "   when '3' then 'CC'";
            IPPAN.G_SQL = IPPAN.G_SQL + "   when '4' then 'その他'";
            IPPAN.G_SQL = IPPAN.G_SQL + "   when '5' then '事業開発'";
            IPPAN.G_SQL = IPPAN.G_SQL + "   when '6' then '１次切断'";
            IPPAN.G_SQL = IPPAN.G_SQL + "   when '7' then 'TD'";
            IPPAN.G_SQL = IPPAN.G_SQL + "   when '8' then '２次切断' end as KOUSAKI";
            IPPAN.G_SQL = IPPAN.G_SQL + ",Case h.ICHIKBN";
            IPPAN.G_SQL = IPPAN.G_SQL + "   when '1' then 'EF'";
            IPPAN.G_SQL = IPPAN.G_SQL + "   when '2' then 'LF'";
            IPPAN.G_SQL = IPPAN.G_SQL + "   when '3' then 'CC'";
            IPPAN.G_SQL = IPPAN.G_SQL + "   when '4' then 'その他'";
            IPPAN.G_SQL = IPPAN.G_SQL + "   when '5' then '事業開発'";
            IPPAN.G_SQL = IPPAN.G_SQL + "   when '6' then '１次切断'";
            IPPAN.G_SQL = IPPAN.G_SQL + "   when '7' then 'TD'";
            IPPAN.G_SQL = IPPAN.G_SQL + "   when '8' then '２次切断' end as SHUKKOICHI";
            IPPAN.G_SQL = IPPAN.G_SQL + ",Case";
            IPPAN.G_SQL = IPPAN.G_SQL + "   when not exists (select t.HINMOKUCD from SIZAI_TANKA_MST t where t.HINMOKUCD= h.HINMOKUCD) then ' '";
            IPPAN.G_SQL = IPPAN.G_SQL + "   else '○'";
            IPPAN.G_SQL = IPPAN.G_SQL + "end  as TANKASETTE";
            IPPAN.G_SQL = IPPAN.G_SQL + " From SIZAI_HINMOKU_MST h";
            IPPAN.G_SQL = IPPAN.G_SQL + " Order by h.HINMOKUCD";

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    //DB接続
                    db.Connect();
                    //SQL実行
                    CKSI0130.dtbHinmoku  = db.ExecSQL(IPPAN.G_SQL);
                }
                catch (Exception)
                {
                    db.Error();
                }
            }

            //結果の列数を取得する
            if (CKSI0130.dtbHinmoku.Columns.Count < 1)
            {
                return functionReturnValue;
            }           
            functionReturnValue = 0;
            return functionReturnValue;
        }

        /// <summary>
        /// Jyoken_Gyosya_Kensaku
        /// </summary>
        /// <param name="Gyosyacd">Gyosyacd</param>
        /// <returns>
        /// ０：成功
        /// １：失敗する
        /// </returns>
        public static short Jyoken_Gyosya_Kensaku(string Gyosyacd)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //条件マスタの検索
            IPPAN.G_SQL = "SELECT j.JYOKENCD, j.JYOKENNM FROM JYOKEN_MST j";
            IPPAN.G_SQL = IPPAN.G_SQL + " JOIN GYOSYA_MST g ON j.JYOKENCD = g.JYOKENCD";
            IPPAN.G_SQL = IPPAN.G_SQL + " WHERE g.GYOSYACD = '" + Gyosyacd + "'";

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

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    //項目セット
                    G_JYOKEN_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }
            functionReturnValue = 0;
            return functionReturnValue;
        }

    }
}
