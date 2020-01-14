//*************************************************************************************
//
//   棚卸ログ検索条件クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.30             DSK吉武   　 新規作成
//
//*************************************************************************************
namespace DBManager.Condition
{
    /// <summary>
    /// 棚卸ログ検索条件
    /// </summary>
    public class LogSearchCondition
    {
        #region フィールド
        /// <summary>
        /// クラス名
        /// </summary>
        internal static readonly string ClassName = "LogSearchCondition";
        #endregion

        #region 列挙体
        /// <summary>
        /// 作業日時の検索種類
        /// </summary>
        public enum OperationDateConditions
        {
            /// <summary>
            /// 最小
            /// </summary>
            Min,
            /// <summary>
            /// 最大
            /// </summary>
            Max,
            /// <summary>
            /// 最小～最大
            /// </summary>
            MinMax,
            /// <summary>
            /// なし
            /// </summary>
            Noting
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// システム分類
        /// </summary>
        public string SystemCategory { get; set; }

        /// <summary>
        /// ログ種別
        /// </summary>
        public string LogType { get; set; }

        /// <summary>
        /// 社員番号を検索に使用するか
        /// </summary>
        public bool UseEmployeeNo { get; set; }

        /// <summary>
        /// 社員番号
        /// </summary>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// 作業日時を検索に使用するか
        /// </summary>
        public bool UseOperationDate { get; set; }

        /// <summary>
        /// 作業日時の検索種類
        /// </summary>
        public OperationDateConditions OperationDateCondition { get; set; }

        /// <summary>
        /// 作業日時の最小値
        /// </summary>
        public string MinOperationDate { get; set; }

        /// <summary>
        /// 作業日時の最大値
        /// </summary>
        public string MaxOperationDate { get; set; }

        /// <summary>
        /// 作業日時
        /// </summary>
        public string OperationDate { get; set; }

        /// <summary>
        /// 入力作業を検索に使用するか
        /// </summary>
        public bool UseWorkKbn { get; set; }

        /// <summary>
        /// 入力作業
        /// </summary>
        public string WorkKbn { get; set; }

        /// <summary>
        /// 操作種別を検索に使用するか
        /// </summary>
        public bool UseOperateType { get; set; }

        /// <summary>
        /// 操作種別
        /// </summary>
        public string OperateType { get; set; }

        /// <summary>
        /// 操作内容を検索に使用するか
        /// </summary>
        public bool UseOperateContent { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string OperateContent { get; set; }
        
        #endregion

        #region コンストラクタ
        public LogSearchCondition()
        {
            OperationDateCondition = OperationDateConditions.Noting;
        }
        #endregion
    }
}
