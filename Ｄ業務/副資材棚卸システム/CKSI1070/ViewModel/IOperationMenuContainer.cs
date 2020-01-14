//*************************************************************************************
//
//   棚卸操作メニューコンテナ(interface)
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   15.08.08              DSK吉武   　 新規作成
//
//*************************************************************************************
namespace CKSI1070.ViewModel
{
    /// <summary>
    /// 棚卸操作メニューコンテナ
    /// </summary>
    public interface IOperationMenuContainer
    {
        #region Interface
        void NotifyTanaorosiOperationMenuModified();
        #endregion
    }
}
