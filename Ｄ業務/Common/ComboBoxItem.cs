//*************************************************************************************
//
//   ComboBox用アイテム
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   19.04.08              DSK吉武      新規作成
//
//*************************************************************************************
namespace Common
{
    /// <summary>
    /// ComboBox用のItemクラス
    /// </summary>
    public class ComboBoxItem
    {
        /// <summary>
        /// キー
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 値
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cd">キー</param>
        /// <param name="name">値</param>
        public ComboBoxItem(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
