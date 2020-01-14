using Shared;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Converter
{
    /// <summary>
    /// 資材区分コンバータ
	/// </summary>
    internal sealed class SizaiKbnConverter : IValueConverter
    {
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
            string shizaiKbn = "";
            int sizaiKbn = -1;

            if (value != null)
            {
                string stringValue = value.ToString();
                sizaiKbn = Int32.Parse(stringValue);

                switch (sizaiKbn)
                {
                    case 1:
                        shizaiKbn = "SK";
                        break;
                    case 2:
                        shizaiKbn = "EF";
                        break;
                    case 3:
                        shizaiKbn = "LF";
                        break;
                    case 4:
                        shizaiKbn = "築炉";
                        break;
                    case 5:
                        shizaiKbn = "LD";
                        break;
                    case 6:
                        shizaiKbn = "TD";
                        break;
                    case 7:
                        shizaiKbn = "CC";
                        break;
                    case 8:
                        shizaiKbn = "他";
                        break;
                    default:
                        break;
                }
            }

            return shizaiKbn;
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

    }
}
