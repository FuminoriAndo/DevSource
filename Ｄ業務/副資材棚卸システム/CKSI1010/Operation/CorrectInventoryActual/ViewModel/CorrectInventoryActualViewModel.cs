using CKSI1010.Common;
using CKSI1010.Core;
using CKSI1010.Operation.Common.ViewModel;
using CKSI1010.Operation.InputPaymentCheck.ViewModel;
using CKSI1010.Operation.UsePaymentCheck.ViewModel;
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
//   当月量修正画面のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.CorrectInventoryActual.ViewModel
{
    /// <summary>
    /// 当月量修正画面のビューモデル
    /// </summary>
    public class CorrectInventoryActualViewModel : OperationViewModelBase
    {

        #region フィールド

        /// <summary>
        /// 受払い種別
        /// </summary>
        internal UkebaraiType UkebaraiType;

        /// <summary>
        /// エラーリスト(使用高払い)
        /// </summary>
        internal IList<UsePaymentRecordViewModel> usePaymentList = null;

        /// <summary>
        /// エラーリスト(入庫払い)
        /// </summary>
        internal IList<InputPaymentRecordViewModel> inputPaymentList = null;

        /// <summary>
        /// 資材区分と置場区分の紐付けけリスト
        /// </summary>
        private readonly IList<SizaiCategoryTiePlaceDefinition> sizaiPlaceList = null;

        /// <summary>
        /// 編集レコードビューモデル
        /// </summary>
        private ObservableCollection<CorrectInventoryActualRecordViewModel> editRecords = null;

        /// <summary>
        /// 編集レコードビューモデルの一覧
        /// </summary>
        private IList<ObservableCollection<CorrectInventoryActualRecordViewModel>> editRecordsList = null;

        /// <summary>
        /// 選択項目Index
        /// </summary>
        private int selectIndex = 0;

        /// <summary>
        /// 前ページがあるかどうか
        /// </summary>
        private bool hasPrev = false;

        /// <summary>
        /// 次ページがあるかどうか
        /// </summary>
        private bool hasNext = false;

        /// <summary>
        /// 現在のページ
        /// </summary>
        private int currentPage = 0;

        /// <summary>
        /// 選択項目
        /// </summary>
        private CorrectInventoryActualRecordViewModel selectItem = null;

        /// <summary>
        /// 表示画面の作業区分
        /// </summary>
        private string workKbn = string.Empty;

        /// <summary>
        /// 表示データ(情報)
        /// </summary>
        private string infomation = string.Empty;

        /// <summary>
        /// 表示データ(表示内容)
        /// </summary>
        private string informationDetail1 = string.Empty;

        /// <summary>
        /// 表示データ(表示内容)
        /// </summary>
        private string informationDetail2 = string.Empty;

        /// <summary>
        /// 表示コード(表示内容に紐づくコード)
        /// </summary>
        private string informationCode = string.Empty;

        /// <summary>
        /// 状態(エラーなし：0、エラー：1、警告：2)
        /// </summary>
        private string status = string.Empty;

        /// <summary>
        /// 複数の警告が発生しているか
        /// </summary>
        private bool isMultiWarning = false;

        /// <summary>
        /// 費目コード
        /// </summary>
        private string himokuCd = string.Empty;

        /// <summary>
        /// 費目名
        /// </summary>
        private string himokuName = string.Empty;

        /// <summary>
        /// 内訳
        /// </summary>
        private string utiwake = string.Empty;

        /// <summary>
        /// 内訳名
        /// </summary>
        private string utiwakeName = string.Empty;

        /// <summary>
        /// 棚番
        /// </summary>
        private string tanaban = string.Empty;

        /// <summary>
        /// 品目CD
        /// </summary>
        private string hinmokuCd = string.Empty;

        /// <summary>
        /// 品目名
        /// </summary>
        private string hinmokuName = string.Empty;

        /// <summary>
        /// 最終ページ
        /// </summary>
        private int lastPage = 0;

        /// <summary>
        /// 月初在庫
        /// </summary>
        private long gessyoZaiko = 0;

        /// <summary>
        /// 月末在庫
        /// </summary>
        private long getsumatsuZaiko = 0;

        /// <summary>
        /// 当月入庫
        /// </summary>
        private string nyuko = string.Empty;

        /// <summary>
        /// EF出庫
        /// </summary>
        private string sef = string.Empty;

        /// <summary>
        /// LF出庫
        /// </summary>
        private string slf = string.Empty;

        /// <summary>
        /// CC出庫
        /// </summary>
        private string scc = string.Empty;

        /// <summary>
        /// その他
        /// </summary>
        private string ssonota = string.Empty;

        /// <summary>
        /// 事業開発
        /// </summary>
        private string sjigyo = string.Empty;

        /// <summary>
        /// 1次切断
        /// </summary>
        private string s1ji = string.Empty;

        /// <summary>
        /// TD出庫
        /// </summary>
        private string std = string.Empty;

        /// <summary>
        /// 2次切断
        /// </summary>
        private string s2ji = string.Empty;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CorrectInventoryActualViewModel(IDataService dataService) : base(dataService)
        {
            sizaiPlaceList = new List<SizaiCategoryTiePlaceDefinition>();
            editRecordsList = new List<ObservableCollection<CorrectInventoryActualRecordViewModel>>();
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 棚卸ワークデータ
        /// </summary>
        public IList<InventoryWork> InventoriesWork { get; } = new List<InventoryWork>();

        /// <summary>
        /// 実施年月
        /// </summary>
        public OperationYearMonth OperationYearMonth => SharedViewModel.Instance.OperationYearMonth;

        /// <summary>
        /// 編集レコードビューモデル
        /// </summary>
        public ObservableCollection<CorrectInventoryActualRecordViewModel> EditRecords
        {
            get{ return editRecords; }
            set{ Set(ref editRecords, value); }
        }

        /// <summary>
        /// 選択項目のインデックス
        /// </summary>
        public int SelectIndex
        {
            get { return selectIndex; }
            set { Set(ref selectIndex, value); }
        }

        /// <summary>
        /// 選択項目
        /// </summary>
        public CorrectInventoryActualRecordViewModel SelectItem
        {
            get { return selectItem; }
            set { Set(ref selectItem, value); }
        }

        /// <summary>
        /// 前ページがあるかどうか
        /// </summary>
        public bool HasPrev
        {
            get { return hasPrev; }
            set { Set(ref hasPrev, value); }
        }

        /// <summary>
        /// 次ページがあるかどうか
        /// </summary>
        public bool HasNext
        {
            get { return hasNext; }
            set { Set(ref hasNext, value); }
        }

        /// <summary>
        /// 表示データ(情報)
        /// </summary>
        public string Infomation
        {
            get { return infomation; }
            set { Set(ref infomation, value); }
        }

        /// <summary>
        /// 表示データ(表示内容)
        /// </summary>
        public string InformationDetail1
        {
            get { return informationDetail1; }
            set { Set(ref informationDetail1, value); }
        }

        /// <summary>
        /// 表示データ(表示内容)
        /// </summary>
        public string InformationDetail2
        {
            get { return informationDetail2; }
            set { Set(ref informationDetail2, value); }
        }

        /// <summary>
        /// 表示コード(表示内容に紐づくコード)
        /// </summary>
        public string InformationCode
        {
            get { return informationCode; }
            set { Set(ref informationCode, value); }
        }

        /// <summary>
        /// エラー状態(エラーなし：0、エラー：1、警告：2)
        /// </summary>
        public string Status
        {
            get { return status; }
            set { Set(ref status, value); }
        }

        /// <summary>
        /// エラー状態(エラーなし：0、エラー：1、警告：2)
        /// </summary>
        public bool IsMultiWarning
        {
            get { return isMultiWarning; }
            set { Set(ref isMultiWarning, value); }
        }

        /// <summary>
        /// 費目CD
        /// </summary>
        public string HimokuCd
        {
            get { return himokuCd; }
            set { Set(ref himokuCd, value); }
        }

        /// <summary>
        /// 費目名
        /// </summary>
        public string HimokuName
        {
            get { return himokuName; }
            set { Set(ref himokuName, value); }
        }

        /// <summary>
        /// 内訳
        /// </summary>
        public string Utiwake
        {
            get { return utiwake; }
            set { Set(ref utiwake, value); }
        }

        /// <summary>
        /// 内訳名
        /// </summary>
        public string UtiwakeName
        {
            get { return utiwakeName; }
            set { Set(ref utiwakeName, value); }
        }

        /// <summary>
        /// 棚番
        /// </summary>
        public string Tanaban
        {
            get { return tanaban; }
            set { Set(ref tanaban, value); }
        }

        /// <summary>
        /// 品目CD
        /// </summary>
        public string HinmokuCd
        {
            get { return hinmokuCd; }
            set { Set(ref hinmokuCd, value); }
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
        /// 月初在庫
        /// </summary>
        public long GessyoZaiko
        {
            get { return gessyoZaiko; }
            set { Set(ref gessyoZaiko, value); }
        }

        /// <summary>
        /// 月末在庫
        /// </summary>
        public long GetsumatsuZaiko
        {
            get { return getsumatsuZaiko; }
            set { Set(ref getsumatsuZaiko, value); }
        }

        /// <summary>
        /// 当月入庫
        /// </summary>
        public string Nyuko
        {
            get { return nyuko; }
            set { Set(ref nyuko, value); }
        }

        /// <summary>
        /// EF出庫
        /// </summary>
        public string SEF
        {
            get { return sef; }
            set { Set(ref sef, value); }
        }

        /// <summary>
        /// LF出庫
        /// </summary>
        public string SLF
        {
            get { return slf; }
            set { Set(ref slf, value); }
        }

        /// <summary>
        /// CC出庫
        /// </summary>
        public string SCC
        {
            get { return scc; }
            set { Set(ref scc, value); }
        }

        /// <summary>
        /// その他
        /// </summary>
        public string SSonota
        {
            get { return ssonota; }
            set { Set(ref ssonota, value); }
        }

        /// <summary>
        /// 事業開発
        /// </summary>
        public string Sjigyo
        {
            get { return sjigyo; }
            set { Set(ref sjigyo, value); }
        }

        /// <summary>
        /// 1次切断
        /// </summary>
        public string S1ji
        {
            get { return s1ji; }
            set { Set(ref s1ji, value); }
        }

        /// <summary>
        /// TD出庫
        /// </summary>
        public string STD
        {
            get { return std; }
            set { Set(ref std, value); }
        }

        /// <summary>
        /// 2次切断
        /// </summary>
        public string S2ji
        {
            get { return s2ji; }
            set { Set(ref s2ji, value); }
        }

        /// <summary>
        /// ページ
        /// </summary>
        public int Page
        {
            get { return currentPage; }
            set
            {
                if (value <= 0)
                {
                    value = 1;
                }
                else if (value > editRecordsList.Count())
                {
                    value = editRecordsList.Count();
                }

                Set(ref currentPage, value);

                HasPrev = (currentPage > 1);
                HasNext = (currentPage < lastPage);

                refreshPage();
            }
        }

        /// <summary>
        /// 最終ページ
        /// </summary>
        public int LastPage
        {
            get { return lastPage; }
            set { Set(ref lastPage, value); }
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
                BusyStatus.IsBusy = true;

                // リストの初期化
                editRecordsList.Add(new ObservableCollection<CorrectInventoryActualRecordViewModel>());
                LastPage = editRecordsList.Count();
                Page = 1;

                // 定義ファイルのオープン
                var settings = Path.Combine(Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");
                var doc = XDocument.Load(settings);

                IList<long> shizaiKbnList = new List<long> {(long)SizaiCategory.SK, (long)SizaiCategory.EF, (long)SizaiCategory.LF,
                                                            (long)SizaiCategory.Building, (long)SizaiCategory.LD, (long)SizaiCategory.TD,
                                                            (long)SizaiCategory.CC, (long)SizaiCategory.Others};

                sizaiPlaceList.Clear();

                foreach (long shizaiKbn in shizaiKbnList)
                {
                    var define = doc.XPathSelectElement
                        ($"/Settings/SizaiKbnTiePlaceDefinitions/SizaiKbnTiePlaceDefinition[@SizaiKbn={shizaiKbn.ToString()}]");
                    if (define != null)
                    {
                        sizaiPlaceList.Add(new SizaiCategoryTiePlaceDefinition()
                        {
                            ShizaiKbnTypes = (ShizaiKbnTypes)shizaiKbn,
                            ShizaiKbnType = define.Attribute("SizaiKbn").Value,
                            PlaceCode = define.Attribute("PlaceCode").Value
                        });
                    }
                }

                IList<InventoryWork> workList = new List<InventoryWork>();

                InventoriesWork.Clear();
                editRecordsList.Clear();
                int page = 0;
                long errorCount = 0;

                // 受払いチェック(使用高払い)から呼出された場合
                if (UkebaraiType == UkebaraiType.SiyoudakaBarai)
                {
                    foreach (var item in usePaymentList)
                    {
                        workList.Clear();
                        workList.AddRange(await DataService.GetInventoriesWorkAsync(item.Himoku, item.Utiwake, item.ShelfNo, item.HinmokuCd));
                        InventoriesWork.AddRange(workList);

                        editRecordsList.Add(new ObservableCollection<CorrectInventoryActualRecordViewModel>());
                        setList(editRecordsList[page++], workList, ref errorCount);
                    }

                }

                // 受払いチェック(入庫払い)から呼出された場合
                else if (UkebaraiType == UkebaraiType.NyukoBarai)
                {
                    foreach (var item in inputPaymentList)
                    {
                        workList.Clear();
                        workList.AddRange(await DataService.GetInventoriesWorkAsync(item.Himoku, item.Utiwake, item.ShelfNo, item.HinmokuCd));
                        InventoriesWork.AddRange(workList);

                        editRecordsList.Add(new ObservableCollection<CorrectInventoryActualRecordViewModel>());
                        setList(editRecordsList[page++], workList, ref errorCount);
                    }

                }

                if (editRecordsList.Count == 0)
                {
                    // データが存在しなかった場合は、空のリストを作成する
                    editRecordsList.Add(new ObservableCollection<CorrectInventoryActualRecordViewModel>());
                }

                LastPage = editRecordsList.Count();
                Page = 1;

                // 最初の品目を表示する
                EditRecords = editRecordsList[currentPage - 1];

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
        /// ページのヘッダー項目の更新
        /// </summary>
        private void refreshPage()
        {
            IsMultiWarning = false;

            if (UkebaraiType == UkebaraiType.NyukoBarai )
            {
                Infomation = inputPaymentList[currentPage - 1].Infomation;
                Status = inputPaymentList[currentPage - 1].Status.ToString().Substring(0,1);
                InformationCode = inputPaymentList[currentPage - 1].InformationCode;
                HimokuCd = inputPaymentList[currentPage - 1].Himoku;
                HimokuName = inputPaymentList[currentPage - 1].HimokuName;
                Utiwake = inputPaymentList[currentPage - 1].Utiwake;
                UtiwakeName = inputPaymentList[currentPage - 1].UtiwakeName;
                Tanaban = inputPaymentList[currentPage - 1].ShelfNo;
                HinmokuCd = inputPaymentList[currentPage - 1].HinmokuCd;
                HinmokuName = inputPaymentList[currentPage - 1].HinmokuName;
                GessyoZaiko = long.Parse(inputPaymentList[currentPage - 1].Szaiko);
                GetsumatsuZaiko = long.Parse(inputPaymentList[currentPage - 1].Ezaiko);
                Nyuko = inputPaymentList[currentPage - 1].Nyuko;
                SEF = inputPaymentList[currentPage - 1].SEF;
                SLF = inputPaymentList[currentPage - 1].SLF;
                SCC = inputPaymentList[currentPage - 1].SCC;
                SSonota = inputPaymentList[currentPage - 1].SSonota;
                Sjigyo = inputPaymentList[currentPage - 1].Sjigyo;
                S1ji = inputPaymentList[currentPage - 1].S1ji;
                STD = inputPaymentList[currentPage - 1].STD;
                S2ji = inputPaymentList[currentPage - 1].S2ji;
            }

            else if(UkebaraiType == UkebaraiType.SiyoudakaBarai)
            {
                Infomation = usePaymentList[currentPage - 1].Infomation;
                Status = usePaymentList[currentPage - 1].Status.ToString().Substring(0, 1);
                InformationCode = usePaymentList[currentPage - 1].InformationCode; ;
                HimokuCd = usePaymentList[currentPage - 1].Himoku;
                HimokuName = usePaymentList[currentPage - 1].HimokuName;
                Utiwake = usePaymentList[currentPage - 1].Utiwake;
                UtiwakeName = usePaymentList[currentPage - 1].UtiwakeName;
                Tanaban = usePaymentList[currentPage - 1].ShelfNo;
                HinmokuCd = usePaymentList[currentPage - 1].HinmokuCd;
                HinmokuName = usePaymentList[currentPage - 1].HinmokuName;
                GessyoZaiko = long.Parse(usePaymentList[currentPage - 1].Szaiko);
                GetsumatsuZaiko = long.Parse(usePaymentList[currentPage - 1].Ezaiko);
                Nyuko = usePaymentList[currentPage - 1].Nyuko;
                SEF = usePaymentList[currentPage - 1].SEF;
                SLF = usePaymentList[currentPage - 1].SLF;
                SCC = usePaymentList[currentPage - 1].SCC;
                SSonota = usePaymentList[currentPage - 1].SSonota;
                Sjigyo = usePaymentList[currentPage - 1].Sjigyo;
                S1ji = usePaymentList[currentPage - 1].S1ji;
                STD = usePaymentList[currentPage - 1].STD;
                S2ji = usePaymentList[currentPage - 1].S2ji;
            }

            // 定義ファイルのオープン
            var settings = Path.Combine(Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Settings.xml");
            var doc = XDocument.Load(settings);
            XElement content = null;

            if (this.Status == Constants.Error_Status)
            {
                content = doc.XPathSelectElement($"/Settings/Message/Error/message[@Code={InformationCode}]");
                this.InformationDetail1 = content.Attribute("Content").Value;
                this.InformationDetail2 = string.Empty;
            }

            else if(this.Status == Constants.Warning_Status)
            {

                if (!InformationCode.Equals(Constants.Ukebarai_MultiWarning))
                {
                    content = doc.XPathSelectElement($"/Settings/Message/Warning/message[@Code={InformationCode}]");
                    this.InformationDetail1 = content.Attribute("Content").Value;
                    this.InformationDetail2 = string.Empty;
                }
                else
                {
                    content = doc.XPathSelectElement($"/Settings/Message/Warning/message[@Code={int.Parse(Constants.Ukebarai_Difference_Start_And_End_Zaiko_Limit_Over)}]");
                    this.InformationDetail1 = content.Attribute("Content").Value;
                    content = doc.XPathSelectElement($"/Settings/Message/Warning/message[@Code={int.Parse(Constants.Ukebarai_Do_Not_Shukko_Mukesaki)}]");
                    this.InformationDetail2 = content.Attribute("Content").Value;
                    IsMultiWarning = true;
                }
            }
        }

        /// <summary>
        /// エラーリストの設定
        /// </summary>
        /// <param name="usePaymentList">使用高払いデータ一覧</param>
        /// <param name="workKbn">作業区分</param>
        public void SetList(IList<UsePaymentRecordViewModel> usePaymentList, string workKbn)
        {
            UkebaraiType = UkebaraiType.SiyoudakaBarai;
            this.usePaymentList = usePaymentList;
            this.workKbn = workKbn;
        }

        /// <summary>
        /// エラーリストの設定
        /// </summary>
        /// <param name="inputPaymentList">入庫払いデータ一覧</param>
        /// <param name="workKbn">作業区分</param>
        public void SetList(IList<InputPaymentRecordViewModel> inputPaymentList, string workKbn)
        {
            UkebaraiType = UkebaraiType.NyukoBarai;
            this.inputPaymentList = inputPaymentList;
            this.workKbn = workKbn;
        }

        /// <summary>
        /// 棚卸ワークデータを一覧に設定
        /// </summary>
        /// <param name="viewList">画面のリスト</param>
        /// <param name="workList">棚卸ワークデータリスト</param>
        /// <param name="errorCount">エラー件数</param>
        private void setList(ObservableCollection<CorrectInventoryActualRecordViewModel> viewList, 
                                                    IList<InventoryWork> workList, ref long errorCount)
        {
            viewList.Clear();

            foreach (InventoryWork item in workList)
            {
                ShizaiKbnTypes type = (ShizaiKbnTypes)long.Parse(item.ShizaiKbn);
                SizaiCategoryTiePlaceDefinition defItem = sizaiPlaceList.FirstOrDefault(x => (x.ShizaiKbnTypes == type));

                viewList.Add(new CorrectInventoryActualRecordViewModel()
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
                    IsError = false
                });

                // データエラー判定
                if(viewList[viewList.Count-1].IsTogetuYosouError) errorCount++;
            }

        }

        /// <summary>
        /// 変更した値をDB更新用の一時データに反映する
        /// </summary>
        /// <param name="viewList">画面のリスト</param>
        private void updateValue(ObservableCollection<CorrectInventoryActualRecordViewModel> viewList)
        {
            int index = 0;

            foreach (CorrectInventoryActualRecordViewModel item in viewList)
            {
                index = InventoriesWork.IndexOf(
                    InventoriesWork.FirstOrDefault(x => (x.ShizaiKbn == item.SIZAI_KBN
                                                      && x.WorkKbn == item.WORK_KBN
                                                      && x.ItemOrder == item.ITEM_ORDER
                                                      && x.ItemCode == item.HINMOKUCD
                                                      && x.SupplierCode == item.GYOSYACD))
                );

                InventoriesWork[index].CurrentValue = item.TogetuValueToNumber;
                InventoriesWork[index].CurrentExpValue = (long)item.TogetuYosouValue;
            }
        }

        /// <summary>
        /// 値のチェック
        /// </summary>
        /// <param name="viewList">画面のリスト</param>
        /// <returns>チェック結果</returns>
        protected bool checkValue(ObservableCollection<CorrectInventoryActualRecordViewModel> viewList)
        {
            bool hasError = false;
            int errorItemIndex = -1;
            byte errKbn = 0x00;
            int index = 0;

            foreach (CorrectInventoryActualRecordViewModel item in viewList)
            {
                // エラー情報初期化
                item.IsError = false;
                item.IsWarning = false;
                var togetuValue = string.IsNullOrEmpty(item.TogetuValue) ?
                    0 : long.Parse(item.TogetuValue.Replace(",", string.Empty));

                // 未入力チェック
                if (item.TogetuValue == null || item.TogetuValue.Length == 0)
                {
                    if (errorItemIndex < 0)
                    {
                        errorItemIndex = index;
                        hasError = true;
                    }
                    item.IsError = true;
                    item.IsNoProblemData = false;
                    errKbn |= Constants.ActualValue_Not_Input;
                }

                // 入力文字長チェック
                else if (item.TogetuValue.Replace(",", "").Length > 10)
                {
                    if (errorItemIndex < 0)
                    {
                        errorItemIndex = index;
                        hasError = true;
                    }
                    item.IsError = true;
                    item.IsNoProblemData = false;
                    errKbn |= Constants.ActualValue_Over_Input_Length;
                }

                // 当月量 >= (当月入庫量　+ 前月在庫)
                else if (togetuValue > (item.NyukoValue + item.ZengetuValue))
                {
                    if (errorItemIndex < 0)
                    {
                        errorItemIndex = index;
                        hasError = true;
                    }
                    item.IsError = true;
                    item.IsNoProblemData = false;
                    errKbn |= Constants.ActualValue_Over_Stock;
                }

                else
                {
                    if (item.ShizaiKbn == ShizaiKbnTypes.SK)
                    {

                        if (item.TogetuValueToLong != item.TogetuYosouValue)
                        {
                            // 当月量と当月量（予想）が異なる場合は、警告にする。
                            if (errorItemIndex < 0)
                            {
                                errorItemIndex = index;
                                hasError = true;
                            }
                            item.IsWarning = true;
                            item.IsNoProblemData = false;
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

            if (hasError)
            {
                SelectIndex = errorItemIndex;

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

        #endregion

        #region コマンド

        /// <summary>
        /// 更新ボタン押下
        /// </summary>
        public ICommand FireUpdate => new RelayCommand(async () =>
        {
            string workKbn
                = SharedModel.Instance.WorkKbn.Equals(SizaiWorkCategory.TanaoroshiWork) ? 
                    SizaiWorkCategory.TanaoroshiWork.GetStringValue() : SizaiWorkCategory.KensinWork.GetStringValue();
            bool hasErr = false;
            string errorContent = string.Empty;
            string errorCode = string.Empty;
            string sql = string.Empty;
            string sqlParam = string.Empty;
            string timestamp = string.Empty;
            bool hasException = false;

            try
            {
                BusyStatus.IsBusy = true;

                // 品目別に当月量の入力チェックをする
                for (int i = 0; i < editRecordsList.Count(); i++)
                {
                    if (!checkValue(editRecordsList[i]))
                    {
                        hasErr = true;
                        break;
                    }
                }
                if (hasErr) return;

                // 品目別に棚卸ワークを更新する
                for (int i = 0; i < editRecordsList.Count(); i++)
                {
                    updateValue(editRecordsList[i]);
                }

                // 棚卸データ入力の場合
                await DataService.UpdateInventoriesWorkAsync(InventoriesWork);

                // 資材棚卸トランデータ削除
                await DataService.DeleteInventoriesTranAsync(OperationYearMonth.YearMonth.ToString());

                // 資材棚卸トランデータ登録
                await DataService.EntryInventoriesTranAsync(OperationYearMonth.YearMonth.ToString());

                // 経理報告データ作成を起動する
                ExternalProcessAccessor.StartKeiriHoukokuData();

                // 画面を閉じる
                CloseModalWindow();

                // クローズイベントを通知する
                CloseEvent(new CloseEventArgs(false));

                // クローズした後は、呼び出し元から登録されたイベントハンドラを削除する
                CloseEvent -= CloseEvent;

                return;
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

                if (!hasErr)
                {
                    LogType logType = hasException ? LogType.Error : LogType.Normal;
                    SizaiWorkCategory sizaiWorkCategory
                        = workKbn.Equals(SizaiWorkCategory.TanaoroshiWork.GetStringValue()) ?
                            SizaiWorkCategory.TanaoroshiWork : SizaiWorkCategory.KensinWork;

                    // 棚卸ログの挿入
                    await InsertTanaorosiLog(logType, sizaiWorkCategory, LogOperationType.CorrectInventoryActual,
                                         LogOperationContent.Update, null, errorContent, errorCode);
                }

                if (hasException) 
                { 
                    MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
                }
            }
        });

        /// <summary>
        /// 閉じるボタン押下
        /// </summary>
        public ICommand FireClose => new RelayCommand(() =>
        {
            // モーダル画面を閉じる
            CloseModalWindow();

            // クローズイベントを通知する
            CloseEvent(new CloseEventArgs(true));

            // クローズした後は、呼び出し元から登録されたイベントハンドラを削除する
            CloseEvent -= CloseEvent;

        });

        /// <summary>
        /// 前データ
        /// </summary>
        public ICommand FirePrev => new RelayCommand(() =>
        {
            Page--;
            EditRecords = editRecordsList[currentPage-1];
        });

        /// <summary>
        /// 次データ
        /// </summary>
        public ICommand FireNext => new RelayCommand(() =>
        {
            Page++;
            EditRecords = editRecordsList[currentPage-1];
        });

        #endregion

        #region イベント

        public delegate void CloseEventHandler(CloseEventArgs e);
        public event CloseEventHandler CloseEvent;

        #endregion
    }

    #region 内部クラス

    /// <summary>
    /// 「閉じる」のイベント引数クラス
    /// </summary>
    public class CloseEventArgs : EventArgs
    {
        private readonly bool cancel;

        public CloseEventArgs(bool cancel)
        {
            this.cancel = cancel;
        }

        public bool Cancel { get { return cancel; } }
    }

    #endregion
}