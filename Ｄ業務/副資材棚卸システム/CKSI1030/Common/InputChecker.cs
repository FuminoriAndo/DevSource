using System;
//*************************************************************************************
//
//   入力データのチェッククラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.02.01              DSK   　     新規作成
//
//*************************************************************************************
namespace Common
{
    /// <summary>
    /// 入力データのチェック
    /// </summary>
    internal static class InputChecker
    {
        #region フィールド
        /// <summary>
        /// システム分類の最大文字数
        /// </summary>
        private const int SystemCategory_MaxLength = 4;

        /// <summary>
        /// システム分類名の最大バイト数
        /// </summary>
        private const int SystemCategoryName_MaxByte = 30;

        #endregion

        #region メソッド
        /// <summary>
        /// システム分類チェック
        /// </summary>
        /// <param name="systemCategory">システム分類</param>
        /// <returns>チェック結果</returns>
        internal static string CheckSystemCategory(string systemCategory)
        {
            string result = string.Empty;

            if (String.IsNullOrEmpty(systemCategory))
            {
                result = "NoSystemCategoryValue";
            }

            return result;
        }


        /// <summary>
        /// システム分類名のチェック
        /// </summary>
        /// <param name="systemCategoryName">システム分類名</param>
        /// <returns>チェック結果</returns>
        internal static string CheckSystemCategoryName(string systemCategoryName)
        {
            string result = string.Empty;

            if (String.IsNullOrEmpty(systemCategoryName))
            {
                result = "NoSystemCategoryNameValue";
            }
            else
            {
                System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("Shift_JIS");
                byte[] btBytes = encoding.GetBytes(systemCategoryName.TrimEnd());

                int length = btBytes.Length;
                if (SystemCategoryName_MaxByte < length)
                {
                    result = "InvalidNoSystemCategoryNameLength";
                }
            }

            return result;
        }

        #endregion
    }
}