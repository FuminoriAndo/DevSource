//*************************************************************************************
//
//   検索条件モデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************
using static CKSI1090.Common.Constants;

namespace CKSI1090.Shared.Model
{
    /// <summary>
    /// 検索条件モデル
    /// </summary>
    public class SearchCondition
    {

        #region プロパティ

        /// <summary>
        /// 作業日を検索に使用するか
        /// </summary>
        public bool UseOperationDate { get; set; } = false;

        /// <summary>
        /// 作業日の最小値
        /// </summary>
        public string MinOperationDate { get; set; } = string.Empty;

        /// <summary>
        /// 作業日の最大値
        /// </summary>
        public string MaxOperationDate { get; set; } = string.Empty;

        /// <summary>
        /// 作業日時の検索種類
        /// </summary>
        public OperationDateSearchTypes OperationDateSearchType { get; set; }

        /// <summary>
        /// 品目コードを検索に使用するか
        /// </summary>
        public bool UseHinmokuCode { get; set; } = false;

        /// <summary>
        /// 品目コードの最小値
        /// </summary>
        public string MinHinmokuCode { get; set; } = string.Empty;

        /// <summary>
        /// 品目コードの最大値
        /// </summary>
        public string MaxHinmokuCode { get; set; } = string.Empty;

        /// <summary>
        /// 品目コードの検索種類
        /// </summary>
        public HinmokuCodeSearchTypes HinmokuCodeSearchType { get; set; }

        /// <summary>
        /// 業者コードを検索に使用するか
        /// </summary>
        public bool UseGyosyaCode { get; set; } = false;

        /// <summary>
        /// 業者コードの最小値
        /// </summary>
        public string MinGyosyaCode { get; set; } = string.Empty;

        /// <summary>
        /// 業者コードの最大値
        /// </summary>
        public string MaxGyosyaCode { get; set; } = string.Empty;

        /// <summary>
        /// 業者コードの検索種類
        /// </summary>
        public GyosyaCodeSearchTypes GyosyaCodeSearchType { get; set; }

        /// <summary>
        /// 作業誌区分を検索に使用するか
        /// </summary>
        public bool UseWorkNoteKbn { get; set; } = false;

        /// <summary>
        /// 作業誌区分
        /// </summary>
        public string WorkNoteKbn { get; set; } = string.Empty;

        /// <summary>
        /// 向先を検索に使用するか
        /// </summary>
        public bool UseMukesaki { get; set; } = false;

        /// <summary>
        /// 向先
        /// </summary>
        public string Mukesaki { get; set; } = string.Empty;

        /// <summary>
        /// 承認を検索に使用するか
        /// </summary>
        public bool UseApproval { get; set; } = false;

        /// <summary>
        /// 承認
        /// </summary>
        public bool Approval { get; set; } = false;

        #endregion
    }
}
