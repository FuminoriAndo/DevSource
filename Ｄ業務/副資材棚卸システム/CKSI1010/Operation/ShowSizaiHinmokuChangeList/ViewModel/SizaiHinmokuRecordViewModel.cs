using GalaSoft.MvvmLight;
using System;
//*************************************************************************************
//
//   資材品目のレコードビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.ShowSizaiHinmokuChangeList.ViewModel
{
    /// <summary>
    /// 資材品目のレコードビューモデル
    /// </summary>
    public class SizaiHinmokuRecordViewModel : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 品目CD
        /// </summary>
        private string hinmokuCode = string.Empty;

        /// <summary>
        /// 品目名
        /// </summary>
        private string hinmokuName = string.Empty;

        /// <summary>
        /// 費目
        /// </summary>
        private string himoku = string.Empty;

        /// <summary>
        /// 内訳
        /// </summary>
        private string utiwake = string.Empty;

        /// <summary>
        /// 棚番
        /// </summary>
        private string tanaban = string.Empty;

        /// <summary>
        /// 単位
        /// </summary>
        private string tani = string.Empty;

        /// <summary>
        /// 受払い種別(種別)
        /// </summary>
        private string ukebaraiType = string.Empty;

        /// <summary>
        /// 水分区分(水分引き)
        /// </summary>
        private string suibunKbn = string.Empty;

        /// <summary>
        /// 検収区分(検収明細出力)
        /// </summary>
        private string kensyuKbn = string.Empty;

        /// <summary>
        /// 報告区分(経理報告)
        /// </summary>
        private string houkokuKbn = string.Empty;

        /// <summary>
        /// 出庫位置区分(出庫位置)
        /// </summary>
        private string ichiKbn = string.Empty;

        /// <summary>
        /// 向先
        /// </summary>
        private string mukesaki = string.Empty;

        /// <summary>
        /// 単価設定
        /// </summary>
        private string tankaSetting = string.Empty;

        /// <summary>
        /// 更新日
        /// </summary>
        private DateTime updYMD;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SizaiHinmokuRecordViewModel() { }

        #endregion

        #region プロパティ

        /// <summary>
        /// 品目CD
        /// </summary>
        public string HinmokuCode
        {
            get { return hinmokuCode; }
            set { Set(ref this.hinmokuCode, value); }
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
        /// 費目
        /// </summary>
        public string Himoku
        {
            get { return himoku; }
            set { Set(ref this.himoku, value); }
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
        /// 棚番
        /// </summary>
        public string Tanaban
        {
            get { return tanaban; }
            set { Set(ref this.tanaban, value); }
        }

        /// <summary>
        /// 単位
        /// </summary>
        public string Tani
        {
            get { return tani; }
            set { Set(ref this.tani, value); }
        }

        /// <summary>
        /// 受払い種別(種別)
        /// </summary>
        public string UkebaraiType
        {
            get { return ukebaraiType; }
            set { Set(ref this.ukebaraiType, value); }
        }

        /// <summary>
        /// 水分区分(水分引き)
        /// </summary>
        public string SuibunKbn
        {
            get { return suibunKbn; }
            set { Set(ref this.suibunKbn, value); }
        }

        /// <summary>
        /// 検収区分(検収明細出力)
        /// </summary>
        public string KensyuKbn
        {
            get { return kensyuKbn; }
            set { Set(ref this.kensyuKbn, value); }
        }

        /// <summary>
        /// 報告区分(経理報告)
        /// </summary>
        public string HoukokuKbn
        {
            get { return houkokuKbn; }
            set { Set(ref this.houkokuKbn, value); }
        }

        /// <summary>
        /// 出庫位置区分(出庫位置)
        /// </summary>
        public string IchiKbn
        {
            get { return ichiKbn; }
            set { Set(ref this.ichiKbn, value); }
        }

        /// <summary>
        /// 向先
        /// </summary>
        public string Mukesaki
        {
            get { return mukesaki; }
            set { Set(ref this.mukesaki, value); }
        }

        /// <summary>
        /// 単価設定
        /// </summary>
        public string TankaSetting
        {
            get { return tankaSetting; }
            set { Set(ref this.tankaSetting, value); }
        }

        /// <summary>
        /// 更新日
        /// </summary>
        public DateTime UpdYMD
        {
            get { return updYMD; }
            set { Set(ref this.updYMD, value); }
        }

        #endregion

    }
}
