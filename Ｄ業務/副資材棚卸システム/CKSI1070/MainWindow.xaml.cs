using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CKSI1070.Common;
using CKSI1070.ViewModel;
using Common;
using Core;
using DBManager;
using DBManager.Constants;
using DBManager.Model;
using Microsoft.Windows.Controls;
using Shared;

namespace CKSI1070
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, IOperationMenuContainer
    {
        #region フィールド
        /// <summary>
        /// DB操作の制御オブジェクト
        /// </summary>
        private DBAccessor _accessor = null;
        #endregion

        #region 列挙体
        /// <summary>
        /// DB操作
        /// </summary>
        private enum ProcessType
        {
            /// <summary>
            /// 挿入
            /// </summary>
            Insert,
            /// <summary>
            /// 更新
            /// </summary>
            Update
        }
        #endregion

        #region プロパティ
        /// <summary>
        /// 画面モード管理
        /// </summary>
        public OperationMenuManagementMode OperationGroupManagementMode
        {
            get;
            private set;
        }

        /// <summary>
        /// 処理状態
        /// </summary>
        public ProcessState ProcessState
        {
            get;
            private set;
        }

        /// <summary>
        /// 所属名
        /// </summary>
        public ObservableCollection<ComboBoxViewModel> Deployments
        {
            get;
            private set;
        }

        /// <summary>
        /// システム分類
        /// </summary>
        public ObservableCollection<ComboBoxViewModel> TanaorosiSystemCategorys
        {
            get;
            private set;
        }

        /// <summary>
        /// グループ区分
        /// </summary>
        public ObservableCollection<ComboBoxViewModel> TanaorosiGroupKbns
        {
            get;
            private set;
        }

        /// <summary>
        /// 作業区分
        /// </summary>
        public ObservableCollection<ComboBoxViewModel> WorkCategorys
        {
            get;
            private set;
        }

        /// <summary>
        /// 操作種類
        /// </summary>
        public ObservableCollection<ComboBoxViewModel> OperationTypes
        {
            get;
            private set;
        }

        /// <summary>
        /// 操作コード
        /// </summary>
        public ObservableCollection<ComboBoxViewModel> OperationCodes
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 棚卸操作メニュー一覧
        /// </summary>
        public ObservableCollection<OperationMenuRow> OperationMenuRows
        {
            get;
            private set;
        }

        /// <summary>
        /// 棚卸操作メニューの変更状態
        /// </summary>
        public OperationMenuModification OperationMenuModification
        {
            get;
            private set;
        }

        /// <summary>
        /// 新規棚卸操作メニュー
        /// </summary>
        public OperationMenuRow NewOperationMenu
        {
            get;
            private set;
        }

        /// <summary>
        /// クローズ処理メッセージ表示可否
        /// </summary>
        public bool IsWindowCloseMessage
        {
            get;
            private set;
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            OperationGroupManagementMode = new OperationMenuManagementMode() 
                { CurrentMode = OperationMenuManagementMode.ModeType.Modify };
            ProcessState = new ProcessState();
            OperationMenuModification = new OperationMenuModification();
            OperationMenuRows = new ObservableCollection<OperationMenuRow>();
            NewOperationMenu = new OperationMenuRow(this);
            this._accessor = new DBAccessor(DBConstants.DBType.ORACLE);

            Deployments = new ObservableCollection<ComboBoxViewModel>();
            TanaorosiSystemCategorys = new ObservableCollection<ComboBoxViewModel>();
            TanaorosiGroupKbns = new ObservableCollection<ComboBoxViewModel>();
            WorkCategorys = new ObservableCollection<ComboBoxViewModel>();
            OperationTypes = new ObservableCollection<ComboBoxViewModel>();
            OperationCodes = new ObservableCollection<ComboBoxViewModel>();
            SharedModel.Instance.OperationInfo = new Shared.Model.OperationInfo();
            
            InitializeComponent();

            IsWindowCloseMessage = false;

            // 所属コードコンバータ情報の生成
            if (!CreateDeploymentConverterInfo())
            {
                MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1070, "NonDeploymentMst");
                this.Close();
                return;
            }

            // 所属コードのコンボボックスの生成
            CreateDeploymentsCombo();

            // システム分類コンバータ情報の生成
            CreateSystemCategoryConverterInfo();
            NewOperationMenu.SystemCategory = Constants.SystemCategory.Sizai.GetStringValue();

            // システム分類のコンボボックスの生成
            CreateTanaorosiSystemCategorysCombo();

            // グループ区分コンバータ情報の生成
            CreateGroupKbnConverterInfo();

            // 操作コードコンバート情報の生成
            CreateOperationCodeConverterInfo();

            // 作業用区分コンバータ情報の生成
            CreateWorkCategoryConverterInfo(NewOperationMenu.SystemCategory);

            // 操作種類コンバータ情報の生成
            CreateOperationTypeConverterInfo(NewOperationMenu.SystemCategory);

            // グループ区分のコンボボックスの生成
            CreateTanaorosiGroupKbnsCombo(NewOperationMenu.SystemCategory);

            // 作業区分のコンボボックスの生成
            CreateWorkCategorysCombo(NewOperationMenu.SystemCategory);

            // 操作種類のコンボボックスの生成
            CreateOperationTypesCombo();

            // 操作コードのコンボボックスの生成
            CreateOperationCodesCombo(NewOperationMenu.SystemCategory);

            // 一覧検索
            RefreshOperationMenu();

            IsWindowCloseMessage = true;
        }


        #endregion

        #region メソッド

        /// <summary>
        /// 所属コードコンバータ情報の生成
        /// </summary>
        private bool CreateDeploymentConverterInfo()
        {
            try
            {
                // 所属名称の取得
                SharedModel.Instance.OperationInfo.DeploymentName = this._accessor.SelectDeploymentName();
                if (SharedModel.Instance.OperationInfo.DeploymentName.Count() == 0) return false;
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// システム分類コンバータ情報の生成
        /// </summary>
        private bool CreateSystemCategoryConverterInfo()
        {
            try
            {
                // システム分類の取得
                SharedModel.Instance.OperationInfo.SystemCategory = this._accessor.SelectTanaorosiSystemCategorys();
                if (SharedModel.Instance.OperationInfo.SystemCategory.Count() == 0) return false;
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// グループ区分コンバータ情報の生成
        /// </summary>
        /// <param name="systemCategory">システム分類</param>
        private bool CreateGroupKbnConverterInfo()
        {
            try
            {
                // グループ区分の取得
                SharedModel.Instance.OperationInfo.GroupKbn = this._accessor.SelectTanaorosiGroupKbns();
                if (SharedModel.Instance.OperationInfo.GroupKbn.Count() == 0) return false;
  
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// 操作コードコンバータ情報の生成
        /// </summary>
        /// <param name="systemCategory">システム分類</param>
        private bool CreateOperationCodeConverterInfo()
        {
            try
            {
                // 操作コードの取得
                SharedModel.Instance.OperationInfo.OperationCodes = this._accessor.SelectTanaorosiOperationCodes();
                if (SharedModel.Instance.OperationInfo.OperationCodes.Count() == 0) return false;
            }

            catch (Exception)
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// 作業区分コンバータ情報の生成
        /// </summary>
        /// <param name="systemCategory">システム分類</param>
        private void CreateWorkCategoryConverterInfo(string systemCategory)
        {
            try
            {
                IDictionary<string, string> dictionary = new Dictionary<string, string>();

                if (systemCategory == "1")
                {
                    dictionary.Add("1", "棚卸データ入力作業");
                    dictionary.Add("2", "検針データ入力作業");
                    SharedModel.Instance.OperationInfo.WorkCategory = dictionary;
                }

                else if (systemCategory == "2")
                {
                    dictionary.Add("1", "棚卸データ入力作業");
                    SharedModel.Instance.OperationInfo.WorkCategory = dictionary;
                }
                
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 操作種類コンバータ情報の生成
        /// </summary>
        /// <param name="systemCategory">システム分類</param>
        private void CreateOperationTypeConverterInfo(string systemCategory)
        {
            try
            {
                IDictionary<string, string> dictionary = new Dictionary<string, string>();

                dictionary.Add("0", "例月棚卸");
                dictionary.Add("1", "期末棚卸");
                SharedModel.Instance.OperationInfo.OperationType = dictionary;
                
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 所属コードのコンボボックスの生成
        /// </summary>
        private void CreateDeploymentsCombo()
        {
            try
            {
                // 所属コードの取得
                foreach (var systemCategory in SharedModel.Instance.OperationInfo.DeploymentName)
                {
                    ComboBoxViewModel item = new ComboBoxViewModel();
                    item.Key = systemCategory.Key;
                    item.Value = systemCategory.Value;
                    Deployments.Add(item);
                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// システム分類のコンボボックスの生成
        /// </summary>
        private void CreateTanaorosiSystemCategorysCombo()
        {
            try
            {
                // システム分類の取得
                foreach (var systemCategory in SharedModel.Instance.OperationInfo.SystemCategory)
                {
                    ComboBoxViewModel item = new ComboBoxViewModel();
                    item.Key = systemCategory.Key;
                    item.Value = systemCategory.Value;
                    TanaorosiSystemCategorys.Add(item);
                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// グループ区分のコンボボックスの生成
        /// </summary>
        private bool CreateTanaorosiGroupKbnsCombo(string systemCategory)
        {
            bool ret = true;
            
            try
            {
                TanaorosiGroupKbns.Clear();
                
                // グループ区分の取得
                if (SharedModel.Instance.OperationInfo.GroupKbn != null)
                {
                    foreach (var groupKbn in SharedModel.Instance.OperationInfo.GroupKbn)
                    {
                        if (groupKbn.SystemCategory.Equals(systemCategory))
                        {
                            ComboBoxViewModel item = new ComboBoxViewModel();
                            item.Key = groupKbn.GroupKbn;
                            item.Value = groupKbn.GroupKbnName;
                            TanaorosiGroupKbns.Add(item);
                            ret = true;
                        }
                        else
                        {
                            ret = false;
                        }
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 作業区分のコンボボックスの生成
        /// </summary>
        private bool CreateWorkCategorysCombo(string systemCategory)
        {
            bool ret = true;
            
            try
            {
                WorkCategorys.Clear();

                if (SharedModel.Instance.OperationInfo.WorkCategory != null)
                {
                    foreach (var workCategory in SharedModel.Instance.OperationInfo.WorkCategory)
                    {
                        ComboBoxViewModel item = new ComboBoxViewModel();
                        item.Key = workCategory.Key;
                        item.Value = workCategory.Value;
                        WorkCategorys.Add(item);
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 操作種類のコンボボックスの生成
        /// </summary>
        private void CreateOperationTypesCombo()
        {
            try
            {
                OperationTypes.Clear();
                
                // 操作種類の取得
                foreach (var tanaorosiType in SharedModel.Instance.OperationInfo.OperationType)
                {
                    ComboBoxViewModel item = new ComboBoxViewModel();
                    item.Key = tanaorosiType.Key;
                    item.Value = tanaorosiType.Value;
                    OperationTypes.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 操作コードのコンボボックスの生成
        /// </summary>
        private bool CreateOperationCodesCombo(string systemCategory)
        {
            bool ret = true;
            
            try
            {
                OperationCodes.Clear();
                
                // 操作コードの取得
                if (SharedModel.Instance.OperationInfo.OperationCodes != null)
                {
                    foreach (var operationCode in SharedModel.Instance.OperationInfo.OperationCodes)
                    {
                        if (operationCode.SystemCategory.Equals(systemCategory))
                        {
                            ComboBoxViewModel item = new ComboBoxViewModel();
                            item.Key = operationCode.OperationCode;
                            item.Value = operationCode.OperationName;
                            OperationCodes.Add(item);
                            ret = true;
                        }
                        else
                        {
                            ret = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 操作コードのコンボボックスの取得
        /// <param name="systemCategory">システム分類</param>
        /// <returns>操作コードのコンボボックス</returns>
        /// </summary>
        private ObservableCollection<ComboBoxViewModel> GetOperationsCombo(string systemCategory)
        {
            ObservableCollection<ComboBoxViewModel> ret = new ObservableCollection<ComboBoxViewModel>();

            try
            {
                // 操作コードの取得
                IDictionary<string, string> tanaorosiOperationCodes = this._accessor.SelectTanaorosiOperationCodes(systemCategory);
                foreach (var operationCode in tanaorosiOperationCodes)
                {
                    ComboBoxViewModel item = new ComboBoxViewModel();
                    item.Key = operationCode.Key;
                    item.Value = operationCode.Value;
                    ret.Add(item);
                }
            }

            catch (Exception)
            {
                throw;
            }

            return ret;

        }

        /// <summary>
        /// 棚卸操作メニュー追加イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnAddOperationMenu(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1070, "Add") == MessageManager.ResultType.Yes)
            {
                try
                {
                    string result = CheckOperationMenu(NewOperationMenu, ProcessType.Insert);
                    
                    if (result.Equals(String.Empty))
                    {
                        // 操作メニューの設定
                        IList<TanaorosiOperationMenu> list = new List<TanaorosiOperationMenu>();
                        list.Add(ORMapper.CopyProperty(NewOperationMenu, ORMapper.ProcessType.Insert));

                        // 操作メニューマスタの重複チェック
                        if (this._accessor.IsOverlapCheckTanaorosiOperationMenu(list))
                        {
                            // 入力データが不正
                            MessageManager.ShowError(MessageManager.SystemID.CKSI1070, "OverlapExists");
                            return;
                        }

                        // 操作メニューマスタの挿入
                        var sqlResult = this._accessor.InsertTanaorosiOperationMenu(list);
                        switch (sqlResult)
                        {
                            case DBConstants.SQL_RESULT_OK:
                                OperationMenuRows.Add(new OperationMenuRow(this)
                                {
                                    // 社員所属コード
                                    SyainSZCode = NewOperationMenu.SyainSZCode.TrimEnd(),
                                    // システム分類
                                    SystemCategory = NewOperationMenu.SystemCategory.TrimEnd(),
                                    // グループ区分
                                    GroupKbn = NewOperationMenu.GroupKbn.TrimEnd(),
                                    // 作業区分
                                    WorkCategory = NewOperationMenu.WorkCategory.TrimEnd(),
                                    // 操作種類
                                    OperationType = NewOperationMenu.OperationType.TrimEnd(),
                                    // 操作順
                                    OperationOrder = NewOperationMenu.OperationOrder.TrimEnd(),
                                    // 操作コード
                                    OperationCD = NewOperationMenu.OperationCD.TrimEnd(),
                                    //操作コードコンボボックス
                                    Operations = OperationCodes,
                                    // 変更されているか
                                    IsDirty = false,
                                });

                                // 各種パラメータの初期化
                                NewOperationMenu.SyainSZCode = "J2";
                                NewOperationMenu.SystemCategory = Constants.SystemCategory.Sizai.GetStringValue();
                                NewOperationMenu.GroupKbn = String.Empty;
                                NewOperationMenu.WorkCategory = String.Empty;
                                NewOperationMenu.OperationType = String.Empty;
                                NewOperationMenu.OperationOrder = String.Empty;
                                NewOperationMenu.OperationCD = String.Empty;
                                NewOperationMenu.IsDirty = false;
                                var view = CollectionViewSource.GetDefaultView(OperationMenuRows);
                                view.SortDescriptions.Add(new SortDescription("SyainSZCode", ListSortDirection.Ascending));
                                view.SortDescriptions.Add(new SortDescription("SystemCategory", ListSortDirection.Ascending));
                                view.SortDescriptions.Add(new SortDescription("GroupKbn", ListSortDirection.Ascending));
                                view.SortDescriptions.Add(new SortDescription("WorkCategory", ListSortDirection.Ascending));
                                view.SortDescriptions.Add(new SortDescription("OperationType", ListSortDirection.Ascending));
                                view.SortDescriptions.Add(new SortDescription("OperationOrderForSort", ListSortDirection.Ascending));
                                MessageManager.ShowInformation(MessageManager.SystemID.CKSI1070, "Completed");
                                break;

                            case DBConstants.SQL_RESULT_ALREADY_EXISTS:
                                MessageManager.ShowError(MessageManager.SystemID.CKSI1070, "OverlapExists");
                                break;

                            case DBConstants.SQL_RESULT_NG:
                                MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1070, "SystemError");
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        // 入力データが不正
                        MessageManager.ShowError(MessageManager.SystemID.CKSI1070, result);
                    }
                }
                catch (Exception)
                {
                    // 例外エラー
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1070, "SystemError");
                }
            }
        }

        /// <summary>
        /// 棚卸操作メニュー更新イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnUpdateOperationMenu(object sender, RoutedEventArgs e)
        {
            
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1070, "Modify") == MessageManager.ResultType.Yes)
            {
                var updateOperationMenu = (OperationMenuRow)(((Button)sender).Tag);

                try
                {
                    // 入力データのチェック
                    string result = CheckOperationMenu(updateOperationMenu, ProcessType.Update);

                    if (result.Equals(String.Empty))
                    {
                        // グループ情報の設定
                        List<TanaorosiOperationMenu> list = new List<TanaorosiOperationMenu>();
                        list.Add(ORMapper.CopyProperty(updateOperationMenu, ORMapper.ProcessType.Update));

                        var sqlResult = this._accessor.UpdateTanaorosiOperationMenu(list);
                        switch (sqlResult)
                        {
                            case DBConstants.SQL_RESULT_OK:
                                updateOperationMenu.IsDirty = false;
                                OperationMenuModification.IsDirty = OperationMenuRows.Any(anyOperationMenu => anyOperationMenu.IsDirty);
                                MessageManager.ShowInformation(MessageManager.SystemID.CKSI1070, "Completed");
                                break;

                            case DBConstants.SQL_RESULT_NG:
                                MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1070, "SystemError");
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        // 入力データが不正
                        MessageManager.ShowError(MessageManager.SystemID.CKSI1070, result);
                    }
                }
                catch (Exception)
                {
                    // 例外エラー
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1070, "SystemError");
                }
            }
        }

        /// <summary>
        /// 棚卸操作メニュー削除イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnDeleteOperationMenu(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1070, "Delete") == MessageManager.ResultType.Yes)
            {
                var deleteOperationMenu = (OperationMenuRow)(((Button)sender).Tag);

                try
                {
                    // システム分類情報の設定
                    List<TanaorosiOperationMenu> list = new List<TanaorosiOperationMenu>();
                        list.Add(ORMapper.CopyProperty(deleteOperationMenu, ORMapper.ProcessType.Delete));

                    var sqlResult = this._accessor.DeleteTanaorosiOperationMenu(list);
                    switch (sqlResult)
                    {
                        case DBConstants.SQL_RESULT_OK:
                            OperationMenuRows.Remove(deleteOperationMenu);
                            OperationMenuModification.IsDirty = OperationMenuRows.Any(anyOperationMenu => anyOperationMenu.IsDirty);
                            MessageManager.ShowInformation(MessageManager.SystemID.CKSI1070, "Completed");
                            break;

                        case DBConstants.SQL_RESULT_NG:
                            MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1070, "SystemError");
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    // 例外エラー
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1070, "SystemError");
                }
            }
        }

        /// <summary>
        /// 一括更新イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnUpdateAllOperationMenu(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1070, "Modify") == MessageManager.ResultType.Yes)
            {
                BackgroundWorker worker = new BackgroundWorker();

                try
                {
                    // 処理中か
                    ProcessState.IsBusy = true;
                    // 処理中のメッセージ
                    ProcessState.BusyMessage = "一括更新中...";

                    // 更新数
                    int count = 0;

                    // 入力データのチェック結果
                    string error = string.Empty;

                    // 更新データ
                    IList<TanaorosiOperationMenu> updateList = new List<TanaorosiOperationMenu>();

                    worker.DoWork += (s, ev) =>
                    {

                        foreach (OperationMenuRow updateOperationMenu in OperationMenuRows)
                        {
                            if (updateOperationMenu.IsDirty)
                            {
                                // 入力データのチェック
                                error = CheckOperationMenu(updateOperationMenu, ProcessType.Update);
                                if (error.Equals(String.Empty))
                                {
                                    // 棚卸操作グループ情報の設定
                                    updateList.Add(ORMapper.CopyProperty(updateOperationMenu, ORMapper.ProcessType.Update));
                                }
                                else
                                {
                                    // 入力データが不正
                                    MessageManager.ShowError(MessageManager.SystemID.CKSI1070, error);
                                    return;
                                }
                            }

                            count++;
                        }

                    };

                    worker.RunWorkerCompleted += (s, ex) =>
                    {
                        if (!String.IsNullOrEmpty(error))
                        {
                            // 入力値が不正
                            ProcessState.IsBusy = false;
                            this._dataGrid.SelectedIndex = count;
                        }
                        else
                        {
                            var sqlResult = this._accessor.UpdateTanaorosiOperationMenu(updateList);
                            switch (sqlResult)
                            {
                                case DBConstants.SQL_RESULT_OK:
                                    foreach (var updateOperationMenu in OperationMenuRows)
                                    {
                                        updateOperationMenu.IsDirty = false;
                                    }

                                    OperationMenuModification.IsDirty = false;
                                    ProcessState.IsBusy = false;
                                    MessageManager.ShowInformation(MessageManager.SystemID.CKSI1070, "Completed");
                                    break;

                                case DBConstants.SQL_RESULT_NG:
                                    ProcessState.IsBusy = false;
                                    MessageManager.ShowError(MessageManager.SystemID.CKSI1070, "SystemError");
                                    break;

                                default:
                                    break;
                            }
                        }
                    };
                }
                catch (Exception)
                {
                    ProcessState.IsBusy = false;
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1070, "SystemError");
                }

                worker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// システム分類の変更イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnSystemCategorySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // グループ区分のコンボボックスの生成
            if (!CreateTanaorosiGroupKbnsCombo(NewOperationMenu.SystemCategory))
            {
                string systemCategoryName = (SharedModel.Instance.OperationInfo.SystemCategory[NewOperationMenu.SystemCategory]).Trim();
                MessageManager.ShowWarning(MessageManager.SystemID.CKSI1070, "NonGroupKbnMstTieSystemCategory", systemCategoryName);
            }

            // 作業区分コンバータ情報の生成
            CreateWorkCategoryConverterInfo(NewOperationMenu.SystemCategory);
            // 作業区分のコンボボックスの生成
            CreateWorkCategorysCombo(NewOperationMenu.SystemCategory);

            // 操作コードのコンボボックスの生成
            if (!CreateOperationCodesCombo(NewOperationMenu.SystemCategory))
            {
                string systemCategoryName = (SharedModel.Instance.OperationInfo.SystemCategory[NewOperationMenu.SystemCategory]).Trim();
                MessageManager.ShowWarning(MessageManager.SystemID.CKSI1070, "NonOpeartionCode", systemCategoryName);
            }
        }

        /// <summary>
        /// 棚卸操作メニュー一覧の再検索
        /// </summary>
        private void RefreshOperationMenu()
        {
            BackgroundWorker worker = new BackgroundWorker();

            try
            {
                // 処理中か
                ProcessState.IsBusy = true;
                // 処理中のメッセージ
                ProcessState.BusyMessage = "一覧を取得中...";

                // 棚卸操作メニュー情報
                IList<TanaorosiOperationMenu> tanaorosiOpeartionMenuList = null;

                worker.DoWork += (s, ev) =>
                {
                    // 棚卸操作メニューマスタからの取得
                    tanaorosiOpeartionMenuList = this._accessor.SelectTanaorosiOperationMenu();
                };

                worker.RunWorkerCompleted += (s, ex) =>
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        OperationMenuRows.Clear();

                        foreach (TanaorosiOperationMenu tanaorosiOpeartionMenu in tanaorosiOpeartionMenuList)
                        {
                            OperationMenuRows.Add(new OperationMenuRow(this)
                            {
                                // 社員所属コード
                                SyainSZCode = tanaorosiOpeartionMenu.SyainSZCode.TrimEnd(),
                                // システム分類
                                SystemCategory = tanaorosiOpeartionMenu.SystemCategory.TrimEnd(),
                                // グループ区分
                                GroupKbn = tanaorosiOpeartionMenu.GroupKbn.TrimEnd(),
                                // 作業区分
                                WorkCategory = tanaorosiOpeartionMenu.WorkCategory.TrimEnd(),
                                // 操作種類
                                OperationType = tanaorosiOpeartionMenu.OperationType.ToString(),
                                // 操作順
                                OperationOrder = tanaorosiOpeartionMenu.OperationOrder.ToString(),
                                // 操作コード
                                OperationCD = tanaorosiOpeartionMenu.OperationCode.ToString(),
                                //操作コードコンボボックス
                                Operations = GetOperationsCombo(tanaorosiOpeartionMenu.SystemCategory.TrimEnd()),
                                // 変更されているか
                                IsDirty = false,
                            });
                        }

                        NewOperationMenu.SyainSZCode = "J2";

                        if (OperationMenuRows.Count > 0)
                            this.Loaded += DelegateSetFocusToDataGridView;
                        else
                            this.Loaded += DelegateSetFocusToTextBox;

                        OperationMenuModification.IsDirty = false;
                        ProcessState.IsBusy = false;
                    }));
                };
            }
            catch (Exception)
            {
                OperationMenuModification.IsDirty = false;
                ProcessState.IsBusy = false;
                MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1070, "SystemError");
            }

            worker.RunWorkerAsync();

        }

        /// <summary>
        /// 閉じる「×」ボタン押下時のイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            if (IsWindowCloseMessage && MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1070, "ConfirmExit") == MessageManager.ResultType.No)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 操作メニューの入力データのチェック
        /// </summary>
        /// <param name="row">操作グループオブジェクト</param>
        /// <param name="type">DB操作</param>
        /// <returns>チェック結果</returns>
        private string CheckOperationMenu(OperationMenuRow row, ProcessType type)
        {
            string result = string.Empty;

            switch (type)
            {
                //挿入
                case ProcessType.Insert:
                    // 社員所属コードの入力チェック
                    if (row.SyainSZCode == null || row.SyainSZCode.Trim().Length == 0)
                    {
                        result = "NotSelectSyainSZCD";
                        break;
                    }
                    // システム分類の未選択チェック
                    if (row.SystemCategory == null || row.SystemCategory.Trim().Length == 0)
                    {
                        result = "NotSelectSystemCategory";
                        break;
                    }
                    // グループ区分の未選択チェック
                    if (row.GroupKbn == null || row.GroupKbn.Trim().Length == 0)
                    {
                        result = "NotSelectGroupKbn";
                        break;
                    }
                    // 作業区分の未選択チェック
                    if (row.WorkCategory == null || row.WorkCategory.Trim().Length == 0)
                    {
                        result = "NotSelectWorkCategory";
                        break;
                    }
                    // 操作種類の未選択チェック
                    if (row.OperationType == null || row.OperationType.Trim().Length == 0)
                    {
                        result = "NotSelectOperationType";
                        break;
                    }
                    // 操作順の入力チェック
                    if (row.OperationOrder == null || row.OperationOrder.Trim().Length == 0)
                    {
                        result = "NotInputOperationOrder";
                        break;
                    }
                    // 操作コードの未選択チェック
                    if (row.OperationCD == null || row.OperationCD.Trim().Length == 0)
                    {
                        result = "NotSelectOperationCD";
                        break;
                    }

                    break;

                //更新
                case ProcessType.Update:
                    break;

                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// テキストボックス(コントロール)にフォーカスを設定するデリゲート
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void DelegateSetFocusToTextBox(object sender, RoutedEventArgs e)
        {
            var textBox = DependencyPropertyDescriptor.FromProperty(TextBox.TextProperty, typeof(TextBox));
            if (textBox != null)
            {
                textBox.AddValueChanged(this._dataGrid, (s, ev) => SetFocusToTextBox(this._txtSyainSZCode));
                SetFocusToTextBox(this._txtSyainSZCode);
            }
        }

        /// <summary>
        /// データグリッドビュー((コントロール)にフォーカスを設定するデリゲート
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void DelegateSetFocusToDataGridView(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(this._dataGrid, (s, ev) => SetFocusToDataGridViewFirstRow(this._dataGrid));
                SetFocusToDataGridViewFirstRow(this._dataGrid);
            }
        }

        /// <summary>
        /// テキストボックス(コントロール)にフォーカスを設定する
        /// </summary>
        /// <param name="control">テキストボックス(コントロール)</param>
        private static void SetFocusToTextBox(Control control)
        {
            if (control == null) return;
            Keyboard.Focus(control);
        }

        /// <summary>
        /// データグリッドビュー(コントロール)にフォーカスを設定する
        /// </summary>
        /// <param name="control">データグリッドビュー(コントロール)</param>
        private static void SetFocusToDataGridViewFirstRow(DataGrid control)
        {
            if (control == null) return;

            if (control.Items.Count > 0)
            {
                var item = control.Items[0];
                control.ScrollIntoView(item);
                control.UpdateLayout();
                control.SelectedItem = item;
                var firstCell = control.Columns[0].GetCellContent(item);
                if (firstCell != null)
                {
                    FocusManager.SetIsFocusScope(firstCell, true);
                    FocusManager.SetFocusedElement(firstCell, firstCell);
                    Keyboard.Focus(firstCell);
                }
            }
        }

        #endregion

        #region インターフェース

        /// <summary>
        /// システム分類変更イベント
        /// </summary>
        public void NotifyTanaorosiOperationMenuModified()
        {
            OperationMenuModification.IsDirty = OperationMenuRows.Any(OperationMenu => OperationMenu.IsDirty);
        }

        #endregion
    }
}




