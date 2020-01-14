//*************************************************************************************
//
//   資材品目の項目
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************
using System;

namespace CKSI1010.Shared.Model
{
    /// <summary>
    /// 資材品目の項目
    /// </summary>
    public class SizaiHinmokuItem
    {
        /// <summary>
        /// 品目CD
        /// </summary>
        public string HinmokuCode { get; set; } = string.Empty;

        /// <summary>
        /// 品目名
        /// </summary>
        public string HinmokuName { get; set; } = string.Empty;

        /// <summary>
        /// 費目
        /// </summary>
        public string Himoku { get; set; } = string.Empty;

        /// <summary>
        /// 内訳
        /// </summary>
        public string Utiwake { get; set; } = string.Empty;

        /// <summary>
        /// 棚番
        /// </summary>
        public string Tanaban { get; set; } = string.Empty;

        /// <summary>
        /// 単位
        /// </summary>
        public string Tani { get; set; } = string.Empty;

        /// <summary>
        /// 受払い種別(種別)
        /// </summary>
        public string UkebaraiType { get; set; } = string.Empty;

        /// <summary>
        /// 水分区分(水分引き)
        /// </summary>
        public string SuibunKbn { get; set; } = string.Empty;

        /// <summary>
        /// 検収区分(検収明細出力)
        /// </summary>
        public string KensyuKbn { get; set; } = string.Empty;

        /// <summary>
        /// 報告区分(経理報告)
        /// </summary>
        public string HoukokuKbn { get; set; } = string.Empty;

        /// <summary>
        /// 出庫位置区分(出庫位置)
        /// </summary>
        public string IchiKbn { get; set; } = string.Empty;

        /// <summary>
        /// 向先
        /// </summary>
        public string Mukesaki { get; set; } = string.Empty;

        /// <summary>
        /// 単価設定
        /// </summary>
        public string TankaSetting { get; set; } = string.Empty;

        /// <summary>
        /// 更新日
        /// </summary>
        public DateTime UpdYMD { get; set; } = DateTime.Now;

    }
}
