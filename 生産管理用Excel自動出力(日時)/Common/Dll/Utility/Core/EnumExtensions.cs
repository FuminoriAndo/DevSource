using System;
using System.Collections.Generic;
//*************************************************************************************
//
//   Enum拡張メソッド
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************

namespace Utility.Core
{
    /// <summary>
    /// Enum拡張メソッド
    /// </summary>
    /// <typeparam name="T">任意のオブジェクトの型</typeparam>
    public static class EnumExtensions<T>
    {
        #region メソッド

        /// <summary>
        /// Enum定義をForEachに渡すためのヘルパクラスを返却する
        /// </summary>
        /// <returns>Enum定義をForEachに渡すためのヘルパクラス</returns>
        public static EnumerateEnum Enumerate()
        {
            return new EnumerateEnum();
        }

        #endregion

        #region 内部クラス

        /// <summary>
        /// ForEach用のEnumerator取得メソッドを持つヘルパクラス
        /// </summary>
        public class EnumerateEnum
        {
            /// <summary>
            /// Enumeratorの取得
            /// </summary>
            /// <returns>Enumerator</returns>
            public IEnumerator<T> GetEnumerator()
            {
                foreach (T e in Enum.GetValues(typeof(T)))
                    yield return e;
            }
        }

        #endregion
    }
}