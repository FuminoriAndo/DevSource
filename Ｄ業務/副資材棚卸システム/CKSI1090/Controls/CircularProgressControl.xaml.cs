using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;
//*************************************************************************************
//
//   回転型の進捗表示コントロール
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1090.Controls
{
    /// <summary>
    /// 回転型の進捗表示コントロール
    /// </summary>
    public partial class CircularProgressControl
    {
        #region フィールド

        /// <summary>
        /// アニメーション用タイマー
        /// </summary>
        private readonly DispatcherTimer _animationTimer;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CircularProgressControl()
        {
            InitializeComponent();

            _animationTimer = new DispatcherTimer(
                DispatcherPriority.ContextIdle, Dispatcher) {Interval = new TimeSpan(0, 0, 0, 0, 50)};
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 開始
        /// </summary>
        private void Start()
        {
            _animationTimer.Tick += OnTick;
            _animationTimer.Start();
        }

        /// <summary>
        /// 停止
        /// </summary>
        private void Stop()
        {
            _animationTimer.Stop();
            Mouse.OverrideCursor = Cursors.Arrow;
            _animationTimer.Tick -= OnTick;
        }

        /// <summary>
        /// タイマーイベントのハンドラ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベント</param>
        private void OnTick(object sender, EventArgs e)
        {
            SpinnerRotate.Angle = (SpinnerRotate.Angle + 36) % 360;
        }

        /// <summary>
        /// コントロール完了時のイベントハンドラ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // 円の位置を設定する
            const double offset = Math.PI;
            const double step = Math.PI * 2 / 10.0;

            SetPosition(C0, offset, 0.0, step);
            SetPosition(C1, offset, 1.0, step);
            SetPosition(C2, offset, 2.0, step);
            SetPosition(C3, offset, 3.0, step);
            SetPosition(C4, offset, 4.0, step);
            SetPosition(C5, offset, 5.0, step);
            SetPosition(C6, offset, 6.0, step);
            SetPosition(C7, offset, 7.0, step);
            SetPosition(C8, offset, 8.0, step);
        }

        /// <summary>
        /// 円の位置の設定
        /// </summary>
        /// <param name="ellipse">円</param>
        /// <param name="offset">オフセット</param>
        /// <param name="posOffSet">円毎のオフセット</param>
        /// <param name="step">円のステップ番号</param>
        private static void SetPosition(Ellipse ellipse, double offset,
            double posOffSet, double step)
        {
            ellipse.SetValue(Canvas.LeftProperty, 10.0
                + Math.Sin(offset + posOffSet * step) * 10.0);

            ellipse.SetValue(Canvas.TopProperty, 10
                + Math.Cos(offset + posOffSet * step) * 10.0);
        }

        /// <summary>
        /// アンロードのイベントハンドラ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        /// <summary>
        /// 可視状態の変更イベントハンドラ
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="e">イベントデータ</param>
        private void OnVisibleChanged(object sender,
            DependencyPropertyChangedEventArgs e)
        {
            bool isVisible = (bool)e.NewValue;

            if (isVisible)
                Start();
            else
                Stop();
        }

        #endregion
    }
}