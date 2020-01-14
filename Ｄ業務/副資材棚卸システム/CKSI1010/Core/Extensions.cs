using CKSI1010.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using static CKSI1010.Common.Constants;
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

namespace CKSI1010.Core
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
        internal static string GetName(this SystemCategory systemCategory)
        {
            return Enum.GetName(typeof(SystemCategory), systemCategory);
        }

        /// <summary>
        /// システム分類のValueを取得する
        /// </summary>
        internal static int GetIntValue(this SystemCategory systemCategory)
        {
            return (int)systemCategory;
        }

        /// <summary>
        /// システム分類のValueを取得する
        /// </summary>
        internal static string GetStringValue(this SystemCategory systemCategory)
        {
            return ((int)systemCategory).ToString();
        }

        /// <summary>
        /// 資材区分のNameを取得する
        /// <summary>
        internal static string GetName(this SizaiCategory sizaiCategory)
        {
            return Enum.GetName(typeof(SizaiCategory), sizaiCategory);
        }

        /// <summary>
        /// 資材区分のValueを取得する
        /// </summary>
        internal static int GetIntValue(this SizaiCategory sizaiCategory)
        {
            return (int)sizaiCategory;
        }

        /// <summary>
        /// 資材区分のValueを取得する
        /// </summary>
        internal static string GetStringValue(this SizaiCategory sizaiCategory)
        {
            return ((int)sizaiCategory).ToString();
        }

        /// <summary>
        /// 資材区分のNameを取得する
        /// <summary>
        internal static string GetName(this ShizaiKbnTypes shizaiKbnTypes)
        {
            return Enum.GetName(typeof(ShizaiKbnTypes), shizaiKbnTypes);
        }

        /// <summary>
        /// 資材区分のValueを取得する
        /// </summary>
        internal static int GetIntValue(this ShizaiKbnTypes shizaiKbnTypes)
        {
            return (int)shizaiKbnTypes;
        }

        /// <summary>
        /// 資材区分のValueを取得する
        /// </summary>
        internal static string GetStringValue(this ShizaiKbnTypes shizaiKbnTypes)
        {
            return ((int)shizaiKbnTypes).ToString();
        }

        /// <summary>
        /// 棚卸グループ区分のNameを取得する
        /// <summary>
        internal static string GetName(this SizaiGroupCategory sizaiGroupCategory)
        {
            return Enum.GetName(typeof(SizaiGroupCategory), sizaiGroupCategory);
        }

        /// <summary>
        /// 棚卸グループ区分のValueを取得する
        /// </summary>
        internal static int GetIntValue(this SizaiGroupCategory sizaiGroupCategory)
        {
            return (int)sizaiGroupCategory;
        }

        /// <summary>
        /// 棚卸グループ区分のValueを取得する
        /// </summary>
        internal static string GetStringValue(this SizaiGroupCategory sizaiGroupCategory)
        {
            return ((int)sizaiGroupCategory).ToString();
        }

        /// <summary>
        /// 資材入力作業区分のNameを取得する
        /// <summary>
        internal static string GetName(this SizaiWorkCategory sizaiWorkCategory)
        {
            return Enum.GetName(typeof(SizaiWorkCategory), sizaiWorkCategory);
        }

        /// <summary>
        /// 資材入力作業区分のValueを取得する
        /// </summary>
        internal static int GetIntValue(this SizaiWorkCategory sizaiWorkCategory)
        {
            return (int)sizaiWorkCategory;
        }

        /// <summary>
        /// 資材入力作業区分のValueを取得する
        /// </summary>
        internal static string GetStringValue(this SizaiWorkCategory sizaiWorkCategory)
        {
            return ((int)sizaiWorkCategory).ToString();
        }

        /// <summary>
        /// 資材作業誌区分のNameを取得する
        /// <summary>
        internal static string GetName(this SagyosiKbn sagyosiKbn)
        {
            return Enum.GetName(typeof(SagyosiKbn), sagyosiKbn);
        }

        /// <summary>
        /// 資材作業誌区分のValueを取得する
        /// </summary>
        internal static int GetIntValue(this SagyosiKbn sagyosiKbn)
        {
            return (int)sagyosiKbn;
        }

        /// <summary>
        /// 資材作業誌区分のValueを取得する
        /// </summary>
        internal static string GetStringValue(this SagyosiKbn sagyosiKbn)
        {
            return ((int)sagyosiKbn).ToString();
        }

        /// <summary>
        /// 操作状況のNameを取得する
        /// <summary>
        internal static string GetName(this OperationCondition operationCondition)
        {
            return Enum.GetName(typeof(OperationCondition), operationCondition);
        }

        /// <summary>
        /// 操作状況のValueを取得する
        /// </summary>
        internal static int GetIntValue(this OperationCondition operationCondition)
        {
            return (int)operationCondition;
        }

        /// <summary>
        /// 操作状況のValueを取得する
        /// </summary>
        internal static string GetStringValue(this OperationCondition operationCondition)
        {
            return ((int)operationCondition).ToString();
        }

        /// <summary>
        /// 向先のNameを取得する
        /// <summary>
        internal static string GetName(this Mukesaki mukesaki)
        {
            return Enum.GetName(typeof(Mukesaki), mukesaki);
        }

        /// <summary>
        /// 向先のValueを取得する
        /// </summary>
        internal static int GetIntValue(this Mukesaki mukesaki)
        {
            return (int)mukesaki;
        }

        /// <summary>
        /// 向先のValueを取得する
        /// </summary>
        internal static string GetStringValue(this Mukesaki mukesaki)
        {
            return ((int)mukesaki).ToString();
        }

        /// <summary>
        /// 資材置場のNameを取得する
        /// <summary>
        internal static string GetName(this SizaiPlace sizaiPlace)
        {
            return Enum.GetName(typeof(SizaiPlace), sizaiPlace);
        }

        /// <summary>
        /// 資材置場のValueを取得する
        /// </summary>
        internal static int GetIntValue(this SizaiPlace sizaiPlace)
        {
            return (int)sizaiPlace;
        }

        /// <summary>
        /// 資材置場のValueを取得する
        /// </summary>
        internal static string GetStringValue(this SizaiPlace sizaiPlace)
        {
            return ((int)sizaiPlace).ToString();
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