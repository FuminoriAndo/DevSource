﻿using System;
using System.Globalization;
using System.Windows.Data;
//*************************************************************************************
//
//   String型をInt型に変換するコンバータクラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace Converter
{
    /// <summary>
    /// String型をInt型に変換するコンバータクラス
    /// </summary>
    internal sealed class StringToIntConverter : IValueConverter
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
            if (value != null)
            { 
                string stringValue = value.ToString();
                if(!string.IsNullOrEmpty(stringValue))
                { 
                    return int.Parse(stringValue);
                }
            }

            return value;
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
