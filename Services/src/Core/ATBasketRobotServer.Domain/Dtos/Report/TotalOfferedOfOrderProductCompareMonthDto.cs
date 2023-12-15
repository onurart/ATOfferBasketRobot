namespace ATBasketRobotServer.Domain.Dtos.Report;
public sealed record TotalOfferedOfOrderProductCompareMonthDto(int Months, double OrderedProduct, double OfferedProduct, double OrderToOfferRatio);