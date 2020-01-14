//*************************************************************************************
//
//   棚卸ログ
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
    /// 棚卸ログ
    /// </summary>
    public class TanaorosiLog
    {
        #region プロパティ

        /// <summary>
        /// システム分類
        /// </summary>
        public string SystemCategory { get; set; } = string.Empty;

        /// <summary>
        /// ログ種類
        /// </summary>
        public string LogType { get; set; } = string.Empty;

        /// <summary>
        /// 社員コード
        /// </summary>
        public string SyainCode { get; set; } = string.Empty;

        /// <summary>
        /// 作業日時
        /// </summary>
        public string OperationYMD { get; set; } = string.Empty;

        /// <summary>
        /// 作業区分
        /// </summary>
        public string WorkKbn { get; set; } = string.Empty;

        /// <summary>
        /// 操作種別
        /// </summary>
        public string OperationType { get; set; } = string.Empty;

        /// <summary>
        /// 操作内容
        /// </summary>
        public string OperationContent { get; set; } = string.Empty;

        /// <summary>
        /// 備考
        /// </summary>
        public string Bikou { get; set; } = null;

        /// <summary>
        /// エラー内容
        /// </summary>
        public string ErrorContent { get; set; } = null;

        /// <summary>
        /// エラーコード
        /// </summary>
        public string ErrorCode { get; set; } = null;

        #endregion
    }
}
