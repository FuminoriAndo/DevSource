using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CKSI1030.Common;
using CKSI1030.ViewModel;
using Common;
using DBManager;
using DBManager.Constants;
using DBManager.Model;
using Microsoft.Windows.Controls;
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

namespace CKSI1030
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, ISystemCategoryContainer
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
        public SystemCategoryManagementMode SystemCategoryManagementMode
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
        /// システム分類一覧
        /// </summary>
        public ObservableCollection<SystemCategoryInfo> SystemCategorys
        {
            get;
            private set;
        }

        /// <summary>
        /// システム分類の変更状態
        /// </summary>
        public SystemCategoryModification SystemCategoryModification
        {
            get;
            private set;
        }

        /// <summary>
        /// 新規システム分類
        /// </summary>
        public SystemCategoryInfo NewSystemCategory
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
            SystemCategoryManagementMode = new SystemCategoryManagementMode() 
                { CurrentMode = SystemCategoryManagementMode.ModeType.Modify };
            ProcessState = new ProcessState();
            SystemCategoryModification = new SystemCategoryModification();
            SystemCategorys = new ObservableCollection<SystemCategoryInfo>();
            NewSystemCategory = new SystemCategoryInfo(this);
            this._accessor = new DBAccessor(DBConstants.DBType.ORACLE);
            InitializeComponent();
            RefreshSystemCategorys();
        }

        #endregion

        #region メソッド

        /// <summary>
        /// システム分類追加イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnAddSystemCategory(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1030, "Add") == MessageManager.ResultType.Yes)
            {
                try
                {
                    string result = CheckSystemCategory(NewSystemCategory, ProcessType.Insert);
                    
                    if (result.Equals(String.Empty))
                    {

                        // システム分類情報の設定
                        IList<TanaorosiSystemCategory> list = new List<TanaorosiSystemCategory>();
                        list.Add(ORMapper.CopyProperty(NewSystemCategory, ORMapper.ProcessType.Insert));

                        // システム分類マスタの挿入
                        var sqlResult = this._accessor.InsertTanaorosiSystemCategory(list);
                        switch (sqlResult)
                        {
                            case DBConstants.SQL_RESULT_OK:
                                SystemCategorys.Add(new SystemCategoryInfo(this)
                                {
                                    // システム分類
                                    SystemCategory = NewSystemCategory.SystemCategory.Trim(),
                                    // システム分類名
                                    SystemCategoryName = NewSystemCategory.SystemCategoryName.Trim(),
                                    // 変更されているか
                                    IsDirty = false,
                                });

                                // 各種パラメータの初期化
                                NewSystemCategory.SystemCategory = string.Empty;
                                NewSystemCategory.SystemCategoryName = string.Empty;
                                NewSystemCategory.IsDirty = false;
                                var view = CollectionViewSource.GetDefaultView(SystemCategorys);
                                view.SortDescriptions.Add(new SortDescription("SystemCategory", ListSortDirection.Ascending));
                                MessageManager.ShowInformation(MessageManager.SystemID.CKSI1030, "Completed");
                                break;

                            case DBConstants.SQL_RESULT_ALREADY_EXISTS:
                                MessageManager.ShowError(MessageManager.SystemID.CKSI1030, "AlreadyExists");
                                break;

                            case DBConstants.SQL_RESULT_NG:
                                MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1030, "SystemError");
                                break;

                            default:
                                break;
                        }
                    }

                    else
                    {
                        // 入力データが不正
                        MessageManager.ShowError(MessageManager.SystemID.CKSI1030, result);
                    }
                }

                catch (Exception)
                {
                    // 例外エラー
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1030, "SystemError");
                }
            }
        }

        /// <summary>
        /// システム分類更新イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnUpdateSystemCategory(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1030, "Modify") == MessageManager.ResultType.Yes)
            {
                var updateSystemCategory = (SystemCategoryInfo)(((Button)sender).Tag);

                try
                {
                    // 入力データのチェック
                    string result = CheckSystemCategory(updateSystemCategory, ProcessType.Update);

                    if (result.Equals(String.Empty))
                    {
                        // システム分類情報の設定
                        List<TanaorosiSystemCategory> list = new List<TanaorosiSystemCategory>();
                        list.Add(ORMapper.CopyProperty(updateSystemCategory, ORMapper.ProcessType.Update));

                        var sqlResult = this._accessor.UpdateTanaorosiSystemCategory(list);
                        switch (sqlResult)
                        {
                            case DBConstants.SQL_RESULT_OK:
                                updateSystemCategory.IsDirty = false;
                                SystemCategoryModification.IsDirty = SystemCategorys.Any(anySystemCategory => anySystemCategory.IsDirty);
                                MessageManager.ShowInformation(MessageManager.SystemID.CKSI1030, "Completed");
                                break;

                            case DBConstants.SQL_RESULT_NG:
                                MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1030, "SystemError");
                                break;

                            default:
                                break;
                        }
                    }

                    else
                    {
                        // 入力データが不正
                        MessageManager.ShowError(MessageManager.SystemID.CKSI1030, result);
                    }
                }

                catch (Exception)
                {
                    // 例外エラー
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1030, "SystemError");
                }
            }
        }

        /// <summary>
        /// システム分類削除イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnDeleteSystemCategory(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1030, "Delete") == MessageManager.ResultType.Yes)
            {
                var deleteSystemCategory = (SystemCategoryInfo)(((Button)sender).Tag);

                try
                {
                    // システム分類情報の設定
                    List<TanaorosiSystemCategory> list = new List<TanaorosiSystemCategory>();
                        list.Add(ORMapper.CopyProperty(deleteSystemCategory, ORMapper.ProcessType.Delete));

                    var sqlResult = this._accessor.DeleteTanaorosiSystemCategory(list);
                    switch (sqlResult)
                    {
                        case DBConstants.SQL_RESULT_OK:
                            SystemCategorys.Remove(deleteSystemCategory);
                            SystemCategoryModification.IsDirty = SystemCategorys.Any(anySystemCategory => anySystemCategory.IsDirty);
                            MessageManager.ShowInformation(MessageManager.SystemID.CKSI1030, "Completed");
                            break;

                        case DBConstants.SQL_RESULT_NG:
                            MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1030, "SystemError");
                            break;

                        default:
                            break;
                    }

                }

                catch (Exception)
                {
                    // 例外エラー
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1030, "SystemError");
                }
            }
        }

        /// <summary>
        /// 一括更新イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnUpdateAllSystemCategoryInfo(object sender, RoutedEventArgs e)
        {
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1030, "Modify") == MessageManager.ResultType.Yes)
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
                    IList<TanaorosiSystemCategory> updateList = new List<TanaorosiSystemCategory>();

                    worker.DoWork += (s, ev) =>
                    {

                        foreach (SystemCategoryInfo updateSystemCategory in SystemCategorys)
                        {
                            if (updateSystemCategory.IsDirty)
                            {
                                // 入力データのチェック
                                error = CheckSystemCategory(updateSystemCategory, ProcessType.Update);
                                if (error.Equals(String.Empty))
                                {
                                    // システム分類情報の設定
                                    updateList.Add(ORMapper.CopyProperty(updateSystemCategory, ORMapper.ProcessType.Update));
                                }
                                else
                                {
                                    // 入力データが不正
                                    MessageManager.ShowError(MessageManager.SystemID.CKSI1030, error);
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
                            var sqlResult = this._accessor.UpdateTanaorosiSystemCategory(updateList);
                            switch (sqlResult)
                            {
                                case DBConstants.SQL_RESULT_OK:
                                    foreach (var updateSystemCategory in SystemCategorys)
                                    {
                                        updateSystemCategory.IsDirty = false;
                                    }

                                    SystemCategoryModification.IsDirty = false;
                                    ProcessState.IsBusy = false;
                                    MessageManager.ShowInformation(MessageManager.SystemID.CKSI1030, "Completed");
                                    break;

                                case DBConstants.SQL_RESULT_NG:
                                    ProcessState.IsBusy = false;
                                    MessageManager.ShowError(MessageManager.SystemID.CKSI1030, "SystemError");
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
                    MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1030, "SystemError");
                }

                worker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// システム分類一覧の再検索
        /// </summary>
        private void RefreshSystemCategorys()
        {
            BackgroundWorker worker = new BackgroundWorker();

            try
            {
                // 処理中か
                ProcessState.IsBusy = true;
                // 処理中のメッセージ
                ProcessState.BusyMessage = "一覧を取得中...";

                // 棚卸システム分類情報
                IList<TanaorosiSystemCategory> tanaorosiSystemCategoryList = null;

                worker.DoWork += (s, ev) =>
                {
                    // 棚卸システム分類マスタからの取得
                    tanaorosiSystemCategoryList = this._accessor.SelectTanaorosiSystemCategory();
                };

                worker.RunWorkerCompleted += (s, ex) =>
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        SystemCategorys.Clear();

                        foreach (TanaorosiSystemCategory tanaorosiSystemCategory in tanaorosiSystemCategoryList)
                        {
                            SystemCategorys.Add(new SystemCategoryInfo(this)
                            {
                                //システム分類
                                SystemCategory = tanaorosiSystemCategory.SystemCategory,
                                //システム分類名
                                SystemCategoryName = tanaorosiSystemCategory.SystemCategoryName,
                                // 変更されているか
                                IsDirty = false
                            });
                        }

                        if (SystemCategorys.Count > 0)
                        {
                            this.Loaded += DelegateSetFocusToDataGridView;
                        }
                        else
                        {
                            this.Loaded += DelegateSetFocusToTextBox;
                        }

                        SystemCategoryModification.IsDirty = false;
                        ProcessState.IsBusy = false;
                    }));
                };
            }

            catch (Exception)
            {
                SystemCategoryModification.IsDirty = false;
                ProcessState.IsBusy = false;
                MessageManager.ShowExclamation(MessageManager.SystemID.CKSI1030, "SystemError");
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
            if (MessageManager.ShowQuestion(MessageManager.SystemID.CKSI1030, "ConfirmExit") == MessageManager.ResultType.No)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// システム分類の入力データのチェック
        /// </summary>
        /// <param name="user">システム分類オブジェクト</param>
        /// <param name="type">DB操作</param>
        /// <returns>チェック結果</returns>
        private string CheckSystemCategory(SystemCategoryInfo systemCategory, ProcessType type)
        {
            string result = string.Empty;

            switch (type)
            {
                //挿入
                case ProcessType.Insert:
                    //システム分類のチェック
                    result = InputChecker.CheckSystemCategory(systemCategory.SystemCategory);
                    if (result.Equals(String.Empty))
                    {
                        result = InputChecker.CheckSystemCategoryName(systemCategory.SystemCategoryName);
                    }
                    break;

                //更新
                case ProcessType.Update:
                    //システム分類名名のチェック
                    result = InputChecker.CheckSystemCategoryName(systemCategory.SystemCategoryName);
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
                textBox.AddValueChanged(this._dataGrid, (s, ev) => SetFocusToTextBox(this._textBoxSystemCategory));
                SetFocusToTextBox(this._textBoxSystemCategory);
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
        private static void SetFocusToTextBox(TextBox control)
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
        public void NotifyTanaorosiSystemCategoryModified()
        {
            SystemCategoryModification.IsDirty = SystemCategorys.Any(systemCategory => systemCategory.IsDirty);
        }

        #endregion
    }
}




