using ViewModel;
namespace CKSI1080.ViewModel
{
    /// <summary>
    /// コンボボックス用ViewModel
    /// </summary>
    public class ComboBoxViewModel : ViewModelBase
    {
        /// <summary>
        /// キー
        /// </summary>
        private string key;

        /// <summary>
        /// 値
        /// </summary>
        private string value;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ComboBoxViewModel()
        {
        }

        /// <summary>
        /// キー
        /// </summary>
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                if (key != value)
                {
                    key = value;
                    OnPropertyChanged("Key");
                }
            }
        }

        /// <summary>
        /// 値
        /// </summary>
        public string Value
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    OnPropertyChanged("Value");
                }
            }
        }
    }
}
