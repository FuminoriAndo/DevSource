using ViewModel;
//*************************************************************************************
//
//   棚卸グループ区分情報クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.02.01              DSK       　 新規作成
//
//*************************************************************************************
namespace CKSI1040.ViewModel
{
    /// <summary>
    /// 棚卸グループ区分
    /// </summary>
    public class GroupKbnRow : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// ユーザーコンテナ
        /// </summary>
        private IGroupKbnContainer container;

        /// <summary>
        /// 変更されているか
        /// </summary>
        private bool isDirty;

        /// <summary>
        /// システム分類区分
        /// </summary>
        private string systemCategory;
        
        /// <summary>
        /// グループ区分
        /// </summary>
        private string groupKbn;

        /// <summary>
        /// グループ区分名
        /// </summary>
        private string groupKbnName;

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
                        container.NotifyTanaorosiGroupKbnModified();
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
                        container.NotifyTanaorosiGroupKbnModified();
                    }
                }
            }
        }
        /// <summary>
        /// グループ区分名
        /// </summary>
        public string GroupKbnName
        {
            get
            {
                return groupKbnName;
            }
            set
            {
                if (groupKbnName != value)
                {
                    groupKbnName = value;
                    OnPropertyChanged("GroupKbnName");
                    IsDirty = true;
                    if (container != null)
                    {
                        container.NotifyTanaorosiGroupKbnModified();
                    }
                }
            }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GroupKbnRow(IGroupKbnContainer container)
        {
            this.container = container;
        }

        #endregion
    }
}
