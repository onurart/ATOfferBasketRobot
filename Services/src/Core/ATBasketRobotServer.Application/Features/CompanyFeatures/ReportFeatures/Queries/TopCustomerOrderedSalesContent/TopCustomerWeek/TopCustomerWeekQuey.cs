using ATBasketRobotServer.Application.Messaging;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TopCustomerOrderedSalesContent.TopCustomerWeek;

public sealed record TopCustomerWeekQuey(string companyId, string customerId) : IQuery<TopCustomerWeekResponse>;
