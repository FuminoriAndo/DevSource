using CKSI1010.Core;
using CKSI1010.Operation.Common.ViewModel;
using CKSI1010.Shared;
using CKSI1010.Shared.Model;
using CKSI1010.Shared.ViewModel;
using Common;
using log4net;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static CKSI1010.Common.Constants;
using static Common.MessageManager;
//*************************************************************************************
//
//   手順確認のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.ShowFlow.ViewModel
{
    /// <summary>
    /// 手順確認のビューモデル
    /// </summary>
    public class ShowFlowViewModel : OperationViewModelBase
    {
        #region プロパティ

        /// <summary>
        /// 操作一覧
        /// </summary>
        public ObservableCollection<FlowItem> Items { get; } = new ObservableCollection<FlowItem>();

        /// <summary>
        /// 実施年月
        /// </summary>
        public OperationYearMonth OperationYearMonth => SharedViewModel.Instance.OperationYearMonth;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataService">データサービス</param>
        public ShowFlowViewModel(IDataService dataService) : base(dataService)
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal override async Task InitializeAsync()
        {
            try
            {
                await UpdateOperationCondition(OperationCondition.Fix, false);

                Items.Clear();
                Items.AddRange(SharedViewModel.Instance.Operations
                        .Where(operation => !string.IsNullOrEmpty(operation.Note))
                        .Select((operation, index) => new FlowItem() { No = index + 1, Operation = operation }));

                await base.InitializeAsync();
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }
        }

        #endregion
    }
}