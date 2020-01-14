using System;
using System.Runtime.InteropServices;
//*************************************************************************************
//
//   Window操作用クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace CKSI1090.Common
{
    /// <summary>
    /// Window操作用クラス
    /// </summary>
    internal class WindowOperator
    {
        #region 列挙型

        /// <summary>
        /// ShowWindow Commands 定数の列挙型
        /// </summary>
        public enum ShowWindowEnum : int
        {
            SW_HIDE = 0,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_MAX = 11
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 最前面に表示されるウィンドウを指定する。
        /// </summary>
        /// <param name="hWnd">対象となるウィンドウハンドル</param>
        /// <returns>正常に終了した場合に true が返ります</returns>
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// ウィンドウの状態を変更する。
        /// </summary>
        /// <param name="hWnd">対象となるウィンドウハンドル</param>
        /// <param name="nCmdShow">ウィンドウの状態を示す ShowWindowEnum 列挙子</param>
        /// <returns>設定前にウィンドウが可視状態だった場合、trueを返却する</returns>
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, ShowWindowEnum nCmdShow);

        /// <summary>
        /// 指定したウィンドウが最小化されているかどうかを判定する。
        /// </summary>
        /// <param name="hWnd">対象となるウィンドウハンドル</param>
        /// <returns>ウィンドウが最小化に設定されている場合、trueを返却する</returns>
        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        /// <summary>
        /// 指定したプロセスを前面に表示する。
        /// </summary>
        /// <param name="target">アクティブ表示対象プロセス</param>
        /// <returns>正常にウィンドウを前面に表示できた場合、trueを返す。</returns>
        internal static bool ForegroundWindow(Microsoft.Office.Interop.Excel.Application app)
        {
            bool result = false;

            // ウィンドウが最小化されていた場合に元に戻す。
            if (IsIconic((IntPtr)(app.Hwnd)))
                ShowWindowAsync((IntPtr)(app.Hwnd), ShowWindowEnum.SW_RESTORE);

            // ウィンドウを前面に移動する。
            result = SetForegroundWindow((IntPtr)(app.Hwnd));

            return result;
        }

        /// <summary>
        /// 指定したプロセスを前面に表示する。
        /// </summary>
        /// <param name="process">アクティブ表示対象プロセス</param>
        /// <returns>正常にウィンドウを前面に表示できた場合、trueを返す。</returns>
        internal static bool ForegroundWindow(System.Diagnostics.Process process)
        {
            bool result = false;

            // ウィンドウが最小化されていた場合に元に戻す。
            if (IsIconic((IntPtr)(process.Handle)))
                ShowWindowAsync((IntPtr)(process.Handle), ShowWindowEnum.SW_RESTORE);

            // ウィンドウを前面に移動する。
            result = SetForegroundWindow((IntPtr)(process.Handle));

            return result;
        }

        #endregion
    }
}