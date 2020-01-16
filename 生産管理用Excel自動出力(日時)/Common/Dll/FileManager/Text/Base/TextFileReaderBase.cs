//*************************************************************************************
//
//   テキストファイルリーダの基底
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using FileManager.Reader.Interface;

namespace FileManager.Text.Base
{
    /// <summary>
    /// テキストファイルリーダの基底
    /// </summary>
    public class TextFileReaderBase : IFileReader
    {
        #region フィールド

        /// <summary>
        /// 固定長ファイルオブジェクト
        /// </summary>
        protected FixedLengthDocument document = null;

        /// <summary>
        /// 読込み対象のファイルパス
        /// </summary>
        public string FilePath { get; set; } 

        /// <summary>
        /// 読込み設定ファイルのパス
        /// </summary>
        public string SettingFilePath { get; set; }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TextFileReaderBase()
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// ファイル読込
        /// </summary>
        /// <param name="filePath">読込み対象のファイルパス</param>
        /// <param name="settingFilePath">読込み設定ファイルのパス</param>
        public virtual void Read(string filePath, string settingFilePath)
        {
            try
            {
                FilePath = filePath;
                SettingFilePath = settingFilePath;
                document = FixedLengthDocument.Load(filePath, settingFilePath);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 読込んだファイルの妥当性の検証
        /// </summary>
        public virtual void Validate()
        {
        }

        /// <summary>
        /// レコードの取得
        /// </summary>
        /// <returns>レコード</returns>
        public virtual IList<string> GetRecords()
        {
            var recordGroup = getRecordGroup("Detail");
            return recordGroup.Records;
        }

        /// <summary>
        /// レコード件数の取得
        /// </summary>
        /// <returns>レコード件数</returns>
        public virtual int GetRecordCount()
        {
            return document.GetRecordCount("Detail");
        }

        /// <summary>
        /// 指定したレコードグループの取得
        /// </summary>
        /// <param name="keyname">指定するキー</param>
        /// <returns>指定したレコードグループ</returns>
        private RecordGroup getRecordGroup(string keyname)
        {
            try
            {
                var groups = document.RecordGroups;
                var recordGroup = groups.FirstOrDefault(x => x.GetFormatName() == keyname);
                return recordGroup;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion

    }
}