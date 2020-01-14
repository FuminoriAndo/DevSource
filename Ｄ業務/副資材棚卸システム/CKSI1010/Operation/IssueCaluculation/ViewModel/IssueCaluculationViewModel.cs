using CKSI1010.Common;
using CKSI1010.Common.Excel.DTO;
using CKSI1010.Operation.Common.ViewModel;
using CKSI1010.Shared;
using CKSI1010.Shared.Model;
using CKSI1010.Shared.ViewModel;
using Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CKSI1010.Common.Constants;
using static Common.MessageManager;
//*************************************************************************************
//
//   計算書発行画面のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.IssueCaluculation.ViewModel
{
    /// <summary>
    /// 計算書発行画面のビューモデル
    /// </summary>
    public class IssueCaluculationViewModel : OperationViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 使用高払い受払い計算書
        /// </summary>
        private readonly string USE_PAYMENT = "使用高払い受払い計算書";

        /// <summary>
        /// 入庫払い受払い計算書
        /// </summary>
        private readonly string INPUT_PAYMENT = "入庫払い受払い計算書";

        /// <summary>
        /// 使用高払い受払い計算書を印刷するか
        /// </summary>
        private bool printSiyoudaka = true;

        /// <summary>
        /// 入庫払い受払い計算書を印刷するか
        /// </summary>
        private bool printNyuko = true;

        /// <summary>
        /// 確定状態か
        /// </summary>
        private bool isFixed = false;

        /// <summary>
        /// Excelアクセッサー
        /// </summary>
        private ExcelAccessor excelAccessor = null;

        /// <summary>
        /// 使用高払いデータ
        /// </summary>
        private IList<Ukebarai> siyoudakabaraiData = null;

        /// <summary>
        /// 入庫払いデータ
        /// </summary>
        private IList<Ukebarai> nyukobaraiData = null;

        /// <summary>
        /// 印刷対象外の受払い(使用高払い)情報
        /// </summary>
        private IList<PaymentInfo> excludeUsePaymentInfo = null;

        /// <summary>
        /// 印刷対象外の受払い(入庫払い)情報
        /// </summary>
        private IList<PaymentInfo> excludeInputPaymentInfo = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataService">データサービス</param>
        public IssueCaluculationViewModel(IDataService dataService) : base(dataService)
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 使用高払い受払い計算書を印刷するか
        /// </summary>
        public bool PrintSiyoudaka
        {
            get { return printSiyoudaka; }
            set { Set(ref printSiyoudaka, value); }

        }
        /// <summary>
        /// 入庫払い受払い計算書を印刷するか
        /// </summary>
        public bool PrintNyuko
        {
            get { return printNyuko; }
            set { Set(ref printNyuko, value); }
        }

        /// <summary>
        /// 確定状態か
        /// </summary>
        public bool IsFixed
        {
            get { return isFixed; }
            set { Set(ref isFixed, value); }
        }

        /// <summary>
        /// 印刷対象のデータがあるか
        /// </summary>
        private bool hasPrintData
        {
            get
            {
                bool ret = true;

                if (PrintSiyoudaka && !PrintNyuko)
                {
                    if(siyoudakabaraiData.Count == 0) ret = false;
                }
                else if (!PrintSiyoudaka && PrintNyuko)
                {
                    if (nyukobaraiData.Count == 0) ret = false;
                }
                else if (PrintSiyoudaka && PrintNyuko)
                {
                    if ((siyoudakabaraiData.Count == 0) && (nyukobaraiData.Count == 0)) ret = false;
                }

                return ret;
            }
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

                // 使用高払い受払いデータの取得
                siyoudakabaraiData = await getSiyoudakabaraiData();

                // 入庫払い受払いデータの取得
                nyukobaraiData = await getNyukobaraiData();

                // 共有データに保持している印刷対象外の使用高払い受払い情報を内部のフィールドに移送
                excludeUsePaymentInfo = new List<PaymentInfo>();

                if (SharedModel.Instance.ExcludeUsePaymentInfo != null)
                {
                    foreach (var list in SharedModel.Instance.ExcludeUsePaymentInfo)
                    {
                        foreach (var item in list.Value)
                        {
                            excludeUsePaymentInfo.Add(item);
                        }
                    }
                }

                // 共有データに保持している印刷対象外の入庫払い受払い情報を内部のフィールドに移送
                excludeInputPaymentInfo = new List<PaymentInfo>();

                if (SharedModel.Instance.ExcludeInputPaymentInfo != null)
                {
                    foreach (var list in SharedModel.Instance.ExcludeInputPaymentInfo)
                    {
                        foreach (var item in list.Value)
                        {
                            excludeInputPaymentInfo.Add(item);
                        }
                    }
                }

                // 使用高払い受払いデータの再生成
                reCreateSiyoudakabaraiData(ref siyoudakabaraiData);

                // 入庫払い受払いデータの再生成
                reCreateNyuukoBaraiData(ref nyukobaraiData);

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
            string errorContent = string.Empty;
            string errorCode = string.Empty;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;

            if (!PrintSiyoudaka && !PrintNyuko)
            {
                MessageManager.ShowExclamation(SystemID.CKSI1010, "TargetNotChecked");
                return false;
            }

            try
            {
                BusyStatus.IsBusy = true;

                var tcs = new TaskCompletionSource<bool>();
                var thread = new Thread(async () =>
                {
                    try
                    {
                        // 印刷実行
                        await Print();
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
                if (PrintSiyoudaka || PrintNyuko)
                {
                    if (hasPrintData)
                    {
                        LogType logType = hasException ? LogType.Error : LogType.Normal;
                        StringBuilder sb = new StringBuilder();
                        if (PrintSiyoudaka && siyoudakabaraiData.Count > 0)
                        {
                            sb.Append(USE_PAYMENT);
                            if (PrintNyuko) sb.Append(Constants.Comma_StringDefine + INPUT_PAYMENT);
                        }
                        else if(PrintNyuko && nyukobaraiData.Count > 0)
                        {
                            if (PrintNyuko) sb.Append(INPUT_PAYMENT);
                        }
                        string bikou = sb.ToString();

                        // 棚卸ログの挿入
                        await InsertTanaorosiLog(logType, SharedModel.Instance.WorkKbn,
                                                 LogOperationType.IssueCaluculation,
                                                 LogOperationContent.Print, bikou, errorContent, errorCode);
                        if (!hasException)
                        {
                            MessageManager.ShowInformation(SystemID.CKSI1010, "PrintCompleted");
                        }
                        else
                        {
                            MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                        }
                    }
                    else
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "NotTargetPrintData");
                    }
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
            string errorContent = string.Empty;
            string errorCode = string.Empty;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;

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
                                         LogOperationType.IssueCaluculation,
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
        /// 終了
        /// </summary>
        internal override async Task<bool> ExitAsync()
        {
            if (!IsFixed)
            {
                MessageManager.ShowExclamation(SystemID.CKSI1010, "CanNotWorkCompleted");
                return false;
            }

            else
            {
                MessageManager.ShowInformation(SystemID.CKSI1010, "TanaorosiWorkCompleted");
                return await base.ExitAsync();
            }
        }

        /// <summary>
        /// 使用高払い受払いデータの取得
        /// </summary>
        private async Task<IList<Ukebarai>> getSiyoudakabaraiData()
        {
            IList<Ukebarai> siyoudakabarai = null;

            try
            {
                // 使用高払い受払いデータの取得
                siyoudakabarai = (await DataService.GetSiyoudakaData()).ToList();
            }

            catch (Exception)
            {
                throw;
            }

            return siyoudakabarai;
        }

        /// <summary>
        /// 入庫払い受払データ取得
        /// </summary>
        private async Task<IList<Ukebarai>> getNyukobaraiData()
        {

            IList<Ukebarai> nyukobarai = null;

            try
            {
                // 入庫払い受払いデータの取得
                nyukobarai = (await DataService.GetNyukoData()).ToList();
            }

            catch (Exception)
            {
                throw;
            }

            return nyukobarai;
        }

        /// <summary>
        /// 使用高払い受払いデータの再生成
        /// </summary>
        /// <param name="siyoudakaBarai">使用高払い受払いデータ</param>
        private void reCreateSiyoudakabaraiData(ref IList<Ukebarai> siyoudakaBarai)
        {
            try
            {
                IList<PaymentInfo> rePaymentInfo = new List<PaymentInfo>();
                IList<Ukebarai> reIssueCalculationModel = new List<Ukebarai>();

                foreach (var item in excludeUsePaymentInfo)
                {
                    rePaymentInfo.Add(item);
                }

                foreach (var item in siyoudakaBarai)
                {
                    var exclude
                        = rePaymentInfo.FirstOrDefault
                        (n => (n.Himoku == item.Himoku)
                           && (n.Utiwake == item.Utiwake)
                           && (n.Tanaban == item.Tanaban)
                           && (n.HinmokuCode == item.HinmokuCode)
                           && (n.KouzaName == item.KouzaName));

                    if (exclude == null)
                    {
                        reIssueCalculationModel.Add(item);
                    }
                }

                siyoudakaBarai = new List<Ukebarai>(reIssueCalculationModel);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 入庫払い受払いデータの再生成
        /// </summary>
        /// <param name="nyukoBarai">入庫払い受払いデータ</param>
        private void reCreateNyuukoBaraiData(ref IList<Ukebarai> nyukoBarai)
        {
            try
            {
                IList<PaymentInfo> rePaymentInfo = new List<PaymentInfo>();
                IList<Ukebarai> reIssueCalculationModel = new List<Ukebarai>();

                foreach (var item in excludeInputPaymentInfo)
                {
                    rePaymentInfo.Add(item);
                }

                foreach (var item in nyukoBarai)
                {
                    var exclude
                        = rePaymentInfo.FirstOrDefault
                        (n => (n.Himoku == item.Himoku)
                           && (n.Utiwake == item.Utiwake)
                           && (n.Tanaban == item.Tanaban));

                    if (exclude == null)
                    {
                        reIssueCalculationModel.Add(item);
                    }
                }

                nyukoBarai = new List<Ukebarai>(reIssueCalculationModel);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 印刷
        /// </summary>
        public async Task<bool> Print()
        {
            try
            {
                excelAccessor = ExcelAccessor.GetInstance();

                if (PrintSiyoudaka)
                {
                    IList<UkebaraiDTO> outputUkebaraiList = new List<UkebaraiDTO>();

                    try
                    {
                        foreach (var item in siyoudakabaraiData)
                        {
                            var ukebarai = new UkebaraiDTO();
                            await CopyProperties(item, out ukebarai);
                            outputUkebaraiList.Add(ukebarai);
                        }

                        if (hasPrintData && siyoudakabaraiData.Count > 0)
                        {
                            // 使用高払い受払い計算書の出力
                            await excelAccessor.OutputSiyoudakabaraiUkebarai(outputUkebaraiList);
                        }
                    }

                    catch (Exception)
                    {
                        throw;
                    }
                }

                if (PrintNyuko)
                {
                    IList<UkebaraiDTO> outputNyukoList = new List<UkebaraiDTO>();

                    try
                    {
                        foreach (var item in nyukobaraiData)
                        {
                            var nyukobarai = new UkebaraiDTO();
                            await CopyProperties(item, out nyukobarai);
                            outputNyukoList.Add(nyukobarai);
                        }

                        if (hasPrintData && nyukobaraiData.Count > 0)
                        {
                            // 入庫払い受払い計算書の出力
                            await excelAccessor.OutputNyukobaraiUkebarai(outputNyukoList);
                        }
                    }

                    catch (Exception)
                    {
                        throw;
                    }
                }

                return await Task.FromResult(true);
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}

    
