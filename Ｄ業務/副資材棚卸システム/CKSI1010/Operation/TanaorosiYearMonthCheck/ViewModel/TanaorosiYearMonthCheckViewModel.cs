using CKSI1010.Operation.Common.ViewModel;
using CKSI1010.Shared;
using CKSI1010.Shared.Model;
using CKSI1010.Shared.ViewModel;
using Common;
using log4net;
using System;
using System.Reflection;
using System.Threading.Tasks;
using static CKSI1010.Common.Constants;
using static Common.MessageManager;
//*************************************************************************************
//
//   実施月の確認画面のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.TanaorosiYearMonthCheck.ViewModel
{
    /// <summary>
    /// 実施月の確認画面のビューモデル
    /// </summary>
    public class TanaorosiYearMonthCheckViewModel : OperationViewModelBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TanaorosiYearMonthCheckViewModel(IDataService dataService)
            : base(dataService)
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 実施年月
        /// </summary>
        public OperationYearMonth OperationYearMonth => SharedViewModel.Instance.OperationYearMonth;

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