//*************************************************************************************
//
//   モデルジェネレータのプロバイダー
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using FileManager.Reader.Interface;
using Generator.Interface;
using ProductionControlDailyExcelCreator.Types;

namespace ProductionControlDailyExcelCreator.Generator.Model
{
    /// <summary>
    /// モデルジェネレータのプロバイダー
    /// </summary>
    public class GenerateModelProvidor
    {
        #region メソッド

        /// <summary>
        /// モデルジェネレータの生成(Text用)
        /// </summary>
        /// <param name="modelType">モデルの種類</param>
        /// <param name="fileReader">ファイルリーダー用のインターフェース</param>
        /// <returns>モデルジェネレータ</returns>
        public static IModelGenerator GetModelGenerator(ModelType modelType, IFileReader fileReader)
        {
            IModelGenerator generator = null;

            try
            {
                switch (modelType)
                {
                    case ModelType.PMGQ010F:
                        generator = new PMGQ010BModelGenerator(fileReader);
                        break;
                    case ModelType.PMPA280F:
                        generator = new PMPA260BModelGenerator(fileReader);
                        break;
                    case ModelType.PMPD610F:
                        generator = new PMPD330BModelGenerator(fileReader);
                        break;
                    case ModelType.PMPF070F:
                        generator = new PMPF070BModelGenerator(fileReader);
                        break;
                    case ModelType.PMPF090F:
                        generator = new PMPF090BModelGenerator(fileReader);
                        break;
                    case ModelType.PQGA186F:
                        generator = new PQGA186BModelGenerator(fileReader);
                        break;
                    case ModelType.PQGA380F:
                        generator = new PQGA380BModelGenerator(fileReader);
                        break;
                    case ModelType.PQGA420F:
                        generator = new PQGA420BModelGenerator(fileReader);
                        break;
                    case ModelType.PQGA820F:
                        generator = new PQGA820BModelGenerator(fileReader);
                        break;
                    case ModelType.SSYM040F:
                        generator = new SSYM040BModelGenerator(fileReader);
                        break;
                    case ModelType.SSYM050F:
                        generator = new SSYM050BModelGenerator(fileReader);
                        break;
                    case ModelType.SSZA400B:
                        generator = new SSZA040BModelGenerator(fileReader);
                        break;
                    default:
                        break;
                }

                return generator;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
