//*************************************************************************************
//
//   棚卸操作コンテナ(interface)
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1050.ViewModel
{
    /// <summary>
    /// 棚卸操作コンテナ
    /// </summary>
    public interface ITanaorosiOperationContainer
    {
        #region Interface
        void NotifyTanaorosiOperationModified();
        #endregion
    }
}
