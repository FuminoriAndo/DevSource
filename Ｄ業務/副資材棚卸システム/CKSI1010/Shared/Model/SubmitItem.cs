//*************************************************************************************
//
//   期末提出用項目(費目内訳棚番単位)
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
    /// 期末提出用項目(費目内訳棚番単位)
    /// </summary>
    public class SubmitItem
    {
        #region プロパティ

        /// <summary>
        /// 費目コード
        /// </summary>
        public string ExpenseCode { get; set; } = string.Empty;

        /// <summary>
        /// 費目名
        /// </summary>
        public string ExpenseName { get; set; } = string.Empty;

        /// <summary>
        /// 内訳コード
        /// </summary>
        public string BreakdownCode { get; set; } = string.Empty;

        /// <summary>
        /// 内訳名
        /// </summary>
        public string BreakdownName { get; set; } = string.Empty;

        /// <summary>
        /// 棚番
        /// </summary>
        public string Shelf { get; set; } = string.Empty;

        /// <summary>
        /// 棚番名1
        /// </summary>
        public string ShelfName1 { get; set; } = string.Empty;

        /// <summary>
        /// 棚番名2
        /// </summary>
        public string ShelfName2 { get; set; } = string.Empty;

        /// <summary>
        /// 単位
        /// </summary>
        public string Unit { get; set; } = string.Empty;

        #endregion
    }
}
