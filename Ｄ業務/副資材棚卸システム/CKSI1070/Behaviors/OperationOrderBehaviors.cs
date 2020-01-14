using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Windows.Controls;
//*************************************************************************************
//
//   入力値制限ビヘイビア
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   15.08.08              DSK吉武   　 新規作成
//
//*************************************************************************************
namespace Behaviors
{
    #region クラス

    /// <summary>
    /// 操作順のビヘイビア
    /// </summary>
    public class OperationOrderBehaviors
    {
        public static readonly DependencyProperty IsNumericProperty =
                    DependencyProperty.RegisterAttached(
                        "IsNumeric", typeof(bool),
                        typeof(OperationOrderBehaviors),
                        new UIPropertyMetadata(false, IsNumericChanged)
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

        private static void IsNumericChanged
            (DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            var textBox = sender as TextBox;
            if (textBox == null) return;

            // イベントの登録・削除 
            textBox.KeyDown -= OnKeyDown;
            textBox.TextChanged -= OnTextChanged;
            textBox.LostFocus -= OnTextLostFocus;
            var newValue = (bool)e.NewValue;
            if (newValue)
            {
                textBox.KeyDown += OnKeyDown;
                textBox.TextChanged += OnTextChanged;
                textBox.LostFocus += OnTextLostFocus;
                DataObject.AddPastingHandler(textBox, TextBoxPastingEventHandler);
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

        private static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;
            if (textBox.Text.Contains(" "))
            {
                textBox.Text = textBox.Text.Replace(" ", "");
            }
        }

        private static void TextBoxPastingEventHandler(object sender, DataObjectPastingEventArgs e)
        {
            var textBox = (sender as TextBox);
            var clipboard = e.DataObject.GetData(typeof(string)) as string;
            if (textBox != null && !string.IsNullOrEmpty(clipboard))
            {
                bool ret = Regex.IsMatch(clipboard, "^[0-9]+$");
                if (!ret)
                {
                    e.CancelCommand();
                    e.Handled = true;
                }
            }
        }

        private static void OnTextLostFocus(object sender, System.EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;
            int num = 0;
            if (int.TryParse(textBox.Text, out num))
            {
                textBox.Text = num.ToString();
            }
        }
    }
   
    #endregion
  
}
