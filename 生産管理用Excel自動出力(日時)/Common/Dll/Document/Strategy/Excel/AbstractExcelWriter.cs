//*************************************************************************************
//
//   Excel出力用データ書込みのストラテジ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;

namespace Document.Strategy.Excel
{
    /// <summary>
    /// Excel出力用データ書込みのストラテジ
    /// </summary>
    public abstract class AbstractExcelWriter : AbstractDocumentWriterBase, IDisposable
    {
        #region プロパティ

        /// <summary>
        /// 現在時刻
        /// </summary>
        public DateTime DateTimeNow { get; set; }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期処理
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        /// 書込み前処理
        /// </summary>
        public override void PreWrite()
        {
        }

        /// <summary>
        /// ヘッダーの書込み
        /// </summary>
        /// <typeparam name="T">ヘッダーを継承した任意のオブジェクトの型</typeparam>
        /// <param name="header">ファイルのヘッダー情報</param>
        public override void WriteHeader<T>(T header)
        {
        }

        /// <summary>
        /// 詳細の書込み
        /// </summary>
        /// <typeparam name="T">詳細を継承した任意のオブジェクトの型</typeparam>
        /// <param name="detail">ファイルの詳細情報</param>
        public override void WriteDetail<T>(IList<T> detail)
        {
        }

        /// <summary>
        /// フッターの書込み
        /// </summary>
        /// <typeparam name="T">フッターを継承した任意のオブジェクトの型</typeparam>
        /// <param name="footer">ファイルのフッター情報</param>
        public override void WriteFooter<T>(T footer)
        {
        }

        /// <summary>
        /// 書込み後処理
        /// </summary>
        public override void PostWrite()
        {
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public override void Terminate()
        {
        }

        /// <summary>
        /// 設定情報のロード
        /// </summary>
        /// <param name="filePath">設定情報ファイルのパス</param>
        public override void LoadSettings(string filePath)
        {
        }

        /// <summary>
        /// COMオブジェクトの解放
        /// </summary>
        protected abstract void ReleaseComObject();

        /// <summary>
        /// 年月日の書込み
        /// </summary>
        /// <param name="range">書込み範囲</param>
        /// <param name="datetime">現在時刻</param>
        protected virtual void WriteYearMonth(Range range, DateTime datetime)
        {
            range.Value2 = datetime.ToString("G");
        }

        /// <summary>
        /// タイトルの書込み
        /// </summary>
        /// <param name="range">書込み範囲</param>
        /// <param name="title">タイトル</param>
        protected virtual void WriteTitle(Range range, string title)
        {
            range.Value2 = title;
        }

        #region Dispaoseパターン

        /// <summary>
        /// 重複する呼び出しを検出するには
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// 破棄
        /// </summary>
        /// <param name="disposing">破棄する/しない</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    GC.SuppressFinalize(this);
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~AbstractExcelWriter()
        {
            Dispose(false);
        }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        #endregion

    }
}
