using System;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;
using System.Collections;

namespace Project1
{
    /// <summary>
    /// ODBC操作クラス
    /// </summary>
    class C_ODBC : IDisposable
    {
        /// <summary>
        /// oracleエラー：ロック取得失敗
        /// </summary>
        public const int ORA_00054 = 54;

        /// <summary>
        /// ストアド設定構造体
        /// </summary>
        public struct StoredParam{
            public string Name;
            public OdbcType Type;
            public int Size;
            public ParameterDirection Direction;
            public object Input;
            public string Output;
        }

        /// <summary>
        /// SQLコネクション
        /// </summary>
        private OdbcConnection _con = null;

        /// <summary>
        /// トランザクション・オブジェクト
        /// </summary>
        private OdbcTransaction _trn = null;

        /// <summary>
        /// エラー情報
        /// </summary>
        private OdbcException _ex = null;

        /// <summary>
        /// DB接続
        /// </summary>
        /// <param name="dsn">データソース名</param>
        /// <param name="dbn">データベース名</param>
        /// <param name="uid">ユーザーID</param>
        /// <param name="pas">パスワード</param>
        public void Connect(String dsn, String dbn, String uid, String pas)
        {
            try
            {
                if (_con == null)
                {
                    _con = new OdbcConnection();
                }

                // 接続文字列作成
                string cst = "";
                cst = cst + ";DSN=" + dsn;
                cst = cst + ";Database=" + dbn;
                cst = cst + ";UID=" + uid;
                cst = cst + ";PWD=" + pas;
                cst += ";";

                _con.ConnectionString = cst;

                _con.Open();
            }
            catch (OdbcException odbcEx)
            {
                _ex = odbcEx;
                throw odbcEx;
            }
            catch (Exception ex)
            {
                throw new Exception("Connect Error", ex);
            }
        }

        /// <summary>
        /// DB接続
        /// </summary>
        public void Connect()
        {
            try
            {
                if (_con == null)
                {
                    _con = new OdbcConnection();
                }

                // 接続文字列作成
                string cst = "";
                cst += ";DSN=so_oracle";
                cst += ";Database=GM";
                cst += ";UID=SA";
                cst += ";PWD=SA";
                cst += ";";

                _con.ConnectionString = cst;

                _con.Open();
            }
            catch (OdbcException odbcEx)
            {
                _ex = odbcEx;
                throw odbcEx;
            }
            catch (Exception ex)
            {
                throw new Exception("Connect Error", ex);
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
                if (_con != null)
                {
                    _con.Close();
                }
            }
            catch (OdbcException odbcEx)
            {
                _ex = odbcEx;
                throw odbcEx;
            }
            catch (Exception ex)
            {
                throw new Exception("Disconnect Error", ex);
            }
            finally
            {
                _con = null;
            }
        }

        /// <summary>
        /// SQLの実行
        /// </summary>
        /// <param name="sql">SQL文</param>
        public DataTable ExecSQL(String sql)
        {
            DataTable dt = new DataTable();

            try
            {
                OdbcCommand sqlCommand = new OdbcCommand(sql, _con, _trn);
                OdbcDataAdapter adapter = new OdbcDataAdapter(sqlCommand);

                adapter.Fill(dt);
                adapter.Dispose();
                sqlCommand.Dispose();
            }
            catch (OdbcException odbcEx)
            {
                _ex = odbcEx;
                throw odbcEx;
            }
            catch (Exception ex)
            {
                throw new Exception("ExecSQL Error", ex);
            }

            return dt;
        }

        /// <summary>
        /// ストアドプロシージャの実行
        /// </summary>
        /// <param name="sCmd">ストアドのコマンド</param>
        /// <param name="arylist">設定するパラメータ</param>
        public void ExecStored(string sCmd, ref ArrayList arylist)
        {
            OdbcCommand cmd = null;
            OdbcParameter prm = null;

            if (arylist == null)
            {
                arylist = new ArrayList();
            }

            try
            {
                cmd = new OdbcCommand(sCmd, _con, _trn);
                cmd.CommandType = CommandType.StoredProcedure;
                
                for (int i = 0; i < arylist.Count; i++)
                {
                    StoredParam sp = (StoredParam)arylist[i];
                    prm = cmd.Parameters.Add(sp.Name, sp.Type);
                    
                    // サイズ設定
                    if (0 < sp.Size)
                    {
                        prm.Size = sp.Size;
                    }

                    // Direction設定
                    prm.Direction = sp.Direction;
                    if (sp.Direction == ParameterDirection.Input
                     || sp.Direction == ParameterDirection.InputOutput)
                    {
                        prm.Value = sp.Input;
                    }
                }

                // 実行
                cmd.ExecuteNonQuery();

                for (int i = 0; i < arylist.Count; i++)
                {
                    StoredParam sp = (StoredParam)arylist[i];
                    // 出力対象のデータを取得
                    if (sp.Direction == ParameterDirection.Output
                     || sp.Direction == ParameterDirection.InputOutput)
                    {
                        sp.Output = Convert.ToString(cmd.Parameters[sp.Name].Value);
                    }
                    arylist[i] = sp;
                }
            }
            catch (OdbcException odbcEx)
            {
                _ex = odbcEx;
                throw odbcEx;
            }
            catch (Exception ex)
            {
                throw new Exception("ExecStored Error", ex);
            }
        }

        /// <summary>
        /// トランザクション開始
        /// </summary>
        public void BeginTrans()
        {
            try
            {
                _trn = _con.BeginTransaction();
            }
            catch (OdbcException odbcEx)
            {
                _ex = odbcEx;
                this.Error();
            }
        }

        /// <summary>
        /// コミット
        /// </summary>
        public void Commit()
        {
            try
            {
                if (_trn != null)
                {
                    _trn.Commit();
                }
            }
            catch (OdbcException odbcEx)
            {
                _ex = odbcEx;
                this.Error();
            }
            catch (Exception)
            {
                ;
            }
            finally
            {
                _trn = null;
            }
        }

        /// <summary>
        /// ロールバック
        /// </summary>
        public void Rollback()
        {
            try
            {
                if (_trn != null)
                {
                    _trn.Rollback();
                }
            }
            catch (Exception)
            {
                ;
            }
            finally
            {
                _trn = null;
            }
        }

        /// <summary>
        /// ロックチェック
        /// </summary>
        /// <param name="tableName">対象テーブル名</param>
        public void LockCheck(string tableName)
        {
            bool bLoop = true;
            bool bLock = false;
            string sSql = string.Empty;
            string sMsg = "アクセスするデータベースはロック中です,続行しますか。";
            string sTitle = "ロック チェック";
            DialogResult dlgResult = DialogResult.None;

            sSql = "SELECT * FROM " + tableName + " FOR UPDATE WAIT NOWAIT;";

            while (bLoop)
            {
                try
                {
                    // ロックがかかっていない場合はそのまま終了
                    bLoop = false;
                    this.BeginTrans();
                    this.ExecSQL(sSql);
                    this.Commit();
                }
                catch (OdbcException odEx)
                {
                    this.Rollback();

                    for (int i = 0; i < odEx.Errors.Count; i++)
                    {
                        // 対象が排他ロックされていた場合、再度チェックをかけるか確認する
                        if (odEx.Errors[i].NativeError == ORA_00054)
                        {
                            bLock = true;
                            // ORA-00054：ロック取得失敗エラー
                            dlgResult = MessageBox.Show(sMsg, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (dlgResult == DialogResult.Yes)
                            {
                                bLoop = true;
                            }
                            else
                            {
                                this.Disconnect();
                            }
                            break;
                        }
                    }

                    // ロック以外のエラーの場合は通常のエラーとして扱う
                    if (!bLock)
                    {
                        _ex = odEx;
                        this.Error();
                    }
                }
            }
        }

        /// <summary>
        /// テーブル排他ロック
        /// </summary>
        /// <param name="sTableName">テーブル名</param>
        public void TableLockEX(string sTableName)
        {
            //指定したテーブルに排他ロックをかける
            string sSql = "";
            sSql = "LOCK TABLE " + sTableName + " IN EXCLUSIVE MODE;";

            this.ExecSQL(sSql);
        }

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public void Error()
        {
            int errCnt = 0;
            string sMsg = string.Empty;
            string sCrLf = System.Environment.NewLine;

            if (_ex != null)
            {
                // エラー数取得
                errCnt = _ex.Errors.Count;
            }

            // エラーメッセージ出力
            if (errCnt == 0)
            {
                C_COMMON.Msg("エラー -- エラー情報を取得できません。");
            }
            else
            {
                for (int i = 0; i < errCnt; i++)
                {
                    sMsg = _ex.Errors[i].Message;
                    // メッセージ出力
                    C_COMMON.Msg(sMsg);
                }
            }

            C_COMMON.Msg("本アプリケーションはエラーの為、強制終了します。");

            // 終了処理
            this.Rollback();
            this.Disconnect();
            System.Environment.Exit(0);
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            // 接続を開放する
            this.Disconnect();
        }
    }
}
