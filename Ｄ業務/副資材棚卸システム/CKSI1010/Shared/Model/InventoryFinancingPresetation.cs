//*************************************************************************************
//
//   財務提出用棚卸表印刷のモデル
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
    /// 財務提出用棚卸表印刷のモデル
    /// </summary>
    public class InventoryFinancingPresetation
    {
        #region プロパティ

        /// <summary>
        /// 品目名
        /// </summary>
        public string HinmokuName { get; set; } = string.Empty;

        /// <summary>
        /// 費目
        /// </summary>
        public string Himoku { get; set; } = string.Empty;

        /// <summary>
        /// 内訳
        /// </summary>
        public string Utiwake { get; set; } = string.Empty;

        /// <summary>
        /// 棚番
        /// </summary>
        public string Tanaban { get; set; } = string.Empty;

        /// <summary>
        /// 単位
        /// </summary>
        public string Tani { get; set; } = string.Empty;

        /// <summary>
        /// 期末数量
        /// </summary>
        public decimal Kimatusuryo { get; set; } = 0;

        #endregion

    }
}
