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
//   17.02.01              DSK   　     新規作成
//
//*************************************************************************************
namespace Behaviors
{
    #region クラス

    /// <summary>
    /// システム分類のビヘイビア
    /// </summary>
    public class SystemCategoryBehaviors
    {
        public static readonly DependencyProperty IsNumericProperty =
                    DependencyProperty.RegisterAttached(
                        "IsNumeric", typeof(bool),
                        typeof(SystemCategoryBehaviors),
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
            textBox.LostFocus -= onTextLostFocus;
            var newValue = (bool)e.NewValue;
            if (newValue)
            {
                textBox.KeyDown += OnKeyDown;
                textBox.TextChanged += onTextChanged;
                textBox.LostFocus += onTextLostFocus;
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
            if (textBox.Text.Contains(" "))
            {
                textBox.Text = textBox.Text.Replace(" ", "");
            }
        }

        private static void textBoxPastingEventHandler(object sender, DataObjectPastingEventArgs e)
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

        private static void onTextLostFocus(object sender, System.EventArgs e)
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

    public class SystemCategoryNameBehaviors
    {
        public static readonly DependencyProperty IsByteCheckProperty =
                    DependencyProperty.RegisterAttached(
                        "IsByteCheck", typeof(bool),
                        typeof(SystemCategoryNameBehaviors),
                        new UIPropertyMetadata(false, isByteCheckChanged)
                    );

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetIsByteCheck(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsByteCheckProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static void SetIsByteCheck(DependencyObject obj, bool value)
        {
            obj.SetValue(IsByteCheckProperty, value);
        }

        private static void isByteCheckChanged
            (DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            var textBox = sender as TextBox;
            if (textBox == null) return;

            // イベントの登録・削除 
            var newValue = (bool)e.NewValue;
            if (newValue)
            {
                textBox.PreviewKeyDown += OnKeyDown;
            }
        }

        static void OnKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            if ((Key.Delete == e.Key) || (Key.Back == e.Key) || (Key.Tab == e.Key) || (Key.Enter == e.Key) ||
                (Key.Left == e.Key) || (Key.Right == e.Key) || (Key.Up == e.Key) || (Key.Down == e.Key) ||
                (Key.Home == e.Key) || (Key.End == e.Key) || (Key.PageUp == e.Key) || (Key.PageDown == e.Key) ||
                (Key.OemAuto == e.Key || (Key.Escape == e.Key)))
            {
                return;
            }
            else if (e.Key == Key.ImeProcessed)
            {
                if ((Key.Delete == e.ImeProcessedKey) || (Key.Back == e.ImeProcessedKey) || (Key.Tab == e.ImeProcessedKey) || (Key.Enter == e.ImeProcessedKey) ||
                    (Key.Left == e.ImeProcessedKey) || (Key.Right == e.ImeProcessedKey) || (Key.Up == e.ImeProcessedKey) || (Key.Down == e.ImeProcessedKey) ||
                    (Key.Home == e.ImeProcessedKey) || (Key.End == e.ImeProcessedKey) || (Key.PageUp == e.ImeProcessedKey) || (Key.PageDown == e.ImeProcessedKey) ||
                    (Key.OemAuto == e.ImeProcessedKey) || (Key.Escape == e.ImeProcessedKey))
                {
                    return;
                }
            }

            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("Shift_JIS");
            byte[] btBytes = encoding.GetBytes(textBox.Text.TrimEnd());
            
            int length = btBytes.Length;
            if (30 <= length)
            {
                e.Handled = true;
            }
            else if (29 == length && e.Key == Key.ImeProcessed)
            {
                e.Handled = true;
            }
        }
    }
   
    #endregion
  
}
