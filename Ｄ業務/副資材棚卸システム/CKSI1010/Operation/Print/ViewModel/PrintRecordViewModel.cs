//*************************************************************************************
//
//   棚卸表印刷対象のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

using GalaSoft.MvvmLight;

namespace CKSI1010.Operation.Print.ViewModel
{
    /// <summary>
    /// 棚卸表印刷対象のビューモデル
    /// </summary>
    public class PrintRecordViewModel : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 品目CD
        /// </summary>
        private string hinmokuCD = string.Empty;

        /// <summary>
        /// 業者CD
        /// </summary>
        private string gyosyaCD = string.Empty;

        /// <summary>
        /// 品目名
        /// </summary>
        private string hinmokuName = string.Empty;

        /// <summary>
        /// 業者名
        /// </summary>
        private string gyosyaName = string.Empty;

        /// <summary>
        /// 入庫量
        /// </summary>
        private int nyukoRyo = 0;

        /// <summary>
        /// 返品
        /// </summary>
        private int henpin = 0;

        /// <summary>
        /// 倉庫在庫
        /// </summary>
        private int sokoZaiko = 0;

        /// <summary>
        /// EF在庫
        /// </summary>
        private int efZaiko = 0;

        /// <summary>
        /// LF在庫
        /// </summary>
        private int lfZaiko = 0;

        /// <summary>
        /// CC在庫
        /// </summary>
        private int ccZaiko = 0;

        /// <summary>
        /// 備考
        /// </summary>
        private string bikou = string.Empty;

        /// <summary>
        /// 種別
        /// </summary>
        private string syubetu = string.Empty;

        /// <summary>
        /// 並び順
        /// </summary>
        private int order = 0;

        /// <summary>
        /// その他在庫
        /// </summary>
        private decimal stockOther = 0;

        /// <summary>
        /// メーター在庫
        /// </summary>
        private decimal stockMeter = 0;

        /// <summary>
        /// 予備1
        /// </summary>
        private decimal stockReserve1 = 0;

        /// <summary>
        /// 予備2
        /// </summary>
        private decimal stockReserve2 = 0;

        /// <summary>
        /// 払い
        /// </summary>
        private decimal harai = 0;

        /// <summary>
        /// 前月在庫
        /// </summary>
        private decimal lastMonth = 0;

        /// <summary>
        /// 当月量
        /// </summary>
        private string togetsuryo = null;

        #endregion

        #region プロパティ

        /// <summary>
        /// 品目CD
        /// </summary>
        public string HinmokuCD
        {
            get {return hinmokuCD; }
            set { Set(ref hinmokuCD, value); }
        }

        /// <summary>
        /// 業者CD
        /// </summary>
        public string GyosyaCD
        {
            get { return gyosyaCD; }
            set { Set(ref gyosyaCD, value); }
        }

        /// <summary>
        /// 品目名
        /// </summary>
        public string HinmokuName
        {
            get { return hinmokuName; }
            set { Set(ref hinmokuName, value); }
        }

        /// <summary>
        /// 業者名
        /// </summary>
        public string GyosyaName
        {
            get { return gyosyaName; }
            set { Set(ref gyosyaName, value); }
        }

        /// <summary>
        /// 入庫量
        /// </summary>
        public int NyukoRyo
        {
            get { return nyukoRyo; }
            set { Set(ref nyukoRyo, value); }
        }

        /// <summary>
        /// 返品
        /// </summary>
        public int Henpin
        {
            get { return henpin; }
            set { Set(ref henpin, value); }
        }

        /// <summary>
        /// 倉庫在庫
        /// </summary>
        public int SokoZaiko
        {
            get { return sokoZaiko; }
            set { Set(ref sokoZaiko, value); }
        }

        /// <summary>
        /// EF在庫
        /// </summary>
        public int EfZaiko
        {
            get { return efZaiko; }
            set { Set(ref efZaiko, value); }
        }

        /// <summary>
        /// LF在庫
        /// </summary>
        public int LfZaiko
        {
            get { return lfZaiko; }
            set { Set(ref lfZaiko, value); }
        }

        /// <summary>
        /// CC在庫
        /// </summary>
        public int CcZaiko
        {
            get { return ccZaiko; }
            set { Set(ref ccZaiko, value); }
        }

        /// <summary>
        /// 備考
        /// </summary>
        public string Bikou
        {
            get { return bikou; }
            set { Set(ref bikou, value); }
        }

        /// <summary>
        /// 種別(1の場合は帳票の一部を灰色にする)
        /// </summary>
        public string Syubetu
        {
            get { return syubetu; }
            set { Set(ref syubetu, value); }
        }

        /// <summary>
        /// 並び順
        /// </summary>
        public int Order
        {
            get { return order; }
            set { Set(ref order, value); }
        }

        /// <summary>
        /// その他在庫
        /// </summary>
        public decimal StockOther
        {
            get { return stockOther; }
            set { Set(ref stockOther, value); }
        }

        /// <summary>
        /// メーター在庫
        /// </summary>
        public decimal StockMeter
        {
            get { return stockMeter; }
            set { Set(ref stockMeter, value); }
        }

        /// <summary>
        /// 予備1
        /// </summary>
        public decimal StockReserve1
        {
            get { return stockReserve1; }
            set { Set(ref stockReserve1, value); }
        }

        /// <summary>
        /// 予備2
        /// </summary>
        public decimal StockReserve2
        {
            get { return stockReserve2; }
            set { Set(ref stockReserve2, value); }
        }

        /// <summary>
        /// 出庫量
        /// </summary>
        public decimal Harai
        {
            get { return harai; }
            set { Set(ref harai, value); }
        }

        /// <summary>
        /// 前月在庫
        /// </summary>
        public decimal LastMonth
        {
            get { return lastMonth; }
            set { Set(ref lastMonth, value); }
        }

        /// <summary>
        /// 当月量
        /// </summary>
        public string Togetsuryo
        {
            get { return togetsuryo; }
            set { Set(ref togetsuryo, value); }
        }

        /// <summary>
        /// 印刷対象かどうか
        /// 「入庫量」「返品」「前月在庫」「倉庫在庫」「EF在庫」「LF在庫」「CC在庫」がすべて0以外であれば印刷対象となる
        /// </summary>
        public bool IsPrintTarget
        {
            get
            {
                return ((NyukoRyo != 0) || (Henpin != 0) || (SokoZaiko != 0) || (EfZaiko != 0) || (LfZaiko != 0) || (CcZaiko != 0));
            }
        }

        #endregion

    }
}
