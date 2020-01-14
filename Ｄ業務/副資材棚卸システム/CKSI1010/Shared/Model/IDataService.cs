using CKSI1010.Operation.InputInventoryActual.ViewModel;
using CKSI1010.Shared.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CKSI1010.Common.Constants;
//*************************************************************************************
//
//   データーサービス
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
    /// データサービス
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// 業者一覧の取得
        /// </summary>
        /// <returns>業者一覧</returns>
        Task<IEnumerable<Supplier>> GetSuppliersAsync();

        /// <summary>
        /// 棚卸一覧の取得
        /// </summary>
        /// <param name="yearMonth">年月</param>
        /// <returns>棚卸一覧</returns>
        Task<IEnumerable<Inventory>> GetInventoriesAsync(string yearMonth);

        /// <summary>
        /// 作業誌データ(入庫、返品)の取得
        /// </summary>
        /// <param name="yearMonth">年月</param>
        /// <param name="division">区分</param>
        /// <returns>作業誌データ</returns>
        Task<IEnumerable<WorkNoteItem>> GetWorkNotesAsync(string yearMonth, string division);

        /// <summary>
        /// 作業誌データ(出庫)の取得
        /// </summary>
        /// <param name="yearMonth">年月</param>
        /// <param name="divisions">区分</param>
        /// <returns>作業誌データ</returns>
        Task<IEnumerable<OutWorkNoteItem>> GetOutWorkNotesAsync(string yearMonth, params string[] divisions);

        /// <summary>
        /// 作業誌データ(直送)の取得
        /// </summary>
        /// <param name="yearMonth">年月</param>
        /// <param name="divisions">区分</param>
        /// <returns>作業誌データ</returns>
        Task<IEnumerable<OutWorkNoteItem>> GetDirectOutWorkNotesAsync(string yearMonth, params string[] divisions);

        /// <summary>
        /// 作業誌データ（液酸入出庫）の取得
        /// </summary>
        /// <param name="yearMonth">年月</param>
        /// <param name="divisions">区分</param>
        /// <returns>作業誌データ</returns>
        Task<IEnumerable<MeterDataWorkNoteItem>> GetMeterDataWorkNotesAsync(string yearMonth, params string[] divisions);

        /// <summary>
        /// 品目一覧の取得
        /// </summary>
        /// <param name="type">種別</param>
        /// <returns>品目一覧</returns>
        Task<IEnumerable<Material>> GetMaterialsAsync(string type = null);

        /// <summary>
        /// 期末提出用データの取得
        /// </summary>
        /// <returns>期末提出用データ</returns>
        Task<IEnumerable<SubmitItem>> GetSubmitItemsAsync();

        /// <summary>
        /// 印刷用データ取得
        /// </summary>
        /// <param name="type">取得対象</param>
        /// <returns>印刷用データ</returns>
        Task<IEnumerable<InventoryPrint>> GetPrintModelAsync(int type);

        /// <summary>
        /// データ削除(印刷)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<int> DeletePrintData(IList<InventoryPrint> data, int type);

        /// <summary>
        /// データ削除(棚卸実績値)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<int> DeletetInventoriesWorkData(IList<InputInventoryActualRecordViewModel> data);

        /// <summary>
        /// データ更新(印刷)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<int> MergePrintData(IList<InventoryPrint> data, int type);

        Task<string> CodeToName(string type, string code);
        
        /// <summary>
        /// データ更新(印刷)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<int> UpdateWrk(IList<InventoryPrint> data, int type);

        /// <summary>
        /// データ更新(印刷)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<int> UpdateSizaiTanaorosiWork(IList<InventoryPrint> data, int type);

        /// <summary>
        /// データ更新（作業誌データ液酸入出庫を資材棚卸ワークへ更新）
        /// </summary>
        /// <param name="data"></param>
        /// <param name="work_kbn"></param>
        /// <returns></returns>
        Task<int> UpdateSizaiTanaorosiWorkInOut(IList<MeterDataWorkNoteItem> data, string work_kbn);

        /// <summary>
        /// 棚卸マスタデータ取得
        /// </summary>
        /// <param name="shizaiKbn">資材区分</param>
        /// <param name="workKbn">作業区分</param>
        /// <param name="hinmokuCd">品目CD</param>
        /// <param name="gyosyaCd">業者CD</param>
        /// <returns>棚卸マスタデータ</returns>
        Task<IEnumerable<InventoryMaster>> GetInventoryMasterAsync(int shizaiKbn, int workKbn, string hinmokuCd, string gyosyaCd);

        /// 棚卸ワーク一覧の取得
        /// </summary>
        /// <returns>棚卸ワーク一覧</returns>
        Task<IEnumerable<InventoryWork>> GetInventoriesWorkAsync(string workKbn);

        /// <summary>
        /// 棚卸ワーク更新
        /// </summary>
        /// <param name="workList">棚卸ワーク一覧</param>
        /// <returns>結果</returns>
        Task<int> UpdateInventoriesWorkAsync(IList<InventoryWork> list);

        /// <summary>
        /// 単価入力チェック
        /// </summary>
        /// <returns>単価未入力の品目CD一覧</returns>
        Task<IEnumerable<Tuple<string, string>>> CheckNotEnteredTanka();

        /// <summary>
        /// 立会い用棚卸表印刷データ取得
        /// </summary>
        /// <param name="date">取得したい日(yyyyMM)</param>
        /// <returns>印刷用データ</returns>
        Task<IEnumerable<InventoryWitness>> GetPrintWitnessModelAsync(string date);

        /// <summary>
        /// 立会い用棚卸表印刷データ更新
        /// </summary>
        /// <param name="datalist">データ一覧</param>
        /// <returns>印刷用データ</returns>
        Task<int> SetPrintWitnessModelAsync(IList<InventoryWitness> datalist);

        /// <summary>
        /// 財務提出用棚卸表印刷データ取得
        /// </summary>
        /// <returns>印刷用データ</returns>
        Task<IEnumerable<InventoryFinancingPresetation>> GetPrintFinancingPresetationModelAsync();

        /// <summary>
        /// 計算書(使用高払い)
        /// </summary>
        /// <returns>計算書(使用高払い)</returns>
        Task<IEnumerable<Ukebarai>> GetSiyoudakaData();

        /// <summary>
        /// 計算書(入庫払い)
        /// </summary>
        /// <returns>計算書(入庫払い)</returns>
        Task<IEnumerable<Ukebarai>> GetNyukoData();

        /// <summary>
        /// 資材棚卸トランデータ削除
        /// </summary>
        /// <param name="tanaYm">棚卸年月(yyyyMM)</param>
        /// <returns>結果</returns>
        Task<int> DeleteInventoriesTranAsync(string tanaYm);

        /// <summary>
        /// 資材棚卸トランデータ削除
        /// </summary>
        /// <param name="tanaYm">棚卸年月(yyyyMM)</param>
        /// <returns>結果</returns>
        Task<int> DeleteSizaiTanaorosiTranAsync(string tanaYm, string workKbn);

        /// <summary>
        /// 資材棚卸トランデータ登録
        /// </summary>
        /// <param name="tanaYm">棚卸年月(yyyyMM)</param>
        /// <returns>結果</returns>
        Task<int> EntryInventoriesTranAsync(string tanaYm);

        /// <summary>
        /// 資材棚卸トランデータ登録
        /// </summary>
        /// <param name="tanaYm">棚卸年月(yyyyMM)</param>
        /// <param name="workKbn">作業区分</param>
        /// <returns>結果</returns>
        Task<int> EntryInventoriesTranAsync(string tanaYm, string workKbn);

        /// 棚卸期間の取得
        /// </summary>
        /// <param name="category">システム分類(1：資材 2：部品 (スペース)：未設定)</param>
        /// <returns>棚卸一覧</returns>
        Task<IEnumerable<InventoryTerm>> GetInventoryPeriodsAsync(string category);

        /// <summary>
        /// 操作一覧の取得
        /// </summary>
        /// <param name="systemCategory">システムカテゴリー</param>
        /// <param name="syainCd">社員コード</param>
        /// <param name="workKbn">作業区分(1:棚卸、2:検針)</param>
        /// <param name="isTermEnd">期末か</param>
        /// <returns>操作者情報</returns>
        Task<IEnumerable<OperationInfo>> GetOperatorAsync(string systemCategory, string syainCd, string workKbn, bool isTermEnd);

        /// <summary>
        /// 棚卸ワーク一覧取得
        /// </summary>
        /// <param name="himoku">費目</param>
        /// <param name="utiwake">内訳</param>
        /// <param name="tanaban">棚番</param>
        /// <param name="hinmokuCd">品目CD</param>
        /// <returns>棚卸ワーク一覧</returns>
        Task<IEnumerable<InventoryWork>> GetInventoriesWorkAsync(string himoku, string utiwake, string tanaban, string hinmokuCd = "");

        /// <summary>
        /// 資材棚卸操作グループ区分の取得
        /// </summary>
        /// <param name="systemCategory">システムカテゴリー</param>
        /// <param name="syainCd">社員CD</param>
        /// <returns>資材棚卸操作グループ区分</returns>
        Task<string> GetSizaiTanaorosiGroupKbn(string systemCategory, string syainCd);

        /// <summary>
        /// 棚卸進捗状況マスタの更新
        /// </summary>
        /// <param name="workKbn">作業区分</param>
        /// <param name="operationNo">操作No</param>
        /// <param name="operationCondition">操作状況</param>
        /// <returns>結果</returns>
        Task<int> UpdateTanaorosiProgressMst(string workKbn,int operationNo, string operationCondition);

        /// <summary>
        /// 棚卸進捗状況マスタの更新
        /// </summary>
        /// <param name="workKbn">作業区分</param>
        /// <param name="operationNo">操作No</param>
        /// <returns>結果</returns>
        Task<int> UpdateTanaorosiProgressMst(string workKbn, int operationNo);

        /// <summary>
        /// 棚卸進捗状況の取得
        /// </summary>
        /// <param name="workKbn">作業区分</param>
        /// <returns>棚卸進捗状況</returns>
        Task<IEnumerable<TanaorosiProgress>> GetTanaorosiProgress(string workKbn);

        /// <summary>
        /// 指定された内訳の向先が正しいかをチェックする
        /// </summary>
        /// <param name="himoku">費目</param>
        /// <param name="uchiwake">内訳</param>
        /// <param name="tanaban">棚番</param>
        /// <returns>正しい場合:true/正しくない場合:false</returns>
        Task<bool> CheckMukesaki(string himoku, string uchiwake, string tanaban);

        Task<IEnumerable<InventoryWork>> GetInventoriesWorkFromMsterAsync(string workKbn);

        Task<IEnumerable<InventoryWork>> GetInventoriesKensinWorkAsync(string workKbn);

        Task<int> UpdateSizaiKensinData(IList<InventoryWork> list);

        Task<int> UpdateSizaiTanaorosiMst(IList<InventoryWork> list);

        Task<int> UpdateSizaiTanaorosiWork(IList<InventoryWork> list);

        /// <summary>
        /// 品目マスタ存在チェック
        /// </summary>
        /// <returns>品目CD一覧</returns>
        Task<IEnumerable<Tuple<string, string>>> CheckHinmokuMaster();

        /// <summary>
        /// 棚卸ログの挿入
        /// </summary>
        /// <param name="tanaorosiLog">棚卸ログ</param>
        /// <returns>結果</returns>
        Task<int> InsertTanaorosiLog(TanaorosiLog tanaorosiLog);

        /// <summary>
        /// 棚卸詳細ログの挿入
        /// </summary>
        /// <param name="tanaorosiDetailLog">棚卸詳細ログ</param>
        /// <returns>結果</returns>
        Task<int> InsertTanaorosiDetailLog(TanaorosiDetailLog tanaorosiDetailLog);

        /// <summary>
        /// 棚卸実施年月における資材品目(マスタ)の変更データの取得
        /// </summary>
        /// <param name="tanaYm">棚卸実施年月</param>
        /// <returns>資材品目(マスタ)の変更データ</returns>
        Task<IEnumerable<SizaiHinmokuItem>> GetSizaiHinmokuChangeData(string tanaYm);

        /// <summary>
        /// 受払い種別の取得
        /// </summary>
        /// <param name="hinmokuCode">品目コード</param>
        /// <returns>受払い種別</returns>
        Task<UkebaraiType> GetUkebaraiSyubetuAsync(string hinmokuCode);

        /// <summary>
        /// 受払い種別の取得
        /// </summary>
        /// <param name="hinmokuCode">品目コード</param>
        /// <returns>受払い種別</returns>
        UkebaraiType GetUkebaraiSyubetu(string hinmokuCode);

    }
}
