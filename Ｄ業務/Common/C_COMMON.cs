using System.Windows.Forms;
using System;
// 2015.01.22 yoshitake OBIC7仕訳連携対応(step3) start.
using System.Data;
using System.Data.Odbc;
// 2015.01.22 yoshitake OBIC7仕訳連携対応(step3) end.

namespace Project1
{
    /// <summary>
    /// 共通関数を記述(主にコンバート差分の吸収)
    /// </summary>
    public class C_COMMON
    {
        /// <summary>
        /// メッセージのタイトル(プロジェクト名)
        /// </summary>
        public static string G_COMMON_MSGTITLE = string.Empty;

        /// <summary>
        /// CrystalReport接続文字列
        /// </summary>
        public static string G_CRYSTALREPORT_CONNECT = "DSN=so_oracle;UID=SA;PWD=SA;DSQ=GM";

        /// <summary>
        /// カルチャ識別子：日本語
        /// </summary>
        public static int G_CMN_LOCALID_JP = 0x0411;

        /// <summary>
        /// メッセージ出力
        /// </summary>
        /// <param name="sMsg">出力するメッセージ</param>
        /// <returns>結果</returns>
        public static DialogResult Msg(string sMsg)
        {
            return MessageBox.Show(sMsg, G_COMMON_MSGTITLE, MessageBoxButtons.OK);
        }

        /// <summary>
        /// メッセージ出力
        /// </summary>
        /// <param name="sMsg">出力するメッセージ</param>
        /// <param name="sTitle">タイトル</param>
        /// <returns>結果</returns>
        public static DialogResult Msg(string sMsg, string sTitle)
        {
            return MessageBox.Show(sMsg, sTitle, MessageBoxButtons.OK);
        }

        /// <summary>
        /// メッセージ出力
        /// </summary>
        /// <param name="sMsg">出力するメッセージ</param>
        /// <param name="btnType">ボタン形式</param>
        /// <returns>結果</returns>
        public static DialogResult Msg(string sMsg, MessageBoxButtons btnType)
        {
            return MessageBox.Show(sMsg, G_COMMON_MSGTITLE, btnType);
        }

        /// <summary>
        /// メッセージ出力
        /// </summary>
        /// <param name="sMsg">出力するメッセージ</param>
        /// <param name="btnType">ボタン形式</param>
        /// <param name="sTitle">タイトル</param>
        /// <returns>結果</returns>
        public static DialogResult Msg(string sMsg, MessageBoxButtons btnType, string sTitle)
        {
            return MessageBox.Show(sMsg, sTitle, btnType);
        }

        /// <summary>
        /// メッセージ出力
        /// </summary>
        /// <param name="sMsg">出力するメッセージ</param>
        /// <param name="btnType">ボタン形式</param>
        /// <param name="icnType">アイコン形式</param>
        /// <returns></returns>
        public static DialogResult Msg(string sMsg, MessageBoxButtons btnType, MessageBoxIcon icnType)
        {
            return MessageBox.Show(sMsg, G_COMMON_MSGTITLE, btnType, icnType);
        }

        /// <summary>
        /// 書式設定(数値変換)
        /// </summary>
        /// <param name="sStr">対象文字列</param>
        /// <param name="sFmt">書式</param>
        /// <returns>結果</returns>
        public static string FormatToNum(string sStr, string sFmt)
        {
            string sRet = string.Empty;
            decimal num = 0;
            if (decimal.TryParse(sStr, out num))
            {
                sRet = string.Format("{0:" + sFmt + "}", num);
            }
            else
            {
                sRet = string.Format("{0:" + sFmt + "}", sStr);
            }
            return sRet;
        }

        /// <summary>
        /// decimal変換処理
        /// </summary>
        /// <param name="sStr">対象文字列</param>
        /// <returns>結果</returns>
        public static decimal ToDecimal(string sStr)
        {
            decimal num = 0;
            if (string.IsNullOrEmpty(sStr))
            {
                //変換不能の場合、0を返す
                return 0;
            }

            if (decimal.TryParse(sStr.Trim(), out num))
            {
                //変換可能の場合、変換したものを返す
                return num;
            }

            //変換不能の場合、0を返す
            return 0;
        }

        /// <summary>
        /// 数値変換前文字列チェック処理
        /// </summary>
        /// <param name="sStr">対象文字列</param>
        /// <returns>結果</returns>
        public static string ChkStrToNum(string sStr)
        {
            //引数の文字列が数値変換不能の場合、
            //変換時の例外発生を防ぐため0を返す。
            string sRet = "0";
            decimal num = 0;
            if (decimal.TryParse(sStr, out num))
            {
                //変換可能の場合、そのまま使用する
                sRet = sStr;
            }
            return sRet;
        }
        /// <summary>
        /// 文字列の左端から指定したバイト数分の文字列を返します。
        /// </summary>
        /// <param name="stTarget">取り出す元になる文字列</param>
        /// <param name="iByteSize">取り出すバイト数</param>
        /// <returns>左端から指定されたバイト数分の文字列</returns>
        public static string LeftB(string stTarget, int iByteSize)
        {
            System.Text.Encoding hEncoding = System.Text.Encoding.GetEncoding("Shift_JIS");
            byte[] btBytes = hEncoding.GetBytes(stTarget);

            return hEncoding.GetString(btBytes, 0, iByteSize);
        }

        /// <summary>
        /// 半角 1 バイト、全角 2 バイトとして、指定された文字列のバイト数を返します。
        /// </summary>
        /// <param name="stTarget">バイト数取得の対象となる文字列</param>
        /// <returns>半角 1 バイト、全角 2 バイトでカウントされたバイト数</returns>
        public static int LenB(string stTarget)
        {
            return System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(stTarget);
        }

        /// -----------------------------------------------------------------------------------------
        /// <summary>
        ///     文字列の指定されたバイト位置以降のすべての文字列を返します。</summary>
        /// <param name="stTarget">
        ///     取り出す元になる文字列。</param>
        /// <param name="iStart">
        ///     取り出しを開始する位置。</param>
        /// <returns>
        ///     指定されたバイト位置以降のすべての文字列。</returns>
        /// -----------------------------------------------------------------------------------------
        public static string MidB(string stTarget, int iStart)
        {
            System.Text.Encoding hEncoding = System.Text.Encoding.GetEncoding("Shift_JIS");
            byte[] btBytes = hEncoding.GetBytes(stTarget);

            return hEncoding.GetString(btBytes, iStart - 1, btBytes.Length - iStart + 1);
        }

        /// -----------------------------------------------------------------------------------------
        /// <summary>
        ///     文字列の指定されたバイト位置から、指定されたバイト数分の文字列を返します。</summary>
        /// <param name="stTarget">
        ///     取り出す元になる文字列。</param>
        /// <param name="iStart">
        ///     取り出しを開始する位置。</param>
        /// <param name="iByteSize">
        ///     取り出すバイト数。</param>
        /// <returns>
        ///     指定されたバイト位置から指定されたバイト数分の文字列。</returns>
        /// -----------------------------------------------------------------------------------------
        public static string MidB(string stTarget, int iStart, int iByteSize)
        {
            System.Text.Encoding hEncoding = System.Text.Encoding.GetEncoding("Shift_JIS");
            byte[] btBytes = hEncoding.GetBytes(stTarget);

            return hEncoding.GetString(btBytes, iStart - 1, iByteSize);
        }

        //複数個所で使用されていたので移植
        /// <summary>
        /// 金額右詰編集処理
        /// </summary>
        /// <param name="Kingaku">編集前金額(文字型)</param>
        /// <param name="Hensyu">編集フォーマット(VB形式)</param>
        /// <param name="Keta">編集後の項目の桁数</param>
        /// <returns></returns>
        public static string MIGIDUME(string Kingaku, string Hensyu, int Keta)
        {
            string L_STR1 = string.Empty;

            L_STR1 = C_COMMON.FormatToNum(Kingaku, Hensyu);
            L_STR1 = L_STR1.Trim();

            return L_STR1.PadLeft(Keta);
        }

        /// <summary>
        /// 　　string型をint型に変換します。
        /// 　　スペース、nullの場合には「0」を返します。
        /// </summary>
        /// <param name="stTarget"></param>
        /// <returns></returns>
        public static int StrintToInt(string stTarget)
        {
            if ((stTarget.Trim() == string.Empty)　|| (stTarget == null))
            {
                return 0;
            }
            else
            {
                return int.Parse(stTarget);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stYY"></param>
        /// <returns></returns>
        public static string DateYYChanged(string stYY)
        {
            string stYYYY = string.Empty;

            if ((stYY.Length >= 1) &&
                (stYY.Length <= 3))
            {
                stYYYY = C_COMMON.FormatToNum(stYY, "2000");
            }
            else
            {
                stYYYY = stYY;
            }
            return stYYYY;
        }
        public static string[] StringSplitB(string stTarget, int spByte)
        {
            string tmpstr = string.Empty;
            string[] sRet = new string[2];
            sRet[0] = string.Empty;
            sRet[1] = string.Empty;

            if (string.IsNullOrEmpty(stTarget))
            {
                return sRet;
            }
            if (spByte < 1 || spByte >= LenB(stTarget))
            {
                sRet[0] = stTarget;

                return sRet;
            }

            tmpstr = MidB(stTarget, spByte, 2);
            if (tmpstr.Length == 1)
            {
                sRet[0] = MidB(stTarget, 1, spByte-1);
                sRet[1] = MidB(stTarget, spByte);
            }
            else
            {
                sRet[0] = MidB(stTarget, 1, spByte);
                sRet[1] = MidB(stTarget, spByte + 1);
            }
           return sRet;
        }
// 2015.01.22 yoshitake OBIC7仕訳連携対応(step3) start.
        /// <summary>
        /// 工程名称が存在するかを確認
        /// </summary>
        /// <param name="kouteiCD">工程CD</param>
        /// <returns>結果の値(有：0 無：1)</returns>
        public static int CheckKouteiNM(string kouteiCD)
        {
            //結果
            int nRet = 1;
            DataTable tbl = null;

            //工程マスタの検索
            IPPAN.G_SQL = "SELECT KOUTEINM FROM KOUTEI_MST ";
            IPPAN.G_SQL += "WHERE KOUTEICD = '" + kouteiCD + "'";

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
                    db.Error();
                }
            }

            // 結果の列数を取得する
            if ((tbl.Columns.Count >= 1) && (tbl.Rows.Count > 0))
            {
                //データがある
                nRet = 0;
            }
            return nRet;
        }

        /// <summary>
        /// 工程マスタから工程名称を取得
        /// </summary>
        /// <param name="kouteiCD">工程CD</param>
        /// <param name="kouteiNM">工程名称</param>
        /// <returns>結果の値(取得OK：0 取得NG : 1)</returns>
        public static int GetKouteiNM(string kouteiCD, out string kouteiNM)
        {
            //結果
            int nRet = 1;
            kouteiNM = string.Empty;
            DataTable tbl = null;

            //工程マスタの検索
            IPPAN.G_SQL = "SELECT KOUTEINM FROM KOUTEI_MST ";
            IPPAN.G_SQL += "WHERE KOUTEICD = '" + kouteiCD + "'";

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
                    db.Error();
                }
            }

            // 結果の列数を取得する
            if (tbl.Columns.Count < 1)
            {
                return nRet;
            }

            //データがない
            if (tbl.Rows.Count == 0)
            {
                return nRet;
            }

            //工程名称取得
            kouteiNM = tbl.Rows[0][0].ToString();

            nRet = 0;
            return nRet;
        }
    }
// 2015.01.22 yoshitake OBIC7仕訳連携対応(step3) end.
}
