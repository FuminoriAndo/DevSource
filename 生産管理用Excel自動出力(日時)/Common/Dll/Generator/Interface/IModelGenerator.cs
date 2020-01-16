//*************************************************************************************
//
//   モデルジェネレータのインターフェース
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System.Collections.Generic;
using Model.Base;

namespace Generator.Interface
{
    /// <summary>
    /// モデルジェネレータのインターフェース
    /// </summary>
    public interface IModelGenerator
    {
        #region メソッド

        /// <summary>
        /// モデルのリストのジェネレート
        /// </summary>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <returns>モデルのリスト</returns>
        IList<T> GenerateList<T>() where T : ModelBase;

        #endregion
    }
}