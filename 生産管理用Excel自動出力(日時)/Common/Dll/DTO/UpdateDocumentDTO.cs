using DTO.Base;
//*************************************************************************************
//
//   更新ドキュメントDTO
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
namespace DTO
{
    /// <summary>
    /// 更新ドキュメントDTO
    /// </summary>
    public class UpdateDocumentDTO : DocumentDTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="updateFileName">更新ファイル名</param>
        public UpdateDocumentDTO(string updateFileName)
        {
            UpdateFileName = updateFileName;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 更新ファイル名
        /// </summary>
        public string UpdateFileName { get; set; }

        #endregion

    }
}
