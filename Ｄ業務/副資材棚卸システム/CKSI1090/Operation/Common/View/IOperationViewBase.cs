using System.Threading.Tasks;
//*************************************************************************************
//
//   各棚卸操作用ユーザーコントロールを操作するためのインターフェース
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090.Operation.Common.View
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
        /// ×ボタン
        /// </summary>
        /// <returns>非同期タスク</returns>
        Task<bool> CloseAsync();

        #endregion

    }
}
