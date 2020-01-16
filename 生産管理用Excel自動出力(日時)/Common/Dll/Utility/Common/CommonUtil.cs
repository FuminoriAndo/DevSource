using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Security.AccessControl;
using System.Security.Principal;
//*************************************************************************************
//
//   共通ユーティリティ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************

namespace Utility.Common
{
    /// <summary>
    /// 共通ユーティリティ
    /// </summary>
    public class CommonUtil
    {
        #region メソッド

        /// <summary>
        /// エンコーディングされたバイト数の取得
        /// </summary>
        /// <param name="stTarget">対象文字列</param>
        /// <param name="encoding">エンコーディング</param>
        /// <returns>バイト数</returns>
        public static int GetByteCount(string stTarget, string encoding)
        {
            try
            {
                // エンコーディングされたバイト数の取得
                Encoding encode = Encoding.GetEncoding(encoding);
                return encode.GetByteCount(stTarget);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 文字列の指定されたバイト位置から、指定されたバイト数分の文字列を返す
        /// </summary>
        /// <param name="stTarget">
        ///     取り出す元になる文字列</param>
        /// <param name="start">
        ///     取り出しを開始する位置</param>
        /// <param name="byteSize">
        ///     取り出すバイト数</param>
        /// <param name="encoding">
        ///     エンコーディング</param>
        /// <returns>
        ///     指定されたバイト位置から指定されたバイト数分の文字列</returns>
        public static string GetString(string stTarget, int start, int byteSize, string encoding)
        {
            try
            {
                Encoding encode = Encoding.GetEncoding(encoding);
                byte[] btBytes = encode.GetBytes(stTarget);
                return encode.GetString(btBytes, start, byteSize);
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}