//*************************************************************************************
//
//   カスタムログのマネージャ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ   担当者    修正内容
//   18.09.01             DSK吉武   新規作成
//
//*************************************************************************************
using System;

namespace ProductionControlDailyExcelCreator.Log
{
    /// <summary>
    /// カスタムログのマネージャ
    /// </summary>
    internal class CustomLogManager
    {
        #region フィールド

        /// <summary>
        /// シングルトン
        /// </summary>
        private static CustomLogManager singleton = null;

        /// <summary>
        /// カスタムロガー
        /// </summary>
        private CustomLogger logger = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logFolderName">ログフォルダ名</param>
        /// <param name="logDefaultFlieName">ログファイル名(デフォルト)</param>
        private CustomLogManager(string logFolderName, string logDefaultFileName)
        {
            try
            {
                logger = new CustomLogger(logFolderName, logDefaultFileName);
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region メソッド

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <param name="logFolderName">ログフォルダ名</param>
        /// <param name="logDefaultFlieName">ログファイル名(デフォルト)</param>
        /// <returns>CustomLogManagerのインスタンス</returns>
        internal static CustomLogManager GetInstance(string logFolderName, string logDefaultFileName)
        {
            try
            {
                if (singleton == null) singleton = new CustomLogManager(logFolderName, logDefaultFileName);
                return singleton;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ログ書込み
        /// </summary>
        /// <param name="message">ログ出力メッセージ</param>
        internal void Write(string message)
        {
            try
            {
                this.logger.Write(message);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ログの世代管理の削除
        /// </summary>
        internal void CleanUp()
        {
            try
            {
                this.logger.CleanUp();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        internal void Terminate()
        {
            try
            {
                this.logger.Terminate();
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
