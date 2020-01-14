using Shared;
using System;
using System.Globalization;
using System.Windows.Data;
using DBManager.Model;

namespace Converter
{
	/// <summary>
    /// 社員マスタ所属コードの紐付け用コンバータ
	/// </summary>
    internal sealed class EmployeeDeploymentCodeConverter : IValueConverter
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
            string deploymentCode = null;
            if (SharedModel.Instance.OperationInfo != null && SharedModel.Instance.OperationInfo.EmployeeInfo != null && !String.IsNullOrEmpty(stringValue))
            {
                if (SharedModel.Instance.OperationInfo.EmployeeInfo.ContainsKey(stringValue))
                {
                    EmployeeInfo enployeeInfo = SharedModel.Instance.OperationInfo.EmployeeInfo[stringValue];
                    deploymentCode = enployeeInfo.DeploymentCode.Trim();
                }
                else
                {
                    deploymentCode = "";
                }
            }

            return deploymentCode;
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
