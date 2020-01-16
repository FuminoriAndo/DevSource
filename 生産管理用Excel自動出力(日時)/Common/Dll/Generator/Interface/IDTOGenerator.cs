//*************************************************************************************
//
//   DTOジェネレータのインターフェース
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System.Collections.Generic;
using DTO.Base;
using Model.Base;

namespace Generator.Interface
{
    /// <summary>
    /// DTOジェネレータのインターフェース
    /// </summary>
    public interface IDTOGenerator
    {
        #region メソッド

        /// <summary>
        /// DTOのリストのジェネレート
        /// </summary>
        /// <typeparam name="V">DTOBaseを継承したクラス</typeparam>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <param name="models">モデルのリスト</param>
        /// <returns>DTOのリスト</returns>
        IList<V> GenerateList<T, V>(IList<T> models) where T : ModelBase where V : DTOBase;

        #endregion
    }
}