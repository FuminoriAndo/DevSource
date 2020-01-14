//*************************************************************************************
//
//   棚卸システム分類
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
    /// 棚卸システム分類
    /// </summary>
    public class TanaorosiSystemCategory
    {
        #region プロパティ
        /// <summary>
        /// システム分類
        /// </summary>
        public string SystemCategory { get; set; }
        /// <summary>
        /// システム分類名
        /// </summary>
        public string SystemCategoryName { get; set; }
        #endregion
    }
}
