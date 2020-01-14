using CKSI1010.Shared.ViewModel;
using Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//*************************************************************************************
//
//   共有データ(ビューモデル用)
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Shared
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
        /// 操作一覧
        /// </summary>
        public ObservableCollection<Common.Operation> Operations { get; } = new ObservableCollection<Common.Operation>();

        /// <summary>
        /// 実施年月
        /// </summary>
        public OperationYearMonth OperationYearMonth { get; } = new OperationYearMonth();

        /// <summary>
        /// 印刷対象操作
        /// </summary>
        public List<string> PrintOerations { get; } = new List<string>();

        /// <summary>
        /// 確定対象操作
        /// </summary>
        public List<string> FixOerations { get; } = new List<string>();

        /// <summary>
        /// 更新対象操作
        /// </summary>
        public List<string> UpdateOerations { get; } = new List<string>();

        /// <summary>
        /// 修正対象操作
        /// </summary>
        public List<string> ModifyOerations { get; } = new List<string>();

        /// <summary>
        /// 操作状況対象外操作
        /// </summary>
        public List<string> ConditionExcludedOperation { get; } = new List<string>();

        /// <summary>
        /// 向先
        /// </summary>
        public IDictionary<string, string> Mukesaki { get; set; }

        /// <summary>
        /// 現在の操作
        /// </summary>
        public Common.Operation CurrentOperation { get; set; }

        /// <summary>
        /// 前の操作
        /// </summary>
        public Common.Operation PreviousOperation { get; set; }

        #endregion

    }
}
