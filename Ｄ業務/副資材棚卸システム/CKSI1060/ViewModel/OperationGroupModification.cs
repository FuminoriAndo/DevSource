//*************************************************************************************
//
//   棚卸操作グループの変更状態管理クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   15.08.08              DSK吉武   　 新規作成
//
//*************************************************************************************
namespace CKSI1060.ViewModel
{
    /// <summary>
    /// 棚卸操作グループの変更状態管理
    /// </summary>
    public class OperationGroupModification : ViewModelBase
    {
        #region フィールド
        /// <summary>
        /// 変更されているか
        /// </summary>
        private bool isDirty;
        #endregion

        #region プロパティ
        /// <summary>
        /// 変更されているか
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return isDirty;
            }
            set
            {
                if (isDirty != value)
                {
                    isDirty = value;
                    OnPropertyChanged("IsDirty");
                }
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OperationGroupModification()
        {
        }
        #endregion
    }
}
