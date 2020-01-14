//*************************************************************************************
//
//   棚卸操作グループコンテナ(interface)
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   15.08.08              DSK吉武   　 新規作成
//
//*************************************************************************************
namespace CKSI1060.ViewModel
{
    /// <summary>
    /// 棚卸操作グループコンテナ
    /// </summary>
    public interface IOperationGroupContainer
    {
        #region Interface
        void NotifyTanaorosiOperationGroupModified();
        #endregion
    }
}
