using System;
using System.Data;
using System.Data.Odbc;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Project1
{
    static class IPPAN4
    {
        //*************************************************************************************
        //
        //
        //   プログラム名　　　一般関数
        //
        //
        //   修正履歴
        //
        //   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
        //   13.07.09             HIT桑島     新規作成
        //
        //*************************************************************************************

        [DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int WNetCancelConnection2(string lpName, Int32 dwFlags, bool fForce);     
        [DllImport("mpr.dll", EntryPoint = "WNetAddConnection2", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]    
        private static extern int WNetAddConnection2(ref NETRESOURCE lpNetResource, string lpPassword, string lpUsername, Int32 dwFlags);
        [StructLayout(LayoutKind.Sequential)]    
        internal struct NETRESOURCE    
        {
            public int dwScope;
            public int dwType;
            public int dwDisplayType;
            public int dwUsage;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpLocalName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpRemoteName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpComment;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpProvider;
        }

        /// <summary>
        /// 共有フォルダへ認証を通す
        /// </summary>
        /// <param name="shareName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool FolderAuthentication(string shareName, string userName, string password)
        {

            if (userName.Trim() == string.Empty)
            {
                //ユーザー名が設定されてない場合は、認証しない
                return true;
            }

            // 接続情報を設定
            NETRESOURCE netResource = new NETRESOURCE();
            netResource.dwScope = 0;
            netResource.dwType = 1;
            netResource.dwDisplayType = 0;
            netResource.dwUsage = 0;
            netResource.lpLocalName = "";
            netResource.lpRemoteName = shareName;
            netResource.lpProvider = "";
            int ret = 0;
            try
            {                
                //認証解除
                ret = WNetCancelConnection2(shareName, 0, true);
                //共有フォルダに接続
                if (WNetAddConnection2(ref netResource, password, userName, 0) != 0)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cdnm"></param>
        /// <returns></returns>
        public static string getNM(string cdnm)
        {
            if (cdnm == null)
            {
                cdnm = string.Empty;
            }
            cdnm = cdnm.Trim();
            if (cdnm == string.Empty)
            {
                return cdnm;
            }

            int i = cdnm.IndexOf(" ");
            if (i == 0)
            {
                return string.Empty;
            }
            return cdnm.Substring(i + 1, cdnm.Length - i - 1);
        }


        /// <summary>
        /// 支払条件のコンボボックスへアイテムを設定
        /// </summary>
        /// <returns></returns>
        public static int JyokenComboInit(ComboBox cmb)
        {
            DataTable tbl = GetJyokenMst();

            // データがない
            if (tbl == null)
            {
                return 1;
            }

            cmb.DataSource = tbl;
            cmb.ValueMember = "JYOKENCD";
            cmb.DisplayMember = "JYOKENNM";

            return 0;
        }

        /// <summary>
        /// 費目コードのコンボボックスへアイテムを設定
        /// </summary>
        /// <param name="kamokucd"></param>
        /// <returns></returns>
        public static int HimokuComboInit(ComboBox cmb, string kamokucd)
        {
            DataTable tbl = GetHimokuMst(kamokucd);

            // データがない
            if (tbl == null)
            {
                return 1;
            }

            cmb.DataSource = tbl;
            cmb.ValueMember = "HIMOKUCD";
            cmb.DisplayMember = "HIMOKUNM";

            return 0;
        }

        /// <summary>
        /// 費目内訳コードのコンボボックスへアイテムを設定
        /// </summary>
        /// <param name="kamokucd"></param>
        /// <returns></returns>
        public static int HimokuUtiwakeComboInit(ComboBox cmb, string kamokucd, string meisaicd1)
        {
            DataTable tbl = GetHimokuUtiwakeMst(kamokucd, meisaicd1);

            // データがない
            if (tbl == null)
            {
                return 1;
            }

            cmb.DataSource = tbl;
            cmb.ValueMember = "MEISAICD2";
            cmb.DisplayMember = "MEISAINM";

            return 0;
        }

        /// <summary>
        /// 条件マスタから全レコードをデータテーブルで取得する。
        /// </summary>
        /// <returns></returns>
        private static DataTable GetJyokenMst()
        {
            DataTable tbl = null;

            IPPAN.G_SQL = "SELECT JYOKENCD, CONCAT(CONCAT(JYOKENCD, ' '), JYOKENNM) AS JYOKENNM FROM JYOKEN_MST";

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
                catch (Exception)
                {
                    return null;
                }
            }

            return tbl;
        }

        /// <summary>
        /// 費目マスタをデータテーブルで取得する。
        /// </summary>
        /// <param name="kamokucd"></param>
        /// <returns></returns>
        private static DataTable GetHimokuMst(string kamokucd)
        {
            DataTable tbl = null;

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT H.HIMOKUCD, CONCAT(CONCAT(H.HIMOKUCD, ' '), H.HIMOKUNM) AS HIMOKUNM ");
            sql.AppendLine("FROM HIMOKU_MST H ");
            sql.AppendLine("  INNER JOIN HIMOKU_UTIWAKE_MST HU ");
            sql.AppendLine("  ON H.HIMOKUCD = HU.MEISAICD1 ");
            sql.AppendLine("WHERE HU.KAMOKUCD = '" + kamokucd + "' ");

            IPPAN.G_SQL = sql.ToString();

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
                catch (Exception)
                {
                    return null;
                }
            }

            return tbl;
        }

        /// <summary>
        /// 費目内訳マスタをデータテーブルで取得する。
        /// </summary>
        /// <param name="kamokucd"></param>
        /// <returns></returns>
        private static DataTable GetHimokuUtiwakeMst(string kamokucd, string meisaiCD1)
        {
            DataTable tbl = null;

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT MEISAICD2, CONCAT(CONCAT(MEISAICD2, ' '), MEISAINM) AS MEISAINM ");
            sql.AppendLine("FROM HIMOKU_UTIWAKE_MST ");
            sql.AppendLine("WHERE KAMOKUCD = '" + kamokucd + "' ");
            sql.AppendLine(" AND MEISAICD1 = '" + meisaiCD1 + "' ");

            IPPAN.G_SQL = sql.ToString();

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
                catch (Exception)
                {
                    return null;
                }
            }

            return tbl;
        }

    }
}
