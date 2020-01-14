//*************************************************************************************
//
//   作業誌(出庫用)の項目
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Shared.Model
{
    /// <summary>
    /// 作業誌(出庫用)の項目
    /// </summary>
    public class OutWorkNoteItem : WorkNoteItem
    {
        #region プロパティ

        /// <summary>
        /// 向先
        /// </summary>
        public string Mukesaki { get; set; } = string.Empty;

        #endregion
    }
}
