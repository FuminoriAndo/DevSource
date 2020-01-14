using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Data.Odbc;

namespace Project1
{
    static class CKSI0060
    {
        public static string G_KBYM;
        public static string G_SKYM;
        public static string G_HIMOKU;
        public static string G_UTIWAKE;
        public static string G_TANABAN;
        public static string G_GYOSYANM;
// 2015.02.25 nishi OBIC7仕訳連携 Step3対応 start.
        //public static string G_HIMOKUNM;
// 2015.02.25 nishi OBIC7仕訳連携 Step3対応 end.
        public static string G_KAMOKUNM2;
        public static string G_UTIWAKENM;
        public static string G_KHINMOKUNM;
        public static string G_JYOKENCD;
        public static string G_JYOKENNM;
        public static decimal G_TANKA;
        public static string G_TANICD;
        public static string G_TANI;
        public static string G_MUKESAKI;
        public static string G_HINMOKUNM;

        public static string G_UserId;
        public static string G_OfficeId;
        public static string G_Syokuicd;
// 2016.12.12 yoshitake 副資材棚卸システム再構築 start
        public static string G_START_SIZAI_TANAOROSI_SYSTEM;
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end

        //マスタエリア
        public static string[] G_HINMOKU_AREA = new string[11];
        public static string[] G_CONTROL_AREA = new string[11];
        public static string[] G_GYOSYA_AREA = new string[51];
        public static string[] G_HIMOKU_AREA = new string[11];
        public static string[] G_KAMOKU_AREA = new string[11];
        public static string[] G_UTIWAKE_AREA = new string[11];
        public static string[] G_KHINMOKU_AREA = new string[11];
        public static string[] G_TANKA_AREA = new string[11];
        public static string[] G_TANI_AREA = new string[11];
        public static string[] G_JYOKEN_AREA = new string[11];

        //トランエリア
        public static string[] G_Sagyosi_Area = new string[21];
        public static string[] G_Tanaorosi_Area = new string[21];
        public static string[] G_Kensyu_Area = new string[51];
        public static string[] G_Motocho_Area = new string[51];

        public static short Hinmoku_Kensaku(string L_Hinmokucd)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //品目マスタの検索
            IPPAN.G_SQL = "SELECT HIMOKU,UTIWAKE,TANABAN,MUKESAKI,HINMOKUNM FROM SIZAI_HINMOKU_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKUCD = '" + L_Hinmokucd + "' ";

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
                    G_HINMOKU_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_HIMOKU = Strings.Mid(G_HINMOKU_AREA[0], 1, 2);
            G_UTIWAKE = Strings.Mid(G_HINMOKU_AREA[1], 1, 2);
            G_TANABAN = Strings.Mid(G_HINMOKU_AREA[2], 1, 2);
            G_MUKESAKI = Strings.Mid(G_HINMOKU_AREA[3], 1, 1);
            G_HINMOKUNM = Strings.Mid(G_HINMOKU_AREA[4], 1, 20);

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short Control_Kensaku()
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //コントロールマスタの検索
            IPPAN.G_SQL = "SELECT * FROM SIZAI_CONTROL_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE ID = 'CKSI'";

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
                    G_CONTROL_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_SKYM = Strings.Mid(G_CONTROL_AREA[1], 1, 6);
            G_KBYM = Strings.Mid(G_CONTROL_AREA[2], 1, 6);

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short Gyosya_Kensaku(string L_Gyosyacd)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //業者マスタの検索
            IPPAN.G_SQL = "SELECT * FROM GYOSYA_MST ";
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
                    G_GYOSYA_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_GYOSYANM = Strings.Mid(G_GYOSYA_AREA[11], 1, 20);

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
        //        catch (OdbcException)
        //        {
        //            db.Error();
        //        }
        //        //2013.04.13 miura mori start
        //        catch (Exception)
        //        {
        //            db.Error();
        //        }
        //        //2013.04.13 miura mori end
        //    }

        //    //結果の列数を取得する
        //    if (tbl.Columns.Count < 1)
        //    {
        //        return functionReturnValue;
        //    }

        //    //データがない
        //    if (tbl.Rows.Count == 0)
        //    {
        //        return functionReturnValue;
        //    }

        //    for (int i = 0; i < tbl.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < tbl.Columns.Count; j++)
        //        {
        //            //項目セット
        //            G_HIMOKU_AREA[j] = tbl.Rows[i][j].ToString();
        //        }
        //    }

        //    G_HIMOKUNM = Strings.Mid(G_HIMOKU_AREA[0], 1, 10);

        //    functionReturnValue = 0;
        //    return functionReturnValue;
        //}
// 2015.02.25 nishi OBIC7仕訳連携 Step3対応 end.
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
                    G_KAMOKU_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_KAMOKUNM2 = Strings.Mid(G_KAMOKU_AREA[0], 1, 10);

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short Utiwake_Kensaku(string L_Himoku, string L_Utiwake)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //製造費、費目内訳マスタの検索
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
                    G_UTIWAKE_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_UTIWAKENM = Strings.Mid(G_UTIWAKE_AREA[0], 1, 10);

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short KHinmoku_Kensaku(string L_Himoku, string L_Utiwake, string L_Tanaban)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //品目マスタの検索
            IPPAN.G_SQL = "SELECT HINMOKUNM,TANICD FROM HINMOKU_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HIMOKUCD = '" + L_Himoku + "' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "and UTIWAKECD = '" + L_Utiwake + "' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "and TANACD = '" + L_Tanaban + "'";

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
                    G_KHINMOKU_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }
// 2017.1.24 yoshitake 副資材棚卸システム再構築 start.
            G_KHINMOKUNM = Strings.Mid(G_KHINMOKU_AREA[0], 1, 20);
            //G_KHINMOKUNM = Strings.Mid(G_KHINMOKU_AREA[0], 1, 10);
// 2017.1.24 yoshitake 副資材棚卸システム再構築 end.
            G_TANICD = Strings.Mid(G_KHINMOKU_AREA[1], 1, 2);

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short Tanka_Kensaku(string L_Hinmokucd, string L_Gyosyacd)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //単価マスタの検索
            IPPAN.G_SQL = "SELECT TANKA,JYOKENCD FROM SIZAI_TANKA_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKUCD = '" + L_Hinmokucd + "' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "and GYOSYACD = '" + L_Gyosyacd + "' ";

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
                    G_TANKA_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_TANKA = Convert.ToDecimal(Strings.Mid(G_TANKA_AREA[0], 1, 20));
            G_JYOKENCD = Strings.Mid(G_TANKA_AREA[1], 1, 2);

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short Tani_Kensaku(string L_Tanicd)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //単位マスタの検索
            IPPAN.G_SQL = "SELECT TANINM FROM TANI_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE TANICD = '" + L_Tanicd + "' ";

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
                    G_TANI_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_TANI = Strings.Mid(G_TANI_AREA[0], 1, 20);

            functionReturnValue = 0;
            return functionReturnValue;
        }

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
                    G_JYOKEN_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_JYOKENNM = Strings.Mid(G_JYOKEN_AREA[0], 1, 23);

            functionReturnValue = 0;
            return functionReturnValue;
        }
    }
}
