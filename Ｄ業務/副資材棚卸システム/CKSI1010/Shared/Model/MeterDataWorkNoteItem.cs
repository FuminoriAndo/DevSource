//*************************************************************************************
//
//   作業誌の項目
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
    /// 作業誌項目（品目CD、業者CD）
    /// </summary>
    public class MeterDataWorkNoteItem
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
        /// 向先
        /// </summary>
        public string Mukesaki { get; set; } = string.Empty;

        /// <summary>
        /// 区分
        /// </summary>
        public string Kubun { get; set; } = string.Empty;

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Amount { get; set; } = 0;

        #endregion
    }
}
