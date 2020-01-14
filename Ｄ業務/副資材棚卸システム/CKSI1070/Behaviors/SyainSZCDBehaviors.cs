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
    /// 社員所属コードのビヘイビア
    /// </summary>
    public class SyainSZCDBehaviors
    {
        public static readonly DependencyProperty IsAlphaNumericProperty =
                    DependencyProperty.RegisterAttached(
                        "IsAlphaNumeric", typeof(bool),
                        typeof(SyainSZCDBehaviors),
                        new UIPropertyMetadata(false, IsAlphaNumericChanged)
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

        private static void IsAlphaNumericChanged
            (DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            var textBox = sender as TextBox;
            if (textBox == null) return;

            var newValue = (bool)e.NewValue;
            if (newValue)
            {
                DataObject.AddPastingHandler(textBox, IsAlphaNumericPastingEventHandler);
            }
        }

        private static void IsAlphaNumericPastingEventHandler(object sender, DataObjectPastingEventArgs e)
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
   
    #endregion
  
}
