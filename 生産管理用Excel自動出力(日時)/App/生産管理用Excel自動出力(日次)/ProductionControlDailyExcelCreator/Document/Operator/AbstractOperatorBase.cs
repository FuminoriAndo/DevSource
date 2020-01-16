//*************************************************************************************
//
//   ドキュメント操作
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using DTO.Base;

namespace ProductionControlDailyExcelCreator.Document.Operator
{
    /// <summary>
    /// ドキュメント操作(基底)
    /// </summary>
    internal abstract class AbstractOperatorBase
    {
        #region メソッド

        /// <summary>
        /// 技術特殊引合品チェックリスト(PMGQ010F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal abstract bool OutputPMGQ010FList<T>(T document) where T : DocumentDTOBase;

        /// <summary>
        /// 圧延計画リスト(加熱炉用）(PMPA280F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal abstract bool OutputPMPA280FList<T>(T document) where T : DocumentDTOBase;

        /// <summary>
        /// 未計画受注リスト(PMPD610F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal abstract bool OutputPMPD610FList<T>(T document) where T : DocumentDTOBase;

        /// <summary>
        /// 未採取リスト(PMPF070F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal abstract bool OutputPMPF070FList<T>(T document) where T : DocumentDTOBase;

        /// <summary>
        /// 余剰品引当可能在庫リスト(PMPF090F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal abstract bool OutputPMPF090FList<T>(T document) where T : DocumentDTOBase;

        /// <summary>
        /// 外販出荷計画リスト(PQGA186F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal abstract bool OutputPQGA186FList<T>(T document) where T : DocumentDTOBase;

        /// <summary>
        /// 外販出荷実績報告書(PQGA380F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal abstract bool OutputPQGA380FList<T>(T document) where T : DocumentDTOBase;

        /// <summary>
        /// 外販受注残リスト(PQGA420F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal abstract bool OutputPQGA420FList<T>(T document) where T : DocumentDTOBase;

        /// <summary>
        /// 外販出荷計画リスト(PQGA820F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal abstract bool OutputPQGA820FList<T>(T document) where T : DocumentDTOBase;

        /// <summary>
        /// 発生屑・単板・神鉄払出明細(SSYM040F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal abstract bool OutputSSYM040FList<T>(T document) where T : DocumentDTOBase;

        /// <summary>
        /// スクラップ外販明細(SSYM050F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal abstract bool OutputSSYM050FList<T>(T document) where T : DocumentDTOBase;

        /// <summary>
        /// 在庫売り受注一覧表(SSZA400B)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        protected internal abstract bool OutputSSZA400BList<T>(T document) where T : DocumentDTOBase;

        #endregion
    }
}
