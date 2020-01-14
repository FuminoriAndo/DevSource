//*************************************************************************************
//
//   定数クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

using static CKSI1010.Common.Constants;

namespace CKSI1010.Common
{
    /// <summary>
    /// 定数クラス
    /// </summary>
    public static class Constants
    {
        #region 定数

        /// <summary>
        /// SQL結果(OK)
        /// </summary>
        public const int SQL_RESULT_OK = 0;

        /// <summary>
        /// SQL結果(キー重複)
        /// </summary>
        public const int SQL_RESULT_ALREADY_EXISTS = 1;

        /// <summary>
        /// SQL結果(NG)
        /// </summary>
        public const int SQL_RESULT_NG = -1;

        /// <summary>
        /// 読点
        /// </summary>
        public const string Comma_StringDefine = "、";

        /// <summary>
        /// SK
        /// </summary>
        public const string SK_StringDefine = "SK";

        /// <summary>
        /// EF
        /// </summary>
        public const string EF_StringDefine = "EF";

        /// <summary>
        /// LF
        /// </summary>
        public const string LF_StringDefine = "LF";

        /// <summary>
        /// 築炉
        /// </summary>
        public const string Chikuro_StringDefine = "築炉";

        /// <summary>
        /// 築炉
        /// </summary>
        public const string Chikuro_String_Another_Define = "CHIKURO";

        /// <summary>
        /// LD
        /// </summary>
        public const string LD_StringDefine = "LD";

        /// <summary>
        /// TD
        /// </summary>
        public const string TD_StringDefine = "TD";

        /// <summary>
        /// CC
        /// </summary>
        public const string CC_StringDefine = "CC";

        /// <summary>
        /// 他
        /// </summary>
        public const string Others_StringDefine = "他";

        /// <summary>
        /// 他
        /// </summary>
        public const string Others_String_Another_Define = "ETC";

        /// <summary>
        /// 品目コード
        /// </summary>
        public const string HinmokuCode_StringDefine = "品目CD ： ";

        /// <summary>
        /// 品目名
        /// </summary>
        public const string HinmokuName_StringDefine = "品目名:";

        /// <summary>
        /// 業者名
        /// </summary>
        public const string GyosyaName_StringDefine = "業者名:";

        /// <summary>
        /// 備考
        /// </summary>
        public const string Bikou_StringDefine = "備考:";

        /// <summary>
        /// 並び順
        /// </summary>
        public const string SortOrder_StringDefine = "並び順：";

        /// <summary>
        /// 費目03(副原材料費)
        /// </summary>
        public const string Himoku_FukuGenzaiRyouhi = "03";

        /// <summary>
        /// 通常(エラー、警告なし)
        /// </summary>
        public const string Normal_Status = "0";

        /// <summary>
        /// 通常(エラー、警告なし)
        /// </summary>
        public const string Normal_Information = "0";

        /// <summary>
        /// エラー
        /// </summary>
        public const string Error_StringDefine = "エラー";

        /// <summary>
        /// エラー
        /// </summary>
        public const string Error_Status = "1";

        /// <summary>
        /// 警告
        /// </summary>
        public const string Warning_StringDefine = "警告";

        /// <summary>
        /// 警告
        /// </summary>
        public const string Warning_Status = "2";

        /// <summary>
        /// 警告(複数)
        /// </summary>
        public const string MultiWarning_Status = "3";

        /// <summary>
        /// 在庫・入庫・出庫数量の何れかにマイナス値が登録されている
        /// </summary>
        public const string Ukebarai_Amount_Minus = "1";

        /// <summary>
        /// 月末、月初在庫の桁の差が閾値を超えている
        /// </summary>
        public const string Ukebarai_Difference_Start_And_End_Zaiko_Limit_Over = "1";

        /// <summary>
        /// 出庫してはいけない向先に出庫されている
        /// </summary>
        public const string Ukebarai_Do_Not_Shukko_Mukesaki = "2";

        /// <summary>
        /// 警告(複数)
        /// </summary>
        public const string Ukebarai_MultiWarning = "3";

        /// <summary>
        /// エラー区分（エラーなし）
        /// </summary>
        public const byte ActualValue_No_Error = 0x00;

        /// <summary>
        /// エラー区分（未入力）
        /// </summary>
        public const byte ActualValue_Not_Input = 0x01;

        /// <summary>
        /// エラー区分（当月量予想オーバー）
        /// </summary>
        public const byte ActualValue_Over_Yosou = 0x02;

        /// <summary>
        /// エラー区分（入力文字長オーバー）
        /// </summary>
        public const byte ActualValue_Over_Input_Length = 0x04;

        /// <summary>
        /// エラー区分（当月量 >= 当月入庫量 + 前月在庫量）
        /// </summary>
        public const byte ActualValue_Over_Stock = 0x08;

        #endregion

        #region 列挙型

        /// <summary>
        /// システム分類
        /// </summary>
        public enum SystemCategory
        {
            Sizai = 1, // 資材
            Buhin = 2  // 部品
        }

        /// <summary>
        /// 資材区分
        /// </summary>
        public enum SizaiCategory
        {
            None = 0,
            SK = 1,
            EF = 2,
            LF = 3,
            Building = 4,
            LD = 5,
            TD = 6,
            CC = 7,
            Others = 8,
            Meter = 9,
            Yobi1 = 10,
            Yobi2 = 11
        }

        /// <summary>
        /// 資材区分(棚卸表印刷用)
        /// </summary>
        public enum InventoryPrintTieSizaiCategory
        {
            SK = 0,
            EF = 1,
            LF = 2,
            Chikuro = 3,
            LD = 4,
            TD = 5,
            CC = 6,
            ETC = 7,
            Max = 8
        }

        /// <summary>
        /// 向先
        /// </summary>
        public enum Mukesaki
        {
            EF = 1,
            LF = 2,
            Building = 2,
            LD = 2,
            CC = 3,
            TD = 7,
            Others = 4
        }

        /// <summary>
        /// 資材置場
        /// </summary>
        public enum SizaiPlace
        {
            SK = 1,
            EF = 2,
            LF = 3,
            CC = 4,
            Others = 5,
            Meter = 6,
            Yobi1 = 7,
            Yobi2 = 8
        }

        /// <summary>
        /// 棚卸グループ区分
        /// </summary>
        public enum SizaiGroupCategory
        {
            All = 1,        // 全作業
            Tanaoroshi = 2, // 棚卸データ入力作業
            Kensin = 3      // 検針データ入力作業
        }

        /// <summary>
        /// 資材入力作業区分
        /// </summary>
        public enum SizaiWorkCategory
        {
            TanaoroshiWork = 1, // 棚卸データ入力作業
            KensinWork = 2      // 検針データ入力作業
        }

        /// <summary>
        /// 資材作業誌区分
        /// </summary>
        public enum SagyosiKbn
        {
            Nyuko = 1,    // 入庫
            Shukko = 2,   // 出庫
            Chokusou = 3, // 直送
            Henpin = 4    // 返品
        }

        /// <summary>
        /// 受払い種別
        /// </summary>
        public enum UkebaraiType
        {
            SiyoudakaBarai = 0, // 使用高払い
            NyukoBarai = 1      // 入庫払い
        }

        /// <summary>
        /// 操作状況
        /// </summary>
        public enum OperationCondition
        {
            Modify = 0, // 修正
            Fix = 1,    // 確定
        }

        /// <summary>
        /// 検針データの種類
        /// </summary>
        public enum KensinDataType
        {
            Nyuko = 1,
            Shukko = 2
        }

        /// <summary>
        /// 編集モード
        /// </summary>
        public enum EditMode
        {
            None = 0,
            Add = 1,
            Modify = 2
        }

        /// <summary>
        /// ログ種類
        /// </summary>
        public enum LogType
        {
           Normal = 1, //通常
           Error = 2 //エラー
        }

        /// <summary>
        /// ログ操作種別
        /// </summary>
        public enum LogOperationType
        {
            TanaorosiYearMonthCheck = 1,  //実施月の確認
            ShowFlow = 2, //手順確認
            PrintInventorySheet = 3,   // 棚卸表印刷
            InputInventoryActual = 4,　// 棚卸実績値入力(棚卸データ入力)
            UpdateInventoryActual = 5, // 棚卸実績更新
            InputInputWorkNote = 6, //資材班作業誌入力(液酸入庫)
            CreateDetails = 7, //検収明細書作成
            InputOutputWorkNote = 8, //資材班作業誌入力(液酸出庫)
            UsePaymentCheck = 9,  // 受払いチェック(使用高払い)
            InputPaymentCheck = 10,// 受払いチェック(入庫払い)
            IssueCaluculation = 11, // 計算書発行
            PrintWitnessInventory = 12, //立会い用棚卸表印刷
            PrintFinancingPresetationInventory = 13, //財務提出用棚卸表印刷
            CreatePurchasingCheckData = 14, //購買検収データ作成
            CorrectInventoryActual = 15, //当月量修正
            ShowSizaiHinmokuChangeList = 16 //資材品目マスタの変更履歴
        }

        /// <summary>
        /// ログ操作内容
        /// </summary>
        public enum LogOperationContent
        {
            Fix = 1,     // 確定
            Modify = 2,  // 修正
            Print = 3,   // 印刷
            Update  =4,  // 更新
            Excute = 5,  // 実行
            Save = 6,    // 保存
            Add = 7,     // 追加
            Change = 8,  // 変更
            Delete= 9,   // 削除
            Up = 10,     // 上へ
            Down = 11,   // 下へ
            Next = 12,   // 次へ
            Previous = 13, // 前へ
            Close = 14   // 閉じる
        }

        #endregion
    }

    /// <summary>
    /// 受払い種別のヘルパー
    /// </summary>
    public static class UkebaraiTypeHelper
    {

        #region メソッド

        /// <summary>
        /// 受払い種別を判別するためのValueを取得する
        /// </summary>
        public static string GetMeanValue(this UkebaraiType ukebaraiType)
        {
            string[] meanValue = { "2", "1" };
            return meanValue[(int)ukebaraiType];
        }

        #endregion
    }
}