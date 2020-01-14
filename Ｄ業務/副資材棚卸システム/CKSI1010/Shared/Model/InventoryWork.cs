//*************************************************************************************
//
//   棚卸項目(ワーク)
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
    /// 棚卸項目(ワーク)
    /// </summary>
    public class InventoryWork
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
        /// 当月量
        /// </summary>
        public string CurrentValue { get; set; } = string.Empty;

        /// <summary>
        /// 当月量（予想）
        /// </summary>
        public long CurrentExpValue { get; set; } = 0;

        /// <summary>
        /// 入庫量
        /// </summary>
        public long InputValue { get; set; } = 0;

        /// <summary>
        /// 出庫量
        /// </summary>
        public long OutputValue { get; set; } = 0;

        /// <summary>
        /// 返品
        /// </summary>
        public long ReturnValue { get; set; } = 0;

        /// <summary>
        /// 倉庫在庫
        /// </summary>
        public long StockInWarehouse { get; set; } = 0;

        /// <summary>
        /// EF在庫
        /// </summary>
        public long StockEF { get; set; } = 0;

        /// <summary>
        /// LF在庫
        /// </summary>
        public long StockLF { get; set; } = 0;

        /// <summary>
        /// CC在庫
        /// </summary>
        public long StockCC { get; set; } = 0;

        /// <summary>
        /// 他（その他）在庫
        /// </summary>
        public long StockOthers { get; set; } = 0;

        /// <summary>
        /// メータ在庫
        /// </summary>
        public long StockMeter { get; set; } = 0;

        /// <summary>
        /// 予備１在庫
        /// </summary>
        public long StockYobi1 { get; set; } = 0;

        /// <summary>
        /// 予備２在庫
        /// </summary>
        public long StockYobi2 { get; set; } = 0;

        #endregion

    }
}
