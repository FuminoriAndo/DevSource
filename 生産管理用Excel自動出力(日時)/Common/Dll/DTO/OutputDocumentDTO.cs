//*************************************************************************************
//
//   出力ドキュメントDTO
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using DTO.Base;
using System.Collections.Generic;

namespace DTO
{
    /// <summary>
    /// 出力ドキュメントDTO
    /// </summary>
    /// <typeparam name="T">DTOの基底を継承した任意のオブジェクトの型</typeparam>
    public class OutputDocumentDTO<T> : DocumentDTOBase where T : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputData">出力データ</param>
        public OutputDocumentDTO(T outputData)
        {
            OutputData = outputData;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputList">出力データ</param>
        public OutputDocumentDTO(IList<T> outputList)
        {
            OutputList = outputList;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputFileName">出力ファイル名</param>
        /// <param name="outputData">出力データ</param>
        public OutputDocumentDTO(string outputFileName, T outputData)
        {
            OutputFileName = outputFileName;
            OutputData = outputData;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputFileName">出力ファイル名</param>
        /// <param name="outputList">出力データ</param>
        public OutputDocumentDTO(string outputFileName, IList<T> outputList)
        {
            OutputFileName = outputFileName;
            OutputList = outputList;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 出力ファイ名
        /// </summary>
        public string OutputFileName { get; set; }

        /// <summary>
        /// 出力データ
        /// </summary>
        public T OutputData { get; set; }

        /// <summary>
        /// 出力データ
        /// </summary>
        public IList<T> OutputList { get; set; }

        #endregion

    }
}
