using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
//*************************************************************************************
//
//   入力値制限ビヘイビア
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.30              DSK吉武   　 新規作成
//
//*************************************************************************************
namespace Behaviors
{
    #region クラス
    /// <summary>
    /// 作業日時の入力値制限ビヘイビア
    /// </summary>
    public class InsertDateBehaviors
    {
        public static readonly DependencyProperty IsNumericProperty =
                    DependencyProperty.RegisterAttached(
                        "IsNumeric", typeof(bool),
                        typeof(InsertDateBehaviors),
                        new UIPropertyMetadata(false, isNumericChanged)
                    );

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetIsNumeric(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsNumericProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static void SetIsNumeric(DependencyObject obj, bool value)
        {
            obj.SetValue(IsNumericProperty, value);
        }

        private static void isNumericChanged
            (DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            var textBox = sender as TextBox;
            if (textBox == null) return;

            // イベントの登録・削除 
            textBox.KeyDown -= OnKeyDown;
            textBox.TextChanged -= onTextChanged;
            var newValue = (bool)e.NewValue;
            if (newValue)
            {
                textBox.KeyDown += OnKeyDown;
                textBox.TextChanged += onTextChanged;
                DataObject.AddPastingHandler(textBox, textBoxPastingEventHandler);
            }
        }

        static void OnKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            if ((Keyboard.Modifiers != ModifierKeys.Shift) && (Key.D0 <= e.Key && e.Key <= Key.D9) ||
                (Keyboard.Modifiers != ModifierKeys.Shift) && (Key.NumPad0 <= e.Key && e.Key <= Key.NumPad9) ||
                (Key.Delete == e.Key) || (Key.Back == e.Key) || (Key.Tab == e.Key))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private static void onTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;
        }

        private static void textBoxPastingEventHandler(object sender, DataObjectPastingEventArgs e)
        {
            var textBox = (sender as TextBox);
            var clipboard = e.DataObject.GetData(typeof(string)) as string;
            if (textBox != null && !string.IsNullOrEmpty(clipboard))
            {
                bool ret = (Regex.IsMatch(clipboard, "^[0-9]+$")) || (Regex.IsMatch(clipboard, "[ ]"));
                if (!ret)
                {
                    e.CancelCommand();
                    e.Handled = true;
                }
            }
        }
        #endregion
    }
}
