using CKSI1010.Operation.Common.ViewModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
//*************************************************************************************
//
//   各棚卸操作用の基底ユーザーコントロール
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
    /// 各棚卸操作用の基底ユーザーコントロール
    /// </summary>
    public class OperationViewBase : UserControl, IOperationViewBase
    {
        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>非同期タスク</returns>
        public Task InitializeAsync()
        {
            var viewModel = DataContext as OperationViewModelBase;
            Debug.Assert(viewModel != null);

            return viewModel.InitializeAsync();
        }

        /// <summary>
        /// 確定
        /// </summary>
        /// <returns>非同期タスク</returns>
        public Task<bool> CommitAsync()
        {
            var viewModel = DataContext as OperationViewModelBase;
            Debug.Assert(viewModel != null);

            return viewModel.CommitAsync();
        }

        /// <summary>
        /// 修正
        /// </summary>
        /// <returns>非同期タスク</returns>
        public Task<bool> ModifyAsync()
        {
            var viewModel = DataContext as OperationViewModelBase;
            Debug.Assert(viewModel != null);

            return viewModel.ModifyAsync();
        }

        /// <summary>
        /// 印刷
        /// </summary>
        /// <returns>非同期タスク</returns>
        public Task<bool> PrintAsync()
        {
            var viewModel = DataContext as OperationViewModelBase;
            Debug.Assert(viewModel != null);

            return viewModel.PrintAsync();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns>非同期タスク</returns>
        public Task<bool> UpdateAsync()
        {
            var viewModel = DataContext as OperationViewModelBase;
            Debug.Assert(viewModel != null);

            return viewModel.UpdateAsync();
        }

        /// <summary>
        /// 次へ
        /// </summary>
        /// <returns>非同期タスク</returns>
        public Task<bool> GoNextAsync()
        {
            var viewModel = DataContext as OperationViewModelBase;
            Debug.Assert(viewModel != null);

            return viewModel.GoNextAsync();
        }

        /// <summary>
        /// 前へ
        /// </summary>
        /// <returns>非同期タスク</returns>
        public Task<bool> BackPreviousAsync()
        {
            var viewModel = DataContext as OperationViewModelBase;
            Debug.Assert(viewModel != null);

            return viewModel.BackPreviousAsync();
        }

        /// <summary>
        /// 終了
        /// </summary>
        /// <returns>非同期タスク</returns>
        public Task<bool> ExitAsync()
        {
            var viewModel = DataContext as OperationViewModelBase;
            Debug.Assert(viewModel != null);

            return viewModel.ExitAsync();
        }

        /// <summary>
        /// ×ボタン
        /// </summary>
        /// <returns>非同期タスク</returns>
        public Task<bool> CloseAsync()
        {
            var viewModel = DataContext as OperationViewModelBase;
            Debug.Assert(viewModel != null);

            return viewModel.CloseAsync();
        }

        /// <summary>
        /// ぱんくずリスト選択
        /// </summary>
        /// <returns>非同期タスク</returns>
        public Task<bool> SelecOperationAsync()
        {
            var viewModel = DataContext as OperationViewModelBase;
            Debug.Assert(viewModel != null);

            return viewModel.SelecOperationAsync();
        }

        /// <summary>
        /// ぱんくずリスト選択(前処理)
        /// </summary>
        /// <returns>非同期タスク</returns>
        public Task<bool> BeforeSelecOperationAsync()
        {
            var viewModel = DataContext as OperationViewModelBase;
            Debug.Assert(viewModel != null);

            return viewModel.BeforeSelecOperationAsync();
        }

        #endregion
    }
}
