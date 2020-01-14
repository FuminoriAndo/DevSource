using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.Odbc;
//↓2013.08.08 kuwajima 追加要望対応
using System.IO;
using Microsoft.VisualBasic.PowerPacks.Printing.Compatibility.VB6;
//↑2013.08.08 kuwajima 追加要望対応

namespace Project1
{
    internal partial class FRM_CKSI0040M : System.Windows.Forms.Form
    {
        //*************************************************************************************
        //
        //
        //   プログラム名　　　検収一覧表発行
        //
        //
        //   修正履歴
        //
        //   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
        //   03.01.15           NTIS浅田　　Ｓ／Ｏサーバ更新の為、VB4.0→VB6.0SP5へのバージョンアップ対応
        //   13.08.08           HIT 桑島　　追加要望対応
        //*************************************************************************************
        //↓2013.08.21 kuwajima 追加要望対応
        private FaxController faxController;
        //↑2013.08.21 kuwajima 追加要望対応

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //↓2013.08.23 kuwajima FAXSDKがセットアップされて無い場合の例外に対応
            //Application.Run(new FRM_CKSI0040M());
            try
            {
                Application.Run(new FRM_CKSI0040M());
            }
            catch (Exception)
            {
                MessageBox.Show("システム管理者に連絡してください。");
            }
            //↑2013.08.23 kuwajima FAXSDKがセットアップされて無い場合の例外に対応
        }

        [System.Security.Permissions.SecurityPermission(
            System.Security.Permissions.SecurityAction.LinkDemand,
            Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x112;
            const long SC_CLOSE = 0xF060L;

            if (m.Msg == WM_SYSCOMMAND &&
               (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE)
            {
                return;
            }

            base.WndProc(ref m);
        }

        // 業者マスタ読込み処理
        public short GYOSYA_KENSAKU(string L_CD)
        {
            short functionReturnValue = 1;

            string[] L_MST_AREA = new string[2];
            DataTable tbl = null;

            // 業者コードをｷｰにﾃﾞｰﾀﾍﾞｰｽ検索
            IPPAN.G_SQL = "Select GYOSYACD,KOZANM from GYOSYA_MST WHERE GYOSYACD = '" + L_CD + "'";

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

            //行の各列のﾃﾞｰﾀの値を取得する
            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                //項目をﾜｰｸにｾｯﾄ
                L_MST_AREA[j] = tbl.Rows[0][j].ToString();
            }

            CKSI0040_01.G_GYOSYANM = Strings.Mid(L_MST_AREA[1], 1, 20);

            functionReturnValue = 0;
            return functionReturnValue;
        }

        //↓2013.08.08 kuwajima 追加要望対応
        private bool isFaxSendGyosya(string gyosyacd)
        {
            DataTable tbl = null;

            IPPAN.G_SQL = "SELECT FAX FROM GYOSYA_MST WHERE GYOSYACD = '" + gyosyacd + "'";

            using (C_ODBC db = new C_ODBC())
            {
                try
                {
                    db.Connect();
                    tbl = db.ExecSQL(IPPAN.G_SQL);
                }
                catch (Exception)
                {
                    db.Error();
                }
            }

            if (tbl.Rows.Count == 0)
            {
                return false;
            }

            if (tbl.Rows[0]["FAX"].ToString() == "1")
            {
                //FAX送信対象
                return true;
            }
            else
            {
                //FAX送信対象外
                return false;
            }
        }
        //↑2013.08.08 kuwajima 追加要望対応

        private void GAMEN_CLEAR()
        {
            TXT_NEN.Text = "";
            TXT_TUKI.Text = "";
            TXT_GYOSYACD.Text = "";
            LBL_GYOSYANM.Text = "";
        }

        public short DATA_KENSAKU(int L_KBN)
        {
            short functionReturnValue = 1;

            //読込データ処理用ワーク
            string[] L_AREA = new string[6];

            bool L_SAISYO = false;
            string L_HINMOKUCD = null;
            string L_GYOSYACD = null;
            string L_SYUBETU = null;
            string ZENGETU = null;
            DataTable tbl = null;

            L_SAISYO = true;

            //副資材作業誌トラン・副資材棚卸トランの品目コード及び
            //それに該当する副資材品目マスタの種別の検索

            if (Strings.Mid(CKSI0040_01.G_NENGETU, 5, 2) == "01")
            {
                ZENGETU = Convert.ToString(Convert.ToInt32(CKSI0040_01.G_NENGETU) - 89);
            }
            else
            {
                ZENGETU = Convert.ToString(Convert.ToInt32(CKSI0040_01.G_NENGETU) - 1);
            }

            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "SELECT ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_SAGYOSI_TRN.HINMOKUCD, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_SAGYOSI_TRN.GYOSYACD, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.SYUBETU, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.HINMOKUNM, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.TANI ";
            IPPAN.G_SQL = IPPAN.G_SQL + "From ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_SAGYOSI_TRN, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "Where ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.HINMOKUCD = SIZAI_SAGYOSI_TRN.HINMOKUCD AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.KENSYUKBN = '1' AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_SAGYOSI_TRN.SAGYOBI LIKE '" + CKSI0040_01.G_NENGETU + "__' ";

            if (L_KBN == 1)
            {
                IPPAN.G_SQL = IPPAN.G_SQL + "AND SIZAI_SAGYOSI_TRN.GYOSYACD = '" + TXT_GYOSYACD.Text + "' ";
            }
            IPPAN.G_SQL = IPPAN.G_SQL + "Union ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SELECT ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_TANAOROSI_TRN.HINMOKUCD, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_TANAOROSI_TRN.GYOSYACD, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.SYUBETU, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.HINMOKUNM, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.TANI ";
            IPPAN.G_SQL = IPPAN.G_SQL + "From ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_TANAOROSI_TRN, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "Where ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.HINMOKUCD = SIZAI_TANAOROSI_TRN.HINMOKUCD AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.KENSYUKBN = '1' AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.SYUBETU = '2' AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_TANAOROSI_TRN.TANAYM = '" + CKSI0040_01.G_NENGETU + "' ";
            if (L_KBN == 1)
            {
                IPPAN.G_SQL = IPPAN.G_SQL + "AND SIZAI_TANAOROSI_TRN.GYOSYACD = '" + TXT_GYOSYACD.Text + "' ";
            }
            IPPAN.G_SQL = IPPAN.G_SQL + "Union ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SELECT ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_TANAOROSI_TRN.HINMOKUCD, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_TANAOROSI_TRN.GYOSYACD, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.SYUBETU, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.HINMOKUNM, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.TANI ";
            IPPAN.G_SQL = IPPAN.G_SQL + "From ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_TANAOROSI_TRN, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "Where ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.HINMOKUCD = SIZAI_TANAOROSI_TRN.HINMOKUCD AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.KENSYUKBN = '1' AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_HINMOKU_MST.SYUBETU = '2' AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SIZAI_TANAOROSI_TRN.TANAYM = '" + ZENGETU + "' ";
            if (L_KBN == 1)
            {
                IPPAN.G_SQL = IPPAN.G_SQL + "AND SIZAI_TANAOROSI_TRN.GYOSYACD = '" + TXT_GYOSYACD.Text + "' ";
            }
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
                //対象データがありません
                IPPAN.Error_Msg("E536", 0, " "); 
                return functionReturnValue;
            }

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int j = 0; j < tbl.Columns.Count; j++)
                {
                    //項目セット
                    L_AREA[j] = tbl.Rows[i][j].ToString();
                }

                L_HINMOKUCD = "";
                L_GYOSYACD = "";
                L_SYUBETU = "";
                CKSI0040_01.G_HINMOKUNM = "";
                CKSI0040_01.G_TANI = "";

                L_HINMOKUCD = L_AREA[0];
                L_GYOSYACD = L_AREA[1];
                L_SYUBETU = L_AREA[2];
                CKSI0040_01.G_HINMOKUNM = L_AREA[3];
                CKSI0040_01.G_TANI = L_AREA[4];

                if (L_SAISYO == true)
                {
                    //副資材検収明細トランの削除
                    KENSYU_SAKUJYO();
                    L_SAISYO = false;
                }

                //副資材検収明細トランの作成
                DATA_SAKUSEI(L_HINMOKUCD, L_GYOSYACD, L_SYUBETU);
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public void DATA_SAKUSEI(string L_HINMOKUCD, string L_GYOSYACD, string L_SYUBETU)
        {
            CKSI0040_01.G_S_SURYO = 0;
            CKSI0040_01.G_S_SUIBUN = 0;
            CKSI0040_01.G_T_ZENGETU = 0;
            CKSI0040_01.G_T_TOUGETU = 0;
            CKSI0040_01.G_KENSYURYO = 0;

            switch (L_SYUBETU)
            {
                case "1":
                    IPPAN.G_RET = SAGYOSI_KENSAKU(L_HINMOKUCD, L_GYOSYACD);
                    CKSI0040_01.G_KENSYURYO = CKSI0040_01.G_S_SURYO - CKSI0040_01.G_S_SUIBUN;
                    break;
                case "2":
                    IPPAN.G_RET = SAGYOSI_KENSAKU(L_HINMOKUCD, L_GYOSYACD);
                    //前月棚卸データ
                    IPPAN.G_RET = TANAOROSI_KENSAKU(L_HINMOKUCD, L_GYOSYACD, 0);
                    //当月棚卸データ
                    IPPAN.G_RET = TANAOROSI_KENSAKU(L_HINMOKUCD, L_GYOSYACD, 1);
                    CKSI0040_01.G_KENSYURYO = CKSI0040_01.G_T_ZENGETU + CKSI0040_01.G_S_SURYO - CKSI0040_01.G_S_SUIBUN - CKSI0040_01.G_T_TOUGETU;
                    break;
            }

            IPPAN.G_RET = KENSYU_SAKUSEI(L_HINMOKUCD, L_GYOSYACD, L_SYUBETU);
        }

        public short SAGYOSI_KENSAKU(string L_HINMOKUCD, string L_GYOSYACD)
        {
            short functionReturnValue = 1;

            string[] L_AREA = new string[6];
            DataTable tbl = null;

            //副資材作業誌トランの検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "SELECT HINMOKUCD,GYOSYACD,KUBUN,SUM(SURYO),SUM(SUIBUN) ";
            IPPAN.G_SQL = IPPAN.G_SQL + "From SIZAI_SAGYOSI_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "Where ( HINMOKUCD = '" + L_HINMOKUCD + "' AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "GYOSYACD = '" + L_GYOSYACD + "' AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SAGYOBI LIKE '" + CKSI0040_01.G_NENGETU + "__' ) AND  ";
            IPPAN.G_SQL = IPPAN.G_SQL + "( KUBUN = '1' OR KUBUN = '3' OR KUBUN = '4' ) ";
            IPPAN.G_SQL = IPPAN.G_SQL + "GROUP BY HINMOKUCD,GYOSYACD,KUBUN ";

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
                    L_AREA[j] = tbl.Rows[i][j].ToString();
                }

                switch (L_AREA[2])
                {
                    case "1":
                    case "3":
                        CKSI0040_01.G_S_SURYO = CKSI0040_01.G_S_SURYO + Convert.ToDecimal(L_AREA[3]);
                        CKSI0040_01.G_S_SUIBUN = CKSI0040_01.G_S_SUIBUN + Convert.ToDecimal(L_AREA[4]);
                        break;
                    case "4":
                        CKSI0040_01.G_S_SURYO = CKSI0040_01.G_S_SURYO - Convert.ToDecimal(L_AREA[3]);
                        CKSI0040_01.G_S_SUIBUN = CKSI0040_01.G_S_SUIBUN - Convert.ToDecimal(L_AREA[4]);
                        break;
                }
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public short KENSYU_SAKUSEI(string L_HINMOKUCD, string L_GYOSYACD, string L_SYUBETU)
        {
            short functionReturnValue = 1;

            string[] L_AREA = new string[5];

            string L_KOZANM = null;
            string L_TANTONM = null;
            string L_FAXNO = null;
            DataTable tbl = null;

            IPPAN.G_datetime = String.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);

            //業者マスタの検索
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "SELECT GYOSYACD,KOZANM,TANTONM,FAXNO ";
            IPPAN.G_SQL = IPPAN.G_SQL + "From GYOSYA_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "Where GYOSYACD = '" + L_GYOSYACD + "' ";

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

            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                //項目セット
                L_AREA[j] = tbl.Rows[0][j].ToString();
            }

            L_KOZANM = L_AREA[1];
            L_TANTONM = L_AREA[2];
            L_FAXNO = L_AREA[3];

            //副資材検収明細トランの作成

            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "INSERT INTO SIZAI_KMEISAI_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "( ";
            IPPAN.G_SQL = IPPAN.G_SQL + "GYOSYACD, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKUCD, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "KENSYUYY, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "KENSYUMM, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "GYOSYANM, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "TANTONM, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "FAXNO, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "HINMOKUNM, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "TANINM, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "ZENZAN, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "NYUKO, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SUIBUN, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "KENSYU, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "TOZAN, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "UPDYMD, ";
            IPPAN.G_SQL = IPPAN.G_SQL + "SYUBETU ) ";
            IPPAN.G_SQL = IPPAN.G_SQL + "VALUES ";
            IPPAN.G_SQL = IPPAN.G_SQL + "( ";
            IPPAN.G_SQL = IPPAN.G_SQL + "'" + Strings.Mid(L_GYOSYACD, 1, 4) + "', ";
            IPPAN.G_SQL = IPPAN.G_SQL + "'" + Strings.Mid(L_HINMOKUCD, 1, 4) + "', ";
            IPPAN.G_SQL = IPPAN.G_SQL + "'" + Strings.Mid(CKSI0040_01.G_NENGETU, 3, 2) + "', ";
            IPPAN.G_SQL = IPPAN.G_SQL + "'" + Strings.Mid(CKSI0040_01.G_NENGETU, 5, 2) + "', ";
            IPPAN.G_SQL = IPPAN.G_SQL + "'" + IPPAN.Space_Set(Strings.Trim(Strings.Mid(L_KOZANM, 1, 20)), 20, 2) + "', ";
            IPPAN.G_SQL = IPPAN.G_SQL + "'" + IPPAN.Space_Set(Strings.Trim(Strings.Mid(L_TANTONM, 1, 6)), 6, 2) + "', ";
            IPPAN.G_SQL = IPPAN.G_SQL + "'" + L_FAXNO + "', ";
            IPPAN.G_SQL = IPPAN.G_SQL + "'" + IPPAN.Space_Set(Strings.Trim(Strings.Mid(CKSI0040_01.G_HINMOKUNM, 1, 20)), 20, 2) + "', ";
            IPPAN.G_SQL = IPPAN.G_SQL + "'" + IPPAN.Space_Set(Strings.Trim(Strings.Mid(CKSI0040_01.G_TANI, 1, 2)), 2, 2) + "', ";
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0040_01.G_T_ZENGETU + ", ";
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0040_01.G_S_SURYO + ", ";
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0040_01.G_S_SUIBUN + ", ";
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0040_01.G_KENSYURYO + ",";
            IPPAN.G_SQL = IPPAN.G_SQL + CKSI0040_01.G_T_TOUGETU + ", ";
            IPPAN.G_SQL = IPPAN.G_SQL + "to_date('" + IPPAN.G_datetime + "', 'YYYY/MM/DD HH24:MI:SS'), ";
            IPPAN.G_SQL = IPPAN.G_SQL + "'" + L_SYUBETU + "' ";
            IPPAN.G_SQL = IPPAN.G_SQL + ") ";

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

        public void KENSYU_SAKUJYO()
        {

            //副資材検収明細トランの削除
            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "DELETE SIZAI_KMEISAI_TRN ";

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
                    //SQLトランザクション終了
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
        }

        public short TANAOROSI_KENSAKU(string L_HINMOKUCD, string L_GYOSYACD, short L_KBN)
        {
            short functionReturnValue = 1;

            string[] L_AREA = new string[6];
            DataTable tbl = null;

            //副資材棚卸トランの検索

            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "SELECT HINMOKUCD,GYOSYACD,(ZSOUKO + ZEF + ZLF + ZCC + ZSONOTA + ZMETER + ZYOBI1 + ZYOBI2) ";
            IPPAN.G_SQL = IPPAN.G_SQL + "From SIZAI_TANAOROSI_TRN ";
            IPPAN.G_SQL = IPPAN.G_SQL + "Where HINMOKUCD = '" + L_HINMOKUCD + "' AND ";
            IPPAN.G_SQL = IPPAN.G_SQL + "GYOSYACD = '" + L_GYOSYACD + "' AND ";
            if (L_KBN == 0)
            {
                //前月データ検索
                if (Strings.Mid(CKSI0040_01.G_NENGETU, 5, 2) == "01")
                {
                    IPPAN.G_SQL = IPPAN.G_SQL + "TANAYM = '" + (Convert.ToInt32(CKSI0040_01.G_NENGETU) - 89) + "' ";
                }
                else
                {
                    IPPAN.G_SQL = IPPAN.G_SQL + "TANAYM = '" + (Convert.ToInt32(CKSI0040_01.G_NENGETU) - 1) + "' ";
                }
            }
            else if (L_KBN == 1)
            {
                //当月データ検索
                IPPAN.G_SQL = IPPAN.G_SQL + "TANAYM = '" + CKSI0040_01.G_NENGETU + "' ";
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

            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                //項目セット
                L_AREA[j] = tbl.Rows[0][j].ToString();
            }

            if (L_KBN == 0)
            {
                //前月データ検索
                CKSI0040_01.G_T_ZENGETU = Convert.ToDecimal(L_AREA[2]);
            }
            else if (L_KBN == 1)
            {
                //当月データ検索
                CKSI0040_01.G_T_TOUGETU = Convert.ToDecimal(L_AREA[2]);
            }

            functionReturnValue = 0;
            return functionReturnValue;
        }

        public short GAMEN_CHECK()
        {
            short functionReturnValue = 1;

            if (IPPAN.C_Allspace(TXT_NEN.Text) == 0)
            {
                TXT_NEN.Focus();
                TXT_NEN_Enter(TXT_NEN, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            //入力禁止文字チェック
            else if (IPPAN.Input_Check(TXT_NEN.Text) != 0)
            {
                TXT_NEN.Focus();
                TXT_NEN_Enter(TXT_NEN, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }
            else if (IPPAN.Numeric_Check(TXT_NEN.Text) != 0)
            {
                TXT_NEN.Focus();
                TXT_NEN_Enter(TXT_NEN, new System.EventArgs());
                //日付入力エラー
                IPPAN.Error_Msg("E201", 0, " ");
                return functionReturnValue;
            }
            if (IPPAN.C_Allspace(TXT_TUKI.Text) == 0)
            {
                TXT_TUKI.Focus();
                TXT_TUKI_Enter(TXT_TUKI, new System.EventArgs());
                //必須項目入力エラー
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            //入力禁止文字チェック
            else if (IPPAN.Input_Check(TXT_TUKI.Text) != 0)
            {
                TXT_TUKI.Focus();
                TXT_TUKI_Enter(TXT_TUKI, new System.EventArgs());
                //入力エラー
                IPPAN.Error_Msg("E202", 0, " ");
                return functionReturnValue;
            }
            else if ((IPPAN.Numeric_Check(TXT_TUKI.Text) != 0) || (Convert.ToInt16(TXT_TUKI.Text) < 1 || Convert.ToInt16(TXT_TUKI.Text) > 12))
            {
                TXT_TUKI.Focus();
                TXT_TUKI_Enter(TXT_TUKI, new System.EventArgs());
                //日付入力エラー
                IPPAN.Error_Msg("E201", 0, " ");
                return functionReturnValue;
            }
            CKSI0040_01.G_NENGETU = "";
            CKSI0040_01.G_NENGETU = TXT_NEN.Text + TXT_TUKI.Text + "01";
            //↓2013.08.08 kuwajima 追加要望対応
            //if (IPPAN.Date_Henkan(ref CKSI0040_01.G_NENGETU) != 0)
            //{
            //    TXT_NEN.Focus();
            //    TXT_NEN_Enter(TXT_NEN, new System.EventArgs());
            //    //日付入力エラー
            //    IPPAN.Error_Msg("E201", 0, " ");
            //    return functionReturnValue;
            //}
            //else if (IPPAN.Date_Check2(CKSI0040_01.G_NENGETU) != 0)
            //{
            //    TXT_NEN.Focus();
            //    TXT_NEN_Enter(TXT_NEN, new System.EventArgs());
            //    //日付入力エラー
            //    IPPAN.Error_Msg("E201", 0, " ");
            //    return functionReturnValue;
            //}
            if (IPPAN.Date_Check2(CKSI0040_01.G_NENGETU) != 0)
            {
                TXT_NEN.Focus();
                TXT_NEN_Enter(TXT_NEN, new System.EventArgs());
                //日付入力エラー
                IPPAN.Error_Msg("E201", 0, " ");
                return functionReturnValue;
            }

            if (rdoFaxYes.Checked && TXT_GYOSYACD.Text.Trim() == string.Empty)
            {
                //FAX送信時は業者コードが必須（コード存在チェックは別に行っているのでここではしない）
                TXT_GYOSYACD.Focus();
                TXT_GYOSYACD_Enter(TXT_GYOSYACD, new System.EventArgs());
                IPPAN.Error_Msg("E200", 0, " ");
                return functionReturnValue;
            }
            //↑2013.08.08 kuwajima 追加要望対応

            CKSI0040_01.G_NENGETU = Strings.Mid(CKSI0040_01.G_NENGETU, 1, 6);
            IPPAN.G_RET = IPPAN.C_Allspace(TXT_GYOSYACD.Text);
            if (IPPAN.G_RET != 0)
            {
                //入力禁止文字チェック
                if (IPPAN.Input_Check(TXT_GYOSYACD.Text) != 0)
                {
                    LBL_GYOSYANM.Text = "";
                    TXT_GYOSYACD.Focus();
                    TXT_GYOSYACD_Enter(TXT_GYOSYACD, new System.EventArgs());
                    //入力エラー
                    IPPAN.Error_Msg("E202", 0, " ");
                    return functionReturnValue;
                }
                //業者名存在チェック
                else if (GYOSYA_KENSAKU(TXT_GYOSYACD.Text) != 0)
                {
                    LBL_GYOSYANM.Text = "";
                    TXT_GYOSYACD.Focus();
                    TXT_GYOSYACD_Enter(TXT_GYOSYACD, new System.EventArgs());
                    //業者マスタが見つかりません
                    IPPAN.Error_Msg("E102", 0, " ");
                    return functionReturnValue;
                }
                //↓2013.08.08 kuwajima 追加要望対応
                else if (rdoFaxYes.Checked && isFaxSendGyosya(TXT_GYOSYACD.Text) == false)
                {
                    LBL_GYOSYANM.Text = "";
                    TXT_GYOSYACD.Focus();
                    TXT_GYOSYACD_Enter(TXT_GYOSYACD, new System.EventArgs());
                    //業者マスタが見つかりません
                    IPPAN.Error_Msg("E503", 0, " ");
                    return functionReturnValue;
                }
                //↑2013.08.08 kuwajima 追加要望対応
            }
            functionReturnValue = 0;
            return functionReturnValue;
        }

        public short GET_NENGETU()
        {
            short functionReturnValue = 1;

            string[] L_MST_AREA = new string[3];
            DataTable tbl = null;

            //副資材コントロールマスタの検索

            IPPAN.G_SQL = "";
            IPPAN.G_SQL = "Select ID,SKYM,KBYM from SIZAI_CONTROL_MST ";
            IPPAN.G_SQL = IPPAN.G_SQL + "WHERE ID = 'CKSI' ";

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
                //副資材コントロールマスタが見つかりません。
                IPPAN.Error_Msg("E700", 0, " ");
                return functionReturnValue;
            }

            for (int j = 0; j < tbl.Columns.Count; j++)
            {
                //項目セット
                L_MST_AREA[j] = "";
                L_MST_AREA[j] = tbl.Rows[0][j].ToString();
            }

            //検収年月
            CKSI0040_01.G_NENGETU = Strings.Trim(Strings.Mid(L_MST_AREA[1], 1, 6));
            //検収年月
            CKSI0040_01.G_NENGETU1 = Strings.Trim(Strings.Mid(L_MST_AREA[2], 1, 6));

            functionReturnValue = 0;
            return functionReturnValue;
        }

        private DialogResult CHK_FaxDevice()
        {
            DialogResult dResult = DialogResult.No;
            do
            {
                bool faxDevice = false;
                foreach (Printer prn in GlobalModule.Printers)
                {
                    Printer p;
                    p = prn;
                    if (p.DeviceName == getFaxDeviceName())
                    {
                        faxDevice = true; //FAXが利用可能
                        dResult = DialogResult.Yes;
                        return dResult;
                    }
                }
                if (rdoFaxYes.Checked && faxDevice == false)
                {
                    dResult = C_COMMON.Msg("まいと～くFaxクライアントが使用できません。" + Strings.Chr(13) + "まいと～くFaxクライアントがインストールされているか確認してください。" + Strings.Chr(13) + "確認後’はい’ボタンを押してください。" + Strings.Chr(13) + "’いいえ’でプリンタに出力します。", MessageBoxButtons.YesNo);
                    if (dResult == DialogResult.No)
                    {
                        //プリンタ出力
                        break;
                    }
                }
                else
                {
                    break;
                }
            } while (true);
            return dResult;

        }
        private void CMD_Button_Click(System.Object eventSender, System.EventArgs eventArgs)
        {
            short Index = CMD_Button.GetIndex((Button)eventSender);
            string sSql = "";
            DataTable tbl = null;

            switch (Index)
            {
                case 0:
                    //閉じる
                    this.Close();
                    break;
                case 1:
                    //印刷
                    CMD_Button[0].Enabled = false;
                    CMD_Button[1].Enabled = false;

                    //砂時計ポインタ
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
                    //画面の入力内容チェック
                    IPPAN.G_RET = GAMEN_CHECK();
                    //標準ポインタ
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    if (IPPAN.G_RET != 0)
                    {
                        CMD_Button[0].Enabled = true;
                        CMD_Button[1].Enabled = true;
                        return;
                    }
                    //該当するデータの検索及び
                    //副資材検収明細トランの作成
                    //砂時計ポインタ
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
                    IPPAN.G_RET = DATA_KENSAKU(IPPAN.C_Allspace(TXT_GYOSYACD.Text));
                    //標準ポインタ
                    IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
                    if (IPPAN.G_RET != 0)
                    {
                        CMD_Button[0].Enabled = true;
                        CMD_Button[1].Enabled = true;
                        CMD_Button[1].Focus();
                        return;
                    }
                    //↓2013.08.08 kuwajima 追加要望対応
                    //sSql = "SELECT TANTONM, GYOSYANM, SYUBETU, GYOSYACD, HINMOKUCD, KENSYUYY, KENSYUMM, ";
                    sSql = "SELECT TANTONM, GYOSYANM, SYUBETU, GYOSYACD, HINMOKUCD, CONCAT('20', KENSYUYY) AS KENSYUYY, KENSYUMM, ";
                    //↑2013.08.08 kuwajima 追加要望対応
                    sSql = sSql + " FAXNO, TANINM, ZENZAN, KENSYU, TOZAN, NYUKO, HINMOKUNM, SUIBUN ";
                    sSql = sSql + " FROM   SIZAI_KMEISAI_TRN";
                    sSql = sSql + " WHERE KENSYUYY='" + Strings.Mid(CKSI0040_01.G_NENGETU, 3, 2) + "'";
                    sSql = sSql + " AND KENSYUMM='" + Strings.Mid(CKSI0040_01.G_NENGETU, 5, 2) + "'";
                    sSql = sSql + " ORDER BY GYOSYACD, HINMOKUCD";
                    using (C_ODBC db = new C_ODBC())
                    {
                        try
                        {
                            //DB接続
                            db.Connect();
                            //SQL実行
                            tbl = db.ExecSQL(sSql);
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

                    try
                    {
                        CrystalDecisions.CrystalReports.Engine.ReportDocument CRP_REP = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        //レポートファイル指定
                        CRP_REP.Load("CKSI0040.rpt");
                        //データセット設定
                        CRP_REP.SetDataSource(tbl);

                        //↓2013.08.08 kuwajima 追加要望対応
                        ////印刷部数：1部
                        ////出力先：プリンター
                        ////印刷開始：1部、部単位に印刷しない、全ページ印刷
                        //CRP_REP.PrintToPrinter(1, false, 0, 0);

                        if (rdoFaxNo.Checked)
                        {
                            //印刷部数：1部
                            //出力先：プリンター
                            //印刷開始：1部、部単位に印刷しない、全ページ印刷
                            CRP_REP.PrintToPrinter(1, false, 0, 0);
                        }
                        else
                        {
                            if (CHK_FaxDevice() == DialogResult.Yes)
                            {
                                try
                                {
                                    //FAXクライアント起動する
                                    if (FaxCommon.FAX_CHK(axMcRemote1))
                                    {
                                        //↓2013.08.21 kuwajima
                                        ////FAX出力用に帳票をPDFにする
                                        //string docName = getPdfPath() + @"\" + DateTime.Now.ToString("yyyyMMddhhmmss") + "副資材検収明細書.pdf";
                                        //CRP_REP.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4Small;
                                        //CRP_REP.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, docName);

                                        ////FAX出力 外部連携CSVの作成（業者は１件のみ指定されているのでFAX番号等は先頭レコードを使う）
                                        //C_FAXCSV.FaxData fd = new C_FAXCSV.FaxData();
                                        //fd.GenkoFileName = docName;
                                        //fd.FaxNo = tbl.Rows[0]["FAXNO"].ToString();
                                        //fd.Name = tbl.Rows[0]["GYOSYANM"].ToString();
                                        //fd.InterFaxFlag = C_FAXCSV.FaxData.InterFaxFlagValue.False;

                                        //C_FAXCSV faxCsv = new C_FAXCSV("CKKB0060", getFaxDataPath());
                                        //faxCsv.AddFaxData(fd);
                                        //if (faxCsv.FaxSend())
                                        //{
                                        //    MessageBox.Show("FAX送信データを作成しました。");
                                        //}
                                        //else
                                        //{
                                        //    MessageBox.Show("FAX送信データの作成に失敗しました。\nCSVデータ保存先 " + getFaxDataPath() + "を、ご確認ください。");
                                        //}
                                        //↑2013.08.21 kuwajima

                                        //↓2013.08.21 kuwajima
                                        faxController = new FaxController(axMcRemote1);
                                        faxController.SendFax(tbl.Rows[0]["FAXNO"].ToString(),
                                                              tbl.Rows[0]["GYOSYANM"].ToString(),
                                                              tbl.Rows[0]["TANTONM"].ToString(),
                                                              CRP_REP);
                                        //↑2013.08.21 kuwajima
                                        axMcRemote1.DisConnect();
                                    }
                                    else
                                    {
                                        //印刷部数：1部
                                        //出力先：プリンター
                                        //印刷開始：1部、部単位に印刷しない、全ページ印刷
                                        CRP_REP.PrintToPrinter(1, false, 0, 0);
                                    }
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("FAX送信データの作成に失敗しました。\nシステム管理者に連絡してください。");
                                }

                            }
                            else
                            {
                                //印刷部数：1部
                                //出力先：プリンター
                                //印刷開始：1部、部単位に印刷しない、全ページ印刷
                                CRP_REP.PrintToPrinter(1, false, 0, 0);
                            }
                        }
                        //↑2013.08.08 kuwajima 追加要望対応
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
                    if (CKSI0040_01.G_START_SIZAI_TANAOROSI_SYSTEM == "1")
                    {
                        this.Close();
                    }
                    else
                    {
                        CMD_Button[0].Enabled = true;
                        CMD_Button[1].Enabled = true;
                        CMD_Button[0].Focus();
                    }

                    //CMD_Button[0].Enabled = true;
                    //CMD_Button[1].Enabled = true;
                    //CMD_Button[0].Focus();
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.

                    break;
            }
        }

        private void setEventCtrlAry()
        {
            //コントロール配列にイベントを設定
            //CMD_Button
            for (short i = 0; i < this.CMD_Button.Count(); i++)
            {
                this.CMD_Button[i].Click += new EventHandler(CMD_Button_Click);
            }
        }

        private void FRM_CKSI0040M_Load(System.Object eventSender, System.EventArgs eventArgs)
        {
            string L_Search = null;
            string L_Command2 = null;

            //プロジェクト名を設定
            C_COMMON.G_COMMON_MSGTITLE = "CKSI0040";

            //コントロール配列にイベントを設定
            setEventCtrlAry();

            Show();

            //初期フォーカス設定
            TXT_NEN.Focus();

            //↓2013.08.08 kuwajima 追加要望対応
            //FAX送信用フォルダの初期化（３日経過したPDFファイルがあったら削除）
            //workFileDelete(3);
            //↑2013.08.08 kuwajima 追加要望対応

            //待機する秒数
            const short Waittime = 1;
            //起動時刻を待避
            System.DateTime now_old = DateTime.Now;

            //現在の時刻－起動時刻が待機する秒数になるまでループする
            while (!(DateTime.FromOADate(DateAndTime.Now.ToOADate() - Convert.ToDateTime(now_old).ToOADate()) >= DateAndTime.TimeSerial(0, 0, Waittime)))
            {
                System.Windows.Forms.Application.DoEvents();
            }

            //引数のチェック
            IPPAN.G_COMMAND = Interaction.Command();

            L_Search = ",";

            // コマンド ライン引数が指定されていないとき。
            if (string.IsNullOrEmpty(IPPAN.G_COMMAND))
            {
                //コマンド ライン引数が指定されていません
                IPPAN.Error_Msg("E507", 0, " ");
                System.Environment.Exit(0);
            }
            else
            {
// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
                string[] args = IPPAN.G_COMMAND.Split(',');
                CKSI0040_01.G_USERID = args[0];
                CKSI0040_01.G_OFFICEID = args[1];
                CKSI0040_01.G_SYOKUICD = args[2];
                if (args.Length >= 4)
                {
                    CKSI0040_01.G_START_SIZAI_TANAOROSI_SYSTEM = args[3];
                }
                //CKSI0040_01.G_USERID = Strings.Mid(IPPAN.G_COMMAND, 1, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) - 1);
                //L_Command2 = Strings.Mid(IPPAN.G_COMMAND, Strings.InStr(1, IPPAN.G_COMMAND, L_Search, CompareMethod.Binary) + 1, 30);
                //CKSI0040_01.G_OFFICEID = Strings.Mid(L_Command2, 1, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) - 1);
                //CKSI0040_01.G_SYOKUICD = Strings.Mid(L_Command2, Strings.InStr(1, L_Command2, L_Search, CompareMethod.Binary) + 1, 2);
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.
// 2017.02.01 yoshitake 副資材棚卸システム再構築 start.
                ////購買以外の時は副資材ユーザかどうかの判断
                //if (CKSI0040_01.G_OFFICEID != "KOUBAI")
                //{
                //    //ユーザーＩＤが副資材ユーザ以外かを見る
                //    IPPAN.G_RET = FS_USER_CHECK.User_Check(CKSI0040_01.G_USERID);
                //    if (IPPAN.G_RET != 0)
                //    {
                //        System.Environment.Exit(0);
                //    }
                //}
// 2017.02.01 yoshitake 副資材棚卸システム再構築 end.
                IPPAN.G_RET = IPPAN2.TIMER_SET(CKSI0040_01.G_USERID);
                if (IPPAN.G_RET != 0)
                {
                    System.Environment.Exit(0);
                }
            }

            //画面初期化
            GAMEN_CLEAR();

            //砂時計ポインタ
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_WAIT);
            //検収年月取得
            IPPAN.G_RET = GET_NENGETU();
            //標準ポインタ
            IPPAN.Pointer_Change(this, IPPAN.C_CURSOR_DEFAULT);
            if (IPPAN.G_RET != 0)
            {
                return;
            }
// 2017.02.01 yoshitake 副資材棚卸システム再構築 start.
            //if (CKSI0040_01.G_OFFICEID == "KOUBAI")
            //{
            //    //↓2013.08.08 kuwajima　追加要望対応
            //    //TXT_NEN.Text = Strings.Mid(CKSI0040_01.G_NENGETU1, 3, 2);
            //    TXT_NEN.Text = Strings.Mid(CKSI0040_01.G_NENGETU1, 1, 4);
            //    //↑2013.08.08 kuwajima　追加要望対応
            //    TXT_TUKI.Text = Strings.Mid(CKSI0040_01.G_NENGETU1, 5, 2);
            //}
            //else
            //{
// 2017.02.01 yoshitake 副資材棚卸システム再構築 end.
            //↓2013.08.08 kuwajima　追加要望対応
            //TXT_NEN.Text = Strings.Mid(CKSI0040_01.G_NENGETU, 3, 2);
            grpFaxSend.Visible = false;
            TXT_NEN.Text = Strings.Mid(CKSI0040_01.G_NENGETU, 1, 4);
            //↑2013.08.08 kuwajima　追加要望対応
            TXT_TUKI.Text = Strings.Mid(CKSI0040_01.G_NENGETU, 5, 2);
// 2017.02.01 yoshitake 副資材棚卸システム再構築 start.
            //}
// 2017.02.01 yoshitake 副資材棚卸システム再構築 end.
// 2016.12.12 yoshitake 副資材棚卸システム再構築 start.
            if (CKSI0040_01.G_START_SIZAI_TANAOROSI_SYSTEM != null)
            {
                if (CKSI0040_01.G_START_SIZAI_TANAOROSI_SYSTEM.Equals("1"))
                {
                    this._CMD_Button_0.Enabled = false;
                }
            }
// 2016.12.12 yoshitake 副資材棚卸システム再構築 end.
        }

        private void TMR_TIMER_Tick(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Control_Init(this, "検収明細書発行", "CKSI0040", SO.SO_USERNAME, SO.SO_OFFICENAME, "1.01");
        }

        private void TXT_GYOSYACD_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
            //入力前状態保存		
            CKSI0040_01.G_CD = TXT_GYOSYACD.Text;
        }

        private void TXT_GYOSYACD_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);

            TXT_GYOSYACD.Text = C_COMMON.FormatToNum(TXT_GYOSYACD.Text, "0000");

            //内容が変更されていて且つ空白じゃないとき
            if (CKSI0040_01.G_CD != TXT_GYOSYACD.Text && IPPAN.C_Allspace(TXT_GYOSYACD.Text) != 0)
            {
                //入力禁止文字チェック
                IPPAN.G_RET = IPPAN.Input_Check(TXT_GYOSYACD.Text);
                if (IPPAN.G_RET != 0)
                {
                    LBL_GYOSYANM.Text = "";
                    TXT_GYOSYACD.Focus();
                    TXT_GYOSYACD_Enter(TXT_GYOSYACD, new System.EventArgs());
                    //入力エラー
                    IPPAN.Error_Msg("E202", 0, " ");
                    return;
                }

                //業者名取得
                IPPAN.G_RET = GYOSYA_KENSAKU(TXT_GYOSYACD.Text);
                if (IPPAN.G_RET != 0)
                {
                    LBL_GYOSYANM.Text = "";
                    TXT_GYOSYACD.Focus();
                    TXT_GYOSYACD_Enter(TXT_GYOSYACD, new System.EventArgs());
                    //業者マスタが見つかりません
                    IPPAN.Error_Msg("E102", 0, " ");
                    return;
                }

                LBL_GYOSYANM.Text = CKSI0040_01.G_GYOSYANM;
            }
            else if (IPPAN.C_Allspace(TXT_GYOSYACD.Text) == 0)
            {
                //空白のとき
                LBL_GYOSYANM.Text = "";
            }
        }

        private void TXT_NEN_TextChanged(System.Object eventSender, System.EventArgs eventArgs)
        {
            //↓2013.08.08 kuwajima 追加要望対応
            //if (TXT_NEN.Visible == true && Strings.Len(TXT_NEN.Text) >= 2)
            if (TXT_NEN.Visible == true && Strings.Len(TXT_NEN.Text) >= 4)
            //↑2013.08.08 kuwajima 追加要望対応
            {
                TXT_TUKI.Focus();
            }
        }

        private void TXT_NEN_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void TXT_NEN_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);

            TXT_NEN.Text = IPPAN2.Numeric_Hensyu3(TXT_NEN.Text);
            //↓2013.08.08 kuwajima 追加要望対応
            //TXT_NEN.Text = C_COMMON.FormatToNum(TXT_NEN.Text, "00");
            TXT_NEN.Text = C_COMMON.DateYYChanged(TXT_NEN.Text.Trim());
            //↑2013.08.08 kuwajima 追加要望対応
        }

        private void TXT_TUKI_Enter(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Got_Text(this.ActiveControl, IPPAN.C_CONTROL_COLOR_ENTER);
        }

        private void TXT_TUKI_Leave(System.Object eventSender, System.EventArgs eventArgs)
        {
            IPPAN.Lost_Text(IPPAN.C_CONTROL_COLOR_LEAVE);

            TXT_TUKI.Text = IPPAN2.Numeric_Hensyu3(TXT_TUKI.Text);
            TXT_TUKI.Text = C_COMMON.FormatToNum(TXT_TUKI.Text, "00");

            if (IPPAN.C_Allspace(TXT_TUKI.Text) != 0)
            {
                if ((this.Visible == true) && (Convert.ToInt16(TXT_TUKI.Text) < 1 || Convert.ToInt16(TXT_TUKI.Text) > 12))
                {
                    TXT_TUKI.Focus();
                    TXT_TUKI_Enter(TXT_TUKI, new System.EventArgs());
                    //日付入力エラー
                    IPPAN.Error_Msg("E201", 0, " ");
                    return;
                }
            }
        }
        //↓2013.08.08 kuwajima 追加要望対応
        /// <summary>
        /// FAX送信データの保存先を取得
        /// </summary>
        /// <returns></returns>
        private string getFaxDataPath()
        {
            try
            {
                string myTalkCsvPath = string.Empty;

                string L_File = null;
                string L_RECORD = null;
                int L_FileNo = 0;
                int L_SepPos = 0;
                FileInfo finfo = new FileInfo(Application.ExecutablePath);
                L_File = finfo.Directory + "\\CKSI0040.INI";

                L_FileNo = FileSystem.FreeFile();
                FileSystem.FileOpen(L_FileNo, L_File, OpenMode.Input, OpenAccess.Default, OpenShare.Shared, -1);

                //初期値ファイルが終了まで繰り返し
                while (!FileSystem.EOF(L_FileNo))
                {
                    //初期値ファイル読み込み
                    FileSystem.Input(L_FileNo, ref L_RECORD);

                    //コメント行の判定
                    if ((Strings.Mid(L_RECORD, 1, 1) == "[") || (Strings.Mid(L_RECORD, 1, 1) == ";"))
                    {
                        //分割位置に0を設定
                        L_SepPos = 0;
                    }
                    else
                    {
                        //分割位置を設定
                        L_SepPos = Strings.InStr(L_RECORD, "=", CompareMethod.Binary);
                    }

                    if ((L_SepPos >= 1))
                    {
                        switch (Strings.Mid(L_RECORD, 1, L_SepPos - 1))
                        {
                            case "MyTalkCsvPath":
                                //FAX外部連携用フォルダ
                                myTalkCsvPath = Strings.Mid(L_RECORD, L_SepPos + 1, Strings.Len(L_RECORD));
                                break;
                            default:

                                break;
                        }
                    }

                }

                //初期値ファイルクローズ
                FileSystem.FileClose(L_FileNo);

                return myTalkCsvPath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// FAX送信デバイス名を取得
        /// </summary>
        /// <returns></returns>
        private string getFaxDeviceName()
        {
            try
            {
                string faxDeviceName = string.Empty;

                string L_File = null;
                string L_RECORD = null;
                int L_FileNo = 0;
                int L_SepPos = 0;
                FileInfo finfo = new FileInfo(Application.ExecutablePath);
                L_File = finfo.Directory + "\\CKSI0040.INI";

                L_FileNo = FileSystem.FreeFile();
                FileSystem.FileOpen(L_FileNo, L_File, OpenMode.Input, OpenAccess.Default, OpenShare.Shared, -1);

                //初期値ファイルが終了まで繰り返し
                while (!FileSystem.EOF(L_FileNo))
                {
                    //初期値ファイル読み込み
                    FileSystem.Input(L_FileNo, ref L_RECORD);

                    //コメント行の判定
                    if ((Strings.Mid(L_RECORD, 1, 1) == "[") || (Strings.Mid(L_RECORD, 1, 1) == ";"))
                    {
                        //分割位置に0を設定
                        L_SepPos = 0;
                    }
                    else
                    {
                        //分割位置を設定
                        L_SepPos = Strings.InStr(L_RECORD, "=", CompareMethod.Binary);
                    }

                    if ((L_SepPos >= 1))
                    {
                        switch (Strings.Mid(L_RECORD, 1, L_SepPos - 1))
                        {
                            case "FAXDeviceName":
                                //マイト～クＦＡＸのデバイス名
                                faxDeviceName = Strings.Mid(L_RECORD, L_SepPos + 1, Strings.Len(L_RECORD));
                                break;
                            default:

                                break;
                        }
                    }

                }

                //初期値ファイルクローズ
                FileSystem.FileClose(L_FileNo);

                return faxDeviceName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// PDFファイルの保存フォルダを取得する。
        /// </summary>
        /// <returns></returns>
        private string getPdfPath()
        {
            FileInfo finfo = new FileInfo(Application.ExecutablePath);
            return finfo.Directory + "\\CKSI0040Work";
        }

        /// <summary>
        /// 指定した日付を経過したFAX送信ファイルを削除
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        private void workFileDelete(int day)
        {
            string workDir = getPdfPath();

            if (Directory.Exists(workDir))
            {
                DateTime deleteDate = DateTime.Now.AddDays(day * -1);
                string[] files = Directory.GetFiles(workDir, "*.pdf", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    if (File.GetCreationTime(files[i]) < deleteDate)
                    {
                        //指定期間より古いPDFファイルを削除
                        File.Delete(files[i]);
                    }
                }
            }
            else
            {
                //フォルダが無い場合は、フォルダを作成
                DirectoryInfo di = Directory.CreateDirectory(workDir);
            }
        }
        //↑2013.08.08 kuwajima 追加要望対応
    }
}
