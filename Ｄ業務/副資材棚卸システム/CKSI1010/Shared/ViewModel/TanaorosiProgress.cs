using GalaSoft.MvvmLight;
//*************************************************************************************
//
//   棚卸進捗状況
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Shared.ViewModel
{
    /// <summary>
    /// 棚卸進捗状況
    /// </summary>
    public class TanaorosiProgress : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 操作No
        /// </summary>
        private int operationNo;

        /// <summary>
        /// 操作状況
        /// </summary>
        private string operationCondition;

        #endregion

        #region プロパティ

        /// <summary>
        /// 操作No
        /// </summary>
        public int OperationNo
        {
            get { return operationNo; }
            set { Set(ref operationNo, value); }
        }

        /// <summary>
        /// 操作状況
        /// </summary>
        public string OperationCondition
        {
            get { return operationCondition; }
            set { Set(ref operationCondition, value); }
        }

        #endregion
    }
}
