//*************************************************************************************
//
//   ファイルのリーダ用のインターフェース
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System.Collections.Generic;

namespace FileManager.Reader.Interface
{
    /// <summary>
    /// ファイルのリーダ用のインターフェース
    /// </summary>
    public interface IFileReader
    {

        #region プロパティ

        /// <summary>
        /// 読込み対象のファイルパス
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        /// 読込み設定ファイルのパス
        /// </summary>
        string SettingFilePath { get; set; }

        #endregion

        #region メソッド

        /// <summary>
        /// ファイル読込
        /// </summary>
        /// <param name="filePath">読込み対象のファイルパス</param>
        /// <param name="settingFilePath">読込み設定ファイルのパス</param>
        void Read(string filePath, string settingFilePath);

        /// <summary>
        /// 読込んだファイルの妥当性の検証
        /// </summary>
        void Validate();

        /// <summary>
        /// レコードの取得
        /// </summary>
        /// <returns>レコード</returns>
        IList<string> GetRecords();

        /// <summary>
        /// レコード件数の取得
        /// </summary>
        /// <returns>レコード件数</returns>
        int GetRecordCount();

        #endregion
    }
}
