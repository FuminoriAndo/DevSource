using System;
//*************************************************************************************
//
//   入力データのチェッククラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.30              DSK吉武   　 新規作成
//
//*************************************************************************************
namespace Common
{
    /// <summary>
    /// 入力データのチェック
    /// </summary>
    internal static class InputChecker
    {
        #region フィールド

        /// <summary>
        /// 作業日時の最大文字数
        /// </summary>
        private const int OperationDate_MaxLength = 6;
     
        #endregion

        #region メソッド
        /// <summary>
        /// 社員番号のチェック
        /// </summary>
        /// <param name="employeeNo">社員番号</param>
        /// <returns>チェック結果</returns>
        internal static string CheckEmployeeNo(string employeeNo)
        {
            string result = string.Empty;

            if (String.IsNullOrEmpty(employeeNo))
            {
                result = "NoEmployeeNoValue";
            }

            return result;
        }

        /// <summary>
        /// 作業日のチェック
        /// </summary>
        /// <param name="minOperationDate">作業日(最小)</param>
        /// <param name="maxOperationDate">作業日(最大)</param>
        /// <returns>チェック結果</returns>
        internal static string CheckOperationDate(string minOperationDate, string maxOperationDate)
        {
            string result = string.Empty;

            if ((String.IsNullOrEmpty(minOperationDate)) && (String.IsNullOrEmpty(maxOperationDate)))
            {
                result = "NoOperationDateValue";
            }
            else
            {
                if (!String.IsNullOrEmpty(minOperationDate))
                {
                    int minLength = minOperationDate.Length;
                    if (minLength < OperationDate_MaxLength)
                    {
                        result = "InvalidMinOperationDateLength";
                    }
                }

                if (!String.IsNullOrEmpty(maxOperationDate))
                {
                    int maxLength = maxOperationDate.Length;
                    if (maxLength < OperationDate_MaxLength)
                    {
                        result = "InvalidMaxOperationDateLength";
                    }
                }
            }
            return result;
        }
        
        /// <summary>
        /// 入力作業のチェック
        /// </summary>
        /// <param name="workKbn">入力作業区分</param>
        /// <returns>チェック結果</returns>
        internal static string CheckWorkKbn(string workKbn)
        {
            string result = string.Empty;

            if (String.IsNullOrEmpty(workKbn))
            {
                result = "NoWorkKbnValue";
            }
            return result;
        }

        /// <summary>
        /// 操作種別のチェック
        /// </summary>
        /// <param name="operateType">操作種別</param>
        /// <returns>チェック結果</returns>
        internal static string CheckOperationType(string operateType)
        {
            string result = string.Empty;

            if (String.IsNullOrEmpty(operateType))
            {
                result = "NoOperateTypeValue";
            }
            return result;
        }

        /// <summary>
        /// 操作種別のチェック
        /// </summary>
        /// <param name="operateContent">操作種別</param>
        /// <returns>チェック結果</returns>
        internal static string CheckOperationContent(string operateContent)
        {
            string result = string.Empty;

            if (String.IsNullOrEmpty(operateContent))
            {
                result = "NoOperateContentValue";
            }
            return result;
        }

        #endregion
    }
}