using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Data.Odbc;

namespace Project1
{
    static class CKSI0140
    {
        //**************************************************************'
        //                                                              '
        //   プログラム名　　　仕訳データ作成                           '
        //                                                              '
        //    年月日              担当者                                '
        //                                                              '
        //   修正履歴                                                   '
        //   修正年月日  Ｒｅｖ  　担当者　　 　修正内容                '
        //   03.02.12             NTIS浅田　  Ｓ／Ｏサーバ更新の影響により、多重FreeStmt部分をコメント化
        //   13.06.12             HIT 綱本    消費税対応                '
        //**************************************************************'

        public static string G_UserId;
        public static string G_OfficeId;
        public static string G_Syokuicd;

        //口座名義（漢字）
        public static string G_Gyosyanm;
        public static string G_Nen;
        public static string G_Tuki;
        public static string G_KBYM;
        public static string G_SIWAKEFLG;

        public static string[] G_Siwake_Area = new string[101];
        public static string[] G_Kensyu_Area = new string[51];
        //--------------------------------
        //口座名義（ＡＮＫ）
        public static string GS_Gyosyanm;
        public static string GS_Ginkonm;
        public static string GS_Sitennm;
        public static string GS_Kozano;
        //仕訳業者コード
//2015/09/26 yoshitake 不具合修正 -start
        //public static string GS_Siwakegcd;
//2015/09/26 yoshitake 不具合修正 -end
        //仕訳業者名
//2015/09/26 yoshitake 不具合修正 -start
        //public static string GS_Siwakegnm;
//2015/09/26 yoshitake 不具合修正 -end
        public static string GS_Syubetu;
        //単位名
        public static string G_TANI;

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
//2015/09/26 yoshitake 不具合修正 -start
            //GS_Siwakegcd = Strings.Mid(GYOMU.G_TRAN_AREA[18], 1, 3);
//2015/09/26 yoshitake 不具合修正 -end

            functionReturnValue = 0;
            return functionReturnValue;
        }
//2015/09/26 yoshitake 不具合修正 -start
        //public static short Siwakeg_Kensaku(string L_Siwakegcd)
        //{
        //    short functionReturnValue = 1;

        //    DataTable tbl = null;

        //    //仕訳業者マスタの検索
        //    IPPAN.G_SQL = "SELECT GYOSYANM FROM SIWAKEG_MST ";
        //    IPPAN.G_SQL = IPPAN.G_SQL + "WHERE SIWAKEGCD = '" + L_Siwakegcd + "'";

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

        //    GS_Siwakegnm = Strings.Mid(GYOMU.G_TRAN_AREA[0], 1, 10);

        //    functionReturnValue = 0;
        //    return functionReturnValue;
        //}
//2015/09/26 yoshitake 不具合修正 -end

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
                    GYOMU.G_TRAN_AREA[j] = tbl.Rows[i][j].ToString();
                    //項目セット
                }
            }

            G_KBYM = Strings.Mid(GYOMU.G_TRAN_AREA[2], 1, 6);
            G_SIWAKEFLG = Strings.Mid(GYOMU.G_TRAN_AREA[5], 1, 6);

            functionReturnValue = 0;
            return functionReturnValue;
        }


        // 13.06.12 get_syohizeiに統一するため削除  start
         //public static decimal Syohizei_Kensaku(string L_KBN, string L_I)
         //{
         //    decimal functionReturnValue = default(decimal);
             //string[] vSyohizeiMstBuf = new string[11];
             //DataTable tbl = null;

             ////消費税率取得（指定年月日）

             ////初期値設定
             //functionReturnValue = 5;

             //IPPAN.G_SQL = "";
             //if (L_I == "1")
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
         //            vSyohizeiMstBuf[j] = tbl.Rows[i][j].ToString();
         //        }
         //    }

         //    //税率
         //    functionReturnValue = Convert.ToDecimal(vSyohizeiMstBuf[0]);
         //    return functionReturnValue;
         //}
         // 13.06.12 get_syohizeiに統一するため削除  end
         

        public static short Tani_Kensaku(string L_Himoku, string L_Utiwake, string L_Tanaban)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //単位マスタの検索
            IPPAN.G_SQL = "SELECT TANINM FROM HINMOKU_MST,TANI_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE HINMOKU_MST.HIMOKUCD = '" + L_Himoku + "' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "and HINMOKU_MST.UTIWAKECD = '" + L_Utiwake + "' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "and HINMOKU_MST.TANACD = '" + L_Tanaban + "' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "and TANI_MST.TANICD = HINMOKU_MST.TANICD ";

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

            G_TANI = Strings.Mid(GYOMU.G_TRAN_AREA[0], 1, 2);

            functionReturnValue = 0;
            return functionReturnValue;
        }
    }
}
