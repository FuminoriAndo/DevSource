//*************************************************************************************
//
//   共通定義
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
namespace Utility.Types
{
    #region 列挙型

    /// <summary>
    /// エンコードの種類
    /// </summary>
    public enum EncodingType
    {
        /// <summary>
        /// ASCII
        /// </summary>
        ASCII,
        /// <summary>
        /// EUC
        /// </summary>
        EUC,
        /// <summary>
        /// JIS
        /// </summary>
        JIS,
        /// <summary>
        /// SJIS
        /// </summary>
        SJIS,
        /// <summary>
        /// UTF7
        /// </summary>
        UTF7,
        /// <summary>
        /// UTF8
        /// </summary>
        UTF8,
        /// <summary>
        /// UTF16
        /// </summary>
        UTF16

    }

    /// <summary>
    /// 記号の種類
    /// </summary>
    public enum SymbolType
    {
        /// <summary>
        /// From～To
        /// </summary>
        FromTo,

        /// <summary>
        /// カンマ
        /// </summary>
        Comma,

        /// <summary>
        /// ハイフン
        /// </summary>
        Dash,

        /// <summary>
        /// スラッシュ
        /// </summary>
        Slash,

        /// <summary>
        /// アンダースコア
        /// </summary>
        Underscore,

        /// <summary>
        /// 点
        /// </summary>
        Point,

        /// <summary>
        /// ピリオド
        /// </summary>
        Period,

        /// <summary>
        /// X
        /// </summary>
        Multiple,

        /// <summary>
        /// +
        /// </summary>
        Plus
    }

    /// <summary>
    /// 拡張子の種類
    /// </summary>
    public enum ExtensionType
    {
        /// <summary>
        /// Text
        /// </summary>
        TEXT,

        /// <summary>
        /// Excel(XLS)
        /// </summary>
        XLS,

        /// <summary>
        /// Excel(XLSX)
        /// </summary>
        XLSX,

        /// <summary>
        /// Log
        /// </summary>
        LOG
    }

    /// <summary>
    /// 色の種類
    /// </summary>
    public enum ColorType
    {
        /// <summary>
        /// 黒
        /// </summary>
        Black
    }

    /// <summary>
    /// 数値の種類
    /// </summary>
    public enum NumericType
    {
        /// <summary>
        /// 0
        /// </summary>
        Zero = 0,
        /// <summary>
        /// 1
        /// </summary>
        One = 1,
        /// <summary>
        /// 2
        /// </summary>
        Two = 2,
        /// <summary>
        /// 3
        /// </summary>
        Three = 3,
        /// <summary>
        /// 4
        /// </summary>
        Four = 4,
        /// <summary>
        /// 5
        /// </summary>
        Five = 5,
        /// <summary>
        /// 6
        /// </summary>
        Six = 6,
        /// <summary>
        /// 7
        /// </summary>
        Seven = 7,
        /// <summary>
        /// 8
        /// </summary>
        Eight = 8,
        /// <summary>
        /// 9
        /// </summary>
        Nine = 9,
        /// <summary>
        /// 10
        /// </summary>
        Ten = 10,
        /// <summary>
        /// 11
        /// </summary>
        Eleven = 11,
        /// <summary>
        /// 12
        /// </summary>
        Twelve = 12,
        /// <summary>
        /// 13
        /// </summary>
        Thirteen = 13,
        /// <summary>
        /// 14
        /// </summary>
        Fourteen = 14,
        /// <summary>
        /// 15
        /// </summary>
        Fifteen = 15
    }

    /// <summary>
    /// システム分類
    /// </summary>
    public enum SystemCategory
    {
        /// <summary>
        /// 資材
        /// </summary>
        Sizai = 1,
        /// <summary>
        /// 部品
        /// </summary>
        Buhin = 2
    }

    /// <summary>
    /// 操作状況
    /// </summary>
    public enum OperationConditionType
    {
        /// <summary>
        /// 修正
        /// </summary>
        Modify = 0,
        /// <summary>
        /// 確定
        /// </summary>
        Fix = 1
    }

    #endregion

    #region ヘルパー

    /// <summary>
    /// エンコーディング種類のヘルパー
    /// </summary>
    public static class EncodingTypeHelper
    {
        #region メソッド

        /// <summary>
        /// エンコーディング種類のValueを取得する
        /// </summary>
        public static string GetValue(this EncodingType encoding)
        {
            string[] encodingTypes = { "ASCII", "EUC-JP", "ISO-2022-JP", "SHIFT-JIS", "UTF-7", "UTF-8", "UNICODE" };
            return encodingTypes[(int)encoding];
        }

        #endregion
    }

    /// <summary>
    /// 記号のヘルパー
    /// </summary>
    public static class SymbolTypeHelper
    {
        #region メソッド

        /// <summary>
        /// 拡張子のValueを取得する
        /// </summary>
        public static string GetValue(this SymbolType symbolType)
        {
            string[] symbols = { "～", ",", "-", "/", "_", "、", ".", "X", "+" };
            return symbols[(int)symbolType];
        }

        #endregion
    }

    /// <summary>
    /// 拡張子のヘルパー
    /// </summary>
    public static class ExtensionTypeHelper
    {

        #region メソッド

        /// <summary>
        /// 拡張子のValueを取得する
        /// </summary>
        public static string GetValue(this ExtensionType extensionType)
        {
            string[] extensions = { ".txt", ".xls", ".xlsx", ".log" };
            return extensions[(int)extensionType];
        }

        #endregion
    }

    /// <summary>
    /// 色の種類のヘルパー
    /// </summary>
    public static class ColorTypeHelper
    {
        #region メソッド

        /// <summary>
        /// 色の種類を文字列で取得
        /// </summary>
        public static string GetStringValue(this ColorType value)
        {
            string[] values = { "Black" };
            return values[(int)value];
        }

        #endregion
    }

    /// <summary>
    /// 数値のヘルパー
    /// </summary>
    public static class NumericTypeHelper
    {
        #region メソッド

        /// <summary>
        /// 数値を文字列で取得
        /// </summary>
        public static string GetStringValue(this NumericType value)
        {
            return ((int)value).ToString();
        }

        #endregion
    }

    #endregion
}