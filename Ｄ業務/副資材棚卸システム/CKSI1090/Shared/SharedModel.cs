using CKSI1090.Shared.Model;
using Core;
//*************************************************************************************
//
//   共有データ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090.Shared
{
    /// <summary>
    /// 共有データ
    /// </summary>
    public class SharedModel : Singleton<SharedModel>
    {
        #region プロパティ

        /// 起動引数
        /// </summary>
        public StartupArgs StartupArgs { get; } = new StartupArgs();

        #endregion

    }
}
