using System;
using System.Runtime.Serialization;
//*************************************************************************************
//
//   棚卸ログ用Exception
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Common
{
    /// <summary>
    /// 棚卸ログ用Exception
    /// </summary>
    [Serializable]
    public class TanaorosiLogException : Exception
    {
        #region プロパティ

        /// <summary>
        /// エラーコード
        /// </summary>
        public string ErrorCode { get; set; } = string.Empty;

        /// <summary>
        /// SQL
        /// </summary>
        public string SQL { get; set; } = string.Empty;

        /// <summary>
        /// SQLパラメータ
        /// </summary>
        public string SqlParam { get; set; } = string.Empty;

        /// <summary>
        /// タイムスタンプ
        /// </summary>
        public string TimeStamp { get; set; } = string.Empty;

        #endregion

        #region コンストラクタ

        public TanaorosiLogException()
        {
        }

        public TanaorosiLogException(string message) : base(message)
        {
        }

        protected TanaorosiLogException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public TanaorosiLogException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion
    }
}