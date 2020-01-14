using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Data.Odbc;

namespace Project1
{
    static class IPPAN2
    {
        //******************************************************************************************************'
        //   修正履歴                                                                                           '
        //   修正年月日  Ｒｅｖ  　担当者　　 　修正内容                                                        '
        //    13.06.10             HIT 綱本    消費税対応                                                     　'
        //    13.06.19             HIT 綱本　　消費税対応                                                       '
        //******************************************************************************************************'

        private const int C_LCID_JPN = 0x0411;

        //消費税率取得用ワーク
        //税区分１（入力形式）
        //public static string G_SZEI_ZEIKBN1;
        //税区分２（ファイル）
        //public static string G_SZEI_ZEIKBN2;
        //税率
        //public static decimal G_SZEI_ZEIRITU;
        //開始日
        //public static string G_SZEI_STARTYMD;
        //開始日
        //public static string G_SZEI_ENDYMD;
        //消費税率マスタワーク
        //public static string[] G_SZEI_AREA = new string[11];

        //タイマーセット処理用ワーク
        public static string[] G_MST_AREA = new string[3];


        //13.06.10 tsunamoto 消費税対応 start
        //Get_Syohizeiメソッド戻り値用クラス
        public class ZeiInfo
        {
            public int      zeikeisan;  // 計算用の税率の値
            public string   zeihyoji;   // 表示用の税率の値
            public decimal  zeiritu;    // 税率の値
            public string   zeikbn1;    // 税区分1の値
            public string   zeikbn2;    // 税区分2の値
            public double   zeinuki;    // 税抜金額の値
            public double   zeikomi;    // 税込金額の値
            public double   syohizei;   // 消費税の値
            public int      error_flag; // エラーフラグ


            public ZeiInfo()
            {
                zeikeisan  = 1;
                zeihyoji   = "";
                zeiritu    = 0;
                zeikbn1    = "";
                zeikbn2    = "";
                zeinuki    = 0;
                zeikomi    = 0;
                syohizei   = 0;
                error_flag = 0;
            }
        }
        //13.06.10 tsunamoto 消費税対応 end
        
        // StarOffice用
        public static short TIMER_SET(string L_SyainCD)
        {
            short functionReturnValue = 1;

            DataTable tbl = null;

            //社員マスタの検索
            IPPAN.G_SQL = "";
            //2013.09.20 DSK yoshida START
            //IPPAN.G_SQL = "Select SYAINNM,SYAINSZCD from SYAIN_MST ";
            IPPAN.G_SQL = "Select SYAINNM,SYAINSZCD from FL_SYAIN_MST_VIEW ";
            //2013.09.20 DSK yoshida END
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE SYAINCD = '" + L_SyainCD + "'";

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    // SQL実行
                    db.Connect();
                    tbl = db.ExecSQL(IPPAN.G_SQL);

                    // 結果の列数を取得する
                    if (tbl.Columns.Count < 1)
                    {
                        return functionReturnValue;
                    }

                    // データがない
                    if (tbl.Rows.Count == 0)
                    {
                        //社員マスタが見つかりません
                        IPPAN.Error_Msg("E113", 0, " ");
                        return functionReturnValue;
                    }

                    for (int i = 0; i < tbl.Rows.Count; i++)
                    {
                        for (int j = 0; j < tbl.Columns.Count; j++)
                        {
                            // 項目セット
                            G_MST_AREA[j] = tbl.Rows[i][j].ToString();
                        }
                    }

                    SO.SO_USERNAME = Strings.Trim(Strings.Mid(G_MST_AREA[0], 1, 15));

                    //所属マスタの検索
                    IPPAN.G_SQL = "";
                    IPPAN.G_SQL = "Select SZNM from SYOZO_MST ";
                    IPPAN.G_SQL = IPPAN.G_SQL + "WHERE SZCD = '" + G_MST_AREA[1] + "'";

                    // SQL実行
                    tbl = db.ExecSQL(IPPAN.G_SQL);

                    // 結果の列数を取得する
                    if (tbl.Columns.Count < 1)
                    {
                        return functionReturnValue;
                    }

                    // データがない
                    if (tbl.Rows.Count == 0)
                    {
                        //所属マスタが見つかりません
                        IPPAN.Error_Msg("E115", 0, " ");
                        return functionReturnValue;
                    }

                    for (int i = 0; i < tbl.Rows.Count; i++)
                    {
                        for (int j = 0; j < tbl.Columns.Count; j++)
                        {
                            // 項目セット
                            G_MST_AREA[j] = tbl.Rows[i][j].ToString();
                        }
                    }

                    SO.SO_OFFICENAME = Strings.Trim(Strings.Mid(G_MST_AREA[0], 1, 15));
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


            functionReturnValue = 0;
            return functionReturnValue;
        }

        public static string NIBYTE_HENKAN(string L_MOJI, int L_NAGASA)
        {
            string L_MOJIHENKAN = string.Empty;

            L_MOJIHENKAN = Strings.StrConv(L_MOJI, VbStrConv.Wide, C_LCID_JPN);

            while (Strings.InStr(L_MOJIHENKAN, "\\", CompareMethod.Binary) != 0)
            {
                L_MOJIHENKAN = Strings.Mid(L_MOJIHENKAN, 1, Strings.InStr(L_MOJIHENKAN, "\\", CompareMethod.Binary) - 1) + "￥" + Strings.Mid(L_MOJIHENKAN, Strings.InStr(L_MOJIHENKAN, "\\", CompareMethod.Binary) + 1, Strings.Len(L_MOJIHENKAN));
            }

            return Strings.Mid(L_MOJIHENKAN, 1, L_NAGASA);
        }

        public static string Numeric_Hensyu2(string L_DATE)
        {
            string functionReturnValue = "";
            int L_LEN = 0;
            int L_I = 0;
            string L_STR = null;
            string L_STR2 = null;
            //先頭にマイナス符号がある時に１をたてる
            int L_Minus_FLG = 0;
            int L_Syou_FLG = 0;

            L_Minus_FLG = 0;
            L_Syou_FLG = 0;
            L_STR = "";
            L_STR2 = Strings.Trim(L_DATE);
            L_LEN = Strings.Len(Strings.Trim(L_DATE));
            if (L_LEN <= 0)
            {
                return functionReturnValue;
            }
            for (L_I = 1; L_I <= Strings.Len(L_STR2); L_I++)
            {
                switch (Strings.Mid(L_STR2, L_I, 1))
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        L_STR = L_STR + Strings.Mid(L_STR2, L_I, 1);
                        break;
                    case "-":
                        if (L_I == 1)
                        {
                            L_Minus_FLG = 1;
                        }
                        break;
                    case ".":
                        if (L_Syou_FLG == 0)
                        {
                            L_STR = L_STR + Strings.Mid(L_STR2, L_I, 1);
                            L_Syou_FLG = 1;
                        }
                        break;
                    default:
                        break;
                }
            }

            if (L_Minus_FLG == 1)
            {
                L_STR = Convert.ToString(Convert.ToInt32(L_STR) * -1);
            }

            functionReturnValue = L_STR;
            return functionReturnValue;

        }

        //数字編集（正の整数のみ）
        public static string Numeric_Hensyu3(string L_DATE)
        {
            string functionReturnValue = "";
            int L_LEN = 0;
            int L_I = 0;
            string L_STR = string.Empty;
            string L_STR2 = string.Empty;

            L_STR = "";
            L_STR2 = Strings.Trim(L_DATE);
            L_LEN = Strings.Len(Strings.Trim(L_DATE));
            if (L_LEN <= 0)
            {
                return functionReturnValue;
            }
            for (L_I = 1; L_I <= Strings.Len(L_STR2); L_I++)
            {
                switch (Strings.Mid(L_STR2, L_I, 1))
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        L_STR = L_STR + Strings.Mid(L_STR2, L_I, 1);
                        break;
                    default:
                        break;
                }
            }

            functionReturnValue = L_STR;
            return functionReturnValue;
        }


        //数字編集（正の整数、小数）
        public static string Numeric_Hensyu4(string L_DATE)
        {
            string functionReturnValue = "";
            int L_LEN = 0;
            int L_I = 0;
            string L_STR = string.Empty;
            string L_STR2 = string.Empty;
            //小数点があった時に１をたてる
            int L_TEN_FLG = 0;

            L_TEN_FLG = 0;
            L_STR = "";
            L_STR2 = Strings.Trim(L_DATE);
            L_LEN = Strings.Len(Strings.Trim(L_DATE));
            if (L_LEN <= 0)
            {
                return functionReturnValue;
            }
            for (L_I = 1; L_I <= Strings.Len(L_STR2); L_I++)
            {
                switch (Strings.Mid(L_STR2, L_I, 1))
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        L_STR = L_STR + Strings.Mid(L_STR2, L_I, 1);
                        break;
                    case ".":
                        if (L_TEN_FLG == 0)
                        {
                            L_STR = L_STR + Strings.Mid(L_STR2, L_I, 1);
                            L_TEN_FLG = 1;
                        }
                        break;
                    default:
                        break;
                }
            }

            functionReturnValue = L_STR;
            return functionReturnValue;
        }

        //消費税率取得（指定年月日）
       //public static decimal Get_Syohizei(string L_DATE)      13.06.10 DEL

        //13.06.10 tsunamoto 消費税対応 start
        /// <summary>
        /// 指定年月日を基準にDBから税率などの値を取得する
        /// </summary>
        /// <param name="L_DATE">指定年月日</param>
        /// <returns>ZeiInfoクラス</returns>
        public static ZeiInfo Get_Syohizei(string L_DATE)
        {            
            //戻り値設定
            ZeiInfo zeiinfo = new ZeiInfo();
        //13.06.10 tsunamoto 消費税対応 end

            //decimal functionReturnValue = default(decimal);   13.06.10 DEL

            DataTable tbl = null;

            // 初期値設定
            //functionReturnValue = 5;                          13.06.10 DEL

            //規定日付セット
            if (string.IsNullOrEmpty(L_DATE))
            {
                L_DATE = String.Format("{0:yyyyMMdd}", DateTime.Now);
            }
            
            //13.06.19 DEL start
            //IPPAN.G_SQL = "";
            //IPPAN.G_SQL = "SELECT SYOHIZEI_MST.ZEIKBN1,";
            //IPPAN.G_SQL = IPPAN.G_SQL + "SYOHIZEI_MST.ZEIKBN2,SYOHIZEI_MST.ZEIRITU,";
            //IPPAN.G_SQL = IPPAN.G_SQL + "SYOHIZEI_MST.STARTYMD,SYOHIZEI_MST.ENDYMD,";
            //IPPAN.G_SQL = IPPAN.G_SQL + "SYOHIZEI_MST.UPDYMD FROM SYOHIZEI_MST WHERE ";
            //IPPAN.G_SQL = IPPAN.G_SQL + "(SYOHIZEI_MST.STARTYMD<='" + L_DATE + "') AND ";
            //IPPAN.G_SQL = IPPAN.G_SQL + "(SYOHIZEI_MST.ENDYMD>='" + L_DATE + "')";
            //13.06.19 DEL end

            //13.06.19 tsunamoto 消費税 start
            System.Text.StringBuilder sqlTarget = new System.Text.StringBuilder();
            sqlTarget.Append("SELECT ZEIKBN1,ZEIKBN2,ZEIRITU");
            sqlTarget.Append(" FROM SYOHIZEI_MST WHERE ");
            sqlTarget.Append("(STARTYMD<='" + L_DATE + "') AND (ENDYMD>='" + L_DATE + "')");
            //13.06.19 tsunamoto 消費税 end

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    // SQL実行
                    db.Connect();
                   // tbl = db.ExecSQL(IPPAN.G_SQL);            13.06.19 DEL
                    tbl = db.ExecSQL(sqlTarget.ToString()); //13.06.19 tsunamoto 消費税対応
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

            // 結果の列数を取得する
            if (tbl.Columns.Count < 1)
            {
                //return functionReturnValue;                    13.06.10 DEL
                return zeiinfo; //13.06.10 tsunamoto 消費税対応
            }

            // データがない
            if (tbl.Rows.Count == 0)
            {
                return zeiinfo; //13.06.10 tsunamoto 消費税対応
                //return functionReturnValue;                    13.06.10 DEL
            }

            //for (int j = 0; j < tbl.Columns.Count; j++)        
            //{
            //    // 項目セット
            //    G_SZEI_AREA[j] = tbl.Rows[0][j].ToString();
            //}

            //13.06.10 DEL start
            ////消費税率取得用ワーク編集
            //G_SZEI_ZEIKBN1 = G_SZEI_AREA[0];
            ////税区分１（入力形式）
            //G_SZEI_ZEIKBN2 = G_SZEI_AREA[1];
            ////税区分２（ファイル）
            //G_SZEI_ZEIRITU = Convert.ToDecimal(G_SZEI_AREA[2]);
            ////税率
            //G_SZEI_STARTYMD = G_SZEI_AREA[3];
            ////開始日
            //G_SZEI_ENDYMD = G_SZEI_AREA[4];
            ////開始日

            //税率
            //functionReturnValue = G_SZEI_ZEIRITU;
            //13.06.19 DEL end 

            //13.06.19 tsunamoto 消費税対応 start
            zeiinfo.zeikbn1 = tbl.Rows[0]["ZEIKBN1"].ToString();
            zeiinfo.zeikbn2 = tbl.Rows[0]["ZEIKBN2"].ToString();
            zeiinfo.zeiritu = Convert.ToDecimal(tbl.Rows[0]["ZEIRITU"]);
            zeiinfo.zeikeisan = Convert.ToInt16(tbl.Rows[0]["ZEIRITU"]) + 100;
            zeiinfo.zeihyoji = Strings.StrConv(tbl.Rows[0]["ZEIRITU"].ToString(), VbStrConv.Wide, 0) + "％";

            return zeiinfo; 
            //13.06.19 tsunamoto 消費税対応 end

            //return functionReturnValue;                       13.06.10 DEL
            
        }


        //13.06.10 tsunamoto 消費税対応 start
        /// <summary>
        /// 税区分を基準にDBから税率などの値を取得する
        /// </summary>
        /// <param name="kbn">1の場合、引数ZEIKBNは税区分1,2の場合、引数ZEIKBNは税区分2</param>
        /// <param name="ZEIKBN">税区分の値</param>
        /// <returns>ZeiInfoクラス</returns>
        public static ZeiInfo Get_Syohizei(int kbn , string ZEIKBN)
        {
            //テーブルから値をとるための変換
            if (ZEIKBN == "")
            {
                ZEIKBN = " ";
            }

            //戻り値の設定
            ZeiInfo zeiinfo = new ZeiInfo();
            zeiinfo.zeikbn1 = ZEIKBN;
            zeiinfo.zeikbn2 = ZEIKBN;
            
            //無効な値の場合
            if(kbn != 1 && kbn != 2)
            {
                zeiinfo.error_flag = 1;
                return zeiinfo;
            }
            
            //SQL文の設定
            System.Text.StringBuilder sqlTarget = new System.Text.StringBuilder();
            sqlTarget.Append("SELECT ZEIKBN1,ZEIKBN2,ZEIRITU FROM SYOHIZEI_MST WHERE");
            if (kbn == 1)
            {
                sqlTarget.Append(" ZEIKBN1='" + ZEIKBN + "'");
            }
            else
            {
                sqlTarget.Append(" ZEIKBN2='" + ZEIKBN + "'");
            }

            DataTable tbl = null;


            //DB接続
            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    // SQL実行
                    db.Connect();
                    tbl = db.ExecSQL(sqlTarget.ToString());
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

            // 結果の列数を取得する
            if (tbl.Columns.Count < 1)
            {
                return zeiinfo;
            }

            // データがない
            if (tbl.Rows.Count == 0)
            {
                if (ZEIKBN != " ")
                {
                    zeiinfo = Get_Syohizei(1, " ");

                }
                return zeiinfo;
            }


            // 戻り値に値をセット
            zeiinfo.zeikbn1 = tbl.Rows[0]["ZEIKBN1"].ToString();
            zeiinfo.zeikbn2 = tbl.Rows[0]["ZEIKBN2"].ToString();
            zeiinfo.zeiritu = Convert.ToDecimal(tbl.Rows[0]["ZEIRITU"]);
            zeiinfo.zeikeisan = Convert.ToInt16(tbl.Rows[0]["ZEIRITU"]) + 100;
            zeiinfo.zeihyoji  = Strings.StrConv(tbl.Rows[0]["ZEIRITU"].ToString(), VbStrConv.Wide, 0) + "％";

            return zeiinfo;
        }
        //13.06.10 tsunamoto 消費税対応 end

        //13.06.10 tsunamoto 消費税対応 start
        /// <summary>
        /// 指定日時と金額から、税抜金額、税込金額、消費税を求め、テーブルから取得した税区分や税率なども返す。
        /// </summary>
        /// <param name="L_DATE">指定年月日</param>
        /// <param name="zei">引数が税込、四捨五入まるめなら「1」,切上げまるめなら「2」、引数が税抜、四捨五入まるめなら「3」,切上げまるめなら「4」</param>
        /// <param name="KINGAKU">入力金額</param>
        /// <returns>ZeiInfoクラス</returns>
        public static ZeiInfo Get_Syohizei(string L_DATE, int zei, double KINGAKU)
        {
            //戻り値の設定
            ZeiInfo zeiinfo = new ZeiInfo();

            //日時指定がない場合には、現在日時をセットする
            if (string.IsNullOrEmpty(L_DATE))
            {
                L_DATE = String.Format("{0:yyyyMMdd}", DateTime.Now);
            }

            //引数が無効な場合の処理
            if (zei < 1 || 4 < zei)
            {
                zeiinfo.error_flag = 1;
                return zeiinfo;
            }

            //SQL文の設定
            System.Text.StringBuilder sqlTarget = new System.Text.StringBuilder();
            sqlTarget.Append("SELECT ZEIKBN1,ZEIKBN2,ZEIRITU ");
            sqlTarget.Append("FROM SYOHIZEI_MST WHERE ");
            sqlTarget.Append("(STARTYMD<='" + L_DATE + "') AND (ENDYMD>='" + L_DATE + "')");

            DataTable tbl = null;

            //DB接続
            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    // SQL実行
                    db.Connect();
                    tbl = db.ExecSQL(sqlTarget.ToString());
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

            // 結果の列数を取得する
            if (tbl.Columns.Count < 1)
            {
                return zeiinfo;
            }

            // データがない
            if (tbl.Rows.Count == 0)
            {
                return zeiinfo;
            }

            // 戻り値に値をセット
            zeiinfo.zeikbn1 = tbl.Rows[0]["ZEIKBN1"].ToString();
            zeiinfo.zeikbn2 = tbl.Rows[0]["ZEIKBN2"].ToString();
            zeiinfo.zeiritu = Convert.ToDecimal(tbl.Rows[0]["ZEIRITU"]);
            zeiinfo.zeikeisan = Convert.ToInt16(tbl.Rows[0]["ZEIRITU"]) + 100;
            zeiinfo.zeihyoji  = Strings.StrConv(tbl.Rows[0]["ZEIRITU"].ToString(), VbStrConv.Wide, 0) + "％";


            if (zei == 1)
            {
                //税抜金額(四捨五入でまるめ)と消費税を計算
                zeiinfo.zeinuki = IPPAN.Marume_RTN(KINGAKU * 100 / (zeiinfo.zeikeisan), 0, 1);
                zeiinfo.zeikomi = KINGAKU;
                zeiinfo.syohizei = zeiinfo.zeikomi - zeiinfo.zeinuki;

            }
            else if (zei == 2)
            {
                //税抜金額(切り上げでまるめ)と消費税を計算
                zeiinfo.zeinuki = IPPAN.Marume_RTN(KINGAKU * 100 / (zeiinfo.zeikeisan), 1, 1);
                zeiinfo.zeikomi = KINGAKU;
                zeiinfo.syohizei = zeiinfo.zeikomi - zeiinfo.zeinuki;
            }
            else if (zei == 3)
            {
                //税込金額(四捨五入でまるめ)と消費税を計算
                zeiinfo.zeikomi = IPPAN.Marume_RTN(KINGAKU * zeiinfo.zeikeisan / 100, 0, 1);
                zeiinfo.zeinuki = KINGAKU;
                zeiinfo.syohizei = zeiinfo.zeikomi - zeiinfo.zeinuki;
            }
            else
            {
                //税込金額(切り上げでまるめ)と消費税を計算
                zeiinfo.zeikomi = IPPAN.Marume_RTN(KINGAKU * zeiinfo.zeikeisan / 100, 1, 1);
                zeiinfo.zeinuki = KINGAKU;
                zeiinfo.syohizei = zeiinfo.zeikomi - zeiinfo.zeinuki;
            }

            return zeiinfo;
        }
        //13.06.10 tsunamoto 消費税対応 end

        //13.06.10 tsunamoto 消費税対応 start
        /// <summary>
        /// 税区分と金額から、税抜金額、税込金額、消費税を求める。テーブルから取得した税区分や税率なども返す。       
        /// </summary>
        /// <param name="kbn">1の場合、引数ZEIKBNは税区分1,2の場合、引数ZEIKBNは税区分2</param>
        /// <param name="ZEIKBN">税区分の値</param>
        /// <param name="zei">引数が税込、四捨五入まるめなら「1」,切上げまるめなら「2」、引数が税抜、四捨五入まるめなら「3」,切上げまるめなら「4」</param>
        /// <param name="KINGAKU">入力金額 </param>
        /// <returns>ZeiInfoクラス</returns>
        public static ZeiInfo Get_Syohizei(int kbn, string ZEIKBN , int zei , double KINGAKU)
        {
            //テーブルから値をとるための変換
            if (ZEIKBN == "")
            {
                ZEIKBN = " ";
            }

            //戻り値の設定
            ZeiInfo zeiinfo = new ZeiInfo();
            zeiinfo.zeikbn1 = ZEIKBN;
            zeiinfo.zeikbn2 = ZEIKBN;

            //引数が無効な値の場合
            if (kbn != 1 && kbn != 2)
            {
                zeiinfo.error_flag = 1;
                return zeiinfo;
            }

            if ( zei < 1 || 4 < zei)
            {
                zeiinfo.error_flag = 1;
                return zeiinfo;
            }

            //SQL文の設定
            System.Text.StringBuilder sqlTarget = new System.Text.StringBuilder();
            sqlTarget.Append("SELECT ZEIKBN1,ZEIKBN2,ZEIRITU FROM SYOHIZEI_MST WHERE");
            if (kbn == 1)
            {
                sqlTarget.Append(" ZEIKBN1='" + ZEIKBN + "'");
            }
            else
            {
                sqlTarget.Append(" ZEIKBN2='" + ZEIKBN + "'");
            }

            DataTable tbl = null;

            //DBに接続
            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    // SQL実行
                    db.Connect();
                    tbl = db.ExecSQL(sqlTarget.ToString());
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

            // 結果の列数を取得する
            if (tbl.Columns.Count < 1)
            {
                return zeiinfo;
            }

            // データがない
            if (tbl.Rows.Count == 0)
            {

                if (ZEIKBN != " ")
                {
                    zeiinfo = Get_Syohizei(1, " ", zei, KINGAKU);

                }
                return zeiinfo;
            }

            // 戻り値に値をセット
            zeiinfo.zeikbn1   = tbl.Rows[0]["ZEIKBN1"].ToString();
            zeiinfo.zeikbn2   = tbl.Rows[0]["ZEIKBN2"].ToString();
            zeiinfo.zeiritu   = Convert.ToDecimal(tbl.Rows[0]["ZEIRITU"]);
            zeiinfo.zeikeisan = Convert.ToInt16(tbl.Rows[0]["ZEIRITU"]) + 100;
            zeiinfo.zeihyoji = Strings.StrConv(tbl.Rows[0]["ZEIRITU"].ToString(), VbStrConv.Wide, 0) + "％";

            if (zei == 1)
            {
                //税抜金額(四捨五入でまるめ)と消費税を計算
                zeiinfo.zeinuki  = IPPAN.Marume_RTN(KINGAKU * 100 / (zeiinfo.zeikeisan), 0, 1);
                zeiinfo.zeikomi  = KINGAKU;
                zeiinfo.syohizei = zeiinfo.zeikomi - zeiinfo.zeinuki;
                
            }
            else if (zei == 2)
            {
                //税抜金額(切り上げでまるめ)と消費税を計算
                zeiinfo.zeinuki = IPPAN.Marume_RTN(KINGAKU * 100 / (zeiinfo.zeikeisan), 1, 1);
                zeiinfo.zeikomi = KINGAKU;
                zeiinfo.syohizei = zeiinfo.zeikomi - zeiinfo.zeinuki;
            }
            else if (zei == 3)
            {
                //税込金額(四捨五入でまるめ)と消費税を計算
                zeiinfo.zeikomi = IPPAN.Marume_RTN(KINGAKU * zeiinfo.zeikeisan / 100, 0, 1);
                zeiinfo.zeinuki = KINGAKU;
                zeiinfo.syohizei = zeiinfo.zeikomi - zeiinfo.zeinuki;
            }
            else
            {
                //税込金額(切り上げでまるめ)と消費税を計算
                zeiinfo.zeikomi = IPPAN.Marume_RTN(KINGAKU * zeiinfo.zeikeisan / 100, 1, 1);
                zeiinfo.zeinuki = KINGAKU;
                zeiinfo.syohizei = zeiinfo.zeikomi - zeiinfo.zeinuki;
            }

            return zeiinfo;
        }
        //13.06.10 tsunamoto 消費税対応 end

        public static string MixSpace_Set(string ss, int offs)
        {
            string functionReturnValue = string.Empty;
            //機能：全角／半角混在文字列の左端から指定バイト数分の文字列を得る。
            //　　　余白には半角スペース埋めを行なう。
            //引数：ss...元となる全角／半角混在文字列
            //　　　offs...結果文字列のバイト数
            //戻値：結果文字列
            int i = 0;
            //文字位置
            int addr = 0;
            //バイト位置

            i = 1;
            addr = 0;
            functionReturnValue = "";
            ss = Strings.Trim(ss);
            while (addr < offs)
            {
                if (string.IsNullOrEmpty(Strings.Mid(ss, i, 1)))
                {
                    break;
                }
                //制御コードをカット
                if (Strings.Asc(Strings.Mid(ss, i, 1)) == 0 || Strings.Asc(Strings.Mid(ss, i, 1)) == 8 || Strings.Asc(Strings.Mid(ss, i, 1)) == 9 || Strings.Asc(Strings.Mid(ss, i, 1)) == 10 || Strings.Asc(Strings.Mid(ss, i, 1)) == 13)
                {
                }
                else
                {
                    if (Strings.Asc(Strings.Mid(ss, i, 1)) < 0 || Strings.Asc(Strings.Mid(ss, i, 1)) > 255)
                    {
                        //最終文字がまたがる時はカット
                        if (addr + 2 > offs)
                        {
                            break;
                        }
                        functionReturnValue = functionReturnValue + Strings.Mid(ss, i, 1);
                        //全角のときはバイト位置に＋２
                        addr = addr + 2;
                    }
                    else
                    {
                        functionReturnValue = functionReturnValue + Strings.Mid(ss, i, 1);
                        //半角のときはバイト位置に＋１
                        addr = addr + 1;
                    }
                }
                //文字位置を＋１
                i++;
            }
            //余りはスペース埋め
            while (addr < offs)
            {
                functionReturnValue = functionReturnValue + " ";
                //バイト位置を＋１
                addr = addr + 1;
            }
            return functionReturnValue;
        }

        public static short Input_Check2(string L_MOJI)
        {
            short functionReturnValue = 1;

            //文字位置
            int i = 0;

            //文字ワーク
            string L_W_MOJI = string.Empty;

            i = 1;
            L_W_MOJI = Strings.Trim(L_MOJI);

            while (Strings.Len(L_W_MOJI) >= i)
            {
                if (string.IsNullOrEmpty(Strings.Mid(L_W_MOJI, i, 1)))
                {
                    break;
                }
                if (Strings.Asc(Strings.Mid(L_W_MOJI, i, 1)) < 0 || Strings.Asc(Strings.Mid(L_W_MOJI, i, 1)) > 255)
                {
                    return functionReturnValue;
                }
                else
                {
                    //文字位置を＋１
                    i++;
                }
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }
    }
}
