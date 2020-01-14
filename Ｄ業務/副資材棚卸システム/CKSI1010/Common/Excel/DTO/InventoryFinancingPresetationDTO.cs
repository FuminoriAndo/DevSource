//*************************************************************************************
//
//   財務提出用棚卸表のDTO
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Common.Excel.DTO
{
    /// <summary>
    /// 財務提出用棚卸表のDTO
    /// </summary>
    public class InventoryFinancingPresetationDTO
    {
        #region プロパティ

        /// <summary>
        /// 品目名
        /// </summary>
        public string HinmokuName { get; set; }

        /// <summary>
        /// 費目
        /// </summary>
        public string Himoku { get; set; }

        /// <summary>
        /// 内訳
        /// </summary>
        public string Utiwake { get; set; }

        /// <summary>
        /// 棚番
        /// </summary>
        public string Tanaban { get; set; }

        /// <summary>
        /// 単位
        /// </summary>
        public string Tani { get; set; }

        /// <summary>
        /// 期末数量
        /// </summary>
        public decimal Kimatusuryo { get; set; }

        #endregion

    }
}
