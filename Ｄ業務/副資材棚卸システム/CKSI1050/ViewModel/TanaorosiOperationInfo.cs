//*************************************************************************************
//
//   棚卸操作情報クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1050.ViewModel
{
    /// <summary>
    /// 棚卸操作情報
    /// </summary>
    public class TanaorosiOperationInfo : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// ユーザーコンテナ
        /// </summary>
        private ITanaorosiOperationContainer container;

        /// <summary>
        /// 変更されているか
        /// </summary>
        private bool isDirty;

        /// <summary>
        /// システム分類区分
        /// </summary>
        private string systemCategory;
        
        /// <summary>
        /// 操作コード
        /// </summary>
        private string operationCode;

        /// <summary>
        /// 操作名
        /// </summary>
        private string operationName;

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
                        container.NotifyTanaorosiOperationModified();
                    }
                }
            }
        }

        /// <summary>
        /// 操作コード
        /// </summary>
        public string OperationCode
        {
            get
            {
                return operationCode;
            }
            set
            {
                if (operationCode != value)
                {
                    operationCode = value;
                    OnPropertyChanged("OperationCode");
                    IsDirty = true;
                    if (container != null)
                    {
                        container.NotifyTanaorosiOperationModified();
                    }
                }
            }
        }
        /// <summary>
        /// 棚卸操作名
        /// </summary>
        public string OperationName
        {
            get
            {
                return operationName;
            }
            set
            {
                if (operationName != value)
                {
                    operationName = value;
                    OnPropertyChanged("OperationName");
                    IsDirty = true;
                    if (container != null)
                    {
                        container.NotifyTanaorosiOperationModified();
                    }
                }
            }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TanaorosiOperationInfo(ITanaorosiOperationContainer container)
        {
            this.container = container;
        }
        #endregion
    }
}
