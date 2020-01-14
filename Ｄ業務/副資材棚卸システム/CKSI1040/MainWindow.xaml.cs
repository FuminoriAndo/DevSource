using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CKSI1040.Common;
using CKSI1040.ViewModel;
using Common;
using DBManager;
using DBManager.Constants;
using DBManager.Model;
using Microsoft.Windows.Controls;
using Shared;

//*************************************************************************************
//
//   MainWindow.xaml の相互作用ロジック
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.02.01              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1040
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, IGroupKbnContainer
    {
        #region フィールド

        /// <summary>
        /// DB操作の制御オブジェクト
        /// </summary>
        private DBAccessor _accessor = null;

        #endregion

        #region 列挙型

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
        public GroupKbnManagementMode GroupKbnManagementMode
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
        /// グループ区分一覧
        /// </summary>
        public ObservableCollection<GroupKbnRow> GroupKbnRows
        {
            get;
            private set;
        }

        /// <summary>
        /// グループ区分の変更状態
        /// </summary>
        public GroupKbnModification GroupKbnModification
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
        /// 新規グループ区分
        /// </summary>
        public GroupKbnRow NewGroupKbn
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
            GroupKbnManagementMode = new GroupKbnManagementMode() 
                { CurrentMode = GroupKbnManagementMode.ModeType.Modify };
            ProcessState = new ProcessState();
            GroupKbnModification = new GroupKbnModification();
            GroupKbnRows = new ObservableCollection<GroupKbnRow>();
            TanaorosiSystemCategorys = new ObservableCollection<ComboBoxViewModel>();
            NewGroupKbn = new GroupKbnRow(this);
            this._accessor = new DBAccessor(DBConstants.DBType.ORACLE);
            InitializeComponent();
            CreateTanaorosiSystemCategorysCombo();
            GroupKbnCategorys();
        }

        #endregion

        #region メソッド

        /// <summary>
        /// グループ区分追加イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnAddGroupKbn(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1040, "Add") == MessageManager.ResultType.Yes)
            {
                try
                {
                    string result = CheckGroupKbn(NewGroupKbn, ProcessType.Insert);
                    
                    if (result.Equals(String.Empty))
                    {

                        // グループ区分情報の設定
                        IList<TanaorosiGroupKbn> list = new List<TanaorosiGroupKbn>();
                        list.Add(ORMapper.CopyProperty(NewGroupKbn, ORMapper.ProcessType.Insert));

                        // グループ区分マスタの挿入
                        var sqlResult = this._accessor.InsertTanaorosiGroupKbn(list);
                        switch (sqlResult)
                        {
                            case DBConstants.SQL_RESULT_OK:
                                GroupKbnRows.Add(new GroupKbnRow(this)
                                {
                                    // システム分類
                                    SystemCategory = NewGroupKbn.SystemCategory.Trim(),
                                    // グループ区分
                                    GroupKbn = NewGroupKbn.GroupKbn.Trim(),
                                    // グループ区分名
                                    GroupKbnName = NewGroupKbn.GroupKbnName.Trim(),
                                    // 変更されているか
                                    IsDirty = false,
                                });

                                // 各種パラメータの初期化
                                NewGroupKbn.SystemCategory = string.Empty;
                                NewGroupKbn.GroupKbn = string.Empty;
                                NewGroupKbn.GroupKbnName = string.Empty;
                                NewGroupKbn.IsDirty = false;
                                var view = CollectionViewSource.GetDefaultView(GroupKbnRows);
                                view.SortDescriptions.Add(new SortDescription("SystemCategory", ListSortDirection.Ascending));
                                view.SortDescriptions.Add(new SortDescription("GroupKbn", ListSortDirection.Ascending));
                                MessageManager.ShowInformation(MessageManager.SystemID.CKSI1040, "Completed");
                                break;

                            case DBConstants.SQL_RESULT_ALREADY_EXISTS:
                                MessageManager.ShowError(MessageManager.SystemID.CKSI1040, "AlreadyExists");
                                break;

                            case DBConstants.SQL_RESULT_NG:
                                MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1040, "SystemError");
                                break;

                            default:
                                break;
                        }
                    }

                    else
                    {
                        // 入力データが不正
                        MessageManager.ShowError(MessageManager.SystemID.CKSI1040, result);
                    }
                }

                catch (Exception)
                {
                    // 例外エラー
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1040, "SystemError");
                }
            }
        }

        /// <summary>
        /// グループ区分更新イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnUpdateGroupKbn(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1040, "Modify") == MessageManager.ResultType.Yes)
            {
                var updateGroupKbn = (GroupKbnRow)(((Button)sender).Tag);

                try
                {
                    // 入力データのチェック
                    string result = CheckGroupKbn(updateGroupKbn, ProcessType.Update);

                    if (result.Equals(String.Empty))
                    {
                        // グループ情報の設定
                        List<TanaorosiGroupKbn> list = new List<TanaorosiGroupKbn>();
                        list.Add(ORMapper.CopyProperty(updateGroupKbn, ORMapper.ProcessType.Update));

                        var sqlResult = this._accessor.UpdateTanaorosiGroupKbn(list);
                        switch (sqlResult)
                        {
                            case DBConstants.SQL_RESULT_OK:
                                updateGroupKbn.IsDirty = false;
                                GroupKbnModification.IsDirty = GroupKbnRows.Any(anyGroupKbn => anyGroupKbn.IsDirty);
                                MessageManager.ShowInformation(MessageManager.SystemID.CKSI1040, "Completed");
                                break;

                            case DBConstants.SQL_RESULT_NG:
                                MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1040, "SystemError");
                                break;

                            default:
                                break;
                        }
                    }

                    else
                    {
                        // 入力データが不正
                        MessageManager.ShowError(MessageManager.SystemID.CKSI1040, result);
                    }
                }

                catch (Exception)
                {
                    // 例外エラー
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1040, "SystemError");
                }
            }
        }

        /// <summary>
        /// グループ区分削除イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnDeleteGroupKbn(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1040, "Delete") == MessageManager.ResultType.Yes)
            {
                var deleteGroupKbn = (GroupKbnRow)(((Button)sender).Tag);

                try
                {
                    // システム分類情報の設定
                    List<TanaorosiGroupKbn> list = new List<TanaorosiGroupKbn>();
                        list.Add(ORMapper.CopyProperty(deleteGroupKbn, ORMapper.ProcessType.Delete));

                        var sqlResult = this._accessor.DeleteTanaorosiGroupKbn(list);
                    switch (sqlResult)
                    {
                        case DBConstants.SQL_RESULT_OK:
                            GroupKbnRows.Remove(deleteGroupKbn);
                            GroupKbnModification.IsDirty = GroupKbnRows.Any(anyGroupKbn => anyGroupKbn.IsDirty);
                            MessageManager.ShowInformation(MessageManager.SystemID.CKSI1040, "Completed");
                            break;

                        case DBConstants.SQL_RESULT_NG:
                            MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1040, "SystemError");
                            break;

                        default:
                            break;
                    }

                }

                catch (Exception)
                {
                    // 例外エラー
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1040, "SystemError");
                }
            }
        }

        /// <summary>
        /// 一括更新イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnUpdateAllGroupKbn(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1040, "Modify") == MessageManager.ResultType.Yes)
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
                    IList<TanaorosiGroupKbn> updateList = new List<TanaorosiGroupKbn>();

                    worker.DoWork += (s, ev) =>
                    {

                        foreach (GroupKbnRow updateGroupKbn in GroupKbnRows)
                        {
                            if (updateGroupKbn.IsDirty)
                            {
                                // 入力データのチェック
                                error = CheckGroupKbn(updateGroupKbn, ProcessType.Update);
                                if (error.Equals(String.Empty))
                                {
                                    // グループ区分情報の設定
                                    updateList.Add(ORMapper.CopyProperty(updateGroupKbn, ORMapper.ProcessType.Update));
                                }
                                else
                                {
                                    // 入力データが不正
                                    MessageManager.ShowError(MessageManager.SystemID.CKSI1040, error);
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
                            var sqlResult = this._accessor.UpdateTanaorosiGroupKbn(updateList);
                            switch (sqlResult)
                            {
                                case DBConstants.SQL_RESULT_OK:
                                    foreach (var updateGroupKbn in GroupKbnRows)
                                    {
                                        updateGroupKbn.IsDirty = false;
                                    }

                                    GroupKbnModification.IsDirty = false;
                                    ProcessState.IsBusy = false;
                                    MessageManager.ShowInformation(MessageManager.SystemID.CKSI1040, "Completed");
                                    break;

                                case DBConstants.SQL_RESULT_NG:
                                    ProcessState.IsBusy = false;
                                    MessageManager.ShowError(MessageManager.SystemID.CKSI1040, "SystemError");
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
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1040, "SystemError");
                }

                worker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// グループ区分一覧の再検索
        /// </summary>
        private void GroupKbnCategorys()
        {
            BackgroundWorker worker = new BackgroundWorker();

            try
            {
                // 処理中か
                ProcessState.IsBusy = true;
                // 処理中のメッセージ
                ProcessState.BusyMessage = "一覧を取得中...";

                // 棚卸システム分類情報
                IList<TanaorosiGroupKbn> tanaorosiGroupKbnList = null;

                worker.DoWork += (s, ev) =>
                {
                    // 棚卸システム分類マスタからの取得
                    tanaorosiGroupKbnList = this._accessor.SelectTanaorosiGroupKbn();
                };

                worker.RunWorkerCompleted += (s, ex) =>
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        GroupKbnRows.Clear();

                        foreach (TanaorosiGroupKbn tanaorosiGroupKbn in tanaorosiGroupKbnList)
                        {
                            GroupKbnRows.Add(new GroupKbnRow(this)
                            {
                                //システム分類
                                SystemCategory = tanaorosiGroupKbn.SystemCategory,
                                //グループ区分
                                GroupKbn = tanaorosiGroupKbn.GroupKbn,
                                //グループ区分名
                                GroupKbnName = tanaorosiGroupKbn.GroupKbnName,
                                // 変更されているか
                                IsDirty = false
                            });
                        }

                        if (GroupKbnRows.Count > 0)
                            this.Loaded += DelegateSetFocusToDataGridView;
                        else
                            this.Loaded += DelegateSetFocusToTextBox;

                        GroupKbnModification.IsDirty = false;
                        ProcessState.IsBusy = false;
                    }));
                };
            }

            catch (Exception)
            {
                GroupKbnModification.IsDirty = false;
                ProcessState.IsBusy = false;
                MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1040, "SystemError");
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
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1040, "ConfirmExit") == MessageManager.ResultType.No)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// グループ区分の入力データのチェック
        /// </summary>
        /// <param name="user">グループ区分オブジェクト</param>
        /// <param name="type">DB操作</param>
        /// <returns>チェック結果</returns>
        private string CheckGroupKbn(GroupKbnRow row, ProcessType type)
        {
            string result = string.Empty;

            switch (type)
            {
                //挿入
                case ProcessType.Insert:
                    //システム分類のチェック
                    result = InputChecker.CheckSystemCategory(row.SystemCategory);
                    if (result.Equals(String.Empty))
                    {
                        //グループ区分のチェック
                        result = InputChecker.CheckGroupKbn(row.GroupKbn);
                        if (result.Equals(String.Empty))
                        {
                            //グループ区分名のチェック
                            result = InputChecker.CheckGroupKbnName(row.GroupKbnName);
                        }
                    }
                    break;

                //更新
                case ProcessType.Update:
                    //グループ区分名のチェック
                    result = InputChecker.CheckGroupKbnName(row.GroupKbnName);
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
                textBox.AddValueChanged(this._dataGrid, (s, ev) => SetFocusToTextBox(this._txtGroupKbn));
                SetFocusToTextBox(this._txtGroupKbn);
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
        public void NotifyTanaorosiGroupKbnModified()
        {
            GroupKbnModification.IsDirty = GroupKbnRows.Any(systemCategory => systemCategory.IsDirty);
        }

        #endregion
    }
}




