using System;
using System.Globalization;
using System.Windows.Data;
//*************************************************************************************
//
//   Bool値を反転させるコンバータ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.02.01              DSK   　     新規作成
//
//*************************************************************************************
namespace Converter
{
	/// <summary>
	/// Bool値を反転させるコンバータ
	/// </summary>
	[ValueConversion(typeof(bool), typeof(bool))]
	internal sealed class BooleanNegationConverter : IValueConverter
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
			return !(bool)value;
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
			return !(bool)value;
        }
        #endregion
    }
}
