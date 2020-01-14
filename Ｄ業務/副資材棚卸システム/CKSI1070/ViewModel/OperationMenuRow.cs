using System.Collections.ObjectModel;
using ViewModel;
//*************************************************************************************
//
//   棚卸操作グループ情報クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   15.08.08              DSK吉武   　 新規作成
//
//*************************************************************************************
namespace CKSI1070.ViewModel
{
    /// <summary>
    /// 棚卸操作グループ
    /// </summary>
    public class OperationMenuRow : ViewModelBase
    {
        #region フィールド
        /// <summary>
        /// ユーザーコンテナ
        /// </summary>
        private IOperationMenuContainer container;

        /// <summary>
        /// 変更されているか
        /// </summary>
        private bool isDirty;
        
        /// <summary>
        /// 社員所属コード
        /// </summary>
        private string syainSZCode;

        /// <summary>
        /// システム分類区分
        /// </summary>
        private string systemCategory;

        /// <summary>
        /// グループ区分
        /// </summary>
        private string groupKbn;

        /// <summary>
        /// 作業区分
        /// </summary>
        private string workCategory;

        /// <summary>
        /// 操作種別
        /// </summary>
        private string operationType;

        /// <summary>
        /// 操作順
        /// </summary>
        private string operationOrder;

        /// <summary>
        /// 操作順(ソート用)
        /// </summary>
        private int operationOrderForSort;

        /// <summary>
        /// 操作コード
        /// </summary>
        private string operationCD;

        /// <summary>
        /// 操作コードコンボボックス
        /// </summary>
        private ObservableCollection<ComboBoxViewModel> operations;

        #endregion

        #region プロパティ
        /// <summary>
        /// 変更されているか
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return isDirty;
            }
            set
            {
                if (isDirty != value)
                {
                    isDirty = value;
                    OnPropertyChanged("IsDirty");
                }
            }
        }

        /// <summary>
        /// 社員所属コード
        /// </summary>
        public string SyainSZCode
        {
            get
            {
                return syainSZCode;
            }
            set
            {
                if (syainSZCode != value)
                {
                    syainSZCode = value;
                    OnPropertyChanged("SyainSZCode");
                    IsDirty = true;
                    if (container != null)
                    {
                        container.NotifyTanaorosiOperationMenuModified();
                    }
                }
            }
        }
        /// <summary>
        /// システム分類区分
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
                    IsDirty = true;
                    if (container != null)
                    {
                        container.NotifyTanaorosiOperationMenuModified();
                    }
                }
            }
        }
        /// <summary>
        /// グループ区分
        /// </summary>
        public string GroupKbn
        {
            get
            {
                return groupKbn;
            }
            set
            {
                if (groupKbn != value)
                {
                    groupKbn = value;
                    OnPropertyChanged("GroupKbn");
                    IsDirty = true;
                    if (container != null)
                    {
                        container.NotifyTanaorosiOperationMenuModified();
                    }
                }
            }
        }

        /// <summary>
        /// 作業区分
        /// </summary>
        public string WorkCategory
        {
            get
            {
                return workCategory;
            }
            set
            {
                if (workCategory != value)
                {
                    workCategory = value;
                    OnPropertyChanged("WorkCategory");
                    IsDirty = true;
                    if (container != null)
                    {
                        container.NotifyTanaorosiOperationMenuModified();
                    }
                }
            }
        }

        /// <summary>
        /// 操作種別
        /// </summary>
        public string OperationType
        {
            get
            {
                return operationType;
            }
            set
            {
                if (operationType != value)
                {
                    operationType = value;
                    OnPropertyChanged("OperationType");
                    IsDirty = true;
                    if (container != null)
                    {
                        container.NotifyTanaorosiOperationMenuModified();
                    }
                }
            }
        }

        /// <summary>
        /// 操作順
        /// </summary>
        public string OperationOrder
        {
            get
            {
                return operationOrder;
            }
            set
            {
                if (operationOrder != value)
                {
                    operationOrder = value;
                    OnPropertyChanged("OperationOrder");
                    IsDirty = true;
                    if (container != null)
                    {
                        container.NotifyTanaorosiOperationMenuModified();
                    }

                    if(operationOrder != null)

                        if (operationOrder.Trim() != string.Empty)
                        {
                            OperationOrderForSort = int.Parse(operationOrder);
                        }
                }
            }
        }

        /// <summary>
        /// 操作順(ソート用)
        /// </summary>
        public int OperationOrderForSort
        {
            get
            {
                return operationOrderForSort;
            }
            set
            {
                if (operationOrderForSort != value)
                {
                    operationOrderForSort = value;
                    OnPropertyChanged("OperationOrderForSort");
                }
            }
        }

        /// <summary>
        /// 操作コード
        /// </summary>
        public string OperationCD
        {
            get
            {
                return operationCD;
            }
            set
            {
                if (operationCD != value)
                {
                    operationCD = value;
                    OnPropertyChanged("OperationCD");
                    IsDirty = true;
                    if (container != null)
                    {
                        container.NotifyTanaorosiOperationMenuModified();
                    }
                }
            }
        }


        /// <summary>
        /// 操作コードコンボボックス
        /// </summary>
        public ObservableCollection<ComboBoxViewModel> Operations
        {
            get
            {
                return operations;
            }
            set
            {
                if (operations != value)
                {
                    operations = value;
                    OnPropertyChanged("Operations");
                    IsDirty = true;
                    if (container != null)
                    {
                        container.NotifyTanaorosiOperationMenuModified();
                    }
                }
            }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OperationMenuRow(IOperationMenuContainer container)
        {
            this.container = container;
        }
        #endregion
    }
}
