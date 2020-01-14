//*************************************************************************************
//
//   入力チェッククラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090.Common
{
    /// <summary>
    /// 入力チェッククラス
    /// </summary>
    internal class InputChecker
    {

        #region フィールド

        /// <summary>
        /// エラーなし
        /// </summary>
        internal const int No_Error = 0;

        /// <summary>
        /// 未入力
        /// </summary>
        internal const int Not_Input = 1;

        /// <summary>
        /// 未選択
        /// </summary>
        internal const int Not_Select = 2;

        /// <summary>
        /// 文字列桁不足
        /// </summary>
        internal const int Str_Not_Enough_Length = 3;

        /// <summary>
        /// 作業日時の最大文字数
        /// </summary>
        internal const int OperationDate_Length = 6;

        /// <summary>
        /// 品目コードの最大文字数
        /// </summary>
        internal const int HinmokuCode_Length = 4;

        /// <summary>
        /// 業者コードの最大文字数
        /// </summary>
        internal const int GyousyaCode_Length = 4;

        #endregion

        #region メソッド

        /// <summary>
        /// 作業日のチェック
        /// </summary>
        /// <param name="minOperationDate">作業日(最小)</param>
        /// <param name="maxOperationDate">作業日(最大)</param>
        /// <returns>チェック結果</returns>
        internal static int CheckOperationDate(string minOperationDate, string maxOperationDate)
        {
            int result = No_Error;

            if ((string.IsNullOrEmpty(minOperationDate)) && (string.IsNullOrEmpty(maxOperationDate)))
            {
                result = Not_Input;
            }

            else
            {
                if (!string.IsNullOrEmpty(minOperationDate))
                {
                    int minLength = minOperationDate.Length;
                    if (minLength < OperationDate_Length)
                    {
                        result = Str_Not_Enough_Length;
                    }
                }

                if (!string.IsNullOrEmpty(maxOperationDate))
                {
                    int maxLength = maxOperationDate.Length;
                    if (maxLength < OperationDate_Length)
                    {
                        result = Str_Not_Enough_Length;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 品目コードのチェック
        /// </summary>
        /// <param name="minHinmokuCode">品目コード(最小)</param>
        /// <param name="maxHinmokuCode">品目コード(最大)</param>
        /// <returns>チェック結果</returns>
        internal static int CheckHinmokuCode(string minHinmokuCode, string maxHinmokuCode)
        {
            int result = No_Error;

            if ((string.IsNullOrEmpty(minHinmokuCode)) && (string.IsNullOrEmpty(maxHinmokuCode)))
            {
                result = Not_Input;
            }

            else
            {
                if (!string.IsNullOrEmpty(minHinmokuCode))
                {
                    int minLength = minHinmokuCode.Length;
                    if (minLength < HinmokuCode_Length)
                    {
                        result = Str_Not_Enough_Length;
                    }
                }

                if (!string.IsNullOrEmpty(maxHinmokuCode))
                {
                    int maxLength = maxHinmokuCode.Length;
                    if (maxLength < HinmokuCode_Length)
                    {
                        result = Str_Not_Enough_Length;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 業者コードのチェック
        /// </summary>
        /// <param name="minGyosyaCode">業者コード(最小)</param>
        /// <param name="maxGyosyaCode">業者コード(最大)</param>
        /// <returns>チェック結果</returns>
        internal static int CheckGyosyaCode(string minGyosyaCode, string maxGyosyaCode)
        {
            int result = No_Error;

            if (!string.IsNullOrEmpty(minGyosyaCode))
            {
                int minLength = minGyosyaCode.Length;
                if (minLength < GyousyaCode_Length)
                {
                    result = Str_Not_Enough_Length;
                }
            }

            if (!string.IsNullOrEmpty(maxGyosyaCode))
            {
                int maxLength = maxGyosyaCode.Length;
                if (maxLength < GyousyaCode_Length)
                {
                    result = Str_Not_Enough_Length;
                }
            }

            return result;
        }

        /// <summary>
        /// 作業誌区分のチェック
        /// </summary>
        /// <param name="workKbn">作業誌区分</param>
        /// <returns>チェック結果</returns>
        internal static int CheckWorkNoteKbn(string workKbn)
        {
            int result = No_Error;

            if (string.IsNullOrEmpty(workKbn))
            {
                result = Not_Select;
            }

            return result;
        }

        /// <summary>
        /// 向先のチェック
        /// </summary>
        /// <param name="mukesaki">向先</param>
        /// <returns>チェック結果</returns>
        internal static int CheckMukesaki(string mukesaki)
        {
            int result = No_Error;

            if (string.IsNullOrEmpty(mukesaki))
            {
                result = Not_Select;
            }

            return result;
        }

        #endregion
    }
}