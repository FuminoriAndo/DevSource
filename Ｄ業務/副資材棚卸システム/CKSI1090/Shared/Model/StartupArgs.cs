using Core;
//*************************************************************************************
//
//   起動時の引数
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090.Shared.Model
{
    /// <summary>
    /// 起動時の引数
    /// </summary>
    public class StartupArgs : Singleton<StartupArgs>
    {
        #region プロパティ

        /// <summary>
        /// 社員コード
        /// </summary>
        internal string EmployeeCode { get; set; } = string.Empty;

        /// <summary>
        /// 職位コード
        /// </summary>
        internal string PositionCode { get; set; } = string.Empty;

        /// <summary>
        /// 所属コード
        /// </summary>
        internal string BelongingCode { get; set; } = string.Empty;

        #endregion
    }
}
