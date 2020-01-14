//*************************************************************************************
//
//   作業誌の項目(出庫用)
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

using static CKSI1010.Common.Constants;

namespace CKSI1010.Shared.Model
{
    /// <summary>
    /// 作業誌の項目(出庫用)
    /// </summary>
    public class ShukkoWorkNoteItem : WorkNoteItem
    {
        #region プロパティ

        /// <summary>
        /// 向先
        /// </summary>
        internal Target Mukesaki { get; set; }
        #endregion
    }
}
