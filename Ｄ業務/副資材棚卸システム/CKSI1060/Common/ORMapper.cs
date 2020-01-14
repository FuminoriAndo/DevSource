using System.Collections.Generic;
using CKSI1060.ViewModel;
using DBManager.Condition;
using DBManager.Model;
using Shared;
//*************************************************************************************
//
//   DBとViewModelのMapper
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   15.08.08              DSK吉武   　 新規作成
//
//*************************************************************************************
namespace CKSI1060.Common
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
        /// 操作グループのプロパティのコピー(ViewModel→Model)
        /// </summary>
        /// <param name="row">グループ区分(ViewModel)</param>
        /// <param name="type">DB操作</param>
        /// <returns>システム分類(Model)</returns>
        internal static TanaorosiOpeartionGroup CopyProperty(OperationGroupRow row, ProcessType type)
        {
            TanaorosiOpeartionGroup tanaorosiOpeartionGroup = new TanaorosiOpeartionGroup();

            switch (type)
            {
                //挿入
                case ProcessType.Insert:
                //更新
                case ProcessType.Update:
                    tanaorosiOpeartionGroup.SyainCode = row.SyainCode.TrimEnd();
                    tanaorosiOpeartionGroup.SyainSZCode = row.SyainSZCode.TrimEnd();
                    tanaorosiOpeartionGroup.SystemCategory = row.SystemCategory.TrimEnd();
                    tanaorosiOpeartionGroup.GroupKbn = row.GroupKbn.TrimEnd();
                    break;
                //削除
                case ProcessType.Delete:
                    tanaorosiOpeartionGroup.SyainCode = row.SyainCode.TrimEnd();
                    tanaorosiOpeartionGroup.SyainSZCode = row.SyainSZCode.TrimEnd();
                    tanaorosiOpeartionGroup.SystemCategory = row.SystemCategory.TrimEnd();
                    break;
                default:
                    break;
            }
            ;

            return tanaorosiOpeartionGroup;
        }

        #endregion
    }
}
   
