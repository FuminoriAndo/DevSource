using GalaSoft.MvvmLight;
//*************************************************************************************
//
//   手順項目
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.ShowFlow.ViewModel
{
    /// <summary>
    /// 手順項目
    /// </summary>
    public class FlowItem : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 処理番号
        /// </summary>
        private int no;

        #endregion

        #region プロパティ

        /// <summary>
        /// 処理番号
        /// </summary>
        public int No
        {
            get { return no; }
            set { Set(ref no, value); }
        }

        /// <summary>
        /// 棚卸操作情報
        /// </summary>
        public CKSI1010.Common.Operation Operation { get; internal set; }

        #endregion
    }
}
