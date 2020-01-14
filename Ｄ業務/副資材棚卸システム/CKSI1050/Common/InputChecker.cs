using System;
//*************************************************************************************
//
//   入力データのチェッククラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
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
        /// 棚卸操作名の最大バイト数
        /// </summary>
        private const int TanaorosiOperationName_MaxByte = 60;

        #endregion

        #region メソッド

        /// <summary>
        /// システム分類のチェック
        /// </summary>
        /// <param name="systemCategory">システム分類</param>
        /// <returns>チェック結果</returns>
        internal static string CheckSystemCategory(string systemCategory)
        {
            string result = string.Empty;

            if (String.IsNullOrEmpty(systemCategory))
            {
                result = "NotSelectSystemCategory";
            }

            return result;
        }

        /// <summary>
        /// 棚卸操作コードチェック
        /// </summary>
        /// <param name="operationCode">棚卸操作コード</param>
        /// <returns>チェック結果</returns>
        internal static string CheckTanaorosiOperationCode(string operationCode)
        {
            string result = string.Empty;

            if (String.IsNullOrEmpty(operationCode))
            {
                result = "NoTanaorosiOperationCodeValue";
            }

            return result;
        }

        /// <summary>
        /// 棚卸操作名のチェック
        /// </summary>
        /// <param name="operationName">棚卸操作名</param>
        /// <returns>チェック結果</returns>
        internal static string CheckTanaorosiOperationName(string operationName)
        {
            string result = string.Empty;

            if (String.IsNullOrEmpty(operationName))
            {
                result = "NoTanaorosiOperationNameValue";
            }
            else
            {
                System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("Shift_JIS");
                byte[] btBytes = encoding.GetBytes(operationName.TrimEnd());

                int length = btBytes.Length;
                if (TanaorosiOperationName_MaxByte < length)
                {
                    result = "InvalidTanaorosiOperaionNameLength";
                }
            }

            return result;
        }

        #endregion
    }
}