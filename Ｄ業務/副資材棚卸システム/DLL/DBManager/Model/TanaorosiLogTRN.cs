//*************************************************************************************
//
//   棚卸ログトラン
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
    /// ログトラン
    /// </summary>
    public class TanaorosiLogTRN
    {
        #region プロパティ

        /// <summary>
        /// システム分類
        /// </summary>
        public string SystemCategory { get; set; }

        /// <summary>
        /// 社員コード
        /// </summary>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// 社員名
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 作業日時
        /// </summary>
        public string OperetionDate { get; set; }

        /// <summary>
        /// 作業区分
        /// </summary>
        public string WorkKBN { get; set; }

        /// <summary>
        /// 操作種別
        /// </summary>
        public string OperateType { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string OpereteContent { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        public string Bikou { get; set; }

        /// <summary>
        /// エラー内容
        /// </summary>
        public string ErrorContent { get; set; }

        /// <summary>
        /// エラーコード
        /// </summary>
        public string ErrorCode { get; set; }

        #endregion
    }
}
