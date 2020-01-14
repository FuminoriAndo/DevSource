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
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static CKSI1010.Common.Constants;
using static Common.MessageManager;
//*************************************************************************************
//
//   資材品目の変更履歴画面のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.ShowSizaiHinmokuChangeList.ViewModel
{
    /// <summary>
    /// 資材品目の変更履歴画面のビューモデル
    /// </summary>
    public class ShowSizaiHinmokuChangeListViewModel : OperationViewModelBase
    {

        #region フィールド

        /// <summary>
        /// XML操作オブジェクト
        /// </summary>
        private XMLOperator xmlOperator = null;

        /// <summary>
        /// Excelアクセッサー
        /// </summary>
        private ExcelAccessor excelAccessor = null;

        #endregion

        #region プロパティ

        /// <summary>
        /// 実施年月
        /// </summary>
        public OperationYearMonth OperationYearMonth => SharedViewModel.Instance.OperationYearMonth;

        /// <summary>
        /// 資材品目マスタの変更履歴一覧
        /// </summary>
        public ObservableCollection<SizaiHinmokuRecordViewModel> SizaiHinmokuRecords
        {
            get;
            private set;
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataService"></param>
        public ShowSizaiHinmokuChangeListViewModel(IDataService dataService) : base(dataService)
        {
            SizaiHinmokuRecords = new ObservableCollection<SizaiHinmokuRecordViewModel>();

            // XML操作オブジェクトのインスタンスの取得
            xmlOperator = XMLOperator.GetInstance();
            // 向先の種類の取得
            SharedViewModel.Instance.Mukesaki = this.xmlOperator.GetMukesakiType();
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
                BusyStatus.IsBusy = true;

                await UpdateOperationCondition(OperationCondition.Fix, false);

                await search();

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
        /// 印刷
        /// </summary>
        internal override async Task<bool> PrintAsync()
        {
            bool hasException = false;
            string errorContent = null;

            try
            {
                BusyStatus.IsBusy = true;

                var tcs = new TaskCompletionSource<bool>();
                var thread = new Thread(async () =>
                {

                    try
                    {
                        // 印刷実行
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
                return await base.PrintAsync();
            }

            catch (Exception e)
            {
                hasException = true;
                errorContent = e.Message;

                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                return false;
            }

            finally
            {
                LogType logType = hasException ? LogType.Error : LogType.Normal;

                // 棚卸ログの挿入
                await InsertTanaorosiLog(logType, SizaiWorkCategory.TanaoroshiWork,
                                         LogOperationType.ShowSizaiHinmokuChangeList,
                                         LogOperationContent.Print, null, errorContent, null);

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
        /// 次へ
        /// </summary>
        internal override async Task<bool> GoNextAsync()
        {
            try
            {
                BusyStatus.IsBusy = true;

                // 品目マスタ存在チェック処理
                var noHinmokuList = await DataService.CheckHinmokuMaster();

                // 品目マスタにないデータが存在するか
                if (noHinmokuList.Count() > 0)
                {
                    string notHinmoku = Constants.HinmokuCode_StringDefine + noHinmokuList.FirstOrDefault().Item1;
                    // 品目マスタに存在しない品目コードのメッセージを表示する
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "NotHasHinmokuCodeOfSizai_Hinmoku_Mst", notHinmoku);
                }

                return await base.GoNextAsync();
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
        /// 検索
        /// </summary>
        private async Task<bool> search()
        {
            try
            {
                SizaiHinmokuRecords.Clear();

                string tanaYm = OperationYearMonth.YearMonth.ToString();

                // 当月の資材品目(マスタ)の変更データを取得する
                var list = await DataService.GetSizaiHinmokuChangeData(tanaYm);

                foreach (var item in list)
                {
                    SizaiHinmokuRecordViewModel record = new SizaiHinmokuRecordViewModel();
                    await CopyProperties(item, out record);
                    SizaiHinmokuRecords.Add(record);
                }

                if(SizaiHinmokuRecords.Count == 0)
                {
                    // 現在の操作を操作不可にする
                    CommonUtility.ExcuteCanNotOperation();
                    MessageManager.ShowInformation(SystemID.CKSI1010, "NothingSizaiHinmokuChangeList");
                }
            }

            catch (Exception)
            {
                throw;
            }

            return await Task.FromResult(true);
        }

        /// <summary>
        /// 印刷
        /// </summary>
        private async Task<bool> print()
        {
            IList<SizaiHinmokuDTO> outputList = new List<SizaiHinmokuDTO>();

            try
            {
                foreach (var item in SizaiHinmokuRecords)
                {
                    var excelItem = new SizaiHinmokuDTO();
                    await CopyProperties(item, out excelItem);
                    outputList.Add(excelItem);
                }

                // Excelアクセッサーのインスタンスの取得
                excelAccessor = ExcelAccessor.GetInstance();
                // 資材品目マスタの変更履歴の出力
                await excelAccessor.OutputSizaiHinmokuChangeList(outputList);
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