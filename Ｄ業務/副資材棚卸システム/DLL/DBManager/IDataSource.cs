using System;
using System.Collections.Generic;
using DBManager.Constants;
using DBManager.Model;
using DBManager.Condition;
//*************************************************************************************
//
//   DB操作クラス(基底)
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.11.16              DSK   　     新規作成
//
//*************************************************************************************
namespace DBManager
{
    /// <summary>
    /// DB操作(基底)
    /// </summary>
    internal interface IDataSource
    {
        #region メソッド
        /// <summary>
        /// DB接続
        /// </summary>
        void Connect();

        /// <summary>
        /// DB切断
        /// </summary>
        void Disconnect();

        /// <summary>
        /// トランザクション開始
        /// </summary>
        void BeginTransaction();

        // <summary>
        /// コミット
        /// </summary>
        void Commit();

        /// <summary>
        /// ロールバック
        /// </summary>
        void Rollback();

        /// <summary>
        /// 排他ロックの実行
        /// </summary>
        /// <param name="tableName">ロック対象テーブル名</param>
        void ExecuteLock(string tableName);

        /// <summary>
        /// 排他ロックのチェック
        /// </summary>
        /// <param name="tableName">ロック対象テーブル名</param>
        void CheckLock(string sTableName);

        /// <summary>
        /// 棚卸システム分類の取得
        /// </summary>
        /// <param name="employee">社員情報</param>
        /// <returns>棚卸システム分類</returns>
        IList<TanaorosiSystemCategory> SelectTanaorosiSystemCategory();

        /// <summary>
        /// 棚卸システム分類の登録
        /// </summary>
        /// <param name="tanaorosiSystemCategory">棚卸システム分類</param>
        /// <returns>登録結果</returns>
        int InsertTanaorosiSystemCategory(IList<TanaorosiSystemCategory> tanaorosiSystemCategory);

        /// <summary>
        /// 棚卸システム分類の更新
        /// </summary>
        /// <param name="tanaorosiSystemCategory">棚卸システム分類</param>
        /// <returns>更新結果</returns>
        int UpdateTanaorosiSystemCategory(IList<TanaorosiSystemCategory> tanaorosiSystemCategory);

        /// <summary>
        /// 棚卸システム分類の削除
        /// </summary>
        /// <param name="tanaorosiSystemCategory">棚卸システム分類</param>
        /// <returns>削除結果</returns>
        int DeleteTanaorosiSystemCategory(IList<TanaorosiSystemCategory> tanaorosiSystemCategory);

        /// <summary>
        /// 棚卸グループ区分の取得
        /// </summary>
        /// <returns>棚卸グループ区分</returns>
        IList<TanaorosiGroupKbn> SelectTanaorosiGroupKbn();

        /// <summary>
        /// 棚卸グループ区分の登録
        /// </summary>
        /// <param name="tanaorosiGroupKbn">棚卸グループ区分</param>
        /// <returns>登録結果</returns>
        int InsertTanaorosiGroupKbn(IList<TanaorosiGroupKbn> tanaorosiGroupKbn);

        /// <summary>
        /// 棚卸グループ区分の更新
        /// </summary>
        /// <param name="tanaorosiGroupKbn">棚卸グループ区分</param>
        /// <returns>更新結果</returns>
        int UpdateTanaorosiGroupKbn(IList<TanaorosiGroupKbn> tanaorosiGroupKbn);

        /// <summary>
        /// 棚卸グループ区分の削除
        /// </summary>
        /// <param name="tanaorosiGroupKbn">棚卸グループ区分</param>
        /// <returns>削除結果</returns>
        int DeleteTanaorosiGroupKbn(IList<TanaorosiGroupKbn> tanaorosiGroupKbn);

        /// <summary>
        /// 棚卸操作の取得
        /// </summary>
        /// <returns>棚卸操作</returns>
        IList<TanaorosiOperation> SelectTanaorosiOperation();

        /// <summary>
        /// 棚卸操作の登録
        /// </summary>
        /// <param name="tanaorosiOperation">棚卸操作</param>
        /// <returns>登録結果</returns>
        int InsertTanaorosiOperation(IList<TanaorosiOperation> tanaorosiOperation);

        /// <summary>
        /// 棚卸操作の更新
        /// </summary>
        /// <param name="tanaorosiOperation">棚卸操作</param>
        /// <returns>更新結果</returns>
        int UpdateTanaorosiOperation(IList<TanaorosiOperation> tanaorosiOperation);

        /// <summary>
        /// 棚卸操作の削除
        /// </summary>
        /// <param name="tanaorosiOperation">棚卸操作</param>
        /// <returns>削除結果</returns>
        int DeleteTanaorosiOperation(IList<TanaorosiOperation> tanaorosiOperation);

        /// <summary>
        /// 棚卸操作グループの取得
        /// </summary>
        /// <returns>棚卸操作グループ</returns>
        IList<TanaorosiOpeartionGroup> SelectTanaorosiOpeartionGroup();

        /// <summary>
        /// 棚卸操作グループの登録
        /// </summary>
        /// <param name="tanaorosiOpeartionGroup">棚卸操作グループ</param>
        /// <returns>登録結果</returns>
        int InsertTanaorosiOpeartionGroup(IList<TanaorosiOpeartionGroup> tanaorosiOpeartionGroup);

        /// <summary>
        /// 棚卸操作グループの更新
        /// </summary>
        /// <param name="tanaorosiOpeartionGroup">棚卸操作グループ</param>
        /// <returns>更新結果</returns>
        int UpdateTanaorosiOpeartionGroup(IList<TanaorosiOpeartionGroup> tanaorosiOpeartionGroup);

        /// <summary>
        /// 棚卸操作グループの削除
        /// </summary>
        /// <param name="tanaorosiOperationMenu">棚卸操作グループ</param>
        /// <returns>削除結果</returns>
        int DeleteTanaorosiOpeartionGroup(IList<TanaorosiOpeartionGroup> tanaorosiOpeartionGroup);

        /// <summary>
        /// 棚卸操作メニューの取得
        /// </summary>
        /// <returns>棚卸操作メニュー</returns>
        IList<TanaorosiOperationMenu> SelectTanaorosiOperationMenu();

        /// <summary>
        /// 棚卸操作メニューの重複チェック
        /// </summary>
        /// <param name="tanaorosiOperationMenu">棚卸操作メニュー</param>
        /// <returns>重複有無</returns>
        bool IsOverlapCheckTanaorosiOperationMenu(IList<TanaorosiOperationMenu> tanaorosiOperationMenu);

        /// <summary>
        /// 棚卸操作メニューの登録
        /// </summary>
        /// <param name="tanaorosiOperationMenu">棚卸操作メニュー</param>
        /// <returns>登録結果</returns>
        int InsertTanaorosiOperationMenu(IList<TanaorosiOperationMenu> tanaorosiOperationMenu);

        /// <summary>
        /// 棚卸操作メニューの更新
        /// </summary>
        /// <param name="tanaorosiOperationMenu">棚卸操作メニュー</param>
        /// <returns>更新結果</returns>
        int UpdateTanaorosiOperationMenu(IList<TanaorosiOperationMenu> tanaorosiOperationMenu);

        /// <summary>
        /// 棚卸操作メニューの削除
        /// </summary>
        /// <param name="tanaorosiOperationMenu">棚卸操作メニュー</param>
        /// <returns>削除結果</returns>
        int DeleteTanaorosiOperationMenu(IList<TanaorosiOperationMenu> tanaorosiOperationMenu);

        /// <summary>
        /// 棚卸ログトランの取得
        /// </summary>
        /// <returns>棚卸ログトラン</returns>
        IList<TanaorosiLogTRN> SelectTanaorosiLogTRN(LogSearchCondition searchCondition);

        /// <summary>
        /// 棚卸詳細ログトランの取得
        /// </summary>
        /// <returns>棚卸ログトラン</returns>
        IList<TanaorosiDetailLogTRN> SelectTanaorosiDetailLogTRN(LogSearchCondition searchCondition);

        /// <summary>
        /// ユーザの取得
        /// </summary>
        /// <returns>ユーザ</returns>
        IDictionary<string, EmployeeInfo> SelectUsersInfo();

        /// <summary>
        /// 部署名の取得
        /// </summary>
        /// <returns>部署名</returns>
        IDictionary<string, string> SelectDeploymentName();

        /// <summary>
        /// 棚卸システム分類の取得
        /// </summary>
        /// <returns>棚卸システム分類</returns>
        IDictionary<string, string> SelectTanaorosiSystemCategorys();

        /// <summary>
        /// 棚卸グループ区分の取得
        /// </summary>
        /// <returns>棚卸グループ区分</returns>
        IList<TanaorosiGroupKbn> SelectTanaorosiGroupKbns();

        /// <summary>
        /// 棚卸グループ区分の取得
        /// </summary>
        /// <param name="systemCategory">システム分類</param>
        /// <returns>棚卸グループ区分</returns>
        IDictionary<string, string> SelectTanaorosiGroupKbns(string systemCategory);

        /// <summary>
        /// 操作コードの取得
        /// </summary>
        /// <returns>操作コード</returns>
        IList<TanaorosiOperation> SelectTanaorosiOperationCodes();

        /// <summary>
        /// 操作コードの取得
        /// </summary>
        /// <param name="systemCategory">システム分類</param>
        /// <returns>操作コード</returns>
        IDictionary<string, string> SelectTanaorosiOperationCodes(string systemCategory);

        /// <summary>
        /// 棚卸操作ユーザの取得
        /// </summary>
        /// <returns>棚卸操作ユーザ</returns>
        IDictionary<string, string> SelectTanaorosiOpeationUsers();

        #endregion
    }
}