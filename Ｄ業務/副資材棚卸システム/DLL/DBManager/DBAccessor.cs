using System;
using System.Collections.Generic;
using DBManager.Constants;
using DBManager.Model;
using DBManager.Condition;
//*************************************************************************************
//
//   DB操作の制御クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.11.16              DSK   　 　　新規作成
//
//*************************************************************************************

namespace DBManager
{
    /// <summary>
    /// DB操作の制御
    /// </summary>
    public class DBAccessor
    {
        #region フィールド
        /// <summary>
        /// DB操作用オブジェクト
        /// </summary>
        private IDataSource dataSource = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">DB種類</param>
        public DBAccessor(DBConstants.DBType type)
        {
            this.dataSource = CreateDataSource(type);
        }
        #endregion

        #region ファクトリー
        /// <summary>
        /// DB操作用オブジェクトを生成する
        /// </summary>
        /// <param name="type">DB種類</param>
        /// <returns>DB操作用オブジェクト</returns>
        private IDataSource CreateDataSource(DBConstants.DBType type)
        {
            IDataSource dataSource = null;

            switch (type)
            {
                case DBConstants.DBType.ORACLE:
                    dataSource = new OracleDataSource();
                    break;
                default:
                    break;
            }
            return dataSource;
        }
        #endregion

        #region メソッド
        /// <summary>
        /// 棚卸システム分類の取得
        /// </summary>
        /// <returns>棚卸システム分類</returns>
        public IList<TanaorosiSystemCategory> SelectTanaorosiSystemCategory()
        {
            IList<TanaorosiSystemCategory> ret = null;

            try
            {
                ret = this.dataSource.SelectTanaorosiSystemCategory();
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸システム分類の登録
        /// </summary>
        /// <param name="tanaorosiSystemCategory">棚卸システム分類</param>
        /// <returns>登録結果</returns>
        public int InsertTanaorosiSystemCategory(IList<TanaorosiSystemCategory> tanaorosiSystemCategory)
        {
            int ret = DBConstants.SQL_RESULT_NG;

            try
            {
                ret = this.dataSource.InsertTanaorosiSystemCategory(tanaorosiSystemCategory);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸システム分類の更新
        /// </summary>
        /// <param name="tanaorosiSystemCategory">棚卸システム分類</param>
        /// <returns>更新結果</returns>
        public int UpdateTanaorosiSystemCategory(IList<TanaorosiSystemCategory> tanaorosiSystemCategory)
        {
            int ret = DBConstants.SQL_RESULT_NG;

            try
            {
                ret = this.dataSource.UpdateTanaorosiSystemCategory(tanaorosiSystemCategory);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸システム分類の削除
        /// </summary>
        /// <param name="tanaorosiSystemCategory">棚卸システム分類</param>
        /// <returns>削除結果</returns>
        public int DeleteTanaorosiSystemCategory(IList<TanaorosiSystemCategory> tanaorosiSystemCategory)
        {
            int ret = DBConstants.SQL_RESULT_NG;

            try
            {
                ret = this.dataSource.DeleteTanaorosiSystemCategory(tanaorosiSystemCategory);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸グループ区分の取得
        /// </summary>
        /// <returns>棚卸グループ区分</returns>
        public IList<TanaorosiGroupKbn> SelectTanaorosiGroupKbn()
        {
            IList<TanaorosiGroupKbn> ret = null;

            try
            {
                ret = this.dataSource.SelectTanaorosiGroupKbn();
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸グループ区分の登録
        /// </summary>
        /// <param name="tanaorosiGroupKbn">棚卸グループ区分</param>
        /// <returns>登録結果</returns>
        public int InsertTanaorosiGroupKbn(IList<TanaorosiGroupKbn> tanaorosiGroupKbn)
        {
            int ret = DBConstants.SQL_RESULT_NG;

            try
            {
                ret = this.dataSource.InsertTanaorosiGroupKbn(tanaorosiGroupKbn);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸グループ区分の更新
        /// </summary>
        /// <param name="tanaorosiGroupKbn">棚卸グループ区分</param>
        /// <returns>更新結果</returns>
        public int UpdateTanaorosiGroupKbn(IList<TanaorosiGroupKbn> tanaorosiGroupKbn)
        {
            int ret = DBConstants.SQL_RESULT_NG;

            try
            {
                ret = this.dataSource.UpdateTanaorosiGroupKbn(tanaorosiGroupKbn);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸グループ区分の削除
        /// </summary>
        /// <param name="tanaorosiGroupKbn">棚卸グループ区分</param>
        /// <returns>削除結果</returns>
        public int DeleteTanaorosiGroupKbn(IList<TanaorosiGroupKbn> tanaorosiGroupKbn)
        {
            int ret = DBConstants.SQL_RESULT_NG;

            try
            {
                ret = this.dataSource.DeleteTanaorosiGroupKbn(tanaorosiGroupKbn);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸操作の取得
        /// </summary>
        /// <returns>棚卸操作</returns>
        public IList<TanaorosiOperation> SelectTanaorosiOperation()
        {
            IList<TanaorosiOperation> ret = null;

            try
            {
                ret = this.dataSource.SelectTanaorosiOperation();
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸操作の登録
        /// </summary>
        /// <param name="tanaorosiOperation">棚卸操作</param>
        /// <returns>登録結果</returns>
        public int InsertTanaorosiOperation(IList<TanaorosiOperation> tanaorosiOperation)
        {
            int ret = DBConstants.SQL_RESULT_NG;

            try
            {
                ret = this.dataSource.InsertTanaorosiOperation(tanaorosiOperation);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸操作の更新
        /// </summary>
        /// <param name="tanaorosiOperation">棚卸操作</param>
        /// <returns>更新結果</returns>
        public int UpdateTanaorosiOperation(IList<TanaorosiOperation> tanaorosiOperation)
        {
            int ret = DBConstants.SQL_RESULT_NG;

            try
            {
                ret = this.dataSource.UpdateTanaorosiOperation(tanaorosiOperation);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸操作の削除
        /// </summary>
        /// <param name="tanaorosiOperation">棚卸操作</param>
        /// <returns>削除結果</returns>
        public int DeleteTanaorosiOperation(IList<TanaorosiOperation> tanaorosiOperation)
        {
            int ret = DBConstants.SQL_RESULT_NG;

            try
            {
                ret = this.dataSource.DeleteTanaorosiOperation(tanaorosiOperation);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸操作グループの取得
        /// </summary>
        /// <returns>棚卸操作グループ</returns>
        public IList<TanaorosiOpeartionGroup> SelectTanaorosiOpeartionGroup()
        {
            IList<TanaorosiOpeartionGroup> ret = null;

            try
            {
                ret = this.dataSource.SelectTanaorosiOpeartionGroup();
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸操作グループの登録
        /// </summary>
        /// <param name="tanaorosiOpeartionGroup">棚卸操作グループ</param>
        /// <returns>登録結果</returns>
        public int InsertTanaorosiOpeartionGroup(IList<TanaorosiOpeartionGroup> tanaorosiOpeartionGroup)
        {
            int ret = DBConstants.SQL_RESULT_NG;

            try
            {
                ret = this.dataSource.InsertTanaorosiOpeartionGroup(tanaorosiOpeartionGroup);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸操作グループの更新
        /// </summary>
        /// <param name="tanaorosiOpeartionGroup">棚卸操作グループ</param>
        /// <returns>更新結果</returns>
        public int UpdateTanaorosiOpeartionGroup(IList<TanaorosiOpeartionGroup> tanaorosiOpeartionGroup)
        {
            int ret = DBConstants.SQL_RESULT_NG;

            try
            {
                ret = this.dataSource.UpdateTanaorosiOpeartionGroup(tanaorosiOpeartionGroup);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸操作グループの削除
        /// </summary>
        /// <param name="tanaorosiOperationMenu">棚卸操作グループ</param>
        /// <returns>削除結果</returns>
        public int DeleteTanaorosiOpeartionGroup(IList<TanaorosiOpeartionGroup> tanaorosiOpeartionGroup)
        {
            int ret = DBConstants.SQL_RESULT_NG;

            try
            {
                ret = this.dataSource.DeleteTanaorosiOpeartionGroup(tanaorosiOpeartionGroup);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸操作メニューの取得
        /// </summary>
        /// <returns>棚卸操作メニュー</returns>
        public IList<TanaorosiOperationMenu> SelectTanaorosiOperationMenu()
        {
            IList<TanaorosiOperationMenu> ret = null;

            try
            {
                ret = this.dataSource.SelectTanaorosiOperationMenu();
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸操作メニューの重複チェック
        /// </summary>
        /// <param name="tanaorosiOperationMenu">棚卸操作メニュー</param>
        /// <returns>重複有無</returns>
        public bool IsOverlapCheckTanaorosiOperationMenu(IList<TanaorosiOperationMenu> tanaorosiOperationMenu)
        {
            bool ret = false;

            try
            {
                ret = this.dataSource.IsOverlapCheckTanaorosiOperationMenu(tanaorosiOperationMenu);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸操作メニューの登録
        /// </summary>
        /// <param name="tanaorosiOperationMenu">棚卸操作メニュー</param>
        /// <returns>登録結果</returns>
        public int InsertTanaorosiOperationMenu(IList<TanaorosiOperationMenu> tanaorosiOperationMenu)
        {
            int ret = DBConstants.SQL_RESULT_NG;

            try
            {
                ret = this.dataSource.InsertTanaorosiOperationMenu(tanaorosiOperationMenu);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸操作メニューの更新
        /// </summary>
        /// <param name="tanaorosiOperationMenu">棚卸操作メニュー</param>
        /// <returns>更新結果</returns>
        public int UpdateTanaorosiOperationMenu(IList<TanaorosiOperationMenu> tanaorosiOperationMenu)
        {
            int ret = DBConstants.SQL_RESULT_NG;

            try
            {
                ret = this.dataSource.UpdateTanaorosiOperationMenu(tanaorosiOperationMenu);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸操作メニューの削除
        /// </summary>
        /// <param name="tanaorosiOperationMenu">棚卸操作メニュー</param>
        /// <returns>削除結果</returns>
        public int DeleteTanaorosiOperationMenu(IList<TanaorosiOperationMenu> tanaorosiOperationMenu)
        {
            int ret = DBConstants.SQL_RESULT_NG;

            try
            {
                ret = this.dataSource.DeleteTanaorosiOperationMenu(tanaorosiOperationMenu);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸ログトランの取得
        /// </summary>
        /// <returns>棚卸ログトラン</returns>
        public IList<TanaorosiLogTRN> SelectTanaorosiLogTRN(LogSearchCondition searchCondition)
        {
            IList<TanaorosiLogTRN> ret = null;

            try
            {
                ret = this.dataSource.SelectTanaorosiLogTRN(searchCondition);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 棚卸詳細ログトランの取得
        /// </summary>
        /// <returns>棚卸ログトラン</returns>
        public IList<TanaorosiDetailLogTRN> SelectTanaorosiDetailLogTRN(LogSearchCondition searchCondition)
        {
            IList<TanaorosiDetailLogTRN> ret = null;

            try
            {
                ret = this.dataSource.SelectTanaorosiDetailLogTRN(searchCondition);
            }
            catch (Exception)
            {
                throw;
            }

            return ret;
        }

        /// <summary>
        /// ユーザ情報の取得
        /// </summary>
        /// <returns>ユーザ情報</returns>
        public IDictionary<string, EmployeeInfo> SelectUsersInfo()
        {
            IDictionary<string, EmployeeInfo> result = null;

            try
            {
                result = this.dataSource.SelectUsersInfo();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 部署名の取得
        /// </summary>
        /// <returns>部署名</returns>
        public IDictionary<string, string> SelectDeploymentName()
        {
            IDictionary<string, string> result = null;

            try
            {
                result = this.dataSource.SelectDeploymentName();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 棚卸システム分類の取得
        /// </summary>
        /// <returns>棚卸システム分類</returns>
        public IDictionary<string, string> SelectTanaorosiSystemCategorys()
        {
            IDictionary<string, string> result = null;

            try
            {
                result = this.dataSource.SelectTanaorosiSystemCategorys();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 棚卸グループ区分の取得
        /// </summary>
        /// <returns>棚卸グループ区分</returns>
        public IList<TanaorosiGroupKbn> SelectTanaorosiGroupKbns()
        {
            IList<TanaorosiGroupKbn> result = null;

            try
            {
                result = this.dataSource.SelectTanaorosiGroupKbns();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 棚卸グループ区分の取得
        /// </summary>
        /// <param name="systemCategory">システム分類</param>
        /// <returns>棚卸グループ区分</returns>
        public IDictionary<string, string> SelectTanaorosiGroupKbns(string systemCategory)
        {
            IDictionary<string, string> result = null;

            try
            {
                result = this.dataSource.SelectTanaorosiGroupKbns(systemCategory);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 操作コードの取得
        /// </summary>
        /// <returns>操作コード</returns>
        public IList<TanaorosiOperation> SelectTanaorosiOperationCodes()
        {
            IList<TanaorosiOperation> result = null;

            try
            {
                result = this.dataSource.SelectTanaorosiOperationCodes();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 操作コードの取得
        /// </summary>
        /// <param name="systemCategory">システム分類</param>
        /// <returns>操作コード</returns>
        public IDictionary<string, string> SelectTanaorosiOperationCodes(string systemCategory)
        {
            IDictionary<string, string> result = null;

            try
            {
                result = this.dataSource.SelectTanaorosiOperationCodes(systemCategory);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// </summary>
        /// <returns>棚卸操作ユーザ</returns>
        public IDictionary<string, string> SelectTanaorosiOpeationUsers()
        {
            IDictionary<string, string> result = null;

            try
            {
                result = this.dataSource.SelectTanaorosiOpeationUsers();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion
    }
}
