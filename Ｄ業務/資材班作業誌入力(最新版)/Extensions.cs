using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
//*************************************************************************************
//
//   拡張メソッド
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace Project1
{
    /// <summary>
    /// 拡張メソッド
    /// </summary>
    internal static class Extensions
    {
        #region メソッド

        /// <summary>
        /// 資材区分の名前を取得する
        /// </summary>
        /// <param name="sizaiCategory">資材区分</param>
        /// <returns>資材区分の名前</returns>
        internal static string GetName(this SizaiConstants.SizaiCategory sizaiCategory)
        {
            return Enum.GetName(typeof(SizaiConstants.SizaiCategory), sizaiCategory);
        }

        /// <summary>
        /// 資材区分のValueを取得する
        /// </summary>
        /// <param name="sizaiCategory">資材区分</param>
        /// <returns>資材区分のValue</returns>
        internal static int GetIntValue(this SizaiConstants.SizaiCategory sizaiCategory)
        {
            return (int)sizaiCategory;
        }

        /// <summary>
        /// 資材区分のValueを取得する
        /// </summary>
        /// <param name="sizaiCategory">資材区分</param>
        /// <returns>資材区分のValue</returns>
        internal static string GetStringValue(this SizaiConstants.SizaiCategory sizaiCategory)
        {
            return ((int)sizaiCategory).ToString();
        }

        /// <summary>
        /// 向先のNameを取得する
        /// </summary>
        /// <param name="mukesaki">向先</param>
        /// <returns>向先のName</returns>
        internal static string GetName(this SizaiConstants.Mukesaki mukesaki)
        {
            return Enum.GetName(typeof(SizaiConstants.Mukesaki), mukesaki);
        }

        /// <summary>
        /// 向先のValueを取得する
        /// </summary>
        /// <param name="mukesaki">向先</param>
        /// <returns>向先のValue</returns>
        internal static int GetIntValue(this SizaiConstants.Mukesaki mukesaki)
        {
            return (int)mukesaki;
        }

        /// <summary>
        /// 向先のValueを取得する
        /// </summary>
        /// <param name="mukesaki">向先</param>
        /// <returns>向先のValue</returns>
        internal static string GetStringValue(this SizaiConstants.Mukesaki mukesaki)
        {
            return ((int)mukesaki).ToString();
        }

        #endregion
    }
}