using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Utility.Types;
using System.Text;
//*************************************************************************************
//
//   拡張メソッド
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************

namespace Utility.Core
{
    /// <summary>
    /// 拡張メソッド
    /// </summary>
    public static class UtilExtensions
    {
        #region メソッド

        /// <summary>
        /// Nullのコレクションを空のコレクションで返却する
        /// </summary>
        /// <typeparam name="T">コレクションが管理するオブジェクトの型</typeparam>
        /// <param name="collection">IEnumerable型のオブジェクト</param>
        /// <returns>IEnumerable型のオブジェクト</returns>
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> collection)
        {
            try
            {
                return collection ?? Enumerable.Empty<T>();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// IEnumerableを実装したオブジェクトの要素に対して繰り返し処理を行う
        /// </summary>
        /// <typeparam name="T">IEnumerableインターフェースが管理するオブジェクトの型</typeparam>
        /// <param name="source">IEnumerableを実装したオブジェクト</param>
        /// <param name="action">繰り返し処理</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            try
            {
                foreach (T item in source)
                {
                    action(item);
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// IEnumerable型のコレクションの要素を追加する
        /// </summary>
        /// <typeparam name="T">コレクションが管理するオブジェクトの型</typeparam>
        /// <param name="collection">追加先のコレクション</param>
        /// <param name="source">追加元のコレクション</param>
        /// <returns>追加後のコレクション</returns>
        public static ICollection<T> AddRange<T>(this ICollection<T> collection, IEnumerable<T> source)
        {
            try
            {
                foreach (var item in source)
                {
                    collection.Add(item);
                }

                return collection;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// XMLの属性値の取得
        /// </summary>
        /// <param name="e">XMLエレメント</param>
        /// <param name="attributeName">XMLの属性名</param>
        /// <returns>XMLの属性値</returns>
        public static string AttributeValue(this XElement e, string attributeName)
        {
            try
            {
                var attr = e.Attribute(attributeName);
                if (attr == null)
                    return string.Empty;
                else
                    return attr.Value;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// XMLの属性値の取得
        /// </summary>
        /// <param name="e">XMLエレメント</param>
        /// <param name="attributeName">XMLの属性名</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>XMLの属性値</returns>
        public static string AttributeValue(this XElement e, string attributeName, string defaultValue)
        {
            try
            {
                var attr = e.Attribute(attributeName);
                if (attr == null)
                    return defaultValue;
                else
                    return attr.Value;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// XMLの属性値の取得
        /// </summary>
        /// <typeparam name="T">任意のオブジェクトの型</typeparam>
        /// <param name="e">XMLエレメント</param>
        /// <param name="attributeName">XMLの属性名</param>
        /// <returns>XMLの属性値(任意のオブジェクト)</returns>
        public static T AttributeValue<T>(this XElement e, string attributeName)
        {
            try
            {
                var attr = e.Attribute(attributeName);
                if (attr == null)
                    return default(T);
                else
                    return (T)Convert.ChangeType(attr.Value, typeof(T));
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// XMLの属性値の取得
        /// </summary>
        /// <typeparam name="T">任意のオブジェクト</typeparam>
        /// <param name="e">XMLエレメント</param>
        /// <param name="attributeName">XMLの属性名</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>XMLの属性値(任意のオブジェクト)</returns>
        public static T AttributeValue<T>(this XElement e, string attributeName, T defaultValue)
        {
            try
            {
                var attr = e.Attribute(attributeName);
                if (attr == null)
                    return defaultValue;
                else
                    return (T)Convert.ChangeType(attr.Value, typeof(T));
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ディクショナリから指定したキーの値を取得
        /// </summary>
        /// <typeparam name="TKey">任意のキー</typeparam>
        /// <typeparam name="TValue">任意の値</typeparam>
        /// <param name="dictionary">ディクショナリ</param>
        /// <param name="key">任意のキー</param>
        /// <returns>任意の値</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            try
            {
                TValue result;
                // keyが見つからない場合はdefault(TValue) を代入
                dictionary.TryGetValue(key, out result);
                return result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 任意の値が下限～上限の間に入っているかを判定する
        /// </summary>
        /// <typeparam name="T">任意のオブジェクト</typeparam>
        /// <param name="value">任意の値</param>
        /// <param name="min">下限値</param>
        /// <param name="max">上限値</param>
        /// <returns>結果</returns>
        public static bool InBetween<T>(this T value, T min, T max) where T : IComparable<T>
        {
            try
            {
                return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// メソッドチェーン
        /// </summary>
        /// <typeparam name="T">任意のオブジェクト</typeparam>
        /// <param name="source">チェーン元</param>
        /// <param name="call">チェーンアクション</param>
        /// <returns>アクション後のチェーン</returns>
        public static T Chain<T>(this T source, Action<T> call)
        {
            try
            {
                if (source == null)
                    return source;
                call(source);
                return source;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// データテーブルからIEnumerable型のエンティティへのマッピング
        /// </summary>
        /// <typeparam name="T">マッピングするクラスの型</typeparam>
        /// <param name="dt">データテーブル</param>
        /// <returns>マッピングしたIEnumerable型のDTO</returns>
        public static IEnumerable<T> MapEnumerableEntity<T>(this DataTable dt) where T : new()
        {
            int i = 0;
            while (i < dt.Rows.Count)
            {
                var r = dt.Rows[i].MapEntity<T>();
                i++;
                yield return r;
            }
        }

        /// <summary>
        /// データローからエンティティへのマッピング
        /// </summary>
        /// <typeparam name="T">マッピングするクラスの型</typeparam>
        /// <param name="row">データロー</param>
        /// <returns>マッピングしたDTO</returns>
        public static T MapEntity<T>(this DataRow row) where T : new()
        {
            try
            {
                var resultData = new T();
                var t = resultData.GetType();
                var pis = t.GetProperties();

                foreach (var pi in pis)
                {
                    if(row.Table.Columns.Contains(pi.Name) && !row.IsNull(pi.Name))
                        pi.SetValue(resultData, row[pi.Name], null);
                }

                return resultData;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ファイルパスの拡張子を取得する
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>拡張子</returns>
        public static string GetExtension(this string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                return fileInfo.Extension;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 指定のバイト数になるまで末尾を空白文字で埋める
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <param name="encode">エンコーディング種類</param>
        /// <param name="byteLength">バイト数</param>
        /// <returns>末尾を空白文字で埋めた後の文字列</returns>
        internal static string PadRightSJIS(this string str, EncodingType encode, int byteLength)
        {
            var enc = Encoding.GetEncoding(encode.GetValue());
            var strLen = enc.GetByteCount(str);
            if (strLen < byteLength)
            {
                return str + new string(' ', byteLength - strLen);
            }

            return str;
        }

        /// <summary>
        /// 半角数字を全角数字に変換する
        /// </summary>
        /// <param name="s">変換する文字列</param>
        /// <returns>変換後の文字列</returns>
        public static string HanToZenNum(this string s)
        {
            try
            {
                return Regex.Replace(s, "[0-9]", p => ((char)(p.Value[0] - '0' + '０')).ToString());
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 全角数字を半角数字に変換する
        /// </summary>
        /// <param name="s">変換する文字列</param>
        /// <returns>変換後の文字列</returns>
        public static string ZenToHanNum(this string s)
        {
            try
            {
                return Regex.Replace(s, "[０-９]", p => ((char)(p.Value[0] - '０' + '0')).ToString());
            }

            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 半角アルファベットを全角アルファベットに変換する
        /// </summary>
        /// <param name="s">変換する文字列</param>
        /// <returns>変換後の文字列</returns>
        public static string HanToZenAlpha(this string s)
        {
            try
            {
                var str = Regex.Replace(s, "[a-z]", p => ((char)(p.Value[0] - 'a' + 'ａ')).ToString());
                return Regex.Replace(str, "[A-Z]", p => ((char)(p.Value[0] - 'A' + 'Ａ')).ToString());
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 全角アルファベットを半角アルファベットに変換する
        /// </summary>
        /// <param name="s">変換する文字列</param>
        /// <returns>変換後の文字列</returns>
        public static string ZenToHanAlpha(this string s)
        {
            try
            {
                var str = Regex.Replace(s, "[ａ-ｚ]", p => ((char)(p.Value[0] - 'ａ' + 'a')).ToString());
                return Regex.Replace(str, "[Ａ-Ｚ]", p => ((char)(p.Value[0] - 'Ａ' + 'A')).ToString());
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 文字列を指定したデリミタでスプリットする
        /// </summary>
        /// <param name="source">対象の文字列</param>
        /// <param name="delimiter">デリミタ</param>
        /// <returns>"スプリットした文字列のリスト</returns>
        public static IList<string> SplitToList(this string source, string delimiter)
        {
            IList<string> ret = new List<string>();

            try
            {
                if (!string.IsNullOrEmpty(source))
                {
                    string[] array = source.Split(Convert.ToChar(delimiter));
                    ret.AddRange(array);
                }

                return ret;
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 文字列の先頭にある「"0"」を削除する
        /// </summary>
        /// <param name="source">削除前の文字列</param>
        /// <returns>削除後の文字列</returns>
        public static string TrimStartZero(this string source)
        {
            try
            {
                return source.TrimStart(Convert.ToChar(((int)NumericType.Zero).ToString()));
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 文字列の先頭にある「"0"」を削除し、文字列をInt型の値に変換する。
        /// </summary>
        /// <param name="source">変換前(文字列)</param>
        /// <returns>変換後(値)</returns>
        public static int TrimStartZeroWithConvertToInt(this string source)
        {
            try
            {
                var target = source.TrimStartZero().Trim();
                return target != string.Empty ? int.Parse(target) : 0;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}