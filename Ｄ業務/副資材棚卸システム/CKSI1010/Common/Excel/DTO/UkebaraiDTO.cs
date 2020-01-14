//*************************************************************************************
//
//   受払いのDTO
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
    /// 受払いのDTO
    /// </summary>
    public class UkebaraiDTO
    {
        #region プロパティ

        /// <summary>
        /// 費目
        /// </summary>
        public string Himoku { get; set; } = string.Empty;

        /// <summary>
        /// 費目名
        /// </summary>
        public string HimokuName { get; set; } = string.Empty;

        /// <summary>
        /// 年月
        /// </summary>
        public string YearMonth { get; set; } = string.Empty;

        /// <summary>
        /// 内訳
        /// </summary>
        public string Utiwake { get; set; } = string.Empty;

        /// <summary>
        /// 内訳名
        /// </summary>
        public string UtiwakeName { get; set; } = string.Empty;

        /// <summary>
        /// 棚番
        /// </summary>
        public string Tanaban { get; set; } = string.Empty;

        /// <summary>
        /// 品目CD
        /// </summary>
        public string HinmokuCode { get; set; } = string.Empty;

        /// <summary>
        /// 品目名
        /// </summary>
        public string HinmokuName { get; set; } = string.Empty;

        /// <summary>
        /// 口座名
        /// </summary>
        public string KouzaName { get; set; } = string.Empty;

        /// <summary>
        /// 単位
        /// </summary>
        public string Tani { get; set; } = string.Empty;

        /// <summary>
        /// 月初在庫
        /// </summary>
        public decimal SZaiko { get; set; } = 0;

        /// <summary>
        /// 当月入庫
        /// </summary>
        public decimal Nyuko { get; set; } = 0;

        /// <summary>
        /// EF出庫
        /// </summary>
        public decimal EFShukko { get; set; } = 0;

        /// <summary>
        /// LF出庫
        /// </summary>
        public decimal LFShukko { get; set; } = 0;

        /// <summary>
        /// CC出庫
        /// </summary>
        public decimal CCShukko { get; set; } = 0;

        /// <summary>
        /// その他
        /// </summary>
        public decimal OtherShukko { get; set; } = 0;

        /// <summary>
        /// 事業開発
        /// </summary>
        public decimal BusinessDevelopment { get; set; } = 0;

        /// <summary>
        /// 1次切断
        /// </summary>
        public decimal PrimaryCutting { get; set; } = 0;

        /// <summary>
        /// TD出庫
        /// </summary>
        public decimal TDShukko { get; set; } = 0;

        /// <summary>
        /// 2次切断
        /// </summary>
        public decimal SecondarycCutting { get; set; } = 0;

        /// <summary>
        /// 月末在庫
        /// </summary>
        public decimal EZaiko { get; set; } = 0;

        #endregion
    }
}
