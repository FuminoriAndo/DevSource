//*************************************************************************************
//
//   受払いチェック(使用高払い)のパラメータ情報用のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************
using GalaSoft.MvvmLight;

namespace CKSI1010.Operation.UsePaymentCheck.ViewModel
{
    /// <summary>
    /// 受払いチェック(使用高払い)のパラメータ情報用のビューモデル
    /// </summary>
    public class UsePaymentRecordViewModel : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 選択されているか
        /// </summary>
        private bool isChecked = false;

        /// <summary>
        /// 表示データ(情報)
        /// </summary>
        private string infomation = string.Empty;

        /// <summary>
        /// 表示データ(表示内容)
        /// </summary>
        private string informationDetail = string.Empty;

        /// <summary>
        /// 表示コード(表示内容に紐づくコード)
        /// </summary>
        private string informationCode = string.Empty;

        /// <summary>
        /// 表示データ(内訳)
        /// </summary>
        private string utiwake = string.Empty;

        /// <summary>
        /// 表示データ(内訳名)
        /// </summary>
        private string utiwakeName = string.Empty;

        /// <summary>
        /// 表示データ(棚番)
        /// </summary>
        private string shelfNo = string.Empty;

        /// <summary>
        /// 品目CD
        /// </summary>
        private string hinmokuCd = string.Empty;

        /// <summary>
        /// 表示データ(品目名)
        /// </summary>
        private string hinmokuName = string.Empty;

        /// <summary>
        /// 表示データ(口座名2)
        /// </summary>
        private string kouzaName2 = string.Empty;

        /// <summary>
        /// 表示データ(単位)
        /// </summary>
        private string unit = string.Empty;

        /// <summary>
        /// 月初在庫
        /// </summary>
        private string szaiko = string.Empty;

        /// <summary>
        /// 当月入庫
        /// </summary>
        private string nyuko = string.Empty;

        /// <summary>
        /// EF出庫
        /// </summary>
        private string sef = string.Empty;

        /// <summary>
        /// LF出庫
        /// </summary>
        private string slf = string.Empty;

        /// <summary>
        /// CC出庫
        /// </summary>
        private string scc = string.Empty;

        /// <summary>
        /// その他
        /// </summary>
        private string ssonota = string.Empty;

        /// <summary>
        /// 事業開発
        /// </summary>
        private string sjigyo = string.Empty;

        /// <summary>
        /// 1次切断
        /// </summary>
        private string s1ji = string.Empty;

        /// <summary>
        /// TD出庫
        /// </summary>
        private string std = string.Empty;

        /// <summary>
        /// 2次切断
        /// </summary>
        private string s2ji = string.Empty;

        /// <summary>
        /// 月末在庫
        /// </summary>
        private string ezaiko = string.Empty;

        /// <summary>
        /// 状態(エラーなし：0、エラー：1、警告：2)
        /// </summary>
        private int status = 0;

        /// <summary>
        /// 選択行かどうか
        /// </summary>
        private bool isSelected = false;

        /// <summary>
        /// 費目
        /// </summary>
        private string himoku = string.Empty;

        /// <summary>
        /// 費目名
        /// </summary>
        private string himokuName = string.Empty;

        #endregion

        #region プロパティ

        /// <summary>
        /// 選択されているか
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
            set { Set(ref this.isChecked, value); }
        }

        /// <summary>
        /// 情報
        /// </summary>
        public string Infomation
        {
            get { return infomation; }
            set { Set(ref this.infomation, value); }
        }

        /// <summary>
        /// 表示データ(表示内容)
        /// </summary>
        public string InformationDetail
        {
            get { return informationDetail; }
            set { Set(ref this.informationDetail, value); }
        }

        /// <summary>
        /// 表示コード(表示内容に紐づくコード)
        /// </summary>
        public string InformationCode
        {
            get { return informationCode; }
            set { Set(ref this.informationCode, value); }
        }

        /// <summary>
        /// 内訳
        /// </summary>
        public string Utiwake
        {
            get { return utiwake; }
            set { Set(ref this.utiwake, value); }
        }

        /// <summary>
        /// 内訳名
        /// </summary>
        public string UtiwakeName
        {
            get { return utiwakeName; }
            set { Set(ref this.utiwakeName, value); }
        }

        /// <summary>
        /// 棚番
        /// </summary>
        public string ShelfNo
        {
            get { return shelfNo; }
            set { Set(ref this.shelfNo, value); }
        }

        /// <summary>
        /// 品目CD
        /// </summary>
        public string HinmokuCd
        {
            get { return hinmokuCd; }
            set { Set(ref this.hinmokuCd, value); }
        }

        /// <summary>
        /// 品目名
        /// </summary>
        public string HinmokuName
        {
            get { return hinmokuName; }
            set { Set(ref this.hinmokuName, value); }
        }

        /// <summary>
        /// 口座名2
        /// </summary>
        public string KouzaName2
        {
            get { return kouzaName2; }
            set { Set(ref this.kouzaName2, value); }
        }

        /// <summary>
        /// 単位
        /// </summary>
        public string Unit
        {
            get { return unit; }
            set { Set(ref this.unit, value); }
        }

        /// <summary>
        /// 月初在庫
        /// </summary>
        public string Szaiko
        {
            get { return szaiko; }
            set { Set(ref this.szaiko, value); }
        }

        /// <summary>
        /// 当月入庫
        /// </summary>
        public string Nyuko
        {
            get { return nyuko; }
            set { Set(ref this.nyuko, value); }
        }

        /// <summary>
        /// EF出庫
        /// </summary>
        public string SEF
        {
            get { return sef; }
            set { Set(ref this.sef, value); }
        }

        /// <summary>
        /// LF出庫
        /// </summary>
        public string SLF
        {
            get { return slf; }
            set { Set(ref this.slf, value); }
        }

        /// <summary>
        /// CC出庫
        /// </summary>
        public string SCC
        {
            get { return scc; }
            set { Set(ref this.scc, value); }
        }

        /// <summary>
        /// その他
        /// </summary>
        public string SSonota
        {
            get { return ssonota; }
            set { Set(ref this.ssonota, value); }
        }

        /// <summary>
        /// 事業開発
        /// </summary>
        public string Sjigyo
        {
            get { return sjigyo; }
            set { Set(ref this.sjigyo, value); }
        }

        /// <summary>
        /// 1次切断
        /// </summary>
        public string S1ji
        {
            get { return s1ji; }
            set { Set(ref this.s1ji, value); }
        }

        /// <summary>
        /// TD出庫
        /// </summary>
        public string STD
        {
            get { return std; }
            set { Set(ref this.std, value); }
        }

        /// <summary>
        /// 2次切断
        /// </summary>
        public string S2ji
        {
            get { return s2ji; }
            set { Set(ref this.s2ji, value); }
        }

        /// <summary>
        /// 月末在庫
        /// </summary>
        public string Ezaiko
        {
            get { return ezaiko; }
            set { Set(ref this.ezaiko, value); }
        }

        /// <summary>
        /// エラー状態(エラーなし：0、エラー：1、警告：2)
        /// </summary>
        public int Status
        {
            get { return status; }
            set { Set(ref this.status, value); }
        }

        /// <summary>
        /// 選択行かどうか
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set { Set(ref this.isSelected, value); }
        }

        /// <summary>
        /// 費目
        /// </summary>
        public string Himoku
        {
            get{ return himoku; }
            set { Set(ref this.himoku, value); }
        }

        /// <summary>
        /// 費目名
        /// </summary>
        public string HimokuName
        {
            get { return himokuName; }
            set { Set(ref this.himokuName, value); }
        }

        #endregion

    }
}
