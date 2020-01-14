using GalaSoft.MvvmLight;
using CKSI1010.Types;
using CKSI1010.Core;
using static CKSI1010.Common.Constants;
//*************************************************************************************
//
//   棚卸実績値入力画面(棚卸データ入力作業)のパラメータ情報用のビューモデル
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1010.Operation.InputInventoryActual.ViewModel
{
    /// <summary>
    /// 棚卸実績値入力画面(棚卸データ入力作業)のパラメータ情報用のビューモデル
    /// </summary>
    public class InputInventoryActualRecordViewModel : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 品目CD
        /// </summary>
        private string himokuCd = string.Empty;

        /// <summary>
        /// 業者CD
        /// </summary>
        private string gyosyaCd = string.Empty;

        /// <summary>
        /// 品目名
        /// </summary>
        private string hinmokuName = string.Empty;

        /// <summary>
        /// 業者名
        /// </summary>
        private string gyosyaName = string.Empty;

        /// <summary>
        /// 当月量エラー有無
        /// </summary>
        private bool isError = false;

        /// <summary>
        /// 当月量警告有無
        /// </summary>
        private bool isWarning = false;

        /// <summary>
        /// データ問題なし
        /// </summary>
        private bool isNoProblemData = false;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputInventoryActualRecordViewModel()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 資材区分(キー)
        /// </summary>
        public string SIZAI_KBN { get; set; } = string.Empty;

        /// <summary>
        /// 作業区分(キー)
        /// </summary>
        public string WORK_KBN { get; set; } = string.Empty;

        /// <summary>
        /// 並び順(キー)
        /// </summary>
        public long ITEM_ORDER { get; set; } = 0;

        /// <summary>
        /// 品目CD(キー)
        /// </summary>
        public string HINMOKUCD { get; set; } = string.Empty;

        /// <summary>
        /// 業者CD(キー)
        /// </summary>
        public string GYOSYACD { get; set; } = string.Empty;

        /// <summary>
        /// 置場コード
        /// </summary>
        public string PlaceCode { get; set; } = string.Empty;

        /// <summary>
        /// 資材区分
        /// </summary>
        public ShizaiKbnTypes ShizaiKbn { get; set; }

        /// <summary>
        /// 作業区分
        /// </summary>
        public string WorkKbn { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// </summary>
        public long Order { get; set; } = 0;

        /// <summary>
        /// 当月量
        /// </summary>
        public string TogetuValue { get; set; } = string.Empty;

        /// <summary>
        /// 当月量数値
        /// </summary>
        public long TogetuValueToLong
        {
            get
            {
                if(TogetuValue == null || TogetuValue.Length == 0)
                {
                    return 0;
                }
                else
                {
                    return long.Parse(TogetuValue.Replace(",",""));
                }
            }
        }

        /// <summary>
        /// 当月量数字
        /// </summary>
        public string TogetuValueToNumber
        {
            get
            {
                if (TogetuValue == null || TogetuValue.Length == 0)
                {
                    return TogetuValue;
                }
                else
                {
                    return TogetuValue.Replace(",", "");
                }
            }
            set
            {
                if(value != null && value.Length > 0)
                {
                    TogetuValue = long.Parse(value).ToString("#,0");
                }
                else
                {
                    TogetuValue = value;
                }
            }
        }

        /// <summary>
        /// 入庫量
        /// </summary>
        public long NyukoValue { get; set; } = 0;

        /// <summary>
        /// 出庫量
        /// </summary>
        public long HaraiValue { get; set; } = 0;

        /// <summary>
        /// 返品
        /// </summary>
        public long HenpinValue { get; set; } = 0;

        /// <summary>
        /// 倉庫在庫
        /// </summary>
        public long SoukoValue { get; set; } = 0;

        /// <summary>
        /// EF在庫
        /// </summary>
        public long EfValue { get; set; } = 0;

        /// <summary>
        /// LF在庫
        /// </summary>
        public long LfValue { get; set; } = 0;

        /// <summary>
        /// CC在庫
        /// </summary>
        public long CcValue { get; set; } = 0;

        /// <summary>
        /// 他在庫
        /// </summary>
        public long OthersValue { get; set; } = 0;

        /// <summary>
        /// メーター在庫
        /// </summary>
        public long MeterValue { get; set; } = 0;

        /// <summary>
        /// 予備１在庫
        /// </summary>
        public long Yobi1Value { get; set; } = 0;

        /// <summary>
        /// 予備２在庫
        /// </summary>
        public long Yobi2Value { get; set; } = 0;

        /// <summary>
        /// 品目CD
        /// </summary>
        public string HinmokuCd
        {
            get { return himokuCd; }
            set { Set(ref himokuCd, value); }
        }

        /// <summary>
        /// 業者CD
        /// </summary>
        public string GyosyaCd
        {
            get { return gyosyaCd; }
            set { Set(ref gyosyaCd, value); }
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
        /// 業者CD
        /// </summary>
        public string GyosyaName
        {
            get { return gyosyaName; }
            set { Set(ref gyosyaName, value); }
        }

        /// <summary>
        /// 当月量（予想）
        /// 計算式：入庫量－出庫量－返品＋在庫
        /// </summary>
        public long TogetuYosouValue
        {
            get
            {
                long togetuYosouValue = 0;

                if (PlaceCode == null)
                {
                    return 0;
                }

                if (PlaceCode.Equals(SizaiPlace.SK.GetStringValue()))  // SK
                {
                    togetuYosouValue = NyukoValue - HaraiValue - HenpinValue + SoukoValue;
                }
                else if (PlaceCode.Equals(SizaiPlace.EF.GetStringValue()))  // EF
                {
                    togetuYosouValue = HaraiValue + EfValue;
                }
                else if (PlaceCode.Equals(SizaiPlace.LF.GetStringValue()))  // LF
                {
                    togetuYosouValue = HaraiValue + LfValue;
                }
                else if (PlaceCode.Equals(SizaiPlace.CC.GetStringValue()))  // CC
                {
                    togetuYosouValue = HaraiValue + CcValue;
                }
                else if (PlaceCode.Equals(SizaiPlace.Others.GetStringValue()))  // 他
                {
                    togetuYosouValue = HaraiValue + OthersValue;
                }
                else if (PlaceCode.Equals(SizaiPlace.Meter.GetStringValue()))  // メーター
                {
                    togetuYosouValue = HaraiValue + MeterValue;
                }
                else if (PlaceCode.Equals(SizaiPlace.Yobi1.GetStringValue()))  // 予備１
                {
                    togetuYosouValue = HaraiValue + Yobi1Value;
                }
                else if (PlaceCode.Equals(SizaiPlace.Yobi2.GetStringValue()))  // 予備２
                {
                    togetuYosouValue = HaraiValue + Yobi2Value;
                }

                return togetuYosouValue;
            }
        }

        /// <summary>
        /// 前月末在庫
        /// </summary>
        public long ZengetuValue
        {
            get
            {
                return SoukoValue + EfValue + LfValue + CcValue + OthersValue + MeterValue + Yobi1Value + Yobi2Value;
            }
        }

        /// <summary>
        /// 当月量エラー有無
        /// </summary>
        public bool IsError
        {
            get { return isError; }
            set { Set(ref isError, value); }
        }

        /// <summary>
        /// 当月量警告有無
        /// </summary>
        public bool IsWarning
        {
            get { return isWarning; }
            set { Set(ref isWarning, value); }
        }

        /// <summary>
        /// エラー、警告なし
        /// </summary>
        public bool IsNoProblemData
        {
            get { return isNoProblemData; }
            set { Set(ref isNoProblemData, value); }
        }

        /// <summary>
        /// 当月量（予想）エラー有無
        /// </summary>
        public bool IsTogetuYosouError
        {
            get { return (TogetuYosouValue<0); }
        }

        #endregion

    }
}
