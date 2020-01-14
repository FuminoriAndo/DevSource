using Shared;
using System;
using System.Globalization;
using System.Windows.Data;
using Shared.Model;
using DBManager.Model;

namespace Converter
{
	/// <summary>
    /// グループ区分の紐付け用コンバータ
	/// </summary>
    internal sealed class GroupKbnConverter : IValueConverter
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
            string stringValue = (string)value;
            string groupKbn = string.Empty;

            if (SharedModel.Instance.OperationInfo != null 
                    && SharedModel.Instance.OperationInfo.GroupKbn != null && !string.IsNullOrEmpty(stringValue))
            {
                foreach (var item in SharedModel.Instance.OperationInfo.GroupKbn)
                {
                    if(stringValue.Equals(item.GroupKbn))
                    {
                        groupKbn = stringValue + ":" + item.GroupKbnName;
                        break;
                    }
                }
            }

            return groupKbn;
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
