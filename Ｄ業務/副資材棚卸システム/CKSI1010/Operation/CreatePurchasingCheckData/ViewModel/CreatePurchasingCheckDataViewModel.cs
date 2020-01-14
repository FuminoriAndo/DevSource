using CKSI1010.Common;
using CKSI1010.Operation.Common.ViewModel;
using CKSI1010.Shared;
using CKSI1010.Shared.Model;
using CKSI1010.Shared.ViewModel;
using Common;
using GalaSoft.MvvmLight.CommandWpf;
using log4net;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using static CKSI1010.Common.Constants;
using static Common.MessageManager;
//*************************************************************************************
//
//   購買検収データ作成のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.CreatePurchasingCheckData.ViewModel
{
    /// <summary>
    /// 購買検収データ作成のビューモデル
    /// </summary>
    public class CreatePurchasingCheckDataViewModel : OperationViewModelBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataService">データサービス</param>
        public CreatePurchasingCheckDataViewModel(IDataService dataService) : base(dataService)
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 実施年月
        /// </summary>
        public OperationYearMonth OperationYearMonth => SharedViewModel.Instance.OperationYearMonth;

        /// <summary>
        /// 実行済みか
        /// </summary>
        private bool executed = false;

        /// <summary>
        /// 実行済みか
        /// </summary>
        public bool Executed
        {
            get { return executed; }
            set { Set(ref executed, value); }
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 終了
        /// </summary>
        internal override async Task<bool> ExitAsync()
        {
            if (!Executed)
            {
                MessageManager.ShowExclamation(SystemID.CKSI1010, "CanNotWorkCompleted");
                return false;
            }

            else
            {
                MessageManager.ShowInformation(SystemID.CKSI1010, "KensinWorkCompleted");
                return await base.ExitAsync();
            }
        }

        #endregion

        #region コマンド

        /// <summary>
        /// 実行ボタンの押下
        /// </summary>
        public ICommand FireExecute => new RelayCommand(async () =>
        {
            bool hasException = false;
            string errorContent = string.Empty;
            string errorCode = string.Empty;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;

            try
            {
                BusyStatus.IsBusy = true;

                // 購買検収データ作成を起動する
                ExternalProcessAccessor.StartKobaiKenshuDataSakusei();
                // 一度しか実行できないので、実行済みであることを記憶する。
                Executed = true;
            }

            catch (Exception e)
            {
                hasException = true;

                if (e is TanaorosiLogException)
                {
                    TanaorosiLogException logException = e as TanaorosiLogException;
                    timestamp = logException.TimeStamp;
                    sql = logException.SQL;
                    sqlParam = logException.SqlParam;
                    errorContent = logException.Message;
                    errorCode = logException.ErrorCode;
                    CommonUtility.WriteLogExceptionSQL(timestamp, sql, errorContent, errorCode, sqlParam);
                }
                else
                {
                    errorContent = e.Message;
                }

                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
            }

            finally
            {
                // 棚卸ログの挿入
                LogType logType = hasException ? LogType.Error : LogType.Normal;
                await InsertTanaorosiLog(logType, SharedModel.Instance.WorkKbn,
                                         LogOperationType.CreatePurchasingCheckData, 
                                         LogOperationContent.Excute, null, errorContent, errorCode);
                if (hasException)
                {
                    MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                }

                BusyStatus.IsBusy = false;
            }
        });

        #endregion
    }
}