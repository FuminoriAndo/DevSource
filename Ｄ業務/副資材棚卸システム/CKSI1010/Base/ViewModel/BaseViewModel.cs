using CKSI1010.Common;
using CKSI1010.Core;
using CKSI1010.Operation.Common.View;
using CKSI1010.Operation.SelectInputWork.View;
using CKSI1010.Operation.SelectInputWork.ViewModel;
using CKSI1010.Shared;
using CKSI1010.Shared.Model;
using CKSI1010.Shared.ViewModel;
using Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.XPath;
using static CKSI1010.Common.Constants;
using static Common.MessageManager;
using CKSI1010.Operation.SettingDisplay.ViewModel;
//*************************************************************************************
//
//   ベース画面のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Base.ViewModel
{
    /// <summary>
    /// ベース画面のビューモデル
    /// </summary>
    public class BaseViewModel : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// データサービス
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// 現在の棚卸操作
        /// </summary>
        private Common.Operation currentOperation;

        /// <summary>
        /// 棚卸操作用ユーザーコントロールを操作するためのインターフェース
        /// </summary>
        private IOperationViewBase operation;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataService">データサービス</param>
        public BaseViewModel(IDataService dataService)
        {
            this.dataService = dataService;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 操作一覧
        /// </summary>
        public ObservableCollection<Common.Operation> Operations => SharedViewModel.Instance.Operations;

        /// <summary>
        /// アプリケーションの状態
        /// </summary>
        public BusyStatus BusyStatus => SharedViewModel.Instance.BusyStatus;

        /// <summary>
        /// モーダル画面表示状態
        /// </summary>
        private bool isShowModal = false;

        /// <summary>
        /// 現在の操作
        /// </summary>
        public Common.Operation CurrentOperation
        {
            get { return currentOperation; }
            set
            {
                var lastOperation = currentOperation;

                if (!Set(ref currentOperation, value))
                {
                    return;
                }

                var userControl = (UserControl)Activator.CreateInstance(CurrentOperation.UserControl);
                operation = (IOperationViewBase)userControl;
                SharedViewModel.Instance.CurrentOperation = currentOperation;
                operation.InitializeAsync();

                Messenger.Default.Send(new ChangeOperationNotificationMessage(this)
                {
                    Operation = userControl
                });

                if (lastOperation != null)
                {
                    lastOperation.IsSelected = false;
                }

                CurrentOperation.IsSelected = true;
            }
        }

        /// <summary>
        /// モーダル表示
        /// </summary>
        public bool IsShowModal
        {
            get { return isShowModal; }
            set { Set(ref isShowModal, value); }
        }

        #endregion

        #region コマンド

        /// <summary>
        /// ロードイベントの発火
        /// </summary>
        public ICommand FireLoaded => new RelayCommand(async () =>
        {
            try
            {
                BusyStatus.IsBusy = true;

                // 棚卸ログ用フォルダの存在チェック(なければフォルダを作成する)
                if (!CommonUtility.ExistTanaorosiLogFolder()) CommonUtility.CreateTanaorosiLogFolder();

                // 期数の算出
                IEnumerable<InventoryTerm> periods = await dataService.GetInventoryPeriodsAsync(SystemCategory.Sizai.GetStringValue());

                // 定義ファイルのオープン
                var settings = Path.Combine(Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");
                var doc = XDocument.Load(settings);

                // 期末月の読込み
                SharedViewModel.Instance.OperationYearMonth.TermEndMonths.AddRange(
                    doc.XPathSelectElements("/Settings/EndTerms/EndTerm").Select(element => (int)element.Attribute("Month")));

                // 実施年月
                SharedViewModel.Instance.OperationYearMonth.YearMonth = int.Parse(periods.First().YearMonth);
                // 実施年
                SharedViewModel.Instance.OperationYearMonth.Year = int.Parse(periods.First().YearMonth.Substring(0, 4));
                // 実施月
                SharedViewModel.Instance.OperationYearMonth.Month = int.Parse(periods.First().YearMonth.Substring(4, 2));
                // 期数
                SharedViewModel.Instance.OperationYearMonth.Term = periods.First().Term;
                // 上期/下期
                SharedViewModel.Instance.OperationYearMonth.Half = periods.First().Half;

                // 各種データの取込み
                // 業者マスタ
                SharedModel.Instance.Suppliers.Clear();
                SharedModel.Instance.Suppliers.AddRange(await dataService.GetSuppliersAsync());

                // 先月棚卸データ
                var lastMonth = new DateTime(SharedViewModel.Instance.OperationYearMonth.Year, SharedViewModel.Instance.OperationYearMonth.Month, 1).AddMonths(-1);
                SharedModel.Instance.LastInventories.Clear();
                SharedModel.Instance.LastInventories.AddRange(await dataService.GetInventoriesAsync(lastMonth.ToString("yyyyMM")));

                // 入庫データ
                SharedModel.Instance.Receivings.Clear();
                SharedModel.Instance.Receivings.AddRange
                    (await dataService.GetWorkNotesAsync(SharedViewModel.Instance.OperationYearMonth.YearMonth.ToString(), SagyosiKbn.Nyuko.GetStringValue()));

                // 出庫データ
                SharedModel.Instance.Leavings.Clear();
                SharedModel.Instance.Leavings.AddRange
                    (await dataService.GetOutWorkNotesAsync(SharedViewModel.Instance.OperationYearMonth.YearMonth.ToString(), SagyosiKbn.Shukko.GetStringValue()));

                // 直送データ
                SharedModel.Instance.DirectDeliverys.Clear();
                SharedModel.Instance.DirectDeliverys.AddRange
                    (await dataService.GetDirectOutWorkNotesAsync(SharedViewModel.Instance.OperationYearMonth.YearMonth.ToString(), SagyosiKbn.Chokusou.GetStringValue()));

                // 返品データ
                SharedModel.Instance.Returns.Clear();
                SharedModel.Instance.Returns.AddRange
                    (await dataService.GetWorkNotesAsync(SharedViewModel.Instance.OperationYearMonth.YearMonth.ToString(), SagyosiKbn.Henpin.GetStringValue()));

                // 品目マスタ
                SharedModel.Instance.Items.Clear();
                SharedModel.Instance.Items.AddRange(await dataService.GetMaterialsAsync());

                // 資材棚卸操作グループ区分
                string groupCategory 
                    = await dataService.GetSizaiTanaorosiGroupKbn(SystemCategory.Sizai.GetStringValue(), SharedModel.Instance.StartupArgs.EmployeeCode);

                // 印刷対象操作
                SharedViewModel.Instance.PrintOerations.AddRange(
                    doc.XPathSelectElements("/Settings/PrintOperations/PrintOperation").Select(element => (string)element.Attribute("Target")));

                // 確定対象操作
                SharedViewModel.Instance.FixOerations.AddRange(
                    doc.XPathSelectElements("/Settings/FixOperations/FixOperation").Select(element => (string)element.Attribute("Target")));

                // 更新対象操作
                SharedViewModel.Instance.UpdateOerations.AddRange(
                    doc.XPathSelectElements("/Settings/UpdateOperations/UpdateOperation").Select(element => (string)element.Attribute("Target")));

                // 修正対象操作
                SharedViewModel.Instance.ModifyOerations.AddRange(
                    doc.XPathSelectElements("/Settings/ModifyOperations/ModifyOperation").Select(element => (string)element.Attribute("Target")));

                // 操作状況対象外操作
                SharedViewModel.Instance.ConditionExcludedOperation.AddRange(
                    doc.XPathSelectElements("/Settings/ConditionExcludeOperations/ConditionExcludeOperation").Select(element => (string)element.Attribute("Target")));

                // 棚卸操作グループ区分が未設定
                if (String.IsNullOrEmpty(groupCategory))
                {
                    MessageManager.ShowInformation(SystemID.CKSI1010, "NotRegistTanaorosiOperationGroup");
                    Application.Current.Shutdown();
                }

                // 全作業
                else if (groupCategory.Equals(SizaiGroupCategory.All.GetStringValue()))
                {
                    // 選択画面表示
                    var view = new SelectInputWorkView();
                    var viewModel = new SelectInputWorkViewModel(dataService);
                    view.DataContext = viewModel;
                    Messenger.Default.Send(new ShowModalWindowMessage(this)
                    {
                        Operation = view
                    });

                    viewModel.InputWorkSelected += new InputWorkSelected((o, e) => executeOperation(doc, e.WorkKbn));

                }

                else
                {
                    // 操作を開始する
                    executeOperation(doc, groupCategory.Equals(SizaiGroupCategory.Tanaoroshi.GetStringValue()) 
                            ? SizaiWorkCategory.TanaoroshiWork.GetStringValue() : SizaiWorkCategory.KensinWork.GetStringValue());
                }
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                Application.Current.Shutdown(0);
            }

            finally
            {
                BusyStatus.IsBusy = false;
            }
        });

        /// <summary>
        /// 操作を開始する
        /// </summary>
        /// <param name="doc">XDocumentオブジェクト</param>
        /// <param name="workKbn">作業区分(1:棚卸、2:検針)</param>
        private async void executeOperation(XDocument doc, string workKbn)
        {
            // 作業区分を共有データに保持
            SharedModel.Instance.WorkKbn 
                = workKbn.Equals(SizaiWorkCategory.TanaoroshiWork.GetStringValue()) 
                    ? SizaiWorkCategory.TanaoroshiWork : SizaiWorkCategory.KensinWork;

            // 操作コード一覧の取得
            IEnumerable <OperationInfo> oparationInfo 
                = (await dataService.GetOperatorAsync(SystemCategory.Sizai.GetStringValue(), 
                    SharedModel.Instance.StartupArgs.EmployeeCode, workKbn, SharedViewModel.Instance.OperationYearMonth.TermEnd));

            // 操作一覧の設定
            Operations.AddRange(oparationInfo.Select((oparation, index) =>
            {
                var elem = workKbn.Equals(SizaiWorkCategory.TanaoroshiWork.GetStringValue()) ?
                    doc.XPathSelectElement($"/Settings/OperationDefinitions/Tanaorosi/OperationDefinition[@Title='{oparation.Name.Trim()}']") :
                    doc.XPathSelectElement($"/Settings/OperationDefinitions/Kensin/OperationDefinition[@Title='{oparation.Name.Trim()}']");

                var userControl = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(
                    eachType => eachType.Name == (string)elem.Attribute("UserControlName"));

                return new Common.Operation()
                {
                    Index = index,
                    WorkType = oparation.WorkType.Trim(),
                    Code = oparation.Code, 
                    Title = (string)elem.Attribute("Title"), 
                    Note = (string)elem.Attribute("Note"),
                    UserControl = userControl, 
                    IsStartOperation = index == 0,
                    IsLastOperation = index == oparationInfo.Count() - 1,
                    IsMiddleOperation = (0 < index && index < oparationInfo.Count() - 1),
                    IsPrintVisibleOperation = SharedViewModel.Instance.PrintOerations.Any(element => element == (string)elem.Attribute("Title")),
                    IsFixVisibleOperation = SharedViewModel.Instance.FixOerations.Any(element => element == (string)elem.Attribute("Title")),
                    IsUpdateVisibleOperation = SharedViewModel.Instance.UpdateOerations.Any(element => element == (string)elem.Attribute("Title")),
                    IsModifyVisibleOperation = SharedViewModel.Instance.ModifyOerations.Any(element => element == (string)elem.Attribute("Title")),
                    IsConditionExcludedOperation = SharedViewModel.Instance.ConditionExcludedOperation.Any(element => element == (string)elem.Attribute("Title")),
                    IsModified = true
                };
            }));

            // 棚卸データ入力作業
            if(workKbn.Equals(SizaiWorkCategory.TanaoroshiWork.GetStringValue()))
            {
                // 棚卸進捗状況(棚卸データ入力作業)の取得
                IEnumerable<TanaorosiProgress> tanaorosiProgress 
                    = await dataService.GetTanaorosiProgress(SizaiWorkCategory.TanaoroshiWork.GetStringValue());

                // 操作情報の補正
                correctOperations(tanaorosiProgress);
            }

            else
            {
                var elem = SharedViewModel.Instance.OperationYearMonth.TermEnd ?
                    doc.XPathSelectElement($"/Settings/LastOperationNo/Tanaorosi/Kimatu/No"):
                    doc.XPathSelectElement($"/Settings/LastOperationNo/Tanaorosi/Reigetu/No");

                int tanaorosiLastOperationNo = int.Parse((string)elem.Attribute("Target"));

                // 棚卸進捗状況(棚卸データ入力作業)の取得
                IEnumerable<TanaorosiProgress> tanaorosiProgress 
                    = await dataService.GetTanaorosiProgress(SizaiWorkCategory.TanaoroshiWork.GetStringValue());

                // 棚卸データ入力作業が完了しているかどうか
                if ((tanaorosiProgress.First().OperationNo == tanaorosiLastOperationNo) 
                    && (tanaorosiProgress.First().OperationCondition == OperationCondition.Fix.GetStringValue()))
                {
                    // 棚卸進捗状況(検針データ入力作業)の取得
                    IEnumerable<TanaorosiProgress> kensinProgress 
                        = await dataService.GetTanaorosiProgress(SizaiWorkCategory.KensinWork.GetStringValue());

                    // 操作情報の補正
                    correctOperations(kensinProgress);
                }

                else
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "TanaorosiWorkNotCompleted");
                    Application.Current.Shutdown();
                }

            }

        }

        /// <summary>
        /// 操作情報の補正
        /// </summary>
        /// <param name="tanaorosiProgress">棚卸進捗状況</param>
        private void correctOperations(IEnumerable<TanaorosiProgress> tanaorosiProgress)
        {
            // 直近の操作の作業が完了しているかどうかのフラグを共有データに保持
            SharedViewModel.Instance.Operations[tanaorosiProgress.First().OperationNo - 1].IsFixed
                = tanaorosiProgress.First().OperationCondition == OperationCondition.Fix.GetStringValue() ? true : false;

            // 直近の操作の作業が未完了かどうかのフラグを共有データに保持
            SharedViewModel.Instance.Operations[tanaorosiProgress.First().OperationNo - 1].IsModified
                = !SharedViewModel.Instance.Operations[tanaorosiProgress.First().OperationNo - 1].IsFixed;

            // 直近の操作を現在の操作とする
            CurrentOperation = Operations[tanaorosiProgress.First().OperationNo - 1];

            // 前工程の操作は完了扱いにする
            for (int i = 0; i < CurrentOperation.Index; i++)
            {
                Operations[i].IsFixed = true;
                SharedViewModel.Instance.Operations[i].IsFixed = true;
                Operations[i].IsModified = false;
                SharedViewModel.Instance.Operations[i].IsModified = false;
            }
        }

        /// <summary>
        /// 画面を生成する
        /// </summary>
        public UserControl CreateWindow(string windowName)
        {
            var tempControl = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(
                eachType => eachType.Name == windowName);

            var userControl = (UserControl)Activator.CreateInstance(tempControl);
            return userControl;
        }

        /// <summary>
        /// 画面をモーダル表示する
        /// </summary>
        public void ShowModalWindow(UserControl userControl)
        {
            ((IOperationViewBase)userControl).InitializeAsync();

            Messenger.Default.Send(new ShowModalWindowMessage(this)
            {
                Operation = (UserControl)userControl
            });
        }

        /// <summary>
        /// モーダル画面を閉じる
        /// </summary>
        public void CloseModalWindow()
        {
            Messenger.Default.Send(new CloseModalWindowMessage(this)
            {
            });
        }

        /// <summary>
        /// 設定画面が閉じられた時のコールバック
        /// </summary>
        /// <param name="e">イベント引数</param>
        private void OnSettingDisplayClose(SettingDisplayCloseEventArgs e)
        {
            // 特に何もしない
        }

        /// <summary>
        /// 棚卸操作選択の発火
        /// </summary>
        public ICommand FireSelectOperation => new RelayCommand<Common.Operation>(async operation =>
        {
            try
            {
                if (operation != null)
                {
                    if(operation.Index >=  1)
                    {
                        SharedViewModel.Instance.PreviousOperation 
                            = SharedViewModel.Instance.Operations[operation.Index - 1];

                        if (!SharedViewModel.Instance.PreviousOperation.IsFixed)
                        {
                            MessageManager.ShowExclamation(SystemID.CKSI1010, "CanNotGoNext");
                            return;
                        }
                    }

                    var previousOperationUserControl
                        = (UserControl)Activator.CreateInstance(SharedViewModel.Instance.PreviousOperation.UserControl);
                    IOperationViewBase previousOperationViewBase = (IOperationViewBase)previousOperationUserControl;
                    bool previousResult = await previousOperationViewBase.BeforeSelecOperationAsync();
                    if (!previousResult) return;

                    var userControl = (UserControl)Activator.CreateInstance(operation.UserControl);
                    IOperationViewBase operationViewBase = (IOperationViewBase)userControl;
                    bool result = await operationViewBase.SelecOperationAsync();
                    if (!result) return;
                }

                CurrentOperation = operation;
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

        });

        /// <summary>
        /// 「設定画面」ボタン押下イベントの発火
        /// </summary>
        public ICommand FireSettingDisplay => new RelayCommand(() =>
        {
            // 画面表示
            var userControl = CreateWindow("SettingDisplayView");
            var viewModel = ((OperationViewBase)userControl).DataContext as SettingDisplayViewModel;
            // クローズイベントを受け取るメソッドを追加する
            viewModel.CloseEvent += new SettingDisplayViewModel
                                        .CloseEventHandler(OnSettingDisplayClose);
            // モーダル画面を表示する
            ShowModalWindow(userControl);
        });

        /// <summary>
        /// [×]イベントの発火
        /// </summary>
        public ICommand FireClose => new RelayCommand(async () =>
        {
            if (operation != null)
            {
                bool result = await operation.CloseAsync();
                if (!result) return;
            }

            if (MessageManager.ShowQuestion(SystemID.CKSI1010, "ConfirmExit") == ResultType.No)
            {
                return;
            }

            Application.Current.Shutdown();
        });

        /// <summary>
        /// [次へ]イベントの発火
        /// </summary>
        public ICommand FireGoNext => new RelayCommand(async () =>
        {
            if (operation != null)
            {
                bool result = await operation.GoNextAsync();
                if (!result) return;
            }

            var currentIndex = CurrentOperation.Index;
            if (currentIndex < Operations.Count - 1)
            {
                CurrentOperation = Operations[currentIndex + 1];
            }
        });

        /// <summary>
        /// [戻る]イベントの発火
        /// </summary>
        public ICommand FireBackPrevious => new RelayCommand(async () =>
        {
            if (operation != null)
            {
                bool result = await operation.BackPreviousAsync();
                if (!result) return;
            }

            var currentIndex = CurrentOperation.Index;
            if (0 < currentIndex)
            {
                CurrentOperation = Operations[currentIndex - 1];
            }
        });

        /// <summary>
        /// [印刷]イベントの発火
        /// </summary>
        public ICommand FirePrint => new RelayCommand(async () =>
        {
            if (operation != null)
            {
                bool result = await operation.PrintAsync();
                if (!result) return;
            }
        });

        /// <summary>
        /// [更新]イベントの発火
        /// </summary>
        public ICommand FireUpdate => new RelayCommand(async () =>
        {
            if (operation != null)
            {
                bool result = await operation.UpdateAsync();
                if (!result) return;
            }
        });

        /// <summary>
        /// [確定]イベントの発火
        /// </summary>
        public ICommand FireFix => new RelayCommand(async () =>
        {
            if (operation != null)
            {
                bool result = await operation.CommitAsync();
                if (!result) return;
            }
        });

        /// <summary>
        /// [修正]イベントの発火
        /// </summary>
        public ICommand FireModify => new RelayCommand(async () =>
        {
            if (operation != null)
            {
                bool result = await operation.ModifyAsync();
                if (!result) return;
            }
        });

        /// <summary>
        /// 終了イベントの発火
        /// </summary>
        public ICommand FireExit => new RelayCommand(async () =>
        {
            if (operation != null)
            {
                bool result = await operation.ExitAsync();
                if (!result) return;
            }

            Application.Current.Shutdown();

        });

        #endregion
    }
}