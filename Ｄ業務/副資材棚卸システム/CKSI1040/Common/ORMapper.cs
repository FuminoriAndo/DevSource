using System.Collections.Generic;
using CKSI1040.ViewModel;
using DBManager.Model;
//*************************************************************************************
//
//   DBとViewModelのMapper
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.02.01              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1040.Common
{
    /// <summary>
    /// DBとViewModelのMapper
    /// </summary>
    internal class ORMapper
    {
        /// <summary>
        /// DB操作
        /// </summary>
        internal enum ProcessType
        {
            /// <summary>
            /// 挿入
            /// </summary>
            Insert,
            /// <summary>
            /// 更新
            /// </summary>
            Update,
            /// <summary>
            /// 削除
            /// </summary>
            Delete,
            /// <summary>
            /// 検索
            /// </summary>
            Select
        }
        
        #region メソッド

        /// <summary>
        /// グループ区分のプロパティのコピー(ViewModel→Model)
        /// </summary>
        /// <param name="row">グループ区分(ViewModel)</param>
        /// <param name="type">DB操作</param>
        /// <returns>システム分類(Model)</returns>
        internal static TanaorosiGroupKbn CopyProperty(GroupKbnRow row, ProcessType type)
        {
            TanaorosiGroupKbn tanaorosiGroupKbn = new TanaorosiGroupKbn();

            switch (type)
            {
                //挿入
                case ProcessType.Insert:
                //更新
                case ProcessType.Update:
                    tanaorosiGroupKbn.SystemCategory = row.SystemCategory.TrimEnd();
                    tanaorosiGroupKbn.GroupKbn = row.GroupKbn.TrimEnd();
                    tanaorosiGroupKbn.GroupKbnName = row.GroupKbnName.TrimEnd();
                    break;
                //削除
                case ProcessType.Delete:
                    tanaorosiGroupKbn.SystemCategory = row.SystemCategory;
                    tanaorosiGroupKbn.GroupKbn = row.GroupKbn;
                    break;
                default:
                    break;
            }
            ;

            return tanaorosiGroupKbn;
        }

        #endregion
    }
}
   
