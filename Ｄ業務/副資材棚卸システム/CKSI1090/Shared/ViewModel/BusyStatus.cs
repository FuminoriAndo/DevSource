using GalaSoft.MvvmLight;
//*************************************************************************************
//
//   ビジー状態
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090.Shared.ViewModel
{
    /// <summary>
    /// ビジー状態
    /// </summary>
    public class BusyStatus : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// ビジーか
        /// </summary>
        private bool isBusy = false;

        #endregion

        #region プロパティ

        /// <summary>
        /// ビジーか
        /// </summary>
        public bool IsBusy
        {
            get { return isBusy; }
            set { Set(ref isBusy, value); }
        }

        #endregion
    }
}
