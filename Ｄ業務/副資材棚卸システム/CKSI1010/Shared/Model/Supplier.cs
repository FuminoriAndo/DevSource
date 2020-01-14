//*************************************************************************************
//
//   業者
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
    /// 業者
    /// </summary>
    public class Supplier
    {
        #region プロパティ

        /// <summary>
        /// コード
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 業者名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 業者名(カナ)
        /// </summary>
        public string KanaName { get; set; } = string.Empty;

        /// <summary>
        /// 条件コード
        /// </summary>
        public string ConditionCode { get; set; } = string.Empty;

        /// <summary>
        /// 口座名
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 口座名(カナ)
        /// </summary>
        public string KanaAccount { get; set; } = string.Empty;

        #endregion
    }
}
