//*************************************************************************************
//
//   棚卸ログ検索条件クラス
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
    /// ログ検索条件
    /// </summary>
    public class SearchCondition : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// システム分類
        /// </summary>
        private string systemCategory;

        /// <summary>
        /// ログ種別
        /// </summary>
        private string logType;

        /// <summary>
        /// 社員番号を検索に使用するか
        /// </summary>
        private bool useEmployeeNo;

        /// <summary>
        /// 社員番号の最小値
        /// </summary>
        private string employeeNo;

        /// <summary>
        /// 作業日時を検索に使用するか
        /// </summary>
        private bool useOperationDate;

        /// <summary>
        /// 作業日時の最小値
        /// </summary>
        private string minOperationDate;

        /// <summary>
        /// 作業日時の最大値
        /// </summary>
        private string maxOperationDate;

        /// <summary>
        /// 入力作業区分を検索に使用するか
        /// </summary>
        private bool useWorkKbn;

        /// <summary>
        /// 入力作業区分
        /// </summary>
        private string workKbn;

        /// <summary>
        /// 操作種別を検索に使用するか
        /// </summary>
        private bool useOperateType;

        /// <summary>
        /// 操作種別
        /// </summary>
        private string operateType;

        /// <summary>
        /// 操作内容を検索に使用するか
        /// </summary>
        private bool useOperateContent;

        /// <summary>
        /// 操作内容
        /// </summary>
        private string operateContent;

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
        /// ログ種別
        /// </summary>
        public string LogType
        {
            get
            {
                return logType;
            }
            set
            {
                if (logType != value)
                {
                    logType = value;
                    OnPropertyChanged("LogType");
                }
            }
        }

        /// <summary>
        /// 社員番号を検索に使用するか
        /// </summary>
        public bool UseEmployeeNo
        {
            get
            {
                return useEmployeeNo;
            }
            set
            {
                if (useEmployeeNo != value)
                {
                    useEmployeeNo = value;
                    OnPropertyChanged("UseEmployeeNo");
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
        /// 作業日時を検索に使用するか
        /// </summary>
        public bool UseOperationDate
        {
            get
            {
                return useOperationDate;
            }
            set
            {
                if (useOperationDate != value)
                {
                    useOperationDate = value;
                    OnPropertyChanged("UseOperationDate");
                }
            }
        }

        /// <summary>
        /// 作業日時の最小値
        /// </summary>
        public string MinOperationDate
        {
            get
            {
                return minOperationDate;
            }
            set
            {
                if (minOperationDate != value)
                {
                    minOperationDate = value;
                    OnPropertyChanged("MinOperationDate");
                }
            }
        }

        /// <summary>
        /// 作業日時の最大値
        /// </summary>
        public string MaxOperationDate
        {
            get
            {
                return maxOperationDate;
            }
            set
            {
                if (maxOperationDate != value)
                {
                    maxOperationDate = value;
                    OnPropertyChanged("MaxOperationDate");
                }
            }
        }

        /// <summary>
        /// 入力作業区分を検索に使用するか
        /// </summary>
        public bool UseWorkKbn
        {
            get
            {
                return useWorkKbn;
            }
            set
            {
                if (useWorkKbn != value)
                {
                    useWorkKbn = value;
                    OnPropertyChanged("UseWorkKbn");
                }
            }
        }

        /// <summary>
        /// 入力作業区分
        /// </summary>
        public string WorkKbn
        {
            get
            {
                return workKbn;
            }
            set
            {
                if (workKbn != value)
                {
                    workKbn = value;
                    OnPropertyChanged("WorkKbn");
                }
            }
        }

        /// <summary>
        /// 操作種別を検索に使用するか
        /// </summary>
        public bool UseOperateType
        {
            get
            {
                return useOperateType;
            }
            set
            {
                if (useOperateType != value)
                {
                    useOperateType = value;
                    OnPropertyChanged("UseOperateType");
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
        /// 操作内容を検索に使用するか
        /// </summary>
        public bool UseOperateContent
        {
            get
            {
                return useOperateContent;
            }
            set
            {
                if (useOperateContent != value)
                {
                    useOperateContent = value;
                    OnPropertyChanged("UseOperateContent");
                }
            }
        }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string OperateContent
        {
            get
            {
                return operateContent;
            }
            set
            {
                if (operateContent != value)
                {
                    operateContent = value;
                    OnPropertyChanged("OperateContent");
                }
            }
        }
        
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SearchCondition()
        {
        }
        #endregion
    }

}
