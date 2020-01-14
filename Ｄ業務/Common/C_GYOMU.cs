using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Data.Odbc;

namespace Project1
{
    /// <summary>
    /// 業務関数
    /// </summary>
	static class GYOMU
	{
        //dbのワークエリア
        //トランのワークエリア
        public static string[] G_TRAN_AREA = new string[81];
        //備考トランのワークエリア
        public static string[] G_BIKOU_AREA = new string[21];
        //社員マスタのワークエリア
        public static string[] G_SYAIN_AREA = new string[5];
        //所属マスタのワークエリア
        public static string[] G_SYOZO_AREA = new string[3];
        //銀行営業日マスタのワークエリア
        public static string[] G_EIGYO_AREA = new string[33];
        //発番№範囲マスタのワークエリア
        public static string[] G_HANI_AREA = new string[2];
        //部門マスタのワークエリア
        public static string[] G_BUMON_AREA = new string[100];

		public static string G_SYAIN_CD = string.Empty;
        public static string G_BUMON_CD = string.Empty;
        public static string G_SYOZOKU_CD = string.Empty;
        public static string G_SYAIN_NM = string.Empty;
        public static string G_SYAINYMD = string.Empty;
        public static string G_BUMON_NM = string.Empty;
        public static string G_BUMON1_CD = string.Empty;
        public static string G_BUMON2_CD = string.Empty;

        public static string G_SYOZOKU_NM = string.Empty;
        public static string G_SEQNO = string.Empty;
        public static string G_KEIHI = string.Empty;
        public static string G_YOSAN = string.Empty;
        public static string G_SYOKUI = string.Empty;
        public static string G_OFFICE_SYOKUI = string.Empty;

        //共通精算処理時の呼び元が贈答精算処理かどうか判定するフラグ
        public static string G_KeichoFlg = string.Empty;

        //最終承認職位コードマスタの検索
        public static short Syokui_Kensaku(string L_GYOMU, string L_HANTEI)
		{
            //L_GYOMU:業務区分 L_HANTEI:職位判定金額
            short functionReturnValue = 1;
            DataTable tbl = null;

			IPPAN.G_SQL = "Select SYOKUI from SYONIN_MST WHERE ";
			IPPAN.G_SQL = IPPAN.G_SQL + "OFCID = '" + Strings.Trim(SO.SO_OFFICEID) + "' And ";
			IPPAN.G_SQL = IPPAN.G_SQL + "GYOMUKBN = '" + Strings.Trim(L_GYOMU) + "' and ";
			IPPAN.G_SQL = IPPAN.G_SQL + "(HANFROM <= NVL(Trim('" + L_HANTEI + "'), 0) and ";
			IPPAN.G_SQL = IPPAN.G_SQL + "HANTO >= NVL(Trim('" + L_HANTEI + "'), 0))";

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    // SQL実行
                    db.Connect();
                    tbl = db.ExecSQL(IPPAN.G_SQL);

                    //結果の列数を取得する
                    if (0 < tbl.Columns.Count)
                    {
                        //データがない
                        if (tbl.Rows.Count == 0)
                        {
                            IPPAN.G_SQL = "Select SYOKUI from SYONIN_MST WHERE ";
                            IPPAN.G_SQL = IPPAN.G_SQL + "OFCID = '" + Strings.Trim(SO.SO_OFFICEID) + "' And ";
                            IPPAN.G_SQL = IPPAN.G_SQL + "GYOMUKBN = '" + Strings.Trim(L_GYOMU) + "' and ";
                            IPPAN.G_SQL = IPPAN.G_SQL + "HANTO = 0";

                            // SQL実行
                            tbl = db.ExecSQL(IPPAN.G_SQL);

                            //結果の列数を取得する
                            if (0 < tbl.Columns.Count)
                            {
                                //データがない
                                if (tbl.Rows.Count == 0)
                                {
                                    IPPAN.G_SQL = "Select SYOKUI from SYONIN_MST WHERE ";
                                    IPPAN.G_SQL = IPPAN.G_SQL + "OFCID = '" + Strings.Space(10) + "' And ";
                                    IPPAN.G_SQL = IPPAN.G_SQL + "GYOMUKBN = '" + Strings.Trim(L_GYOMU) + "' and ";
                                    IPPAN.G_SQL = IPPAN.G_SQL + "(HANFROM <= NVL(Trim('" + L_HANTEI + "'), 0) and ";
                                    IPPAN.G_SQL = IPPAN.G_SQL + "HANTO >= NVL(Trim('" + L_HANTEI + "'), 0))";

                                    // SQL実行
                                    tbl = db.ExecSQL(IPPAN.G_SQL);

                                    //結果の列数を取得する
                                    if (0 < tbl.Columns.Count)
                                    {
                                        if (tbl.Rows.Count == 0)
                                        {
                                            IPPAN.G_SQL = "Select SYOKUI from SYONIN_MST WHERE ";
                                            IPPAN.G_SQL = IPPAN.G_SQL + "OFCID = '" + Strings.Space(10) + "' And ";
                                            IPPAN.G_SQL = IPPAN.G_SQL + "GYOMUKBN = '" + Strings.Trim(L_GYOMU) + "' and ";
                                            IPPAN.G_SQL = IPPAN.G_SQL + "HANTO = 0";

                                            // SQL実行
                                            tbl = db.ExecSQL(IPPAN.G_SQL);

                                            //結果の列数を取得する
                                            if (0 < tbl.Columns.Count)
                                            {
                                                if (tbl.Rows.Count == 0)
                                                {
                                                    return functionReturnValue;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        for (int i = 0; i < tbl.Rows.Count; i++)
                        {
                            for (int j = 0; j < tbl.Columns.Count; j++)
                            {
                                //項目セット
                                G_SYOKUI = tbl.Rows[i][j].ToString();
                            }
                        }
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

			functionReturnValue = 0;
			return functionReturnValue;
		}

        //部門マスタの検索 データがないときは１を返す。
        public static short Bumon_Kensaku(string L_Kubun)
		{
            short functionReturnValue = 1;
            DataTable tbl = null;
            int nIdx = 0;

			switch (Strings.Trim(L_Kubun)) {
				case "1":
				case "2":
					IPPAN.G_SQL = "Select BUMONCD,BUMONNM from BUMON_MST WHERE BUMONKBN = '" + L_Kubun + "' order by BUMONCD";
					break;
				default:
					IPPAN.G_SQL = "Select BUMONCD,BUMONNM from BUMON_MST order by BUMONCD";
					break;
			}

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
                return functionReturnValue;
            }

            //　データがない
            if (tbl.Rows.Count == 0)
            {
                return functionReturnValue;
            }

            nIdx = 0;
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
					//項目セット
                    G_BUMON_AREA[nIdx] = tbl.Rows[i][j].ToString();
					nIdx++;
				}
			}

			functionReturnValue = 0;
			return functionReturnValue;
		}

        //発行№管理マスタの検索
        public static short Hakko_Kensaku(C_ODBC db, string L_GYOMU)
		{
            //L_GYOMU:業務区分
            short functionReturnValue = 1;

            DataTable tbl = null;
            string L_WYMD = string.Empty;
            string sTmp = string.Empty;

            L_WYMD = String.Format("{0:yyyyMM}", DateTime.Now);

			//発番№範囲マスタ検索
			IPPAN.G_RET = Hani_Kensaku(L_GYOMU);

            IPPAN.G_SQL = "Select SEQNO from HATUBAN_MST WHERE GYOUKBN = '" + Strings.Trim(L_GYOMU) + "' and KANRIYM = '" + L_WYMD + "'";

            try
            {
                //トランザクションは呼出し元で制御
                //排他ロック実施
                db.TableLockEX("HATUBAN_MST");
                //SQL実行
                tbl = db.ExecSQL(IPPAN.G_SQL);

                //結果の列数を取得する
                if (0 < tbl.Columns.Count)
                {
                    //データがない
                    if (tbl.Rows.Count == 0)
                    {
                        //データがないときは発番№範囲マスタのＦＲＯＭで新規登録
                        IPPAN.G_SQL = "insert into HATUBAN_MST values ('" + Strings.Trim(L_GYOMU) + "','" + L_WYMD + "','" + Strings.Trim(G_HANI_AREA[0]) + "')";
                        db.ExecSQL(IPPAN.G_SQL);
                        db.Commit();
                        //トランザクション再開
                        db.BeginTrans();

                        //発番の初期値を変更
                        int num = 0;
                        int.TryParse(Strings.Trim(G_HANI_AREA[0]), out num);
                        G_SEQNO = Strings.Trim(L_GYOMU) + String.Format("{0:yyyyMM}", DateTime.Now) + String.Format("{0:0000}", num);

                        functionReturnValue = 0;
                        return functionReturnValue;
                    }

                    for (int i = 0; i < tbl.Rows.Count; i++)
                    {
                        for (int j = 0; j < tbl.Columns.Count; j++)
                        {
                            //項目セット
                            G_SEQNO = tbl.Rows[i][j].ToString();
                        }
                    }

                    //１プラスした値が発番№範囲マスタのＴＯを超えているときはプログラム終了
                    if (Convert.ToInt32(G_SEQNO) + 1 > Convert.ToInt32(G_HANI_AREA[1]))
                    {
                        //申請№が発番№範囲マスタの範囲を超えています
                        IPPAN.Error_Msg("E203", 0, " ");
                        db.Rollback();
                        db.Disconnect();
                        System.Environment.Exit(0);
                    }

                    //１プラスして取得
                    G_SEQNO = Strings.Trim(L_GYOMU) + String.Format("{0:yyyyMM}", DateTime.Now) + String.Format("{0:0000}", Convert.ToInt32(G_SEQNO) + 1);
                    IPPAN.G_SQL = "UPDATE HATUBAN_MST SET SEQNO = '" + Strings.Mid(G_SEQNO, 9, 4) + "' WHERE GYOUKBN = '" + Strings.Trim(L_GYOMU) + "' and KANRIYM = '" + L_WYMD + "'";
                    db.ExecSQL(IPPAN.G_SQL);
                }

                // 終了処理
                db.Commit();
                //トランザクション再開
                db.BeginTrans();
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

        //社員マスタの検索 データがないときは１を返す。
		public static short Syain_Kensaku(string L_SyainCD)
		{
            //L_SyainCD:社員コード
            short functionReturnValue = 1;
            DataTable tbl = null;

            //2013.09.20 DSK yoshida START
            //IPPAN.G_SQL = "Select SYAINCD,SYAINNM,SYAINSZCD,BUMON,SYAINYMD from SYAIN_MST WHERE SYAINCD = '" + Strings.Trim(L_SyainCD) + "'";
            IPPAN.G_SQL = "Select SYAINCD,SYAINNM,SYAINSZCD,BUMON,SYAINYMD from FL_SYAIN_MST_VIEW WHERE SYAINCD = '" + Strings.Trim(L_SyainCD) + "'";
            //2013.09.20 DSK yoshida END

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
                    G_SYAIN_AREA[j] = tbl.Rows[i][j].ToString();
				}
			}

            G_SYAIN_CD = G_SYAIN_AREA[0];
            G_SYAIN_NM = IPPAN.Space_Cut(G_SYAIN_AREA[1]);
            G_SYOZOKU_CD = G_SYAIN_AREA[2];
            G_BUMON_CD = G_SYAIN_AREA[3];
            G_SYAINYMD = G_SYAIN_AREA[4];

			functionReturnValue = 0;
			
			return functionReturnValue;
		}

        //所属マスタの検索 データがないときは１を返す。
		public static int Syozoku_Kensaku()
		{
			int functionReturnValue = 1;
            DataTable tbl = null;
            
			IPPAN.G_SQL = "Select SZNM,BUMON1,BUMON2 from SYOZO_MST WHERE SZCD = '" + G_SYOZOKU_CD + "'";

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
                    G_SYOZO_AREA[j] = tbl.Rows[i][j].ToString();
				}
			}

            G_SYOZOKU_NM = IPPAN.Space_Cut(G_SYOZO_AREA[0]);
            G_BUMON1_CD = G_SYOZO_AREA[1];
            G_BUMON2_CD = G_SYOZO_AREA[2];

            functionReturnValue = 0;
			return functionReturnValue;
		}
        
        //承認申請の登録
		public static int SYONIN_TOROKU(string L_Kubun, string L_USERID, string L_OFFICEID, string L_SEINO)
		{
            //L_Kubun:処理区分-> 0:申請 1:精算
            int functionReturnValue = 1;
			
            IPPAN.G_datetime = String.Format("{0:yyyyMMdd HH:mm:ss}", DateTime.Now);

			IPPAN.G_SQL = "insert into SYONIN_TRN values (";
			IPPAN.G_SQL = IPPAN.G_SQL + "'" + (G_TRAN_AREA[0]) + "'";
			IPPAN.G_SQL = IPPAN.G_SQL + ",'" + (L_SEINO) + "'";
			IPPAN.G_SQL = IPPAN.G_SQL + ",'" + (L_Kubun) + "'";
			IPPAN.G_SQL = IPPAN.G_SQL + ",'" + (L_USERID) + "'";
			IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(G_SYAIN_NM, 15, 2) + "'";
			IPPAN.G_SQL = IPPAN.G_SQL + ",'" + Strings.Left(L_OFFICEID, 5) + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(G_SYOZOKU_NM, 15, 2) + "'";
			IPPAN.G_SQL = IPPAN.G_SQL + ",'" + (G_BUMON_CD) + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",'" + IPPAN.Space_Set(G_BUMON_NM, 15, 2) + "'";
			IPPAN.G_SQL = IPPAN.G_SQL + ",'" + (Strings.Space(20)) + "'";
            IPPAN.G_SQL = IPPAN.G_SQL + ",to_date('" + (IPPAN.G_datetime) + "', 'YYYY/MM/DD HH24:MI:SS')";
            IPPAN.G_SQL = IPPAN.G_SQL + ",to_date('" + (IPPAN.G_datetime) + "', 'YYYY/MM/DD HH24:MI:SS'))";

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    //DB接続
                    db.Connect();
                    //トランザクション開始
                    db.BeginTrans();
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
            }

            functionReturnValue = 0;
			return functionReturnValue;
		}



		public static int OFFICESYO_KENSAKU(string L_OFFICE_ID)
		{
            //L_OFFICE_ID:オフィスＩＤ
            int functionReturnValue = 1;
            
            DataTable tbl = null;
            
            IPPAN.G_SQL = "Select SYOKUI from OFFICESYO_MST WHERE OFFICEID = '" + L_OFFICE_ID + "'";

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
                    G_OFFICE_SYOKUI = tbl.Rows[i][j].ToString();
				}
			}

			functionReturnValue = 0;
			return functionReturnValue;
		}

        //銀行営業日マスタ検索
		public static int EIGYO_KENSAKU(string L_YYYYMM)
		{
			//L_YYMM:YYYYMM形式の日付
			int functionReturnValue = 1;
            DataTable tbl = null;
            
			IPPAN.G_SQL = "Select * from GINEIG_MST WHERE EIGYM = '" + L_YYYYMM + "'";

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
                    G_EIGYO_AREA[j] = tbl.Rows[i][j].ToString();
				}
			}

			functionReturnValue = 0;
			return functionReturnValue;
		}

        //銀行営業日のﾁｪｯｸ
		public static int Eigyo_Check(string L_YMD)
		{
            //L_YMD:YYYYMMDD形式の日付
            int functionReturnValue = -1;

			IPPAN.G_RET = EIGYO_KENSAKU(Strings.Mid(L_YMD, 1, 6));

            if (G_EIGYO_AREA[Convert.ToInt32(Strings.Mid(L_YMD, 7, 2))] != "0")
            {
                return functionReturnValue;
            }

			functionReturnValue = 0;
			return functionReturnValue;
		}

        //発番№範囲マスタ検索
		public static int Hani_Kensaku(string L_GYOMU)
		{
            //L_GYOMU:業務区分
            int functionReturnValue = 1;

            DataTable tbl = null;
            
			IPPAN.G_SQL = "Select SEQSTR,SEQEND from HATUBAN_HANI_MST WHERE GYOUKBN = '" + Strings.Trim(L_GYOMU) + "'";

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
                    G_HANI_AREA[j] = tbl.Rows[i][j].ToString();
                }
            }

			functionReturnValue = 0;
			
			return functionReturnValue;
		}

        //引数に日付を追加した発番Ｍ検索ロジック
        public static int Denpyo_Hakko_Kensaku(C_ODBC db, string L_GYOMU, string L_WYMD)
        {
            //L_GYOMU:業務区分 L_WYMD:日付(yyyymm)
            int functionReturnValue = 1;

            DataTable tbl = null;
            string sTmp = string.Empty;

            //発番№範囲マスタ検索
            IPPAN.G_RET = Hani_Kensaku(L_GYOMU);

            try
            {
                IPPAN.G_SQL = "Select SEQNO from HATUBAN_MST WHERE GYOUKBN = '" + Strings.Trim(L_GYOMU) + "' and KANRIYM = '" + L_WYMD + "'";

                //トランザクションは呼出し元で制御
                //排他ロック実施
                db.TableLockEX("HATUBAN_MST");
                //SQL実行
                tbl = db.ExecSQL(IPPAN.G_SQL);

                // 結果の列数を取得する
                if (0 < tbl.Columns.Count)
                {
                    // データがない
                    if (tbl.Rows.Count == 0)
                    {
                        //データがないときは発番№範囲マスタのＦＲＯＭで新規登録
                        IPPAN.G_SQL = "insert into HATUBAN_MST values ('" + Strings.Trim(L_GYOMU) + "','" + L_WYMD + "','" + Strings.Trim(G_HANI_AREA[0]) + "')";
                        db.ExecSQL(IPPAN.G_SQL);

                        int num = 0;
                        int.TryParse(Strings.Trim(G_HANI_AREA[0]), out num);
                        G_SEQNO = Strings.Trim(L_GYOMU) + L_WYMD + String.Format("{0:0000}", num);

                        functionReturnValue = 0;
                        return functionReturnValue;
                    }

                    for (int i = 0; i < tbl.Rows.Count; i++)
                    {
                        for (int j = 0; j < tbl.Columns.Count; j++)
                        {
                            // 項目セット
                            G_SEQNO = tbl.Rows[i][j].ToString();
                        }
                    }

                    //１プラスした値が発番№範囲マスタのＴＯを超えているときはプログラム終了
                    if (Convert.ToInt32(G_SEQNO) + 1 > Convert.ToInt32(G_HANI_AREA[1]))
                    {
                        //申請№が発番№範囲マスタの範囲を超えています
                        IPPAN.Error_Msg("E203", 0, " ");
                        return functionReturnValue;
                    }

                    //１プラスして取得
                    G_SEQNO = Strings.Trim(L_GYOMU) + L_WYMD + String.Format("{0:0000}", Convert.ToInt32(G_SEQNO) + 1);

                    IPPAN.G_SQL = "UPDATE HATUBAN_MST SET SEQNO = '" + Strings.Mid(G_SEQNO, 9, 4) + "' WHERE GYOUKBN = '" + Strings.Trim(L_GYOMU) + "' and KANRIYM = '" + L_WYMD + "'";

                    //SQL実行
                    db.ExecSQL(IPPAN.G_SQL);
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

            functionReturnValue = 0;
            return functionReturnValue;
        }
	}
}
