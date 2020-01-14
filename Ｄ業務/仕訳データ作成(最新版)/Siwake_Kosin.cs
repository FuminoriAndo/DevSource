using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Diagnostics;
using System.Data.Odbc;
using CKLib.Syohizei;
using CKLib.Syohizei.Types;

namespace Project1
{
    static class Siwake_Kosin
    {
        //*************************************************************************************
        //
        //   プログラム名
        //
        //   修正履歴
        //
        //   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
        //   2003.02.25            NTIS浅田 　  Ｓ／Ｏサーバ更新の為、SPACE_SET句の追加
        //   2013.06.12            HIT 綱本     消費税対応
        //*************************************************************************************
        static short F_INIT;

        static string F_KENSYUNO;
        static string F_SIMEYM;
        static string F_KUBUN;
        static string F_KENSYUBI;
        static string F_GYOSYACD;
        static string F_GYOSYANM;
        static string F_BUMON;
        static string F_BUMONNM;
        static string F_HIMOKU;
        static string F_HIMOKUNM;
        static string F_UTIWAKE;
        static string F_UTIWAKENM;
        static string F_TANABAN;
        static string F_HINMOKUNM;
        static string F_JYOKENCD;
        static string F_JYOKENNM;
        static string F_TANI;
        static decimal F_SURYO;
        static decimal F_TANKA;
        static decimal F_KINGAKU;
        static string F_ZEIKBN;
        static decimal F_SYOHIZEI;
        static decimal F_GKINGAKU;
        static string F_KENSYUFLG;
        static string F_HACHUNO;
        static string F_PAGENO;
        static string F_UPDYMD;
        static string F_HINMOKUCD2;
        static string F_HINMOKUNM2;
        static string F_DELFLG;
        static string F_ZEIRITU_HOJO_KBN;

// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -start
        /// <summary>
        /// 消費税リゾルバー
        /// </summary>
        private static SyohizeiResolver syohizeiResolver = null;
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -end

        public static short Siwake_Sakusei(string KBYM)
        {
            short functionReturnValue = 1;
            DataTable tbl = null;

            F_INIT = 1;

            //副資材検収トランを検索
            IPPAN.G_SQL = "SELECT * FROM SIZAI_KENSYU_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "where SIMEYM = '" + KBYM + "' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "and KENSYUFLG = '1' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "and DELFLG = ' ' ";
            IPPAN.G_SQL = IPPAN.G_SQL + "order by GYOSYACD,KUBUN,HIMOKU,UTIWAKE,TANABAN ";
            Debug.Print(IPPAN.G_SQL);

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
                    CKSI0140.G_Kensyu_Area[j] = tbl.Rows[i][j].ToString();
                }

                //仕訳申請トラン２項目セット＆ＩＮＳＥＲＴ
                if (Siwake_Set() == 1)
                {
                    return functionReturnValue;
                }
            }

            //仕訳申請トラン２項目セット＆ＩＮＳＥＲＴ（最後のデータ）
            CKSI0140.G_Kensyu_Area[4] = "";
            CKSI0140.G_Kensyu_Area[8] = "";
            CKSI0140.G_Kensyu_Area[10] = "";
            CKSI0140.G_Kensyu_Area[12] = "";
            CKSI0140.G_Kensyu_Area[2] = "";

            if (Siwake_Set() == 1)
            {
                return functionReturnValue;
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private static short Siwake_Set()
        {
            short functionReturnValue = 1;

            try
            {
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -start
                syohizeiResolver = SyohizeiResolver.GetInstance();
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Add -end
                
                if (Strings.Mid(CKSI0140.G_Kensyu_Area[4], 1, 4) == F_GYOSYACD && Strings.Mid(CKSI0140.G_Kensyu_Area[8], 1, 2) == F_HIMOKU && Strings.Mid(CKSI0140.G_Kensyu_Area[10], 1, 2) == F_UTIWAKE && Strings.Mid(CKSI0140.G_Kensyu_Area[12], 1, 2) == F_TANABAN && Strings.Mid(CKSI0140.G_Kensyu_Area[2], 1, 1) == F_KUBUN)
                {
                    if (F_HIMOKU == "09" && F_UTIWAKE == "02" && F_TANABAN == "06")
                    {
                        switch (CKSI0140.G_Kensyu_Area[27])
                        {
                            case "A003":
                            case "A007":
                                F_SURYO = F_SURYO + Convert.ToDecimal(CKSI0140.G_Kensyu_Area[17]);
                                F_TANI = "Ｍ";
                                break;
                            default:
                                F_SURYO = F_SURYO + 0;
                                break;
                        }
                    }
                    else
                    {
                        if (Strings.Mid(CKSI0140.G_Kensyu_Area[16], 1, 1) == F_TANI)
                        {
                            F_SURYO = F_SURYO + Convert.ToDecimal(CKSI0140.G_Kensyu_Area[17]);
                        }
                        else
                        {
                            F_SURYO = 1;
                            F_TANI = "式";
                        }
                    }
                    if (Convert.ToDecimal(CKSI0140.G_Kensyu_Area[18]) != F_TANKA)
                    {
                        F_TANKA = 0;
                    }
                    F_GKINGAKU = F_GKINGAKU + Convert.ToDecimal(CKSI0140.G_Kensyu_Area[22]);
                }
                else
                {
                    //-----------------------------------------------------項目セット
                    if (F_INIT == 0)
                    {
                        using (C_ODBC db = new C_ODBC())
                        {
                            try
                            {
                                //DB接続
                                db.Connect();
                                //トランザクション開始
                                db.BeginTrans();
                                //Siwake_Insert内でコミットする
                                IPPAN.G_RET = GYOMU.Hakko_Kensaku(db, "SD");
                                CKSI0140.G_Siwake_Area[0] = GYOMU.G_SEQNO;
                                CKSI0140.G_Siwake_Area[1] = "00";
// 2014.11.11 Nishi OBIC7仕訳連携 start.
                                //CKSI0140.G_Siwake_Area[2] = F_BUMON;
// 2014.11.11 Nishi OBIC7仕訳連携 end.
                                CKSI0140.G_Siwake_Area[3] = F_KENSYUBI;
                                CKSI0140.G_Siwake_Area[4] = "B";
                                CKSI0140.G_Siwake_Area[5] = "05";
                                CKSI0140.G_Siwake_Area[6] = "05";
                                CKSI0140.G_Siwake_Area[7] = "SD    ";
                                IPPAN.G_RET = GYOMU.Denpyo_Hakko_Kensaku(db, "ZZ", Strings.Mid(CKSI0140.G_Siwake_Area[3], 1, 6));
                                CKSI0140.G_Siwake_Area[8] = GYOMU.G_SEQNO;
                                CKSI0140.G_Siwake_Area[9] = GYOMU.G_SEQNO;
                                if (F_KUBUN == "0")
                                {
                                    CKSI0140.G_Siwake_Area[10] = "110";
// 2015.01.29 yoshitake OBIC7仕訳連携対応(step3) start.
                                    CKSI0140.G_Siwake_Area[12] = F_HIMOKU + F_UTIWAKE + "000000";
                                    CKSI0140.G_Siwake_Area[80] = F_BUMON;
                                    //CKSI0140.G_Siwake_Area[12] = F_BUMON + F_HIMOKU + F_UTIWAKE + "0000";
// 2015.01.29 yoshitake OBIC7仕訳連携対応(step3) end.
// 2014.11.11 Nishi OBIC7仕訳連携 start.
                                    CKSI0140.G_Siwake_Area[2] = "27";
// 2014.11.11 Nishi OBIC7仕訳連携 end.
                                }
                                else
                                {
                                    CKSI0140.G_Siwake_Area[10] = "111";
// 2015.01.30 yoshitake OBIC7仕訳連携対応(step3) start.
                                    CKSI0140.G_Siwake_Area[12] = F_HIMOKU + F_UTIWAKE + "000000";
                                    CKSI0140.G_Siwake_Area[80] = "04";
                                    //CKSI0140.G_Siwake_Area[12] = F_HIMOKU + "00" + F_UTIWAKE + "0004";
// 2015.01.30 yoshitake OBIC7仕訳連携対応(step3) end.
// 2014.11.11 Nishi OBIC7仕訳連携 start.
                                    CKSI0140.G_Siwake_Area[2] = "  ";
// 2014.11.11 Nishi OBIC7仕訳連携 end.
                                }
                                CKSI0140.G_Siwake_Area[11] = "2";
                                CKSI0140.G_Siwake_Area[13] = "402";
                                CKSI0140.G_Siwake_Area[14] = "4";
                                if (CKSI0140.Gyosya_Kensaku(F_GYOSYACD) == 1)
                                {
                                    //業者マスタが見つかりません
                                    IPPAN.Error_Msg("E102", 0, " ");
                                    return functionReturnValue;
                                }
// 2015.02.04 yoshitake OBIC7仕訳連携対応(step3) start.
                                CKSI0140.G_Siwake_Area[15] = "0000000000";
                                //CKSI0140.G_Siwake_Area[15] = "00" + Strings.Mid(CKSI0140.GS_Siwakegcd, 1, 3) + "00000";
// 2015.02.04 yoshitake OBIC7仕訳連携対応(step3) end.
                                CKSI0140.G_Siwake_Area[16] = "120";
                                CKSI0140.G_Siwake_Area[17] = "2";
                                CKSI0140.G_Siwake_Area[18] = "0000000000";
                                CKSI0140.G_Siwake_Area[19] = "402";
                                CKSI0140.G_Siwake_Area[20] = "4";
// 2015.02.04 yoshitake OBIC7仕訳連携対応(step3) start.
                                CKSI0140.G_Siwake_Area[21] = "0000000000";
                                //CKSI0140.G_Siwake_Area[21] = "00" + Strings.Mid(CKSI0140.GS_Siwakegcd, 1, 3) + "00000";
// 2015.02.04 yoshitake OBIC7仕訳連携対応(step3) end.
                                if (F_KUBUN == "0")
                                {
                                    CKSI0140.G_Siwake_Area[22] = IPPAN.Space_Set("製造費", 10, 2);
                                }
                                else
                                {
                                    CKSI0140.G_Siwake_Area[22] = IPPAN.Space_Set("貯蔵品", 10, 2);
                                }
                                CKSI0140.G_Siwake_Area[23] = IPPAN.Space_Set("買掛金", 10, 2);
                                CKSI0140.G_Siwake_Area[24] = IPPAN.Space_Set("仮払消費税", 10, 2);
                                CKSI0140.G_Siwake_Area[25] = IPPAN.Space_Set("買掛金", 10, 2);
                                //合計金額から金額を逆算（小数第一位四捨五入）
                                //if (F_ZEIKBN == "1")                                                                                                      13.06.12 DELETE
                                //{                                                                                                                         13.06.12 DELETE
                                //    F_KINGAKU = (decimal)IPPAN.Marume_RTN((double)(F_GKINGAKU / (CKSI0140.Syohizei_Kensaku(" ", "1") / 100 + 1)), 0, 1);  13.06.12 DELETE
                                //}                                                                                                                         13.06.12 DELETE
                                //else                                                                                                                      13.06.12 DELETE
                                //{                                                                                                                         13.06.12 DELETE
                                //    F_KINGAKU = (decimal)IPPAN.Marume_RTN((double)(F_GKINGAKU / (CKSI0140.Syohizei_Kensaku("1", "1") / 100 + 1)), 0, 1);  13.06.12 DELETE
                                //}                                                                                                                         13.06.12 DELETE
                                //F_SYOHIZEI = F_GKINGAKU - F_KINGAKU;                                                                                      13.06.12 DELETE

// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -start
                                var target = syohizeiResolver.GetSyohizeiDTO(SyohizeiSearchConditionType.InfoZeiKbn, F_ZEIKBN, RoundingTyp.ZeinukiRound, (double)F_GKINGAKU);
                                F_KINGAKU = (decimal)target.ZeinukiKingaku;
                                F_SYOHIZEI = (decimal)target.Syohizei;
                                ////13.06.12 tsunamoto 消費税対応 start
                                //IPPAN2.ZeiInfo zeiinfo = IPPAN2.Get_Syohizei(2, F_ZEIKBN, 1, (double)F_GKINGAKU);
                                //F_KINGAKU  = (decimal)zeiinfo.zeinuki;
                                //F_SYOHIZEI = (decimal)zeiinfo.syohizei;
                                ////13.06.12 tsunamoto 消費税対応 end
// 2019/04/15 DSK yoshitake 消費税10%に伴う精算システム軽減税率対応 Mod -end

                                //合計金額－金額で消費税を計算
                                CKSI0140.G_Siwake_Area[26] = Convert.ToString(F_SYOHIZEI);
                                CKSI0140.G_Siwake_Area[27] = Convert.ToString(F_KINGAKU);
                                CKSI0140.G_Siwake_Area[28] = Convert.ToString(F_KINGAKU);
                                CKSI0140.G_Siwake_Area[29] = Convert.ToString(F_GKINGAKU);
                                CKSI0140.G_Siwake_Area[30] = IPPAN.Space_Set(F_HIMOKU, 3, 1);
                                CKSI0140.G_Siwake_Area[31] = IPPAN.Space_Set(F_HIMOKUNM, 10, 2);
                                CKSI0140.G_Siwake_Area[32] = "   ";
                                CKSI0140.G_Siwake_Area[33] = "　　　　　　　　　　";
                                CKSI0140.G_Siwake_Area[34] = IPPAN.Space_Set(F_UTIWAKE, 3, 1);
                                CKSI0140.G_Siwake_Area[35] = IPPAN.Space_Set(F_UTIWAKENM, 10, 2);
//2015/09/26 yoshitake 不具合修正 -start
                                //if (CKSI0140.Siwakeg_Kensaku(Strings.Mid(CKSI0140.GS_Siwakegcd, 1, 3)) == 1)
                                //{
                                //    //仕訳業者マスタが見つかりません
                                //    IPPAN.Error_Msg("E506", 0, " ");
                                //    return functionReturnValue;
                                //}
//2015/09/26 yoshitake 不具合修正 -end
//2015/09/26 yoshitake 不具合修正 -start
                                CKSI0140.G_Siwake_Area[36] = IPPAN.Space_Set(string.Empty, 3, 1);
                                //CKSI0140.G_Siwake_Area[36] = Strings.Mid(CKSI0140.GS_Siwakegcd, 1, 3);
//2015/09/26 yoshitake 不具合修正 -end
//2015/09/26 yoshitake 不具合修正 -start
                                CKSI0140.G_Siwake_Area[37] = IPPAN.Space_Set(string.Empty, 5, 2);
                                //CKSI0140.G_Siwake_Area[37] = Strings.Mid(CKSI0140.GS_Siwakegnm, 1, 10);
//2015/09/26 yoshitake 不具合修正 -end
                                CKSI0140.G_Siwake_Area[38] = IPPAN.Space_Set(F_TANABAN, 10, 1);
                                CKSI0140.G_Siwake_Area[39] = "          ";
                                CKSI0140.G_Siwake_Area[40] = "   ";
                                CKSI0140.G_Siwake_Area[41] = "   ";
                                CKSI0140.G_Siwake_Area[42] = F_ZEIKBN;
                                CKSI0140.G_Siwake_Area[43] = F_GYOSYACD;
                                CKSI0140.G_Siwake_Area[44] = F_JYOKENCD;
                                CKSI0140.G_Siwake_Area[45] = IPPAN.Space_Set(F_HINMOKUNM, 30, 2);
                                CKSI0140.G_Siwake_Area[46] = Strings.Mid(F_TANI, 1, 1);
                                CKSI0140.G_Siwake_Area[47] = Convert.ToString(System.Math.Abs(F_SURYO));
                                CKSI0140.G_Siwake_Area[48] = Convert.ToString(F_TANKA);
                                CKSI0140.G_Siwake_Area[49] = Strings.Mid(CKSI0140.GS_Gyosyanm, 1, 9);
                                CKSI0140.G_Siwake_Area[50] = IPPAN.Space_Set(Strings.Mid(CKSI0140.GS_Ginkonm, 1, 9), 9, 1);
                                CKSI0140.G_Siwake_Area[51] = IPPAN.Space_Set(Strings.Mid(CKSI0140.GS_Sitennm, 1, 9), 9, 1);
                                if (CKSI0140.GS_Syubetu == "1")
                                {
                                    CKSI0140.G_Siwake_Area[52] = IPPAN.Space_Set("ﾌ " + CKSI0140.GS_Kozano, 9, 1);
                                }
                                else
                                {
                                    CKSI0140.G_Siwake_Area[52] = IPPAN.Space_Set("ﾄ " + CKSI0140.GS_Kozano, 9, 1);
                                }
                                CKSI0140.G_Siwake_Area[53] = "   ";
                                CKSI0140.G_Siwake_Area[54] = "          ";
                                if (F_TANKA == 0)
                                {
                                    CKSI0140.G_Siwake_Area[55] = IPPAN.Space_Set(Strings.StrConv(Strings.Mid(CKSI0140.G_Siwake_Area[0], 1, 2) + Strings.Mid(CKSI0140.G_Siwake_Area[0], 5, 8), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP), 30, 2);
                                }
                                else
                                {
                                    CKSI0140.G_Siwake_Area[55] = IPPAN.Space_Set(Strings.StrConv(Strings.Mid(CKSI0140.G_Siwake_Area[0], 1, 2) + Strings.Mid(CKSI0140.G_Siwake_Area[0], 5, 8) + "　単価：" + String.Format("{0:##,###,##0.000}", F_TANKA), VbStrConv.Wide, C_COMMON.G_CMN_LOCALID_JP), 30, 2);
                                }
                                if (F_SURYO < 0)
                                {
                                    CKSI0140.G_Siwake_Area[56] = IPPAN.Space_Set("返品", 30, 2) + IPPAN.Space_Set("", 30, 2);
                                }
                                else
                                {
                                    CKSI0140.G_Siwake_Area[56] = IPPAN.Space_Set("", 30, 2) + IPPAN.Space_Set("", 30, 2);
                                }
                                CKSI0140.G_Siwake_Area[57] = GYOMU.G_SEQNO;
                                CKSI0140.G_Siwake_Area[58] = " ";
                                CKSI0140.G_Siwake_Area[59] = CKSI0140.G_Siwake_Area[0];
                                CKSI0140.G_Siwake_Area[60] = CKSI0140.G_Siwake_Area[1];
                                CKSI0140.G_Siwake_Area[61] = "            ";
                                CKSI0140.G_Siwake_Area[62] = " ";
                                CKSI0140.G_Siwake_Area[63] = " ";
                                CKSI0140.G_Siwake_Area[64] = " ";
                                CKSI0140.G_Siwake_Area[65] = " ";
                                CKSI0140.G_Siwake_Area[66] = "                 ";
                                CKSI0140.G_Siwake_Area[67] = "　　　　　　　　　　　　　　　";
                                CKSI0140.G_Siwake_Area[68] = "　　　　　　　　　　　　　　　";
                                CKSI0140.G_Siwake_Area[69] = "　　　　　　　　　　　　　　　";
                                CKSI0140.G_Siwake_Area[70] = "　　　　　　　　　　　　　　　";
                                CKSI0140.G_Siwake_Area[71] = "　　　　　　　　　　　　　　　";
                                CKSI0140.G_Siwake_Area[72] = "　　　　　　　　　　　　　　　";
                                CKSI0140.G_Siwake_Area[73] = IPPAN.Space_Set(SO.SO_USERNAME, 15, 2);
                                CKSI0140.G_Siwake_Area[74] = IPPAN.Space_Set(CKSI0140.G_UserId, 4, 1);
                                CKSI0140.G_Siwake_Area[76] = "    ";
                                CKSI0140.G_Siwake_Area[77] = " ";
                                CKSI0140.G_Siwake_Area[78] = " ";
                                CKSI0140.G_Siwake_Area[79] = " ";

                                CKSI0140.G_Siwake_Area[81] = F_ZEIRITU_HOJO_KBN;
                                //仕訳申請トラン２を登録
                                if (Siwake_Insert(db) == 1)
                                {
                                    return functionReturnValue;
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
                    }

                    F_INIT = 0;

                    F_KENSYUNO = CKSI0140.G_Kensyu_Area[0];
                    F_SIMEYM = CKSI0140.G_Kensyu_Area[1];
                    F_KUBUN = Strings.Mid(CKSI0140.G_Kensyu_Area[2], 1, 1);
                    F_KENSYUBI = CKSI0140.G_Kensyu_Area[3];
                    F_GYOSYACD = Strings.Mid(CKSI0140.G_Kensyu_Area[4], 1, 4);
                    F_GYOSYANM = Strings.Mid(CKSI0140.G_Kensyu_Area[5], 1, 20);
                    F_BUMON = CKSI0140.G_Kensyu_Area[6];
                    F_BUMONNM = Strings.Mid(CKSI0140.G_Kensyu_Area[7], 1, 10);
                    F_HIMOKU = Strings.Mid(CKSI0140.G_Kensyu_Area[8], 1, 2);
                    F_HIMOKUNM = Strings.Mid(CKSI0140.G_Kensyu_Area[9], 1, 10);
                    F_UTIWAKE = Strings.Mid(CKSI0140.G_Kensyu_Area[10], 1, 2);
                    F_UTIWAKENM = Strings.Mid(CKSI0140.G_Kensyu_Area[11], 1, 10);
                    F_TANABAN = Strings.Mid(CKSI0140.G_Kensyu_Area[12], 1, 2);
                    F_HINMOKUNM = Strings.Mid(CKSI0140.G_Kensyu_Area[13], 1, 20);
                    F_HINMOKUCD2 = CKSI0140.G_Kensyu_Area[27];
                    F_HINMOKUNM2 = Strings.Mid(CKSI0140.G_Kensyu_Area[28], 1, 20);
                    F_JYOKENCD = CKSI0140.G_Kensyu_Area[14];
                    F_JYOKENNM = Strings.Mid(CKSI0140.G_Kensyu_Area[15], 1, 23);
                    F_TANI = Strings.Mid(CKSI0140.G_Kensyu_Area[16], 1, 1);
                    F_SURYO = Convert.ToDecimal(CKSI0140.G_Kensyu_Area[17]);
                    if (F_HIMOKU == "09" && F_UTIWAKE == "02" && F_TANABAN == "06")
                    {
                        switch (F_HINMOKUCD2)
                        {
                            case "A003":
                            case "A007":
                                F_SURYO = Convert.ToDecimal(CKSI0140.G_Kensyu_Area[17]);
                                break;
                            default:
                                F_SURYO = 0;
                                break;
                        }
                    }
                    F_TANKA = Convert.ToDecimal(CKSI0140.G_Kensyu_Area[18]);
                    F_KINGAKU = Convert.ToDecimal(CKSI0140.G_Kensyu_Area[19]);
                    F_ZEIKBN = CKSI0140.G_Kensyu_Area[20];
                    F_SYOHIZEI = Convert.ToDecimal(CKSI0140.G_Kensyu_Area[21]);
                    F_GKINGAKU = Convert.ToDecimal(CKSI0140.G_Kensyu_Area[22]);
                    F_KENSYUFLG = CKSI0140.G_Kensyu_Area[23];
                    F_HACHUNO = CKSI0140.G_Kensyu_Area[24];
                    F_PAGENO = CKSI0140.G_Kensyu_Area[25];
                    F_UPDYMD = CKSI0140.G_Kensyu_Area[26];
                    F_DELFLG = CKSI0140.G_Kensyu_Area[29];
                    F_ZEIRITU_HOJO_KBN = CKSI0140.G_Kensyu_Area[30];
                }
            }
            catch (Exception)
            {
                C_COMMON.Msg("エラーが発生しました。品目:" + CKSI0140.G_Kensyu_Area[27] + " 業者:" + CKSI0140.G_Kensyu_Area[4]);
                return functionReturnValue;
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private static short Siwake_Insert(C_ODBC db)
        {
            short functionReturnValue = 1;
            string L_datetime = null;

            L_datetime = String.Format("{0:yyyyMMdd HH:mm:ss}", DateTime.Now);
// 2015.02.02 yoshitake OBIC7仕訳連携対応(step3) start.
            IPPAN.G_SQL = "insert into SIWAKE_SINSEI2_KAIKEI_TRN values (";
            //IPPAN.G_SQL = "insert into SIWAKE_SINSEI2_TRN values (";
// 2015.02.02 yoshitake OBIC7仕訳連携対応(step3) end.
            IPPAN.G_SQL = IPPAN.G_SQL + "'" + CKSI0140.G_Siwake_Area[0] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[1] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[2] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[3] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[4] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[5] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[6] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[7] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[8] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[9] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[10] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[11] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[12] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[13] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[14] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[15] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[16] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[17] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[18] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[19] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[20] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[21] + "'";

            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(CKSI0140.G_Siwake_Area[22], 1, 10), 10, 2) + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(CKSI0140.G_Siwake_Area[23], 1, 10), 10, 2) + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(CKSI0140.G_Siwake_Area[24], 1, 10), 10, 2) + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(CKSI0140.G_Siwake_Area[25], 1, 10), 10, 2) + "'";

            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0140.G_Siwake_Area[26];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0140.G_Siwake_Area[27];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0140.G_Siwake_Area[28];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0140.G_Siwake_Area[29];
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[30] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(CKSI0140.G_Siwake_Area[31], 1, 10), 10, 2) + "'";

            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[32] + "'";

            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(CKSI0140.G_Siwake_Area[33], 1, 10), 10, 2) + "'";

            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[34] + "'";

            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(CKSI0140.G_Siwake_Area[35], 1, 10), 10, 2) + "'";

            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[36] + "'";

            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(CKSI0140.G_Siwake_Area[37], 1, 10), 10, 2) + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(CKSI0140.G_Siwake_Area[38], 1, 10), 10, 1) + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(CKSI0140.G_Siwake_Area[39], 1, 10), 10, 1) + "'";

            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[40] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[41] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[42] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[43] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[44] + "'";

            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(CKSI0140.G_Siwake_Area[45], 1, 30), 30, 2) + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(CKSI0140.G_Siwake_Area[46], 1, 1), 1, 2) + "'";

            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0140.G_Siwake_Area[47];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0140.G_Siwake_Area[48];
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[49] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[50] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[51] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[52] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[53] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[54] + "'";

            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(CKSI0140.G_Siwake_Area[55], 1, 30), 30, 2) + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(CKSI0140.G_Siwake_Area[56], 1, 60), 60, 2) + "'";

            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[57] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[58] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[59] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[60] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[61] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[62] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[63] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[64] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[65] + "'";

            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(Strings.Mid(CKSI0140.G_Siwake_Area[66], 1, 17), 17, 1) + "'";

            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[67] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[68] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[69] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[70] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[71] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[72] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[73] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[74] + "'";

            IPPAN.G_SQL = IPPAN.G_SQL + ",to_date('" + L_datetime + "', 'YYYY/MM/DD HH24:MI:SS')";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[76] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[77] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[78] + "'";
// 2015.02.03 yoshitake OBIC7仕訳連携対応(step3) start.
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[79] + "'";
            //工程CD
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[80] + "'";
            //消費税補助区分
            IPPAN.G_SQL += ",'" + CKSI0140.G_Siwake_Area[81] + "')";
            //IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0140.G_Siwake_Area[79] + "')";
// 2015.02.03 yoshitake OBIC7仕訳連携対応(step3) end.
            Debug.Print(IPPAN.G_SQL);

            try
            {
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

            functionReturnValue = 0;
            return functionReturnValue;
        }
    }
}
