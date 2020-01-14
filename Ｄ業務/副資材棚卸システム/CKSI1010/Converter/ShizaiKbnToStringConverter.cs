using System;
using System.Globalization;
using System.Windows.Data;
using static CKSI1010.Common.Constants;
//*************************************************************************************
//
//   資材区分を文字列に変換するコンバータクラス
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
    /// 資材区分を文字列に変換するコンバータクラス
    /// </summary>
    internal sealed class ShizaiKbnToStringConverter : IValueConverter
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
            string shizaiKbn = string.Empty;

            switch((SizaiCategory)value)
            {
                case SizaiCategory.SK:
                    shizaiKbn = "SK";
                    break;
                case SizaiCategory.EF:
                    shizaiKbn = "EF";
                    break;
                case SizaiCategory.LF:
                    shizaiKbn = "LF";
                    break;
                case SizaiCategory.Building:
                    shizaiKbn = "築炉";
                    break;
                case SizaiCategory.LD:
                    shizaiKbn = "LD";
                    break;
                case SizaiCategory.TD:
                    shizaiKbn = "TD";
                    break;
                case SizaiCategory.CC:
                    shizaiKbn = "CC";
                    break;
                case SizaiCategory.Others:
                    shizaiKbn = "他";
                    break;
                case SizaiCategory.Meter:
                    shizaiKbn = "メータ";
                    break;
                case SizaiCategory.Yobi1:
                    shizaiKbn = "予備１";
                    break;
                case SizaiCategory.Yobi2:
                    shizaiKbn = "予備２";
                    break;
                default:
                    shizaiKbn = "未設定";
                    break;
            }

            return shizaiKbn;
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