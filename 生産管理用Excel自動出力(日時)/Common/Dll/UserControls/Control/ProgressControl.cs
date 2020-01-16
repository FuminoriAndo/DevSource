//*************************************************************************************
//
//   進捗バーコントロール
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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserControls.Control
{
    public partial class ProgressControl : UserControl
    {
        #region フィールド
        /// <summary>
        /// ボタン文字(動作前)
        /// </summary>
        private string buttonText = string.Empty;

        /// <summary>
        /// ボタン文字(動作中)
        /// </summary>
        private string buttonTextAct = string.Empty;

        /// <summary>
        /// 実行引数
        /// </summary>
        private object argment = null;

        #endregion

        #region プロパティ

        /// <summary>
        /// 実行引数
        /// </summary>
        public object Argment
        {
            get
            {
               return  argment;
            }
            set
            {
                argment = value;
            }
        }
        #endregion

        #region コンストラクタ
        public ProgressControl()
        {
            InitializeComponent();
        }
        #endregion

        #region メソッド

        /// <summary>
        /// 実行
        /// </summary>
        public void Excute()
        {
            try
            {
                // 更新が行われているときは、何もしない
                if (backgroundWorker1.IsBusy)
                    return;

                this.Cursor = Cursors.WaitCursor;
                // BackgroundWorkerのProgressChangedイベントが発生するようにする
                backgroundWorker1.WorkerReportsProgress = true;
                // キャンセル可能にする
                backgroundWorker1.WorkerSupportsCancellation = true;

                // DoWorkで取得できるパラメータを指定して処理を開始する
                // パラメータが必要なければ省略できる
                backgroundWorker1.RunWorkerAsync(Argment);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// メッセージ表示
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        public void DisplayProgressMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.Update();
        }
        #endregion

        #region BackGroundWorker_CallbackEvent

        /// <summary>
        /// DoWorkイベントハンドラ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        public delegate void DoWorkHandler(BackgroundWorker bgWorker, DoWorkEventArgs e);
        public event DoWorkHandler DoWork;

        /// <summary>
        /// ProgressChangedイベントハンドラ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        /// <remarks>コントロールの操作は必ずここで行い、DoWorkでは絶対にしない</remarks>
        public delegate void ProgressChangedHandler(BackgroundWorker bgWorker, ProgressChangedEventArgs e);
        public event ProgressChangedHandler ProgressChanged;

        /// <summary>
        /// RunWorkerCompletedイベントハンドラ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        /// <remarks>処理が終わったときに呼び出される</remarks>
        public delegate void RunWorkerCompletedHandler(BackgroundWorker bgWorker, RunWorkerCompletedEventArgs e);
        public event RunWorkerCompletedHandler RunWorkerCompleted;
        #endregion
            
        #region BackGroundWorker_OwnEvent
        /// <summary>
        /// BackgroundWorker1のDoWorkイベントハンドラ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        /// <remarks>ここで時間のかかる処理を行う</remarks>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // 共通実装(前)
            // なし

            // イベント
            if (DoWork != null) DoWork((BackgroundWorker)sender, e);

            // 共通実装(後)
            // なし
        }

        /// <summary>
        /// BackgroundWorker1のProgressChangedイベントハンドラ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        /// <remarks>コントロールの操作は必ずここで行い、DoWorkでは絶対にしない</remarks>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // 共通実装(前)
            progressBar1.Value = e.ProgressPercentage;
            DisplayProgressMessage(e.UserState.ToString());

            // イベント
            if (ProgressChanged != null) ProgressChanged((BackgroundWorker)sender, e);

            // 共通実装(後)
            // なし
        }

        /// <summary>
        /// BackgroundWorker1のRunWorkerCompletedイベントハンドラ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        /// <remarks>処理が終わったときに呼び出される</remarks>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblMessage.Text = string.Empty;
            this.Cursor = Cursors.Default;

            // イベント
            if (RunWorkerCompleted != null) RunWorkerCompleted((BackgroundWorker)sender, e);
        }
        #endregion

    }
}
