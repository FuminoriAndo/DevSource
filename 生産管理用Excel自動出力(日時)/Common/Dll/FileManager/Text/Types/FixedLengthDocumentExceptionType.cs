//*************************************************************************************
//
//   固定長ファイル例外種別
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
namespace FileManager.Text.Types
{
    /// <summary>
    /// 固定長ファイル例外種別
    /// </summary>
    public enum FixedLengthDocumentExceptionType
    {
        #region 列挙体

        /// <summary>
        /// 定義ファイルバージョンエラー
        /// </summary>
        DefinitionFileVersionError,

        /// <summary>
        /// 定義ファイルフォーマットエラー
        /// </summary>
        DefinitionFileFormatError,

        /// <summary>
        /// 固定長ファイルフォーマットエラー
        /// </summary>
        FixedLengthDocumentRecordFormatError,

        /// <summary>
        /// 値取得引数エラー
        /// </summary>
        GetValueArgumentsError,

        /// <summary>
        /// 不明
        /// </summary>
        Unknown

        #endregion
    }

    #region ヘルパー

    /// <summary>
    /// 固定長ファイル例外種別のヘルパー
    /// </summary>
    internal static class FixedLengthDocumentExceptionTypeHelper
    {
        #region メソッド

        /// <summary>
        /// 固定長ファイル例外種別のValueを取得する
        /// </summary>
        internal static string GetMessageValue(this FixedLengthDocumentExceptionType type)
        {
            string[] types = { "定義ファイルのバージョンエラーです", 
                               "定義ファイルのフォーマットエラーです", 
                               "固定長ファイルのフォーマットエラーです", 
                               "値取得引数のエラーです", "不明なエラーです"};
            return types[(int)type];
        }

        #endregion
    }

    #endregion
}
