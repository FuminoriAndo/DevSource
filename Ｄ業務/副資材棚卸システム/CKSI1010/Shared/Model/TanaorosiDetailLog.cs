//*************************************************************************************
//
//   棚卸詳細ログ
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
    /// 棚卸詳細ログ
    /// </summary>
    public class TanaorosiDetailLog
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
        /// 資材区分
        /// </summary>
        public string SizaiKbn { get; set; } = string.Empty;

        /// <summary>
        /// 品目コード
        /// </summary>
        public string HinmokuCode { get; set; } = string.Empty;

        /// <summary>
        /// 業者コード
        /// </summary>
        public string GyosyaCode { get; set; } = string.Empty;

        /// <summary>
        /// 品目名
        /// </summary>
        public string HinmokuNM { get; set; } = string.Empty;

        /// <summary>
        /// 業者名
        /// </summary>
        public string GyosyaNM { get; set; } = string.Empty;

        /// <summary>
        /// 備考
        /// </summary>
        public string Bikou { get; set; } = null;

        /// <summary>
        /// 変更内容1
        /// </summary>
        public string UpdateContent1 { get; set; } = null;

        /// <summary>
        /// 変更内容2
        /// </summary>
        public string UpdateContent2 { get; set; } = null;

        /// <summary>
        /// 変更内容3
        /// </summary>
        public string UpdateContent3 { get; set; } = null;

        /// <summary>
        /// エラー内容
        /// </summary>
        public string ErrorContent { get; set; } = null;

        #endregion
    }
}
