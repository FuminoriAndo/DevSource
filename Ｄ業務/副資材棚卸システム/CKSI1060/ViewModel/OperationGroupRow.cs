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
using System.Collections.ObjectModel;
namespace CKSI1060.ViewModel
{
    /// <summary>
    /// 棚卸操作グループ
    /// </summary>
    public class OperationGroupRow : ViewModelBase
    {
        #region フィールド
        /// <summary>
        /// ユーザーコンテナ
        /// </summary>
        private IOperationGroupContainer container;

        /// <summary>
        /// 変更されているか
        /// </summary>
        private bool isDirty;
        
        /// <summary>
        /// 社員コード
        /// </summary>
        private string syainCode;

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
        /// グループ区分コンボボックス
        /// </summary>
        private ObservableCollection<ComboBoxViewModel> groupKbns;

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
        /// 社員コード
        /// </summary>
        public string SyainCode
        {
            get
            {
                return syainCode;
            }
            set
            {
                if (syainCode != value)
                {
                    syainCode = value;
                    OnPropertyChanged("SyainCode");
                    IsDirty = true;
                    if (container != null)
                    {
                        container.NotifyTanaorosiOperationGroupModified();
                    }
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
                        container.NotifyTanaorosiOperationGroupModified();
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
                        container.NotifyTanaorosiOperationGroupModified();
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
                        container.NotifyTanaorosiOperationGroupModified();
                    }
                }
            }
        }

        /// <summary>
        /// グループ区分コンボボックス
        /// </summary>
        public ObservableCollection<ComboBoxViewModel> GroupKbns
        {

            get
            {
                return groupKbns;
            }
            set
            {
                if (groupKbns != value)
                {
                    groupKbns = value;
                    OnPropertyChanged("GroupKbns");
                    IsDirty = true;
                    if (container != null)
                    {
                        container.NotifyTanaorosiOperationGroupModified();
                    }
                }
            }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OperationGroupRow(IOperationGroupContainer container)
        {
            this.container = container;
        }
        #endregion
    }
}
