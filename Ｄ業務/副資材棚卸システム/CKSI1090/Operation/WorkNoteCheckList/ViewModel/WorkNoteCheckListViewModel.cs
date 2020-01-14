using CKSI1090.Common;
using CKSI1090.Common.Excel;
using CKSI1090.Common.Excel.DTO;
using CKSI1090.Operation.Common.ViewModel;
using CKSI1090.Shared;
using CKSI1090.Shared.Model;
using Common;
using GalaSoft.MvvmLight.Command;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using ViewModel;
using static Common.MessageManager;
//*************************************************************************************
//
//   作業誌チェックリストの画面ビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1090.Operation.WorkNoteCheckList.ViewModel
{
    /// <summary>
    /// 作業誌チェックリストの画面ビューモデル
    /// </summary>
    public class WorkNoteCheckListViewModel : OperationViewModelBase, IWorkNoteRecordContainer
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

        /// <summary>
        /// レコード件数が0か
        /// </summary>
        private bool isNoRecord = false;

        /// <summary>
        /// 検索可能か
        /// </summary>
        private bool canSearch = false;

        /// <summary>
        /// 編集可能か
        /// </summary>
        private bool canEdit = false;

        /// <summary>
        /// 資材事務所スタッフか
        /// </summary>
        private bool isSizaiOfficeStaff = false;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataService">データサービス</param>
        public WorkNoteCheckListViewModel(IDataService dataService) : base(dataService)
        {
            try
            {
                IsNoRecord = true;

                SearchCondition = new SearchConditionViewModel();
                SearchCondition.UseOperationDate = true;
                DateTime date = DateTime.Today;
                date = date.AddDays(-1);
                SearchCondition.MinOperationDate = date.ToString("yyMMdd");
                WorkNoteComboList = new ObservableCollection<ComboBoxViewModel>();
                MukesakiComboList = new ObservableCollection<ComboBoxViewModel>();
                WorkNoteRecords = new ObservableCollection<WorkNoteRecordViewModel>();
                WorkNoteRecordModification = new WorkNoteRecordModificationViewModel();

                // 複数のスレッドからのコレクションのアクセスを有効にする
                BindingOperations.EnableCollectionSynchronization(WorkNoteRecords, new object());
                // XML操作オブジェクトのインスタンスの取得
                xmlOperator = XMLOperator.GetInstance();
                // Excelアクセッサーのインスタンスの取得
                excelAccessor = ExcelAccessor.GetInstance();

                // 作業誌区分のコンボボックスの生成
                createWorkNoteComboList();
                // 向先のコンボボックスの生成
                createMukesakiComboList();
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1090, "FatalError");
            }
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// レコード件数が0か
        /// </summary>
        public bool IsNoRecord
        {
            get { return isNoRecord; }
            set { Set(ref this.isNoRecord, value); }
        }

        /// <summary>
        /// 検索可能か
        /// </summary>
        public bool CanSearch
        {
            get { return canSearch; }
            set { Set(ref this.canSearch, value); }
        }

        /// <summary>
        /// 編集可能か
        /// </summary>
        public bool CanEdit
        {
            get { return canEdit; }
            set { Set(ref this.canEdit, value); }
        }

        /// <summary>
        /// 資材事務所スタッフか
        /// </summary>
        public bool IsSizaiOfficeStaff
        {
            get { return isSizaiOfficeStaff; }
            set { Set(ref this.isSizaiOfficeStaff, value); }
        }

        /// <summary>
        /// 検索条件
        /// </summary>
        public SearchConditionViewModel SearchCondition
        {
            get;
            private set;
        }

        /// <summary>
        /// 作業誌区分
        /// </summary>
        public ObservableCollection<ComboBoxViewModel> WorkNoteComboList
        {
            get;
            private set;
        }

        /// <summary>
        /// 向先
        /// </summary>
        public ObservableCollection<ComboBoxViewModel> MukesakiComboList
        {
            get;
            private set;
        }

        /// <summary>
        /// 作業誌データ一覧
        /// </summary>
        public ObservableCollection<WorkNoteRecordViewModel> WorkNoteRecords
        {
            get;
            private set;
        }

        /// <summary>
        /// 作業誌チェックリストのレコードの変更状態
        /// </summary>
        public WorkNoteRecordModificationViewModel WorkNoteRecordModification
        {
            get;
            private set;
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
                IsSizaiOfficeStaff = SharedViewModel.Instance.IsSizaiOfficeStaff;
                await base.InitializeAsync();
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1090, "FatalError");
            }

        }

        /// <summary>
        /// ×ボタン
        /// </summary>
        internal override async Task<bool> CloseAsync()
        {
            try
            {
                return await base.CloseAsync();
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1090, "FatalError");

                return false;
            }

        }

        /// <summary>
        /// 検索
        /// </summary>
        private async Task<bool> search()
        {
            try
            {
                WorkNoteRecords.Clear();

                // SearchCondition(Model)への移送
                SearchCondition condition = transferSearchConditionModel(SearchCondition);

                // 作業誌一覧の取得
                var list = await DataService.GetWorkNoteList(condition);

                foreach (var item in list)
                {
                    WorkNoteRecordViewModel record = new WorkNoteRecordViewModel(this);
                    record.IsChecked = item.Approval;
                    record.OperationDate = item.OperationDate;
                    record.Seq = item.Seq;
                    record.WorkNoteType = item.WorkNoteType;
                    record.HinmokuCode = item.HinmokuCode;
                    record.GyosyaCode = item.GyosyaCode;
                    record.Mukesaki = item.Mukesaki;
                    record.HinmokuName = item.HinmokuName;
                    record.GyosyaName = item.GyosyaName;
                    record.Amount = item.Amount;
                    record.Suibunritu = item.Suibunritu;
                    WorkNoteRecords.Add(record);
                }

                IsNoRecord = WorkNoteRecords.Count == 0 ? true : false;

                CanEdit = false;

                foreach (var item in WorkNoteRecords)
                {
                    item.IsDirty = false;

                    if (!(int.Parse(item.OperationDate.Substring(0, 6)) < SharedViewModel.Instance.OperationYearMonth.YearMonth))
                    {
                        CanEdit = true;
                    }
                }

                WorkNoteRecordModification.IsDirty = false;
            }

            catch (Exception)
            {
                throw;
            }

            return await Task.FromResult(true);

        }

        /// <summary>
        /// 更新
        /// </summary>
        private async Task<bool> update()
        {
            IList<WorkNoteItem> models = new List<WorkNoteItem>();

            try
            {
                var list = WorkNoteRecords.Where(record => record.IsDirty);

                foreach (var item in list)
                {
                    WorkNoteItem model = new WorkNoteItem();
                    model.OperationDate = item.OperationDate;
                    model.Seq = item.Seq;
                    model.WorkNoteType = item.WorkNoteType;
                    model.HinmokuCode = item.HinmokuCode;
                    model.GyosyaCode = item.GyosyaCode;
                    model.Mukesaki = item.Mukesaki;
                    model.HinmokuName = item.HinmokuName;
                    model.GyosyaName = item.GyosyaName;
                    model.Amount = item.Amount;
                    model.Suibunritu = item.Suibunritu;
                    model.Approval = item.IsChecked;
                    models.Add(model);
                }

                // 作業誌一覧の更新
                await DataService.UpdateWorkNoteList(models);

                foreach (var item in list)
                {
                    item.IsDirty = false;
                }

                WorkNoteRecordModification.IsDirty = false;

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
            IList<WorkNoteCheckListDTO> outputList = new List<WorkNoteCheckListDTO>();

            try
            {
                foreach (var item in WorkNoteRecords)
                {
                    var excelItem = new WorkNoteCheckListDTO();
                    excelItem.IsChecked = item.IsChecked;
                    excelItem.OperationDate = item.OperationDate;
                    excelItem.WorkNoteType = item.WorkNoteType;
                    excelItem.HinmokuCode = item.HinmokuCode;
                    excelItem.GyosyaCode = item.GyosyaCode;
                    excelItem.Mukesaki = item.Mukesaki;
                    excelItem.HinmokuName = item.HinmokuName;
                    excelItem.GyosyaName = item.GyosyaName;
                    excelItem.Amount = item.Amount;
                    excelItem.Suibunritu = item.Suibunritu;
                    outputList.Add(excelItem);
                }

                // 作業誌チェックリストの出力
                await excelAccessor.OutputSagyosiCheckList(outputList);
            }

            catch (Exception)
            {
                throw;
            }

            return await Task.FromResult(true);

        }

        /// <summary>
        /// 作業誌区分のコンボボックスの生成
        /// </summary>
        private void createWorkNoteComboList()
        {
            try
            {
                // 作業誌区分の取得
                SharedViewModel.Instance.WorkNoteKbn = this.xmlOperator.GetWorkNoteKbn();

                foreach (var dict in SharedViewModel.Instance.WorkNoteKbn)
                {
                    ComboBoxViewModel item = new ComboBoxViewModel();
                    item.Key = dict.Key;
                    item.Value = dict.Value;

                    WorkNoteComboList.Add(item);
                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 向先のコンボボックスの生成
        /// </summary>
        private void createMukesakiComboList()
        {
            try
            {
                // 向先の取得
                SharedViewModel.Instance.Mukesaki = this.xmlOperator.GetMukesakiType();
                foreach (var dict in SharedViewModel.Instance.Mukesaki)
                {
                    ComboBoxViewModel item = new ComboBoxViewModel();
                    item.Key = dict.Key;
                    item.Value = dict.Value;

                    MukesakiComboList.Add(item);
                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 検索可能かをチェックする
        /// </summary>
        /// <returns>チェック結果</returns>
        private bool checkCanSearch()
        {
            try
            {
                if (SearchCondition.UseOperationDate) return true;
                if (SearchCondition.UseWorkNoteKbn) return true;
                if (SearchCondition.UseHinmokuCode) return true;
                if (SearchCondition.UseGyosyaCode) return true;
                if (SearchCondition.UseMukesaki) return true;
                if (SearchCondition.UseApproval) return true;

                MessageManager.ShowExclamation(SystemID.CKSI1090, "NotSetSearchCondition");
            }

            catch (Exception)
            {
                throw;
            }

            return false;
        }

        /// <summary>
        /// 検索条件の入力チェック
        /// </summary>
        /// <returns>チェック結果</returns>
        private bool checkSearchCondition()
        {
            bool ret = false;
            int checkResult = InputChecker.No_Error;

            try
            {
                // 作業日を検索に使用するか
                if (SearchCondition.UseOperationDate)
                {
                    // 作業日の入力チェック
                    checkResult = InputChecker.CheckOperationDate
                        (SearchCondition.MinOperationDate, SearchCondition.MaxOperationDate);

                    if (checkResult != InputChecker.No_Error)
                    {
                        switch (checkResult)
                        {
                            // 未入力
                            case InputChecker.Not_Input:
                                MessageManager.ShowExclamation(SystemID.CKSI1090, "NotInput", Constants.OperationDate_StringDefine);
                                break;
                            // 文字列桁不足
                            case InputChecker.Str_Not_Enough_Length:
                                MessageManager.ShowExclamation
                                    (SystemID.CKSI1090, "NotEnoughLength",
                                        Constants.OperationDate_StringDefine, Convert.ToString(InputChecker.OperationDate_Length));
                                break;
                            default:
                                break;
                        }

                        return ret;
                    }
                }

                // 作業誌区分を検索に使用するか
                if (SearchCondition.UseWorkNoteKbn)
                {
                    // 作業誌区分の入力チェック
                    checkResult = InputChecker.CheckWorkNoteKbn(SearchCondition.WorkNoteKbn);

                    if (checkResult != InputChecker.No_Error)
                    {
                        switch (checkResult)
                        {
                            // 未選択
                            case InputChecker.Not_Select:
                                MessageManager.ShowExclamation(SystemID.CKSI1090, "NotSelect", Constants.WorkNoteKbn_StringDefine);
                                break;
                            default:
                                break;
                        }

                        return ret;
                    }
                }

                // 品目コードを検索に使用するか
                if (SearchCondition.UseHinmokuCode)
                {
                    // 品目コードの入力チェック
                    checkResult = InputChecker.CheckHinmokuCode
                        (SearchCondition.MinHinmokuCode, SearchCondition.MaxHinmokuCode);

                    if (checkResult != InputChecker.No_Error)
                    {
                        switch (checkResult)
                        {
                            // 未入力
                            case InputChecker.Not_Input:
                                MessageManager.ShowExclamation(SystemID.CKSI1090, "NotInput", Constants.HinmokuCode_StringDefine);
                                break;
                            // 文字列桁不足
                            case InputChecker.Str_Not_Enough_Length:
                                MessageManager.ShowExclamation
                                    (SystemID.CKSI1090, "NotEnoughLength",
                                        Constants.HinmokuCode_StringDefine, Convert.ToString(InputChecker.HinmokuCode_Length));
                                break;
                            default:
                                break;
                        }

                        return ret;
                    }
                }

                // 業者コードを検索に使用するか
                if (SearchCondition.UseGyosyaCode)
                {
                    // 業者コードの入力チェック
                    checkResult = InputChecker.CheckGyosyaCode
                        (SearchCondition.MinGyosyaCode, SearchCondition.MaxGyosyaCode);

                    if (checkResult != InputChecker.No_Error)
                    {
                        switch (checkResult)
                        {
                            // 未入力
                            case InputChecker.Not_Input:
                                MessageManager.ShowExclamation(SystemID.CKSI1090, "NotInput", Constants.GyosyaCode_StringDefine);
                                break;
                            // 文字列桁不足
                            case InputChecker.Str_Not_Enough_Length:
                                MessageManager.ShowExclamation
                                    (SystemID.CKSI1090, "NotEnoughLength",
                                        Constants.GyosyaCode_StringDefine, Convert.ToString(InputChecker.GyousyaCode_Length));
                                break;
                            default:
                                break;
                        }

                        return ret;
                    }
                }

                // 向先を検索に使用するか
                if (SearchCondition.UseMukesaki)
                {
                    // 向先の入力チェック
                    checkResult = InputChecker.CheckMukesaki(SearchCondition.Mukesaki);

                    if (checkResult != InputChecker.No_Error)
                    {
                        switch (checkResult)
                        {
                            // 未選択
                            case InputChecker.Not_Select:
                                MessageManager.ShowExclamation(SystemID.CKSI1090, "NotSelect", Constants.Mukesaki_StringDefine);
                                break;
                            default:
                                break;
                        }

                        return ret;
                    }
                }

                ret = true;
            }

            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 検索条件モデルへの移送
        /// </summary>
        /// <param name="viewModel">検索条件ビューモデル</param>
        /// <returns>検索条件モデル</returns>
        private SearchCondition transferSearchConditionModel(SearchConditionViewModel viewModel)
        {
            SearchCondition condition = new SearchCondition();

            try
            {
                // 作業日を検索に使用するか
                condition.UseOperationDate = viewModel.UseOperationDate;
                // 作業日(最小値)
                condition.MinOperationDate = viewModel.MinOperationDate;
                // 作業日(最大値)
                condition.MaxOperationDate = viewModel.MaxOperationDate;

                if (viewModel.UseOperationDate)
                {
                    if (string.IsNullOrEmpty(viewModel.MinOperationDate)
                            || string.IsNullOrEmpty(viewModel.MaxOperationDate))
                    {
                        if (string.IsNullOrEmpty(viewModel.MinOperationDate))
                        {
                            // 作業日の検索種類(最大)
                            condition.OperationDateSearchType = Constants.OperationDateSearchTypes.Max;
                        }
                        else
                        {
                            // 作業日の検索種類(最小)
                            condition.OperationDateSearchType = Constants.OperationDateSearchTypes.Min;
                        }
                    }
                    else
                    {
                        // 作業日の検索種類(最小～最大)
                        condition.OperationDateSearchType = Constants.OperationDateSearchTypes.MinMax;
                    }
                }

                // 作業誌区分を検索に使用するか
                condition.UseWorkNoteKbn = viewModel.UseWorkNoteKbn;
                // 作業誌区分
                condition.WorkNoteKbn = viewModel.WorkNoteKbn;

                // 品目コードを検索に使用するか
                condition.UseHinmokuCode = viewModel.UseHinmokuCode;
                // 品目コード(最小値)
                condition.MinHinmokuCode = viewModel.MinHinmokuCode;
                // 品目コード(最大値)
                condition.MaxHinmokuCode = viewModel.MaxHinmokuCode;

                if (viewModel.UseHinmokuCode)
                {
                    if (string.IsNullOrEmpty(viewModel.MinHinmokuCode)
                            || string.IsNullOrEmpty(viewModel.MaxHinmokuCode))
                    {
                        if (string.IsNullOrEmpty(viewModel.MinHinmokuCode))
                        {
                            // 品目コードの検索種類(最大)
                            condition.HinmokuCodeSearchType = Constants.HinmokuCodeSearchTypes.Max;
                        }
                        else
                        {
                            // 品目コードの検索種類(最小)
                            condition.HinmokuCodeSearchType = Constants.HinmokuCodeSearchTypes.Min;
                        }
                    }
                    else
                    {
                        // 品目コードの検索種類(最小～最大)
                        condition.HinmokuCodeSearchType = Constants.HinmokuCodeSearchTypes.MinMax;
                    }
                }

                // 業者コードを検索に使用するか
                condition.UseGyosyaCode = viewModel.UseGyosyaCode;
                // 業者コード(最小)
                condition.MinGyosyaCode = viewModel.MinGyosyaCode;
                // 業者コード(最大)
                condition.MaxGyosyaCode = viewModel.MaxGyosyaCode;

                if (viewModel.UseGyosyaCode)
                {
                   if (string.IsNullOrEmpty(viewModel.MinGyosyaCode)
                                && string.IsNullOrEmpty(viewModel.MaxGyosyaCode))
                    {
                        // 業者コードの検索種類(空白)
                        condition.GyosyaCodeSearchType = Constants.GyosyaCodeSearchTypes.Blank;
                        condition.MinGyosyaCode = condition.MinGyosyaCode.PadRight(4);
                        condition.MaxGyosyaCode = condition.MaxGyosyaCode.PadRight(4);
                    }

                    else if (string.IsNullOrEmpty(viewModel.MinGyosyaCode)
                            || string.IsNullOrEmpty(viewModel.MaxGyosyaCode))
                    {
                        if (string.IsNullOrEmpty(viewModel.MinGyosyaCode))
                        {
                            // 業者コードの検索種類(最大)
                            condition.GyosyaCodeSearchType = Constants.GyosyaCodeSearchTypes.Max;
                        }
                        else
                        {
                            // 業者コードの検索種類(最小)
                            condition.GyosyaCodeSearchType = Constants.GyosyaCodeSearchTypes.Min;
                        }
                    }

                    else
                    {
                        // 業者コードの検索種類(最小～最大)
                        condition.GyosyaCodeSearchType = Constants.GyosyaCodeSearchTypes.MinMax;
                    }
                }

                // 向先を検索に使用するか
                condition.UseMukesaki = viewModel.UseMukesaki;
                // 向先
                condition.Mukesaki = viewModel.Mukesaki;

                // 承認を検索に使用するか
                condition.UseApproval = viewModel.UseApproval;
                // 承認
                condition.Approval = viewModel.Approval;
            }

            catch (Exception)
            {
                throw;
            }

            return condition;

        }

        #endregion

        #region コマンド

        /// <summary>
        /// 検索コマンド
        /// </summary>
        public ICommand FireSearch => new RelayCommand(async () =>
        {
            bool checkResult = false;

            try
            {
                BusyStatus.IsBusy = true;

                var tcs = new TaskCompletionSource<bool>();

                var thread = new Thread(async () =>
                {
                    try
                    {
                        // 検索可能かをチェック
                        CanSearch = checkCanSearch();

                        if (CanSearch)
                        {
                            // 検索条件の入力チェック
                            checkResult = checkSearchCondition();
                            if (checkResult)
                            {
                                // 検索実行
                                await search();
                            }
                        }
                        
                        tcs.SetResult(true);
                    }

                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                });

                thread.SetApartmentState(ApartmentState.MTA);
                thread.Start();

                await tcs.Task;

                BusyStatus.IsBusy = false;

                if(checkResult)
                { 
                    if (IsNoRecord)
                    {
                        MessageManager.ShowInformation(SystemID.CKSI1090, "DataNotFound");
                    }
                }
            }

            catch (Exception e)
            {
                BusyStatus.IsBusy = false;
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1090, "FatalError");
            }

        });

        /// <summary>
        /// 更新コマンド
        /// </summary>
        public ICommand FireUpdate => new RelayCommand(async () =>
        {
            try
            {
                BusyStatus.IsBusy = true;

                if (MessageManager.ShowQuestion(SystemID.CKSI1090, "ConfirmUpdate") == ResultType.Yes)
                {
                    // 更新実行
                    await update();
                    MessageManager.ShowInformation(SystemID.CKSI1090, "CompletedUpdate");
                }
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1090, "FatalError");
            }

            finally
            {
                BusyStatus.IsBusy = false;
            }
        });

        /// <summary>
        /// 全選択コマンド
        /// </summary>
        public ICommand FireSelectAll => new RelayCommand(() =>
        {
            try
            {
                BusyStatus.IsBusy = true;

                foreach (var item in WorkNoteRecords)
                {
                    if (!(int.Parse(item.OperationDate.Substring(0, 6)) < SharedViewModel.Instance.OperationYearMonth.YearMonth))
                    {
                        item.IsChecked = true;
                    }
                }
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1090, "FatalError");
            }

            finally
            {
                BusyStatus.IsBusy = false;
            }
        });

        /// <summary>
        /// 全解除コマンド
        /// </summary>
        public ICommand FireDeSelectAll => new RelayCommand(() =>
        {
            try
            {
                BusyStatus.IsBusy = true;

                foreach (var item in WorkNoteRecords)
                {
                    if (!(int.Parse(item.OperationDate.Substring(0, 6)) < SharedViewModel.Instance.OperationYearMonth.YearMonth))
                    {
                        item.IsChecked = false;
                    }
                }
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1090, "FatalError");
            }

            finally
            {
                BusyStatus.IsBusy = false;
            }
        });

        /// <summary>
        /// 印刷コマンド
        /// </summary>
        public ICommand FirePrint => new RelayCommand(async () =>
        {
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
                MessageManager.ShowInformation(SystemID.CKSI1090, "CompletedPrint");
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());

                if (e is ExcelInsertPageLimitOverException)
                {
                    MessageManager.ShowError(SystemID.CKSI1090, "ExcelInsertPageLimitOver");
                }

                else
                {
                    MessageManager.ShowError(SystemID.CKSI1090, "FatalError");
                }
            }

            finally
            {
                BusyStatus.IsBusy = false;
            }
        });
        #endregion

        #region インターフェース

        /// <summary>
        /// 作業誌チェックリストのレコードの編集通知
        /// </summary>
        public void NotifyWorkNoteRecordModified()
        {
            WorkNoteRecordModification.IsDirty = WorkNoteRecords.Any(record => record.IsDirty);
        }
        #endregion
    }

}
