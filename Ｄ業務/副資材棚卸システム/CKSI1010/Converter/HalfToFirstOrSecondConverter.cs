﻿using System;
using System.Globalization;
using System.Windows.Data;
//*************************************************************************************
//
//   設定値より上期・下期に変換するコンバータクラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Converter
{
    /// <summary>
    /// 設定値より上期・下期に変換するコンバータクラス
    /// </summary>
    internal sealed class HalfToFirstOrSecondConverter : IValueConverter
    {
        #region メソッド

        /// <summary>
        /// ソースからターゲット方向への変換処理
        /// </summary>
        /// <param name="value">バインディングソース</param>
        /// <param name="targetType">ターゲットのタイプ</param>
        /// <param name="parameter">パラメーター</param>
        /// <param name="culture">カルチャ</param>
        /// <returns>変換結果</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var half = (int)value;
            return half == 0 ? "上" : "下";
        }

        /// <summary>
        /// ターゲットからソースへの変換
        /// </summary>
        /// <param name="value">ターゲットの値</param>
        /// <param name="targetType">ターゲットの型</param>
        /// <param name="parameter">パラメーター</param>
        /// <param name="culture">カルチャ</param>
        /// <returns>変換された値</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}