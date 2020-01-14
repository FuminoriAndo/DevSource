using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Common;
using DBManager;
using DBManager.Condition;
using DBManager.Constants;
using Microsoft.Windows.Controls;
using CKSI1080.ViewModel;
using CKSI1080.Common;
using DBManager.Model;
using Shared;
using ViewModel;
using CKSI1080.Types;

namespace CKSI1080
{
    /// <summary>
    /// ucWorkLog.xaml の相互作用ロジック
    /// </summary>
    public partial class ucWorkLog : UserControl
    {
        #region 列挙体

        /// <summary>
        /// システム分類
        /// </summary>
        internal enum SystemCategory
        {
            /// <summary>
            /// 資材
            /// </summary>
            Sizai,

            /// <summary>
            /// 部品倉庫
            /// </summary>
            BS
        }

        #endregion

        #region フィールド
        /// <summary>
        /// DB操作の制御オブジェクト
        /// </summary>
        private DBAccessor _accessor = null;

        /// <summary>
        /// XML操作オブジェクト
        /// </summary>
        private XMLOperator _xmlOperator = null;

        /// <summary>
        /// 作業ログ抽出件数
        /// </summary>
        //private long _logRecordCount = 0;

        /// <summary>
        /// 通常ログ
        /// </summary>
        private const string NOMAL_LOG = "1";

        /// <summary>
        /// エラーログ
        /// </summary>
        private const string ERROR_LOG = "2";

        /// <summary>
        /// 作業分類のマップ
        /// </summary>
        private IDictionary<string, string> workCategoysMap = null;

        /// <summary>
        /// 操作種類のマップ
        /// </summary>
        private IDictionary<string, string> operationTypesMap = null;

        /// <summary>
        /// 操作内容のマップ
        /// </summary>
        private IDictionary<string, string> operationContentsMap = null;

        #endregion

        #region 列挙体
        /// <summary>
        /// 画面モード
        /// </summary>
        public enum DisplayModeType
        {
            /// <summary>
            /// 作業ログ
            /// </summary>
            WorkLog,

            /// <summary>
            /// 作業ログ詳細
            /// </summary>
            WorkLogDetails,

            /// <summary>
            /// 作業エラーログ
            /// </summary>
            WorkErrorLog,

            /// <summary>
            /// 作業エラー詳細ログ
            /// </summary>
            WorkErrorLogDetails,

            /// <summary>
            /// 部品倉庫棚卸作業ログ
            /// </summary>
            BSInventoryWorkLog,

            /// <summary>
            /// 部品倉庫棚卸作業エラーログ
            /// </summary>
            BSInventoryWorkErrorLog
        }

        /// <summary>
        /// 検索テーブル
        /// </summary>
        private enum SearchTable
        {
            /// <summary>
            /// 棚卸ログ
            /// </summary>
            TanaorosiLogTRN,

            /// <summary>
            /// 棚卸ログ詳細
            /// </summary>
            TanaorosiDetailLogTRN
        }
        #endregion

        #region 依存関係プロパティ
        /// <summary>
        /// ログ表示画面
        /// </summary>
        public static readonly DependencyProperty LogScreenDisplayModeProperty =
            DependencyProperty.Register("LogScreenDisplay",
                                        typeof(DisplayModeType),
                                        typeof(ucWorkLog),
                                        null);
        public DisplayModeType LogScreenDisplay
        {
            get { return (DisplayModeType)GetValue(LogScreenDisplayModeProperty); }
            set { SetValue(LogScreenDisplayModeProperty, value); }
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 検索条件
        /// </summary>
        public SearchCondition SearchCondition
        {
            get;
            private set;
        }

        ///// <summary>
        ///// 保存検索条件(実際に検索を行った際の条件)
        ///// </summary>
        //public SearchCondition SearchedCondition
        //{
        //    get;
        //    private set;
        //}

        /// <summary>
        /// 処理状態
        /// </summary>
        public ProcessState ProcessState
        {
            get;
            private set;
        }

        /// <summary>
        /// 画面モード
        /// </summary>
        public DisplayMode DisplayMode
        {
            get;
            private set;
        }

        /// <summary>
        /// ログ一覧
        /// </summary>
        public ObservableCollection<WorkLog> Logs
        {
            get;
            private set;
        }

        /// <summary>
        /// 社員番号
        /// </summary>
        public ObservableCollection<ComboBoxViewModel> TanaorosiOperationUsers
        {
            get;
            private set;
        }

        /// <summary>
        /// 入力作業
        /// </summary>
        public ObservableCollection<ComboBoxViewModel> WorkCategoys
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
        /// 操作内容
        /// </summary>
        public ObservableCollection<ComboBoxViewModel> OperationContents
        {
            get;
            private set;
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ucWorkLog()
        {
            try
            {
                ProcessState = new ProcessState();
                DisplayMode = new DisplayMode();
                Logs = new ObservableCollection<WorkLog>();
                SearchCondition = new SearchCondition();
                TanaorosiOperationUsers = new ObservableCollection<ComboBoxViewModel>();
                WorkCategoys = new ObservableCollection<ComboBoxViewModel>();
                OperationTypes = new ObservableCollection<ComboBoxViewModel>();
                OperationContents = new ObservableCollection<ComboBoxViewModel>();
                this._accessor = new DBAccessor(DBConstants.DBType.ORACLE);
                this._xmlOperator = XMLOperator.GetInstance();
                SharedModel.Instance.OperationInfo = new Shared.Model.OperationInfo();
                workCategoysMap = new Dictionary<string, string>();
                operationTypesMap = new Dictionary<string, string>();
                operationContentsMap = new Dictionary<string, string>();

                InitializeComponent();

                //this._logRecordCountLabel.Content = this._logRecordCount;

                // 社員のコンボボックスの生成
                CreateTanaorosiOperationUsersCombo();
                // 作業区分のコンボボックスの生成
                CreateWorkCategorysCombo();

                // ユーザコントロールのロード
                Loaded += new RoutedEventHandler(ucWorkLog_Loaded);
            }

            catch (Exception)
            {
                //システムエラー
                MessageManager.ShowError(MessageManager.SystemID.CKSI1080, "SystemError");
            }

        }
        #endregion

        #region メソッド

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucWorkLog_Loaded(object sender, RoutedEventArgs e)
        {
            var inventoryCategory = InventoryCategory.SIZAI;
            
            // 一覧の表示設定
            switch (LogScreenDisplay)
            {
                //作業ログ
                case DisplayModeType.WorkLog:
                    this._shizaiKBN.Visibility = Visibility.Collapsed;
                    this._hinmokuCode.Visibility = Visibility.Collapsed;
                    this._gyosyaCode.Visibility = Visibility.Collapsed;
                    this._hinmokuName.Visibility = Visibility.Collapsed;
                    this._gyosyaName.Visibility = Visibility.Collapsed;
                    this._bikouDetails.Visibility = Visibility.Collapsed;
                    this._updateContent1.Visibility = Visibility.Collapsed;
                    this._updateContent2.Visibility = Visibility.Collapsed;
                    this._updateContent3.Visibility = Visibility.Collapsed;
                    this._errorContent.Visibility = Visibility.Collapsed;
                    //this._errorCode.Visibility = Visibility.Collapsed;
                    this.SearchCondition.SystemCategory = SystemCategory.Sizai.GetValue();
                    this.SearchCondition.LogType = NOMAL_LOG;
                    DisplayMode.IsPartsWarehouse = false;
                    break;
                //作業ログ（詳細）
                case DisplayModeType.WorkLogDetails:
                    this._bikou.Visibility = Visibility.Collapsed;
                    this._errorContent.Visibility = Visibility.Collapsed;
                    //this._errorCode.Visibility = Visibility.Collapsed;
                    this.SearchCondition.SystemCategory = SystemCategory.Sizai.GetValue();
                    this.SearchCondition.LogType = NOMAL_LOG;
                    DisplayMode.IsPartsWarehouse = false;
                    break;
                //作業エラーログ
                case DisplayModeType.WorkErrorLog:
                    this._shizaiKBN.Visibility = Visibility.Collapsed;
                    this._hinmokuCode.Visibility = Visibility.Collapsed;
                    this._gyosyaCode.Visibility = Visibility.Collapsed;
                    this._hinmokuName.Visibility = Visibility.Collapsed;
                    this._gyosyaName.Visibility = Visibility.Collapsed;
                    this._bikouDetails.Visibility = Visibility.Collapsed;
                    this._updateContent1.Visibility = Visibility.Collapsed;
                    this._updateContent2.Visibility = Visibility.Collapsed;
                    this._updateContent3.Visibility = Visibility.Collapsed;
                    this.SearchCondition.SystemCategory = SystemCategory.Sizai.GetValue();
                    this.SearchCondition.LogType = ERROR_LOG;
                    DisplayMode.IsPartsWarehouse = false;
                    break;
                //作業エラーログ（詳細）
                case DisplayModeType.WorkErrorLogDetails:
                    this._bikou.Visibility = Visibility.Collapsed;
                    //this._errorCode.Visibility = Visibility.Collapsed;
                    this.SearchCondition.SystemCategory = SystemCategory.Sizai.GetValue();
                    this.SearchCondition.LogType = ERROR_LOG;
                    DisplayMode.IsPartsWarehouse = false;
                    break;
                //部品倉庫棚卸作業ログ
                case DisplayModeType.BSInventoryWorkLog:
                    //this._workKBN.Visibility = Visibility.Collapsed;
                    this._shizaiKBN.Visibility = Visibility.Collapsed;
                    this._hinmokuCode.Visibility = Visibility.Collapsed;
                    this._gyosyaCode.Visibility = Visibility.Collapsed;
                    this._hinmokuName.Visibility = Visibility.Collapsed;
                    this._gyosyaName.Visibility = Visibility.Collapsed;
                    this._bikouDetails.Visibility = Visibility.Collapsed;
                    this._updateContent1.Visibility = Visibility.Collapsed;
                    this._updateContent2.Visibility = Visibility.Collapsed;
                    this._updateContent3.Visibility = Visibility.Collapsed;
                    this._errorContent.Visibility = Visibility.Collapsed;
                    //this._errorCode.Visibility = Visibility.Collapsed;
                    this.SearchCondition.SystemCategory = SystemCategory.BS.GetValue();
                    this.SearchCondition.LogType = NOMAL_LOG;
                    DisplayMode.IsPartsWarehouse = true;
                    inventoryCategory = InventoryCategory.BS;
                    break;
                //部品倉庫棚卸作業エラーログ
                case DisplayModeType.BSInventoryWorkErrorLog:
                    //this._workKBN.Visibility = Visibility.Collapsed;
                    this._shizaiKBN.Visibility = Visibility.Collapsed;
                    this._hinmokuCode.Visibility = Visibility.Collapsed;
                    this._gyosyaCode.Visibility = Visibility.Collapsed;
                    this._hinmokuName.Visibility = Visibility.Collapsed;
                    this._gyosyaName.Visibility = Visibility.Collapsed;
                    this._bikouDetails.Visibility = Visibility.Collapsed;
                    this._updateContent1.Visibility = Visibility.Collapsed;
                    this._updateContent2.Visibility = Visibility.Collapsed;
                    this._updateContent3.Visibility = Visibility.Collapsed;
                    this.SearchCondition.SystemCategory = SystemCategory.BS.GetValue();
                    this.SearchCondition.LogType = ERROR_LOG;
                    DisplayMode.IsPartsWarehouse = true;
                    inventoryCategory = InventoryCategory.BS;
                    break;
                default:
                    break;
            }
            // 操作種別のコンボボックスの生成
            CreateOperationTypeCombo(inventoryCategory);
            // 操作内容のコンボボックスの生成
            CreateOperationContentCombo(inventoryCategory);
            Loaded -= new RoutedEventHandler(ucWorkLog_Loaded);
        }

        /// <summary>
        /// 社員のコンボボックスの生成
        /// </summary>
        private void CreateTanaorosiOperationUsersCombo()
        {
            try
            {
                // 棚卸操作ユーザの取得
                IDictionary<string, string> tanaorosiOperationUsers = this._accessor.SelectTanaorosiOpeationUsers();
                foreach (var user in tanaorosiOperationUsers)
                {
                    ComboBoxViewModel item = new ComboBoxViewModel();
                    item.Key = user.Key;
                    item.Value = user.Value;
                    TanaorosiOperationUsers.Add(item);
                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 作業区分のコンボボックスの生成
        /// </summary>
        private void CreateWorkCategorysCombo()
        {
            try
            {
                // 作業区分の取得
                var workKCategory = this._xmlOperator.GetWorkCategory();
                foreach (var dict in workKCategory)
                {
                    ComboBoxViewModel item = new ComboBoxViewModel();
                    item.Key = dict.Key;
                    item.Value = dict.Value;
                    WorkCategoys.Add(item);
                }
                workCategoysMap.Clear();
                foreach (var item in WorkCategoys)
                {
                    workCategoysMap.Add(item.Key, item.Value);
                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 操作種別のコンボボックスの生成
        /// </summary>
        private void CreateOperationTypeCombo(InventoryCategory category)
        {
            try
            {
                // 操作種別の取得
                var operationType = this._xmlOperator.GetOperationType(category);

                foreach (var dict in operationType)
                {
                    ComboBoxViewModel item = new ComboBoxViewModel();
                    item.Key = dict.Key;
                    item.Value = dict.Value;
                    OperationTypes.Add(item);
                }
                operationTypesMap.Clear();
                foreach (var item in OperationTypes)
                {
                    operationTypesMap.Add(item.Key, item.Value);
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 操作内容のコンボボックスの生成
        /// </summary>
        private void CreateOperationContentCombo(InventoryCategory category)
        {
            try
            {
                // 操作内容の取得
                var operationContent = this._xmlOperator.GetOperationContent(category);

                foreach (var dict in operationContent)
                {
                    ComboBoxViewModel item = new ComboBoxViewModel();
                    item.Key = dict.Key;
                    item.Value = dict.Value;
                    OperationContents.Add(item);
                }
                operationContentsMap.Clear();
                foreach (var item in OperationContents)
                {
                    operationContentsMap.Add(item.Key, item.Value);
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// クリアボタンイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnSetSearchConditionClearButton(object sender, RoutedEventArgs e)
        {
            SetSearchConditionClearMode();
        }

        /// <summary>
        /// 検索イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnSearchButtonClicked(object sender, RoutedEventArgs e)
        {
            SearchLog(this.LogScreenDisplay);
        }

        /// <summary>
        /// 画面モードを条件設定モードに設定する(条件クリア)
        /// </summary>
        private void SetSearchConditionClearMode()
        {
            //clearSearchCondition();
            //refreshSaveSearchedCondition();
            Logs.Clear();
            //this._logRecordCount = 0;
            //this._logRecordCountLabel.Content = this._logRecordCount;
        }

        /// <summary>
        /// 作業ログの検索
        /// </summary>
        /// <param name="type">画面モード</param>
        private void SearchLog(DisplayModeType type)
        {
            switch (type)
            {
                //作業ログ画面
                case DisplayModeType.WorkLog:
                    SearchTanaorosiLog(SearchTable.TanaorosiLogTRN, "作業ログ");
                    break;
                //作業ログ画面（詳細）
                case DisplayModeType.WorkLogDetails:
                    SearchTanaorosiLog(SearchTable.TanaorosiDetailLogTRN, "作業エラーログ");
                    break;
                //作業エラーログ画面
                case DisplayModeType.WorkErrorLog:
                    SearchTanaorosiLog(SearchTable.TanaorosiLogTRN, "作業詳細エラーログ");
                    break;
                //作業エラーログ画面（詳細）
                case DisplayModeType.WorkErrorLogDetails:
                    SearchTanaorosiLog(SearchTable.TanaorosiDetailLogTRN, "作業詳細エラーログ");
                    break;
                    //部品倉庫棚卸作業ログ
                case DisplayModeType.BSInventoryWorkLog:
                    SearchTanaorosiLog(SearchTable.TanaorosiLogTRN, "部品倉庫棚卸作業ログ");
                    break;
                    //部品倉庫棚卸作業エラーログ
                case DisplayModeType.BSInventoryWorkErrorLog:
                    SearchTanaorosiLog(SearchTable.TanaorosiLogTRN, "部品倉庫棚卸作業エラーログ");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// ログ検索
        /// </summary>
        private void SearchTanaorosiLog(SearchTable searchTable, string screenName)
        {
            //検索条件のチェック
            string result = CheckSearchCondition(SearchCondition);

            if (String.IsNullOrEmpty(result))
            {
                BackgroundWorker worker = new BackgroundWorker();

                try
                {
                    //処理中か
                    ProcessState.IsBusy = true;
                    // 処理中メッセージ
                    ProcessState.BusyMessage = screenName + "を検索中です...";

                    //棚卸ログデータ
                    IList<TanaorosiLogTRN> tanaorosiLogs = null;
                    IList<TanaorosiDetailLogTRN> tanaorosiDetailLogs = null;

                    worker.DoWork += (s, ev) =>
                    {
                        //棚卸ログ検索条件の設定
                        LogSearchCondition searchCondition = ORMapper.SetLogSearchCondition(SearchCondition);

                        //棚卸ログの検索
                        if (searchTable == SearchTable.TanaorosiLogTRN)
                        {
                            tanaorosiLogs = this._accessor.SelectTanaorosiLogTRN(searchCondition);
                        }
                        else
                        {
                            tanaorosiDetailLogs = this._accessor.SelectTanaorosiDetailLogTRN(searchCondition);
                        }
                    };

                    worker.RunWorkerCompleted += (s, ex) =>
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Logs.Clear();

                            // 検索結果を一覧へ反映
                            if (searchTable == SearchTable.TanaorosiLogTRN)
                            {
                                ReflectTanaorosiLogTRNSearchResult(tanaorosiLogs);
                            }
                            else
                            {
                                ReflectTanaorosiDetailLogLogTRNSearchResult(tanaorosiDetailLogs);
                            }

                            //refreshSaveSearchedCondition();
                            ProcessState.IsBusy = false;

                            if (Logs.Count == 0)
                            {
                                //該当データなし
                                MessageManager.ShowError(MessageManager.SystemID.CKSI1080, "DataNotFind");
                            }
                            else
                            {
                                //対象の保存作業ログデータが1件以上
                                //this._logRecordCountLabel.Content = Logs.Count.ToString();
                                ScrollToTopDataGrid(this._dataGridSave);
                            }
                        }));
                    };
                }

                catch (Exception)
                {
                    ProcessState.IsBusy = false;
                    //システムエラー
                    MessageManager.ShowError(MessageManager.SystemID.CKSI1080, "SystemError");
                }

                worker.RunWorkerAsync();
            }
            else
            {
                // 検索条件エラー
                MessageManager.ShowError(MessageManager.SystemID.CKSI1080, result);
            }
        }

        /// <summary>
        /// 棚卸ログ検索結果反映
        /// </summary>
        /// <param name="logs">ログ一覧</param>
        private void ReflectTanaorosiLogTRNSearchResult(IList<TanaorosiLogTRN> logs)
        {
            foreach (TanaorosiLogTRN log in logs)
            {
                Logs.Add(new WorkLog()
                {
                    //システム分類
                    SystemCategory = log.SystemCategory,
                    //社員番号
                    EmployeeNo = log.EmployeeNo,
                    //社員名
                    EmployeeName = log.EmployeeName,
                    //作業日時
                    OperetionDate = log.OperetionDate.Substring(0, 4) + "/" + log.OperetionDate.Substring(4, 2) + "/" + log.OperetionDate.Substring(6, 2) + " "
                                  + log.OperetionDate.Substring(8, 2) + ":" + log.OperetionDate.Substring(10, 2) + ":" + log.OperetionDate.Substring(12, 2),
                    //作業区分
                    WorkKBN = workCategoysMap[log.WorkKBN],
                    //操作種別
                    OperateType = operationTypesMap[log.OperateType],
                    //操作内容
                    OpereteContent = operationContentsMap[log.OpereteContent],
                    //備考
                    Bikou = log.Bikou,
                    //エラー内容
                    ErrorContent = log.ErrorContent,
                    //エラーコード
                    ErrorCode = log.ErrorCode,
                });
            }
        }

        /// <summary>
        /// 棚卸詳細ログ検索結果反映
        /// </summary>
        /// <param name="logs">ログ一覧</param>
        private void ReflectTanaorosiDetailLogLogTRNSearchResult(IList<TanaorosiDetailLogTRN> logs)
        {
            foreach (TanaorosiDetailLogTRN log in logs)
            {
                Logs.Add(new WorkLog()
                {
                    //社員番号
                    EmployeeNo = log.EmployeeNo,
                    //社員名
                    EmployeeName = log.EmployeeName,
                    //作業日時
                    OperetionDate = log.OperetionDate.Substring(0, 4) + "/" + log.OperetionDate.Substring(4, 2) + "/" + log.OperetionDate.Substring(6, 2) + " "
                                  + log.OperetionDate.Substring(8, 2) + ":" + log.OperetionDate.Substring(10, 2) + ":" + log.OperetionDate.Substring(12, 2),
                    //作業区分
                    WorkKBN = workCategoysMap[log.WorkKBN],
                    //操作種別
                    OperateType = operationTypesMap[log.OperateType],
                    //操作内容
                    OpereteContent = operationContentsMap[log.OpereteContent],
                    //備考
                    Bikou = log.Bikou,
                    //エラー内容
                    ErrorContent = log.ErrorContent,
                    //エラーコード
                    ErrorCode = log.ErrorCode,
                    //資材区分
                    ShizaiKBN = log.SizaiKBN,
                    //品目コード
                    HinmokuCode = log.HinmokuCD,
                    //業者コード
                    GyosyaCode = log.GyosyaCD,
                    //品目名
                    HinmokuName = log.HinmokuNM,
                    //業者名
                    GyosyaName = log.GyosyaNM,
                    //変更内容1
                    UpdateContent1 = log.UpdateContent1,
                    //変更内容2
                    UpdateContent2 = log.UpdateContent2,
                    //変更内容3
                    UpdateContent3 = log.UpdateContent3
                });
            }
        }

        /// <summary>
        /// 部品倉庫棚卸作業ログ検索結果反映
        /// </summary>
        /// <param name="logs">ログ一覧</param>
        private void ReflectInventoryWorkLogSearchResult(IList<TanaorosiDetailLogTRN> logs)
        {
            foreach (TanaorosiDetailLogTRN log in logs)
            {
                Logs.Add(new WorkLog()
                {
                    //社員番号
                    EmployeeNo = log.EmployeeNo,
                    //社員名
                    EmployeeName = log.EmployeeName,
                    //作業日時
                    OperetionDate = log.OperetionDate.Substring(0, 4) + "/" + log.OperetionDate.Substring(4, 2) + "/" + log.OperetionDate.Substring(6, 2) + " "
                                  + log.OperetionDate.Substring(8, 2) + ":" + log.OperetionDate.Substring(10, 2) + ":" + log.OperetionDate.Substring(12, 2),
                    //作業区分
                    WorkKBN = log.WorkKBN,
                    //操作種別
                    OperateType = log.OperateType,
                    //操作内容
                    OpereteContent = log.OpereteContent,
                    //備考
                    Bikou = log.Bikou,
                });
            }
        }

        /// <summary>
        /// 部品倉庫棚卸作業エラーログ検索結果反映
        /// </summary>
        /// <param name="logs">ログ一覧</param>
        private void ReflectInventoryWorkErrorLogSearchResult(IList<TanaorosiDetailLogTRN> logs)
        {
            foreach (TanaorosiDetailLogTRN log in logs)
            {
                Logs.Add(new WorkLog()
                {
                    //社員番号
                    EmployeeNo = log.EmployeeNo,
                    //社員名
                    EmployeeName = log.EmployeeName,
                    //作業日時
                    OperetionDate = log.OperetionDate.Substring(0, 4) + "/" + log.OperetionDate.Substring(4, 2) + "/" + log.OperetionDate.Substring(6, 2) + " "
                                  + log.OperetionDate.Substring(8, 2) + ":" + log.OperetionDate.Substring(10, 2) + ":" + log.OperetionDate.Substring(12, 2),
                    //操作種別
                    OperateType = log.OperateType,
                    //操作内容
                    OpereteContent = log.OpereteContent,
                    //備考
                    Bikou = log.Bikou,
                    //エラー内容
                    ErrorContent = log.ErrorContent,
                    //エラーコード
                    ErrorCode = log.ErrorCode,
                    //資材区分
                    ShizaiKBN = log.SizaiKBN,
                    //品目コード
                    HinmokuCode = log.HinmokuCD,
                    //業者コード
                    GyosyaCode = log.GyosyaCD,
                    //品目名
                    HinmokuName = log.HinmokuNM,
                    //業者名
                    GyosyaName = log.GyosyaNM,
                });
            }
        }

        ///// <summary>
        ///// 保存検索条件のクリア
        ///// </summary>
        //private void clearSearchCondition()
        //{
        //    SearchCondition.UseEmployeeNo = false;
        //    SearchCondition.EmployeeNo = null;
        //    SearchCondition.UseOperationDate = false;
        //    SearchCondition.MinOperationDate = null;
        //    SearchCondition.MaxOperationDate = null;
        //    SearchCondition.UseWorkKbn = false;
        //    SearchCondition.WorkKbn = null;
        //    SearchCondition.UseOperateType = false;
        //    SearchCondition.OperateType = null;
        //    SearchCondition.UseOperateContent = false;
        //    SearchCondition.OperateContent = null;
        //}

        ///// <summary>
        ///// 保存検索条件(実際に検索を行った際の条件)
        ///// </summary>
        //private void refreshSaveSearchedCondition()
        //{
        //    SearchedCondition.UseEmployeeNo = SearchCondition.UseEmployeeNo;
        //    SearchedCondition.EmployeeNo = SearchCondition.EmployeeNo;
        //    SearchedCondition.UseOperationDate = SearchCondition.UseOperationDate;
        //    SearchedCondition.MinOperationDate = SearchCondition.MinOperationDate;
        //    SearchedCondition.MaxOperationDate = SearchCondition.MaxOperationDate;
        //    SearchedCondition.UseWorkKbn = SearchCondition.UseWorkKbn;
        //    SearchedCondition.WorkKbn = SearchCondition.WorkKbn;
        //    SearchedCondition.UseOperateType = SearchCondition.UseOperateType;
        //    SearchedCondition.OperateType = SearchCondition.OperateType;
        //    SearchedCondition.UseOperateContent = SearchCondition.UseOperateContent;
        //    SearchedCondition.OperateContent = SearchCondition.OperateContent;

        //}


        /// <summary>
        /// 検索条件のチェック
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>チェック結果</returns>
        private string CheckSearchCondition(ViewModelBase condition)
        {
            string result = string.Empty;

            if (condition is SearchCondition)
            {
                SearchCondition SearchCondition = condition as SearchCondition;
                result = CheckLogSearchCondition(SearchCondition);
            }

            return result;
        }

        /// <summary>
        /// 作業ログの検索条件のチェック
        /// </summary>
        /// <param name="SearchCondition">保存作業ログの検索条件</param>
        /// <returns>チェック結果</returns>
        private string CheckLogSearchCondition(SearchCondition SearchCondition)
        {
            string result = string.Empty;

            //社員番号のチェック
            if (SearchCondition.UseEmployeeNo)
            {
                result = InputChecker.CheckEmployeeNo(SearchCondition.EmployeeNo);
                if (!String.IsNullOrEmpty(result))
                {
                    return result;
                }
            }

            //作業日時のチェック
            if (SearchCondition.UseOperationDate)
            {
                result = InputChecker.CheckOperationDate(SearchCondition.MinOperationDate, SearchCondition.MaxOperationDate);
                if (!String.IsNullOrEmpty(result))
                {
                    return result;
                }
            }

            //入力作業のチェック
            if (SearchCondition.UseWorkKbn)
            {
                result = InputChecker.CheckWorkKbn(SearchCondition.WorkKbn);
                if (!String.IsNullOrEmpty(result))
                {
                    return result;
                }
            }


            //操作種別のチェック
            if (SearchCondition.UseOperateType)
            {
                result = InputChecker.CheckOperationType(SearchCondition.OperateType);
                if (!String.IsNullOrEmpty(result))
                {
                    return result;
                }
            }

            //操作内容のチェック
            if (SearchCondition.UseOperateContent)
            {
                result = InputChecker.CheckOperationContent(SearchCondition.OperateContent);
                if (!String.IsNullOrEmpty(result))
                {
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// DataGridのスクロールを先頭に移動する
        /// </summary>
        /// <param name="dataGrid">DataGrid</param>
        private void ScrollToTopDataGrid(DataGrid dataGrid)
        {
            if (dataGrid.Items.Count > 0)
            {
                var border = VisualTreeHelper.GetChild(dataGrid, 0) as Decorator;
                if (border != null)
                {
                    var scroll = border.Child as ScrollViewer;
                    if (scroll != null)
                    {
                        scroll.ScrollToTop();
                    }
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// 棚卸調査表(提出用)のExcelシートの種類のヘルパー
    /// </summary>
    internal static class SystemCategoryHelper
    {
        #region メソッド

        /// <summary>
        /// システム分類のValueを取得する
        /// </summary>
        internal static string GetValue(this CKSI1080.ucWorkLog.SystemCategory category)
        {
            string[] categorys = { "1", "2" };
            return categorys[(int)category];
        }

        #endregion
    }
}
