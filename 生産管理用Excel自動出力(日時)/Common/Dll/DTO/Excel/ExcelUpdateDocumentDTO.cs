//*************************************************************************************
//
//  Excel更新ドキュメントDTO
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************

namespace DTO.Excel
{
    /// <summary>
    /// Excel更新ドキュメントDTO
    /// </summary>
    public class ExcelUpdateDocumentDTO : UpdateDocumentDTO
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="updateFileName">更新ファイル名</param>
        public ExcelUpdateDocumentDTO(string updateFileName)
            : base(updateFileName)
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 出力用ファイルのパス
        /// </summary>
        public string UpdateBookPath { get; set; }

        /// <summary>
        /// 出力用ファイルのフォルダ
        /// </summary>
        public string UpdateBookFolder { get; set; }

        #endregion
    }
}
