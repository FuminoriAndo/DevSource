//*************************************************************************************
//
///  各操作のExcel操作用カスタムDTO
///  
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using DTO.Base;
using DTO.Excel;
using ProductionControlDailyExcelCreator.Common.Container;

namespace ProductionControlDailyExcelCreator.DTO
{
    /// <summary>
    /// 生産管理のExcel出力ドキュメントDTO
    /// </summary>
    /// <typeparam name="T">DTOの基底を継承した任意のオブジェクトの型</typeparam>
    public class ProductionControlExcelOutputDocument<T> : ExcelOutputDocumentDTO<T> where T : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputList">出力データ</param>
        public ProductionControlExcelOutputDocument(T outputData) : base(outputData)
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 年
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// ドキュメントに対する操作完了時のコンテナ
        /// </summary>
        public IDocumentOperationContainer DocumentOperationContainer { get; set; }

        #endregion
    }

    /// <summary>
    /// 技術特殊引合品チェックリスト(PMGQ010F)のExcel出力ドキュメントDTO
    /// </summary>
    /// <typeparam name="T">DTOの基底を継承した任意のオブジェクトの型</typeparam>
    public class PMGQ010FExcelOutputDocument<T> : ProductionControlExcelOutputDocument<T> where T : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputList">出力データ</param>
        public PMGQ010FExcelOutputDocument(T outputData) : base(outputData)
        {
        }

        #endregion
    }

    /// <summary>
    /// 圧延計画リスト(加熱炉用）(PMPA280F)のExcel出力ドキュメントDTO
    /// </summary>
    /// <typeparam name="T">DTOの基底を継承した任意のオブジェクトの型</typeparam>
    public class PMPA280FExcelOutputDocument<T> : ProductionControlExcelOutputDocument<T> where T : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputList">出力データ</param>
        public PMPA280FExcelOutputDocument(T outputData) : base(outputData)
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// RCNo(ロールチャンス№)
        /// </summary>
        public string RCNo { get; set; }

        /// <summary>
        /// 現在時刻
        /// </summary>
        public DateTime DateTimeNow { get; set; }

        #endregion
    }

    /// <summary>
    /// 未計画受注リスト(PMPD610F)のExcel出力ドキュメントDTO
    /// </summary>
    /// <typeparam name="T">DTOの基底を継承した任意のオブジェクトの型</typeparam>
    public class PMPD610FExcelOutputDocument<T> : ProductionControlExcelOutputDocument<T> where T : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputList">出力データ</param>
        public PMPD610FExcelOutputDocument(T outputData) : base(outputData)
        {
        }

        #endregion
    }

    /// <summary>
    /// 未採取リスト(PMPF070F)のExcel出力ドキュメントDTO
    /// </summary>
    /// <typeparam name="T">DTOの基底を継承した任意のオブジェクトの型</typeparam>
    public class PMPF070FExcelOutputDocument<T> : ProductionControlExcelOutputDocument<T> where T : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputList">出力データ</param>
        public PMPF070FExcelOutputDocument(T outputData) : base(outputData)
        {
        }

        #endregion
    }

    /// <summary>
    /// 余剰品引当可能在庫リスト(PMPF090F)のExcel出力ドキュメントDTO
    /// </summary>
    /// <typeparam name="T">DTOの基底を継承した任意のオブジェクトの型</typeparam>
    public class PMPF090FExcelOutputDocument<T> : ProductionControlExcelOutputDocument<T> where T : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputList">出力データ</param>
        public PMPF090FExcelOutputDocument(T outputData) : base(outputData)
        {
        }

        #endregion
    }

    /// <summary>
    /// 外販出荷計画リスト(PQGA186F)のExcel出力ドキュメントDTO
    /// </summary>
    /// <typeparam name="T">DTOの基底を継承した任意のオブジェクトの型</typeparam>
    public class PQGA186FExcelOutputDocument<T> : ProductionControlExcelOutputDocument<T> where T : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputList">出力データ</param>
        public PQGA186FExcelOutputDocument(T outputData) : base(outputData)
        {
        }

        #endregion
    }

    /// <summary>
    /// 外販出荷実績報告書(PQGA380F)のExcel出力ドキュメントDTO
    /// </summary>
    /// <typeparam name="T">DTOの基底を継承した任意のオブジェクトの型</typeparam>
    public class PQGA380FExcelOutputDocument<T> : ProductionControlExcelOutputDocument<T> where T : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputList">出力データ</param>
        public PQGA380FExcelOutputDocument(T outputData) : base(outputData)
        {
        }

        #endregion
    }

    /// <summary>
    /// 外販受注残リスト(PQGA420F)のExcel出力ドキュメントDTO
    /// </summary>
    /// <typeparam name="T">DTOの基底を継承した任意のオブジェクトの型</typeparam>
    public class PQGA420FExcelOutputDocument<T> : ProductionControlExcelOutputDocument<T> where T : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputList">出力データ</param>
        public PQGA420FExcelOutputDocument(T outputData) : base(outputData)
        {
        }

        #endregion
    }

    /// <summary>
    /// 外販出荷計画リスト(PQGA820F)のExcel出力ドキュメントDTO
    /// </summary>
    /// <typeparam name="T">DTOの基底を継承した任意のオブジェクトの型</typeparam>
    public class PQGA820FExcelOutputDocument<T> : ProductionControlExcelOutputDocument<T> where T : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputList">出力データ</param>
        public PQGA820FExcelOutputDocument(T outputData) : base(outputData)
        {
        }

        #endregion
    }

    /// <summary>
    /// 発生屑・単板・伸鉄払出明細(SSYM040F)のExcel出力ドキュメントDTO
    /// </summary>
    /// <typeparam name="T">DTOの基底を継承した任意のオブジェクトの型</typeparam>
    public class SSYM040FExcelOutputDocument<T> : ProductionControlExcelOutputDocument<T> where T : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputList">出力データ</param>
        public SSYM040FExcelOutputDocument(T outputData) : base(outputData)
        {
        }

        #endregion
    }

    /// <summary>
    /// スクラップ外販明細(SSYM050F)のExcel出力ドキュメントDTO
    /// </summary>
    /// <typeparam name="T">DTOの基底を継承した任意のオブジェクトの型</typeparam>
    public class SSYM050FExcelOutputDocument<T> : ProductionControlExcelOutputDocument<T> where T : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputList">出力データ</param>
        public SSYM050FExcelOutputDocument(T outputData) : base(outputData)
        {
        }

        #endregion
    }

    /// <summary>
    /// スクラップ外販明細(SSZA400B)のExcel出力ドキュメントDTO
    /// </summary>
    /// <typeparam name="T">DTOの基底を継承した任意のオブジェクトの型</typeparam>
    public class SSZA400BExcelOutputDocument<T> : ProductionControlExcelOutputDocument<T> where T : DTOBase
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputList">出力データ</param>
        public SSZA400BExcelOutputDocument(T outputData) : base(outputData)
        {
        }

        #endregion
    }
}
