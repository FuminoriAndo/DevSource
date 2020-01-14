//*************************************************************************************
//
//   棚卸操作メニュー
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
    /// 棚卸操作メニュー
    /// </summary>
    public class TanaorosiOperationMenu
    {
        #region プロパティ
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
        /// <summary>
        /// 作業区分
        /// </summary>
        public string WorkCategory { get; set; }
        /// <summary>
        /// 操作種類
        /// </summary>
        public int OperationType { get; set; }
        /// <summary>
        /// 操作順
        /// </summary>
		public int OperationOrder { get; set; }
        /// <summary>
        /// 操作コード
        /// </summary>
        public int OperationCode { get; set; }
		
        #endregion
    }
}
