using CKSI1010.Shared;
using System;
using System.Globalization;
using System.Windows.Data;
//*************************************************************************************
//
//   向先の紐付け用コンバータクラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Converter
{
    /// <summary>
    /// 向先の紐付け用コンバータ
    /// </summary>
    internal sealed class MukesakiTieConverter : IValueConverter
    {
        #region メソッド
        
        /// <summary>
        /// ソースからターゲット方向への反映時の変換処理
        /// </summary>
        /// <param name="value">ソース</param>
        /// <param name="targetType">ターゲットのタイプ</param>
        /// <param name="parameter">パラメーター</param>
        /// <param name="culture">カルチャ</param>
        /// <returns>変換された値</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = (string)value;
            if (String.IsNullOrEmpty(stringValue.Trim()))
            {
                return value;
            }
            else
            {
                return SharedViewModel.Instance.Mukesaki != null ? SharedViewModel.Instance.Mukesaki[stringValue] : null;
            }
        }

        /// <summary>
        /// ターゲットからソース方向への反映時の変換処理
        /// </summary>
        /// <param name="value">ターゲットタイプ</param>
        /// <param name="targetType">変換後の型</param>
        /// <param name="parameter">パラメーター</param>
        /// <param name="culture">カルチャ</param>
        /// <returns>変換された値</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
