//*************************************************************************************
//
//   ドキュメント書込みストラテジ(基底)
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
    /// ドキュメント書込みストラテジ(基底)
    /// </summary>
    public abstract class AbstractDocumentWriterBase
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
        public abstract void Initialize();

        /// <summary>
        /// 書込み前処理
        /// </summary>
        public abstract void PreWrite();

        /// <summary>
        /// ヘッダーの書込み
        /// </summary>
        /// <typeparam name="T">ヘッダーを継承した任意のオブジェクトの型</typeparam>
        /// <param name="header">ファイルのヘッダー情報</param>
        public abstract void WriteHeader<T>(T header) where T : FileHeaderDTO;

        /// <summary>
        /// 詳細の書込み
        /// </summary>
        /// <typeparam name="T">詳細を継承した任意のオブジェクトの型</typeparam>
        /// <param name="detail">ファイルの詳細情報</param>
        public abstract void WriteDetail<T>(IList<T> detail) where T : FileDetailDTO;

        /// <summary>
        /// フッターの書込み
        /// </summary>
        /// <typeparam name="T">フッターを継承した任意のオブジェクトの型</typeparam>
        /// <param name="footer">ファイルのフッター情報</param>
        public abstract void WriteFooter<T>(T footer) where T : FileFooterDTO;

        /// <summary>
        /// 書込み後処理
        /// </summary>
        public abstract void PostWrite();

        /// <summary>
        /// 終了処理
        /// </summary>
        public abstract void Terminate();

        /// <summary>
        /// 設定情報のロード
        /// </summary>
        /// <param name="filePath">設定情報ファイルのパス</param>
        public abstract void LoadSettings(string filePath);

        #endregion

    }
}
