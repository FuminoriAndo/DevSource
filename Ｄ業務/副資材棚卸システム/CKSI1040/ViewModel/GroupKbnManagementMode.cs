using ViewModel;
//*************************************************************************************
//
//   画面モード管理クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.02.01              DSK       　 新規作成
//
//*************************************************************************************
namespace CKSI1040.ViewModel
{
    /// <summary>
    /// 画面モード管理
    /// </summary>
    public class GroupKbnManagementMode : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 現在の画面モード
        /// </summary>
        private ModeType currentMode;

        #endregion

        #region プロパティ

        /// <summary>
        /// 画面モード
        /// </summary>
        public enum ModeType
        {
             /// <summary>
            /// 追加
            /// </summary>
            Add,
            /// <summary>
            /// 修正
            /// </summary>
            Modify
        }
        
        /// <summary>
        /// 現在の画面モード
        /// </summary>
        public ModeType CurrentMode
        {
            get
            {
                return currentMode;
            }
            set
            {
                if (currentMode != value)
                {
                    currentMode = value;
                    OnPropertyChanged("CurrentMode");
                }
            }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GroupKbnManagementMode()
        {
        }

        #endregion
    }
}
