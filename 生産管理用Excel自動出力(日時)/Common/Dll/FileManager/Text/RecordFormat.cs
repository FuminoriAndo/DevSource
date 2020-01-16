//*************************************************************************************
//
//   レコードフォーマットクラス
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
    /// レコードフォーマットクラス
    /// </summary>
    public class RecordFormat
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RecordFormat(string encode, string name, int length, int checkPriority, List<KeyItem> keyItems, List<Item> items)
        {
            Encode = encode;
            Name = name;
            Length = length;
            CheckPriority = checkPriority;
            Items = items;
            KeyItemMap = new Dictionary<KeyItem, Item>();
            keyItems.ForEach(keyItem => 
            {
                KeyItemMap.Add(keyItem, items.FirstOrDefault(item => item.Name == keyItem.Name));
            });
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// エンコーディング
        /// </summary>
        public string Encode { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// レコード全体の桁数
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// チェック優先順位
        /// </summary>
        public int CheckPriority { get; private set; }

        /// <summary>
        /// キー項目一覧
        /// </summary>
        public IDictionary<KeyItem, Item> KeyItemMap { get; private set; }

        /// <summary>
        /// 項目一覧
        /// </summary>
        public List<Item> Items { get; private set; }

        #endregion

        #region メソッド

        /// <summary>
        /// フォーマットに一致するレコードか
        /// </summary>
        /// <param name="record">レコード</param>
        /// <returns>一致する場合、trueを返す</returns>
        public bool IsMatch(string record)
        {
            try
            {
                // エンコーディングされたバイト数の取得
                int byteCount = CommonUtil.GetByteCount(record, Encode);
                
                // 全体の桁数チェック
                if (byteCount != Length)
                {
                    return false;
                }

                // キー値チェック
                if (KeyItemMap.Any
                        (keyItem => record.Substring(keyItem.Value.Position - 1, keyItem.Value.Length) != keyItem.Key.Value))
                {
                    return false;
                };

                return true;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
