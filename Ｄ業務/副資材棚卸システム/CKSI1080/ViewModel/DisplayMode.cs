//*************************************************************************************
//
//   画面モード
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
    /// 画面モード
    /// </summary>
    public class DisplayMode : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 部品倉庫かどうか
        /// </summary>
        private bool isPartsWarehouse;

        #endregion

        #region プロパティ

        /// <summary>
        /// 部品倉庫かどうか
        /// </summary>
        public bool IsPartsWarehouse
        {
            get
            {
                return isPartsWarehouse;
            }
            set
            {
                if (isPartsWarehouse != value)
                {
                    isPartsWarehouse = value;
                    OnPropertyChanged("IsPartsWarehouse");
                }
            }
        }
        
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DisplayMode()
        {
        }
        #endregion
    }

}
