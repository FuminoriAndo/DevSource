//*************************************************************************************
//
//   操作
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
    /// 操作
    /// </summary>
    public class OperationInfo
    {
        #region プロパティ

        /// <summary>
        /// 操作コード
        /// </summary>
        public int Code { get; set; } = 0;

        /// <summary>
        /// 操作名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 作業区分
        /// </summary>
        public string WorkType { get; set; } = string.Empty;

        #endregion

    }
}
