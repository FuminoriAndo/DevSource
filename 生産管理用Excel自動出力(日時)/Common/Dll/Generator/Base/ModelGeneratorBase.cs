//*************************************************************************************
//
//   モデルジェネレータの基底
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using FileManager.Reader;
using FileManager.Reader.Interface;

namespace Generator.Base
{
    /// <summary>
    /// モデルジェネレータの基底
    /// </summary>
    public class ModelGeneratorBase
    {
        #region フィールド

        /// <summary>
        /// ファイルリーダー
        /// </summary>
        private IFileReader fileReader = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ModelGeneratorBase()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileReader">ファイルリーダー</param>
        public ModelGeneratorBase(IFileReader fileReader)
        {
            this.fileReader = fileReader;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// ファイルリーダー
        /// </summary>
        protected IFileReader FileReader
        {
            get { return fileReader;}
            set { fileReader = value;}
        } 

        #endregion
    }
}