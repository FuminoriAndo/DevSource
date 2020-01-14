using CKSI1090.Operation.Common.ViewModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
//*************************************************************************************
//
//   基底ユーザーコントロール
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
    /// 基底ユーザーコントロール
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
        /// ×ボタン
        /// </summary>
        /// <returns>非同期タスク</returns>
        public Task<bool> CloseAsync()
        {
            var viewModel = DataContext as OperationViewModelBase;
            Debug.Assert(viewModel != null);

            return viewModel.CloseAsync();
        }

        #endregion
    }
}
