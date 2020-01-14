using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//*************************************************************************************
//
//   入力データのチェッククラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   15.08.08              DSK吉武   　 新規作成
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
        /// グループ区分名の最大バイト数
        /// </summary>
        private const int GroupKbnName_MaxByte = 30;

        #endregion

        #region メソッド
        /// <summary>
        /// グループ区分チェック
        /// </summary>
        /// <param name="groupKbn">グループ区分</param>
        /// <returns>チェック結果</returns>
        internal static string CheckGroupKbn(string groupKbn)
        {
            string result = string.Empty;

            if (String.IsNullOrEmpty(groupKbn))
            {
                result = "NoGroupKbnValue";
            }

            return result;
        }


        /// <summary>
        /// グループ区分名のチェック
        /// </summary>
        /// <param name="groupKbnName">グループ区分名</param>
        /// <returns>チェック結果</returns>
        internal static string CheckGroupKbnName(string groupKbnName)
        {
            string result = string.Empty;

            if (String.IsNullOrEmpty(groupKbnName))
            {
                result = "NoGroupKbnNameValue";
            }
            else
            {
                System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("Shift_JIS");
                byte[] btBytes = encoding.GetBytes(groupKbnName.TrimEnd());

                int length = btBytes.Length;
                if (GroupKbnName_MaxByte < length)
                {
                    result = "InvalidNoGroupKbnNameLength";
                }
            }

            return result;
        }

        #endregion
    }
}