//*************************************************************************************
//
//   固定長ファイル例外クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using FileManager.Text.Types;
using System;

namespace FileManager.Text.CustomException
{
    /// <summary>
    /// 固定長ファイル例外クラス
    /// </summary>
    public class FixedLengthDocumentException : Exception
    {
        #region フィールド

        /// <summary>
        /// 固定長ファイル例外種別
        /// </summary>
        public FixedLengthDocumentExceptionType Type { get; private set; }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">固定長ファイル例外種別</param>
        public FixedLengthDocumentException(FixedLengthDocumentExceptionType type)
        {
            Type = type;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">固定長ファイル例外種別</param>
        /// <param name="innerException">内部例外</param>
        public FixedLengthDocumentException(FixedLengthDocumentExceptionType type, Exception innerException)
            : base(string.Empty, innerException)
        {
            Type = type;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">固定長ファイル例外種別</param>
        /// <param name="message">メッセージ</param>
        /// <param name="innerException">内部例外</param>
        public FixedLengthDocumentException(FixedLengthDocumentExceptionType type, string message, Exception innerException)
            : base(message, innerException)
        {
            Type = type;
        }

        #endregion
    }
}
