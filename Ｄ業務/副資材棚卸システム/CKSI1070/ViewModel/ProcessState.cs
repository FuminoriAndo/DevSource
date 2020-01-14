//*************************************************************************************
//
//   処理状態クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   15.08.08              DSK吉武   　 新規作成
//
//*************************************************************************************
using ViewModel;
namespace CKSI1070.ViewModel
{
    /// <summary>
    /// 処理状態
    /// </summary>
    public class ProcessState : ViewModelBase
    {
        #region フィールド
        /// <summary>
        /// 処理中か
        /// </summary>
        private bool isBusy;

        /// <summary>
        /// 処理中に表示するメッセージ
        /// </summary>
        private string busyMessage;
        #endregion

        #region プロパティ
        /// <summary>
        /// 処理中か
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }

        /// <summary>
        /// 処理中に表示するメッセージ
        /// </summary>
        public string BusyMessage
        {
            get
            {
                return busyMessage;
            }
            set
            {
                if (busyMessage != value)
                {
                    busyMessage = value;
                    OnPropertyChanged("BusyMessage");
                }
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProcessState()
        {
        }
        #endregion
    }
}
