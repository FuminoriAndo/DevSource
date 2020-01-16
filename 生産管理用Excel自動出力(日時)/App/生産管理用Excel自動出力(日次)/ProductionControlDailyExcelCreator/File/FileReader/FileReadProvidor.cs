//*************************************************************************************
//
//   ファイルリードのプロバイダー
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using FileManager.Reader.Interface;
using ProductionControlDailyExcelCreator.Types;
using Utility.Types;

namespace ProductionControlDailyExcelCreator.FileReader
{
    /// <summary>
    /// ファイルリードのプロバイダー
    /// </summary>
    public class FileReadProvidor
    {
        #region フィールド

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileReadProvidor()
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// ファイルリーダーの取得
        /// </summary>
        /// <param name="modelType">モデルの種類</param>
        /// <param name="extension">拡張子</param>
        /// <returns>ファイルリーダー</returns>
        public static IFileReader GetFileReader(ModelType modelType, ExtensionType extension)
        {
            IFileReader fileReader = null;

            try
            {
                switch (modelType)
                {
                    // PMGQ010F
                    case ModelType.PMGQ010F:
                        switch (extension)
                        {
                            // Textファイル
                            case ExtensionType.TEXT:
                                fileReader = new PMGQ010BTextFileReader();
                                break;
                            default:
                                break;
                        }
                        break;
                    // PMPA280F
                    case ModelType.PMPA280F:
                        switch (extension)
                        {
                            // Textファイル
                            case ExtensionType.TEXT:
                                fileReader = new PMPA260BTextFileReader();
                                break;
                            default:
                                break;
                        }
                        break;
                    // PMPD610F
                    case ModelType.PMPD610F:
                        switch (extension)
                        {
                            // Textファイル
                            case ExtensionType.TEXT:
                                fileReader = new PMPD330BTextFileReader();
                                break;
                            default:
                                break;
                        }
                        break;
                    // PMPF070F
                    case ModelType.PMPF070F:
                        switch (extension)
                        {
                            // Textファイル
                            case ExtensionType.TEXT:
                                fileReader = new PMPF070BTextFileReader();
                                break;
                            default:
                                break;
                        }
                        break;
                    // PMPF090F
                    case ModelType.PMPF090F:
                        switch (extension)
                        {
                            // Textファイル
                            case ExtensionType.TEXT:
                                fileReader = new PMPF090BTextFileReader();
                                break;
                            default:
                                break;
                        }
                        break;
                    // PQGA186F
                    case ModelType.PQGA186F:
                        switch (extension)
                        {
                            // Textファイル
                            case ExtensionType.TEXT:
                                fileReader = new PQGA186BTextFileReader();
                                break;
                            default:
                                break;
                        }
                        break;
                    // PQGA380F
                    case ModelType.PQGA380F:
                        switch (extension)
                        {
                            // Textファイル
                            case ExtensionType.TEXT:
                                fileReader = new PQGA380BTextFileReader();
                                break;
                            default:
                                break;
                        }
                        break;
                    // PQGA420F
                    case ModelType.PQGA420F:
                        switch (extension)
                        {
                            // Textファイル
                            case ExtensionType.TEXT:
                                fileReader = new PQGA420BTextFileReader();
                                break;
                            default:
                                break;
                        }
                        break;
                    // PQGA820F
                    case ModelType.PQGA820F:
                        switch (extension)
                        {
                            // Textファイル
                            case ExtensionType.TEXT:
                                fileReader = new PQGA820BTextFileReader();
                                break;
                            default:
                                break;
                        }
                        break;
                    // SSYM040F
                    case ModelType.SSYM040F:
                        switch (extension)
                        {
                            // Textファイル
                            case ExtensionType.TEXT:
                                fileReader = new SSYM040BTextFileReader();
                                break;
                            default:
                                break;
                        }
                        break;
                    // SSYM050F
                    case ModelType.SSYM050F:
                        switch (extension)
                        {
                            // Textファイル
                            case ExtensionType.TEXT:
                                fileReader = new SSYM050BTextFileReader();
                                break;
                            default:
                                break;
                        }
                        break;
                    // SSZA400B
                    case ModelType.SSZA400B:
                        switch (extension)
                        {
                            // Textファイル
                            case ExtensionType.TEXT:
                                fileReader = new SSZA040BTextFileReader();
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }

                return fileReader;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
