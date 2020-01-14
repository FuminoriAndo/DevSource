using CKSI1010.Common;
using CKSI1010.Common.Excel.DTO;
using CKSI1010.Operation.Common.ViewModel;
using CKSI1010.Shared;
using CKSI1010.Shared.Model;
using CKSI1010.Shared.ViewModel;
using CKSI1010.Operation.Common;
using Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static CKSI1010.Common.Constants;
using static Common.MessageManager;
//*************************************************************************************
//
//   立会い用棚卸表印刷画面のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.PrintWitnessInventory.ViewModel
{
    /// <summary>
    /// 立会い用棚卸表印刷画面のビューモデル
    /// </summary>
    public class PrintWitnessInventoryViewModel : OperationViewModelBase
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
        public PrintWitnessInventoryViewModel(IDataService dataService) : base(dataService)
        {
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
                IsFixed = CommonUtility.IsCurrentOperationFixed();
                await base.InitializeAsync();
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }
        }

        /// <summary>
        /// 印刷
        /// </summary>
        internal override async Task<bool> PrintAsync()
        {
            bool hasException = false;
            string errorContent = null;
            string errorCode = null;
            string sql = null;
            string sqlParam = null;
            string timestamp = null;

            try
            {
                BusyStatus.IsBusy = true;

                var tcs = new TaskCompletionSource<bool>();
                var thread = new Thread(async () =>
                {
                    try
                    {
                        // 立会い用棚卸表の印刷
                        await print();
                        tcs.SetResult(true);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();

                await tcs.Task;

                // 操作状況の更新
                await UpdateOperationCondition(OperationCondition.Fix);
                IsFixed = true;

                return await base.PrintAsync();
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
                // 棚卸ログの挿入
                LogType logType = hasException ? LogType.Error : LogType.Normal;
                await InsertTanaorosiLog(logType, SharedModel.Instance.WorkKbn,
                                         LogOperationType.PrintWitnessInventory,
                                         LogOperationContent.Print, null, errorContent, errorCode);
                if (!hasException)
                {
                    MessageManager.ShowInformation(SystemID.CKSI1010, "PrintCompleted");
                }
                else
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
            bool hasException = false;
            string errorContent = null;
            string errorCode = null;
            string sql = null;
            string sqlParam = null;
            string timestamp = null;

            try
            {
                BusyStatus.IsBusy = true;

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
                // 棚卸ログの挿入
                LogType logType = hasException ? LogType.Error : LogType.Normal;
                await InsertTanaorosiLog(logType, SharedModel.Instance.WorkKbn,
                                         LogOperationType.PrintWitnessInventory,
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
        /// 印刷処理
        /// </summary>
        private async Task<bool> print()
        {
            // 立会い用棚卸表印刷データの一覧
            IList<InventoryWitnessDTO> outputList = new List<InventoryWitnessDTO>();

            try
            {
                // 実施年月
                string tanaYm = OperationYearMonth.YearMonth.ToString();
                // 立会い用棚卸表印刷データの取得
                var originalData = await DataService.GetPrintWitnessModelAsync(tanaYm);
                // 立会い用棚卸表印刷表のデータを整形する
                var shapeData = OperationUtility.ShapeWitnessInventoryData(originalData.ToList());

                foreach (var item in shapeData)
                {
                    var excelItem = new InventoryWitnessDTO();
                    // ViewModelからModel(Excel出力用)へプロパティの移送
                    await CopyProperties(item, out excelItem);
                    // 立会い用棚卸表印刷データの一覧に追加する
                    outputList.Add(excelItem);
                }

                // Excelアクセッサーのインスタンスの取得
                excelAccessor = ExcelAccessor.GetInstance();
                // 立会い用棚卸表の出力
                await excelAccessor.OutputWitnessInventoryList(outputList);
            }

            catch (Exception)
            {
                throw;
            }

            return await Task.FromResult(true);
        }

        #endregion
    }
}
