using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Windows.Forms;

namespace Project1
{
    static class kensaku
    {
        public static string G_HINMOKU_CD;

        //dbのワークエリア
        //トランのワークエリア
        public static string[] G_TRAN_AREA = new string[36];
        //備考トランのワークエリア
        public static string[] G_BIKOU_AREA = new string[21];
        //所属マスタのワークエリア
        public static string[] G_SYOZOKU_AREA = new string[2];
        //社員マスタのワークエリア
        public static string[] G_SYAIN_AREA = new string[4];

        //業者マスタの検索 データがないときは１を返す。
        public static short Zaiko_Kensaku(string L_HINCD, ListBox lbList)
        {
            short functionReturnValue = 1;

            string L_LIST = null;
            int LST_NYUKO = 0;
            int LST_SYUKKO = 0;
            int LST_CHOKU = 0;
            int LST_HENPIN = 0;
            string LST_HINNM = "";

            int SUM_SURYO = 0;
            int SUM_TANASU = 0;

            string SV_HINMOKUCD = "";
            short SV_KUBUN = 0;
            DataTable tbl = null;

            SV_HINMOKUCD = Strings.Space(4);
            SV_KUBUN = 0;

            LST_NYUKO = 0;
            LST_SYUKKO = 0;
            LST_CHOKU = 0;
            LST_HENPIN = 0;

            IPPAN.G_SQL = "SELECT A.HINMOKUCD, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "A.HINMOKUNM, A.TANASURYO, B.KUBUN, B.SURYO FROM ";
            IPPAN.G_SQL = IPPAN.G_SQL + "TANAOROSI_VIEW A, SAGYOSI_VIEW B WHERE ";

            if (IPPAN.C_Allspace(L_HINCD) == 1)
            {
                IPPAN.G_SQL = IPPAN.G_SQL + "A.HINMOKUCD = '" + L_HINCD + "' AND ";
            }

            IPPAN.G_SQL = IPPAN.G_SQL + "A.HINMOKUCD = B.HINMOKUCD ";
            IPPAN.G_SQL = IPPAN.G_SQL + "ORDER BY A.HINMOKUCD, B.KUBUN, A.HINMOKUNM, B.SURYO ";

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

            //データがない
            if (tbl.Rows.Count == 0)
            {
                //viewが見つかりません。
                IPPAN.Error_Msg("E999", 0, " ");
                return functionReturnValue;
            }

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    //項目セット
                    switch (j)
                    {
                        case 3:
                        case 5:
                            if (string.IsNullOrEmpty(Strings.Trim(tbl.Rows[i][j].ToString())))
                            {
                                G_TRAN_AREA[j] = "0";
                            }
                            else
                            {
                                G_TRAN_AREA[j] = tbl.Rows[i][j].ToString();
                            }
                            break;

                        case 4:
                            if (string.IsNullOrEmpty(Strings.Trim(tbl.Rows[i][j].ToString())))
                            {
                                G_TRAN_AREA[j] = "0";
                            }
                            else
                            {
                                G_TRAN_AREA[j] = tbl.Rows[i][j].ToString();
                            }
                            break;

                        default:
                            G_TRAN_AREA[j] = tbl.Rows[i][j].ToString();
                            break;
                    }
                }

                if (G_TRAN_AREA[2] == "0" && G_TRAN_AREA[4] == "0")
                {
                    if (IPPAN.C_Allspace(L_HINCD) == 1)
                    {
                        C_COMMON.Msg("該当するデータがありません。");
                        return functionReturnValue;
                    }
                }
                else
                {
                    //一件目のキーセット
                    if (SV_HINMOKUCD == Strings.Space(4))
                    {
                        SV_HINMOKUCD = Strings.Mid(G_TRAN_AREA[0], 1, 4);
                        SV_KUBUN = Convert.ToInt16(Strings.Mid(G_TRAN_AREA[3], 1, 1));
                        LST_HINNM = Strings.Mid(G_TRAN_AREA[1], 1, 20);
                    }

                    //品目コードが同一であるとき
                    if (SV_HINMOKUCD == Strings.Mid(G_TRAN_AREA[0], 1, 4))
                    {
                        switch (Strings.Mid(G_TRAN_AREA[3], 1, 1))
                        {
                            case "1":
                                LST_NYUKO = Convert.ToInt32(G_TRAN_AREA[4]);
                                break;
                            case "2":
                                LST_SYUKKO = Convert.ToInt32(G_TRAN_AREA[4]);
                                break;
                            case "3":
                                LST_CHOKU = Convert.ToInt32(G_TRAN_AREA[4]);
                                break;
                            case "4":
                                LST_HENPIN = Convert.ToInt32(G_TRAN_AREA[4]);
                                break;
                        }

                        //棚卸トランデータが複数あるときの対応処理
                        if (SV_KUBUN == Convert.ToDouble(G_TRAN_AREA[3]))
                        {
                            SUM_SURYO = SUM_SURYO + Convert.ToInt32(G_TRAN_AREA[2]);
                            SUM_TANASU = SUM_TANASU + Convert.ToInt32(G_TRAN_AREA[2]);
                        }
                    }
                    else
                    {
                        //品目コードが異なるとき
                        SUM_SURYO = SUM_SURYO + LST_NYUKO - LST_SYUKKO - LST_HENPIN;
                        L_LIST = Strings.Space(1) + SV_HINMOKUCD + Strings.Space(2) + LST_HINNM;
                        L_LIST = L_LIST + IPPAN.Money_Hensyu(Convert.ToString(SUM_TANASU), "###,###,##0");
                        L_LIST = L_LIST + IPPAN.Money_Hensyu(Convert.ToString(LST_NYUKO), "###,###,##0");
                        L_LIST = L_LIST + IPPAN.Money_Hensyu(Convert.ToString(LST_SYUKKO), "###,###,##0");
                        L_LIST = L_LIST + IPPAN.Money_Hensyu(Convert.ToString(LST_HENPIN), "###,###,##0");
                        L_LIST = L_LIST + IPPAN.Money_Hensyu(Convert.ToString(SUM_SURYO), "###,###,##0");
                        L_LIST = L_LIST + IPPAN.Money_Hensyu(Convert.ToString(LST_CHOKU), "###,###,##0");

                        //リストに追加
                        lbList.Items.Add(L_LIST);

                        LST_NYUKO = 0;
                        LST_SYUKKO = 0;
                        LST_CHOKU = 0;
                        LST_HENPIN = 0;
                        switch (Strings.Mid(G_TRAN_AREA[3], 1, 1))
                        {
                            case "1":
                                LST_NYUKO = Convert.ToInt32(G_TRAN_AREA[4]);
                                break;
                            case "2":
                                LST_SYUKKO = Convert.ToInt32(G_TRAN_AREA[4]);
                                break;
                            case "3":
                                LST_CHOKU = Convert.ToInt32(G_TRAN_AREA[4]);
                                break;
                            case "4":
                                LST_HENPIN = Convert.ToInt32(G_TRAN_AREA[4]);
                                break;
                        }
                        SUM_SURYO = Convert.ToInt32(G_TRAN_AREA[2]);
                        SUM_TANASU = Convert.ToInt32(G_TRAN_AREA[2]);
                        SV_HINMOKUCD = Strings.Mid(G_TRAN_AREA[0], 1, 4);
                        SV_KUBUN = Convert.ToInt16(Strings.Mid(G_TRAN_AREA[3], 1, 1));
                        LST_HINNM = Strings.Mid(G_TRAN_AREA[1], 1, 20);
                    }
                }
            }

            //最終レコードを表示するための処理
            SUM_SURYO = SUM_SURYO + LST_NYUKO - LST_SYUKKO - LST_HENPIN;
            L_LIST = Strings.Space(1) + SV_HINMOKUCD + Strings.Space(2) + LST_HINNM;
            L_LIST = L_LIST + IPPAN.Money_Hensyu(Convert.ToString(SUM_TANASU), "###,###,##0");
            L_LIST = L_LIST + IPPAN.Money_Hensyu(Convert.ToString(LST_NYUKO), "###,###,##0");
            L_LIST = L_LIST + IPPAN.Money_Hensyu(Convert.ToString(LST_SYUKKO), "###,###,##0");
            L_LIST = L_LIST + IPPAN.Money_Hensyu(Convert.ToString(LST_HENPIN), "###,###,##0");
            L_LIST = L_LIST + IPPAN.Money_Hensyu(Convert.ToString(SUM_SURYO), "###,###,##0");
            L_LIST = L_LIST + IPPAN.Money_Hensyu(Convert.ToString(LST_CHOKU), "###,###,##0");

            //リストに追加
            lbList.Items.Add(L_LIST);

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static short Hinmoku_Kensaku(string L_CD)
        {
            short functionReturnValue = 1;

            string[] L_MST_AREA = new string[3];
            DataTable tbl = null;

            IPPAN.G_SQL = "Select HINMOKUCD from SIZAI_HINMOKU_MST WHERE ";

            if (IPPAN.C_Allspace(L_CD) == 1)
            {
                IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKUCD = '" + L_CD + "' ";
            }

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
                // 項目をﾜｰｸにｾｯﾄ
                L_MST_AREA[j] = tbl.Rows[0][j].ToString();
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }
    }
}
