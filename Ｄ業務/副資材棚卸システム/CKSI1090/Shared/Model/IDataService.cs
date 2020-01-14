using System.Collections.Generic;
using System.Threading.Tasks;
//*************************************************************************************
//
//   データーサービス
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
    /// データサービス
    /// </summary>
    public interface IDataService
    {   
        /// <summary>
        /// 棚卸期間の取得
        /// </summary>
        /// <param name="category">システム分類(1：資材 2：部品 (スペース)：未設定)</param>
        /// <returns>棚卸期間</returns>
        Task<IEnumerable<InventoryTerm>> GetInventoryPeriodsAsync(string category);

        /// <summary>
        /// 棚卸操作グループマスタに存在する資材ユーザであるかどうかをチェックする
        /// </summary>
        /// <param name="employeeCode">社員コード</param>
        /// <param name="belongingCode">所属コード</param>
        /// <returns>結果</returns>
        Task<bool> CheckUserInTanaorosiOperationGroupMst(string employeeCode, string belongingCode);

        /// <summary>
        /// 作業誌一覧の取得
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>作業誌一覧</returns>
        Task<IEnumerable<WorkNoteItem>> GetWorkNoteList(SearchCondition condition);

        /// <summary>
        /// 作業誌一覧の更新
        /// </summary>
        /// <param name="models">作業誌項目のモデル/param>
        /// <returns>結果</returns>
        Task<int> UpdateWorkNoteList(IList<WorkNoteItem> models);

    }
}
