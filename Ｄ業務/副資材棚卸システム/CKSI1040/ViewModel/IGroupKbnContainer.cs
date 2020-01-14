//*************************************************************************************
//
//   棚卸グループ区分コンテナ(interface)
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.02.01              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1040.ViewModel
{
    /// <summary>
    /// 棚卸グループ区分コンテナ
    /// </summary>
    public interface IGroupKbnContainer
    {
        #region インターフェース

        void NotifyTanaorosiGroupKbnModified();

        #endregion
    }
}
