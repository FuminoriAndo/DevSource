using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CKSI1050.Common;
using CKSI1050.ViewModel;
using Common;
using DBManager;
using DBManager.Constants;
using DBManager.Model;
using Microsoft.Windows.Controls;
using Shared;

namespace CKSI1050
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, ITanaorosiOperationContainer
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
        public TanaorosiOperationManagementMode TanaorosiOperationManagementMode
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
        /// 棚卸操作一覧
        /// </summary>
        public ObservableCollection<TanaorosiOperationInfo> TanaorosiOperations
        {
            get;
            private set;
        }

        /// <summary>
        /// 棚卸操作の変更状態
        /// </summary>
        public TanaorosiOperationModification TanaorosiOperationModification
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
        /// 新規棚卸操作
        /// </summary>
        public TanaorosiOperationInfo NewTanaorosiOperation
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
            TanaorosiOperationManagementMode = new TanaorosiOperationManagementMode() 
                { CurrentMode = TanaorosiOperationManagementMode.ModeType.Modify };
            ProcessState = new ProcessState();
            TanaorosiSystemCategorys = new ObservableCollection<ComboBoxViewModel>();
            TanaorosiOperationModification = new TanaorosiOperationModification();
            TanaorosiOperations = new ObservableCollection<TanaorosiOperationInfo>();
            NewTanaorosiOperation = new TanaorosiOperationInfo(this);
            this._accessor = new DBAccessor(DBConstants.DBType.ORACLE);
            InitializeComponent();
            CreateTanaorosiSystemCategorysCombo();
            RefreshTanaorosiOperationInfoList();
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 棚卸操作追加イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnAddTanaorosiOperation(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1050, "Add") == MessageManager.ResultType.Yes)
            {
                try
                {
                    string result = CheckTanaorosiOperation(NewTanaorosiOperation, ProcessType.Insert);
                    
                    if (result.Equals(String.Empty))
                    {

                        // 棚卸操作情報の設定
                        IList<TanaorosiOperation> list = new List<TanaorosiOperation>();
                        list.Add(ORMapper.CopyProperty(NewTanaorosiOperation, ORMapper.ProcessType.Insert));

                        // 棚卸操作マスタの挿入
                        var sqlResult = this._accessor.InsertTanaorosiOperation(list);
                        switch (sqlResult)
                        {
                            case DBConstants.SQL_RESULT_OK:
                                TanaorosiOperations.Add(new TanaorosiOperationInfo(this)
                                {
                                    // システム分類
                                    SystemCategory = NewTanaorosiOperation.SystemCategory,
                                    // 操作コード
                                    OperationCode = NewTanaorosiOperation.OperationCode,
                                    // 操作名
                                    OperationName = NewTanaorosiOperation.OperationName,
                                    // 変更されているか
                                    IsDirty = false,
                                });

                                // 各種パラメータの初期化
                                NewTanaorosiOperation.SystemCategory = string.Empty;
                                NewTanaorosiOperation.OperationCode = string.Empty;
                                NewTanaorosiOperation.OperationName = string.Empty;
                                NewTanaorosiOperation.IsDirty = false;
                                var view = CollectionViewSource.GetDefaultView(TanaorosiOperations);
                                view.SortDescriptions.Add(new SortDescription("SystemCategory", ListSortDirection.Ascending));
                                view.SortDescriptions.Add(new SortDescription("OperationCode", ListSortDirection.Ascending));
                                MessageManager.ShowInformation(MessageManager.SystemID.CKSI1050, "Completed");
                                break;

                            case DBConstants.SQL_RESULT_ALREADY_EXISTS:
                                MessageManager.ShowError(MessageManager.SystemID.CKSI1050, "AlreadyExists");
                                break;

                            case DBConstants.SQL_RESULT_NG:
                                MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1050, "SystemError");
                                break;

                            default:
                                break;
                        }
                    }

                    else
                    {
                        // 入力データが不正
                        MessageManager.ShowError(MessageManager.SystemID.CKSI1050, result);
                    }
                }

                catch (Exception)
                {
                    // 例外エラー
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1050, "SystemError");
                }
            }
        }

        /// <summary>
        /// 棚卸操作更新イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnUpdateTanaorosiOperation(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1050, "Modify") == MessageManager.ResultType.Yes)
            {
                var updateTanaorosiOperation = (TanaorosiOperationInfo)(((Button)sender).Tag);

                try
                {
                    // 入力データのチェック
                    string result = CheckTanaorosiOperation(updateTanaorosiOperation, ProcessType.Update);

                    if (result.Equals(String.Empty))
                    {
                        // 棚卸操作情報の設定
                        IList<TanaorosiOperation> list = new List<TanaorosiOperation>();
                        list.Add(ORMapper.CopyProperty(updateTanaorosiOperation, ORMapper.ProcessType.Update));

                        var sqlResult = this._accessor.UpdateTanaorosiOperation(list);
                        switch (sqlResult)
                        {
                            case DBConstants.SQL_RESULT_OK:
                                updateTanaorosiOperation.IsDirty = false;
                                TanaorosiOperationModification.IsDirty = TanaorosiOperations.Any(anyTanaorosiOperation => anyTanaorosiOperation.IsDirty);
                                MessageManager.ShowInformation(MessageManager.SystemID.CKSI1050, "Completed");
                                break;

                            case DBConstants.SQL_RESULT_NG:
                                MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1050, "SystemError");
                                break;

                            default:
                                break;
                        }
                    }

                    else
                    {
                        // 入力データが不正
                        MessageManager.ShowError(MessageManager.SystemID.CKSI1050, result);
                    }
                }

                catch (Exception)
                {
                    // 例外エラー
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1050, "SystemError");
                }
            }
        }

        /// <summary>
        /// 棚卸操作削除イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnDeleteTanaorosiOperation(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1050, "Delete") == MessageManager.ResultType.Yes)
            {
                var deleteTanaorosiOperation = (TanaorosiOperationInfo)(((Button)sender).Tag);

                try
                {
                    // 棚卸操作情報の設定
                    IList<TanaorosiOperation> list = new List<TanaorosiOperation>();
                    list.Add(ORMapper.CopyProperty(deleteTanaorosiOperation, ORMapper.ProcessType.Delete));

                    var sqlResult = this._accessor.DeleteTanaorosiOperation(list);
                    switch (sqlResult)
                    {
                        case DBConstants.SQL_RESULT_OK:
                            TanaorosiOperations.Remove(deleteTanaorosiOperation);
                            TanaorosiOperationModification.IsDirty = TanaorosiOperations.Any(anyTanaorosiOperation => anyTanaorosiOperation.IsDirty);
                            MessageManager.ShowInformation(MessageManager.SystemID.CKSI1050, "Completed");
                            break;

                        case DBConstants.SQL_RESULT_NG:
                            MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1050, "SystemError");
                            break;

                        default:
                            break;
                    }

                }

                catch (Exception)
                {
                    // 例外エラー
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1050, "SystemError");
                }
            }
        }

        /// <summary>
        /// 一括更新イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnUpdateAllTanaorosiOperation(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1050, "Modify") == MessageManager.ResultType.Yes)
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
                    IList<TanaorosiOperation> updateList = new List<TanaorosiOperation>();

                    worker.DoWork += (s, ev) =>
                    {

                        foreach (TanaorosiOperationInfo updateTanaorosiOperation in TanaorosiOperations)
                        {
                            if (updateTanaorosiOperation.IsDirty)
                            {
                                // 入力データのチェック
                                error = CheckTanaorosiOperation(updateTanaorosiOperation, ProcessType.Update);
                                if (error.Equals(String.Empty))
                                {
                                    // 棚卸操作情報の設定
                                    updateList.Add(ORMapper.CopyProperty(updateTanaorosiOperation, ORMapper.ProcessType.Update));
                                }
                                else
                                {
                                    // 入力データが不正
                                    MessageManager.ShowError(MessageManager.SystemID.CKSI1050, error);
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
                            var sqlResult = this._accessor.UpdateTanaorosiOperation(updateList);
                            switch (sqlResult)
                            {
                                case DBConstants.SQL_RESULT_OK:
                                    foreach (var updateTanaorosiOpeation in TanaorosiOperations)
                                    {
                                        updateTanaorosiOpeation.IsDirty = false;
                                    }

                                    TanaorosiOperationModification.IsDirty = false;
                                    ProcessState.IsBusy = false;
                                    MessageManager.ShowInformation(MessageManager.SystemID.CKSI1050, "Completed");
                                    break;

                                case DBConstants.SQL_RESULT_NG:
                                    ProcessState.IsBusy = false;
                                    MessageManager.ShowError(MessageManager.SystemID.CKSI1050, "SystemError");
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
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1050, "SystemError");
                }

                worker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// 棚卸操作一覧の再検索
        /// </summary>
        private void RefreshTanaorosiOperationInfoList()
        {
            BackgroundWorker worker = new BackgroundWorker();

            try
            {
                // 処理中か
                ProcessState.IsBusy = true;
                // 処理中のメッセージ
                ProcessState.BusyMessage = "一覧を取得中...";

                // 棚卸操作情報
                IList<TanaorosiOperation> tanaorosiOperationList = null;

                worker.DoWork += (s, ev) =>
                {
                    // 棚卸操作マスタからの取得
                    tanaorosiOperationList = this._accessor.SelectTanaorosiOperation();
                };

                worker.RunWorkerCompleted += (s, ex) =>
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        TanaorosiOperations.Clear();

                        foreach (var tanaorosiOperation in tanaorosiOperationList)
                        {
                            TanaorosiOperations.Add(new TanaorosiOperationInfo(this)
                            {
                                // システム分類
                                SystemCategory = tanaorosiOperation.SystemCategory.Trim(),
                                // 操作名
                                OperationCode = tanaorosiOperation.OperationCode.Trim(),
                                // 操作コード
                                OperationName = tanaorosiOperation.OperationName.Trim(),
                                // 変更されているか
                                IsDirty = false
                            });
                        }

                        if (TanaorosiOperations.Count > 0)
                            this.Loaded += DelegateSetFocusToDataGridView;
                        else
                            this.Loaded += DelegateSetFocusToTextBox;

                        TanaorosiOperationModification.IsDirty = false;
                        ProcessState.IsBusy = false;
                    }));
                };
            }

            catch (Exception)
            {
                TanaorosiOperationModification.IsDirty = false;
                ProcessState.IsBusy = false;
                MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1050, "SystemError");
            }

            worker.RunWorkerAsync();

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
                SharedModel.Instance.SystemCategory = tanaorosiSystemCategorys;
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
        /// 閉じる「×」ボタン押下時のイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1050, "ConfirmExit") == MessageManager.ResultType.No)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 棚卸操作の入力データのチェック
        /// </summary>
        /// <param name="user">棚卸操作オブジェクト</param>
        /// <param name="type">DB操作</param>
        /// <returns>チェック結果</returns>
        private string CheckTanaorosiOperation(TanaorosiOperationInfo operationInfo, ProcessType type)
        {
            string result = string.Empty;

            switch (type)
            {
                //挿入
                case ProcessType.Insert:
                    //システム分類のチェック
                    result = InputChecker.CheckSystemCategory(operationInfo.SystemCategory);
                    if (result.Equals(String.Empty))
                    {
                        // 操作コードのチェック
                        result = InputChecker.CheckTanaorosiOperationCode(operationInfo.OperationCode);
                        if (result.Equals(String.Empty))
                        {
                            result = InputChecker.CheckTanaorosiOperationName(operationInfo.OperationName);
                        }
                    }
                    break;

                //更新
                case ProcessType.Update:
                    // 操作名のチェック
                    result = InputChecker.CheckTanaorosiOperationName(operationInfo.OperationName);
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
                textBox.AddValueChanged(this._dataGrid, (s, ev) => SetFocusToTextBox(this._txtOperationCD));
                SetFocusToTextBox(this._txtOperationCD);
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
        /// 棚卸操作変更イベント
        /// </summary>
        public void NotifyTanaorosiOperationModified()
        {
            TanaorosiOperationModification.IsDirty = TanaorosiOperations.Any(tanaorosiOperation => tanaorosiOperation.IsDirty);
        }

        #endregion
    }
}




