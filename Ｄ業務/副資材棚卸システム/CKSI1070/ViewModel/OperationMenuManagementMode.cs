//*************************************************************************************
//
//   画面モード管理クラス
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
    /// 画面モード管理
    /// </summary>
    public class OperationMenuManagementMode : ViewModelBase
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
        public OperationMenuManagementMode()
        {
        }
        #endregion
    }
}
