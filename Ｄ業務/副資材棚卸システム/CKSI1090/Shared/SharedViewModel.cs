using CKSI1090.Shared.ViewModel;
using Core;
using System.Collections.Generic;
//*************************************************************************************
//
//   共有データ(ビューモデル用)
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090.Shared
{
    /// <summary>
    /// 共有データ(ビューモデル用)
    /// </summary>
    public class SharedViewModel : SingletonViewModelBase<SharedViewModel>
    {
        #region プロパティ

        /// <summary>
        /// ビジー状態
        /// </summary>
        public BusyStatus BusyStatus { get; } = new BusyStatus();

        /// <summary>
        /// 実施年月
        /// </summary>
        public OperationYearMonth OperationYearMonth { get; } = new OperationYearMonth();

        /// <summary>
        /// 作業誌区分
        /// </summary>
        public IDictionary<string, string> WorkNoteKbn { get; set; } = null;

        /// <summary>
        /// 向先
        /// </summary>
        public IDictionary<string, string> Mukesaki { get; set; } = null;

        /// <summary>
        /// 資材事務所スタッフか
        /// </summary>
        public bool IsSizaiOfficeStaff { get; set; } = false;

        #endregion

    }
}
