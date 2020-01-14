using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Diagnostics;
using System.Data.Odbc;
using CKLib.Syohizei;
using CKLib.Syohizei.Types;

namespace Project1
{
    static class Kensyu_Kosin
    {
        static int wNyukoSuryo;
        static int wZaiko;
        static int wZaiko2;

        static string F_JYOKENCD;
        static int F_SURYO;
        static decimal F_TANKA;
        static int F_KINGAKU;
        static int F_SYOHIZEI;
        static int F_GKINGAKU;
        static string F_BUMON;

// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -start
        /// <summary>
        /// 消費税リゾルバー
        /// </summary>
        private static SyohizeiResolver syohizeiResolver = null;
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -end

        public static short Kensyu_Sakusei(C_ODBC db, string sKensyuyy, string sKensyumm)
        {
            short functionReturnValue = 1;
            string wYm = null;
            DataTable tbl = null;

            //作業誌、棚卸より品目を抽出する（重複品目は排除）
            if (string.Compare(Strings.Mid(CKSI0060.G_SKYM, 5, 2), "02") < 0)
            {
                wYm = String.Format("{0:0000}", Convert.ToInt32(Strings.Mid(CKSI0060.G_SKYM, 1, 4)) - 1) + "12";
            }
            else
            {
                wYm = Strings.Mid(CKSI0060.G_SKYM, 1, 4) + String.Format("{0:00}", Convert.ToInt32(Strings.Mid(CKSI0060.G_SKYM, 5, 2)) - 1);
            }

            IPPAN.G_SQL = "SELECT B.HIMOKU,B.UTIWAKE,B.TANABAN,A.GYOSYACD,A.HINMOKUCD,B.SYUBETU,B.TANI FROM SIZAI_SAGYOSI_TRN A,SIZAI_HINMOKU_MST B ";
            IPPAN.G_SQL = IPPAN.G_SQL + "where A.HINMOKUCD = B.HINMOKUCD ";
            //曖昧検索
            IPPAN.G_SQL = IPPAN.G_SQL + "AND A.SAGYOBI LIKE '" + CKSI0060.G_SKYM + "__' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "and A.KUBUN <> '2'";
            IPPAN.G_SQL = IPPAN.G_SQL + "UNION ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SELECT B.HIMOKU,B.UTIWAKE,B.TANABAN,A.GYOSYACD,A.HINMOKUCD,B.SYUBETU,B.TANI from SIZAI_TANAOROSI_TRN A,SIZAI_HINMOKU_MST B ";
            IPPAN.G_SQL = IPPAN.G_SQL + "where A.HINMOKUCD = B.HINMOKUCD ";
            IPPAN.G_SQL = IPPAN.G_SQL + "and A.TANAYM >= '" + wYm + "' and A.TANAYM <= '" + CKSI0060.G_SKYM + "' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "and A.GYOSYACD <> '    ' ";
            Debug.Print(IPPAN.G_SQL);

            try
            {
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

            // 結果の列数を取得する
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

                //副資材作業誌トランを検索
                wNyukoSuryo = 0;

                //水分除く
                IPPAN.G_RET = Sagyosi_Kensaku(db, GYOMU.G_TRAN_AREA[4], GYOMU.G_TRAN_AREA[3]);

                //副資材棚卸トランを検索（当月）
                wZaiko = 0;

                //使用高払いのとき
                if (Strings.Mid(GYOMU.G_TRAN_AREA[5], 1, 1) == "2")
                {
                    IPPAN.G_RET = Tanaorosi_Kensaku(db, GYOMU.G_TRAN_AREA[4], GYOMU.G_TRAN_AREA[3]);
                }

                //副資材棚卸トランを検索（前月）
                wZaiko2 = 0;

                //使用高払いのとき
                if (Strings.Mid(GYOMU.G_TRAN_AREA[5], 1, 1) == "2")
                {
                    IPPAN.G_RET = Tanaorosi_Kensaku2(db, GYOMU.G_TRAN_AREA[4], GYOMU.G_TRAN_AREA[3]);
                }

                //副資材検収トラン項目セット＆ＩＮＳＥＲＴ
                if (Kensyu_Set(db, GYOMU.G_TRAN_AREA[4], GYOMU.G_TRAN_AREA[3], sKensyuyy, sKensyumm) == 1)
                {
                    return functionReturnValue;
                }
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private static short Sagyosi_Kensaku(C_ODBC db, string L_Hinmokucd, string L_Gyosyacd)
        {
            short functionReturnValue = 1;
            DataTable tbl = null;

            //副資材作業誌トランの検索
            IPPAN.G_SQL = "SELECT KUBUN,MUKESAKI,SURYO,SUIBUN FROM SIZAI_SAGYOSI_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE SAGYOBI LIKE '" + CKSI0060.G_SKYM + "__' ";
            //曖昧検索
            IPPAN.G_SQL = IPPAN.G_SQL + "and HINMOKUCD = '" + L_Hinmokucd + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + "and GYOSYACD = '" + L_Gyosyacd + "'";

            try
            {
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
                    CKSI0060.G_Sagyosi_Area[j] = tbl.Rows[i][j].ToString();
                }

                switch (Strings.Mid(CKSI0060.G_Sagyosi_Area[0], 1, 1))
                {
                    case "1":
                        //入庫量
                        wNyukoSuryo = wNyukoSuryo + Convert.ToInt32(CKSI0060.G_Sagyosi_Area[2]) - Convert.ToInt32(CKSI0060.G_Sagyosi_Area[3]);
                        break;
                    case "3":
                        //直送量
                        wNyukoSuryo = wNyukoSuryo + Convert.ToInt32(CKSI0060.G_Sagyosi_Area[2]) - Convert.ToInt32(CKSI0060.G_Sagyosi_Area[3]);
                        break;
                    case "4":
                        //返品量
                        wNyukoSuryo = wNyukoSuryo - Convert.ToInt32(CKSI0060.G_Sagyosi_Area[2]) + Convert.ToInt32(CKSI0060.G_Sagyosi_Area[3]);
                        break;
                }

            }

            functionReturnValue = 1;
            return functionReturnValue;
        }

        private static short Tanaorosi_Kensaku(C_ODBC db, string L_Hinmokucd, string L_Gyosyacd)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //副資材棚卸トランの検索（当月）
            IPPAN.G_SQL = "SELECT ZSOUKO+ZEF+ZLF+ZCC+ZSONOTA+ZMETER+ZYOBI1+ZYOBI2 FROM SIZAI_TANAOROSI_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE TANAYM = '" + CKSI0060.G_SKYM + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + "and HINMOKUCD = '" + L_Hinmokucd + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + "and GYOSYACD = '" + L_Gyosyacd + "'";

            try
            {
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
                    CKSI0060.G_Tanaorosi_Area[j] = tbl.Rows[i][j].ToString();
                }

                wZaiko = wZaiko + Convert.ToInt32(CKSI0060.G_Tanaorosi_Area[0]);
            }

            functionReturnValue = 1;
            return functionReturnValue;
        }

        private static short Tanaorosi_Kensaku2(C_ODBC db, string L_Hinmokucd, string L_Gyosyacd)
        {
            short functionReturnValue = 1;
            string wYm = null;
            DataTable tbl = null;

            //副資材棚卸トランの検索（前月）
            if (string.Compare(Strings.Mid(CKSI0060.G_SKYM, 5, 2), "02") < 0)
            {
                wYm = String.Format("{0:0000}", Convert.ToInt32(Strings.Mid(CKSI0060.G_SKYM, 1, 4)) - 1) + "12";
            }
            else
            {
                wYm = Strings.Mid(CKSI0060.G_SKYM, 1, 4) + String.Format("{0:00}", Convert.ToInt32(Strings.Mid(CKSI0060.G_SKYM, 5, 2)) - 1);
            }
            IPPAN.G_SQL = "SELECT ZSOUKO+ZEF+ZLF+ZCC+ZSONOTA+ZMETER+ZYOBI1+ZYOBI2 FROM SIZAI_TANAOROSI_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE TANAYM = '" + wYm + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + "and HINMOKUCD = '" + L_Hinmokucd + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + "and GYOSYACD = '" + L_Gyosyacd + "'";

            try
            {
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
                    CKSI0060.G_Tanaorosi_Area[j] = tbl.Rows[i][j].ToString();
                }

                wZaiko2 = wZaiko2 + Convert.ToInt32(CKSI0060.G_Tanaorosi_Area[0]);
            }

            functionReturnValue = 1;
            return functionReturnValue;
        }

        private static short Kensyu_Set(C_ODBC db, string L_Hinmokucd, string L_Gyosyacd, string sKensyuyy, string sKensyumm)
        {
            short functionReturnValue = 1;

            try
            {
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -start
                syohizeiResolver = SyohizeiResolver.GetInstance();
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -end
                
                //入庫払い
                if (Strings.Mid(GYOMU.G_TRAN_AREA[5], 1, 1) == "1")
                {
                    F_SURYO = wNyukoSuryo;
                    F_BUMON = "04";
                }
                else
                {
                    F_SURYO = wZaiko2 + wNyukoSuryo - wZaiko;
                    //副資材品目マスタを検索し部門をセット
                    if (CKSI0060.Hinmoku_Kensaku(GYOMU.G_TRAN_AREA[4]) == 1)
                    {
                        F_BUMON = "  ";
                    }
                    else
                    {
                        switch (CKSI0060.G_MUKESAKI)
                        {
                            case "1":
                                F_BUMON = "01";
                                break;
                            case "2":
                                F_BUMON = "02";
                                break;
                            case "3":
                                F_BUMON = "03";
                                break;
                            case "8":
                                F_BUMON = "03";
                                break;
                            default:
                                F_BUMON = "  ";
                                break;
                        }
                    }
                }
                //副資材単価マスタを検索し単価、支払条件コードをセット
                if (CKSI0060.Tanka_Kensaku(GYOMU.G_TRAN_AREA[4], GYOMU.G_TRAN_AREA[3]) == 1)
                {
                    F_TANKA = 0;
                    F_JYOKENCD = "";
                }
                else
                {
                    F_TANKA = CKSI0060.G_TANKA;
                    F_JYOKENCD = CKSI0060.G_JYOKENCD;
                }
                F_KINGAKU = (int)IPPAN.Marume_RTN((double)(F_SURYO * F_TANKA), 0, 1);
                //-----------------------------------------------------項目セット
                //検収№
                IPPAN.G_RET = GYOMU.Hakko_Kensaku(db, "UU");
                CKSI0060.G_Kensyu_Area[0] = GYOMU.G_SEQNO;

                //締年月
                // 2014.02.06 DSK NISHI START
                CKSI0060.G_Kensyu_Area[1] = sKensyuyy + sKensyumm;
                //if (string.Compare(sKensyuyy, "95") < 0)
                //{
                //    CKSI0060.G_Kensyu_Area[1] = "20" + sKensyuyy + sKensyumm;
                //}
                //else
                //{
                //    CKSI0060.G_Kensyu_Area[1] = "19" + sKensyuyy + sKensyumm;
                //}
                // 2014.02.06 DSK NISHI END
                //区分
                if (Strings.Mid(GYOMU.G_TRAN_AREA[5], 1, 1) == "1")
                {
                    //入庫払いのとき貯蔵品
                    CKSI0060.G_Kensyu_Area[2] = "1";
                }
                else
                {
                    //使用高払いのとき製造費
                    CKSI0060.G_Kensyu_Area[2] = "0";
                }
                //検収日
                // 2014.02.06 DSK NISHI START
                CKSI0060.G_Kensyu_Area[3] = sKensyuyy + sKensyumm;
                //if (string.Compare(sKensyuyy, "95") >= 0)
                //{
                //    CKSI0060.G_Kensyu_Area[3] = "19" + sKensyuyy + sKensyumm;
                //}
                //else
                //{
                //    CKSI0060.G_Kensyu_Area[3] = "20" + sKensyuyy + sKensyumm;
                //}
                // 2014.02.06 DSK NISHI END
                //-----------------------------------検収日は末日をセット
                if (Strings.Mid(CKSI0060.G_Kensyu_Area[3], 5, 2) == "01" || Strings.Mid(CKSI0060.G_Kensyu_Area[3], 5, 2) == "03" || Strings.Mid(CKSI0060.G_Kensyu_Area[3], 5, 2) == "05" || Strings.Mid(CKSI0060.G_Kensyu_Area[3], 5, 2) == "07" || Strings.Mid(CKSI0060.G_Kensyu_Area[3], 5, 2) == "08" || Strings.Mid(CKSI0060.G_Kensyu_Area[3], 5, 2) == "10" || Strings.Mid(CKSI0060.G_Kensyu_Area[3], 5, 2) == "12")
                {
                    CKSI0060.G_Kensyu_Area[3] = CKSI0060.G_Kensyu_Area[3] + "31";
                }
                else if (Strings.Mid(CKSI0060.G_Kensyu_Area[3], 5, 2) == "04" || Strings.Mid(CKSI0060.G_Kensyu_Area[3], 5, 2) == "04" || Strings.Mid(CKSI0060.G_Kensyu_Area[3], 5, 2) == "06" || Strings.Mid(CKSI0060.G_Kensyu_Area[3], 5, 2) == "09" || Strings.Mid(CKSI0060.G_Kensyu_Area[3], 5, 2) == "11")
                {
                    CKSI0060.G_Kensyu_Area[3] = CKSI0060.G_Kensyu_Area[3] + "30";
                }
                else
                {
                    if (Conversion.Val(Strings.Mid(CKSI0060.G_Kensyu_Area[3], 1, 4)) % 4 == 0)
                    {
                        CKSI0060.G_Kensyu_Area[3] = CKSI0060.G_Kensyu_Area[3] + "29";
                    }
                    else
                    {
                        CKSI0060.G_Kensyu_Area[3] = CKSI0060.G_Kensyu_Area[3] + "28";
                    }
                }
                //業者
                CKSI0060.G_Kensyu_Area[4] = Strings.Mid(GYOMU.G_TRAN_AREA[3], 1, 4);
                if (CKSI0060.Gyosya_Kensaku(CKSI0060.G_Kensyu_Area[4]) == 1)
                {
                    CKSI0060.G_Kensyu_Area[5] = "　　　　　　　　　　　　　　　　　　　　";
                }
                else
                {
                    CKSI0060.G_Kensyu_Area[5] = IPPAN.Space_Set(CKSI0060.G_GYOSYANM, 20, 2);
                }
                //部門
                CKSI0060.G_Kensyu_Area[6] = F_BUMON;
                if (Strings.Mid(GYOMU.G_TRAN_AREA[5], 1, 1) == "2")
                {
// 2015.01.21 yoshitake OBIC7仕訳連携対応(step3) start.
                    if (C_COMMON.GetKouteiNM(F_BUMON, out CKSI0060.G_KAMOKUNM2) == 1)
                    //if (CKSI0060.Kamoku_Kensaku("110", F_BUMON) == 1)
// 2015.01.21 yoshitake OBIC7仕訳連携対応(step3) end.
                    {
                        CKSI0060.G_Kensyu_Area[7] = "　　　　　　　　　　";
                    }
                    else
                    {
                        CKSI0060.G_Kensyu_Area[7] = CKSI0060.G_KAMOKUNM2;
                    }
                }
                else
                {
                    CKSI0060.G_Kensyu_Area[7] = "製鋼共通　　　　　　";
                }
                //費目
                CKSI0060.G_Kensyu_Area[8] = Strings.Mid(GYOMU.G_TRAN_AREA[0], 1, 2);
                if (CKSI0060.G_Kensyu_Area[2] == "1")
                {
                    //貯蔵品のとき科目マスタ検索
                    if (CKSI0060.Kamoku_Kensaku("111", CKSI0060.G_Kensyu_Area[8]) == 1)
                    {
                        CKSI0060.G_Kensyu_Area[9] = "　　　　　　　　　　";
                    }
                    else
                    {
                        CKSI0060.G_Kensyu_Area[9] = IPPAN.Space_Set(CKSI0060.G_KAMOKUNM2, 10, 2);
                    }
                }
                else
                {
                    //製造費のとき費目マスタ検索
// 2015.01.07 nishi OBIC7仕訳連携 Step3対応 start.
                    if (CKSI0060.Kamoku_Kensaku("110", CKSI0060.G_Kensyu_Area[8]) == 1)
                    {
                        CKSI0060.G_Kensyu_Area[9] = "　　　　　　　　　　";
                    }
                    else
                    {
                        CKSI0060.G_Kensyu_Area[9] = IPPAN.Space_Set(CKSI0060.G_KAMOKUNM2, 10, 2);
                    }
                    //if (CKSI0060.Himoku_Kensaku(CKSI0060.G_Kensyu_Area[8]) == 1)
                    //{
                    //    CKSI0060.G_Kensyu_Area[9] = "　　　　　　　　　　";
                    //}
                    //else
                    //{
                    //    CKSI0060.G_Kensyu_Area[9] = IPPAN.Space_Set(CKSI0060.G_HIMOKUNM, 10, 2);
                    //}
// 2015.01.07 nishi OBIC7仕訳連携 Step3対応 end.
                }
                //内訳
                CKSI0060.G_Kensyu_Area[10] = Strings.Mid(GYOMU.G_TRAN_AREA[1], 1, 2);
                if (CKSI0060.Utiwake_Kensaku(CKSI0060.G_Kensyu_Area[8], CKSI0060.G_Kensyu_Area[10]) == 1)
                {
                    CKSI0060.G_Kensyu_Area[11] = "　　　　　　　　　　";
                }
                else
                {
                    CKSI0060.G_Kensyu_Area[11] = IPPAN.Space_Set(CKSI0060.G_UTIWAKENM, 10, 2);
                }
                //棚番
                CKSI0060.G_Kensyu_Area[12] = Strings.Mid(GYOMU.G_TRAN_AREA[2], 1, 2);
                //品目名
                if (CKSI0060.KHinmoku_Kensaku(CKSI0060.G_Kensyu_Area[8], CKSI0060.G_Kensyu_Area[10], CKSI0060.G_Kensyu_Area[12]) == 1)
                {
                    CKSI0060.G_Kensyu_Area[13] = "　　　　　　　　　　　　　　　　　　　　";
                }
                else
                {
                    CKSI0060.G_Kensyu_Area[13] = IPPAN.Space_Set(CKSI0060.G_KHINMOKUNM, 20, 2);
                }
                //支払条件
                CKSI0060.G_Kensyu_Area[14] = IPPAN.Space_Set(F_JYOKENCD, 2, 1);
                if (CKSI0060.Jyoken_Kensaku(CKSI0060.G_Kensyu_Area[14]) == 1)
                {
                    CKSI0060.G_Kensyu_Area[15] = "　　　　　　　　　　　　　　　　　　　　　　　";
                }
                else
                {
                    CKSI0060.G_Kensyu_Area[15] = IPPAN.Space_Set(CKSI0060.G_JYOKENNM, 23, 2);
                }
                //単位
                CKSI0060.G_Kensyu_Area[16] = IPPAN.Space_Set(Strings.Trim(Strings.Mid(GYOMU.G_TRAN_AREA[6], 1, 2)), 2, 2);
                //数量
                CKSI0060.G_Kensyu_Area[17] = Convert.ToString(F_SURYO);
                //単価
                CKSI0060.G_Kensyu_Area[18] = Convert.ToString(F_TANKA);
                //金額
                CKSI0060.G_Kensyu_Area[19] = Convert.ToString(F_KINGAKU);
                //税率区分
                //CKSI0060.G_Kensyu_Area[20] = "1";                                                                                         13.06.12 DELETE
                //消費税
                //F_SYOHIZEI = (int)IPPAN.Marume_RTN((double)(F_KINGAKU * IPPAN2.Get_Syohizei(CKSI0060.G_Kensyu_Area[3]) / 100), 1, 1);     13.06.12 DELETE

// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
                var target = syohizeiResolver.GetSyohizeiDTO(SyohizeiSearchConditionType.YMD, CKSI0060.G_Kensyu_Area[3], RoundingTyp.ZeikomiRound, F_KINGAKU);
                //税率区分
                CKSI0060.G_Kensyu_Area[20] = target.InfoZeiKbn;
                //消費税
                F_SYOHIZEI = (int)target.Syohizei;
                ////13.06.12 tsunamoto 消費税対応 start
                //IPPAN2.ZeiInfo zeiinfo = IPPAN2.Get_Syohizei(CKSI0060.G_Kensyu_Area[3], 3, F_KINGAKU);
                ////税率区分
                //CKSI0060.G_Kensyu_Area[20] = zeiinfo.zeikbn2;
                ////消費税
                //F_SYOHIZEI = (int)zeiinfo.syohizei;
                ////13.06.12 tsunamoto 消費税対応 end
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end

                //消費税計算（切り上げ）
                CKSI0060.G_Kensyu_Area[21] = Convert.ToString(F_SYOHIZEI);
                //金額
                F_GKINGAKU = F_KINGAKU + F_SYOHIZEI;
                //合計金額
                CKSI0060.G_Kensyu_Area[22] = Convert.ToString(F_GKINGAKU);
                //検収済区分
                CKSI0060.G_Kensyu_Area[23] = " ";
                //発注№
                CKSI0060.G_Kensyu_Area[24] = "            ";
                //ページ№
                CKSI0060.G_Kensyu_Area[25] = "0";
                //品目（副資材品目マスタ）
                CKSI0060.G_Kensyu_Area[27] = GYOMU.G_TRAN_AREA[4];
                if (CKSI0060.Hinmoku_Kensaku(GYOMU.G_TRAN_AREA[4]) == 1)
                {
                    CKSI0060.G_Kensyu_Area[28] = "　　　　　　　　　　　　　　　　　　　　";
                }
                else
                {
                    CKSI0060.G_Kensyu_Area[28] = IPPAN.Space_Set(CKSI0060.G_HINMOKUNM, 20, 2);
                }

                //削除フラグ
                CKSI0060.G_Kensyu_Area[29] = " ";

                //副資材検収トランを登録
                if (F_SURYO != 0)
                {
                    if (Kensyu_Insert(db) == 1)
                    {
                        return functionReturnValue;
                    }
                }

                functionReturnValue = 0;
                return functionReturnValue;
            }
            catch (Exception)
            {
                C_COMMON.Msg("エラーが発生しました。品目:" + GYOMU.G_TRAN_AREA[4] + " 業者:" + GYOMU.G_TRAN_AREA[3]);
                return functionReturnValue;
            }
        }

        private static short Kensyu_Insert(C_ODBC db)
        {
            short functionReturnValue = 1;
            string L_datetime = null;

            L_datetime = String.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);

            IPPAN.G_SQL = "insert into SIZAI_KENSYU_TRN values (";
            IPPAN.G_SQL = IPPAN.G_SQL + "'" + CKSI0060.G_Kensyu_Area[0] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[1] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[2] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[3] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[4] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[5] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[6] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[7] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[8] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[9] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[10] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[11] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[12] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[13] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[14] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[15] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[16] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0060.G_Kensyu_Area[17];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0060.G_Kensyu_Area[18];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0060.G_Kensyu_Area[19];
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[20] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0060.G_Kensyu_Area[21];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0060.G_Kensyu_Area[22];
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[23] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[24] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0060.G_Kensyu_Area[25];
            IPPAN.G_SQL = IPPAN.G_SQL + ",to_date('" + L_datetime + "' , 'YYYY/MM/DD HH24:MI:SS')";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[27] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[28] + "'";
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[29] + "'";
            IPPAN.G_SQL += ",NULL)";
            //IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0060.G_Kensyu_Area[29] + "')";
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end
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
    }
}
