using Core;
using System.Collections.Generic;
using DBManager.Model;
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
        /// 社員情報
        /// </summary>
        public IDictionary<string, EmployeeInfo> EmployeeInfo { get; set; }

        /// <summary>
        /// 部署名
        /// </summary>
        public IDictionary<string, string> DeploymentName { get; set; }

        /// <summary>
        /// システム分類
        /// </summary>
        public IDictionary<string, string> SystemCategory { get; set; }

        #endregion
 
    }
}
