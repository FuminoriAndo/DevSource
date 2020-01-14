using CKSI1090.Common;
using CKSI1090.Core;
using CKSI1090.Properties;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//*************************************************************************************
//
//   データーサービスの実装
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1090.Shared.Model
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
        /// 棚卸操作グループマスタに存在する資材ユーザであるかどうかをチェックする
        /// </summary>
        /// <param name="employeeCode">社員コード</param>
        /// <returns>結果</returns>
        public async Task<bool> CheckUserInTanaorosiOperationGroupMst(string employeeCode, string belongingCode)
        {
            bool ret = false;
            int count = 0;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    StringBuilder sql = new StringBuilder();
                    sql.Append($@"select Count(*) from TANAOROSI_OPERATION_GROUP_MST ");
                    sql.Append($@"where SYAINCD = '{employeeCode}'");
                    sql.Append($@" AND SYSTEM_CATEGORY = '{Constants.SystemCategory.Sizai.GetStringValue()}'");
                    string searchSQL = sql.ToString();

                    OracleCommand command = new OracleCommand(searchSQL, conn);

                    OracleDataReader reader;
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        count = int.Parse((reader[0]).ToString());
                    }
                }

                if (count >= 1)
                    ret = true;

            }

            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 作業誌一覧の取得
        /// </summary>
        /// <param name="SearchCondition">検索条件</param>
        /// <returns>作業誌一覧</returns>
        public async Task<IEnumerable<WorkNoteItem>> GetWorkNoteList(SearchCondition condition)
        {
            try
            {
                bool isAnyParam = false;

                StringBuilder sql = new StringBuilder();
                sql.Append($@"select SAGYOBI, SEQ, KUBUN, HINMOKUCD, GYOSYACD, MUKESAKI, ");
                sql.Append($@"HINMOKUNM, GYOSYANM, SURYO, SUIBUNRITU, APPROVAL_FLG");
                sql.Append($@" from SIZAI_SAGYOSI_TRN");
                sql.Append($@" where");

                // 作業日
                if (condition.UseOperationDate)
                {
                    switch (condition.OperationDateSearchType)
                    {
                        case Constants.OperationDateSearchTypes.Min:
                            sql.Append($@" SAGYOBI >= '{20 + condition.MinOperationDate}'");
                            break;
                        case Constants.OperationDateSearchTypes.Max:
                            sql.Append($@" SAGYOBI <= '{20 + condition.MaxOperationDate}'");
                            break;
                        case Constants.OperationDateSearchTypes.MinMax:
                            sql.Append($@" SAGYOBI BETWEEN '{ 20 + condition.MinOperationDate}'");
                            sql.Append($@" AND '{ 20 + condition.MaxOperationDate}'");
                            break;
                        default:
                            break;
                    }
                    isAnyParam = true;
                }


                // 作業誌区分
                if (condition.UseWorkNoteKbn)
                {
                    if(isAnyParam) sql.Append($@" AND");
                    sql.Append($@" KUBUN = '{condition.WorkNoteKbn}'");
                    isAnyParam = true;
                }

                // 品目コード
                if (condition.UseHinmokuCode)
                {
                    if (isAnyParam) sql.Append($@" AND");
                    switch (condition.HinmokuCodeSearchType)
                    {
                        case Constants.HinmokuCodeSearchTypes.Min:
                            sql.Append($@" HINMOKUCD >= '{condition.MinHinmokuCode}'");
                            break;
                        case Constants.HinmokuCodeSearchTypes.Max:
                            sql.Append($@" HINMOKUCD <= '{condition.MaxHinmokuCode}'");
                            break;
                        case Constants.HinmokuCodeSearchTypes.MinMax:
                            sql.Append($@" HINMOKUCD BETWEEN '{condition.MinHinmokuCode}'");
                            sql.Append($@" AND '{condition.MaxHinmokuCode}'");
                            break;
                        default:
                            break;
                    }
                    isAnyParam = true;
                }

                // 業者コード
                if (condition.UseGyosyaCode)
                {
                    if (isAnyParam) sql.Append($@" AND");
                    switch (condition.GyosyaCodeSearchType)
                    {
                        case Constants.GyosyaCodeSearchTypes.Min:
                            sql.Append($@" GYOSYACD >= '{condition.MinGyosyaCode}'");
                            break;
                        case Constants.GyosyaCodeSearchTypes.Max:
                            sql.Append($@" GYOSYACD <= '{condition.MaxGyosyaCode}'");
                            break;
                        case Constants.GyosyaCodeSearchTypes.MinMax:
                            sql.Append($@" GYOSYACD BETWEEN '{condition.MinGyosyaCode}'");
                            sql.Append($@" AND '{condition.MaxGyosyaCode}'");
                            break;
                        case Constants.GyosyaCodeSearchTypes.Blank:
                            sql.Append($@" GYOSYACD = '{condition.MinGyosyaCode}'");
                            break;
                        default:
                            break;
                    }
                    isAnyParam = true;
                }

                // 向先
                if (condition.UseMukesaki)
                {
                    if (isAnyParam) sql.Append($@" AND");
                    sql.Append($@" MUKESAKI = '{condition.Mukesaki}'");
                    isAnyParam = true;
                }

                // 承認
                if (condition.UseApproval)
                {
                    if (isAnyParam) sql.Append($@" AND");
                    string approval = condition.Approval ? "1" : "0";
                    sql.Append($@" APPROVAL_FLG = '{approval}'");
                    isAnyParam = true;
                }

                sql.Append($@" order by SAGYOBI asc, SEQ asc");

                string searchSQL = sql.ToString();

                using (var conn = new OracleConnection(connectionString))
                {

                    await conn.OpenAsync();

                    return (await conn.QueryAsync(searchSQL))
                            .Select(result => new WorkNoteItem()
                            {
                                OperationDate = result.SAGYOBI,
                                Seq = result.SEQ,
                                WorkNoteType = result.KUBUN,
                                HinmokuCode = result.HINMOKUCD,
                                GyosyaCode = result.GYOSYACD,
                                Mukesaki = result.MUKESAKI,
                                HinmokuName = result.HINMOKUNM,
                                GyosyaName = result.GYOSYANM,
                                Amount = result.SURYO,
                                Suibunritu = result.SUIBUNRITU,
                                Approval = result.APPROVAL_FLG == "1" ? true : false
                            });
                }
            }

            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 作業誌一覧の更新
        /// </summary>
        /// <param name="models">作業誌項目のモデル/param>
        /// <returns>結果</returns>
        public async Task<int> UpdateWorkNoteList(IList<WorkNoteItem> models)
        {
            string operationDate = string.Empty;
            string seq = string.Empty;
            string approval = string.Empty;
            StringBuilder sqlBuilder = new StringBuilder();
            string updateSql = string.Empty;
            OracleCommand updateCommand = null;

            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();

                    var trans = conn.BeginTransaction();

                    foreach (var model in models)
                    {
                        operationDate = model.OperationDate;
                        seq = model.Seq;
                        approval = model.Approval ? "1" : "0";
                        sqlBuilder.Clear();
                        sqlBuilder.Append($@"update SIZAI_SAGYOSI_TRN set APPROVAL_FLG = '{approval}', UPDYMD = SYSDATE ");
                        sqlBuilder.Append($@"where SAGYOBI = '{operationDate}' and Seq = '{seq}'");
                        updateSql = sqlBuilder.ToString();
                        updateCommand = new OracleCommand(updateSql, conn);
                        await updateCommand.ExecuteNonQueryAsync();
                    }

                    trans.Commit();
                }
            }

            catch (Exception)
            {
                throw;
            }

            return await Task.FromResult(Constants.SQL_RESULT_OK);
        }

        #endregion
    }
}
