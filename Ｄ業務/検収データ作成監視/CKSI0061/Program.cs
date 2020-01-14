using System;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace CKSI0061
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Assembly assem = Assembly.GetExecutingAssembly();

                GuidAttribute guid = Attribute.GetCustomAttribute(assem, typeof(GuidAttribute)) as GuidAttribute;
                Mutex mutex = new Mutex(false, guid.Value);
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("既に起動しています。", Properties.Resources.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new CKSI0061M());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
