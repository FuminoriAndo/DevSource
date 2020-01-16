//*************************************************************************************
//
//   カスタムモデルジェネレータ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using System.Collections.Generic;
using FileManager.Reader.Interface;
using Generator.Base;
using Generator.Interface;
using Model;
using Model.Base;
using ProductionControlDailyExcelCreator.FileReader;

namespace ProductionControlDailyExcelCreator.Generator.Model
{
    /// <summary>
    /// PMGQ010Bのモデルのジェネレータ
    /// </summary>
    public class PMGQ010BModelGenerator : ModelGeneratorBase, IModelGenerator
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileReader">ファイルリーダー</param>
        public PMGQ010BModelGenerator(IFileReader fileReader) : base(fileReader)
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// モデルのリストのジェネレート
        /// </summary>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <returns>モデルのリスト</returns>
        public IList<T> GenerateList<T>() where T : ModelBase
        {
            var list = new List<PMGQ010BModel>();
            var reader = FileReader as PMGQ010BTextFileReader;

            try
            {
                var recordCount = reader.GetRecordCount();
                for (int index = 0; index < recordCount; index++)
                {
                    var model = new PMGQ010BModel()
                    {
                        OrderNo = reader.GetOrderNo(index),
                        CustomerName = reader.GetCustomerName(index),
                        StandardName = reader.GetStandardName(index),
                        ProductSize = reader.GetProductSize(index),
                        ProductAtu = reader.GetProductAtu(index),
                        ProductHaba = reader.GetProductHaba(index),
                        ProductNaga = reader.GetProductNaga(index),
                        TM = reader.GetTM(index),
                        ProductCode = reader.GetProductCode(index),
                        RollSize = reader.GetRollSize(index),
                        RollHaba = reader.GetRollHaba(index),
                        RollNaga = reader.GetRollNaga(index),
                        TPClassfication = reader.GetTPClassfication(index),
                        Lot = reader.GetLot(index),
                        Lot1 = reader.GetLot1(index),
                        Lot2 = reader.GetLot2(index),
                        AtuenYMD = reader.GetAtuenYMD(index),
                        CHNo = reader.GetCHNo(index),
                        SlabNo = reader.GetSlabNo(index),
                        RollNo = reader.GetRollNo(index),
                        ExtractionYMD = reader.GetExtractionYMD(index)
                    };

                    list.Add(model);
                }

                return (IList<T>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PMPA260Bのモデルのジェネレータ
    /// </summary>
    public class PMPA260BModelGenerator : ModelGeneratorBase, IModelGenerator
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileReader">ファイルリーダー</param>
        public PMPA260BModelGenerator(IFileReader fileReader) : base(fileReader)
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// モデルのリストのジェネレート
        /// </summary>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <returns>モデルのリスト</returns>
        public IList<T> GenerateList<T>() where T : ModelBase
        {
            var list = new List<PMPA260BModel>();
            var reader = FileReader as PMPA260BTextFileReader;

            try
            {
                var recordCount = reader.GetRecordCount();
                for (int index = 0; index < recordCount; index++)
                {
                    var model = new PMPA260BModel()
                    {
                        RCNO = reader.GetRCNO(index),
                        Work = reader.GetWork(index),
                        AtuenOrder = reader.GetAtuenOrder(index),
                        LotNo = reader.GetLotNo(index),
                        Lot1 = reader.GetLot1(index),
                        Lot2 = reader.GetLot2(index),
                        CHNo = reader.GetCHNo(index),
                        CHDash = reader.GetCHDash(index),
                        Slab1No = reader.GetSlab1No(index),
                        Slab2No = reader.GetSlab2No(index),
                        MaterialsIdentification = reader.GetMaterialsIdentification(index),
                        MaterialsIdentification1 = reader.GetMaterialsIdentification1(index),
                        MaterialsIdentification2 = reader.GetMaterialsIdentification2(index),
                        TPManagement = reader.GetTPManagement(index),
                        AtuTapera = reader.GetAtuTapera(index),
                        HabaTapera = reader.GetHabaTapera(index),
                        HCDivision = reader.GetHCDivision(index),
                        ExtractionHHMM = reader.GetExtractionHHMM(index),
                        ExtractionHH = reader.GetExtractionHH(index),
                        ExtractionMM = reader.GetExtractionMM(index),
                        Standard = reader.GetStandard(index),
                        RollSize = reader.GetRollSize(index),
                        RollAtu = reader.GetRollAtu(index),
                        RollHaba = reader.GetRollHaba(index),
                        RollNaga = reader.GetRollNaga(index),
                        SlabSize = reader.GetSlabSize(index),
                        SlabAtu = reader.GetSlabAtu(index),
                        SlabHaba = reader.GetSlabHaba(index),
                        SlabNaga = reader.GetSlabNaga(index),
                        SlabWeight = reader.GetSlabWeight(index),
                        SlabCount = reader.GetSlabCount(index),
                        RePlan = reader.GetRePlan(index),
                        Haste = reader.GetHaste(index),
                        Export = reader.GetExport(index),
                        ExtractionTemperature = reader.GetExtractionTemperature(index),
                        TestPressure = reader.GetTestPressure(index),
                        PlateHaba = reader.GetPlateHaba(index),
                        Strain = reader.GetStrain(index)
                    };

                    list.Add(model);
                }

                return (IList<T>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PMPA330Bのモデルのジェネレータ
    /// </summary>
    public class PMPD330BModelGenerator : ModelGeneratorBase, IModelGenerator
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileReader">ファイルリーダー</param>
        public PMPD330BModelGenerator(IFileReader fileReader) : base(fileReader)
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// モデルのリストのジェネレート
        /// </summary>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <returns>モデルのリスト</returns>
        public IList<T> GenerateList<T>() where T : ModelBase
        {
            var list = new List<PMPD330BModel>();
            var reader = FileReader as PMPD330BTextFileReader;

            try
            {
                var recordCount = reader.GetRecordCount();
                for (int index = 0; index < recordCount; index++)
                {
                    var model = new PMPD330BModel()
                    {
                        SteelsName = reader.GetSteelsName(index),
                        SteelsType = reader.GetSteelsType(index),
                        StandardName = reader.GetStandardName(index),
                        ProductSize = reader.GetProductSize(index),
                        ProductAtu = reader.GetProductAtu(index),
                        ProductHaba = reader.GetProductHaba(index),
                        ProductNaga = reader.GetProductNaga(index),
                        TM = reader.GetTM(index),
                        ProductCode = reader.GetProductCode(index),
                        OrdersNo = reader.GetOrdersNo(index),
                        CustomerName = reader.GetCustomerName(index),
                        DeliveryDate = reader.GetDeliveryDate(index),
                        DeliveryDateRank = reader.GetDeliveryDateRank(index),
                        Maisuu = reader.GetMaisuu(index),
                        RollSize = reader.GetRollSize(index),
                        RollHaba = reader.GetRollHaba(index),
                        RollNaga = reader.GetRollNaga(index),
                        HeatInsulation = reader.GetHeatInsulation(index),
                        SlabSize = reader.GetSlabSize(index),
                        SlabAtu = reader.GetSlabAtu(index),
                        SlabHaba = reader.GetSlabHaba(index),
                        SlabNaga = reader.GetSlabNaga(index),
                        SlabWeight = reader.GetSlabWeight(index),
                        SlabCount = reader.GetSlabCount(index),
                        HasteDivision = reader.GetHasteDivision(index),
                        SpecialDivision = reader.GetSpecialDivision(index),
                        ExportDivision = reader.GetExportDivision(index)
                    };

                    list.Add(model);
                }

                return (IList<T>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PMPF070Bのモデルのジェネレータ
    /// </summary>
    public class PMPF070BModelGenerator : ModelGeneratorBase, IModelGenerator
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileReader">ファイルリーダー</param>
        public PMPF070BModelGenerator(IFileReader fileReader) : base(fileReader)
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// モデルのリストのジェネレート
        /// </summary>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <returns>モデルのリスト</returns>
        public IList<T> GenerateList<T>() where T : ModelBase
        {
            var list = new List<PMPF070BModel>();
            var reader = FileReader as PMPF070BTextFileReader;

            try
            {
                var recordCount = reader.GetRecordCount();
                for (int index = 0; index < recordCount; index++)
                {
                    var model = new PMPF070BModel()
                    {
                        OrderNo = reader.GetOrderNo(index),
                        OrderNo1 = reader.GetOrderNo1(index),
                        OrderNo2 = reader.GetOrderNo2(index),
                        OrderNo3 = reader.GetOrderNo3(index),
                        DeliveryDate = reader.GetDeliveryDate(index),
                        DeliveryDateMM = reader.GetDeliveryDateMM(index),
                        DeliveryDateDD = reader.GetDeliveryDateDD(index),
                        DeliveryDateRank = reader.GetDeliveryDateRank(index),
                        CustomerName = reader.GetCustomerName(index),
                        LotNo = reader.GetLotNo(index),
                        Lot1 = reader.GetLot1(index),
                        Lot2 = reader.GetLot2(index),
                        RollNo = reader.GetRollNo(index),
                        NotCollectedLot = reader.GetNotCollectedLot(index),
                        NotCollectedLotNo1 = reader.GetNotCollectedLotNo1(index),
                        NotCollectedLotNo2 = reader.GetNotCollectedLotNo2(index),
                        Status = reader.GetStatus(index),
                        PlanStandardCode = reader.GetPlanStandardCode(index),
                        PlanSize = reader.GetPlanSize(index),
                        PlanAtu = reader.GetPlanAtu(index),
                        PlanHaba = reader.GetPlanHaba(index),
                        PlanNaga = reader.GetPlanNaga(index),
                        PlanTM = reader.GetPlanTM(index),
                        PlanProductCode = reader.GetPlanProductCode(index),
                        PlanMaisuu = reader.GetPlanMaisuu(index),
                        PerformanceStandardCode = reader.GetPerformanceStandardCode(index),
                        PerformanceSize = reader.GetPerformanceSize(index),
                        PerformanceAtu = reader.GetPerformanceAtu(index),
                        PerformanceHaba = reader.GetPerformanceHaba(index),
                        PerformanceNaga = reader.GetPerformanceNaga(index),
                        PerformanceTM = reader.GetPerformanceTM(index),
                        PerformanceProdutCode = reader.GetPerformanceProdutCode(index),
                        PerformanceMaisuu = reader.GetPerformanceMaisuu(index),
                        GeneratingStep = reader.GetGeneratingStep(index),
                        Special = reader.GetSpecial(index),
                        ExtractionYMD = reader.GetExtractionYMD(index)
                    };

                    list.Add(model);
                }

                return (IList<T>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PMPF090Bのモデルのジェネレータ
    /// </summary>
    public class PMPF090BModelGenerator : ModelGeneratorBase, IModelGenerator
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileReader">ファイルリーダー</param>
        public PMPF090BModelGenerator(IFileReader fileReader) : base(fileReader)
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// モデルのリストのジェネレート
        /// </summary>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <returns>モデルのリスト</returns>
        public IList<T> GenerateList<T>() where T : ModelBase
        {
            var list = new List<PMPF090BModel>();
            var reader = FileReader as PMPF090BTextFileReader;

            try
            {
                var recordCount = reader.GetRecordCount();
                for (int index = 0; index < recordCount; index++)
                {
                    var model = new PMPF090BModel()
                    {
                        StandardName = reader.GetStandardName(index),
                        ProductAtu = reader.GetProductAtu(index),
                        ProductHaba = reader.GetProductHaba(index),
                        ProductNaga = reader.GetProductNaga(index),
                        TM = reader.GetTM(index),
                        ProductCode = reader.GetProductCode(index),
                        Yard = reader.GetYard(index),
                        Maisuu = reader.GetMaisuu(index),
                        TieMaisuu = reader.GetTieMaisuu(index),
                        ReservationClassfication = reader.GetReservationClassfication(index),
                        NotCollectClassfication = reader.GetNotCollectClassfication(index),
                        OrderNo = reader.GetOrderNo(index),
                        CustomerName = reader.GetCustomerName(index),
                        UnPlan = reader.GetUnPlan(index),
                        DeliveryDate = reader.GetDeliveryDate(index),
                        DeliveryDateRank = reader.GetDeliveryDateRank(index),                    
                    };

                    list.Add(model);
                }

                return (IList<T>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PQGA186Bのモデルのジェネレータ
    /// </summary>
    public class PQGA186BModelGenerator : ModelGeneratorBase, IModelGenerator
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileReader">ファイルリーダー</param>
        public PQGA186BModelGenerator(IFileReader fileReader) : base(fileReader)
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// モデルのリストのジェネレート
        /// </summary>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <returns>モデルのリスト</returns>
        public IList<T> GenerateList<T>() where T : ModelBase
        {
            var list = new List<PQGA186BModel>();
            var reader = FileReader as PQGA186BTextFileReader;

            try
            {
                var recordCount = reader.GetRecordCount();
                for (int index = 0; index < recordCount; index++)
                {
                    var model = new PQGA186BModel()
                    {
                        IssuNoAndDate = reader.GetIssuNoAndDate(index),
                        CHNo = reader.GetCHNo(index),
                        StandardName = reader.GetStandardName(index),
                        SlabSize = reader.GetSlabSize(index),
                        SlabAtu = reader.GetSlabAtu(index),
                        SlabHaba = reader.GetSlabHaba(index),
                        SlabNaga = reader.GetSlabNaga(index),
                        UnitWeight = reader.GetUnitWeight(index),
                        OrderNo = reader.GetOrderNo(index),
                        PlanNo = reader.GetPlanNo(index),
                        ExtractionYMD = reader.GetExtractionYMD(index)
                    };

                    list.Add(model);
                }

                return (IList<T>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PQGA380Bのモデルのジェネレータ
    /// </summary>
    public class PQGA380BModelGenerator : ModelGeneratorBase, IModelGenerator
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileReader">ファイルリーダー</param>
        public PQGA380BModelGenerator(IFileReader fileReader) : base(fileReader)
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// モデルのリストのジェネレート
        /// </summary>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <returns>モデルのリスト</returns>
        public IList<T> GenerateList<T>() where T : ModelBase
        {
            var list = new List<PQGA380BModel>();
            var reader = FileReader as PQGA380BTextFileReader;

            try
            {
                var recordCount = reader.GetRecordCount();
                for (int index = 0; index < recordCount; index++)
                {
                    var model = new PQGA380BModel()
                    {
                        OrderNo = reader.GetOrderNo(index),
                        CustomerCode = reader.GetCustomerCode(index),
                        CustomerName = reader.GetCustomerName(index),
                        DisutributorCode = reader.GetDisutributorCode(index),
                        City = reader.GetCity(index),
                        Collectiion = reader.GetCollectiion(index),
                        Kind = reader.GetKind(index),
                        StandardCode = reader.GetStandardCode(index),
                        SettlementCondition = reader.GetSettlementCondition(index),
                        DisutributorName = reader.GetDisutributorName(index),
                        StandardName = reader.GetStandardName(index),
                        ProductSize = reader.GetProductSize(index),
                        ProductAtu = reader.GetProductAtu(index),
                        ProductHaba = reader.GetProductHaba(index),
                        ProductNaga = reader.GetProductNaga(index),
                        ShipmentHonsuu = reader.GetShipmentHonsuu(index),
                        ShipmentWeight = reader.GetShipmentWeight(index),
                        UnitPrice = reader.GetUnitPrice(index),
                        TransferCondition = reader.GetTransferCondition(index),
                        CHNo = reader.GetCHNo(index),
                        Slab1No1 = reader.GetSlab1No1(index),
                        Slab1No2 = reader.GetSlab1No2(index),
                        Slab1No3 = reader.GetSlab1No3(index),
                        Slab1No4 = reader.GetSlab1No4(index),
                        Slab1No5 = reader.GetSlab1No5(index),
                        Slab1No6 = reader.GetSlab1No6(index),
                        Slab1No7 = reader.GetSlab1No7(index),
                        Slab1No8 = reader.GetSlab1No8(index),
                        Slab1No9 = reader.GetSlab1No9(index),
                        Slab1No10 = reader.GetSlab1No10(index),
                        SlabHonsuu = reader.GetSlabHonsuu(index),
                        SlabWeight = reader.GetSlabWeight(index),
                        ExtractionYMD = reader.GetExtractionYMD(index)
                    };

                    list.Add(model);
                }

                return (IList<T>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PQGA420Bのモデルのジェネレータ
    /// </summary>
    public class PQGA420BModelGenerator : ModelGeneratorBase, IModelGenerator
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileReader">ファイルリーダー</param>
        public PQGA420BModelGenerator(IFileReader fileReader) : base(fileReader)
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// モデルのリストのジェネレート
        /// </summary>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <returns>モデルのリスト</returns>
        public IList<T> GenerateList<T>() where T : ModelBase
        {
            var list = new List<PQGA420BModel>();
            var reader = FileReader as PQGA420BTextFileReader;

            try
            {
                var recordCount = reader.GetRecordCount();
                for (int index = 0; index < recordCount; index++)
                {
                    var model = new PQGA420BModel()
                    {
                        OrderNo = reader.GetOrderNo(index),
                        CustomerCode = reader.GetCustomerCode(index),
                        CustomerName = reader.GetCustomerName(index),
                        DisutributorCode = reader.GetDisutributorCode(index),
                        City = reader.GetCity(index),
                        Collection = reader.GetCollection(index),
                        Kind = reader.GetKind(index),
                        StandardCode = reader.GetStandardCode(index),
                        SettlementCondition = reader.GetSettlementCondition(index),
                        DisutributorName = reader.GetDisutributorName(index),
                        StandardName = reader.GetStandardName(index),
                        SlabSize = reader.GetSlabSize(index),
                        SlabAtu = reader.GetSlabAtu(index),
                        SlabHaba = reader.GetSlabHaba(index),
                        SlabNaga = reader.GetSlabNaga(index),
                        Honsuu = reader.GetHonsuu(index),
                        Weight = reader.GetWeight(index),
                        DeliveryDate = reader.GetDeliveryDate(index),
                        DeliveryDateRank = reader.GetDeliveryDateRank(index),
                        TransferCondition = reader.GetTransferCondition(index),
                        ExtractionYMD = reader.GetExtractionYMD(index),
                    };

                    list.Add(model);
                }

                return (IList<T>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PQGA820Bのモデルのジェネレータ
    /// </summary>
    public class PQGA820BModelGenerator : ModelGeneratorBase, IModelGenerator
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileReader">ファイルリーダー</param>
        public PQGA820BModelGenerator(IFileReader fileReader) : base(fileReader)
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// モデルのリストのジェネレート
        /// </summary>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <returns>モデルのリスト</returns>
        public IList<T> GenerateList<T>() where T : ModelBase
        {
            var list = new List<PQGA820BModel>();
            var reader = FileReader as PQGA820BTextFileReader;

            try
            {
                var recordCount = reader.GetRecordCount();
                for (int index = 0; index < recordCount; index++)
                {
                    var model = new PQGA820BModel()
                    {
                        ShipmentYYMMDD = reader.GetShipmentYYMMDD(index),
                        ZK081Honsuu = reader.GetZK081Honsuu(index),
                        ZK081Weight = reader.GetZK081Weight(index),
                        ZK085Honsuu = reader.GetZK085Honsuu(index),
                        ZK085Weight = reader.GetZK085Weight(index),
                        ZK121Honsuu = reader.GetZK121Honsuu(index),
                        ZK121Weight = reader.GetZK121Weight(index),
                        ZK172Honsuu = reader.GetZK172Honsuu(index),
                        ZK172Weight = reader.GetZK172Weight(index),
                        SPHCHonsuu = reader.GetSPHCHonsuu(index),
                        SPHCWeight = reader.GetSPHCWeight(index),
                        SS400Honsuu = reader.GetSS400Honsuu(index),
                        SS400Weight = reader.GetSS400Weight(index),
                        OtherHonsuu = reader.GetOtherHonsuu(index),
                        OtherWeight = reader.GetOtherWeight(index),
                        TotalHonsuu = reader.GetTotalHonsuu(index),
                        TotalWeight = reader.GetTotalWeight(index)
                    };

                    list.Add(model);
                }

                return (IList<T>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// SSYM040Bのモデルのジェネレータ
    /// </summary>
    public class SSYM040BModelGenerator : ModelGeneratorBase, IModelGenerator
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileReader">ファイルリーダー</param>
        public SSYM040BModelGenerator(IFileReader fileReader) : base(fileReader)
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// モデルのリストのジェネレート
        /// </summary>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <returns>モデルのリスト</returns>
        public IList<T> GenerateList<T>() where T : ModelBase
        {
            var list = new List<SSYM040BModel>();
            var reader = FileReader as SSYM040BTextFileReader;

            try
            {
                var recordCount = reader.GetRecordCount();
                for (int index = 0; index < recordCount; index++)
                {
                    var model = new SSYM040BModel()
                    {
                        YYYYMMDD = reader.GetYYYYMMDD(index),
                        MiddlePlateWaste = reader.GetMiddlePlateWaste(index),
                        NissinReturnWaste = reader.GetNissinReturnWaste(index),
                        TrimmerWaste = reader.GetTrimmerWaste(index),
                        AtuItaLineWaste = reader.GetAtuItaLineWaste(index),
                        PlanerWaste = reader.GetPlanerWaste(index),
                        LaserWaste = reader.GetLaserWaste(index),
                        PlanerChitaWaste = reader.GetPlanerChitaWaste(index),
                        MissRollWaste = reader.GetMissRollWaste(index),
                        ColumuReturnWaste = reader.GetColumuReturnWaste(index),
                        Total = reader.GetTotal(index)
                    };

                    list.Add(model);
                }

                return (IList<T>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// SSYM050Bのモデルのジェネレータ
    /// </summary>
    public class SSYM050BModelGenerator : ModelGeneratorBase, IModelGenerator
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileReader">ファイルリーダー</param>
        public SSYM050BModelGenerator(IFileReader fileReader) : base(fileReader)
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// モデルのリストのジェネレート
        /// </summary>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <returns>モデルのリスト</returns>
        public IList<T> GenerateList<T>() where T : ModelBase
        {
            var list = new List<SSYM050BModel>();
            var reader = FileReader as SSYM050BTextFileReader;

            try
            {
                var recordCount = reader.GetRecordCount();
                for (int index = 0; index < recordCount; index++)
                {
                    var model = new SSYM050BModel()
                    {
                        YYYYMMDD = reader.GetYYYYMMDD(index),
                        EndShearWaste = reader.GetEndShearWaste(index),
                        PlanerWaste = reader.GetPlanerWaste(index),
                        TrimmerWaste = reader.GetTrimmerWaste(index),
                        LaserWaste = reader.GetLaserWaste(index),
                        OtherWaste = reader.GetOtherWaste(index),
                        Total = reader.GetTotal(index)
                    };

                    list.Add(model);
                }

                return (IList<T>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// SSZA400Bのモデルのジェネレータ
    /// </summary>
    public class SSZA040BModelGenerator : ModelGeneratorBase, IModelGenerator
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileReader">ファイルリーダー</param>
        public SSZA040BModelGenerator(IFileReader fileReader) : base(fileReader)
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// モデルのリストのジェネレート
        /// </summary>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <returns>モデルのリスト</returns>
        public IList<T> GenerateList<T>() where T : ModelBase
        {
            var list = new List<SSZA040BModel>();
            var reader = FileReader as SSZA040BTextFileReader;

            try
            {
                var recordCount = reader.GetRecordCount();
                for (int index = 0; index < recordCount; index++)
                {
                    var model = new SSZA040BModel()
                    {
                        OrderNo = reader.GetOrderNo(index),
                        StandardName = reader.GetStandardName(index),
                        ProductSize = reader.GetProductSize(index),
                        ProductAtu = reader.GetProductAtu(index),
                        ProductHaba = reader.GetProductHaba(index),
                        ProductNaga = reader.GetProductNaga(index),
                        TM = reader.GetTM(index),
                        ProductCode = reader.GetProductCode(index),
                        CustomerName = reader.GetCustomerName(index),
                        OrderCount = reader.GetOrderCount(index),
                        OrderWeight = reader.GetOrderWeight(index),
                        ShipmentCount = reader.GetShipmentCount(index),
                        TransferPlaceName = reader.GetTransferPlaceName(index),
                        TransferCondition = reader.GetTransferCondition(index),
                        DeliveryDate = reader.GetDeliveryDate(index),
                        DeliveryDateRank = reader.GetDeliveryDateRank(index),
                        ProvisionClassfication = reader.GetProvisionClassfication(index),
                        ExtractionYMD = reader.GetExtractionYMD(index)
                    };

                    list.Add(model);
                }

                return (IList<T>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
