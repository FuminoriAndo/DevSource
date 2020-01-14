using CKSI1050.ViewModel;
using DBManager.Model;
//*************************************************************************************
//
//   DBとViewModelのMapper
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1050.Common
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
        /// 棚卸操作のプロパティのコピー(ViewModel→Model)
        /// </summary>
        /// <param name="user">棚卸操作(ViewModel)</param>
        /// <param name="type">DB操作</param>
        /// <returns>棚卸操作(Model)</returns>
        internal static TanaorosiOperation CopyProperty(TanaorosiOperationInfo operationInfo, ProcessType type)
        {
            TanaorosiOperation operationInfoModel = new TanaorosiOperation();

            switch (type)
            {
                //挿入
                case ProcessType.Insert:
                //更新
                case ProcessType.Update:
                    operationInfoModel.SystemCategory = operationInfo.SystemCategory.TrimEnd();
                    operationInfoModel.OperationCode = operationInfo.OperationCode.TrimEnd();
                    operationInfoModel.OperationName = operationInfo.OperationName.TrimEnd();
                    break;
                //削除
                case ProcessType.Delete:
                    operationInfoModel.SystemCategory = operationInfo.SystemCategory;
                    operationInfoModel.OperationCode = operationInfo.OperationCode;
                    break;
                default:
                    break;
            }

            return operationInfoModel;
        }

        #endregion
    }
}
   
