//*************************************************************************************
//
//   カスタムDTOジェネレータ
//
//   修正履歴
//
//   修正年月日  Ｒｅｖ    担当者       修正内容
//   18.07.10              DSK吉武      新規作成
//
//*************************************************************************************
using System;
using System.Collections.Generic;
using DTO.Base;
using Generator.Interface;
using Model;
using Model.Base;
using ProductionControlDailyExcelCreator.DTO;
using Utility.Types;
using Utility.Core;
using Microsoft.VisualBasic;

namespace ProductionControlDailyExcelCreator.Generator.DTO
{
    /// <summary>
    /// PMGQ010BDTOのジェネレータ
    /// </summary>
    public class PMGQ010BDTOGenerator : IDTOGenerator
    {
        #region メソッド

        /// <summary>
        /// DTOのリストのジェネレート
        /// </summary>
        /// <typeparam name="V">DTOBaseを継承したクラス</typeparam>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <param name="models">モデルのリスト</param>
        /// <returns>DTOのリスト</returns>
        public IList<V> GenerateList<T, V>(IList<T> models)
            where T : ModelBase
            where V : DTOBase
        {
            var list = new List<PMGQ010BDTO>();

            try
            {
                var targetModels = models as IList<PMGQ010BModel>;
                targetModels.ForEach(item =>
                {
                    var dto = new PMGQ010BDTO()
                    {
                        OrderNo = item.OrderNo,
                        CustomerName = item.CustomerName.Trim(),
                        StandardName = item.StandardName.Trim(),
                        ProductSize = item.ProductSize
                                        == NumericType.Zero.GetStringValue() + Strings.Space(3) + NumericType.Zero.GetStringValue() ?
                                        string.Empty : item.ProductSize,
                        ProductAtu = item.ProductAtu,
                        ProductHaba = item.ProductHaba,
                        ProductNaga = item.ProductNaga,
                        RollSize = item.RollSize
                                        == NumericType.Zero.GetStringValue() + Strings.Space(3) + NumericType.Zero.GetStringValue() ?
                                        string.Empty : item.RollSize,
                        RollHaba = item.RollHaba,
                        RollNaga = item.RollNaga,
                        TM = item.TM,
                        TPClassfication = item.TPClassfication,
                        Lot = item.Lot,
                        AtuenYMD = item.AtuenYMD.Trim() != string.Empty ?
                                   item.AtuenYMD.Substring(0, 2) + SymbolType.Slash.GetValue()
                                   + item.AtuenYMD.Substring(2, 2) + SymbolType.Slash.GetValue()
                                   + item.AtuenYMD.Substring(4, 2) : item.AtuenYMD,
                        SlabNo = item.SlabNo,
                        RollNo = item.RollNo
                    };

                    list.Add(dto);
                });

                return (IList<V>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PMPA260Bのジェネレータ
    /// </summary>
    public class PMPA260BDTOGenerator : IDTOGenerator
    {
        #region メソッド

        /// <summary>
        /// DTOのリストのジェネレート
        /// </summary>
        /// <typeparam name="V">DTOBaseを継承したクラス</typeparam>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <param name="models">モデルのリスト</param>
        /// <returns>DTOのリスト</returns>
        public IList<V> GenerateList<T, V>(IList<T> models)
            where T : ModelBase
            where V : DTOBase
        {
            var list = new List<PMPA260BDTO>();

            try
            {
                var targetModels = models as IList<PMPA260BModel>;
                targetModels.ForEach(item =>
                {
                    var dto = new PMPA260BDTO()
                    {
                        RCNO = item.RCNO,
                        Work = item.Work,
                        AtuenOrder = item.AtuenOrder,
                        LotNo = item.LotNo,
                        Slab1No = item.CHNo + item.CHDash + item.Slab1No,
                        MaterialsIdentification = item.MaterialsIdentification,
                        AtuTapera = item.AtuTapera,
                        HabaTapera = item.HabaTapera,
                        HCDivision = item.HCDivision,
                        ExtractionHHMM = item.ExtractionHHMM,
                        Standard = item.Standard,
                        RollSize = item.RollSize,
                        RollAtu = item.RollAtu,
                        RollHaba = item.RollHaba,
                        RollNaga = item.RollNaga,
                        SlabSize = item.SlabSize,
                        SlabAtu = item.SlabAtu,
                        SlabHaba = item.SlabHaba,
                        SlabNaga = item.SlabNaga,
                        SlabWeight = item.SlabWeight.TrimStartZeroWithConvertToInt(),
                        SlabCount = item.SlabCount.TrimStartZeroWithConvertToInt(),
                        RePlan = item.RePlan,
                        Haste = item.Haste,
                        ExtractionTemperature = item.ExtractionTemperature.TrimStartZeroWithConvertToInt(),
                        TestPressure = item.TestPressure,
                        PlateHaba = item.PlateHaba,
                        Strain = item.Strain
                    };

                    list.Add(dto);
                });

                return (IList<V>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PMPD330Bのジェネレータ
    /// </summary>
    public class PMPD330BDTOGenerator : IDTOGenerator
    {
        #region メソッド

        /// <summary>
        /// DTOのリストのジェネレート
        /// </summary>
        /// <typeparam name="V">DTOBaseを継承したクラス</typeparam>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <param name="models">モデルのリスト</param>
        /// <returns>DTOのリスト</returns>
        public IList<V> GenerateList<T, V>(IList<T> models)
            where T : ModelBase
            where V : DTOBase
        {
            var list = new List<PMPD330BDTO>();

            try
            {
                var targetModels = models as IList<PMPD330BModel>;
                targetModels.ForEach(item =>
                {
                    var dto = new PMPD330BDTO()
                    {
                        SteelsName = item.SteelsName,
                        SteelsType = item.SteelsType,
                        StandardName = item.StandardName,
                        ProductSize = item.ProductSize,
                        ProductAtu = item.ProductAtu,
                        ProductHaba = item.ProductHaba,
                        ProductNaga = item.ProductNaga,
                        TM = item.TM,
                        ProductCode = item.ProductCode,
                        OrdersNo = item.OrdersNo,
                        CustomerName = item.CustomerName,
                        DeliveryDate = item.DeliveryDate,
                        DeliveryDateRank = item.DeliveryDateRank,
                        Maisuu = item.Maisuu.TrimStartZeroWithConvertToInt(),
                        RollSize = item.RollSize,
                        RollHaba = item.RollHaba,
                        RollNaga = item.RollNaga,
                        SlabSize = item.SlabSize,
                        SlabAtu = item.SlabAtu,
                        SlabHaba = item.SlabHaba,
                        SlabNaga = item.SlabNaga,
                        SlabWeight = item.SlabWeight.TrimStartZeroWithConvertToInt(),
                        SlabCount = item.SlabCount.TrimStartZeroWithConvertToInt(),
                        HasteDivision = item.HasteDivision,
                        SpecialDivision = item.SpecialDivision,
                        ExportDivision = item.ExportDivision
                    };

                    list.Add(dto);
                });

                return (IList<V>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PMPF070BDTOのジェネレータ
    /// </summary>
    public class PMPF070BDTOGenerator : IDTOGenerator
    {
        #region メソッド

        /// <summary>
        /// DTOのリストのジェネレート
        /// </summary>
        /// <typeparam name="V">DTOBaseを継承したクラス</typeparam>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <param name="models">モデルのリスト</param>
        /// <returns>DTOのリスト</returns>
        public IList<V> GenerateList<T, V>(IList<T> models)
            where T : ModelBase
            where V : DTOBase
        {
            var list = new List<PMPF070BDTO>();

            try
            {
                var targetModels = models as IList<PMPF070BModel>;
                targetModels.ForEach(item =>
                {
                    var dto = new PMPF070BDTO()
                    {
                        OrderNo = item.OrderNo,
                        DeliveryDate = item.DeliveryDate,
                        DeliveryDateRank = item.DeliveryDateRank,
                        CustomerName = item.CustomerName,
                        LotNo = item.LotNo,
                        RollNo = item.RollNo,
                        NotCollectedLot = item.NotCollectedLot,
                        Status = item.Status,
                        PlanStandardCode = item.PlanStandardCode,
                        PlanSize = item.PlanSize,
                        PlanAtu = item.PlanAtu,
                        PlanHaba = item.PlanHaba,
                        PlanNaga = item.PlanNaga,
                        PlanTM = item.PlanTM,
                        PlanProductCode = item.PlanProductCode,
                        PlanMaisuu = item.PlanMaisuu.TrimStartZeroWithConvertToInt(),
                        PerformanceStandardCode = item.PerformanceStandardCode,
                        PerformanceSize = item.PerformanceSize,
                        PerformanceAtu = item.PerformanceAtu,
                        PerformanceHaba = item.PerformanceHaba,
                        PerformanceNaga = item.PerformanceNaga,
                        PerformanceTM = item.PerformanceTM,
                        PerformanceProdutCode = item.PerformanceProdutCode,
                        PerformanceMaisuu = item.PerformanceMaisuu.TrimStartZeroWithConvertToInt(),
                        GeneratingStep = item.GeneratingStep,
                        Special = item.Special,
                        ExtractionYMD = item.ExtractionYMD
                    };

                    list.Add(dto);
                });

                return (IList<V>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PMPF090BDTOのジェネレータ
    /// </summary>
    public class PMPF090BDTOGenerator : IDTOGenerator
    {
        #region メソッド

        /// <summary>
        /// DTOのリストのジェネレート
        /// </summary>
        /// <typeparam name="V">DTOBaseを継承したクラス</typeparam>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <param name="models">モデルのリスト</param>
        /// <returns>DTOのリスト</returns>
        public IList<V> GenerateList<T, V>(IList<T> models)
            where T : ModelBase
            where V : DTOBase
        {
            var list = new List<PMPF090BDTO>();

            try
            {
                var targetModels = models as IList<PMPF090BModel>;
                targetModels.ForEach(item =>
                {
                    var dto = new PMPF090BDTO()
                    {
                        StandardName = item.StandardName,
                        //ProductSize = item.ProductSize,
                        ProductAtu = item.ProductAtu,
                        ProductHaba = item.ProductHaba,
                        ProductNaga = item.ProductNaga,
                        TM = item.TM,
                        ProductCode = item.ProductCode,
                        Yard = item.Yard,
                        Maisuu = item.Maisuu.TrimStartZeroWithConvertToInt(),
                        TieMaisuu = item.TieMaisuu.TrimStartZeroWithConvertToInt(),
                        ReservationClassfication = item.ReservationClassfication,
                        NotCollectClassfication = item.NotCollectClassfication,
                        OrderNo = item.OrderNo,
                        CustomerName = item.CustomerName,
                        UnPlan = item.UnPlan.TrimStartZeroWithConvertToInt(),
                        DeliveryDate = item.DeliveryDate,
                        DeliveryDateRank = item.DeliveryDateRank,

                        //GeneratingProductClassfication1 = item.GeneratingProductClassfication1,
                        //ItaNo1 = item.ItaNo1,
                        //TestAcceptance1 = item.TestAcceptance1,
                        //PileUp1 = item.PileUp1,
                        //GeneratingProductClassfication2 = item.GeneratingProductClassfication2,
                        //ItaNo2 = item.ItaNo2,
                        //TestAcceptance2 = item.TestAcceptance2,
                        //PileUp2 = item.PileUp2,
                    };

                    list.Add(dto);
                });

                return (IList<V>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PQGA186BDTOのジェネレータ
    /// </summary>
    public class PQGA186BDTOGenerator : IDTOGenerator
    {
        #region メソッド

        /// <summary>
        /// DTOのリストのジェネレート
        /// </summary>
        /// <typeparam name="V">DTOBaseを継承したクラス</typeparam>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <param name="models">モデルのリスト</param>
        /// <returns>DTOのリスト</returns>
        public IList<V> GenerateList<T, V>(IList<T> models)
            where T : ModelBase
            where V : DTOBase
        {
            var list = new List<PQGA186BDTO>();

            try
            {
                var targetModels = models as IList<PQGA186BModel>;
                targetModels.ForEach(item =>
                {
                    var dto = new PQGA186BDTO()
                    {
                        IssuNoAndDate = item.IssuNoAndDate,
                        CHNo = item.CHNo,
                        StandardName = item.StandardName,
                        SlabSize = item.SlabSize,
                        SlabAtu = item.SlabAtu,
                        SlabHaba = item.SlabHaba,
                        SlabNaga = item.SlabNaga,
                        UnitWeight = item.UnitWeight.TrimStartZeroWithConvertToInt(),
                        OrderNo = item.OrderNo,
                        PlanNo = item.PlanNo
                    };

                    list.Add(dto);
                });

                return (IList<V>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PQGA380BDTOのジェネレータ
    /// </summary>
    public class PQGA380BDTOGenerator : IDTOGenerator
    {
        #region メソッド

        /// <summary>
        /// DTOのリストのジェネレート
        /// </summary>
        /// <typeparam name="V">DTOBaseを継承したクラス</typeparam>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <param name="models">モデルのリスト</param>
        /// <returns>DTOのリスト</returns>
        public IList<V> GenerateList<T, V>(IList<T> models)
            where T : ModelBase
            where V : DTOBase
        {
            var list = new List<PQGA380BDTO>();

            try
            {
                var targetModels = models as IList<PQGA380BModel>;
                targetModels.ForEach(item =>
                {
                    var dto = new PQGA380BDTO()
                    {
                        OrderNo = item.OrderNo,
                        CustomerName = item.CustomerName,
                        City = item.City,
                        Collectiion = item.Collectiion,
                        Kind = item.Kind,
                        CHNo = item.CHNo,
                        Slab1No1 = item.Slab1No1,
                        Slab1No2 = item.Slab1No2,
                        Slab1No3 = item.Slab1No3,
                        Slab1No4 = item.Slab1No4,
                        Slab1No5 = item.Slab1No5,
                        Slab1No6 = item.Slab1No6,
                        Slab1No7 = item.Slab1No7,
                        Slab1No8 = item.Slab1No8,
                        Slab1No9 = item.Slab1No9,
                        Slab1No10 = item.Slab1No10,
                        SettlementCondition = item.SettlementCondition,
                        DisutributorName = item.DisutributorName,
                        StandardName = item.StandardName,
                        ProductSize = item.ProductSize,
                        ProductAtu = item.ProductAtu,
                        ProductHaba = item.ProductHaba,
                        ProductNaga = item.ProductNaga,
                        ShipmentHonsuu = item.SlabHonsuu.TrimStartZeroWithConvertToInt(),
                        ShipmentWeight = item.SlabWeight.TrimStartZeroWithConvertToInt(),
                        UnitPrice = item.UnitPrice.TrimStartZeroWithConvertToInt(),
                        TransferCondition = item.TransferCondition,
                        ExtractionYMD = item.ExtractionYMD
                    };

                    list.Add(dto);
                });

                return (IList<V>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PQGA420BDTOのジェネレータ
    /// </summary>
    public class PQGA420BDTOGenerator : IDTOGenerator
    {
        #region メソッド

        /// <summary>
        /// DTOのリストのジェネレート
        /// </summary>
        /// <typeparam name="V">DTOBaseを継承したクラス</typeparam>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <param name="models">モデルのリスト</param>
        /// <returns>DTOのリスト</returns>
        public IList<V> GenerateList<T, V>(IList<T> models)
            where T : ModelBase
            where V : DTOBase
        {
            var list = new List<PQGA420BDTO>();

            try
            {
                var targetModels = models as IList<PQGA420BModel>;
                targetModels.ForEach(item =>
                {
                    var dto = new PQGA420BDTO()
                    {
                        OrderNo = item.OrderNo,
                        CustomerCode = item.CustomerCode,
                        CustomerName = item.CustomerName,
                        DisutributorCode = item.DisutributorCode,
                        City = item.City,
                        Collection = item.Collection,
                        Kind = item.Kind,
                        StandardCode = item.StandardCode,
                        SettlementCondition = item.SettlementCondition,
                        DisutributorName = item.DisutributorName,
                        StandardName = item.StandardName,
                        SlabSize = item.SlabSize,
                        SlabAtu = item.SlabAtu,
                        SlabHaba = item.SlabHaba,
                        SlabNaga = item.SlabHaba,
                        Honsuu = item.Honsuu.TrimStartZeroWithConvertToInt(),
                        Weight = item.Weight.TrimStartZeroWithConvertToInt(),
                        DeliveryDate = item.DeliveryDate.Trim() != string.Empty ?
                                       item.DeliveryDate.Substring(0, 2) + SymbolType.Slash.GetValue()
                                     + item.DeliveryDate.Substring(2, 2) + SymbolType.Slash.GetValue()
                                     + item.DeliveryDate.Substring(4, 2) : item.DeliveryDate,
                        DeliveryDateRank = item.DeliveryDateRank,
                        TransferCondition = item.TransferCondition
                    };

                    list.Add(dto);
                });

                return (IList<V>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// PQGA820BDTOのジェネレータ
    /// </summary>
    public class PQGA820BDTOGenerator : IDTOGenerator
    {
        #region メソッド

        /// <summary>
        /// DTOのリストのジェネレート
        /// </summary>
        /// <typeparam name="V">DTOBaseを継承したクラス</typeparam>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <param name="models">モデルのリスト</param>
        /// <returns>DTOのリスト</returns>
        public IList<V> GenerateList<T, V>(IList<T> models)
            where T : ModelBase
            where V : DTOBase
        {
            var list = new List<PQGA820BDTO>();

            try
            {
                var targetModels = models as IList<PQGA820BModel>;
                targetModels.ForEach(item =>
                {
                    var dto = new PQGA820BDTO()
                    {
                        ShipmentYYMMDD = item.ShipmentYYMMDD,
                        ZK081Honsuu = item.ZK081Honsuu.TrimStartZeroWithConvertToInt(),
                        ZK081Weight = item.ZK081Weight.TrimStartZeroWithConvertToInt(),
                        ZK085Honsuu = item.ZK085Honsuu.TrimStartZeroWithConvertToInt(),
                        ZK085Weight = item.ZK085Weight.TrimStartZeroWithConvertToInt(),
                        ZK121Honsuu = item.ZK121Honsuu.TrimStartZeroWithConvertToInt(),
                        ZK121Weight = item.ZK121Weight.TrimStartZeroWithConvertToInt(),
                        ZK172Honsuu = item.ZK172Honsuu.TrimStartZeroWithConvertToInt(),
                        ZK172Weight = item.ZK172Weight.TrimStartZeroWithConvertToInt(),
                        SPHCHonsuu = item.SPHCHonsuu.TrimStartZeroWithConvertToInt(),
                        SPHCWeight = item.SPHCWeight.TrimStartZeroWithConvertToInt(),
                        SS400Honsuu = item.SS400Honsuu.TrimStartZeroWithConvertToInt(),
                        SS400Weight = item.SS400Weight.TrimStartZeroWithConvertToInt(),
                        OtherHonsuu = item.OtherHonsuu.TrimStartZeroWithConvertToInt(),
                        OtherWeight = item.OtherWeight.TrimStartZeroWithConvertToInt(),
                        TotalHonsuu = item.TotalHonsuu.TrimStartZeroWithConvertToInt(),
                        TotalWeight = item.TotalWeight.TrimStartZeroWithConvertToInt()
                    };

                    list.Add(dto);
                });

                return (IList<V>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// SSYM040Bのジェネレータ
    /// </summary>
    public class SSYM040BDTOGenerator : IDTOGenerator
    {
        #region メソッド

        /// <summary>
        /// DTOのリストのジェネレート
        /// </summary>
        /// <typeparam name="V">DTOBaseを継承したクラス</typeparam>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <param name="models">モデルのリスト</param>
        /// <returns>DTOのリスト</returns>
        public IList<V> GenerateList<T, V>(IList<T> models)
            where T : ModelBase
            where V : DTOBase
        {
            var list = new List<SSYM040BDTO>();

            try
            {
                var targetModels = models as IList<SSYM040BModel>;
                targetModels.ForEach(item =>
                {
                    var dto = new SSYM040BDTO()
                    {
                        YYYYMMDD = item.YYYYMMDD,
                        MiddlePlateWaste = item.MiddlePlateWaste.TrimStartZeroWithConvertToInt(),
                        NissinReturnWaste = item.NissinReturnWaste.TrimStartZeroWithConvertToInt(),
                        TrimmerWaste = item.TrimmerWaste.TrimStartZeroWithConvertToInt(),
                        AtuItaLineWaste = item.AtuItaLineWaste.TrimStartZeroWithConvertToInt(),
                        PlanerWaste = item.PlanerWaste.TrimStartZeroWithConvertToInt(),
                        LaserWaste = item.LaserWaste.TrimStartZeroWithConvertToInt(),
                        PlanerChitaWaste = item.PlanerChitaWaste.TrimStartZeroWithConvertToInt(),
                        ColumuReturnWaste = item.ColumuReturnWaste.TrimStartZeroWithConvertToInt(),
                        Total = item.Total.TrimStartZeroWithConvertToInt()
                    };

                    list.Add(dto);
                });

                return (IList<V>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// SSYM050BDTOのジェネレータ
    /// </summary>
    public class SSYM050BDTOGenerator : IDTOGenerator
    {
        #region メソッド

        /// <summary>
        /// DTOのリストのジェネレート
        /// </summary>
        /// <typeparam name="V">DTOBaseを継承したクラス</typeparam>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <param name="models">モデルのリスト</param>
        /// <returns>DTOのリスト</returns>
        public IList<V> GenerateList<T, V>(IList<T> models)
            where T : ModelBase
            where V : DTOBase
        {
            var list = new List<SSYM050BDTO>();

            try
            {
                var targetModels = models as IList<SSYM050BModel>;
                targetModels.ForEach(item =>
                {
                    var dto = new SSYM050BDTO()
                    {
                        YYYYMMDD = item.YYYYMMDD,
                        EndShearWaste = item.EndShearWaste.TrimStartZeroWithConvertToInt(),
                        PlanerWaste = item.PlanerWaste.TrimStartZeroWithConvertToInt(),
                        TrimmerWaste = item.TrimmerWaste.TrimStartZeroWithConvertToInt(),
                        LaserWaste = item.LaserWaste.TrimStartZeroWithConvertToInt(),
                        OtherWaste = item.OtherWaste.TrimStartZeroWithConvertToInt(),
                        Total = item.Total.TrimStartZeroWithConvertToInt()
                    };

                    list.Add(dto);
                });

                return (IList<V>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// SSZA040BDTOのジェネレータ
    /// </summary>
    public class SSZA040BDTOGenerator : IDTOGenerator
    {
        #region メソッド

        /// <summary>
        /// DTOのリストのジェネレート
        /// </summary>
        /// <typeparam name="V">DTOBaseを継承したクラス</typeparam>
        /// <typeparam name="T">ModelBaseを継承したクラス</typeparam>
        /// <param name="models">モデルのリスト</param>
        /// <returns>DTOのリスト</returns>
        public IList<V> GenerateList<T, V>(IList<T> models)
            where T : ModelBase
            where V : DTOBase
        {
            var list = new List<SSZA040BDTO>();

            try
            {
                var targetModels = models as IList<SSZA040BModel>;
                targetModels.ForEach(item =>
                {
                    var dto = new SSZA040BDTO()
                    {
                        OrderNo = item.OrderNo,
                        StandardName = item.StandardName,
                        ProductSize = item.ProductSize,
                        ProductAtu = item.ProductAtu,
                        ProductHaba = item.ProductHaba,
                        ProductNaga = item.ProductNaga,
                        TM = item.TM,
                        ProductCode = item.ProductCode,
                        CustomerName = item.CustomerName,
                        OrderCount = item.OrderCount.TrimStartZeroWithConvertToInt(),
                        OrderWeight = item.OrderWeight.TrimStartZeroWithConvertToInt(),
                        ShipmentCount = item.ShipmentCount.TrimStartZeroWithConvertToInt(),
                        TransferPlaceName = item.TransferPlaceName,
                        TransferCondition = item.TransferCondition,
                        DeliveryDate = item.DeliveryDate.Trim() != string.Empty ?
                                       item.DeliveryDate.Substring(0, 2) + SymbolType.Slash.GetValue() + item.DeliveryDate.Substring(2, 2) : item.DeliveryDate,
                        DeliveryDateRank = item.DeliveryDateRank,
                        ProvisionClassfication = item.ProvisionClassfication
                    };

                    list.Add(dto);
                });

                return (IList<V>)list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}