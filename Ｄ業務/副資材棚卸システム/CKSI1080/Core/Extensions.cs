using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
//*************************************************************************************
//
//   拡張メソッド
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ  　担当者　　 　修正内容
//   16.12.12              DSK   　     新規作成
//
//*************************************************************************************

namespace Core
{
    /// <summary>
    /// 拡張メソッド
    /// </summary>
    internal static class Extensions
    {
        #region メソッド

        internal static string AttributeValue(this XElement e, string attributeName)
        {
            var attr = e.Attribute(attributeName);
            if (attr == null)
                return string.Empty;
            else
                return attr.Value;
        }

        internal static string AttributeValue(this XElement e, string attributeName, string defaultValue)
        {
            var attr = e.Attribute(attributeName);
            if (attr == null)
                return defaultValue;
            else
                return attr.Value;
        }

        internal static T AttributeValue<T>(this XElement e, string attributeName)
        {
            var attr = e.Attribute(attributeName);
            if (attr == null)
                return default(T);
            else
                return (T)Convert.ChangeType(attr.Value, typeof(T));
        }

        internal static T AttributeValue<T>(this XElement e, string attributeName, T defaultValue)
        {
            var attr = e.Attribute(attributeName);
            if (attr == null)
                return defaultValue;
            else
                return (T)Convert.ChangeType(attr.Value, typeof(T));
        }

        #endregion
    }
}