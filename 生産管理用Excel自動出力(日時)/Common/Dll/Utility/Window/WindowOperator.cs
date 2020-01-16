using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
//*************************************************************************************
//
//   Window操作を行うための静的クラス
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
namespace Utility.Window
{
    /// <summary>
    /// Window操作を行うための静的クラス
    /// </summary>
    public static class WindowOperator
    {
        #region フィールド

        private static Process target_proc = null;
        private static IntPtr target_hwnd = IntPtr.Zero;

        #endregion

        #region 列挙型

        /// <summary>
        /// ShowWindowコマンド
        /// </summary>
        public enum ShowWindowEnum : int
        {
            /// <summary>
            /// SW_HIDE
            /// </summary>
            SW_HIDE = 0,

            /// <summary>
            /// SW_NORMAL
            /// </summary>
            SW_NORMAL = 1,

            /// <summary>
            /// SW_SHOWMINIMIZED
            /// </summary>
            SW_SHOWMINIMIZED = 2,

            /// <summary>
            /// SW_MAXIMIZE
            /// </summary>
            SW_MAXIMIZE = 3,

            /// <summary>
            /// SW_SHOWNOACTIVATE
            /// </summary>
            SW_SHOWNOACTIVATE = 4,

            /// <summary>
            /// SW_SHOW
            /// </summary>
            SW_SHOW = 5,

            /// <summary>
            /// SW_MINIMIZE
            /// </summary>
            SW_MINIMIZE = 6,

            /// <summary>
            /// SW_SHOWMINNOACTIVE
            /// </summary>
            SW_SHOWMINNOACTIVE = 7,

            /// <summary>
            /// SW_SHOWNA
            /// </summary>
            SW_SHOWNA = 8,

            /// <summary>
            /// SW_RESTORE
            /// </summary>
            SW_RESTORE = 9,

            /// <summary>
            /// SW_SHOWDEFAULT
            /// </summary>
            SW_SHOWDEFAULT = 10,

            /// <summary>
            /// SW_MAX
            /// </summary>
            SW_MAX = 11
        }

        #endregion

        #region Win32API宣言

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

        [DllImport("user32.dll")]
        private static extern uint getWindowThreadProcessId(IntPtr hWnd, ref uint procId);

        // コールバックメソッドのデリゲート
        private delegate int enumerateWindowsCallback(IntPtr hWnd, int lParam);

        [DllImport("user32", EntryPoint = "EnumWindows")]
        private static extern int enumWindows(enumerateWindowsCallback lpEnumFunc, int lParam);

        #endregion

        #region メソッド

        /// <summary>
        /// ウィンドウを列挙するためのコールバックメソッド
        /// </summary>
        /// <param name="hWnd">対象となるウィンドウハンドル</param>
        /// <param name="lParam">パラメータ</param>
        /// <returns></returns>
        public static int EnumerateWindows(IntPtr hWnd, int lParam)
        {
            uint procId = 0;
            uint result = getWindowThreadProcessId(hWnd, ref procId);
            if (procId == target_proc.Id)
            {
                // 同じIDで複数のウィンドウが見つかる場合がある
                // とりあえず最初のウィンドウが見つかった時点で終了する
                target_hwnd = hWnd;
                return 0;
            }

            // 列挙を継続するには0以外を返す必要がある
            return 1;
        }

        /// <summary>
        /// プロセス名とプログラム名が同一のプロセスを見つけ、
        /// それが所持しているメインウィンドウを前面に表示する。
        /// </summary>
        /// <returns>正常にウィンドウを前面に表示できた場合、trueを返却する</returns>
        public static bool ForegroundWindow()
        {
            bool result = false;
            Process current = Process.GetCurrentProcess();
            Process target = null;
            string procName = current.ProcessName;

            try
            {
                // 稼働中のプロセスから、アクティブなプロセスと同一のプロセス名を持つプロセスを取得する
                Process[] procs = Process.GetProcesses();

                foreach (Process proc in procs)
                {
                    if (procName.IndexOf(proc.ProcessName) > -1)
                    {
                        if ((int)proc.MainWindowHandle == 0)
                        {
                            continue;
                        }
                        target = proc;
                        break;
                    }
                }

                result = foregroundWindow(target);
            }
            catch (Exception)
            {
            }

            return result;
        }
        /// <summary>
        /// 指定プロセスを起動する。
        /// </summary>
        /// <param name="path">対象ディレクトリ</param>
        /// <param name="procName">プロセス名称</param>
        /// <param name="arguments">コマンド引数</param>
        /// <param name="doMaximized">最大化するかどうか</param>
        public static bool StartupWindow(string path, string procName, string arguments, bool doMaximized)
        {
            try
            {
                // ProcessStartInfo の新しいインスタンスを生成する
                ProcessStartInfo hPsInfo = (new ProcessStartInfo());
                hPsInfo.FileName = procName;
                hPsInfo.Arguments = arguments;
                hPsInfo.CreateNoWindow = true;
                hPsInfo.WorkingDirectory = path;
                if(doMaximized)
                    hPsInfo.WindowStyle = ProcessWindowStyle.Maximized;  //最大化

                // プロセスを起動する
                Process.Start(hPsInfo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 全てのプロセスから起動対象のプロセスが存在するかチェックし、
        /// 存在しない場合は対象プロセスを起動する。
        /// 存在する場合はそのプロセスを最前面に表示する。
        /// </summary>
        /// <param name="procName">プロセス名称</param>
        public static bool CanStartupWindow(string procName)
        {
            try
            {
                bool startup = true;
                Process target = null;
                Process[] procs = Process.GetProcesses();

                foreach (Process proc in procs)
                {
                    if (procName.IndexOf(proc.ProcessName) > -1)
                    {
                        target = proc;
                        break;
                    }
                }

                if (target != null)
                {
                    foregroundWindow(target);
                    startup = false;
                }
                return startup;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 指定したプロセスを前面に表示する。
        /// </summary>
        /// <param name="target">アクティブ表示対象プロセス</param>
        /// <returns>正常にウィンドウを前面に表示できた場合、trueを返す。</returns>
        private static bool foregroundWindow(Process target)
        {
            bool result = false;
            if (target != null)
            {
                // ウィンドウが最小化されていた場合に元に戻す。
                if (IsIconic(target.MainWindowHandle))
                    ShowWindowAsync(target.MainWindowHandle, ShowWindowEnum.SW_RESTORE);

                // ウィンドウを前面に移動する。
                result = SetForegroundWindow(target.MainWindowHandle);
            }

            return result;
        }

        /// <summary>
        /// 指定したプロセスを前面に表示する。
        /// </summary>
        /// <param name="app">アクティブ表示対象プロセス</param>
        /// <returns>正常にウィンドウを前面に表示できた場合、trueを返す。</returns>
        public static bool ForegroundWindow(Microsoft.Office.Interop.Excel.Application app)
        {
            bool result = false;

            // ウィンドウが最小化されていた場合に元に戻す。
            if (IsIconic((IntPtr)(app.Hwnd)))
                ShowWindowAsync((IntPtr)(app.Hwnd), ShowWindowEnum.SW_RESTORE);

            // ウィンドウを前面に移動する。
            result = SetForegroundWindow((IntPtr)(app.Hwnd));

            return result;
        }

        #endregion
    }
}