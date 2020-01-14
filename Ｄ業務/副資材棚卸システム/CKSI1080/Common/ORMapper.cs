using System;
using DBManager.Condition;
using CKSI1080.ViewModel;
using ViewModel;
//*************************************************************************************
//
//   DBとViewModelのMapper
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.30              DSK吉武   　 新規作成
//
//*************************************************************************************
namespace CKSI1080.Common
{
    /// <summary>
    /// DBとViewModelのMapper
    /// </summary>
    internal class ORMapper
    {
        /// <summary>
        /// 棚卸ログ検索条件への設定
        /// </summary>
        /// <param name="SearchCondition">棚卸ログ検索条件</param>
        /// <returns>棚卸ログ検索条件</returns>
        internal static LogSearchCondition SetLogSearchCondition(ViewModelBase searchCondition)
        {
            LogSearchCondition condition = null;

            if (searchCondition is SearchCondition)
            {
                condition = SetLogSearchCondition(searchCondition as SearchCondition);
            }

            return condition;
        }

        /// <summary>
        /// 棚卸ログ検索条件への設定
        /// </summary>
        /// <param name="saveSearchCondition">棚卸ログ検索条件</param>
        /// <returns>棚卸ログ検索条件</returns>
        private static LogSearchCondition SetLogSearchCondition(SearchCondition searchCondition)
        {
            LogSearchCondition condition = new LogSearchCondition();

            //システムカテゴリ
            condition.SystemCategory = searchCondition.SystemCategory;

            //ログ種別
            condition.LogType = searchCondition.LogType;

            //社員番号
            condition.UseEmployeeNo = searchCondition.UseEmployeeNo;
            condition.EmployeeNo = searchCondition.EmployeeNo;

            //作業日時
            condition.UseOperationDate = searchCondition.UseOperationDate;
            condition.MinOperationDate = "20" + searchCondition.MinOperationDate + "000000";
            condition.MaxOperationDate = "20" + searchCondition.MaxOperationDate + "999999";

            if (condition.UseOperationDate)
            {
                if (String.IsNullOrEmpty(searchCondition.MinOperationDate)
                        || String.IsNullOrEmpty(searchCondition.MaxOperationDate))
                {
                    if (String.IsNullOrEmpty(searchCondition.MinOperationDate))
                    {
                        condition.OperationDateCondition = LogSearchCondition.OperationDateConditions.Max;
                    }
                    else
                    {
                        condition.OperationDateCondition = LogSearchCondition.OperationDateConditions.Min;
                    }
                }
                else
                {
                    condition.OperationDateCondition = LogSearchCondition.OperationDateConditions.MinMax;
                }
            }

            //入力作業
            condition.UseWorkKbn = searchCondition.UseWorkKbn;
            condition.WorkKbn = searchCondition.WorkKbn;

            //操作種別
            condition.UseOperateType = searchCondition.UseOperateType;
            condition.OperateType = searchCondition.OperateType;

            //操作内容
            condition.UseOperateContent = searchCondition.UseOperateContent;
            condition.OperateContent = searchCondition.OperateContent;

            return condition;
        }
    }
}

