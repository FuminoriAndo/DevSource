//*************************************************************************************
//
//   作業誌チェックリストのレコードのコンテナ(interface)
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
    /// 作業誌チェックリストのレコードのコンテナ
    /// </summary>
    public interface IWorkNoteRecordContainer
    {
        #region インターフェース

        /// <summary>
        /// 作業誌チェックリストのレコードの編集通知
        /// </summary>
        void NotifyWorkNoteRecordModified();

        #endregion
    }
}
