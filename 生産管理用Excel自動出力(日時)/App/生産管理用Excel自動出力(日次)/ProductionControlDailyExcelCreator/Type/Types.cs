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
namespace ProductionControlDailyExcelCreator.Types
{
    #region 列挙型

    /// <summary>
    /// モデルの種類
    /// </summary>
    public enum ModelType
    {
        /// <summary>
        /// PMGQ010F
        /// </summary>
        PMGQ010F = 0,

        /// <summary>
        /// PMPA280F
        /// </summary>
        PMPA280F = 1,

        /// <summary>
        /// PMPD610F
        /// </summary>
        PMPD610F = 2,

        /// <summary>
        /// PMPF070F
        /// </summary>
        PMPF070F = 3,

        /// <summary>
        /// PMPF090F
        /// </summary>
        PMPF090F = 4,

        /// <summary>
        /// PQGA186F
        /// </summary>
        PQGA186F = 5,

        /// <summary>
        /// PQGA380F
        /// </summary>
        PQGA380F = 6,

        /// <summary>
        /// PQGA420F
        /// </summary>
        PQGA420F = 7,

        /// <summary>
        /// PQGA820F
        /// </summary>
        PQGA820F = 8,

        /// <summary>
        /// SSYM040F
        /// </summary>
        SSYM040F = 9,

        /// <summary>
        /// SSYM050F
        /// </summary>
        SSYM050F = 10,

        /// <summary>
        /// SSZA400B
        /// </summary>
        SSZA400B = 11,
    }

    /// <summary>
    /// 技術特殊引合品チェックリスト(PMGQ010B)のExcelシートの種類
    /// </summary>
    internal enum PMGQ010BExcelSheetType
    {
        /// <summary>
        /// 詳細
        /// </summary>
        Detail
    }

    /// <summary>
    /// 圧延計画リスト(PMPA260B)のExcelシートの種類
    /// </summary>
    internal enum PMPA260BExcelSheetType
    {
        /// <summary>
        /// 勤務１
        /// </summary>
        Kinmu1 = 1,
        /// <summary>
        /// 勤務２
        /// </summary>
        Kinmu2 = 2,
        /// <summary>
        /// 勤務３
        /// </summary>
        Kinmu3 = 3,
        /// <summary>
        /// 勤務４
        /// </summary>
        Kinmu4 = 4,
    }

    /// <summary>
    /// 未計画受注リスト(PMPD330B)のExcelシートの種類
    /// </summary>
    internal enum PMPD330BExcelSheetType
    {
        /// <summary>
        /// 詳細
        /// </summary>
        Detail
    }

    /// <summary>
    /// 未採取リスト(PMPF070B)のExcelシートの種類
    /// </summary>
    internal enum PMPF070BExcelSheetType
    {
        /// <summary>
        /// 詳細
        /// </summary>
        Detail
    }

    /// <summary>
    /// 余剰品引当可能在庫リスト(PMPF090B)のExcelシートの種類
    /// </summary>
    internal enum PMPF090BExcelSheetType
    {
        /// <summary>
        /// 詳細
        /// </summary>
        Detail
    }

    /// <summary>
    /// 外販出荷計画リスト(PQGA186B)のExcelシートの種類
    /// </summary>
    internal enum PQGA186BExcelSheetType
    {
        /// <summary>
        /// 詳細
        /// </summary>
        Detail
    }

    /// <summary>
    /// 外販出荷実績報告書(PQGA380B)のExcelシートの種類
    /// </summary>
    internal enum PQGA380BExcelSheetType
    {
        /// <summary>
        /// 詳細
        /// </summary>
        Detail
    }

    /// <summary>
    /// 外販受注残(PQGA420B)のExcelシートの種類
    /// </summary>
    internal enum PQGA420BExcelSheetType
    {
        /// <summary>
        /// 詳細
        /// </summary>
        Detail
    }

    /// <summary>
    /// 外販出荷計画リスト(PQGA820B)のExcelシートの種類
    /// </summary>
    internal enum PQGA820BExcelSheetType
    {
        /// <summary>
        /// 詳細
        /// </summary>
        Detail
    }

    /// <summary>
    /// 発生屑・単板・神鉄払出明細注残(SSYM040B)のExcelシートの種類
    /// </summary>
    internal enum SSYM040BExcelSheetType
    {
        /// <summary>
        /// 詳細
        /// </summary>
        Detail
    }

    /// <summary>
    /// スクラップ外販明細(SSYM050B)のExcelシートの種類
    /// </summary>
    internal enum SSYM050BExcelSheetType
    {
        /// <summary>
        /// 詳細
        /// </summary>
        Detail
    }

    /// <summary>
    /// 在庫売り受注一覧表(SSZA040B)のExcelシートの種類
    /// </summary>
    internal enum SSZA040BExcelSheetType
    {
        /// <summary>
        /// 詳細
        /// </summary>
        Detail
    }

    #endregion

    #region ヘルパー

    /// <summary>
    /// 技術特殊引合品チェックリスト(PMGQ010B)のExcelシートの種類のヘルパー
    /// </summary>
    internal static class PMGQ010BExcelSheetTypeHelper
    {
        #region メソッド

        /// <summary>
        /// 技術特殊引合品チェックリスト(PMGQ010B)のExcelシートの種類のValueを取得する
        /// </summary>
        internal static string GetValue(this PMGQ010BExcelSheetType excelSheetType)
        {
            string[] excelSheets = { "詳細" };
            return excelSheets[(int)excelSheetType];
        }

        #endregion
    }

    /// <summary>
    /// 圧延計画リスト(PMPA260B)のExcelシートの種類のヘルパー
    /// </summary>
    internal static class PMPA260BExcelSheetTypeHelper
    {
        #region メソッド

        /// <summary>
        /// 圧延計画リスト(PMPA260B)のExcelシートの種類のValueを取得する
        /// </summary>
        internal static string GetValue(this PMPA260BExcelSheetType excelSheetType)
        {
            string[] excelSheets = { string.Empty, "勤務1", "勤務2", "勤務3", "勤務4" };
            return excelSheets[(int)excelSheetType];
        }

        #endregion
    }

    /// <summary>
    /// 未計画受注リスト(PMPD330B)のExcelシートの種類のヘルパー
    /// </summary>
    internal static class PMPD330BExcelSheetTypeHelper
    {
        #region メソッド

        /// <summary>
        /// 未計画受注リスト(PMPD330B)のExcelシートの種類のValueを取得する
        /// </summary>
        internal static string GetValue(this PMPD330BExcelSheetType excelSheetType)
        {
            string[] excelSheets = { "詳細" };
            return excelSheets[(int)excelSheetType];
        }

        #endregion
    }

    /// <summary>
    /// 未採取リスト(PMPF070B)のExcelシートの種類のヘルパー
    /// </summary>
    internal static class PMPF070BExcelSheetTypeHelper
    {
        #region メソッド

        /// <summary>
        /// 未採取リスト(PMPF070B)のExcelシートの種類のValueを取得する
        /// </summary>
        internal static string GetValue(this PMPF070BExcelSheetType excelSheetType)
        {
            string[] excelSheets = { "詳細" };
            return excelSheets[(int)excelSheetType];
        }

        #endregion
    }

    /// <summary>
    /// 余剰品引当可能在庫リスト(PMPF090B)のExcelシートのヘルパー
    /// </summary>
    internal static class PMPF090BExcelSheetTypeHelper
    {
        #region メソッド

        /// <summary>
        /// 余剰品引当可能在庫リスト(PMPF090B)のExcelシートの種類のValueを取得する
        /// </summary>
        internal static string GetValue(this PMPF090BExcelSheetType excelSheetType)
        {
            string[] excelSheets = { "詳細" };
            return excelSheets[(int)excelSheetType];
        }

        #endregion
    }

    /// <summary>
    /// 外販出荷計画リスト(PQGA186B)のExcelシートの種類のヘルパー
    /// </summary>
    internal static class PQGA186BExcelSheetTypeHelper
    {
        #region メソッド

        /// <summary>
        /// 外販出荷計画リスト(PQGA186B)のExcelシートの種類のValueを取得する
        /// </summary>
        internal static string GetValue(this PQGA186BExcelSheetType excelSheetType)
        {
            string[] excelSheets = { "詳細" };
            return excelSheets[(int)excelSheetType];
        }

        #endregion
    }

    /// <summary>
    /// 外販出荷実績報告書(PQGA380B)のExcelシートの種類のヘルパー
    /// </summary>
    internal static class PQGA380BExcelSheetTypeHelper
    {
        #region メソッド

        /// <summary>
        /// 外販出荷実績報告書(PQGA380B)のExcelシートの種類のValueを取得する
        /// </summary>
        internal static string GetValue(this PQGA380BExcelSheetType excelSheetType)
        {
            string[] excelSheets = { "詳細" };
            return excelSheets[(int)excelSheetType];
        }

        #endregion
    }

    /// <summary>
    /// 外販受注残リスト(PQGA420B)のExcelシートの種類のヘルパー
    /// </summary>
    internal static class PQGA420BExcelSheetTypeHelper
    {
        #region メソッド

        /// <summary>
        /// 外販受注残リスト(PQGA420B)のExcelシートの種類のValueを取得する
        /// </summary>
        internal static string GetValue(this PQGA420BExcelSheetType excelSheetType)
        {
            string[] excelSheets = { "詳細" };
            return excelSheets[(int)excelSheetType];
        }

        #endregion
    }

    /// <summary>
    /// 外販出荷計画リスト(PQGA820B)のExcelシートの種類のヘルパー
    /// </summary>
    internal static class PQGA820BExcelSheetTypeHelper
    {
        #region メソッド

        /// <summary>
        /// 外販出荷計画リスト(PQGA820B)のExcelシートの種類のValueを取得する
        /// </summary>
        internal static string GetValue(this PQGA820BExcelSheetType excelSheetType)
        {
            string[] excelSheets = { "詳細" };
            return excelSheets[(int)excelSheetType];
        }

        #endregion
    }

    /// <summary>
    /// 発生屑・単板・神鉄払出明細注残(SSYM040B)のExcelシートの種類のヘルパー
    /// </summary>
    internal static class SSYM040BExcelSheetTypeHelper
    {
        #region メソッド

        /// <summary>
        /// 発生屑・単板・神鉄払出明細注残(SSYM040B)のExcelシートの種類のValueを取得する
        /// </summary>
        internal static string GetValue(this SSYM040BExcelSheetType excelSheetType)
        {
            string[] excelSheets = { "詳細" };
            return excelSheets[(int)excelSheetType];
        }

        #endregion
    }

    /// <summary>
    /// スクラップ外販明細(SSYM050B)のExcelシートの種類のヘルパー
    /// </summary>
    internal static class SSYM050BExcelSheetTypeHelper
    {
        #region メソッド

        /// <summary>
        /// スクラップ外販明細(SSYM050B)のExcelシートの種類のValueを取得する
        /// </summary>
        internal static string GetValue(this SSYM050BExcelSheetType excelSheetType)
        {
            string[] excelSheets = { "詳細" };
            return excelSheets[(int)excelSheetType];
        }

        #endregion
    }

    /// <summary>
    /// 在庫売り受注一覧表(SSZA040B)のExcelシートの種類のヘルパー
    /// </summary>
    internal static class SSZA040BExcelSheetTypeHelper
    {
        #region メソッド

        /// <summary>
        /// 在庫売り受注一覧表(SSZA040B)のExcelシートの種類のValueを取得する
        /// </summary>
        internal static string GetValue(this SSZA040BExcelSheetType excelSheetType)
        {
            string[] excelSheets = { "詳細" };
            return excelSheets[(int)excelSheetType];
        }

        #endregion
    }

    #endregion
}

