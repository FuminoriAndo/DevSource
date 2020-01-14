using System.Collections.Generic;
using CKSI1070.ViewModel;
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
namespace CKSI1070.Common
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
        /// 操作メニューのプロパティのコピー(ViewModel→Model)
        /// </summary>
        /// <param name="row">操作メニュー(ViewModel)</param>
        /// <param name="type">DB操作</param>
        /// <returns>システム分類(Model)</returns>
        internal static TanaorosiOperationMenu CopyProperty(OperationMenuRow row, ProcessType type)
        {
            TanaorosiOperationMenu tanaorosiOperationMenu = new TanaorosiOperationMenu();

            switch (type)
            {
                //挿入
                case ProcessType.Insert:
                //更新
                case ProcessType.Update:
                    tanaorosiOperationMenu.SyainSZCode = row.SyainSZCode.TrimEnd().PadRight(5, ' ');
                    tanaorosiOperationMenu.SystemCategory = row.SystemCategory.TrimEnd().PadRight(2, ' ');
                    tanaorosiOperationMenu.GroupKbn = row.GroupKbn.TrimEnd().PadRight(2, ' ');
                    tanaorosiOperationMenu.WorkCategory = row.WorkCategory.TrimEnd().PadRight(2, ' ');
                    tanaorosiOperationMenu.OperationType = int.Parse(row.OperationType.TrimEnd());
                    tanaorosiOperationMenu.OperationOrder = int.Parse(row.OperationOrder.TrimEnd());
                    tanaorosiOperationMenu.OperationCode = int.Parse(row.OperationCD.TrimEnd());
                    break;
                //削除
                case ProcessType.Delete:
                    tanaorosiOperationMenu.SyainSZCode = row.SyainSZCode.TrimEnd().PadRight(5, ' ');
                    tanaorosiOperationMenu.SystemCategory = row.SystemCategory.TrimEnd().PadRight(2, ' ');
                    tanaorosiOperationMenu.GroupKbn = row.GroupKbn.TrimEnd().PadRight(2, ' ');
                    tanaorosiOperationMenu.WorkCategory = row.WorkCategory.TrimEnd().PadRight(2, ' ');
                    tanaorosiOperationMenu.OperationType = int.Parse(row.OperationType.TrimEnd());
                    tanaorosiOperationMenu.OperationOrder = int.Parse(row.OperationOrder.TrimEnd());
                    break;
                default:
                    break;
            }

            return tanaorosiOperationMenu;
        }

        #endregion
    }
}
   
