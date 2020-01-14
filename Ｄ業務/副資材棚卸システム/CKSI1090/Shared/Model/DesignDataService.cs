using CKSI1090.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
//*************************************************************************************
//
//   デザインモードのデーターサービスの実装
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1090.Shared.Model
{
    /// <summary>
    /// デザインモードのデータサービス
    /// </summary>
    internal class DesignDataService : IDataService
    {
        #region メソッド

        /// <summary>
        /// 棚卸期間の取得
        /// </summary>
        /// <param name="category">システム分類(1：資材 2：部品 (スペース)：未設定)</param>
        /// <returns>棚卸期間</returns>
        public Task<IEnumerable<InventoryTerm>> GetInventoryPeriodsAsync(string category)
        {
            return null;
        }

        /// <summary>
        /// 棚卸操作グループマスタに存在する資材ユーザであるかどうかをチェックする
        /// </summary>
        /// <param name="employeeCode">社員コード</param>
        /// <returns>結果</returns>
        public Task<bool> CheckUserInTanaorosiOperationGroupMst(string employeeCode, string belongingCode)
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// 作業誌一覧の取得
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>作業誌一覧</returns>
        public Task<IEnumerable<WorkNoteItem>> GetWorkNoteList(SearchCondition condition)
        {
            return null;
        }

        /// <summary>
        /// 作業誌一覧の更新
        /// </summary>
        /// <param name="models">作業誌項目のモデル/param>
        /// <returns>結果</returns>
        public Task<int> UpdateWorkNoteList(IList<WorkNoteItem> models)
        {
            return Task.FromResult(Constants.SQL_RESULT_OK);
        }

        #endregion
    }
}