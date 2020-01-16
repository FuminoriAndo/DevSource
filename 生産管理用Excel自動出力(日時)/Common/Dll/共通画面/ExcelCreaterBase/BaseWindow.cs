//*************************************************************************************
//
//   共通画面
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ExcelCreatorBase
{
    /// <summary>
    /// 共通画面
    /// </summary>
    public partial class BaseWindow : Form
    {
        #region フィールド

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BaseWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region イベント

        /// <summary>
        /// フォーム表示イベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        /// <remarks>フォーム起動時１回のみ動くイベント</remarks>
        private void Form_Shown(object sender, EventArgs e)
        {
            // 初期処理
            if (Initialize(sender, e))
            {
                this.progressControl1.Excute();
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// 閉じる「×」ボタン押下時のイベント
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        /// <remarks>閉じる「×」ボタン押下時に動くイベント</remarks>
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            // 終了処理
            Terminate(sender, e);
        }

        #endregion

        #region BackGroundWorkerイベント

        /// <summary>
        /// ProgressControlのDoWorkイベントハンドラ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        /// <remarks>ここで時間のかかる処理を行う</remarks>
        private void BackgroundWorker1_DoWork(BackgroundWorker bgWorker, DoWorkEventArgs e)
        {
            // バックグラウンド処理
            DoInBackground(bgWorker, e);
        }

        /// <summary>
        /// ProgressControlのRunWorkerCompletedイベントハンドラ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        /// <remarks>処理が終わったときに呼び出される</remarks>
        private void BackgroundWorker1_RunWorkerCompleted(
            BackgroundWorker bgWorker, RunWorkerCompletedEventArgs e)
        {
            // バックグラウンド処理完了後に行う処理
            OnPostExecute(bgWorker, e);
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期処理
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        /// <returns>結果</returns>
        protected virtual bool Initialize(object sender, EventArgs e)
        {
            return true;
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        /// <returns>結果</returns>
        protected virtual bool Terminate(object sender, FormClosingEventArgs e)
        {
            return true;
        }

        /// <summary>
        /// バックグラウンド処理
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        protected virtual void DoInBackground(BackgroundWorker bgWorker, DoWorkEventArgs e)
        {
        }

        /// <summary>
        /// バックグラウンド処理完了後に行う処理
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        protected virtual void OnPostExecute(BackgroundWorker bgWorker, RunWorkerCompletedEventArgs e)
        {
        }

        #endregion

    }
}
