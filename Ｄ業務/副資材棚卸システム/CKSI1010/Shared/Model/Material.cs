//*************************************************************************************
//
//   品目
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
    /// 品目
    /// </summary>
    public class Material
    {
        #region プロパティ

        /// <summary>
        /// 品目コード
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 品目名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 費目
        /// </summary>
        public string Expense { get; set; } = string.Empty;

        /// <summary>
        /// 内訳
        /// </summary>
        public string Breakdown { get; set; } = string.Empty;

        /// <summary>
        /// 棚番
        /// </summary>
        public string Shelf { get; set; } = string.Empty;

        /// <summary>
        /// 単位
        /// </summary>
        public string Unit { get; set; } = string.Empty;

        /// <summary>
        /// 種別
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 水分
        /// </summary>
        public string Moisture { get; set; } = string.Empty;

        /// <summary>
        /// 検収明細
        /// </summary>
        public string Acceptance { get; set; } = string.Empty;

        /// <summary>
        /// 経理報告
        /// </summary>
        public string Report { get; set; } = string.Empty;

        /// <summary>
        /// 出庫位置
        /// </summary>
        public string IssuePlace { get; set; } = string.Empty;

        /// <summary>
        /// 向先
        /// </summary>
        public string PickupPlace { get; set; } = string.Empty;

        #endregion
    }
}
