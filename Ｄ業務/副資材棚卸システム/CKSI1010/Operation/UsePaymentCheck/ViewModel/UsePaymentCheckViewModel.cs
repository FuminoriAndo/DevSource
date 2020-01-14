using CKSI1010.Common;
using CKSI1010.Core;
using CKSI1010.Operation.Common.View;
using CKSI1010.Operation.Common.ViewModel;
using CKSI1010.Operation.CorrectInventoryActual.ViewModel;
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
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static CKSI1010.Common.Constants;
using static Common.MessageManager;
//*************************************************************************************
//
//   受払いチェック(使用高払い)のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.UsePaymentCheck.ViewModel
{
    /// <summary>
    /// 受払いチェック(使用高払い)のビューモデル
    /// </summary>
    public class UsePaymentCheckViewModel : OperationViewModelBase
    {

        #region フィールド

        /// <summary>
        /// 確定状態か
        /// </summary>
        private bool isFixed = false;

        /// <summary>
        /// 費目(表示用)
        /// </summary>
        private string himoku = string.Empty;

        /// <summary>
        /// 費目名(表示用)
        /// </summary>
        private string himokuName = string.Empty;

        /// <summary>
        /// 表示データ　
        /// </summary>
        private ObservableCollection<UsePaymentRecordViewModel> usePaymentRecords = null;

        /// <summary>
        /// グルーピングデータの表示インデックス
        /// </summary>
        private int viewIndex = 0;

        /// <summary>
        /// エラー件数
        /// </summary>
        private int errorCount = 0;

        /// <summary>
        /// 警告件数
        /// </summary>
        private int warningCount = 0;

        /// <summary>
        /// 前ページボタン有効無効
        /// </summary>
        private bool back = true;

        /// <summary>
        /// 次ページボタン有効無効
        /// </summary>
        private bool next = true;

        /// <summary>
        /// 警告かどうか
        /// </summary>
        private bool isWarning = false;

        /// <summary>
        /// データに問題ないか(エラー、警告は0か)
        /// </summary>
        private bool isNoProblemData = false;

        /// <summary>
        /// 現在のページ
        /// </summary>
        private int currentPage = 0;

        #endregion

        #region プロパティ

        /// <summary>
        /// 実施年月
        /// </summary>
        public OperationYearMonth OperationYearMonth => SharedViewModel.Instance.OperationYearMonth;

        /// <summary>
        /// 確定状態か
        /// </summary>
        public bool IsFixed
        {
            get { return isFixed; }
            set { Set(ref isFixed, value); }
        }

        /// <summary>
        /// 表示データ
        /// </summary>
        public ObservableCollection<UsePaymentRecordViewModel> UsePaymentRecords
        {
            get { return usePaymentRecords; }
            set { Set(ref usePaymentRecords, value); }
        }

        /// <summary>
        /// 費目でグルーピングされたデータ(表示元データ)
        /// </summary>
        public IEnumerable<IGrouping<string, UsePaymentRecordViewModel>> GroupingData
        {
            get;
            private set;
        }

        /// <summary>
        /// エラー件数
        /// </summary>
        public int ErrorCount
        {
            get { return errorCount; }
            set { Set(ref errorCount, value); }
        }

        /// <summary>
        /// エラー件数
        /// </summary>
        public int WarningCount
        {
            get { return warningCount; }
            set { Set(ref warningCount, value); }
        }

        /// <summary>
        /// 費目(表示用)
        /// </summary>
        public string Himoku
        {
            get { return himoku; }
            set { Set(ref himoku, value); }
        }

        /// <summary>
        /// 費目名(表示用)
        /// </summary>        
        public string HimokuName
        {
            get { return himokuName; }
            set { Set(ref himokuName, value); }
        }

        /// <summary>
        /// 前ページボタン有効無効
        /// </summary>
        public bool Back
        {
            get { return back; }
            set { Set(ref back, value); }
        }

        /// <summary>
        /// 次ページボタン有効無効
        /// </summary>
        public bool Next
        {
            get { return next; }
            set { Set(ref next, value); }
        }

        /// <summary>
        /// 警告かどうか
        /// </summary>
        public bool IsWarning
        {
            get { return isWarning; }
            set { Set(ref isWarning, value); }
        }

        /// <summary>
        /// データに問題ないか(エラー、警告は0か)
        /// </summary>
        public bool IsNoProblemData
        {
            get { return isNoProblemData; }
            set { Set(ref isNoProblemData, value); }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataService">データサービス</param>
        public UsePaymentCheckViewModel(IDataService dataService) : base(dataService)
        {
            SharedModel.Instance.IsUsePaymentSelectAll = true;
            UsePaymentRecords = new ObservableCollection<UsePaymentRecordViewModel>();
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
                viewIndex = 0;

                // 表示データの更新
                UpdateViewData(viewIndex);

                Back = false;
                Next = GroupingData.Count() == 1 ? false : true;
                if(!isError())
                {
                    await UpdateOperationCondition(OperationCondition.Fix);
                    IsFixed = true;
                }
                else
                {
                    await UpdateOperationCondition(OperationCondition.Modify);
                    IsFixed = false;
                }

                await base.InitializeAsync();
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
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
                    MessageManager.ShowError(SystemID.CKSI1010, "UkebaraiErrorNotCleared");
                    return false;
                }

                // 共有データに印刷対象外の使用高払い受払い情報を設定する
                setExcludeUsePaymentInfoDictionay(currentPage);
                return await base.GoNextAsync();
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
        /// ぱんくずリスト選択(前処理)
        /// </summary>
        internal override async Task<bool> BeforeSelecOperationAsync()
        {
            try
            {
                BusyStatus.IsBusy = true;

                if (!IsFixed)
                {
                    MessageManager.ShowError(SystemID.CKSI1010, "UkebaraiErrorNotCleared");
                    return false;
                }

                // 共有データに印刷対象外の使用高払い受払い情報を設定する
                setExcludeUsePaymentInfoDictionay(currentPage);
                return await base.BeforeSelecOperationAsync();
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
        /// 当月量修正画面からの戻り
        /// </summary>
        /// <param name="e"></param>
        private async void callBackCorrectInventoryActualView(CloseEventArgs e)
        {
            try
            {
                GroupingData = null;
                // 表示データの更新
                UpdateViewData(viewIndex);
                if (!isError())
                {
                    await UpdateOperationCondition(OperationCondition.Fix);
                    IsFixed = true;
                }
                else
                {
                    await UpdateOperationCondition(OperationCondition.Modify);
                    IsFixed = false;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 表示用の元データを取得する(費目でグルーピング)
        /// </summary>
        private async void GetGroupedData()
        {
            try
            {
                // 使用高払いのデータを取得
                var modelList = await DataService.GetSiyoudakaData();

                ObservableCollection<UsePaymentRecordViewModel> records
                    = new ObservableCollection<UsePaymentRecordViewModel>();

                foreach (var item in modelList)
                {
                    UsePaymentRecordViewModel data = new UsePaymentRecordViewModel();

                    data.Himoku = item.Himoku;
                    data.HimokuName = item.HimokuName;
                    data.Utiwake = item.Utiwake;
                    data.UtiwakeName = item.UtiwakeName;
                    data.ShelfNo = item.Tanaban;
                    data.KouzaName2 = item.KouzaName;
                    data.Unit = item.Tani;
                    data.Szaiko = item.SZaiko.ToString();
                    data.Nyuko = item.Nyuko.ToString();
                    data.SEF = item.EFShukko.ToString();
                    data.SLF = item.LFShukko.ToString();
                    data.SCC = item.CCShukko.ToString();
                    data.SSonota = item.OtherShukko.ToString();
                    data.Sjigyo = item.BusinessDevelopment.ToString();
                    data.S1ji = item.PrimaryCutting.ToString();
                    data.STD = item.TDShukko.ToString();
                    data.S2ji = item.SecondarycCutting.ToString();
                    data.Ezaiko = item.EZaiko.ToString();
                    data.HinmokuName = item.HinmokuName;
                    data.HinmokuCd = item.HinmokuCode;

                    //状態を取得する
                    string statusVal = string.Empty;
                    string status = (GetStatus(item, ref statusVal)).ToString();
                    data.Status = int.Parse(status.Substring(0, 1));
                    data.Infomation = statusVal;
                    data.InformationCode = status.Length > 1 ? status.Substring(1, 1) : Constants.Normal_Information;

                    records.Add(data);
                }

                //費目でグルーピングする
                GroupingData = records.GroupBy(a => a.Himoku);

                if (GroupingData.Count() > 1)
                {
                    Next = true;
                }

                //データに問題はないか(エラー、警告は0か)
                IsNoProblemData = (ErrorCount == 0 && WarningCount == 0) ? true : false;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 表示データの更新
        /// </summary>
        /// <param name="index">表示対象となるグルーピングされたデータのインデックス</param>
        private void UpdateViewData(int index)
        {
            try
            {
                if (!SharedModel.Instance.IsUsePaymentSelectInitialized)
                {
                    // 共有データに印刷対象外の使用高払い受払い情報を設定する
                    setExcludeUsePaymentInfoDictionay(index);
                }
                else
                {
                    SharedModel.Instance.IsUsePaymentSelectInitialized = false;
                }

                // エラー・警告件数のクリア
                ErrorCount = 0;
                WarningCount = 0;

                // 表示用の元データを取得し、費目でグルーピングする。
                GetGroupedData();

                // インデックスで指定されたデータを表示データへ詰め替える
                if ((index < 0) || ((index + 1 > GroupingData.Count())))
                {
                    // インデックスが範囲外の場合は何もしない
                    return;
                }

                // 表示データに詰め替える
                var eachData = GroupingData.ElementAt(index);

                // 表示用の費目設定
                Himoku = eachData.Key;

                // 表示用の費目名設定
                HimokuName = eachData.ElementAt(0).HimokuName;

                UsePaymentRecords.Clear();

                foreach (var item in eachData)
                {
                    item.IsChecked = SharedModel.Instance.IsUsePaymentSelectAll ? true : false;
                    UsePaymentRecords.Add(item);
                }

                // 共有データから対象ページの印刷対象外の使用高払い受払い情報を取得する
                IList<PaymentInfo> excludePaymentInfoList = getExcludeUsePaymentInfoList(index);

                // 使用高払い受払いデータの再生成
                reCreateUsePaymentDataList(excludePaymentInfoList);

                currentPage = index;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 状態取得
        /// </summary>
        /// <param name="data">受払いデータ</param>
        /// <param name="statusVal">状態の値</param>
        /// <returns>状態</returns>
        private int GetStatus(Ukebarai data, ref string statusVal)
        {
            // 在庫・入庫・出庫数量の何れかにマイナス値が登録されていないか？
            if ((data.SZaiko < 0) ||
                (data.Nyuko < 0) ||
                (data.EFShukko < 0) ||
                (data.LFShukko < 0) ||
                (data.CCShukko < 0) ||
                (data.OtherShukko < 0) ||
                (data.BusinessDevelopment < 0) ||
                (data.PrimaryCutting < 0) ||
                (data.TDShukko < 0) ||
                (data.SecondarycCutting < 0) ||
                (data.EZaiko < 0)
                )
            {
                statusVal = Constants.Error_StringDefine;
                ErrorCount += 1;
                return int.Parse(Constants.Error_Status + Constants.Ukebarai_Amount_Minus);
            }

            // 月末、月初在庫の桁の差が閾値を超えていないか?
            if ((data.SZaiko > 999999) || (data.EZaiko > 999999))
            {
                int szaikoCount = data.SZaiko.ToString().Length;
                int ezaikoCount = data.EZaiko.ToString().Length;

                if (szaikoCount != ezaikoCount)
                {
                    statusVal = Constants.Warning_StringDefine;
                    WarningCount++;
                    return int.Parse(Constants.Warning_Status + Constants.Ukebarai_Difference_Start_And_End_Zaiko_Limit_Over);
                }
            }

            return int.Parse(Constants.Normal_Status);
        }

        /// <summary>
        /// エラーかどうか
        /// </summary>
        /// <returns>結果</returns>
        private bool isError()
        {
            bool ret = false;

            foreach (var item in GroupingData)
            {
                var tmp = item.Where(a => a.Status == int.Parse(Constants.Error_Status));
                if ((tmp != null) && (tmp.Count() > 0))
                {
                    ret = true;
                    break;
                }
            }

            return ret;
        }

        /// <summary>
        /// 共有データに印刷対象外の使用高払い受払い情報を設定する
        /// </summary>
        /// <param name="index">対象ページ</param>
        private void setExcludeUsePaymentInfoDictionay(int index)
        {
            try
            {
                IList<PaymentInfo> excludeInfo = new List<PaymentInfo>();

                foreach (var item in UsePaymentRecords)
                {
                    if (!item.IsChecked)
                    {
                        PaymentInfo paymentInfo = new PaymentInfo();
                        paymentInfo.Himoku = item.Himoku;
                        paymentInfo.Utiwake = item.Utiwake;
                        paymentInfo.Tanaban = item.ShelfNo;
                        paymentInfo.HinmokuCode = item.HinmokuCd;
                        paymentInfo.KouzaName = item.KouzaName2;
                        excludeInfo.Add(paymentInfo);
                    }
                }

                SharedModel.Instance.ExcludeUsePaymentInfo[currentPage] = excludeInfo;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 共有データから対象ページの印刷対象外の使用高払い受払い情報を取得する
        /// </summary>
        /// <param name="index">対象ページ</param>
        /// <returns>対象ページの印刷対象外の使用高払い受払い情報</returns>
        private IList<PaymentInfo> getExcludeUsePaymentInfoList(int index)
        {
            try
            {
                IList<PaymentInfo> ret = null;

                if (SharedModel.Instance.ExcludeUsePaymentInfo.ContainsKey(index))
                {
                    ret = SharedModel.Instance.ExcludeUsePaymentInfo[index];
                }

                return ret;
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 使用高払い受払いデータの再生成
        /// </summary>
        /// <param name="excludePaymentInfo">対象ページの印刷対象外の使用高払い受払い情報</param>
        private void reCreateUsePaymentDataList(IList<PaymentInfo> excludePaymentInfo)
        {
            try
            {
                ObservableCollection<UsePaymentRecordViewModel> records
                    = new ObservableCollection<UsePaymentRecordViewModel>();

                foreach (var item in UsePaymentRecords)
                {
                    if (excludePaymentInfo != null)
                    {
                        var exclude = excludePaymentInfo.FirstOrDefault
                            (n => (n.Himoku == item.Himoku)
                                && (n.Utiwake == item.Utiwake)
                                && (n.Tanaban == item.ShelfNo)
                                && (n.HinmokuCode == item.HinmokuCd)
                                && (n.KouzaName == item.KouzaName2));

                        if (SharedModel.Instance.IsUsePaymentSelectAll)
                        {
                            if (exclude != null) item.IsChecked = false;
                        }
                        else
                        {
                            if (exclude == null) item.IsChecked = true;
                        }
                    }

                    records.Add(item);
                }

                UsePaymentRecords = new ObservableCollection<UsePaymentRecordViewModel>(records);
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region コマンド

        /// <summary>
        /// 次へ前へ
        /// </summary>
        public ICommand FireButtonClick => new RelayCommand<bool>(isNext =>
        {
            try
            {
                // インデックスの更新
                viewIndex = isNext ? viewIndex + 1 : viewIndex - 1;

                // 表示データの更新
                UpdateViewData(viewIndex);

                // 前ページ/次ページの活性制御
                Back = viewIndex == 0 ? false : true;
                Next = GroupingData.Count() - 1 <= viewIndex ? false : true;
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

        });

        /// <summary>
        /// 当月量修正実行
        /// </summary>
        public ICommand FireExecute => new RelayCommand(() =>
        {
            try
            {
                IList<UsePaymentRecordViewModel> errorlist = new List<UsePaymentRecordViewModel>();
                foreach (var item in GroupingData)
                {
                    var tmp = item.Where(a => (a.Status != 0));
                    errorlist.AddRange(tmp);
                }

                // 画面表示
                UserControl userControl = CreateWindow("CorrectInventoryActualView");
                CorrectInventoryActualViewModel viewModel 
                    = ((OperationViewBase)userControl).DataContext as CorrectInventoryActualViewModel;
                viewModel.SetList(errorlist, CommonUtility.GetCurrentOperationWorkKbn());

                // クローズイベントを受け取るメソッドを追加する
                viewModel.CloseEvent += new CorrectInventoryActualViewModel
                                            .CloseEventHandler(callBackCorrectInventoryActualView);
                // モーダル画面を表示する
                ShowModalWindow(userControl);
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
        public ICommand FireSelectAllData => new RelayCommand(() =>
        {
            try
            {
                SharedModel.Instance.IsUsePaymentSelectAll = true;

                // 印刷対象外の使用高払い受払い情報の共有データのクリア
                foreach (var info in SharedModel.Instance.ExcludeUsePaymentInfo)
                {
                    info.Value.Clear();
                }

                foreach (var item in UsePaymentRecords)
                {
                    item.IsChecked = true;
                }
            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

        });

        /// <summary>
        /// 全解除
        /// </summary>
        public ICommand FireDeSelectAllData => new RelayCommand(() =>
        {
            try
            {
                SharedModel.Instance.IsUsePaymentSelectAll = false;

                int count = GroupingData.Count();
                for (int index = 0; index < count; index++)
                {
                    var eachData = GroupingData.ElementAt(index);
                    IList<PaymentInfo> excludeInfo = new List<PaymentInfo>();

                    foreach (var item in eachData)
                    {
                        PaymentInfo paymentInfo = new PaymentInfo();
                        paymentInfo.Himoku = item.Himoku;
                        paymentInfo.Utiwake = item.Utiwake;
                        paymentInfo.Tanaban = item.ShelfNo;
                        paymentInfo.HinmokuCode = item.HinmokuCd;
                        paymentInfo.KouzaName = item.KouzaName2;
                        excludeInfo.Add(paymentInfo);
                    }

                    SharedModel.Instance.ExcludeUsePaymentInfo[index] = excludeInfo;
                }

                foreach (var item in UsePaymentRecords)
                {
                    item.IsChecked = false;
                }

            }

            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Fatal(e.ToString());
                MessageManager.ShowError(SystemID.CKSI1010, "FatalError");
            }

        });

        #endregion
    }
}