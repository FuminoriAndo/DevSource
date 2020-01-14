using GalaSoft.MvvmLight;
//*************************************************************************************
//
//   作業誌チェックリストのレコードの変更状態管理
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1090.Operation.WorkNoteCheckList.ViewModel
{
    /// <summary>
    /// 作業誌チェックリストのレコードの変更状態管理
    /// </summary>
    public class WorkNoteRecordModificationViewModel : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 変更されているか
        /// </summary>
        private bool isDirty = false;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WorkNoteRecordModificationViewModel()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 変更されているか
        /// </summary>
        public bool IsDirty
        {
            get { return isDirty; }
            set { Set(ref this.isDirty, value); }
        }

        #endregion

    }
}
