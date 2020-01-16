//*************************************************************************************
//
//   ドキュメント読込みストラテジ(基底)
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System.Collections.Generic;
using DTO.Base;
using Utility.Xml;

namespace Document.Strategy
{
    /// <summary>
    /// ドキュメント読込みストラテジ(基底)
    /// </summary>
    public abstract class AbstractDocumentReaderBase
    {
        #region フィールド

        /// <summary>
        /// XMLファイルのパス
        /// </summary>
        protected string xmlPath = null;

        /// <summary>
        /// XML操作用オブジェクト
        /// </summary>
        protected XmlOperator xmlOperator = null;

        #endregion
        
        #region メソッド

        /// <summary>
        /// 初期処理
        /// </summary>
        protected internal abstract void Initialize();

        /// <summary>
        /// 読込み前処理
        /// </summary>
        protected internal abstract void PreRead();

        /// <summary>
        /// 詳細の読込み
        /// </summary>
        /// <typeparam name="T">詳細を継承した任意のオブジェクトの型</typeparam>
        protected internal abstract IList<T> Read<T>() where T : FileDetailDTO;

        /// <summary>
        /// 読込み後処理
        /// </summary>
        protected internal abstract void PostRead();

        /// <summary>
        /// 終了処理
        /// </summary>
        protected internal abstract void Terminate();

        /// <summary>
        /// 設定情報のロード
        /// </summary>
        /// <param name="filePath">設定情報ファイルのパス</param>
        protected abstract void LoadSettings(string filePath);

        #endregion

    }
}
