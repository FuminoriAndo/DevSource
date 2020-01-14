using System;
using System.Globalization;
using System.Windows.Data;
//*************************************************************************************
//
//   Enum型の要素の値から文字列へのコンバータ
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
    /// Enum型の要素の値から文字列へのコンバータ
	/// </summary>
	internal sealed class EnumToStringConverter : IValueConverter
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
            return Enum.GetName(value.GetType(), value);
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
