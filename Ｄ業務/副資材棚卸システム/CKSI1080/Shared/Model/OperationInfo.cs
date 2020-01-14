using Core;
using System.Collections.Generic;
//*************************************************************************************
//
//   操作情報
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace Shared.Model
{
    /// <summary>
    /// 操作情報
    /// </summary>
    public class OperationInfo : Singleton<OperationInfo>
    {

        #region プロパティ

        /// <summary>
        /// 作業区分
        /// </summary>
        public IDictionary<string, string> WorkKCategory { get; set; }

        /// <summary>
        /// 操作種類
        /// </summary>
        public IDictionary<string, string> OperationType { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public IDictionary<string, string> OperationContent { get; set; }

        /// <summary>
        /// 社員名
        /// </summary>
        public IDictionary<string, string> EmployeeName { get; set; }

        /// <summary>
        /// 所属部署コード
        /// </summary>
        public IDictionary<string, string> DeploymentCode { get; set; }

        /// <summary>
        /// 所属部署名
        /// </summary>
        public IDictionary<string, string> DeploymentName { get; set; }

        #endregion
 
    }
}
