using System.Threading.Tasks;
//*************************************************************************************
//
//   各棚卸操作用ユーザーコントロールを操作するためのインターフェース
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.Common.View
{
    /// <summary>
    /// 各棚卸操作用ユーザーコントロールを操作するためのインターフェース
    /// </summary>
    internal interface IOperationViewBase
    {
        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>非同期タスク</returns>
        Task InitializeAsync();

        /// <summary>
        /// 確定
        /// </summary>
        /// <returns>非同期タスク</returns>
        Task<bool> CommitAsync();

        /// <summary>
        /// 修正
        /// </summary>
        /// <returns>非同期タスク</returns>
        Task<bool> ModifyAsync();

        /// <summary>
        /// 印刷
        /// </summary>
        /// <returns>非同期タスク</returns>
        Task<bool> PrintAsync();

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns>非同期タスク</returns>
        Task<bool> UpdateAsync();

        /// <summary>
        /// 次へ
        /// </summary>
        /// <returns>非同期タスク</returns>
        Task<bool> GoNextAsync();

        /// <summary>
        /// 前へ
        /// </summary>
        /// <returns>非同期タスク</returns>
        Task<bool> BackPreviousAsync();

        /// <summary>
        /// 終了
        /// </summary>
        /// <returns>非同期タスク</returns>
        Task<bool> ExitAsync();

        /// <summary>
        /// ×ボタン
        /// </summary>
        /// <returns>非同期タスク</returns>
        Task<bool> CloseAsync();

        /// <summary>
        /// ぱんくずリスト選択
        /// </summary>
        /// <returns>非同期タスク</returns>
        Task<bool> SelecOperationAsync();

        /// <summary>
        /// ぱんくずリスト選択(前処理)
        /// </summary>
        /// <returns>非同期タスク</returns>
        Task<bool> BeforeSelecOperationAsync();

        #endregion

    }
}
