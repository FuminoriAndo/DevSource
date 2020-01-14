//*************************************************************************************
//
//   棚卸期間
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
    /// 棚卸期間
    /// </summary>
    public class InventoryTerm
    {
        #region プロパティ

        /// <summary>
        /// 棚卸年月
        /// </summary>
        public string YearMonth { get; set; } = string.Empty;

        /// <summary>
        /// 期日(期)
        /// </summary>
        public int Term { get; set; } = 0;

        /// <summary>
        /// 上期/下期
        /// </summary>
        public int Half { get; set; } = 0;

        #endregion
    }
}
