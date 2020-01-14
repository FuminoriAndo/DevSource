using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CKSI0061
{
    /// <summary>
    /// 購買検収データ作成受信監視クラス
    /// </summary>
    public partial class CKSI0061M : Form
    {
        /// <summary>
        /// ソケット
        /// </summary>
        private Socket ReceiveSocket;
        /// <summary>
        /// 接続待ちスレッド
        /// </summary>
        private Thread StartListeningThread;
        /// <summary>
        /// 接続待ちスレッド終了指示フラグ(volatile指定)
        /// </summary>
        private volatile bool SLTAlive;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CKSI0061M()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        /// <summary>
        /// フォーム起動時イベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">イベント引数</param>
        private void CKSI0061M_Load(object sender, EventArgs e)
        {
            try
            {
                System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetEntryAssembly();
                string path = myAssembly.Location.Replace(myAssembly.ManifestModule.ToString(), string.Empty);
                XDocument versionDoc = XDocument.Load(System.IO.Path.Combine(path, "CKSI0061.settings"));
                XElement result = versionDoc.XPathSelectElement("host");
    
                // スレッド終了指示フラグを未終了に初期化
                SLTAlive = false;
                // Socket の生成
                ReceiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // ホストのIPアドレスとポート番号の指定
                IPEndPoint EndPointHost = new IPEndPoint(IPAddress.Parse(result.Element("ip").Value), int.Parse(result.Element("port").Value));
                // ローカル エンドポイント(IPｱﾄﾞﾚｽ等の情報)と関連付け
                ReceiveSocket.Bind(EndPointHost);
                // 電文取り出しの接続がまだ保留中におけるキューの最大長
                ReceiveSocket.Listen(100);
                // 接続待ち用スレッドを作成
                StartListeningThread = new Thread(StartListening);
                // 接続待ち用スレッドを開始
                StartListeningThread.Start();
                // スレッド終了指示フラグを未終了に設定
                SLTAlive = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// フォーム閉鎖時イベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">イベント引数</param>
        private void CKSI0061M_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // 接続待ち終了ボタンのクリックイベント
                if (SLTAlive)  // 接続待ちｽﾚｯﾄﾞが作成されていて使える場合
                {
                    if (ReceiveSocket != null)
                    {
                        // 接続要求受け入れの終了
                        ReceiveSocket.Close();
                    }
                    //スレッド終了指示フラグを終了に設定
                    SLTAlive = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 確認ボタン押下ボタンのクリックイベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">イベント引数</param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 接続待ちスレッド用メソッド
        /// </summary>
        private void StartListening()
        {
            try
            {
                // 受信の受付を行なうための無限ループ
                while (SLTAlive)    // ｽﾚｯﾄﾞ終了指示ﾌﾗｸﾞでの終了指示がある場合はﾙｰﾌﾟ終了
                {
                    // クライアントからの接続を受け付ける
                    Socket ClientSocket = ReceiveSocket.Accept(); // Socketｸﾗｲｱﾝﾄ
                    // クライアントからの電文の受信
                    byte[] ReceiveData = new byte[2000];
                    int ResSize = ClientSocket.Receive(ReceiveData, ReceiveData.Length, SocketFlags.None);
                    byte[] ReceiveData1 = new byte[ResSize];
                    for (int i = 0; i < ResSize; i++)
                    {
                        ReceiveData1[i] = ReceiveData[i];
                    }
                    string str = Encoding.Unicode.GetString(ReceiveData1);
                    //文字列作成、表示
                    listBox1.Items.Insert(0, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " + str + " " + Properties.Resources.MSG_RECEIVEDATA);
                    // Socketｸﾗｲｱﾝﾄをｸﾛｰｽﾞ
                    ClientSocket.Shutdown(SocketShutdown.Both);
                    ClientSocket.Close();
                    //フォーム表示
                    this.WindowState = FormWindowState.Normal;
                    this.Show();
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// タスクアイコンのダブルクリックイベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">イベント引数</param>
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Normal;
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// メニューのクリックイベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">イベント引数</param>
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ToolStripMenuItem1.Name.CompareTo((sender as ToolStripMenuItem).Name) == 0)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.Show();
                }
                else
                {
                    if (MessageBox.Show(Properties.Resources.MSG_APP_EXIT, Properties.Resources.APP_TITLE,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
