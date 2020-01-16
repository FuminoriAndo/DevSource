using System;
using System.Runtime.InteropServices;
//*************************************************************************************
//
//   ネットワーク接続クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
namespace Utility.Network
{
    /// <summary>
    /// ネットワーク接続クラス
    /// </summary>
    public class NetConnector : IDisposable
    {
        #region Win32API宣言

        /// <summary>
        /// NetResource
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct NetResource
        {
            /// <summary>
            /// Scope
            /// </summary>
            public int scope;
            /// <summary>
            /// Type
            /// </summary>
            public int type;
            /// <summary>
            /// DisplayTypeタイプ
            /// </summary>
            public int displayType;
            /// <summary>
            /// Usage
            /// </summary>
            public int usage;
            /// <summary>
            /// LocalName
            /// </summary>
            [MarshalAs(UnmanagedType.LPStr)]
            public string localName;
            /// <summary>
            /// RemoteName
            /// </summary>
            [MarshalAs(UnmanagedType.LPStr)]
            public string remoteName;
            /// <summary>
            /// Comment
            /// </summary>
            [MarshalAs(UnmanagedType.LPStr)]
            public string comment;
            /// <summary>
            /// Provider
            /// </summary>
            [MarshalAs(UnmanagedType.LPStr)]
            public string provider;
        }

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(ref NetResource netResource, string password, string username, Int32 flags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string remoteName, Int32 flags, bool force);

        #endregion

        #region フィールド

        /// <summary>
        /// 破棄されたか
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// 接続したか
        /// </summary>
        private bool connected = false;

        /// <summary>
        /// リモート名(接続先パス)
        /// </summary>
        private string remoteName = string.Empty;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="remoteName">リモート名(接続先パス)</param>
        /// <param name="userName">ユーザ名</param>
        /// <param name="password">パスワード</param>
        public NetConnector(string remoteName, string userName, string password)
        {
            try
            {
                NetResource res = new NetResource()
                {
                    scope = 0,
                    type = 1,
                    displayType = 0,
                    usage = 0,
                    localName = null,
                    remoteName = remoteName,
                    comment = null,
                    provider = null
                };

                WNetCancelConnection2(remoteName, 0, true);
                int ret = WNetAddConnection2(ref res, password, userName, 0);
                connected = (ret == 0);
                this.remoteName = remoteName;
            }
            catch
            {
            }
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 破棄する
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 破棄する
        /// </summary>
        /// <param name="disposing">アンマネージを破棄するか</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing && connected && !string.IsNullOrEmpty(remoteName))
            {
                WNetCancelConnection2(remoteName, 0, true);

                connected = false;
            }

            disposed = true;
        }

        #endregion

    }
}
