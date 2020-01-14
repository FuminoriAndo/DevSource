using CKSI1010.Common;
using CKSI1010.Core;
using CKSI1010.Operation.Common.ViewModel;
using CKSI1010.Shared;
using CKSI1010.Shared.Model;
using CKSI1010.Shared.ViewModel;
using CKSI1010.Types;
using Common;
using GalaSoft.MvvmLight.CommandWpf;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.XPath;
using static CKSI1010.Common.Constants;
using static Common.MessageManager;
//*************************************************************************************
//
//   棚卸実績値入力画面の基底ビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.InputInventoryActual.ViewModel
{
    /// <summary>
    /// 棚卸実績値入力画面の基底ビューモデル
    /// </summary>
    public abstract class InputInventoryActualBaseViewModel : OperationViewModelBase
    {
        #region フィールド

        /// <summary>
        /// エラーカウント
        /// </summary>
        private long errorCount = 0;

        /// <summary>
        /// 確定状態か
        /// </summary>
        private bool isFixed = false;

        /// <summary>
        /// SK一覧
        /// </summary>
        protected ObservableCollection<InputInventoryActualRecordViewModel> skList
            = new ObservableCollection<InputInventoryActualRecordViewModel>();

        /// <summary>
        /// EF一覧
        /// </summary>
        protected ObservableCollection<InputInventoryActualRecordViewModel> efList
            = new ObservableCollection<InputInventoryActualRecordViewModel>();

        /// <summary>
        /// LF一覧
        /// </summary>
        protected ObservableCollection<InputInventoryActualRecordViewModel> lfList
            = new ObservableCollection<InputInventoryActualRecordViewModel>();

        /// <summary>
        /// 築炉一覧
        /// </summary>
        protected ObservableCollection<InputInventoryActualRecordViewModel> buildingList
            = new ObservableCollection<InputInventoryActualRecordViewModel>();

        /// <summary>
        /// LD一覧
        /// </summary>
        protected ObservableCollection<InputInventoryActualRecordViewModel> ldList
            = new ObservableCollection<InputInventoryActualRecordViewModel>();

        /// <summary>
        /// TD一覧
        /// </summary>
        protected ObservableCollection<InputInventoryActualRecordViewModel> tdList
            = new ObservableCollection<InputInventoryActualRecordViewModel>();

        /// <summary>
        /// CC一覧
        /// </summary>
        protected ObservableCollection<InputInventoryActualRecordViewModel> ccList
            = new ObservableCollection<InputInventoryActualRecordViewModel>();

        /// <summary>
        /// その他一覧
        /// </summary>
        protected ObservableCollection<InputInventoryActualRecordViewModel> othersList
             = new ObservableCollection<InputInventoryActualRecordViewModel>();

        /// <summary>
        /// 選択されている資材区分
        /// </summary>
        private ShizaiKbnTypes selectShizaiKbn;

        /// <summary>
        /// 資材区分と置場区分の紐付けリスト
        /// </summary>
        protected readonly List<SizaiCategoryTiePlaceDefinition> SizaiCategoryTiePlaceList = null;

        /// <summary>
        /// SKを表示しているか
        /// </summary>
        private bool showSK = false;

        /// <summary>
        /// EFを表示しているか
        /// </summary>
        private bool showEF = false;

        /// <summary>
        /// LFを表示しているか
        /// </summary>
        private bool showLF = false;

        /// <summary>
        /// 築炉を表示しているか
        /// </summary>
        private bool showBuilding = false;

        /// <summary>
        /// LDを表示しているか
        /// </summary>
        private bool showLD = false;

        /// <summary>
        /// TDを表示しているか
        /// </summary>
        private bool showTD = false;

        /// <summary>
        /// CCを表示しているか
        /// </summary>
        private bool showCC = false;

        /// <summary>
        /// その他を表示しているか
        /// </summary>
        private bool showOthers = false;

        /// <summary>
        /// 選択されているインデックス
        /// </summary>
        private int selectIndex = -1;

        /// <summary>
        /// 選択されているインデックス(SK)
        /// </summary>
        private int selectSKIndex = -1;

        /// <summary>
        /// 選択されているインデックス(LF)
        /// </summary>
        private int selectLFIndex = -1;

        /// <summary>
        /// 選択されているインデックス(EF)
        /// </summary>
        private int selectEFIndex = -1;

        /// <summary>
        /// 選択されているインデックス(築炉)
        /// </summary>
        private int selectBuildingIndex = -1;

        /// <summary>
        /// 選択されているインデックス(LD)
        /// </summary>
        private int selectLDIndex = -1;

        /// <summary>
        /// 選択されているインデックス(TD)
        /// </summary>
        private int selectTDIndex = -1;

        /// <summary>
        /// 選択されているインデックス(CC)
        /// </summary>
        private int selectCCIndex = -1;

        /// <summary>
        /// 選択されているインデックス(その他)
        /// </summary>
        private int selectOthersIndex = -1;

        /// <summary>
        /// 選択レコード
        /// </summary>
        private InputInventoryActualRecordViewModel selectItem = null;

        /// <summary>
        /// 棚卸ワークデータを資材棚卸マスタから取得したかどうか
        /// </summary>
        private bool isInventoryWorkFromMst = false;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputInventoryActualBaseViewModel(IDataService dataService) : base(dataService)
        {
            ShowSK = true;
            ShowEF = false;
            ShowLF = false;
            ShowBuilding = false;
            ShowLD = false;
            ShowTD = false;
            ShowCC = false;
            ShowOthers = false;

            SelectShizaiKbn = ShizaiKbnTypes.SK;
            SelectIndex = -1;

            WorkKbn = SharedModel.Instance.WorkKbn;
            InventoryWork = new List<InventoryWork>();
            SizaiCategoryTiePlaceList = new List<SizaiCategoryTiePlaceDefinition>();

            // 資材区分と置場区分の紐付けリストの作成
            createSizaiCategoryTiePlaceList();
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 実施年月
        /// </summary>
        public OperationYearMonth OperationYearMonth => SharedViewModel.Instance.OperationYearMonth;

        /// <summary>
        /// 作業区分
        /// </summary>
        protected SizaiWorkCategory WorkKbn { get; set; }

        /// <summary>
        /// 棚卸ワークデータ
        /// </summary>
        protected List<InventoryWork> InventoryWork { get; set; } = null;

        /// <summary>
        /// 確定状態か
        /// </summary>
        public bool IsFixed
        {
            get { return isFixed; }
            set { Set(ref isFixed, value); }
        }

        /// <summary>
        /// SK一覧
        /// </summary>
        public ObservableCollection<InputInventoryActualRecordViewModel> SkList
        {
            get
            {
                return skList;
            }
        }

        /// <summary>
        /// EF一覧
        /// </summary>
        public ObservableCollection<InputInventoryActualRecordViewModel> EfList
        {
            get
            {
                return efList;
            }
        }

        /// <summary>
        /// LF一覧
        /// </summary>
        public ObservableCollection<InputInventoryActualRecordViewModel> LfList
        {
            get
            {
                return lfList;
            }
        }

        /// <summary>
        /// 築炉一覧
        /// </summary>
        public ObservableCollection<InputInventoryActualRecordViewModel> BuildingList
        {
            get
            {
                return buildingList;
            }
        }

        /// <summary>
        /// LD一覧
        /// </summary>
        public ObservableCollection<InputInventoryActualRecordViewModel> LdList
        {
            get
            {
                return ldList;
            }
        }

        /// <summary>
        /// TD一覧
        /// </summary>
        public ObservableCollection<InputInventoryActualRecordViewModel> TdList
        {
            get
            {
                return tdList;
            }
        }

        /// <summary>
        /// CC一覧
        /// </summary>
        public ObservableCollection<InputInventoryActualRecordViewModel> CcList
        {
            get
            {
                return ccList;
            }
        }

        /// <summary>
        /// その他一覧
        /// </summary>
        public ObservableCollection<InputInventoryActualRecordViewModel> OthersList
        {
            get
            {
                return othersList;
            }
        }

        /// <summary>
        /// 選択資材区分
        /// </summary>
        public ShizaiKbnTypes SelectShizaiKbn
        {
            get { return selectShizaiKbn; }
            set { Set(ref selectShizaiKbn, value); }
        }

        /// <summary>
        /// SKを表示しているか
        /// </summary>
        public bool ShowSK
        {
            get { return showSK; }
            set { Set(ref showSK, value); }
        }

        /// <summary>
        /// EFを表示しているか
        /// </summary>
        public bool ShowEF
        {
            get { return showEF; }
            set { Set(ref showEF, value); }
        }

        /// <summary>
        /// LFを表示しているか
        /// </summary>
        public bool ShowLF
        {
            get { return showLF; }
            set { Set(ref showLF, value); }
        }

        /// <summary>
        /// 築炉を表示しているか
        /// </summary>
        public bool ShowBuilding
        {
            get { return showBuilding; }
            set { Set(ref showBuilding, value); }
        }

        /// <summary>
        /// LDを表示しているか
        /// </summary>
        public bool ShowLD
        {
            get { return showLD; }
            set { Set(ref showLD, value); }
        }

        /// <summary>
        /// TDを表示しているか
        /// </summary>
        public bool ShowTD
        {
            get { return showTD; }
            set { Set(ref showTD, value); }
        }

        /// <summary>
        /// CCを表示しているか
        /// </summary>
        public bool ShowCC
        {
            get { return showCC; }
            set { Set(ref showCC, value); }
        }

        /// <summary>
        /// その他を表示しているか
        /// </summary>
        public bool ShowOthers
        {
            get { return showOthers; }
            set { Set(ref showOthers, value); }
        }

        /// <summary>
        /// 選択項目Index
        /// </summary>
        public int SelectIndex
        {
            get { return selectIndex; }
            set { Set(ref selectIndex, value); }
        }

        /// <summary>
        /// 選択項目
        /// </summary>
        public InputInventoryActualRecordViewModel SelectItem
        {
            get { return selectItem; }
            set { Set(ref selectItem, value); }
        }

        #endregion

        #region コマンド

        /// <summary>
        /// SK選択
        /// </summary>
        public ICommand FireSelectSK => new RelayCommand(() =>
        {
            try
            {
                refreshList(SkList);

                ShowSK = true;
                ShowEF = false;
                ShowLF = false;
                ShowBuilding = false;
                ShowLD = false;
                ShowTD = false;
                ShowCC = false;
                ShowOthers = false;

                memorySelectIndex(SelectShizaiKbn);
                SelectShizaiKbn = ShizaiKbnTypes.SK;
                SelectIndex = selectSKIndex;
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

        });

        /// <summary>
        /// EF選択
        /// </summary>
        public ICommand FireSelectEF => new RelayCommand(() =>
        {
            try
            {
                refreshList(EfList);

                ShowSK = false;
                ShowEF = true;
                ShowLF = false;
                ShowBuilding = false;
                ShowLD = false;
                ShowTD = false;
                ShowCC = false;
                ShowOthers = false;

                memorySelectIndex(SelectShizaiKbn);
                SelectShizaiKbn = ShizaiKbnTypes.EF;
                SelectIndex = selectEFIndex;
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

        });

        /// <summary>
        /// LF選択
        /// </summary>
        public ICommand FireSelectLF => new RelayCommand(() =>
        {
            try
            {
                refreshList(LfList);

                ShowSK = false;
                ShowEF = false;
                ShowLF = true;
                ShowBuilding = false;
                ShowLD = false;
                ShowTD = false;
                ShowCC = false;
                ShowOthers = false;

                memorySelectIndex(SelectShizaiKbn);
                SelectShizaiKbn = ShizaiKbnTypes.LF;
                SelectIndex = selectLFIndex;
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

        });

        /// <summary>
        /// Building選択
        /// </summary>
        public ICommand FireSelectBuilding => new RelayCommand(() =>
        {
            try
            {
                refreshList(BuildingList);

                ShowSK = false;
                ShowEF = false;
                ShowLF = false;
                ShowBuilding = true;
                ShowLD = false;
                ShowTD = false;
                ShowCC = false;
                ShowOthers = false;

                memorySelectIndex(SelectShizaiKbn);
                SelectShizaiKbn = ShizaiKbnTypes.Building;
                SelectIndex = selectBuildingIndex;
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

        });

        /// <summary>
        /// LD選択
        /// </summary>
        public ICommand FireSelectLD => new RelayCommand(() =>
        {
            try
            {
                refreshList(LdList);

                ShowSK = false;
                ShowEF = false;
                ShowLF = false;
                ShowBuilding = false;
                ShowLD = true;
                ShowTD = false;
                ShowCC = false;
                ShowOthers = false;

                memorySelectIndex(SelectShizaiKbn);
                SelectShizaiKbn = ShizaiKbnTypes.LD;
                SelectIndex = selectLDIndex;
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

        });

        /// <summary>
        /// TD選択
        /// </summary>
        public ICommand FireSelectTD => new RelayCommand(() =>
        {
            try
            {
                refreshList(TdList);

                ShowSK = false;
                ShowEF = false;
                ShowLF = false;
                ShowBuilding = false;
                ShowLD = false;
                ShowTD = true;
                ShowCC = false;
                ShowOthers = false;

                memorySelectIndex(SelectShizaiKbn);
                SelectShizaiKbn = ShizaiKbnTypes.TD;
                SelectIndex = selectTDIndex;
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

        });

        /// <summary>
        /// CC選択
        /// </summary>
        public ICommand FireSelectCC => new RelayCommand(() =>
        {
            try
            {
                refreshList(CcList);

                ShowSK = false;
                ShowEF = false;
                ShowLF = false;
                ShowBuilding = false;
                ShowLD = false;
                ShowTD = false;
                ShowCC = true;
                ShowOthers = false;

                memorySelectIndex(SelectShizaiKbn);
                SelectShizaiKbn = ShizaiKbnTypes.CC;
                SelectIndex = selectCCIndex;
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

        });

        /// <summary>
        /// その他選択
        /// </summary>
        public ICommand FireSelectOthers => new RelayCommand(() =>
        {
            try
            {
                refreshList(OthersList);

                ShowSK = false;
                ShowEF = false;
                ShowLF = false;
                ShowBuilding = false;
                ShowLD = false;
                ShowTD = false;
                ShowCC = false;
                ShowOthers = true;

                memorySelectIndex(SelectShizaiKbn);
                SelectShizaiKbn = ShizaiKbnTypes.Others;
                SelectIndex = selectOthersIndex;
            }

            catch (Exception)
            {
                throw;
            }
        });

        /// <summary>
        /// 保存ボタン押下
        /// </summary>
        public ICommand FireSave => new RelayCommand(async () =>
        {
            bool hasException = false;
            string errorContent = null;
            string errorCode = null;
            string sql = null;
            string sqlParam = null;
            string timestamp = null;

            try
            {
                BusyStatus.IsBusy = true;
                await saveInventoriesWorkAsync();
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
            }

            finally
            {
                BusyStatus.IsBusy = false;

                if (!hasException)
                {
                    MessageManager.ShowInformation(SystemID.CKSI1010, "SaveCompleted");
                }
                else
                {
                    MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                }

                // 棚卸ログの挿入
                LogType logType = hasException ? LogType.Error : LogType.Normal;
                await InsertTanaorosiLog(logType, WorkKbn, LogOperationType.InputInventoryActual,
                                         LogOperationContent.Save, null, errorContent, errorCode);
            }
        });

        #endregion

        #region 抽象メソッド

        /// <summary>
        /// 棚卸ワークデータの取得
        /// </summary>
        /// <returns>棚卸ワークデータ</returns>
        protected abstract Task<IEnumerable<InventoryWork>> GetInventoryWork();

        /// <summary>
        /// 棚卸ワークデータの取得(棚卸マスタのデータをもとにする)
        /// </summary>
        /// <returns>棚卸ワークデータ</returns>

        protected abstract Task<IEnumerable<InventoryWork>> GetInventoryWorkFromMst();

        /// <summary>
        /// 単価入力チェック
        /// </summary>
        protected abstract Task<bool> CheckTankaInput();

        #endregion

        #region メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns>非同期タスク</returns>
        internal override async Task InitializeAsync()
        {
            errorCount = 0;

            try
            {
                BusyStatus.IsBusy = true;

                IsFixed = CommonUtility.IsCurrentOperationFixed();

                // 棚卸ワークデータの初期化
                InventoryWork.Clear();

                // 棚卸ワークデータの取得
                InventoryWork.AddRange(await GetInventoryWork());

                if (InventoryWork.Count == 0 && WorkKbn.Equals(SizaiWorkCategory.KensinWork))
                {
                    // 棚卸ワークデータの取得(棚卸マスタのデータをもとにする)
                    InventoryWork.AddRange(await GetInventoryWorkFromMst());
                    isInventoryWorkFromMst = true;
                }

                // SK
                setList(SkList, InventoryWork.Where
                    (item => (item.ShizaiKbn.Trim() == ShizaiKbnTypes.SK.GetStringValue())).ToList(), 
                    ref errorCount);
                // EF
                setList(EfList, InventoryWork.Where
                    (item => (item.ShizaiKbn.Trim() == ShizaiKbnTypes.EF.GetStringValue())).ToList(), 
                    ref errorCount);
                // LF
                setList(LfList, InventoryWork.Where
                    (item => (item.ShizaiKbn.Trim() == ShizaiKbnTypes.LF.GetStringValue())).ToList(), 
                    ref errorCount);
                // 築炉
                setList(BuildingList, InventoryWork.Where
                    (item => (item.ShizaiKbn.Trim() == ShizaiKbnTypes.Building.GetStringValue())).ToList(), 
                    ref errorCount);
                // LD
                setList(LdList, InventoryWork.Where
                    (item => (item.ShizaiKbn.Trim() == ShizaiKbnTypes.LD.GetStringValue())).ToList(), 
                    ref errorCount);
                // TD
                setList(TdList, InventoryWork.Where
                    (item => (item.ShizaiKbn.Trim() == ShizaiKbnTypes.TD.GetStringValue())).ToList(), 
                    ref errorCount);
                // CC
                setList(CcList, InventoryWork.Where
                    (item => (item.ShizaiKbn.Trim() == ShizaiKbnTypes.CC.GetStringValue())).ToList(), 
                    ref errorCount);
                // その他
                setList(OthersList, InventoryWork.Where
                    (item => (item.ShizaiKbn.Trim() == ShizaiKbnTypes.Others.GetStringValue())).ToList(), 
                    ref errorCount);

                isInventoryWorkFromMst = false;
                await base.InitializeAsync();

                // 当月量(予想)にマイナスがある場合、メッセージを表示する。
                if (errorCount > 0)
                {
                    MessageManager.ShowExclamation(SystemID.CKSI1010, "HasMinusTogetuYosouAmount");
                }
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
        /// 確定
        /// </summary>
        internal override async Task<bool> CommitAsync()
        {
            bool hasException = false;
            bool hasError = false;
            string errorContent = null;
            string errorCode = null;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;

            try
            {
                BusyStatus.IsBusy = true;

                if (!CheckValue())
                {
                    hasError = true;
                    return false;
                }

                updateValue(SkList);
                updateValue(EfList);
                updateValue(LfList);
                updateValue(BuildingList);
                updateValue(LdList);
                updateValue(TdList);
                updateValue(CcList);
                updateValue(OthersList);

                await DataService.UpdateInventoriesWorkAsync(InventoryWork);

                // 操作状況の更新
                await UpdateOperationCondition(OperationCondition.Fix);
                IsFixed = true;

                return await base.CommitAsync();
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

                BusyStatus.IsBusy = false;

                if (!hasException)
                {
                    if (!hasError)
                    {
                        MessageManager.ShowInformation(SystemID.CKSI1010, "CommitCompleted");
                    }
                }
                else
                {
                    MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                }

                // 棚卸ログの挿入
                LogType logType = hasException ? LogType.Error : LogType.Normal;
                await InsertTanaorosiLog(logType, WorkKbn, LogOperationType.InputInventoryActual,
                                         LogOperationContent.Fix, null, errorContent, errorCode);
            }

        }

        /// <summary>
        /// 修正
        /// </summary>
        internal override async Task<bool> ModifyAsync()
        {
            string errorContent = null;
            string errorCode = null;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;
            bool hasException = false;

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

                BusyStatus.IsBusy = false;

                // 棚卸ログの挿入
                LogType logType = hasException ? LogType.Error : LogType.Normal;
                await InsertTanaorosiLog(logType, WorkKbn,
                                         LogOperationType.InputInventoryActual,
                                         LogOperationContent.Modify, null, errorContent, errorCode);
            }
        }

        /// <summary>
        /// 次へ進む
        /// </summary>
        internal override async Task<bool> GoNextAsync()
        {
            bool hasException = false;
            string errorContent = null;
            string errorCode = null;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;
            bool hasTankaError = false;

            if (!IsFixed)
            {
                MessageManager.ShowExclamation(SystemID.CKSI1010, "CanNotGoNext");
                return false;
            }

            else
            {
                try
                {
                    BusyStatus.IsBusy = true;

                    // 単価入力チェック
                    bool ret = await CheckTankaInput();

                    if (!ret)
                    {
                        // 単価未入力の品目が存在する旨のメッセージを表示する
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "NotInputHinmokuTanka",
                            CommonUtility.GetSizaiTanaorosiExcelBookPath());
                        hasTankaError = true;
                        return false;
                    }

                    else
                    {
                        return await base.GoNextAsync();
                    }

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
                    BusyStatus.IsBusy = false;

                    if (IsFixed)
                    {
                        if (hasTankaError)
                        {
                            // 棚卸ログの挿入
                            await InsertTanaorosiLog(LogType.Normal, WorkKbn,
                                                     LogOperationType.InputInventoryActual,
                                                     LogOperationContent.Next, null, errorContent, errorCode);
                        }
                    }

                    if (hasException)
                    {
                        // 棚卸ログの挿入
                        await InsertTanaorosiLog(LogType.Error, WorkKbn,
                                                 LogOperationType.InputInventoryActual,
                                                 LogOperationContent.Next, null, errorContent, errorCode);
                    }
                }
            }
        }

        /// <summary>
        /// 前へ
        /// </summary>
        internal override async Task<bool> BackPreviousAsync()
        {
            return await base.BackPreviousAsync();
        }

        /// <summary>
        /// 棚卸操作選択の発火
        /// </summary>
        internal override async Task<bool> SelecOperationAsync()
        {
            return await base.SelecOperationAsync();
        }

        /// <summary>
        /// 資材区分と置場区分の紐付けリストの作成
        /// </summary>
        private void createSizaiCategoryTiePlaceList()
        {
            // 定義ファイルのオープン
            var settings = Path.Combine
                (Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");
            var doc = XDocument.Load(settings);

            List<long> shizaiKbnList = new List<long> {(long)ShizaiKbnTypes.SK,
                (long)ShizaiKbnTypes.EF,(long)ShizaiKbnTypes.LF,
                (long)ShizaiKbnTypes.Building,(long)ShizaiKbnTypes.LD,
                (long)ShizaiKbnTypes.TD,(long)ShizaiKbnTypes.CC,
                (long)ShizaiKbnTypes.Others};

            foreach (long shizaiKbn in shizaiKbnList)
            {
                var define = doc.XPathSelectElement
                    ($"/Settings/SizaiKbnTiePlaceDefinitions/SizaiKbnTiePlaceDefinition[@SizaiKbn={shizaiKbn.ToString()}]");

                if (define != null)
                {
                    SizaiCategoryTiePlaceList.Add(new SizaiCategoryTiePlaceDefinition()
                    {
                        ShizaiKbnTypes = (ShizaiKbnTypes)shizaiKbn,
                        ShizaiKbnType = define.Attribute("SizaiKbn").Value,
                        PlaceCode = define.Attribute("PlaceCode").Value
                    });
                }
            }
        }

        /// <summary>
        /// データ保存
        /// </summary>
        private async Task<int> saveInventoriesWorkAsync()
        {
            updateValue(SkList);
            updateValue(EfList);
            updateValue(LfList);
            updateValue(BuildingList);
            updateValue(LdList);
            updateValue(TdList);
            updateValue(CcList);
            updateValue(OthersList);

            return await DataService.UpdateInventoriesWorkAsync(InventoryWork);
        }

        /// <summary>
        /// 画面に表示する棚卸実績データの設定
        /// </summary>
        /// <param name="viewList">画面のリスト</param>
        /// <param name="workList">棚卸ワークデータリスト</param>
        private void setList(ObservableCollection<InputInventoryActualRecordViewModel> viewList, 
                                List<InventoryWork> workList, ref long errorCount)
        {
            try
            {
                viewList.Clear();
                workList.Sort((itemA, itemB) => itemA.ItemOrder.CompareTo(itemB.ItemOrder));

                foreach (InventoryWork item in workList)
                {
                    ShizaiKbnTypes type = (ShizaiKbnTypes)long.Parse(item.ShizaiKbn);
                    SizaiCategoryTiePlaceDefinition defItem 
                        = SizaiCategoryTiePlaceList.Find(x => (x.ShizaiKbnTypes == type));

                    if (!isInventoryWorkFromMst)
                    {
                        viewList.Add(new InputInventoryActualRecordViewModel()
                        {
                            SIZAI_KBN = item.ShizaiKbn,
                            WORK_KBN = item.WorkKbn,
                            ITEM_ORDER = item.ItemOrder,
                            HINMOKUCD = item.ItemCode,
                            GYOSYACD = item.SupplierCode,
                            ShizaiKbn = type,
                            WorkKbn = item.WorkKbn,
                            Order = item.ItemOrder,
                            HinmokuCd = item.ItemCode,
                            GyosyaCd = item.SupplierCode,
                            HinmokuName = item.ItemName,
                            GyosyaName = item.SupplierName,
                            TogetuValueToNumber = item.CurrentValue,
                            NyukoValue = item.InputValue,
                            HaraiValue = item.OutputValue,
                            HenpinValue = item.ReturnValue,
                            SoukoValue = item.StockInWarehouse,
                            EfValue = item.StockEF,
                            LfValue = item.StockLF,
                            CcValue = item.StockCC,
                            OthersValue = item.StockOthers,
                            MeterValue = item.StockMeter,
                            Yobi1Value = item.StockYobi1,
                            Yobi2Value = item.StockYobi2,
                            PlaceCode = (defItem != null) ? defItem.PlaceCode : string.Empty,
                            IsError = false,
                            IsWarning = false,
                            IsNoProblemData = false
                        });
                    }

                    else
                    {
                        long soukoValue = 0;
                        long efValue = 0;
                        long lfValue = 0;
                        long ccValue = 0;
                        long other = 0;
                        long meter = 0;
                        long reserve1 = 0;
                        long reserve2 = 0;
                        long lastMonthValue = 0;
                        long nyuko = 0;
                        long harai = 0;
                        long henpin = 0;

                        // 品目コードの受払い種別が使用高払いかどうか
                        var isSiyoudakaBarai
                            = DataService.GetUkebaraiSyubetu(item.ItemCode) == UkebaraiType.SiyoudakaBarai ? true : false;
                        // 前月在庫を設定
                        Inventory inventory = null;
                        if (isSiyoudakaBarai)
                        {
                            // 品目コード、業者コードをキーにして先月棚卸データを取得
                            inventory = SharedModel.Instance.LastInventories
                                            .FirstOrDefault(itemTie => (itemTie.ItemCode == item.ItemCode) && (itemTie.SupplierCode == item.SupplierCode));
                        }
                        else
                        {
                            // 品目コードをキーにして先月棚卸データを取得
                            inventory = SharedModel.Instance.LastInventories
                                           .FirstOrDefault(itemTie => itemTie.ItemCode == item.ItemCode);
                        }

                        if (null != inventory)
                        {
                            soukoValue = (int)inventory.StockInWarehouse;
                            efValue = (int)inventory.StockEF;
                            lfValue = (int)inventory.StockLF;
                            ccValue = (int)inventory.StockCC;
                            other = (int)inventory.StockOther;
                            meter = (int)inventory.StockMeter;
                            reserve1 = (int)inventory.StockReserve1;
                            reserve2 = (int)inventory.StockReserve2;
                            lastMonthValue 
                                = ((int)inventory.StockInWarehouse 
                                + (int)inventory.StockEF 
                                + (int)inventory.StockLF 
                                + (int)inventory.StockCC);
                        }

                        // 入庫情報を設定
                        WorkNoteItem receiving = null;
                        if (isSiyoudakaBarai)
                        {
                            // 品目コード、業者コードをキーにして入庫情報を取得
                            receiving = SharedModel.Instance.Receivings
                                            .FirstOrDefault(itemTie => (itemTie.ItemCode == item.ItemCode) 
                                            && (itemTie.GyosyaCD == item.SupplierCode));

                            if (null != receiving)
                            {
                                nyuko = (int)receiving.Amount;
                            }
                        }
                        else
                        {
                            // 品目コードをキーにして入庫情報を取得
                            receiving = SharedModel.Instance.Receivings
                                            .FirstOrDefault(itemTie => itemTie.ItemCode == item.ItemCode);

                            if (null != receiving)
                            {
                                // 入庫払いは業者コードを指定しないため、
                                // 取得した入庫情報のサマリを入庫量に設定
                                nyuko = (int)SharedModel.Instance.Receivings.Where(n => n.ItemCode == item.ItemCode).Select(x => x.Amount).Sum();
                            }
                        }

                        // 出庫情報を設定
                        switch ((ShizaiKbnTypes)int.Parse(item.ShizaiKbn))
                        {
                            case ShizaiKbnTypes.SK:
                                var leaveingSK 
                                    = SharedModel.Instance.Leavings
                                        .FirstOrDefault(n => n.ItemCode == item.ItemCode);
                                if (null != leaveingSK)
                                {
                                    harai += (int)leaveingSK.Amount;
                                }
                                break;

                            case ShizaiKbnTypes.EF:
                                var leaveingEF 
                                    = SharedModel.Instance.Leavings
                                        .FirstOrDefault(n => (n.ItemCode == item.ItemCode) 
                                            && (n.Mukesaki == Mukesaki.EF.GetStringValue()));
                                if (null != leaveingEF)
                                {
                                    harai += (int)leaveingEF.Amount;
                                }
                                break;

                            case ShizaiKbnTypes.LF:
                                var leaveingLF 
                                    = SharedModel.Instance.Leavings
                                        .FirstOrDefault(n => (n.ItemCode == item.ItemCode) 
                                            && (n.Mukesaki == Mukesaki.LF.GetStringValue()));
                                if (null != leaveingLF)
                                {
                                    harai += (int)leaveingLF.Amount;
                                }
                                break;

                            case ShizaiKbnTypes.Building:
                                var leaveingBuilding 
                                    = SharedModel.Instance.Leavings
                                        .FirstOrDefault(n => (n.ItemCode == item.ItemCode) 
                                            && (n.Mukesaki == Mukesaki.Building.GetStringValue()));
                                if (null != leaveingBuilding)
                                {
                                    harai += (int)leaveingBuilding.Amount;
                                }
                                break;

                            case ShizaiKbnTypes.LD:
                                var leaveingLD 
                                    = SharedModel.Instance.Leavings
                                        .FirstOrDefault(n => (n.ItemCode == item.ItemCode) 
                                            && (n.Mukesaki == Mukesaki.LD.GetStringValue()));
                                if (null != leaveingLD)
                                {
                                    harai += (int)leaveingLD.Amount;
                                }
                                break;

                            case ShizaiKbnTypes.CC:
                                var leaveingCC 
                                    = SharedModel.Instance.Leavings
                                        .FirstOrDefault(n => (n.ItemCode == item.ItemCode) 
                                            && (n.Mukesaki == Mukesaki.CC.GetStringValue()));
                                if (null != leaveingCC)
                                {
                                    harai += (int)leaveingCC.Amount;
                                }
                                break;

                            case ShizaiKbnTypes.TD:
                                var leaveingTD 
                                    = SharedModel.Instance.Leavings
                                        .FirstOrDefault(n => (n.ItemCode == item.ItemCode) 
                                            && (n.Mukesaki == Mukesaki.TD.GetStringValue()));
                                if (null != leaveingTD)
                                {
                                    harai += (int)leaveingTD.Amount;
                                }
                                break;

                            case ShizaiKbnTypes.Others:
                                var leaveingETC 
                                    = SharedModel.Instance.Leavings
                                        .FirstOrDefault(n => (n.ItemCode == item.ItemCode) 
                                            && (n.Mukesaki == Mukesaki.Others.GetStringValue()));
                                if (null != leaveingETC)
                                {
                                    harai += (int)leaveingETC.Amount;
                                }
                                break;

                            default:
                                break;
                        }

                        // 直送情報を設定
                        switch ((ShizaiKbnTypes)int.Parse(item.ShizaiKbn))
                        {
                            case ShizaiKbnTypes.SK:
                                OutWorkNoteItem directDeliverysSK = null;
                                if (isSiyoudakaBarai)
                                {
                                    // 品目コード、業者コードをキーにして直送(SK)情報を取得
                                    directDeliverysSK = SharedModel.Instance.DirectDeliverys
                                                            .FirstOrDefault(n => (n.ItemCode == item.ItemCode)
                                                            && (n.GyosyaCD == item.SupplierCode));

                                    if (null != directDeliverysSK)
                                    {
                                        nyuko += (int)directDeliverysSK.Amount;
                                        harai += (int)directDeliverysSK.Amount;
                                    }
                                }
                                else
                                {
                                    // 品目コードをキーにして直送(SK)情報を取得
                                    directDeliverysSK = SharedModel.Instance.DirectDeliverys
                                                            .FirstOrDefault(n => n.ItemCode == item.ItemCode);

                                    if (null != directDeliverysSK)
                                    {
                                        // 入庫払いは業者コードを指定しないため、
                                        // 取得した直送(SK)情報のサマリを入庫量/出庫量に設定
                                        var amount = (int)SharedModel.Instance.DirectDeliverys.Where(n => n.ItemCode == item.ItemCode).Select(x => x.Amount).Sum();
                                        nyuko += amount;
                                        harai += amount;
                                    }
                                }
                                break;

                            case ShizaiKbnTypes.EF:
                                OutWorkNoteItem directDeliverysEF = null;
                                if (isSiyoudakaBarai)
                                {
                                    // 品目コード、業者コード、向先をキーにして直送(EF)情報を取得
                                    directDeliverysEF = SharedModel.Instance.DirectDeliverys
                                                            .FirstOrDefault(n => (n.ItemCode == item.ItemCode)
                                                            && (n.GyosyaCD == item.SupplierCode)
                                                            && (n.Mukesaki == Mukesaki.EF.GetStringValue()));

                                    if (null != directDeliverysEF)
                                    {
                                        nyuko += (int)directDeliverysEF.Amount;
                                        harai += (int)directDeliverysEF.Amount;
                                    }
                                }
                                else
                                {
                                    // 品目コード、向先をキーにして直送(EF)情報を取得
                                    directDeliverysEF = SharedModel.Instance.DirectDeliverys
                                                            .FirstOrDefault(n => (n.ItemCode == item.ItemCode)
                                                            && (n.Mukesaki == Mukesaki.EF.GetStringValue()));

                                    if (null != directDeliverysEF)
                                    {
                                        // 入庫払いは業者コードを指定しないため、
                                        // 取得した直送(EF)情報のサマリを入庫量/出庫量に設定
                                        var amount = (int)SharedModel.Instance.DirectDeliverys
                                            .Where(n => (n.ItemCode == item.ItemCode)
                                            && (n.Mukesaki == Mukesaki.EF.GetStringValue())).Select(x => x.Amount).Sum();
                                        nyuko += amount;
                                        harai += amount;
                                    }
                                }
                                break;

                            case ShizaiKbnTypes.LF:
                                OutWorkNoteItem directDeliverysLF = null;
                                if (isSiyoudakaBarai)
                                {
                                    // 品目コード、業者コード、向先をキーにして直送(LF)情報を取得
                                    directDeliverysLF = SharedModel.Instance.DirectDeliverys
                                                            .FirstOrDefault(n => (n.ItemCode == item.ItemCode)
                                                            && (n.GyosyaCD == item.SupplierCode)
                                                            && (n.Mukesaki == Mukesaki.LF.GetStringValue()));

                                    if (null != directDeliverysLF)
                                    {
                                        nyuko += (int)directDeliverysLF.Amount;
                                        harai += (int)directDeliverysLF.Amount;
                                    }
                                }
                                else
                                {
                                    // 品目コード、向先をキーにして直送(LF)情報を取得
                                    directDeliverysLF = SharedModel.Instance.DirectDeliverys
                                                            .FirstOrDefault(n => (n.ItemCode == item.ItemCode)
                                                            && (n.Mukesaki == Mukesaki.LF.GetStringValue()));


                                    if (null != directDeliverysLF)
                                    {
                                        // 入庫払いは業者コードを指定しないため、
                                        // 取得した直送(LF)情報のサマリを入庫量/出庫量に設定
                                        var amount = (int)SharedModel.Instance.DirectDeliverys
                                            .Where(n => (n.ItemCode == item.ItemCode)
                                            && (n.Mukesaki == Mukesaki.LF.GetStringValue())).Select(x => x.Amount).Sum();
                                        nyuko += amount;
                                        harai += amount;
                                    }
                                }
                                break;

                            case ShizaiKbnTypes.Building:
                                OutWorkNoteItem directDeliverysBuilding = null;
                                if (isSiyoudakaBarai)
                                {
                                    // 品目コード、業者コード、向先をキーにして直送(築炉)情報を取得
                                    directDeliverysBuilding = SharedModel.Instance.DirectDeliverys
                                                                .FirstOrDefault(n => (n.ItemCode == item.ItemCode)
                                                                && (n.GyosyaCD == item.SupplierCode)
                                                                && (n.Mukesaki == Mukesaki.Building.GetStringValue()));

                                    if (null != directDeliverysBuilding)
                                    {
                                        nyuko += (int)directDeliverysBuilding.Amount;
                                        harai += (int)directDeliverysBuilding.Amount;
                                    }
                                }
                                else
                                {
                                    directDeliverysBuilding = SharedModel.Instance.DirectDeliverys
                                                                .FirstOrDefault(n => (n.ItemCode == item.ItemCode)
                                                                && (n.Mukesaki == Mukesaki.Building.GetStringValue()));

                                    if (null != directDeliverysBuilding)
                                    {
                                        // 入庫払いは業者コードを指定しないため、
                                        // 取得した直送(築炉)情報のサマリを入庫量/出庫量に設定
                                        var amount = (int)SharedModel.Instance.DirectDeliverys
                                            .Where(n => (n.ItemCode == item.ItemCode)
                                            && (n.Mukesaki == Mukesaki.Building.GetStringValue())).Select(x => x.Amount).Sum();
                                        nyuko += amount;
                                        harai += amount;
                                    }
                                }
                                break;

                            case ShizaiKbnTypes.LD:
                                OutWorkNoteItem directDeliverysLD = null;
                                if (isSiyoudakaBarai)
                                {
                                    // 品目コード、業者コード、向先をキーにして直送(LD)情報を取得
                                    directDeliverysLD = SharedModel.Instance.DirectDeliverys
                                                            .FirstOrDefault(n => (n.ItemCode == item.ItemCode)
                                                            && (n.GyosyaCD == item.SupplierCode)
                                                            && (n.Mukesaki == Mukesaki.LD.GetStringValue()));

                                    if (null != directDeliverysLD)
                                    {
                                        nyuko += (int)directDeliverysLD.Amount;
                                        harai += (int)directDeliverysLD.Amount;
                                    }
                                }
                                else
                                {
                                    // 品目コード、向先をキーにして直送(LD)情報を取得
                                    directDeliverysLD = SharedModel.Instance.DirectDeliverys
                                                            .FirstOrDefault(n => (n.ItemCode == item.ItemCode)
                                                            && (n.Mukesaki == Mukesaki.LD.GetStringValue()));

                                    if (null != directDeliverysLD)
                                    {
                                        // 入庫払いは業者コードを指定しないため、
                                        // 取得した直送(LD)情報のサマリを入庫量/出庫量に設定
                                        var amount = (int)SharedModel.Instance.DirectDeliverys
                                            .Where(n => (n.ItemCode == item.ItemCode)
                                            && (n.Mukesaki == Mukesaki.LD.GetStringValue())).Select(x => x.Amount).Sum();
                                        nyuko += amount;
                                        harai += amount;
                                    }
                                }
                                break;

                            case ShizaiKbnTypes.CC:
                                OutWorkNoteItem directDeliverysCC = null;
                                if (isSiyoudakaBarai)
                                {
                                    // 品目コード、業者コード、向先をキーにして直送(CC)情報を取得
                                    directDeliverysCC = SharedModel.Instance.DirectDeliverys
                                                            .FirstOrDefault(n => (n.ItemCode == item.ItemCode)
                                                            && (n.GyosyaCD == item.SupplierCode)
                                                            && (n.Mukesaki == Mukesaki.CC.GetStringValue()));

                                    if (null != directDeliverysCC)
                                    {
                                        nyuko += (int)directDeliverysCC.Amount;
                                        harai += (int)directDeliverysCC.Amount;
                                    }
                                }
                                else
                                {
                                    // 品目コード、向先をキーにして直送(CC)情報を取得
                                    directDeliverysCC = SharedModel.Instance.DirectDeliverys
                                                            .FirstOrDefault(n => (n.ItemCode == item.ItemCode)
                                                            && (n.Mukesaki == Mukesaki.CC.GetStringValue()));

                                    if (null != directDeliverysCC)
                                    {
                                        // 入庫払いは業者コードを指定しないため、
                                        // 取得した直送(CC)情報のサマリを入庫量/出庫量に設定
                                        var amount = (int)SharedModel.Instance.DirectDeliverys
                                            .Where(n => (n.ItemCode == item.ItemCode)
                                            && (n.Mukesaki == Mukesaki.CC.GetStringValue())).Select(x => x.Amount).Sum();
                                        nyuko += amount;
                                        harai += amount;
                                    }
                                }
                                break;

                            case ShizaiKbnTypes.TD:
                                OutWorkNoteItem directDeliverysTD = null;
                                if (isSiyoudakaBarai)
                                {
                                    // 品目コード、業者コード、向先をキーにして直送(TD)情報を取得
                                    directDeliverysTD = SharedModel.Instance.DirectDeliverys
                                                            .FirstOrDefault(n => (n.ItemCode == item.ItemCode)
                                                            && (n.GyosyaCD == item.SupplierCode)
                                                            && (n.Mukesaki == Mukesaki.TD.GetStringValue()));

                                    if (null != directDeliverysTD)
                                    {
                                        nyuko += (int)directDeliverysTD.Amount;
                                        harai += (int)directDeliverysTD.Amount;
                                    }
                                }
                                else
                                {
                                    // 品目コード、向先をキーにして直送(TD)情報を取得
                                    directDeliverysTD = SharedModel.Instance.DirectDeliverys
                                                            .FirstOrDefault(n => (n.ItemCode == item.ItemCode)
                                                            && (n.Mukesaki == Mukesaki.TD.GetStringValue()));

                                    if (null != directDeliverysTD)
                                    {
                                        // 入庫払いは業者コードを指定しないため、
                                        // 取得した直送(TD)情報のサマリを入庫量/出庫量に設定
                                        var amount = (int)SharedModel.Instance.DirectDeliverys
                                            .Where(n => (n.ItemCode == item.ItemCode)
                                            && (n.Mukesaki == Mukesaki.TD.GetStringValue())).Select(x => x.Amount).Sum();
                                        nyuko += amount;
                                        harai += amount;
                                    }
                                }
                                break;

                            case ShizaiKbnTypes.Others:
                                OutWorkNoteItem directDeliverysETC = null;
                                if (isSiyoudakaBarai)
                                {
                                    // 品目コード、業者コード、向先をキーにして直送(その他)情報を取得
                                    directDeliverysETC = SharedModel.Instance.DirectDeliverys
                                                            .FirstOrDefault(n => (n.ItemCode == item.ItemCode)
                                                            && (n.GyosyaCD == item.SupplierCode)
                                                            && (n.Mukesaki == Mukesaki.Others.GetStringValue()));

                                    if (null != directDeliverysETC)
                                    {
                                        nyuko += (int)directDeliverysETC.Amount;
                                        harai += (int)directDeliverysETC.Amount;
                                    }
                                }
                                else
                                {
                                    // 品目コード、向先をキーにして直送(その他)情報を取得
                                    directDeliverysETC = SharedModel.Instance.DirectDeliverys
                                                            .FirstOrDefault(n => (n.ItemCode == item.ItemCode)
                                                            && (n.Mukesaki == Mukesaki.Others.GetStringValue()));


                                    if (null != directDeliverysETC)
                                    {
                                        // 入庫払いは業者コードを指定しないため、
                                        // 取得した直送(その他)情報のサマリを入庫量/出庫量に設定
                                        var amount = (int)SharedModel.Instance.DirectDeliverys
                                            .Where(n => (n.ItemCode == item.ItemCode)
                                            && (n.Mukesaki == Mukesaki.Others.GetStringValue())).Select(x => x.Amount).Sum();
                                        nyuko += amount;
                                        harai += amount;
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
                                            .FirstOrDefault(itemTie => (itemTie.ItemCode == item.ItemCode) && (itemTie.GyosyaCD == item.SupplierCode));

                            if (null != returns)
                            {
                                henpin = (int)returns.Amount;
                            }
                        }
                        else
                        {
                            // 品目コードをキーにして返品情報を取得
                            returns = SharedModel.Instance.Returns
                                            .FirstOrDefault(itemTie => itemTie.ItemCode == item.ItemCode);

                            if (null != returns)
                            {
                                // 入庫払いは業者コードを指定しないため、
                                // 取得した返品報のサマリを入庫量 /出庫量に設定
                                henpin = (int)SharedModel.Instance.Receivings.Where(n => n.ItemCode == item.ItemCode).Select(x => x.Amount).Sum();
                            }
                        }

                        viewList.Add(new InputInventoryActualRecordViewModel()
                        {
                            SIZAI_KBN = item.ShizaiKbn,
                            WORK_KBN = item.WorkKbn,
                            ITEM_ORDER = item.ItemOrder,
                            HINMOKUCD = item.ItemCode,
                            GYOSYACD = item.SupplierCode,
                            ShizaiKbn = type,
                            WorkKbn = item.WorkKbn,
                            Order = item.ItemOrder,
                            HinmokuCd = item.ItemCode,
                            GyosyaCd = item.SupplierCode,
                            HinmokuName = item.ItemName,
                            GyosyaName = item.SupplierName,
                            TogetuValueToNumber = item.CurrentValue,
                            NyukoValue = nyuko,
                            HaraiValue = harai,
                            HenpinValue = henpin,
                            SoukoValue = soukoValue,
                            EfValue = efValue,
                            LfValue = lfValue,
                            CcValue = ccValue,
                            OthersValue = other,
                            MeterValue = meter,
                            Yobi1Value = reserve1,
                            Yobi2Value = reserve2,
                            PlaceCode = (defItem != null) ? defItem.PlaceCode : string.Empty,
                            IsError = false,
                            IsWarning = false,
                            IsNoProblemData = false
                        });
                    }

                    // データエラー判定
                    if (viewList[viewList.Count - 1].IsTogetuYosouError == true)
                    {
                        errorCount++;
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 画面で変更した値をデータベース更新用の一時データに反映する
        /// </summary>
        /// <param name="viewList">画面のリスト</param>
        private void updateValue(ObservableCollection<InputInventoryActualRecordViewModel> viewList)
        {
            int index = 0;

            try
            {
                foreach (InputInventoryActualRecordViewModel item in viewList)
                {
                    index = InventoryWork.IndexOf(
                        InventoryWork.Find(x => (x.ShizaiKbn?.Trim() == item.SIZAI_KBN?.Trim()
                        && x.WorkKbn?.Trim() == item.WORK_KBN?.Trim()
                        && x.ItemCode?.Trim() == item.HINMOKUCD?.Trim()
                        && x.SupplierCode?.Trim() == item.GYOSYACD?.Trim()))
                    );

                    InventoryWork[index].CurrentValue = item.TogetuValueToNumber;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 画面に表示する棚卸実績データのリフレッシュ
        /// </summary>
        /// <param name="viewList">棚卸実績データ</param>
        private void refreshList(ObservableCollection<InputInventoryActualRecordViewModel> viewList)
        {
            try
            {
                ObservableCollection<InputInventoryActualRecordViewModel> copyViewList
                        = new ObservableCollection<InputInventoryActualRecordViewModel>();

                foreach (InputInventoryActualRecordViewModel item in viewList)
                {
                    copyViewList.Add(new InputInventoryActualRecordViewModel()
                    {
                        SIZAI_KBN = item.SIZAI_KBN,
                        WORK_KBN = item.WORK_KBN,
                        ITEM_ORDER = item.ITEM_ORDER,
                        HINMOKUCD = item.HINMOKUCD,
                        GYOSYACD = item.GYOSYACD,
                        ShizaiKbn = item.ShizaiKbn,
                        WorkKbn = item.WorkKbn,
                        Order = item.Order,
                        HinmokuCd = item.HinmokuCd,
                        GyosyaCd = item.GyosyaCd,
                        HinmokuName = item.HinmokuName,
                        GyosyaName = item.GyosyaName,
                        TogetuValueToNumber = item.TogetuValueToNumber,
                        NyukoValue = item.NyukoValue,
                        HaraiValue = item.HaraiValue,
                        HenpinValue = item.HenpinValue,
                        SoukoValue = item.SoukoValue,
                        EfValue = item.EfValue,
                        LfValue = item.LfValue,
                        CcValue = item.CcValue,
                        OthersValue = item.OthersValue,
                        MeterValue = item.MeterValue,
                        Yobi1Value = item.Yobi1Value,
                        Yobi2Value = item.Yobi2Value,
                        PlaceCode = item.PlaceCode,
                        IsError = item.IsError,
                        IsWarning = item.IsWarning,
                        IsNoProblemData = item.IsNoProblemData
                    });
                }

                viewList.Clear();

                foreach (InputInventoryActualRecordViewModel item in copyViewList)
                {
                    viewList.Add(new InputInventoryActualRecordViewModel()
                    {
                        SIZAI_KBN = item.SIZAI_KBN,
                        WORK_KBN = item.WORK_KBN,
                        ITEM_ORDER = item.ITEM_ORDER,
                        HINMOKUCD = item.HINMOKUCD,
                        GYOSYACD = item.GYOSYACD,
                        ShizaiKbn = item.ShizaiKbn,
                        WorkKbn = item.WorkKbn,
                        Order = item.Order,
                        HinmokuCd = item.HinmokuCd,
                        GyosyaCd = item.GyosyaCd,
                        HinmokuName = item.HinmokuName,
                        GyosyaName = item.GyosyaName,
                        TogetuValueToNumber = item.TogetuValueToNumber,
                        NyukoValue = item.NyukoValue,
                        HaraiValue = item.HaraiValue,
                        HenpinValue = item.HenpinValue,
                        SoukoValue = item.SoukoValue,
                        EfValue = item.EfValue,
                        LfValue = item.LfValue,
                        CcValue = item.CcValue,
                        OthersValue = item.OthersValue,
                        MeterValue = item.MeterValue,
                        Yobi1Value = item.Yobi1Value,
                        Yobi2Value = item.Yobi2Value,
                        PlaceCode = item.PlaceCode,
                        IsError = item.IsError,
                        IsWarning = item.IsWarning,
                        IsNoProblemData = item.IsNoProblemData
                    });
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 選択されているIndexを保持する
        /// </summary>
        /// <param name="selectSizaiKbn">資材区分</param>
        private void memorySelectIndex(ShizaiKbnTypes selectSizaiKbn)
        {
            switch (SelectShizaiKbn)
            {
                case ShizaiKbnTypes.SK:
                    selectSKIndex = SelectIndex;
                    break;
                case ShizaiKbnTypes.LF:
                    selectLFIndex = SelectIndex;
                    break;
                case ShizaiKbnTypes.EF:
                    selectEFIndex = SelectIndex;
                    break;
                case ShizaiKbnTypes.Building:
                    selectBuildingIndex = SelectIndex;
                    break;
                case ShizaiKbnTypes.LD:
                    selectLDIndex = SelectIndex;
                    break;
                case ShizaiKbnTypes.TD:
                    selectTDIndex = SelectIndex;
                    break;
                case ShizaiKbnTypes.CC:
                    selectCCIndex = SelectIndex;
                    break;
                case ShizaiKbnTypes.Others:
                    selectOthersIndex = SelectIndex;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 値チェック
        /// </summary>
        /// <returns>チェック結果</returns>
        protected bool CheckValue()
        {
            bool hasError = false;
            int errorItemIndex = -1;
            ShizaiKbnTypes errType = ShizaiKbnTypes.None;

            byte errKbn = 0x00;
            bool criticalErr = false;
            bool isCritical = false;

            try
            {
                // 各資材区分ごとに入力チェックを行う
                if (!checkValue(SkList, ref errorItemIndex, ref errKbn, ref criticalErr))
                {
                    hasError = true;
                    errType = (errType == ShizaiKbnTypes.None) ? ShizaiKbnTypes.SK : errType;
                    if (!isCritical && criticalErr) { errType = ShizaiKbnTypes.SK; isCritical = true; }
                }
                if (!checkValue(EfList, ref errorItemIndex, ref errKbn, ref criticalErr))
                {
                    hasError = true;
                    errType = (errType == ShizaiKbnTypes.None) ? ShizaiKbnTypes.EF : errType;
                    if (!isCritical && criticalErr) { errType = ShizaiKbnTypes.EF; isCritical = true; }
                }
                if (!checkValue(LfList, ref errorItemIndex, ref errKbn, ref criticalErr))
                {
                    hasError = true;
                    errType = (errType == ShizaiKbnTypes.None) ? ShizaiKbnTypes.LF : errType;
                    if (!isCritical && criticalErr) { errType = ShizaiKbnTypes.LF; isCritical = true; }
                }
                if (!checkValue(BuildingList, ref errorItemIndex, ref errKbn, ref criticalErr))
                {
                    hasError = true;
                    errType = (errType == ShizaiKbnTypes.None) ? ShizaiKbnTypes.Building : errType;
                    if (!isCritical && criticalErr) { errType = ShizaiKbnTypes.Building; isCritical = true; }
                }
                if (!checkValue(LdList, ref errorItemIndex, ref errKbn, ref criticalErr))
                {
                    hasError = true;
                    errType = (errType == ShizaiKbnTypes.None) ? ShizaiKbnTypes.LD : errType;
                    if (!isCritical && criticalErr) { errType = ShizaiKbnTypes.LD; isCritical = true; }
                }
                if (!checkValue(TdList, ref errorItemIndex, ref errKbn, ref criticalErr))
                {
                    hasError = true;
                    errType = (errType == ShizaiKbnTypes.None) ? ShizaiKbnTypes.TD : errType;
                    if (!isCritical && criticalErr) { errType = ShizaiKbnTypes.TD; isCritical = true; }
                }
                if (!checkValue(CcList, ref errorItemIndex, ref errKbn, ref criticalErr))
                {
                    hasError = true;
                    errType = (errType == ShizaiKbnTypes.None) ? ShizaiKbnTypes.CC : errType;
                    if (!isCritical && criticalErr) { errType = ShizaiKbnTypes.CC; isCritical = true; }
                }
                if (!checkValue(OthersList, ref errorItemIndex, ref errKbn, ref criticalErr))
                {
                    hasError = true;
                    errType = (errType == ShizaiKbnTypes.None) ? ShizaiKbnTypes.Others : errType;
                    if (!isCritical && criticalErr) { errType = ShizaiKbnTypes.Others; isCritical = true; }
                }

                if (hasError)
                {
                    ShowSK = (errType == ShizaiKbnTypes.SK);
                    ShowEF = (errType == ShizaiKbnTypes.EF);
                    ShowLF = (errType == ShizaiKbnTypes.LF);
                    ShowBuilding = (errType == ShizaiKbnTypes.Building);
                    ShowLD = (errType == ShizaiKbnTypes.LD);
                    ShowTD = (errType == ShizaiKbnTypes.TD);
                    ShowCC = (errType == ShizaiKbnTypes.CC);
                    ShowOthers = (errType == ShizaiKbnTypes.Others);

                    SelectShizaiKbn = errType;
                    SelectIndex = errorItemIndex;

                    if (errorCount > 0)
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "HasMinusTogetuYosouAmount");
                        return false;
                    }

                    if ((errKbn & Constants.ActualValue_Not_Input) == Constants.ActualValue_Not_Input)
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "NotInputTogetuAmount");
                        return false;
                    }

                    if ((errKbn & Constants.ActualValue_Over_Input_Length) == Constants.ActualValue_Over_Input_Length)
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "OverInputTogetuAmount");
                        return false;
                    }

                    if ((errKbn & Constants.ActualValue_Over_Stock) == Constants.ActualValue_Over_Stock)
                    {
                        MessageManager.ShowExclamation(SystemID.CKSI1010, "OverStockInputTogetuAmount");
                        return false;
                    }

                    if ((errKbn & Constants.ActualValue_Over_Yosou) == Constants.ActualValue_Over_Yosou)
                    {
                        if (MessageManager.ShowQuestion(SystemID.CKSI1010, "TogetuAmountOverTogetuYosouAmount") == ResultType.No)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <param name="viewList">画面のリスト</param>
        /// <param name="errorItemIndex">エラー項目のインデックス</param>
        /// <param name="errKbn">エラー区分</param>
        /// <returns>チェック結果 true=OK/false=NG</returns>
        private bool checkValue(ObservableCollection<InputInventoryActualRecordViewModel> viewList, 
                                ref int errorItemIndex, ref byte errKbn, ref bool criticalErr)
        {
            bool result = true;
            int index = 0;
            criticalErr = false;

            try
            {
                foreach (InputInventoryActualRecordViewModel item in viewList)
                {
                    // エラー情報初期化
                    item.IsError = false;
                    item.IsWarning = false;
                    var togetuValue = string.IsNullOrEmpty(item.TogetuValue)? 
                                        0 : long.Parse(item.TogetuValue.Replace(",", string.Empty));

                    // 未入力チェック
                    if (item.TogetuValue == null || item.TogetuValue.Length == 0)
                    {
                        if (errorItemIndex < 0)
                        {
                            errorItemIndex = index;
                        }
                        if (!criticalErr)
                        {
                            errorItemIndex = index;
                            criticalErr = true;
                        }

                        item.IsError = true;
                        item.IsNoProblemData = false;
                        result = false;
                        errKbn |= Constants.ActualValue_Not_Input;
                    }

                    // 入力文字長チェック
                    else if (item.TogetuValue.Replace(",", "").Length > 10)
                    {
                        if (errorItemIndex < 0)
                        {
                            errorItemIndex = index;
                        }
                        if (!criticalErr)
                        {
                            errorItemIndex = index;
                            criticalErr = true;
                        }
                        item.IsError = true;
                        item.IsNoProblemData = false;
                        result = false;
                        errKbn |= Constants.ActualValue_Over_Input_Length;
                    }

                    // 当月量 >= (当月入庫量　+ 前月在庫)
                    else if (togetuValue > (item.NyukoValue + item.ZengetuValue))
                    {
                        if (errorItemIndex < 0)
                        {
                            errorItemIndex = index;
                        }
                        if (!criticalErr)
                        {
                            errorItemIndex = index;
                            criticalErr = true;
                        }
                        item.IsError = true;
                        item.IsNoProblemData = false;
                        result = false;
                        errKbn |= Constants.ActualValue_Over_Stock;
                    }

                    else
                    {
                        if (item.ShizaiKbn == ShizaiKbnTypes.SK)
                        {
                            if (item.TogetuValueToLong != item.TogetuYosouValue)
                            {
                                // 当月量と当月量（予想）が異なる場合は警告にする。
                                if (errorItemIndex < 0)
                                {
                                    errorItemIndex = index;
                                }
                                item.IsWarning = true;
                                item.IsNoProblemData = false;
                                result = false;
                                errKbn |= Constants.ActualValue_Over_Yosou;
                            }

                            else
                            {
                                item.IsNoProblemData = true;
                            }
                        }
                        else
                        {
                            if (item.TogetuValueToLong > item.TogetuYosouValue)
                            {
                                // 当月量が当月量（予想）を超える場合は警告にする。
                                if (errorItemIndex < 0)
                                {
                                    errorItemIndex = index;
                                }
                                item.IsWarning = true;
                                item.IsNoProblemData = false;
                                result = false;
                                errKbn |= Constants.ActualValue_Over_Yosou;
                            }

                            else
                            {
                                item.IsNoProblemData = true;
                            }
                        }
                    }
                    index++;
                }

                return result;
            }

            catch (Exception　ex)
            {
                throw;
            }
        }

        #endregion
    }
}