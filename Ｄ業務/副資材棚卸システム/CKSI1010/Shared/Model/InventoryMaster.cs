//*************************************************************************************
//
//   棚卸項目(マスタ)
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
    /// 棚卸項目(マスタ)
    /// </summary>
    public class InventoryMaster
    {
        #region プロパティ

        /// <summary>
        /// 資材区分
        /// </summary>
        public string ShizaiKbn { get; set; } = string.Empty;

        /// <summary>
        /// 作業区分
        /// </summary>
        public string WorkKbn { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// </summary>
        public long ItemOrder { get; set; } = 0;

        /// <summary>
        /// 品目CD
        /// </summary>
        public string ItemCode { get; set; } = string.Empty;

        /// <summary>
        /// 業者CD
        /// </summary>
        public string SupplierCode { get; set; } = string.Empty;

        /// <summary>
        /// 品目名
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 業者名
        /// </summary>
        public string SupplierName { get; set; } = string.Empty;

        /// <summary>
        /// 備考
        /// </summary>
        public string Bikou { get; set; } = string.Empty;

        #endregion
    }
}
