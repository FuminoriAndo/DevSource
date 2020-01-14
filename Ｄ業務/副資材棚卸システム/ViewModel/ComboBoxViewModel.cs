using GalaSoft.MvvmLight;
//*************************************************************************************
//
//   コンボボックス用ビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************
namespace ViewModel
{
    /// <summary>
    /// コンボボックス用ビューモデル
    /// </summary>
    public class ComboBoxViewModel : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// キー
        /// </summary>
        private string key = string.Empty;

        /// <summary>
        /// 値
        /// </summary>
        private string value = string.Empty;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ComboBoxViewModel()
        {
        }

        #endregion

        /// <summary>
        /// キー
        /// </summary>
        public string Key
        {
            get { return this.key; }
            set { Set(ref this.key, value); }
        }

        /// <summary>
        /// 値
        /// </summary>
        public string Value
        {
            get { return this.value; }
            set { Set(ref this.value, value); }
        }
    }
}
