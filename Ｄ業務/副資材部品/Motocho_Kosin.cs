using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Diagnostics;
using System.Data.Odbc;

namespace Project1
{
    static class Motocho_Kosin
    {
        static int wNyukoSuryo;
        static int wSuibunryo;
        static int wSyukoSuryo1;
        static int wSyukoSuryo2;
        static int wSyukoSuryo3;
        static int wSyukoSuryo4;
        static int wSyukoSuryo5;
        static int wSyukoSuryo6;
        static int wSyukoSuryo7;
        static int wSyukoSuryo8;
        static int wSyukoSuryo9;
        static int wSyukoSuryo0;
        static int wZSouko;
        static int wZEf;
        static int wZLf;
        static int wZCc;
        static int wZSonota;
        static int wZMeter;
        static int wZYobi1;
        static int wZYobi2;
        static int wZSouko2;
        static int wZEf2;
        static int wZLf2;
        static int wZCc2;
        static int wZSonota2;
        static int wZMeter2;
        static int wZYobi12;
        static int wZYobi22;

        public static short Motocho_Sakusei(C_ODBC db)
        {
            short functionReturnValue = 1;
            string wYm = null;
            DataTable tbl = null;

            //作業誌、棚卸より品目を抽出する（重複品目は排除）
            if (string.Compare(Strings.Mid(CKSI0050_01.G_NENGETU, 5, 2), "02") < 0)
            {
                wYm = String.Format("{0:0000}", Conversion.Val(Strings.Mid(CKSI0050_01.G_NENGETU, 1, 4)) - 1) + "12";
            }
            else
            {
                wYm = Strings.Mid(CKSI0050_01.G_NENGETU, 1, 4) + String.Format("{0:00}", Conversion.Val(Strings.Mid(CKSI0050_01.G_NENGETU, 5, 2)) - 1);
            }
            IPPAN.G_SQL = "SELECT HINMOKUCD FROM SIZAI_HINMOKU_MST ";
            Debug.Print(IPPAN.G_SQL);

            try
            {
                //トランザクションは呼出し元で制御
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
                    CKSI0050_01.G_TRAN_AREA[j] = tbl.Rows[i][j].ToString();
                }

                //副資材作業誌トランを検索
                wNyukoSuryo = 0;
                wSuibunryo = 0;
                wSyukoSuryo1 = 0;
                wSyukoSuryo2 = 0;
                wSyukoSuryo3 = 0;
                wSyukoSuryo4 = 0;
                wSyukoSuryo5 = 0;
                wSyukoSuryo6 = 0;
                wSyukoSuryo7 = 0;
                wSyukoSuryo8 = 0;
                wSyukoSuryo9 = 0;
                wSyukoSuryo0 = 0;
                IPPAN.G_RET = Sagyosi_Kensaku(db, CKSI0050_01.G_TRAN_AREA[0]);

                //副資材棚卸トランを検索（当月）
                wZSouko = 0;
                wZEf = 0;
                wZLf = 0;
                wZCc = 0;
                wZSonota = 0;
                wZMeter = 0;
                wZYobi1 = 0;
                wZYobi2 = 0;
                IPPAN.G_RET = Tanaorosi_Kensaku(db, CKSI0050_01.G_TRAN_AREA[0]);

                //副資材棚卸トランを検索（前月）
                wZSouko2 = 0;
                wZEf2 = 0;
                wZLf2 = 0;
                wZCc2 = 0;
                wZSonota2 = 0;
                wZMeter2 = 0;
                wZYobi12 = 0;
                wZYobi22 = 0;
                IPPAN.G_RET = Tanaorosi_Kensaku2(db, CKSI0050_01.G_TRAN_AREA[0]);

                //副資材元帳トラン項目セット
                if (Motocho_Set(Convert.ToString(CKSI0050_01.G_TRAN_AREA[0])) == 1)
                {
                    return functionReturnValue;
                }

                //副資材元帳トランを登録
                if (Motocho_Insert(db) == 1)
                {
                    return functionReturnValue;
                }
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private static short Sagyosi_Kensaku(C_ODBC db, string L_Hinmokucd)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //副資材作業誌トランの検索
            IPPAN.G_SQL = "SELECT KUBUN,MUKESAKI,SURYO,SUIBUN FROM SIZAI_SAGYOSI_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE SAGYOBI LIKE '" + CKSI0050_01.G_NENGETU + "__' ";
            //曖昧検索
            IPPAN.G_SQL = IPPAN.G_SQL + "and HINMOKUCD = '" + L_Hinmokucd + "'";

            try
            {
                //トランザクションは呼出し元で制御
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
                    CKSI0050_01.G_Sagyosi_Area[j] = tbl.Rows[i][j].ToString();
                }

                switch (Strings.Mid(CKSI0050_01.G_Sagyosi_Area[0], 1, 1))
                {
                    case "1":
                        wNyukoSuryo = wNyukoSuryo + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]);
                        //入庫量
                        wSuibunryo = wSuibunryo + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[3]);
                        //水分量
                        break;
                    case "2":
                        switch (Strings.Mid(CKSI0050_01.G_Sagyosi_Area[1], 1, 1))
                        {
                            //出庫量
                            case "1":
                                wSyukoSuryo1 = wSyukoSuryo1 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]);
                                break;
                            case "2":
                                wSyukoSuryo2 = wSyukoSuryo2 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]);
                                break;
                            case "3":
                                wSyukoSuryo3 = wSyukoSuryo3 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]);
                                break;
                            case "4":
                                wSyukoSuryo4 = wSyukoSuryo4 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]);
                                break;
                            case "5":
                                wSyukoSuryo5 = wSyukoSuryo5 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]);
                                break;
                            case "6":
                                wSyukoSuryo6 = wSyukoSuryo6 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]);
                                break;
                            case "7":
                                wSyukoSuryo7 = wSyukoSuryo7 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]);
                                break;
                            case "8":
                                wSyukoSuryo8 = wSyukoSuryo8 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]);
                                break;
                            case "9":
                                wSyukoSuryo9 = wSyukoSuryo9 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]);
                                break;
                            case "0":
                                wSyukoSuryo0 = wSyukoSuryo0 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]);
                                break;
                        }
                        break;
                    case "3":
                        wNyukoSuryo = wNyukoSuryo + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]);
                        //入庫量
                        wSuibunryo = wSuibunryo + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[3]);
                        //水分量
                        switch (Strings.Mid(CKSI0050_01.G_Sagyosi_Area[1], 1, 1))
                        {
                            //出庫量
                            case "1":
                                wSyukoSuryo1 = wSyukoSuryo1 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]) - Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[3]);
                                break;
                            case "2":
                                wSyukoSuryo2 = wSyukoSuryo2 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]) - Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[3]);
                                break;
                            case "3":
                                wSyukoSuryo3 = wSyukoSuryo3 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]) - Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[3]);
                                break;
                            case "4":
                                wSyukoSuryo4 = wSyukoSuryo4 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]) - Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[3]);
                                break;
                            case "5":
                                wSyukoSuryo5 = wSyukoSuryo5 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]) - Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[3]);
                                break;
                            case "6":
                                wSyukoSuryo6 = wSyukoSuryo6 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]) - Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[3]);
                                break;
                            case "7":
                                wSyukoSuryo7 = wSyukoSuryo7 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]) - Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[3]);
                                break;
                            case "8":
                                wSyukoSuryo8 = wSyukoSuryo8 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]) - Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[3]);
                                break;
                            case "9":
                                wSyukoSuryo9 = wSyukoSuryo9 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]) - Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[3]);
                                break;
                            case "0":
                                wSyukoSuryo0 = wSyukoSuryo0 + Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]) - Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[3]);
                                break;
                        }
                        break;
                    case "4":
                        wNyukoSuryo = wNyukoSuryo - Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[2]);
                        //入庫量
                        wSuibunryo = wSuibunryo - Convert.ToInt32(CKSI0050_01.G_Sagyosi_Area[3]);
                        //水分量
                        break;
                }
            }

            functionReturnValue = 1;
            return functionReturnValue;
        }

        private static short Tanaorosi_Kensaku(C_ODBC db, string L_Hinmokucd)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //副資材棚卸トランの検索（当月）
            IPPAN.G_SQL = "SELECT ZSOUKO,ZEF,ZLF,ZCC,ZSONOTA,ZMETER,ZYOBI1,ZYOBI2 FROM SIZAI_TANAOROSI_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE TANAYM = '" + CKSI0050_01.G_NENGETU + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + "and HINMOKUCD = '" + L_Hinmokucd + "'";

            try
            {
                //トランザクションは呼出し元で制御
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
                    CKSI0050_01.G_Tanaorosi_Area[j] = tbl.Rows[i][j].ToString();
                }

                wZSouko = wZSouko + Convert.ToInt32(CKSI0050_01.G_Tanaorosi_Area[0]);
                wZEf = wZEf + Convert.ToInt32(CKSI0050_01.G_Tanaorosi_Area[1]);
                wZLf = wZLf + Convert.ToInt32(CKSI0050_01.G_Tanaorosi_Area[2]);
                wZCc = wZCc + Convert.ToInt32(CKSI0050_01.G_Tanaorosi_Area[3]);
                wZSonota = wZSonota + Convert.ToInt32(CKSI0050_01.G_Tanaorosi_Area[4]);
                wZMeter = wZMeter + Convert.ToInt32(CKSI0050_01.G_Tanaorosi_Area[5]);
                wZYobi1 = wZYobi1 + Convert.ToInt32(CKSI0050_01.G_Tanaorosi_Area[6]);
                wZYobi2 = wZYobi2 + Convert.ToInt32(CKSI0050_01.G_Tanaorosi_Area[7]);
            }

            functionReturnValue = 1;
            return functionReturnValue;
        }

        private static short Tanaorosi_Kensaku2(C_ODBC db, string L_Hinmokucd)
        {
            short functionReturnValue = 1;
            string wYm = null;

            DataTable tbl = null;

            //副資材棚卸トランの検索（前月）
            if (string.Compare(Strings.Mid(CKSI0050_01.G_NENGETU, 5, 2), "02") < 0)
            {
                wYm = String.Format("{0:0000}", Convert.ToInt32(Strings.Mid(CKSI0050_01.G_NENGETU, 1, 4)) - 1) + "12";
            }
            else
            {
                wYm = Strings.Mid(CKSI0050_01.G_NENGETU, 1, 4) + String.Format("{0:00}", Convert.ToInt32(Strings.Mid(CKSI0050_01.G_NENGETU, 5, 2)) - 1);
            }
            IPPAN.G_SQL = "SELECT ZSOUKO,ZEF,ZLF,ZCC,ZSONOTA,ZMETER,ZYOBI1,ZYOBI2 FROM SIZAI_TANAOROSI_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE TANAYM = '" + wYm + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + "and HINMOKUCD = '" + L_Hinmokucd + "'";

            try
            {
                //トランザクションは呼出し元で制御
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
                    CKSI0050_01.G_Tanaorosi_Area[j] = tbl.Rows[i][j].ToString();
                }

                wZSouko2 = wZSouko2 + Convert.ToInt32(CKSI0050_01.G_Tanaorosi_Area[0]);
                wZEf2 = wZEf2 + Convert.ToInt32(CKSI0050_01.G_Tanaorosi_Area[1]);
                wZLf2 = wZLf2 + Convert.ToInt32(CKSI0050_01.G_Tanaorosi_Area[2]);
                wZCc2 = wZCc2 + Convert.ToInt32(CKSI0050_01.G_Tanaorosi_Area[3]);
                wZSonota2 = wZSonota2 + Convert.ToInt32(CKSI0050_01.G_Tanaorosi_Area[4]);
                wZMeter2 = wZMeter2 + Convert.ToInt32(CKSI0050_01.G_Tanaorosi_Area[5]);
                wZYobi12 = wZYobi12 + Convert.ToInt32(CKSI0050_01.G_Tanaorosi_Area[6]);
                wZYobi22 = wZYobi22 + Convert.ToInt32(CKSI0050_01.G_Tanaorosi_Area[7]);
            }

            functionReturnValue = 1;
            return functionReturnValue;
        }

        private static short Motocho_Set(string L_Hinmokucd)
        {
            short functionReturnValue = 1;
            int wSiyoryo1 = 0;
            int wSiyoryo2 = 0;
            int wSiyoryo3 = 0;
            int wSiyoryo4 = 0;
            int wSiyoryo5 = 0;
            int wSiyoryo6 = 0;
            int wSiyoryo7 = 0;
            int wSiyoryo8 = 0;
            int wSiyoryo9 = 0;
            int wSiyoryo0 = 0;

            CKSI0050_01.G_Motocho_Area[0] = IPPAN.Space_Set(CKSI0050_01.G_NENGETU, 6, 1);
            CKSI0050_01.G_Motocho_Area[1] = IPPAN.Space_Set(L_Hinmokucd, 4, 1);
            CKSI0050_01.G_Motocho_Area[2] = Convert.ToString(wZSouko2 + wZEf2 + wZLf2 + wZCc2 + wZSonota2 + wZMeter2 + wZYobi12 + wZYobi22);
            CKSI0050_01.G_Motocho_Area[3] = Convert.ToString(wNyukoSuryo);
            CKSI0050_01.G_Motocho_Area[4] = Convert.ToString(wSuibunryo);
            CKSI0050_01.G_Motocho_Area[5] = Convert.ToString(wZSouko + wZEf + wZLf + wZCc + wZSonota + wZMeter + wZYobi1 + wZYobi2);
            CKSI0050_01.G_Motocho_Area[6] = Convert.ToString(wZSouko);
            CKSI0050_01.G_Motocho_Area[7] = Convert.ToString(wZEf);
            CKSI0050_01.G_Motocho_Area[8] = Convert.ToString(wZLf);
            CKSI0050_01.G_Motocho_Area[9] = Convert.ToString(wZCc);
            CKSI0050_01.G_Motocho_Area[10] = Convert.ToString(wZSonota);
            CKSI0050_01.G_Motocho_Area[11] = Convert.ToString(wZMeter);
            CKSI0050_01.G_Motocho_Area[12] = Convert.ToString(wZYobi1);
            CKSI0050_01.G_Motocho_Area[13] = Convert.ToString(wZYobi2);

            wSiyoryo1 = wZEf2 + wSyukoSuryo1 - wZEf;
            wSiyoryo2 = wZLf2 + wSyukoSuryo2 - wZLf;
            wSiyoryo3 = wZCc2 + wSyukoSuryo3 - wZCc;

            if (L_Hinmokucd == "0110" || L_Hinmokucd == "0130" || L_Hinmokucd == "0131")
            {
                wSiyoryo4 = wSyukoSuryo4;
            }
            else
            {
                wSiyoryo4 = wZSonota2 + wSyukoSuryo4 - wZSonota;
            }
            wSiyoryo5 = wSyukoSuryo5;
            wSiyoryo6 = wSyukoSuryo6;
            wSiyoryo7 = wSyukoSuryo7;
            wSiyoryo8 = wSyukoSuryo8;
            wSiyoryo9 = wSyukoSuryo9;
            wSiyoryo0 = wSyukoSuryo0;

            CKSI0050_01.G_Motocho_Area[14] = Convert.ToString(wSiyoryo1 + wSiyoryo2 + wSiyoryo3 + wSiyoryo4 + wSiyoryo5 + wSiyoryo6 + wSiyoryo7 + wSiyoryo8 + wSiyoryo9 + wSiyoryo0);
            CKSI0050_01.G_Motocho_Area[15] = Convert.ToString(wSiyoryo1);
            CKSI0050_01.G_Motocho_Area[16] = Convert.ToString(wSiyoryo2);
            CKSI0050_01.G_Motocho_Area[17] = Convert.ToString(wSiyoryo3);
            CKSI0050_01.G_Motocho_Area[18] = Convert.ToString(wSiyoryo4);
            CKSI0050_01.G_Motocho_Area[19] = Convert.ToString(wSiyoryo5);
            CKSI0050_01.G_Motocho_Area[20] = Convert.ToString(wSiyoryo6);
            CKSI0050_01.G_Motocho_Area[21] = Convert.ToString(wSiyoryo7);
            CKSI0050_01.G_Motocho_Area[22] = Convert.ToString(wSiyoryo8);
            CKSI0050_01.G_Motocho_Area[23] = Convert.ToString(wSiyoryo9);
            CKSI0050_01.G_Motocho_Area[24] = Convert.ToString(wSiyoryo0);
            if (CKSI0050_01.Hinmoku_Kensaku(L_Hinmokucd) == 1)
            {
                //副資材品目マスタが見つかりません
                IPPAN.Error_Msg("E701", 0, " ");
                return functionReturnValue;
            }
            CKSI0050_01.G_Motocho_Area[25] = CKSI0050_01.G_HIMOKU;
            CKSI0050_01.G_Motocho_Area[26] = CKSI0050_01.G_UTIWAKE;
            CKSI0050_01.G_Motocho_Area[27] = CKSI0050_01.G_TANABAN;

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private static short Motocho_Insert(C_ODBC db)
        {
            short functionReturnValue = 1;
            string L_datetime = null;

            L_datetime = String.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);

            IPPAN.G_SQL = "insert into SIZAI_MOTOCHO_TRN values (";
            IPPAN.G_SQL = IPPAN.G_SQL + "'" + CKSI0050_01.G_Motocho_Area[0] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0050_01.G_Motocho_Area[1] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[2];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[3];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[4];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[5];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[6];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[7];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[8];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[9];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[10];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[11];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[12];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[13];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[14];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[15];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[16];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[17];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[18];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[19];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[20];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[21];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[22];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[23];
            IPPAN.G_SQL = IPPAN.G_SQL + "," + CKSI0050_01.G_Motocho_Area[24];
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0050_01.G_Motocho_Area[25] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0050_01.G_Motocho_Area[26] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + CKSI0050_01.G_Motocho_Area[27] + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",to_date('" + L_datetime + "', 'YYYY/MM/DD HH24:MI:SS'))";
            
            Debug.Print(IPPAN.G_SQL);

            try
            {
                //トランザクションは呼出し元で制御
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

        public static short Motocho_Sakujo(C_ODBC db)
        {
            short functionReturnValue = 1;

            IPPAN.G_SQL = "delete from SIZAI_MOTOCHO_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "where MOTOYM = '" + CKSI0050_01.G_NENGETU + "' ";
            Debug.Print(IPPAN.G_SQL);

            try
            {
                //トランザクションは呼出し元で制御
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
