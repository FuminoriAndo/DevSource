//*************************************************************************************
//
//   カスタムファイルリーダのインターフェース
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using FileManager.Reader.Interface;

namespace ProductionControlDailyExcelCreator.FileReader.Interface
{
    /// <summary>
    /// PMGQ010Bファイルのリーダ用のインターフェース
    /// </summary>
    public interface IPMGQ010BReader : IFileReader
    {
        #region メソッド

        /// <summary>
        /// 受注№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№</returns>
        string GetOrderNo(int recordIndex);

        /// <summary>
        /// 需要家名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家名</returns>
        string GetCustomerName(int recordIndex);

        /// <summary>
        /// 規格名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格名</returns>
        string GetStandardName(int recordIndex);

        /// <summary>
        /// 製品サイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品サイズ</returns>
        string GetProductSize(int recordIndex);

        /// <summary>
        /// 製品(厚)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(厚)</returns>
        string GetProductAtu(int recordIndex);

        /// <summary>
        /// 製品(巾)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(巾)</returns>
        string GetProductHaba(int recordIndex);

        /// <summary>
        /// 製品(長)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(長)</returns>
        string GetProductNaga(int recordIndex);

        /// <summary>
        /// 定耳の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>定耳</returns>
        string GetTM(int recordIndex);

        /// <summary>
        /// 製品コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品コード</returns>
        string GetProductCode(int recordIndex);

        /// <summary>
        /// ロールサイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロールサイズ</returns>
        string GetRollSize(int recordIndex);

        /// <summary>
        /// ロールサイズ(巾)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロールサイズ(巾)</returns>
        string GetRollHaba(int recordIndex);

        /// <summary>
        /// ロールサイズ(長)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロールサイズ(長)</returns>
        string GetRollNaga(int recordIndex);

        /// <summary>
        /// TP区分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>TP区分</returns>
        string GetTPClassfication(int recordIndex);

        /// <summary>
        /// ロットの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロット</returns>
        string GetLot(int recordIndex);

        /// <summary>
        /// ロット1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロット1</returns>
        string GetLot1(int recordIndex);

        /// <summary>
        /// ロット2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロット2</returns>
        string GetLot2(int recordIndex);

        /// <summary>
        /// 圧延予定の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>圧延予定</returns>
        string GetAtuenYMD(int recordIndex);

        /// <summary>
        /// チャージ№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>チャージ№</returns>
        string GetCHNo(int recordIndex);

        /// <summary>
        /// スラブ№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ№</returns>
        string GetSlabNo(int recordIndex);

        /// <summary>
        /// ロール№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロール№</returns>
        string GetRollNo(int recordIndex);

        /// <summary>
        /// 抽出日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出日</returns>
        string GetExtractionYMD(int recordIndex);

        #endregion

    }

    /// <summary>
    /// PMPA260Bファイルのリーダ用のインターフェース
    /// </summary>
    public interface IPMPA260BReader : IFileReader
    {
        #region メソッド

        /// <summary>
        /// RCNOの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>RCNO</returns>
        string GetRCNO(int recordIndex);

        /// <summary>
        /// 勤務の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>勤務</returns>
        string GetWork(int recordIndex);

        /// <summary>
        /// 圧延順の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>圧延順</returns>
        string GetAtuenOrder(int recordIndex);

        /// <summary>
        /// ロット№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロット№</returns>
        string GetLotNo(int recordIndex);

        /// <summary>
        /// ロット1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロット1</returns>
        string GetLot1(int recordIndex);

        /// <summary>
        /// ロット2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロット2</returns>
        string GetLot2(int recordIndex);

        /// <summary>
        /// チャージ№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>チャージ№</returns>
        string GetCHNo(int recordIndex);

        /// <summary>
        /// チャージ("-")の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>チャージ("-")</returns>
        string GetCHDash(int recordIndex);

        /// <summary>
        /// 1次鋳片№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>1次鋳片№</returns>
        string GetSlab1No(int recordIndex);

        /// <summary>
        /// 2次鋳片№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>2次鋳片№</returns>
        string GetSlab2No(int recordIndex);

        /// <summary>
        /// 材料識別の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>材料識別</returns>
        string GetMaterialsIdentification(int recordIndex);

        /// <summary>
        /// 材料識別1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>材料識別1</returns>
        string GetMaterialsIdentification1(int recordIndex);

        /// <summary>
        /// 材料識別2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>材料識別2</returns>
        string GetMaterialsIdentification2(int recordIndex);

        /// <summary>
        /// ＴＰ管理の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ＴＰ管理</returns>
        string GetTPManagement(int recordIndex);

        /// <summary>
        /// 厚テーバーの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>厚テーバー</returns>
        string GetAtuTapera(int recordIndex);

        /// <summary>
        /// 巾テーパーの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>巾テーパー</returns>
        string GetHabaTapera(int recordIndex);

        /// <summary>
        /// ＨＣ区分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ＨＣ区分</returns>
        string GetHCDivision(int recordIndex);

        /// <summary>
        /// 抽出時分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出時分</returns>
        string GetExtractionHHMM(int recordIndex);

        /// <summary>
        /// 抽出時の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出時</returns>
        string GetExtractionHH(int recordIndex);

        /// <summary>
        /// 抽出分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出分</returns>
        string GetExtractionMM(int recordIndex);

        /// <summary>
        /// 規格の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格</returns>
        string GetStandard(int recordIndex);

        /// <summary>
        /// ロールサイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロールサイズ</returns>
        string GetRollSize(int recordIndex);

        /// <summary>
        /// ロール厚の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロール厚</returns>
        string GetRollAtu(int recordIndex);

        /// <summary>
        /// ロール巾の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロール巾</returns>
        string GetRollHaba(int recordIndex);

        /// <summary>
        /// ロール長の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロール長</returns>
        string GetRollNaga(int recordIndex);

        /// <summary>
        /// スラブサイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ</returns>
        string GetSlabSize(int recordIndex);

        /// <summary>
        /// スラブ厚の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ厚</returns>
        string GetSlabAtu(int recordIndex);

        /// <summary>
        /// スラブ巾の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ巾</returns>
        string GetSlabHaba(int recordIndex);

        /// <summary>
        /// スラブ長の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ長</returns>
        string GetSlabNaga(int recordIndex);

        /// <summary>
        /// スラブ重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ重量</returns>
        string GetSlabWeight(int recordIndex);

        /// <summary>
        /// 本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>本数</returns>
        string GetSlabCount(int recordIndex);

        /// <summary>
        /// 再計画の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>再計画</returns>
        string GetRePlan(int recordIndex);

        /// <summary>
        /// 急ぎの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>急ぎ</returns>
        string GetHaste(int recordIndex);

        /// <summary>
        /// 輸出の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>輸出</returns>
        string GetExport(int recordIndex);

        /// <summary>
        /// 抽出温度の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出温度</returns>
        string GetExtractionTemperature(int recordIndex);

        /// <summary>
        /// 試圧の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>試圧</returns>
        string GetTestPressure(int recordIndex);

        /// <summary>
        /// 板巾の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>板巾</returns>
        string GetPlateHaba(int recordIndex);

        /// <summary>
        /// 歪の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>歪</returns>
        string GetStrain(int recordIndex);

        #endregion

    }

    /// <summary>
    /// PMPD330Bファイルのリーダ用のインターフェース
    /// </summary>
    public interface IPMPD330BReader : IFileReader
    {
        #region メソッド

        /// <summary>
        /// 鋼種名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>鋼種名</returns>
        string GetSteelsName(int recordIndex);

        /// <summary>
        /// 鋼種別の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>鋼種別</returns>
        string GetSteelsType(int recordIndex);

        /// <summary>
        /// 規格名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格名</returns>
        string GetStandardName(int recordIndex);

        /// <summary>
        /// 製品サイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品サイズ</returns>
        string GetProductSize(int recordIndex);

        /// <summary>
        /// 製品厚の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品厚</returns>
        string GetProductAtu(int recordIndex);

        /// <summary>
        /// 製品巾の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品巾</returns>
        string GetProductHaba(int recordIndex);

        /// <summary>
        /// 製品長の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品長</returns>
        string GetProductNaga(int recordIndex);

        /// <summary>
        /// 定耳の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>定耳</returns>
        string GetTM(int recordIndex);

        /// <summary>
        /// 製品コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品コード</returns>
        string GetProductCode(int recordIndex);

        /// <summary>
        /// 受注№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№</returns>
        string GetOrdersNo(int recordIndex);

        /// <summary>
        /// 需要家名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家名</returns>
        string GetCustomerName(int recordIndex);

        /// <summary>
        /// 納期月日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期月日</returns>
        string GetDeliveryDate(int recordIndex);

        /// <summary>
        /// 納期ランクの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期ランク</returns>
        string GetDeliveryDateRank(int recordIndex);

        /// <summary>
        /// 枚数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>枚数</returns>
        string GetMaisuu(int recordIndex);

        /// <summary>
        /// ロールサイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロールサイズ</returns>
        string GetRollSize(int recordIndex);

        /// <summary>
        /// ロール巾の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロール巾</returns>
        string GetRollHaba(int recordIndex);

        /// <summary>
        /// ロール長の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロール長</returns>
        string GetRollNaga(int recordIndex);

        /// <summary>
        /// 保温区分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>保温区分</returns>
        string GetHeatInsulation(int recordIndex);

        /// <summary>
        /// スラブサイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ</returns>
        string GetSlabSize(int recordIndex);

        /// <summary>
        /// スラブ厚の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ厚</returns>
        string GetSlabAtu(int recordIndex);

        /// <summary>
        /// スラブ巾の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ巾</returns>
        string GetSlabHaba(int recordIndex);

        /// <summary>
        /// スラブ長の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ長</returns>
        string GetSlabNaga(int recordIndex);

        /// <summary>
        /// スラブ重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ重量</returns>
        string GetSlabWeight(int recordIndex);

        /// <summary>
        /// スラブ本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブ本数</returns>
        string GetSlabCount(int recordIndex);

        /// <summary>
        /// 急ぎ区分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>急ぎ区分</returns>
        string GetHasteDivision(int recordIndex);

        /// <summary>
        /// 特殊区分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>特殊区分</returns>
        string GetSpecialDivision(int recordIndex);

        /// <summary>
        /// 輸出区分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>輸出区分</returns>
        string GetExportDivision(int recordIndex);

        #endregion

    }

    /// <summary>
    /// PMPF070Bファイルのリーダ用のインターフェース
    /// </summary>
    public interface IPMPF070BReader : IFileReader
    {
        #region メソッド

        /// <summary>
        /// 受注№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№</returns>
        string GetOrderNo(int recordIndex);

        /// <summary>
        /// 受注№1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№1</returns>
        string GetOrderNo1(int recordIndex);

        /// <summary>
        /// 受注№2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№2</returns>
        string GetOrderNo2(int recordIndex);

        /// <summary>
        /// 受注№3の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№3</returns>
        string GetOrderNo3(int recordIndex);

        /// <summary>
        /// 納期の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期</returns>
        string GetDeliveryDate(int recordIndex);

        /// <summary>
        /// 納期(MM)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期(MM)</returns>
        string GetDeliveryDateMM(int recordIndex);

        /// <summary>
        /// 納期(DD)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期(DD)</returns>
        string GetDeliveryDateDD(int recordIndex);

        /// <summary>
        /// 納期ランクの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期ランク</returns>
        string GetDeliveryDateRank(int recordIndex);

        /// <summary>
        /// 需要家名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家名</returns>
        string GetCustomerName(int recordIndex);

        /// <summary>
        /// ロットNoの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロットNo</returns>
        string GetLotNo(int recordIndex);

        /// <summary>
        /// ロット1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロット1</returns>
        string GetLot1(int recordIndex);

        /// <summary>
        /// ロット2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロット2</returns>
        string GetLot2(int recordIndex);

        /// <summary>
        /// ロール№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ロール№</returns>
        string GetRollNo(int recordIndex);

        /// <summary>
        /// 未採取ロットの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>未採取ロット</returns>
        string GetNotCollectedLot(int recordIndex);

        /// <summary>
        /// 未採取ロット1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>未採取ロット1</returns>
        string GetNotCollectedLotNo1(int recordIndex);

        /// <summary>
        /// 未採取ロット2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>未採取ロット2</returns>
        string GetNotCollectedLotNo2(int recordIndex);

        /// <summary>
        /// 状態の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>状態</returns>
        string GetStatus(int recordIndex);

        /// <summary>
        /// 計画規格コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画規格コード</returns>
        string GetPlanStandardCode(int recordIndex);

        /// <summary>
        /// 計画サイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画サイズ</returns>
        string GetPlanSize(int recordIndex);

        /// <summary>
        /// 計画厚の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画厚</returns>
        string GetPlanAtu(int recordIndex);

        /// <summary>
        /// 計画巾の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画巾</returns>
        string GetPlanHaba(int recordIndex);

        /// <summary>
        /// 計画長の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画長</returns>
        string GetPlanNaga(int recordIndex);

        /// <summary>
        /// 計画耳の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画耳</returns>
        string GetPlanTM(int recordIndex);

        /// <summary>
        /// 計画製品コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画製品コード</returns>
        string GetPlanProductCode(int recordIndex);

        /// <summary>
        /// 計画枚数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画枚数</returns>
        string GetPlanMaisuu(int recordIndex);

        /// <summary>
        /// 実績規格コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>実績規格コード</returns>
        string GetPerformanceStandardCode(int recordIndex);

        /// <summary>
        /// 実績サイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>実績サイズ</returns>
        string GetPerformanceSize(int recordIndex);

        /// <summary>
        /// 実績サイズ(厚)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>実績サイズ(厚)</returns>
        string GetPerformanceAtu(int recordIndex);

        /// <summary>
        /// 実績サイズ(巾)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>実績サイズ(巾)</returns>
        string GetPerformanceHaba(int recordIndex);

        /// <summary>
        /// 実績サイズ(長)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>実績サイズ(長)</returns>
        string GetPerformanceNaga(int recordIndex);

        /// <summary>
        /// 実績耳の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>実績耳</returns>
        string GetPerformanceTM(int recordIndex);

        /// <summary>
        /// 実績製品コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>実績製品コード</returns>
        string GetPerformanceProdutCode(int recordIndex);

        /// <summary>
        /// 実績枚数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>実績枚数</returns>
        string GetPerformanceMaisuu(int recordIndex);

        /// <summary>
        /// 発生工程の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>発生工程</returns>
        string GetGeneratingStep(int recordIndex);

        /// <summary>
        /// 特殊の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>特殊</returns>
        string GetSpecial(int recordIndex);

        /// <summary>
        /// 抽出日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出日</returns>
        string GetExtractionYMD(int recordIndex);

        #endregion

    }

    /// <summary>
    /// PMPF090Bファイルのリーダ用のインターフェース
    /// </summary>
    public interface IPMPF090BReader : IFileReader
    {
        #region メソッド

        /// <summary>
        /// 未採取区分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>未採取区分</returns>
        string GetNotCollectClassfication(int recordIndex);

        /// <summary>
        /// 受注№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№</returns>
        string GetOrderNo(int recordIndex);

        /// <summary>
        /// 受注№1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№1</returns>
        string GetOrderNo1(int recordIndex);

        /// <summary>
        /// 受注№2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№2</returns>
        string GetOrderNo2(int recordIndex);

        /// <summary>
        /// 受注№3の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№3</returns>
        string GetOrderNo3(int recordIndex);

        /// <summary>
        /// 需要家名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家名</returns>
        string GetCustomerName(int recordIndex);

        /// <summary>
        /// 未計画の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>未計画</returns>
        string GetUnPlan(int recordIndex);

        /// <summary>
        /// 納期の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期</returns>
        string GetDeliveryDate(int recordIndex);

        /// <summary>
        /// 納期ランクの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期ランク</returns>
        string GetDeliveryDateRank(int recordIndex);

        /// <summary>
        /// 規格名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格名</returns>
        string GetStandardName(int recordIndex);

        /// <summary>
        /// 製品サイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品サイズ</returns>
        string GetProductSize(int recordIndex);

        /// <summary>
        /// 製品(厚)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(厚)</returns>
        string GetProductAtu(int recordIndex);

        /// <summary>
        /// 製品(巾)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(巾)</returns>
        string GetProductHaba(int recordIndex);

        /// <summary>
        /// 製品(長)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(長)</returns>
        string GetProductNaga(int recordIndex);

        /// <summary>
        /// 定耳の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>定耳</returns>
        string GetTM(int recordIndex);

        /// <summary>
        /// 製品コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品コード</returns>
        string GetProductCode(int recordIndex);

        /// <summary>
        /// 置場の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>置場</returns>
        string GetYard(int recordIndex);

        /// <summary>
        /// 置場コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>置場コード</returns>
        string GetYardCode(int recordIndex);

        /// <summary>
        /// 置場Xの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>置場X</returns>
        string GetYardX(int recordIndex);

        /// <summary>
        /// 置場Yの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>置場Y</returns>
        string GetYardY(int recordIndex);

        /// <summary>
        /// 枚数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>枚数</returns>
        string GetMaisuu(int recordIndex);

        /// <summary>
        /// 紐付枚数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>紐付枚数</returns>
        string GetTieMaisuu(int recordIndex);

        /// <summary>
        /// 予約区分の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>予約区分</returns>
        string GetReservationClassfication(int recordIndex);

        /// <summary>
        /// 発生品区分1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>発生品区分1</returns>
        string GetGeneratingProductClassfication1(int recordIndex);

        /// <summary>
        /// 板№1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>板№1</returns>
        string GetItaNo1(int recordIndex);

        /// <summary>
        /// 試験合否1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>試験合否1</returns>
        string GetTestAcceptance1(int recordIndex);

        /// <summary>
        /// 積み上げ1の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>積み上げ1</returns>
        string GetPileUp1(int recordIndex);

        /// <summary>
        /// 発生品区分2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>発生品区分2</returns>
        string GetGeneratingProductClassfication2(int recordIndex);

        /// <summary>
        /// 板№2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>板№2</returns>
        string GetItaNo2(int recordIndex);

        /// <summary>
        /// 試験合否2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>試験合否2</returns>
        string GetTestAcceptance2(int recordIndex);

        /// <summary>
        /// 積み上げ2の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>積み上げ2</returns>
        string GetPileUp2(int recordIndex);

        /// <summary>
        /// 抽出日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出日</returns>
        string GetExtractionYMD(int recordIndex);

        #endregion

    }

    /// <summary>
    /// PQGA186Bファイルのリーダ用のインターフェース
    /// </summary>
    public interface IPQGA186BReader : IFileReader
    {
        #region メソッド

        /// <summary>
        /// 発行№+日付の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>発行№+日付</returns>
        string GetIssuNoAndDate(int recordIndex);

        /// <summary>
        /// チャージ№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>チャージ№</returns>
        string GetCHNo(int recordIndex);

        /// <summary>
        /// 規格名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格名</returns>
        string GetStandardName(int recordIndex);

        /// <summary>
        /// スラブサイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ</returns>
        string GetSlabSize(int recordIndex);

        /// <summary>
        /// スラブサイズ(厚)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ(厚)</returns>
        string GetSlabAtu(int recordIndex);

        /// <summary>
        /// スラブサイズ(巾)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ(巾)</returns>
        string GetSlabHaba(int recordIndex);

        /// <summary>
        /// スラブサイズ(長)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ(長)</returns>
        string GetSlabNaga(int recordIndex);

        /// <summary>
        /// 単重の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>単重</returns>
        string GetUnitWeight(int recordIndex);

        /// <summary>
        /// 受注№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№</returns>
        string GetOrderNo(int recordIndex);

        /// <summary>
        /// 計画№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>計画№</returns>
        string GetPlanNo(int recordIndex);

        /// <summary>
        /// 抽出日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出日</returns>
        string GetExtractionYMD(int recordIndex);

        #endregion

    }

    /// <summary>
    /// PQGA380Bファイルのリーダ用のインターフェース
    /// </summary>
    public interface IPQGA380BReader : IFileReader
    {
        #region メソッド

        /// <summary>
        /// 受注№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№</returns>
        string GetOrderNo(int recordIndex);

        /// <summary>
        /// 需要家コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家コード</returns>
        string GetCustomerCode(int recordIndex);

        /// <summary>
        /// 需要家名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家名</returns>
        string GetCustomerName(int recordIndex);

        /// <summary>
        /// 商社コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>商社コード</returns>
        string GetDisutributorCode(int recordIndex);

        /// <summary>
        /// 都市の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>都市</returns>
        string GetCity(int recordIndex);

        /// <summary>
        /// 回収の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>回収</returns>
        string GetCollectiion(int recordIndex);

        /// <summary>
        /// 品種の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>品種</returns>
        string GetKind(int recordIndex);

        /// <summary>
        /// 規格コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格コード</returns>
        string GetStandardCode(int recordIndex);

        /// <summary>
        /// 決済条件の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>決済条件</returns>
        string GetSettlementCondition(int recordIndex);

        /// <summary>
        /// 商社名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>商社名</returns>
        string GetDisutributorName(int recordIndex);

        /// <summary>
        /// 規格名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格名</returns>
        string GetStandardName(int recordIndex);

        /// <summary>
        /// 製品サイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品サイズ</returns>
        string GetProductSize(int recordIndex);

        /// <summary>
        /// 製品(厚)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(厚)</returns>
        string GetProductAtu(int recordIndex);

        /// <summary>
        /// 製品(巾)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(巾)</returns>
        string GetProductHaba(int recordIndex);

        /// <summary>
        /// 製品(長)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(長)</returns>
        string GetProductNaga(int recordIndex);

        /// <summary>
        /// 出荷本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>出荷本数</returns>
        string GetShipmentHonsuu(int recordIndex);

        /// <summary>
        /// 出荷重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>出荷重量</returns>
        string GetShipmentWeight(int recordIndex);

        /// <summary>
        /// 単価の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>単価</returns>
        string GetUnitPrice(int recordIndex);

        /// <summary>
        /// 受渡条件の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受渡条件</returns>
        string GetTransferCondition(int recordIndex);

        /// <summary>
        /// チャージ№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>チャージ№</returns>
        string GetCHNo(int recordIndex);

        /// <summary>
        /// １次№①の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№①</returns>
        string GetSlab1No1(int recordIndex);

        /// <summary>
        /// １次№②の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№②</returns>
        string GetSlab1No2(int recordIndex);

        /// <summary>
        /// １次№③の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№③</returns>
        string GetSlab1No3(int recordIndex);

        /// <summary>
        /// １次№④の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№④</returns>
        string GetSlab1No4(int recordIndex);

        /// <summary>
        /// １次№⑤の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№⑤</returns>
        string GetSlab1No5(int recordIndex);

        /// <summary>
        /// １次№⑥の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№⑥</returns>
        string GetSlab1No6(int recordIndex);

        /// <summary>
        /// １次№⑦の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№⑦</returns>
        string GetSlab1No7(int recordIndex);

        /// <summary>
        /// １次№⑧の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№⑧</returns>
        string GetSlab1No8(int recordIndex);

        /// <summary>
        /// １次№⑨の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№⑨</returns>
        string GetSlab1No9(int recordIndex);

        /// <summary>
        /// １次№⑩の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>１次№⑩</returns>
        string GetSlab1No10(int recordIndex);

        /// <summary>
        /// チャージスラブ本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>チャージスラブ本数</returns>
        string GetSlabHonsuu(int recordIndex);

        /// <summary>
        /// チャージスラブ重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>チャージスラブ重量</returns>
        string GetSlabWeight(int recordIndex);

        /// <summary>
        /// 抽出日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出日</returns>
        string GetExtractionYMD(int recordIndex);

        #endregion

    }

    /// <summary>
    /// PQGA420Bファイルのリーダ用のインターフェース
    /// </summary>
    public interface IPQGA420BReader : IFileReader
    {
        #region メソッド

        /// <summary>
        /// 受注№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№</returns>
        string GetOrderNo(int recordIndex);

        /// <summary>
        /// 需要家コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家コード</returns>
        string GetCustomerCode(int recordIndex);

        /// <summary>
        /// 需要家名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家名</returns>
        string GetCustomerName(int recordIndex);

        /// <summary>
        /// 商社コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>商社コード</returns>
        string GetDisutributorCode(int recordIndex);

        /// <summary>
        /// 都市の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>都市</returns>
        string GetCity(int recordIndex);

        /// <summary>
        /// 回収の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>回収</returns>
        string GetCollection(int recordIndex);

        /// <summary>
        /// 品種の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>品種</returns>
        string GetKind(int recordIndex);

        /// <summary>
        /// 規格コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格コード</returns>
        string GetStandardCode(int recordIndex);

        /// <summary>
        /// 決済条件の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>決済条件</returns>
        string GetSettlementCondition(int recordIndex);

        /// <summary>
        /// 商社名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>商社名</returns>
        string GetDisutributorName(int recordIndex);

        /// <summary>
        /// 規格名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格名</returns>
        string GetStandardName(int recordIndex);

        /// <summary>
        /// スラブサイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ</returns>
        string GetSlabSize(int recordIndex);

        /// <summary>
        /// スラブサイズ(厚)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ(厚)</returns>
        string GetSlabAtu(int recordIndex);

        /// <summary>
        /// スラブサイズ(巾)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ(巾)</returns>
        string GetSlabHaba(int recordIndex);

        /// <summary>
        /// スラブサイズ(長)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>スラブサイズ(長)</returns>
        string GetSlabNaga(int recordIndex);

        /// <summary>
        /// 本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>本数</returns>
        string GetHonsuu(int recordIndex);

        /// <summary>
        /// 重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>重量</returns>
        string GetWeight(int recordIndex);

        /// <summary>
        /// 納期の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期</returns>
        string GetDeliveryDate(int recordIndex);

        /// <summary>
        /// ランクの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ランク</returns>
        string GetDeliveryDateRank(int recordIndex);

        /// <summary>
        /// 受渡し条件の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受渡し条件</returns>
        string GetTransferCondition(int recordIndex);

        /// <summary>
        /// 抽出日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出日</returns>
        string GetExtractionYMD(int recordIndex);

        #endregion

    }

    /// <summary>
    /// PQGA820Bファイルのリーダ用のインターフェース
    /// </summary>
    public interface IPQGA820BReader : IFileReader
    {
        #region メソッド

        /// <summary>
        /// 出荷日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>出荷日</returns>
        string GetShipmentYYMMDD(int recordIndex);

        /// <summary>
        /// ZK081本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ZK081本数</returns>
        string GetZK081Honsuu(int recordIndex);

        /// <summary>
        /// ZK081重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ZK081重量</returns>
        string GetZK081Weight(int recordIndex);

        /// <summary>
        /// ZK085本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ZK085本数</returns>
        string GetZK085Honsuu(int recordIndex);

        /// <summary>
        /// ZK085重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ZK085重量</returns>
        string GetZK085Weight(int recordIndex);

        /// <summary>
        /// ZK121本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ZK121本数</returns>
        string GetZK121Honsuu(int recordIndex);

        /// <summary>
        /// ZK121重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ZK121重量</returns>
        string GetZK121Weight(int recordIndex);

        /// <summary>
        /// ZK172本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ZK172本数</returns>
        string GetZK172Honsuu(int recordIndex);

        /// <summary>
        /// ZK172重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ZK172重量</returns>
        string GetZK172Weight(int recordIndex);

        /// <summary>
        /// ＳＰＨＣ本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ＳＰＨＣ本数</returns>
        string GetSPHCHonsuu(int recordIndex);

        /// <summary>
        /// ＳＰＨＣ重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ＳＰＨＣ重量</returns>
        string GetSPHCWeight(int recordIndex);

        /// <summary>
        /// ＳＳ400本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ＳＳ400本数</returns>
        string GetSS400Honsuu(int recordIndex);

        /// <summary>
        /// ＳＳ400重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ＳＳ400重量</returns>
        string GetSS400Weight(int recordIndex);

        /// <summary>
        /// その他本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>その他本数</returns>
        string GetOtherHonsuu(int recordIndex);

        /// <summary>
        /// その他重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>その他重量</returns>
        string GetOtherWeight(int recordIndex);

        /// <summary>
        /// 合計本数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>合計本数</returns>
        string GetTotalHonsuu(int recordIndex);

        /// <summary>
        /// 合計重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>合計重量</returns>
        string GetTotalWeight(int recordIndex);

        #endregion

    }

    /// <summary>
    /// SSYM040Bファイルのリーダ用のインターフェース
    /// </summary>
    public interface ISSYM040BReader : IFileReader
    {
        #region メソッド

        /// <summary>
        /// 年月日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>年月日</returns>
        string GetYYYYMMDD(int recordIndex);

        /// <summary>
        /// 中板屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>中板屑</returns>
        string GetMiddlePlateWaste(int recordIndex);

        /// <summary>
        /// 日清リターン屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>日清リターン屑</returns>
        string GetNissinReturnWaste(int recordIndex);

        /// <summary>
        /// トリーマ屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>トリーマ屑</returns>
        string GetTrimmerWaste(int recordIndex);

        /// <summary>
        /// 厚板ライン屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>厚板ライン屑</returns>
        string GetAtuItaLineWaste(int recordIndex);

        /// <summary>
        /// プレーナ屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>プレーナ屑</returns>
        string GetPlanerWaste(int recordIndex);

        /// <summary>
        /// レーザー屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>レーザー屑</returns>
        string GetLaserWaste(int recordIndex);

        /// <summary>
        /// プレーナ知多屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>プレーナ知多屑</returns>
        string GetPlanerChitaWaste(int recordIndex);

        /// <summary>
        /// ミスロール屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>ミスロール屑</returns>
        string GetMissRollWaste(int recordIndex);

        /// <summary>
        /// コラム返品屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>コラム返品屑</returns>
        string GetColumuReturnWaste(int recordIndex);

        /// <summary>
        /// 合計の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>合計</returns>
        string GetTotal(int recordIndex);

        #endregion

    }

    /// <summary>
    /// SSYM050Bファイルのリーダ用のインターフェース
    /// </summary>
    public interface ISSYM050BReader : IFileReader
    {
        #region メソッド

        /// <summary>
        /// 年月日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>年月日</returns>
        string GetYYYYMMDD(int recordIndex);

        /// <summary>
        /// エンドシャー屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>エンドシャー屑</returns>
        string GetEndShearWaste(int recordIndex);

        /// <summary>
        /// プレーナ屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>プレーナ屑</returns>
        string GetPlanerWaste(int recordIndex);

        /// <summary>
        /// トリーマ屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>トリーマ屑</returns>
        string GetTrimmerWaste(int recordIndex);

        /// <summary>
        /// レーザー屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>レーザー屑</returns>
        string GetLaserWaste(int recordIndex);

        /// <summary>
        /// その他屑の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>その他屑</returns>
        string GetOtherWaste(int recordIndex);

        /// <summary>
        /// 合計の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>合計</returns>
        string GetTotal(int recordIndex);

        #endregion

    }

    /// <summary>
    /// SSZA040Bファイルのリーダ用のインターフェース
    /// </summary>
    public interface ISSZA040BReader : IFileReader
    {
        #region メソッド

        /// <summary>
        /// 受注№の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注№</returns>
        string GetOrderNo(int recordIndex);

        /// <summary>
        /// 規格名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>規格名</returns>
        string GetStandardName(int recordIndex);

        /// <summary>
        /// 製品サイズの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品サイズ</returns>
        string GetProductSize(int recordIndex);

        /// <summary>
        /// 製品(厚)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(厚)</returns>
        string GetProductAtu(int recordIndex);

        /// <summary>
        /// 製品(巾)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(巾)</returns>
        string GetProductHaba(int recordIndex);

        /// <summary>
        /// 製品(長)の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品(長)</returns>
        string GetProductNaga(int recordIndex);

        /// <summary>
        /// 定耳の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>定耳</returns>
        string GetTM(int recordIndex);

        /// <summary>
        /// 製品コードの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>製品コード</returns>
        string GetProductCode(int recordIndex);

        /// <summary>
        /// 需要家名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>需要家名</returns>
        string GetCustomerName(int recordIndex);

        /// <summary>
        /// 受注枚数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注枚数</returns>
        string GetOrderCount(int recordIndex);

        /// <summary>
        /// 受注重量の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受注重量</returns>
        string GetOrderWeight(int recordIndex);

        /// <summary>
        /// 出荷枚数の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>出荷枚数</returns>
        string GetShipmentCount(int recordIndex);

        /// <summary>
        /// 受渡場所名の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受渡場所名</returns>
        string GetTransferPlaceName(int recordIndex);

        /// <summary>
        /// 受渡条件の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>受渡条件</returns>
        string GetTransferCondition(int recordIndex);

        /// <summary>
        /// 納期の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期</returns>
        string GetDeliveryDate(int recordIndex);

        /// <summary>
        /// 納期ランクの取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>納期ランク</returns>
        string GetDeliveryDateRank(int recordIndex);

        /// <summary>
        /// 引当の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>引当</returns>
        string GetProvisionClassfication(int recordIndex);

        /// <summary>
        /// 抽出日の取得
        /// </summary>
        /// <param name="recordIndex">レコードインデックス</param>
        /// <returns>抽出日</returns>
        string GetExtractionYMD(int recordIndex);

        #endregion

    }
}