//*************************************************************************************
//
//   PMGQ010Bのモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using Model.Base;

namespace Model
{
    /// <summary>
    /// PMGQ010Bのモデル
    /// </summary>
    public class PMGQ010BModel : ModelBase
    {
        #region プロパティ

        /// <summary>
        /// 受注№
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 需要家名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 規格名
        /// </summary>
        public string StandardName { get; set; }

        /// <summary>
        /// 製品サイズ
        /// </summary>
        public string ProductSize { get; set; }

        /// <summary>
        /// 製品(厚)
        /// </summary>
        public string ProductAtu { get; set; }

        /// <summary>
        /// 製品(巾)
        /// </summary>
        public string ProductHaba { get; set; }

        /// <summary>
        /// 製品(長)
        /// </summary>
        public string ProductNaga { get; set; }

        /// <summary>
        /// 定耳
        /// </summary>
        public string TM { get; set; }

        /// <summary>
        /// 製品コード
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// ロールサイズ
        /// </summary>
        public string RollSize { get; set; }

        /// <summary>
        /// ロール巾
        /// </summary>
        public string RollHaba { get; set; }

        /// <summary>
        /// ロール長
        /// </summary>
        public string RollNaga { get; set; }

        /// <summary>
        /// TP区分
        /// </summary>
        public string TPClassfication { get; set; }

        /// <summary>
        /// ロット
        /// </summary>
        public string Lot { get; set; }

        /// <summary>
        /// ロット1
        /// </summary>
        public string Lot1 { get; set; }

        /// <summary>
        /// ロット2
        /// </summary>
        public string Lot2 { get; set; }

        /// <summary>
        /// 圧延予定
        /// </summary>
        public string AtuenYMD { get; set; }

        /// <summary>
        /// チャージ№
        /// </summary>
        public string CHNo { get; set; }

        /// <summary>
        /// スラブ№
        /// </summary>
        public string SlabNo { get; set; }

        /// <summary>
        /// ロール№
        /// </summary>
        public string RollNo { get; set; }

        /// <summary>
        /// 抽出日
        /// </summary>
        public string ExtractionYMD { get; set; }

        #endregion
    }
}
