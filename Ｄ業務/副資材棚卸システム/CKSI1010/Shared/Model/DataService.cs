using CKSI1010.Common;
using CKSI1010.Operation.InputInventoryActual.ViewModel;
using CKSI1010.Properties;
using CKSI1010.Shared.ViewModel;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CKSI1010.Common.Constants;
//*************************************************************************************
//
//   データーサービスの実装
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
    /// データーサービスの実装
    /// </summary>
    internal class DataService : IDataService
    {
        #region フィールド

        /// <summary>
        /// DB接続
        /// </summary>
        private readonly string connectionString;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataService()
        {
            connectionString = $"User ID={Settings.Default.UserId}; Password={Settings.Default.Password}; Data Source={Settings.Default.DataSource}";
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 業者一覧の取得
        /// </summary>
        /// <returns>業者一覧</returns>
        public async Task<IEnumerable<Supplier>> GetSuppliersAsync()
        {
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    return (await conn.QueryAsync(
                        "select * from GYOSYA_MST order by GYOSYACD"))
                        .Select(result => new Supplier()
                        {
                            Code = result.GYOSYACD,
                            Name = result.GYOSYANM,
                            KanaName = result.GYOSYANMA,
                            ConditionCode = result.JYOKENCD,
                            Account = result.KOZANM,
                            KanaAccount = result.KOZANMA
                        });
                }
            }

            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 棚卸一覧の取得
        /// </summary>
        /// <param name="yearMonth">年月</param>
        /// <returns>棚卸一覧</returns>
        public async Task<IEnumerable<Inventory>> GetInventoriesAsync(string yearMonth)
        {
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    return (await conn.QueryAsync(
                        @"select HINMOKUCD,GYOSYACD,HINMOKUNM,GYOSYANM,ZSOUKO,ZEF,ZLF,ZCC,ZSONOTA,ZMETER,ZYOBI1,ZYOBI2
                      from SIZAI_TANAOROSI_TRN 
                      where TANAYM = :TANAYM", new { TANAYM = yearMonth }))
                        .Select(result => new Inventory()
                        {
                            ItemCode = result.HINMOKUCD,
                            SupplierCode = result.GYOSYACD,
                            ItemName = result.HINMOKUNM,
                            SupplierName = result.GYOSYANM,
                            StockInWarehouse = result.ZSOUKO,
                            StockEF = result.ZEF,
                            StockLF = result.ZLF,
                            StockCC = result.ZCC,
                            StockOther = result.ZSONOTA,
                            StockMeter = result.ZMETER,
                            StockReserve1 = result.ZYOBI1,
                            StockReserve2 = result.ZYOBI2
                        });
                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 作業誌データ(出庫)の取得
        /// </summary>
        /// <param name="yearMonth">年月</param>
        /// <param name="division">区分</param>
        /// <returns>作業誌データ</returns>
        public async Task<IEnumerable<WorkNoteItem>> GetWorkNotesAsync(string yearMonth, string division)
        {
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    return (await conn.QueryAsync(
                        $@"select HINMOKUCD, GYOSYACD, SUM(SURYO) as SURYO
                      from  SIZAI_SAGYOSI_TRN 
                      where SAGYOBI between '{yearMonth}01' and '{yearMonth}31' and KUBUN='{division}' 
                      group by HINMOKUCD, GYOSYACD 
                      order by HINMOKUCD, GYOSYACD"))
                          .Select(result => new WorkNoteItem()
                          {
                              ItemCode = result.HINMOKUCD,
                              GyosyaCD = result.GYOSYACD,
                              Amount = result.SURYO,
                          });
                }
            }

            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 作業誌データ(入庫、返品)の取得
        /// </summary>
        /// <param name="yearMonth">年月</param>
        /// <param name="divisions">区分</param>
        /// <returns>作業誌データ</returns>
        public async Task<IEnumerable<OutWorkNoteItem>> GetOutWorkNotesAsync(string yearMonth, params string[] divisions)
        {
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    var condition = string.Empty;
                    if (divisions.Length > 0)
                    {
                        condition = $"and KUBUN in ({string.Join(",", divisions.Select(division => "'" + division + "'"))})";
                    }

                    await conn.OpenAsync();

                    return (await conn.QueryAsync(
                        $@"select HINMOKUCD, MUKESAKI, SUM(SURYO) as SURYO
                      from  SIZAI_SAGYOSI_TRN 
                      where SAGYOBI between '{yearMonth}01' and '{yearMonth}31'
                      {condition}
                      group by HINMOKUCD, MUKESAKI 
                      order by HINMOKUCD, MUKESAKI"))
                          .Select(result => new OutWorkNoteItem()
                          {
                              ItemCode = result.HINMOKUCD,
                              Mukesaki = result.MUKESAKI,
                              Amount = result.SURYO
                          });
                }
            }

            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 作業誌データ(直送)の取得
        /// </summary>
        /// <param name="yearMonth">年月</param>
        /// <param name="divisions">区分</param>
        /// <returns>作業誌データ</returns>
        public async Task<IEnumerable<OutWorkNoteItem>> GetDirectOutWorkNotesAsync(string yearMonth, params string[] divisions)
        {
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    var condition = string.Empty;
                    if (divisions.Length > 0)
                    {
                        condition = $"and KUBUN in ({string.Join(",", divisions.Select(division => "'" + division + "'"))})";
                    }

                    await conn.OpenAsync();

                    return (await conn.QueryAsync(
                        $@"select HINMOKUCD, GYOSYACD, MUKESAKI, SUM(SURYO) as SURYO
                      from  SIZAI_SAGYOSI_TRN 
                      where SAGYOBI between '{yearMonth}01' and '{yearMonth}31'
                      {condition}
                      group by HINMOKUCD, GYOSYACD, MUKESAKI  
                      order by HINMOKUCD, GYOSYACD, MUKESAKI"))
                          .Select(result => new OutWorkNoteItem()
                          {
                              ItemCode = result.HINMOKUCD,
                              GyosyaCD = result.GYOSYACD,
                              Mukesaki = result.MUKESAKI,
                              Amount = result.SURYO,

                          });
                }
            }

            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 作業誌データの取得
        /// </summary>
        /// <param name="yearMonth">年月</param>
        /// <param name="divisions">区分</param>
        /// <returns>作業誌データ</returns>
        public async Task<IEnumerable<MeterDataWorkNoteItem>> GetMeterDataWorkNotesAsync(string yearMonth, params string[] divisions)
        {
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    var condition = string.Empty;
                    if (divisions.Length > 0)
                    {
                        condition = $"and KUBUN in ({string.Join(",", divisions.Select(division => "'" + division + "'"))})";
                    }

                    string lastMonth = (new DateTime(int.Parse(yearMonth.Substring(0, 4)), int.Parse(yearMonth.Substring(4, 2)), 1)).AddMonths(1).AddDays(-1).ToString("yyyyMMdd");

                    await conn.OpenAsync();

                    return (await conn.QueryAsync(
                        $@"select HINMOKUCD, GYOSYACD, MUKESAKI, KUBUN, SUM(SURYO) as SURYO
                      from  SIZAI_SAGYOSI_TRN 
                      where SAGYOBI = '{lastMonth}'
                      {condition}
                      group by HINMOKUCD, GYOSYACD, MUKESAKI, KUBUN
                      order by HINMOKUCD, GYOSYACD, MUKESAKI, KUBUN"))
                          .Select(result => new MeterDataWorkNoteItem()
                          {
                              HinmokuCD = result.HINMOKUCD,
                              GyosyaCD = result.GYOSYACD,
                              Mukesaki = result.MUKESAKI,
                              Kubun = result.KUBUN,
                              Amount = result.SURYO
                          });
                }
            }

            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 品目一覧の取得
        /// </summary>
        /// <param name="type">種別</param>
        /// <returns>品目一覧</returns>
        public async Task<IEnumerable<Material>> GetMaterialsAsync(string type = null)
        {
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    var condition = string.Empty;
                    if (type != null)
                    {
                        condition = $"where SYUBETU={type}";
                    }

                    await conn.OpenAsync();

                    return (await conn.QueryAsync(
                        $@"select HINMOKUCD, HINMOKUNM, HIMOKU, UTIWAKE, TANABAN, TANI, SYUBETU, ICHIKBN, MUKESAKI
                       from  SIZAI_HINMOKU_MST 
                       {condition}
                       order by HINMOKUCD"))
                           .Select(result => new Material()
                           {
                               Code = result.HINMOKUCD,
                               Name = result.HINMOKUNM,
                               Expense = result.HIMOKU,
                               Breakdown = result.UTIWAKE,
                               Shelf = result.TANABAN,
                               Unit = result.TANI,
                               Type = result.SYUBETU,
                               Moisture = result.SUIBUNKBN,
                               Acceptance = result.KENSYUKBN,
                               Report = result.HOUKOKUKBN,
                               IssuePlace = result.ICHIKBN,
                               PickupPlace = result.MUKESAKI
                           });
                }
            }

            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 期末提出用データの取得
        /// </summary>
        /// <returns>期末提出用データ</returns>
        public async Task<IEnumerable<SubmitItem>> GetSubmitItemsAsync()
        {
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    return (await conn.QueryAsync(
                        @"select a.HIMOKUCD, b.HIMOKUNM, a.UTIWAKECD, c.HIMOUTINM, a.TANACD, a.HINMOKUNMA, a.HINMOKUNM, d.TANINM	
                      from HINMOKU_MST a,SA.HIMOKU_MST b ,SA.SEIZO_HIMOKU_MST c ,SA.TANI_MST d 	
                      where c.HIMOKUCD = b.HIMOKUCD 
                      and a.TANICD = d.TANICD
                      and b.HIMOKUCD = a.HIMOKUCD
                      and c.UTIWAKECD = a.UTIWAKECD	
                      order by a.HIMOKUCD,a.UTIWAKECD,a.TANACD"))
                          .Select(result => new SubmitItem()
                          {
                              ExpenseCode = result.HIMOKUCD,
                              ExpenseName = result.HIMOKUNM,
                              BreakdownCode = result.UTIWAKECD,
                              BreakdownName = result.HIMOUTINM,
                              Shelf = result.TANACD,
                              ShelfName1 = result.HINMOKUNMA,
                              ShelfName2 = result.HINMOKUNM,
                              Unit = result.TANI
                          });
                }
            }

            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 印刷用データ取得
        /// </summary>
        /// <param name="type">取得対象</param>
        /// <returns>印刷用データ</returns>
        public async Task<IEnumerable<InventoryPrint>> GetPrintModelAsync(int type)
        {
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    return (await conn.QueryAsync(
                            $@"select a.HINMOKUCD, a.GYOSYACD, a.HINMOKUNM, a.GYOSYANM, a.BIKOU, b.SYUBETU, a.ITEM_ORDER , a.WORK_KBN , c.TOGETU
                           from SIZAI_TANAOROSI_MST a, SIZAI_HINMOKU_MST b, SIZAI_TANAOROSI_WRK c
                           where a.SIZAI_KBN = {type} and a.HINMOKUCD = b.HINMOKUCD(+) and TRIM(a.WORK_KBN) = '1' and (a.SIZAI_KBN = c.SIZAI_KBN(+) and a.WORK_KBN = c.WORK_KBN(+) and a.HINMOKUCD = c.HINMOKUCD(+) and a.GYOSYACD = c.GYOSYACD(+))
                           order by ITEM_ORDER"
                        ))
                          .Select(result => new InventoryPrint()
                          {
                              HinmokuCD = result.HINMOKUCD,
                              GyosyaCD = result.GYOSYACD,
                              HinmokuName = result.HINMOKUNM,
                              GyosyaName = result.GYOSYANM,
                              Bikou = result.BIKOU,
                              Syubetu = result.SYUBETU,
                              ItemOrder = result.ITEM_ORDER,
                              Workkbn = result.WORK_KBN.Trim(),
                              Togetsuryo = (result.TOGETU == null) ? string.Empty : getTogetu(result.TOGETU)

                          });
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// データ削除(印刷)
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <param name="type">資材区分</param>
        /// <returns>結果</returns>
        public async Task<int> DeletePrintData(IList<InventoryPrint> data, int type)
        {
            string sql = string.Empty;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    foreach (var item in data)
                    {
                        sql = $@"delete from SIZAI_TANAOROSI_MST where SIZAI_KBN = {type} and WORK_KBN = 1 and HINMOKUCD = '{item.HinmokuCD}' and GYOSYACD='{item.GyosyaCD}'";
                        OracleCommand commandDeleteSizaiTanaorosiMst = new OracleCommand(sql, conn);
                        commandDeleteSizaiTanaorosiMst.ExecuteNonQuery();

                        sql = $@"delete from SIZAI_TANAOROSI_WRK where SIZAI_KBN = {type} and WORK_KBN = 1 and HINMOKUCD = '{item.HinmokuCD}' and GYOSYACD='{item.GyosyaCD}'";
                        OracleCommand commandDeleteSizaiTanaorosiWork = new OracleCommand(sql, conn);
                        commandDeleteSizaiTanaorosiWork.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                {
                    throw;
                }
            }

            return Constants.SQL_RESULT_OK;
        }

        /// <summary>
        /// データ削除(棚卸実績値)
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <returns>結果</returns>
        public async Task<int> DeletetInventoriesWorkData(IList<InputInventoryActualRecordViewModel> data)
        {
            string sql = string.Empty;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    var trans = conn.BeginTransaction();

                    foreach (var item in data)
                    {
                        sql = $@"delete from SIZAI_TANAOROSI_MST where SIZAI_KBN = '{item.SIZAI_KBN}' and WORK_KBN = '{item.WorkKbn}' and HINMOKUCD = '{item.HINMOKUCD}' and GYOSYACD='{item.GYOSYACD}'";
                        OracleCommand commandDeleteSizaiTanaorosiMst = new OracleCommand(sql, conn);
                        commandDeleteSizaiTanaorosiMst.ExecuteNonQuery();

                        sql = $@"delete from SIZAI_TANAOROSI_WRK where SIZAI_KBN = '{item.SIZAI_KBN}' and WORK_KBN = '{item.WorkKbn}' and HINMOKUCD = '{item.HINMOKUCD}' and GYOSYACD='{item.GYOSYACD}'";
                        OracleCommand commandDeleteSizaiTanaorosiWork = new OracleCommand(sql, conn);
                        commandDeleteSizaiTanaorosiWork.ExecuteNonQuery();
                    }

                    trans.Commit();
                }
            }

            catch (Exception ex)
            {
                if(ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                { 
                    throw;
                }
            }

            return Constants.SQL_RESULT_OK;
        }

        /// <summary>
        /// データ更新(印刷)
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <param name="type">資材区分</param>
        /// <returns>結果</returns>
        public async Task<int> MergePrintData(IList<InventoryPrint> data, int type)
        {
            bool isSelectSQL = false;
            bool isInsertSQL = false;
            bool isUpdateSQL = false;
            string selectSql = string.Empty;
            string insertSql = string.Empty;
            string updateSql = string.Empty;

            string sqlParam = string.Empty;

            string employeeCode = SharedModel.Instance.StartupArgs.EmployeeCode;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    var trans = conn.BeginTransaction();
                    selectSql = @"select ITEM_ORDER, HINMOKUNM, GYOSYANM, BIKOU  from SIZAI_TANAOROSI_MST 
                                          where TRIM(SIZAI_KBN) = :type and TRIM(WORK_KBN) = '1' and HINMOKUCD = :hinmokuCD and GYOSYACD = :gyosyaCD";
                    insertSql = @"insert into SIZAI_TANAOROSI_MST(SIZAI_KBN, WORK_KBN, ITEM_ORDER, HINMOKUCD, GYOSYACD, HINMOKUNM, GYOSYANM, BIKOU, INSUSER, INSYMD,  UPDUSER, UPDYMD)
                                          values(:type, '1', :counter, :hinmokuCD, :gyosyaCD, :hinmokuName, :gyosyaName , :bikou, :employeeCode, SYSDATE ,:employeeCode, SYSDATE)";
                    updateSql = @"update SIZAI_TANAOROSI_MST set ITEM_ORDER = :counter, HINMOKUNM = :hinmokuName, GYOSYANM = :gyosyaName, BIKOU = :bikou, UPDUSER = :employeeCode, UPDYMD = SYSDATE
                                          where TRIM(SIZAI_KBN) = :type and TRIM(WORK_KBN) = '1' and HINMOKUCD = :hinmokuCD and GYOSYACD = :gyosyaCD";
                    int counter = 1;
                    foreach (var item in data)
                    {
                        isSelectSQL = false;
                        isInsertSQL = false;
                        isUpdateSQL = false;

                        var param = new
                        {
                            type = type,
                            counter = counter,
                            hinmokuCD = item.HinmokuCD == null ? new string(' ', 4) : item.HinmokuCD.PadRight(4),
                            gyosyaCD = item.GyosyaCD == null ? new string(' ', 4) : item.GyosyaCD.PadRight(4),
                            hinmokuName = CommonUtility.PadRightSJIS(item.HinmokuName, 40),
                            gyosyaName = CommonUtility.PadRightSJIS(item.GyosyaName, 40),
                            bikou = CommonUtility.PadRightSJIS(item.Bikou, 60),
                            employeeCode = employeeCode
                        };
                        sqlParam = param.ToString();

                        isSelectSQL = true;
                        var originalData = conn.QuerySingleOrDefault(selectSql, param);
                        if (originalData != null && originalData.ITEM_ORDER == param.counter && originalData.HINMOKUNM == param.hinmokuName && originalData.GYOSYANM == param.gyosyaName && originalData.BIKOU == param.bikou)
                        {
                            counter++;
                            continue;
                        }

                        if (originalData == null)
                        { 
                            isSelectSQL = false;
                            isInsertSQL = true;
                            await conn.ExecuteAsync(insertSql, param);
                        }
                        else
                        {
                            isSelectSQL = false;
                            isUpdateSQL = true;
                            await conn.ExecuteAsync(updateSql, param);
                        }
                        counter++;
                    }
                    trans.Commit();
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    string sql = string.Empty;
                    if(isSelectSQL) sql = selectSql;
                    else if(isInsertSQL) sql = insertSql;
                    else if(isUpdateSQL) sql = updateSql;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql, sqlParam);
                }
                else
                {
                    throw;
                }
            }

            return Constants.SQL_RESULT_OK;
        }

        /// <summary>
        /// コードと名称との紐付け
        /// </summary>
        /// <param name="type">種類</param>
        /// <param name="code">コード</param>
        /// <returns>名称</returns>
        public async Task<string> CodeToName(string type, string code)
        {
            string ret = string.Empty;

            string table = "SIZAI_HINMOKU_MST";
            string val = "HINMOKUNM";
            string col = "HINMOKUCD";

            if (type == "Gyosya")
            {
                table = "GYOSYA_MST";
                val = "KOZANM";
                col = "GYOSYACD";
            }

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string sql = $@"select {val} from {table} where {col} = '{code}'";

                    OracleCommand command = new OracleCommand(sql, conn);

                    var tmp = command.ExecuteScalar();

                    if (null != tmp)
                    {
                        ret = tmp.ToString();
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// データ更新(印刷)
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <param name="type">資材区分</param>
        /// <returns>結果</returns>
        public async Task<int> UpdateWrk(IList<InventoryPrint> data, int type)
        {
            string sql = string.Empty;
            string sqlParam = string.Empty;

            string employeeCode = SharedModel.Instance.StartupArgs.EmployeeCode;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();
                    var trans = conn.BeginTransaction();

                    await conn.ExecuteAsync("delete SIZAI_TANAOROSI_WRK where TRIM(SIZAI_KBN) = :type AND WORK_KBN = 1", new { type = type });
                    int counter = 1;
                    foreach (var item in data)
                    {
                        sql = @"insert into SIZAI_TANAOROSI_WRK 
                            (SIZAI_KBN,WORK_KBN,ITEM_ORDER,HINMOKUCD,GYOSYACD,HINMOKUNM,GYOSYANM,TOGETU,TOGETU_YOSOU,NYUKO,HARAI,HENPIN,ZSOUKO,ZEF,ZLF,ZCC,ZSONOTA,ZMETER,ZYOBI1,ZYOBI2,INSUSER,INSYMD,UPDUSER,UPDYMD)
                            VALUES
                            (:type,'1',:counter,:hinmokuCD,:gyosyaCD,:hinmokuName,:gyosyaName,:togetu,:togetuYosou,:nyukoRyo,:harai,:henpin,:sokoZaiko,:efZaiko,:lfZaiko,:ccZaiko,:stockOther,:stockMeter,:stockReserve1,:stockReserve2,:employeeCode,SYSDATE,:employeeCode,SYSDATE)";

                        var param = new
                        {
                            type = type,
                            counter = counter,
                            hinmokuCD = item.HinmokuCD == null ? new string(' ', 4) : item.HinmokuCD.PadRight(4),
                            gyosyaCD = item.GyosyaCD?.PadRight(1) ?? new string(' ', 4),
                            hinmokuName = CommonUtility.PadRightSJIS(item.HinmokuName, 40),
                            gyosyaName = CommonUtility.PadRightSJIS(item.GyosyaName, 40),
                            togetu = item.Togetsuryo,
                            togetuYosou = 0,
                            nyukoRyo = item.NyukoRyo,
                            harai = item.Harai,
                            henpin = item.Henpin,
                            sokoZaiko = item.SKZaiko,
                            efZaiko = item.EFZaiko,
                            lfZaiko = item.LFZaiko,
                            ccZaiko = item.CCZaiko,
                            stockOther = item.OtherZaiko,
                            stockMeter = item.MeterZaiko,
                            stockReserve1 = item.Reserve1Zaiko,
                            stockReserve2 = item.Reserve2Zaiko,
                            employeeCode = employeeCode
                        };
                        sqlParam = param.ToString();
                        await conn.ExecuteAsync(sql, param);

                        counter++;
                    }
                    trans.Commit();
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql, sqlParam);
                }
                else
                {
                    throw;
                }
            }

            return Constants.SQL_RESULT_OK;
        }

        /// <summary>
        /// データ更新(印刷)
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <param name="type">資材区分</param>
        /// <returns>結果</returns>
        [Obsolete("未使用")]
        public async Task<int> UpdateSizaiTanaorosiWork(IList<InventoryPrint> data, int type)
        {
            string employeeCode = SharedModel.Instance.StartupArgs.EmployeeCode;
            string sqlParam = string.Empty;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    var trans = conn.BeginTransaction();
                    string selectSql = @"select ITEM_ORDER, HINMOKUNM, GYOSYANM from SIZAI_TANAOROSI_WRK 
                                          where TRIM(SIZAI_KBN) = :type and TRIM(WORK_KBN) = '1' and HINMOKUCD = :hinmokuCD and GYOSYACD = :gyosyaCD";
                    string insertSql = @"insert into SIZAI_TANAOROSI_WRK(SIZAI_KBN, WORK_KBN, ITEM_ORDER, HINMOKUCD, GYOSYACD, HINMOKUNM, GYOSYANM, TOGETU,TOGETU_YOSOU, NYUKO, HARAI, HENPIN, ZSOUKO, ZEF, ZLF, ZCC, ZSONOTA, ZMETER, ZYOBI1, ZYOBI2, INSUSER, INSYMD, UPDUSER, UPDYMD)
                                          values(:type, '1', :counter, :hinmokuCD, :gyosyaCD, :hinmokuName, :gyosyaName, :togetu, :togetuYosou, :nyuko, :harai, :henpin, :zSK, :zEF, :zLF, :zCC, :zSONOTA, :zMETER, :zYOBI1, :ZYOBI2, :employeeCode, SYSDATE ,:employeeCode, SYSDATE)";
                    string updateSql = @"update SIZAI_TANAOROSI_WRK set ITEM_ORDER = :counter, HINMOKUNM = :hinmokuName, GYOSYANM = :gyosyaName, TOGETU = :togetu, TOGETU_YOSOU = :togetuYosou, NYUKO = :nyuko, HARAI = :harai, HENPIN = :henpin, ZSOUKO = :zSK, ZEF = :zEF, ZLF = :zLF,
                                          ZCC = :zCC, ZSONOTA = :zSONOTA, ZMETER = :zMETER, ZYOBI1 = :zYOBI1, ZYOBI2 = :zYOBI2, UPDUSER = :employeeCode, UPDYMD = SYSDATE 
                                          where TRIM(SIZAI_KBN) = :type and TRIM(WORK_KBN) = '1' and HINMOKUCD = :hinmokuCD and GYOSYACD = :gyosyaCD";
                    int counter = 1;
                    foreach (var item in data)
                    {
                        var param = new
                        {
                            type = type,
                            counter = counter,
                            hinmokuCD = item.HinmokuCD == null ? new string(' ', 4) : item.HinmokuCD.PadRight(4),
                            gyosyaCD = item.GyosyaCD == null ? new string(' ', 4) : item.GyosyaCD.PadRight(4),
                            hinmokuName = CommonUtility.PadRightSJIS(item.HinmokuName, 40),
                            gyosyaName = CommonUtility.PadRightSJIS(item.GyosyaName, 40),
                            togetu = item.Togetsuryo,
                            togetuYosou = 0,
                            nyuko = item.NyukoRyo,
                            harai = item.Harai,
                            henpin = item.Henpin,
                            zSK = item.SKZaiko,
                            zEF = item.EFZaiko,
                            zLF = item.LFZaiko,
                            zCC = item.CCZaiko,
                            zSONOTA = item.OtherZaiko,
                            zMETER = item.MeterZaiko,
                            zYOBI1 = item.Reserve1Zaiko,
                            zYOBI2 = item.Reserve2Zaiko,
                            employeeCode = employeeCode
                        };
                        sqlParam = param.ToString();

                        var originalData = conn.QuerySingleOrDefault(selectSql, param);
                        if (originalData != null && originalData.ITEM_ORDER == param.counter && originalData.HINMOKUNM == param.hinmokuName && originalData.GYOSYANM == param.gyosyaName && originalData.NYUKO == param.nyuko && originalData.HARAI == param.harai && originalData.HENPIN == param.henpin)
                        {
                            counter++;
                            continue;
                        }

                        if (originalData == null)
                            await conn.ExecuteAsync(insertSql, param);
                        else
                            await conn.ExecuteAsync(updateSql, param);
                        counter++;
                    }
                    trans.Commit();
                }
            }

            catch (Exception)
            {
                throw;
            }

            return Constants.SQL_RESULT_OK;

        }

        /// <summary>
        /// データ更新
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <param name="type">資材区分</param>
        /// <returns>結果</returns>
        public async Task<int> UpdateSizaiKensinData(IList<InventoryWork> list)
        {
            try
            {
                await UpdateSizaiTanaorosiMst(list);
                await UpdateSizaiTanaorosiWork(list);
            }

            catch (Exception)
            {
                throw;
            }

            return Constants.SQL_RESULT_OK;
        }
        
        /// <summary>
        /// データ更新
        /// </summary>
        /// <param name="list">対象データ</param>
        /// <returns>結果</returns>
        public async Task<int> UpdateSizaiTanaorosiMst(IList<InventoryWork> list)
        {
            bool isSelectSQL = false;
            bool isInsertSQL = false;
            bool isUpdateSQL = false;
            string selectSql = string.Empty;
            string insertSql = string.Empty;
            string updateSql = string.Empty;
            string sqlParam = string.Empty;

            string employeeCode = SharedModel.Instance.StartupArgs.EmployeeCode;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    var trans = conn.BeginTransaction();
                    selectSql = @"select ITEM_ORDER, HINMOKUNM, GYOSYANM, BIKOU  from SIZAI_TANAOROSI_MST 
                                          where TRIM(SIZAI_KBN) = :type and TRIM(WORK_KBN) = :kbn and HINMOKUCD = :hinmokuCD and GYOSYACD = :gyosyaCD";
                    insertSql = @"insert into SIZAI_TANAOROSI_MST(SIZAI_KBN, WORK_KBN, ITEM_ORDER, HINMOKUCD, GYOSYACD, HINMOKUNM, GYOSYANM, BIKOU, INSUSER, INSYMD,  UPDUSER, UPDYMD)
                                          values(:type, :kbn, :counter, :hinmokuCD, :gyosyaCD, :hinmokuName, :gyosyaName , :bikou, :employeeCode, SYSDATE ,:employeeCode, SYSDATE)";
                    updateSql = @"update SIZAI_TANAOROSI_MST set ITEM_ORDER = :counter, HINMOKUNM = :hinmokuName, GYOSYANM = :gyosyaName, BIKOU = :bikou, UPDUSER = :employeeCode, UPDYMD = SYSDATE
                                          where TRIM(SIZAI_KBN) = :type and TRIM(WORK_KBN) = :kbn and HINMOKUCD = :hinmokuCD and GYOSYACD = :gyosyaCD";
                    foreach (var item in list)
                    {
                        isSelectSQL = false;
                        isInsertSQL = false;
                        isUpdateSQL = false;

                        var param = new
                        {
                            type = item.ShizaiKbn.Trim(' '),
                            kbn = item.WorkKbn.Trim(' '),
                            counter = item.ItemOrder,
                            hinmokuCD = item.ItemCode == null ? new string(' ', 4) : item.ItemCode.PadRight(4),
                            gyosyaCD = item.SupplierCode?.PadRight(1) ?? new string(' ', 4),
                            hinmokuName = CommonUtility.PadRightSJIS(item.ItemName, 40),
                            gyosyaName = CommonUtility.PadRightSJIS(item.SupplierName, 40),
                            bikou = new string(' ', 60),
                            employeeCode = employeeCode

                        };
                        sqlParam = param.ToString();

                        isSelectSQL = true;
                        var originalData = conn.QuerySingleOrDefault(selectSql, param);
                        if (originalData != null && originalData.ITEM_ORDER == param.counter)
                        {
                            continue;
                        }

                        if (originalData == null)
                        { 
                            isSelectSQL = false;
                            isInsertSQL = true;
                            await conn.ExecuteAsync(insertSql, param);
                        }
                        else
                        {
                            isSelectSQL = false;
                            isUpdateSQL = true;
                            await conn.ExecuteAsync(updateSql, param);
                        }
                    }
                    trans.Commit();
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    string sql = string.Empty;
                    if (isSelectSQL) sql = selectSql;
                    else if (isInsertSQL) sql = insertSql;
                    else if (isUpdateSQL) sql = updateSql;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql, sqlParam);
                }
                else
                {
                    throw;
                }
            }

            return Constants.SQL_RESULT_OK;
        }

        /// <summary>
        /// データ更新
        /// </summary>
        /// <param name="list">対象データ</param>
        /// <returns>結果</returns>
        public async Task<int> UpdateSizaiTanaorosiWork(IList<InventoryWork> list)
        {
            bool isSelectSQL = false;
            bool isInsertSQL = false;
            bool isUpdateSQL = false;
            string selectSql = string.Empty;
            string insertSql = string.Empty;
            string updateSql = string.Empty;
            string sqlParam = string.Empty;

            string employeeCode = SharedModel.Instance.StartupArgs.EmployeeCode;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    var trans = conn.BeginTransaction();
                    selectSql = @"select ITEM_ORDER, HINMOKUNM, GYOSYANM from SIZAI_TANAOROSI_WRK 
                                          where TRIM(SIZAI_KBN) = :type and TRIM(WORK_KBN) = :kbn and HINMOKUCD = :hinmokuCD and GYOSYACD = :gyosyaCD";
                    insertSql = @"insert into SIZAI_TANAOROSI_WRK(SIZAI_KBN, WORK_KBN, ITEM_ORDER, HINMOKUCD, GYOSYACD, HINMOKUNM, GYOSYANM, TOGETU,TOGETU_YOSOU, NYUKO, HARAI, HENPIN, ZSOUKO, ZEF, ZLF, ZCC, ZSONOTA, ZMETER, ZYOBI1, ZYOBI2, INSUSER, INSYMD, UPDUSER, UPDYMD)
                                          values(:type, :kbn, :counter, :hinmokuCD, :gyosyaCD, :hinmokuName, :gyosyaName, :togetu, :togetuYosou, :nyuko, :harai, :henpin, :zSK, :zEF, :zLF, :zCC, :zSONOTA, :zMETER, :zYOBI1, :ZYOBI2, :employeeCode, SYSDATE ,:employeeCode, SYSDATE)";
                    updateSql = @"update SIZAI_TANAOROSI_WRK set ITEM_ORDER = :counter, TOGETU = :togetu, UPDUSER = :employeeCode, UPDYMD = SYSDATE 
                                          where TRIM(SIZAI_KBN) = :type and TRIM(WORK_KBN) = :kbn and HINMOKUCD = :hinmokuCD and GYOSYACD = :gyosyaCD";

                    foreach (var item in list)
                    {
                        isSelectSQL = false;
                        isInsertSQL = false;
                        isUpdateSQL = false;

                        var param = new
                        {
                            type = item.ShizaiKbn.Trim(' '),
                            kbn = item.WorkKbn.Trim(' '),
                            counter = item.ItemOrder,
                            hinmokuCD = item.ItemCode == null ? new string(' ', 4) : item.ItemCode.PadRight(4),
                            gyosyaCD = item.SupplierCode?.PadRight(1) ?? new string(' ', 4),
                            hinmokuName = CommonUtility.PadRightSJIS(item.ItemName, 40),
                            gyosyaName = CommonUtility.PadRightSJIS(item.SupplierName, 40),
                            togetu = item.CurrentValue,
                            togetuYosou = item.CurrentExpValue,
                            nyuko = item.InputValue,
                            harai = item.OutputValue,
                            henpin = item.ReturnValue,
                            zSK = item.StockInWarehouse,
                            zEF = item.StockEF,
                            zLF = item.StockLF,
                            zCC = item.StockCC,
                            zSONOTA = item.StockOthers,
                            zMETER = item.StockMeter,
                            zYOBI1 = item.StockYobi1,
                            zYOBI2 = item.StockYobi2,
                            employeeCode = employeeCode

                        };
                        sqlParam = param.ToString();

                        isSelectSQL = true;
                        var originalData = conn.QuerySingleOrDefault(selectSql, param);
                        if (originalData != null && originalData.ITEM_ORDER == param.counter && originalData.TOGETU == param.togetu)
                        {
                            continue;
                        }

                        if (originalData == null)
                        {
                            isSelectSQL = false;
                            isInsertSQL = true;
                            await conn.ExecuteAsync(insertSql, param);
                        }
                        else
                        {
                            isSelectSQL = false;
                            isUpdateSQL = true;
                            await conn.ExecuteAsync(updateSql, param);
                        }
                    }
                    trans.Commit();
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    string sql = string.Empty;
                    if (isSelectSQL) sql = selectSql;
                    else if (isInsertSQL) sql = insertSql;
                    else if (isUpdateSQL) sql = updateSql;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql, sqlParam);
                }
                else
                {
                    throw;
                }
            }

            return Constants.SQL_RESULT_OK;

        }

        /// <summary>
        /// データ更新（作業誌データ液酸入出庫を資材棚卸トランへ更新）
        /// </summary>
        /// <param name="data">対象データ</param>
        /// <param name="work_kbn">作業区分</param>
        /// <returns>結果</returns>
        public async Task<int> UpdateSizaiTanaorosiWorkInOut(IList<MeterDataWorkNoteItem> data, string work_kbn = "2")
        {
            bool isSelectSQL = false;
            bool isUpdateSQL = false;
            string selectSql = string.Empty;
            string updateSql = string.Empty;
            string sqlParam = string.Empty;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    var trans = conn.BeginTransaction();
                    selectSql = @"select HINMOKUCD from SIZAI_TANAOROSI_WRK 
                                          where TRIM(SIZAI_KBN) = :sizai_kbn and TRIM(WORK_KBN) = :kbn and HINMOKUCD = :hinmokuCD";

                    foreach (var item in data)
                    {
                        isSelectSQL = false;
                        isUpdateSQL = false;
                        
                        string update = string.Empty;

                        if (item.Kubun == "1")
                            update = "set NYUKO = :amount ";
                        else if (item.Kubun == "3")
                            update = "set NYUKO = :amount, HARAI = :amount ";
                        else if (item.Kubun == "2")
                            update = "set HARAI = :amount ";
                        else
                            continue;

                        updateSql = @"update SIZAI_TANAOROSI_WRK " + update + @", UPDUSER = :employeeCode, UPDYMD = SYSDATE 
                                          where TRIM(SIZAI_KBN) = :sizai_kbn and TRIM(WORK_KBN) = :kbn and HINMOKUCD = :hinmokuCD";

                        var param = new
                        {
                            kbn = work_kbn,
                            hinmokuCD = item.HinmokuCD == null ? new string(' ', 4) : item.HinmokuCD.PadRight(4),
                            gyosyaCD = item.GyosyaCD?.PadRight(1) ?? new string(' ', 4),
                            sizai_kbn = (item.Mukesaki == "1") ? "2" 
                                        : (item.Mukesaki == "2") ? "3"
                                        : (item.Mukesaki == "3") ? "7" : "8",
                            amount = item.Amount,
                            employeeCode = SharedModel.Instance.StartupArgs.EmployeeCode
                        };
                        sqlParam = param.ToString();

                        isSelectSQL = true;
                        var originalData = conn.QuerySingleOrDefault(selectSql, param);
                        if (originalData == null)
                        {
                            continue;
                        }

                        isSelectSQL = false;
                        isUpdateSQL = true;
                        await conn.ExecuteAsync(updateSql, param);
                    }
                    trans.Commit();
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    string sql = string.Empty;
                    if (isSelectSQL) sql = selectSql;
                    else if (isUpdateSQL) sql = updateSql;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql, sqlParam);
                }
                else
                {
                    throw;
                }
            }

            return Constants.SQL_RESULT_OK;

        }

        /// <summary>
        /// 棚卸マスタデータ取得
        /// </summary>
        /// <param name="shizaiKbn">資材区分</param>
        /// <param name="workKbn">作業区分</param>
        /// <param name="hinmokuCd">品目CD</param>
        /// <param name="gyosyaCd">業者CD</param>
        /// <returns>棚卸マスタデータ</returns>
        public async Task<IEnumerable<InventoryMaster>> GetInventoryMasterAsync(int shizaiKbn, int workKbn, string hinmokuCd, string gyosyaCd)
        {
            string sql = $@"select SIZAI_KBN,WORK_KBN,ITEM_ORDER,HINMOKUCD,GYOSYACD,HINMOKUNM,GYOSYANM,BIKOU" +
                      $@" from SIZAI_TANAOROSI_MST" +
                      $@" where SIZAI_KBN = {shizaiKbn} and  WORK_KBN = {workKbn} and HINMOKUCD = '{hinmokuCd}' and GYOSYACD = '{gyosyaCd}' " +
                      $@" order by ITEM_ORDER";
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    return (await conn.QueryAsync(sql))
                        .Select(result => new InventoryMaster()
                        {
                            ShizaiKbn = result.SIZAI_KBN,
                            WorkKbn = result.WORK_KBN,
                            ItemOrder = result.ITEM_ORDER,
                            ItemCode = result.HINMOKUCD,
                            SupplierCode = result.GYOSYACD,
                            ItemName = result.HINMOKUNM,
                            SupplierName = result.GYOSYANM,
                            Bikou = result.BIKOU
                        });
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 棚卸ワーク一覧の取得
        /// </summary>
        /// <param name="workKbn">作業区分</param>
        /// <returns>棚卸ワーク一覧</returns>
        public async Task<IEnumerable<InventoryWork>> GetInventoriesWorkAsync(string workKbn)
        {
            string sql = @"select SIZAI_KBN,WORK_KBN,ITEM_ORDER,HINMOKUCD,GYOSYACD,HINMOKUNM,GYOSYANM," +
                             @"TOGETU,TOGETU_YOSOU,NYUKO,HARAI,HENPIN," +
                             @"ZSOUKO,ZEF,ZLF,ZCC,ZSONOTA,ZMETER,ZYOBI1,ZYOBI2" +
                             @" from SIZAI_TANAOROSI_WRK" +
                             @" where WORK_KBN = {0} AND ((NYUKO > 0) OR (HARAI > 0) OR (HENPIN > 0) OR (ZSOUKO > 0) OR (ZEF > 0) OR (ZLF > 0) OR (ZCC > 0)) "+
                      @" order by SIZAI_KBN,WORK_KBN,ITEM_ORDER";
            string execSql = string.Format(sql, workKbn);

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    return (await conn.QueryAsync(execSql))
                        .Select(result => new InventoryWork()
                        {
                            ShizaiKbn = result.SIZAI_KBN,
                            WorkKbn = result.WORK_KBN,
                            ItemOrder = result.ITEM_ORDER,
                            ItemCode = result.HINMOKUCD,
                            SupplierCode = result.GYOSYACD,
                            ItemName = result.HINMOKUNM,
                            SupplierName = result.GYOSYANM,
                            CurrentValue = getTogetu(result.TOGETU),
                            CurrentExpValue = result.TOGETU_YOSOU,
                            InputValue = result.NYUKO,
                            OutputValue = result.HARAI,
                            ReturnValue = result.HENPIN,
                            StockInWarehouse = result.ZSOUKO,
                            StockEF = result.ZEF,
                            StockLF = result.ZLF,
                            StockCC = result.ZCC,
                            StockOthers = result.ZSONOTA,
                            StockMeter = result.ZMETER,
                            StockYobi1 = result.ZYOBI1,
                            StockYobi2 = result.ZYOBI2
                        });
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), execSql);
                }
                else
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// 棚卸ワーク一覧の取得
        /// </summary>
        /// <param name="workKbn">作業区分</param>
        /// <returns>棚卸ワーク一覧</returns>
        public async Task<IEnumerable<InventoryWork>> GetInventoriesKensinWorkAsync(string workKbn)
        {
            string sql = @"select SIZAI_KBN,WORK_KBN,ITEM_ORDER,HINMOKUCD,GYOSYACD,HINMOKUNM,GYOSYANM," +
                             @"TOGETU,TOGETU_YOSOU,NYUKO,HARAI,HENPIN," +
                             @"ZSOUKO,ZEF,ZLF,ZCC,ZSONOTA,ZMETER,ZYOBI1,ZYOBI2" +
                             @" from SIZAI_TANAOROSI_WRK" +
                             @" where WORK_KBN = {0} " +
                             @" order by SIZAI_KBN,WORK_KBN,ITEM_ORDER";
            string execSql = string.Format(sql, workKbn);

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    return (await conn.QueryAsync(execSql))
                        .Select(result => new InventoryWork()
                        {
                            ShizaiKbn = result.SIZAI_KBN,
                            WorkKbn = result.WORK_KBN,
                            ItemOrder = result.ITEM_ORDER,
                            ItemCode = result.HINMOKUCD,
                            SupplierCode = result.GYOSYACD,
                            ItemName = result.HINMOKUNM,
                            SupplierName = result.GYOSYANM,
                            CurrentValue = getTogetu(result.TOGETU),
                            CurrentExpValue = result.TOGETU_YOSOU,
                            InputValue = result.NYUKO,
                            OutputValue = result.HARAI,
                            ReturnValue = result.HENPIN,
                            StockInWarehouse = result.ZSOUKO,
                            StockEF = result.ZEF,
                            StockLF = result.ZLF,
                            StockCC = result.ZCC,
                            StockOthers = result.ZSONOTA,
                            StockMeter = result.ZMETER,
                            StockYobi1 = result.ZYOBI1,
                            StockYobi2 = result.ZYOBI2
                        });
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), execSql);
                }
                else
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// 資材棚卸マスタ一覧の取得
        /// </summary>
        /// <param name="type">資材区分</param>
        /// <returns>棚卸ワーク一覧</returns>
        public async Task<IEnumerable<InventoryWork>> GetInventoriesWorkFromMsterAsync(string workKbn)
        {
            string sql = $@"select SIZAI_KBN,WORK_KBN,ITEM_ORDER,HINMOKUCD,GYOSYACD,HINMOKUNM,GYOSYANM" +
                     $@" from SIZAI_TANAOROSI_MST where WORK_KBN = {workKbn} order by SIZAI_KBN,WORK_KBN,ITEM_ORDER";
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    return (await conn.QueryAsync(sql))
                        .Select(result => new InventoryWork()
                        {
                            ShizaiKbn = result.SIZAI_KBN,
                            WorkKbn = result.WORK_KBN,
                            ItemOrder = result.ITEM_ORDER,
                            ItemCode = result.HINMOKUCD,
                            SupplierCode = result.GYOSYACD,
                            ItemName = result.HINMOKUNM,
                            SupplierName = result.GYOSYANM,
                            CurrentValue = null,
                            CurrentExpValue = 0,
                            InputValue = 0,
                            OutputValue = 0,
                            ReturnValue = 0,
                            StockInWarehouse = 0,
                            StockEF = 0,
                            StockLF = 0,
                            StockCC = 0,
                            StockOthers = 0,
                            StockMeter = 0,
                            StockYobi1 = 0,
                            StockYobi2 = 0
                        });
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 棚卸ワーク更新
        /// </summary>
        /// <param name="list">棚卸ワーク一覧</param>
        /// <returns>結果</returns>
        public async Task<int> UpdateInventoriesWorkAsync(IList<InventoryWork> list)
        {
            string sql = string.Empty;
            string sqlParam = string.Empty;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    OracleCommand cmd = conn.CreateCommand();

                    cmd.CommandText = @"UPDATE SIZAI_TANAOROSI_WRK SET TOGETU = :TOGETU, TOGETU_YOSOU = :TOGETU_YOSOU, UPDYMD = SYSDATE" +
                                      @" WHERE SIZAI_KBN = :SIZAI_KBN AND WORK_KBN = :WORK_KBN AND ITEM_ORDER = :ITEM_ORDER AND HINMOKUCD = :HINMOKUCD AND GYOSYACD = :GYOSYACD ";
                    OracleParameter pramTogetu = cmd.Parameters.Add("TOGETU", OracleDbType.Varchar2, System.Data.ParameterDirection.Input);
                    OracleParameter pramTogetuYosou = cmd.Parameters.Add("TOGETU_YOSOU", OracleDbType.Long, System.Data.ParameterDirection.Input);
                    OracleParameter pramSizaiKbn = cmd.Parameters.Add("SIZAI_KBN", OracleDbType.Char, System.Data.ParameterDirection.Input);
                    OracleParameter pramWorkKbn = cmd.Parameters.Add("WORK_KBN", OracleDbType.Char, System.Data.ParameterDirection.Input);
                    OracleParameter pramItemOrder = cmd.Parameters.Add("ITEM_ORDER", OracleDbType.Long, System.Data.ParameterDirection.Input);
                    OracleParameter pramHinmokuCd = cmd.Parameters.Add("HINMOKUCD", OracleDbType.Char, System.Data.ParameterDirection.Input);
                    OracleParameter pramGyosyaCd = cmd.Parameters.Add("GYOSYACD", OracleDbType.Char, System.Data.ParameterDirection.Input);
                    sql = cmd.CommandText;

                    foreach (InventoryWork item in list)
                    {
                        // 入力されているもののみを更新
                        if(item.CurrentValue != null && item.CurrentValue.Length > 0)
                        {
                            pramSizaiKbn.Value = item.ShizaiKbn;
                            pramWorkKbn.Value = item.WorkKbn;
                            pramItemOrder.Value = item.ItemOrder;
                            pramHinmokuCd.Value = item.ItemCode;
                            pramGyosyaCd.Value = item.SupplierCode;
                            pramTogetu.Value = item.CurrentValue;
                            pramTogetuYosou.Value = item.CurrentExpValue;

                            var param = new
                            {
                                TOGETU = item.CurrentValue,
                                TOGETU_YOSOU = item.CurrentExpValue,
                                SIZAI_KBN = item.ShizaiKbn,
                                WORK_KBN = item.WorkKbn,
                                ITEM_ORDER = item.ItemOrder,
                                HINMOKUCD = item.ItemCode,
                                GYOSYACD = item.SupplierCode

                            };
                            sqlParam = param.ToString();

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql, sqlParam);
                }
                else
                {
                    throw;
                }
            }

            return 0;
        }

        /// <summary>
        /// 単価入力チェック
        /// </summary>
        /// <returns>単価未入力の品目CD一覧</returns>
        public async Task<IEnumerable<Tuple<string, string>>> CheckNotEnteredTanka()
        {
            var notEnteredList = new List<System.Tuple<string, string>>();
            string sql = string.Empty;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    sql = $@"select h.HINMOKUCD, h.HINMOKUNM from SIZAI_HINMOKU_MST h where not exists ( select * from SIZAI_TANKA_MST t where h.HINMOKUCD = t.HINMOKUCD )";

                    OracleCommand command = new OracleCommand(sql, conn);

                    OracleDataReader reader;
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        notEnteredList.Add(Tuple.Create<string, string>(reader.GetString(0), reader.GetString(1)));
                    }
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                {
                    throw;
                }
            }

            return notEnteredList;

        }

        /// <summary>
        /// 立会い用棚卸表印刷データ取得
        /// </summary>
        /// <param name="date">取得したい日(yyyyMM)</param>
        /// <returns>印刷用データ</returns>
        public async Task<IEnumerable<InventoryWitness>> GetPrintWitnessModelAsync(string date)
        {
            string sql = string.Empty;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    sql = $@"SELECT WOW02.HINMOKUCD, WOW02.HIMOKU, WOW02.UTIWAKE, WOW02.TANABAN, WOW02.HINMOKUNM, WOW02.TANI,
                               WOW01.ZSOUKO, WOW01.ZEF, WOW01.ZLF, WOW01.ZCC, WOW01.ZSONOTA, WOW01.ZMETER
                                 FROM
                                      (SELECT TANAYM, HINMOKUCD, GYOSYACD, HINMOKUNM, GYOSYANM, ZSOUKO, ZEF, ZLF, ZCC, ZSONOTA, ZMETER, ZYOBI1, ZYOBI2, UPDYMD
                                            FROM SIZAI_TANAOROSI_TRN WHERE TANAYM='{date}' )  WOW01 ,
                                                 (SELECT HINMOKUCD, HINMOKUNM, HIMOKU, UTIWAKE, TANABAN, TANI, SYUBETU, SUIBUNKBN, KENSYUKBN, HOUKOKUKBN, ICHIKBN, MUKESAKI, UPDYMD
                                                       FROM SIZAI_HINMOKU_MST WHERE SYUBETU ='1' )  WOW02 
                                                         WHERE WOW01.HINMOKUCD (+) = WOW02.HINMOKUCD
                                                              ORDER BY WOW02.HIMOKU, WOW02.UTIWAKE, WOW02.TANABAN, WOW02.HINMOKUCD";

                    return (await conn.QueryAsync(sql))
                          .Select(result => new InventoryWitness()
                          {
                              HinmokuCD = result.HINMOKUCD,
                              HinmokuName = result.HINMOKUNM,
                              Himoku = result.HIMOKU,
                              Utiwake = result.UTIWAKE,
                              Tani = result.TANI,
                              Tanaban = result.TANABAN,
                              ZSK = result.ZSOUKO == null ? 0 : result.ZSOUKO,
                              ZEF = result.ZEF == null ? 0 : result.ZEF,
                              ZLF = result.ZLF == null ? 0 : result.ZLF,
                              ZETC = result.ZSONOTA == null ? 0 : result.ZSONOTA,
                              ZCC = result.ZCC == null ? 0 : result.ZCC,
                              ZMeter = result.ZMETER == null ? 0 : result.ZMETER,
                              GenbaSuryo = (
                               (result.ZLF == null ? 0 : result.ZLF) +
                                (result.ZEF == null ? 0 : result.ZEF) +
                                (result.ZCC == null ? 0 : result.ZCC) +
                                (result.ZSONOTA == null ? 0 : result.ZSONOTA) +
                                (result.ZMETER == null ? 0 : result.ZMETER)),
                              Total = (
                               (result.ZEF == null ? 0 : result.ZEF) +
                               (result.ZLF == null ? 0 : result.ZLF) +
                               (result.ZCC == null ? 0 : result.ZCC) +
                               (result.ZSONOTA == null ? 0 : result.ZSONOTA) +
                               (result.ZMETER == null ? 0 : result.ZMETER) +
                               (result.ZSOUKO == null ? 0 : result.ZSOUKO))

                          });
                }
            }
            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 財務提出用棚卸表印刷データの取得
        /// </summary>
        /// <returns>印刷用データ</returns>
        public async Task<IEnumerable<InventoryFinancingPresetation>> GetPrintFinancingPresetationModelAsync()
        {
            string sql = string.Empty;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();
                    sql = $@"select * from SIZAI_TANAOROSI_TATIAI_WRK order by HIMOKU, UTIWAKE, TANABAN";
                    return (await conn.QueryAsync(sql)).Select(result => new InventoryFinancingPresetation()
                        {
                            Himoku = result.HIMOKU,
                            Utiwake = result.UTIWAKE,
                            Tanaban = result.TANABAN,
                            HinmokuName = result.HINMOKUNM,
                            Tani = result.TANI,
                            Kimatusuryo = result.KIMATU_SURYOU
                        });
                }
            }
            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 立会い用棚卸表印刷データ更新
        /// </summary>
        /// <param name="datalist">データ一覧</param>
        /// <returns>印刷用データ</returns>
        public async Task<int> SetPrintWitnessModelAsync(IList<InventoryWitness> datalist)
        {
            string sql = string.Empty;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string deleteSQL = $@"delete from SIZAI_TANAOROSI_TATIAI_WRK";
                    sql = deleteSQL;

                    OracleCommand command = new OracleCommand(deleteSQL, conn);

                    var tmp = command.ExecuteNonQuery();
                    sql = $@"select HINMOKUNM, HIMOKUCD, TANACD, UTIWAKECD from HINMOKU_MST";

                    var hinmeiList = (await conn.QueryAsync(sql)).Select(result => new InventoryWitness()
                        {
                            HinmokuName = result.HINMOKUNM,
                            Himoku = result.HIMOKUCD,
                            Tanaban = result.TANACD,
                            Utiwake = result.UTIWAKECD
                        });

                    for (int i = 0; i < datalist.Count(); ++i)
                    {
                        var each = hinmeiList.Where(n => { return n.Himoku == datalist[i].Himoku && n.Tanaban == datalist[i].Tanaban && n.Utiwake == datalist[i].Utiwake; }).FirstOrDefault();

                        datalist[i].HinmokuName = each.HinmokuName;

                    }

                    string employeeCode = SharedModel.Instance.StartupArgs.EmployeeCode;

                    for (int i = 0; i < datalist.Count(); i++)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append($@"insert into SIZAI_TANAOROSI_TATIAI_WRK( HIMOKU, UTIWAKE, TANABAN, HINMOKUNM, TANI, KIMATU_SURYOU, INSUSER, INSYMD, UPDUSER, UPDYMD )");
                        sb.Append($@" values('{datalist[i].Himoku}', '{datalist[i].Utiwake}', '{datalist[i].Tanaban}', '{datalist[i].HinmokuName}','{datalist[i].Tani}',");
                        sb.Append($@"'{datalist[i].Total}', '{employeeCode}', SYSDATE, '{employeeCode}', SYSDATE)");
                        string insertSQL = sb.ToString();
                        sql = insertSQL;

                        OracleCommand cmd = conn.CreateCommand();
                        cmd.CommandText = insertSQL;

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                {
                    throw;
                }
            }

            return Constants.SQL_RESULT_OK;
        }

        /// <summary>
        /// 計算書(使用高払い)
        /// </summary>
        /// <returns>計算書(使用高払い)</returns>
        public async Task<IEnumerable<Ukebarai>> GetSiyoudakaData()
        {
            string sql = string.Empty;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();
                    sql =
                        $@"SELECT WOW01.HIMOKU, WOW02.HIMOKUNM, WOW01.UTIWAKE, WOW03.HIMOUTINM, WOW04.TANABAN, WOW01.HINMOKUNM, WOW05.KOZANM, WOW04.TANI,
                        WOW04.SZAIKO, WOW04.NYUKO, WOW04.SEF, WOW04.SLF, WOW04.SCC, WOW04.SSONOTA, WOW04.SJIGYO,
                        WOW04.S1JI, WOW04.STD, WOW04.S2JI, WOW04.EZAIKO, WOW04.HINMOKUCD
                        FROM SIZAI_HINMOKU_MST WOW01, HIMOKU_MST WOW02, SEIZO_HIMOKU_MST WOW03, SIZAI_HOUKOKU_TRN WOW04, GYOSYA_MST WOW05, SIZAI_TANKA_MST WOW06
                        WHERE WOW01.HIMOKU = WOW03.HIMOKUCD AND WOW03.HIMOKUCD = WOW02.HIMOKUCD AND WOW02.HIMOKUCD = WOW04.HIMOKU
                        AND WOW01.UTIWAKE = WOW03.UTIWAKECD AND WOW03.UTIWAKECD = WOW04.UTIWAKE AND WOW01.TANABAN  = WOW04.TANABAN
                        AND WOW06.HINMOKUCD = WOW01.HINMOKUCD AND WOW01.HINMOKUCD = WOW04.HINMOKUCD AND WOW05.GYOSYACD = WOW06.GYOSYACD AND WOW04.HINMOKUCD<>'   '
                        ORDER BY WOW01.HIMOKU, WOW01.UTIWAKE, WOW04.TANABAN,WOW01.HINMOKUNM";
                    return (await conn.QueryAsync(sql)).Select(result => new Ukebarai()
                        {
                            Himoku = result.HIMOKU,
                            HimokuName = result.HIMOKUNM,
                            Utiwake = result.UTIWAKE,
                            UtiwakeName = result.HIMOUTINM,
                            Tanaban = result.TANABAN,
                            HinmokuCode = result.HINMOKUCD,
                            HinmokuName = result.HINMOKUNM,
                            KouzaName = result.KOZANM,
                            Tani = result.TANI,
                            SZaiko = result.SZAIKO,
                            Nyuko = result.NYUKO,
                            EFShukko = result.SEF,
                            LFShukko = result.SLF,
                            CCShukko = result.SCC,
                            OtherShukko = result.SSONOTA,
                            BusinessDevelopment = result.SJIGYO,
                            PrimaryCutting = result.S1JI,
                            TDShukko = result.STD,
                            SecondarycCutting = result.S2JI,
                            EZaiko = result.EZAIKO
                        });
                }
            }
            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 計算書(入庫払い)
        /// </summary>
        /// <returns>計算書(入庫払い)</returns>
        public async Task<IEnumerable<Ukebarai>> GetNyukoData()
        {
            string sql = string.Empty;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();
                    sql =
                            $@"SELECT WOW01.HIMOKUCD, WOW02.HIMOKUNM, WOW01.UTIWAKECD, WOW03.HIMOUTINM, WOW04.TANABAN, WOW01.HINMOKUNM, WOW04.TANI, 
                            WOW04.SZAIKO, WOW04.NYUKO, WOW04.SEF, WOW04.SLF, WOW04.SCC, WOW04.SSONOTA, WOW04.SJIGYO,
                            WOW04.S1JI, WOW04.STD, WOW04.S2JI, WOW04.EZAIKO
                            FROM HINMOKU_MST WOW01, HIMOKU_MST WOW02, SEIZO_HIMOKU_MST WOW03, SIZAI_HOUKOKU_TRN WOW04
                            WHERE WOW01.HIMOKUCD = WOW03.HIMOKUCD AND WOW03.HIMOKUCD = WOW02.HIMOKUCD AND WOW02.HIMOKUCD = WOW04.HIMOKU AND WOW01.UTIWAKECD = WOW03.UTIWAKECD
                            AND WOW03.UTIWAKECD = WOW04.UTIWAKE AND WOW01.TANACD = WOW04.TANABAN AND WOW04.HINMOKUCD='    '
                            ORDER BY WOW01.HIMOKUCD, WOW01.UTIWAKECD, WOW04.TANABAN";

                    return (await conn.QueryAsync(sql)).Select(result => new Ukebarai()
                        {
                            Himoku = result.HIMOKUCD,
                            HimokuName = result.HIMOKUNM,
                            Utiwake = result.UTIWAKECD,
                            UtiwakeName = result.HIMOUTINM,
                            Tanaban = result.TANABAN,
                            HinmokuCode = string.Empty,
                            HinmokuName = result.HINMOKUNM,
                            Tani = result.TANI,
                            SZaiko = result.SZAIKO,
                            Nyuko = result.NYUKO,
                            EFShukko = result.SEF,
                            LFShukko = result.SLF,
                            CCShukko = result.SCC,
                            OtherShukko = result.SSONOTA,
                            BusinessDevelopment = result.SJIGYO,
                            PrimaryCutting = result.S1JI,
                            TDShukko = result.STD,
                            SecondarycCutting = result.S2JI,
                            EZaiko = result.EZAIKO
                        });
                }
            }
            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// 資材棚卸トランデータ削除
        /// </summary>
        /// <param name="tanaYm">棚卸年月(yyyyMM)</param>
        /// <returns>結果</returns>
        public async Task<int> DeleteInventoriesTranAsync(string tanaYm)
        {
            string sql = string.Empty;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    OracleCommand cmd = conn.CreateCommand();

                    cmd.CommandText = $@"DELETE FROM SIZAI_TANAOROSI_TRN WHERE TANAYM = '{tanaYm}'";
                    sql = cmd.CommandText;
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                {
                    throw;
                }
            }

            return Constants.SQL_RESULT_OK;
        }

        /// <summary>
        /// 資材棚卸トランデータ削除
        /// </summary>
        /// <param name="tanaYm">棚卸年月(yyyyMM)</param>
        /// <returns>結果</returns>
        public async Task<int> DeleteSizaiTanaorosiTranAsync(string tanaYm,string workKbn)
        {
            string sql = string.Empty;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    OracleCommand cmd = conn.CreateCommand();

                    cmd.CommandText = $@"DELETE FROM SIZAI_TANAOROSI_TRN WHERE TANAYM = '{tanaYm}' AND HINMOKUCD in(SELECT HINMOKUCD from SIZAI_TANAOROSI_WRK WHERE WORK_KBN = '{workKbn}')";
                    sql = cmd.CommandText;
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                {
                    throw;
                }
            }

            return Constants.SQL_RESULT_OK;
        }

        /// <summary>
        /// 資材棚卸トランデータ登録
        /// </summary>
        /// <param name="tanaYm">棚卸年月(yyyyMM)</param>
        /// <returns>結果</returns>
        public async Task<int> EntryInventoriesTranAsync(string tanaYm)
        {
            string sql = string.Empty;
            string sqlParam = string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.Append("INSERT INTO SIZAI_TANAOROSI_TRN(");
            sb.Append("TANAYM, HINMOKUCD, GYOSYACD, HINMOKUNM, GYOSYANM, ZSOUKO, ZEF, ZLF, ZCC, ZSONOTA, ZMETER, ZYOBI1, ZYOBI2, UPDYMD");
            sb.Append(") ");
            sb.Append("SELECT :TANAYM,t.HINMOKUCD,t.GYOSYACD,h.HINMOKUNM,case when g.GYOSYANM is null then cast(' ' as char(40)) else g.GYOSYANM end,ZSOUKO,ZEF,ZLF,ZCC,ZSONOTA,ZMETER,ZYOBI1,ZYOBI2,SYSDATE");
            sb.Append(" FROM( ");
            sb.Append("SELECT HINMOKUCD, GYOSYACD");
            sb.Append(",SUM(ZSOUKO) as ZSOUKO");
            sb.Append(",SUM(ZEF) as ZEF");
            sb.Append(",SUM(ZLF) as ZLF");
            sb.Append(",SUM(ZCC) as ZCC");
            sb.Append(",SUM(ZSONOTA) as ZSONOTA");
            sb.Append(",SUM(ZMETER) as ZMETER");
            sb.Append(",SUM(ZYOBI1) as ZYOBI1");
            sb.Append(",SUM(ZYOBI2) as ZYOBI2");
            sb.Append(" FROM( ");
            sb.Append("SELECT ");
            sb.Append(" HINMOKUCD, GYOSYACD");
            sb.Append(",DECODE(SIZAI_KBN, 1, TO_NUMBER(TOGETU), 0) as ZSOUKO");
            sb.Append(",DECODE(SIZAI_KBN, 2, TO_NUMBER(TOGETU), 0) as ZEF");
            sb.Append(",DECODE(SIZAI_KBN, 3, TO_NUMBER(TOGETU), 4, TO_NUMBER(TOGETU), 5, TO_NUMBER(TOGETU), 0) as ZLF");
            sb.Append(",DECODE(SIZAI_KBN, 6, TO_NUMBER(TOGETU), 7, TO_NUMBER(TOGETU), 0) as ZCC");
            sb.Append(",DECODE(SIZAI_KBN, 8, TO_NUMBER(TOGETU), 0) as ZSONOTA");
            sb.Append(",DECODE(SIZAI_KBN, 9, TO_NUMBER(TOGETU), 0) as ZMETER");
            sb.Append(",DECODE(SIZAI_KBN, 10, TO_NUMBER(TOGETU), 0) as ZYOBI1");
            sb.Append(",DECODE(SIZAI_KBN, 11, TO_NUMBER(TOGETU), 0) as ZYOBI2");
            sb.Append(" FROM SIZAI_TANAOROSI_WRK");
            sb.Append(" WHERE TOGETU is not NULL");
            sb.Append(" )");
            sb.Append(" GROUP BY HINMOKUCD, GYOSYACD");
            sb.Append(") t, sizai_hinmoku_mst h, gyosya_mst g ");
            sb.Append("WHERE t.HINMOKUCD = h.HINMOKUCD");
            sb.Append("  AND t.GYOSYACD = g.GYOSYACD(+) ");

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    OracleCommand cmd = conn.CreateCommand();
                    OracleParameter pramTanaYm = cmd.Parameters.Add("TANAYM", OracleDbType.Char, System.Data.ParameterDirection.Input);

                    cmd.CommandText = sb.ToString();
                    sql = cmd.CommandText;
                    pramTanaYm.Value = tanaYm;
                    var param = new
                    {
                        TANAYM = pramTanaYm.Value
                    };
                    sqlParam = param.ToString();

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql, sqlParam);
                }
                else
                {
                    throw;
                }
            }

            return Constants.SQL_RESULT_OK;
        }

        /// <summary>
        /// 資材棚卸トランデータ登録
        /// </summary>
        /// <param name="tanaYm">棚卸年月(yyyyMM)</param>
        /// <param name="workKbn">作業区分</param>
        /// <returns>結果</returns>
        public async Task<int> EntryInventoriesTranAsync(string tanaYm, string workKbn)
        {
            string sql = string.Empty;
            string sqlParam = string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.Append("INSERT INTO SIZAI_TANAOROSI_TRN(");
            sb.Append("TANAYM, HINMOKUCD, GYOSYACD, HINMOKUNM, GYOSYANM, ZSOUKO, ZEF, ZLF, ZCC, ZSONOTA, ZMETER, ZYOBI1, ZYOBI2, UPDYMD");
            sb.Append(") ");
            sb.Append("SELECT :TANAYM,t.HINMOKUCD,t.GYOSYACD,h.HINMOKUNM,case when g.GYOSYANM is null then cast(' ' as char(40)) else g.GYOSYANM end,ZSOUKO,ZEF,ZLF,ZCC,ZSONOTA,ZMETER,ZYOBI1,ZYOBI2,SYSDATE");
            sb.Append(" FROM( ");
            sb.Append("SELECT HINMOKUCD, GYOSYACD");
            sb.Append(",SUM(ZSOUKO) as ZSOUKO");
            sb.Append(",SUM(ZEF) as ZEF");
            sb.Append(",SUM(ZLF) as ZLF");
            sb.Append(",SUM(ZCC) as ZCC");
            sb.Append(",SUM(ZSONOTA) as ZSONOTA");
            sb.Append(",SUM(ZMETER) as ZMETER");
            sb.Append(",SUM(ZYOBI1) as ZYOBI1");
            sb.Append(",SUM(ZYOBI2) as ZYOBI2");
            sb.Append(" FROM( ");
            sb.Append("SELECT ");
            sb.Append(" HINMOKUCD, GYOSYACD");
            sb.Append(",DECODE(SIZAI_KBN, 1, TO_NUMBER(TOGETU), 0) as ZSOUKO");
            sb.Append(",DECODE(SIZAI_KBN, 2, TO_NUMBER(TOGETU), 0) as ZEF");
            sb.Append(",DECODE(SIZAI_KBN, 3, TO_NUMBER(TOGETU), 4, TO_NUMBER(TOGETU), 5, TO_NUMBER(TOGETU), 0) as ZLF");
            sb.Append(",DECODE(SIZAI_KBN, 6, TO_NUMBER(TOGETU), 7, TO_NUMBER(TOGETU), 0) as ZCC");
            sb.Append(",DECODE(SIZAI_KBN, 8, TO_NUMBER(TOGETU), 0) as ZSONOTA");
            sb.Append(",DECODE(SIZAI_KBN, 9, TO_NUMBER(TOGETU), 0) as ZMETER");
            sb.Append(",DECODE(SIZAI_KBN, 10, TO_NUMBER(TOGETU), 0) as ZYOBI1");
            sb.Append(",DECODE(SIZAI_KBN, 11, TO_NUMBER(TOGETU), 0) as ZYOBI2");
            sb.Append(" FROM SIZAI_TANAOROSI_WRK");
            sb.Append(" WHERE SIZAI_TANAOROSI_WRK.WORK_KBN = :WORK_KBN AND TOGETU is not NULL");
            sb.Append(" )");
            sb.Append(" GROUP BY HINMOKUCD, GYOSYACD");
            sb.Append(") t, sizai_hinmoku_mst h, gyosya_mst g ");
            sb.Append("WHERE t.HINMOKUCD = h.HINMOKUCD");
            sb.Append("  AND t.GYOSYACD = g.GYOSYACD(+) ");

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    OracleCommand cmd = conn.CreateCommand();
                    OracleParameter pramTanaYm = cmd.Parameters.Add("TANAYM", OracleDbType.Char, System.Data.ParameterDirection.Input);
                    OracleParameter pramWorkKbn = cmd.Parameters.Add("WORK_KBN", OracleDbType.Char, System.Data.ParameterDirection.Input);

                    cmd.CommandText = sb.ToString();
                    pramTanaYm.Value = tanaYm;
                    pramWorkKbn.Value = workKbn;
                    var param = new
                    {
                        TANAYM = pramTanaYm.Value,
                        WORK_KBN = pramWorkKbn.Value
                    };
                    sqlParam = param.ToString();

                    sql = cmd.CommandText;
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql, sqlParam);
                }
                else
                {
                    throw;
                }
            }

            return Constants.SQL_RESULT_OK;
        }

        /// <summary>
        /// 棚卸期間の取得
        /// </summary>
        /// <param name="category">システム分類(1：資材 2：部品 (スペース)：未設定)</param>
        /// <returns>棚卸期間</returns>
        public async Task<IEnumerable<InventoryTerm>> GetInventoryPeriodsAsync(string category)
        {
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    return (await conn.QueryAsync(
                            $@"select TANAYM,KAIKEI_NENDO,KAIKEI_KIKAN from TANAOROSI_KAIKEI_KIKAN_MST where SYSTEM_CATEGORY='{category}'"))
                        .Select(result => new InventoryTerm()
                        {
                            YearMonth = result.TANAYM,
                            Term = result.KAIKEI_NENDO,
                            Half = result.KAIKEI_KIKAN
                        });
                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 操作一覧の取得
        /// </summary>
        /// <param name="systemCategory">システムカテゴリー</param>
        /// <param name="syainCd">社員コード</param>
        /// <param name="workClass">作業区分(1:棚卸、2:検針)</param>
        /// <param name="isTermEnd">期末か</param>
        /// <returns>操作者情報</returns>
        public async Task<IEnumerable<OperationInfo>> GetOperatorAsync(string systemCategory, string syainCd, string workKbn, bool isTermEnd)
        {
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    var sb = new StringBuilder();
                    sb.Append("select c.OPERATION_CD , c.OPERATION_NAME , b.WORK_KBN");
                    sb.Append(" from TANAOROSI_OPERATION_GROUP_MST a");
                    sb.Append(" inner join TANAOROSI_OPERATION_MENU_MST b on a.SYAINSZCD = b.SYAINSZCD and a.SYSTEM_CATEGORY = b.SYSTEM_CATEGORY and a.GROUP_KBN = b.GROUP_KBN");
                    sb.Append(" inner join TANAOROSI_OPERATION_MST c on b.OPERATION_CD = c.OPERATION_CD and b.SYSTEM_CATEGORY = c.SYSTEM_CATEGORY ");
                    sb.Append(" inner join SYAIN_MST d on a.SYAINCD = d.SYAINCD and a.SYAINSZCD = b.SYAINSZCD");
                    sb.Append(" where TRIM(d.SYAINCD) = :SYAINCD and TRIM(b.SYSTEM_CATEGORY) = :SYSTEM_CATEGORY and TRIM(b.WORK_KBN) = :WORK_KBN and TRIM(b.OPERATION_TYPE) = :OPERATION_TYPE");
                    sb.Append(" order by b.OPERATION_ORDER");

                    return (await conn.QueryAsync(sb.ToString(), new { SYAINCD = syainCd, SYSTEM_CATEGORY = systemCategory, WORK_KBN = workKbn, OPERATION_TYPE = isTermEnd ? "1" : "0" }))
                        .Select(result => new OperationInfo()
                        {
                            WorkType = result.WORK_KBN,
                            Code = result.OPERATION_CD,
                            Name = result.OPERATION_NAME
                        });
                }
            }

            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 棚卸ワーク一覧取得
        /// </summary>
        /// <param name="workKbn">作業区分</param>
        /// <param name="himoku">費目</param>
        /// <param name="utiwake">内訳</param>
        /// <param name="tanaban">棚番</param>
        /// <param name="hinmokuCd">品目コード</param>
        /// <returns>棚卸ワーク一覧</returns>
        public async Task<IEnumerable<InventoryWork>> GetInventoriesWorkAsync(string himoku, string utiwake, string tanaban, string hinmokuCd = "")
        {
            string sql = string.Empty;
        
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select TW.SIZAI_KBN,TW.WORK_KBN,TW.ITEM_ORDER,TW.HINMOKUCD,TW.GYOSYACD,TW.HINMOKUNM,TW.GYOSYANM,");
            sb.Append(@"TW.TOGETU,TW.TOGETU_YOSOU,TW.NYUKO,TW.HARAI,TW.HENPIN,");
            sb.Append(@"TW.ZSOUKO,TW.ZEF,TW.ZLF,TW.ZCC,TW.ZSONOTA,TW.ZMETER,TW.ZYOBI1,TW.ZYOBI2");
            sb.Append(@" from SIZAI_TANAOROSI_WRK TW, SIZAI_HINMOKU_MST HM");
            sb.Append($@" where HM.HIMOKU = {himoku} and HM.UTIWAKE = {utiwake} and HM.TANABAN = {tanaban} and TW.TOGETU is not null ");
            if (hinmokuCd.Trim() != string.Empty)
            {
                sb.Append($@" and HM.HINMOKUCD = '{hinmokuCd}' ");
            }
            sb.Append(@" and TW.HINMOKUCD = HM.HINMOKUCD ");
            sb.Append(@" order by TW.SIZAI_KBN,TW.HINMOKUCD,TW.WORK_KBN,TW.ITEM_ORDER");

            sql = sb.ToString();

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    return (await conn.QueryAsync(sql))
                        .Select(result => new InventoryWork()
                        {
                            ShizaiKbn = result.SIZAI_KBN,
                            WorkKbn = result.WORK_KBN,
                            ItemOrder = result.ITEM_ORDER,
                            ItemCode = result.HINMOKUCD,
                            SupplierCode = result.GYOSYACD,
                            ItemName = result.HINMOKUNM,
                            SupplierName = result.GYOSYANM,
                            CurrentValue = getTogetu(result.TOGETU),
                            CurrentExpValue = result.TOGETU_YOSOU,
                            InputValue = result.NYUKO,
                            OutputValue = result.HARAI,
                            ReturnValue = result.HENPIN,
                            StockInWarehouse = result.ZSOUKO,
                            StockEF = result.ZEF,
                            StockLF = result.ZLF,
                            StockCC = result.ZCC,
                            StockOthers = result.ZSONOTA,
                            StockMeter = result.ZMETER,
                            StockYobi1 = result.ZYOBI1,
                            StockYobi2 = result.ZYOBI2
                        });
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                {
                    throw;
                }
            }
        }

        private string getTogetu(string value) {return value; }
        private string getTogetu(long value) {return value.ToString(); }

        /// <summary>
        /// 資材棚卸操作グループ区分の取得
        /// </summary>
        /// <param name="systemCategory">システムカテゴリー</param>
        /// <param name="syainCd">社員CD</param>
        /// <returns>資材棚卸操作グループ区分</returns>
        public async Task<string> GetSizaiTanaorosiGroupKbn(string systemCategory, string syainCd)
        {
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"select g.GROUP_KBN from TANAOROSI_OPERATION_GROUP_MST g");
                    sb.Append($@" inner join SYAIN_MST s on s.SYAINSZCD = g.SYAINSZCD and s.SYAINCD = g.SYAINCD");
                    sb.Append($@" where TRIM(g.SYAINCD) = :SYAINCD and TRIM(g.SYSTEM_CATEGORY) = :SYSTEM_CATEGORY");
                    string sql = sb.ToString();

                    return (await conn.QueryAsync(sql, new { SYSTEM_CATEGORY = systemCategory, SYAINCD = syainCd })).FirstOrDefault()?.GROUP_KBN.Trim();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 棚卸進捗状況マスタの更新
        /// </summary>
        /// <param name="workKbn">作業区分</param>
        /// <param name="operationNo">操作No</param>
        /// <param name="operationCondition">操作状況</param>
        /// <returns>結果</returns>
        public async Task<int> UpdateTanaorosiProgressMst(string workKbn, int operationNo, string operationCondition)
        {
            string sql = string.Empty;
        
            try
            {
                string updateUser = SharedModel.Instance.StartupArgs.EmployeeCode;

                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    StringBuilder sb = new StringBuilder();
                    sb.Append($@"UPDATE TANAOROSI_PROGRESS_MST SET OPERATION_NO='{operationNo}',");
                    sb.Append($@"OPERATION_CONDITION='{operationCondition}', UPDUSER='{updateUser}', UPDYMD=SYSDATE");
                    sb.Append($@" WHERE SYSTEM_CATEGORY='1' AND WORK_KBN='{workKbn}'");
                    sql = sb.ToString();

                    OracleCommand cmd = conn.CreateCommand();
                    cmd.CommandText = sql;

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                {
                    throw;
                }
            }

            return Constants.SQL_RESULT_OK;
        }

        /// <summary>
        /// 棚卸進捗状況マスタの更新
        /// </summary>
        /// <param name="workKbn">作業区分</param>
        /// <param name="operationNo">操作No</param>
        /// <returns>結果</returns>
        public async Task<int> UpdateTanaorosiProgressMst(string workKbn, int operationNo)
        {
            string sql = string.Empty;

            try
            {
                string updateUser = SharedModel.Instance.StartupArgs.EmployeeCode;

                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    StringBuilder sb = new StringBuilder();
                    sb.Append($@"UPDATE TANAOROSI_PROGRESS_MST SET OPERATION_NO='{operationNo}',");
                    sb.Append($@"UPDUSER='{updateUser}', UPDYMD=SYSDATE");
                    sb.Append($@" WHERE SYSTEM_CATEGORY='1' AND WORK_KBN='{workKbn}'");
                    sql = sb.ToString();

                    OracleCommand cmd = conn.CreateCommand();
                    cmd.CommandText = sql;

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                {
                    throw;
                }
            }

            return Constants.SQL_RESULT_OK;
        }

        /// <summary>
        /// 棚卸進捗状況の取得
        /// </summary>
        /// <param name="workKbn">作業区分</param>
        /// <returns>棚卸進捗状況</returns>
        public async Task<IEnumerable<TanaorosiProgress>> GetTanaorosiProgress(string workKbn)
        {
            string sql = string.Empty;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    sql = $@"select OPERATION_NO,OPERATION_CONDITION from TANAOROSI_PROGRESS_MST WHERE SYSTEM_CATEGORY='1' AND WORK_KBN='{workKbn}'";

                    return (await conn.QueryAsync(sql))
                        .Select(result => new TanaorosiProgress()
                        {
                            OperationNo = result.OPERATION_NO,
                            OperationCondition = result.OPERATION_CONDITION
                            
                        });
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// 指定された内訳の向先が正しいかをチェックする
        /// </summary>
        /// <param name="himoku">費目</param>
        /// <param name="uchiwake">内訳</param>
        /// <param name="tanaban">棚番</param>
        /// <returns>正しい場合:true/正しくない場合:false</returns>
        public async Task<bool> CheckMukesaki(string himoku, string uchiwake, string tanaban)
        {
            string sql = string.Empty;
            string sqlParam = string.Empty;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    sql = @"select 1 from SIZAI_HINMOKU_MST m
                                  inner join SIZAI_HOUKOKU_TRN t on m.HIMOKU = t.HIMOKU and m.UTIWAKE = t.UTIWAKE and m.TANABAN = t.TANABAN
                                  where t.HIMOKU = :HIMOKU and t.UTIWAKE = :UTIWAKE and t.TANABAN = :TANABAN
                                  and NOT ((m.MUKESAKI = '1' and t.SLF = 0 and t.SCC = 0) or (m.MUKESAKI = '2' and t.SEF = 0 and t.SCC = 0) or (m.MUKESAKI = '3' and t.SEF = 0 and t.SLF = 0))";
                    var param = new { HIMOKU = himoku, UTIWAKE = uchiwake, TANABAN = tanaban };
                    sqlParam = param.ToString();

                    return (await conn.QueryAsync(sql, param)).Any();
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql, sqlParam);
                }
                else
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// 品目マスタ存在チェック
        /// </summary>
        /// <returns>品目CD一覧</returns>
        public async Task<IEnumerable<Tuple<string, string>>> CheckHinmokuMaster()
        {
            var notHinmokuList = new List<System.Tuple<string, string>>();
            string sql = string.Empty;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    sql = $@"select hinmokucd, gyosyacd from sizai_tanaorosi_mst t where not exists (select * from sizai_hinmoku_mst h where t.HINMOKUCD = h.HINMOKUCD)";

                    OracleCommand command = new OracleCommand(sql, conn);

                    OracleDataReader reader;
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        notHinmokuList.Add(Tuple.Create<string, string>(reader.GetString(0), reader.GetString(1)));
                    }
                }
            }

            catch (Exception ex)
            {
                if (ex is OracleException)
                {
                    OracleException exception = ex as OracleException;
                    throw createTanaorosiLogException(exception.Message, (exception.Number).ToString("X5"), sql);
                }
                else
                {
                    throw;
                }
            }

            return notHinmokuList;
        }

        /// <summary>
        /// タイムスタンプの取得
        /// </summary>
        /// <returns>タイムスタンプ</returns>
        private string GetTimeStamp()
        {
            string ret = null;
            DateTime timeStamp = new DateTime(0);

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    conn.OpenAsync();

                    string sql = $@"SELECT SYSDATE FROM DUAL";

                    OracleCommand command = new OracleCommand(sql, conn);

                    OracleDataReader reader;
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        timeStamp = (DateTime)reader[0];
                    }
                }

                ret = timeStamp.ToString();
            }

            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// LogExceptionを生成する
        /// </summary>
        /// <param name="errorContent">エラー内容</param>
        /// <param name="errorCode">エラーコード</param>
        /// <param name="sql">SQL</param>
        /// <param name="sqlParam">SQLパラメータ</param>
        private TanaorosiLogException createTanaorosiLogException(string errorContent,string errorCode, string sql, string sqlParam = "")
        {
            TanaorosiLogException logException = new TanaorosiLogException(errorContent);

            try
            {
                logException.TimeStamp = GetTimeStamp();
                logException.ErrorCode = errorCode;
                logException.SQL = sql;
                logException.SqlParam = sqlParam;
            }

            catch (Exception)
            {
                throw;
            }

            return logException;
        }

        /// <summary>
        /// 棚卸ログの挿入
        /// </summary>
        /// <param name="tanaorosiLog">棚卸ログ</param>
        /// <returns>結果</returns>
        public async Task<int> InsertTanaorosiLog(TanaorosiLog tanaorosiLog)
        {
            int ret = 0;
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();
                    var trans = conn.BeginTransaction();
                    string insertSql = @"INSERT INTO
                                        TANAOROSI_LOG_TRN
                                      ( SYSTEM_CATEGORY
                                      , LOG_TYPE
                                      , SYAINCD
                                      , OPERATION_YMD
                                      , WORK_KBN
                                      , OPERATION_TYPE
                                      , OPERATION_CONTENT
                                      , BIKOU
                                      , ERROR_CONTENT
                                      , ERROR_CODE
                                      )
                                  VALUES
                                      ( :systemCategory,
                                      :logType,
                                      :syainCode,"
                                      + "TO_CHAR(SYSDATE, 'yyyymmddhh24miss'), "
                                      + ":workKbn,"
                                      + ":operationType,"
                                      + ":operationContent,"
                                      + ":bikou,"
                                      + ":errorContent,"
                                      + ":errorCode)";
                    var param = new
                    {
                        systemCategory = tanaorosiLog.SystemCategory,
                        logType = tanaorosiLog.LogType,
                        syainCode = tanaorosiLog.SyainCode,
                        workKbn = tanaorosiLog.WorkKbn,
                        operationType = tanaorosiLog.OperationType,
                        operationContent = tanaorosiLog.OperationContent,
                        bikou = tanaorosiLog.Bikou,
                        errorContent = tanaorosiLog.ErrorContent,
                        errorCode = tanaorosiLog.ErrorCode,
                    };
                    await conn.ExecuteAsync(insertSql, param);
                    trans.Commit();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }

        /// <summary>
        /// 棚卸詳細ログの挿入
        /// </summary>
        /// <param name="tanaorosiDetailLog">棚卸詳細ログ</param>
        /// <returns>結果</returns>
        public async Task<int> InsertTanaorosiDetailLog(TanaorosiDetailLog tanaorosiDetailLog)
        {
            int ret = 0;
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();
                    var trans = conn.BeginTransaction();
                    string insertSql = @"INSERT INTO
                                        TANAOROSI_DETAIL_LOG_TRN
                                      ( SYSTEM_CATEGORY
                                      , LOG_TYPE
                                      , SYAINCD
                                      , OPERATION_YMD
                                      , WORK_KBN
                                      , OPERATION_TYPE
                                      , OPERATION_CONTENT
                                      , SIZAI_KBN
                                      , HINMOKUCD
                                      , GYOSYACD
                                      , HINMOKUNM
                                      , GYOSYANM
                                      , BIKOU
                                      , UPDATE_CONTENT1
                                      , UPDATE_CONTENT2
                                      , UPDATE_CONTENT3
                                      , ERROR_CONTENT
                                      )
                                  VALUES
                                      ( :systemCategory,
                                        :logType,
                                        :syainCode,"
                                      + "TO_CHAR(SYSDATE, 'yyyymmddhh24miss'), "
                                      + ":workKbn,"
                                      + ":operationType,"
                                      + ":operationContent,"
                                      + ":sizaiKbn,"
                                      + ":hinmokuCode,"
                                      + ":gyosyaCode,"
                                      + ":hinmokuNM,"
                                      + ":gyosyaNM,"
                                      + ":bikou,"
                                      + ":updateContent1,"
                                      + ":updateContent2,"
                                      + ":updateContent3,"
                                      + ":errorContent)";
                    var param = new
                    {
                        systemCategory = tanaorosiDetailLog.SystemCategory,
                        logType = tanaorosiDetailLog.LogType,
                        syainCode = tanaorosiDetailLog.SyainCode,
                        workKbn = tanaorosiDetailLog.WorkKbn,
                        operationType = tanaorosiDetailLog.OperationType,
                        operationContent = tanaorosiDetailLog.OperationContent,
                        sizaiKbn = tanaorosiDetailLog.SizaiKbn,
                        hinmokuCode = tanaorosiDetailLog.HinmokuCode,
                        gyosyaCode = tanaorosiDetailLog.GyosyaCode,
                        hinmokuNM = tanaorosiDetailLog.HinmokuNM,
                        gyosyaNM = tanaorosiDetailLog.GyosyaNM,
                        bikou = tanaorosiDetailLog.Bikou,
                        updateContent1 = tanaorosiDetailLog.UpdateContent1,
                        updateContent2 = tanaorosiDetailLog.UpdateContent2,
                        updateContent3 = tanaorosiDetailLog.UpdateContent3,
                        errorContent = tanaorosiDetailLog.ErrorContent
                    };
                    await conn.ExecuteAsync(insertSql, param);
                    trans.Commit();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }

        /// <summary>
        /// 棚卸実施年月における資材品目(マスタ)の変更データの取得
        /// </summary>
        /// <param name="tanaYm">棚卸実施年月</param>
        /// <returns>資材品目(マスタ)の変更データ</returns>
        public async Task<IEnumerable<SizaiHinmokuItem>> GetSizaiHinmokuChangeData(string tanaYm)
        {
            try
            {
                // 当月開始日
                string thisMonthStart = tanaYm.Substring(0,4) + "/" + tanaYm.Substring(4, 2) + "/" + "01";
                //// 当月開始日をDateTime型に変換
                //DateTime date = DateTime.Parse(Convert.ToString(thisMonthStart));
                //// 当月の終了日を取得する
                //int thisMonthLastDay = DateTime.DaysInMonth(date.Year, date.Month);
                //// 当月終了日
                //string thisMonthEnd = tanaYm.Substring(0, 4) + "/" + tanaYm.Substring(4, 2) + "/" 
                //                        + Convert.ToString(thisMonthLastDay);

                using (var conn = new OracleConnection(connectionString))
                {
                    string sql = $@"select h.HINMOKUCD as HINMOKUCD,
                                        trim(trim('　' from h.HINMOKUNM)) as HINMOKUNM,
                                        h.HIMOKU as HIMOKU, 
                                        h.UTIWAKE as UTIWAKE, 
                                        h.TANABAN as TANABAN,
                                        h.TANI as TANI, 
                                        h.SYUBETU as SYUBETU, 
                                        h.SUIBUNKBN as SUIBUNKBN, 
                                        h.KENSYUKBN as KENSYUKBN,
                                        h.HOUKOKUKBN as HOUKOKUKBN,
                                        h.ICHIKBN as ICHIKBN,
                                        h.MUKESAKI as MUKESAKI,
                                        case 
                                          when not exists (select t.HINMOKUCD from SIZAI_TANKA_MST t where t.HINMOKUCD= h.HINMOKUCD) then ' ' else '○' end  as TANKASETTE,
                                        h.UPDYMD as UPDYMD
                                          from SIZAI_HINMOKU_MST h
                                            where h.UPDYMD >= '{thisMonthStart}' 
                                              order by UPDYMD desc, HIMOKU, UTIWAKE, TANABAN, HINMOKUCD";

                    await conn.OpenAsync();

                    return (await conn.QueryAsync(sql))
                                .Select(result => new SizaiHinmokuItem()
                                {
                                    HinmokuCode = result.HINMOKUCD,
                                    HinmokuName = result.HINMOKUNM,
                                    Himoku = result.HIMOKU,
                                    Utiwake = result.UTIWAKE,
                                    Tanaban = result.TANABAN,
                                    Tani = result.TANI,
                                    UkebaraiType = result.SYUBETU,
                                    SuibunKbn = result.SUIBUNKBN,
                                    KensyuKbn = result.KENSYUKBN,
                                    HoukokuKbn = result.HOUKOKUKBN,
                                    IchiKbn = result.ICHIKBN,
                                    Mukesaki = result.MUKESAKI,
                                    TankaSetting = result.TANKASETTE,
                                    UpdYMD = result.UPDYMD
                                });
                }

            }

            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 受払い種別の取得
        /// </summary>
        /// <param name="hinmokuCode">品目コード</param>
        /// <returns>受払い種別</returns>
        public async Task<UkebaraiType> GetUkebaraiSyubetuAsync(string hinmokuCode)
        {
            UkebaraiType ret = default(UkebaraiType);

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string sql = $@"select SYUBETU from SIZAI_HINMOKU_MST where HINMOKUCD = '{hinmokuCode}'";

                    OracleCommand command = new OracleCommand(sql, conn);

                    var tmp = command.ExecuteScalar();

                    if (null != tmp)
                    {
                        if (tmp.ToString() == UkebaraiType.SiyoudakaBarai.GetMeanValue())
                            ret = UkebaraiType.SiyoudakaBarai;
                        else if (tmp.ToString() == UkebaraiType.NyukoBarai.GetMeanValue())
                            ret = UkebaraiType.NyukoBarai;
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 受払い種別の取得
        /// </summary>
        /// <param name="hinmokuCode">品目コード</param>
        /// <returns>受払い種別</returns>
        public UkebaraiType GetUkebaraiSyubetu(string hinmokuCode)
        {
            UkebaraiType ret = default(UkebaraiType);

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    string sql = $@"select SYUBETU from SIZAI_HINMOKU_MST where HINMOKUCD = '{hinmokuCode}'";

                    OracleCommand command = new OracleCommand(sql, conn);

                    var tmp = command.ExecuteScalar();

                    if (null != tmp)
                    {
                        if (tmp.ToString() == UkebaraiType.SiyoudakaBarai.GetMeanValue())
                            ret = UkebaraiType.SiyoudakaBarai;
                        else if (tmp.ToString() == UkebaraiType.NyukoBarai.GetMeanValue())
                            ret = UkebaraiType.NyukoBarai;
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        #endregion
    }
}
