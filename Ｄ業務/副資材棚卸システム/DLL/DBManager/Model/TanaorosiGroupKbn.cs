//*************************************************************************************
//
//   棚卸グループ区分
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
    /// 棚卸グループ区分
    /// </summary>
    public class TanaorosiGroupKbn
    {
        #region プロパティ
        /// <summary>
        /// システム分類
        /// </summary>
        public string SystemCategory { get; set; }
        /// <summary>
        /// グループ区分
        /// </summary>
        public string GroupKbn { get; set; }
        /// <summary>
        /// グループ区分名
        /// </summary>
        public string GroupKbnName { get; set; }
        #endregion
    }
}
