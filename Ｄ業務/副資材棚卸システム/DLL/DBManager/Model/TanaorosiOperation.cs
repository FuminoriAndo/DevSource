//*************************************************************************************
//
//   棚卸操作
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
    /// 棚卸操作
    /// </summary>
    public class TanaorosiOperation
    {
        #region プロパティ
        /// <summary>
        /// システム分類区分
        /// </summary>
        public string SystemCategory { get; set; }
        /// <summary>
        /// 操作コード
        /// </summary>
        public string OperationCode { get; set; }
        /// <summary>
        /// 操作名
        /// </summary>
        public string OperationName { get; set; }
        #endregion
    }
}
