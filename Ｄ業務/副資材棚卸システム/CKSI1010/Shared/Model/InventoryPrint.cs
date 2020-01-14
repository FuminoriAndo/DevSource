//*************************************************************************************
//
//   棚卸表印刷のモデル
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
    /// 棚卸表印刷のモデル
    /// </summary>
    public class InventoryPrint
    {
        #region プロパティ

        /// <summary>
        /// 品目CD
        /// </summary>
        public string HinmokuCD { get; set; } = string.Empty;

        /// <summary>
        /// 業者CD
        /// </summary>
        public string GyosyaCD { get; set; } = string.Empty;

        /// <summary>
        /// 品目名
        /// </summary>
        public string HinmokuName { get; set; } = string.Empty;

        /// <summary>
        /// 業者名
        /// </summary>
        public string GyosyaName { get; set; } = string.Empty;

        /// <summary>
        /// 入庫量
        /// </summary>
        public int NyukoRyo { get; set; } = 0;

        /// <summary>
        /// 返品
        /// </summary>
        public int Henpin { get; set; } = 0;

        /// <summary>
        /// 倉庫在庫
        /// </summary>
        public int SKZaiko { get; set; } = 0;

        /// <summary>
        /// EF在庫
        /// </summary>
        public int EFZaiko { get; set; } = 0;

        /// <summary>
        /// LF在庫
        /// </summary>
        public int LFZaiko { get; set; } = 0;

        /// <summary>
        /// CC在庫
        /// </summary>
        public int CCZaiko { get; set; } = 0;

        /// <summary>
        /// その他在庫
        /// </summary>
        public decimal OtherZaiko { get; set; } = 0;

        /// <summary>
        /// メーター在庫
        /// </summary>
        public decimal MeterZaiko { get; set; } = 0;

        /// <summary>
        /// 予備1
        /// </summary>
        public decimal Reserve1Zaiko { get; set; } = 0;

        /// <summary>
        /// 予備2
        /// </summary>
        public decimal Reserve2Zaiko { get; set; } = 0;

        /// <summary>
        /// 備考
        /// </summary>
        public string Bikou { get; set; } = string.Empty;

        /// <summary>
        /// 種別
        /// </summary>
        public string Syubetu { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// </summary>
        public int ItemOrder { get; set; } = 0;

        /// <summary>
        /// 出庫量
        /// </summary>
        public decimal Harai { get; set; } = 0;

        /// <summary>
        /// 作業区分
        /// </summary>
        public string Workkbn { get; set; } = string.Empty;

        /// <summary>
        /// 当月量
        /// </summary>
        public string Togetsuryo { get; set; } = string.Empty;

        #endregion
    }
}
