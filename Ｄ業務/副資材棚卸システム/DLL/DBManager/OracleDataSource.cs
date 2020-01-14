using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using DBManager.Constants;
using DBManager.Model;
using Oracle.DataAccess.Client;
using DBManager.Condition;
//*************************************************************************************
//
//   DB操作クラス(Oracle)
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
    /// DB操作(Oracle)
    /// </summary>
    internal class OracleDataSource : IDataSource, IDisposable
    {
        #region フィールド

        private int KEY_ALREADY_EXISTS = 0x0000001;

        /// <summary>
        /// コネクションオブジェクト
        /// </summary>
        private OracleConnection connection = null;

        /// <summary>
        /// トランザクションオブジェクト
        /// </summary>
        private OracleTransaction trnsaction = null;

        /// <summary>
        ///  Oracle DB接続情報オブジェクト
        /// </summary>
        private ConnectInformation information = null;

        /// <summary>
        /// トランザクションが開始されているかを判定するフラグ
        /// </summary>
        private bool beginTransaction = false;
        #endregion

        #region 内部クラス
        /// <summary>
        /// Oracle DB接続情報
        /// </summary>
        private class ConnectInformation
        {
            /// <summary>
            /// データソース
            /// </summary>
            internal string DataSource { get; set; }
            /// <summary>
            /// ユーザー名
            /// </summary>
            internal string UserName { get; set; }
            /// <summary>
            /// パスワード
            /// </summary>
            internal string Password { get; set; }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ        
        /// </summary>
        public OracleDataSource()
        {
            string xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DBManager.xml");
            XDocument xdoc = XDocument.Load(xmlPath);
            this.information = new ConnectInformation();
            this.information.DataSource = xdoc.XPathSelectElement("//oracle/setting[@name='DataSource']").Attribute("value").Value;
            this.information.UserName = xdoc.XPathSelectElement("//oracle/setting[@name='UserName']").Attribute("value").Value;
            this.information.Password = xdoc.XPathSelectElement("//oracle/setting[@name='Password']").Attribute("value").Value;
        }
        #endregion

        #region メソッド
        /// <summary>
        /// DB接続
        /// </summary>
        public void Connect()
        {
            try
            {
                if (this.connection == null)
                {
                    StringBuilder sbConn = new StringBuilder();
                    // ユーザー名
                    sbConn.Append("User Id=");
                    sbConn.Append(this.information.UserName);

                    // パスワード
                    sbConn.Append(";");
                    sbConn.Append("Password=");
                    sbConn.Append(this.information.Password);

                    // データソース
                    sbConn.Append(";");
                    sbConn.Append("Data Source=");
                    sbConn.Append(this.information.DataSource);

                    // コネクションオブジェクト作成
                    this.connection = new OracleConnection(sbConn.ToString());
                }

                this.connection.Open();
            }

            catch (OracleException)
            {
                throw;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// DB切断
        /// </summary>
        public void Disconnect()
        {
            this.Rollback();

            try
            {
                if (this.connection != null)
                {
                    if (IsBeginTransaction())
                    {
                        this.Rollback();
                    }

                    this.connection.Close();
                    this.connection.Dispose();
                }
            }

            catch (OracleException)
            {
                throw;
            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.connection = null;
            }
        }

        /// <summary>
        /// トランザクション開始
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                this.beginTransaction = true;
                this.trnsaction = this.connection.BeginTransaction();
            }

            catch (OracleException)
            {
                throw;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// トランザクションが開始されているか
        /// </summary>
        /// <returns>結果</returns>
        public bool IsBeginTransaction()
        {
            return this.beginTransaction;
        }

        /// <summary>
        /// コミット
        /// </summary>
        public void Commit()
        {
            try
            {
                if (this.trnsaction != null)
                {
                    this.trnsaction.Commit();
                }
            }

            catch (OracleException)
            {
                throw;
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                this.trnsaction = null;
            }
        }

        /// <summary>
        /// ロールバック
        /// </summary>
        public void Rollback()
        {
            try
            {
                if (this.trnsaction != null)
                {
                    this.trnsaction.Rollback();
                }

                this.beginTransaction = false;
            }

            catch (OracleException)
            {
                throw;
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                this.trnsaction = null;
            }
        }

        /// <summary>
        /// 排他ロックの実行
        /// </summary>
        /// <param name="tableName">ロック対象テーブル名</param>
        public void ExecuteLock(string tableName)
        {
        }

        /// <summary>
        /// 排他ロックのチェック
        /// </summary>
        /// <param name="tableName">ロック対象テーブル名</param>
        public void CheckLock(string tableName)
        {
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            this.Disconnect();
        }

        /// <summary>
        /// システム時間の取得
        /// </summary>
        private DateTime getSysdate()
        {
            DateTime result = new DateTime(0);
            OracleCommand cmd = null;
            OracleDataReader odr = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT SYSDATE FROM DUAL");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                odr = cmd.ExecuteReader();

                while (odr.Read())
                {
                    result = (DateTime)odr[0];
                }

                odr.Close();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return result;

        }

        /// <summary>
        /// 棚卸システム分類の取得
        /// </summary>
        public IList<TanaorosiSystemCategory> SelectTanaorosiSystemCategory()
        {
            IList<TanaorosiSystemCategory> result = new List<TanaorosiSystemCategory>();
            OracleCommand cmd = null;
            OracleDataReader odr = null;
            
            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT ");
                sql.Append("SYSTEM_CATEGORY,"); // システム分類      
                sql.Append("SYSTEM_CATEGORY_NAME "); // システム分類名
                sql.Append("FROM ");
                sql.Append("TANAOROSI_SYSTEM_CATEGORY_MST ");
                sql.Append("ORDER BY ");
                sql.Append("TANAOROSI_SYSTEM_CATEGORY_MST.SYSTEM_CATEGORY ");
                sql.Append("ASC");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                odr = cmd.ExecuteReader();

                while (odr.Read())
                {
                    var tanaorosiSytemCategory = new TanaorosiSystemCategory();
                    tanaorosiSytemCategory.SystemCategory
                        = odr["SYSTEM_CATEGORY"].ToString().TrimEnd();
                    tanaorosiSytemCategory.SystemCategoryName
                        = odr["SYSTEM_CATEGORY_NAME"].ToString().TrimEnd();
                    result.Add(tanaorosiSytemCategory);
                }

                odr.Close();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return result;

        }

        /// <summary>
        /// 棚卸システム分類の登録
        /// </summary>
        public int InsertTanaorosiSystemCategory(IList<TanaorosiSystemCategory> tanaorosiSystemCategory)
        {
            int result = DBConstants.SQL_RESULT_NG;
            DateTime sysdate = getSysdate();

            OracleCommand cmd = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                foreach (var info in tanaorosiSystemCategory)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("INSERT INTO ");
                    sql.Append("TANAOROSI_SYSTEM_CATEGORY_MST");
                    sql.Append("(SYSTEM_CATEGORY,SYSTEM_CATEGORY_NAME,UPDYMD) ");
                    sql.Append("VALUES");
                    sql.Append("(:SYSTEM_CATEGORY,:SYSTEM_CATEGORY_NAME,:UPDYMD)");

                    cmd = new OracleCommand(sql.ToString(), this.connection);
                    cmd.BindByName = true;
                    
                    cmd.Parameters.Add(":SYSTEM_CATEGORY", info.SystemCategory);
                    cmd.Parameters.Add(":SYSTEM_CATEGORY_NAME", info.SystemCategoryName);
                    cmd.Parameters.Add(":UPDYMD", sysdate);

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Rollback();

                        if (this.connection != null)
                        {
                            this.connection.Close();
                        }
                        
                        return result;
                    }
                }

                Commit();

                result = DBConstants.SQL_RESULT_OK;
               
            }

            catch (OracleException ex)
            {
                Rollback();

                if (ex.Number == KEY_ALREADY_EXISTS)
                {
                    result = DBConstants.SQL_RESULT_ALREADY_EXISTS;
                }
                else
                {
                    throw;
                }
                
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 棚卸システム分類の更新
        /// </summary>
        public int UpdateTanaorosiSystemCategory(IList<TanaorosiSystemCategory> tanaorosiSystemCategory)
        {
            int result = DBConstants.SQL_RESULT_NG;
            DateTime sysdate = getSysdate();

            OracleCommand cmd = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                foreach (var info in tanaorosiSystemCategory)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("UPDATE ");
                    sql.Append("TANAOROSI_SYSTEM_CATEGORY_MST ");
                    sql.Append("SET ");
                    sql.Append("SYSTEM_CATEGORY_NAME =:SYSTEM_CATEGORY_NAME,");
                    sql.Append("UPDYMD=:UPDYMD ");
                    sql.Append("WHERE ");
                    sql.Append("SYSTEM_CATEGORY=:SYSTEM_CATEGORY");

                    cmd = new OracleCommand(sql.ToString(), this.connection);
                    cmd.BindByName = true;

                    cmd.Parameters.Add(":SYSTEM_CATEGORY", String.Format("{0, -2}", info.SystemCategory));
                    cmd.Parameters.Add(":SYSTEM_CATEGORY_NAME", info.SystemCategoryName);
                    cmd.Parameters.Add(":UPDYMD", sysdate);

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Rollback();

                        if (this.connection != null)
                        {
                            this.connection.Close();
                        }

                        return result;
                    }
                }

                Commit();

                result = DBConstants.SQL_RESULT_OK;
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return result;

        }

        /// <summary>
        /// 棚卸システム分類の削除
        /// </summary>
        public int DeleteTanaorosiSystemCategory(IList<TanaorosiSystemCategory> tanaorosiSystemCategory)
        {
            int result = DBConstants.SQL_RESULT_NG;

            OracleCommand cmd = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                foreach (var info in tanaorosiSystemCategory)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("DELETE ");
                    sql.Append("FROM TANAOROSI_SYSTEM_CATEGORY_MST ");
                    sql.Append("WHERE ");
                    sql.Append("SYSTEM_CATEGORY=:SYSTEM_CATEGORY");
                   
                    cmd = new OracleCommand(sql.ToString(), this.connection);
                    cmd.BindByName = true;

                    cmd.Parameters.Add(":SYSTEM_CATEGORY", String.Format("{0, -2}", info.SystemCategory));

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Rollback();

                        if (this.connection != null)
                        {
                            this.connection.Close();
                        }

                        return result;
                    }
                }

                Commit();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            result = DBConstants.SQL_RESULT_OK;
            return result;

        }

        /// <summary>
        /// 棚卸グループ区分の取得
        /// </summary>
        public IList<TanaorosiGroupKbn> SelectTanaorosiGroupKbn()
        {
            IList<TanaorosiGroupKbn> result = new List<TanaorosiGroupKbn>();
            OracleCommand cmd = null;
            OracleDataReader odr = null;

             try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT ");
                sql.Append("       SYSTEM_CATEGORY");  // システム分類   
                sql.Append("      ,GROUP_KBN");        // グループ区分      
                sql.Append("      ,GROUP_KBN_NAME");   // グループ区分名
                sql.Append("  FROM TANAOROSI_GROUP_KBN_MST");
                sql.Append(" ORDER BY SYSTEM_CATEGORY ASC, GROUP_KBN ASC");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                odr = cmd.ExecuteReader();

                while (odr.Read())
                {
                    var tanaorosiGroupKbn = new TanaorosiGroupKbn();
                    tanaorosiGroupKbn.SystemCategory
                        = odr["SYSTEM_CATEGORY"].ToString().TrimEnd();
                    tanaorosiGroupKbn.GroupKbn
                        = odr["GROUP_KBN"].ToString().TrimEnd();
                    tanaorosiGroupKbn.GroupKbnName
                        = odr["GROUP_KBN_NAME"].ToString().TrimEnd();
                    result.Add(tanaorosiGroupKbn);
                }

                odr.Close();
            }

             catch (OracleException)
             {
                 Rollback();
                 throw;
             }

             catch (Exception)
             {
                 Rollback();
                 throw;
             }

             finally
             {
                 if (this.connection != null)
                 {
                     this.connection.Close();
                 }
             }

            return result;
        }

        /// <summary>
        /// 棚卸グループ区分の登録
        /// </summary>
        public int InsertTanaorosiGroupKbn(IList<TanaorosiGroupKbn> tanaorosiGroupKbn)
        {
            int result = DBConstants.SQL_RESULT_NG;

            OracleCommand cmd = null;
            DateTime sysdate = getSysdate();

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                foreach (var info in tanaorosiGroupKbn)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("INSERT INTO ");
                    sql.Append("  TANAOROSI_GROUP_KBN_MST");
                    sql.Append("  (");
                    sql.Append("       SYSTEM_CATEGORY");
                    sql.Append("      ,GROUP_KBN");
                    sql.Append("      ,GROUP_KBN_NAME");
                    sql.Append("      ,UPDYMD");
                    sql.Append("  )VALUES(");
                    sql.Append("      :SYSTEM_CATEGORY");
                    sql.Append("     ,:GROUP_KBN");
                    sql.Append("     ,:GROUP_KBN_NAME");
                    sql.Append("     ,:UPDYMD");
                    sql.Append("  )");

                    cmd = new OracleCommand(sql.ToString(), this.connection);
                    cmd.BindByName = true;

                    cmd.Parameters.Add(":SYSTEM_CATEGORY", info.SystemCategory);
                    cmd.Parameters.Add(":GROUP_KBN", info.GroupKbn);
                    cmd.Parameters.Add(":GROUP_KBN_NAME", info.GroupKbnName);
                    cmd.Parameters.Add(":UPDYMD", sysdate);

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Rollback();
                        return result;
                    }
                }

                Commit();
                result = DBConstants.SQL_RESULT_OK;
            }

            catch (OracleException ex)
            {
                Rollback();

                if (ex.Number == KEY_ALREADY_EXISTS)
                {
                    result = DBConstants.SQL_RESULT_ALREADY_EXISTS;
                }
                else
                {
                    throw;
                }
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return result;

        }

        /// <summary>
        /// 棚卸グループ区分の更新
        /// </summary>
        public int UpdateTanaorosiGroupKbn(IList<TanaorosiGroupKbn> tanaorosiGroupKbn)
        {
            int result = DBConstants.SQL_RESULT_NG;

            OracleCommand cmd = null;
            DateTime sysdate = getSysdate();

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                foreach (var info in tanaorosiGroupKbn)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append(" UPDATE");
                    sql.Append("   TANAOROSI_GROUP_KBN_MST");
                    sql.Append(" SET ");
                    sql.Append("   GROUP_KBN_NAME = :GROUP_KBN_NAME");
                    sql.Append("  ,UPDYMD = :UPDYMD");
                    sql.Append(" WHERE");
                    sql.Append("   SYSTEM_CATEGORY = :SYSTEM_CATEGORY");
                    sql.Append("   AND GROUP_KBN = :GROUP_KBN");

                    cmd = new OracleCommand(sql.ToString(), this.connection);
                    cmd.BindByName = true;

                    cmd.Parameters.Add(":SYSTEM_CATEGORY", String.Format("{0, -2}", info.SystemCategory));
                    cmd.Parameters.Add(":GROUP_KBN", String.Format("{0, -2}", info.GroupKbn));
                    cmd.Parameters.Add(":GROUP_KBN_NAME", info.GroupKbnName);
                    cmd.Parameters.Add(":UPDYMD", sysdate);

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Rollback();
                        return result;
                    }
                }

                Commit();
                result = DBConstants.SQL_RESULT_OK;
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return result;

        }

        /// <summary>
        /// 棚卸グループ区分の削除
        /// </summary>
        public int DeleteTanaorosiGroupKbn(IList<TanaorosiGroupKbn> tanaorosiGroupKbn)
        {
            int result = DBConstants.SQL_RESULT_NG;

            OracleCommand cmd = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                foreach (var info in tanaorosiGroupKbn)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append(" DELETE ");
                    sql.Append("   FROM  TANAOROSI_GROUP_KBN_MST ");
                    sql.Append("  WHERE ");
                    sql.Append("   SYSTEM_CATEGORY = :SYSTEM_CATEGORY");
                    sql.Append("   AND GROUP_KBN = :GROUP_KBN");

                    cmd = new OracleCommand(sql.ToString(), this.connection);
                    cmd.BindByName = true;

                    cmd.Parameters.Add(":SYSTEM_CATEGORY", String.Format("{0, -2}", info.SystemCategory));
                    cmd.Parameters.Add(":GROUP_KBN", String.Format("{0, -2}", info.GroupKbn));

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Rollback();

                        if (this.connection != null)
                        {
                            this.connection.Close();
                        }

                        return result;
                    }
                }

                Commit();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            result = DBConstants.SQL_RESULT_OK;
            return result;
        }

        /// <summary>
        /// 棚卸操作の取得
        /// </summary>
        public IList<TanaorosiOperation> SelectTanaorosiOperation()
        {
            IList<TanaorosiOperation> result = new List<TanaorosiOperation>();
            OracleCommand cmd = null;
            OracleDataReader odr = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT ");
                sql.Append("SYSTEM_CATEGORY,"); // システム分類 
                sql.Append("OPERATION_CD,");    // 操作コード    
                sql.Append("OPERATION_NAME ");  // 操作順
                sql.Append("FROM ");
                sql.Append("TANAOROSI_OPERATION_MST ");
                sql.Append("ORDER BY ");
                sql.Append("SYSTEM_CATEGORY ASC,OPERATION_CD ASC ");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                odr = cmd.ExecuteReader();

                while (odr.Read())
                {
                    var tanaorosiOperation = new TanaorosiOperation();
                    tanaorosiOperation.SystemCategory
                        = odr["SYSTEM_CATEGORY"].ToString();
                    tanaorosiOperation.OperationCode
                        = odr["OPERATION_CD"].ToString();
                    tanaorosiOperation.OperationName
                        = odr["OPERATION_NAME"].ToString();
                    result.Add(tanaorosiOperation);
                }

                odr.Close();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 棚卸操作の登録
        /// </summary>
        public int InsertTanaorosiOperation(IList<TanaorosiOperation> tanaorosiOperation)
        {
            int result = DBConstants.SQL_RESULT_NG;

            OracleCommand cmd = null;
            DateTime sysdate = getSysdate();

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                foreach (var info in tanaorosiOperation)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("INSERT INTO ");
                    sql.Append("TANAOROSI_OPERATION_MST ");
                    sql.Append("(SYSTEM_CATEGORY,OPERATION_CD,OPERATION_NAME,UPDYMD)");
                    sql.Append("VALUES");
                    sql.Append("(:SYSTEM_CATEGORY,:OPERATION_CD,:OPERATION_NAME,:UPD_YMD)");

                    cmd = new OracleCommand(sql.ToString(), this.connection);
                    cmd.BindByName = true;

                    cmd.Parameters.Add(":SYSTEM_CATEGORY", String.Format("{0, -2}", info.SystemCategory));
                    cmd.Parameters.Add(":OPERATION_CD", Int32.Parse(info.OperationCode));
                    cmd.Parameters.Add(":OPERATION_NAME", info.OperationName);
                    cmd.Parameters.Add(":UPD_YMD", sysdate);

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Rollback();

                        if (this.connection != null)
                        {
                            this.connection.Close();
                        }

                        return result;
                    }
                }

                Commit();

                result = DBConstants.SQL_RESULT_OK;

            }

            catch (OracleException ex)
            {
                Rollback();

                if (ex.Number == KEY_ALREADY_EXISTS)
                {
                    result = DBConstants.SQL_RESULT_ALREADY_EXISTS;
                }
                else
                {
                    throw;
                }

            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return result;

        }

        /// <summary>
        /// 棚卸操作の更新
        /// </summary>
        public int UpdateTanaorosiOperation(IList<TanaorosiOperation> tanaorosiOperation)
        {
            int result = DBConstants.SQL_RESULT_NG;

            OracleCommand cmd = null;
            DateTime sysdate = getSysdate();

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                foreach (var info in tanaorosiOperation)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("UPDATE ");
                    sql.Append("TANAOROSI_OPERATION_MST ");
                    sql.Append("SET ");
                    sql.Append("OPERATION_NAME=:OPERATION_NAME,");
                    sql.Append("UPDYMD=:UPDYMD ");
                    sql.Append("WHERE ");
                    sql.Append("SYSTEM_CATEGORY = :SYSTEM_CATEGORY ");
                    sql.Append("AND OPERATION_CD=:OPERATION_CD");

                    cmd = new OracleCommand(sql.ToString(), this.connection);
                    cmd.BindByName = true;

                    cmd.Parameters.Add(":SYSTEM_CATEGORY", String.Format("{0, -2}", info.SystemCategory));
                    cmd.Parameters.Add(":OPERATION_CD", Int32.Parse(String.Format("{0, -2}", info.OperationCode)));
                    cmd.Parameters.Add(":OPERATION_NAME", info.OperationName);
                    cmd.Parameters.Add(":UPDYMD", sysdate);

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Rollback();

                        if (this.connection != null)
                        {
                            this.connection.Close();
                        }

                        return result;
                    }
                }

                Commit();

                result = DBConstants.SQL_RESULT_OK;
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return result;

        }

        /// <summary>
        /// 棚卸操作の削除
        /// </summary>
        public int DeleteTanaorosiOperation(IList<TanaorosiOperation> tanaorosiOperation)
        {
            int result = DBConstants.SQL_RESULT_NG;

            OracleCommand cmd = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                foreach (var info in tanaorosiOperation)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("DELETE ");
                    sql.Append("FROM TANAOROSI_OPERATION_MST ");
                    sql.Append("WHERE ");
                    sql.Append("SYSTEM_CATEGORY = :SYSTEM_CATEGORY ");
                    sql.Append("AND OPERATION_CD=:OPERATION_CD");

                    cmd = new OracleCommand(sql.ToString(), this.connection);
                    cmd.BindByName = true;

                    cmd.Parameters.Add(":SYSTEM_CATEGORY", String.Format("{0, -2}", info.SystemCategory));
                    cmd.Parameters.Add(":OPERATION_CD", Int32.Parse(String.Format("{0, -2}", info.OperationCode)));

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Rollback();
                        return result;
                    }
                }

                Commit();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            result = DBConstants.SQL_RESULT_OK;
            return result;

        }

        /// <summary>
        /// 棚卸操作グループの取得
        /// </summary>
        public IList<TanaorosiOpeartionGroup> SelectTanaorosiOpeartionGroup()
        {
            IList<TanaorosiOpeartionGroup> result = new List<TanaorosiOpeartionGroup>();
            OracleCommand cmd = null;
            OracleDataReader odr = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT ");
                sql.Append("        SYAINCD");          // 社員番号
                sql.Append("       ,SYAINSZCD");        // 所属部署コード
                sql.Append("       ,SYSTEM_CATEGORY");  // システム分類
                sql.Append("       ,GROUP_KBN");        // グループ区分
                sql.Append("   FROM TANAOROSI_OPERATION_GROUP_MST");
                sql.Append("  ORDER BY SYAINCD ASC, SYAINSZCD ASC, SYSTEM_CATEGORY ASC, GROUP_KBN");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                odr = cmd.ExecuteReader();

                while (odr.Read())
                {
                    var tanaorosiOpeartionGroup = new TanaorosiOpeartionGroup();
                    tanaorosiOpeartionGroup.SyainCode
						= odr["SYAINCD"].ToString().TrimEnd();
                    tanaorosiOpeartionGroup.SyainSZCode
                        = odr["SYAINSZCD"].ToString().TrimEnd();
					tanaorosiOpeartionGroup.SystemCategory
                        = odr["SYSTEM_CATEGORY"].ToString().TrimEnd();
                    tanaorosiOpeartionGroup.GroupKbn
						= odr["GROUP_KBN"].ToString().TrimEnd();
                    result.Add(tanaorosiOpeartionGroup);
                }

                odr.Close();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 棚卸操作グループの登録
        /// </summary>
        public int InsertTanaorosiOpeartionGroup(IList<TanaorosiOpeartionGroup> tanaorosiOpeartionGroup)
        {
            int result = DBConstants.SQL_RESULT_NG;

            OracleCommand cmd = null;
            DateTime sysdate = getSysdate();

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                foreach (var info in tanaorosiOpeartionGroup)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append(" INSERT INTO");
                    sql.Append("        TANAOROSI_OPERATION_GROUP_MST");
                    sql.Append("       (");
                    sql.Append("        SYAINCD");
                    sql.Append("       ,SYAINSZCD");
                    sql.Append("       ,SYSTEM_CATEGORY");
                    sql.Append("       ,GROUP_KBN");
                    sql.Append("       ,UPDYMD");
                    sql.Append("  )VALUES(");
                    sql.Append("        :SYAINCD");
                    sql.Append("       ,:SYAINSZCD");
                    sql.Append("       ,:SYSTEM_CATEGORY");
                    sql.Append("       ,:GROUP_KBN");
                    sql.Append("       ,:UPDYMD");
                    sql.Append("  )");

                    cmd = new OracleCommand(sql.ToString(), this.connection);
                    cmd.BindByName = true;

                    cmd.Parameters.Add(":SYAINCD", info.SyainCode);
                    cmd.Parameters.Add(":SYAINSZCD", info.SyainSZCode);
                    cmd.Parameters.Add(":SYSTEM_CATEGORY", info.SystemCategory);
                    cmd.Parameters.Add(":GROUP_KBN", info.GroupKbn);
                    cmd.Parameters.Add(":UPDYMD", sysdate);

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Rollback();
                        return result;
                    }
                }

                Commit();
                result = DBConstants.SQL_RESULT_OK;
            }

            catch (OracleException ex)
            {
                Rollback();

                if (ex.Number == KEY_ALREADY_EXISTS)
                {
                    result = DBConstants.SQL_RESULT_ALREADY_EXISTS;
                }
                else
                {
                    throw;
                }
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return result;

        }

        /// <summary>
        /// 棚卸操作グループの更新
        /// </summary>
        public int UpdateTanaorosiOpeartionGroup(IList<TanaorosiOpeartionGroup> tanaorosiOpeartionGroup)
        {
            int result = DBConstants.SQL_RESULT_NG;

            OracleCommand cmd = null;
            DateTime sysdate = getSysdate();

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                foreach (var info in tanaorosiOpeartionGroup)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append(" UPDATE ");
                    sql.Append("   TANAOROSI_OPERATION_GROUP_MST");
                    sql.Append(" SET ");
                    sql.Append("   GROUP_KBN = :GROUP_KBN");
                    sql.Append("  ,UPDYMD = :UPDYMD");
                    sql.Append(" WHERE ");
                    sql.Append("        SYAINCD = :SYAINCD");
                    sql.Append("   AND SYAINSZCD = :SYAINSZCD");
                    sql.Append("   AND SYSTEM_CATEGORY = :SYSTEM_CATEGORY");

                    cmd = new OracleCommand(sql.ToString(), this.connection);
                    cmd.BindByName = true;

                    cmd.Parameters.Add(":SYAINCD", String.Format("{0, -4}", info.SyainCode));
                    cmd.Parameters.Add(":SYAINSZCD", String.Format("{0, -5}", info.SyainSZCode));
                    cmd.Parameters.Add(":SYSTEM_CATEGORY", String.Format("{0, -2}", info.SystemCategory));
                    cmd.Parameters.Add(":GROUP_KBN", String.Format("{0, -2}", info.GroupKbn));
                    cmd.Parameters.Add(":UPDYMD", sysdate);

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Rollback();
                        return result;
                    }
                }

                Commit();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            result = DBConstants.SQL_RESULT_OK;
            return result;

        }

        /// <summary>
        /// 棚卸操作グループの削除
        /// </summary>
        public int DeleteTanaorosiOpeartionGroup(IList<TanaorosiOpeartionGroup> tanaorosiOpeartionGroup)
        {
            int result = DBConstants.SQL_RESULT_NG;

            OracleCommand cmd = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                foreach (var info in tanaorosiOpeartionGroup)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append(" DELETE ");
                    sql.Append("   FROM  TANAOROSI_OPERATION_GROUP_MST");
                    sql.Append("  WHERE ");
                    sql.Append("         SYAINCD = :SYAINCD");
                    sql.Append("    AND  SYAINSZCD = :SYAINSZCD");
                    sql.Append("    AND  SYSTEM_CATEGORY = :SYSTEM_CATEGORY");

                    cmd = new OracleCommand(sql.ToString(), this.connection);
                    cmd.BindByName = true;

                    cmd.Parameters.Add(":SYAINCD", String.Format("{0, -4}", info.SyainCode));
                    cmd.Parameters.Add(":SYAINSZCD", String.Format("{0, -5}", info.SyainSZCode));
                    cmd.Parameters.Add(":SYSTEM_CATEGORY", String.Format("{0, -2}", info.SystemCategory));

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Rollback();
                        return result;
                    }
                }
                Commit();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return DBConstants.SQL_RESULT_OK;
        }

        /// <summary>
        /// 棚卸操作メニューの取得
        /// </summary>
        public IList<TanaorosiOperationMenu> SelectTanaorosiOperationMenu()
        {
            IList<TanaorosiOperationMenu> result = new List<TanaorosiOperationMenu>();
            OracleCommand cmd = null;
            OracleDataReader odr = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT ");
				sql.Append("       SYAINSZCD");          // 社員所属コード
                sql.Append("      ,SYSTEM_CATEGORY");    // システム分類      
                sql.Append("      ,GROUP_KBN");          // グループ区分 
                sql.Append("      ,WORK_KBN");           // 作業区分 
                sql.Append("      ,OPERATION_TYPE");     // 操作種類
				sql.Append("      ,OPERATION_ORDER");    // 操作順
				sql.Append("      ,OPERATION_CD");       // 操作コード
                sql.Append("  FROM TANAOROSI_OPERATION_MENU_MST");
                sql.Append(" ORDER BY SYAINSZCD,SYSTEM_CATEGORY,GROUP_KBN,WORK_KBN,OPERATION_TYPE,OPERATION_ORDER,OPERATION_CD");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                odr = cmd.ExecuteReader();

                while (odr.Read())
                {
					var tanaorosiOperationMenu = new TanaorosiOperationMenu();
					tanaorosiOperationMenu.SyainSZCode = odr["SYAINSZCD"].ToString();
					tanaorosiOperationMenu.SystemCategory = odr["SYSTEM_CATEGORY"].ToString();
                    tanaorosiOperationMenu.GroupKbn = odr["GROUP_KBN"].ToString();
                    tanaorosiOperationMenu.WorkCategory = odr["WORK_KBN"].ToString();
                    tanaorosiOperationMenu.OperationType = int.Parse(odr["OPERATION_TYPE"].ToString());
					tanaorosiOperationMenu.OperationOrder = int.Parse(odr["OPERATION_ORDER"].ToString());
					tanaorosiOperationMenu.OperationCode = int.Parse(odr["OPERATION_CD"].ToString());
                    result.Add(tanaorosiOperationMenu);
                }

                odr.Close();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 棚卸操作メニューの重複チェック
        /// </summary>
        /// <param name="tanaorosiOperationMenu">棚卸操作メニュー</param>
        /// <returns>重複有無</returns>
        public bool IsOverlapCheckTanaorosiOperationMenu(IList<TanaorosiOperationMenu> tanaorosiOperationMenu)
        {
            OracleCommand cmd = null;
            OracleDataReader odr = null;

            try
            {
                // DB接続
                Connect();

                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT COUNT(*) as cnt FROM TANAOROSI_OPERATION_MENU_MST");
                sql.Append(" WHERE ");
                sql.Append("       SYAINSZCD=:SYAINSZCD");
                sql.Append("   AND SYSTEM_CATEGORY=:SYSTEM_CATEGORY");
                sql.Append("   AND GROUP_KBN=:GROUP_KBN");
                sql.Append("   AND WORK_KBN=:WORK_KBN");
                sql.Append("   AND OPERATION_TYPE=:OPERATION_TYPE");
                sql.Append("   AND OPERATION_ORDER=:OPERATION_ORDER");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                foreach (var info in tanaorosiOperationMenu)
                {
                    cmd.Parameters.Add(":SYAINSZCD", info.SyainSZCode);
                    cmd.Parameters.Add(":SYSTEM_CATEGORY", info.SystemCategory);
                    cmd.Parameters.Add(":GROUP_KBN", info.GroupKbn);
                    cmd.Parameters.Add(":WORK_KBN", info.WorkCategory);
                    cmd.Parameters.Add(":OPERATION_TYPE", String.Format("{0, -1}", info.OperationType));
                    cmd.Parameters.Add(":OPERATION_ORDER", String.Format("{0, -2}", info.OperationOrder));

                    odr = cmd.ExecuteReader();

                    if (odr.Read())
                    {
                        if (odr["cnt"].ToString() != "0")
                        {
                            break;
                        }
                    }
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (odr != null)
                {
                    odr.Close();
                }
                if (this.connection != null)
                {
                    this.connection.Close();
                }
                if (cmd != null)
                {
                    cmd = null;
                }
            }
        }

        /// <summary>
        /// 棚卸操作メニューの登録
        /// </summary>
        public int InsertTanaorosiOperationMenu(IList<TanaorosiOperationMenu> tanaorosiOperationMenu)
        {
            int result = DBConstants.SQL_RESULT_NG;

            OracleCommand cmd = null;
            DateTime sysdate = getSysdate();

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                foreach (var info in tanaorosiOperationMenu)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("INSERT INTO TANAOROSI_OPERATION_MENU_MST(");
                    sql.Append("       SYAINSZCD");
                    sql.Append("      ,SYSTEM_CATEGORY");
                    sql.Append("      ,GROUP_KBN");
                    sql.Append("      ,WORK_KBN");
                    sql.Append("      ,OPERATION_TYPE");
                    sql.Append("      ,OPERATION_ORDER");
                    sql.Append("      ,OPERATION_CD");
                    sql.Append("      ,UPDYMD");
                    sql.Append(" )VALUES(");
                    sql.Append("       :SYAINSZCD");
                    sql.Append("      ,:SYSTEM_CATEGORY");
                    sql.Append("      ,:GROUP_KBN");
                    sql.Append("      ,:WORK_KBN");
                    sql.Append("      ,:OPERATION_TYPE");
                    sql.Append("      ,:OPERATION_ORDER");
                    sql.Append("      ,:OPERATION_CD");
                    sql.Append("      ,:UPDYMD)");

                    cmd = new OracleCommand(sql.ToString(), this.connection);
                    cmd.BindByName = true;

                    cmd.Parameters.Add(":SYAINSZCD", info.SyainSZCode);
                    cmd.Parameters.Add(":SYSTEM_CATEGORY", info.SystemCategory);
                    cmd.Parameters.Add(":GROUP_KBN", info.GroupKbn);
                    cmd.Parameters.Add(":WORK_KBN", info.WorkCategory);
                    cmd.Parameters.Add(":OPERATION_TYPE", String.Format("{0, -1}", info.OperationType));
                    cmd.Parameters.Add(":OPERATION_ORDER", String.Format("{0, -2}", info.OperationOrder));
                    cmd.Parameters.Add(":OPERATION_CD", String.Format("{0, -3}", info.OperationCode));
                    cmd.Parameters.Add(":UPDYMD", sysdate);

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Rollback();
                        return result;
                    }
                }

                Commit();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
                if (cmd != null)
                {
                    cmd = null;
                }
            }

            result =  DBConstants.SQL_RESULT_OK;
            return result;

        }

        /// <summary>
        /// 棚卸操作メニューの更新
        /// </summary>
        public int UpdateTanaorosiOperationMenu(IList<TanaorosiOperationMenu> tanaorosiOperationMenu)
        {
            int result = DBConstants.SQL_RESULT_NG;

            OracleCommand cmd = null;
            DateTime sysdate = getSysdate();

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                foreach (var info in tanaorosiOperationMenu)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("UPDATE ");
                    sql.Append("       TANAOROSI_OPERATION_MENU_MST");
                    sql.Append("   SET ");
                    sql.Append("       OPERATION_CD=:OPERATION_CD");
                    sql.Append("      ,UPDYMD=:UPDYMD ");
                    sql.Append(" WHERE SYAINSZCD=:SYAINSZCD");
                    sql.Append("   AND SYSTEM_CATEGORY=:SYSTEM_CATEGORY");
                    sql.Append("   AND GROUP_KBN=:GROUP_KBN");
                    sql.Append("   AND WORK_KBN=:WORK_KBN");
                    sql.Append("   AND OPERATION_TYPE=:OPERATION_TYPE");
                    sql.Append("   AND OPERATION_ORDER=:OPERATION_ORDER");

                    cmd = new OracleCommand(sql.ToString(), this.connection);
                    cmd.BindByName = true;

                    cmd.Parameters.Add(":SYAINSZCD", info.SyainSZCode);
                    cmd.Parameters.Add(":SYSTEM_CATEGORY", info.SystemCategory);
                    cmd.Parameters.Add(":GROUP_KBN", info.GroupKbn);
                    cmd.Parameters.Add(":WORK_KBN", info.WorkCategory);
                    cmd.Parameters.Add(":OPERATION_TYPE", String.Format("{0, -1}", info.OperationType));
                    cmd.Parameters.Add(":OPERATION_ORDER", String.Format("{0, -2}", info.OperationOrder));
                    cmd.Parameters.Add(":OPERATION_CD", String.Format("{0, -3}", info.OperationCode));
                    cmd.Parameters.Add(":UPDYMD", sysdate);

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Rollback();
                        return result;
                    }
                }

                Commit();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
                if (cmd != null)
                {
                    cmd = null;
                }
            }

            result = DBConstants.SQL_RESULT_OK;
            return result;

        }

        /// <summary>
        /// 棚卸操作メニューの削除
        /// </summary>
        public int DeleteTanaorosiOperationMenu(IList<TanaorosiOperationMenu> tanaorosiOperationMenu)
        {
            int result = DBConstants.SQL_RESULT_NG;

            OracleCommand cmd = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                foreach (var info in tanaorosiOperationMenu)
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("DELETE FROM TANAOROSI_OPERATION_MENU_MST ");
                    sql.Append("WHERE ");
                    sql.Append("SYAINSZCD=:SYAINSZCD ");
                    sql.Append("AND ");
                    sql.Append("SYSTEM_CATEGORY=:SYSTEM_CATEGORY ");
                    sql.Append("AND ");
                    sql.Append("GROUP_KBN=:GROUP_KBN ");
                    sql.Append("AND ");
                    sql.Append("WORK_KBN=:WORK_CATEGORY ");
                    sql.Append("AND ");
                    sql.Append("OPERATION_TYPE=:OPERATION_TYPE ");
                    sql.Append("AND ");
                    sql.Append("OPERATION_ORDER=:OPERATION_ORDER ");

                    cmd = new OracleCommand(sql.ToString(), this.connection);
                    cmd.BindByName = true;

                    cmd.Parameters.Add(":SYAINSZCD", info.SyainSZCode);
                    cmd.Parameters.Add(":SYSTEM_CATEGORY", info.SystemCategory);
                    cmd.Parameters.Add(":GROUP_KBN", info.GroupKbn);
                    cmd.Parameters.Add(":WORK_CATEGORY", info.WorkCategory);
                    cmd.Parameters.Add(":OPERATION_TYPE", info.OperationType);
                    cmd.Parameters.Add(":OPERATION_ORDER", info.OperationOrder);

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Rollback();
                        return result;
                    }
                }

                Commit();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
                if (cmd != null)
                {
                    cmd = null;
                }
            }

            result = DBConstants.SQL_RESULT_OK;
            return result;

        }

        /// <summary>
        /// 棚卸ログトランの取得
        /// </summary>
        public IList<TanaorosiLogTRN> SelectTanaorosiLogTRN(LogSearchCondition searchCondition)
        {
            OracleCommand cmd = null;
            OracleDataReader odr = null;
            IList<TanaorosiLogTRN> logs = new List<TanaorosiLogTRN>(); 

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT ");
                sql.Append("       LOG.SYSTEM_CATEGORY AS SYSTEM_CATEGORY");    // システム分類
                sql.Append("      ,LOG.SYAINCD AS SYAINCD");                    // 社員コード
                sql.Append("      ,SYA.SYAINNM AS SYAINNM");                    // 社員名
                sql.Append("      ,LOG.OPERATION_YMD AS OPERATION_YMD");        // 作業日時
                sql.Append("      ,LOG.WORK_KBN AS WORK_KBN");                  // 作業区分
                sql.Append("      ,LOG.OPERATION_TYPE AS OPERATION_TYPE");      // 操作種別
                sql.Append("      ,LOG.OPERATION_CONTENT AS OPERATION_CONTENT"); // 操作内容
                sql.Append("      ,LOG.BIKOU AS BIKOU");                        // 備考
                sql.Append("      ,LOG.ERROR_CONTENT AS ERROR_CONTENT");        // エラー内容
                sql.Append("      ,LOG.ERROR_CODE AS ERROR_CODE");              // エラーコード
                sql.Append(" FROM  TANAOROSI_LOG_TRN LOG ");
                sql.Append(" LEFT JOIN ");
                sql.Append(" SYAIN_MST SYA ON ");
                sql.Append(" LOG.SYAINCD = SYA.SYAINCD ");
                sql.Append(" WHERE ");
                sql.Append("       LOG.SYSTEM_CATEGORY = '" + searchCondition.SystemCategory + "' ");
                sql.Append("       AND LOG.LOG_TYPE = '" + searchCondition.LogType + "' ");
                if (searchCondition.UseEmployeeNo) sql.Append(" AND LOG.SYAINCD = '" + searchCondition.EmployeeNo + "' ");
                if (searchCondition.UseOperationDate)
                {
                    switch (searchCondition.OperationDateCondition)
                    {
                        case LogSearchCondition.OperationDateConditions.Min:
                            sql.Append(" AND LOG.OPERATION_YMD >= '" + searchCondition.MinOperationDate + "' ");
                            break;
                        case LogSearchCondition.OperationDateConditions.Max:
                            sql.Append(" AND LOG.OPERATION_YMD <= '" + searchCondition.MaxOperationDate + "' ");
                            break;
                        case LogSearchCondition.OperationDateConditions.MinMax:
                            sql.Append(" AND LOG.OPERATION_YMD BETWEEN '" + searchCondition.MinOperationDate + "' ");
                            sql.Append(" AND '" + searchCondition.MaxOperationDate + "' ");
                            break;
                        default:
                            break;
                    }
                }
                if (searchCondition.UseWorkKbn) sql.Append(" AND LOG.WORK_KBN = '" + searchCondition.WorkKbn + "' ");
                if (searchCondition.UseOperateType) sql.Append(" AND LOG.OPERATION_TYPE = '" + searchCondition.OperateType + "' ");
                if (searchCondition.UseOperateContent) sql.Append(" AND LOG.OPERATION_CONTENT = '" + searchCondition.OperateContent + "' ");
                sql.Append(" ORDER BY ");
                sql.Append("       LOG.LOG_TYPE ASC");
                sql.Append("      ,LOG.SYAINCD ASC");
                sql.Append("      ,LOG.OPERATION_YMD DESC");
                sql.Append("      ,LOG.WORK_KBN ASC");
                sql.Append("      ,LOG.OPERATION_TYPE ASC");
                sql.Append("      ,LOG.OPERATION_CONTENT ASC");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                odr = cmd.ExecuteReader();

                while (odr.Read())
                {
                    var tanaorosiLogTRN = new TanaorosiLogTRN();
                    tanaorosiLogTRN.SystemCategory = odr["SYSTEM_CATEGORY"].ToString();
                    tanaorosiLogTRN.EmployeeNo = odr["SYAINCD"].ToString();
                    tanaorosiLogTRN.EmployeeName = odr["SYAINNM"].ToString();
                    tanaorosiLogTRN.OperetionDate = odr["OPERATION_YMD"].ToString();
                    tanaorosiLogTRN.WorkKBN = odr["WORK_KBN"].ToString();
                    tanaorosiLogTRN.OperateType = odr["OPERATION_TYPE"].ToString();
                    tanaorosiLogTRN.OpereteContent = odr["OPERATION_CONTENT"].ToString();
                    tanaorosiLogTRN.Bikou = odr["BIKOU"].ToString();
                    tanaorosiLogTRN.ErrorContent = odr["ERROR_CONTENT"].ToString();
                    tanaorosiLogTRN.ErrorCode = odr["ERROR_CODE"].ToString();
                    logs.Add(tanaorosiLogTRN);
                }

                odr.Close();

            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return logs;

        }

        /// <summary>
        /// 棚卸詳細ログトランの取得
        /// </summary>
        public IList<TanaorosiDetailLogTRN> SelectTanaorosiDetailLogTRN(LogSearchCondition searchCondition)
        {
            OracleCommand cmd = null;
            OracleDataReader odr = null;
            IList<TanaorosiDetailLogTRN> logs = new List<TanaorosiDetailLogTRN>(); 
            
            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT ");
                sql.Append("       LOG.SYAINCD AS SYAINCD");                    // 社員コード
                sql.Append("      ,SYA.SYAINNM AS SYAINNM");                    // 社員名
                sql.Append("      ,LOG.OPERATION_YMD AS OPERATION_YMD");        // 作業日時
                sql.Append("      ,LOG.WORK_KBN AS WORK_KBN");                  // 作業区分
                sql.Append("      ,LOG.OPERATION_TYPE AS OPERATION_TYPE");      // 操作種別
                sql.Append("      ,LOG.OPERATION_CONTENT AS OPERATION_CONTENT");// 操作内容
                sql.Append("      ,LOG.SIZAI_KBN AS SIZAI_KBN");                // 資材区分
                sql.Append("      ,LOG.HINMOKUCD AS HINMOKUCD");                // 品目CD
                sql.Append("      ,LOG.GYOSYACD AS GYOSYACD");                  // 業者CD 
                sql.Append("      ,LOG.HINMOKUNM AS HINMOKUNM");                // 品目名
                sql.Append("      ,LOG.GYOSYANM AS GYOSYANM");                  // 業者名
                sql.Append("      ,LOG.BIKOU AS BIKOU");                        // 備考
                sql.Append("      ,LOG.UPDATE_CONTENT1 AS UPDATE_CONTENT1");    // 変更内容１
                sql.Append("      ,LOG.UPDATE_CONTENT2 AS UPDATE_CONTENT2");    // 変更内容２
                sql.Append("      ,LOG.UPDATE_CONTENT3 AS UPDATE_CONTENT3");    // 変更内容３
                sql.Append("      ,LOG.ERROR_CONTENT AS ERROR_CONTENT");        // エラー内容
                sql.Append(" FROM  TANAOROSI_DETAIL_LOG_TRN LOG ");
                sql.Append(" LEFT JOIN ");
                sql.Append(" SYAIN_MST SYA ON ");
                sql.Append(" LOG.SYAINCD = SYA.SYAINCD ");
                sql.Append(" WHERE ");
                sql.Append("       LOG_TYPE = '" + searchCondition.LogType + "' ");
                if (searchCondition.UseEmployeeNo) sql.Append(" AND LOG.SYAINCD = '" + searchCondition.EmployeeNo + "' ");
                if (searchCondition.UseOperationDate)
                {
                    switch (searchCondition.OperationDateCondition)
                    {
                        case LogSearchCondition.OperationDateConditions.Min:
                            sql.Append(" AND LOG.OPERATION_YMD >= '" + searchCondition.MinOperationDate + "' ");
                            break;
                        case LogSearchCondition.OperationDateConditions.Max:
                            sql.Append(" AND LOG.OPERATION_YMD <= '" + searchCondition.MaxOperationDate + "' ");
                            break;
                        case LogSearchCondition.OperationDateConditions.MinMax:
                            sql.Append(" AND LOG.OPERATION_YMD BETWEEN '" + searchCondition.MinOperationDate + "' ");
                            sql.Append(" AND '20" + searchCondition.MaxOperationDate + "' ");
                            break;
                        default:
                            break;
                    }
                }
                if (searchCondition.UseWorkKbn) sql.Append(" AND LOG.WORK_KBN = '" + searchCondition.WorkKbn + "' ");
                if (searchCondition.UseOperateType) sql.Append(" AND LOG.OPERATION_TYPE = '" + searchCondition.OperateType + "' ");
                if (searchCondition.UseOperateContent) sql.Append(" AND LOG.OPERATION_CONTENT = '" + searchCondition.OperateContent + "' ");
                sql.Append(" ORDER BY ");
                sql.Append("       LOG.LOG_TYPE ASC");
                sql.Append("      ,LOG.SYAINCD ASC");
                sql.Append("      ,LOG.OPERATION_YMD DESC");
                sql.Append("      ,LOG.WORK_KBN ASC");
                sql.Append("      ,LOG.OPERATION_TYPE ASC");
                sql.Append("      ,LOG.OPERATION_CONTENT ASC");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                odr = cmd.ExecuteReader();

                while (odr.Read())
                {
                    var tanaorosiDetailLogTRN = new TanaorosiDetailLogTRN();
                    tanaorosiDetailLogTRN.EmployeeNo = odr["SYAINCD"].ToString();
                    tanaorosiDetailLogTRN.EmployeeName = odr["SYAINNM"].ToString();
                    tanaorosiDetailLogTRN.OperetionDate = odr["OPERATION_YMD"].ToString();
                    tanaorosiDetailLogTRN.WorkKBN = odr["WORK_KBN"].ToString();
                    tanaorosiDetailLogTRN.OperateType = odr["OPERATION_TYPE"].ToString();
                    tanaorosiDetailLogTRN.OpereteContent = odr["OPERATION_CONTENT"].ToString();
                    tanaorosiDetailLogTRN.SizaiKBN = odr["SIZAI_KBN"].ToString();
                    tanaorosiDetailLogTRN.HinmokuCD = odr["HINMOKUCD"].ToString();
                    tanaorosiDetailLogTRN.GyosyaCD = odr["GYOSYACD"].ToString();
                    tanaorosiDetailLogTRN.HinmokuNM = odr["HINMOKUNM"].ToString();
                    tanaorosiDetailLogTRN.GyosyaNM = odr["GYOSYANM"].ToString();
                    tanaorosiDetailLogTRN.Bikou = odr["BIKOU"].ToString();
                    tanaorosiDetailLogTRN.UpdateContent1 = odr["UPDATE_CONTENT1"].ToString();
                    tanaorosiDetailLogTRN.UpdateContent2 = odr["UPDATE_CONTENT2"].ToString();
                    tanaorosiDetailLogTRN.UpdateContent3 = odr["UPDATE_CONTENT3"].ToString();
                    tanaorosiDetailLogTRN.ErrorContent = odr["ERROR_CONTENT"].ToString();
                    logs.Add(tanaorosiDetailLogTRN);
                }

                odr.Close();

            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return logs;

        }

        /// <summary>
        /// ユーザの取得
        /// </summary>
        /// <returns>ユーザ</returns>
        public IDictionary<string, EmployeeInfo> SelectUsersInfo()
        {
            IDictionary<string, EmployeeInfo> dictionary = new Dictionary<string, EmployeeInfo>();
            OracleCommand cmd = null;
            OracleDataReader odr = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT ");
                sql.Append("        SY.SYAINCD   AS SYAINCD "); // 社員コード
                sql.Append("       ,SY.SYAINNM   AS SYAINNM "); // 社員名
                sql.Append("       ,SY.SYAINSZCD AS SZCD    "); // 所属コード
                sql.Append("       ,SZ.SZNM      AS SZNM    "); // 所属名
                sql.Append("   FROM SYAIN_MST SY ");
                sql.Append("       ,SYOZO_MST SZ ");
                sql.Append("  WHERE SY.SYAINSZCD = SZ.SZCD ");
                sql.Append("  ORDER BY ");
                sql.Append("        SY.SYAINCD ASC ");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                odr = cmd.ExecuteReader();

                while (odr.Read())
                {
                    if (!dictionary.ContainsKey(odr["SYAINCD"].ToString().TrimEnd()))
                    {
                        EmployeeInfo employeeInfo = new EmployeeInfo();
                        employeeInfo.EmployeeName = odr["SYAINNM"].ToString();
                        employeeInfo.DeploymentCode = odr["SZCD"].ToString();
                        employeeInfo.DeploymentName = odr["SZNM"].ToString();
                        dictionary.Add(odr["SYAINCD"].ToString().TrimEnd(), employeeInfo);
                    }
                }

                odr.Close();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return dictionary;
        }

        /// <summary>
        /// 部署名の取得
        /// </summary>
        /// <returns>部署名</returns>
        public IDictionary<string, string> SelectDeploymentName()
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            OracleCommand cmd = null;
            OracleDataReader odr = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT ");
                sql.Append("        SZCD"); // 部署コード
                sql.Append("       ,SZNM"); //部署名
                sql.Append("   FROM SYOZO_MST ");
                sql.Append("  ORDER BY ");
                sql.Append("        SZCD ASC ");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                odr = cmd.ExecuteReader();

                while (odr.Read())
                {
                    dictionary.Add(odr["SZCD"].ToString().TrimEnd(), odr["SZNM"].ToString());
                }

                odr.Close();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return dictionary;
        }

        /// <summary>
        /// 棚卸システム分類の取得
        /// </summary>
        /// <returns>棚卸システム分類</returns>
        public IDictionary<string, string> SelectTanaorosiSystemCategorys()
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            OracleCommand cmd = null;
            OracleDataReader odr = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT ");
                sql.Append("SYSTEM_CATEGORY,"); // システム分類      
                sql.Append("SYSTEM_CATEGORY_NAME "); // システム分類名
                sql.Append("FROM ");
                sql.Append("TANAOROSI_SYSTEM_CATEGORY_MST ");
                sql.Append("ORDER BY ");
                sql.Append("TANAOROSI_SYSTEM_CATEGORY_MST.SYSTEM_CATEGORY ");
                sql.Append("ASC");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                odr = cmd.ExecuteReader();

                while (odr.Read())
                {
                    dictionary.Add(odr["SYSTEM_CATEGORY"].ToString().TrimEnd(), odr["SYSTEM_CATEGORY_NAME"].ToString());
                }

                odr.Close();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return dictionary;
        }

        /// <summary>
        /// 棚卸グループ区分の取得
        /// </summary>
        /// <returns>棚卸グループ区分</returns>
        public IList<TanaorosiGroupKbn> SelectTanaorosiGroupKbns()
        {
            IList<TanaorosiGroupKbn> list = new List<TanaorosiGroupKbn>();
            OracleCommand cmd = null;
            OracleDataReader odr = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT ");
                sql.Append("       SYSTEM_CATEGORY");  // システムカテゴリ    
                sql.Append("      ,GROUP_KBN");        // グループ区分      
                sql.Append("      ,GROUP_KBN_NAME");   // グループ区分名
                sql.Append("  FROM TANAOROSI_GROUP_KBN_MST");
                sql.Append(" ORDER BY SYSTEM_CATEGORY ASC, GROUP_KBN ASC");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                odr = cmd.ExecuteReader();

                while (odr.Read())
                {
                    TanaorosiGroupKbn groupKbn = new TanaorosiGroupKbn();
                    groupKbn.SystemCategory = odr["SYSTEM_CATEGORY"].ToString().TrimEnd();
                    groupKbn.GroupKbn = odr["GROUP_KBN"].ToString().TrimEnd();
                    groupKbn.GroupKbnName = odr["GROUP_KBN_NAME"].ToString().TrimEnd();
                    list.Add(groupKbn);
                }

                odr.Close();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return list;
        }

        /// <summary>
        /// 棚卸グループ区分の取得
        /// </summary>
        /// <param name="systemCategory">システム分類</param>
        /// <returns>棚卸グループ区分</returns>
        public IDictionary<string, string> SelectTanaorosiGroupKbns(string systemCategory)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            OracleCommand cmd = null;
            OracleDataReader odr = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT ");
                sql.Append("       GROUP_KBN");  // グループ区分      
                sql.Append("      ,GROUP_KBN_NAME"); // グループ区分名
                sql.Append("  FROM TANAOROSI_GROUP_KBN_MST");
                sql.Append(" WHERE ");
                sql.Append(" SYSTEM_CATEGORY = :SYSTEM_CATEGORY ");
                sql.Append(" ORDER BY GROUP_KBN ASC");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                cmd.Parameters.Add(":SYSTEM_CATEGORY", String.Format("{0, -2}", systemCategory));

                odr = cmd.ExecuteReader();

                while (odr.Read())
                {
                    dictionary.Add(odr["GROUP_KBN"].ToString().TrimEnd(), odr["GROUP_KBN_NAME"].ToString());
                }

                odr.Close();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return dictionary;
        }

        /// <summary>
        /// 操作コードの取得
        /// </summary>
        /// <returns>操作コード</returns>
        public IList<TanaorosiOperation> SelectTanaorosiOperationCodes()
        {
            IList<TanaorosiOperation> list = new List<TanaorosiOperation>();
            OracleCommand cmd = null;
            OracleDataReader odr = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT ");
                sql.Append("       SYSTEM_CATEGORY");   // システム分類
                sql.Append("      ,OPERATION_CD");      // 操作コード
                sql.Append("      ,OPERATION_NAME");    // 操作名
                sql.Append("  FROM TANAOROSI_OPERATION_MST");
                sql.Append(" ORDER BY SYSTEM_CATEGORY ASC, OPERATION_CD ASC");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                odr = cmd.ExecuteReader();

                while (odr.Read())
                {
                    TanaorosiOperation operation = new TanaorosiOperation();
                    operation.SystemCategory = odr["SYSTEM_CATEGORY"].ToString().TrimEnd();
                    operation.OperationCode = odr["OPERATION_CD"].ToString().TrimEnd();
                    operation.OperationName = odr["OPERATION_NAME"].ToString().TrimEnd();
                    list.Add(operation);
                }

                odr.Close();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return list;
        }

        /// <summary>
        /// 操作コードの取得
        /// </summary>
        /// <param name="systemCategory">システム分類</param>
        /// <returns>操作コード</returns>
        public IDictionary<string, string> SelectTanaorosiOperationCodes(string systemCategory)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            OracleCommand cmd = null;
            OracleDataReader odr = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT ");
                sql.Append("       OPERATION_CD");   // 操作コード
                sql.Append("      ,OPERATION_NAME"); // 操作名
                sql.Append("  FROM TANAOROSI_OPERATION_MST ");
                sql.Append(" WHERE ");
                sql.Append(" SYSTEM_CATEGORY = :SYSTEM_CATEGORY ");
                sql.Append(" ORDER BY OPERATION_CD ASC");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                cmd.Parameters.Add(":SYSTEM_CATEGORY", String.Format("{0, -2}", systemCategory));

                odr = cmd.ExecuteReader();

                while (odr.Read())
                {
                    dictionary.Add(odr["OPERATION_CD"].ToString().TrimEnd(), odr["OPERATION_NAME"].ToString().TrimEnd());
                }

                odr.Close();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return dictionary;
        }

        /// <summary>
        /// 棚卸操作ユーザの取得
        /// </summary>
        /// <returns>棚卸操作ユーザ</returns>
        public IDictionary<string, string> SelectTanaorosiOpeationUsers()
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            OracleCommand cmd = null;
            OracleDataReader odr = null;

            try
            {
                // DB接続
                Connect();

                // トランザクション開始
                BeginTransaction();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT ");
                sql.Append("TANAOROSI_OPERATION_GROUP_MST.SYAINCD AS SYAINCD, "); // 社員コード
                sql.Append("SYAIN_MST.SYAINNM AS SYAINNM "); // 社員名
                sql.Append("FROM ");
                sql.Append("TANAOROSI_OPERATION_GROUP_MST, ");
                sql.Append("SYAIN_MST ");
                sql.Append("WHERE ");
                sql.Append("TANAOROSI_OPERATION_GROUP_MST.SYAINCD=SYAIN_MST.SYAINCD ");
                sql.Append("ORDER BY ");
                sql.Append("TANAOROSI_OPERATION_GROUP_MST.SYAINCD ");
                sql.Append("ASC");

                cmd = new OracleCommand(sql.ToString(), this.connection);
                cmd.BindByName = true;

                odr = cmd.ExecuteReader();

                while (odr.Read())
                {
                    if(!dictionary.ContainsKey(odr["SYAINCD"].ToString()))
                    {
                        dictionary.Add(odr["SYAINCD"].ToString(), odr["SYAINNM"].ToString());
                    }
                }

                odr.Close();
            }

            catch (OracleException)
            {
                Rollback();
                throw;
            }

            catch (Exception)
            {
                Rollback();
                throw;
            }

            finally
            {
                if (this.connection != null)
                {
                    this.connection.Close();
                }
            }

            return dictionary;
        }


        #endregion
    }
}
