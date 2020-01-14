//*************************************************************************************
//
//   棚卸操作グループ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.11.16              DSK   　     新規作成
//
//*************************************************************************************
namespace DBManager.Model
{
    /// <summary>
    /// 棚卸操作グループ
    /// </summary>
    public class TanaorosiOpeartionGroup
    {
        #region プロパティ
        /// <summary>
        /// 社員コード
        /// </summary>
        public string SyainCode { get; set; }
        ///<summary>
		/// <summary>
        /// 社員所属コード
        /// </summary>
        public string SyainSZCode { get; set; }
        /// <summary>
        /// システム分類
        /// </summary>
        public string SystemCategory { get; set; }
		/// <summary>
        /// グループ区分
        /// </summary>
        public string GroupKbn { get; set; }
        #endregion
    }
}
