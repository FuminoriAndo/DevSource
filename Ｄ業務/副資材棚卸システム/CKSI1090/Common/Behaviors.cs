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
//   17.04.28              DSK   　     新規作成
//
//*************************************************************************************
namespace CKSI1090.Common.Behaviors
{
    /// <summary>
    /// 作業日時の入力値制限ビヘイビア
    /// </summary>
    public class OperationDateBehaviors
    {
        public static readonly DependencyProperty IsNumericProperty =
                    DependencyProperty.RegisterAttached(
                        "IsNumeric", typeof(bool),
                        typeof(OperationDateBehaviors),
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
    }

    /// <summary>
    /// 品目CDの入力値制限ビヘイビア
    /// </summary>
    public class HinmokuCodeBehaviors
    {
        public static readonly DependencyProperty IsAlphaNumericProperty =
                    DependencyProperty.RegisterAttached(
                        "IsAlphaNumeric", typeof(bool),
                        typeof(HinmokuCodeBehaviors),
                        new UIPropertyMetadata(false, isAlphaNumericChanged)
                    );

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetIsAlphaNumeric(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAlphaNumericProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static void SetIsAlphaNumeric(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAlphaNumericProperty, value);
        }

        private static void isAlphaNumericChanged
            (DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            var textBox = sender as TextBox;
            if (textBox == null) return;

            var newValue = (bool)e.NewValue;
            if (newValue)
            {
                DataObject.AddPastingHandler(textBox, isAlphaNumericPastingEventHandler);
            }
        }

        private static void isAlphaNumericPastingEventHandler(object sender, DataObjectPastingEventArgs e)
        {
            var textBox = (sender as TextBox);
            var clipboard = e.DataObject.GetData(typeof(string)) as string;
            if (textBox != null && !string.IsNullOrEmpty(clipboard))
            {
                bool ret = Regex.IsMatch(clipboard, "^[a-zA-Z0-9]+$") || (Regex.IsMatch(clipboard, "[ ]"));
                if (!ret)
                {
                    e.CancelCommand();
                    e.Handled = true;
                }
            }
        }
    }

    /// <summary>
    /// 業者CDの入力値制限ビヘイビア
    /// </summary>
    public class GyosyaCodeBehaviors
    {
        public static readonly DependencyProperty IsAlphaNumericProperty =
                    DependencyProperty.RegisterAttached(
                        "IsAlphaNumeric", typeof(bool),
                        typeof(GyosyaCodeBehaviors),
                        new UIPropertyMetadata(false, isAlphaNumericChanged)
                    );

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetIsAlphaNumeric(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAlphaNumericProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static void SetIsAlphaNumeric(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAlphaNumericProperty, value);
        }

        private static void isAlphaNumericChanged
            (DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            var textBox = sender as TextBox;
            if (textBox == null) return;

            var newValue = (bool)e.NewValue;
            if (newValue)
            {
                DataObject.AddPastingHandler(textBox, isAlphaNumericPastingEventHandler);
            }
        }

        private static void isAlphaNumericPastingEventHandler(object sender, DataObjectPastingEventArgs e)
        {
            var textBox = (sender as TextBox);
            var clipboard = e.DataObject.GetData(typeof(string)) as string;
            if (textBox != null && !string.IsNullOrEmpty(clipboard))
            {
                bool ret = Regex.IsMatch(clipboard, "^[a-zA-Z0-9]+$") || (Regex.IsMatch(clipboard, "[ ]"));
                if (!ret)
                {
                    e.CancelCommand();
                    e.Handled = true;
                }
            }
        }
    }
}

