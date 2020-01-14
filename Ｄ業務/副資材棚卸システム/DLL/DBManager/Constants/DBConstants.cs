//*************************************************************************************
//
//   DB用定義クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.11.16              DSK   　     新規作成
//
//*************************************************************************************
namespace DBManager.Constants
{
    /// <summary>
    /// DB用定義
    /// </summary>
    public static class DBConstants
    {
        #region フィールド
        /// <summary>
        /// SQL操作結果(OK)
        /// </summary>
        public const int SQL_RESULT_OK = 1;

        /// <summary>
        /// SQL操作結果(ALREADY_EXIST)
        /// </summary>
        public const int SQL_RESULT_ALREADY_EXISTS = 2;

        /// <summary>
        /// SQL操作結果(NG)
        /// </summary>
        public const int SQL_RESULT_NG = -1;

        /// <summary>
        /// 例月棚卸
        /// </summary>
        public const string REIGETSU = "例月棚卸";

        /// <summary>
        /// 期末棚卸
        /// </summary>
        public const string KIMATSU = "期末棚卸";
        #endregion

        #region 列挙体
        /// <summary>
        /// DB種類
        /// </summary>
        public enum DBType
        {
            /// <summary>
            /// Oracle
            /// </summary>
            ORACLE
        }

        /// <summary>
        /// 棚卸種別
        /// </summary>
        public enum TanaorosiType : int
        {
            REIGETSU = 0,
            KIMATSU = 1
        }
        #endregion
    }
}
