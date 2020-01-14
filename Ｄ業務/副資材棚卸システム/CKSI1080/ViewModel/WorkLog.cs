//*************************************************************************************
//
//   棚卸ログ情報クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.30              DSK吉武   　 新規作成
//
//*************************************************************************************
using ViewModel;
namespace CKSI1080.ViewModel
{
    /// <summary>
    /// 棚卸ログ情報
    /// </summary>
    public class WorkLog : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// システム分類
        /// </summary>
        private string systemCategory;

        /// <summary>
        /// 社員番号
        /// </summary>
        private string employeeNo;

        /// <summary>
        /// 社員名称
        /// </summary>
        private string employeeName;

         /// <summary>
        /// 作業日時
        /// </summary>
        private string operetionDate;

        /// <summary>
        /// 作業区分
        /// </summary>
        private string workKBN;
        
        /// <summary>
        /// 操作種別
        /// </summary>
        private string operateType;

        /// <summary>
        /// 操作内容
        /// </summary>
        private string opereteContent;

        /// <summary>
        /// 備考
        /// </summary>
        private string bikou;

        /// <summary>
        /// エラー内容
        /// </summary>
        private string errorContent;

         /// <summary>
        /// エラーコード
        /// </summary>
        private string errorCode;

        /// <summary>
        /// 資材区分
        /// </summary>
        private string shizaiKBN;

        /// <summary>
        /// 品目コード
        /// </summary>
        private string hinmokuCode;

        /// <summary>
        /// 業者コード
        /// </summary>
        private string gyosyaCode;

        /// <summary>
        /// 品目名
        /// </summary>
        private string hinmokuName;

        /// <summary>
        /// 業者名
        /// </summary>
        private string gyosyaName;

        /// <summary>
        /// 変更内容１
        /// </summary>
        private string updateContent1;

        /// <summary>
        /// 変更内容２
        /// </summary>
        private string updateContent2;

        /// <summary>
        /// 変更内容３
        /// </summary>
        private string updateContent3;

        #endregion

        #region プロパティ

        /// <summary>
        /// システム分類
        /// </summary>
        public string SystemCategory
        {
            get
            {
                return systemCategory;
            }
            set
            {
                if (systemCategory != value)
                {
                    systemCategory = value;
                    OnPropertyChanged("SystemCategory");
                }
            }
        }

        /// <summary>
        /// 社員番号
        /// </summary>
        public string EmployeeNo
        {
            get
            {
                return employeeNo;
            }
            set
            {
                if (employeeNo != value)
                {
                    employeeNo = value;
                    OnPropertyChanged("EmployeeNo");
                }
            }
        }

        /// <summary>
        /// 社員名
        /// </summary>
        public string EmployeeName
        {
            get
            {
                return employeeName;
            }
            set
            {
                if (employeeName != value)
                {
                    employeeName = value;
                    OnPropertyChanged("EmployeeName");
                }
            }
        }

        /// <summary>
        /// 作業日時
        /// </summary>
        public string OperetionDate
        {
            get
            {
                return operetionDate;
            }
            set
            {
                if (operetionDate != value)
                {
                    operetionDate = value;
                    OnPropertyChanged("OperetionDate");
                }
            }
        }

        /// <summary>
        /// 作業区分
        /// </summary>
        public string WorkKBN
        {
            get
            {
                return workKBN;
            }
            set
            {
                if (workKBN != value)
                {
                    workKBN = value;
                    OnPropertyChanged("WorkKBN");
                }
            }
        }

        /// <summary>
        /// 操作種別
        /// </summary>
        public string OperateType
        {
            get
            {
                return operateType;
            }
            set
            {
                if (operateType != value)
                {
                    operateType = value;
                    OnPropertyChanged("OperateType");
                }
            }
        }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string OpereteContent
        {
            get
            {
                return opereteContent;
            }
            set
            {
                if (opereteContent != value)
                {
                    opereteContent = value;
                    OnPropertyChanged("OpereteContent");
                }
            }
        }

        /// <summary>
        /// 備考
        /// </summary>
        public string Bikou
        {
            get
            {
                return bikou;
            }
            set
            {
                if (bikou != value)
                {
                    bikou = value;
                    OnPropertyChanged("Bikou");
                }
            }
        }

        /// <summary>
        /// エラー内容
        /// </summary>
        public string ErrorContent
        {
            get
            {
                return errorContent;
            }
            set
            {
                if (errorContent != value)
                {
                    errorContent = value;
                    OnPropertyChanged("ErrorContent");
                }
            }
        }

        /// <summary>
        /// エラーコード
        /// </summary>
        public string ErrorCode
        {
            get
            {
                return errorCode;
            }
            set
            {
                if (errorCode != value)
                {
                    errorCode = value;
                    OnPropertyChanged("ErrorCode");
                }
            }
        }

        /// <summary>
        /// 資材内容
        /// </summary>
        public string ShizaiKBN
        {
            get
            {
                return shizaiKBN;
            }
            set
            {
                if (shizaiKBN != value)
                {
                    shizaiKBN = value;
                    OnPropertyChanged("ShizaiKBN");
                }
            }
        }

        /// <summary>
        /// 品目コード
        /// </summary>
        public string HinmokuCode
        {
            get
            {
                return hinmokuCode;
            }
            set
            {
                if (hinmokuCode != value)
                {
                    hinmokuCode = value;
                    OnPropertyChanged("HinmokuCode");
                }
            }
        }

        /// <summary>
        /// 業者コード
        /// </summary>
        public string GyosyaCode
        {
            get
            {
                return gyosyaCode;
            }
            set
            {
                if (gyosyaCode != value)
                {
                    gyosyaCode = value;
                    OnPropertyChanged("GyosyaCode");
                }
            }
        }

        /// <summary>
        /// 品目名
        /// </summary>
        public string HinmokuName
        {
            get
            {
                return hinmokuName;
            }
            set
            {
                if (hinmokuName != value)
                {
                    hinmokuName = value;
                    OnPropertyChanged("HinmokuName");
                }
            }
        }

        /// <summary>
        /// 業者名
        /// </summary>
        public string GyosyaName
        {
            get
            {
                return gyosyaName;
            }
            set
            {
                if (gyosyaName != value)
                {
                    gyosyaName = value;
                    OnPropertyChanged("GyosyaName");
                }
            }
        }

        /// <summary>
        /// 変更内容１
        /// </summary>
        public string UpdateContent1
        {
            get
            {
                return updateContent1;
            }
            set
            {
                if (updateContent1 != value)
                {
                    updateContent1 = value;
                    OnPropertyChanged("UpdateContent1");
                }
            }
        }

        /// <summary>
        /// 変更内容２
        /// </summary>
        public string UpdateContent2
        {
            get
            {
                return updateContent2;
            }
            set
            {
                if (updateContent2 != value)
                {
                    updateContent2 = value;
                    OnPropertyChanged("UpdateContent2");
                }
            }
        }

        /// <summary>
        /// 変更内容３
        /// </summary>
        public string UpdateContent3
        {
            get
            {
                return updateContent3;
            }
            set
            {
                if (updateContent3 != value)
                {
                    updateContent3 = value;
                    OnPropertyChanged("UpdateContent3");
                }
            }
        }
        
        
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WorkLog()
        {
        }
        #endregion
    }

}
