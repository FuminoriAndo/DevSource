//*************************************************************************************
//
//   ドキュメント更新ストラテジ(基底)
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;

namespace Document.Strategy
{
    /// <summary>
    /// ドキュメント更新ストラテジ(基底)
    /// </summary>
    public abstract class AbstractDocumentUpdaterBase
    {
        #region フィールド

        #endregion
        
        #region メソッド

        /// <summary>
        /// 初期処理
        /// </summary>
        protected internal abstract void Initialize();

        /// <summary>
        /// 更新
        /// </summary>
        protected internal abstract void Update();

        /// <summary>
        /// 更新
        /// <param name="func">更新条件のファンクション</param>
        /// </summary>
        protected internal abstract void Update(Func<bool> func);

        /// <summary>
        /// 終了処理
        /// </summary>
        protected internal abstract void Terminate();

        #endregion

    }
}
