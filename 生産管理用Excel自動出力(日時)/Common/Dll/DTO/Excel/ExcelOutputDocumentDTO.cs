//*************************************************************************************
//
//   Excel出力ドキュメントDTO
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System.Collections.Generic;
using DTO.Base;

namespace DTO.Excel
{
    /// <summary>
    /// Excel出力ドキュメントDTO
    /// </summary>
    /// <typeparam name="T">DTOの基底を継承した任意のオブジェクトの型</typeparam>
    public class ExcelOutputDocumentDTO<T> : OutputDocumentDTO<T> where T : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputData">出力データ</param>
        public ExcelOutputDocumentDTO(T outputData) : base(outputData)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputList">出力データ</param>
        public ExcelOutputDocumentDTO(IList<T> outputList) : base(outputList)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputFileName">出力ファイル名</param>
        /// <param name="outputData">出力データ</param>
        public ExcelOutputDocumentDTO(string outputFileName, T outputData)
            : base(outputFileName, outputData)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputFileName">出力ファイル名</param>
        /// <param name="outputList">出力データ</param>
        public ExcelOutputDocumentDTO(string outputFileName, IList<T> outputList)
            : base(outputFileName, outputList)
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// テンプレートブックのファイルパス
        /// </summary>
        public string TemplateBookPath { get; set; }

        /// <summary>
        /// 出力用ファイルのパス
        /// </summary>
        public string OutputBookPath { get; set; }

        /// <summary>
        /// 出力用ファイルのフォルダ
        /// </summary>
        public string OutputBookFolder { get; set; }

        /// <summary>
        /// プレビューを表示するかどうか
        /// </summary>
        public bool IsShowPreview { get; set; }

        #endregion
    }
}
