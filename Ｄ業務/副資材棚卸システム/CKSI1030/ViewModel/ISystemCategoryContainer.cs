//*************************************************************************************
//
//   棚卸システム分類コンテナ(interface)
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.02.01              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1030.ViewModel
{
    /// <summary>
    /// 棚卸システム分類コンテナ
    /// </summary>
    public interface ISystemCategoryContainer
    {
        #region インターフェース

        void NotifyTanaorosiSystemCategoryModified();

        #endregion
    }
}
