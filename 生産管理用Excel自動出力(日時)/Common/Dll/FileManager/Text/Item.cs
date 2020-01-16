//*************************************************************************************
//
//   項目クラス
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
    /// 項目クラス
    /// </summary>
    public class Item
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="position">桁位置</param>
        /// <param name="length">桁数</param>
        public Item(string name, int position, int length)
        {
            Name = name;
            Position = position;
            Length = length;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 桁位置
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// 桁数
        /// </summary>
        public int Length { get; set; }

        #endregion
    }
}
