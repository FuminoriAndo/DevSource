using CKSI1090.Base.ViewModel;
using CKSI1090.Operation.WorkNoteCheckList.ViewModel;
using CKSI1090.Shared.Model;
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
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090
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
                SimpleIoc.Default.Register<WorkNoteCheckListViewModel>();
            }
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// ベース画面のビューモデル
        /// </summary>
        public BaseViewModel BaseViewModel => ServiceLocator.Current.GetInstance<BaseViewModel>();

        /// <summary>
        /// 作業誌チェックリストのビューモデル
        /// </summary>
        public WorkNoteCheckListViewModel WorkNoteCheckListViewModel => ServiceLocator.Current.GetInstance<WorkNoteCheckListViewModel>();

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