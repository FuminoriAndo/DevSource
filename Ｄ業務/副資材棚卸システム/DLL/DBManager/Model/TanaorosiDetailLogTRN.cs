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
    public class TanaorosiDetailLogTRN
    {
        #region プロパティ
        /// <summary>
        /// ログ種類
        /// </summary>
        public string LogType { get; set; }

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
        /// 資材区分
        /// </summary>
        public string SizaiKBN { get; set; }

        /// <summary>
        /// 品目CD
        /// </summary>
        public string HinmokuCD { get; set; }

        /// <summary>
        /// 業者CD
        /// </summary>
        public string GyosyaCD { get; set; }

        /// <summary>
        /// 品目名
        /// </summary>
        public string HinmokuNM { get; set; }

        /// <summary>
        /// 業者名
        /// </summary>
        public string GyosyaNM { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        public string Bikou { get; set; }

        /// <summary>
        /// 変更内容１
        /// </summary>
        public string UpdateContent1 { get; set; }

        /// <summary>
        /// 変更内容２
        /// </summary>
        public string UpdateContent2 { get; set; }

        /// <summary>
        /// 変更内容３
        /// </summary>
        public string UpdateContent3 { get; set; }

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
