//*************************************************************************************
//
//   作業誌チェックリストのレコードビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************
using CKSI1090.Shared;
using GalaSoft.MvvmLight;

namespace CKSI1090.Operation.WorkNoteCheckList.ViewModel
{
    /// <summary>
    /// 作業誌チェックリストのレコードビューモデル
    /// </summary>
    public class WorkNoteRecordViewModel: ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 作業誌チェックリストのコンテナ
        /// </summary>
        private IWorkNoteRecordContainer container;

        /// <summary>
        /// 編集可能か
        /// </summary>
        private bool canEdit = true;

        /// <summary>
        /// 変更されているか
        /// </summary>
        private bool isDirty = false;

        /// <summary>
        /// 選択されているか
        /// </summary>
        private bool isChecked = false;

        /// <summary>
        /// 作業日
        /// </summary>
        private string operationDate = string.Empty;

        /// <summary>
        /// 連番
        /// </summary>
        private string seq = string.Empty;

        /// <summary>
        /// 作業誌区分
        /// </summary>
        private string workNoteType = string.Empty;

        /// <summary>
        /// 品目CD
        /// </summary>
        private string hinmokuCode = string.Empty;

        /// <summary>
        /// 業者CD
        /// </summary>
        private string gyosyaCode = string.Empty;

        /// <summary>
        /// 向先
        /// </summary>
        private string mukesaki = string.Empty;

        /// <summary>
        /// 品目名
        /// </summary>
        private string hinmokuName = string.Empty;

        /// <summary>
        /// 業者名
        /// </summary>
        private string gyosyaName = string.Empty;

        /// <summary>
        /// 数量
        /// </summary>
        private decimal amount = 0;

        /// <summary>
        /// 水分率
        /// </summary>
        private decimal suibunritu = 0;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WorkNoteRecordViewModel(IWorkNoteRecordContainer container)
        {
            this.container = container;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 編集可能か
        /// </summary>
        public bool CanEdit
        {
            get { return canEdit; }
            set { Set(ref this.canEdit, value); }
        }

        /// <summary>
        /// 変更されているか
        /// </summary>
        public bool IsDirty
        {
            get { return isDirty; }
            set { Set(ref this.isDirty, value); }
        }

        /// <summary>
        /// 選択されているか
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                Set(ref this.isChecked, value);
                IsDirty = true;
                if (container != null)
                {
                    container.NotifyWorkNoteRecordModified();
                }
            }
        }

        /// <summary>
        /// 作業日
        /// </summary>
        public string OperationDate
        {
            get { return operationDate; }
            set
            {
                Set(ref this.operationDate, value);
                if(int.Parse(operationDate.Substring(0,6)) < SharedViewModel.Instance.OperationYearMonth.YearMonth)
                {
                    CanEdit = false;
                }
            }
        }

        /// <summary>
        /// 連番
        /// </summary>
        public string Seq
        {
            get { return seq; }
            set { Set(ref this.seq, value); }
        }

        /// <summary>
        /// 作業誌区分
        /// </summary>
        public string WorkNoteType
        {
            get { return workNoteType; }
            set { Set(ref this.workNoteType, value); }
        }

        /// <summary>
        /// 品目CD
        /// </summary>
        public string HinmokuCode
        {
            get { return hinmokuCode; }
            set { Set(ref this.hinmokuCode, value); }
        }

        /// <summary>
        /// 業者CD
        /// </summary>
        public string GyosyaCode
        {
            get { return gyosyaCode; }
            set { Set(ref this.gyosyaCode, value); }
        }

        /// <summary>
        /// 向先
        /// </summary>
        public string Mukesaki
        {
            get { return mukesaki; }
            set { Set(ref this.mukesaki, value); }
        }

        /// <summary>
        /// 品目名
        /// </summary>
        public string HinmokuName
        {
            get { return hinmokuName; }
            set { Set(ref this.hinmokuName, value); }
        }

        /// <summary>
        /// 業者名
        /// </summary>
        public string GyosyaName
        {
            get { return gyosyaName; }
            set { Set(ref this.gyosyaName, value); }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Amount
        {
            get { return amount; }
            set { Set(ref this.amount, value); }
        }

        /// <summary>
        /// 水分率
        /// </summary>
        public decimal Suibunritu
        {
            get { return suibunritu; }
            set { Set(ref this.suibunritu, value); }
        }

        #endregion

    }
}
