//*************************************************************************************
//
//   ファイルDTOの基底
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System.Collections.Generic;

namespace DTO.Base
{
    /// <summary>
    /// ファイルDTOの基底
    /// </summary>
    public class FileBaseDTO<T> : DTOBase where T : FileDetailDTO
    {
        #region プロパティ

        /// <summary>
        /// ファイルヘッダー情報
        /// </summary>
        public FileHeaderDTO FileHeaderInfo { get; set; }

        /// <summary>
        /// ファイル詳細情報
        /// </summary>
        public IList<T> FileDetailInfo { get; set; }

        /// <summary>
        /// ファイルフッター情報
        /// </summary>
        public FileFooterDTO FileFooterInfo { get; set; }

        #endregion
    }

    /// <summary>
    /// ファイルヘッダー
    /// </summary>
    public class FileHeaderDTO
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileHeaderDTO()
        {
        }

        #endregion
    }

    /// <summary>
    /// ファイル詳細
    /// </summary>
    public class FileDetailDTO
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileDetailDTO()
        {
        }

        #endregion
    }

    /// <summary>
    /// ファイルフッター
    /// </summary>
    public class FileFooterDTO
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileFooterDTO()
        {
        }

        #endregion
    }
}
