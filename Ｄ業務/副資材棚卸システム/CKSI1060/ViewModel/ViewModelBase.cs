using System.ComponentModel;
//*************************************************************************************
//
//   ビューモデルクラス(基底)
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   15.08.08              DSK吉武   　 新規作成
//
//*************************************************************************************
namespace CKSI1060.ViewModel
{
    /// <summary>
    /// ビューモデル(基底)
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region イベントハンドラ
        /// <summary>
        /// プロパティ変更時のイベントハンドラ
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// プロパティ変更時のイベントハンドラ
        /// </summary>
        /// <param name="name">プロパティ名</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
