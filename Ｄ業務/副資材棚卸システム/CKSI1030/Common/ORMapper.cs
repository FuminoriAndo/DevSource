using CKSI1030.ViewModel;
using DBManager.Model;
//*************************************************************************************
//
//   ViewModelとModelのO/RMapper
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.02.01              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1030.Common
{
    /// <summary>
    /// ViewModelとModelのO/RMapper
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
        /// システム分類のプロパティのコピー(ViewModel→Model)
        /// </summary>
        /// <param name="user">システム分類(ViewModel)</param>
        /// <param name="type">DB操作</param>
        /// <returns>システム分類(Model)</returns>
        internal static TanaorosiSystemCategory CopyProperty(SystemCategoryInfo systemCategory, ProcessType type)
        {
            TanaorosiSystemCategory systemCategoryModel = new TanaorosiSystemCategory();

            switch (type)
            {
                //挿入
                case ProcessType.Insert:
                //更新
                case ProcessType.Update:
                    systemCategoryModel.SystemCategory = systemCategory.SystemCategory.TrimEnd();
                    systemCategoryModel.SystemCategoryName = systemCategory.SystemCategoryName.TrimEnd();
                    break;
                //削除
                case ProcessType.Delete:
                    systemCategoryModel.SystemCategory = systemCategory.SystemCategory;
                    break;
                default:
                    break;
            }
            ;

            return systemCategoryModel;
        }

        #endregion
    }
}
   
