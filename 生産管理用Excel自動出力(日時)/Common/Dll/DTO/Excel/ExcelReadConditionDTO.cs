using DTO.Base;
//*************************************************************************************
//
//   Excelの読込み条件のDTO
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
    /// Excelの読込み条件のDTO
    /// </summary>
    public class ExcelReadConditionDTO : ConditionDTOBase
    {
        #region プロパティ

        /// <summary>
        /// シート名
        /// </summary>
        public string ReadSheetName { get; set; }

        /// <summary>
        /// 読込む行の開始位置
        /// </summary>
        public int ReadStartRowPos { get; set; }

        /// <summary>
        /// 読込む列の開始位置
        /// </summary>
        public int ReadStartColPos { get; set; }

        /// <summary>
        /// 読込む行の終了位置
        /// </summary>
        public int ReadEndRowPos { get; set; }

        /// <summary>
        /// 読込む列の終了位置
        /// </summary>
        public int ReadEndColPos { get; set; }

        #endregion
    }
}
