//*************************************************************************************
//
//   PMPA260Bのモデル
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
    /// PMPA260Bのモデル
    /// </summary>
    public class PMPA260BModel : ModelBase
    {
        #region プロパティ

        /// <summary>
        /// RCNO
        /// </summary>
        public string RCNO { get; set; }

        /// <summary>
        /// 勤務
        /// </summary>
        public string Work { get; set; }

        /// <summary>
        /// 圧延順
        /// </summary>
        public string AtuenOrder { get; set; }

        /// <summary>
        /// ロット№
        /// </summary>
        public string LotNo { get; set; }

        /// <summary>
        /// ロット1
        /// </summary>
        public string Lot1 { get; set; }

        /// <summary>
        /// ロット2
        /// </summary>
        public string Lot2 { get; set; }

        /// <summary>
        /// チャージ№
        /// </summary>
        public string CHNo { get; set; }

        /// <summary>
        /// チャージ("-")
        /// </summary>
        public string CHDash { get; set; }

        /// <summary>
        /// 1次鋳片№
        /// </summary>
        public string Slab1No { get; set; }

        /// <summary>
        /// 2次鋳片№
        /// </summary>
        public string Slab2No { get; set; }

        /// <summary>
        /// 材料識別
        /// </summary>
        public string MaterialsIdentification { get; set; }

        /// <summary>
        /// 材料識別1
        /// </summary>
        public string MaterialsIdentification1 { get; set; }

        /// <summary>
        /// 材料識別2
        /// </summary>
        public string MaterialsIdentification2 { get; set; }

        /// <summary>
        /// ＴＰ管理
        /// </summary>
        public string TPManagement { get; set; }

        /// <summary>
        /// 厚テーバー
        /// </summary>
        public string AtuTapera { get; set; }

        /// <summary>
        /// 巾テーパー
        /// </summary>
        public string HabaTapera { get; set; }

        /// <summary>
        /// ＨＣ区分
        /// </summary>
        public string HCDivision { get; set; }

        /// <summary>
        /// 抽出時分
        /// </summary>
        public string ExtractionHHMM { get; set; }

        /// <summary>
        /// 抽出時
        /// </summary>
        public string ExtractionHH { get; set; }

        /// <summary>
        /// 抽出分
        /// </summary>
        public string ExtractionMM { get; set; }

        /// <summary>
        /// 規格
        /// </summary>
        public string Standard { get; set; }

        /// <summary>
        /// ロールサイズ
        /// </summary>
        public string RollSize { get; set; }

        /// <summary>
        /// ロール厚
        /// </summary>
        public string RollAtu { get; set; }

        /// <summary>
        /// ロール巾
        /// </summary>
        public string RollHaba { get; set; }

        /// <summary>
        /// ロール長
        /// </summary>
        public string RollNaga { get; set; }

        /// <summary>
        /// スラブサイズ
        /// </summary>
        public string SlabSize { get; set; }

        /// <summary>
        /// スラブ厚
        /// </summary>
        public string SlabAtu { get; set; }

        /// <summary>
        /// スラブ巾
        /// </summary>
        public string SlabHaba { get; set; }

        /// <summary>
        /// スラブ長
        /// </summary>
        public string SlabNaga { get; set; }

        /// <summary>
        /// スラブ重量
        /// </summary>
        public string SlabWeight { get; set; }

        /// <summary>
        /// 本数
        /// </summary>
        public string SlabCount { get; set; }

        /// <summary>
        /// 再計画
        /// </summary>
        public string RePlan { get; set; }

        /// <summary>
        /// 急ぎ
        /// </summary>
        public string Haste { get; set; }

        /// <summary>
        /// 輸出
        /// </summary>
        public string Export { get; set; }

        /// <summary>
        /// 抽出温度
        /// </summary>
        public string ExtractionTemperature { get; set; }

        /// <summary>
        /// 試圧
        /// </summary>
        public string TestPressure { get; set; }

        /// <summary>
        /// 板巾
        /// </summary>
        public string PlateHaba { get; set; }

        /// <summary>
        /// 歪
        /// </summary>
        public string Strain { get; set; }

        #endregion
    }
}
