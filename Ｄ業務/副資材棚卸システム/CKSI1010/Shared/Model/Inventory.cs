//*************************************************************************************
//
//   棚卸項目
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Shared.Model
{
    #region プロパティ

    /// <summary>
    /// 棚卸項目
    /// </summary>
    public class Inventory
    {
        /// <summary>
        /// 置場
        /// </summary>
        public int Place { get; set; } = 0;

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
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 当月量
        /// </summary>
        public int CurrentValue { get; set; } = 0;

        /// <summary>
        /// 入庫量
        /// </summary>
        public int InputValue { get; set; } = 0;

        /// <summary>
        /// 返品
        /// </summary>
        public int ReturnValue { get; set; } = 0;

        /// <summary>
        /// 倉庫在庫
        /// </summary>
        public decimal StockInWarehouse { get; set; } = 0;

        /// <summary>
        /// EF在庫
        /// </summary>
        public decimal StockEF { get; set; } = 0;

        /// <summary>
        /// LF在庫
        /// </summary>
        public decimal StockLF { get; set; } = 0;

        /// <summary>
        /// CC在庫
        /// </summary>
        public decimal StockCC { get; set; } = 0;

        /// <summary>
        /// その他在庫
        /// </summary>
        public decimal StockOther { get; set; } = 0;

        /// <summary>
        /// メーター在庫
        /// </summary>
        public decimal StockMeter { get; set; } = 0;

        /// <summary>
        /// 予備1
        /// </summary>
        public decimal StockReserve1 { get; set; } = 0;

        /// <summary>
        /// 予備2
        /// </summary>
        public decimal StockReserve2 { get; set; } = 0;

        /// <summary>
        /// 区分
        /// </summary>
        public int Division { get; set; } = 0;

        #endregion
    }
}
