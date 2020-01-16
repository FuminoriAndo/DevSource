//*************************************************************************************
//
//   ドキュメントマネージャ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using Document.Types;
using DTO.Base;
using ProductionControlDailyExcelCreator.Document.Operator;
using ProductionControlDocumentOperator.Document.Operator;

namespace ProductionControlDailyExcelCreator.Document
{
    /// <summary>
    /// ドキュメントマネージャ
    /// </summary>
    public class DocumentManager
    {
        #region フィールド

        /// <summary>
        /// ドキュメント操作用オブジェクト
        /// </summary>
        private AbstractOperatorBase baseOperator = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">ドキュメント種類</param>
        public DocumentManager(DocumentType type)
        {
            // ドキュメント操作用オブジェクトの生成
            baseOperator = createOperator(type);
        }

        #endregion

        #region メソッド

        /// <summary>
        /// ドキュメント操作用オブジェクトを生成する
        /// </summary>
        /// <param name="type">ドキュメント種類</param>
        /// <returns>ドキュメント操作用オブジェクト</returns>
        private AbstractOperatorBase createOperator(DocumentType type)
        {
            AbstractOperatorBase baseOperator = null;

            switch (type)
            {
                // Excel
                case DocumentType.Excel:
                    baseOperator = new ExcelOperator();
                    break;
                default:
                    break;
            }

            return baseOperator;
        }

        /// <summary>
        /// 技術特殊引合品チェックリスト(PMGQ010F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        public bool OutputPMGQ010FList<T>(T document) where T : DocumentDTOBase
        {
            try
            {
                return baseOperator.OutputPMGQ010FList(document);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 圧延計画リスト(加熱炉用）(PMPA280F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        public bool OutputPMPA280FList<T>(T document) where T : DocumentDTOBase
        {
            try
            {
                return baseOperator.OutputPMPA280FList(document);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 未計画受注リスト(PMPD610F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        public bool OutputPMPD610FList<T>(T document) where T : DocumentDTOBase
        {
            try
            {
                return baseOperator.OutputPMPD610FList(document);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 未採取リスト(PMPF070F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        public bool OutputPMPF070FList<T>(T document) where T : DocumentDTOBase
        {
            try
            {
                return baseOperator.OutputPMPF070FList(document);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 余剰品引当可能在庫リスト(PMPF090F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        public bool OutputPMPF090FList<T>(T document) where T : DocumentDTOBase
        {
            try
            {
                return baseOperator.OutputPMPF090FList(document);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 外販出荷計画リスト(PQGA186F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        public bool OutputPQGA186FList<T>(T document) where T : DocumentDTOBase
        {
            try
            {
                return baseOperator.OutputPQGA186FList(document);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 外販出荷実績報告書(PQGA380F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        public bool OutputPQGA380FList<T>(T document) where T : DocumentDTOBase
        {
            try
            {
                return baseOperator.OutputPQGA380FList(document);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 外販受注残リスト(PQGA420F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        public bool OutputPQGA420FList<T>(T document) where T : DocumentDTOBase
        {
            try
            {
                return baseOperator.OutputPQGA420FList(document);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 外販出荷計画リスト(PQGA820F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        public bool OutputPQGA820FList<T>(T document) where T : DocumentDTOBase
        {
            try
            {
                return baseOperator.OutputPQGA820FList(document);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 発生屑・単板・神鉄払出明細(SSYM040F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        public bool OutputSSYM040FList<T>(T document) where T : DocumentDTOBase
        {
            try
            {
                return baseOperator.OutputSSYM040FList(document);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スクラップ外販明細(SSYM050F)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        public bool OutputSSYM050FList<T>(T document) where T : DocumentDTOBase
        {
            try
            {
                return baseOperator.OutputSSYM050FList(document);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 在庫売り受注一覧表(SSZA400B)の出力
        /// </summary>
        /// <param name="document">ドキュメントのDTO</param>
        /// <returns>結果</returns>
        public bool OutputSSZA400BList<T>(T document) where T : DocumentDTOBase
        {
            try
            {
                return baseOperator.OutputSSZA400BList(document);
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}

