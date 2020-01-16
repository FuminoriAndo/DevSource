//*************************************************************************************
//
//   ドキュメント用定義
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
namespace Document.Types
{
    #region 列挙体

    /// <summary>
    /// ドキュメントの種類
    /// </summary>
    public enum DocumentType
    {
        /// <summary>
        /// Word
        /// </summary>
        Word,

        /// <summary>
        /// Excel
        /// </summary>
        Excel,

        /// <summary>
        /// Pdf
        /// </summary>
        Pdf,

        /// <summary>
        /// ラベルプリント
        /// </summary>
        LabelPrint
    }

    #endregion
}
