using CKSI1010.Common;
using CKSI1010.Core;
using CKSI1010.Operation.Common.ViewModel;
using CKSI1010.Shared;
using CKSI1010.Shared.Model;
using CKSI1010.Shared.ViewModel;
using Common;
using log4net;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static CKSI1010.Common.Constants;
using static Common.MessageManager;
//*************************************************************************************
//
//   棚卸実績値更新画面のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.UpdateInventoryActual.ViewModel
{
    /// <summary>
    /// 棚卸実績値更新画面のビューモデル
    /// </summary>
    public class UpdateInventoryActualViewModel : OperationViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 確定状態か
        /// </summary>
        private bool isFixed = false;

        /// <summary>
        /// Excelアクセッサー
        /// </summary>
        private ExcelAccessor excelAccessor = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataService">データサービス</param>
        public UpdateInventoryActualViewModel(IDataService dataService) : base(dataService)
        {
            // Excelアクセッサーのインスタンスの取得
            excelAccessor = ExcelAccessor.GetInstance();
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 確定状態か
        /// </summary>
        public bool IsFixed
        {
            get { return isFixed; }
            set { Set(ref isFixed, value); }
        }

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
                BusyStatus.IsBusy = true;
                IsFixed = CommonUtility.IsCurrentOperationFixed();
                await base.InitializeAsync();
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

            finally
            {
                BusyStatus.IsBusy = false;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        internal override async Task<bool> UpdateAsync()
        {
            string workKbn = string.Empty;
            string errorContent = string.Empty;
            string errorCode = string.Empty;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;
            bool hasException = false;

            try
            {
                BusyStatus.IsBusy = true;

                // 資材棚卸トランデータ削除
                await DataService.DeleteSizaiTanaorosiTranAsync
                    (OperationYearMonth.YearMonth.ToString(), SharedModel.Instance.WorkKbn.GetStringValue());

                // 資材棚卸トランデータ登録
                await DataService.EntryInventoriesTranAsync
                    (OperationYearMonth.YearMonth.ToString(), SharedModel.Instance.WorkKbn.GetStringValue());

                // 経理報告データ作成を起動する
                ExternalProcessAccessor.StartKeiriHoukokuData();

                // 操作状況の更新
                await UpdateOperationCondition(OperationCondition.Fix);
                IsFixed = true;

                return await base.UpdateAsync();
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
                return false;
            }

            finally
            {
                LogType logType = hasException ? LogType.Error : LogType.Normal;
                SizaiWorkCategory sizaiWorkCategory = SharedModel.Instance.WorkKbn;

                // 棚卸ログの挿入
                await InsertTanaorosiLog(logType, sizaiWorkCategory, 
                                         LogOperationType.UpdateInventoryActual, 
                                         LogOperationContent.Update, null,errorContent, errorCode);
                if (hasException)
                {
                    MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                }

                BusyStatus.IsBusy = false;
            }
        }

        /// <summary>
        /// 修正
        /// </summary>
        internal override async Task<bool> ModifyAsync()
        {
            string workKbn = string.Empty;
            string errorContent = string.Empty;
            string errorCode = string.Empty;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;
            bool hasException = false;

            try
            {
                BusyStatus.IsBusy = true;

                // 共有データで保持している印刷対象外受払い情報の初期化
                CommonUtility.InitializeExcludeInputPaymentInfo();

                // 操作状況の更新
                await UpdateOperationCondition(OperationCondition.Modify);
                IsFixed = false;

                return await base.ModifyAsync();
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
                return false;
            }

            finally
            {
                LogType logType = hasException ? LogType.Error : LogType.Normal;
                SizaiWorkCategory sizaiWorkCategory = SharedModel.Instance.WorkKbn;

                // 棚卸ログの挿入
                await InsertTanaorosiLog(logType, sizaiWorkCategory, 
                                         LogOperationType.UpdateInventoryActual, 
                                         LogOperationContent.Modify, null, errorContent, errorCode);
                if (hasException)
                {
                    MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                }

                BusyStatus.IsBusy = false;
            }
        }

        /// <summary>
        /// 次へ
        /// </summary>
        internal override async Task<bool> GoNextAsync()
        {
            try
            {
                BusyStatus.IsBusy = true;

                if (!IsFixed)
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "CanNotGoNext");
                    return false;
                }
                else
                {
                    return await base.GoNextAsync();
                }
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                return false;
            }

            finally
            {
                BusyStatus.IsBusy = false;
            }
        }

        /// <summary>
        /// 棚卸操作選択の発火
        /// </summary>
        internal override async Task<bool> SelecOperationAsync()
        {
            try
            {
                BusyStatus.IsBusy = true;

                // 棚卸データ入力作業の場合、単価未入力チェックを行う。
                if(SharedModel.Instance.WorkKbn == SizaiWorkCategory.TanaoroshiWork)
                {
                    // 単価未入力の品目一覧を取得する
                    var list = await DataService.CheckNotEnteredTanka();

                    // 単価未入力の品目が存在するか
                    if (list.Count() > 0)
                    {
                        // 単価未入力リストを出力する
                        await excelAccessor.OutputUnInputTankaList(list.ToList());

                        // 単価未入力の品目が存在する旨のメッセージを表示する
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "NotInputHinmokuTanka",
                            CommonUtility.GetSizaiTanaorosiExcelBookPath());

                        return false;
                    }
                }

                return await base.SelecOperationAsync();
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                return false;
            }

            finally
            {
                BusyStatus.IsBusy = false;
            }
        }
        #endregion
    }
}
