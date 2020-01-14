//*************************************************************************************
//
//   立会い用棚卸表印刷のモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Shared.Model
{
    /// <summary>
    /// 立会い用棚卸表印刷のモデル
    /// </summary>
    public class InventoryWitness
    {
        #region プロパティ

        /// <summary>
        /// 品目CD
        /// </summary>
        public string HinmokuCD { get; set; }

        /// <summary>
        /// 品目名
        /// </summary>
        public string HinmokuName { get; set; }

        /// <summary>
        /// HIMOKU
        /// </summary>
        public string Himoku { get; set; }

        /// <summary>
        /// 内訳
        /// </summary>
        public string Utiwake { get; set; }

        /// <summary>
        /// 棚番
        /// </summary>
        public string Tanaban { get; set; }

        /// <summary>
        /// 単位
        /// </summary>
        public string Tani { get; set; }

        /// <summary>
        /// 倉庫在庫
        /// </summary>
        public long ZSK { get; set; }

        /// <summary>
        /// EF在庫
        /// </summary>
        public long ZEF { get; set; }

        /// <summary>
        /// LF在庫
        /// </summary>
        public long ZLF { get; set; }

        /// <summary>
        /// CC在庫
        /// </summary>
        public long ZCC { get; set; }

        /// <summary>
        /// その他在庫
        /// </summary>
        public long ZETC { get; set; }

        /// <summary>
        /// メータ在庫
        /// </summary>
        public long ZMeter { get; set; }

        /// <summary>
        /// 現場数量
        /// </summary>
        public long GenbaSuryo { get; set; }

        /// <summary>
        /// 合計
        /// </summary>
        public long Total { get; set; }

        #endregion
    }
}
