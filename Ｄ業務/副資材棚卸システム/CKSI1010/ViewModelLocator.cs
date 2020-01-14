using CKSI1010.Base.ViewModel;
using CKSI1010.Operation.CorrectInventoryActual.ViewModel;
using CKSI1010.Operation.CreateDetails.ViewModel;
using CKSI1010.Operation.CreatePurchasingCheckData.ViewModel;
using CKSI1010.Operation.InputInputWorkNote.ViewModel;
using CKSI1010.Operation.InputInventoryActual.ViewModel;
using CKSI1010.Operation.InputOutputWorkNote.ViewModel;
using CKSI1010.Operation.InputPaymentCheck.ViewModel;
using CKSI1010.Operation.IssueCaluculation.ViewModel;
using CKSI1010.Operation.Print.ViewModel;
using CKSI1010.Operation.PrintFinancingPresetationInventory.ViewModel;
using CKSI1010.Operation.PrintWitnessInventory.ViewModel;
using CKSI1010.Operation.SettingDisplay.ViewModel;
using CKSI1010.Operation.ShowFlow.ViewModel;
using CKSI1010.Operation.ShowSizaiHinmokuChangeList.ViewModel;
using CKSI1010.Operation.TanaorosiYearMonthCheck.ViewModel;
using CKSI1010.Operation.UpdateInventoryActual.ViewModel;
using CKSI1010.Operation.UsePaymentCheck.ViewModel;
using CKSI1010.Shared.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
//*************************************************************************************
//
//   すべてのビューモデルへの参照
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12    初        DSK   　     新規作成
//   18.07.01    ２        DSK吉武   　 設定画面の追加に伴う対応
//
//*************************************************************************************

namespace CKSI1010
{
    /// <summary>
    /// すべてのビューモデルへの参照
    /// </summary>
    public class ViewModelLocator
    {
        #region コンストラクタ

        /// <summary>
        /// スタティックなコンストラクタ
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();

                SimpleIoc.Default.Register<BaseViewModel>();
                SimpleIoc.Default.Register<TanaorosiYearMonthCheckViewModel>();
                SimpleIoc.Default.Register<ShowFlowViewModel>();
                SimpleIoc.Default.Register<ShowSizaiHinmokuChangeListViewModel>();
                SimpleIoc.Default.Register<PrintViewModel>();
                SimpleIoc.Default.Register<InputInputWorkNoteViewModel>();
                SimpleIoc.Default.Register<InputInventoryActualInventoryDataViewModel>();
                SimpleIoc.Default.Register<InputInventoryActualMeterDataViewModel>();
                SimpleIoc.Default.Register<UpdateInventoryActualViewModel>();
                SimpleIoc.Default.Register<IssueCaluculationViewModel>();
                SimpleIoc.Default.Register<InputOutputWorkNoteViewModel>();
                SimpleIoc.Default.Register<CreateDetailsViewModel>();
                SimpleIoc.Default.Register<PrintWitnessInventoryViewModel>();
                SimpleIoc.Default.Register<PrintFinancingPresetationInventoryViewModel>();
                SimpleIoc.Default.Register<CreatePurchasingCheckDataViewModel>();
                SimpleIoc.Default.Register<UsePaymentCheckViewModel>();
                SimpleIoc.Default.Register<UsePaymentRecordViewModel>();
                SimpleIoc.Default.Register<InputPaymentCheckViewModel>();
                SimpleIoc.Default.Register<CorrectInventoryActualViewModel>();
                SimpleIoc.Default.Register<SettingDisplayViewModel>();
            }
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// ベース画面のビューモデル
        /// </summary>
        public BaseViewModel BaseViewModel => ServiceLocator.Current.GetInstance<BaseViewModel>();

        /// <summary>
        /// 実施月の確認画面のビューモデル
        /// </summary>
        public TanaorosiYearMonthCheckViewModel TanaorosiYearMonthCheckViewModel => ServiceLocator.Current.GetInstance<TanaorosiYearMonthCheckViewModel>();

        /// <summary>
        /// 手順確認のビューモデル
        /// </summary>
        public ShowFlowViewModel StartViewModel => ServiceLocator.Current.GetInstance<ShowFlowViewModel>();

        /// <summary>
        /// 資材品目の変更履歴画面のビューモデル
        /// </summary>
        public ShowSizaiHinmokuChangeListViewModel ShowSizaiHinmokuChangeListViewModel => ServiceLocator.Current.GetInstance<ShowSizaiHinmokuChangeListViewModel>();

        /// <summary>
        /// 棚卸表印刷画面のビューモデル
        /// </summary>
        public PrintViewModel PrintViewModel => ServiceLocator.Current.GetInstance<PrintViewModel>();

        /// <summary>
        /// 棚卸実績値入力（棚卸データ入力作業）画面のビューモデル
        /// </summary>
        public InputInventoryActualInventoryDataViewModel InputInventoryActualInventoryDataViewModel => ServiceLocator.Current.GetInstance<InputInventoryActualInventoryDataViewModel>();

        /// <summary>
        /// 棚卸実績値入力（検針データ入力作業）画面のビューモデル
        /// </summary>
        public InputInventoryActualMeterDataViewModel InputInventoryActualMeterDataViewModel => ServiceLocator.Current.GetInstance<InputInventoryActualMeterDataViewModel>();

        /// <summary>
        /// 棚卸実績値更新画面のビューモデル
        /// </summary>
        public UpdateInventoryActualViewModel UpdateInventoryActualViewModel => ServiceLocator.Current.GetInstance<UpdateInventoryActualViewModel>();

        /// <summary>
        /// 資材班作業誌入力(液酸入庫)画面のビューモデル
        /// </summary>
        public InputInputWorkNoteViewModel InputInputWorkNoteViewModel => ServiceLocator.Current.GetInstance<InputInputWorkNoteViewModel>();

        /// <summary>
        /// 検収明細書作成画面のビューモデル
        /// </summary>
        public CreateDetailsViewModel CreateDetailsViewModel => ServiceLocator.Current.GetInstance<CreateDetailsViewModel>();

        /// <summary>
        /// 資材班作業誌入力(液酸出庫)画面のビューモデル
        /// </summary>
        public InputOutputWorkNoteViewModel InputOutputWorkNoteViewModel => ServiceLocator.Current.GetInstance<InputOutputWorkNoteViewModel>();

        /// <summary>
        /// 受払いチェック(使用高払い)画面のビューモデル
        /// </summary>
        public UsePaymentCheckViewModel UsePaymentCheckViewModel => ServiceLocator.Current.GetInstance<UsePaymentCheckViewModel>();

        /// <summary>
        /// 受払いチェック(入庫払い)画面のビューモデル
        /// </summary>
        public InputPaymentCheckViewModel InputPaymentCheckViewModel => ServiceLocator.Current.GetInstance<InputPaymentCheckViewModel>();

        /// <summary>
        /// 当月量修正画面のビューモデル
        /// </summary>
        public CorrectInventoryActualViewModel CorrectInventoryActualViewModel => ServiceLocator.Current.GetInstance<CorrectInventoryActualViewModel>();

        /// <summary>
        /// 計算書発行画面のビューモデル
        /// </summary>
        public IssueCaluculationViewModel IssueCaluculationViewModel => ServiceLocator.Current.GetInstance<IssueCaluculationViewModel>();

        /// <summary>
        /// 立会い用棚卸表印刷画面のビューモデル
        /// </summary>
        public PrintWitnessInventoryViewModel PrintWitnessInventoryViewModel => ServiceLocator.Current.GetInstance<PrintWitnessInventoryViewModel>();

        /// <summary>
        /// 財務提出用棚卸表印刷画面のビューモデル
        /// </summary>
        public PrintFinancingPresetationInventoryViewModel PrintFinancingPresetationInventoryViewModel => ServiceLocator.Current.GetInstance<PrintFinancingPresetationInventoryViewModel>();

        /// <summary>
        /// 購買検収データ作成画面のビューモデル
        /// </summary>
        public CreatePurchasingCheckDataViewModel CreatePurchasingCheckDataViewModel => ServiceLocator.Current.GetInstance<CreatePurchasingCheckDataViewModel>();

        /// <summary>
        /// 設定画面のビューモデル
        /// </summary>
        public SettingDisplayViewModel SettingDisplayViewModel => ServiceLocator.Current.GetInstance<SettingDisplayViewModel>();

        #endregion

        #region メソッド

        /// <summary>
        /// クリーンアップ
        /// </summary>
        public static void Cleanup()
        {
        }

        #endregion
    }
}