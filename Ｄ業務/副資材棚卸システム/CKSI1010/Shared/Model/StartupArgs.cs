using CKSI1010.Core;
using Core;
//*************************************************************************************
//
//   起動時の引数
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Shared.Model
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
        public string EmployeeCode { get; set; } = string.Empty;

        /// <summary>
        /// 職位コード
        /// </summary>
        public string PositionCode { get; set; } = string.Empty;

        /// <summary>
        /// 所属コード
        /// </summary>
        public string BelongingCode { get; set; } = string.Empty;

        #endregion
    }
}
