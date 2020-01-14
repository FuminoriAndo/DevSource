
//*************************************************************************************
//
//   社員情報
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace DBManager.Model
{
    /// <summary>
    /// 社員情報
    /// </summary>
    public class EmployeeInfo
    {

        #region プロパティ

        /// <summary>
        /// 社員名
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 所属部署コード
        /// </summary>
        public string DeploymentCode { get; set; }

        /// <summary>
        /// 所属部署名
        /// </summary>
        public string DeploymentName { get; set; }

        #endregion
 
    }
}
