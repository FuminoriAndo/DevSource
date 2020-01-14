using CKSI1010.Operation.InputInventoryActual.ViewModel;
using CKSI1010.Shared.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CKSI1010.Common.Constants;
//*************************************************************************************
//
//   デザインモードのデーターサービスの実装
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
    /// デザインモードのデータサービス
    /// </summary>
    internal class DesignDataService : IDataService
    {
        #region メソッド

        /// <summary>
        /// 業者一覧の取得
        /// </summary>
        /// <returns>業者一覧</returns>
        public Task<IEnumerable<Supplier>> GetSuppliersAsync()
        {
            return null;
        }

        /// <summary>
        /// 棚卸一覧の取得
        /// </summary>
        /// <param name="yearMonth">年月</param>
        /// <returns>棚卸一覧</returns>
        public Task<IEnumerable<Inventory>> GetInventoriesAsync(string yearMonth)
        {
            return null;
        }

        /// <summary>
        /// 作業誌データ(入庫、返品)の取得
        /// </summary>
        /// <param name="yearMonth">年月</param>
        /// <param name="division">区分</param>
        /// <returns>作業誌データ</returns>
        public Task<IEnumerable<WorkNoteItem>> GetWorkNotesAsync(string yearMonth, string division)
        {
            return null;

        }

        /// <summary>
        /// 作業誌データ(出庫)の取得
        /// </summary>
        /// <param name="yearMonth">年月</param>
        /// <param name="divisions">区分</param>
        /// <returns>作業誌データ</returns>
        public Task<IEnumerable<OutWorkNoteItem>> GetOutWorkNotesAsync(string yearMonth, params string[] divisions)
        {
            return null;
        }

        /// <summary>
        /// 作業誌データ(直送)の取得
        /// </summary>
        /// <param name="yearMonth">年月</param>
        /// <param name="divisions">区分</param>
        /// <returns>作業誌データ</returns>
        public Task<IEnumerable<OutWorkNoteItem>> GetDirectOutWorkNotesAsync(string yearMonth, params string[] divisions)
        {
            return null;
        }

        /// <summary>
        /// 作業誌データ（液酸入庫）の取得
        /// </summary>
        /// <param name="yearMonth">年月</param>
        /// <param name="division">区分</param>
        /// <returns>作業誌データ</returns>
        public Task<IEnumerable<MeterDataWorkNoteItem>> GetMeterDataWorkNotesAsync(string yearMonth, params string[] divisions)
        {
            return null;
        }

        /// <summary>
        /// 品目一覧の取得
        /// </summary>
        /// <param name="type">種別</param>
        /// <returns>品目一覧</returns>
        public Task<IEnumerable<Material>> GetMaterialsAsync(string type = null)
        {
            return null;
        }

        /// <summary>
        /// 期末提出用データの取得
        /// </summary>
        /// <returns>期末提出用データ</returns>
        public Task<IEnumerable<SubmitItem>> GetSubmitItemsAsync()
        {
            return null;
        }

        /// <summary>
        /// 印刷用データ取得
        /// </summary>
        /// <param name="type">取得対象</param>
        /// <returns>印刷用データ</returns>
        public Task<IEnumerable<InventoryPrint>> GetPrintModelAsync(int type)
        {
            return null;
        }

        /// <summary>
        /// データ削除(印刷)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task<int> DeletePrintData(IList<InventoryPrint> data, int type)
        {
            return null;
        }

        /// <summary>
        /// データ削除(棚卸実績値)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task<int> DeletetInventoriesWorkData(IList<InputInventoryActualRecordViewModel> data)
        {
            return null;
        }

        /// <summary>
        /// データ更新(印刷)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task<int> MergePrintData(IList<InventoryPrint> data, int type)
        {
            return null;
        }

        /// <summary>
        /// データ更新(印刷)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task<int> UpdateWrk(IList<InventoryPrint> data, int type)
        {
            return null;
        }

        /// <summary>
        /// データ更新(印刷)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task<int> UpdateSizaiTanaorosiWork(IList<InventoryPrint> data, int type)
        {
            return null;

        }

        /// <summary>
        /// データ更新（作業誌データ液酸入出庫を資材棚卸トランへ更新）
        /// </summary>
        /// <param name="data"></param>
        /// <param name="work_kbn"></param>
        /// <returns></returns>
        public Task<int> UpdateSizaiTanaorosiWorkInOut(IList<MeterDataWorkNoteItem> data, string work_kbn = "2")
        {
            return null;
        }

        public Task<string> CodeToName(string type, string code)
        {
            return null;
        }

        /// <summary>
        /// 棚卸マスタデータ取得
        /// </summary>
        /// <param name="shizaiKbn">資材区分</param>
        /// <param name="workKbn">作業区分</param>
        /// <param name="hinmokuCd">品目CD</param>
        /// <param name="gyosyaCd">業者CD</param>
        /// <returns>棚卸マスタデータ</returns>
        public Task<IEnumerable<InventoryMaster>> GetInventoryMasterAsync(int shizaiKbn, int workKbn, string hinmokuCd, string gyosyaCd)
        {
            return null;
        }

        /// <summary>
        /// 棚卸ワーク一覧の取得
        /// </summary>
        /// <returns>棚卸ワーク一覧</returns>
        public Task<IEnumerable<InventoryWork>> GetInventoriesWorkAsync(string workKbn)
        {
            return null;
        }

        /// <summary>
        /// 棚卸ワーク一覧の取得
        /// </summary>
        /// <param name="workKbn">作業区分</param>
        /// <returns>棚卸ワーク一覧</returns>
        public Task<IEnumerable<InventoryWork>> GetInventoriesKensinWorkAsync(string workKbn)
        {
            return null;
        }

        /// <summary>
        /// 棚卸ワーク更新の取得
        /// </summary>
        /// <param name="list">棚卸ワーク一覧</param>
        public Task<int> UpdateInventoriesWorkAsync(IList<InventoryWork> list)
        {
            return null;
        }

        /// <summary>
        /// 単価入力チェック
        /// </summary>
        /// <returns>単価未入力の品目CD一覧</returns>
        public Task<IEnumerable<System.Tuple<string, string>>> CheckNotEnteredTanka()
        {
            return null;
        }

        /// <summary>
        /// 立会い用棚卸表印刷
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public Task<IEnumerable<InventoryWitness>> GetPrintWitnessModelAsync(string date)
        {
            return null;
        }

        /// <summary>
        /// 財務提出用棚卸表印刷データ取得
        /// </summary>
        /// <returns>印刷用データ</returns>
        public Task<IEnumerable<InventoryFinancingPresetation>> GetPrintFinancingPresetationModelAsync()
        {
            return null;
        }

        /// <summary>
        /// 立会い用棚卸表印刷データ更新
        /// </summary>
        /// <param name="datalist">データ一覧</param>
        /// <returns>印刷用データ</returns>
        public Task<int> SetPrintWitnessModelAsync(IList<InventoryWitness> datalist)
        {
            return null;
        }

        /// <summary>
        /// 計算書(使用高払い)
        /// </summary>
        /// <returns>計算書(使用高払い)</returns>
        public Task<IEnumerable<Ukebarai>> GetSiyoudakaData()
        {
            return null;
        }

        /// <summary>
        /// 計算書(入庫払い)
        /// </summary>
        /// <returns>計算書(入庫払い)</returns>
        public Task<IEnumerable<Ukebarai>> GetNyukoData()
        {
            return null;
        }

        /// <summary>
        /// 資材棚卸トランデータ削除
        /// </summary>
        /// <param name="tanaYm">棚卸年月(yyyyMM)</param>
        /// <returns>結果</returns>
        public Task<int> DeleteInventoriesTranAsync(string tanaYm)
        {
            return null;
        }

        /// <summary>
        /// 資材棚卸トランデータ削除
        /// </summary>
        /// <param name="tanaYm">棚卸年月(yyyyMM)</param>
        /// <returns>結果</returns>
        public Task<int> DeleteSizaiTanaorosiTranAsync(string tanaYm, string workKbn)
        {
            return null;
        }

        /// <summary>
        /// 資材棚卸トランデータ登録
        /// </summary>
        /// <param name="tanaYm">棚卸年月(yyyyMM)</param>
        /// <returns>結果</returns>
        public Task<int> EntryInventoriesTranAsync(string tanaYm)
        {
            return null;
        }

        /// <summary>
        /// 資材棚卸トランデータ登録
        /// </summary>
        /// <param name="tanaYm">棚卸年月(yyyyMM)</param>
        /// <param name="workKbn">作業区分</param>
        /// <returns>結果</returns>
        public Task<int> EntryInventoriesTranAsync(string tanaYm, string workKbn)
        {
            return null;
        }

        /// 棚卸期間の取得
        /// </summary>
        /// <param name="category">システム分類(1：資材 2：部品 (スペース)：未設定)</param>
        /// <returns>棚卸一覧</returns>
        public Task<IEnumerable<InventoryTerm>> GetInventoryPeriodsAsync(string category)
        {
            return null;
        }

        /// <summary>
        /// 操作一覧の取得
        /// </summary>
        /// <param name="systemCategory">システムカテゴリー</param>
        /// <param name="syainCd">社員コード</param>
        /// <param name="workClass">作業区分(1:棚卸、2:検針)</param>
        /// <param name="isTermEnd">期末か</param>
        /// <returns>操作者情報</returns>
        public Task<IEnumerable<OperationInfo>> GetOperatorAsync(string systemCategory, string syainCd, string workKbn, bool isTermEnd)
        {
            return null;
        }

        /// <summary>
        /// 棚卸ワーク一覧取得
        /// </summary>
        /// <param name="himoku">費目</param>
        /// <param name="utiwake">内訳</param>
        /// <param name="tanaban">棚番</param>
        /// <param name="hinmokuCd">品目CD</param>
        /// <returns>棚卸ワーク一覧</returns>
        public Task<IEnumerable<InventoryWork>> GetInventoriesWorkAsync(string himoku, string utiwake, string tanaban, string hinmokuCd = "")
        {
            return null;
        }

        /// <summary>
        /// 資材棚卸操作グループ区分の取得
        /// </summary>
        /// <param name="systemCategory">システムカテゴリー</param>
        /// <param name="syainCd">社員CD</param>
        /// <returns>資材棚卸操作グループ区分</returns>
        public Task<string> GetSizaiTanaorosiGroupKbn(string systemCategory, string syainCd)
        {
            return null;
        }

        /// <summary>
        /// 棚卸進捗状況マスタの更新
        /// </summary>
        /// <param name="workKbn">作業区分</param>
        /// <param name="operationNo">操作No</param>
        /// <param name="operationCondition">操作状況</param>
        /// <returns>結果</returns>
        public Task<int> UpdateTanaorosiProgressMst(string workKbn, int operationNo, string operationCondition)
        {
            return null;
        }

        /// <summary>
        /// 棚卸進捗状況マスタの更新
        /// </summary>
        /// <param name="workKbn">作業区分</param>
        /// <param name="operationNo">操作No</param>
        /// <param name="operationCondition">操作状況</param>
        /// <returns>結果</returns>
        public Task<int> UpdateTanaorosiProgressMst(string workKbn, int operationNo)
        {
            return null;
        }

        /// <summary>
        /// 棚卸進捗状況の取得
        /// </summary>
        /// <param name="workKbn">作業区分</param>
        /// <returns>棚卸進捗状況</returns>
        public Task<IEnumerable<TanaorosiProgress>> GetTanaorosiProgress(string workKbn)
        {
            return null;
        }

        /// <summary>
        /// 指定された内訳の向先が正しいかをチェックする
        /// </summary>
        /// <param name="himoku">費目</param>
        /// <param name="uchiwake">内訳</param>
        /// <param name="tanaban">棚番</param>
        /// <returns>正しい場合:true/正しくない場合:false</returns>
        public Task<bool> CheckMukesaki(string himoku, string uchiwake, string tanaban)
        {
            return null;
        }

        /// <summary>
        /// 資材棚卸マスタ一覧の取得
        /// </summary>
        /// <param name="type">資材区分</param>
        /// <returns>棚卸ワーク一覧</returns>
        public Task<IEnumerable<InventoryWork>> GetInventoriesWorkFromMsterAsync(string workKbn)
        {
            return null;
        }

        public Task<int> UpdateSizaiKensinData(IList<InventoryWork> list)
        {
            return null;
        }

        public Task<int> UpdateSizaiTanaorosiMst(IList<InventoryWork> list)
        {
            return null;
        }

        public Task<int> UpdateSizaiTanaorosiWork(IList<InventoryWork> list)
        {
            return null;
        }

        /// <summary>
        /// 品目マスタ存在チェック
        /// </summary>
        /// <returns>品目CD一覧</returns>
        public Task<IEnumerable<System.Tuple<string, string>>> CheckHinmokuMaster()
        {
            return null;
        }

        /// <summary>
        /// 棚卸ログの挿入
        /// </summary>
        /// <param name="tanaorosiLog">棚卸ログ</param>
        /// <returns>結果</returns>
        public Task<int> InsertTanaorosiLog(TanaorosiLog tanaorosiLog)
        {
            return null;
        }

        /// <summary>
        /// 棚卸詳細ログの挿入
        /// </summary>
        /// <param name="tanaorosiDetailLog">棚卸詳細ログ</param>
        /// <returns>結果</returns>
        public Task<int> InsertTanaorosiDetailLog(TanaorosiDetailLog tanaorosiDetailLog)
        {
            return null;
        }

        /// <summary>
        /// 棚卸実施年月における資材品目(マスタ)の変更データの取得
        /// </summary>
        /// <param name="tanaYm">棚卸実施年月</param>
        /// <returns>資材品目(マスタ)の変更データ</returns>
        public Task<IEnumerable<SizaiHinmokuItem>> GetSizaiHinmokuChangeData(string tanaYm)
        {
            return null;
        }

        /// <summary>
        /// 受払い種別の取得
        /// </summary>
        /// <param name="hinmokuCode">品目コード</param>
        /// <returns>受払い種別</returns>
        public Task<UkebaraiType> GetUkebaraiSyubetuAsync(string hinmokuCode)
        {
            return null;
        }

        /// <summary>
        /// 受払い種別の取得
        /// </summary>
        /// <param name="hinmokuCode">品目コード</param>
        /// <returns>受払い種別</returns>
        public UkebaraiType GetUkebaraiSyubetu(string hinmokuCode)
        {
            return default(UkebaraiType);
        }

        #endregion
    }
}