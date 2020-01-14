using GalaSoft.MvvmLight;
using static CKSI1090.Common.Constants;
//*************************************************************************************
//
//   検索条件ビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1090.Operation.WorkNoteCheckList.ViewModel
{
    /// <summary>
    /// 検索条件ビューモデル
    /// </summary>
    public class SearchConditionViewModel : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 作業日を検索に使用するか
        /// </summary>
        private bool useOperationDate = false;

        /// <summary>
        /// 作業日の最小値
        /// </summary>
        private string minOperationDate = string.Empty;

        /// <summary>
        /// 作業日の最大値
        /// </summary>
        private string maxOperationDate = string.Empty;

        /// <summary>
        /// 品目コードを検索に使用するか
        /// </summary>
        private bool useHinmokuCode = false;

        /// <summary>
        /// 品目コードの最小値
        /// </summary>
        private string minHinmokuCode = string.Empty;

        /// <summary>
        /// 品目コードの最大値
        /// </summary>
        private string maxHinmokuCode = string.Empty;

        /// <summary>
        /// 業者コードを検索に使用するか
        /// </summary>
        private bool useGyosyaCode = false;

        /// <summary>
        /// 業者コードの最小値
        /// </summary>
        private string minGyosyaCode = string.Empty;

        /// <summary>
        /// 業者コードの最大値
        /// </summary>
        private string maxGyosyaCode = string.Empty;

        /// <summary>
        /// 作業誌区分を検索に使用するか
        /// </summary>
        private bool useWorkNoteKbn = false;

        /// <summary>
        /// 作業誌区分
        /// </summary>
        private string workNoteKbn = string.Empty;

        /// <summary>
        /// 向先を検索に使用するか
        /// </summary>
        private bool useMukesaki = false;

        /// <summary>
        /// 向先
        /// </summary>
        private string mukesaki = string.Empty;

        /// <summary>
        /// 承認を検索に使用するか
        /// </summary>
        private bool useApproval = false;

        /// <summary>
        /// 承認
        /// </summary>
        private bool approval = false;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SearchConditionViewModel() { }

        #endregion

        #region プロパティ

        /// <summary>
        /// 作業日を検索に使用するか
        /// </summary>
        public bool UseOperationDate
        {
            get { return this.useOperationDate; }
            set { Set(ref this.useOperationDate, value); }
        }

        /// <summary>
        /// 作業日の最小値
        /// </summary>
        public string MinOperationDate
        {
            get { return this.minOperationDate; }
            set { Set(ref this.minOperationDate, value); }
        }

        /// <summary>
        /// 作業日の最大値
        /// </summary>
        public string MaxOperationDate
        {
            get { return this.maxOperationDate; }
            set { Set(ref this.maxOperationDate, value); }
        }

        /// <summary>
        /// 作業日時の検索種類
        /// </summary>
        public OperationDateSearchTypes OperationDateSearchType { get; set; }

        /// <summary>
        /// 品目コードを検索に使用するか
        /// </summary>
        public bool UseHinmokuCode
        {
            get { return this.useHinmokuCode; }
            set { Set(ref this.useHinmokuCode, value); }
        }

        /// <summary>
        /// 品目コードの最小値
        /// </summary>
        public string MinHinmokuCode
        {
            get { return this.minHinmokuCode; }
            set { Set(ref this.minHinmokuCode, value); }
        }

        /// <summary>
        /// 品目コードの最大値
        /// </summary>
        public string MaxHinmokuCode
        {
            get { return this.maxHinmokuCode; }
            set { Set(ref this.maxHinmokuCode, value); }
        }

        /// <summary>
        /// 品目コードの検索種類
        /// </summary>
        public HinmokuCodeSearchTypes HinmokuCodeSearchType { get; set; }

        /// <summary>
        /// 業者コードを検索に使用するか
        /// </summary>
        public bool UseGyosyaCode
        {
            get { return this.useGyosyaCode; }
            set { Set(ref this.useGyosyaCode, value); }
        }

        /// <summary>
        /// 業者コードの最小値
        /// </summary>
        public string MinGyosyaCode
        {
            get { return this.minGyosyaCode; }
            set { Set(ref this.minGyosyaCode, value); }
        }

        /// <summary>
        /// 業者コードの最大値
        /// </summary>
        public string MaxGyosyaCode
        {
            get { return this.maxGyosyaCode; }
            set { Set(ref this.maxGyosyaCode, value); }
        }

        /// <summary>
        /// 業者コードの検索種類
        /// </summary>
        public GyosyaCodeSearchTypes GyosyaCodeSearchType { get; set; }

        /// <summary>
        /// 作業誌区分を検索に使用するか
        /// </summary>
        public bool UseWorkNoteKbn
        {
            get { return this.useWorkNoteKbn; }
            set { Set(ref this.useWorkNoteKbn, value); }
        }

        /// <summary>
        /// 作業誌区分
        /// </summary>
        public string WorkNoteKbn
        {
            get { return this.workNoteKbn; }
            set { Set(ref this.workNoteKbn, value); }
        }

        /// <summary>
        /// 向先を検索に使用するか
        /// </summary>
        public bool UseMukesaki
        {
            get { return this.useMukesaki; }
            set { Set(ref this.useMukesaki, value); }
        }

        /// <summary>
        /// 向先
        /// </summary>
        public string Mukesaki
        {
            get { return this.mukesaki; }
            set { Set(ref this.mukesaki, value); }
        }

        /// <summary>
        /// 承認を検索に使用するか
        /// </summary>
        public bool UseApproval
        {
            get { return this.useApproval; }
            set { Set(ref this.useApproval, value); }
        }

        /// <summary>
        /// 承認
        /// </summary>
        public bool Approval
        {
            get { return this.approval; }
            set { Set(ref this.approval, value); }
        }

        #endregion

    }

}
