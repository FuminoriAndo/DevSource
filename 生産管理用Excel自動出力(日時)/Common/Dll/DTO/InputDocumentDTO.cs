//*************************************************************************************
//
//   読込みドキュメントDTO
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using DTO.Base;

namespace DTO
{
    /// <summary>
    /// 読込みドキュメントDTO
    /// </summary>
    /// <typeparam name="T">DTOの基底を継承した任意のオブジェクトの型</typeparam>
    public class InputDocumentDTO<T> : DocumentDTOBase where T : ConditionDTOBase
    {
        #region プロパティ

        /// <summary>
        /// 読込むファイル名
        /// </summary>
        public string InputFileName { get; set; }

        /// <summary>
        /// 読込む条件
        /// </summary>
        public T ReadCondition { get; set; }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputDocumentDTO()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="codition">読込む条件</param>
        public InputDocumentDTO(T codition)
        {
            ReadCondition = codition;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="inputFileName">入力ファイル名</param>
        /// <param name="codition">読込む条件</param>
        public InputDocumentDTO(string inputFileName, T codition)
        {
            InputFileName = inputFileName;
            ReadCondition = codition;
        }

        #endregion

    }
}
