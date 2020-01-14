using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Common;
//*************************************************************************************
//
//   拡張メソッド
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace Core
{
    /// <summary>
    /// 拡張メソッド
    /// </summary>
    internal static class Extensions
    {
        #region メソッド

        /// <summary>
        /// IEnumerable型からObservableCollection型に変換する
        /// </summary>
        /// <typeparam name="T">コレクションが管理するオブジェクトの型</typeparam>
        /// <param name="collection">IEnumerable型のオブジェクト</param>
        /// <returns>ObservableCollection型のオブジェクト</returns>
        internal static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }

        /// <summary>
        /// IEnumerable型のコレクションの要素を追加する
        /// </summary>
        /// <typeparam name="T">コレクションが管理するオブジェクトの型</typeparam>
        /// <param name="collection">追加先のコレクション</param>
        /// <param name="source">追加元のコレクション</param>
        /// <returns>追加後のコレクション</returns>
        internal static ICollection<T> AddRange<T>(this ICollection<T> collection, IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                collection.Add(item);
            }
            return collection;
        }

        /// <summary>
        /// システム分類のNameを取得する
        /// <summary>
        internal static string GetName(this Constants.SystemCategory systemCategory)
        {
            return Enum.GetName(typeof(Constants.SystemCategory), systemCategory);
        }

        /// <summary>
        /// システム分類のValueを取得する
        /// </summary>
        internal static int GetIntValue(this Constants.SystemCategory systemCategory)
        {
            return (int)systemCategory;
        }

        /// <summary>
        /// システム分類のValueを取得する
        /// </summary>
        internal static string GetStringValue(this Constants.SystemCategory systemCategory)
        {
            return ((int)systemCategory).ToString();
        }

        /// <summary>
        /// XMLの属性値の取得
        /// </summary>
        /// <param name="e">XMLエレメント</param>
        /// <param name="attributeName">XMLの属性名</param>
        /// <returns>XMLの属性値</returns>
        internal static string AttributeValue(this XElement e, string attributeName)
        {
            var attr = e.Attribute(attributeName);
            if (attr == null)
                return string.Empty;
            else
                return attr.Value;
        }

        #endregion
    }
}