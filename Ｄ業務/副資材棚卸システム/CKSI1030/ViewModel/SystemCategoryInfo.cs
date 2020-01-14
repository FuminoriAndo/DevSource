using ViewModel;
//*************************************************************************************
//
//   棚卸システム分類情報クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.02.01              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1030.ViewModel
{
    /// <summary>
    /// 棚卸システム分類
    /// </summary>
    public class SystemCategoryInfo : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// ユーザーコンテナ
        /// </summary>
        private ISystemCategoryContainer container;

        /// <summary>
        /// 変更されているか
        /// </summary>
        private bool isDirty;
        
        /// <summary>
        /// システム分類
        /// </summary>
        private string systemCategory;

        /// <summary>
        /// システム分類名
        /// </summary>
        private string systemCategoryName;

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
                    IsDirty = true;
                    if (container != null)
                    {
                        container.NotifyTanaorosiSystemCategoryModified();
                    }
                }
            }
        }
        /// <summary>
        /// システム分類名
        /// </summary>
        public string SystemCategoryName
        {
            get
            {
                return systemCategoryName;
            }
            set
            {
                if (systemCategoryName != value)
                {
                    systemCategoryName = value;
                    OnPropertyChanged("SystemCategoryName");
                    IsDirty = true;
                    if (container != null)
                    {
                        container.NotifyTanaorosiSystemCategoryModified();
                    }
                }
            }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SystemCategoryInfo(ISystemCategoryContainer container)
        {
            this.container = container;
        }

        #endregion
    }
}
