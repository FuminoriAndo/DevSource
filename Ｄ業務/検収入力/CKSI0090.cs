using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Data.Odbc;

namespace Project1
{
    static class CKSI0090
    {

        //**************************************************************'
        //                                                              '
        //   更新履歴                                                   '
        //   更新日付    Rev     修正者      内容                       '
        //   13.06.12            tsunamoto   消費税対応                 '
        //**************************************************************'

        public static string G_UserId;
        public static string G_OfficeId;
        public static string G_Syokuicd;

        //口座名義（漢字）
        public static string G_Gyosyanm;
        public static string G_Nen;
        public static string G_Tuki;
        public static string G_Kamokunm1;
        public static string G_Kamokunm2;
        public static string G_Mukesaki;
        public static string G_Himoku;
        public static string G_Himokunm;
        public static string G_Utiwake;
        public static string G_Utiwakenm;
        public static string G_Tanaban;
        public static string G_Hinmokunm;
        public static string G_Hinmokunm2;
        public static string G_Jyokencd;
        public static string G_Jyokennm;
        public static string G_KBYM;

        public static string G_Suryo;
        public static string G_Tanka;
        public static string G_Kingaku;
        public static string G_Syohizei;
        public static string G_Gkingaku;

        public static string[,] G_Kensyu_Area = new string[101, 41];
        public static int G_Current_Data;
        public static string[] G_Siwake_Area = new string[101];
        //--------------------------------

        //口座名義（ＡＮＫ）
        public static string GS_Gyosyanm;
        public static string GS_Ginkonm;
        public static string GS_Sitennm;
        public static string GS_Kozano;
        //仕訳業者コード
        public static string GS_Siwakegcd;
        //仕訳業者名
        public static string GS_Siwakegnm;
        public static string GS_Syubetu;

        public static short Hizuke_Kensaku()
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //日付マスタの検索
            IPPAN.G_SQL = "SELECT NEN1,TUKI1 FROM HIZUKE_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE PKEY = 'DATE'";

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
            }

            G_Nen = Strings.Mid(GYOMU.G_TRAN_AREA[0], 1, 4);
            G_Tuki = Strings.Mid(GYOMU.G_TRAN_AREA[1], 1, 2);
            
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
                    GYOMU.G_TRAN_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_Gyosyanm = Strings.Mid(GYOMU.G_TRAN_AREA[11], 1, 20);
            GS_Gyosyanm = Strings.Mid(GYOMU.G_TRAN_AREA[10], 1, 30);
            GS_Ginkonm = Strings.Mid(GYOMU.G_TRAN_AREA[5], 1, 15);
            GS_Sitennm = Strings.Mid(GYOMU.G_TRAN_AREA[7], 1, 15);
            GS_Syubetu = Strings.Mid(GYOMU.G_TRAN_AREA[8], 1, 1);
            GS_Kozano = Strings.Mid(GYOMU.G_TRAN_AREA[9], 1, 7);
            GS_Siwakegcd = Strings.Mid(GYOMU.G_TRAN_AREA[18], 1, 3);

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short Siwakeg_Kensaku(string L_Siwakegcd)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //仕訳業者マスタの検索
            IPPAN.G_SQL = "SELECT GYOSYANM FROM SIWAKEG_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE SIWAKEGCD = '" + L_Siwakegcd + "'";

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
            }

            GS_Siwakegnm = Strings.Mid(GYOMU.G_TRAN_AREA[0], 1, 10);

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
            }

            G_Kamokunm2 = Strings.Mid(GYOMU.G_TRAN_AREA[0], 1, 10);

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
        //            GYOMU.G_TRAN_AREA[j] = tbl.Rows[i][j].ToString();
        //        }
        //    }

        //    G_Himokunm = Strings.Mid(GYOMU.G_TRAN_AREA[0], 1, 10);

        //    functionReturnValue = 0;
        //    return functionReturnValue;
        //}
// 2015.02.25 nishi OBIC7仕訳連携 Step3対応 end.

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
                    GYOMU.G_TRAN_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_Utiwakenm = Strings.Mid(GYOMU.G_TRAN_AREA[0], 1, 10);

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short Hinmoku_Kensaku(string L_Himoku, string L_Utiwake, string L_Tanaban)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //品目マスタの検索
            IPPAN.G_SQL = "SELECT HINMOKUNM FROM HINMOKU_MST ";
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
                    GYOMU.G_TRAN_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_Hinmokunm = Strings.Mid(GYOMU.G_TRAN_AREA[0], 1, 20);

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short SHinmoku_Kensaku(string L_Hinmokucd)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //副資材品目マスタの検索
            IPPAN.G_SQL = "SELECT HINMOKUNM,HIMOKU,UTIWAKE,TANABAN,MUKESAKI FROM SIZAI_HINMOKU_MST ";
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
                    GYOMU.G_TRAN_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_Hinmokunm2 = Strings.Mid(GYOMU.G_TRAN_AREA[0], 1, 20);
            G_Himoku = Strings.Mid(GYOMU.G_TRAN_AREA[1], 1, 2);
            G_Utiwake = Strings.Mid(GYOMU.G_TRAN_AREA[2], 1, 2);
            G_Tanaban = Strings.Mid(GYOMU.G_TRAN_AREA[3], 1, 2);
            G_Mukesaki = Strings.Mid(GYOMU.G_TRAN_AREA[4], 1, 1);

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short STanka_Kensaku(string L_Gyosyacd, string L_Hinmokucd)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //副資材品目マスタの検索
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
                    GYOMU.G_TRAN_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_Tanka = Strings.Mid(GYOMU.G_TRAN_AREA[0], 1, 20);
            G_Jyokencd = Strings.Mid(GYOMU.G_TRAN_AREA[1], 1, 2);

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
                    GYOMU.G_TRAN_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_Jyokennm = Strings.Mid(GYOMU.G_TRAN_AREA[0], 1, 23);

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short Control_Kensaku()
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //コントロールマスタの検索
            IPPAN.G_SQL = "SELECT KBYM FROM SIZAI_CONTROL_MST ";
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
                    GYOMU.G_TRAN_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

            G_KBYM = Strings.Mid(GYOMU.G_TRAN_AREA[0], 1, 6);

            functionReturnValue = 0;
            return functionReturnValue;
        }

        //13.06.12 Syohizei_Kensakuを削除し、Get_Syohizeiに統一 DELETE start
        //消費税率取得（指定年月日）
        //public static decimal Syohizei_Kensaku(string L_KBN, string L_I)
        //{
        //    decimal functionReturnValue = default(decimal);
        //    string[] vSyohizeiMstBuf = new string[11];
        //    DataTable tbl = null;

            //初期値設定
        //    functionReturnValue = 5;

        //    IPPAN.G_SQL = "";
        //    if (L_I == "1")
        //    {
        //        IPPAN.G_SQL = "SELECT ZEIRITU ";
        //        IPPAN.G_SQL = IPPAN.G_SQL + "FROM SYOHIZEI_MST WHERE ZEIKBN1 = '" + L_KBN + "'";
        //    }
        //    else
        //    {
        //        IPPAN.G_SQL = "SELECT ZEIRITU ";
        //        IPPAN.G_SQL = IPPAN.G_SQL + "FROM SYOHIZEI_MST WHERE ZEIKBN2 = '" + L_KBN + "'";
        //    }

        //    using (C_ODBC db = new C_ODBC())
        //    {
        //        try
        //        {
                    //DB接続
        //            db.Connect();
                    //SQL実行
        //            tbl = db.ExecSQL(IPPAN.G_SQL);
        //        }
        //        catch (OdbcException)
        //        {
        //            db.Error();
        //        }
        //        //2013.04.13 miura mori start
         //       catch (Exception)
        //        {
        //            db.Error();
        //        }
                //2013.04.13 miura mori end
        //    }

            //結果の列数を取得する
        //    if (tbl.Columns.Count < 1)
        //    {
        //        return functionReturnValue;
        //    }

            //データがない
        //    if (tbl.Rows.Count == 0)
        //    {
        //        return functionReturnValue;
        //    }

        //    for (int j = 0; j < tbl.Columns.Count; j++)
        //    {
                //項目セット
        //        vSyohizeiMstBuf[j] = tbl.Rows[0][j].ToString();
        //    }

            //税率
        //    functionReturnValue = Convert.ToDecimal(vSyohizeiMstBuf[0]);
        //    return functionReturnValue;
        //}
        //13.06.12 Syohizei_Kensakuを削除し、Get_Syohizeiに統一 DELETE end
         
    }
}
