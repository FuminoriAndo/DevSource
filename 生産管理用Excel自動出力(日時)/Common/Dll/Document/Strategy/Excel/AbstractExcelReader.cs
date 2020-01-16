//*************************************************************************************
//
//   Excel出力用データ読込みのストラテジ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;

namespace Document.Strategy.Excel
{
    /// <summary>
    /// Excelデータの読込みストラテジ
    /// </summary>
    public abstract class AbstractExcelReader : AbstractDocumentReaderBase, IDisposable
    {
        #region メソッド

        /// <summary>
        /// COMオブジェクトの解放
        /// </summary>
        protected abstract void ReleaseComObject();

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
        ~AbstractExcelReader()
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
