//*************************************************************************************
//
//   入力チェッククラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

using System.Text;

namespace CKSI1010.Common
{
    /// <summary>
    /// 入力チェッククラス
    /// </summary>
    internal class InputChecker
    {
        #region フィールド

        /// <summary>
        /// 品目名の入力可能な最大バイト
        /// </summary>
        private const int HinmokuName_Max_Byte = 40;

        /// <summary>
        /// 業者名の入力可能な最大バイト
        /// </summary>
        private const int GyosyaName_Max_Byte = 24;

        /// <summary>
        /// 備考の入力可能な最大バイト
        /// </summary>
        private const int Bikou_Max_Byte = 40;

        /// <summary>
        /// 品目
        /// </summary>
        internal const string Hinmoku_StringDefine = "Hinmoku";

        /// <summary>
        /// 業者
        /// </summary>
        internal const string Gyosya_StringDefine = "Gyosya";

        /// <summary>
        /// 正常
        /// </summary>
        internal const int Err_Check_Str_Success = 0;

        /// <summary>
        /// 文字列不一致
        /// </summary>
        internal const int Err_Check_Str_Unmatch = 1;

        /// <summary>
        /// 文字列超過
        /// </summary>
        internal const int Err_Check_Str_Over = 2;

        #endregion

        #region メソッド

        /// <summary>
        /// 文字列が英数字かどうかを判定します
        /// </summary>
        /// <remarks>大文字・小文字を区別しません</remarks>
        /// <param name="target">対象の文字列</param>
        /// <returns>文字列が英数字の場合はtrue、それ以外はfalse</returns>
        internal static int CheckString(string target)
        {
            var regex = new System.Text.RegularExpressions.Regex("^[0-9a-zA-Z]+$");

            if (!regex.IsMatch(target))
            {
                return Err_Check_Str_Unmatch;
            }

            if (target.Length != 4)
            {
                return Err_Check_Str_Over;
            }

            return 0;

        }

        /// <summary>
        /// 品目名の入力チェック
        /// </summary>
        /// <param name="target">品目名</param>
        /// <returns>チェック結果</returns>
        internal static int CheckHinmokuName(string target)
        {
            int ret = Err_Check_Str_Success;

            int length = Encoding.GetEncoding("Shift_JIS").GetByteCount(target.Trim());
            if (length > HinmokuName_Max_Byte)
            {
                ret = Err_Check_Str_Over;
            }

            return ret;
        }

        /// <summary>
        /// 業者名の入力チェック
        /// </summary>
        /// <param name="target">業者名</param>
        /// <returns>チェック結果</returns>
        internal static int CheckGyosyaName(string target)
        {
            int ret = Err_Check_Str_Success;

            int length = Encoding.GetEncoding("Shift_JIS").GetByteCount(target.Trim());
            if (length > GyosyaName_Max_Byte)
            {
                ret = Err_Check_Str_Over;
            }

            return ret;
        }

        /// <summary>
        /// 備考の入力チェック
        /// </summary>
        /// <param name="target">備考</param>
        /// <returns>チェック結果</returns>
        internal static int CheckBikou(string target)
        {
            int ret = Err_Check_Str_Success;

            int length = Encoding.GetEncoding("Shift_JIS").GetByteCount(target.Trim());
            if (length > Bikou_Max_Byte)
            {
                ret = Err_Check_Str_Over;
            }

            return ret;
        }

        #endregion
    }
}