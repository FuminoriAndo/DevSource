//*************************************************************************************
//
//   カスタムException
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using System.Runtime.Serialization;

namespace ProductionControlDailyExcelCreator.Exceptions
{
    #region ACOS転送ファイルが存在しない場合に発生させる例外

    /// <summary>
    /// ACOS転送ファイルが存在しない場合に発生させる例外
    /// </summary>
    [Serializable]
    public class AcosTransferFileNotFoundException : Exception
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AcosTransferFileNotFoundException()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public AcosTransferFileNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">シリアライズ情報</param>
        /// <param name="context">シリアル化ストリームのコンテキスト</param>
        public AcosTransferFileNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="innerException">インナーException</param>
        public AcosTransferFileNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get; set; }

        #endregion
    }

    /// <summary>
    /// ファイルにデータがない場合に発生させる例外
    /// </summary>
    [Serializable]
    public class FileNoDataException : Exception
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileNoDataException()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public FileNoDataException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">シリアライズ情報</param>
        /// <param name="context">シリアル化ストリームのコンテキスト</param>
        public FileNoDataException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="innerException">インナーException</param>
        public FileNoDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }

    #endregion
}
