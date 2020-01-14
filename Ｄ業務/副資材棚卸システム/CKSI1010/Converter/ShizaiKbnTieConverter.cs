using System;
using System.Globalization;
using System.Windows.Data;
using static CKSI1010.Common.Constants;
//*************************************************************************************
//
//   資材区分を資材区分名称に変換するコンバータクラス
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
    /// 資材区分を資材区分名称に変換するコンバータクラス
    /// </summary>
    internal sealed class ShizaiKbnTieConverter : IValueConverter
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
            int sizaiKbn = -1;

            if (value != null)
            {
                string stringValue = value.ToString();
                sizaiKbn = int.Parse(stringValue);

                switch (sizaiKbn)
                {
                    case (int)SizaiCategory.SK:
                        shizaiKbn = "SK";
                        break;
                    case (int)SizaiCategory.EF:
                        shizaiKbn = "EF";
                        break;
                    case (int)SizaiCategory.LF:
                        shizaiKbn = "LF";
                        break;
                    case (int)SizaiCategory.Building:
                        shizaiKbn = "築炉";
                        break;
                    case (int)SizaiCategory.LD:
                        shizaiKbn = "LD";
                        break;
                    case (int)SizaiCategory.TD:
                        shizaiKbn = "TD";
                        break;
                    case (int)SizaiCategory.CC:
                        shizaiKbn = "CC";
                        break;
                    case (int)SizaiCategory.Others:
                        shizaiKbn = "他";
                        break;
                    case (int)SizaiCategory.Meter:
                        shizaiKbn = "メータ";
                        break;
                    case (int)SizaiCategory.Yobi1:
                        shizaiKbn = "予備１";
                        break;
                    case (int)SizaiCategory.Yobi2:
                        shizaiKbn = "予備２";
                        break;
                    default:
                        shizaiKbn = "未設定";
                        break;
                }
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