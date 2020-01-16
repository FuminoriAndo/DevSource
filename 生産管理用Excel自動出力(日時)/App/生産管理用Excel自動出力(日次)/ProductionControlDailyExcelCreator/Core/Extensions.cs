//*************************************************************************************
//
//   拡張メソッド
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using System.IO;
using Utility.Types;

namespace ProductionControlDailyExcelCreator.Core
{
    /// <summary>
    /// 拡張メソッド
    /// </summary>
    internal static class Extensions
    {
        #region メソッド

        /// <summary>
        /// Excelワークブック名の変換
        /// </summary>
        /// <param name="source">変換前(文字列)</param>
        /// <param name="dateTimeNow">現在時刻</param>
        /// <returns>変換後(文字列)</returns>
        internal static string ConvertExcelWorkBookName(this string source, DateTime dateTimeNow)
        {
            try
            {
                return Path.GetFileNameWithoutExtension(source)
                    + SymbolType.Underscore.GetValue()
                    + dateTimeNow.ToString("yyyyMMddHHmm")
                    + Path.GetExtension(source);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 文字列末尾から部分文字列を取得する
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <param name="length">部分文字列の長さ</param>
        /// <returns>文字列の右から抽出された部分文字列</returns>
        internal static string SubstringRight(this string target, int length)
        {
            try
            {
                return target.Substring(target.Length - length, length);
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion

    }
}