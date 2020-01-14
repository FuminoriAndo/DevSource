using CKSI1010.Common;
using CKSI1010.Core;
using CKSI1010.Operation.Common.ViewModel;
using CKSI1010.Shared;
using CKSI1010.Shared.Model;
using CKSI1010.Shared.ViewModel;
using Common;
using GalaSoft.MvvmLight.CommandWpf;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static CKSI1010.Common.Constants;
using static Common.MessageManager;
//*************************************************************************************
//
//   印刷画面のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.Print.ViewModel
{
    /// <summary>
    /// 印刷画面のビューモデル
    /// </summary>
    public class PrintViewModel : OperationViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 印刷対象
        /// </summary>
        private bool[] checkList = new bool[] { true, true, true, true, true, true, true, true };

        /// <summary>
        /// 編集有無
        /// </summary>
        private bool[] editList = new bool[] { false, false, false, false, false, false, false, false };

        /// <summary>
        /// 初回フラグ
        /// </summary>
        private bool isFirstTime = false;

        /// <summary>
        /// 確定状態か
        /// </summary>
        private bool isFixed = false;

        /// <summary>
        /// 編集状態か
        /// </summary>
        private bool isModifyEditMode = false;

        /// <summary>
        /// 編集フォームが表示されているか
        /// </summary>
        private bool isShowEditForm = false;

        /// <summary>
        /// データ一覧
        /// </summary>
        private IDictionary<InventoryPrintTieSizaiCategory, ObservableCollection<PrintRecordViewModel>> dataList 
            = new Dictionary<InventoryPrintTieSizaiCategory, ObservableCollection<PrintRecordViewModel>>();

        /// <summary>
        /// 削除データ一覧
        /// </summary>
        private IDictionary<InventoryPrintTieSizaiCategory, List<InventoryPrint>> deletedDataList 
            = new Dictionary<InventoryPrintTieSizaiCategory, List<InventoryPrint>>();

        /// <summary>
        /// 表示対象
        /// </summary>
        private InventoryPrintTieSizaiCategory selectSIzaiCategory;

        /// <summary>
        /// 編集モード表示
        /// </summary>
        private Visibility editModeVisibility = new Visibility();

        /// <summary>
        /// ボタン表示
        /// </summary>
        private Visibility editOperationVisibility = new Visibility();

        /// <summary>
        /// データグリッド高さ
        /// </summary>
        private int dataGridHeight = 0;

        /// <summary>
        /// 編集モード入力欄(品目CD)
        /// </summary>
        private string hinmokuCD = string.Empty;

        /// <summary>
        /// 編集モード入力欄(業者CD)
        /// </summary>
        private string gyosyaCD = string.Empty;

        /// <summary>
        /// 編集モード入力欄(品目名)
        /// </summary>
        private string hinmokuName = string.Empty;

        /// <summary>
        /// 編集モード入力欄(業者CD)
        /// </summary>
        private string gyosyaName = string.Empty;

        /// <summary>
        /// 編集モード入力欄(備考)
        /// </summary>
        private string bikou = string.Empty;

        /// <summary>
        /// 当月量クリア
        /// </summary>
        private bool togetuClear = false;

        /// <summary>
        /// 追加・編集時の品目CD/業者CDの読み取り専用
        /// </summary>
        private bool isReadonlyCD = false;
        /// <summary>
        /// 追加・編集時の品目名/業者名の読み取り専用
        /// </summary>
        private bool isReadonlyName = false;

        private double[] opacities = new double[] { 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5 };

        /// <summary>
        /// Excelアクセッサー
        /// </summary>
        private ExcelAccessor excelAccessor = null;

        /// <summary>
        /// 印刷対象データ
        /// </summary>
        private ObservableCollection<PrintRecordViewModel> printTargetList = new ObservableCollection<PrintRecordViewModel>();

        /// <summary>
        /// 編集モード
        /// </summary>
        private EditMode editMode = EditMode.None;

        #endregion

        #region コンストラクタ

        /// <summary>
        ///  コンストラクタ
        /// </summary>
        /// <param name="dataService">データサービス</param>
        public PrintViewModel(IDataService dataService) : base(dataService)
        {
            editModeVisibility = Visibility.Collapsed;
            editOperationVisibility = Visibility.Visible;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// SK
        /// </summary>
        public double OpecitySK
        {
            get { return opacities[(int)InventoryPrintTieSizaiCategory.SK]; }
            set { Set(ref opacities[(int)InventoryPrintTieSizaiCategory.SK], value); }
        }

        /// <summary>
        /// EF
        /// </summary>
        public double OpecityEF
        {
            get { return opacities[(int)InventoryPrintTieSizaiCategory.EF]; }
            set { Set(ref opacities[(int)InventoryPrintTieSizaiCategory.EF], value); }
        }

        /// <summary>
        /// LF
        /// </summary>
        public double OpecityLF
        {
            get { return opacities[(int)InventoryPrintTieSizaiCategory.LF]; }
            set { Set(ref opacities[(int)InventoryPrintTieSizaiCategory.LF], value); }
        }

        /// <summary>
        /// 築炉
        /// </summary>
        public double OpecityBuilding
        {
            get { return opacities[(int)InventoryPrintTieSizaiCategory.Chikuro]; }
            set { Set(ref opacities[(int)InventoryPrintTieSizaiCategory.Chikuro], value); }
        }

        /// <summary>
        /// LD
        /// </summary>
        public double OpecityLD
        {
            get { return opacities[(int)InventoryPrintTieSizaiCategory.LD]; }
            set { Set(ref opacities[(int)InventoryPrintTieSizaiCategory.LD], value); }
        }

        /// <summary>
        /// TD
        /// </summary>
        public double OpecityTD
        {
            get { return opacities[(int)InventoryPrintTieSizaiCategory.TD]; }
            set { Set(ref opacities[(int)InventoryPrintTieSizaiCategory.TD], value); }
        }

        /// <summary>
        /// CC
        /// </summary>
        public double OpecityCC
        {
            get { return opacities[(int)InventoryPrintTieSizaiCategory.CC]; }
            set { Set(ref opacities[(int)InventoryPrintTieSizaiCategory.CC], value); }
        }

        /// <summary>
        /// その他
        /// </summary>
        public double OpecityOthers
        {
            get { return opacities[(int)InventoryPrintTieSizaiCategory.ETC]; }
            set { Set(ref opacities[(int)InventoryPrintTieSizaiCategory.ETC], value); }
        }

        /// <summary>
        /// 当月量クリア
        /// </summary>
        public bool TogetuClear
        {
            get { return togetuClear; }
            set { Set(ref togetuClear, value); }
        }

        /// <summary>
        /// 追加・編集時の品目CD/業者CDの読み取り専用
        /// </summary>
        public bool IsReadonlyCD
        {
            get { return isReadonlyCD; }
            set { Set(ref isReadonlyCD, value); }
        }
        /// <summary>
        /// 追加・編集時の品目名/業者名の読み取り専用
        /// </summary>        
        public bool IsReadonlyName
        {
            get { return isReadonlyName; }
            set { Set(ref isReadonlyName, value); }
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
        /// 編集状態か
        /// </summary>
        public bool IsModifyEditMode
        {
            get { return isModifyEditMode; }
            set { Set(ref isModifyEditMode, value); }
        }

        /// <summary>
        /// 編集フォームが表示されているか
        /// </summary>
        public bool IsShowEditForm
        {
            get { return isShowEditForm; }
            set { Set(ref isShowEditForm, value); }
        }

        /// <summary>
        /// 実施年月
        /// </summary>
        public OperationYearMonth OperationYearMonth => SharedViewModel.Instance.OperationYearMonth;

        /// <summary>
        /// SKを印刷するか
        /// </summary>
        public bool PrintSK
        {
            get { return checkList[(int)InventoryPrintTieSizaiCategory.SK]; }
            set { Set(ref checkList[(int)InventoryPrintTieSizaiCategory.SK], value); }
        }

        /// <summary>
        /// EFを印刷するか
        /// </summary>
        public bool PrintEF
        {
            get { return checkList[(int)InventoryPrintTieSizaiCategory.EF]; }
            set { Set(ref checkList[(int)InventoryPrintTieSizaiCategory.EF], value); }
        }

        /// <summary>
        /// LFを印刷するか
        /// </summary>
        public bool PrintLF
        {
            get { return checkList[(int)InventoryPrintTieSizaiCategory.LF]; }
            set { Set(ref checkList[(int)InventoryPrintTieSizaiCategory.LF], value); }
        }

        /// <summary>
        /// 築炉を印刷するか
        /// </summary>
        public bool PrintBuilding
        {
            get { return checkList[(int)InventoryPrintTieSizaiCategory.Chikuro]; }
            set { Set(ref checkList[(int)InventoryPrintTieSizaiCategory.Chikuro], value); }
        }

        /// <summary>
        /// LDを印刷するか
        /// </summary>
        public bool PrintLD
        {
            get { return checkList[(int)InventoryPrintTieSizaiCategory.LD]; }
            set { Set(ref checkList[(int)InventoryPrintTieSizaiCategory.LD], value); }
        }

        /// <summary>
        /// TDを印刷するか
        /// </summary>
        public bool PrintTD
        {
            get { return checkList[(int)InventoryPrintTieSizaiCategory.TD]; }
            set { Set(ref checkList[(int)InventoryPrintTieSizaiCategory.TD], value); }
        }

        /// <summary>
        /// CCを印刷するか
        /// </summary>
        public bool PrintCC
        {
            get { return checkList[(int)InventoryPrintTieSizaiCategory.CC]; }
            set { Set(ref checkList[(int)InventoryPrintTieSizaiCategory.CC], value); }
        }

        /// <summary>
        /// その他を印刷するか
        /// </summary>
        public bool PrintOthers
        {
            get { return checkList[(int)InventoryPrintTieSizaiCategory.ETC]; }
            set { Set(ref checkList[(int)InventoryPrintTieSizaiCategory.ETC], value); }
        }

        /// <summary>
        /// 印刷対象データ
        /// </summary>
        public ObservableCollection<PrintRecordViewModel> PrintTargetList
        {
            get { return printTargetList; }
            set { Set(ref printTargetList, value); }
        }

        /// <summary>
        /// 編集モード
        /// </summary>
        public Visibility EditModeVisibility
        {
            set { Set(ref editModeVisibility, value); }
            get { return editModeVisibility; }
        }

        /// <summary>
        /// 選択モード
        /// </summary>
        public Visibility EditOperationVisibility
        {
            set { Set(ref editOperationVisibility, value); }
            get { return editOperationVisibility; }
        }

        /// <summary>
        /// データグリッド高さ
        /// </summary>
        public int DataGridHeight
        {
            get { return dataGridHeight; }
            set { Set(ref dataGridHeight, value); }
        }

        /// <summary>
        /// 品目CD
        /// </summary>
        public string HinmokuCD
        {
            get { return hinmokuCD; }
            set { Set(ref hinmokuCD, value); }
        }

        /// <summary>
        /// 業者CD
        /// </summary>
        public string GyosyaCD
        {
            get { return gyosyaCD; }
            set { Set(ref gyosyaCD, value); }
        }

        /// <summary>
        /// 品目名
        /// </summary>
        public string HinmokuName
        {
            get { return hinmokuName; }
            set { Set(ref hinmokuName, value); }
        }

        /// <summary>
        /// 業者名
        /// </summary>
        public string GyosyaName
        {
            get { return gyosyaName; }
            set { Set(ref gyosyaName, value); }
        }

        /// <summary>
        /// 備考
        /// </summary>
        public string Bikou
        {
            get { return bikou; }
            set { Set(ref bikou, value); }
        }

        /// <summary>
        /// カレント行
        /// </summary>
        private int current;
        public int Current
        {
            get { return current; }
            set { Set(ref current, value); }
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal override async Task InitializeAsync()
        {
            //画面表示時に必ずデータ更新するため、保持データをクリアしておく

            try
            {
                BusyStatus.IsBusy = true;

                dataList.Clear();
                deletedDataList.Clear();
                selectSIzaiCategory = InventoryPrintTieSizaiCategory.SK;

                Initialize();
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

            await base.InitializeAsync();
        }

        /// <summary>
        /// 初期化実処理
        /// </summary>
        private void Initialize()
        {
            try
            {
                IsFixed = CommonUtility.IsCurrentOperationFixed();

                isFirstTime = true;
                editModeVisibility = Visibility.Collapsed;
                editOperationVisibility = Visibility.Visible;
                DataGridHeight = 250;

                selectSIzaiCategory = InventoryPrintTieSizaiCategory.SK;

                OpecitySK = 1.0;

                TogetuClear = false;
                IsReadonlyCD = true;
                IsReadonlyName = true;

                for (int i = (int)InventoryPrintTieSizaiCategory.Max; i >= 0; i--)
                {
                    selectSIzaiCategory = (InventoryPrintTieSizaiCategory)i;

                    updateTarget();
                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 印刷
        /// </summary>
        internal override async Task<bool> PrintAsync()
        {
            bool hasException = false;
            string errorContent = null;
            string errorCode = null;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;
            bool isTargetNotChecked = false;
            bool isAnyChecked = false;

            try
            {
                if (!checkList.Any(c => c))
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "TargetNotChecked");
                    isTargetNotChecked = true;
                    return false;
                }

                BusyStatus.IsBusy = true;

                // Excelアクセッサーのインスタンスの取得
                excelAccessor = ExcelAccessor.GetInstance();
                var tcs = new TaskCompletionSource<bool>();
                var thread = new Thread(async () =>
                {
                    try
                    {
                        for (int i = 0; i < (int)InventoryPrintTieSizaiCategory.Max; ++i)
                        {
                            if (checkList[i])
                            {
                                // 資材棚卸し調査表の出力
                                await excelAccessor.OutputInventoryPrint
                                    ((InventoryPrintTieSizaiCategory)i, dataList[(InventoryPrintTieSizaiCategory)i].ToArray(), TogetuClear);
                            }
                        }

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

                //資材棚卸マスタの更新
                await updateSizaiTanaorosi();

                //資材棚卸ワークへ登録
                await updateSizaiTanaorosiWrk();

                //編集有無フラグのリセット
                for (int i = 0; i < (int)InventoryPrintTieSizaiCategory.Max; ++i)
                    editList[i] = false;

                isFirstTime = false;

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
                if (!isTargetNotChecked)
                {
                    LogType logType = hasException ? LogType.Error : LogType.Normal;
                    string bikou = null;
                    if (!hasException)
                    {
                        StringBuilder sb = new StringBuilder();
                        // SK
                        if (PrintSK)
                        {
                            sb.Append(Constants.SK_StringDefine);
                            isAnyChecked = true;
                        }
                        // EF
                        if (PrintEF)
                        {
                            if (isAnyChecked)
                            {
                                sb.Append(Constants.Comma_StringDefine);
                            }
                            sb.Append(Constants.EF_StringDefine);
                            isAnyChecked = true;
                        }
                        // LF
                        if (PrintLF)
                        {
                            if (isAnyChecked)
                            {
                                sb.Append(Constants.Comma_StringDefine);
                            }
                            sb.Append(Constants.LF_StringDefine);
                            isAnyChecked = true;
                        }
                        // 築炉
                        if (PrintBuilding)
                        {
                            if (isAnyChecked)
                            {
                                sb.Append(Constants.Comma_StringDefine);
                            }
                            sb.Append(Constants.Chikuro_StringDefine);
                            isAnyChecked = true;
                        }
                        // LD
                        if (PrintLD)
                        {
                            if (isAnyChecked)
                            {
                                sb.Append(Constants.Comma_StringDefine);
                            }
                            sb.Append(Constants.LD_StringDefine);
                            isAnyChecked = true;
                        }
                        // TD
                        if (PrintTD)
                        {
                            if (isAnyChecked)
                            {
                                sb.Append(Constants.Comma_StringDefine);
                            }
                            sb.Append(Constants.TD_StringDefine);
                            isAnyChecked = true;
                        }
                        // CC
                        if (PrintCC)
                        {
                            if (isAnyChecked)
                            {
                                sb.Append(Constants.Comma_StringDefine);
                            }
                            sb.Append(Constants.CC_StringDefine);
                            isAnyChecked = true;
                        }
                        // 他
                        if (PrintOthers)
                        {
                            if (isAnyChecked)
                            {
                                sb.Append(Constants.Comma_StringDefine);
                            }
                            sb.Append(Constants.Others_StringDefine);
                            isAnyChecked = true;
                        }
                        bikou = sb.ToString();
                    }

                    // 棚卸ログの挿入
                    await InsertTanaorosiLog(logType, SizaiWorkCategory.TanaoroshiWork,
                                                LogOperationType.PrintInventorySheet,
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

                BusyStatus.IsBusy = false;
            }

        }

        /// <summary>
        /// 修正
        /// </summary>
        internal override async Task<bool> ModifyAsync()
        {
            bool hasException = false;
            string errorContent = null;
            string errorCode = null;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;

            try
            {
                BusyStatus.IsBusy = true;

                // 共有データで保持している印刷対象外受払い情報の初期化
                CommonUtility.InitializeExcludeInputPaymentInfo();

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
                LogType logType = hasException ? LogType.Error : LogType.Normal;

                // 棚卸ログの挿入
                await InsertTanaorosiLog(logType, SizaiWorkCategory.TanaoroshiWork,
                                         LogOperationType.PrintInventorySheet,
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
                    // 品目マスタ存在チェック処理
                    var noHinmokuList = await DataService.CheckHinmokuMaster();
                    // 品目マスタにないデータが存在するか
                    if (noHinmokuList.Count() > 0)
                    {
                        string notHinmoku = Constants.HinmokuCode_StringDefine + noHinmokuList.FirstOrDefault().Item1;
                        // 品目マスタに存在しない品目コードのメッセージを表示する
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "NotHasHinmokuCodeOfSizai_Hinmoku_Mst", notHinmoku);
                        return false;
                    }

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
        /// 前へ
        /// </summary>
        internal override async Task<bool> BackPreviousAsync()
        {
            try
            {
                bool isEdit = false;

                //編集有無フラグのリセット
                for (int i = 0; i < (int)InventoryPrintTieSizaiCategory.Max; ++i)
                {
                    if (editList[i])
                    {
                        isEdit = true;
                        break;
                    }
                }

                if (isEdit)
                {
                    if (MessageManager.ShowQuestion(SystemID.CKSI1010, "ConfirmSavePrintData") == ResultType.Yes)
                    {
                        BusyStatus.IsBusy = true;

                        //資材棚卸マスタの更新
                        await updateSizaiTanaorosi();

                        //資材棚卸ワークへ登録
                        await updateSizaiTanaorosiWrk();

                        //編集有無フラグのリセット
                        for (int i = 0; i < (int)InventoryPrintTieSizaiCategory.Max; ++i)
                            editList[i] = false;

                        isFirstTime = false;

                        BusyStatus.IsBusy = false;
                        MessageManager.ShowInformation(SystemID.CKSI1010, "SaveCompleted");

                    }
                }

                return await base.BackPreviousAsync();

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
        /// ぱんくずリストの選択
        /// </summary>
        internal override async Task<bool> SelecOperationAsync()
        {
            try
            {
                BusyStatus.IsBusy = true;

                // 品目マスタ存在チェック処理
                var noHinmokuList = await DataService.CheckHinmokuMaster();
                // 品目マスタにないデータが存在するか
                if (noHinmokuList.Count() > 0)
                {
                    string notHinmoku = Constants.HinmokuCode_StringDefine+ noHinmokuList.FirstOrDefault().Item1;
                    // 品目マスタに存在しない品目コードのメッセージを表示する
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "NotHasHinmokuCodeOfSizai_Hinmoku_Mst", notHinmoku);
                }

                return await base.SelecOperationAsync();
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
        /// ×ボタン
        /// </summary>
        internal override async Task<bool> CloseAsync()
        {
            bool isEdit = false;
            bool hasException = false;
            string errorContent = null;
            string errorCode = null;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;

            try
            {
                //編集有無フラグのリセット
                for (int i = 0; i < (int)InventoryPrintTieSizaiCategory.Max; ++i)
                {
                    if (editList[i])
                    {
                        isEdit = true;
                        break;
                    }
                }

                if (isEdit)
                {
                    if (MessageManager.ShowQuestion(SystemID.CKSI1010, "ConfirmSavePrintData") == ResultType.Yes)
                    {
                        BusyStatus.IsBusy = true;

                        //資材棚卸マスタの更新
                        await updateSizaiTanaorosi();

                        //資材棚卸ワークへ登録
                        await updateSizaiTanaorosiWrk();

                        //編集有無フラグのリセット
                        for (int i = 0; i < (int)InventoryPrintTieSizaiCategory.Max; ++i)
                            editList[i] = false;

                        isFirstTime = false;

                        BusyStatus.IsBusy = false;
                        MessageManager.ShowInformation(SystemID.CKSI1010, "SaveCompleted");

                    }
                }

                return await base.CloseAsync();

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
                if (isEdit)
                {
                    LogType logType = hasException ? LogType.Error : LogType.Normal;
                    // 棚卸ログの挿入
                    await InsertTanaorosiLog(logType, SizaiWorkCategory.TanaoroshiWork,
                                             LogOperationType.PrintInventorySheet,
                                             LogOperationContent.Close, null,errorContent, errorCode);
                    if(hasException)
                    {
                        MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                    }
                }
            }
        }

        /// <summary>
        /// 資材棚卸マスタの更新
        /// </summary>
        private async Task<bool> updateSizaiTanaorosi()
        {
            try
            {
                for (int i = 0; i < (int)InventoryPrintTieSizaiCategory.Max; ++i)
                {
                    // 変更あった資材区分のみ処理する
                    if (false == editList[(int)i])
                    {
                        continue;
                    }

                    // 削除データがあれば削除する
                    if (deletedDataList.ContainsKey((InventoryPrintTieSizaiCategory)i))
                    {
                        if (0 < deletedDataList[(InventoryPrintTieSizaiCategory)i].Count())
                        {
                            // データ削除
                            await DataService.DeletePrintData(deletedDataList[(InventoryPrintTieSizaiCategory)i], i + 1);
                        }
                    }

                    var records = dataList[(InventoryPrintTieSizaiCategory)i].Select(n => new InventoryPrint()
                    {
                        Bikou = n.Bikou,
                        CCZaiko = n.CcZaiko,
                        EFZaiko = n.EfZaiko,
                        GyosyaCD = n.GyosyaCD,
                        GyosyaName = n.GyosyaName,
                        Henpin = n.Henpin,
                        HinmokuCD = n.HinmokuCD,
                        HinmokuName = n.HinmokuName,
                        LFZaiko = n.LfZaiko,
                        NyukoRyo = n.NyukoRyo,
                        ItemOrder = n.Order,
                        SKZaiko = n.SokoZaiko,
                        MeterZaiko = n.StockMeter,
                        OtherZaiko = n.StockOther,
                        Reserve1Zaiko = n.StockReserve1,
                        Reserve2Zaiko = n.StockReserve2,
                        Harai = n.Harai,
                        Workkbn = SizaiWorkCategory.TanaoroshiWork.GetStringValue()
                    });

                    // データ更新
                    var tmp = await DataService.MergePrintData(records.ToList(), i + 1);
                }

                return true;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 資材棚卸ワークの登録
        /// </summary>
        private async Task<bool> updateSizaiTanaorosiWrk()
        {
            try
            {
                IList<InventoryPrint> sizaiTanaorosiWrkList = new List<InventoryPrint>();

                for (int i = 0; i < (int)InventoryPrintTieSizaiCategory.Max; ++i)
                {
                    // 変更あった資材区分のみ処理する
                    // 初回は必ず更新する
                    if ((false == editList[(int)i]) && (isFirstTime == false))
                    {
                        continue;
                    }

                    var list = dataList[(InventoryPrintTieSizaiCategory)i].Select(n => new InventoryPrint()
                    {
                        Bikou = n.Bikou,
                        CCZaiko = n.CcZaiko,
                        EFZaiko = n.EfZaiko,
                        GyosyaCD = n.GyosyaCD,
                        GyosyaName = n.GyosyaName,
                        Henpin = n.Henpin,
                        HinmokuCD = n.HinmokuCD,
                        HinmokuName = n.HinmokuName,
                        Togetsuryo = (TogetuClear) ? string.Empty : n.Togetsuryo,
                        LFZaiko = n.LfZaiko,
                        NyukoRyo = n.NyukoRyo,
                        ItemOrder = n.Order,
                        SKZaiko = n.SokoZaiko,
                        MeterZaiko = n.StockMeter,
                        OtherZaiko = n.StockOther,
                        Reserve1Zaiko = n.StockReserve1,
                        Reserve2Zaiko = n.StockReserve2,
                        Harai = n.Harai,
                        Workkbn = SizaiWorkCategory.TanaoroshiWork.GetStringValue()
                    });

                    // データ更新
                    await DataService.UpdateWrk(list.ToList(), i + 1);
                }

                return true;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 編集モード更新
        /// </summary>
        private void updateEditMode()
        {
            try
            {
                if (editModeVisibility == Visibility.Collapsed)
                {
                    EditModeVisibility = Visibility.Visible;
                    EditOperationVisibility = Visibility.Collapsed;

                    DataGridHeight = 220;

                    if (editMode == EditMode.Add)
                    {
                        IsReadonlyCD = false;
                        IsReadonlyName = true;
                    }
                    else if (editMode == EditMode.Modify)
                    {
                        IsReadonlyCD = true;
                        IsReadonlyName = false;
                    }

                    IsShowEditForm = true;

                }
                else
                {
                    //解除
                    EditModeVisibility = Visibility.Collapsed;
                    EditOperationVisibility = Visibility.Visible;
                    DataGridHeight = 250;

                    IsShowEditForm = false;
                }
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

        }

        /// <summary>
        /// 編集データクリア
        /// </summary>
        private void clear()
        {
            HinmokuCD = string.Empty;
            GyosyaCD = string.Empty;
            HinmokuName = string.Empty;
            GyosyaName = string.Empty;
            Bikou = string.Empty;
        }

        /// <summary>
        /// 表示対象区分のデータ更新
        /// </summary>
        private async void updateTarget()
        {
            try
            {
                // DBから該当の資材区分の印刷対象データを取得する
                var allList = await DataService.GetPrintModelAsync((int)selectSIzaiCategory + 1);

                var modelList = allList.ToList().Where(n => n.Workkbn.Equals(SizaiWorkCategory.TanaoroshiWork.GetStringValue()));

                // 格納用変数
                ObservableCollection<PrintRecordViewModel> targetList = new ObservableCollection<PrintRecordViewModel>();

                // 表示データ保持
                if (false == dataList.ContainsKey(selectSIzaiCategory))
                {
                    foreach (var eachItem in modelList)
                    {
                        PrintRecordViewModel tmpTarget = new PrintRecordViewModel();
                        tmpTarget.HinmokuCD = eachItem.HinmokuCD;
                        tmpTarget.GyosyaCD = eachItem.GyosyaCD;
                        tmpTarget.HinmokuName = eachItem.HinmokuName;
                        tmpTarget.GyosyaName = eachItem.GyosyaName;
                        tmpTarget.Bikou = eachItem.Bikou;
                        tmpTarget.Syubetu = eachItem.Syubetu;
                        tmpTarget.Order = eachItem.ItemOrder;
                        tmpTarget.Togetsuryo = eachItem.Togetsuryo;

                        // 品目コードの受払い種別が使用高払いかどうか
                        var isSiyoudakaBarai 
                            = await DataService.GetUkebaraiSyubetuAsync(tmpTarget.HinmokuCD) == UkebaraiType.SiyoudakaBarai ? true : false;
                        // 前月在庫情報を設定
                        Inventory inventory = null;
                        if (isSiyoudakaBarai)
                        {
                            // 品目コード、業者コードをキーにして先月棚卸データを取得
                            inventory = SharedModel.Instance.LastInventories
                                            .FirstOrDefault
                                            (item => (item.ItemCode == tmpTarget.HinmokuCD) 
                                                  && (item.SupplierCode == tmpTarget.GyosyaCD));
                        }
                        else
                        {
                            // 品目コードをキーにして先月棚卸データを取得
                            inventory = SharedModel.Instance.LastInventories
                                            .FirstOrDefault(item => item.ItemCode == tmpTarget.HinmokuCD);
                        }
                        
                        if (null != inventory)
                        {
                            tmpTarget.SokoZaiko = (int)inventory.StockInWarehouse;
                            tmpTarget.EfZaiko = (int)inventory.StockEF;
                            tmpTarget.LfZaiko = (int)inventory.StockLF;
                            tmpTarget.CcZaiko = (int)inventory.StockCC;
                            tmpTarget.StockOther = inventory.StockOther;
                            tmpTarget.StockMeter = inventory.StockMeter;
                            tmpTarget.StockReserve1 = inventory.StockReserve1;
                            tmpTarget.StockReserve2 = inventory.StockReserve2;
                            tmpTarget.LastMonth = ((int)inventory.StockInWarehouse + (int)inventory.StockEF + (int)inventory.StockLF + (int)inventory.StockCC);
                        }

                        // 入庫情報を設定
                        if (isSiyoudakaBarai)
                        {
                            // 品目コード、業者コードをキーにして入庫情報を取得
                            var receiving = SharedModel.Instance.Receivings
                                                .FirstOrDefault
                                                (n => (n.ItemCode == tmpTarget.HinmokuCD) 
                                                   && (n.GyosyaCD == tmpTarget.GyosyaCD));

                            if (null != receiving)
                            {
                                tmpTarget.NyukoRyo = (int)receiving.Amount;
                            }
                        }
                        else
                        {
                            // 品目コードをキーにして入庫情報を取得
                            var receiving = SharedModel.Instance.Receivings
                                            .FirstOrDefault(n => n.ItemCode == tmpTarget.HinmokuCD);
                            if (null != receiving)
                            {
                                // 入庫払いは業者コードを指定しないため、
                                // 取得した入庫情報のサマリを入庫量に設定
                                tmpTarget.NyukoRyo = (int)SharedModel.Instance.Receivings.Where(n => n.ItemCode == tmpTarget.HinmokuCD).Select(x => x.Amount).Sum();
                            }
                        }

                        // 出庫情報を設定
                        switch (selectSIzaiCategory)
                        {
                            case InventoryPrintTieSizaiCategory.SK:
                                var leaveingSK = SharedModel.Instance.Leavings
                                    .FirstOrDefault(n => n.ItemCode == tmpTarget.HinmokuCD);
                                if (null != leaveingSK)
                                {
                                    tmpTarget.Harai += (int)leaveingSK.Amount;
                                }
                                break;

                            case InventoryPrintTieSizaiCategory.EF:
                                var leaveingEF = SharedModel.Instance.Leavings
                                    .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD) 
                                        && (n.Mukesaki == Mukesaki.EF.GetStringValue()));
                                if (null != leaveingEF)
                                {
                                    tmpTarget.Harai += (int)leaveingEF.Amount;
                                }
                                break;

                            case InventoryPrintTieSizaiCategory.LF:
                                var leaveingLF = SharedModel.Instance.Leavings
                                    .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD) 
                                        && (n.Mukesaki == Mukesaki.LF.GetStringValue()));
                                if (null != leaveingLF)
                                {
                                    tmpTarget.Harai += (int)leaveingLF.Amount;
                                }
                                break;

                            case InventoryPrintTieSizaiCategory.Chikuro:
                                var leaveingChikuro = SharedModel.Instance.Leavings
                                    .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD) 
                                        && (n.Mukesaki == Mukesaki.Building.GetStringValue()));
                                if (null != leaveingChikuro)
                                {
                                    tmpTarget.Harai += (int)leaveingChikuro.Amount;
                                }
                                break;

                            case InventoryPrintTieSizaiCategory.LD:
                                var leaveingLD = SharedModel.Instance.Leavings
                                    .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD) 
                                        && (n.Mukesaki == Mukesaki.LD.GetStringValue()));
                                if (null != leaveingLD)
                                {
                                    tmpTarget.Harai += (int)leaveingLD.Amount;
                                }
                                break;


                            case InventoryPrintTieSizaiCategory.CC:
                                var leaveingCC = SharedModel.Instance.Leavings
                                    .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD) 
                                        && (n.Mukesaki == Mukesaki.CC.GetStringValue()));
                                if (null != leaveingCC)
                                {
                                    tmpTarget.Harai += (int)leaveingCC.Amount;
                                }
                                break;

                            case InventoryPrintTieSizaiCategory.TD:
                                var leaveingTD = SharedModel.Instance.Leavings
                                    .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD) 
                                        && (n.Mukesaki == Mukesaki.TD.GetStringValue()));
                                if (null != leaveingTD)
                                {
                                    tmpTarget.Harai += (int)leaveingTD.Amount;
                                }
                                break;

                            case InventoryPrintTieSizaiCategory.ETC:
                                var leaveingETC = SharedModel.Instance.Leavings
                                    .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD) 
                                        && (n.Mukesaki == Mukesaki.Others.GetStringValue()));
                                if (null != leaveingETC)
                                {
                                    tmpTarget.Harai += (int)leaveingETC.Amount;
                                }
                                break;

                            default:
                                break;
                        }

                        // 直送情報を設定
                        switch (selectSIzaiCategory)
                        {
                            case InventoryPrintTieSizaiCategory.SK:
                                OutWorkNoteItem directDeliverysSK = null;
                                if (isSiyoudakaBarai)
                                {
                                    // 品目コード、業者コードをキーにして直送(SK)情報を取得
                                    directDeliverysSK = SharedModel.Instance.DirectDeliverys
                                        .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD) && (n.GyosyaCD == tmpTarget.GyosyaCD));

                                    if (null != directDeliverysSK)
                                    {
                                        tmpTarget.NyukoRyo += (int)directDeliverysSK.Amount;
                                        tmpTarget.Harai += (int)directDeliverysSK.Amount;
                                    }
                                }
                                else
                                {
                                    // 品目コードをキーにして直送(SK)情報を取得
                                    directDeliverysSK = SharedModel.Instance.DirectDeliverys
                                        .FirstOrDefault(n => n.ItemCode == tmpTarget.HinmokuCD);

                                    if (null != directDeliverysSK)
                                    {
                                        // 入庫払いは業者コードを指定しないため、
                                        // 取得した直送(SK)情報のサマリを入庫量/出庫量に設定
                                        var amount = (int)SharedModel.Instance.DirectDeliverys.Where(n => n.ItemCode == tmpTarget.HinmokuCD).Select(x => x.Amount).Sum();
                                        tmpTarget.NyukoRyo += amount;
                                        tmpTarget.Harai += amount;
                                    }
                                }

                                break;

                            case InventoryPrintTieSizaiCategory.EF:
                                OutWorkNoteItem directDeliverysEF = null;
                                if (isSiyoudakaBarai)
                                {
                                    // 品目コード、業者コード、向先をキーにして直送(EF)情報を取得
                                    directDeliverysEF = SharedModel.Instance.DirectDeliverys
                                        .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                        && (n.GyosyaCD == tmpTarget.GyosyaCD)
                                        && (n.Mukesaki == Mukesaki.EF.GetStringValue()));

                                    if (null != directDeliverysEF)
                                    {
                                        tmpTarget.NyukoRyo += (int)directDeliverysEF.Amount;
                                        tmpTarget.Harai += (int)directDeliverysEF.Amount;
                                    }
                                }
                                else
                                {
                                    // 品目コード、向先をキーにして直送(EF)情報を取得
                                    directDeliverysEF = SharedModel.Instance.DirectDeliverys
                                        .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                        && (n.Mukesaki == Mukesaki.EF.GetStringValue()));

                                    if (null != directDeliverysEF)
                                    {
                                        // 入庫払いは業者コードを指定しないため、
                                        // 取得した直送(EF)情報のサマリを入庫量/出庫量に設定
                                        var amount = (int)SharedModel.Instance.DirectDeliverys
                                            .Where(n => (n.ItemCode == tmpTarget.HinmokuCD) 
                                            && (n.Mukesaki == Mukesaki.EF.GetStringValue())).Select(x => x.Amount).Sum();
                                        tmpTarget.NyukoRyo += amount;
                                        tmpTarget.Harai += amount;
                                    }
                                }

                                break;

                            case InventoryPrintTieSizaiCategory.LF:
                                OutWorkNoteItem directDeliverysLF = null;
                                if (isSiyoudakaBarai)
                                {
                                    // 品目コード、業者コード、向先をキーにして直送(LF)情報を取得
                                    directDeliverysLF = SharedModel.Instance.DirectDeliverys
                                        .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.GyosyaCD == tmpTarget.GyosyaCD)
                                            && (n.Mukesaki == Mukesaki.LF.GetStringValue()));

                                    if (null != directDeliverysLF)
                                    {
                                        tmpTarget.NyukoRyo += (int)directDeliverysLF.Amount;
                                        tmpTarget.Harai += (int)directDeliverysLF.Amount;
                                    }
                                }
                                else
                                {
                                    // 品目コード、向先をキーにして直送(LF)情報を取得
                                    directDeliverysLF = SharedModel.Instance.DirectDeliverys
                                        .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.Mukesaki == Mukesaki.LF.GetStringValue()));

                                    if (null != directDeliverysLF)
                                    {
                                        // 入庫払いは業者コードを指定しないため、
                                        // 取得した直送(LF)情報のサマリを入庫量/出庫量に設定
                                        var amount = (int)SharedModel.Instance.DirectDeliverys
                                            .Where(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.Mukesaki == Mukesaki.LF.GetStringValue())).Select(x => x.Amount).Sum();
                                        tmpTarget.NyukoRyo += amount;
                                        tmpTarget.Harai += amount;
                                    }
                                }
                                break;

                            case InventoryPrintTieSizaiCategory.Chikuro:
                                OutWorkNoteItem directDeliverysChikuro = null;
                                if (isSiyoudakaBarai)
                                {
                                    // 品目コード、業者コード、向先をキーにして直送(築炉)情報を取得
                                    directDeliverysChikuro = SharedModel.Instance.DirectDeliverys
                                        .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.GyosyaCD == tmpTarget.GyosyaCD)
                                            && (n.Mukesaki == Mukesaki.Building.GetStringValue()));

                                    if (null != directDeliverysChikuro)
                                    {
                                        tmpTarget.NyukoRyo += (int)directDeliverysChikuro.Amount;
                                        tmpTarget.Harai += (int)directDeliverysChikuro.Amount;
                                    }
                                }
                                else
                                {
                                    // 品目コード、向先をキーにして直送(築炉)情報を取得
                                    directDeliverysChikuro = SharedModel.Instance.DirectDeliverys
                                        .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.Mukesaki == Mukesaki.Building.GetStringValue()));

                                    if (null != directDeliverysChikuro)
                                    {
                                        // 入庫払いは業者コードを指定しないため、
                                        // 取得した直送(築炉)情報のサマリを入庫量/出庫量に設定
                                        var amount = (int)SharedModel.Instance.DirectDeliverys
                                            .Where(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.Mukesaki == Mukesaki.Building.GetStringValue())).Select(x => x.Amount).Sum();
                                        tmpTarget.NyukoRyo += amount;
                                        tmpTarget.Harai += amount;
                                    }
                                }
                                break;

                            case InventoryPrintTieSizaiCategory.LD:
                                OutWorkNoteItem directDeliverysLD = null;
                                if (isSiyoudakaBarai)
                                {
                                    // 品目コード、業者コード、向先をキーにして直送(LD)情報を取得
                                    directDeliverysLD = SharedModel.Instance.DirectDeliverys
                                        .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.GyosyaCD == tmpTarget.GyosyaCD)
                                            && (n.Mukesaki == Mukesaki.LD.GetStringValue()));

                                    if (null != directDeliverysLD)
                                    {
                                        tmpTarget.NyukoRyo += (int)directDeliverysLD.Amount;
                                        tmpTarget.Harai += (int)directDeliverysLD.Amount;
                                    }
                                }
                                else
                                {
                                    // 品目コード、向先をキーにして直送(LD)情報を取得
                                    directDeliverysLD = SharedModel.Instance.DirectDeliverys
                                        .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.Mukesaki == Mukesaki.LD.GetStringValue()));

                                    if (null != directDeliverysLD)
                                    {
                                        // 入庫払いは業者コードを指定しないため、
                                        // 取得した直送(LD)情報のサマリを入庫量/出庫量に設定
                                        var amount = (int)SharedModel.Instance.DirectDeliverys
                                            .Where(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.Mukesaki == Mukesaki.LD.GetStringValue())).Select(x => x.Amount).Sum();
                                        tmpTarget.NyukoRyo += amount;
                                        tmpTarget.Harai += amount;
                                    }
                                }
                                break;

                            case InventoryPrintTieSizaiCategory.CC:
                                OutWorkNoteItem directDeliverysCC = null;
                                if (isSiyoudakaBarai)
                                {
                                    // 品目コード、業者コード、向先をキーにして直送(CC)情報を取得
                                    directDeliverysCC = SharedModel.Instance.DirectDeliverys
                                        .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.GyosyaCD == tmpTarget.GyosyaCD)
                                            && (n.Mukesaki == Mukesaki.CC.GetStringValue()));

                                    if (null != directDeliverysCC)
                                    {
                                        tmpTarget.NyukoRyo += (int)directDeliverysCC.Amount;
                                        tmpTarget.Harai += (int)directDeliverysCC.Amount;
                                    }
                                }
                                else
                                {
                                    // 品目コード、向先をキーにして直送(CC)情報を取得
                                    directDeliverysCC = SharedModel.Instance.DirectDeliverys
                                        .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.Mukesaki == Mukesaki.CC.GetStringValue()));

                                    if (null != directDeliverysCC)
                                    {
                                        // 入庫払いは業者コードを指定しないため、
                                        // 取得した直送(CC)情報のサマリを入庫量/出庫量に設定
                                        var amount = (int)SharedModel.Instance.DirectDeliverys
                                            .Where(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.Mukesaki == Mukesaki.CC.GetStringValue())).Select(x => x.Amount).Sum();
                                        tmpTarget.NyukoRyo += amount;
                                        tmpTarget.Harai += amount;
                                    }
                                }

                                break;

                            case InventoryPrintTieSizaiCategory.TD:
                                OutWorkNoteItem directDeliverysTD = null;
                                if (isSiyoudakaBarai)
                                {
                                    // 品目コード、業者コード、向先をキーにして直送(TD)情報を取得
                                    directDeliverysTD = SharedModel.Instance.DirectDeliverys
                                        .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.GyosyaCD == tmpTarget.GyosyaCD)
                                            && (n.Mukesaki == Mukesaki.TD.GetStringValue()));

                                    if (null != directDeliverysTD)
                                    {
                                        tmpTarget.NyukoRyo += (int)directDeliverysTD.Amount;
                                        tmpTarget.Harai += (int)directDeliverysTD.Amount;
                                    }
                                }
                                else
                                {
                                    // 品目コード、向先をキーにして直送(TD)情報を取得
                                    directDeliverysTD = SharedModel.Instance.DirectDeliverys
                                        .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.Mukesaki == Mukesaki.TD.GetStringValue()));

                                    if (null != directDeliverysTD)
                                    {
                                        // 入庫払いは業者コードを指定しないため、
                                        // 取得した直送(TD)情報のサマリを入庫量/出庫量に設定
                                        var amount = (int)SharedModel.Instance.DirectDeliverys
                                            .Where(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.Mukesaki == Mukesaki.TD.GetStringValue())).Select(x => x.Amount).Sum();
                                        tmpTarget.NyukoRyo += amount;
                                        tmpTarget.Harai += amount;
                                    }
                                }

                                break;

                            case InventoryPrintTieSizaiCategory.ETC:
                                OutWorkNoteItem directDeliverysETC = null;
                                if (isSiyoudakaBarai)
                                {
                                    // 品目コード、業者コード、向先をキーにして直送(その他)情報を取得
                                    directDeliverysETC = SharedModel.Instance.DirectDeliverys
                                        .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.GyosyaCD == tmpTarget.GyosyaCD)
                                            && (n.Mukesaki == Mukesaki.Others.GetStringValue()));

                                    if (null != directDeliverysETC)
                                    {
                                        tmpTarget.NyukoRyo += (int)directDeliverysETC.Amount;
                                        tmpTarget.Harai += (int)directDeliverysETC.Amount;
                                    }
                                }

                                else
                                {
                                    // 品目コード、向先をキーにして直送(その他)情報を取得
                                    directDeliverysETC = SharedModel.Instance.DirectDeliverys
                                        .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.Mukesaki == Mukesaki.Others.GetStringValue()));

                                    if (null != directDeliverysETC)
                                    {
                                        // 入庫払いは業者コードを指定しないため、
                                        // 取得した直送(その他)情報のサマリを入庫量/出庫量に設定
                                        var amount = (int)SharedModel.Instance.DirectDeliverys
                                            .Where(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                            && (n.Mukesaki == Mukesaki.Others.GetStringValue())).Select(x => x.Amount).Sum();
                                        tmpTarget.NyukoRyo += amount;
                                        tmpTarget.Harai += amount;
                                    }
                                }

                                break;

                            default:
                                break;
                        }

                        // 返品情報を設定
                        WorkNoteItem returns = null;
                        if (isSiyoudakaBarai)
                        {
                            // 品目コード、業者コード、向先をキーにして返品情報を取得
                            returns = SharedModel.Instance.Returns
                                            .FirstOrDefault(n => (n.ItemCode == tmpTarget.HinmokuCD)
                                                && (n.GyosyaCD == tmpTarget.GyosyaCD));

                            if (null != returns)
                            {
                                tmpTarget.Henpin = (int)returns.Amount;
                            }
                        }
                        else
                        {
                            // 品目コードをキーにして返品情報を取得
                            returns = SharedModel.Instance.Returns
                                            .FirstOrDefault(n => n.ItemCode == tmpTarget.HinmokuCD);

                            if (null != returns)
                            {
                                // 入庫払いは業者コードを指定しないため、
                                // 取得した返品報のサマリを入庫量 /出庫量に設定
                                tmpTarget.Henpin = (int)SharedModel.Instance.Receivings.Where(n => n.ItemCode == tmpTarget.HinmokuCD).Select(x => x.Amount).Sum();
                            }
                        }

                        targetList.Add(tmpTarget);
                    }

                    dataList.Add(selectSIzaiCategory, targetList);
                }

                PrintTargetList = new ObservableCollection<PrintRecordViewModel>(dataList[selectSIzaiCategory]);
            }

            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        #region コマンド

        /// <summary>
        /// 資材区分(タブ)の切り替え
        /// </summary>
        public ICommand FireChangeTab => new RelayCommand<string>(button =>
        {
            try
            {
                // 編集箇所クリア
                clear();

                // 編集モード解除
                if (editModeVisibility != Visibility.Collapsed)
                {
                    updateEditMode();
                }

                OpecitySK = 0.5;
                OpecityEF = 0.5;
                OpecityLF = 0.5;
                OpecityBuilding = 0.5;
                OpecityLD = 0.5;
                OpecityTD = 0.5;
                OpecityCC = 0.5;
                OpecityOthers = 0.5;

                InventoryPrintTieSizaiCategory category = selectSIzaiCategory;

                switch (button)
                {
                    case Constants.SK_StringDefine:
                        category = InventoryPrintTieSizaiCategory.SK;
                        OpecitySK = 1.0;
                        break;
                    case Constants.EF_StringDefine:
                        category = InventoryPrintTieSizaiCategory.EF;
                        OpecityEF = 1.0;
                        break;
                    case Constants.LF_StringDefine:
                        category = InventoryPrintTieSizaiCategory.LF;
                        OpecityLF = 1.0;
                        break;

                    case Constants.Chikuro_String_Another_Define:
                        category = InventoryPrintTieSizaiCategory.Chikuro;
                        OpecityBuilding = 1.0;
                        break;
                    case Constants.LD_StringDefine:
                        category = InventoryPrintTieSizaiCategory.LD;
                        OpecityLD = 1.0;
                        break;

                    case Constants.TD_StringDefine:
                        category = InventoryPrintTieSizaiCategory.TD;
                        OpecityTD = 1.0;
                        break;
                    case Constants.CC_StringDefine:
                        category = InventoryPrintTieSizaiCategory.CC;
                        OpecityCC = 1.0;
                        break;
                    case Constants.Others_String_Another_Define:
                        category = InventoryPrintTieSizaiCategory.ETC;
                        OpecityOthers = 1.0;
                        break;
                    default:
                        break;
                }

                selectSIzaiCategory = category;

                updateTarget();
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

        });

        /// <summary>
        /// 全選択
        /// </summary>
        public ICommand FireSelectAll => new RelayCommand(() =>
        {
            PrintSK = true;
            PrintEF = true;
            PrintLF = true;
            PrintBuilding = true;
            PrintLD = true;
            PrintTD = true;
            PrintCC = true;
            PrintOthers = true;
        });

        /// <summary>
        /// 全選択解除
        /// </summary>
        public ICommand FireDeselectAll => new RelayCommand(() =>
        {
            PrintSK = false;
            PrintEF = false;
            PrintLF = false;
            PrintBuilding = false;
            PrintLD = false;
            PrintTD = false;
            PrintCC = false;
            PrintOthers = false;
        });

        /// <summary>
        /// ロストフォーカス
        /// </summary>
        public ICommand FireLostFocus => new RelayCommand<string>(type =>
        {
            string code = string.Empty;
            try
            {
                // 品目コード
                if (type == InputChecker.Hinmoku_StringDefine)
                {
                    // 空文字の場合は何もしない
                    if (HinmokuCD.Length == 0)
                    {
                        HinmokuName = string.Empty;
                        return;
                    }

                    code = HinmokuCD;

                    var check = InputChecker.CheckString(HinmokuCD);
                    if (InputChecker.Err_Check_Str_Unmatch == check)
                    {

                        MessageManager.ShowExclamation(SystemID.CKSI1010, "UnmatchCharsType");
                        HinmokuCD = string.Empty;
                        return;
                    }
                    else if (InputChecker.Err_Check_Str_Over == check)
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "UnmatchCharsCountHINMOKU");
                        HinmokuCD = string.Empty;
                        return;
                    }
                }

                // 業者コード
                else if (type == InputChecker.Gyosya_StringDefine)
                {
                    // 空文字の場合は何もしない
                    if (GyosyaCD.Length == 0)
                    {
                        GyosyaName = string.Empty;
                        return;
                    }

                    code = GyosyaCD;

                    var check = InputChecker.CheckString(GyosyaCD);
                    if (InputChecker.Err_Check_Str_Unmatch == check)
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "UnmatchCharsType");
                        GyosyaCD = string.Empty;
                        return;
                    }
                    else if (InputChecker.Err_Check_Str_Over == check)
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "UnmatchCharsCountGYOSYA");
                        GyosyaCD = string.Empty;
                        return;
                    }
                }

                // コード(品目コード or 業者コード)に紐付く名称を取得する
                var ret = DataService.CodeToName(type, code);

                // 取得できない場合
                if ((ret == null) || (ret.Result.Length == 0))
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "InputNotExistCode");

                    if (type == InputChecker.Hinmoku_StringDefine)
                    {
                        HinmokuCD = string.Empty;
                    }
                    else
                    {
                        GyosyaCD = string.Empty;
                    }

                    return;
                }
                // 取得できたので更新する
                if (type == InputChecker.Hinmoku_StringDefine)
                {
                    HinmokuName = ret.Result;
                }
                else
                {
                    GyosyaName = ret.Result;
                }
                return;
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

        });

        /// <summary>
        /// 追加
        /// </summary>
        public ICommand FireUpdateEditMode => new RelayCommand(() =>
        {
            try
            {
                editMode = EditMode.Add;
                clear();
                IsModifyEditMode = false;
                updateEditMode();
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }
        });

        /// <summary>
        /// 上へ下へ
        /// </summary>
        public ICommand FireReOrder => new RelayCommand<int>(async direction =>
        {
            int oldCurrent = -1;
            bool hasException = false;
            LogOperationContent operationContent;
            string beforeHinmokuCD = string.Empty;
            string beforeGyosyaCD = string.Empty;
            string beforeHinmokuName = string.Empty;
            string beforeGyosyaName = string.Empty;
            string beforeBikou = string.Empty;
            string errorContent = null;
            string errorCode = null;

            try
            {
                if (current != -1) //任意の項目が選択されている
                {
                    if (0 == direction) //下
                    {
                        if (printTargetList.Count >= (current + 2))
                        {
                            oldCurrent = current;
                            printTargetList.Move(current, current + 1);
                            dataList[selectSIzaiCategory].Move(oldCurrent, oldCurrent + 1);
                            Current = current;

                            editList[(int)selectSIzaiCategory] = true;
                        }
                    }
                    else
                    {
                        // 上
                        if (0 <= (current - 1))
                        {
                            oldCurrent = current;
                            printTargetList.Move(current, current - 1);
                            dataList[selectSIzaiCategory].Move(oldCurrent, oldCurrent - 1);
                            Current = current;

                            editList[(int)selectSIzaiCategory] = true;
                        }
                    }

                    // 移動前の品目CD、業者CD、品目名、業者名、備考を保持する
                    if (oldCurrent != -1)
                    {
                        beforeHinmokuCD = dataList[selectSIzaiCategory][oldCurrent].HinmokuCD;
                        beforeGyosyaCD = dataList[selectSIzaiCategory][oldCurrent].GyosyaCD;
                        beforeHinmokuName = dataList[selectSIzaiCategory][oldCurrent].HinmokuName;
                        beforeGyosyaName = dataList[selectSIzaiCategory][oldCurrent].GyosyaName;
                        beforeBikou = dataList[selectSIzaiCategory][oldCurrent].Bikou;
                    }
                }

                else
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "SelectSortItem");
                }
            }

            catch (Exception e)
            {
                hasException = true;
                errorContent = e.Message;
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

            finally
            {
                if (current != -1)
                {
                    LogType logType = hasException ? LogType.Error : LogType.Normal;
                    operationContent = direction == 0 ? LogOperationContent.Down : LogOperationContent.Up;

                    // 棚卸ログの挿入
                    await InsertTanaorosiLog(logType, SizaiWorkCategory.TanaoroshiWork, 
                                             LogOperationType.PrintInventorySheet, 
                                             operationContent, null, errorContent, errorCode);
                    if (oldCurrent != -1)
                    {
                        // 棚卸詳細ログの挿入
                        await InsertTanaorosiDetailLog(
                        logType, SizaiWorkCategory.TanaoroshiWork,
                        LogOperationType.PrintInventorySheet, 
                        operationContent, (SizaiCategory)(selectSIzaiCategory + 1),
                        beforeHinmokuCD.PadRight(4),
                        beforeGyosyaCD.PadRight(4),
                        CommonUtility.PadRightSJIS(beforeHinmokuName, 40),
                        CommonUtility.PadRightSJIS(beforeGyosyaName, 24),
                        CommonUtility.PadRightSJIS(beforeBikou, 40),
                        Constants.SortOrder_StringDefine + (Current + 1).ToString(), null, null, errorContent
                        );
                    }
                }
            }
        });

        /// <summary>
        /// 更新
        /// </summary>
        public ICommand FireUpdate => new RelayCommand(() =>
        {
            try
            {
                if (current != -1) //任意の項目が選択されている
                {

                    if (0 >= printTargetList.Count())
                    {
                        return;
                    }

                    editMode = EditMode.Modify;

                    if (current < 0)
                    {
                        if (printTargetList.Count > 0)
                        {
                            Current = 0;
                        }
                        else
                        {
                            return;
                        }
                    }

                    HinmokuCD = printTargetList[current].HinmokuCD?.Trim();
                    GyosyaCD = printTargetList[current].GyosyaCD?.Trim();
                    HinmokuName = printTargetList[current].HinmokuName?.Trim();
                    GyosyaName = printTargetList[current].GyosyaName?.Trim();
                    Bikou = printTargetList[current].Bikou?.Trim();

                    IsShowEditForm = true;
                    IsModifyEditMode = true;

                    updateEditMode();

                }

                else
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "SelectModifyItem");
                }
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

        });

        /// <summary>
        /// 削除処理実行
        /// </summary>
        public ICommand FireDelete => new RelayCommand(async () =>
        {
            bool hasException = false;
            InventoryPrint model = null;
            string errorContent = null;
            string errorCode = null;
            bool isLogExcluded = false;

            try
            {
                if (current != -1) //任意の項目が選択されている
                {
                    if (MessageManager.ShowQuestion(SystemID.CKSI1010, "ConfirmDelete") == ResultType.Yes)
                    {
                        //まだ削除データが存在しない場合、リストを作成
                        if (!deletedDataList.ContainsKey(selectSIzaiCategory))
                        {
                            var tmplist = new List<InventoryPrint>();
                            deletedDataList.Add(selectSIzaiCategory, tmplist);

                            editList[(int)selectSIzaiCategory] = true;
                        }

                        //削除データを作成
                        model = new InventoryPrint();
                        model.HinmokuCD = dataList[selectSIzaiCategory].ElementAt(current).HinmokuCD;
                        model.GyosyaCD = dataList[selectSIzaiCategory].ElementAt(current).GyosyaCD;
                        model.HinmokuName = dataList[selectSIzaiCategory].ElementAt(current).HinmokuName;
                        model.GyosyaName = dataList[selectSIzaiCategory].ElementAt(current).GyosyaName;
                        model.Bikou = dataList[selectSIzaiCategory].ElementAt(current).Bikou;

                        //削除用データを管理しているため、そこへ追加
                        deletedDataList[selectSIzaiCategory].Add(model);

                        dataList[selectSIzaiCategory].RemoveAt(current);
                        PrintTargetList = new ObservableCollection<PrintRecordViewModel>(dataList[selectSIzaiCategory]);
                    }
                    else
                    {
                        isLogExcluded = true;
                    }
                }

                else
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "SelectDeleteItem");
                }
            }

            catch (Exception e)
            {
                hasException = true;
                errorContent = e.Message;
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

            finally
            {
                if (current != -1 && !isLogExcluded)
                {
                    LogType logType = hasException ? LogType.Error : LogType.Normal;

                    // 棚卸ログの挿入
                    await InsertTanaorosiLog(logType, SizaiWorkCategory.TanaoroshiWork, 
                                             LogOperationType.PrintInventorySheet, 
                                             LogOperationContent.Delete, null, errorContent, errorCode);
                    // 棚卸詳細ログの挿入
                    await InsertTanaorosiDetailLog(
                     logType, SizaiWorkCategory.TanaoroshiWork,
                     LogOperationType.PrintInventorySheet, 
                     LogOperationContent.Delete,
                     (SizaiCategory)(selectSIzaiCategory + 1),
                     model.HinmokuCD.PadRight(4),
                     model.GyosyaCD.PadRight(4),
                     CommonUtility.PadRightSJIS(model.HinmokuName, 40),
                     CommonUtility.PadRightSJIS(model.GyosyaName, 24),
                     CommonUtility.PadRightSJIS(model.Bikou, 40),
                     null, null, null, errorContent
                    );
                }
            }

        });

        /// <summary>
        /// 更新処理実行
        /// </summary>
        public ICommand FireUpdateData => new RelayCommand(async () =>
        {
            // 変更前の品目CD、業者CD、品目名、業者名、備考を保持するための変数を定義する
            bool hasInputError = false;
            bool hasException = false;
            InventoryPrint data = null;
            string beforeHinmokuCD = string.Empty;
            string beforeGyosyaCD = string.Empty;
            string beforeHinmokuName = string.Empty;
            string beforeGyosyaName = string.Empty;
            string beforeBikou = string.Empty;
            string afterHinmokuName = string.Empty;
            string afterGyosyaName = string.Empty;
            string afterBikou = string.Empty;
            string errorContent = null;
            string errorCode = null;

            try
            {
                // 品目コード
                if (HinmokuCD == string.Empty)
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "InputIncomplete");
                    hasInputError = true;
                    return;
                }

                var hinmokuCode = HinmokuCD.PadRight(4);
                // 品目コードの受払い種別を取得
                var ukebaraiType = await DataService.GetUkebaraiSyubetuAsync(hinmokuCode);
                var isSiyoudakaBarai = ukebaraiType == UkebaraiType.SiyoudakaBarai ? true : false;
                if (isSiyoudakaBarai)
                {
                    if (string.IsNullOrEmpty(GyosyaCD.Trim()))
                    {
                        // 入力した品目コードの受払い種別が使用高払いで、
                        // 業者コードが入力されていない場合は、入力エラーとする。
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "NotInputGyosyaCode");
                        hasInputError = true;
                        return;
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(GyosyaCD.Trim()))
                    {
                        // 入力した品目コードの受払い種別が入庫払いで、
                        // 業者コードが入力されている場合は、入力エラーとする。
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "DoNotInputGyosyaCode");
                        hasInputError = true;
                        return;
                    }
                }

                // 品目名
                if(HinmokuName != null)
                {
                    // 品目名の入力チェック
                    if (InputChecker.CheckHinmokuName(HinmokuName) == InputChecker.Err_Check_Str_Over)
                    { 
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "UnmatchCharsCountHinmokuName");
                        hasInputError = true;
                        return;
                    }
                }

                // 業者名
                if (GyosyaName != null)
                {
                    // 業者名の入力チェック
                    if (InputChecker.CheckGyosyaName(GyosyaName) == InputChecker.Err_Check_Str_Over)
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "UnmatchCharsCountGyosyaName");
                        hasInputError = true;
                        return;
                    }
                }

                // 備考
                if (Bikou != null)
                {
                    // 備考の入力チェック
                    if (InputChecker.CheckBikou(Bikou) == InputChecker.Err_Check_Str_Over)
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "UnmatchCharsCountBiko");
                        hasInputError = true;
                        return;
                    }
                }

                //データの存在確認が必要
                data = new InventoryPrint();
                data.HinmokuCD = HinmokuCD.PadRight(4);
                data.GyosyaCD = GyosyaCD.PadRight(4);
                data.HinmokuName = CommonUtility.PadRightSJIS(HinmokuName, 40);
                data.GyosyaName = CommonUtility.PadRightSJIS(GyosyaName, 24);
                data.Bikou = CommonUtility.PadRightSJIS(Bikou, 40);

                if (editMode == EditMode.Add) //追加
                {
                    if (dataList[selectSIzaiCategory].Any(d => d.HinmokuCD?.Trim() == data.HinmokuCD?.Trim() && d.GyosyaCD?.Trim() == data.GyosyaCD?.Trim()))
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "InputDuplexdata");
                        hasInputError = true;
                        return;
                    }

                    PrintRecordViewModel tmp = new PrintRecordViewModel();
                    tmp.HinmokuCD = data.HinmokuCD;
                    tmp.GyosyaCD = data.GyosyaCD;
                    tmp.HinmokuName = data.HinmokuName;
                    tmp.GyosyaName = data.GyosyaName;
                    tmp.Bikou = data.Bikou;

                    //DB更新用LISTにも追加する
                    if (current < 0)
                    {
                        Current = 0;
                    }

                    // 前月在庫情報を設定
                    Inventory inventory = null;
                    if (isSiyoudakaBarai)
                    {
                        // 品目コード、業者コードをキーにして先月棚卸データを取得
                        inventory = SharedModel.Instance.LastInventories
                                        .FirstOrDefault
                                        (item => (item.ItemCode == tmp.HinmokuCD)
                                              && (item.SupplierCode == tmp.GyosyaCD));
                    }
                    else
                    {
                        // 品目コードをキーにして先月棚卸データを取得
                        inventory = SharedModel.Instance.LastInventories
                                        .FirstOrDefault(item => item.ItemCode == tmp.HinmokuCD);
                    }

                    if (null != inventory)
                    {
                        tmp.SokoZaiko = (int)inventory.StockInWarehouse;
                        tmp.EfZaiko = (int)inventory.StockEF;
                        tmp.LfZaiko = (int)inventory.StockLF;
                        tmp.CcZaiko = (int)inventory.StockCC;
                        tmp.StockOther = inventory.StockOther;
                        tmp.StockMeter = inventory.StockMeter;
                        tmp.StockReserve1 = inventory.StockReserve1;
                        tmp.StockReserve2 = inventory.StockReserve2;
                        tmp.LastMonth = ((int)inventory.StockInWarehouse + (int)inventory.StockEF + (int)inventory.StockLF + (int)inventory.StockCC);
                    }

                    // 入庫情報を設定
                    if(isSiyoudakaBarai)
                    {
                        // 品目コード、業者コードをキーにして入庫情報を取得
                        var receiving = SharedModel.Instance.Receivings
                                        .FirstOrDefault
                                        (n => (n.ItemCode == tmp.HinmokuCD) 
                                            && (n.GyosyaCD == tmp.GyosyaCD));

                        if (null != receiving)
                        {
                            tmp.NyukoRyo = (int)receiving.Amount;
                        }
                    }
                    else
                    {
                        // 品目コードをキーにして入庫情報を取得
                        var receiving = SharedModel.Instance.Receivings
                                        .FirstOrDefault(n => n.ItemCode == tmp.HinmokuCD);
                        if (null != receiving)
                        {
                            // 入庫払いは業者コードを指定しないため、
                            // 取得した入庫情報のサマリを入庫量に設定
                            tmp.NyukoRyo = (int)SharedModel.Instance.Receivings.Where(n => n.ItemCode == tmp.HinmokuCD).Select(x => x.Amount).Sum();
                        }
                    }

                    // 出庫情報を設定
                    switch (selectSIzaiCategory)
                    {
                        case InventoryPrintTieSizaiCategory.SK:
                            var leaveingSK = SharedModel.Instance.Leavings
                                .FirstOrDefault(n => n.ItemCode == tmp.HinmokuCD);
                            if (null != leaveingSK)
                            {
                                tmp.Harai += (int)leaveingSK.Amount;
                            }
                            break;

                        case InventoryPrintTieSizaiCategory.EF:
                            var leaveingEF = SharedModel.Instance.Leavings
                                .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                    && (n.Mukesaki == Mukesaki.EF.GetStringValue()));
                            if (null != leaveingEF)
                            {
                                tmp.Harai += (int)leaveingEF.Amount;
                            }
                            break;

                        case InventoryPrintTieSizaiCategory.LF:
                            var leaveingLF = SharedModel.Instance.Leavings
                                .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                    && (n.Mukesaki == Mukesaki.LF.GetStringValue()));
                            if (null != leaveingLF)
                            {
                                tmp.Harai += (int)leaveingLF.Amount;
                            }
                            break;

                        case InventoryPrintTieSizaiCategory.Chikuro:
                            var leaveingChikuro = SharedModel.Instance.Leavings
                                .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                    && (n.Mukesaki == Mukesaki.Building.GetStringValue()));
                            if (null != leaveingChikuro)
                            {
                                tmp.Harai += (int)leaveingChikuro.Amount;
                            }
                            break;

                        case InventoryPrintTieSizaiCategory.CC:
                            var leaveingCC = SharedModel.Instance.Leavings
                                .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                    && (n.Mukesaki == Mukesaki.CC.GetStringValue()));
                            if (null != leaveingCC)
                            {
                                tmp.Harai += (int)leaveingCC.Amount;
                            }
                            break;

                        case InventoryPrintTieSizaiCategory.TD:
                            var leaveingTD = SharedModel.Instance.Leavings
                                .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                    && (n.Mukesaki == Mukesaki.TD.GetStringValue()));
                            if (null != leaveingTD)
                            {
                                tmp.Harai += (int)leaveingTD.Amount;
                            }
                            break;

                        case InventoryPrintTieSizaiCategory.ETC:
                            var leaveingETC = SharedModel.Instance.Leavings
                                .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                    && (n.Mukesaki == Mukesaki.Others.GetStringValue()));
                            if (null != leaveingETC)
                            {
                                tmp.Harai += (int)leaveingETC.Amount;
                            }
                            break;

                        default:
                            break;
                    }

                    // 直送情報を設定
                    switch (selectSIzaiCategory)
                    {
                        case InventoryPrintTieSizaiCategory.SK:
                            OutWorkNoteItem directDeliverysSK = null;
                            if (isSiyoudakaBarai)
                            {
                                // 品目コード、業者コードをキーにして直送(SK)情報を取得
                                directDeliverysSK = SharedModel.Instance.DirectDeliverys
                                                            .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                                            && (n.GyosyaCD == tmp.GyosyaCD));

                                if (null != directDeliverysSK)
                                {
                                    tmp.NyukoRyo += (int)directDeliverysSK.Amount;
                                    tmp.Harai += (int)directDeliverysSK.Amount;
                                }
                            }
                            else
                            {
                                // 品目コードをキーにして直送(SK)情報を取得
                                directDeliverysSK = SharedModel.Instance.DirectDeliverys
                                                            .FirstOrDefault(n => n.ItemCode == tmp.HinmokuCD);

                                if (null != directDeliverysSK)
                                {
                                    // 入庫払いは業者コードを指定しないため、
                                    // 取得した直送(SK)情報のサマリを入庫量/出庫量に設定
                                    var amount = (int)SharedModel.Instance.DirectDeliverys.Where(n => n.ItemCode == tmp.HinmokuCD).Select(x => x.Amount).Sum();
                                    tmp.NyukoRyo += amount;
                                    tmp.Harai += amount;
                                }
                            }
                            break;

                        case InventoryPrintTieSizaiCategory.EF:
                            OutWorkNoteItem directDeliverysEF = null;
                            if (isSiyoudakaBarai)
                            {
                                // 品目コード、業者コード、向先をキーにして直送(EF)情報を取得
                                directDeliverysEF = SharedModel.Instance.DirectDeliverys
                                                        .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                                        && (n.GyosyaCD == tmp.GyosyaCD)
                                                        && (n.Mukesaki == Mukesaki.EF.GetStringValue()));

                                if (null != directDeliverysEF)
                                {
                                    tmp.NyukoRyo += (int)directDeliverysEF.Amount;
                                    tmp.Harai += (int)directDeliverysEF.Amount;
                                }
                            }
                            else
                            {
                                // 品目コード、向先をキーにして直送(EF)情報を取得
                                directDeliverysEF = SharedModel.Instance.DirectDeliverys
                                                        .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                                        && (n.Mukesaki == Mukesaki.EF.GetStringValue()));

                                if (null != directDeliverysEF)
                                {
                                    // 入庫払いは業者コードを指定しないため、
                                    // 取得した直送(EF)情報のサマリを入庫量/出庫量に設定
                                    var amount = (int)SharedModel.Instance.DirectDeliverys
                                        .Where(n => (n.ItemCode == tmp.HinmokuCD)
                                        && (n.Mukesaki == Mukesaki.EF.GetStringValue())).Select(x => x.Amount).Sum();
                                    tmp.NyukoRyo += amount;
                                    tmp.Harai += amount;
                                }
                            }
                            break;

                        case InventoryPrintTieSizaiCategory.LF:
                            OutWorkNoteItem directDeliverysLF = null;
                            if (isSiyoudakaBarai)
                            {
                                // 品目コード、業者コード、向先をキーにして直送(LF)情報を取得
                                directDeliverysLF = SharedModel.Instance.DirectDeliverys
                                                        .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                                        && (n.GyosyaCD == tmp.GyosyaCD)
                                                        && (n.Mukesaki == Mukesaki.LF.GetStringValue()));

                                if (null != directDeliverysLF)
                                {
                                    tmp.NyukoRyo += (int)directDeliverysLF.Amount;
                                    tmp.Harai += (int)directDeliverysLF.Amount;
                                }
                            }
                            else
                            {
                                // 品目コード、向先をキーにして直送(LF)情報を取得
                                directDeliverysLF = SharedModel.Instance.DirectDeliverys
                                                        .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                                        && (n.Mukesaki == Mukesaki.LF.GetStringValue()));

                                if (null != directDeliverysLF)
                                {
                                    // 入庫払いは業者コードを指定しないため、
                                    // 取得した直送(LF)情報のサマリを入庫量/出庫量に設定
                                    var amount = (int)SharedModel.Instance.DirectDeliverys
                                        .Where(n => (n.ItemCode == tmp.HinmokuCD)
                                        && (n.Mukesaki == Mukesaki.LF.GetStringValue())).Select(x => x.Amount).Sum();
                                    tmp.NyukoRyo += amount;
                                    tmp.Harai += amount;
                                }
                            }

                            break;

                        case InventoryPrintTieSizaiCategory.Chikuro:
                            OutWorkNoteItem directDeliverysChikuro = null;
                            if (isSiyoudakaBarai)
                            {
                                // 品目コード、業者コード、向先をキーにして直送(築炉)情報を取得
                                directDeliverysChikuro = SharedModel.Instance.DirectDeliverys
                                                        .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                                        && (n.GyosyaCD == tmp.GyosyaCD)
                                                        && (n.Mukesaki == Mukesaki.Building.GetStringValue()));

                                if (null != directDeliverysChikuro)
                                {
                                    tmp.NyukoRyo += (int)directDeliverysChikuro.Amount;
                                    tmp.Harai += (int)directDeliverysChikuro.Amount;
                                }
                            }
                            else
                            {
                                // 品目コード、向先をキーにして直送(LF)情報を取得
                                directDeliverysChikuro = SharedModel.Instance.DirectDeliverys
                                                        .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                                        && (n.Mukesaki == Mukesaki.Building.GetStringValue()));

                                if (null != directDeliverysChikuro)
                                {
                                    // 入庫払いは業者コードを指定しないため、
                                    // 取得した直送(築炉)情報のサマリを入庫量/出庫量に設定
                                    var amount = (int)SharedModel.Instance.DirectDeliverys
                                        .Where(n => (n.ItemCode == tmp.HinmokuCD)
                                        && (n.Mukesaki == Mukesaki.Building.GetStringValue())).Select(x => x.Amount).Sum();
                                    tmp.NyukoRyo += amount;
                                    tmp.Harai += amount;
                                }
                            }

                            break;

                        case InventoryPrintTieSizaiCategory.CC:
                            OutWorkNoteItem directDeliverysCC = null;
                            if (isSiyoudakaBarai)
                            {
                                // 品目コード、業者コード、向先をキーにして直送(築炉)情報を取得
                                directDeliverysCC = SharedModel.Instance.DirectDeliverys
                                                        .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                                        && (n.GyosyaCD == tmp.GyosyaCD)
                                                        && (n.Mukesaki == Mukesaki.CC.GetStringValue()));

                                if (null != directDeliverysCC)
                                {
                                    tmp.NyukoRyo += (int)directDeliverysCC.Amount;
                                    tmp.Harai += (int)directDeliverysCC.Amount;
                                }
                            }
                            else
                            {
                                // 品目コード、向先をキーにして直送(CC)情報を取得
                                directDeliverysCC = SharedModel.Instance.DirectDeliverys
                                                        .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                                        && (n.Mukesaki == Mukesaki.CC.GetStringValue()));

                                if (null != directDeliverysCC)
                                {
                                    // 入庫払いは業者コードを指定しないため、
                                    // 取得した直送(CC)情報のサマリを入庫量/出庫量に設定
                                    var amount = (int)SharedModel.Instance.DirectDeliverys
                                        .Where(n => (n.ItemCode == tmp.HinmokuCD)
                                        && (n.Mukesaki == Mukesaki.CC.GetStringValue())).Select(x => x.Amount).Sum();
                                    tmp.NyukoRyo += amount;
                                    tmp.Harai += amount;
                                }
                            }
 
                            break;

                        case InventoryPrintTieSizaiCategory.TD:
                            OutWorkNoteItem directDeliverysTD = null;
                            if (isSiyoudakaBarai)
                            {
                                // 品目コード、業者コード、向先をキーにして直送(TD)情報を取得
                                directDeliverysTD = SharedModel.Instance.DirectDeliverys
                                                        .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                                        && (n.GyosyaCD == tmp.GyosyaCD)
                                                        && (n.Mukesaki == Mukesaki.TD.GetStringValue()));

                                if (null != directDeliverysTD)
                                {
                                    tmp.NyukoRyo += (int)directDeliverysTD.Amount;
                                    tmp.Harai += (int)directDeliverysTD.Amount;
                                }
                            }
                            else
                            {
                                // 品目コード、向先をキーにして直送(TD)情報を取得
                                directDeliverysTD = SharedModel.Instance.DirectDeliverys
                                                        .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                                        && (n.Mukesaki == Mukesaki.TD.GetStringValue()));

                                if (null != directDeliverysTD)
                                {
                                    // 入庫払いは業者コードを指定しないため、
                                    // 取得した直送(TD)情報のサマリを入庫量/出庫量に設定
                                    var amount = (int)SharedModel.Instance.DirectDeliverys
                                        .Where(n => (n.ItemCode == tmp.HinmokuCD)
                                        && (n.Mukesaki == Mukesaki.TD.GetStringValue())).Select(x => x.Amount).Sum();
                                    tmp.NyukoRyo += amount;
                                    tmp.Harai += amount;
                                }
                            }
                            break;

                        case InventoryPrintTieSizaiCategory.ETC:
                            OutWorkNoteItem directDeliverysETC = null;
                            if (isSiyoudakaBarai)
                            {
                                // 品目コード、業者コード、向先をキーにして直送(その他)情報を取得
                                directDeliverysETC = SharedModel.Instance.DirectDeliverys
                                                        .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                                        && (n.GyosyaCD == tmp.GyosyaCD)
                                                        && (n.Mukesaki == Mukesaki.Others.GetStringValue()));

                                if (null != directDeliverysETC)
                                {
                                    tmp.NyukoRyo += (int)directDeliverysETC.Amount;
                                    tmp.Harai += (int)directDeliverysETC.Amount;
                                }
                            }
                            else
                            {
                                // 品目コード、向先をキーにして直送(その他)情報を取得
                                directDeliverysETC = SharedModel.Instance.DirectDeliverys
                                                        .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD)
                                                        && (n.Mukesaki == Mukesaki.Others.GetStringValue()));

                                if (null != directDeliverysETC)
                                {
                                    // 入庫払いは業者コードを指定しないため、
                                    // 取得した直送(その他)情報のサマリを入庫量/出庫量に設定
                                    var amount = (int)SharedModel.Instance.DirectDeliverys
                                        .Where(n => (n.ItemCode == tmp.HinmokuCD)
                                        && (n.Mukesaki == Mukesaki.Others.GetStringValue())).Select(x => x.Amount).Sum();
                                    tmp.NyukoRyo += amount;
                                    tmp.Harai += amount;
                                }
                            }
                            break;

                        default:
                            break;
                    }

                    // 返品情報を設定
                    WorkNoteItem returns = null;
                    if (isSiyoudakaBarai)
                    {
                        // 品目コード、業者コード、向先をキーにして返品情報を取得
                        returns = SharedModel.Instance.Returns
                                    .FirstOrDefault(n => (n.ItemCode == tmp.HinmokuCD) 
                                    && (n.GyosyaCD == tmp.GyosyaCD));

                        if (null != returns)
                        {
                            tmp.Henpin = (int)returns.Amount;
                        }
                    }
                    else
                    {
                        // 品目コードをキーにして返品情報を取得
                        returns = SharedModel.Instance.Returns
                                    .FirstOrDefault(n => n.ItemCode == tmp.HinmokuCD);

                        if (null != returns)
                        {
                            // 入庫払いは業者コードを指定しないため、
                            // 取得した返品報のサマリを入庫量 /出庫量に設定
                            tmp.Henpin = (int)SharedModel.Instance.Receivings.Where(n => n.ItemCode == tmp.HinmokuCD).Select(x => x.Amount).Sum();
                        }
                    }

                    if (dataList[selectSIzaiCategory].Count == 0)
                    {
                        dataList[selectSIzaiCategory].Insert(current, tmp);
                    }
                    else
                    {
                        dataList[selectSIzaiCategory].Insert(current + 1, tmp);
                    }

                }
                else if (editMode == EditMode.Modify) // 変更
                {
                    if (
                    (dataList[selectSIzaiCategory][current].HinmokuCD == HinmokuCD) &&
                    (dataList[selectSIzaiCategory][current].GyosyaCD == GyosyaCD) &&
                    (dataList[selectSIzaiCategory][current].HinmokuName == HinmokuName) &&
                    (dataList[selectSIzaiCategory][current].GyosyaName == GyosyaName) &&
                    (dataList[selectSIzaiCategory][current].Bikou == Bikou)
                    )
                    {
                        // 値に変更がないためデータの変更はしない
                        clear();

                        updateEditMode();

                        return;
                    }

                    // 変更前の品目CD、業者CD、品目名、業者名、備考を保持する
                    beforeHinmokuCD = dataList[selectSIzaiCategory][current].HinmokuCD;
                    beforeGyosyaCD = dataList[selectSIzaiCategory][current].GyosyaCD;
                    beforeHinmokuName = dataList[selectSIzaiCategory][current].HinmokuName;
                    beforeGyosyaName = dataList[selectSIzaiCategory][current].GyosyaName;
                    beforeBikou = dataList[selectSIzaiCategory][current].Bikou;
                    afterHinmokuName = HinmokuName;
                    afterGyosyaName = GyosyaName;
                    afterBikou = Bikou;

                    dataList[selectSIzaiCategory][current].HinmokuCD = HinmokuCD;
                    dataList[selectSIzaiCategory][current].GyosyaCD = GyosyaCD;
                    dataList[selectSIzaiCategory][current].HinmokuName = HinmokuName;
                    dataList[selectSIzaiCategory][current].GyosyaName = GyosyaName;
                    dataList[selectSIzaiCategory][current].Bikou = Bikou;

                }

                updateTarget();

                editList[(int)selectSIzaiCategory] = true;

                clear();

                IsShowEditForm = false;

                updateEditMode();

            }

            catch (Exception e)
            {
                hasException = true;
                errorContent = e.Message;
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

            finally
            {
                if (!hasInputError)
                {
                    LogType logType = hasException ? LogType.Error : LogType.Normal;
                    switch (editMode)
                    {
                        case EditMode.Add:
                            // 棚卸ログの挿入
                            await InsertTanaorosiLog(logType, SizaiWorkCategory.TanaoroshiWork,
                                                     LogOperationType.PrintInventorySheet, LogOperationContent.Add, null, errorContent, errorCode);
                            // 棚卸詳細ログの挿入
                            await InsertTanaorosiDetailLog(logType,
                                                           SizaiWorkCategory.TanaoroshiWork,
                                                           LogOperationType.PrintInventorySheet,
                                                           LogOperationContent.Add,
                                                           (SizaiCategory)(selectSIzaiCategory + 1),
                                                           data.HinmokuCD,
                                                           data.GyosyaCD,
                                                           data.HinmokuName,
                                                           data.GyosyaName,
                                                           data.Bikou,
                                                           null,null,null,
                                                           errorContent
                                                           );
                            break;

                        case EditMode.Modify:
                            // 棚卸ログの挿入
                            await InsertTanaorosiLog(logType, SizaiWorkCategory.TanaoroshiWork,
                                                     LogOperationType.PrintInventorySheet, LogOperationContent.Change, null, errorContent, errorCode);
                            // 棚卸詳細ログの挿入
                            if (HinmokuCD != null)
                            {
                                await InsertTanaorosiDetailLog(logType,
                                                               SizaiWorkCategory.TanaoroshiWork,
                                                               LogOperationType.PrintInventorySheet,
                                                               LogOperationContent.Change,
                                                               (SizaiCategory)(selectSIzaiCategory + 1),
                                                               beforeHinmokuCD.PadRight(4),
                                                               beforeGyosyaCD.PadRight(4),
                                                               CommonUtility.PadRightSJIS(beforeHinmokuName, 40),
                                                               CommonUtility.PadRightSJIS(beforeGyosyaName, 24),
                                                               CommonUtility.PadRightSJIS(beforeBikou, 40),
                                                               Constants.HinmokuName_StringDefine + afterHinmokuName,
                                                               Constants.GyosyaName_StringDefine+ afterGyosyaName,
                                                               Constants.Bikou_StringDefine+ afterBikou,
                                                               errorContent
                                                               );
                            }
                            break;

                        default:
                            break;
                    }
                }
            }

        });

        #endregion

    }
}