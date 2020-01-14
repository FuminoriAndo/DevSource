//*************************************************************************************
//
//   作業誌の項目
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
    /// 作業誌の項目
    /// </summary>
    public class WorkNoteItem
    {
        #region プロパティ

        /// <summary>
        /// 品目CD
        /// </summary>
        internal string ItemCode { get; set; }

        /// <summary>
        /// 業者CD
        /// </summary>
        internal string GyosyaCD { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        internal decimal Amount { get; set; }

        #endregion
    }
}
