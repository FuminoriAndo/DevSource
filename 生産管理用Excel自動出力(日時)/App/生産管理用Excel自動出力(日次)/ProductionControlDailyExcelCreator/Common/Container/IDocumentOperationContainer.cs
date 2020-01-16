//*************************************************************************************
//
//   ドキュメントに対する操作完了時のコンテナ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   19.08.31    1         DSK          新規作成
//
//*************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductionControlDailyExcelCreator.Common.Container
{
    /// <summary>
    /// ドキュメントに対する操作完了時のコンテナ
    /// </summary>
    public interface IDocumentOperationContainer
    {
        #region インターフェース

        /// <summary>
        /// Excel出力完了の通知
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        void NotifyOutputExcel(string filePath);

        #endregion
    }
}
