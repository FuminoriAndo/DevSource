//*************************************************************************************
//
//   固定長ファイルクラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using FileManager.Text.CustomException;
using FileManager.Text.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace FileManager.Text
{
    /// <summary>
    /// 固定長ファイルクラス
    /// </summary>
    public class FixedLengthDocument
    {
        #region フィールド

        /// <summary>
        /// レコードグループ一覧
        /// </summary>
        private IList<RecordGroup> recordGroups = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private FixedLengthDocument(List<RecordGroup> recordGroups)
        {
            this.recordGroups = recordGroups;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// レコードグループ一覧
        /// </summary>
        public IList<RecordGroup> RecordGroups
        {
            get
            {
                return recordGroups;
            }
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 固定長ファイル読込
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="definitionFilePath">定義ファイルパス</param>
        /// <returns>FixedLengthDocumentオブジェクト</returns>
        public static FixedLengthDocument Load(string filePath, string definitionFilePath)
        {
            // エンコーディング
            string encoding = null;
            
            // 定義ファイルを読み込む。
            List<RecordFormat> recordFormats = null;
            try
            {
                recordFormats = loadRecordFormat(definitionFilePath, out encoding);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message, ex.StackTrace);
                throw ex;
            }

            if (recordFormats == null || recordFormats.Count <= 0)
            {
                return null;
            }

            // 固定長ファイルを読み込む。
            var recordGroups = new List<RecordGroup>();
            using (var sr = new StreamReader(filePath, Encoding.GetEncoding(encoding)))
            {
                var line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    // フォーマットチェック
                    var recordFormat = recordFormats.FirstOrDefault(format => 
                    {
                        return format.IsMatch(line);
                    });

                    if(recordFormat == null)
                    {
                        // 一致するフォーマット無し
                        throw new FixedLengthDocumentException(
                            FixedLengthDocumentExceptionType.FixedLengthDocumentRecordFormatError,
                            FixedLengthDocumentExceptionType.FixedLengthDocumentRecordFormatError.GetMessageValue(), null);
                    }

                    // レコード格納
                    var recordGroup = recordGroups
                        .FirstOrDefault(group => group.GetFormatName() == recordFormat.Name);
                    if (recordGroup == null)
                    {
                        // グループが無い場合、グループ生成してレコード追加
                        recordGroup = new RecordGroup(recordFormat);
                        recordGroup.AddRecord(line);
                        recordGroups.Add(recordGroup);
                    }
                    else
                    {
                        // グループがある場合、レコード追加
                        recordGroup.AddRecord(line);
                    }
                }
            }

            return new FixedLengthDocument(recordGroups);
        }

        /// <summary>
        /// レコードフォーマット読込
        /// </summary>
        /// <param name="definitionFilePath">定義ファイルパス</param>
        /// <param name="encoding">エンコーディング</param>
        /// <returns>レコードフォーマット</returns>
        private static List<RecordFormat> loadRecordFormat(string definitionFilePath, out string encoding)
        {
            var recordFormats = new List<RecordFormat>();
            try
            {
                XDocument doc = XDocument.Load(definitionFilePath);

                // バージョン
                var rootElement = doc.XPathSelectElement("Root");
                var majorVersion = int.Parse(rootElement.Attribute("Version").Value.Substring(0, 1));
                if (majorVersion > 1)
                {
                    // 新しいバージョンのファイルのため読込不可
                    throw new FixedLengthDocumentException(FixedLengthDocumentExceptionType.DefinitionFileVersionError);
                }

                // エンコード
                var recordFormatsElement = rootElement.XPathSelectElement("RecordFormats");
                var encode = recordFormatsElement.Attribute("Encoding").Value;
                encoding = encode;

                // レコード定義一覧
                var recordFormatElements = rootElement.XPathSelectElements("RecordFormats/RecordFormat");
                recordFormatElements.ToList().ForEach(recordFormatElement => 
                {
                    // キー項目一覧
                    List<KeyItem> keyItems = new List<KeyItem>();
                    var keyItemElements = recordFormatElement.XPathSelectElements("KeyItems/KeyItem");
                    keyItemElements.ToList().ForEach(keyItemElement =>
                    {
                        keyItems.Add(new KeyItem(
                            keyItemElement.Attribute("Name").Value,
                            keyItemElement.Attribute("Value").Value));
                    });

                    // 項目
                    List<Item> items = new List<Item>();
                    var itemElements = recordFormatElement.XPathSelectElements("Items/Item");
                    itemElements.ToList().ForEach(itemElement => 
                    {
                        items.Add(new Item(
                            itemElement.Attribute("Name").Value,
                            int.Parse(itemElement.Attribute("Position").Value),
                            int.Parse(itemElement.Attribute("Length").Value)));
                    });

                    // チェック優先順位
                    int checkPriority = int.MaxValue;
                    if(recordFormatElement.Attribute("CheckPriority") != null)
                    {
                        checkPriority = int.Parse(recordFormatElement.Attribute("CheckPriority").Value);
                    }
                    
                    var recordFormat = new RecordFormat(
                        encode,
                        recordFormatElement.Attribute("Name").Value,
                        int.Parse(recordFormatElement.Attribute("Length").Value),
                        checkPriority,
                        keyItems, items);
                    recordFormats.Add(recordFormat);
                });

                // チェック順に並べ替え
                recordFormats.Sort((recordFormat1, recordFormat2) 
                    => recordFormat1.CheckPriority - recordFormat2.CheckPriority);
            }
            catch (Exception ex)
            {
                // 定義ファイルフォーマットエラー
                throw new FixedLengthDocumentException(FixedLengthDocumentExceptionType.DefinitionFileFormatError, ex);
            }

            return recordFormats;
        }

        /// <summary>
        /// 値取得(文字列)
        /// </summary>
        /// <param name="formatName">フォーマット名</param>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <param name="itemName">項目名</param>
        /// <returns>値(文字列)</returns>
        public string GetStringValue(string formatName, int recordIndex, string itemName)
        {
            try
            {
                var recordGroup = recordGroups.FirstOrDefault(group => group.GetFormatName() == formatName);
                return recordGroup.GetValue(recordIndex, itemName).ToString();
            }
            catch (Exception ex)
            {
                throw new FixedLengthDocumentException(FixedLengthDocumentExceptionType.GetValueArgumentsError, ex);
            }
        }

        /// <summary>
        /// 値取得(数値Int型)
        /// </summary>
        /// <param name="formatName">フォーマット名</param>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <param name="itemName">項目名</param>
        /// <returns>値(数値Int型)</returns>
        public int GetIntegerValue(string formatName, int recordIndex, string itemName)
        {
            try
            {
                var recordGroup = recordGroups.FirstOrDefault(group => group.GetFormatName() == formatName);
                return int.Parse(recordGroup.GetValue(recordIndex, itemName).ToString());
            }
            catch (Exception ex)
            {
                throw new FixedLengthDocumentException(FixedLengthDocumentExceptionType.GetValueArgumentsError, ex);
            }
        }

        /// <summary>
        /// 値取得(数値Int型)
        /// </summary>
        /// <param name="formatName">フォーマット名</param>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <param name="itemName">項目名</param>
        /// <returns>値(数値Int型)、数値変換できない場合nullを返す。</returns>
        public int? GetIntegerTryParseValue(string formatName, int recordIndex, string itemName)
        {
            try
            {
                var recordGroup = recordGroups.FirstOrDefault(group => group.GetFormatName() == formatName);
                int integerValue = 0;
                if (int.TryParse(recordGroup.GetValue(recordIndex, itemName).ToString(), out integerValue))
                {
                    return integerValue;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new FixedLengthDocumentException(FixedLengthDocumentExceptionType.GetValueArgumentsError, ex);
            }
        }

        /// <summary>
        /// 値取得(数値Float型)
        /// </summary>
        /// <param name="formatName">フォーマット名</param>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <param name="itemName">項目名</param>
        /// <returns>値(数値Float型)</returns>
        public float GetFloatValue(string formatName, int recordIndex, string itemName)
        {
            try
            {
                var integerValue = GetIntegerTryParseValue(formatName, recordIndex, itemName);
                if (integerValue != null)
                    return ((float)integerValue / 100);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw new FixedLengthDocumentException(FixedLengthDocumentExceptionType.GetValueArgumentsError, ex);
            }
        }

        /// <summary>
        /// 値取得(数値Long型)
        /// </summary>
        /// <param name="formatName">フォーマット名</param>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <param name="itemName">項目名</param>
        /// <returns>値(数値Long型)</returns>
        public long GetLongValue(string formatName, int recordIndex, string itemName)
        {
            try
            {
                var recordGroup = recordGroups.FirstOrDefault(group => group.GetFormatName() == formatName);
                return long.Parse(recordGroup.GetValue(recordIndex, itemName).ToString());
            }
            catch (Exception ex)
            {
                throw new FixedLengthDocumentException(FixedLengthDocumentExceptionType.GetValueArgumentsError, ex);
            }
        }

        /// <summary>
        /// 値取得(数値Long型)
        /// </summary>
        /// <param name="formatName">フォーマット名</param>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <param name="itemName">項目名</param>
        /// <returns>値(数値Long型)、数値変換できない場合nullを返す。</returns>
        public long? GetLongTryParseValue(string formatName, int recordIndex, string itemName)
        {
            try
            {
                var recordGroup = recordGroups.FirstOrDefault(group => group.GetFormatName() == formatName);
                long longValue = 0;
                if (long.TryParse(recordGroup.GetValue(recordIndex, itemName).ToString(), out longValue))
                {
                    return longValue;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new FixedLengthDocumentException(FixedLengthDocumentExceptionType.GetValueArgumentsError, ex);
            }
        }

        /// <summary>
        /// 値取得(日付)
        /// </summary>
        /// <param name="formatName">フォーマット名</param>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <param name="itemName">項目名</param>
        /// <returns>値(日付)</returns>
        public DateTime GetDateTimeValue(string formatName, int recordIndex, string itemName)
        {
            try
            {
                var recordGroup = recordGroups.FirstOrDefault(group => group.GetFormatName() == formatName);
                return DateTime.ParseExact(
                    recordGroup.GetValue(recordIndex, itemName).ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                throw new FixedLengthDocumentException(FixedLengthDocumentExceptionType.GetValueArgumentsError, ex);
            }
        }

        /// <summary>
        /// 値取得(数値DateTime型)
        /// </summary>
        /// <param name="formatName">フォーマット名</param>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <param name="itemName">項目名</param>
        /// <returns>値(数値DateTime型)、数値変換できない場合nullを返す。</returns>
        public DateTime? GetDateTimeTryParseValue(string formatName, int recordIndex, string itemName)
        {
            try
            {
                var recordGroup = recordGroups.FirstOrDefault(group => group.GetFormatName() == formatName);
                DateTime dateTimeValue;
                if (DateTime.TryParse(recordGroup.GetValue(recordIndex, itemName).ToString(), out dateTimeValue))
                {
                    return dateTimeValue;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new FixedLengthDocumentException(FixedLengthDocumentExceptionType.GetValueArgumentsError, ex);
            }
        }

        /// <summary>
        /// フォーマット名に該当するレコード数を取得
        /// </summary>
        /// <param name="formatName">フォーマット名</param>
        /// <returns>レコード数</returns>
        public int GetRecordCount(string formatName)
        {
            try
            {
                var recordGroup = recordGroups.FirstOrDefault(group => group.GetFormatName() == formatName);
                return recordGroup.RecordCount;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
