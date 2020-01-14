using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CKSI1060.Common;
using CKSI1060.ViewModel;
using Common;
using DBManager;
using DBManager.Constants;
using DBManager.Model;
using Microsoft.Windows.Controls;
using Shared;
using Core;

namespace CKSI1060
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, IOperationGroupContainer
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
        public OperationGroupManagementMode OperationGroupManagementMode
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
        /// 棚卸操作グループ一覧
        /// </summary>
        public ObservableCollection<OperationGroupRow> OperationGroupRows
        {
            get;
            private set;
        }

        /// <summary>
        /// 棚卸操作グループの変更状態
        /// </summary>
        public OperationGroupModification OperationGroupModification
        {
            get;
            private set;
        }

        /// <summary>
        /// 新規棚卸操作グループ
        /// </summary>
        public OperationGroupRow NewOperationGroup
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
            OperationGroupManagementMode = new OperationGroupManagementMode() 
                { CurrentMode = OperationGroupManagementMode.ModeType.Modify };
            ProcessState = new ProcessState();
            OperationGroupModification = new OperationGroupModification();
            OperationGroupRows = new ObservableCollection<OperationGroupRow>();
            NewOperationGroup = new OperationGroupRow(this);
            NewOperationGroup.SystemCategory = Constants.SystemCategory.Sizai.GetStringValue();
            this._accessor = new DBAccessor(DBConstants.DBType.ORACLE);

            TanaorosiSystemCategorys = new ObservableCollection<ComboBoxViewModel>();
            TanaorosiGroupKbns = new ObservableCollection<ComboBoxViewModel>();
            SharedModel.Instance.OperationInfo = new Shared.Model.OperationInfo();

            InitializeComponent();

            // 社員情報コンバータ情報の生成
            CreateTanaorosiEmployeeInfoConverterInfo();

            // 部署名コンバータ情報の生成
            CreateDeploymentNameConverterInfo();

            // システム分類のコンボボックスの生成
            CreateTanaorosiSystemCategorysCombo();

            // 一覧検索
            RefreshOperationGroup();
        }


        #endregion

        #region メソッド

        /// <summary>
        /// 社員情報コンバータ情報の生成
        /// </summary>
        private void CreateTanaorosiEmployeeInfoConverterInfo()
        {
            try
            {
                // ユーザ情報の取得
                SharedModel.Instance.OperationInfo.EmployeeInfo = this._accessor.SelectUsersInfo();
            }

            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// 部署名コンバータ情報の生成
        /// </summary>
        private void CreateDeploymentNameConverterInfo()
        {
            try
            {
                // 部署名の取得
                SharedModel.Instance.OperationInfo.DeploymentName = this._accessor.SelectDeploymentName();
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
                IDictionary<string, string> tanaorosiSystemCategorys = this._accessor.SelectTanaorosiSystemCategorys();
                SharedModel.Instance.OperationInfo.SystemCategory = tanaorosiSystemCategorys;
                foreach (var systemCategory in tanaorosiSystemCategorys)
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
                IDictionary<string, string> tanaorosiGroupKbns = this._accessor.SelectTanaorosiGroupKbns(systemCategory);
                foreach (var groupKbn in tanaorosiGroupKbns)
                {
                    ComboBoxViewModel item = new ComboBoxViewModel();
                    item.Key = groupKbn.Key;
                    item.Value = groupKbn.Value;
                    TanaorosiGroupKbns.Add(item);
                }

                if (tanaorosiGroupKbns.Count() == 0) ret = false;
            }

            catch (Exception)
            {
                throw;
            }

            return ret;

        }

        /// <summary>
        /// グループ区分のコンボボックスの取得
        /// <param name="systemCategory">システム分類</param>
        /// <returns>グループ区分のコンボボックス</returns>
        /// </summary>
        private ObservableCollection<ComboBoxViewModel> GetGroupKbnsCombo(string systemCategory)
        {
            ObservableCollection<ComboBoxViewModel> ret = new ObservableCollection<ComboBoxViewModel>();
            
            try
            {
                // グループ区分の取得
                IDictionary<string, string> tanaorosiGroupKbns = this._accessor.SelectTanaorosiGroupKbns(systemCategory);
                foreach (var groupKbn in tanaorosiGroupKbns)
                {
                    ComboBoxViewModel item = new ComboBoxViewModel();
                    item.Key = groupKbn.Key;
                    item.Value = groupKbn.Value;
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
        /// 棚卸操作グループ追加イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnAddOperationGroup(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1060, "Add") == MessageManager.ResultType.Yes)
            {
                
                try
                {
                    string result = CheckOperationGroup(NewOperationGroup, ProcessType.Insert);
                    
                    if (result.Equals(String.Empty))
                    {

                        // 社員所属コード
                        if (SharedModel.Instance.OperationInfo != null && SharedModel.Instance.OperationInfo.EmployeeInfo != null)
                        {
                            EmployeeInfo enployeeInfo = SharedModel.Instance.OperationInfo.EmployeeInfo[NewOperationGroup.SyainCode.TrimEnd()];
                            NewOperationGroup.SyainSZCode = enployeeInfo.DeploymentCode;
                        }
                        // 操作グループの設定
                        IList<TanaorosiOpeartionGroup> list = new List<TanaorosiOpeartionGroup>();
                        list.Add(ORMapper.CopyProperty(NewOperationGroup, ORMapper.ProcessType.Insert));

                        // グループ区分マスタの挿入
                        var sqlResult = this._accessor.InsertTanaorosiOpeartionGroup(list);
                        switch (sqlResult)
                        {
                            case DBConstants.SQL_RESULT_OK:
                                OperationGroupRows.Add(new OperationGroupRow(this)
                                {
                                    // 社員コード
                                    SyainCode = NewOperationGroup.SyainCode.TrimEnd(),
                                    // 社員所属コード
                                    SyainSZCode = NewOperationGroup.SyainSZCode.TrimEnd(),
                                    // システム分類
                                    SystemCategory = NewOperationGroup.SystemCategory.TrimEnd(),
                                    // グループ区分
                                    GroupKbn = NewOperationGroup.GroupKbn.TrimEnd(),
                                    // グループ区分コンボボックス
                                    GroupKbns = TanaorosiGroupKbns,
                                    // 変更されているか
                                    IsDirty = false,
                                });

                                // 各種パラメータの初期化
                                NewOperationGroup.SyainCode = string.Empty;
                                NewOperationGroup.SyainSZCode = string.Empty;
                                NewOperationGroup.SystemCategory = Constants.SystemCategory.Sizai.GetStringValue();
                                NewOperationGroup.GroupKbn = string.Empty;
                                NewOperationGroup.IsDirty = false;
                                var view = CollectionViewSource.GetDefaultView(OperationGroupRows);
                                view.SortDescriptions.Add(new SortDescription("SyainCode", ListSortDirection.Ascending));
                                view.SortDescriptions.Add(new SortDescription("SystemCategory", ListSortDirection.Ascending));
                                MessageManager.ShowInformation(MessageManager.SystemID.CKSI1060, "Completed");
                                break;

                            case DBConstants.SQL_RESULT_ALREADY_EXISTS:
                                MessageManager.ShowError(MessageManager.SystemID.CKSI1060, "AlreadyExists");
                                break;

                            case DBConstants.SQL_RESULT_NG:
                                MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1060, "SystemError");
                                break;

                            default:
                                break;
                        }
                    }

                    else
                    {
                        // 入力データが不正
                        MessageManager.ShowError(MessageManager.SystemID.CKSI1060, result);
                    }
                }

                catch (Exception)
                {
                    // 例外エラー
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1060, "SystemError");
                }
            }
        }

        /// <summary>
        /// 棚卸操作グループ更新イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnUpdateOperationGroup(object sender, RoutedEventArgs e)
        {
            
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1060, "Modify") == MessageManager.ResultType.Yes)
            {
                var updateOperationGroup = (OperationGroupRow)(((Button)sender).Tag);

                try
                {
                    // 入力データのチェック
                    string result = CheckOperationGroup(updateOperationGroup, ProcessType.Update);

                    if (result.Equals(String.Empty))
                    {
                        // グループ情報の設定
                        List<TanaorosiOpeartionGroup> list = new List<TanaorosiOpeartionGroup>();
                        list.Add(ORMapper.CopyProperty(updateOperationGroup, ORMapper.ProcessType.Update));

                        var sqlResult = this._accessor.UpdateTanaorosiOpeartionGroup(list);
                        switch (sqlResult)
                        {
                            case DBConstants.SQL_RESULT_OK:
                                updateOperationGroup.IsDirty = false;
                                OperationGroupModification.IsDirty = OperationGroupRows.Any(anyOperationGroup => anyOperationGroup.IsDirty);
                                MessageManager.ShowInformation(MessageManager.SystemID.CKSI1060, "Completed");
                                break;

                            case DBConstants.SQL_RESULT_NG:
                                MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1060, "SystemError");
                                break;

                            default:
                                break;
                        }
                    }

                    else
                    {
                        // 入力データが不正
                        MessageManager.ShowError(MessageManager.SystemID.CKSI1060, result);
                    }
                }

                catch (Exception)
                {
                    // 例外エラー
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1060, "SystemError");
                }
            }
        }

        /// <summary>
        /// 棚卸操作グループ削除イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnDeleteOperationGroup(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1060, "Delete") == MessageManager.ResultType.Yes)
            {
                var deleteOperationGroup = (OperationGroupRow)(((Button)sender).Tag);

                try
                {
                    // システム分類情報の設定
                    List<TanaorosiOpeartionGroup> list = new List<TanaorosiOpeartionGroup>();
                        list.Add(ORMapper.CopyProperty(deleteOperationGroup, ORMapper.ProcessType.Delete));

                        var sqlResult = this._accessor.DeleteTanaorosiOpeartionGroup(list);
                    switch (sqlResult)
                    {
                        case DBConstants.SQL_RESULT_OK:
                            OperationGroupRows.Remove(deleteOperationGroup);
                            OperationGroupModification.IsDirty = OperationGroupRows.Any(anyOperationGroup => anyOperationGroup.IsDirty);
                            MessageManager.ShowInformation(MessageManager.SystemID.CKSI1060, "Completed");
                            break;

                        case DBConstants.SQL_RESULT_NG:
                            MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1060, "SystemError");
                            break;

                        default:
                            break;
                    }

                }

                catch (Exception)
                {
                    // 例外エラー
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1060, "SystemError");
                }
            }
        }

        /// <summary>
        /// 一括更新イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnUpdateAllOperationGroup(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1060, "Modify") == MessageManager.ResultType.Yes)
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
                    IList<TanaorosiOpeartionGroup> updateList = new List<TanaorosiOpeartionGroup>();

                    worker.DoWork += (s, ev) =>
                    {

                        foreach (OperationGroupRow updateOperationGroup in OperationGroupRows)
                        {
                            if (updateOperationGroup.IsDirty)
                            {
                                // 入力データのチェック
                                error = CheckOperationGroup(updateOperationGroup, ProcessType.Update);
                                if (error.Equals(String.Empty))
                                {
                                    // 棚卸操作グループ情報の設定
                                    updateList.Add(ORMapper.CopyProperty(updateOperationGroup, ORMapper.ProcessType.Update));
                                }
                                else
                                {
                                    // 入力データが不正
                                    MessageManager.ShowError(MessageManager.SystemID.CKSI1060, error);
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
                            var sqlResult = this._accessor.UpdateTanaorosiOpeartionGroup(updateList);
                            switch (sqlResult)
                            {
                                case DBConstants.SQL_RESULT_OK:
                                    foreach (var updateOperationGroup in OperationGroupRows)
                                    {
                                        updateOperationGroup.IsDirty = false;
                                    }

                                    OperationGroupModification.IsDirty = false;
                                    ProcessState.IsBusy = false;
                                    MessageManager.ShowInformation(MessageManager.SystemID.CKSI1060, "Completed");
                                    break;

                                case DBConstants.SQL_RESULT_NG:
                                    ProcessState.IsBusy = false;
                                    MessageManager.ShowError(MessageManager.SystemID.CKSI1060, "SystemError");
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
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1060, "SystemError");
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
            if (!CreateTanaorosiGroupKbnsCombo(NewOperationGroup.SystemCategory))
            {
                string systemCategoryName = (SharedModel.Instance.OperationInfo.SystemCategory[NewOperationGroup.SystemCategory]).Trim();
                MessageManager.ShowWarning(MessageManager.SystemID.CKSI1060, "NonGroupKbnMstTieSystemCategory", systemCategoryName);
            }
        }

        /// <summary>
        /// 棚卸操作グループ一覧の再検索
        /// </summary>
        private void RefreshOperationGroup()
        {
            BackgroundWorker worker = new BackgroundWorker();

            try
            {
                // 処理中か
                ProcessState.IsBusy = true;
                // 処理中のメッセージ
                ProcessState.BusyMessage = "一覧を取得中...";

                // 棚卸操作グループ情報
                IList<TanaorosiOpeartionGroup> tanaorosiOpeartionGroupList = null;

                worker.DoWork += (s, ev) =>
                {
                    // 棚卸操作グループマスタからの取得
                    tanaorosiOpeartionGroupList = this._accessor.SelectTanaorosiOpeartionGroup();
                };

                worker.RunWorkerCompleted += (s, ex) =>
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        OperationGroupRows.Clear();

                        foreach (TanaorosiOpeartionGroup tanaorosiOpeartionGroup in tanaorosiOpeartionGroupList)
                        {
                            OperationGroupRows.Add(new OperationGroupRow(this)
                            {
                                //社員コード
                                SyainCode = tanaorosiOpeartionGroup.SyainCode.Trim(),
                                //社員所属コード
                                SyainSZCode = tanaorosiOpeartionGroup.SyainSZCode.Trim(),
                                //システム分類
                                SystemCategory = tanaorosiOpeartionGroup.SystemCategory.Trim(),
                                //グループ区分
                                GroupKbn = tanaorosiOpeartionGroup.GroupKbn.Trim(),
                                //グループ区分コンボボックス
                                GroupKbns = GetGroupKbnsCombo(tanaorosiOpeartionGroup.SystemCategory.Trim()),
                                // 変更されているか
                                IsDirty = false
                            });
                        }

                        if (OperationGroupRows.Count > 0)
                            this.Loaded += DelegateSetFocusToDataGridView;
                        else
                            this.Loaded += DelegateSetFocusToTextBox;

                        OperationGroupModification.IsDirty = false;
                        ProcessState.IsBusy = false;
                    }));
                };
            }

            catch (Exception)
            {
                OperationGroupModification.IsDirty = false;
                ProcessState.IsBusy = false;
                MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1060, "SystemError");
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
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1060, "ConfirmExit") == MessageManager.ResultType.No)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 操作グループの入力データのチェック
        /// </summary>
        /// <param name="row">操作グループオブジェクト</param>
        /// <param name="type">DB操作</param>
        /// <returns>チェック結果</returns>
        private string CheckOperationGroup(OperationGroupRow row, ProcessType type)
        {
            string result = string.Empty;

            switch (type)
            {
                //挿入
                case ProcessType.Insert:
                    // 社員番号の入力チェック
                    if (row.SyainCode == null || row.SyainCode.Trim().Length != 4)
                    {
                        result = "NotInputSyainCD";
                        break;
                    }
                    else
                    {
                        if (!SharedModel.Instance.OperationInfo.EmployeeInfo.ContainsKey(row.SyainCode.Trim()))
                        {
                            result = "NotInputSyainCD";
                            break;
                        }
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
                textBox.AddValueChanged(this._dataGrid, (s, ev) => SetFocusToTextBox(this._txtSyainName));
                SetFocusToTextBox(this._txtSyainName);
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
        public void NotifyTanaorosiOperationGroupModified()
        {
            OperationGroupModification.IsDirty = OperationGroupRows.Any(OperationGroup => OperationGroup.IsDirty);
        }

        #endregion
    }
}