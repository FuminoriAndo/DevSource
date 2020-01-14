using System;
using System.Runtime.Serialization;
//*************************************************************************************
//
//   改ページの制限値超過用のException
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090.Common.Excel
{
    /// <summary>
    /// 改ページの制限値超過用のException
    /// </summary>
    [Serializable]
    public class ExcelInsertPageLimitOverException : Exception
    {

        #region コンストラクタ

        public ExcelInsertPageLimitOverException()
        {
        }

        public ExcelInsertPageLimitOverException(string message) : base(message)
        {
        }

        protected ExcelInsertPageLimitOverException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ExcelInsertPageLimitOverException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion
    }
}