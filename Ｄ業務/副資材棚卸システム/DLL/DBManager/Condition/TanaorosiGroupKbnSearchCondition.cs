//*************************************************************************************
//
//   棚卸システム分類情報検索条件クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   15.08.08              DSK吉武   　 新規作成
//
//*************************************************************************************
namespace DBManager.Condition
{
    /// <summary>
    /// 棚卸システム分類情報検索条件
    /// </summary>
    public class TanaorosiGroupKbnSearchCondition
    {
        #region フィールド
        /// <summary>
        /// クラス名
        /// </summary>
        internal static readonly string ClassName = "SystemCategorySearchCondition";
        #endregion

        #region 列挙体
        /// <summary>
        /// 検索対象
        /// </summary>
        public enum SearchType
        {
            /// <summary>
            /// 全て(検索条件なし)
            /// </summary>
            All_NoCondition,
            /// <summary>
            /// システム分類
            /// </summary>
            SystemCategory
        }
        #endregion

        #region プロパティ
        /// <summary>
        /// 検索対象
        /// </summary>
        public SearchType SearchTarget { get; set; }

        /// <summary>
        /// システム分類
        /// </summary>
        public string SystemCategory { get; set; }

        /// <summary>
        /// 社員名称
        /// </summary>
        public string SystemCategoryName { get; set; }

        #endregion
    }
}
