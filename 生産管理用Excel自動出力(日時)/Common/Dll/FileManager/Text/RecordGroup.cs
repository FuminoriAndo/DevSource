//*************************************************************************************
//
//   レコードグループクラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using Utility.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileManager.Text
{
    /// <summary>
    /// レコードグループクラス
    /// </summary>
    public class RecordGroup
    {
        #region フィールド

        /// <summary>
        /// レコード定義
        /// </summary>
        private RecordFormat recordFormat = null;

        /// <summary>
        /// レコード一覧
        /// </summary>
        List<string> records = new List<string>();

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="recordFormat">レコードフォーマット</param>
        public RecordGroup(RecordFormat recordFormat)
        {
            this.recordFormat = recordFormat;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// レコード
        /// </summary>
        public List<string> Records
        {
            get { return records; }
        }

        /// <summary>
        /// レコード数
        /// </summary>
        public int RecordCount
        {
            get { return records.Count; }
        }

        #endregion

        #region メソッド

        /// <summary>
        /// フォーマット名称取得
        /// </summary>
        /// <returns>フォーマット名称</returns>
        public string GetFormatName()
        {
            return recordFormat.Name;
        }

        /// <summary>
        /// レコード追加
        /// </summary>
        /// <param name="record">レコード</param>
        public void AddRecord(string record)
        {
            records.Add(record);
        }

        /// <summary>
        /// 指定された行と名称に該当する値を取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <param name="itemName"></param>
        /// <returns>行と名称に該当する値</returns>
        public object GetValue(int recordIndex, string itemName)
        {
            try
            {
                // 対象の値を取得する。
                var item = recordFormat.Items.FirstOrDefault(
                    recordFormatItem => recordFormatItem.Name == itemName);
                if (item == null)
                {
                    return null;
                }

                if (records.Count <= recordIndex)
                {
                    return null;
                }

                return CommonUtil.GetString(records[recordIndex], item.Position - 1, item.Length, recordFormat.Encode);
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
