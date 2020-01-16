//*************************************************************************************
//
//   キー項目クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
namespace FileManager.Text
{
    /// <summary>
    /// キー項目クラス
    /// </summary>
    public class KeyItem
    {
        /// <summary>
        /// キー項目の名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// キー項目の値
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KeyItem(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
