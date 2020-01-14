//*************************************************************************************
//
//   Excel出力用の作業誌チェックリストのDTO
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090.Common.Excel.DTO
{
    /// <summary>
    /// Excel出力用の作業誌チェックリストのDTO
    /// </summary>
    public class WorkNoteCheckListDTO
    {
        #region プロパティ

        /// <summary>
        /// 作業日
        /// </summary>
        public bool IsChecked { get; set; } = false;

        /// <summary>
        /// 作業日
        /// </summary>
        public string OperationDate { get; set; } = string.Empty;

        /// <summary>
        /// 作業誌区分
        /// </summary>
        public string WorkNoteType { get; set; } = string.Empty;

        /// <summary>
        /// 品目CD
        /// </summary>
        public string HinmokuCode { get; set; } = string.Empty;

        /// <summary>
        /// 業者CD
        /// </summary>
        public string GyosyaCode { get; set; } = string.Empty;

        /// <summary>
        /// 向先
        /// </summary>
        public string Mukesaki { get; set; } = string.Empty;

        /// <summary>
        /// 品目名
        /// </summary>
        public string HinmokuName { get; set; } = string.Empty;

        /// <summary>
        /// 業者名
        /// </summary>
        public string GyosyaName { get; set; } = string.Empty;

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Amount { get; set; } = 0;

        /// <summary>
        /// 水分率
        /// </summary>
        public decimal Suibunritu { get; set; } = 0;

        /// <summary>
        /// 確認1
        /// </summary>
        public string ConfirmOne { get; set; } = string.Empty;

        /// <summary>
        /// 確認2
        /// </summary>
        public string ConfirmTwo { get; set; } = string.Empty;

        #endregion
    }
}

